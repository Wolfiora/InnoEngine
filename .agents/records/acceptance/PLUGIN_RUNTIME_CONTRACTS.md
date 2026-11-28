# Third-party runtime plugin acceptance

The [installed fixture](../../../tests/product/fixtures/plugin-contracts/Controller.cs.txt) combines a session subsystem, two-knob input device, unsigned PCM decoder, half-volume bus processor and coroutine waiting for a voice. Its [editor importers](../../../tests/product/fixtures/plugin-contracts/Tone.editor.cs.txt) produce the audio and mixer runtime artifacts. The same sources are projected through the normal scripting compiler in Editor and deployment scopes.

## Behavior and ownership

| Issue | Entry point and observed behavior |
| --- | --- |
| [#96](https://github.com/Wolfiora/InnoEngine/issues/96) | The [installed-package regression](../../../tests/composition/editor/features/Inno.Editor.Scripting.Tests/ScriptCompilationTests.cs) starts a session, installs the contribution, observes its controller, replaces it and waits for collectible code to retire. A failing activator leaves the prior connection usable. Removing the package disconnects it. [Reload regressions](../../../tests/composition/editor/features/Inno.Editor.Scripting.Tests/ScriptReloadTests.cs) cover failed source compilation versus actual removal of dependencies. |
| [#97](https://github.com/Wolfiora/InnoEngine/issues/97) | [InputRuntimeTests](../../../tests/services/input/Inno.Input.Tests/InputRuntimeTests.cs) connect two devices of one type, capture ordered edges, observe a disconnect and read a snapshot retained across removal. The installed fixture verifies that replacement receives a fresh connection identity. |
| [#98](https://github.com/Wolfiora/InnoEngine/issues/98) | [MiniAudioDeviceTests](../../../tests/backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio.Native.Tests/MiniAudioDeviceTests.cs) render known samples through two ordered bus processors. A 0.5 source and half-volume effect produce 0.25. The backend rejects reentrant disposal while a plugin borrows a block. |
| [#99](https://github.com/Wolfiora/InnoEngine/issues/99) | The same native suite checks a 256-frame prefix, partial reads, independent concurrent playback, per-voice seek and natural completion. A pending read observes cancellation on stop and retains borrowed memory until its completion is acknowledged. |
| [#101](https://github.com/Wolfiora/InnoEngine/issues/101), [#21](https://github.com/Wolfiora/InnoEngine/issues/21) | External waits compile through both script scopes. The session owns its scheduler, enters its execution scope and retires affected owners when a type generation changes. The Core coroutine suite exercises the wait state machine. |

## Existing extension boundaries (#102)

| Contribution | How a plugin reaches the engine | Where product or backend support begins |
| --- | --- | --- |
| Render pipeline | An attributed pipeline is resolved from a project pipeline asset. [RenderRuntime.TryActivateDefaultPipeline](../../../src/services/rendering/Inno.Rendering.Runtime/RenderRuntime.Pipelines.cs) validates a candidate generation at a frame boundary and retains the last usable configuration on rejection. | Commands, formats and resources must be representable by the selected rendering device. |
| Shader graph node | [ShaderNodeCompilerRegistry](../../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerRegistry.cs) resolves a compiler generation. The [catalog](../../../src/services/rendering/Inno.Rendering.Shaders/Compilation/ShaderNodeCompilerCatalog.cs) dispatches the node by definition ID and lowers it into shader operations. | A new primitive operation needs support in the shader compiler and the selected target. |
| Source asset format | The [importer registry](../../../src/content/assets/Inno.Assets.Pipeline/Importing/Internal/AssetImporterRegistry.cs) discovers an importer by stable identity and source extension. The fixture handles its own file format and writes the standard audio metadata and encoded payload consumed by playback. | The Player needs a consumer for the emitted artifact and the selected audio codec. Editor importer code belongs to the Editor assembly. |
| Build target | [BuildDistribution](../../../build/composition/Inno.Build.Composition/BuildDistribution.cs) resolves registered build bindings. [StandardBuildDistribution](../../../build/distributions/Inno.Build.Distribution.Standard/StandardBuildDistribution.cs) supplies Windows, macOS and browser bindings. | A new shipping target needs distribution integration, a toolchain and the corresponding product/support pack. A runtime script cannot add its own publication target by installing a subsystem. |

The engine owns the declarations and their English XML. Product Manual coverage remains with InnoEngine.Docs. Issue #102 remains open.

## Reproducing the measurements

Windows 11 x64, .NET SDK 10.0.402, Release, BenchmarkDotNet 0.14.0, in-process toolchain, one launch, three warmups and five measured iterations. The [raw observations](PLUGIN_RUNTIME_MEASUREMENTS.csv) retain the unadjusted workload time, operation count and GC measurement for each case. BenchmarkDotNet's means below apply its overhead correction and outlier policy. Other build activity shared this workstation.

| Workload | Mean per operation | Managed allocation |
| --- | ---: | ---: |
| Two connected devices, one change per device, publish and capture one frame | 1,057.24 ns | 2,080 B |
| Read both device positions from the retained snapshot | 25.83 ns | No measured allocation |
| Custom decoder, open a fresh cursor and prepare the first 256 frames | 34.45 µs | No measured allocation reported by this short job |
| Offline mixer, 480 frames, dry / native low-pass / native delay / external half-volume | 42.49 / 27.10 / 25.22 / 51.94 µs | No measured allocation reported by this short job |
| Frame wait, register / tick / complete | 434.00 / 95.20 / 352.70 ns | 351 B / no measured allocation / no measured allocation |

Audio and custom-device measurements use implementation revision `53362fde` with the device benchmark in this PR. The coroutine and built-in input samples were collected earlier on this branch. The audio device is configured for 48 kHz. A 256-frame block represents 5.33 ms of audio and the 4,096-frame ring holds up to 85.33 ms per stage. The offline timings measure processing cost. Physical output latency requires observation at the device.

The [real-output probe](../../../benchmark/backends/MiniAudio/runtime/Inno.Benchmark.MiniAudio/ProcessorBufferProbe.cs) ran against the default Windows playback device at 48,000 Hz. The first fill supplied 4,096 frames. The next fill supplied 2,400, leaving an estimated 1,696 previously queued frames (35.33 ms) at the point the second pump sampled free space. The device clock advanced 1,440 frames between the two clock samples. The underrun counter read 480 frames before deliberately pausing the producer for 180 ms and 5,984 after recovery. This observation covers the backend ring and excludes driver, converter and loudspeaker latency. Run `--probe processor-buffer <report.md>` with the matched native deployment to repeat it.

The [product fixture](../../../tests/product/fixtures/plugin-contracts/README.md) includes stable imported asset identities, an authored startup scene, the project mixer selection and the commands used to reproduce package and Player publication. Its runtime script observes the actual voice completion reason and reports the first nonzero block received by the effect.

## Product and retirement observations

On Windows x64 the same installed package ran in the Release Editor, a self-contained trimmed CoreCLR game and a NativeAOT game. The Editor reached its controller connection during a 10,000-frame run. Both Players printed the controller connection, audible PCM and natural playback completion markers and finished 600 frames with exit code zero. The NativeAOT product used SDK 10.0.404 and compiler 10.0.15 on engine revision `8c9540c2`. The CoreCLR support pack predates the Editor frame-boundary correction. The original SDK 10.0.402 attempt stopped at its compiler package audit.

The installed-package regression exercises an independently compiled session subsystem that depends on the controller provider. Successful replacement re-creates both device connections. Constructor failure and exclusive-device rejection preserve both identities. Removing the consumer source and the provider package releases both connections.

A separate Windows offline probe ran a registered processor on the standard mixer bus. A change reservation was denied while its callback ran. With a change already reserved, another thread's render supplied silence without entering the old processor. Rebuilding the type catalog disposed the old instance once, and the next block ran a new instance. A forward-only decoder probe observed a rejected seek, disposal after a corrupt opening block, and a DecodeFailed completion with disposal after a later corrupt block.

The [hardware callback samples](PLUGIN_AUDIO_CALLBACK_INTERVALS.csv) were collected independently of GC measurements at revision `8c9540c2`. Twelve Release x64 launches of the existing processor-buffer probe used the default Windows output at 48 kHz. A private instrumented build of the pinned MiniAudio source stored one QueryPerformanceCounter timestamp and frame count per entry to ma_engine_data_callback_internal in a preallocated array. The device owner wrote the observations after stopping the device. Each row contains the interval between two callbacks and the expected period from the first callback's frame count. Across 328 intervals, median absolute timing error was 0.0537 ms and p95 was 9.9901 ms, including the initial burst of callbacks. Excluding the first five intervals in each launch left 268 intervals with median 0.0443 ms, p95 0.2016 ms and maximum 0.3565 ms. These observations apply to the measured output configuration.
