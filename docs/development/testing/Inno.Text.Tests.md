# Inno.Text.Tests

本套件显式声明 `player` Native 产品，在同一目标／配置的正式构建中准备并部署。测试仅消费该输出中的部署索引，不从相邻 Editor 输出或仓库缓存寻找 DLL；缺少部署是验收失败。

[测试与 fixture 索引](README.md)

[Text 索引](../../modules/text/README.md)

验证无效字体源被拒绝、合法字体导入产生 runtime/font-data artifact、服务经 FreeType/HarfBuzz 完成 shaping 与 rasterization。通过[统一 Native 验证入口](README.md#native-validation)准备当前目标／配置的 player 产品及 Text 部署闭包；现存动态库不能代替有效部署索引。
