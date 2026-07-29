#!/usr/bin/env python3
"""Build the enhanced mobile balance workbook and normalized CSV outputs.

This script intentionally uses only the Python standard library so the workflow
does not depend on Excel, openpyxl, or a package restore. It reads the staff
submitted workbook as an XLSX zip package, normalizes the data into additional
tables, and writes a fresh XLSX package with dropdown validations.
"""

from __future__ import annotations

import argparse
import csv
import os
import re
import shutil
import sys
import zipfile
from dataclasses import dataclass
from datetime import datetime, timezone
from pathlib import Path
from typing import Dict, Iterable, List, Optional, Sequence, Tuple
from xml.etree import ElementTree as ET
from xml.sax.saxutils import escape


MAIN_NS = "http://schemas.openxmlformats.org/spreadsheetml/2006/main"
REL_NS = "http://schemas.openxmlformats.org/officeDocument/2006/relationships"
PKG_REL_NS = "http://schemas.openxmlformats.org/package/2006/relationships"
CONTENT_NS = "http://schemas.openxmlformats.org/package/2006/content-types"

ROOT = Path(__file__).resolve().parents[3]
WORK_DIR = ROOT / "docs" / "mobile-balance-adjustments"
SOURCE_XLSX = WORK_DIR / "source" / "MobileBalanceTemplate.staff-revised-2026-07-28.xlsx"
CANONICAL_XLSX = WORK_DIR / "workbooks" / "MobileBalanceTemplate.xlsx"
OUTPUT_DIR = WORK_DIR / "outputs"

SKILLS_CS = ROOT / "Data" / "System" / "Source" / "Skills.cs"
SCRIPTS_ROOT = ROOT / "Data" / "Scripts"


CORE_SHEETS = ("MobileChanges", "NewLootItems", "LootAssignments")


SKILL_ALIASES = {
    "magic resist": "MagicResist",
    "magicresist": "MagicResist",
    "parrying": "Parry",
    "parry": "Parry",
    "blacksmithing": "Blacksmith",
    "blacksmithy": "Blacksmith",
    "merchantile": "Mercantile",
    "mercantile": "Mercantile",
    "aantomy": "Anatomy",
}


CLASS_ALIASES = {
    "PowderofTemperment": "PowderOfTemperament",
    "PheonixFeather": "PhoenixFeather",
    "RoseofMoonPetal": "RoseOfMoonPetal",
    "MetalPigmentsofIslesDread": "MetalPigmentsOfIslesDread",
}


BONUS_ALIASES = {
    "hitchance": "AttackChance",
    "bonusstamina": "BonusStam",
    "hitsregen": "RegenHits",
    "magic resist": "MagicResist",
}


KNOWN_BONUSES = {
    "BonusMana": ("AosAttributes", "item.Attributes.BonusMana", "Yes"),
    "LowerRegCost": ("AosAttributes", "item.Attributes.LowerRegCost", "Yes"),
    "CastRecovery": ("AosAttributes", "item.Attributes.CastRecovery", "Yes"),
    "CastSpeed": ("AosAttributes", "item.Attributes.CastSpeed", "Yes"),
    "SpellDamage": ("AosAttributes", "item.Attributes.SpellDamage", "Yes"),
    "AttackChance": ("AosAttributes", "item.Attributes.AttackChance", "Yes"),
    "DefendChance": ("AosAttributes", "item.Attributes.DefendChance", "Yes"),
    "LowerManaCost": ("AosAttributes", "item.Attributes.LowerManaCost", "Yes"),
    "RegenMana": ("AosAttributes", "item.Attributes.RegenMana", "Yes"),
    "RegenHits": ("AosAttributes", "item.Attributes.RegenHits", "Yes"),
    "RegenStam": ("AosAttributes", "item.Attributes.RegenStam", "Yes"),
    "Luck": ("AosAttributes", "item.Attributes.Luck", "Yes"),
    "BonusHits": ("AosAttributes", "item.Attributes.BonusHits", "Yes"),
    "BonusStam": ("AosAttributes", "item.Attributes.BonusStam", "Yes"),
    "WeaponDamage": ("AosAttributes", "item.Attributes.WeaponDamage", "Yes"),
    "ReflectPhysical": ("AosAttributes", "item.Attributes.ReflectPhysical", "Yes"),
    "SpellChanneling": ("AosAttributes", "item.Attributes.SpellChanneling", "Yes"),
    "SelfRepair": ("AosWeaponOrArmorAttributes", "weapon.WeaponAttributes.SelfRepair or armor.ArmorAttributes.SelfRepair", "No"),
    "HitLightning": ("AosWeaponAttributes", "weapon.WeaponAttributes.HitLightning", "No"),
    "HitLeechMana": ("AosWeaponAttributes", "weapon.WeaponAttributes.HitLeechMana", "No"),
    "HitLeechStam": ("AosWeaponAttributes", "weapon.WeaponAttributes.HitLeechStam", "No"),
    "HitLeechHits": ("AosWeaponAttributes", "weapon.WeaponAttributes.HitLeechHits", "No"),
    "HitPoisonArea": ("AosWeaponAttributes", "weapon.WeaponAttributes.HitPoisonArea", "No"),
    "MageArmor": ("AosArmorAttributes", "armor.ArmorAttributes.MageArmor", "No"),
    "MinDamage": ("BaseWeaponProperty", "weapon.MinDamage", "No"),
    "MaxDamage": ("BaseWeaponProperty", "weapon.MaxDamage", "No"),
    "PhysicalBonus": ("BaseArmorResistanceBonus", "armor.PhysicalBonus", "No"),
    "FireBonus": ("BaseArmorResistanceBonus", "armor.FireBonus", "No"),
    "ColdBonus": ("BaseArmorResistanceBonus", "armor.ColdBonus", "No"),
    "PoisonBonus": ("BaseArmorResistanceBonus", "armor.PoisonBonus", "No"),
    "EnergyBonus": ("BaseArmorResistanceBonus", "armor.EnergyBonus", "No"),
    "Weight": ("ItemProperty", "item.Weight", "No"),
}


WEAPON_BASES = {
    "RoyalSword",
    "Spear",
    "NoDachi",
    "Tetsubo",
    "Crossbow",
    "KilrathiHeavyGun",
    "Mace",
}


ARMOR_BASES = {
    "MetalKiteShield",
    "RoyalChest",
    "BoneChest",
    "BoneHelm",
    "DragonGloves",
    "Robe",
}


LIST_VALUES = {
    "ScopeList": ["ExactMobile", "ListedMobiles", "EpicBossCandidate"],
    "PriorityList": ["High", "Medium", "Low"],
    "ReviewStatusList": ["Ready", "NeedsDecision", "NeedsNormalization", "Blocked"],
    "ImplementationStatusList": ["Pending", "ReadyForImplementation", "NeedsPolicyDecision", "NeedsClassNameDecision", "BlockedByNameConflict"],
    "ClassStatusList": ["ExistingSourceClass", "ExistingSourceClassAfterCorrection", "NewCustomClass", "NameConflict", "NeedsBaseClassReview"],
    "LayerList": ["Talisman", "None", "Other", "OneHanded", "TwoHanded", "Helm", "InnerTorso", "OuterTorso", "Gloves"],
    "LootTypeList": ["Regular", "Blessed", "Cursed", "Newbied"],
    "YesNoList": ["No", "Yes"],
    "OwnerPolicyStatusList": ["NotOwnerBound", "NeedsOwnerBindingPolicy", "OwnerPolicyApproved"],
    "DropTimingList": ["CorpseOnDeath", "GenerateLoot", "ConstructorPack", "SpecialOnDeath", "Other"],
    "DropRuleList": ["NeedsDecision", "ChancePercentOnCorpse", "GuaranteedAlwaysDrop", "GuaranteedOneOfGroup", "ManualSpecial"],
    "DropSemanticStatusList": ["Ready", "NeedsDecision", "Blocked"],
    "SkillModKindList": ["StockPositiveSkillBonus", "CustomSignedEquipSkillMod"],
    "BonusGroupList": sorted({value[0] for value in KNOWN_BONUSES.values()} | {"Unknown"}),
    "BonusStatusList": ["Ready", "NeedsMapping", "NeedsImplementationDecision"],
}


