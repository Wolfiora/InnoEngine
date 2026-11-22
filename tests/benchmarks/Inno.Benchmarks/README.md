# Inno.Benchmarks

BenchmarkDotNet suite for backend-neutral engine hot paths: Core Events, Jobs, Coroutines, Input capture,
Render Graph build/compile, Scene update/query/transform writes, and Scene lifecycle/serialization churn.
It also covers Asset lookup/import/cache (cold and warm Library), content pack open/read/lease, shader graph,
source analysis, lowering and the stage compiler cache, plugin discovery and catalog, hot/cold module reload with
generation retirement, editor graph editing, inspection drawer resolution, file-browser search, and runtime
artifact deployment.

```sh
dotnet run -c Release --project tests/benchmarks/Inno.Benchmarks -- --filter '*' --exporters json --artifacts <output-directory>
```

Build with `-m:1`: parallel builds can race on the native task-host process claim. The default toolchain
rebuilds the generated project and can exceed its two-minute build timeout, so run the built assembly in process:

```sh
dotnet build -c Release -m:1 tests/benchmarks/Inno.Benchmarks
dotnet tests/benchmarks/Inno.Benchmarks/bin/Release/net10.0/Inno.Benchmarks.dll --filter '*' --inProcess --artifacts <output-directory>
dotnet tests/benchmarks/Inno.Benchmarks/bin/Release/net10.0/Inno.Benchmarks.dll --probes <report.md>
```

`--probes` records first-use latency, retained memory and cache hit/miss counts in one process.
Native shaderc, the native editor UI and the full game build are not measured here: the shader compiler toolchain is
replaced through `IShaderCompilerToolchain`, editor classes use the backend-neutral interaction runtime, and deployment
is measured through `AssetPipeline.ExportRuntimeArtifacts`.

Use `--filter '*Scene*'` to select one class and `--job Short` for a quicker, less precise run.
Every class enables `MemoryDiagnoser`; compare `Allocated` per operation across platforms and revisions.
The project is not an `IsTestProject` and has no pass/fail thresholds; it produces measurements only.
