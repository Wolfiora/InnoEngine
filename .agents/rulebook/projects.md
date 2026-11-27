---
paths: ["**/*.csproj", "**/*.sln", "**/*.slnx", "**/*.props", "**/*.targets"]
tags: [solution, directories, new-boundary]
---
# Solutions and projects

## 10. Directory boundaries
- `src/foundation`: Core, extension catalogs, types and generation contracts.
- `src/content`: Assets, References, Scene and Animation.
- `src/services`: backend-neutral domain contracts and runtime services.
- `src/runtime`: Runtime, deployment, generators, Plugins and authoring-time scripting workflows.
- `src/adapters`: neutral SPIs, open Provider/Catalog contracts and Core Events input implementations.
- `backends/<component>`: shared backend runtime, native code and build recipes; each component has one source owner. Component tests belong to `tests/backends/<component>`.
- `platforms/<platform>`: system and SDK integration, explicit targets, thin Editor/Player entries, layouts and Support Packs. A platform must not reference another platform.
- `src/composition`: default engine, Shell, Player and Editor composition entries.
- `entry`: the two lightweight development entries and current-host routing. Other production layers must not reference it. `Inno.Packages.props` pins NuGet versions for the BGCS package family; no source-dependency entry is provided.
- `build`, `tools`, `tests`: neutral unified builds, concrete distribution registration, validation libraries and tests. All engine-owned tests, fixtures and test support belong to the root `tests` tree; upstream tests inside `extern` retain their upstream owner. Production projects must not reference test projects or test support.
- A test project is named after the production project it tests: `<Project>.Tests`, or `<Project>.Integration.Tests`, `<Project>.Native.Tests` and `<Project>.Product.Tests` for the heavier execution layers. A suite that proves one behavior across several projects is named after that behavior and listed with its reason in the architecture validator.
- The `tests` tree mirrors the production tree: a test project lives in the test directory of the project it tests, `src/<layer>/<area>` as `tests/<layer>/<area>` and every other production root under `tests` with its own name, and a cross-project suite in the directory the architecture validator lists for it.
- Test support has one owner. A helper library that several test directories use is named `Inno.Testing.*` and lives in `tests/support`; one that a single test directory uses lives in the `fixtures` directory of that directory. Module and process fixtures loaded as test input stay in the `fixtures` directory of the suite that owns them.
- GitHub workflow entry points live in `.github/workflows`, shared steps in `.github/actions`, and GitHub-specific scripts in `.github/scripts`. Build and verification behavior belongs to its managed owner. Product acceptance runs from `tests/product`; manual performance measurements live exclusively in `benchmark` and its independent solution.
- Place new files in directories matching the existing layers and responsibilities wherever possible.

## 26. Solutions and project files

- Solution Folders represent actual architectural responsibilities. Remove empty groups containing neither valid projects nor Solution Items. Avoid unnecessary single-project wrapper groups for small project collections.
- Authored projects under `entry`, `src`, `backends`, `platforms`, `build`, `tools` and `tests` belong in the Solution, including binding-generation extensions and test fixtures. Exclude `obj`, `artifacts` and independent third-party projects in extern. A project without incoming ProjectReferences is not necessarily unused: check host entries, MSBuild tool calls and generator configuration before removing it.
- Use two-space indentation in `.csproj`, `.props` and `.targets`, blank lines between top-level responsibility blocks, one property per line in long property lists, and `/` in relative ProjectReference paths.
- Merge adjacent reference groups only when their conditions and reference visibility match. Do not reorder Imports, Properties or Targets, or change conditions, metadata or evaluation order. Preserve Editor public/implementation reference boundaries.
- Detailed rules are in `.agents/rulebook/coding.md`. After reorganization, verify valid XML, Solution project identities and configurations, then run affected builds and architecture validation.
- After phases of long builds or acceptance, clean this operation's obsolete tool snapshots, private load directories and test-browser profiles, and check free disk space. Before cleanup, verify the explicit owner, resolved absolute paths, symbolic links and active processes. Retain current artifacts and acceptance logs; do not clean user or SDK global caches. Logical sizes of hardlinked/compressed files do not establish physically reclaimed space.

## 12. Solution and MSBuild readability

- Solution Folders express actual responsibility layers. Remove groups without projects or valid Solution Items. Small repositories do not need extra categories wrapping single projects.
- Include all authored source projects in the Solution, including binding-generation extensions and test fixtures; exclude generated artifacts and third-party projects. Before declaring a project without incoming references obsolete, check host entries, MSBuild tool calls and generation configuration.
- Use two-space indentation and LF in `.csproj`, `.props` and `.targets`, with one blank line between top-level responsibility blocks. Do not mix tabs and spaces.
- Keep simple declarations on one line. Put long attribute lists after the element name, one attribute per line. Do not split attribute values in ways that change paths, conditions or commands.
- Use `/` in relative ProjectReference paths. Group implementation dependencies separately from public-signature dependencies according to project rules. Do not mechanically merge groups with different conditions, visibility or responsibilities.
- Remove empty PropertyGroup/ItemGroup elements with no attributes, content or comments. Merge adjacent reference groups only when conditions and visibility match and no intervening evaluation step exists.
- MSBuild evaluates in order. Formatting-only cleanup verifies property values, conditions, metadata, Imports, Targets and effective project-reference order. General XML sorting is not safe project reorganization.
- Preserve actual platform conditions and SDK/toolchain constraints. Formatting platform projects must not move those concerns into neutral business layers.
