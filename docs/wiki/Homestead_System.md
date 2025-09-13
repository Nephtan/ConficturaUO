# Homestead System

## Overview
The Homestead system expands agriculture and food production with growable crops, cooking, brewing, juicing and wine making. Players cultivate plants, process harvests, and craft beverages and meals.

## Crops
- Seeds must be in the backpack and can't be planted while mounted. They verify the tile supports growth and ensure no overcrowding before creating a seedling【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Crops/Crops/WheatCrop.cs†L37-L73】
- Crops progress through multiple growth stages and become harvestable when fully grown; double‑clicking yields produce or reports that the crop is too young【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Crops/BaseCrop.cs†L226-L269】
- Players can remove unwanted plants via an uproot confirmation gump【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Crops/UpRootGump.cs†L28-L33】
- The system disallows working while mounted by default【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Crops/CropHelper.cs†L87-L91】

**Example:** Double‑click a Wheat Seed in your pack to plant it on suitable soil, wait for it to mature, then double‑click the crop to harvest wheat.

## Cooking
Baker's boards, cauldrons and frying pans open specialized craft menus using the Cooking skill for baking, boiling and grilling【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Cooking/DefBaking.cs†L8-L21】【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Cooking/Items/FryingPan.cs†L7-L19】

**Example:** Use a Baking tool with dough to craft bread or muffins.

## Brewing
Brewer's Tools invoke the brewing menu, letting cooks ferment hops, grain and fruit into mead, ale or cider kegs【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Brewing/Items/Tools/BrewersTools.cs†L7-L21】【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Brewing/Craft/DefBrewing.cs†L7-L22】【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Brewing/Craft/DefBrewing.cs†L100-L148】

**Example:** Combine bitter hops, barley, water and a keg to brew a keg of ale.

## Juicing
Juicer's Tools press a variety of fruits with water and sugar into juice kegs using the Cooking skill【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Juicing/Items/Tools/JuicersTools.cs†L7-L20】【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Juicing/DefJuicing.cs†L7-L22】【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Juicing/DefJuicing.cs†L95-L113】

**Example:** Use apples, water, sugar and a keg to craft a keg of apple juice.

## Winecrafting
Winecrafter Tools use the Alchemy skill to ferment specific grape varieties with sugar and yeast into wine kegs【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Dracana's Winecrafting [2.0]/Items/Craft Items/Tools/WinecraftersTools.cs†L7-L21】【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Dracana's Winecrafting [2.0]/Craft/DefWinecrafting.cs†L11-L26】【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Dracana's Winecrafting [2.0]/Craft/DefWinecrafting.cs†L108-L120】  
Staff can place grape vines with the `[addgv]` command【F:Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Dracana's Winecrafting [2.0]/Commands/AddGrapevines.cs†L18-L31】

**Example:** Harvest Chardonnay grapes from a vine, then use winecrafter tools with sugar, yeast and a keg to start a batch of Chardonnay wine.

## Audience
Players interested in farming and food production, and staff configuring agricultural content.