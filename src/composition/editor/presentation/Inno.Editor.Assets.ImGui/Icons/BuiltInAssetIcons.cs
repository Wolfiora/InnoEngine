using Inno.Editor.Assets;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.Settings;

namespace Inno.Editor.Assets.ImGui;

[AssetIcon(".txt", ImGuiIcon.C_FILE_LINES)]
[AssetIcon(".json", ImGuiIcon.C_FILE_LINES)]
[AssetIcon(".yaml", ImGuiIcon.C_FILE_LINES)]
[AssetIcon(".yml", ImGuiIcon.C_FILE_LINES)]
[AssetIcon(".md", ImGuiIcon.C_FILE_LINES)]
[AssetIcon(".xml", ImGuiIcon.C_FILE_LINES)]
[AssetIcon(".bytes", ImGuiIcon.C_FILE)]
[AssetIcon(".bin", ImGuiIcon.C_FILE)]
[AssetIcon(".dat", ImGuiIcon.C_FILE)]
[AssetIcon(".iscene", "Editor/Appearance/Icons/Scene")]
[AssetIcon(".iprefab", "Editor/Appearance/Icons/Prefab")]
[AssetIcon(".ishader", "Editor/Appearance/Icons/Shader")]
[AssetIcon(".ishadersource", "Editor/Appearance/Icons/ShaderSource")]
[AssetIcon(".imaterial", "Editor/Appearance/Icons/Material")]
[AssetIcon(".irenderpipeline", "Editor/Appearance/Icons/RenderPipeline")]
[AssetIcon(".cs", ImGuiIcon.C_FILE_CODE)]
[AssetIcon(".iasmdef", ImGuiIcon.C_GEARS)]
internal static class BuiltInAssetIcons;
