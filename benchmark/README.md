# Manual engine benchmarks

Each measurement project is an independent BenchmarkDotNet executable. The directory follows the owning engine layer, as in `tests/`. Run commands from the engine root with the SDK in `global.json`.

| Project under `benchmark/` | Measurement owner |
| --- | --- |
| [Inno.Benchmark.Core.Events](foundation/core/Inno.Benchmark.Core.Events/Inno.Benchmark.Core.Events.csproj) | CoreEvent |
| [Inno.Benchmark.Core.Jobs](foundation/core/Inno.Benchmark.Core.Jobs/Inno.Benchmark.Core.Jobs.csproj) | CoreJob |
| [Inno.Benchmark.Core.Coroutines](foundation/core/Inno.Benchmark.Core.Coroutines/Inno.Benchmark.Core.Coroutines.csproj) | CoreCoroutine, CoroutineWait |
| [Inno.Benchmark.Reload](foundation/extensibility/Inno.Benchmark.Reload/Inno.Benchmark.Reload.csproj) | Reload |
| [Inno.Benchmark.Assets](content/assets/Inno.Benchmark.Assets/Inno.Benchmark.Assets.csproj) | Asset, AssetColdWarm, IncrementalAsset, Packaging |
| [Inno.Benchmark.Content](content/deployment/Inno.Benchmark.Content/Inno.Benchmark.Content.csproj) | ContentPack |
| [Inno.Benchmark.Scene](content/scene/Inno.Benchmark.Scene/Inno.Benchmark.Scene.csproj) | Scene, TransformHierarchy, SceneSerialization, SceneChurnPhase |
| [Inno.Benchmark.Runtime](runtime/engine/Inno.Benchmark.Runtime/Inno.Benchmark.Runtime.csproj) | SubsystemLookup |
| [Inno.Benchmark.Plugins](runtime/plugins/Inno.Benchmark.Plugins/Inno.Benchmark.Plugins.csproj) | PluginScan, PluginColdScan, ChangedPlugin |
| [Inno.Benchmark.Input](services/input/Inno.Benchmark.Input/Inno.Benchmark.Input.csproj) | Input |
| [Inno.Benchmark.Rendering](services/rendering/Inno.Benchmark.Rendering/Inno.Benchmark.Rendering.csproj) | RenderGraph, RenderResource |
| [Inno.Benchmark.Shaders](services/rendering/Inno.Benchmark.Shaders/Inno.Benchmark.Shaders.csproj) | Shader, ShaderSource |
| [Inno.Benchmark.Bgfx.Shaderc](backends/Bgfx/build/Inno.Benchmark.Bgfx.Shaderc/Inno.Benchmark.Bgfx.Shaderc.csproj) | NativeShader |
| [Inno.Benchmark.MiniAudio](backends/MiniAudio/runtime/Inno.Benchmark.MiniAudio/Inno.Benchmark.MiniAudio.csproj) | OfflineAudio |
| [Inno.Benchmark.Editor](composition/editor/framework/Inno.Benchmark.Editor/Inno.Benchmark.Editor.csproj) | EditorGraph, GraphHistory, EditorInspection |
| [Inno.Benchmark.Editor.Search](composition/editor/features/Inno.Benchmark.Editor.Search/Inno.Benchmark.Editor.Search.csproj) | EditorSearch |

Build and select cases in one project:

```text
dotnet build benchmark/runtime/engine/Inno.Benchmark.Runtime -c Release
dotnet run --project benchmark/runtime/engine/Inno.Benchmark.Runtime -c Release --no-build -- --list
dotnet run --project benchmark/runtime/engine/Inno.Benchmark.Runtime -c Release --no-build -- --check-case "SubsystemLookupBenchmarks.ResolveInterface/factoryCount=16"
dotnet run --project benchmark/runtime/engine/Inno.Benchmark.Runtime -c Release --no-build -- --case "SubsystemLookupBenchmarks.ResolveInterface/factoryCount=16" --inProcess
```

Each project lists only its own cases and probes. For example, the rendering project owns the resource-read probe:

```text
dotnet run --project benchmark/services/rendering/Inno.Benchmark.Rendering -c Release -- --probe render artifacts/benchmarks/render.md
```

