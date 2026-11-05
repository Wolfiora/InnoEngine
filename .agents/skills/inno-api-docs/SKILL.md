---
name: inno-api-docs
description: Maintain InnoEngine design, usage, API XML and examples against actual public and protected contracts.
---

通过规则加载器补选 wiki、xml，入口见 [文档规则](../../rulebook/documentation.md)。先用 Build 的 docs search/read/symbol 定位唯一项目入口与章节；符号标记和索引新鲜度不代替当前源码验证。

核对实际 public/protected 符号、真实项目引用和调用方，维护受影响的设计、实现、使用教程与调用约束。公开承诺和内部机制分开，所有权、线程、失败和释放不能省略。长页使用同名目录与 api-include；移动时修复入站链接与所属项目关系。方法见 [文档指南](../../../docs/development/documentation.md)。

统一 validate 计划选择 API 与示例检查。输出所属页面、行为变化、已验证内容和剩余限制，不复制源码目录、规则正文或第二份 API 清单；内部颜色变化不触发全引擎文档重写。
