# InnoEngine 架构整改执行计划：极度干净、边界严格、扩展轻松

## 一、目标与完成定义

本计划覆盖上一轮架构审查提出的 **全部 8 项问题**，同时处理它们关联的消费者、生成注册、脚本 API、缓存、生命周期、测试与文档。

目标不是单纯移动文件，而是让以下三件事同时成立：

1. **核心机制独立**：不依赖资产创作、具体平台、第三方后端或文件系统部署方式。
2. **所有权明确**：内容、设备、会话、代际、任务和回调都有唯一 owner。
3. **扩展位置固定**：新增平台、内容来源、图形后端或部署方式时，只修改对应实现和组合入口。

### 1.1 八项问题与整改结果

| 编号 | 当前问题 | 必须达到的结果 |
|---|---|---|
| A01 | `Inno.Rendering` 引用 Assets、References，并承载资产与模型协议 | Rendering Core 只包含图形机制；资产、创作和运行服务独立 |
| A02 | 内容缓存仅凭 `.complete` 判断有效 | 缓存通过完整目录、长度与内容哈希验证；并发、取消与修复安全 |
| A03 | 通用输入实现归属 `Input.Sdl3` | Core Events 到输入快照的实现与 SDL 解耦 |
| A04 | Player、Asset、音频等要求物理路径 | 共享运行流程依赖内容读取契约；具体实现负责物理布局 |
| A05 | Editor、CLI 重复组合目标与部署编译器 | 一个共享构建组合库，宿主只注入上下文 |
| A06 | Presentation 默认返回 `1×1`，掩盖实现缺失 | 明确区分真实尺寸与当前无可用主输出 |
| A07 | Native 热构建重复引导、扫描和宽泛失效 | 消除重复工作，保持完整性校验与正确失效 |
| A08 | 大文件职责集中，帧路径重复分配与完整编译 | 文件按职责展开；帧内临时状态受控；最终图只完整编译一次 |

### 1.2 执行约束

- 保留当前尚未提交的 Editor Native 部署修复，尤其是普通 IDE Build 与 Publish 的原生闭包准备。
- 不恢复旧 API、旧 namespace alias、旧缓存读取或兼容 wrapper。
- 保留现有 Stable Type ID、资产 persistent ID、扩展 ID 与序列化属性语义。
- 当前 Project 数据与源码同步更新；可重建缓存直接失效重建。
- 不手工修改生成绑定，不修改 `extern`。
- 不增加新的生产 `Program`。
- BGCS 保持独立；本轮生成身份和消费集成的整改归属 Inno Build。
- 不通过 `InternalsVisibleTo`、反射穿透或扩大可见性简化测试。
- 不自动提交代码。

---

## 二、文件覆盖与边界验证办法

仓库根目录：

```text
C:\Dev\GameEngineDev\InnoEngine\
```

消费者仓库：

```text
C:\Dev\GameEngineDev\InnoEngine.Canvas\
C:\Dev\GameEngineDev\InnoEngine.Rendering2D\
C:\Dev\GameEngineDev\InnoEngine.Samples\
C:\Dev\GameEngineDev\BindGen-CS\
```

下面的目录树展开本轮受影响文件。标记含义：

- `[新增]`：新职责或必要契约。
- `[迁移]`：保留行为，改变所属程序集或位置。
- `[修改]`：调整契约、消费者或实现。
- `[保留]`：职责正确，保留并验证依赖。
- `[删除]`：被正式替换，不保留转发入口。

### 2.1 建立逐文件执行清单

执行开始时生成：

```text
artifacts/acceptance/architecture-cleanup/
├─ baseline/
│  ├─ revisions.json
│  ├─ environment.json
│  ├─ working-tree.diff
│  ├─ project-dependencies.json
│  ├─ public-api.txt
│  ├─ stable-type-ids.tsv
│  └─ performance.json
├─ file-map.tsv
├─ results/
├─ logs/
└─ captures/
```

`file-map.tsv` 对每个纳入变更闭包的文件记录：

```text
原路径
目标路径
新增／修改／迁移／删除／保留
主要类型
所属职责
公开 API 变化
消费者
对应测试
完成状态
```

覆盖检查采用以下固定规则：

1. 枚举所有生产 `.csproj`，包含共同 MSBuild 注入的引用。
2. 使用语义分析寻找被迁移类型的全部引用。
3. 检查源码、生成器、脚本导出、构建模板、测试 fixture 与 Wiki。
4. 每个旧文件必须有明确去向，每个迁移类型只能有一个实现。
5. 对未修改但处于依赖闭包中的文件，记录“保留”及验证依据。
6. 交付时不得存在未分类的旧路径引用。

### 2.2 目标依赖方向

```text
Rendering Core
└─ 必要 Foundation 契约

Rendering Assets
├─ Rendering Core
├─ Assets / References
└─ 必要 Foundation 契约

Rendering Shaders
├─ Rendering Core
├─ Rendering Assets
└─ 通用 Graph / Foundation

Rendering Assets Authoring
├─ Rendering Assets
├─ Rendering Shaders
└─ Assets Pipeline

Rendering Runtime
├─ Rendering Core
├─ Rendering Assets
├─ Runtime Contracts
└─ Content / References / 必要 Foundation 契约

具体图形 Adapter
├─ Rendering Core
├─ 必要 Adapter SPI
└─ 对应 Native
```

强制禁止：

- `Inno.Rendering → Assets / References / Scene / Editor / Adapter / Native`
- `Rendering Runtime → Assets Pipeline / Rendering Assets Authoring / Shader 编译工具链`
- `Rendering Assets → Rendering Runtime / Rendering Shaders`
- `Player Runtime → 具体 Content Adapter / Build`
- 通用 Build 核心引用具体平台组合库
- Foundation 模块契约携带动态加载器的磁盘缓存要求

---

## 三、A01：完整拆分 Rendering

### 3.1 四个职责

| 项目 | 职责 |
|---|---|
| `Inno.Rendering` | 设备、资源描述、命令、能力、图、反射绑定等图形机制 |
| `Inno.Rendering.Assets` | Shader、Material、Texture、Geometry、Pipeline 的运行时资产契约 |
| `Inno.Rendering.Assets.Authoring` | 导入、源码编辑、目标编译、创作产物与 last-good 保存 |
| `Inno.Rendering.Runtime` | 模型组合、请求调度、资产到 GPU 的解析、缓存和退休 |

`Inno.Rendering.Shaders` 保留 Shader 图、IR 和可替换前端职责。

### 3.2 Rendering Core 目标文件

```text
src/services/rendering/Inno.Rendering/
├─ Inno.Rendering.csproj                          [修改] 删除 Assets、References 等引用
├─ Device/
│  ├─ IRenderDevice.cs                            [修改] Presentation 契约
│  ├─ RenderDevice.cs                             [修改] 同步抽象契约
│  ├─ GraphicsCapabilities.cs                     [保留]
│  ├─ RenderCommands.cs                           [保留]
│  ├─ RenderResources.cs                          [保留]
│  ├─ RenderProtocolIds.cs                        [保留]
│  ├─ RenderPresentationSize.cs                   [修改] 有效尺寸约束
│  ├─ RenderDeviceAllocationCounters.cs           [保留]
│  ├─ RenderTextureReadback.cs                    [保留]
│  ├─ RenderBufferUploads.cs                      [保留]
│  ├─ RecordedRenderCommandEncoder.cs            [保留]
│  ├─ IRenderLayerCompositionProgramProvider.cs   [保留]
│  ├─ RenderVertexLayout.cs                       [拆分] 顶点布局、attribute 与格式
│  ├─ PersistentBufferDescriptor.cs               [拆分]
│  ├─ RenderSamplerState.cs                       [拆分] 采样状态及相关枚举
│  ├─ RenderShaderBindingDescriptor.cs            [拆分] GPU binding 描述
│  ├─ RenderBlendState.cs                         [拆分] Blend 状态及相关枚举
│  ├─ RenderStencilState.cs                       [拆分] Stencil 状态及相关值
│  ├─ RenderRasterState.cs                        [拆分] Raster 状态及相关枚举
│  ├─ GraphicsPipelineDescriptor.cs              [拆分]
│  └─ ComputePipelineDescriptor.cs               [拆分]
├─ Shaders/
│  ├─ ShaderStage.cs                              [迁移] 纯阶段协议
│  ├─ ShaderProgramKind.cs                        [迁移]
│  ├─ ShaderPropertyId.cs                         [迁移] 中立绑定身份
│  ├─ ShaderPropertyType.cs                       [迁移] 中立绑定类型
│  ├─ ShaderPropertyBindingKind.cs                [迁移]
│  ├─ ShaderInterfaceBinding.cs                   [拆分]
│  └─ ShaderInterface.cs                          [迁移] 不包含资产默认值
├─ Targets/
│  ├─ RenderTexture.cs                            [拆分] 中立离屏目标描述
│  └─ RenderTarget.cs                             [拆分] 含紧密相关的 target kind
├─ Views/
│  └─ RenderViewport.cs                           [拆分] 像素区域
├─ Graph/
│  ├─ RenderGraphTypes.cs                         [保留]
│  ├─ RenderGraphBuilder.cs                       [修改]
│  ├─ RenderGraphBuilder.Mutations.cs             [新增]
│  ├─ RenderGraphMutationScope.cs                 [拆分]
│  ├─ RenderGraphNameScope.cs                     [拆分]
│  ├─ RenderPassBuilder.cs                        [拆分]
│  ├─ RenderGraphCompiler.cs                      [修改]
│  ├─ RenderGraphValidationResult.cs              [新增]
│  ├─ RenderGraphValidationState.cs               [新增，internal]
│  ├─ RenderGraphValidator.cs                     [新增，internal]
│  ├─ RenderGraphDependencyAnalysis.cs            [拆分，internal]
│  ├─ RenderGraphScheduling.cs                    [拆分，internal]
│  ├─ RenderGraphResourceAllocation.cs            [拆分，internal]
│  ├─ CompiledRenderGraph.cs                      [保留]
│  └─ IRenderFrameGraphContributor.cs             [保留] 只接收图与 frame index
├─ IRenderFrameUploadService.cs                   [保留] 中立 GPU 上传协议
└─ Properties/
   └─ ScriptingApi.cs                             [修改] 仅导出本程序集所属类型
```

