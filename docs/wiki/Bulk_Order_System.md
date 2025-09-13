# Bulk Order System

## Overview
The Bulk Order System provides crafting quests through Small and Large Bulk Order Deeds (BODs). Each deed specifies how many items to craft, any exceptional or material requirements, and tracks progress toward completion.

## Using Bulk Order Deeds
1. Obtain a small or large BOD from a blacksmith or tailor NPC.
2. Double‑click the deed in your backpack to open its gump.
3. Click **Combine** to target completed items. Small deeds accept crafted items directly, while large deeds consume completed small deeds that match their entries.
4. Once all entries reach the requested amounts, the deed is marked complete and can be turned in for rewards.

### Small Deeds
- Track the current and required item counts along with type and material details.
- The deed verifies that targeted items match the requested type, material, and exceptional status before increasing progress.

### Large Deeds
- Contain multiple entries referencing different small deeds.
- Use the combine button to target completed small deeds; each entry must reach the maximum amount.

## Rewards
Finished deeds grant gold, fame, and a reward item chosen from tables based on quantity, material, and exceptional status. The reward calculators assign points and look up the appropriate reward group.

## Configuration
- `MyServerSettings.AllowMacroResources` controls whether a CAPTCHA is shown before opening BOD gumps.
- Reward tables for smithing and tailoring are defined in `Data/Scripts/Trades/Bulk Orders/Rewards.cs` and can be adjusted by staff.

## Example
1. Request a small blacksmith BOD and receive a deed for **10 exceptional iron daggers**.
2. Craft the daggers and place them in your backpack.
3. Double‑click the deed, choose **Combine**, and target each dagger until the deed shows all 10 items.
4. Return the completed deed to the blacksmith for gold and a random reward item.

## Audience
Players