@dataclass
class ClassInfo:
    class_name: str
    base_name: str
    path: str
    kind: str
    display_name: str = ""
    title: str = ""


@dataclass
class SheetSpec:
    name: str
    title: str
    note: str
    headers: List[str]
    rows: List[Dict[str, object]]
    validations: List[Dict[str, str]]


def rel(path: Path) -> str:
    return path.resolve().relative_to(ROOT).as_posix()


def col_letter(index: int) -> str:
    result = ""
    while index:
        index, remainder = divmod(index - 1, 26)
        result = chr(65 + remainder) + result
    return result


def normalize_key(value: object) -> str:
    return re.sub(r"[^a-z0-9]", "", str(value or "").lower())


def row_value(row: Dict[str, str], *names: str) -> str:
    for name in names:
        if name in row:
            return row[name]
    return ""


def is_number(value: object) -> bool:
    if isinstance(value, (int, float)):
        return True
    text = str(value)
    return bool(re.fullmatch(r"-?\d+(\.\d+)?", text))


def number_value(value: object) -> object:
    if isinstance(value, (int, float)):
        return value
    text = str(value)
    if re.fullmatch(r"-?\d+", text):
        return int(text)
    if re.fullmatch(r"-?\d+\.\d+", text):
        return float(text)
    return value


def read_xlsx_tables(path: Path) -> Dict[str, List[Dict[str, str]]]:
    if not path.exists():
        raise FileNotFoundError(path)

    ns_main = f"{{{MAIN_NS}}}"
    ns_rel = f"{{{REL_NS}}}"
    tables: Dict[str, List[Dict[str, str]]] = {}

    with zipfile.ZipFile(path, "r") as zf:
        shared: List[str] = []
        if "xl/sharedStrings.xml" in zf.namelist():
            shared_root = ET.fromstring(zf.read("xl/sharedStrings.xml"))
            for si in shared_root.findall(f".//{ns_main}si"):
                shared.append("".join(t.text or "" for t in si.findall(f".//{ns_main}t")))

        workbook = ET.fromstring(zf.read("xl/workbook.xml"))
        rels = ET.fromstring(zf.read("xl/_rels/workbook.xml.rels"))
        rel_by_id = {node.attrib["Id"]: node.attrib["Target"] for node in rels}

        def cell_text(cell: ET.Element) -> str:
            cell_type = cell.attrib.get("t", "")
            value_node = cell.find(f"{ns_main}v")
            if cell_type == "s":
                if value_node is None or value_node.text is None:
                    return ""
                return shared[int(value_node.text)]
            if cell_type == "inlineStr":
                return "".join(t.text or "" for t in cell.findall(f".//{ns_main}t"))
            if value_node is not None and value_node.text is not None:
                return value_node.text
            return ""

        for sheet in workbook.findall(f".//{ns_main}sheet"):
            sheet_name = sheet.attrib["name"]
            rel_id = sheet.attrib[f"{ns_rel}id"]
            target = rel_by_id[rel_id]
            entry_name = target.lstrip("/") if target.startswith("/") else f"xl/{target}"
            sheet_root = ET.fromstring(zf.read(entry_name))
            rows: Dict[int, Dict[str, str]] = {}
            for row in sheet_root.findall(f".//{ns_main}sheetData/{ns_main}row"):
                row_index = int(row.attrib["r"])
                row_cells: Dict[str, str] = {}
                for cell in row.findall(f"{ns_main}c"):
                    ref = cell.attrib.get("r", "")
                    col = re.match(r"[A-Z]+", ref).group(0)
                    row_cells[col] = cell_text(cell)
                rows[row_index] = row_cells

            header_cells = rows.get(4, {})
            headers = {col: value for col, value in header_cells.items() if value}
            objects: List[Dict[str, str]] = []
            for row_index in sorted(index for index in rows if index > 4):
                obj: Dict[str, str] = {"Row": str(row_index)}
                has_data = False
                for col, header in headers.items():
                    value = rows[row_index].get(col, "")
                    obj[header] = value
                    has_data = has_data or bool(value)
                if has_data:
                    objects.append(obj)
            tables[sheet_name] = objects
    return tables


def parse_skills() -> List[Dict[str, object]]:
    text = SKILLS_CS.read_text(encoding="utf-8", errors="replace")
    match = re.search(r"public\s+enum\s+SkillName\s*\{(?P<body>.*?)\n\s*\}", text, re.S)
    if not match:
        raise RuntimeError("Could not parse SkillName enum")
    skills: List[Dict[str, object]] = []
    for line in match.group("body").splitlines():
        line = line.strip().rstrip(",")
        item = re.match(r"(?P<name>[A-Za-z_][A-Za-z0-9_]*)\s*=\s*(?P<id>\d+)", line)
        if item:
            skills.append({"SkillId": int(item.group("id")), "SkillName": item.group("name"), "SourcePath": rel(SKILLS_CS)})
    return skills


def scan_classes() -> Dict[str, List[ClassInfo]]:
    result: Dict[str, List[ClassInfo]] = {}
    class_re = re.compile(r"public\s+(?:abstract\s+|sealed\s+|partial\s+)?class\s+([A-Za-z_][A-Za-z0-9_]*)\s*(?::\s*([A-Za-z_][A-Za-z0-9_\.]*))?")
    name_re = re.compile(r'Name\s*=\s*"([^"]+)"')
    title_re = re.compile(r'Title\s*=\s*"([^"]+)"')

    for path in SCRIPTS_ROOT.rglob("*.cs"):
        if any(part.lower() in {"bin", "obj"} for part in path.parts):
            continue
        rel_path = rel(path)
        text = path.read_text(encoding="utf-8", errors="replace")
        classes = class_re.findall(text)
        if not classes:
            continue
        display_match = name_re.search(text)
        title_match = title_re.search(text)
        display_name = display_match.group(1) if display_match else ""
        title = title_match.group(1) if title_match else ""
        for class_name, base_name in classes:
            normalized_path = rel_path.replace("\\", "/")
            if "/Mobiles/" in normalized_path or "/Custom/Mobiles/" in normalized_path:
                kind = "MobileCandidate"
            elif "/Items/" in normalized_path or "/Trades/" in normalized_path:
                kind = "ItemCandidate"
            elif class_name in {"CensusRecords", "MetalPigmentsOfIslesDread", "EnchantedSextant", "JewelImmortality"}:
                kind = "ItemCandidate"
            else:
                kind = "SourceClass"
            info = ClassInfo(class_name, base_name or "", rel_path, kind, display_name, title)
            result.setdefault(class_name, []).append(info)
    return result


def first_class(classes: Dict[str, List[ClassInfo]], class_name: str) -> Optional[ClassInfo]:
    values = classes.get(class_name)
    return values[0] if values else None


def canonical_class(name: str) -> str:
    return CLASS_ALIASES.get(str(name or "").strip(), str(name or "").strip())


def canonical_skill(name: str, skill_by_key: Dict[str, str]) -> str:
    raw = str(name or "").strip()
    if not raw:
        return ""
    alias = SKILL_ALIASES.get(raw.lower())
    if alias:
        return alias
    return skill_by_key.get(normalize_key(raw), raw)


def canonical_bonus(name: str) -> str:
    raw = str(name or "").strip()
    if not raw:
        return ""
    alias = BONUS_ALIASES.get(raw.lower())
    if alias:
        return alias
    by_key = {normalize_key(key): key for key in KNOWN_BONUSES}
    return by_key.get(normalize_key(raw), raw)


