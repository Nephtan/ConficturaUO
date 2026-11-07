"""Re-anchor Confictura Spirit and player-form art in staged ClassicUO assets."""

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
PLAYER_FORMS_SHA256 = "2ee189376602fa67d4a4a7ecc961016cd73529e65bc49a8660319735659612ad"
INDEX_RECORD = struct.Struct("<iii")
FRAME_HEADER = struct.Struct("<hhhh")
UINT32 = struct.Struct("<I")
INT16 = struct.Struct("<h")
SPIRIT_SLOT = 33
HIGH_GROUP_SLOTS = 22 * 5
Y_MASK = 0x3FF << 12
END_OF_FRAME = 0x7FFF7FFF

# anim4 slot, standing padding, and median walking padding in file direction order.
# Other actions share the standing reference so their authored motion is retained.
PLAYER_FORM_ANCHORS = {
    49: ((4, 2, 1, 4, 9), (6, 10, 10, 9, 6)),       # Blue Fungal (341)
    50: ((15, 6, 3, 3, 0), (9, 12, 11, 11, 11)),   # Red Fungal (342)
    59: ((35, 38, 28, 5, 0), (38, 44, 32, 11, 6)), # Salamander (673)
    62: ((0, 10, 36, 43, 37), (5, 8, 23, 37, 32)), # Neptar (676)
    63: ((12, 4, 11, 22, 7), (17, 27, 32, 17, 15)), # Neptar (677)
    64: ((6, 0, 7, 20, 25), (9, 12, 13, 22, 23)), # Tritun (678)
    65: ((6, 0, 7, 20, 25), (9, 12, 13, 22, 23)), # Tritun (690)
}


def sha256(path):
    with path.open("rb") as stream:
        return hashlib.file_digest(stream, "sha256").hexdigest()


def repair_slot(data, index, slot, anchors=None):
    first = slot * HIGH_GROUP_SLOTS
    for action_direction in range(HIGH_GROUP_SLOTS):
        entry = first + action_direction
        offset, length, _ = INDEX_RECORD.unpack_from(index, entry * INDEX_RECORD.size)
        if offset < 0 or length <= 0:
            continue

        new_center = 0
        if anchors is not None:
            standing, walking = anchors
            padding = walking if action_direction < 5 else standing
            new_center = -padding[action_direction % 5]

        frames = offset + 512  # 256 16-bit palette entries
        frame_count, = UINT32.unpack_from(data, frames)
        for number in range(frame_count):
            relative, = UINT32.unpack_from(data, frames + 4 * (number + 1))
            frame = frames + relative
            _, old_center, _, height = FRAME_HEADER.unpack_from(data, frame)

            # Both Spirit variants encode runs for the original positive half-height center.
            if anchors is None:
                old_center = height // 2
            y_shift = old_center - new_center
            INT16.pack_into(data, frame + 2, new_center)
            run = frame + FRAME_HEADER.size
            while True:
                header, = UINT32.unpack_from(data, run)
                if header == END_OF_FRAME:
                    break
                encoded_y = (header & Y_MASK) >> 12
                y = (encoded_y ^ 0x200) - 0x200
                anchored_y = y + y_shift
                if not -512 <= anchored_y < 512:
                    raise ValueError("Repaired animation run exceeds the signed Y field.")
                UINT32.pack_into(data, run, (header & ~Y_MASK) | ((anchored_y & 0x3FF) << 12))
                run += UINT32.size + (header & 0xFFF)


def build_asset(source_dir, output, player_forms=False):
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
        if fingerprint not in SOURCE_SHA256 | {REPAIRED_SHA256, PLAYER_FORMS_SHA256}:
            raise ValueError("Unrecognized anim4.mul. Use a matching Confictura client asset set.")
        expected = PLAYER_FORMS_SHA256 if player_forms or fingerprint == PLAYER_FORMS_SHA256 else REPAIRED_SHA256
        if fingerprint != expected:
            with staged.open("r+b") as stream:
                with mmap.mmap(stream.fileno(), 0) as data:
                    if fingerprint in SOURCE_SHA256:
                        repair_slot(data, index, SPIRIT_SLOT)
                    if player_forms:
                        for slot, anchors in PLAYER_FORM_ANCHORS.items():
                            repair_slot(data, index, slot, anchors)
                    data.flush()

        if sha256(staged) != expected:
            raise ValueError("Repaired anim4.mul did not match the verified asset.")
        if os.name == "nt":
            staged.rename(output)
        else:
            os.link(staged, output)
    finally:
        staged.unlink(missing_ok=True)

    return expected


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("source_dir", type=Path, help="Directory containing anim4.idx and anim4.mul")
    parser.add_argument("output", type=Path, help="New anim4.mul path in a staging directory")
    parser.add_argument(
        "--player-forms",
        action="store_true",
        help="Include the seven corrected gypsy shelf forms with the Spirit fix",
    )
    args = parser.parse_args()
    try:
        fingerprint = build_asset(args.source_dir, args.output, args.player_forms)
    except (OSError, ValueError) as error:
        parser.exit(1, f"{error}\n")
    print(f"{args.output}  SHA-256 {fingerprint}")


if __name__ == "__main__":
    main()