`PipelineResources.cs` 在上述拆分完成后删除。相关枚举与其主要描述类型同文件，避免把一个紧密协议机械拆成大量小文件。

### 3.3 运行时资产目标文件

```text
src/services/rendering/Inno.Rendering.Assets/
├─ Inno.Rendering.Assets.csproj                    [修改] 不再引用 Assets Pipeline、Shaders
├─ Shaders/
│  ├─ ShaderAsset.cs                              [迁移]
│  ├─ ShaderDefinition.cs                         [拆分]
│  ├─ ShaderDefinitionSnapshot.cs                 [迁移，internal]
│  ├─ ShaderDefinitionValidator.cs                [迁移]
│  ├─ ShaderPropertyDefinition.cs                 [拆分]
│  ├─ ShaderKeywordDefinition.cs                  [拆分]
│  ├─ ShaderPassDefinition.cs                     [拆分]
│  ├─ ShaderTechniqueDefinition.cs                [拆分]
│  ├─ ShaderTechniquePass.cs                      [拆分]
│  ├─ ShaderMetadataEntry.cs                      [拆分]
│  ├─ ShaderContractId.cs                         [拆分]
│  ├─ ShaderTechniqueId.cs                        [拆分]
│  ├─ ShaderPassRoleId.cs                         [拆分]
│  ├─ ShaderPropertyBindingOwner.cs               [拆分]
│  ├─ ShaderRenderState.cs                        [拆分] 含创作状态相关枚举
│  ├─ ShaderDiagnostic.cs                         [迁移] 定义验证诊断
│  └─ ShaderSourceLocation.cs                     [拆分]
├─ Materials/
│  ├─ MaterialAsset.cs                            [拆分]
│  ├─ MaterialValue.cs                            [拆分] 含 value kind
│  ├─ MaterialPropertyEntry.cs                    [拆分]
│  ├─ MaterialMetadataEntry.cs                    [拆分]
│  ├─ MaterialPropertyBlock.cs                    [拆分]
│  ├─ MaterialPassResolution.cs                   [拆分]
│  └─ MaterialPassResolver.cs                     [拆分]
├─ Textures/
│  ├─ TextureAsset.cs                             [拆分]
│  ├─ TextureColorSpace.cs                        [拆分]
│  ├─ RenderTextureArtifactSlot.cs                [拆分]
│  ├─ RenderTextureArtifactReference.cs           [拆分]
│  └─ IRenderTextureArtifactSource.cs             [拆分]
├─ Geometry/
│  ├─ GeometryAsset.cs                            [拆分]
│  ├─ GeometryVertex.cs                           [拆分]
│  ├─ GeometrySection.cs                          [拆分]
│  ├─ GeometryData.cs                             [拆分]
│  ├─ GeometryArtifact.cs                         [拆分]
│  └─ GeometryAssetRuntime.cs                     [迁移] 资产数据解析
├─ Pipelines/
│  ├─ RenderPipelineAsset.cs                      [迁移]
│  ├─ RenderFeatureConfiguration.cs               [拆分]
│  ├─ SerializedRenderExtensionState.cs           [拆分]
│  └─ SerializedRenderExtensionStateConverter.cs  [迁移]
├─ Deployment/
│  ├─ RenderShaderArtifact.cs                     [迁移]
│  ├─ RenderShaderPassArtifact.cs                 [拆分]
│  ├─ RenderShaderStageArtifact.cs                [拆分]
│  ├─ RenderShaderArtifactCodec.cs                [迁移]
│  ├─ RenderShaderVariant.cs                      [迁移]
│  └─ RenderTargetArtifactPath.cs                 [迁移] 返回逻辑内容定位
└─ Properties/
   └─ ScriptingApi.cs                             [修改] 运行时资产显式导出
```

关键规则：

- `ShaderDefinition` 中的 Material 默认值与 Asset reference 留在资产层。
- `ShaderInterface` 只表达编译后绑定事实，不引用上述默认值。
- GPU handle 不进入资产持久化。
- 资产到 GPU 的创建与缓存由 Runtime 持有，资产层不创建第二个 GPU owner。
- 迁移序列化类型时保留显式 Stable ID；对采用推导 ID 的持久类型，先记录当前 ID，再以显式声明固定身份。

### 3.4 新建资产创作项目

```text
src/services/rendering/Inno.Rendering.Assets.Authoring/
├─ Inno.Rendering.Assets.Authoring.csproj          [新增]
├─ Importing/
│  ├─ TextureAssetImporter.cs                     [迁移]
│  ├─ ShaderSourceImportSettings.cs               [迁移]
│  ├─ ShaderFunctionImporter.cs                   [迁移]
│  ├─ ShaderFunctionAsset.cs                      [迁移] Authoring-only
│  ├─ ShaderAssetImporter.cs                      [迁移]
│  ├─ RenderPipelineAssetImporter.cs              [迁移]
│  ├─ RenderingAssetFormatException.cs            [迁移]
│  ├─ ObjMeshParser.cs                            [迁移]
│  ├─ MaterialAssetImporter.cs                    [迁移]
│  ├─ GltfMeshParser.cs                           [迁移]
│  ├─ GltfJson.cs                                 [迁移]
│  └─ GeometryAssetImporter.cs                    [迁移]
├─ Editing/
│  └─ ShaderGraphSourceStore.cs                   [迁移]
├─ Compilation/
│  ├─ TextureTargetCompiler.cs                   [迁移]
│  ├─ ShaderStageCompilation.cs                  [迁移]
│  ├─ ShaderSourceBundle.cs                       [迁移]
│  ├─ ShaderLastGoodStore.cs                      [迁移]
│  ├─ ShaderGraphCompilation.cs                   [迁移]
│  ├─ ShaderGraphArtifact.cs                      [迁移]
│  └─ ShaderCompilation.cs                       [迁移]
└─ Properties/
   └─ ScriptingApi.cs                             [新增] Editor-only 导出
```

这些文件从现有 `Inno.Rendering.Assets` 迁出，旧路径删除。

目标编译器与第三方语言解析继续由现有具体 provider 实现，不进入运行时资产项目。

### 3.5 Rendering Runtime 展开

