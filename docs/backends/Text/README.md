# Text 共享 backend

[Backend 索引](../README.md) · [Wiki 首页](../../README.md)

源码、Native 与组件构建仅在 `backends/Text` 维护。平台选择已有能力；复用必须验证目标 ABI、SDK、surface、线程、回调及生命周期。平台专属系统接入留在 platforms。

## 项目

- [Inno.Adapter.Text.FreeTypeHarfBuzz](Inno.Adapter.Text.FreeTypeHarfBuzz.md)
- [Inno.Build.Toolchains.Text](Inno.Build.Toolchains.Text.md)
- [Inno.Native.Text](Inno.Native.Text.md)

## 上游与使用边界

[FreeType](https://github.com/freetype/freetype)、[HarfBuzz](https://github.com/harfbuzz/harfbuzz) 是本 backend 消费的上游源码入口，所选 revision 以仓库 `.gitmodules` 和 gitlink 为准。上游 API 的存在不表示可以绕过 Inno 的 adapter、目标部署或资源 owner；句柄、线程、回调与退休义务由上列 adapter/native 项目页说明。引擎封装没有承诺的上游能力不自动成为产品 API。