def parse_pairs(text: str) -> List[Tuple[str, str]]:
    pairs: List[Tuple[str, str]] = []
    for token in str(text or "").split(";"):
        token = token.strip()
        if not token or "=" not in token:
            continue
        name, value = token.split("=", 1)
        pairs.append((name.strip(), value.strip()))
    return pairs


def resolve_mobile(name: str, classes: Dict[str, List[ClassInfo]]) -> Tuple[str, str, str]:
    raw = str(name or "").strip()
    direct = raw.replace(" ", "").replace("'", "")
    if raw in classes:
        info = first_class(classes, raw)
        return info.class_name, info.path, "MatchedClassName"
    if direct in classes:
        info = first_class(classes, direct)
        return info.class_name, info.path, "MatchedCondensedClassName"

    candidates: Dict[str, ClassInfo] = {}
    for values in classes.values():
        for info in values:
            if info.kind != "MobileCandidate":
                continue
            if info.display_name:
                candidates[normalize_key(info.display_name)] = info
                if info.title:
                    candidates[normalize_key(f"{info.display_name} {info.title}")] = info
    info = candidates.get(normalize_key(raw))
    if info:
        return info.class_name, info.path, "MatchedDisplayName"
    return "", "", "Unresolved"


def item_status(canonical_name: str, canonical_base: str, row: Dict[str, str], classes: Dict[str, List[ClassInfo]]) -> Tuple[str, str]:
    if canonical_name == "Pestilence":
        return "NameConflict", "Existing Pestilence is an obsolete quiver; use a new class name for the mace before implementation."
    if canonical_name in classes:
        if str(row_value(row, "ClassNameRaw", "ClassName")).strip() != canonical_name:
            return "ExistingSourceClassAfterCorrection", "Class token corrected to existing source class."
        return "ExistingSourceClass", "Existing source class resolved."
    if canonical_base in classes:
        if canonical_name == "DreadMace":
            return "NewCustomClass", "Conflicting name resolved."
        return "NewCustomClass", "New class should inherit from the resolved base item class."
    return "NeedsBaseClassReview", "Neither class nor base item resolved to a source class."


def bonus_surface(name: str, base_item: str) -> Tuple[str, str, str]:
    group, surface, signed = KNOWN_BONUSES.get(name, ("Unknown", "Needs mapping", "Unknown"))
    if name == "SelfRepair":
        if base_item in ARMOR_BASES:
            return "AosArmorAttributes", "armor.ArmorAttributes.SelfRepair", signed
        if base_item in WEAPON_BASES:
            return "AosWeaponAttributes", "weapon.WeaponAttributes.SelfRepair", signed
    return group, surface, signed