```text
src/services/rendering/Inno.Rendering.Runtime/
├─ Inno.Rendering.Runtime.csproj                  [修改] 明确引用运行时资产
├─ RenderRuntime.cs                               [修改] 构造、入口、唯一生命周期 owner
├─ RenderRuntime.Frames.cs                        [拆分] Begin/Complete/End frame
├─ RenderRuntime.Requests.cs                      [拆分] 提交、排队与构建请求
├─ RenderRuntime.Models.cs                        [拆分] 模型选择与组合
├─ RenderRuntime.Presentation.cs                  [拆分] 主输出及输入坐标
├─ RenderRuntime.Pipelines.cs                     [拆分] 候选、last-good 与退休
├─ RenderRuntime.Contributors.cs                  [拆分] Contributor 快照与执行
├─ RenderRuntime.Diagnostics.cs                   [拆分] 当前帧诊断对账
├─ RenderRuntime.Retirement.cs                    [拆分] 停止、释放与失败处理
├─ Requests/
│  ├─ RenderRequest.cs                            [迁移]
│  ├─ RenderFrameData.cs                          [迁移]
│  ├─ RenderRequestProvider.cs                    [迁移]
│  └─ IRenderRequestSink.cs                       [迁移]
├─ Models/
│  ├─ RenderOutputSession.cs                      [拆分]
│  ├─ RenderOutputLayer.cs                        [拆分]
│  ├─ RenderOutputRoute.cs                        [拆分]
│  ├─ RenderModelOutput.cs                        [拆分]
│  ├─ IRenderModel.cs                             [拆分]
│  ├─ RenderModelExtensionAttribute.cs            [拆分]
│  ├─ RenderOutputInput.cs                        [迁移]
│  └─ ViewContent.cs                              [迁移] 同一内容采集协议
├─ Pipelines/
│  ├─ RenderPipeline.cs                           [迁移]
│  ├─ RenderPipelineFeature.cs                    [拆分]
│  ├─ RenderPipelineExtensionAttribute.cs         [拆分]
│  ├─ RenderFeatureExtensionAttribute.cs          [拆分]
│  ├─ RenderResourceMap.cs                        [拆分]
│  ├─ RenderPipelineContext.cs                    [拆分]
│  ├─ RenderFeatureContext.cs                     [拆分]
│  └─ RenderExtensionStateContext.cs              [迁移]
├─ Resources/
│  ├─ IRenderResourceService.cs                   [拆分]
│  ├─ RenderResourceProvider.cs                   [迁移]
│  ├─ RenderPersistentResourceId.cs               [拆分]
│  ├─ RenderTextureSubresourceData.cs             [拆分]
│  ├─ RenderGeometry.cs                           [拆分]
│  ├─ RenderGeometrySection.cs                    [拆分]
│  └─ RenderMaterialPass.cs                       [拆分] 包含私有绑定表示
├─ Deployment/
│  ├─ IRenderTargetArtifactProvider.cs            [迁移]
│  ├─ RenderTargetArtifactStatus.cs               [迁移]
│  └─ ContentRenderTargetArtifactProvider.cs      [替换] 基于内容 store
├─ RenderContributorSnapshot.cs                  [新增，internal]
├─ RenderFrameScratch.cs                         [新增，internal]
├─ GraphicsSettings.cs                            [保留]
├─ IRenderRuntimeReloadTransaction.cs             [保留]
├─ RenderRuntimeFactory.cs                        [修改]
├─ RenderResourceService.cs                       [修改] 唯一资源服务实现
├─ RenderResourceStatistics.cs                    [保留]
├─ RenderResourceLimits.cs                        [保留]
├─ RenderResourceCache.cs                         [保留]
├─ RenderTargetStore.cs                           [修改] 无主输出时不伪造尺寸
├─ RenderReadbackOwner.cs                         [保留]
├─ RenderMaterialOwner.cs                         [修改] 新资产契约位置
├─ RenderGeometryOwner.cs                         [修改] 新资产契约位置
├─ RenderFrameUploadService.cs                    [保留]
├─ RenderPipelineGeneration.cs                    [修改]
├─ RenderExtensionRegistry.cs                     [修改]
├─ RenderPresentationComposer.cs                  [保留]
├─ RenderLayerCompositor.cs                       [保留]
├─ RenderRetirementQueue.cs                       [保留]
└─ Properties/
   └─ ScriptingApi.cs                             [修改]
```

`FileRenderTargetArtifactProvider.cs` 删除，不保留旧名称包装层。

### 3.6 必须同步的消费者

| 位置 | 调整 |
|---|---|
| `Inno.Rendering.Shaders` | 明确引用 Core 与 Assets；不引用 Runtime |
| `Inno.Adapter.Rendering.Bgfx` | 只消费 Core；验证没有资产依赖 |
| `Inno.Adapter.Rendering.Authoring` | 编译协议改为引用 Authoring |
| `Inno.Build.Toolchains.Bgfx.Tools` | 引用 Authoring 编译契约，修改相关 compiler 文件 |
| `Inno.Build.Toolchains.Bgfx.Shaders/ShaderArtifactBuilder.cs` | 更新产物与编译契约归属 |
| `Inno.Adapter.Authoring.Default` | 注册新 Authoring 程序集的 importer/provider |
| `Inno.Engine.Default/Rendering/RenderingSubsystem.cs` | 使用 Runtime 模型协议 |
| Editor Rendering、Shaders、ShaderEditor | 更新引用、命名空间与类型使用 |
| Canvas、Rendering2D | 更新当前源码消费者；保持逻辑脚本 namespace |
| Samples | 重建 Plugin、脚本 reference、运行资产与导出闭包 |

脚本仍使用逻辑命名空间 `InnoEngine.Rendering`。实现侧类型迁移通过各项目自己的 `Properties/ScriptingApi.cs` 映射，不建立中央白名单或兼容 facade。

### A01 验收

- 单独编译 `Inno.Rendering`，无需 Assets、References、Editor 或 Native。
- Player 发布闭包不含 Authoring、Assets Pipeline、Shader 编译工具。
- 自定义 Plugin 可以组合 Rendering 机制和 Runtime Pipeline。
- Shader、Material、Geometry、Pipeline 的 Stable ID 与 round-trip 保持正确。
- last-good、Missing、资源 generation 与 reload retirement 回归通过。

---

## 四、A02 与 A04：内容完整性、读取契约和 Player 解耦

这两项必须作为一个完整数据链实施，避免缓存修好了，运行服务仍依赖磁盘目录。

### 4.1 新增中立内容项目

```text
src/content/deployment/Inno.Content/
├─ Inno.Content.csproj                            [新增]
├─ ContentKey.cs                                  [新增] 可移植逻辑定位
├─ ContentEntry.cs                                [新增] 定位、长度、SHA-256
├─ ContentPackDescriptor.cs                       [新增] Pack 身份与文件名
├─ ContentPackIndex.cs                            [新增] 不可变内容索引
├─ ContentPackIndexCodec.cs                       [新增] 当前格式序列化
├─ ContentPackReader.cs                           [新增] Archive 与索引校验
├─ IRuntimeContentStore.cs                        [新增] 只读内容入口
├─ ContentReadLease.cs                            [新增] 固定内容及读取生命周期
├─ PackContentStore.cs                            [新增] 从 owned seekable stream 读取
└─ ContentReadLimits.cs                           [新增] 显式读取与展开预算
```

允许依赖必要 Foundation 契约，不引用 Assets、Scene、Runtime、Build、Adapter 或平台 API。

公开语义：

- `ContentKey` 使用统一逻辑路径规则，不调用宿主 OS 判断路径语义。
- Store 提供不可变 metadata 与 lease，不暴露 `absolutePath`。
- 未找到内容、内容损坏、预算超出和取消分别明确报告。
- Store 关闭后禁止取得新 lease；已取得的 lease 可以完成读取。
- 每个读取流拥有独立读取状态。
- Archive 的共享定位由实现内部同步，不能假设 `ZipArchive` 支持并发访问。
- 输出长度必须与索引一致，读取不能无限展开。
- 索引采用现有序列化体系，不增加 schema version。

### 4.2 Pack writer 与 reader 使用同一协议

```text
build/pipeline/Inno.Build/
├─ Content/
│  └─ ContentPackWriter.cs                        [修改]
├─ Game/
│  └─ GameBuildPipeline.cs                        [修改]
└─ Inno.Build.csproj                              [修改]
```

具体规则：

1. Writer 冻结实际输出文件集合。
2. 为每个 payload 计算逻辑路径、长度与 SHA-256。
3. 写入唯一保留条目 `Content.index`。
4. 索引不包含自身，避免循环哈希。
5. 固定排序与 ZIP 时间，保持确定性。
6. Pack 哈希覆盖索引与全部 payload。
7. `RuntimeContentCatalog` 指向唯一 Pack 身份。
8. Reader 检查索引与 Archive 条目集合完全一致。
9. 拒绝重复路径、大小写折叠冲突、路径逃逸、符号链接条目及长度不符。
10. Writer 和 Reader 不分别维护两套路径规则。

### 4.3 文件缓存 Adapter

```text
src/adapters/content/Inno.Adapter.Content.FileSystem/
├─ Inno.Adapter.Content.FileSystem.csproj          [新增]
├─ FileContentStore.cs                            [新增]
├─ FileContentPreparation.cs                      [新增]
├─ FileContentCacheOptions.cs                     [新增]
├─ ContentCacheGeneration.cs                      [新增，internal]
├─ ContentCacheValidator.cs                       [新增，internal]
├─ ContentCachePublication.cs                     [新增，internal]
└─ ContentCacheRetirement.cs                      [新增，internal]
```

复用 `Inno.Core.IO`，不引用 Build。

缓存布局：

