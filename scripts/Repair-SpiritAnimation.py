"""Re-anchor the Confictura Spirit art in a staged ClassicUO asset directory."""

import argparse
import hashlib
import mmap
import os
import shutil
import struct
import tempfile
from pathlib import Path


INDEX_SHA256 = "c2a7a2c76ef2d2de72944ab33e00cf2cb17eae926600eca1b070f1a5e8da6536"
SOURCE_SHA256 = {
    "416c3c4cf310a46cb37e1af02fe2cc8c82954249f52214ae6c50fa4a280c0da9",
    "99ed322a9aae3811dd2d610fbb34fd7dd24d5c15b095f0cb96b4ae4abbcd4f0b",
}
REPAIRED_SHA256 = "205267a7d48b9fa7f7f78aa5408cf30b56c4331ed6c1dc39e4efaf2e9a7c4e79"
INDEX_RECORD = struct.Struct("<iii")
FRAME_HEADER = struct.Struct("<hhhh")
UINT32 = struct.Struct("<I")
INT16 = struct.Struct("<h")
SPIRIT_SLOT = 33
HIGH_GROUP_SLOTS = 22 * 5
Y_MASK = 0x3FF << 12
END_OF_FRAME = 0x7FFF7FFF


def sha256(path):
    with path.open("rb") as stream:
        return hashlib.file_digest(stream, "sha256").hexdigest()


def repair_frames(data, index):
    # Bodyconv.def maps body 84 to anim4 slot 33 with the monster animation layout.
    first = SPIRIT_SLOT * HIGH_GROUP_SLOTS
    for entry in range(first, first + HIGH_GROUP_SLOTS):
        offset, length, _ = INDEX_RECORD.unpack_from(index, entry * INDEX_RECORD.size)
        if offset < 0 or length <= 0:
            continue

        frames = offset + 512  # 256 16-bit palette entries
        frame_count, = UINT32.unpack_from(data, frames)
        for number in range(frame_count):
            relative, = UINT32.unpack_from(data, frames + 4 * (number + 1))
            frame = frames + relative
            _, _, _, height = FRAME_HEADER.unpack_from(data, frame)

            # Both known inputs encode rows relative to the original half-height center.
            # Move the runs by the same amount so the new center leaves the decoded pixels intact.
            INT16.pack_into(data, frame + 2, 0)
            run = frame + FRAME_HEADER.size
            while True:
                header, = UINT32.unpack_from(data, run)
                if header == END_OF_FRAME:
                    break
                encoded_y = (header & Y_MASK) >> 12
                y = (encoded_y ^ 0x200) - 0x200
                anchored_y = y + height // 2
                UINT32.pack_into(data, run, (header & ~Y_MASK) | ((anchored_y & 0x3FF) << 12))
                run += UINT32.size + (header & 0xFFF)


def build_asset(source_dir, output):
    index = (source_dir / "anim4.idx").read_bytes()
    if hashlib.sha256(index).hexdigest() != INDEX_SHA256:
        raise ValueError("Unrecognized anim4.idx. Use the matching Confictura client asset set.")
    if output.exists():
        raise FileExistsError(f"Output already exists: {output}")

    source = source_dir / "anim4.mul"
    with tempfile.NamedTemporaryFile(dir=output.parent, delete=False) as stream:
        staged = Path(stream.name)

    try:
        shutil.copyfile(source, staged)
        fingerprint = sha256(staged)
        if fingerprint not in SOURCE_SHA256 and fingerprint != REPAIRED_SHA256:
            raise ValueError("Unrecognized anim4.mul. The input was left unchanged.")
        if fingerprint != REPAIRED_SHA256:
            with staged.open("r+b") as stream:
                with mmap.mmap(stream.fileno(), 0) as data:
                    repair_frames(data, index)
                    data.flush()

        if sha256(staged) != REPAIRED_SHA256:
            raise ValueError("Repaired anim4.mul did not match the verified asset.")
        if os.name == "nt":
            staged.rename(output)
        else:
            os.link(staged, output)
    finally:
        staged.unlink(missing_ok=True)

    return REPAIRED_SHA256


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("source_dir", type=Path, help="Directory containing anim4.idx and anim4.mul")
    parser.add_argument("output", type=Path, help="New anim4.mul path in a staging directory")
    args = parser.parse_args()
    try:
        fingerprint = build_asset(args.source_dir, args.output)
    except (OSError, ValueError) as error:
        parser.exit(1, f"{error}\n")
    print(f"{args.output}  SHA-256 {fingerprint}")


if __name__ == "__main__":
    main()
