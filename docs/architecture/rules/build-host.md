## 25. 宿主、平台目标与统一构建

- 共享 Foundation、Shell、Player Runtime 不通过 OperatingSystem.IsBrowser 判断能力。平台入口注入帧调度、模块激活、存储、日志和线程策略；Input 使用同一 Core Events 入口。
- Native 每组件只有一个项目，共同 BGCS 声明与 target profile 分开。Host 单文件位于 Generated/Bindings.cs，目标 managed 单文件位于artifacts/bindings/<component>/<target>/<generationFingerprint>/Generated/Bindings.cs；CMake 中间产物仍归对应 toolchain 的 obj/native。不同目标与生成身份不能覆盖彼此输出。
- BGCS 生成的 ABI carrier 已明确封送；所有绑定组件通过共同 MSBuild 规则启用 DisableRuntimeMarshalling，禁止让运行时重复解释声明或在各组件重复 assembly attribute。验收检查实际参数、返回与回调，不能仅检查 sizeof。
- Cpp2C 目标桥使用同一 artifacts/bindings/<component>/<target>/<generationFingerprint>/Native，与 managed 单文件经过共同 staging 验证和原子提交，不能覆盖宿主 Native/Generated。目标工具链读取当前请求的生成描述，通过 CMake 参数选择桥；Emscripten SDK/Cache/Node/Python 必须以目标项目的 MSBuild workload 选择为准，不从已安装包中猜测最高版本。
- 开发可执行项目只属于 entry 的 InnoEngine.Editor 与 InnoEngine.Build。Inno.Build.Cli 是共享应用库；组件工具链、Shader 编译、Support Pack 事务和架构验证继续作为库组合，MSBuild 使用薄 Task 适配。
- Editor 的普通 IDE/MSBuild 构建与 Publish 必须通过同一 Native 工具链和部署契约准备当前配置的完整原生闭包；构建失败不得报告可运行。运行时 Loader 只加载明确部署的文件，不承担构建、仓库缓存探测或旧符号兼容。内容完全一致的部署应保持原文件，避免重复复制或替换正在使用的 DLL。
- Support Pack 核心通过 IPlayerSupportPackSource 与 IPlayerSupportPackValidator 注册目标，不维护具体平台分支。Inno.Build.Composition 只接收中立绑定贡献；内置 target、managed deployment、Support Pack 和产品组件闭包只在 Inno.Build.Distribution.Standard 注册；Editor、CLI 与 MSBuild 注入宿主上下文后消费同一不可变 distribution。通用 Build 库禁止反向引用该组合库或 Editor。请求的目标与托管部署能力必须在 Support Pack 供给及外部工具启动前统一校验，不能先执行昂贵准备再拒绝不支持的组合。