```text
<宿主指定的应用缓存根>/
└─ Content/
   └─ <packHash>/
      ├─ install.lock
      ├─ current
      ├─ leases/
      │  └─ <generation>.lock
      ├─ generations/
      │  └─ <generation>/
      │     └─ 完整验证过的内容
      └─ staging/
         └─ <candidate>/
```

缓存行为：

- `.complete` 不再是有效性依据，旧实现与旧判断直接删除。
- 每次准备验证当前 generation 的完整文件集合、长度与实际哈希。
- 同长度、同 mtime 的篡改仍必须被发现。
- 缺失、额外、损坏文件触发重建。
- 重建先写独立 staging，完整验证后发布新 generation。
- `current` 指针通过 `AtomicFile` 原子切换。
- 发布新 generation 使用 `AtomicDirectory.Publish`，不覆盖正在被读取的旧目录。
- Reader lease 固定 generation；旧目录仅在没有 reader 时退休。
- 多进程准备由同一写 lease 协调。
- 取消发生在提交前时清理候选；提交后不撤销已经有效的缓存。
- 提交后的退休清理失败单独报告，不能误称新缓存没有提交。
- 重试有界，持续权限错误明确失败。

### 4.4 Foundation IO 的必要扩展

```text
src/foundation/core/Inno.Core.IO/
├─ FileLease.cs                                   [修改] 增加共享读取 lease
├─ IByteDocumentStore.cs                          [新增] 小型文档的读取／写入契约
├─ FileByteDocumentStore.cs                       [新增] 复用 AtomicFile
├─ ReadOnlyByteDocumentStore.cs                   [新增] owned bytes、明确只读
├─ AtomicFile.cs                                  [保留]
├─ AtomicDirectory.cs                             [保留]
├─ FileSystemRename.cs                            [保留]
└─ PathBoundary.cs                               [保留]
```

共享 lease 与独占 lease 必须在 Windows 与 Unix 遵循同一协调语义。锁文件保留，不通过删除锁文件“解锁”。

`IByteDocumentStore` 仅服务文档 IO，不承载资产目录、事件、Identity 或内容发现。

### 4.5 Asset lease 不再暴露路径

```text
src/content/assets/Inno.Assets/
├─ Artifacts/
│  ├─ AssetArtifactInfo.cs                        [修改] 删除 absolutePath
│  └─ IAssetArtifactLookup.cs                     [修改] 保留 metadata／Acquire 语义
├─ Runtime/
│  ├─ AssetResidency.cs                           [修改] 现有 ArtifactLease.OpenRead
│  ├─ ResidencyLease.cs                           [修改] 读取 pin 与释放协调
│  ├─ AssetDatabase.cs                            [修改] 接收内容 store
│  └─ AssetDatabase.Residency.cs                   [修改]
└─ Inno.Assets.csproj                             [修改]
```

固定行为：

- 复用现有 `ArtifactLease.OpenRead()`，不新增第二套 artifact reader。
- `AssetArtifactInfo` 只描述 key、output、hash、length。
- 物理位置留在具体 provider 内部。
- 打开的流持有自己的读取 pin，不能因外层 lease 提前释放而访问已退休内容。
- Asset residency、依赖 retention、预加载预算与退休重试继续保留。
- `AssetDatabase` 不运行 importer，不创建 source mount，不直接创建目录。

创作态同时修改：

```text
src/content/assets/Inno.Assets.Pipeline/Importing/Artifacts/AssetArtifactStore.cs
src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.cs
src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs
```

它们可以管理物理创作缓存，但向运行消费者返回相同的路径中立 lease。

### 4.6 Player 目标文件

```text
src/composition/player/Inno.Player.Runtime/
├─ Inno.Player.Runtime.csproj                     [修改]
├─ PlayerApplication.cs                          [修改] 异步启动与完整取消
├─ PlayerLaunchOptions.cs                        [修改] 注入来源、存储与日志工厂
├─ GamePlayerHost.cs                             [修改] 共用生命周期
└─ Deployment/
   ├─ IPlayerContentSource.cs                    [新增]
   ├─ PlayerContentMetadata.cs                   [新增] owned manifest/catalog bytes
   ├─ IPlayerModuleActivator.cs                  [保留]
   └─ StaticPlayerModuleActivator.cs             [保留]

src/composition/player/Inno.Player/
├─ DesktopPlayerComposition.cs                  [修改]
└─ FilePlayerContentSource.cs                    [新增，internal]

src/composition/player/Inno.Player.Browser/
├─ BrowserPlayerComposition.cs                  [修改]
├─ HttpPlayerContentSource.cs                    [替换 BrowserContentLoader]
├─ BrowserBridge.cs                             [保留]
└─ wwwroot/
   ├─ main.js                                   [修改消费者]
   └─ index.html                                [验证启动]
```

`IPlayerContentSource` 明确提供两个阶段：

1. 异步读取部署 metadata。
2. 根据经过校验的 Pack 描述、应用 namespace 和序列化 generation 准备只读 store。

Desktop 来源负责文件定位与磁盘缓存；HTTP 来源负责下载和 owned pack stream。两者使用同一 Reader。

删除：

```text
src/runtime/engine/Inno.Runtime/Deployment/RuntimeContentDeployment.cs
src/composition/player/Inno.Player.Browser/BrowserContentLoader.cs
```

Browser 不再先把完整内容写进 `/Content`，再让共享 Player 解压成另一份目录。浏览器 SDK 自身的文件系统需求仍属于具体实现边界。

`content-pack.txt` 的重复 Pack 身份删除，Browser 从同一 `catalog.inno` 获取描述。

### 4.7 移除 Session、Settings 和模块上下文中的路径要求

```text
src/runtime/engine/Inno.Runtime/Hosting/
├─ EngineHostBuilder.cs                          [修改] 删除通用 metadata cache 路径
├─ EngineHost.cs                                 [修改]
├─ RuntimeSessionOptions.cs                      [修改] store、日志工厂；删除目录要求
├─ RuntimeSession.cs                             [修改] 不自行创建文件日志
└─ SessionLogSink.cs                             [拆分，internal] Session 过滤与 owner

src/runtime/contracts/Inno.Runtime.Contracts/
└─ RuntimeSubsystemContext.cs                    [修改] 删除 persistentDataDirectory

src/foundation/extensibility/Inno.Extensibility.Modules/
├─ ModuleHostOptions.cs                          [修改] 删除动态缓存配置
├─ ModuleHost.cs                                 [修改]
└─ Catalog/ModuleSourceContext.cs                [修改] 保留 generation 与中立上下文

src/adapters/modules/Inno.Adapter.Modules.DotNet/
└─ DotNetModuleSource.cs                         [修改] 自己持有 artifact root

src/foundation/core/Inno.Core.Settings/
├─ SettingsDocumentStore.cs                      [修改] 接收 IByteDocumentStore
├─ ProjectSettings.cs                            [修改]
└─ ProjectSettingsStore.cs                       [修改]
```

对应宿主修改：

```text
src/composition/editor/host/Inno.Editor.Application/Hosting/EditorHost.cs
src/composition/editor/host/Inno.Editor.Application/Hosting/EditorAuthoringServices.cs
src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettingsStore.cs
build/pipeline/Inno.Build/BuildSettingsStore.cs
build/cli/Inno.Build.Cli/BuildWorkspace.cs
```

静态 Player 不要求动态加载缓存目录。DotNet module 来源继续负责 shadow copy，并保留完整卸载验证。

Player Settings 使用从内容 store 取得的只读文档。写操作明确失败，不能静默创建另一份设置。

### 4.8 存储 namespace 与具体位置分离

```text
src/services/storage/Inno.Storage/
└─ StorageScope.cs                              [新增] 中立逻辑 namespace

src/adapters/storage/Inno.Adapter.Storage/
├─ IStorageBackendFactory.cs                     [修改]
├─ StorageBackendProvider.cs                     [修改]
└─ StorageBackendCatalog.cs                      [修改]

src/adapters/storage/Inno.Adapter.Storage.FileSystem/
└─ FileSystemStorageBackendProvider.cs           [修改] 宿主注入物理根

src/adapters/storage/Inno.Adapter.Storage.Browser/
└─ BrowserStorageBackendProvider.cs              [修改] Origin 与 namespace

src/adapters/default/Inno.Adapter.Default/
├─ DefaultAdapterCatalog.cs                      [修改]
└─ DefaultAdapterCatalogOptions.cs               [新增] 显式存储 factory

src/adapters/default/Inno.Adapter.Authoring.Default/
└─ DefaultAuthoringAdapterCatalog.cs             [修改]

src/composition/default/Inno.Engine.Default/
├─ EngineSessionComposition.cs                  [修改] 会话存储创建边界
└─ Storage/StorageSubsystem.cs                   [修改] 不拼接物理路径
```