To build all domains and the product process metrics hook manually, use `dotnet build benchmark/Inno.Benchmarks.slnx -c Release -m:1`. To list the six external product scenarios, use `dotnet run --project benchmark/product/Inno.Benchmark.Product -c Release -- list`. Neither discovery command runs a workload.

`--list` discovers BenchmarkDotNet methods, arguments and parameter values without initializing fixtures. Copy an exact ID to `--case`; IDs are case sensitive. `--check-case` accepts one or more IDs and only validates selection. BenchmarkDotNet options such as `--filter`, `--job` and `--artifacts` also work. The in-process toolchain uses the built Release output and its native deployment. A Dry run checks execution only. Keep statistical comparisons on the same job, binary, hardware and power configuration.

BDN reports elapsed time and managed allocation per operation. A batch counts as one operation unless its method declares `OperationsPerInvoke`. JSON reports, logs and binary/environment metadata go under `artifacts/benchmarks/<run-id>/` by default. Use a fresh directory when specifying `--artifacts`. `--probe <name> <output.md>` writes counters and a neighboring metadata file. Each manual invocation starts a fresh process. `--probes <output.md>` runs each probe owned by the selected project in a separate child, keeps individual reports and joins them. A nonzero exit denotes invalid selection or failed execution. Exact native case selection returns 3 for unavailable prerequisites.

### Measurement inventory

| Cases / probes | Timed boundary, input and completion signal |
| --- | --- |
| CoreEvent, CoreJob | Emit, enqueue/flush and subscription costs use the existing listener counts. ParallelFor completes 65,536 elements in batches of 1,024; compare 1 and 32 arithmetic steps under each scheduler. |
| CoreCoroutine, CoroutineWait | The original yield-null tick and launch-to-completion remain. For frames, seconds, incomplete Task and live handle, register 100 queued routines on their first tick, or tick them after registration. The handle's dependency remains live. |
| Input, SubsystemLookup | Capture an idle or eventful frame; same-frame press/release queries both edges. Compare 16 reads on a prepared snapshot with 16 reads in an already bound execution context. Resolve one interface in a real Play session with 1, 16 or 64 supplied factories plus the built-in scene factory. |
| SceneChurnPhase, Scene, TransformHierarchy, SceneSerialization | Time creation and unloading independently for objects alone, with data, or with a behavior. World updates, component queries and flat writes keep their own cases. Compare a 1,000-node flat or depth-10 hierarchy; measure an ancestor write and the first leaf read after a write separately. Serialize a prebuilt data-only scene or deserialize fixed bytes; scene attachment and release occur outside the timed deserialize call. |
| Asset, AssetColdWarm, IncrementalAsset, ContentPack, Packaging | Separate in-memory lookup/load, first import into an empty Library, populated Library reopen, unchanged rescan, importing one file into a populated catalog, pack open/lease/read, and export to an empty output. Cold Library/store refers to application state; OS page cache is uncontrolled. The incremental import includes public pipeline refresh and mutation publication. |
| PluginColdScan, PluginScan, ChangedPlugin, Reload | Scan empty or populated Library, restart the service, project the catalog, or replace exactly one package before measuring. Vary package count and asset payload bytes independently in the replacement case. Load/retire and cold-host construction use real module fixtures. `probe/reload` additionally reports activation, explicit retirement wait, forced collections, ALC deltas and final unload completion. |
| RenderGraph, RenderResource | Graph build/compile is separate from resource handling. Texture/material first and repeated prewarm use the real content artifact provider over an in-memory content store. `probe/render` counts actual stream opens/bytes and device calls. Raw acquisition always constructs the 320 KiB input before lookup; compare identical and changed revisions, and measure isolated input construction. The counting device records submissions; timings describe CPU work. |
| Shader, ShaderSource, NativeShader | Managed graph building, source analysis, lowering, and hit/miss use the counting toolchain. `probe/shader-entries` inserts 257 small stages; `probe/shader-bytes` inserts seven 5 MiB results. Both revisit earlier stages and count compiler invocations. NativeShader compiles fixed float4 fragment IR with an empty stage cache per iteration and records the actual shaderc path, SHA-256 and target API. |
| OfflineAudio | MiniAudio no-device mode uses the same mono 48 kHz PCM WAV, loop and 10 ms update for dry, low-pass and delay. Clip preparation and first mix are outside block timing. DecodeToReady starts with a fresh device and times submission through observed Ready, including polling. Allocations concern the measuring thread; the offline device measures no physical callback jitter. |
| EditorGraph, GraphHistory, EditorInspection, EditorSearch | Full graph editing is an end-to-end workload. Separate prebuilt 64/512-node cases move one node with an empty history, or undo and redo exactly that move from depth one. Drawer registration/resolution and public entry enumeration plus text filtering remain independent of native drawing. |

