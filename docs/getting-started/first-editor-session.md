# 第一次启动 Editor

[入门索引](README.md) · [环境准备](prerequisites.md) · [项目与内容](first-project.md)

用 IDE 打开 `InnoEngine.sln`，选择 `entry/InnoEngine.Editor` 的同名可执行项目并运行。普通项目构建会准备当前宿主产品；无需先运行 Build 工具。也可在仓库根使用：

```sh
dotnet run --project entry/InnoEngine.Editor/InnoEngine.Editor.csproj
```

没有传入项目路径时出现目录选择窗口；取消正常退出。等待完整构建成功后才会打开产品窗口。首次准备较慢与启动期重新编译是不同阶段：程序本身直接调用已构建的产品入口，不在运行期间组织源码构建。

引擎 C# 改动后再次构建或运行；游戏脚本和资产由导入与 reload 处理。正常退出让 Host 结束会话、设备、后台任务与 callback，再释放产品读取保护。具体命令、退出码和取消语义见 [Editor 入口](../development/entry/InnoEngine.Editor.md)。
