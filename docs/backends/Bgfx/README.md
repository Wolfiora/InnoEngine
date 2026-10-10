# Bgfx 共享 backend

[Backend 索引](../README.md) · [Wiki 首页](../../README.md)

源码、Native 与组件构建仅在 `backends/Bgfx` 维护。平台选择已有能力；复用必须验证目标 ABI、SDK、surface、线程、回调及生命周期。平台专属系统接入留在 platforms。

## 项目

- [Inno.Adapter.Rendering.Bgfx](Inno.Adapter.Rendering.Bgfx.md)
- [Inno.Build.Toolchains.Bgfx](Inno.Build.Toolchains.Bgfx.md)
- [Inno.Build.Toolchains.Bgfx.Shaders](Inno.Build.Toolchains.Bgfx.Shaders.md)
- [Inno.Build.Toolchains.Bgfx.Tools](Inno.Build.Toolchains.Bgfx.Tools.md)
- [Inno.Native.Bgfx](Inno.Native.Bgfx.md)

## 目标 Shader 的集成测试

共享 backend 的普通库构建不选择产品平台，也不包含产品 Shader。验证嵌入的 Composition、OutputTransfer 与 ImGui 时，必须显式传入所测产品的目标与 Shader 属性；MSBuild 将目标依赖部署到测试工程实际的 `TargetDir`，不能假定测试目录也包含目标名称。

WindowsX64 示例（在仓库根执行，SDK 沿工程 `global.json` 解析）：

```powershell
dotnet test backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/Inno.Adapter.Rendering.Bgfx.Tests.csproj `
  -p:InnoNativeTarget=windows-x64 `
  -p:InnoToolTarget=windows-x64 `
  -p:InnoProductBuildProperties=C:/Dev/GameEngineDev/InnoEngine/platforms/Windows/build/Inno.Build.Windows/EditorProduct.props
```

涉及实际 Native 调用时，使用发行组合解析同一目标的 `ProductNativeBuildPlan`，并经 `ProductNativeDeployment.InstallAsync` 准备该测试输出。测试配置不会向共享 backend 增加默认平台；macOS 实机应选择所属产品的属性和工具宿主，不能运行 Windows 产物替代验收。