The `assets`, `content`, `plugins`, `editor` and `shaders` probes preserve cold/warm, retained-heap and cache observations. Forced collections and ALC counts describe the fixture and observation window. They are not leak thresholds.

### Native prerequisites

For NativeShader select Windows x64 (D3D11) or macOS arm64 (Metal). Set `INNO_BENCHMARK_SHADERC` to the absolute shaderc executable and `INNO_BENCHMARK_SHADER_INCLUDES` to its matching include directory. For OfflineAudio use the Release MiniAudio native deployment, including its deployment index under `native/` next to the built loader assembly. From the engine root, build the benchmark output with this command.

```text
dotnet build benchmark/backends/MiniAudio/runtime/Inno.Benchmark.MiniAudio -c Release
```

Copy the complete `native/` directory from the matching Release product into `benchmark/backends/MiniAudio/runtime/Inno.Benchmark.MiniAudio/bin/Release/net10.0/`. Keep the deployment index and its payloads together. The in-process job reads this deployment.

```text
dotnet run --project benchmark/backends/MiniAudio/runtime/Inno.Benchmark.MiniAudio -c Release --no-build -- --case OfflineAudioBenchmarks.MixDry --inProcess
```

### Product processes

The product project builds the process metrics hook used for frame measurements. Run the product benchmark from the engine checkout on a Windows or macOS desktop host. Supply a prebuilt Release executable or managed entry DLL. Use a disposable copy of the input project because Editor and Build may update project-local state. Pin the project, packaged plugins and scene through `--input-revision` (commit plus package hashes for external inputs). The runner also hashes the actual executable.

```text
dotnet run --project benchmark/product/Inno.Benchmark.Product -c Release -- player-short --executable "<Player executable>" --graphics-api "<graphics API>" --hardware "<hardware description>" --input-revision "<input revision>"
dotnet run --project benchmark/product/Inno.Benchmark.Product -c Release -- editor --executable "<Editor executable>" --project "<project directory>" --graphics-api "<graphics API>" --hardware "<hardware description>" --input-revision "<input revision>"
dotnet run --project benchmark/product/Inno.Benchmark.Product -c Release -- game-warm --executable "<Build executable>" --project "<project directory>" --input-revision "<input revision>" --target "<target ID>" --tools-target "<host target ID>" --support-packs "<support pack directory>" --startup-scene "<startup scene>" --expected-artifact "<relative artifact path>"
dotnet run --project benchmark/product/Inno.Benchmark.Product -c Release -- support-pack --executable "<Build executable>" --input-revision "<engine revision>" --target "<target ID>" --tools-target "<host target ID>" --expected-artifact "<relative artifact path>"
```

* **Player:** short = 600 frames, long = 6,000. Use the same prebuilt product and scene for both. The runner verifies the reported frame count and records final-frame views, draws and dispatches. The current Player enables vsync. GPU time is left empty.
* **Editor:** 600 visible-window frames in the supplied project. The measurement covers the entire process, including startup and shutdown. The runner verifies the frame-limit and shutdown messages.
* **Game build:** cold starts with an empty output path. Warm first builds to that path outside timing, verifies the requested artifact, then repeats the identical command. Both consume an explicit existing support-pack directory. SDK, native and OS caches are left intact.
* **Support pack:** an independent invocation of the production CLI with a fresh destination. `--expected-artifact` selects a required file relative to that destination; a successful build records its hash.
* Each invocation creates a unique output directory under `artifacts/benchmarks/products`. Keep its JSON, stdout, stderr and managed process counters. Wall time covers launch through exit; CPU covers the launched process only. Frame cases use a BCL-only .NET startup hook to capture total managed allocations and per-generation collection counts from hook initialization through process exit. Startup-hook-incompatible deployments retain available process evidence with status unavailable (exit 3). Actual product failures, timeouts and missing expected artifacts are errors.

Use FlappyBird only with mutually compatible project scripts, plugins and Player. A previous package or smoke trace cannot identify the current checkout. No product binaries, device setup or timing jobs are required to list the inventory.
