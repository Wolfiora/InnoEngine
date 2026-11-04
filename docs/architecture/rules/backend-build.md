## 29. Backend 运行接入与生成成本

- SDL 共同窗口创建与焦点读取只有一个 backend owner；平台 integration 仅处理实际 SDK 差异。共享 BGFX Device 通过自身 IBgfxSurfaceIntegration 接收借用 surface，不维护命名平台/ABI 白名单；Default catalog 必须接收明确 rendering factory。
- 附加窗口关闭先解除交互/登记，再退休 renderer surface；渲染端确认已处理 framebuffer 销毁命令后才释放原生窗口。Native callback 不等待或推进帧；最终 drain 仅在 owner thread、闭帧、无 graph/encoder 的安全点执行，失败保留依赖 owner。完成 Task 不是通用 GPU fence。
- Native 生成定义唯一属于组件 Bindings/bindings.props，MSBuild 与库调用共用；Toolchains 只依赖 INativeBindingGenerator，具体 BGCS 应用流程属于 Inno.Build.Bindings，Task 仅映射参数/取消/日志。产品闭包只请求一次完整 binding batch，托管消费保留 Native 已选指纹及逐组件 Static/Shared 请求；动态初始化开关只在 Native 编译边界消费实际链接选择，不能由平台名称或宿主推导。
- 绑定配置路径变量只从 operation 冻结的 Native SDK 环境展开；直接库与 MSBuild 使用同一目标解析，不以修改进程环境或临时 PATH 让生成器获得 SDK。缺失配置在候选创建前失败。
- 产品 binding selection 属于该产品的 SDK 中间目录，Build/Publish 后保留供内容相同的下次构建复用；只由 Native 绑定项目导入，不能让瞬态选择文件成为共同领域项目的编译输入。内容相同不重写时间戳，变化时原子替换；产品清理收回该文件。Native Publish 仍严格核对本次 managed 编译使用的闭包、指纹和源码；保留选择文件不等于持有生成缓存的读取 lease，实际消费者继续独立取得读取保护。
- 初始、批次锁后/生成后及各 Native 锁后/编译后分别 fresh 校验；只在同一明确阶段去重完整内容读取，不能跨等待/执行复用旧结果。可变源码必须复制固定，不以 hardlink 伪装不可变输入。
- 冷 CMake producer 通过共同 executor 固定 recipe 声明的工作区输入；Browser 聚合的组件 include、bridge 与参数必须解析到同一复制树。热缓存命中不复制，外部 SDK 与输出保持自己的位置和 owner，不用字符串替换整个仓库根来误重定向输出。
- Task runtime 只在 IBuildEngine4 Build 生命周期、同一完整请求身份内复用成功不可变 publication；registry 只存 BCL 数据，不持有 SDK owner、lease 或私有 ALC 类型。每个实际载入边界仍有独立 owner；失败/取消不缓存。共享 restore 写入由 Core.IO lease 协调，等待时 Yield/Reacquire，保留锁文件。
- 独立并发托管构建必须通过 SDK 的明确 ArtifactsPath 使用各自 bin/obj owner；共同规则不得覆盖该输出选择。NuGet Runtime 资产按产品真实 Restore 图选择，Native 内容寻址缓存仍通过自身 lease 共享。普通 IDE bin/obj 仅属于一个活动构建，不能把两个独立进程同时写同一路径视为受支持的并发模式。
- 同一 SDK 图中的中立与产品请求也必须隔离实际输出；使用 ArtifactsPath 时，在 SDK 建立默认路径前选择包含产品输入的 ArtifactsProjectName，保留 SDK configuration/TFM/RID pivots 和显式调用方设置。开发 Editor 入口通过现有 owned restore 准备所选产品的完整引用闭包后再构建，不能依赖平台子项目稍后还原来满足入口的直接或 SDK 添加的传递引用。
- Owned NuGet Restore 必须使用标准 `ExcludeRestorePackageImports=true`，编译使用包含已恢复 imports 的独立求值配置。不得让同一缓存 ProjectInstance 把旧 package content items 带入新程序集输出，也不得通过包名特例、补写 NuGet imports 或删除缓存掩盖混装。
- NuGet 依赖使用正常 `.csproj` 的 `PackageReference` 与 SDK Restore，不启用、生成或提交依赖锁文件，不维护逐工程、宿主、RID、AOT 或裁剪的锁清单与命名规则。包版本由正式声明维护，BGCS 包族继续使用统一精确版本；消费者读取 SDK 实际求值的 assets 和包内容。CI 使用普通还原并验证构建、包身份和源码卫生；不得以自制依赖解析器、清空真实 SDK 属性或 ignore 规则代替正确流程。用于并发读写、进程所有权和原子发布的 Core IO lease 不属于依赖锁定，必须保留。

- 引擎管理的构建 SDK 工作缓存与协调锁必须位于对应的 `artifacts` / `obj` 或显式输出 owner 内，不为源码目录中的已退出缓存位置保留全局 ignore 或验证豁免。清理旧工程目录前，核对当前工程/还原图、Git 归属、符号链接、活动进程和完整文件清单；普通运行锁仍遵守现有 owner 退休协议。远端旧分支的历史配置不是当前分支的游离文件，不得跨分支只删清单或改写历史来制造已清理的结果。