- 发布游戏的数据 namespace 默认仍来自 application ID 或用户配置。
- 物理位置由 FileSystem provider 的宿主配置决定。
- Browser provider 将同一 namespace 映射为浏览器存储。
- 保留当前 Editor Edit／Play 与不同 Project 的隔离语义。
- 路径不引入 `InnoEngine` 产品前缀。
- 不提供旧 namespace 双写或回退。

### 4.9 音频、字体、UI 与 Shader 读取同步整改

```text
src/services/audio/Inno.Audio/
├─ IAudioDevice.cs                               [修改] Descriptor 不再包含 artifactPath
├─ AudioClipDescriptor.cs                        [拆分] 仅编码、长度、格式和模式
└─ IAudioClipSource.cs                           [新增] 中立编码读取来源

src/services/audio/Inno.Audio.Runtime/
├─ AudioClipCache.cs                             [修改]
└─ ArtifactAudioClipSource.cs                    [新增，internal]

src/adapters/audio/Inno.Adapter.Audio.MiniAudio/
├─ MiniAudioDevice.cs                            [修改]
├─ MiniAudioEncodedArtifactCache.cs              [新增，internal]
├─ MiniAudioClipPreparation.cs                   [新增，internal]
└─ MiniAudioClipSourceLease.cs                   [新增，internal]

src/services/text/Inno.Text.Runtime/
└─ TextRuntime.cs                                [修改]

src/services/ui/Inno.UI.Runtime/
└─ UiRuntime.cs                                  [修改]
```

音频方案固定为：

- Runtime 通过 lease 提供编码内容。
- MiniAudio 当前文件型入口所需的 materialization 收口在该 Adapter。
- 使用内容身份复用编码缓存，不按 voice 重复复制。
- 准备异步执行，owner thread 只发布准备结果。
- Stream 模式继续使用原生流式解码，不改为整段 PCM 常驻。
- 音频实时 callback 保留在原生侧。
- 销毁前取消并完成准备任务，再释放 source lease。
- 具体 Adapter 的临时文件能力要求不得进入 `Inno.Audio`。
- 不增加 Browser 专用 MiniAudio 项目。

同时修改：

```text
Inno.Rendering.Assets.Authoring/
  Importing/ShaderAssetImporter.cs
  Importing/ShaderFunctionImporter.cs
  Compilation/ShaderSourceBundle.cs
  Compilation/ShaderGraphArtifact.cs

Inno.Editor.Rendering/
  Compilation/EditorRenderTargetArtifactProvider.cs
  Compilation/EditorRenderTargetArtifactProvider.Drafts.cs

Inno.Build.Toolchains.Bgfx.Tools/
  BgfxGameContentCompiler.cs
  BgfxTextureTargetCompiler.cs
  BgfxShadercToolchain.cs
  Intermediate/BgfxShadercToolchain.Typed.cs
  Intermediate/BgfxShadercToolchain.Reflection.cs
  Intermediate/BgfxShaderIrGenerator.cs
```

SDK 必须使用文件时，由具体工具链创建短生命周期输入文件。运行服务不取得这个文件路径。

### A02／A04 验收

- 缓存文件缺失、额外、损坏、同长度同 mtime 篡改都能被检测。
- 两个进程同时准备同一 Pack，不能互删目录或暴露半成品。
- Reader 存活时修复缓存，旧 reader 所持 generation 不被退休删除。
- 损坏 staging、权限拒绝、取消、提交后清理失败分别验证。
- 通过纯内存内容来源完成真实公开 Player／Session 流程。
- 使用真实 MiniAudio 验证 Decode、Stream、预加载取消和退休。
- Shared Player、Runtime Session、AssetDatabase 不直接读取部署文件或创建目录。
- Font、UI、Shader、Texture、Geometry 都通过相同 artifact lease 读取。

---

## 五、A03：通用输入入口归属修正

```text
src/adapters/input/Inno.Adapter.Input/
├─ Inno.Adapter.Input.csproj                     [修改]
├─ InputBackendId.cs                             [修改] 通用 events ID
├─ InputBackendProvider.cs                       [保留]
├─ InputBackendCatalog.cs                        [保留]
├─ IInputBackendFactory.cs                       [保留]
├─ InputBackend.cs                               [保留]
├─ EventInputSource.cs                           [迁移 Sdl3InputSource]
├─ EventInputBackend.cs                          [迁移 Sdl3InputBackend]
└─ EventInputBackendProvider.cs                  [迁移 Sdl3InputBackendProvider]
```

删除整个：

```text
src/adapters/input/Inno.Adapter.Input.Sdl3/
```

具体规则：

- Platform Adapter 将系统事件转换为 Core Events。
- `EventInputSource` 只处理中立事件。
- Desktop、Browser、Editor Play 共用同一实现。
- GameView 焦点策略筛选事件，不新增输入事件总线。
- 消费、失焦释放、窗口身份与 Session 隔离保持原语义。
- 旧 backend ID 从当前配置和消费者中移除，不保留 alias。

同步文件：

```text
src/adapters/common/Inno.Adapter/AdapterSelection.cs
src/adapters/default/Inno.Adapter.Default/DefaultAdapterCatalog.cs
src/composition/editor/panels/Inno.Editor.Panel.GameView/ 相关输入消费者
tests/input/Inno.Input.Tests/InputRuntimeTests.cs
tests/input/Inno.Input.Tests/ShellLifecycleTests.cs
tests/editor/Inno.Editor.PlayMode.Tests/EditorGameInputCaptureTests.cs
```

### A03 验收

- 通用 Input 程序集没有 SDL 或 Native 引用。
- Press／Release／Pointer／Wheel／Text 输入由同一事件入口产生快照。
- 浮动 GameView 聚焦后可输入，失焦后释放全部按键。
- Popup、Modal、前景窗口阻止底层游戏输入。
- 已消费事件不能重新影响游戏。
- Edit、Play、多个窗口之间没有快照串用。

---

## 六、A05：构建组合唯一化

### 6.1 新共享组合库

```text
build/composition/Inno.Build.Composition/
├─ Inno.Build.Composition.csproj                 [新增，库]
├─ BuiltInBuildDistribution.cs                   [新增] 唯一内置注册
├─ BuildDistribution.cs                         [新增] 不可变组合结果
├─ BuildCompositionContext.cs                   [新增] 显式宿主依赖
└─ BuildPipelineFactory.cs                      [新增] 共用 Pipeline 创建
```

`BuildDistribution` 同时提供：

- 平台 target catalog。
- managed deployment catalog。
- Support Pack publisher。
- 目标、部署与 Support Pack 的一致性预检。

不得由调用者分别创建三套内置列表。

### 6.2 修改与删除

```text
build/cli/Inno.Build.Cli/
├─ BuildComposition.cs                          [修改] 只传入 CLI 上下文
├─ EngineBuildWorkflow.cs                       [保留现有 Native 修复]
├─ ProjectBuildWorkflow.cs                      [修改消费者]
└─ Inno.Build.Cli.csproj                         [修改]

build/support/Inno.Build.SupportPacks/
├─ BuiltInPlayerSupportPacks.cs                  [删除] 注册职责迁入 Composition
└─ 具体 Support Pack Source                     [保留] 实现职责不迁入组合库

build/tasks/Inno.Build.Tasks/
├─ PublishSupportPackTask.cs                    [修改] 使用共享组合
└─ Inno.Build.Tasks.csproj                      [修改]

src/composition/editor/host/Inno.Editor.Application/
├─ Hosting/EditorHost.cs                        [修改] 删除目标／compiler 重复列表
└─ Inno.Editor.Application.csproj               [修改]
```

### A05 验收

- Editor、CLI、MSBuild 获得相同的 target 与 deployment 集合。
- 新增一个 fixture target/provider 后，仅在一个组合入口注册即可被三者使用。
- 不支持的 target/deployment 配对在构建前明确失败。
- Build 核心不引用具体 Composition。
- 新组合库没有 `Program.cs`。
- 缺 SDK、取消、失败与旧输出保留回归通过。

---

## 七、A06：Presentation 契约显式化

公开契约调整：

```text
IRenderDevice.primaryPresentationSize
    从默认 1×1 改为必须实现的 RenderPresentationSize?

IRenderDevice.ResizeBackbuffer(...)
    替换为 SetPrimaryPresentationSize(RenderPresentationSize?)
```

语义：

- 有效尺寸：当前可用主输出的真实物理像素尺寸。
- `null`：当前没有可用主输出，包括无 surface 或暂时不可呈现。
- 无效零尺寸值：实现错误，明确拒绝。
- Native SDK 为内部资源保留最小尺寸时，不把该内部值报告为可用输出。

修改文件：

