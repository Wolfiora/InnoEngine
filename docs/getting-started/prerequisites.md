# 环境准备

[入门索引](README.md) · [下一步：启动 Editor](first-editor-session.md)

先核对仓库根 `global.json` 选择的 .NET SDK，并初始化递归子模块。NuGet 包由普通 Restore 还原；首次还原需要网络。Native 第三方源码由当前仓库的子模块提供，不手工修改生成绑定。

开发宿主目前选择 Windows x64 或 macOS ARM64。Windows 还需要 MSVC、Windows SDK 和 CMake 工具；macOS 需要原生 ARM64 SDK、Xcode 工具链与 CMake。准确安装项与检查命令统一见 [平台启动指南](../platform/PRODUCT_STARTUP.md#from-zero)，避免维护两份平台清单。

Editor 的首次构建包含所选产品的完整 Native 准备。之后复用匹配输入的缓存；缓存命中不放宽产物完整性检查。Browser 发布是单独目标，安装其 workload 不是启动桌面 Editor 的前提。
