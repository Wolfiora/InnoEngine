# Inno.Native.LibraryLoading

[Native 索引](README.md) · [Wiki 首页](../README.md)

该项目拥有跨平台动态库加载机制，不引用任何上层领域。

输出根以加载机制程序集所在目录为准，隔离 MSBuild Task 不向 SDK 进程目录部署 Native 文件。单文件发布没有程序集位置时，使用应用目录。该规则对所有宿主一致。

## 公开 API

| API | 当前行为 |
| --- | --- |
| `NativeDllConstants.REPO_ROOT_MARKER_FILE` | 定位源码 checkout 的标记文件名。 |
| `NativeDllConstants.NATIVE_DIR_NAME` / `LIB_DIR_NAME` | 部署 Native 目录与仓库构建产物目录名。 |
| `NativeDllLoader.LoadNativeDll(libraryName)` | 在当前部署输出中按平台文件名查找并加载动态库，为调用程序集注册解析器；缺失时抛出 `DllNotFoundException`。返回值是 Native 层持有的加载句柄。 |
| `FindNativeFile(fileName)` | 按精确文件名查找输出产物；缺失时抛出 `FileNotFoundException`。 |
| `EnsureNativeDll(libraryName)` | 查找当前平台动态库并从源码 checkout 的 `.lib` 部署；发布态使用已有部署文件。 |
| `EnsureNativeFile(fileName)` | 对任意指定文件执行同一部署规则，返回绝对路径；没有 checkout 或目标产物时明确失败。 |
| `DeployNativeFile(sourcePath, relativeOutputPath)` | 把已知源文件部署到输出 `native` 下的相对路径；拒绝绝对路径、目录越界与缺失文件。 |

调用者必须提供受信任文件名或已验证的部署路径，并管理加载句柄生命周期。该项目不提供 export 解析 API；底层适配器使用实际生成的 binding。Native handle 不进入服务层或脚本契约。

开发环境中，`EnsureNativeFile` 会把仓库 `.lib` 目录中的当前产物与应用输出目录按 SHA-256
内容身份比较，仅在内容变化时原子覆盖部署副本。该判断不依赖长度或时间戳，因此重新构建出同尺寸
二进制时也不会继续加载陈旧副本。Support Pack/Player 没有源码 checkout 时只读取已冻结的部署产物。
