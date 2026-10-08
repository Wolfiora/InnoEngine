# Native 边界与生成绑定

[Wiki 首页](../README.md) · [共享后端](../backends/README.md) · [绑定生成](BindingGeneration.md) · [构建机制](../build/Inno.Build.Toolchains.md)

每个 Native 组件只有一个源码 owner，归所属 `backends/<component>/native`。本目录提供通用机制说明；具体 Native API 的唯一项目页位于对应 backend 分类。

```text
backends/<component>/native/Inno.Native.<component>/
├─ Native/include、src                 手写窄语义 facade
├─ Native/Generated                   宿主 C 桥生成物
├─ Bindings                           共同定义、明确目标 profile、生成扩展
├─ Generated/Bindings.cs              宿主 managed 生成物
└─ obj/<target>/<generationFingerprint>/
   ├─ Native                         选定目标桥
   └─ Generated/Bindings.cs           选定目标 managed source
```

纯 C API 不建立无用途 facade 目录。不同 ABI、工具身份和配置不能共享错误布局；目标桥与 managed binding 由同一 generation 描述选择。原生 exports 必须与实际绑定一致，运行 Loader 只读取明确部署文件，不承担构建或缓存探测。

SDK 选择属于平台 Build，组件语义与第三方依赖属于 backend。Emscripten 是 Browser 的 Native 工具链；BGCS 仍是独立生成库，不依赖引擎产品。任何目标生成都通过 facade/config/公开生成入口完成，禁止手改 Bindings 或 extern。