def normalize_data(tables: Dict[str, List[Dict[str, str]]], skills: List[Dict[str, object]], classes: Dict[str, List[ClassInfo]]) -> Dict[str, List[Dict[str, object]]]:
    skill_by_key = {normalize_key(skill["SkillName"]): str(skill["SkillName"]) for skill in skills}
    mobile_rows = tables["MobileChanges"]
    item_rows = tables["NewLootItems"]
    assignment_rows = tables["LootAssignments"]

    normalized: Dict[str, List[Dict[str, object]]] = {}

    enhanced_mobile: List[Dict[str, object]] = []
    mobile_skill_rows: List[Dict[str, object]] = []
    mobile_by_assignment: Dict[str, Dict[str, object]] = {}

    skill_change_id = 1
    for row in mobile_rows:
        resolved_class, path, status = resolve_mobile(row.get("MobileClassOrName", ""), classes)
        skill_changes_raw = row_value(row, "SkillChangesRaw", "SkillChanges")
        skill_pairs = parse_pairs(skill_changes_raw)
        review_status = "Ready"
        notes: List[str] = []
        if status == "Unresolved":
            review_status = "NeedsDecision"
            notes.append("Mobile target did not resolve to a source class or display name.")
        for skill_name, value in skill_pairs:
            canonical = canonical_skill(skill_name, skill_by_key)
            if canonical not in skill_by_key.values():
                review_status = "NeedsNormalization"
                notes.append(f"Skill name needs mapping: {skill_name}")
            skill_min = ""
            skill_max = ""
            skill_value = value
            if "-" in value and not value.startswith("-"):
                left, right = value.split("-", 1)
                skill_min = left.strip()
                skill_max = right.strip()
                skill_value = ""
            mobile_skill_rows.append(
                {
                    "SkillChangeId": f"MSC-{skill_change_id:03d}",
                    "ChangeId": row.get("ChangeId", ""),
                    "MobileClassOrName": row.get("MobileClassOrName", ""),
                    "ResolvedMobileClass": resolved_class,
                    "SkillNameRaw": skill_name,
                    "CanonicalSkillName": canonical,
                    "SkillMin": skill_min,
                    "SkillMax": skill_max,
                    "SkillValue": skill_value,
                    "Operation": "SetSkill",
                    "Status": "Ready" if canonical in skill_by_key.values() else "NeedsSkillNameMapping",
                    "Notes": "",
                }
            )
            skill_change_id += 1
        loot_id = row.get("LootAssignmentIds", "")
        enhanced = {
            "ChangeId": row.get("ChangeId", ""),
            "MobileClassOrName": row.get("MobileClassOrName", ""),
            "ResolvedClassName": resolved_class,
            "KnownFilePath": path or row.get("KnownFilePath", ""),
            "Scope": row.get("Scope", ""),
            "DamageMin": number_value(row.get("DamageMin", "")),
            "DamageMax": number_value(row.get("DamageMax", "")),
            "SkillChangesRaw": skill_changes_raw,
            "LootAssignmentIds": loot_id,
            "Priority": row.get("Priority", ""),
            "ReviewStatus": review_status,
            "ImplementationStatus": "Pending",
            "Notes": row.get("Notes", ""),
            "CodexNotes": " ".join(notes),
        }
        enhanced_mobile.append(enhanced)
        if loot_id:
            mobile_by_assignment[loot_id] = enhanced

    enhanced_items: List[Dict[str, object]] = []
    item_skill_rows: List[Dict[str, object]] = []
    item_bonus_rows: List[Dict[str, object]] = []
    items_by_id: Dict[str, Dict[str, object]] = {}

    skill_mod_id = 1
    bonus_id = 1
    for row in item_rows:
        item_id = row.get("ItemId", "")
        class_name_raw = row_value(row, "ClassNameRaw", "ClassName")
        base_item_raw = row_value(row, "BaseItemRaw", "BaseItem")
        skill_bonuses_raw = row_value(row, "SkillBonusesRaw", "SkillBonuses")
        attributes_raw = row_value(row, "AttributesRaw", "Attributes")
        canonical_name = canonical_class(class_name_raw)
        canonical_base = canonical_class(base_item_raw)
        status, status_notes = item_status(canonical_name, canonical_base, row, classes)
        skill_pairs = parse_pairs(skill_bonuses_raw)
        attribute_pairs = parse_pairs(attributes_raw)
        skill_like_pairs: List[Tuple[str, str]] = []
        bonus_like_pairs: List[Tuple[str, str, str]] = []
        for name, value in skill_pairs:
            canonical = canonical_skill(name, skill_by_key)
            if canonical in skill_by_key.values():
                skill_like_pairs.append((name, value))
            else:
                bonus_like_pairs.append((name, value, "SkillBonuses"))
        for name, value in attribute_pairs:
            bonus_like_pairs.append((name, value, "Attributes"))

        requires_custom_skill_mod = any(value.startswith("-") for _, value in skill_like_pairs) or len(skill_like_pairs) > 5
        for name, value in skill_like_pairs:
            canonical = canonical_skill(name, skill_by_key)
            item_skill_rows.append(
                {
                    "SkillModId": f"ISM-{skill_mod_id:03d}",
                    "ItemId": item_id,
                    "CanonicalClassName": canonical_name,
                    "SkillNameRaw": name,
                    "CanonicalSkillName": canonical,
                    "Value": number_value(value),
                    "ModifierKind": "CustomSignedEquipSkillMod" if requires_custom_skill_mod else "StockPositiveSkillBonus",
                    "ImplementationSurface": "custom equip/unequip SkillMod" if requires_custom_skill_mod else "SkillBonuses.SetValues",
                    "Status": "Ready",
                    "Notes": "Negative or more than five skill mods require custom signed handling." if requires_custom_skill_mod else "",
                }
            )
            skill_mod_id += 1

        for name, value, source_column in bonus_like_pairs:
            canonical = canonical_bonus(name)
            group, surface, supports_negative = bonus_surface(canonical, canonical_base)
            item_bonus_rows.append(
                {
                    "BonusId": f"IB-{bonus_id:03d}",
                    "ItemId": item_id,
                    "CanonicalClassName": canonical_name,
                    "SourceColumn": source_column,
                    "BonusNameRaw": name,
                    "CanonicalBonusName": canonical,
                    "BonusGroup": group,
                    "Value": number_value(value),
                    "ImplementationSurface": surface,
                    "Status": "Ready" if group != "Unknown" else "NeedsMapping",
                    "Notes": "Moved out of SkillBonuses because this is not a SkillName." if source_column == "SkillBonuses" else "",
                }
            )
            bonus_id += 1

        owner_bound = row.get("OwnerBound", "")
        owner_policy = "NeedsOwnerBindingPolicy" if owner_bound == "Yes" else "NotOwnerBound"
        implementation_status = "BlockedByNameConflict" if status == "NameConflict" else ("NeedsPolicyDecision" if owner_policy == "NeedsOwnerBindingPolicy" else "Pending")
        enhanced = {
            "ItemId": item_id,
            "ClassNameRaw": class_name_raw,
            "CanonicalClassName": canonical_name,
            "BaseItemRaw": base_item_raw,
            "CanonicalBaseItem": canonical_base,
            "ExistingClassStatus": status,
            "DisplayName": row.get("DisplayName", ""),
            "ItemIDGraphic": row.get("ItemIDGraphic", ""),
            "Hue": number_value(row.get("Hue", "")),
            "Layer": row.get("Layer", ""),
            "LootType": row.get("LootType", ""),
            "SkillBonusesRaw": skill_bonuses_raw,
            "AttributesRaw": attributes_raw,
            "OwnerBound": owner_bound,
            "OwnerBoundPolicyStatus": owner_policy,
            "PropertyLabel": row.get("PropertyLabel", ""),
            "ImplementationStatus": implementation_status,
            "Notes": row.get("Notes", ""),
            "CodexNotes": status_notes,
        }
        enhanced_items.append(enhanced)
        items_by_id[item_id] = enhanced

    enhanced_assignments: List[Dict[str, object]] = []
    for row in assignment_rows:
        assignment_id = row.get("AssignmentId", "")
        mobile = mobile_by_assignment.get(assignment_id, {})
        item = items_by_id.get(row.get("ItemId", ""), {})
        chance = number_value(row.get("ChancePercent", ""))
        guaranteed = row.get("Guaranteed", "")
        needs_drop_decision = guaranteed == "Yes" and is_number(chance) and float(chance) < 100
        explicit_drop_rule = row.get("DropRule", "")
        explicit_drop_status = row.get("DropSemanticsStatus", "")
        if needs_drop_decision:
            drop_rule = "NeedsDecision"
            drop_status = "NeedsDecision"
        elif guaranteed == "Yes":
            drop_rule = explicit_drop_rule or "GuaranteedAlwaysDrop"
            drop_status = explicit_drop_status or "Ready"
        else:
            drop_rule = explicit_drop_rule or "ChancePercentOnCorpse"
            drop_status = explicit_drop_status or "Ready"
        enhanced_assignments.append(
            {
                "AssignmentId": assignment_id,
                "MobileClassOrName": row.get("MobileClassOrName", ""),
                "ResolvedMobileClass": mobile.get("ResolvedClassName", ""),
                "ItemId": row.get("ItemId", ""),
                "CanonicalItemClass": item.get("CanonicalClassName", ""),
                "DropTiming": row.get("DropTiming", ""),
                "ChancePercent": chance,
                "QuantityMin": number_value(row.get("QuantityMin", "")),
                "QuantityMax": number_value(row.get("QuantityMax", "")),
                "Guaranteed": guaranteed,
                "DropRule": drop_rule,
                "DropSemanticsStatus": drop_status,
                "Notes": row.get("Notes", ""),
                "CodexNotes": "Guaranteed=Yes conflicts with ChancePercent below 100; staff policy required." if needs_drop_decision else "",
            }
        )

    normalized["MobileChanges"] = enhanced_mobile
    normalized["NewLootItems"] = enhanced_items
    normalized["LootAssignments"] = enhanced_assignments
    normalized["MobileSkillChanges"] = mobile_skill_rows
    normalized["ItemSkillMods"] = item_skill_rows
    normalized["ItemBonuses"] = item_bonus_rows

    submitted_skill_notes = {
        str(row_value(row, "SkillName")).strip(): str(row_value(row, "Notes")).strip()
        for row in tables.get("Ref_Skills", [])
        if str(row_value(row, "SkillName")).strip()
    }
    normalized["Ref_Skills"] = [
        {
            "SkillId": skill["SkillId"],
            "SkillName": skill["SkillName"],
            "SourcePath": skill["SourcePath"],
            "Status": "SourceVerified",
            "Notes": submitted_skill_notes.get(str(skill["SkillName"]), ""),
        }
        for skill in skills
    ]

    item_class_rows: Dict[str, Dict[str, object]] = {}
    for class_name, infos in classes.items():
        info = infos[0]
        if info.kind == "ItemCandidate" or class_name in {row["CanonicalClassName"] for row in enhanced_items} or class_name in {row["CanonicalBaseItem"] for row in enhanced_items}:
            item_class_rows[class_name] = {
                "ClassName": class_name,
                "BaseClass": info.base_name,
                "SourcePath": info.path,
                "Kind": info.kind,
                "Status": "SourceVerified",
                "Notes": "Multiple source declarations found." if len(infos) > 1 else "",
            }
    for row in enhanced_items:
        class_name = str(row["CanonicalClassName"])
        if class_name and class_name not in item_class_rows:
            item_class_rows[class_name] = {
                "ClassName": class_name,
                "BaseClass": row["CanonicalBaseItem"],
                "SourcePath": "",
                "Kind": "ProposedItemClass",
                "Status": "Proposed",
                "Notes": row["CodexNotes"],
            }
    normalized["Ref_ItemClasses"] = sorted(item_class_rows.values(), key=lambda item: str(item["ClassName"]))

    mobile_ref_rows: List[Dict[str, object]] = []
    seen_mobile = set()
    for class_name, infos in classes.items():
        for info in infos:
            if info.kind != "MobileCandidate" or info.class_name in seen_mobile:
                continue
            display = " ".join(info.display_name.split())
            if info.title:
                display = " ".join(f"{display} {info.title}".split())
            mobile_ref_rows.append(
                {
                    "ClassName": info.class_name,
                    "DisplayName": display,
                    "SourcePath": info.path,
                    "Status": "SourceVerified",
                    "Notes": "",
                }
            )
            seen_mobile.add(info.class_name)
    normalized["Ref_MobileClasses"] = sorted(mobile_ref_rows, key=lambda item: str(item["ClassName"]))

    normalized["Ref_BonusNames"] = [
        {
            "BonusName": name,
            "BonusGroup": values[0],
            "ImplementationSurface": values[1],
            "SupportsNegative": values[2],
            "Status": "SourceVerified",
            "Notes": "",
        }
        for name, values in sorted(KNOWN_BONUSES.items())
    ]

    normalized["Ref_DropRules"] = [
        {"DropRule": "NeedsDecision", "Meaning": "Staff must choose semantics before implementation.", "Status": "PolicyRequired"},
        {"DropRule": "ChancePercentOnCorpse", "Meaning": "Roll ChancePercent when the mobile dies and add the item on success.", "Status": "Ready"},
        {"DropRule": "GuaranteedAlwaysDrop", "Meaning": "Always add the listed item on death.", "Status": "Ready"},
        {"DropRule": "GuaranteedOneOfGroup", "Meaning": "Choose one item from a grouped assignment; probability semantics need implementation detail.", "Status": "PolicyRequired"},
        {"DropRule": "ManualSpecial", "Meaning": "Requires custom drop code outside a normal corpse roll.", "Status": "PolicyRequired"},
    ]

    review_rows: List[Dict[str, object]] = []
    for row in enhanced_assignments:
        if row["DropSemanticsStatus"] == "NeedsDecision":
            review_rows.append(
                {
                    "IssueId": f"MBR-{len(review_rows) + 1:03d}",
                    "Severity": "P1",
                    "Sheet": "LootAssignments",
                    "RowKey": row["AssignmentId"],
                    "Issue": "Guaranteed=Yes with ChancePercent below 100",
                    "Recommendation": "Choose whether chance or guaranteed semantics control the drop.",
                    "Status": "NeedsDecision",
                }
            )
    for row in enhanced_items:
        if row["OwnerBoundPolicyStatus"] == "NeedsOwnerBindingPolicy":
            review_rows.append(
                {
                    "IssueId": f"MBR-{len(review_rows) + 1:03d}",
                    "Severity": "P1",
                    "Sheet": "NewLootItems",
                    "RowKey": row["ItemId"],
                    "Issue": "OwnerBound=Yes requires binding policy",
                    "Recommendation": "Choose killer, top damager, looter, first equipper, or another binding rule.",
                    "Status": "NeedsDecision",
                }
            )
        if row["ExistingClassStatus"] == "NameConflict":
            review_rows.append(
                {
                    "IssueId": f"MBR-{len(review_rows) + 1:03d}",
                    "Severity": "P1",
                    "Sheet": "NewLootItems",
                    "RowKey": row["ItemId"],
                    "Issue": "Class name conflicts with existing source class",
                    "Recommendation": "Pick a new class name before implementation.",
                    "Status": "NeedsDecision",
                }
            )
    if any(row["ModifierKind"] == "CustomSignedEquipSkillMod" for row in item_skill_rows):
        review_rows.append(
            {
                "IssueId": f"MBR-{len(review_rows) + 1:03d}",
                "Severity": "P1",
                "Sheet": "ItemSkillMods",
                "RowKey": "All negative or over-five skill-mod items",
                "Issue": "Signed skill modifiers cannot use stock AosSkillBonuses packing",
                "Recommendation": "Implement custom equip/unequip SkillMod handling for those items.",
                "Status": "ImplementationConstraint",
            }
        )
    for row in enhanced_mobile:
        if row["ChangeId"] in {"MC-034", "MC-035"} and not row["Priority"]:
            review_rows.append(
                {
                    "IssueId": f"MBR-{len(review_rows) + 1:03d}",
                    "Severity": "P2",
                    "Sheet": "MobileChanges",
                    "RowKey": row["ChangeId"],
                    "Issue": "Priority is blank for a new EpicBossCandidate row",
                    "Recommendation": "Choose High, Medium, or Low. High matches 22 of the other 24 EpicBossCandidate rows.",
                    "Status": "NeedsDecision",
                }
            )
    normalized["review-issues"] = review_rows

    return normalized


