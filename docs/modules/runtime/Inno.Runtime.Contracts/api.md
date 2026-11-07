# Inno.Runtime.Contracts API

[项目入口](../Inno.Runtime.Contracts.md)

## 公开 API

| 契约 | 稳定语义 |
| --- | --- |
| `IRuntimeSubsystem` | Attach、BeginFrame、FixedUpdate、Update、LateUpdate、BeforeRender、Render、AfterRender、EndFrame、Detach、Dispose 的有限生命周期 |
| `RuntimeSubsystem` | 上述非虚公开入口的模板实现；统一 owner thread、frame scope、异常退出、取消与逆序释放 |
| `IRuntimeSubsystemFactory.descriptor` / `Create(context)` | 声明稳定身份、依赖和 owner 范围，构造独立实例 |
| `RuntimeSubsystemId` | 开放 semantic ID；不是 Object Identity 或 CLR 类型名 |
| `RuntimeSubsystemDescriptor` | 冻结 id、order、dependencies、lifetime、requirement、requiredCapabilities；拒绝非法 enum、空 ID、自依赖和重复声明 |
| `RuntimeSubsystemRequirement.Required/Optional` | Required 失败阻止 owner 启动；Optional 只有完成补偿后才能进入 Unavailable，不能吞清理失败 |
| `RuntimeCapabilityId` | Composition 验证后提供的开放、区分大小写能力 ID，不是 backend enum 或 live object Identity |
| `RuntimeSubsystemLifetime` | Host 与 Session 是不同资源所有权，不能把 Host GPU runtime 塞进 Session pipeline |
| `RuntimeFrame` / `RuntimeFixedFrame` | 中立时钟值；不携带 Session、Scene 或设备 |
| `RuntimeSubsystemContext` | 显式 events、diagnostics、identities、types、resources、lifetime、isEditMode、冻结 capabilities；不是任意服务查询容器，不携带物理持久目录 |
| `RuntimeSubsystemRegistrationAttribute(id)` | 在发行程序集声明一个强类型 factory 方法 |
| `RuntimeSubsystemCatalogAttribute` | 要求构建期生成同一 composition 参数类型的 catalog |
