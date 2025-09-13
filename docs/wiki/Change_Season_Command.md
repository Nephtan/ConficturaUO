# Change Season Command

## Overview
The **Change Season** command lets administrators set the season for the current map. Supported seasons are **Spring**, **Summer**, **Autumn** (or **Fall**), **Winter**, and **Desolation**. The change is applied instantly to all players on that map.

## Usage
1. Stand on the map whose season you want to change.
2. Type `[ChangeSeason <Season>]` (for example, `[ChangeSeason Winter]`).
3. Everyone on the map immediately experiences the new season.

*Access Level: Administrator*

## How It Works
- The command validates the requested season name and maps it to an internal season index.
- The current map's `Season` property is updated.
- Every client on the map receives a `SeasonChange` packet and resends world data so visuals refresh.

## Example
```
[ChangeSeason Spring
```

## Audience
Staff