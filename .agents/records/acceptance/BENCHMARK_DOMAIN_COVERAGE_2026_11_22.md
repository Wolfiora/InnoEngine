# Benchmark domain coverage (2026-11-22)

Scope: `tests/benchmarks/Inno.Benchmarks` on branch `benchmark` after merging `origin/code-format`. Benchmark sources only; no production code changed.

## Inputs

- Engine: `benchmark` merge commit `725e8d4e` plus the benchmark commit that adds this record.
- Host: Linux x64 Devin VM, .NET SDK 10.0.402, runtime 10.0.13, BenchmarkDotNet ShortRun with the in-process emit toolchain. Measurements were taken before the style-only split into one type per file; that change did not alter benchmark logic.

## Added coverage

Asset lookup/import/cache (cold and warm Library), content pack open/read/lease, shader graph/source analysis/lowering/stage compiler cache, plugin discovery/catalog (cold and warm), hot/cold module reload and generation retirement, editor graph editing, inspection drawer resolution, file-browser search filtering and runtime artifact deployment (`PackagingBenchmarks`). `--probes <file>` records first-use latency, retained memory and cache hit/miss counts.

## Evidence

| Check | Status |
| --- | --- |
| Release build, `-m:1` | Passed. Parallel builds hit the native task-host process claim race recorded below. |
| New benchmark classes, ShortRun in-process | Passed: all reports produced. |
| Whole suite, Dry in-process | Passed: 104 benchmarks, no exceptions. |
| Probes | Passed. |
| `style check` on the 22 added or changed benchmark sources | Passed: zero findings. The existing benchmark sources keep their earlier findings and were not reformatted. |
| Dry in-process run after the style changes (new classes only) | Passed: 57 benchmarks, no exceptions. |
| Validation planner | Not applicable: the planner inventory does not own `tests/benchmarks`. |
| Windows / macOS execution of the new classes | Not executed. |
| Native shaderc, native Editor UI, game build and Player deployment | Unavailable on Linux. Replaced by a counting `IShaderCompilerToolchain`, the backend-neutral editor interaction runtime and `AssetPipeline.ExportRuntimeArtifacts`. |

## Observations

- The default out-of-process toolchain timed out after 120 seconds while building the generated benchmark project. Runs use `--inProcess`.
- Parallel Release builds failed with `Process storage cannot reuse an exited or unverifiable operation claim` in the ImGuizmo and SDL3 native projects; serialized `-m:1` builds passed.

## Standalone project migration (2026-11-26)

The suite moved from its layout in merge commit `3446dda9` to `benchmarks/Inno.Benchmark.Engine`. It runs manually outside the engine solution. The module reload benchmark uses the V1, V2 and dependency fixtures under `tests/foundation/extensibility/fixtures`.

On Windows x64 with .NET SDK 10.0.402, the serialized Release build completed with zero warnings and errors. All 35 project references resolved. The benchmark runner listed 52 methods, and one in-process Dry run of `ReloadBenchmarks.HotReloadAndRetire` completed. This run verified fixture loading and retirement. The full measurement suite was not rerun.