def write_csv_outputs(normalized: Dict[str, List[Dict[str, object]]]) -> None:
    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    for name, rows in normalized.items():
        path = OUTPUT_DIR / f"{name.lower().replace('_', '-')}.csv"
        headers: List[str] = list(rows[0].keys()) if rows else []
        with path.open("w", encoding="utf-8", newline="") as stream:
            writer = csv.DictWriter(stream, fieldnames=headers, lineterminator="\n")
            writer.writeheader()
            writer.writerows(rows)


def xml_attr(value: object) -> str:
    return escape(str(value), {'"': "&quot;"})


def xml_text(value: object) -> str:
    return escape(str(value))


def cell_xml(row: int, col: int, value: object, style: Optional[int] = None) -> str:
    if value is None or value == "":
        return ""
    ref = f"{col_letter(col)}{row}"
    style_attr = f' s="{style}"' if style is not None else ""
    if isinstance(value, bool):
        return f'<c r="{ref}" t="b"{style_attr}><v>{1 if value else 0}</v></c>'
    if is_number(value):
        return f'<c r="{ref}"{style_attr}><v>{xml_text(number_value(value))}</v></c>'
    return f'<c r="{ref}" t="inlineStr"{style_attr}><is><t>{xml_text(value)}</t></is></c>'


def sheet_xml(spec: SheetSpec, defined_names: Dict[str, str]) -> str:
    data_start = 5
    header_row = 4
    rows_xml: List[str] = []
    rows_xml.append(f'<row r="1">{cell_xml(1, 1, spec.title, 2)}</row>')
    rows_xml.append(f'<row r="2">{cell_xml(2, 1, spec.note, 3)}</row>')
    header_cells = [cell_xml(header_row, index + 1, header, 1) for index, header in enumerate(spec.headers)]
    rows_xml.append(f'<row r="{header_row}">{"".join(header_cells)}</row>')
    for offset, row_obj in enumerate(spec.rows):
        row_index = data_start + offset
        cells = [cell_xml(row_index, index + 1, row_obj.get(header, "")) for index, header in enumerate(spec.headers)]
        rows_xml.append(f'<row r="{row_index}">{"".join(cells)}</row>')

    col_xml: List[str] = []
    for index, header in enumerate(spec.headers, start=1):
        values = [str(row.get(header, "")) for row in spec.rows[:100]]
        width = min(max([len(header)] + [len(value) for value in values]) + 2, 45)
        col_xml.append(f'<col min="{index}" max="{index}" width="{max(width, 10)}" customWidth="1"/>')

    last_row = max(header_row, data_start + len(spec.rows) - 1)
    last_col = col_letter(len(spec.headers))
    validations_xml: List[str] = []
    header_index = {header: index + 1 for index, header in enumerate(spec.headers)}
    for validation in spec.validations:
        column_name = validation["column"]
        if column_name not in header_index:
            continue
        column = col_letter(header_index[column_name])
        sqref = f"{column}{data_start}:{column}1000"
        validation_type = validation["type"]
        formula = validation["formula"]
        if validation_type in {"decimal", "whole"}:
            validations_xml.append(
                f'<dataValidation type="{validation_type}" operator="greaterThanOrEqual" allowBlank="1" showErrorMessage="1" sqref="{sqref}"><formula1>{xml_text(formula)}</formula1></dataValidation>'
            )
        else:
            validations_xml.append(
                f'<dataValidation type="list" allowBlank="1" showErrorMessage="1" sqref="{sqref}"><formula1>{xml_text(formula)}</formula1></dataValidation>'
            )

    validation_block = ""
    if validations_xml:
        validation_block = f'<dataValidations count="{len(validations_xml)}">{"".join(validations_xml)}</dataValidations>'

    return (
        f'<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
        f'<worksheet xmlns="{MAIN_NS}" xmlns:r="{REL_NS}">'
        f'<sheetViews><sheetView workbookViewId="0"><pane ySplit="4" topLeftCell="A5" activePane="bottomLeft" state="frozen"/><selection pane="bottomLeft" activeCell="A5" sqref="A5"/></sheetView></sheetViews>'
        f'<sheetFormatPr defaultRowHeight="15"/>'
        f'<cols>{"".join(col_xml)}</cols>'
        f'<sheetData>{"".join(rows_xml)}</sheetData>'
        f'<autoFilter ref="A{header_row}:{last_col}{last_row}"/>'
        f'{validation_block}'
        f'<pageMargins left="0.7" right="0.7" top="0.75" bottom="0.75" header="0.3" footer="0.3"/>'
        f'</worksheet>'
    )


