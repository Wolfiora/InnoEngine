# Inno.Build.SupportPacks.Core

固定托管源码 workspace 与离线工具源码 owner 分开。准备计划冻结 `InnoShaderToolSourceRoot`，MSBuild 在复制的 Shader 输入上执行编译，但从所选原始工具 owner 取得经过完整性校验和读取保护的工具产物。复制树不创建第二份 BGFX tools 缓存。

[分类索引](README.md) · [Wiki 首页](../../README.md)

## 职责与边界

拥有 Support Pack 的准备、完整性验证、不可变产物发布与自动供给事务。依赖中立 Build、Toolchains 和 Core IO 契约；不注册内置平台，不引用具体后端工具链或 Editor。

## 初始化与工作流

Host 传入一组 IPlayerSupportPackSource，PlayerSupportPackPublisher 冻结唯一目标注册表。
`IPlayerSupportPackSource.CreatePlanAsync(PlayerSupportPackPlanningContext, CancellationToken)` 先验证源、模板、SDK 和宿主，
返回 `PlayerSupportPackPlan`。Planning context 没有输出目录；计划固定目标和 Native selection、managed host/CLI。
Publisher 核对目标并观察取消后取得写 lease，再 fresh 校验输入。先按 `preparationIdentity` 查询 `.preparations` 索引；完整 Pack 的内容哈希和目标验证通过时，直接返回读取 lease。未命中或输出损坏时才创建 staging，并调用 `plan.PrepareAsync(stagingDirectory, token)`。
source 验证完整闭包，内容哈希形成产物身份；
完整目录发布后用 AtomicFile 切换 current。独占 FileLease 协调同一目标的多进程准备。

```csharp
using System.Collections.Generic;
using Inno.Build.SupportPacks;

static PlayerSupportPackPublisher CreatePublisher(IEnumerable<IPlayerSupportPackSource> sources)
{
    return new PlayerSupportPackPublisher(sources);
}
```

内置平台由 [Standard Distribution](Inno.Build.Distribution.Standard.md) 唯一注册，Editor、CLI 和 MSBuild 复用同一分布。
SourcePlayerSupportPackProvisioner 负责从 Host 定位源码并供给当前 Pack；纯发行 Host 可以不配置该能力。

## 准备输入与内容身份

PlayerSupportPackPreparationIdentity 将组件自己的 source recipe、所选绑定配置及继承链、生成器/扩展输入、真实 SDK 求值的 managed graph、模板和平台 validator 组合成准备身份。它与实际 Pack 文件集合的内容身份分开；已准备的自包含 Pack 不要求可重建 Native/Binding 工作目录仍然存在。锁等待后和最终 current 选择前使用 fresh 校验。输入改变或验证失败保留已有 current，缺失、额外或被篡改输出需要重新准备。

相关验证覆盖并发去重、热命中、损坏/额外文件修复和输入变更拒绝。执行结果与平台范围单独保存在 [验收记录](../../../.agents/records/acceptance/current.md)。

## 所有权与失败

Publisher 拥有本次 staging、写 lease 与 current 提交，source 拥有其构建任务和取消。
`PublishAsync` 返回 `PlayerSupportPackPublication`：它唯一持有已提交 Pack 的读取保护，并通过 `diagnostics` 返回提交后的索引或 staging 清理问题。调用方保持到编译、链接或复制完成再释放，CLI、MSBuild 与自动供给 Host 分别将诊断交给自身日志。目录读取仍由中立 catalog 的 `PlayerSupportPackLease` 固定。
提交前的失败或取消保留上次完整输出；已选中的 immutable artifact 不覆盖正在使用的目录。
缺少 source、重复 ID、无效 Native/managed 输入和工具失败明确报告。
真实校验见 BuildCompositionTests、Support Pack publication、取消与缺失启动源码测试。

第三方实现 `IPlayerSupportPackSource` 和继承 `PlayerSupportPackPlan`。protected 构造函数固定目标和输入身份，
构造函数同时接收完整输入身份；`VerifyInputsAsync` 必须重新验证当前输入，不能只检查 mtime。公开 `PrepareAsync` 完成全部任务后返回。计划只保存不可变配置与借用的实现，不持有跨阶段运行工作。
`PlayerSupportPackBuildContext.sdk` 是 `DotNetSdkDescriptor`，`PlayerSupportPackFiles` 使用记录的 CLI entry；
不在 staging 中再次根据 PATH/global.json 选择 SDK。
缺 SDK、预取消、计划为空或目标错误不创建输出；执行、校验和锁等待取消保留旧完整产物。

## 当前缓存行为

当前 publisher 在准备前通过 `preparationIdentity` 查询并验证完整 Pack；命中时跳过 producer。未命中时，托管编译与 PlayerLink 提取使用 `ManagedBuildExecution` 的同一份固定源码副本，SDK 与外部包保留已选择的位置。独立准备入口是 `InnoEngine.Build support-pack --target <target>`。严格输入校验、输出校验与读取保护仍保留。

## Pack 与游戏发布的区别

Pack 保存目标所需的引擎编译引用、Native 文件以及 `PlayerLink` 模板和链接输入。它不包含某个游戏的 Assets、玩法脚本或最终可分发目录。同一个 Pack 可以由多个匹配目标的游戏发布借用，游戏发布仍分别完成自己的内容编译、代码编译和最后打包。

`FilePlayerSupportPackPreparation` 的当前顺序是：准备明确的 Native plan → 在其结果读取保护期间执行 Player managed Build → 复制引擎引用和 Native 输入 → 收集真实 Player 源码及模板 → 平台验证 → publisher 发布。该实现借用注入的 SDK provider、生成服务和 Native plan；创建的工具任务必须结束后才返回。

NativeAOT 或 Web AOT 的游戏代码需要最终编译和链接，准备好 Pack 不能省略这一步。Pack 的完整性验证证明当前文件集合及内容一致，不证明某台设备已经运行过它；发布和实机验收仍是独立门禁。

## API 参考

[完整声明、行为与调用约束](Inno.Build.SupportPacks.Core/api.md)

<!-- api-include: Inno.Build.SupportPacks.Core/api.md -->

## 项目依赖

- [Inno.Core.IO](../../modules/core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Build](Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../modules/extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
