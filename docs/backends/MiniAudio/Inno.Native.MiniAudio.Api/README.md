# Inno.Native.MiniAudio ABI 参考

[权威项目页](../Inno.Native.MiniAudio.md)

以下参考保留当前宿主 ABI 的全部语义签名和源码位置；它们不是服务层或脚本 API。原生调用的 owner、线程和关闭顺序从项目页进入。

- [DataCallback → MaBpfNodePtr](ABI-01.md)
- [MaBpfPtr → MaDataSourceNodeConfig](ABI-02.md)
- [MaDataSourceNodeConfigPtr → CaptureAnonymous](ABI-03.md)
- [CaptureAnonymous → CaptureAnonymous](ABI-04.md)
- [CaptureAnonymous → PlaybackAnonymous](ABI-05.md)
- [PlaybackAnonymous → MaDeviceDataProc](ABI-06.md)
- [MaDeviceDescriptor → MaDeviceId](ABI-07.md)
- [MaDeviceId → MaDeviceId](ABI-08.md)
- [MaDeviceId → MaDeviceId](ABI-09.md)
- [CustomAnonymous → NativeDataFormat](ABI-10.md)
- [MaDeviceInfoPtr → MaFaderConfig](ABI-11.md)
- [MaFaderConfigPtr → MaJobProc](ABI-12.md)
- [MaJobPtr → MaNodeGraphConfig](ABI-13.md)
- [MaNodeGraphConfigPtr → MaPeakNodeConfig](ABI-14.md)
- [MaPeakNodeConfigPtr → MaResourceManagerDataSourcePtr](ABI-15.md)
- [MaResourceManagerDataStream → MaSpatializerListenerConfigPtr](ABI-16.md)
- [MaSpatializerListenerPtr → MiniAudio](ABI-17.md)
- [MiniAudio → MiniAudio](ABI-18.md)
- [MiniAudio → MiniAudio](ABI-19.md)
- [MiniAudio → MiniAudio](ABI-20.md)
- [MiniAudio → MiniAudio](ABI-21.md)
- [MiniAudio → StopCallback](ABI-22.md)
