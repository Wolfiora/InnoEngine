## 2. 命名规范
- 文件名与主类型名保持一致。
- 默认命名空间与目录层级保持一致；`src/composition/editor` 使用第 13 节定义的项目级命名空间规则。
- 类型名使用 `PascalCase`。
- 接口以 `I` 前缀。
- 成员参数使用语义化 `camelCase`。
- 私有字段使用 `m_` 前缀。
- 常量：`C_` 前缀（如 `C_MAX_COUNT`）或语义清晰的 `readonly static` 命名。
- 代码中的 Unity 风格 API（现有的如 `transform`、`scene`、`active`、`name`）保持周边兼容。