```text
src/services/rendering/Inno.Rendering/Device/
  IRenderDevice.cs
  RenderDevice.cs
  RenderPresentationSize.cs

src/adapters/rendering/Inno.Adapter.Rendering.Bgfx/
  BgfxDevice.cs
  BgfxDeviceOptions.cs
  BgfxDeviceBackendProvider.cs                若当前尺寸创建消费者位于此处

src/composition/shell/Inno.Shell/
  Shell.cs

src/services/rendering/Inno.Rendering.Runtime/
  RenderRuntime.Presentation.cs
  RenderTargetStore.cs

src/composition/player/Inno.Player.Runtime/
  GamePlayerHost.cs

src/composition/editor/host/Inno.Editor.Application/Hosting/
  EditorRenderingHostService.cs
```

所有 `IRenderDevice` 实现与测试 device 同步修改，不保留默认实现。

运行规则：

- 当前无主输出时跳过主输出图与其输入坐标换算。
- 离屏请求使用自身有效尺寸，继续执行。
- 尺寸与可用性在帧安全点发布。
- Resize、最小化、恢复、DPI 变化沿同一平台事件流程更新。
- 删除 `Math.Max(1, …)` 掩盖输入 surface 缺失的流程。

### A06 验收

覆盖：

- 正常窗口与真实高 DPI 尺寸。
- 窗口最小化与恢复。
- 无主输出的离屏渲染。
- Resize 与 frame 并发。
- 无效 backend 返回值。
- 失去输入 surface 时不进行错误坐标转换。
- Desktop 与 Web 昼夜颜色、光照方向与星光效果保持正确。

---

## 八、A07：Native 增量构建与 Task 引导整改

### 8.1 统一 Task 引导

```text
build/msbuild/
└─ Inno.Build.Tasks.targets                     [新增] 唯一 Task 引导规则

build/tasks/Inno.Build.Tasks/
├─ PrepareEditorNativeTask.cs                   [修改]
├─ GenerateBindingsTask.cs                      [修改]
├─ CompileShaderTask.cs                         [修改]
├─ PublishSupportPackTask.cs                    [修改]
└─ NativeBindingGenerationIdentity.cs           [修改]

Directory.Build.props                          [修改]
Directory.Build.targets                        [修改]
build/Directory.Build.props                    [修改]

src/adapters/rendering/Inno.Adapter.Rendering.Bgfx/
└─ Inno.Adapter.Rendering.Bgfx.csproj            [修改] 删除独立重复引导

src/composition/editor/host/Inno.Editor.Application/
└─ Inno.Editor.Application.csproj               [修改] 引用共同规则
```

要求：

- 同一配置与工具身份在一次构建闭包中只准备一次 Task。
- 使用隔离、稳定的 host artifact 目录。
- 去除目标 RID、Wasm、AOT 与 IDE 全局属性对工具引导的污染。
- 工具自身显式构建必要 ProjectReference。
- Design-time build 不执行 Native 工作。
- 普通 Build 与 Publish 仍准备完整匹配的原生闭包。
- 不重新引入 CLI 的第二次安装。

### 8.2 构建身份与输入快照

```text
build/toolchains/Inno.Build.Toolchains/
├─ NativeBuildContext.cs                        [修改] operation-owned 输入与工具身份
├─ Native/
│  ├─ NativeBuildRecipe.cs                       [新增] 完整声明 recipe
│  ├─ NativeBuildInput.cs                        [新增] 逻辑身份与物理读取位置分离
│  ├─ NativeInputSnapshot.cs                     [新增，internal]
│  ├─ NativeBuildFingerprint.cs                  [修改]
│  ├─ NativeArtifactPublisher.cs                 [修改]
│  ├─ NativeBindingGenerationDescriptor.cs       [修改]
│  └─ HostNativeToolchain.cs                     [修改]
└─ BuildArtifactManifest.cs                     [修改]
```

指纹必须覆盖：

- target、ABI、配置。
- compiler、SDK、sysroot 与 linker 的实际身份。
- include 搜索顺序、defines、编译及链接参数。
- Native 源码、facade、生成桥与构建定义。
- 实际参与 recipe 的组件代码及共用执行代码。
- BGCS 生成器实现、配置、扩展与目标描述。
- 所需 Native exports 和对应生成身份。

不再以整个无关工具程序集的 MVID 作为组件 recipe 身份。

相关源码以组件声明的输入闭包进入哈希；共用构建代码仍参与失效，不能为了缩小范围漏掉真实依赖。

路径规则：

- 仓库输入使用逻辑根和相对路径作为身份。
- 物理路径用于读取与执行。
- 实际影响产物的路径参数继续参与指纹。
- 托管工具采用确定性构建与源码路径映射；这与 `.NET` 的 [`PathMap` 定义](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/main-compiler-option)一致。
- 跨目录复用必须通过产物一致性测试，不能仅删除绝对路径字符串。

### 8.3 组件接入

逐一修改：

```text
build/toolchains/
├─ Inno.Build.Toolchains.Bgfx/BgfxNativeBuild.cs
├─ Inno.Build.Toolchains.Bgfx.Tools/BgfxShadercToolchain.cs
├─ Inno.Build.Toolchains.ImGui/ImGuiToolchain.cs
├─ Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs
├─ Inno.Build.Toolchains.MiniAudio/MiniAudioToolchain.cs
├─ Inno.Build.Toolchains.Sdl3/Sdl3Toolchain.cs
├─ Inno.Build.Toolchains.Text/TextToolchain.cs
├─ Inno.Build.Toolchains.UI/UiToolchain.cs
├─ Inno.Build.Toolchains.Host/HostNativeBuild.cs
├─ Inno.Build.Toolchains.Host/HostNativeDeployment.cs
└─ Inno.Build.Toolchains.Browser/BrowserToolchain.cs
```

执行优化：

- 同一验证阶段共用的输入文件只枚举、读取一次。
- 等待 lease 后、冷构建完成后等必要验证阶段仍保留。
- 不以长度或 mtime 代替内容哈希。
- 并发修改导致 snapshot 不稳定时，明确拒绝发布。
- Host 组件共享已冻结的 SDK/tool selection。
- 内容一致的部署不复制、不更新 mtime、不替换已加载 DLL。
- Support Pack 与 Native／binding fingerprint 同步失效。

### A07 验收

- 热构建不启动 Native compiler 或重新生成相同 binding。
- 相同 Task 身份只引导一次。
- 修改 Editor UI 不使无关 Native 组件失效。
- 修改 Text facade、桥、配置、compiler 参数会使 Text 正确失效。
- 修改共用 recipe 执行逻辑会使依赖组件失效。
- 同长度、同 mtime 输入修改仍失效。
- Native 输出被损坏时不能命中有效缓存。
- 并发 Build、取消、锁等待、加载中的 DLL、缺 export 都有回归。
- 在同一环境进行冷构建及三次热构建，记录阶段耗时、扫描字节、进程数和部署次数；确认重复成本消除并给出实际改善。

---

## 九、A08：职责展开与帧路径优化

### 9.1 大文件的明确拆分

以下拆分保留唯一状态 owner，不另建事件总线、Undo 栈或 live object 管理表。

```text
src/content/assets/Inno.Assets.Pipeline/Importing/
├─ AssetLoader.cs                              构造、公开入口、生命周期字段
├─ AssetLoader.Importing.cs                     Import／build／commit
├─ AssetLoader.Loading.cs                       Shell、hydrate、依赖加载
├─ AssetLoader.Saving.cs                        Save 与事务回滚
├─ AssetLoader.Catalog.cs                       Catalog、record、索引维护
├─ AssetLoader.Dependencies.cs                  Import/runtime 依赖图
├─ AssetLoader.SourceChanges.cs                 Rescan、rename、delete
├─ AssetLoader.RuntimeExport.cs                 Runtime closure 与导出
├─ AssetLoader.Residency.cs                     Lease、sweep、retention
├─ AssetLoader.Recovery.cs                      Missing recovery 与 identity 激活
├─ AssetLoader.Retirement.cs                    旧代与资源退休
├─ AssetLoader.Diagnostics.cs                   Import health 与诊断
├─ AssetLoader.SourceIO.cs                      Source stamp、metadata、文件事务
└─ AssetLoader.ImportSettings.cs                保留现有职责

src/content/assets/Inno.Assets.Pipeline/
├─ AssetPipeline.cs                            构造、入口与唯一生命周期
├─ AssetPipeline.Loading.cs                     Load／Acquire facade
├─ AssetPipeline.Mutations.cs                   Save／Move／Delete／Create
├─ AssetPipeline.Changes.cs                     Watcher 对账与事件
├─ AssetPipeline.Artifacts.cs                   Artifact 与 runtime export
├─ AssetPipeline.Discovery.cs                   候选 Catalog/Registry
├─ AssetPipeline.Retirement.cs                  Shutdown 与 pending retirement
└─ Sources/
   ├─ AssetPipeline.SourceMounts.cs             完整 Mount 事务收口
   └─ AssetPipeline.Samples.cs                  保留异步 Sample 流程

src/adapters/rendering/Inno.Adapter.Rendering.Bgfx/
├─ BgfxDevice.cs                               构造、设备入口与 owner
├─ BgfxDevice.Frames.cs                        帧与 resize
├─ BgfxDevice.Textures.cs                      Texture 操作
├─ BgfxDevice.Readback.cs                      Readback
├─ BgfxDevice.Graphs.cs                        Graph／pass 执行
├─ BgfxDevice.FrameBuffers.cs                  Framebuffer cache
├─ BgfxDevice.Transients.cs                    Transient reuse
├─ BgfxDevice.Retirement.cs                    Deferred destroy／Dispose
├─ BgfxCommandEncoder.cs                       Encoder 状态与入口
├─ BgfxCommandEncoder.Bindings.cs              Shader/resource binding
├─ BgfxCommandEncoder.Geometry.cs              顶点、索引、实例数据
├─ BgfxCommandEncoder.Draw.cs                  Draw／indirect draw
├─ BgfxCommandEncoder.Compute.cs               Dispatch
├─ BgfxCommandEncoder.Transfers.cs             Copy／blit
├─ BgfxCommandEncoder.State.cs                 Raster／sampler／stencil 映射
└─ BgfxCommandEncoder.Validation.cs            参数及能力校验

src/foundation/core/Inno.Core.Collections/IndexedObjectStore/
├─ IndexedObjectStore.cs                       生命周期与集合 owner
├─ IndexedObjectStore.Keys.cs                  Key/index 管理
├─ IndexedObjectStore.Handles.cs               Sparse/dense generation handle
├─ IndexedObjectStore.Queries.cs               无序查询
└─ IndexedObjectStore.OrderedQueries.cs        有序查询
```