def reference_sheet_xml(normalized: Dict[str, List[Dict[str, object]]]) -> str:
    rows: Dict[int, List[str]] = {
        1: [cell_xml(1, 1, "Mobile Balance Template Reference", 2)],
        2: [
            cell_xml(
                2,
                1,
                "Use these names and formats when filling the data-entry sheets. Structured sheet headers are on row 4 and data begins on row 5.",
                3,
            )
        ],
        4: [cell_xml(4, 1, "How to Fill", 1), cell_xml(4, 4, "Common Skill Aliases", 1)],
        14: [cell_xml(14, 1, "Valid SkillName Values", 1), cell_xml(14, 4, "Common Item Attribute Names", 1)],
    }
    guidance = [
        ("Blank cells", "Mean no change or no value requested."),
        ("Skill syntax", "Use Skill=Value or Skill=Min-Max; separate multiple entries with semicolons."),
        ("Attribute syntax", "Use Attribute=Value; negative values are allowed when intended as drawbacks."),
        ("Damage fields", "Fill both DamageMin and DamageMax when changing melee damage."),
        ("Loot linkage", "Define ItemId on NewLootItems, then reference it from LootAssignments."),
        ("Drop semantics", "Use Guaranteed=No with ChancePercentOnCorpse for independent percentage rolls."),
        ("Ownership", "Use OwnerBound=No unless a binding policy has been explicitly approved."),
    ]
    aliases = [
        ("Parrying", "Parry"),
        ("Wrestling", "FistFighting"),
        ("Mace Fighting", "Bludgeoning"),
        ("Archery", "Marksmanship"),
        ("Eval Int", "Psychology"),
        ("Detect Hidden", "Searching"),
        ("Fishing", "Seafaring"),
        ("Animal Lore", "Druidism"),
        ("Magic Resist", "MagicResist"),
        ("Blacksmithing", "Blacksmith"),
        ("Merchantile", "Mercantile"),
        ("Aantomy", "Anatomy"),
    ]
    for offset in range(max(len(guidance), len(aliases))):
        row_index = 5 + offset
        cells: List[str] = []
        if offset < len(guidance):
            cells.extend(
                [
                    cell_xml(row_index, 1, guidance[offset][0]),
                    cell_xml(row_index, 2, guidance[offset][1]),
                ]
            )
        if offset < len(aliases):
            cells.extend(
                [
                    cell_xml(row_index, 4, aliases[offset][0]),
                    cell_xml(row_index, 5, aliases[offset][1]),
                ]
            )
        rows[row_index] = cells

    skills = normalized["Ref_Skills"]
    bonuses = normalized["Ref_BonusNames"]
    for offset in range(max(len(skills), len(bonuses))):
        row_index = 15 + offset
        cells = []
        if offset < len(skills):
            cells.extend(
                [
                    cell_xml(row_index, 1, skills[offset]["SkillId"]),
                    cell_xml(row_index, 2, skills[offset]["SkillName"]),
                ]
            )
        if offset < len(bonuses):
            bonus_name = str(bonuses[offset]["BonusName"])
            cells.extend(
                [
                    cell_xml(row_index, 4, bonus_name.lower()),
                    cell_xml(row_index, 5, bonus_name),
                ]
            )
        rows[row_index] = cells

    row_xml = "".join(f'<row r="{row_index}">{"".join(rows[row_index])}</row>' for row_index in sorted(rows))
    return (
        f'<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
        f'<worksheet xmlns="{MAIN_NS}" xmlns:r="{REL_NS}">'
        f'<sheetViews><sheetView workbookViewId="0"><pane ySplit="4" topLeftCell="A5" activePane="bottomLeft" state="frozen"/></sheetView></sheetViews>'
        f'<sheetFormatPr defaultRowHeight="15"/>'
        f'<cols><col min="1" max="1" width="22" customWidth="1"/><col min="2" max="2" width="70" customWidth="1"/>'
        f'<col min="3" max="3" width="3" customWidth="1"/><col min="4" max="4" width="28" customWidth="1"/>'
        f'<col min="5" max="5" width="28" customWidth="1"/><col min="6" max="6" width="3" customWidth="1"/></cols>'
        f'<sheetData>{row_xml}</sheetData>'
        f'<pageMargins left="0.7" right="0.7" top="0.75" bottom="0.75" header="0.3" footer="0.3"/>'
        f'</worksheet>'
    )


