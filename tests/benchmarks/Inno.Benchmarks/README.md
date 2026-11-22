# Inno.Benchmarks

BenchmarkDotNet suite for backend-neutral engine hot paths: Core Events, Jobs, Coroutines, Input capture,
Render Graph build/compile, Scene update/query/transform writes, and Scene lifecycle/serialization churn.

```sh
dotnet run -c Release --project tests/benchmarks/Inno.Benchmarks -- --filter '*' --exporters json --artifacts <output-directory>
```

Use `--filter '*Scene*'` to select one class and `--job Short` for a quicker, less precise run.
Every class enables `MemoryDiagnoser`; compare `Allocated` per operation across platforms and revisions.
The project is not an `IsTestProject` and has no pass/fail thresholds; it produces measurements only.
