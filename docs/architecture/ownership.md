# 资源所有权与生命周期

[架构索引](README.md)

所有权回答四个问题：谁创建、谁可以借用、何时失效、谁最终释放。产品组合持有长寿命 owner；领域服务依赖中立契约，backend 将第三方资源限制封装在自身边界。

## 运行对象

当前 session 的 IdentityAllocator 注册运行对象，runtime identity 只在原 registry 和 generation 内有效。跨保存和重载保留 persistent identity；弱引用查询不延长对象生命。Missing 保留恢复所需的中立状态。详见 [Identity](../modules/core/Inno.Core.Identity.md) 和 [恢复事务](identity-and-reload.md)。

## 暂态呈现

Panel 借用当前 session、窗口和帧；绘制借用不能跨帧缓存。操作完成或异常时释放本次 scope，Panel 退休清理自己的状态。公共 Action 与 History 属于 Interactions，样式和布局属于呈现 owner。详见 [Editor UI](editor-ui.md)。

## 构建产物

发布器登记内容身份、目录、依赖与当前根。消费者在读取、复制或编译期间持有读取 lease，释放结果才结束借用；完成 Task 不能代替 GPU fence 或产物保护。退休重新检查 owner、依赖、活动读取者和锁。详见 [构建缓存](build-and-cache.md) 与 [Toolchains](../development/build/Inno.Build.Toolchains.md)。

## 失败与退休

候选准备完成后才发布；失败保留 last-good 与原始诊断。旧 collectible generation 先停止准入、解除强引用，再等待实际 GC barrier。超时属于失败，不把 Pending 当完成。具体释放顺序、线程和重试边界以所属 API 的使用契约为准。