def build_sheet_specs(normalized: Dict[str, List[Dict[str, object]]]) -> List[SheetSpec]:
    def vals(name: str) -> str:
        return f'"{",".join(LIST_VALUES[name])}"'

    specs = [
        SheetSpec(
            "MobileChanges",
            "Mobile balance changes",
            "Staff-facing mobile change table. Use the normalized child sheets for one-row-per-skill data.",
            ["ChangeId", "MobileClassOrName", "ResolvedClassName", "KnownFilePath", "Scope", "DamageMin", "DamageMax", "SkillChangesRaw", "LootAssignmentIds", "Priority", "ReviewStatus", "ImplementationStatus", "Notes", "CodexNotes"],
            normalized["MobileChanges"],
            [
                {"column": "MobileClassOrName", "type": "list", "formula": "MobileDisplayList"},
                {"column": "ResolvedClassName", "type": "list", "formula": "MobileClassList"},
                {"column": "Scope", "type": "list", "formula": vals("ScopeList")},
                {"column": "DamageMin", "type": "decimal", "formula": "0"},
                {"column": "DamageMax", "type": "decimal", "formula": "0"},
                {"column": "Priority", "type": "list", "formula": vals("PriorityList")},
                {"column": "ReviewStatus", "type": "list", "formula": vals("ReviewStatusList")},
                {"column": "ImplementationStatus", "type": "list", "formula": vals("ImplementationStatusList")},
            ],
        ),
        SheetSpec(
            "NewLootItems",
            "Loot item definitions",
            "Original staff item rows plus canonical class/base names and implementation status.",
            ["ItemId", "ClassNameRaw", "CanonicalClassName", "BaseItemRaw", "CanonicalBaseItem", "ExistingClassStatus", "DisplayName", "ItemIDGraphic", "Hue", "Layer", "LootType", "SkillBonusesRaw", "AttributesRaw", "OwnerBound", "OwnerBoundPolicyStatus", "PropertyLabel", "ImplementationStatus", "Notes", "CodexNotes"],
            normalized["NewLootItems"],
            [
                {"column": "CanonicalClassName", "type": "list", "formula": "ItemClassList"},
                {"column": "CanonicalBaseItem", "type": "list", "formula": "ItemClassList"},
                {"column": "ExistingClassStatus", "type": "list", "formula": vals("ClassStatusList")},
                {"column": "Layer", "type": "list", "formula": vals("LayerList")},
                {"column": "LootType", "type": "list", "formula": vals("LootTypeList")},
                {"column": "OwnerBound", "type": "list", "formula": vals("YesNoList")},
                {"column": "OwnerBoundPolicyStatus", "type": "list", "formula": vals("OwnerPolicyStatusList")},
                {"column": "ImplementationStatus", "type": "list", "formula": vals("ImplementationStatusList")},
            ],
        ),
        SheetSpec(
            "LootAssignments",
            "Loot assignments",
            "One row per item drop assignment. Rows with unresolved guaranteed/chance semantics are explicitly flagged.",
            ["AssignmentId", "MobileClassOrName", "ResolvedMobileClass", "ItemId", "CanonicalItemClass", "DropTiming", "ChancePercent", "QuantityMin", "QuantityMax", "Guaranteed", "DropRule", "DropSemanticsStatus", "Notes", "CodexNotes"],
            normalized["LootAssignments"],
            [
                {"column": "ResolvedMobileClass", "type": "list", "formula": "MobileClassList"},
                {"column": "CanonicalItemClass", "type": "list", "formula": "ItemClassList"},
                {"column": "DropTiming", "type": "list", "formula": vals("DropTimingList")},
                {"column": "ChancePercent", "type": "decimal", "formula": "0"},
                {"column": "QuantityMin", "type": "whole", "formula": "0"},
                {"column": "QuantityMax", "type": "whole", "formula": "0"},
                {"column": "Guaranteed", "type": "list", "formula": vals("YesNoList")},
                {"column": "DropRule", "type": "list", "formula": "DropRuleList"},
                {"column": "DropSemanticsStatus", "type": "list", "formula": vals("DropSemanticStatusList")},
            ],
        ),
        SheetSpec(
            "MobileSkillChanges",
            "Normalized mobile skill changes",
            "One row per mobile skill change, using canonical SkillName values.",
            ["SkillChangeId", "ChangeId", "MobileClassOrName", "ResolvedMobileClass", "SkillNameRaw", "CanonicalSkillName", "SkillMin", "SkillMax", "SkillValue", "Operation", "Status", "Notes"],
            normalized["MobileSkillChanges"],
            [
                {"column": "ResolvedMobileClass", "type": "list", "formula": "MobileClassList"},
                {"column": "CanonicalSkillName", "type": "list", "formula": "SkillNameList"},
                {"column": "Operation", "type": "list", "formula": '"SetSkill"'},
                {"column": "Status", "type": "list", "formula": '"Ready,NeedsSkillNameMapping,NeedsDecision"'},
            ],
        ),
        SheetSpec(
            "ItemSkillMods",
            "Normalized item skill modifiers",
            "One row per item skill modifier. Signed or over-five entries are custom equip mods, not stock SkillBonuses.",
            ["SkillModId", "ItemId", "CanonicalClassName", "SkillNameRaw", "CanonicalSkillName", "Value", "ModifierKind", "ImplementationSurface", "Status", "Notes"],
            normalized["ItemSkillMods"],
            [
                {"column": "CanonicalClassName", "type": "list", "formula": "ItemClassList"},
                {"column": "CanonicalSkillName", "type": "list", "formula": "SkillNameList"},
                {"column": "ModifierKind", "type": "list", "formula": vals("SkillModKindList")},
                {"column": "Status", "type": "list", "formula": '"Ready,NeedsSkillNameMapping,NeedsDecision"'},
            ],
        ),
        SheetSpec(
            "ItemBonuses",
            "Normalized item bonuses",
            "One row per item property or bonus, including bonuses moved out of SkillBonuses.",
            ["BonusId", "ItemId", "CanonicalClassName", "SourceColumn", "BonusNameRaw", "CanonicalBonusName", "BonusGroup", "Value", "ImplementationSurface", "Status", "Notes"],
            normalized["ItemBonuses"],
            [
                {"column": "CanonicalClassName", "type": "list", "formula": "ItemClassList"},
                {"column": "CanonicalBonusName", "type": "list", "formula": "BonusNameList"},
                {"column": "BonusGroup", "type": "list", "formula": "BonusGroupList"},
                {"column": "Status", "type": "list", "formula": vals("BonusStatusList")},
            ],
        ),
        SheetSpec(
            "Ref_Skills",
            "Skill reference",
            "Source-generated SkillName enum values from Data/System/Source/Skills.cs.",
            ["SkillId", "SkillName", "SourcePath", "Status", "Notes"],
            normalized["Ref_Skills"],
            [],
        ),
        SheetSpec(
            "Ref_ItemClasses",
            "Item class reference",
            "Source-generated item candidate classes plus proposed new item classes from this workbook.",
            ["ClassName", "BaseClass", "SourcePath", "Kind", "Status", "Notes"],
            normalized["Ref_ItemClasses"],
            [],
        ),
        SheetSpec(
            "Ref_MobileClasses",
            "Mobile class reference",
            "Source-generated mobile candidate classes and display names.",
            ["ClassName", "DisplayName", "SourcePath", "Status", "Notes"],
            normalized["Ref_MobileClasses"],
            [],
        ),
        SheetSpec(
            "Ref_BonusNames",
            "Bonus reference",
            "Canonical item bonus names and implementation surfaces used by Codex.",
            ["BonusName", "BonusGroup", "ImplementationSurface", "SupportsNegative", "Status", "Notes"],
            normalized["Ref_BonusNames"],
            [],
        ),
        SheetSpec(
            "Ref_DropRules",
            "Drop rule reference",
            "Allowed drop rule semantics. NeedsDecision rows require staff policy before code changes.",
            ["DropRule", "Meaning", "Status"],
            normalized["Ref_DropRules"],
            [],
        ),
        SheetSpec(
            "Reference",
            "Mobile Balance Template Reference",
            "Instructional reference sheet with separate sections rather than a row-4 data table.",
            [],
            [],
            [],
        ),
    ]
    return specs


def content_types_xml(sheet_count: int) -> str:
    overrides = [
        '<Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>',
        '<Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/>',
        '<Override PartName="/docProps/core.xml" ContentType="application/vnd.openxmlformats-package.core-properties+xml"/>',
        '<Override PartName="/docProps/app.xml" ContentType="application/vnd.openxmlformats-officedocument.extended-properties+xml"/>',
    ]
    for index in range(1, sheet_count + 1):
        overrides.append(f'<Override PartName="/xl/worksheets/sheet{index}.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>')
    return (
        f'<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
        f'<Types xmlns="{CONTENT_NS}">'
        f'<Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>'
        f'<Default Extension="xml" ContentType="application/xml"/>'
        f'{"".join(overrides)}'
        f'</Types>'
    )


def workbook_xml(specs: Sequence[SheetSpec], defined_ranges: Dict[str, str]) -> str:
    sheets_xml = []
    for index, spec in enumerate(specs, start=1):
        sheets_xml.append(f'<sheet name="{xml_attr(spec.name)}" sheetId="{index}" r:id="rId{index}"/>')
    defined_xml = "".join(f'<definedName name="{xml_attr(name)}">{xml_text(reference)}</definedName>' for name, reference in defined_ranges.items())
    defined_block = f"<definedNames>{defined_xml}</definedNames>" if defined_xml else ""
    return (
        f'<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
        f'<workbook xmlns="{MAIN_NS}" xmlns:r="{REL_NS}">'
        f'<workbookPr/>'
        f'<sheets>{"".join(sheets_xml)}</sheets>'
        f'{defined_block}'
        f'</workbook>'
    )


def workbook_rels_xml(sheet_count: int) -> str:
    rels = []
    for index in range(1, sheet_count + 1):
        rels.append(f'<Relationship Id="rId{index}" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet{index}.xml"/>')
    rels.append(f'<Relationship Id="rId{sheet_count + 1}" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>')
    return f'<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Relationships xmlns="{PKG_REL_NS}">{"".join(rels)}</Relationships>'


def root_rels_xml() -> str:
    return (
        f'<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
        f'<Relationships xmlns="{PKG_REL_NS}">'
        f'<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>'
        f'<Relationship Id="rId2" Type="http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties" Target="docProps/core.xml"/>'
        f'<Relationship Id="rId3" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties" Target="docProps/app.xml"/>'
        f'</Relationships>'
    )


