# Agent Instructions for DioXMLSpawn Processing

This document outlines the specific procedure for converting custom mobile configurations from the DioXMLSpawns.md file into hardcoded C# classes. This is a high-priority refactoring task.


## Primary Objective & Workflow

Your goal is to systematically process the DioXMLSpawns.md table and create C# class files for any mobile that has not yet been hardcoded.

Your workflow must be as follows:



1. **Parse the Table:** Read and parse the markdown table in DioXMLSpawns.md, which is located in this same directory.
2. **Check Status:** For each row, inspect the IsHardCoded column.
3. **Identify Task:** If the IsHardCoded column is empty or does **not** say "Complete", you must perform the hardcoding task for that row.
4. **Process Objects2 Column:** Read the string from the Objects2 column for the target row. This string contains all the information needed to create the custom mobile(s).
5. **Generate C# Files:** For each mobile defined in the Objects2 string (including those separated by OBJ=), generate a new C# class file.
6. **Update Status:** Once all C# files for a single row have been successfully generated, you **must** edit the DioXMLSpawns.md file and update the IsHardCoded column for that row to "Complete".
7. **Commit Changes:** Your final commit must include both the new C# files and the changes to the DioXMLSpawns.md file.


## C# Mobile Generation Rules

Adherence to these rules is mandatory for the successful completion of the task.



* **Inheritance:** The new C# class must inherit from the base mobile class specified in the Objects2 string (e.g., piratecaptain inherits from PirateCaptain). You must locate and analyze the base class in the existing codebase (/Data/Scripts/Mobiles/) to understand its constructor and properties before creating the new class.
* **File Naming:** New files must use PascalCase. The filename should be derived from the custom name property in the Objects2 string (e.g., name/Captain Swag becomes CaptainSwag.cs). If no name property is present, use the base mobile type as the filename.
* **File Path:** All new C# files must be placed in the \Data\Scripts\Custom\Mobiles\ directory.
* **Namespace & Comments:** Follow the general coding conventions outlined in the root AGENTS.md file (Server.Custom.Confictura namespace, well-commented code).
* **Constructor Logic:**
    * Create a [Constructable] public constructor that calls the base class constructor (: base()).
    * All custom properties from the Objects2 string must be set within this constructor.
    * You are expected to intelligently map properties from the string (e.g., hitsmaxseed/900, coldresistseed/100) to their corresponding C# properties (HitsMaxSeed = 900;, ColdResistance = 100;). Be aware that property names in the string may be case-insensitive and need to be mapped to the correct C# property.
    * **Item Handling (ADD/):** When you encounter an ADD/ statement (e.g., ADD/&lt;simplenote/name/Message.../itemID/123>), you must generate the code to create an instance of that item, set its properties, and add it to the mobile's backpack. Example: \
SimpleNote note = new SimpleNote(); \
note.Name = "Message from Blackbart Roberts"; \
note.ItemID = 17087; \
note.NoteString = "Arg, matey! I be taking me ship and crew across the worlds on the Jolly Roger! At any time I could be on any world, pillaging the seas!"; \
note.TitleString = "Location of the Jolly Roger"; \
Backpack.DropItem( note ); \

* **Handling Multiple Mobiles (OBJ=):**
    * The Objects2 string may define multiple mobiles, separated by OBJ=. Each of these is a distinct mobile that must be parsed and created in its own separate C# file.
    * A row in DioXMLSpawns.md is only considered complete when **ALL** mobiles from its Objects2 string have been successfully hardcoded.


## General AGENTS.md Rules

For general Git procedures, citation guidelines, and overall coding style, refer to the root-level AGENTS.md file. The instructions in this file are an extension of the root file and take precedence for this specific task.
