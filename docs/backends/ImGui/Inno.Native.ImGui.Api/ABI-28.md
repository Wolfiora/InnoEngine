# Inno.Native.ImGui ABI · Viewport callbacks

[项目与生命周期](../Inno.Native.ImGui.md#viewport-callback-abi-适配) · [ABI 索引](README.md)

此页来自当前 SDK 编译图的签名清单。原生 handle 的所有权、线程与注销前置条件以项目页为准；这些 ABI 不属于脚本 API。

<!-- api: M:Inno.Native.ImGui.ImGuiContext.RawApi.InnoDestroyPlatformWindowCallbacks(System.Void*) 06763477b7e0acf586414b0ebb7cbd99d8e821c40d71f56bb5b21da7d2047199 -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.RawApi.InnoCanDestroyPlatformWindowCallbacks(System.Void*) f406d9fb914c79f24d6fa3a30b7f324f9cd2b1c801debdc72d91f36f66d6fe9f -->
<!-- api: M:Inno.Native.ImGui.ImGuiContext.RawApi.InnoCreatePlatformWindowCallbacks(Inno.Native.ImGui.ImGuiNativeContext*,,) 42adfa5abe83cfd0a297fa71501c7b8a70fb3258ce8411cff0210233b8700253 -->
<!-- api: T:Inno.Native.ImGui.InnoImGuiWindowCallback eb0ba8567b0c2d4585200eb18962367ae4f698946feaafbcede04f767ec5bf8f -->
<!-- api: M:Inno.Native.ImGui.InnoImGuiWindowCallback.#ctor(System.Object,System.IntPtr) 3f228a87b92983d626efd5e9cb8eeffe53eea6d61ada0b6e7fe8987d63ada2e4 -->
<!-- api: M:Inno.Native.ImGui.InnoImGuiWindowCallback.Invoke(Inno.Native.ImGui.ImGuiViewport*,System.Single,System.Single) b641694a794cafb8e145aea9fb540ea2bdb5ad13c1eabfe5c2d0c9c8e210e3c1 -->
<!-- api: M:Inno.Native.ImGui.InnoImGuiWindowCallback.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.Single,System.Single,System.AsyncCallback,System.Object) 0702af9ffcbecd6c02e8dcb7e2274780020541ec9a63e7eee5973e506580ab15 -->
<!-- api: M:Inno.Native.ImGui.InnoImGuiWindowCallback.EndInvoke(System.IAsyncResult) 7ec23e20a4802f9366ca346e3c594da5fd41bad1878c6d42e0afe332ce5287f3 -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-0981fd6b03cf4b06"></a>[`public void Inno.Native.ImGui.ImGuiContext.RawApi.InnoDestroyPlatformWindowCallbacks(void* registration)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14556) | 仅在同线程 quiescence 查询成功且没有介入其他调用时注销并释放；handle 随即失效。 |
| <a id="api-70d6c5eff2e10947"></a>[`public int Inno.Native.ImGui.ImGuiContext.RawApi.InnoCanDestroyPlatformWindowCallbacks(void* registration)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15762) | 返回 1 表示当前 owner thread/context 且无活动原生 callback frame；0 表示必须保留注册。 |
| <a id="api-7e08d9f92996ba72"></a>[`public void* Inno.Native.ImGui.ImGuiContext.RawApi.InnoCreatePlatformWindowCallbacks(Inno.Native.ImGui.ImGuiNativeContext* context, delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, float, float, void> set_position, delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiViewport*, float, float, void> set_size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L21159) | 在当前 context 的 owner thread 注册两个标量 callback；userdata 必须空闲，空结果不保留 callback。 |
| <a id="api-9ed72a2a3d83fb60"></a>[`public delegate void Inno.Native.ImGui.InnoImGuiWindowCallback(Inno.Native.ImGui.ImGuiViewport* argument0, float argument1, float argument2)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L35548) | 位置/尺寸标量 callback 的生成 ABI 类型；viewport 仅在当前 callback 内借用。 |
| <a id="api-61e2d030796715c1"></a>[`public Inno.Native.ImGui.InnoImGuiWindowCallback.InnoImGuiWindowCallback(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L35548) | 编译器生成的 delegate 成员；实际注册使用静态 unmanaged function pointer，不通过此成员创建自动 owning registration。 |
| <a id="api-593e1a7b7376d9a2"></a>[`public virtual void Inno.Native.ImGui.InnoImGuiWindowCallback.Invoke(Inno.Native.ImGui.ImGuiViewport* argument0, float argument1, float argument2)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L35548) | 编译器生成的 delegate 成员；实际注册使用静态 unmanaged function pointer，不通过此成员创建自动 owning registration。 |
| <a id="api-97f173934f4f60d4"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.InnoImGuiWindowCallback.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* argument0, float argument1, float argument2, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L35548) | 编译器生成的 delegate 成员；实际注册使用静态 unmanaged function pointer，不通过此成员创建自动 owning registration。 |
| <a id="api-d9ce6e77248b1d3f"></a>[`public virtual void Inno.Native.ImGui.InnoImGuiWindowCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L35548) | 编译器生成的 delegate 成员；实际注册使用静态 unmanaged function pointer，不通过此成员创建自动 owning registration。 |