def styles_xml() -> str:
    return (
        f'<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
        f'<styleSheet xmlns="{MAIN_NS}">'
        f'<fonts count="3">'
        f'<font><sz val="11"/><color theme="1"/><name val="Calibri"/><family val="2"/></font>'
        f'<font><b/><sz val="11"/><color theme="1"/><name val="Calibri"/><family val="2"/></font>'
        f'<font><b/><sz val="14"/><color theme="1"/><name val="Calibri"/><family val="2"/></font>'
        f'</fonts>'
        f'<fills count="4">'
        f'<fill><patternFill patternType="none"/></fill>'
        f'<fill><patternFill patternType="gray125"/></fill>'
        f'<fill><patternFill patternType="solid"><fgColor rgb="FFD9EAF7"/><bgColor indexed="64"/></patternFill></fill>'
        f'<fill><patternFill patternType="solid"><fgColor rgb="FFE2F0D9"/><bgColor indexed="64"/></patternFill></fill>'
        f'</fills>'
        f'<borders count="2"><border><left/><right/><top/><bottom/><diagonal/></border><border><left style="thin"/><right style="thin"/><top style="thin"/><bottom style="thin"/><diagonal/></border></borders>'
        f'<cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs>'
        f'<cellXfs count="4">'
        f'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"/>'
        f'<xf numFmtId="0" fontId="1" fillId="2" borderId="1" xfId="0" applyFont="1" applyFill="1" applyBorder="1"/>'
        f'<xf numFmtId="0" fontId="2" fillId="0" borderId="0" xfId="0" applyFont="1"/>'
        f'<xf numFmtId="0" fontId="0" fillId="3" borderId="0" xfId="0" applyFill="1"/>'
        f'</cellXfs>'
        f'<cellStyles count="1"><cellStyle name="Normal" xfId="0" builtinId="0"/></cellStyles>'
        f'<dxfs count="0"/><tableStyles count="0" defaultTableStyle="TableStyleMedium2" defaultPivotStyle="PivotStyleLight16"/>'
        f'</styleSheet>'
    )


def core_props_xml() -> str:
    now = datetime.now(timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ")
    return (
        f'<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
        f'<cp:coreProperties xmlns:cp="http://schemas.openxmlformats.org/package/2006/metadata/core-properties" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:dcterms="http://purl.org/dc/terms/" xmlns:dcmitype="http://purl.org/dc/dcmitype/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">'
        f'<dc:title>Confictura Mobile Balance Template</dc:title>'
        f'<dc:creator>Codex</dc:creator>'
        f'<cp:lastModifiedBy>Codex</cp:lastModifiedBy>'
        f'<dcterms:created xsi:type="dcterms:W3CDTF">{now}</dcterms:created>'
        f'<dcterms:modified xsi:type="dcterms:W3CDTF">{now}</dcterms:modified>'
        f'</cp:coreProperties>'
    )


def app_props_xml(specs: Sequence[SheetSpec]) -> str:
    sheet_names = "".join(f'<vt:lpstr>{xml_text(spec.name)}</vt:lpstr>' for spec in specs)
    return (
        f'<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'
        f'<Properties xmlns="http://schemas.openxmlformats.org/officeDocument/2006/extended-properties" xmlns:vt="http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes">'
        f'<Application>Codex</Application>'
        f'<DocSecurity>0</DocSecurity><ScaleCrop>false</ScaleCrop>'
        f'<HeadingPairs><vt:vector size="2" baseType="variant"><vt:variant><vt:lpstr>Worksheets</vt:lpstr></vt:variant><vt:variant><vt:i4>{len(specs)}</vt:i4></vt:variant></vt:vector></HeadingPairs>'
        f'<TitlesOfParts><vt:vector size="{len(specs)}" baseType="lpstr">{sheet_names}</vt:vector></TitlesOfParts>'
        f'</Properties>'
    )


def write_workbook(normalized: Dict[str, List[Dict[str, object]]]) -> None:
    CANONICAL_XLSX.parent.mkdir(parents=True, exist_ok=True)
    specs = build_sheet_specs(normalized)
    sheet_by_name = {spec.name: spec for spec in specs}
    defined_ranges = {
        "SkillNameList": f"'Ref_Skills'!$B$5:$B${4 + len(sheet_by_name['Ref_Skills'].rows)}",
        "ItemClassList": f"'Ref_ItemClasses'!$A$5:$A${4 + len(sheet_by_name['Ref_ItemClasses'].rows)}",
        "MobileClassList": f"'Ref_MobileClasses'!$A$5:$A${4 + len(sheet_by_name['Ref_MobileClasses'].rows)}",
        "MobileDisplayList": f"'Ref_MobileClasses'!$B$5:$B${4 + len(sheet_by_name['Ref_MobileClasses'].rows)}",
        "BonusNameList": f"'Ref_BonusNames'!$A$5:$A${4 + len(sheet_by_name['Ref_BonusNames'].rows)}",
        "BonusGroupList": f"'Ref_BonusNames'!$B$5:$B${4 + len(sheet_by_name['Ref_BonusNames'].rows)}",
        "DropRuleList": f"'Ref_DropRules'!$A$5:$A${4 + len(sheet_by_name['Ref_DropRules'].rows)}",
    }

    with zipfile.ZipFile(CANONICAL_XLSX, "w", compression=zipfile.ZIP_DEFLATED) as zf:
        zf.writestr("[Content_Types].xml", content_types_xml(len(specs)))
        zf.writestr("_rels/.rels", root_rels_xml())
        zf.writestr("docProps/core.xml", core_props_xml())
        zf.writestr("docProps/app.xml", app_props_xml(specs))
        zf.writestr("xl/workbook.xml", workbook_xml(specs, defined_ranges))
        zf.writestr("xl/_rels/workbook.xml.rels", workbook_rels_xml(len(specs)))
        zf.writestr("xl/styles.xml", styles_xml())
        for index, spec in enumerate(specs, start=1):
            xml = reference_sheet_xml(normalized) if spec.name == "Reference" else sheet_xml(spec, defined_ranges)
            zf.writestr(f"xl/worksheets/sheet{index}.xml", xml)


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument(
        "--source",
        type=Path,
        default=SOURCE_XLSX,
        help="Source XLSX to normalize. Defaults to the preserved July 28 staff revision.",
    )
    mode = parser.add_mutually_exclusive_group()
    mode.add_argument("--csv-only", action="store_true", help="Regenerate CSV outputs without replacing the canonical workbook.")
    mode.add_argument("--workbook-only", action="store_true", help="Regenerate the canonical workbook without replacing CSV outputs.")
    args = parser.parse_args()

    source_xlsx = args.source if args.source.is_absolute() else ROOT / args.source
    if not source_xlsx.exists():
        print(f"Source workbook not found: {source_xlsx}", file=sys.stderr)
        return 1
    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    tables = read_xlsx_tables(source_xlsx)
    missing = [sheet for sheet in CORE_SHEETS if sheet not in tables]
    if missing:
        print(f"Missing required sheets: {', '.join(missing)}", file=sys.stderr)
        return 1
    skills = parse_skills()
    classes = scan_classes()
    normalized = normalize_data(tables, skills, classes)
    if not args.workbook_only:
        write_csv_outputs(normalized)
        print(f"Wrote CSV outputs under {rel(OUTPUT_DIR)}")
    if not args.csv_only:
        write_workbook(normalized)
        print(f"Wrote {rel(CANONICAL_XLSX)}")
    print(f"Read {rel(source_xlsx)}")
    print(f"MobileChanges rows: {len(normalized['MobileChanges'])}")
    print(f"NewLootItems rows: {len(normalized['NewLootItems'])}")
    print(f"LootAssignments rows: {len(normalized['LootAssignments'])}")
    print(f"MobileSkillChanges rows: {len(normalized['MobileSkillChanges'])}")
    print(f"ItemSkillMods rows: {len(normalized['ItemSkillMods'])}")
    print(f"ItemBonuses rows: {len(normalized['ItemBonuses'])}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
