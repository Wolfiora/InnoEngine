```text
InnoEngine/
├── src/composition/editor/
│   ├── contracts/Inno.Editor.Annotations/
│   ├── framework/
│   │   ├── Inno.Editor.Core/Runtime/EditorStatistics.cs
│   │   ├── Inno.Editor.Diagnostics/
│   │   ├── Inno.Editor.Graph/                 # Neutral documents/controller/History
│   │   ├── Inno.Editor.Inspection/            # Reusable inspection contracts
│   │   ├── Inno.Editor.Interactions/
│   │   │   ├── Actions/EditorActionRouter.cs
│   │   │   ├── Menus/EditorMenuCatalog.cs
│   │   │   ├── Editing/EditorEditGesture.cs
│   │   │   ├── Selection/
│   │   │   ├── History/
│   │   │   └── Documents/
│   │   └── Inno.Editor.Settings/
│   ├── features/
│   │   ├── Inno.Editor.Assets/
│   │   ├── Inno.Editor.Audio/
│   │   ├── Inno.Editor.Exporting/
│   │   ├── Inno.Editor.PlayMode/
│   │   ├── Inno.Editor.Rendering/
│   │   ├── Inno.Editor.Scene/Documents/SceneEdits*.cs
│   │   ├── Inno.Editor.Scripting/
│   │   └── Inno.Editor.Shaders/
│   ├── presentation/
│   │   ├── Inno.Editor.ImGui/
│   │   │   ├── Runtime/EditorUiSession.cs
│   │   │   ├── Controls/
│   │   │   ├── Widgets/ImGuiWidget.*.cs
│   │   │   ├── Menus/
│   │   │   └── Styling/
│   │   ├── Inno.Editor.Assets.ImGui/           # Browser presentation policy
│   │   ├── Inno.Editor.Graph.ImGui/Canvas/
│   │   │   ├── GraphCanvasNode.cs
│   │   │   ├── GraphCanvasPort.cs
│   │   │   ├── GraphCanvasLayout.cs
│   │   │   ├── GraphCanvasPointerInput.cs
│   │   │   ├── GraphCanvasPointerResult.cs
│   │   │   ├── GraphCanvasPointerController.cs
│   │   │   ├── GraphCanvasView.cs
│   │   │   └── GraphCanvasView.Nodes.cs
│   │   ├── Inno.Editor.Shaders.ImGui/          # Shader-specific content
│   │   └── Inno.Editor.Workbench/
│   ├── panels/
│   │   ├── Inno.Editor.Panel.FileBrowser/
│   │   ├── Inno.Editor.Panel.GameView/
│   │   ├── Inno.Editor.Panel.Hierarchy/
│   │   ├── Inno.Editor.Panel.Inspector/
│   │   ├── Inno.Editor.Panel.Logging/Console/
│   │   ├── Inno.Editor.Panel.SceneView/
│   │   ├── Inno.Editor.Panel.Settings/
│   │   ├── Inno.Editor.Panel.ShaderEditor/Canvas/
│   │   └── Inno.Editor.Panel.Stats/FrameStatisticsSampler.cs
│   └── hosting/Inno.Editor.Hosting/
│       ├── Hosting/
│       └── Layout/
│           ├── EditorDefaultLayout.cs
│           └── DefaultEditorLayout.ini
├── src/runtime/scripting/Inno.Scripting.Compiler/
│   ├── ScriptCompiler.cs
│   ├── Compilation/ScriptCompilerEngine.cs
│   ├── ScriptCompilerOptions.cs
│   └── ScriptCompilationInputs.cs
├── build/toolchains/Inno.Build.Toolchains/Managed/
│   ├── ManagedBuildInputCapture.cs
│   ├── ManagedBuildInputReader.cs
│   ├── ManagedInputEvaluation.cs
│   └── ManagedCompilerInputs.targets
├── src/foundation/core/
│   ├── Inno.Core.IO/
│   │   ├── AtomicDirectory.cs
│   │   └── FileByteDocumentStore.cs
│   └── Inno.Core.Serialization/
│       ├── SerializationSnapshot.cs
│       ├── Capture/SerializationCaptureKeyBuilder.cs
│       ├── Converters/SerializationConverter.cs
│       └── Metadata/SerializationRevisionAttribute.cs
├── src/content/scene/Inno.Scene/
│   ├── Service/HierarchyService.cs
│   ├── Storage/SceneStore.cs
│   ├── Storage/SceneStructureSnapshot.cs
│   └── Serialization/SceneGraph/
│       ├── SceneGraphSourceIds.cs
│       └── ScenePropertyReferenceScope.cs
├── backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/
│   ├── Context/PlatformImGuiContext.cs
│   ├── Context/PlatformImGuiContext.Fonts.cs
│   └── BuiltIn/Fonts/Supplemental/             # Official font + original OFL
├── tests/
│   ├── editor/                              # Public contract/native input/lifecycle
│   ├── scripting/
│   │   ├── Inno.Editor.Scripting.Tests/
│   │   └── Inno.Scripting.Compiler.Tests/ScriptCachePublicationTests.cs
│   ├── build/Inno.Build.Tests/
│   ├── core/Inno.Core.Serialization.Tests/
│   ├── core/Inno.Core.IO.Tests/
│   ├── scene/Inno.Scene.Tests/
│   ├── tooling/Inno.Tooling.Architecture.Tests/
│   └── Invoke-EditorUiAcceptance.ps1
├── docs/architecture/
│   ├── EDITOR_UI_DESIGN_STANDARD.md
│   ├── EDITOR_UI_ARCHITECTURE.md
│   ├── CURRENT_ACCEPTANCE.md
│   └── CURRENT_ISSUES.md
└── records/architecture/
    ├── EDITOR_UI_IMPLEMENTATION_PLAN.md
    ├── EDITOR_UI_CLOSURE_PLAN.md
    └── acceptance/                           # Dated reports and portable evidence
```

All 30 Editor projects are shown. The tree lists ownership boundaries and changed key files, not every unchanged source file. No empty symmetry project, platform-specific UI project or parallel foundational service is introduced. Native generation remains owned by the existing unmodified BGCS integration and platform contributions.
