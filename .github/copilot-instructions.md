# GitHub Copilot Instructions for "No Missing Gene Icons" RimWorld Mod

## Mod Overview and Purpose

**Mod Name**: No Missing Gene Icons   
**Display Name**: No Missing Gene Icons  
**Author**: Mlie  
**PackageId**: Mlie.NoMissingGeneIcons  

The "No Missing Gene Icons" mod addresses the issue of missing icons for chemical definitions in RimWorld's genetic system. When chemicals are auto-generated into genes without designated icons, they typically default to a missing icon texture, which can lead to visual inconsistency. This mod automatically generates appropriate icons for these genes, enhancing the visual cohesion and user experience within the game.

## Key Features and Systems

- **Automatic Icon Generation**: On game start, the mod iterates through all chemical definitions and checks for missing icons associated with genes. If an icon is missing, the mod generates one using a predefined graphic template.
  
- **Gene Icon Composition**: Utilizes the default drug-related gene icons, overlaid with the icon of the first registered drug using the chemical, ensuring each gene has a distinct and contextually relevant appearance.
  
- **Compatibility**: The mod also supports addiction genes added by the "Gene Extractor Tiers" extension, ensuring it's a versatile addition for modded games.

## Coding Patterns and Conventions

- **Namespace and Class Structure**: The main functionality is encapsulated in two primary C# files—`Main.cs` and `ResizeTool.cs`.
  - **Main.cs**: Contains core methods like `combineTextures`, `generateChemicalIcon`, and `getReadableTexture`, centered around the icon generation logic.
  - **ResizeTool.cs**: Responsible for resizing graphical assets to ensure icons fit properly within the game's UI.

- **Coding Practices**: Methods are camelCased, following C# standards. Use of descriptive method names promotes readability and maintainability. Logging via `logMessage` aids in debugging by providing insights into the mod's execution flow.

## XML Integration

Despite this mod not containing any XML Defs, it integrates directly with RimWorld's def-based system by working dynamically with existing ChemicalDefs at runtime. Copilot users should be aware that the mod enhances game directly through the use of C# programming without the need for XML manipulation.

## Harmony Patching

- **brrainz.harmony Library**: The mod utilizes Harmony for modifying game behavior. Harmony patches allow the mod to seamlessly integrate with RimWorld's existing systems without directly modifying core files, preserving compatibility with future updates and other mods.

- **Patching Strategy**: Copilot suggestions should facilitate the addition of patches by identifying target methods and proposing efficient transpilers or prefix/postfix methods. Ensure compatibility and stability by simulating changes in test environments.

## Suggestions for Copilot

1. **Enhanced Method Suggestions**: When suggesting methods, focus on:
   - Logical groupings of methods in each class.
   - Efficient ways to handle texture operations and graphical manipulations.
   
2. **Harmony Integration**: Propose code snippets for safely applying patches using Harmony. Emphasize methods that maintain compatibility with potential future updates.
   
3. **Error Handling**: Suggest best practices handling scenarios where a texture or resource might fail to load, ensuring the mod fails gracefully without disrupting the game.
   
4. **Optimization Tips**: For operations like texture combination and resizing, offer pathways to optimize for performance since graphical operations can be resource-intensive.

By following these guidelines, GitHub Copilot can assist mod developers in maintaining a clean, effective codebase aligned with the "No Missing Gene Icons" mod's objectives.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

