# Inno.Build.SupportPacks.Core

[分类索引](README.md) · [Wiki 首页](../README.md)

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

已通过的当前证据包括 fixture 并发只准备一次、热命中跳过 producer、损坏/额外文件修复和输入变更拒绝。真实 Windows 请求与性能正在复验，不能据此宣称完整发布矩阵或全部缓存预算已经完成。

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

## 当前源码公开 API 清单

按功能类型核对全部公开与可扩展 protected 声明。类型的组合与所有权从前文工作流进入。

### `Inno.Build.SupportPacks.FilePlayerSupportPackPreparation`

<!-- api: T:Inno.Build.SupportPacks.FilePlayerSupportPackPreparation 2eb799525289294fd50a92b0f54b1d7cbd07fc9a8d952a4c61af1f86526a819f -->
<!-- api: M:Inno.Build.SupportPacks.FilePlayerSupportPackPreparation.#ctor(Inno.Build.BuildTargetId,System.String,System.String,System.String,Inno.Build.IPlayerSupportPackValidator,System.String,System.String,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.INativeToolchainProvider,Inno.Build.Toolchains.BuildHostDescriptor,Inno.Build.Toolchains.INativeBindingGenerator,System.Collections.Generic.IReadOnlyList{System.String}) 70c303ec83b76c60a1452cbf02af12a1eeb088262c7dd42a38a79df6b372f2aa -->
<!-- api: P:Inno.Build.SupportPacks.FilePlayerSupportPackPreparation.target bd77746aabda4c1fe552d081499a7ae4a9df35712429f898143961633320f174 -->
<!-- api: M:Inno.Build.SupportPacks.FilePlayerSupportPackPreparation.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext,System.Threading.CancellationToken) d2196d68db8a5edc6c8e81f49cf019b2e662565b3ec4d7ce61678efe54c70110 -->
<!-- api: M:Inno.Build.SupportPacks.FilePlayerSupportPackPreparation.Validate(System.String) fac8a5d6857488db774283b83e91927c9f71ad79185e6324ec129dbe01dfb835 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e1c2fb6072bb6e28"></a>[`public Inno.Build.SupportPacks.FilePlayerSupportPackPreparation.FilePlayerSupportPackPreparation(Inno.Build.BuildTargetId target, string runtimeIdentifier, string nativePlatform, string nativeExtension, Inno.Build.IPlayerSupportPackValidator validator, string productProject, string templateFile, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.INativeToolchainProvider toolchainProvider, Inno.Build.Toolchains.BuildHostDescriptor host, Inno.Build.Toolchains.INativeBindingGenerator bindingGenerator, System.Collections.Generic.IReadOnlyList<string> preparationInputs)`](../../build/support/Inno.Build.SupportPacks.Core/FilePlayerSupportPackPreparation.cs#L75) | Captures the platform layout and validator without preparing any tools or files.<br>`target`：The publication platform identity.<br>`runtimeIdentifier`：The managed SDK runtime target.<br>`nativePlatform`：The native toolchain target identity required by this source.<br>`nativeExtension`：The runtime library suffix selected by this platform.<br>`validator`：The borrowed platform validator used before publication.<br>`productProject`：The explicit checkout-relative product project.<br>`templateFile`：The explicit checkout-relative publication template.<br>`nativePlan`：The frozen product component closure.<br>`toolchainProvider`：The SDK provider for the explicit native target.<br>`host`：The declared machine executing tools.<br>`bindingGenerator`：The borrowed generation service used by the frozen Native operation.<br>`preparationInputs`：The platform-owned preparation and validator implementation files included in source identity. | — | T:System.ArgumentException: A platform identifier or required target string is blank.<br>T:System.ArgumentNullException: The validator is null. |
| <a id="api-49d5d4fd16d72d61"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPlan> Inno.Build.SupportPacks.FilePlayerSupportPackPreparation.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext context, System.Threading.CancellationToken cancellationToken)`](../../build/support/Inno.Build.SupportPacks.Core/FilePlayerSupportPackPreparation.cs#L132) | Resolves tools and validates required source inputs before any staging exists.<br>返回：A frozen plan borrowing this preparation definition, or a propagated preflight failure.<br>`context`：The read-only source checkout and explicitly selected managed host.<br>`cancellationToken`：Cancels discovery before publication is permitted. | — | — |
| <a id="api-099f0bf54a36cfba"></a>[`public void Inno.Build.SupportPacks.FilePlayerSupportPackPreparation.Validate(string directory)`](../../build/support/Inno.Build.SupportPacks.Core/FilePlayerSupportPackPreparation.cs#L233) | Validates the prepared platform inputs before atomic installation.<br>`directory`：The isolated target pack directory. | — | — |
| <a id="api-6d3d34cca79420a5"></a>[`public Inno.Build.BuildTargetId Inno.Build.SupportPacks.FilePlayerSupportPackPreparation.target { get; }`](../../build/support/Inno.Build.SupportPacks.Core/FilePlayerSupportPackPreparation.cs#L118) | Gets the platform identity whose closure this source prepares. | — | — |
| <a id="api-3ce60cea9d6333af"></a>[`public sealed class Inno.Build.SupportPacks.FilePlayerSupportPackPreparation`](../../build/support/Inno.Build.SupportPacks.Core/FilePlayerSupportPackPreparation.cs#L15) | Prepares file-based product inputs using injected target tools, source locations and native closure. | — | — |

### `Inno.Build.SupportPacks.IPlayerSupportPackSource`

<!-- api: T:Inno.Build.SupportPacks.IPlayerSupportPackSource 90fc19594da2239e7b00eba1ae9f92db11b8dd8cfbb4ae31f557c1fb6df8f84b -->
<!-- api: P:Inno.Build.SupportPacks.IPlayerSupportPackSource.target eecf31a8fb0ba8e256f9072c8cf2ea062e17bdba110dbf79548e3ca27bdc07e0 -->
<!-- api: M:Inno.Build.SupportPacks.IPlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext,System.Threading.CancellationToken) 9d85f35ccaf0fe1ccc0b9c19f23a857449359547822dd2cd2eda64be33bafcca -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7fe95978bbd45c42"></a>[`System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPlan> Inno.Build.SupportPacks.IPlayerSupportPackSource.CreatePlanAsync(Inno.Build.SupportPacks.PlayerSupportPackPlanningContext context, System.Threading.CancellationToken cancellationToken)`](../../build/support/Inno.Build.SupportPacks.Core/IPlayerSupportPackSource.cs#L29) | Validates discovery inputs and freezes a plan before publication output exists.<br>返回：A frozen plan with no running work; missing tools and unsupported hosts fail here.<br>`context`：The source checkout and host used for read-only SDK discovery.<br>`cancellationToken`：Cancels preparation before installation. | — | — |
| <a id="api-06779a7a1aa048d6"></a>[`Inno.Build.BuildTargetId Inno.Build.SupportPacks.IPlayerSupportPackSource.target { get; }`](../../build/support/Inno.Build.SupportPacks.Core/IPlayerSupportPackSource.cs#L15) | Gets the stable target identity implemented by this source. | — | — |
| <a id="api-b0b9f9b7d90a0ab7"></a>[`public interface Inno.Build.SupportPacks.IPlayerSupportPackSource : Inno.Build.IPlayerSupportPackValidator`](../../build/support/Inno.Build.SupportPacks.Core/IPlayerSupportPackSource.cs#L10) | Prepares one platform's runtime and compiler inputs without owning installation or replacement. | — | — |

### `Inno.Build.SupportPacks.PlayerSupportPackBuildContext`

<!-- api: T:Inno.Build.SupportPacks.PlayerSupportPackBuildContext f75d20db3de451c908bec2f6304678b15f3cd5170fd7455d1efcf65c6b3a563d -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackBuildContext.#ctor(System.String,System.String,Inno.Build.Toolchains.DotNetSdkDescriptor) 92cec114a3349cd76b1caf7677948e05f8d241987e1fed9d129e41772feee041 -->
<!-- api: P:Inno.Build.SupportPacks.PlayerSupportPackBuildContext.engineRoot 7dee78638b25f1bf462682685f11082b21bbe89ed957b970c5f6a652c43876e2 -->
<!-- api: P:Inno.Build.SupportPacks.PlayerSupportPackBuildContext.stagingDirectory f56db9267afacbb54f2620896dd17f5ebc0b3c5ac65c90ec21a30c20ce4756b3 -->
<!-- api: P:Inno.Build.SupportPacks.PlayerSupportPackBuildContext.sdk df35006c4b689116e9f79ce39ae98669e9dbd8145c9814ebd50c8281c2612446 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-63806220a73f9ec1"></a>[`public Inno.Build.SupportPacks.PlayerSupportPackBuildContext.PlayerSupportPackBuildContext(string engineRoot, string stagingDirectory, Inno.Build.Toolchains.DotNetSdkDescriptor sdk)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L26) | Creates immutable inputs for an isolated preparation operation.<br>`engineRoot`：The complete engine source checkout.<br>`stagingDirectory`：The empty transaction directory to populate.<br>`sdk`：The project-selected SDK frozen during read-only preflight. | — | T:System.ArgumentException: A required input is blank. |
| <a id="api-bda713ef2dee6c48"></a>[`public string Inno.Build.SupportPacks.PlayerSupportPackBuildContext.engineRoot { get; }`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L42) | Gets the engine source checkout. | — | — |
| <a id="api-6a4223717034eb69"></a>[`public Inno.Build.Toolchains.DotNetSdkDescriptor Inno.Build.SupportPacks.PlayerSupportPackBuildContext.sdk { get; }`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L52) | Gets the frozen SDK host and CLI entry used without resolving another SDK. | — | — |
| <a id="api-84eba9d0f5bfcf34"></a>[`public string Inno.Build.SupportPacks.PlayerSupportPackBuildContext.stagingDirectory { get; }`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L47) | Gets the isolated directory that the source must populate. | — | — |
| <a id="api-8dead9f58893d1ec"></a>[`public sealed class Inno.Build.SupportPacks.PlayerSupportPackBuildContext`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackBuildContext.cs#L9) | Supplies preparation with explicit paths and a selected SDK, excluding installed output. | — | — |

### `Inno.Build.SupportPacks.PlayerSupportPackFiles`

<!-- api: T:Inno.Build.SupportPacks.PlayerSupportPackFiles 8a77c228e34a5dfb9ef13610ff40fa1cf7b9e9cf96e9749fc4e2d5c77c6197e1 -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackFiles.CopyPlayerSourcesAsync(Inno.Build.SupportPacks.PlayerSupportPackBuildContext,System.String,System.String,System.Threading.CancellationToken,System.Collections.Generic.IReadOnlyDictionary{System.String,System.String}) c462e528a0d6af861e9fc4851d9623f229b67cbce9418a95b16eaae7ac70395e -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackFiles.CopyCompositionInputs(System.String,System.String) c56df497f38bc8e28dd02133b4544a00486f9e9be787ecb3130538f4d7d2af03 -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackFiles.CopyReferences(System.String,System.String,System.String) 06fad39d3f89065b4ddafb0fb5eed435b7e24cf6fd2c0f31d308f6d232e963e8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a20aff7bdf3a4095"></a>[`public static void Inno.Build.SupportPacks.PlayerSupportPackFiles.CopyCompositionInputs(string engineRoot, string playerDirectory)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackFiles.cs#L90) | Stages shared registration generators and the checkout's actual SDK selection.<br>`engineRoot`：The selected complete source checkout.<br>`playerDirectory`：The isolated template input directory. | — | T:System.IO.FileNotFoundException: A required built generator is absent. |
| <a id="api-11aa8f226a7dbfc7"></a>[`public static System.Threading.Tasks.Task Inno.Build.SupportPacks.PlayerSupportPackFiles.CopyPlayerSourcesAsync(Inno.Build.SupportPacks.PlayerSupportPackBuildContext context, string project, string destination, System.Threading.CancellationToken cancellationToken, System.Collections.Generic.IReadOnlyDictionary<string, string>? environment = null)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackFiles.cs#L43) | Copies the selected project's evaluated source closure into isolated template input.<br>返回：Completion after all sources have contained, unique publication paths.<br>`context`：The selected checkout, SDK and preparation lifetime.<br>`project`：The explicit product project to evaluate.<br>`destination`：The isolated source input directory.<br>`cancellationToken`：Cancels evaluation and source preparation.<br>`environment`：Optional explicitly selected SDK environment. | — | T:System.IO.InvalidDataException: Source paths collide, escape the root, or the closure is empty. |
| <a id="api-7aad41fc2cec95ca"></a>[`public static void Inno.Build.SupportPacks.PlayerSupportPackFiles.CopyReferences(string buildOutput, string destination, string productAssemblyName)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackFiles.cs#L125) | Stages current runtime references while excluding the selected executable and project scripts.<br>`buildOutput`：The explicit product compilation output directory.<br>`destination`：The isolated compilation reference directory.<br>`productAssemblyName`：The selected executable assembly to exclude from references. | — | T:System.IO.InvalidDataException: The build produced no runtime compilation references. |
| <a id="api-77f15e25690351aa"></a>[`public static class Inno.Build.SupportPacks.PlayerSupportPackFiles`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackFiles.cs#L17) | Prepares explicit product source and reference inputs for isolated Support Pack publication. | — | — |

### `Inno.Build.SupportPacks.PlayerSupportPackPlan`

<!-- api: T:Inno.Build.SupportPacks.PlayerSupportPackPlan 55bd9b580c71aa2df669bdf57bf42743288428c762597faa4a876b1e0c14c0ba -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackPlan.#ctor(Inno.Build.BuildTargetId,System.String) e6feef06bb3ed47999230a19df8f6a9f389af4c4200ec0f8ef3cdeb25f5b6253 -->
<!-- api: P:Inno.Build.SupportPacks.PlayerSupportPackPlan.target 93adcd9b1e1892b23270b33dcf7677ba78fba4cc249be88615f311a59b668dca -->
<!-- api: P:Inno.Build.SupportPacks.PlayerSupportPackPlan.preparationIdentity d339573730bef6f69d65eb4da7e280831b057478901c37c287d27123094e910a -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackPlan.VerifyInputsAsync(System.Threading.CancellationToken) 166d7c400572183c9a631af913f65bbc0f3c8c7cac58a11d5ba36925e02d55a1 -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackPlan.PrepareAsync(System.String,System.Threading.CancellationToken) 1e52db006f7c9fbb1174b8e7f89896afcd2af50e4875dbadb0b26067782fe0c1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5f3283047c32d1b3"></a>[`protected Inno.Build.SupportPacks.PlayerSupportPackPlan.PlayerSupportPackPlan(Inno.Build.BuildTargetId target, string preparationIdentity)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlan.cs#L25) | Fixes the publication identity before preparation is permitted.<br>`target`：The nonempty target whose closure this plan prepares.<br>`preparationIdentity`：The complete lowercase SHA-256 input identity captured without preparing product output. | — | T:System.ArgumentException: The target has no stable identity. |
| <a id="api-b84b621eed025526"></a>[`public abstract System.Threading.Tasks.ValueTask Inno.Build.SupportPacks.PlayerSupportPackPlan.PrepareAsync(string stagingDirectory, System.Threading.CancellationToken cancellationToken)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlan.cs#L76) | Executes frozen inputs in publisher-owned staging without selecting tools again.<br>返回：Completion when the full closure is prepared; failures propagate without publishing.<br>`stagingDirectory`：The isolated empty directory borrowed for this execution only.<br>`cancellationToken`：Cancels execution and drains owned work before returning. | — | — |
| <a id="api-f7108439477f7bab"></a>[`public abstract System.Threading.Tasks.ValueTask Inno.Build.SupportPacks.PlayerSupportPackPlan.VerifyInputsAsync(System.Threading.CancellationToken cancellationToken)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlan.cs#L62) | Freshly verifies the planned source graph after an ownership wait or preparation work.<br>返回：Completion only when all current inputs still match the frozen preparation identity.<br>`cancellationToken`：Cancels verification before selecting any Pack. | — | T:System.InvalidOperationException: The planned inputs changed and cannot be accepted.<br>T:System.OperationCanceledException: Verification was canceled. |
| <a id="api-92f95fb7d731911e"></a>[`public string Inno.Build.SupportPacks.PlayerSupportPackPlan.preparationIdentity { get; }`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlan.cs#L45) | Gets the complete source preparation identity, independently of the resulting Pack content fingerprint. | — | — |
| <a id="api-4965555ebcbb2757"></a>[`public Inno.Build.BuildTargetId Inno.Build.SupportPacks.PlayerSupportPackPlan.target { get; }`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlan.cs#L40) | Gets the immutable target checked by the publisher before creating output. | — | — |
| <a id="api-29f8e879e058a428"></a>[`public abstract class Inno.Build.SupportPacks.PlayerSupportPackPlan`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlan.cs#L11) | Holds validated, frozen preparation inputs without owning running processes or staging resources. | — | — |

### `Inno.Build.SupportPacks.PlayerSupportPackPlanningContext`

<!-- api: T:Inno.Build.SupportPacks.PlayerSupportPackPlanningContext ade583460ad5a9b6c4b8e6ac0edbccce45b217a9df38660113eb78271b63fb93 -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackPlanningContext.#ctor(System.String,System.String) 47f3c4e16152c220655546bce02fca3a695011b13532fcdd7369314a797cfb5e -->
<!-- api: P:Inno.Build.SupportPacks.PlayerSupportPackPlanningContext.engineRoot 9e36d98dffa85846e8d1b2af8512683b659cda717a750cec95d469fe15bc8aad -->
<!-- api: P:Inno.Build.SupportPacks.PlayerSupportPackPlanningContext.dotnetHost 507a99dc436ddefe4d3a240c187e4e5c0a737c0ea103f925e34e66d3eac5967c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-aaa1875757ee78af"></a>[`public Inno.Build.SupportPacks.PlayerSupportPackPlanningContext.PlayerSupportPackPlanningContext(string engineRoot, string dotnetHost)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlanningContext.cs#L22) | Captures the checkout and host from which project-scoped tools are resolved.<br>`engineRoot`：The complete checkout to inspect without modifying publication output.<br>`dotnetHost`：The explicitly selected managed host executable. | — | T:System.ArgumentException: A required input is blank. |
| <a id="api-18882ce33f5f9384"></a>[`public string Inno.Build.SupportPacks.PlayerSupportPackPlanningContext.dotnetHost { get; }`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlanningContext.cs#L40) | Gets the explicit host from which the source freezes its SDK selection. | — | — |
| <a id="api-e7678432d376ffb0"></a>[`public string Inno.Build.SupportPacks.PlayerSupportPackPlanningContext.engineRoot { get; }`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlanningContext.cs#L35) | Gets the borrowed source checkout used for preflight. | — | — |
| <a id="api-7315a4679f5dc69e"></a>[`public sealed class Inno.Build.SupportPacks.PlayerSupportPackPlanningContext`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPlanningContext.cs#L8) | Supplies read-only discovery inputs before a publication directory exists. | — | — |

### `Inno.Build.SupportPacks.PlayerSupportPackPreparationIdentity`

<!-- api: T:Inno.Build.SupportPacks.PlayerSupportPackPreparationIdentity 4b99e6869d10c80cdd92a51b609ade3633733faa5843fc0063a1ceb0d0b358f5 -->
<!-- api: P:Inno.Build.SupportPacks.PlayerSupportPackPreparationIdentity.fingerprint 885c3591550b3bd397037efcf8cb718d4cb545c1a7c2b70175d6996090c953a4 -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackPreparationIdentity.CaptureAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.ProductNativeBuildPlan,Inno.Build.Toolchains.DotNetSdkDescriptor,System.String,System.Collections.Generic.IReadOnlyDictionary{System.String,System.String},System.Collections.Generic.IReadOnlyList{System.String},System.Threading.CancellationToken) 12441352a5b8e4560985974d8cfcc7b3b658d45ab20fe260880232bccceebef2 -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackPreparationIdentity.CreateExecutionAsync(System.Threading.CancellationToken) 9cdb9b2e8343b332a556b78b1528bf33f0fd1cc71bfb3b9df723fb885a7a3fe7 -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackPreparationIdentity.VerifyAsync(System.Threading.CancellationToken) 1f2b1c6991e1306b55ad25b3c9c6f222dbb0be4b82a380cdcf347f36c55d2cb4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ce8322f01b4d882b"></a>[`public static System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPreparationIdentity> Inno.Build.SupportPacks.PlayerSupportPackPreparationIdentity.CaptureAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.ProductNativeBuildPlan nativePlan, Inno.Build.Toolchains.DotNetSdkDescriptor sdk, string project, System.Collections.Generic.IReadOnlyDictionary<string, string> properties, System.Collections.Generic.IReadOnlyList<string> preparationInputs, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPreparationIdentity.cs#L81) | Captures Native declarations, selected binding inputs, the managed graph and explicit packaging inputs.<br>返回：A frozen identity; no Native compilation, binding generation or Player build is executed.<br>`context`：The borrowed operation with a frozen SDK and binding input provider.<br>`nativePlan`：The exact product component closure used by preparation.<br>`sdk`：The selected managed SDK also used for the actual Player build.<br>`project`：The existing Player entry project whose graph supplies managed sources.<br>`properties`：The compiler and product properties used by the managed preparation build.<br>`preparationInputs`：The complete explicit platform layout, template, validator and preparation implementation inputs.<br>`cancellationToken`：Cancels input evaluation and hashing without publishing a Pack. | — | T:System.ArgumentNullException: A required operation or input declaration is null.<br>T:System.InvalidOperationException: The selected source, SDK or generator input graph cannot be resolved.<br>T:System.OperationCanceledException: Input capture was canceled. |
| <a id="api-7b3ea35551752358"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.Toolchains.ManagedBuildExecution> Inno.Build.SupportPacks.PlayerSupportPackPreparationIdentity.CreateExecutionAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPreparationIdentity.cs#L113) | Creates the exclusively owned fixed managed execution planned by this preparation identity.<br>返回：Caller-owned input copies for compilation and source extraction.<br>`cancellationToken`：Cancels materialization without changing any installed Pack. | — | — |
| <a id="api-d2afff3d8c77fe91"></a>[`public System.Threading.Tasks.ValueTask Inno.Build.SupportPacks.PlayerSupportPackPreparationIdentity.VerifyAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPreparationIdentity.cs#L131) | Freshly verifies the complete current source input closure after waiting or preparing output.<br>返回：Completion when the current graph and bytes still match the frozen preparation identity.<br>`cancellationToken`：Cancels verification before cache acceptance or publication. | — | T:System.InvalidOperationException: Preparation inputs changed; no candidate may be selected.<br>T:System.OperationCanceledException: Verification was canceled. |
| <a id="api-5c06c7ba54908770"></a>[`public string Inno.Build.SupportPacks.PlayerSupportPackPreparationIdentity.fingerprint { get; private set; }`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPreparationIdentity.cs#L43) | Gets the frozen source input identity used to find a complete prepared Pack. | — | — |
| <a id="api-73b4749e02c9cda1"></a>[`public sealed class Inno.Build.SupportPacks.PlayerSupportPackPreparationIdentity`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPreparationIdentity.cs#L14) | Freezes a Support Pack's source preparation identity independently of reconstructible build outputs. | — | — |

### `Inno.Build.SupportPacks.PlayerSupportPackPublication`

<!-- api: T:Inno.Build.SupportPacks.PlayerSupportPackPublication a4fd9124718b4382c28849493c730ca80b656b41ebb1c5a50b6d0f824f181114 -->
<!-- api: P:Inno.Build.SupportPacks.PlayerSupportPackPublication.directory 3bd6bf289f7e86554678cd4b04a6bec4b4f5634cbbe67d103b22bfb9e7e3a5cf -->
<!-- api: P:Inno.Build.SupportPacks.PlayerSupportPackPublication.diagnostics 26e03de78339043c8c0f57bead1d683029401bd16766e6cbf399575afd4dc27c -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackPublication.Dispose 467f09ac32b58606cace63e17d5203fc4c60e27c7000f68798b5755aaa8238b9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0f39c9cd52e247de"></a>[`public void Inno.Build.SupportPacks.PlayerSupportPackPublication.Dispose()`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublication.cs#L40) | Releases the selected Pack after every reader and tool has finished; repeated disposal has no effect. | — | — |
| <a id="api-b5d5591af6ee8263"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Build.SupportPacks.PlayerSupportPackPublication.diagnostics { get; }`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublication.cs#L35) | Gets indexing or cleanup diagnostics which do not undo the successful selection. An empty collection means all post-commit work completed. | — | — |
| <a id="api-0b9c8330ed6d548d"></a>[`public string Inno.Build.SupportPacks.PlayerSupportPackPublication.directory { get; }`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublication.cs#L29) | Gets the verified Pack directory while this result holds reading ownership. | — | T:System.ObjectDisposedException: The publication's reading ownership has been released. |
| <a id="api-f9f4dd95de0c4940"></a>[`public sealed class Inno.Build.SupportPacks.PlayerSupportPackPublication : System.IDisposable`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublication.cs#L11) | Owns a successfully selected Pack and immutable diagnostics from work after its commit. | — | — |

### `Inno.Build.SupportPacks.PlayerSupportPackPublisher`

<!-- api: T:Inno.Build.SupportPacks.PlayerSupportPackPublisher d3f3bfc09386cf7c4ece5cfb84875003d27fd68a280c043d4f828a1198d0e97a -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackPublisher.#ctor(System.Collections.Generic.IEnumerable{Inno.Build.SupportPacks.IPlayerSupportPackSource}) 7eeaee66629c7ab66d06df649edcd13d04b3485e419652a7143281727a809018 -->
<!-- api: M:Inno.Build.SupportPacks.PlayerSupportPackPublisher.PublishAsync(System.String,System.String,Inno.Build.BuildTargetId,System.String,System.Threading.CancellationToken) 692aefeed4e41bd6bb317237e24079800c49446a1b47cf2aa226b174bf476a39 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-78734222f8efb1d4"></a>[`public Inno.Build.SupportPacks.PlayerSupportPackPublisher.PlayerSupportPackPublisher(System.Collections.Generic.IEnumerable<Inno.Build.SupportPacks.IPlayerSupportPackSource> sources)`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublisher.cs#L28) | Freezes the platform sources used by this publisher.<br>`sources`：Exactly one source for each supported target identity. | — | T:System.ArgumentException: A source is null or a target is registered more than once. |
| <a id="api-b8b96ea3083d50a9"></a>[`public System.Threading.Tasks.ValueTask<Inno.Build.SupportPacks.PlayerSupportPackPublication> Inno.Build.SupportPacks.PlayerSupportPackPublisher.PublishAsync(string engineRoot, string outputRoot, Inno.Build.BuildTargetId target, string dotnetHost, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublisher.cs#L81) | Prepares a target closure in isolation and publishes its immutable deployment inputs.<br>返回：Reading ownership of the selected Pack and any post-commit diagnostics. Dispose after consuming its files.<br>`engineRoot`：The source checkout containing InnoEngine.sln.<br>`outputRoot`：The directory owning installed target packs.<br>`target`：The registered target identity.<br>`dotnetHost`：The SDK executable selected by the host.<br>`cancellationToken`：Cancels preparation before directory installation begins. | Writers targeting the same pack share an exclusive, cancelable preparation lease. Publication atomically selects a fingerprint directory; previous generations remain valid for readers. | T:System.ArgumentException: A required path is blank or the target has no registered source.<br>T:System.IO.DirectoryNotFoundException: The engine checkout is unavailable.<br>T:System.IO.InvalidDataException: The prepared closure is incomplete or invalid.<br>T:System.IO.IOException: The staging tree, immutable artifact or atomic current index cannot be installed.<br>T:System.OperationCanceledException: Preparation was canceled; the installed pack is unchanged. |
| <a id="api-a9391a540e81f608"></a>[`public sealed class Inno.Build.SupportPacks.PlayerSupportPackPublisher`](../../build/support/Inno.Build.SupportPacks.Core/PlayerSupportPackPublisher.cs#L15) | Installs verified Support Packs with rollback protection using explicitly registered platform sources. | — | — |

### `Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner`

<!-- api: T:Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner 40529fb77a79277c4ca01c49e377bd3c624e2ccb6de0af3c567f6ee169020a88 -->
<!-- api: M:Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner.#ctor(System.String,Inno.Build.SupportPacks.PlayerSupportPackPublisher,System.String,System.Action{System.String}) f827eea7eee6dd841abfe4731a0dcbb7557aca97e31d9920a8f570cccc059f13 -->
<!-- api: M:Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner.TryCreateForHost(System.String,Inno.Build.SupportPacks.PlayerSupportPackPublisher,System.String,System.Action{System.String}) fb517ed4e9cc9d922300833fa9527fede921216e785d75e278ca58c2064431a1 -->
<!-- api: M:Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner.ProvisionAsync(Inno.Build.BuildTargetId,System.String,System.Threading.CancellationToken) de875f3c6a5fb09903ebf38b337de0800560a944f04f4fb4d581e6150529aefa -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-070c345d94b4ca55"></a>[`public Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner.SourcePlayerSupportPackProvisioner(string engineRoot, Inno.Build.SupportPacks.PlayerSupportPackPublisher publisher, string dotnetHost, System.Action<string> reportDiagnostic)`](../../build/support/Inno.Build.SupportPacks.Core/SourcePlayerSupportPackProvisioner.cs#L37) | Creates a provisioner for a complete engine checkout.<br>`engineRoot`：The directory containing InnoEngine.sln and the Player project.<br>`publisher`：The publisher containing the host-selected platform sources.<br>`dotnetHost`：The SDK executable explicitly selected by the host.<br>`reportDiagnostic`：The borrowed host reporter for successful-publication indexing and cleanup diagnostics. | — | T:System.IO.DirectoryNotFoundException: The directory is not an engine checkout. |
| <a id="api-2db6981bf1adb951"></a>[`public System.Threading.Tasks.ValueTask Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner.ProvisionAsync(Inno.Build.BuildTargetId target, string supportPackRoot, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/support/Inno.Build.SupportPacks.Core/SourcePlayerSupportPackProvisioner.cs#L114) | Prepares current target inputs and atomically selects their immutable Support Pack.<br>返回：An operation that completes after the target Pack has been installed and verified.<br>`target`：The target platform and architecture identity.<br>`supportPackRoot`：The directory that owns the installed target directories.<br>`cancellationToken`：Cancels preparation or publication before a new current fingerprint is selected. | — | — |
| <a id="api-9dec136e64aff812"></a>[`public static Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner? Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner.TryCreateForHost(string startDirectory, Inno.Build.SupportPacks.PlayerSupportPackPublisher publisher, string dotnetHost, System.Action<string> reportDiagnostic)`](../../build/support/Inno.Build.SupportPacks.Core/SourcePlayerSupportPackProvisioner.cs#L76) | Finds the checkout that owns the running Editor or build command.<br>返回：A source provisioner, or null when the host is a source-independent distribution.<br>`startDirectory`：The host binary directory from which to search upward.<br>`publisher`：The publisher containing the host-selected platform sources.<br>`dotnetHost`：The SDK executable explicitly selected by the host.<br>`reportDiagnostic`：The borrowed host reporter, retained only by the returned provisioner. | — | T:System.IO.DirectoryNotFoundException: An explicit INNO_ENGINE_ROOT does not identify a complete engine checkout. |
| <a id="api-067bb85108da2022"></a>[`public sealed class Inno.Build.SupportPacks.SourcePlayerSupportPackProvisioner : Inno.Build.IPlayerSupportPackProvisioner`](../../build/support/Inno.Build.SupportPacks.Core/SourcePlayerSupportPackProvisioner.cs#L12) | Prepares current checkout and SDK inputs and selects a verified, immutable Player Support Pack. | — | — |

## 项目依赖

- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Build](Inno.Build.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
