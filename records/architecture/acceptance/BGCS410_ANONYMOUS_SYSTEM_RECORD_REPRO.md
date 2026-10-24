# BGCS 4.1.0：系统头匿名 union 的原生存储断言复现

[本轮验收](BGCS410_CONSUMER_ACCEPTANCE_2026_10_24.md) · [当前问题](../../../docs/architecture/CURRENT_ISSUES.md)

## 已确认的现象

正式 NuGet `BGCS` 和 `BGCS.Extension.BrowserWasm` 均为 **4.1.0**。独立 SDK 工程不引用任何 InnoEngine 程序集。官方生成返回 `success=True`，但其 `storage.assertions.c` 无法由实际 .NET workload 中的 Clang 编译。

原始产品失败发生在 MiniAudio 的 Browser native 编译。将输入缩减为一个公开头后，仍能复现，不需要 MiniAudio、BGFX 或引擎配置。

```c
#include <pthread.h>

typedef struct {
    pthread_cond_t condition;
    pthread_mutex_t mutex;
} anonymous_owner;

anonymous_owner roundtrip(anonymous_owner value);
```

实际 sysroot 的 `pthread_cond_t` 和 `pthread_mutex_t` 包含具名字段 `__u`，其类型是没有 tag 名称的 union。生成代码却把 Clang 的诊断显示名称当成可用于 C 声明的类型名称：

```c
typedef union pthread_cond_t::(unnamed at <sysroot>/include/bits/alltypes.h:120:18)
    (*bgcs_verified_field_0);
```

这不是合法 C。编译器报告 `expected identifier or '('`；后续 `_Generic` 因 typedef 未成立继续报错。精简复现共得到 5 个编译错误。

Windows 下自有头中的 tagged/untagged anonymous union 控制用例能够生成并编译。因此，目前证据支持“系统声明的匿名类型识别、可命名性或存储类型渲染存在缺口”，不能扩大为“全部匿名 union 都不支持”。

## 独立生成方式

在独立目录使用以下工程，显式关闭隐式导入：

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <LangVersion>14.0</LangVersion>
    <ImplicitUsings>disable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="BGCS" Version="[4.1.0]" />
    <PackageReference Include="BGCS.Extension.BrowserWasm" Version="[4.1.0]" />
  </ItemGroup>
</Project>
```

```csharp
using System;
using BGCS;
using BGCS.Extension.BrowserWasm;

await using BindingHost host = BindingGenerator.CreateBuilder()
    .AddPlugin<BrowserWasmPlugin>()
    .Build();
GenerationResult result = await host.GenerateAsync(GenerationRequest.FromFile(args[0]));
foreach (var diagnostic in result.diagnostics)
    Console.WriteLine($"{diagnostic.severity} {diagnostic.code}: {diagnostic.message}");
return result.success ? 0 : 1;
```

将上面的头保存为 `include/pthread-owner.h`，配置为：

```json
{
  "schemaVersion": 2,
  "library": { "id": "pthread-owner", "nativeName": "pthread_owner" },
  "source": {
    "language": "c",
    "languageStandard": "c17",
    "headers": ["include/pthread-owner.h"],
    "includeDirectories": ["include"]
  },
  "targets": [{
    "id": "emscripten-wasm32-emscripten",
    "settings": { "sysRoot": "<selected-workload-sysroot>" }
  }],
  "api": {
    "name": "NativeApi",
    "namespace": "PthreadOwner",
    "runtime": "dotnet.mono-wasm"
  },
  "output": {
    "root": "GeneratedWasm",
    "layout": "singleFile",
    "singleFileName": "Bindings.cs"
  }
}
```

运行 `dotnet run --project Runner -- <absolute-config-path>`，然后使用同一 workload 的 Clang 编译未修改的生成源：

```powershell
& $clang --target=wasm32-unknown-emscripten "--sysroot=$sysroot" -std=c17 `
    "-I$repro/include" "-I$native/include" `
    -c "$native/src/storage.assertions.c" -o "$repro/storage.wasm.o"
```

这里的 `$native` 是 `GeneratedWasm/emscripten-wasm32-emscripten/native`；SDK 路径必须来自实际选定的 workload。本次是 Emscripten 3.1.56 / runtime pack 10.0.13，由 .NET SDK 10.0.402 选择。

## 职责与修复要求

- BGCS Core/Frontend 必须区分稳定类型身份、声明名称与诊断显示名称；系统头和自有头适用同一语义。诊断路径不能成为生成类型名称。
- `BGCS.NativeBridge/Emission/NativeStorageAssertionEmitter.cs` 当前已有不可命名字段分支；应检查系统匿名声明为何没有进入正确分支，以及对应的类型引用和 alias 事实。该位置是排查入口，尚未证明它是唯一根因。
- 这是通用 C 类型及验证代码生成问题，不应在 Browser 扩展中硬编码 `pthread_*`、sysroot 路径或字段名来修复。
- Browser 扩展继续拥有 runtime/ABI lowering 与静态导出适配；引擎继续拥有 SDK 选择、编译、链接、部署和缓存。
- 修复必须保留可验证的布局、字段和 callable ABI 证据。目标语言不能表达某种严格证明时，应采用有明确语义和能力依据的证明策略，或在生成前提供结构化拒绝；不能生成非法 C、删除全部断言或伪称已经验证。
- 不修改第三方头、不修改生成结果、不在引擎中复制 BGCS 类型渲染实现，也不修改 BGCS 源码或包内容。

## SDK 修复验收

1. 以上精简复现的官方生成、原生 C17 编译和桥调用全部通过。
2. 同时覆盖系统头、自有头、typedef alias、嵌套匿名 struct/union、数组和具名 union；检查类型身份与来源无混淆。
3. 保留 tagged/untagged 控制用例，并有正反例证明损坏布局或 callable ABI 仍会被拒绝。
4. 生成源不包含 Clang 的 `(unnamed at ...)` 诊断名称作为类型声明；移动复现目录后仍可生成和编译。
5. 引擎用新正式包恢复 Browser 五组件生成及实际 native 编译，再执行 Mono Wasm / Wasm AOT FlappyBird 发布和运行。生成成功不能替代产品验收。

原始证据保存在 `artifacts/acceptance/bgcs4-2026-10-24/anonymous-union-repro/`：输入、Runner、配置、`generate-pthread-wasm.log`、未修改的生成物及 `compile-pthread-wasm.log`。产品失败日志为同级 `browser-support-isolated.log`。
