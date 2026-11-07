# Inno.Build.Tasks API

[项目入口](../Inno.Build.Tasks.md)

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Build.Tasks.CompileShaderTask`

<!-- api: T:Inno.Build.Tasks.CompileShaderTask c39e8f59c784fa4571af87fdf3d4eebf9b3edc15c26d77418b1b18f6140f3f94 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.EngineRoot 2d7a1e65a459c9c0d03810d863d005ee7f74fa6f9356ec6b7a98edb154b8c3b6 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.Configuration 1ab66675d82182d5d3a85457a80cef012a4e58866258f324a785639bd03b1fa1 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.AssetRoot 88c9d2fc78392d450dee66e56c9bd6558c1576278586867bfc67afa551fca468 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.ShaderPath d4c5b1d62a8fede93cf80bb17bf77b015deb4da77031518bda5cce26cc27b337 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.Target b79bea8a0b61e05c1d0a8d5c44bb022cfc5bc92788175ace1ee9019a55299f78 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.ToolTarget cdbc752af84fb3a27546a5c0750369230f677fa3ae2acf24ddecc12b0720f4e4 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.Renderer ad2c8c4658abb86ca7a1c1f33a3339c7ffa03063f69eb7513102bc72dc9b05d2 -->
<!-- api: P:Inno.Build.Tasks.CompileShaderTask.OutputFile e3e2374357adc2c0ba1e98da55f8cf04feff6a33a69718b6fdffc11ffb9e1663 -->
<!-- api: M:Inno.Build.Tasks.CompileShaderTask.Execute 12cca3ab6d3604126c62a0e36743ef22f1df95ed0e56b484c3fc6bc529fd141b -->
<!-- api: M:Inno.Build.Tasks.CompileShaderTask.Cancel 5507f05d49118d4161d95555b2dc2af93316fdf5ec8f7ce6018c90a0c15277f7 -->
<!-- api: M:Inno.Build.Tasks.CompileShaderTask.#ctor e768f0db28186b6552c40b2e640cbe1bba4b70dec0e5b0e22080ccaf60376ada -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-2e282983370f3b9a"></a>[`public Inno.Build.Tasks.CompileShaderTask.CompileShaderTask()`](../../../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L22) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-3e843d7bc51c1d99"></a>[`public void Inno.Build.Tasks.CompileShaderTask.Cancel()`](../../../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L118) | Cancels offline tool preparation and prevents publication of newly prepared tool products. |
| <a id="api-ca9d1529950eaad3"></a>[`public override bool Inno.Build.Tasks.CompileShaderTask.Execute()`](../../../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L83) | Compiles the requested graph artifact and reports compilation failures through MSBuild.<br>返回：True after writing the artifact; false when validation or compilation fails. |
| <a id="api-4cc2a6806019a2cd"></a>[`public string Inno.Build.Tasks.CompileShaderTask.AssetRoot { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L43) | Gets or sets the authoring source directory. |
| <a id="api-21957baa84608a49"></a>[`public string Inno.Build.Tasks.CompileShaderTask.Configuration { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L37) | Gets or sets the managed and native tool configuration. |
| <a id="api-e944b3105e31d7b3"></a>[`public string Inno.Build.Tasks.CompileShaderTask.EngineRoot { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L32) | Gets or sets the checkout whose native offline tools must match this build. |
| <a id="api-cd302b54af465f25"></a>[`public string Inno.Build.Tasks.CompileShaderTask.OutputFile { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L75) | Gets or sets the compiled file inside an owned Outputs subdirectory. Its parent artifact directory is validated and atomically published with a content manifest. |
| <a id="api-c2071a8cc41cac44"></a>[`public string Inno.Build.Tasks.CompileShaderTask.Renderer { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L68) | Gets or sets the graphics API identifier. |
| <a id="api-58d4eb4f091bffbf"></a>[`public string Inno.Build.Tasks.CompileShaderTask.ShaderPath { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L49) | Gets or sets the source-local shader graph path. |
| <a id="api-afd1629572f60a51"></a>[`public string Inno.Build.Tasks.CompileShaderTask.Target { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L55) | Gets or sets the graphics compiler target platform. |
| <a id="api-88d5c566bc444191"></a>[`public string Inno.Build.Tasks.CompileShaderTask.ToolTarget { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L62) | Gets or sets the explicit native target of the offline compiler executed by this task. This selection is independent of the shader artifact target. |
| <a id="api-b386a65d470033c7"></a>[`public sealed class Inno.Build.Tasks.CompileShaderTask : Microsoft.Build.Utilities.Task, Microsoft.Build.Framework.ICancelableTask`](../../../../build/tasks/Inno.Build.Tasks/CompileShaderTask.cs#L22) | Connects MSBuild to the same graph compiler used by the unified build workflow. |

### `Inno.Build.Tasks.GenerateBindingsTask`

<!-- api: T:Inno.Build.Tasks.GenerateBindingsTask 9888e2e81a05c9847cec0574040e74cd70158c9cf7787ef62a205f347c4b1388 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.EngineRoot bdeffe4ce336627a0cc3a55759f0e1f4fbaba31d5dc31ce8a3e250be349a9a37 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.ComponentId f6a30989c1fd387e18b33971339e59d18e51d64b9043d011675c01bd3792c222 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.ComponentProject 173f9c3c116a09937603bd65ad2d20951169464e5c8a448fbe386f0272855469 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.BindingDefinition 798cdc61b581c31c89094193b0ec7415eb4e01eb6710655fb16fa8adf97e0350 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.TargetId 50bcfc7b63c3066b66427d3727b69f99e8c9b44e64ae082be73e483f30a537cb -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.OutputMode 2e38e8e75860e01000860f0a181aa0d20f74bfb8ad910f4eeae0c60b5f943f68 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.DotnetHost 674d29f561647b7040fc4d987d221c7cb7cc67f1b9659695b1657e610bfa6578 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.CheckOnly e46aac5cb6e527147d053972ea34d8b7a9e6499be4cd851dd7041275c215e1ba -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.DescriptorOutputPath ebe67c4d24f29cb63b2e2386c041d930038193d15e2f6f97e8eb707f16519cf9 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.ExpectedFingerprint c45fbc96ef738c6ccbbaa09a63442aa9248320c261ba3ab67f85f5434c68b3db -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.GeneratedBindings 51e1e88206b45e6308882aafea8531d0b9209b2949698b0fc9469efea32c59a1 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.GenerationFingerprint 3573f4c3015cf9fb7871a0a919fa6d9df75195a5c1f24bf74d892244a99b1924 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.NativeDirectory ef4b387dcc0ac2adb2b0e6f76b18522308e866de74284d032632a6248c261b2f -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.ArtifactDirectory c17ec197a5817bc20134d2e2c30c3c063223732aa5408c75913ebb89263d1ac1 -->
<!-- api: P:Inno.Build.Tasks.GenerateBindingsTask.ArtifactOwnerDirectory 315a73a705fbf1aef3cb82068a3c2623897dee4434b83d3932c6c6046ab53b42 -->
<!-- api: M:Inno.Build.Tasks.GenerateBindingsTask.Cancel 6450d836be0b8cae8c50e013978bec40372ba8b55d248611dba53f9864007117 -->
<!-- api: M:Inno.Build.Tasks.GenerateBindingsTask.Execute 3d0f76b35164ef0bff7c13131341d9139b45174dcd8fe52e61dd9c83833312a2 -->
<!-- api: M:Inno.Build.Tasks.GenerateBindingsTask.#ctor 81c4f9dce488cc2cb313f6dd2e6a3f30e04dde68fb7f26f30024b80c4a6e6ccc -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-ff3d6b09600f6748"></a>[`public Inno.Build.Tasks.GenerateBindingsTask.GenerateBindingsTask()`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L18) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-0e2f3cc8eb4f48dd"></a>[`public void Inno.Build.Tasks.GenerateBindingsTask.Cancel()`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L107) | Cancels owned work before retiring the task cancellation source. |
| <a id="api-5781726cd5c70adf"></a>[`public override bool Inno.Build.Tasks.GenerateBindingsTask.Execute()`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L120) | Executes one explicit component request and publishes task outputs only after success.<br>返回：True after generation or comparison; false after a logged failure and complete cleanup. |
| <a id="api-1a480d62e4caeaba"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.ArtifactDirectory { get; private set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L96) | Gets the protected immutable artifact root, or an empty value for host-source generation. |
| <a id="api-66ed8053f6f55f55"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.ArtifactOwnerDirectory { get; private set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L102) | Gets the publisher root coordinating the selected immutable artifact, or empty for host sources. |
| <a id="api-575a1911eea02136"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.BindingDefinition { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L43) | Gets or sets the sole literal definition consumed by both the project and direct generation. |
| <a id="api-a6f2d71733c80c5a"></a>[`public bool Inno.Build.Tasks.GenerateBindingsTask.CheckOnly { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L62) | Gets or sets comparison without publication. |
| <a id="api-c71184f5383ef80c"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.ComponentId { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L33) | Gets or sets the stable component identity declared by the sole binding definition. |
| <a id="api-982ad2c776d8e749"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.ComponentProject { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L38) | Gets or sets the absolute Native owner project, which must belong to this checkout. |
| <a id="api-d0abb47b973cb34d"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.DescriptorOutputPath { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L66) | Gets or sets an optional request-owned descriptor destination written only after success. |
| <a id="api-67b85ee3fb7aaee3"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.DotnetHost { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L58) | Gets or sets the explicitly selected managed SDK host used to compose the target toolchain. |
| <a id="api-c0c3fd50a5e4cfea"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.EngineRoot { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L28) | Gets or sets the absolute checkout owning the component definition. |
| <a id="api-9264f16119860a61"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.ExpectedFingerprint { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L70) | Gets or sets an optional expected identity; stale requests fail before generation. |
| <a id="api-47ecc7e873a694fd"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.GeneratedBindings { get; private set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L75) | Gets the single source selected after complete publication or successful comparison. |
| <a id="api-aade4e3b794412b5"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.GenerationFingerprint { get; private set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L80) | Gets the complete validated input identity. |
| <a id="api-470866008b43e628"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.NativeDirectory { get; private set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L90) | Gets the complete native artifacts and compilation requirements selected by this operation. |
| <a id="api-af714cac5c42448a"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.OutputMode { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L53) | Gets or sets HostSource or TargetArtifacts without deriving publication from a path. |
| <a id="api-2e58c35aa575bcc7"></a>[`public string Inno.Build.Tasks.GenerateBindingsTask.TargetId { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L48) | Gets or sets the explicit configuration identity selected from the definition. |
| <a id="api-56d9cd602fa62564"></a>[`public sealed class Inno.Build.Tasks.GenerateBindingsTask : Microsoft.Build.Utilities.Task, Microsoft.Build.Framework.ICancelableTask`](../../../../build/tasks/Inno.Build.Tasks/GenerateBindingsTask.cs#L18) | Maps an explicit MSBuild request to the shared binding application without owning generation policy. |

### `Inno.Build.Tasks.MaintainBuildCachesTask`

<!-- api: T:Inno.Build.Tasks.MaintainBuildCachesTask 6a56c37da05d5e03cb999d2fda5e5ad93a6a2fee7890cc42354f599bb8942b8a -->
<!-- api: P:Inno.Build.Tasks.MaintainBuildCachesTask.EngineRoot dd402a9e231b189d206f3f86d6330a90bdb230a9d137563750c9bfbf65c8f96d -->
<!-- api: M:Inno.Build.Tasks.MaintainBuildCachesTask.Execute 1afc95e4d782b4058fbfa1af9962a07bc750ae92db202feb12ab1457aa9f54be -->
<!-- api: M:Inno.Build.Tasks.MaintainBuildCachesTask.#ctor dfec429dd6a59b67a3d9cf466b193a88f0e8979850718ff23c5cf5608ec8d8fb -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-5463b4ef34c0f8d3"></a>[`public Inno.Build.Tasks.MaintainBuildCachesTask.MaintainBuildCachesTask()`](../../../../build/tasks/Inno.Build.Tasks/MaintainBuildCachesTask.cs#L12) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-47d1c42c2532768d"></a>[`public override bool Inno.Build.Tasks.MaintainBuildCachesTask.Execute()`](../../../../build/tasks/Inno.Build.Tasks/MaintainBuildCachesTask.cs#L26) | Attempts maintenance without changing the successful product build outcome.<br>返回：True after completed or explicitly deferred automatic maintenance. |
| <a id="api-537a4fc1c2e564e4"></a>[`public string Inno.Build.Tasks.MaintainBuildCachesTask.EngineRoot { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/MaintainBuildCachesTask.cs#L18) | Gets or sets the absolute checkout owning the registered cache catalog. |
| <a id="api-461c154fdc94dce2"></a>[`public sealed class Inno.Build.Tasks.MaintainBuildCachesTask : Microsoft.Build.Utilities.Task`](../../../../build/tasks/Inno.Build.Tasks/MaintainBuildCachesTask.cs#L12) | Runs bounded cache maintenance after a successful root Editor operation. |

### `Inno.Build.Tasks.PrepareProductNativeTask`

<!-- api: T:Inno.Build.Tasks.PrepareProductNativeTask 55efeac7cb8b6103a78d1e9e245696398b156d9a054bc9030919d2665a3b9b5f -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.EngineRoot a80af1a12910d8c1215a898ba93d22363a51f92a9cb0421732771cdc80334149 -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.OutputDirectory 17f7f369c896945157f2f1a6e02dd6271473aa38a555c4763da1df0e2b0f92b4 -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.ProductId 29fd2b41ad62d4fdb80b86e4b0c9e847c2110c6c6a59b858d46f3d10f0aaa64d -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.TargetId ff6a327ebf667b6491656edbebd2c0a5f8876b2fb3d4ea6476163a6d76f57bbc -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.Configuration 975731c6c2c6c2f7137caed7cd982ce11cc0c863872a25beb4e531eb1a2bc2e4 -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.BindingSelectionOutputPath d4dd4420898a681b95ec745832ae05c588982a65dedc21d74c5a29826f94ddc9 -->
<!-- api: P:Inno.Build.Tasks.PrepareProductNativeTask.BindingSelectionInputPath acda8cb7514bcba45495d508f6c76afa86593abcc33e6cf865206933a8a21686 -->
<!-- api: M:Inno.Build.Tasks.PrepareProductNativeTask.Execute 9c0769b983bdde9b656a1f61ee0ca06dcfc3c42785e00d2721438996c27b2e97 -->
<!-- api: M:Inno.Build.Tasks.PrepareProductNativeTask.Cancel 1836685d1d740271593d145b676374ac6faa86c1ba5296b35ad94fecaa82f820 -->
<!-- api: M:Inno.Build.Tasks.PrepareProductNativeTask.#ctor 3f6cb457d27201c9feacf6a352fdce8fd594d2357f4798bab751017497fe204a -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-7bed92c629f73bb2"></a>[`public Inno.Build.Tasks.PrepareProductNativeTask.PrepareProductNativeTask()`](../../../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L18) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-0b337b5b1b0c5708"></a>[`public void Inno.Build.Tasks.PrepareProductNativeTask.Cancel()`](../../../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L140) | Requests process-tree retirement before or during task execution without racing token disposal. |
| <a id="api-b8ca781d2842b2bf"></a>[`public override bool Inno.Build.Tasks.PrepareProductNativeTask.Execute()`](../../../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L70) | Prepares all host-native products and installs their exact closure after successful validation.<br>返回：True after deployment, or false after reporting a build failure or cancellation to MSBuild. |
| <a id="api-be26d4b8d714e351"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.BindingSelectionInputPath { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L62) | Gets or sets the managed build's frozen selection that publication must preserve before deployment. An empty value denotes initial preparation without a preceding managed compilation. |
| <a id="api-3c90fc6208778207"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.BindingSelectionOutputPath { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L56) | Gets or sets an optional operation-owned selection consumed by the subsequent managed project closure. |
| <a id="api-b480f403e77a86d1"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.Configuration { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L51) | Gets or sets the debug or release configuration used by the managed application. |
| <a id="api-e7cf1daf35a50c42"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.EngineRoot { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L28) | Gets or sets the source checkout containing the unified native toolchains. |
| <a id="api-3d71b74537f79659"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.OutputDirectory { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L34) | Gets or sets the managed application directory receiving the complete native deployment. |
| <a id="api-1d019d5a3b1dffca"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.ProductId { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L40) | Gets or sets the exact product identity whose component closure must be prepared. |
| <a id="api-a11fccffcf2147ca"></a>[`public string Inno.Build.Tasks.PrepareProductNativeTask.TargetId { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L46) | Gets or sets the explicitly declared product target, independent of the task execution machine. |
| <a id="api-90feb81b27588db9"></a>[`public sealed class Inno.Build.Tasks.PrepareProductNativeTask : Microsoft.Build.Utilities.Task, Microsoft.Build.Framework.ICancelableTask`](../../../../build/tasks/Inno.Build.Tasks/PrepareProductNativeTask.cs#L18) | Connects explicit product builds and publication to their target-native closure and exact deployment. |

### `Inno.Build.Tasks.PublishSupportPackTask`

<!-- api: T:Inno.Build.Tasks.PublishSupportPackTask afb7c65f60b0a8f3964a13d725462ebeb09360afa879863eff93e0fb01e0a4ac -->
<!-- api: P:Inno.Build.Tasks.PublishSupportPackTask.EngineRoot 62b4837658cdd6d0c46675dac36f90a40df50adb812e435929ad029ea2c1e453 -->
<!-- api: P:Inno.Build.Tasks.PublishSupportPackTask.OutputRoot 055794987c03c7d294d5c8fa755b269197002815f8a087194cf8a31ecb3f226f -->
<!-- api: P:Inno.Build.Tasks.PublishSupportPackTask.Target 13c85b1db754fe83bf44a772e4848640a8ea59fd8d4dfea9d38303ba01a5fe66 -->
<!-- api: P:Inno.Build.Tasks.PublishSupportPackTask.DotnetHost 6d3632deaede563157464c9b62227f7a6376a40cc24be084209760a499194614 -->
<!-- api: P:Inno.Build.Tasks.PublishSupportPackTask.ToolTarget 773c3baf90f3a29102b99ce14aa6e31c32e5c4f8711d6e0f21dd2f6c2a874219 -->
<!-- api: M:Inno.Build.Tasks.PublishSupportPackTask.Execute d560b8eb68d4d2c71db2a345c8a6453c1c2fc159d3fa0ac59247c2602d07c02a -->
<!-- api: M:Inno.Build.Tasks.PublishSupportPackTask.Cancel 4ed313b958d440c7b332188b188958a8d2c3802dc6cf47e183009d370c5e063b -->
<!-- api: M:Inno.Build.Tasks.PublishSupportPackTask.#ctor 4e1d97a6c9b454101630af2574fbb21bf337ba058d75e6dcf10be889c53aa83c -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-eb6353f8d53da889"></a>[`public Inno.Build.Tasks.PublishSupportPackTask.PublishSupportPackTask()`](../../../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L17) | 由 C# 生成，遵循所属类型的值语义与本页的生命周期约束。 |
| <a id="api-c2cc3ab11edf1284"></a>[`public void Inno.Build.Tasks.PublishSupportPackTask.Cancel()`](../../../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L95) | Requests cancellation before or during execution without racing disposal. |
| <a id="api-340855085dd8722b"></a>[`public override bool Inno.Build.Tasks.PublishSupportPackTask.Execute()`](../../../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L58) | Publishes and validates the target pack, reporting failures through MSBuild.<br>返回：True after installation; false when publication fails or is canceled. |
| <a id="api-cd3d0b94e175c13c"></a>[`public string Inno.Build.Tasks.PublishSupportPackTask.DotnetHost { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L44) | Gets or sets the SDK host executable. |
| <a id="api-40bae46b53808cd2"></a>[`public string Inno.Build.Tasks.PublishSupportPackTask.EngineRoot { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L27) | Gets or sets the engine checkout directory. |
| <a id="api-0574e0cdd7b940fd"></a>[`public string Inno.Build.Tasks.PublishSupportPackTask.OutputRoot { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L33) | Gets or sets the directory owning installed target packs. |
| <a id="api-7c02113ae29f3bb8"></a>[`public string Inno.Build.Tasks.PublishSupportPackTask.Target { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L39) | Gets or sets the requested build target identifier. |
| <a id="api-8ee71db1117460e4"></a>[`public string Inno.Build.Tasks.PublishSupportPackTask.ToolTarget { get; set; }`](../../../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L50) | Gets or sets the independently selected native target for executable compilation tools. |
| <a id="api-8c2e8f3c7f71bbe4"></a>[`public sealed class Inno.Build.Tasks.PublishSupportPackTask : Microsoft.Build.Utilities.Task, Microsoft.Build.Framework.ICancelableTask`](../../../../build/tasks/Inno.Build.Tasks/PublishSupportPackTask.cs#L17) | Connects MSBuild publication to the common atomic Support Pack publisher. |
