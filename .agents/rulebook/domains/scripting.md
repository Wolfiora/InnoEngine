# Scripting

Load complete rule units. The [registry](../coverage.json) owns routing and provenance.

<!-- rule-unit: scripting -->
<a id="scripting"></a>
## 12. Scripting API manifests
- Each project participating in the script API has exactly one `Properties/ScriptingApi.cs`. Do not scatter export attributes through business source or centralize them in a manifest project that depends back on every module.
- Export each type explicitly with `ScriptingApiExport`. Do not restore metadata/property mechanisms that expose an assembly's entire public API.
- Use stable script group names, such as `InnoEngine.Scene`, `InnoEngine.Mathematics` and `InnoEditor.Inspection`, and map actual CLR namespaces with `ScriptingApiNamespace`.
- A new module, such as Rendering, changes only its own `Properties/ScriptingApi.cs`. Do not maintain a central assembly/type allowlist in the Editor compiler.
- Runtime compilation and IDE projects must use the same trimmed reference assemblies. Verify both paths after changing a manifest.
- Compilation-wide/global usings are completely prohibited, including handwritten directives, MSBuild `Using` items, implicit imports and metadata injection. Every source and script file must explicitly declare its own ordinary `using` directives.
- Scripts must use logical namespaces, such as `using InnoEngine.Scene;`, and must not directly use implementation-side `Inno.*` namespaces.
<!-- /rule-unit: scripting -->