Editor 文件保持项目级命名空间：

```text
src/composition/editor/features/Inno.Editor.Scene/Documents/
├─ EditorSceneWorkspace.cs                     生命周期与入口
├─ EditorSceneWorkspace.Persistence.cs         Save/open/source document
├─ EditorSceneWorkspace.SourceChanges.cs       Rename/replacement 对账
├─ EditorSceneWorkspace.Restore.cs             Pending setup 恢复
├─ EditorSceneWorkspace.PlayMode.cs            Play lease 与场景隔离
├─ EditorSceneWorkspace.Reload.cs              Workspace 候选事务
├─ EditorSceneWorkspace.History.cs             文档 history 接入
├─ SceneEdits.cs                               公开 mutation facade
├─ SceneEdits.Objects.cs                       GameObject／Prefab
├─ SceneEdits.Elements.cs                      Component／System
├─ SceneEdits.Properties.cs                    单属性 delta
├─ SceneEdits.Hierarchy.cs                     Placement
└─ SceneEdits.History.cs                       Neutral payload／rollback

src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Browser/
├─ FileBrowserPanel.cs                         Panel 生命周期与状态
├─ FileBrowserPanel.Layout.cs                  Pane、splitter、滚动 owner
├─ FileBrowserPanel.Navigation.cs              Toolbar／breadcrumb
├─ FileBrowserPanel.Filters.cs                 Search／filter
├─ FileBrowserPanel.List.cs                    Table/list
└─ FileBrowserPanel.Grid.cs                    Grid
```

实施顺序：

1. 先按职责移动现有成员，保持行为。
2. 核对语法 token、初始化与执行顺序。
3. 通过原测试。
4. 再单独实施性能和行为修改。
5. 字段仍集中于主要 owner 文件；不在 partial 文件中散落重复状态。

### 9.2 Contributor 快照与 scratch

- 注册变化时生成不可变 Contributor snapshot。
- 帧开始固定一个 snapshot，不每帧复制全量集合。
- 注册／注销在安全点发布，明确当前帧与下一帧语义。
- 私有 scratch 保留容量，帧结束或异常时清空引用。
- 需要借用池化空间时在 `finally` 返回，并清除可能持有 extension 的槽位。
- 不池化外部仍可能持有的公开快照或已编译图。
- 不跨帧缓存 graph handle、pass delegate 或 collectible extension。

### 9.3 Validation 与 Compile 分离

当前 `Validate()` 创建完整 `CompiledRenderGraph` 的路径删除。

新的行为：

- `Validate()` 返回 `RenderGraphValidationResult`，包含有效性、诊断与必要计数。
- Validator 检查资源、使用关系、能力、依赖、cycle 与 view limit。
- Compile 复用当前 revision 的验证分析结果。
- 资源 alias 分配与最终 `CompiledRenderGraph` 构造只在最终 Compile 执行。
- 一个正常渲染帧最终完整编译一次。

### 9.4 Mutation rollback 完整化

- 每个 mutation scope 记录新增资源、pass、output 和分析索引变化。
- 已提交的 pass 声明冻结；后续 scope 不能修改前一 scope 的 pass。
- 所有 pass mutation 经过同一 owner 检查。
- 新 phase 或 resource use 影响旧依赖时，更新受影响依赖闭包。
- 验证状态按 graph revision 失效，不复用过期结果。
- 失败回滚 graph 和验证索引。
- `Commit()` 只接受有效的当前 mutation。
- 最终完整验证保留，作为执行前最后 gate。

### A08 验收

- Contributor 失败不污染已接受的图。
- scope rollback 后的资源、pass、outputs、phase 和诊断与原状态一致。
- cycle、未初始化读取、attachment/store、capability、max views 全部保持检查。
- 对同一图比较优化验证与最终 Compile 的诊断及调度结果。
- 使用 0／1／8／32 个 Contributor、成功／异常／退休场景测量时间与分配。
- 最终 Compile 次数不超过每渲染帧一次。
- Contributor 集合未变化时没有快照复制。
- reload 后 scratch、snapshot、callback 不持有旧 ALC。

---

## 十、测试文件与验证矩阵

### 10.1 新增测试项目

```text
tests/content/
├─ Inno.Content.Tests/
│  ├─ Inno.Content.Tests.csproj
│  ├─ ContentKeyTests.cs
│  ├─ ContentPackIndexTests.cs
│  ├─ PackContentStoreTests.cs
│  └─ ContentReadLeaseTests.cs
└─ Inno.Adapter.Content.FileSystem.Tests/
   ├─ Inno.Adapter.Content.FileSystem.Tests.csproj
   ├─ ContentCacheIntegrityTests.cs
   ├─ ContentCacheConcurrencyTests.cs
   ├─ ContentCacheCancellationTests.cs
   └─ ContentCacheRetirementTests.cs
```

### 10.2 现有测试项目整改

```text
tests/core/Inno.Core.IO.Tests/
  FileLeaseTests.cs                            共享／独占／多进程
  ByteDocumentStoreTests.cs                    新增

tests/assets/Inno.Assets.Tests/
  RuntimeContentStoreTests.cs                  新增
  ArtifactReadLeaseTests.cs                    新增

tests/assets/Inno.Assets.Pipeline.Tests/
  AssetLoaderTests.cs                         路径中立 lease、回归
  SettingsReferenceTests.cs                   文档来源和完整 resolver

tests/runtime/Inno.Runtime.Tests/
  RuntimeSessionTests.cs                     注入内容与日志
  RuntimeRetirementTests.cs                  失败和 pending retirement
  StaticProjectSettingsTests.cs              只读内容 Settings
  PlayerContentSourceTests.cs                新增
  PlayerWithoutFileSystemTests.cs            新增
  GamePresentationSettingsTests.cs           尺寸／输入

tests/rendering/Inno.Rendering.Tests/
  RenderGraphCompilerTests.cs                保留完整语义
  RenderGraphValidationTests.cs              新增
  RenderGraphMutationTests.cs                新增
  PresentationContractTests.cs               新增

tests/rendering/Inno.Rendering.Assets.Tests/
  RuntimeAssetSerializationTests.cs          新增
  MaterialContractTests.cs                   新增
  GeometryArtifactTests.cs                   新增

tests/rendering/Inno.Rendering.Assets.Authoring.Tests/
  从现有 Assets.Tests 迁入创作测试
  ShaderPublicationTests.cs
  ShaderGraphArtifactTests.cs
  ShaderCompilerTests.cs
  RenderingImporterIntegrationTests.cs
  MeshAndStateTests.cs

tests/rendering/Inno.Rendering.Runtime.Tests/
  ContentRenderTargetArtifactProviderTests.cs 替换旧 File provider 测试
  RenderContributorTests.cs                  新增
  RenderFrameAllocationTests.cs              新增
  ShaderProgramPublicationTests.cs           回归
  EmptyRenderingKernelTests.cs               回归

tests/build/Inno.Build.Tests/
  BuildCompositionTests.cs                   新增
  NativeTaskBootstrapTests.cs                新增
  NativeArtifactIdentityTests.cs             recipe 失效
  NativeArtifactPublicationTests.cs          并发／失败
  NativeBindingGenerationTests.cs            身份对应
  NativeBindingCompilationTests.cs           实际消费
  BuildPipelineTests.cs                      新内容协议
  DesktopPublicationTests.cs                 当前部署
  BrowserToolchainTests.cs                   当前部署

tests/audio/
  Inno.Audio.Tests/AudioScriptingApiTests.cs
  Inno.Audio.Runtime.Tests/AudioRuntimeTests.cs
  Inno.Adapter.Audio.MiniAudio.Tests/EncodedContentTests.cs   新增

tests/tooling/Inno.Tooling.Architecture.Tests/
  ArchitectureSymbolTests.cs
  ArchitectureCliTests.cs
  RenderingBoundaryTests.cs                  新增
  PlayerBoundaryTests.cs                     新增
  RuntimeClosureTests.cs                     新增
```

