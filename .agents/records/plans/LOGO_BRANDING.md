# Logo、图标与署名接入

## 获批范围

在 `logo` 分支整理品牌 SVG、512px 透明 PNG、单色源稿和独立图标 TTF；原样迁移 README 截图；署名 xdzzyq；合并四种默认字体并提供无 Session 的原图读取入口。保持单色原轮廓与已有镂空，不新增绘制控件或生成系统。

## Owner 与决策

- 品牌原稿唯一位于 `assets/branding`，截图位于 `assets/screenshots`。
- 字体归 ImGui SDL3 backend 的既有 BuiltIn/Icons；Editor 通过现有常量与目录使用。新增字体排序在两个 Font Awesome 来源之后、CJK 来源之前。
- 原图读取归默认组合 `Inno.Engine.Default.EngineBranding`，通过项目资源链接嵌入同一 PNG；每次返回独立的调用方所有只读流。
- 署名位于 README、品牌说明与 TTF designer metadata；不代替开发指南，不推断额外授权。

## 有限验收

统一 validate 计划选择架构、文档、编译、字体与资源契约检查；补充深浅背景及 14/16/24/32px 字形视觉检查。保留中文、既有图标、字体源共享和生命周期断言。记录执行身份、首次失败与环境限制，不将缺失检查记为通过。

## 当前进度

源码、资源、署名与文档已完成，PNG 读取、原生字体、受影响 API 与示例专项均通过。全量门禁被既有测试支持库的执行层分类阻塞，原始结果与未完成范围见 [验收记录](../acceptance/LOGO_BRANDING_2026_11_07.md)。

使用显式 `C:/Users/23842/.dotnet/dotnet.exe` 的 SDK 10.0.402，未修改全局 SDK 配置。制作阶段 FontTools 依赖位于当前工作区 `artifacts/branding-work`，不进入产品或日常构建。
