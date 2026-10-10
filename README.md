# InnoEngine

**Innovate As You Want.** A plugin-oriented game engine written in C#.

Build your game's rendering models, gameplay systems, and Editor tools on a shared foundation. InnoEngine provides backend-neutral mechanisms, explicit platform composition, and a common asset and plugin workflow.

[Get started](#get-started--windows-x64) · [Wiki](docs/README.md) · [Architecture](docs/architecture/ENGINE_ARCHITECTURE_OVERVIEW.md) · [Extend the engine](docs/architecture/PLATFORM_EXTENSION_GUIDE.md)

![InnoEngine Editor](preview.png)

> Active development. APIs and project data evolve together. Current validation and outstanding device checks are recorded in the [acceptance report](docs/architecture/BACKEND_RUNTIME_BOUNDARY_OPTIMIZATION_ACCEPTANCE.md).

## Get started · Windows x64

**Requirements:** Git, .NET 9 SDK for InnoEngine, and Visual Studio 2022 with **Desktop development with C++**, Windows SDK, and C++ CMake tools. BindGen-CS declares its own SDK in `global.json` (currently .NET 10).

Keep the engine, binding generator, and samples beside each other:

```powershell
git clone --recurse-submodules https://github.com/Wolfiora/InnoEngine.git
git clone https://github.com/FLwolfy/BindGen-CS.git
git clone https://github.com/Wolfiora/InnoEngine.Samples.git
cd InnoEngine

dotnet run --project platforms/Windows/editor/Inno.Editor.Windows -c Debug -- ../InnoEngine.Samples/FlappyBird
```

The first build prepares native libraries, bindings, and Editor shaders automatically. Open the FlappyBird scene and press **Play**.

In an IDE, select **Inno.Editor.Windows** as the startup project and pass the sample's absolute directory as its first program argument. Build that project explicitly; platform products are intentionally excluded from the default solution build.

For macOS startup and Windows/Web game exports, see [Product startup](docs/platform/PRODUCT_STARTUP.md). Players run the exported game, including its code and content.

## Product targets

| Target | Products | Managed deployment |
|---|---|---|
| Windows x64 | Editor, Player | CoreCLR, NativeAOT |
| macOS ARM64 | Editor, Player | CoreCLR, NativeAOT |
| Browser Wasm32 / WebGL 2 | Player | Mono Wasm, Mono Wasm AOT |

Windows and Web have publish-and-run evidence. macOS actual-host validation and remaining GUI, DPI, and audio device checks are tracked separately in the acceptance report. Linux currently contributes native tooling only.

## Built to compose

- **Shared mechanisms.** Identity, serialization, events, assets, scenes, rendering, audio, input, storage, and execution lifecycles.
- **Replaceable implementations.** SDL3, BGFX, MiniAudio, FreeType/HarfBuzz, RmlUi, and ImGui live in reusable backends. Platform integrations connect actual SDK differences.
- **Explicit ownership.** Sessions own resources; candidate publication, Missing recovery, and verified generation retirement support hot reload.
- **Plugins shape the engine.** Rendering models, components, importers, and Editor tools share the same discovery and authoring infrastructure.

Author under `Assets/`; install `.iplugin` packages under `Plugins/`. Installed plugin content enters the existing asset pipeline as read-only sources. [Plugin workflow →](docs/plugins/Inno.Plugins.Authoring.md)

```text
src/         Shared foundation, content, services, runtime, and product hosting
backends/    Reusable adapters, native bindings, and component build recipes
platforms/   System/SDK integration, product entry points, and packaging
build/       Common pipelines, toolchains, and explicit distribution composition
tests/       Domain and cross-component contract tests
extern/      Third-party source dependencies
docs/        Architecture, workflows, and API Wiki
```

New platforms select backends and supply the required integrations. New backends implement domain contracts and join the explicit product/build composition. [Extension guide →](docs/architecture/PLATFORM_EXTENSION_GUIDE.md)

## Learn more

| Start here | Find |
|---|---|
| [API Wiki](docs/README.md) | Project pages and public contracts |
| [Scripting](docs/scripting/README.md) | Game code and extension APIs |
| [Build](docs/build/README.md) | CLI, native toolchains, and game exports |
| [Architecture](docs/architecture/ENGINE_ARCHITECTURE_OVERVIEW.md) | Layers, dependencies, and ownership |

## License

[Apache License 2.0](LICENSE). Third-party dependencies retain their respective licenses.
