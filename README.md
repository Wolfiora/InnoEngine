# InnoEngine

**Innovate As You Want.** A plugin-oriented game engine written in C#.

Build your game's rendering models, gameplay systems, and Editor tools on a shared foundation. InnoEngine provides backend-neutral mechanisms, explicit platform composition, and a common asset and plugin workflow.

[Get started](#get-started) · [Wiki](docs/README.md) · [Architecture](docs/architecture/ENGINE_ARCHITECTURE_OVERVIEW.md) · [Extend the engine](docs/architecture/PLATFORM_EXTENSION_GUIDE.md)

![InnoEngine.Editor](preview.png)

> Active development. APIs and project data evolve together. See [current validation and limitations](docs/architecture/CURRENT_ACCEPTANCE.md) before relying on a product path.

## Get started

**Requirements:** Git, a native x64/ARM64 .NET 9 SDK, and your host's C++ toolchain. Windows x64 requires Visual Studio 2022 C++ desktop tools, Windows SDK, and C++ CMake tools. macOS ARM64 requires Xcode command line tools and CMake on PATH. [Host setup and first launch](docs/platform/PRODUCT_STARTUP.md#from-zero).

```sh
git clone --branch kernel --recurse-submodules https://github.com/Wolfiora/InnoEngine.git
cd InnoEngine
```

Open **InnoEngine.sln** in Rider or Visual Studio. Choose one of two startup projects:

| Start | What it does |
|---|---|
| **InnoEngine.Editor** | Builds the current host Editor through project references and opens a project-folder picker. |
| **InnoEngine.Build** | Prepares host Editor support by default; `editor` builds the complete Editor. Cleaning is explicit. |

The same commands work from a terminal:

```sh
dotnet run --project "entry/InnoEngine.Editor/InnoEngine.Editor.csproj"
```

Use the branch containing these changes; the command above selects `kernel`. BGCS is restored from pinned NuGet packages, and third-party sources are submodules under `extern/`. The first build needs network access and prepares native support and bindings before opening a window. Subsequent builds compile changed C# dependencies and reuse valid support caches. No prior Build run, shared `.run` file, or Browser workload is required for a desktop Editor. Game scripts and assets use import and reload; game exports prepare their own Player support packs.

Have a project already? Pass its directory after `--`. To try FlappyBird, clone [InnoEngine.Samples](https://github.com/Wolfiora/InnoEngine.Samples) separately and select its FlappyBird folder.

[Startup, advanced commands, and game exports →](docs/platform/PRODUCT_STARTUP.md) · [Current acceptance](docs/architecture/CURRENT_ACCEPTANCE.md)

## Product targets

| Target | Products | Managed deployment |
|---|---|---|
| Windows x64 | Editor, Player | CoreCLR, NativeAOT |
| macOS ARM64 | Editor, Player | CoreCLR, NativeAOT |
| Browser Wasm32 / WebGL 2 | Player | Mono Wasm, Mono Wasm AOT |

The current build/cache refactor still requires the Windows and Web product matrix to be rerun. macOS needs validation on a macOS host. GUI, DPI, and audio device checks are tracked separately.

## Built to compose

- **Shared mechanisms.** Identity, serialization, events, assets, scenes, rendering, audio, input, storage, and execution lifecycles.
- **Replaceable implementations.** SDL3, BGFX, MiniAudio, FreeType/HarfBuzz, RmlUi, and ImGui live in reusable backends. Platform integrations connect actual SDK differences.
- **Explicit ownership.** Sessions own resources; candidate publication, Missing recovery, and verified generation retirement support hot reload.
- **Plugins shape the engine.** Rendering models, components, importers, and Editor tools share the same discovery and authoring infrastructure.

Author under `Assets/`; install `.iplugin` packages under `Plugins/`. Installed plugin content enters the existing asset pipeline as read-only sources. [Plugin workflow →](docs/plugins/Inno.Plugins.Authoring.md)

```text
entry/       Two development launchers and host dispatch
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