同时更新 Input、Text、UI、Scripting、Editor History、Graph、PlayMode 的当前消费者测试。文件清单由语义闭包检查补全，不用字符串替换代替编译验证。

### 10.3 实际运行矩阵

使用：

```text
C:\Dev\GameEngineDev\InnoEngine.Samples\FlappyBird
```

必须重新验收：

| 路径 | 验收内容 |
|---|---|
| Windows Editor Debug／Release | 普通 IDE Build、启动、Play、退出 |
| Windows CoreCLR Player | 发布、启动、玩法、音频、持久化 |
| Windows NativeAOT Player | 静态注册、Native 调用、实际运行 |
| Web 解释执行 | 下载、启动、输入、渲染、存储 |
| Web AOT | 链接、回调、启动、玩法 |
| Canvas／Rendering2D | 编译、Plugin 消费、模型组合 |
| Reload | 成功、失败、强引用残留、Full GC barrier、Faulted |
| Editor UI | 浮动 GameView、前景命中、Modal、菜单、滚动条与表单 |

视觉检查包括：

- 昼夜亮度和颜色。
- 光照方向与移动。
- 夜晚星星效果。
- ShaderEditor 小地图。
- 长 label、小窗口、缩放与不同 DPI。
- Export 结束自动关闭。
- 重叠窗口不会关闭或操作底层 Panel。

macOS 实际 Native 编译和运行安排在 macOS 环境执行。当前 Windows 的检查不能替代 macOS 实机结果。

BGCS 重新运行自身托管、NativeAOT 与 Wasm 调用验证；结果单独报告，不把 FlappyBird 当作 BGCS 独立验收。

---

## 十一、架构检查、Solution 与文档

### 11.1 验证器修改

```text
tools/Inno.Tooling.Architecture/
├─ ArchitectureRules.cs                        新项目分类与允许依赖
├─ ArchitectureValidator.cs                    Closure 与旧路径检查
├─ PublicApiBoundaryValidator.cs               资产／Native／创作泄漏
├─ PublicApiDocumentationValidator.cs           新公开 API XML
├─ GenerationCleanupValidator.cs               新 snapshot／scratch 生命周期
└─ CSharpStyleValidator.cs                      新文件和 partial 排版
```

新增强制检查：

- Rendering Core 的禁止依赖。
- Runtime／Player 发布闭包中的 Authoring 程序集。
- Shared Player、AssetDatabase、Runtime Session 的物理部署 IO。
- 通用 Input 的 SDL／Native 引用。
- 各入口重复内置 build 注册。
- 已删除 API、项目、namespace 与路径残留。
- public/protected 签名和实际 ProjectReference 可见性。
- 单项目唯一 scripting 清单。
- Native 生成身份与目标输出隔离。

规则采用允许的项目职责与语义符号检查，不用误伤 `InvalidDataException` 等中立类型的粗暴文本禁用。

### 11.2 项目与 Solution

新增四个生产库：

```text
Inno.Content
Inno.Adapter.Content.FileSystem
Inno.Rendering.Assets.Authoring
Inno.Build.Composition
```

删除：

```text
Inno.Adapter.Input.Sdl3
```

同步：

- `InnoEngine.sln`。
- 每个受影响 `.csproj`。
- Player Support Pack 模板引用。
- Native Task 公共引导。
- Editor public／implementation 引用分组。
- 新测试项目归类。

删除空 Solution Folder，不保留旧项目占位。

### 11.3 文档交付

```text
docs/architecture/
├─ ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md         完整执行清单
├─ ARCHITECTURE_CLEANUP_ACCEPTANCE_2026_10_06.md   当前验收
├─ ENGINE_ARCHITECTURE_OVERVIEW.md                最终程序集与依赖
├─ PLATFORM_RUNTIME_ARCHITECTURE.md              内容／日志／存储组合
├─ PLATFORM_RUNTIME_PLAN_AUDIT.md                旧 Plan 与本轮整改对照
├─ WEB_PLAYER_ARCHITECTURE.md                    新 Browser 启动
├─ IDENTITY_REFERENCE_RELOAD_STANDARD.md         生命周期复核
├─ CSHARP_DEVELOPMENT_STANDARD.md                职责展开与所有权规则
├─ CURRENT_ISSUES.md                             八项状态与剩余证据
└─ README.md                                    索引
```

新增项目页：

```text
docs/assets/Inno.Content.md
docs/assets/Inno.Adapter.Content.FileSystem.md
docs/rendering/Inno.Rendering.Assets.Authoring.md
docs/build/Inno.Build.Composition.md
```

更新 Core.IO、Settings、Assets、Rendering 四层、Input、Storage、Player、Build、Toolchain 与 Editor 的对应项目页及分类索引。

所有公开 API 表格以最终源码为准；内部 cache generation、scratch 和验证索引明确标记为内部实现。

`AGENTS.md` 更新对应边界和禁止反复出现的问题，不重复堆叠相同原则。

---

## 十二、实施顺序与阶段门禁

| 阶段 | 工作 | 通过条件 |
|---|---|---|
| 0 | 保存基线、完整文件映射、API 与性能记录 | 当前问题能复现，现有修复被保存 |
| 1 | Content、IO、缓存、Asset lease、Session、Settings、Player 解耦 | A02／A04 契约与负向测试通过 |
| 2 | Rendering 四层与所有消费者迁移 | A01 边界、序列化、脚本消费通过 |
| 3 | 通用 Input 归属修正 | A03 焦点、消费、隔离通过 |
| 4 | 唯一 Build Composition | A05 三入口一致性通过 |
| 5 | Presentation 可用性与尺寸 | A06 离屏、最小化、高 DPI 通过 |
| 6 | Task 引导与 Native 增量身份 | A07 正确失效、并发与热构建通过 |
| 7 | 大文件展开、scratch、Graph validation | A08 语义等价、分配与退休通过 |
| 8 | 全发布矩阵、UI、文档与最终清理 | 全部本机 gate 与证据闭合 |

每阶段：

1. 同步公开契约与全部消费者。
2. 编译该阶段影响的项目闭包。
3. 执行成功、失败、取消与边界测试。
4. 更新文件映射和验收记录。
5. 删除已经被替代的实现。
6. 失败修复后再进入下一阶段，不用临时兼容层维持编译。

---

## 十三、最终验收报告格式

最终报告必须逐项回答：

### 架构

- A01–A08 是否完成。
- 最终依赖图。
- 新增与删除项目。
- 公开 API 变化及必要性。
- 每项资源的 owner 与退出顺序。
- 如何新增内容来源、平台、图形后端和部署 compiler。

### 正确性

- 实际命令、环境和 revision。
- 测试结果与未通过项。
- 缓存损坏、并发、取消与回滚证据。
- Native exports、生成身份与部署对应关系。
- reload GC barrier 与旧代不可达结果。
- FlappyBird 四条本机发布路径的运行证据。

### 性能

- 冷／热构建前后数据。
- Task 引导与工具进程数量。
- Native 输入扫描次数及字节数。
- 帧分配、Contributor 快照、最终 Compile 次数。
- 性能结论必须由测量支持。

### 清洁度

- 无遗留旧路径或兼容 wrapper。
- 无空项目、空 Solution Folder、冗余 Program。
- 无反向依赖或 Authoring 进入 Player。
- 手写源码符合命名、英文 XML、显式 using 和多参数排版。
- Wiki 项目覆盖、源码签名和链接一致。

**完成标准：八项源码整改、所有消费者和本机必需验收全部通过。未具备执行环境的平台单独标记“未实测”，不能用架构检查通过替代实机验收。**

以上为批准的执行规格。A01–A08 已执行并完成本机验收，最终源码归属、API、实测结果及外部设备边界见[架构整改验收](ARCHITECTURE_CLEANUP_ACCEPTANCE_2026_10_06.md)。Windows 无法访问指定的 `Glass.aiff`，提示音未播放。



