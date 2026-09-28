# Inno.Build.Toolchains

[Build 索引](README.md) · [Native](../native/README.md)

## 公开 API

- `ToolchainEnvironment`：建立受控工具进程环境。
- `BuildArtifactOptions`：描述 debug/release 与目标 artifact 选择。
- `BuildArtifactCopier`：复制明确的 Native 产物。
- `ToolchainLayout`：解析仓库内 toolchain/source/output 布局。

该项目只服务构建机器，不进入 Runtime 或 Player。路径缺失、产物歧义和进程失败必须明确抛出，不使用 fallback tool location。`BuildArtifactCopier.CopyArtifacts` 按最接近产物的 Debug/Release 目录或文件后缀选择目标配置；没有目标文件或多个源落到同一输出名时直接失败，绝不把已有 Debug DLL 当作 Release 产物发布。Windows x64 的 SDL3/BGFX 配置隔离与歧义拒绝由 `TestProject/Tools/NativeArtifact/NativeArtifactProbe.csproj` 验证。
