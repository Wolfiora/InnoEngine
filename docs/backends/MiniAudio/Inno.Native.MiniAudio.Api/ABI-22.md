# Inno.Native.MiniAudio ABI · MiniAudio → StopCallback

[项目与生命周期](../Inno.Native.MiniAudio.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.MiniAudio.MiniAudio</code> · 1621–1712</summary>

<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerListenerSetSpeedOfSound(Inno.Native.MiniAudio.MaSpatializerListener@,System.Single) e0759b4d639ba2cc6d20cf551225b06e2a004bc9dc8aef8dad82a3f5ac903188 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerListenerSetSpeedOfSound(Inno.Native.MiniAudio.MaSpatializerListenerPtr,System.Single) 12e79fdc5413657555c0507b9864ec42a3920a8f8ca1f6590c657862b877259b -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerListenerSetVelocity(Inno.Native.MiniAudio.MaSpatializerListener@,System.Single,System.Single,System.Single) d22a3671f70f6f8f8c62ead371ae39223f6f9effc5b8026b772fbc5b6e2f131f -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerListenerSetVelocity(Inno.Native.MiniAudio.MaSpatializerListenerPtr,System.Single,System.Single,System.Single) fc40b529d6f21afd9a5170f8207f66208b457351eb03bb9248d1d9af6df2b588 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerListenerSetWorldUp(Inno.Native.MiniAudio.MaSpatializerListener@,System.Single,System.Single,System.Single) 00d37e019b28eba6c0d0196b97097319649ea9282dcf25af348828e23f76500a -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerListenerSetWorldUp(Inno.Native.MiniAudio.MaSpatializerListenerPtr,System.Single,System.Single,System.Single) d854b434e732b72fb01b663a84789a7f5a4591377d0d93f08fe18ee56183a7f0 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerListenerUninit(Inno.Native.MiniAudio.MaSpatializerListener@,Inno.Native.MiniAudio.MaAllocationCallbacks@) 2e306211043d508c058234825523f4d56615968547504f99a9072498a3da81bb -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerListenerUninit(Inno.Native.MiniAudio.MaSpatializerListenerPtr,Inno.Native.MiniAudio.MaAllocationCallbacksPtr) b170553282624162dd8fdd99a4fb5e518c48fd03a6e3a0a9734a32a3e34ec558 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerProcessPcmFrames(Inno.Native.MiniAudio.MaSpatializer@,Inno.Native.MiniAudio.MaSpatializerListener@,System.Void*,System.Void*,System.UInt64) e471e23a415e25a5ee6690d4ad8ebc2153c482a0061639f551d09e1a9a33dfe0 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerProcessPcmFrames(Inno.Native.MiniAudio.MaSpatializerPtr,Inno.Native.MiniAudio.MaSpatializerListenerPtr,System.Void*,System.Void*,System.UInt64) e508cd06b204371c8a934f3cdf955fb8115e88160d8ccc0587e6bae1f6d5aa7f -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetAttenuationModel(Inno.Native.MiniAudio.MaSpatializer@,Inno.Native.MiniAudio.MaAttenuationModel) 6b94d296814dcbba4a13aab3cefdca7b02d3e8cd78565fd4624a6efb96962702 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetAttenuationModel(Inno.Native.MiniAudio.MaSpatializerPtr,Inno.Native.MiniAudio.MaAttenuationModel) 7306effefb90e1e664bd9f87734d5cf4ea8338a6a8bec4d0e00a4666e4d88e5c -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetCone(Inno.Native.MiniAudio.MaSpatializer@,System.Single,System.Single,System.Single) 0baa8ca034886cdd62ab65a96e6685551a1e1a6690ec0df787dc3a7d1d1490a2 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetCone(Inno.Native.MiniAudio.MaSpatializerPtr,System.Single,System.Single,System.Single) 7c0cd40dcc93568ad7f2cf9a97519e5dc6ff8ea8d8edde5b03473b7cd0b64150 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetDirection(Inno.Native.MiniAudio.MaSpatializer@,System.Single,System.Single,System.Single) adfd635b81546f05371dad9b11af69e2efc6cf6b087e08cc17e8a7e98d86bfdc -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetDirection(Inno.Native.MiniAudio.MaSpatializerPtr,System.Single,System.Single,System.Single) 168f7ee6833c59b29907e2e39c2ace4ec367e77a87aaeb6b3bea9b1c395aa42d -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetDirectionalAttenuationFactor(Inno.Native.MiniAudio.MaSpatializer@,System.Single) 61b5acacaed8e09c1deb75edc49cd6979bca43137214218c8dd1f113c228b352 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetDirectionalAttenuationFactor(Inno.Native.MiniAudio.MaSpatializerPtr,System.Single) 3e5c33f322c1773f3425266da7f36d7c96603e77270e975986f872feb8a23039 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetDopplerFactor(Inno.Native.MiniAudio.MaSpatializer@,System.Single) fe3dec938f597cfea4f1756c5d8064a75e5b6c704d3fe5b01db5e0a85e27b081 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetDopplerFactor(Inno.Native.MiniAudio.MaSpatializerPtr,System.Single) 577492b441017bd9685a7739c3380d2ae32dc0108585673f9ba8adf185a8c277 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetMasterVolume(Inno.Native.MiniAudio.MaSpatializer@,System.Single) e2868de1ca5e151f9c66362138048d34abb071b0bb88040b2b932373e13553c0 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetMasterVolume(Inno.Native.MiniAudio.MaSpatializerPtr,System.Single) 77eda6b83cc2b95d9e02f0ce541e68a9c694684982c07efc104956b26bbb3b56 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetMaxDistance(Inno.Native.MiniAudio.MaSpatializer@,System.Single) 85e4173a225a45019b035b9e5117fd35ace4535907f962c9d31da53a41a6a498 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetMaxDistance(Inno.Native.MiniAudio.MaSpatializerPtr,System.Single) 6b389a475ce40666c54b8f4edd91d2ebe0e6937a6bcf62805be8591450e1a90a -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetMaxGain(Inno.Native.MiniAudio.MaSpatializer@,System.Single) 8684bd996e8e45894093b05e2122c1fc5678c7683942f2ae1ec7075f000249a9 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetMaxGain(Inno.Native.MiniAudio.MaSpatializerPtr,System.Single) 015a40eee1ab3d326f162a82accbcb68fd5db3dc320a778a2489e4e7ede649d0 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetMinDistance(Inno.Native.MiniAudio.MaSpatializer@,System.Single) 89d72f10b616088c44740b7a98161a455219ee14fc59e8169289677ee6fbb89c -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetMinDistance(Inno.Native.MiniAudio.MaSpatializerPtr,System.Single) 91eebf0f11258e096f3a4de655662f29bec6e7c0e9ef2ab117f30055f5e72cb6 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetMinGain(Inno.Native.MiniAudio.MaSpatializer@,System.Single) 2a9c0aa6dd9e7694f501064536fe484dc2b3e9bc8d499a591d189f1c41e97660 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetMinGain(Inno.Native.MiniAudio.MaSpatializerPtr,System.Single) 711f8696d41bdc1e17a4655ef7b3329ad01da41f2c9f9882da864c9857dd2f0a -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetPosition(Inno.Native.MiniAudio.MaSpatializer@,System.Single,System.Single,System.Single) 5f6c9562246121f6219687ad008acd639b16a90405312ce0187060acfaa13b69 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetPosition(Inno.Native.MiniAudio.MaSpatializerPtr,System.Single,System.Single,System.Single) d23deaac319e6d426630d54d00dc6bb09d5a7b79161e4e58e66a436a0c77181b -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetPositioning(Inno.Native.MiniAudio.MaSpatializer@,Inno.Native.MiniAudio.MaPositioning) b474d63dc3a0d01f950d363a977927dca59784f4d6cf569e3a5c5b0535c3feee -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetPositioning(Inno.Native.MiniAudio.MaSpatializerPtr,Inno.Native.MiniAudio.MaPositioning) aa40c0f8790758a56f6607acca56dcdc83bb7e52122fd6cd476dfb5b8fe76f5a -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetRolloff(Inno.Native.MiniAudio.MaSpatializer@,System.Single) f1c856c44faf1cd174301b1875a84b20eee1704f59f53951be92725148f3d200 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetRolloff(Inno.Native.MiniAudio.MaSpatializerPtr,System.Single) 20b8c5ab228cc712a724851ceab0193ab9607f627b6751fa6956cc76d7c182bc -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetVelocity(Inno.Native.MiniAudio.MaSpatializer@,System.Single,System.Single,System.Single) 4bb6f74d09ff6d3159072c7818651332aaea132fb47b2f431ae677cf7e89b44d -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerSetVelocity(Inno.Native.MiniAudio.MaSpatializerPtr,System.Single,System.Single,System.Single) e6ddd5b2c3087617b6e4b146877b17dda99cd0b5da5a8e294126330254e4cf2b -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerUninit(Inno.Native.MiniAudio.MaSpatializer@,Inno.Native.MiniAudio.MaAllocationCallbacks@) 56d6536a679aa6a74179f135acfe3dde1fe78c83eddeefd9db364d417fa2472f -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpatializerUninit(Inno.Native.MiniAudio.MaSpatializerPtr,Inno.Native.MiniAudio.MaAllocationCallbacksPtr) 6962030256c153281609414f75a751a2b8993d9bc2cbe285ad54f90e6832e581 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpinlockLock(System.UInt32@) 6eed26d5fe4124a31c09926eff7c02df98fa1c87ba76cdf27a8ba5fe52a9ba85 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpinlockLock(System.UInt32*) 32321c7c4cfc9c61a0e17e96396c4625496fce6fa1f6046c4be2bb3a4d38054f -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpinlockLockNoyield(System.UInt32@) d7bbcbd1e3f22671724adc020f595030c3596e88e9ee00c8cbf93b7595d0226b -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpinlockLockNoyield(System.UInt32*) 9aaacd7f98583543bafd6d0c337de90c647fafceec6ef21da34a3ae339cc8ab8 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpinlockUnlock(System.UInt32@) 93b18d2d890fbacac4e661ac1287740bd61b360b2f08d63781faa45d96bd4801 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SpinlockUnlock(System.UInt32*) e6e85083c4fbd672a487517db2a8b6d5fd6a7e5eac867fd829430ebb288fae2f -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SplitterNodeConfigInit(System.UInt32) 1ca97c54b7e05eba8f512be00ef4aa4426de5da2c1397e441d9c87520c218082 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SplitterNodeInit(Inno.Native.MiniAudio.MaNodeGraph@,Inno.Native.MiniAudio.MaSplitterNodeConfig@,Inno.Native.MiniAudio.MaAllocationCallbacks@,Inno.Native.MiniAudio.MaSplitterNode@) b9645d6919b7c28c50f29ad71d8fe12970594ed17ca7e6bb76686722db738f2a -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SplitterNodeInit(Inno.Native.MiniAudio.MaNodeGraphPtr,Inno.Native.MiniAudio.MaSplitterNodeConfigPtr,Inno.Native.MiniAudio.MaAllocationCallbacksPtr,Inno.Native.MiniAudio.MaSplitterNodePtr) cdd180240b0f9d73b4943f714efb4273b6c90aae3c79293a23c094bdb6dd7a0e -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SplitterNodeUninit(Inno.Native.MiniAudio.MaSplitterNode@,Inno.Native.MiniAudio.MaAllocationCallbacks@) 31e98e8d861b151ec655d0d7c5dc2e24391deb3d0d0429a8017e743e40c8b722 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.SplitterNodeUninit(Inno.Native.MiniAudio.MaSplitterNodePtr,Inno.Native.MiniAudio.MaAllocationCallbacksPtr) f25637297950143b3455535279550897977ecec924676e8ca6a7f8ad7575a356 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.Version(System.UInt32@,System.UInt32@,System.UInt32@) 1c9a8af69fea237e68255f3b8dc1da226734fc492f698c88052d8afe9b0e743e -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.Version(System.UInt32*,System.UInt32*,System.UInt32*) 40a52c542efe833bf128e219e919f4083baaf08c0cf94acebba23b8c6ba3d693 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VersionString d6211de2e847a8de03fa5d47f47c3ae526931ed36f7b2d4e3bff22a051fb41da -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsClose(Inno.Native.MiniAudio.MaVfs@,System.IntPtr) e85bfc4493c561828d20c60356862e9bd590dfb263e8639e9e034f2dbf06e28c -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsClose(Inno.Native.MiniAudio.MaVfs*,System.IntPtr) fa95784ee835716c0decd27539bff23a3ad8508aa9fa38b2760b9d828b47042c -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsInfo(Inno.Native.MiniAudio.MaVfs@,System.IntPtr,Inno.Native.MiniAudio.MaFileInfoPtr) 3399c707494ebc4b1c0de5c71f42f450bf021adab25ad51909be2b919f8b6eaa -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsInfo(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,Inno.Native.MiniAudio.MaFileInfo@) aaf8db90363020f6e1caff5b5f2069d9ad72af018cc3afadb27ae0a6e2913094 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsInfo(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,Inno.Native.MiniAudio.MaFileInfoPtr) 0ededebfd760e12a6147645707a4d9d2562e1b088962039e6895430e3214293f -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsOpen(Inno.Native.MiniAudio.MaVfs@,System.String,System.UInt32,System.IntPtr@) 167e40580d19cc5e6d9e932ccb2e007f7e53ccf01e91c2869be4d653835de672 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsOpen(Inno.Native.MiniAudio.MaVfs*,System.Byte*,System.UInt32,System.IntPtr*) 9e295652568f700f7f8b0c59f6ba417345ec0810fdaeb9cc381bbe4e5ae30656 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsOpenAndReadFile(Inno.Native.MiniAudio.MaVfs*,System.Byte*,System.Void**,System.UIntPtr*,Inno.Native.MiniAudio.MaAllocationCallbacks*) 4c860ec39e756916e59cd7fb15aded03bc345376ab0034db01f9bbb98ff9b0a2 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsOpenW(Inno.Native.MiniAudio.MaVfs@,System.UInt16*,System.UInt32,System.IntPtr@) c74aaa401430406f33af2cc8e81d5e8dd94debcbaa73031f38921a173deb4325 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsOpenW(Inno.Native.MiniAudio.MaVfs*,System.UInt16*,System.UInt32,System.IntPtr*) e61eb053821f74bcc5c0baa75c613824f4014feb7268ee3131d3cf8f4ae39c5d -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsRead(Inno.Native.MiniAudio.MaVfs@,System.IntPtr,System.Void*,System.Span{System.UIntPtr}) 9ba5a6461f5d4f1712edb458b9076ad6ddad4f87e1427354945d5e719e1bc101 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsRead(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,System.Void*,System.UIntPtr,System.UIntPtr*) 1cbb9b03786364f1b73b35a1a10558e9db95788f9bf57a1300055dc62dde1caa -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsSeek(Inno.Native.MiniAudio.MaVfs@,System.IntPtr,System.Int64,Inno.Native.MiniAudio.MaSeekOrigin) f118ac83a34293a40e51a4264a225653b181dc9e1958f521bd3d0dc13d824e69 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsSeek(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,System.Int64,Inno.Native.MiniAudio.MaSeekOrigin) d6f4024c290912f0a405318c0ff7997d6b60b0979e95b6eaa9a72b511605fd17 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsTell(Inno.Native.MiniAudio.MaVfs@,System.IntPtr,System.Int64@) 4c369102484ee523afa0a8b058c96a59620068d7464a4cb41e083a54faa150ce -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsTell(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,System.Int64*) 8c0d81244f35ec60dba33fa6fbb7a3a9bffe99719ad39a8529c517fa194f25b7 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsWrite(Inno.Native.MiniAudio.MaVfs@,System.IntPtr,System.Void*,System.Span{System.UIntPtr}) a29f43d5bcb647a05e52ee574eee3b8175ebf6111f5627259df4af55a3e3cfab -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VfsWrite(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,System.Void*,System.UIntPtr,System.UIntPtr*) 5ac2a178d8c06e183bf386fae4a7319a549dce06615aa5a55bd3eb2cd6126b78 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VolumeDbToLinear(System.Single) f5519da4955623016920264e317a07a1004d4d2abe2a92bcc92f59d4d47ac340 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.VolumeLinearToDb(System.Single) d8d29d169314241ae86a83e19f99ed14b41853f0e49b0ae7574fd87e3628e953 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformConfigInit(Inno.Native.MiniAudio.MaFormat,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaWaveformType,System.Double,System.Double) d588243c33ada4b3310ded2064fae07237deff314b7ae80d87fe963708a0b1e8 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformInit(Inno.Native.MiniAudio.MaWaveformConfig@,Inno.Native.MiniAudio.MaWaveform@) ca99b27b61067991deaddbebe9e0528baff60ff83b54ea8fef45549567169212 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformInit(Inno.Native.MiniAudio.MaWaveformConfigPtr,Inno.Native.MiniAudio.MaWaveformPtr) 88dd5e9d4227726ce33eac07d2775683c0f5a61963a103c7c10aa63aebb4b120 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformReadPcmFrames(Inno.Native.MiniAudio.MaWaveform@,System.Void*,System.UInt64,System.UInt64*) e447ac10c2576ce88a969d67666251022d592bf8356abc8750e23eceae51c4dd -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformReadPcmFrames(Inno.Native.MiniAudio.MaWaveformPtr,System.Void*,System.Span{System.UInt64}) c595ee90e0ffa294ea1696416db6ef2619126f5f38d08ec1b22ed0abc7edc8fa -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformReadPcmFrames(Inno.Native.MiniAudio.MaWaveformPtr,System.Void*,System.UInt64,System.UInt64*) 2a7a6b5a4a4c1963ee8eec4cce815b9ffd1f760dbbea5cab393fe80fc5003e3a -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformSeekToPcmFrame(Inno.Native.MiniAudio.MaWaveform@,System.UInt64) ccb80c1b2e326fab87895ec889d2acf0db26aff0cc6e94029b3fc35faec06d27 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformSeekToPcmFrame(Inno.Native.MiniAudio.MaWaveformPtr,System.UInt64) 0bc1311c1b2d9dcc348addb79649e2925779a5e500757a75e3673b1da4748a8b -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformSetAmplitude(Inno.Native.MiniAudio.MaWaveform@,System.Double) d052388396afda989d6393eea3dc3be120df12954e7ed6f7e6984367d90337a7 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformSetAmplitude(Inno.Native.MiniAudio.MaWaveformPtr,System.Double) 926a93d5e3085d7aa062aa9974fd77f1eb7dc21262dbb2ef2fb9a258db8d3270 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformSetFrequency(Inno.Native.MiniAudio.MaWaveform@,System.Double) 4b27b19ba56e3c58ead01d4353028782898e4359e42929c0f8042d1603e53876 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformSetFrequency(Inno.Native.MiniAudio.MaWaveformPtr,System.Double) 4f03a42b332b5dc9cf77065a28fb92560cef0cdcb412b2346e91aab29e0ff037 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformSetSampleRate(Inno.Native.MiniAudio.MaWaveform@,System.UInt32) 00a722f2affbae8ecd6ba5ffd06b338139ea4da026aa9221583c8b1f1fa18dbd -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformSetSampleRate(Inno.Native.MiniAudio.MaWaveformPtr,System.UInt32) 2d171d0b45935143ac2d284f34263c4eede6050776d757202a78ea67ce9b9a61 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformSetType(Inno.Native.MiniAudio.MaWaveform@,Inno.Native.MiniAudio.MaWaveformType) 21c9ee6cd50a535d66a2792d51fe5cf5124ae9aa06255d9ce662957646c7104d -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformSetType(Inno.Native.MiniAudio.MaWaveformPtr,Inno.Native.MiniAudio.MaWaveformType) 21ab97ed838f5410bc7b91238d8289e328639d9c9f3dd56bca1fd28ce64e3093 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformUninit(Inno.Native.MiniAudio.MaWaveform@) b16738a62eb9bae885b29a43406f6cfaba574acb27c8517350dbde579e508387 -->
<!-- api: M:Inno.Native.MiniAudio.MiniAudio.WaveformUninit(Inno.Native.MiniAudio.MaWaveformPtr) 76557637360c06c4348e1448b080c66135ef05c2e519430e9dcb55c70b3d07a0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d499d851f224cae1"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerListenerSetSpeedOfSound(ref Inno.Native.MiniAudio.MaSpatializerListener pListener, float speedOfSound)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31668) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-92f04b54647f1efb"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerListenerSetSpeedOfSound(Inno.Native.MiniAudio.MaSpatializerListenerPtr pListener, float speedOfSound)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31662) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4e9eb81a980e56d6"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerListenerSetVelocity(ref Inno.Native.MiniAudio.MaSpatializerListener pListener, float x, float y, float z)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31636) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-26302c25287210cb"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerListenerSetVelocity(Inno.Native.MiniAudio.MaSpatializerListenerPtr pListener, float x, float y, float z)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31630) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d523d35d0640b4d8"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerListenerSetWorldUp(ref Inno.Native.MiniAudio.MaSpatializerListener pListener, float x, float y, float z)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31700) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-14e5502a4a881a3f"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerListenerSetWorldUp(Inno.Native.MiniAudio.MaSpatializerListenerPtr pListener, float x, float y, float z)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31694) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-68c6feebd8f69868"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerListenerUninit(ref Inno.Native.MiniAudio.MaSpatializerListener pListener, ref Inno.Native.MiniAudio.MaAllocationCallbacks pAllocationCallbacks)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31502) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bdbd40be716be448"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerListenerUninit(Inno.Native.MiniAudio.MaSpatializerListenerPtr pListener, Inno.Native.MiniAudio.MaAllocationCallbacksPtr pAllocationCallbacks)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31496) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-69c85dbd1e751f74"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.SpatializerProcessPcmFrames(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, ref Inno.Native.MiniAudio.MaSpatializerListener pListener, void* pFramesOut, void* pFramesIn, ulong frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31859) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d3dbf69f9ab068ca"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.SpatializerProcessPcmFrames(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, Inno.Native.MiniAudio.MaSpatializerListenerPtr pListener, void* pFramesOut, void* pFramesIn, ulong frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31852) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1eff722905b03f3c"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetAttenuationModel(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, Inno.Native.MiniAudio.MaAttenuationModel attenuationModel)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31955) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe4b3aca67e8aaff"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetAttenuationModel(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, Inno.Native.MiniAudio.MaAttenuationModel attenuationModel)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31949) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-77e3576d2405e512"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetCone(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, float innerAngleInRadians, float outerAngleInRadians, float outerGain)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32179) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6cd18a4ffe7a61d7"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetCone(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, float innerAngleInRadians, float outerAngleInRadians, float outerGain)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32173) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3c5503b833c06aeb"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetDirection(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, float x, float y, float z)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32320) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0425941cc1bac6fd"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetDirection(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, float x, float y, float z)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32314) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a3c06ff05a27630f"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetDirectionalAttenuationFactor(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, float directionalAttenuationFactor)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32256) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b0e4e5dcaf0b73bb"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetDirectionalAttenuationFactor(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, float directionalAttenuationFactor)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32250) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-052036df96236a3e"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetDopplerFactor(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, float dopplerFactor)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32224) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bf75feebae6d0ff6"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetDopplerFactor(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, float dopplerFactor)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32218) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9f1554a3fa3e3948"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.SpatializerSetMasterVolume(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, float volume)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31879) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a6531fa9c84a9905"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.SpatializerSetMasterVolume(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, float volume)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31872) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d048c4c04310366a"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetMaxDistance(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, float maxDistance)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32147) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-40ad06868168a4be"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetMaxDistance(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, float maxDistance)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32141) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d01569610bbe4821"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetMaxGain(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, float maxGain)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32083) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5b3037e7dee58cf5"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetMaxGain(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, float maxGain)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32077) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d8170d2118226571"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetMinDistance(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, float minDistance)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32115) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cdaac021df5e27a6"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetMinDistance(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, float minDistance)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32109) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-71b9061cd1f35110"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetMinGain(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, float minGain)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32051) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-90a839bc7a5524e2"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetMinGain(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, float minGain)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32045) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-082bdb8f15fdd89d"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetPosition(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, float x, float y, float z)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32288) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-51e81ba08b49ac0c"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetPosition(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, float x, float y, float z)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32282) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e0982ea87ed5b12"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetPositioning(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, Inno.Native.MiniAudio.MaPositioning positioning)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31987) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-045f30e7c037d401"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetPositioning(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, Inno.Native.MiniAudio.MaPositioning positioning)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31981) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3bd97d791a6036f4"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetRolloff(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, float rolloff)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32019) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-37860926a95d0710"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetRolloff(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, float rolloff)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32013) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6bd696c0cf657e62"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetVelocity(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, float x, float y, float z)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32352) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4328ba54a1d6e678"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerSetVelocity(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, float x, float y, float z)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L32346) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9f2a8cfa5ea4636f"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerUninit(ref Inno.Native.MiniAudio.MaSpatializer pSpatializer, ref Inno.Native.MiniAudio.MaAllocationCallbacks pAllocationCallbacks)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31840) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3fd25be6544124c3"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SpatializerUninit(Inno.Native.MiniAudio.MaSpatializerPtr pSpatializer, Inno.Native.MiniAudio.MaAllocationCallbacksPtr pAllocationCallbacks)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L31834) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f9bb74dc936bb321"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.SpinlockLock(ref uint pSpinlock)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L34947) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ffa39be53217329f"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.SpinlockLock(uint* pSpinlock)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L34940) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7ea2b3da4cd9c2c0"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.SpinlockLockNoyield(ref uint pSpinlock)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L34963) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7bdf1b3e3e458b18"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.SpinlockLockNoyield(uint* pSpinlock)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L34956) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-318b62e8b4cb4d08"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.SpinlockUnlock(ref uint pSpinlock)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L34979) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6424f1d05200aaa8"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.SpinlockUnlock(uint* pSpinlock)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L34972) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6e6ee620d86c194d"></a>[`public static Inno.Native.MiniAudio.MaSplitterNodeConfig Inno.Native.MiniAudio.MiniAudio.SplitterNodeConfigInit(uint channels)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L39322) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-36cbb2afc53575b3"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.SplitterNodeInit(ref Inno.Native.MiniAudio.MaNodeGraph pNodeGraph, ref Inno.Native.MiniAudio.MaSplitterNodeConfig pConfig, ref Inno.Native.MiniAudio.MaAllocationCallbacks pAllocationCallbacks, ref Inno.Native.MiniAudio.MaSplitterNode pSplitterNode)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L39336) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d8d13b8fc950808c"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.SplitterNodeInit(Inno.Native.MiniAudio.MaNodeGraphPtr pNodeGraph, Inno.Native.MiniAudio.MaSplitterNodeConfigPtr pConfig, Inno.Native.MiniAudio.MaAllocationCallbacksPtr pAllocationCallbacks, Inno.Native.MiniAudio.MaSplitterNodePtr pSplitterNode)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L39329) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ba5a327d44912c91"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SplitterNodeUninit(ref Inno.Native.MiniAudio.MaSplitterNode pSplitterNode, ref Inno.Native.MiniAudio.MaAllocationCallbacks pAllocationCallbacks)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L39361) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-56961becd6cb3009"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.SplitterNodeUninit(Inno.Native.MiniAudio.MaSplitterNodePtr pSplitterNode, Inno.Native.MiniAudio.MaAllocationCallbacksPtr pAllocationCallbacks)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L39355) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-350ebc7c8ab7d828"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.Version(ref uint pMajor, ref uint pMinor, ref uint pRevision)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L28703) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ad56c8aaaa3a13e7"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.Version(uint* pMajor, uint* pMinor, uint* pRevision)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L28697) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-486361b7499f6be8"></a>[`public static string? Inno.Native.MiniAudio.MiniAudio.VersionString()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L28718) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-198a48cab08e43ed"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsClose(ref Inno.Native.MiniAudio.MaVfs pVFS, nint file)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37028) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5e07d085b6dbd4ce"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsClose(Inno.Native.MiniAudio.MaVfs* pVFS, nint file)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37021) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8ac818e92ca8f9ea"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsInfo(ref Inno.Native.MiniAudio.MaVfs pVFS, nint file, Inno.Native.MiniAudio.MaFileInfoPtr pInfo)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37127) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-19f88d1a09b19825"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsInfo(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, ref Inno.Native.MiniAudio.MaFileInfo pInfo)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37117) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5c668425b3e80f67"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsInfo(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, Inno.Native.MiniAudio.MaFileInfoPtr pInfo)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37110) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-52a7a2b93528dc95"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsOpen(ref Inno.Native.MiniAudio.MaVfs pVFS, string pFilePath, uint openMode, ref nint pFile)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L36980) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7457b841f45dc378"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsOpen(Inno.Native.MiniAudio.MaVfs* pVFS, byte* pFilePath, uint openMode, nint* pFile)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L36973) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5d26d039548dbc89"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsOpenAndReadFile(Inno.Native.MiniAudio.MaVfs* pVFS, byte* pFilePath, void** ppData, nuint* pSize, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37136) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a2ee6cd5db321c7b"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsOpenW(ref Inno.Native.MiniAudio.MaVfs pVFS, ushort* pFilePath, uint openMode, ref nint pFile)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37009) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cf3021821cf626b8"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsOpenW(Inno.Native.MiniAudio.MaVfs* pVFS, ushort* pFilePath, uint openMode, nint* pFile)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37002) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-11cef9374d0a55ff"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsRead(ref Inno.Native.MiniAudio.MaVfs pVFS, nint file, void* pDst, System.Span<nuint> pBytesRead)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37044) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-13de3689dd56cbf0"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsRead(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, void* pDst, nuint sizeInBytes, nuint* pBytesRead)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37037) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f2eca07bc686ac52"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsSeek(ref Inno.Native.MiniAudio.MaVfs pVFS, nint file, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37082) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f29f9eef62870653"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsSeek(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37075) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f8bb1371ffd692dd"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsTell(ref Inno.Native.MiniAudio.MaVfs pVFS, nint file, ref long pCursor)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37098) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c3c0086837c33e9b"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsTell(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, long* pCursor)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37091) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e31c583824de7386"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsWrite(ref Inno.Native.MiniAudio.MaVfs pVFS, nint file, void* pSrc, System.Span<nuint> pBytesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37063) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-184009c6af83b3ff"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.VfsWrite(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, void* pSrc, nuint sizeInBytes, nuint* pBytesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37056) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-19ec336b44766457"></a>[`public static float Inno.Native.MiniAudio.MiniAudio.VolumeDbToLinear(float gain)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L36950) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9eb466dd08e90812"></a>[`public static float Inno.Native.MiniAudio.MiniAudio.VolumeLinearToDb(float factor)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L36943) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4606655295c32504"></a>[`public static Inno.Native.MiniAudio.MaWaveformConfig Inno.Native.MiniAudio.MiniAudio.WaveformConfigInit(Inno.Native.MiniAudio.MaFormat format, uint channels, uint sampleRate, Inno.Native.MiniAudio.MaWaveformType type, double amplitude, double frequency)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37788) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8f4cf69ba996e068"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformInit(ref Inno.Native.MiniAudio.MaWaveformConfig pConfig, ref Inno.Native.MiniAudio.MaWaveform pWaveform)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37802) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b3c3745cf8eff6bf"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformInit(Inno.Native.MiniAudio.MaWaveformConfigPtr pConfig, Inno.Native.MiniAudio.MaWaveformPtr pWaveform)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37795) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b9a5e5763b7e9a21"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformReadPcmFrames(ref Inno.Native.MiniAudio.MaWaveform pWaveform, void* pFramesOut, ulong frameCount, ulong* pFramesRead)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37837) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-323578231bddd196"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformReadPcmFrames(Inno.Native.MiniAudio.MaWaveformPtr pWaveform, void* pFramesOut, System.Span<ulong> pFramesRead)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37847) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-24f15685b06b967a"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformReadPcmFrames(Inno.Native.MiniAudio.MaWaveformPtr pWaveform, void* pFramesOut, ulong frameCount, ulong* pFramesRead)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37830) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f32c44ffcba5f928"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformSeekToPcmFrame(ref Inno.Native.MiniAudio.MaWaveform pWaveform, ulong frameIndex)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37863) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e1d82c9af47156c1"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformSeekToPcmFrame(Inno.Native.MiniAudio.MaWaveformPtr pWaveform, ulong frameIndex)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37856) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0965cbf68f2e5165"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformSetAmplitude(ref Inno.Native.MiniAudio.MaWaveform pWaveform, double amplitude)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37880) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-90f5a9d625350375"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformSetAmplitude(Inno.Native.MiniAudio.MaWaveformPtr pWaveform, double amplitude)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37873) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0f443d2285fda842"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformSetFrequency(ref Inno.Native.MiniAudio.MaWaveform pWaveform, double frequency)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37897) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-becddca7e27b5421"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformSetFrequency(Inno.Native.MiniAudio.MaWaveformPtr pWaveform, double frequency)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37890) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c75f7107d4de664a"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformSetSampleRate(ref Inno.Native.MiniAudio.MaWaveform pWaveform, uint sampleRate)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37931) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-491248e23f4bb694"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformSetSampleRate(Inno.Native.MiniAudio.MaWaveformPtr pWaveform, uint sampleRate)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37924) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e702dbf960913da"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformSetType(ref Inno.Native.MiniAudio.MaWaveform pWaveform, Inno.Native.MiniAudio.MaWaveformType type)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37914) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bbf042028c91a9dc"></a>[`public static Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MiniAudio.WaveformSetType(Inno.Native.MiniAudio.MaWaveformPtr pWaveform, Inno.Native.MiniAudio.MaWaveformType type)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37907) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c73c0147be493e37"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.WaveformUninit(ref Inno.Native.MiniAudio.MaWaveform pWaveform)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37821) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-692408967bbf5678"></a>[`public static void Inno.Native.MiniAudio.MiniAudio.WaveformUninit(Inno.Native.MiniAudio.MaWaveformPtr pWaveform)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L37815) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.NotificationCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.NotificationCallback c5b54b29c3262f705a7823cbb7abe6f1057e26ab459f62a64aa1217a75a24710 -->
<!-- api: M:Inno.Native.MiniAudio.NotificationCallback.#ctor(System.Object,System.IntPtr) 272944cccb79fb7a492f4b77c8e6d6acadfdb94d575d6f5a9eb89ca828379ec3 -->
<!-- api: M:Inno.Native.MiniAudio.NotificationCallback.Invoke(Inno.Native.MiniAudio.MaDeviceNotification*) 25da4b528d716c7297a3d610da6552a0b9df1c9590c6c2982d208f244a81b535 -->
<!-- api: M:Inno.Native.MiniAudio.NotificationCallback.BeginInvoke(Inno.Native.MiniAudio.MaDeviceNotification*,System.AsyncCallback,System.Object) 83c309dd005e3aa40d1f1d98309d50dc29052cc5fa5d9392911c49482d947048 -->
<!-- api: M:Inno.Native.MiniAudio.NotificationCallback.EndInvoke(System.IAsyncResult) 251aa23fcea7e2915ad1716ae0d4e29d3e611c8d8097667de6419fed9c97d50e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-740eef8e1198a4dd"></a>[`public Inno.Native.MiniAudio.NotificationCallback.NotificationCallback(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14026) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b2f9c44f4faad099"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.NotificationCallback.BeginInvoke(Inno.Native.MiniAudio.MaDeviceNotification* pNotification, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14026) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fb032575a1e7ea77"></a>[`public virtual void Inno.Native.MiniAudio.NotificationCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14026) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-63683ecf8951c5be"></a>[`public virtual void Inno.Native.MiniAudio.NotificationCallback.Invoke(Inno.Native.MiniAudio.MaDeviceNotification* pNotification)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14026) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-62f621fd8892d15b"></a>[`public delegate void Inno.Native.MiniAudio.NotificationCallback(Inno.Native.MiniAudio.MaDeviceNotification* pNotification)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14026) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.NotificationCallback1</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.NotificationCallback1 78f9f73a0914a036f586a6e63ddecb01a7cbf72d1aa158bd099c7ef5773e9efa -->
<!-- api: M:Inno.Native.MiniAudio.NotificationCallback1.#ctor(System.Object,System.IntPtr) ef5c749f5d31eefd2dd08eab3b8e0771545ace1e03333ba7870a48662f4138f2 -->
<!-- api: M:Inno.Native.MiniAudio.NotificationCallback1.Invoke(Inno.Native.MiniAudio.MaDeviceNotification*) 32ddc2e45ad6045d0e96e1a76c02653aa007b53d13e0b53e957293196f48d187 -->
<!-- api: M:Inno.Native.MiniAudio.NotificationCallback1.BeginInvoke(Inno.Native.MiniAudio.MaDeviceNotification*,System.AsyncCallback,System.Object) 64321a7411ace176d9004f3af5dd4a7093ebc56ded88c6f958f1063eb3391021 -->
<!-- api: M:Inno.Native.MiniAudio.NotificationCallback1.EndInvoke(System.IAsyncResult) d50ac5b31961925c5093210707e2e62df33a84229e4f550a665534b723f24a34 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d93ac39ebed06da0"></a>[`public Inno.Native.MiniAudio.NotificationCallback1.NotificationCallback1(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14316) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-818fac957bda6265"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.NotificationCallback1.BeginInvoke(Inno.Native.MiniAudio.MaDeviceNotification* pNotification, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14316) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2ca9b3c7abbb073b"></a>[`public virtual void Inno.Native.MiniAudio.NotificationCallback1.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14316) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e331913b5df7a12e"></a>[`public virtual void Inno.Native.MiniAudio.NotificationCallback1.Invoke(Inno.Native.MiniAudio.MaDeviceNotification* pNotification)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14316) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0923d9fac41aa115"></a>[`public delegate void Inno.Native.MiniAudio.NotificationCallback1(Inno.Native.MiniAudio.MaDeviceNotification* pNotification)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14316) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnClose</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnClose 7916a016a1bf1d9f68c9fb71bcef41590ab483a85d378e654afea72990ce6d27 -->
<!-- api: M:Inno.Native.MiniAudio.OnClose.#ctor(System.Object,System.IntPtr) 8e7015d1f9b55c2d471fd8f7183315b6370726a601a33e146643fd1f36e1bcab -->
<!-- api: M:Inno.Native.MiniAudio.OnClose.Invoke(Inno.Native.MiniAudio.MaVfs*,System.IntPtr) 33ab72ed0b1aa9f20d3edb81e6420c77ac21e9a5707a93f82a39491fcbbec85f -->
<!-- api: M:Inno.Native.MiniAudio.OnClose.BeginInvoke(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,System.AsyncCallback,System.Object) ac0cb982978e7f32f94ffe654c667a2810ee1e6414d015dd237a7d752acea537 -->
<!-- api: M:Inno.Native.MiniAudio.OnClose.EndInvoke(System.IAsyncResult) fc09d36b216ac6a83fe553a2777e244eab960cbbf9b1f19ed01da9dbc0d4e515 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-41ae17ead56dc66f"></a>[`public Inno.Native.MiniAudio.OnClose.OnClose(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14126) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-66820b23064bc4a7"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnClose.BeginInvoke(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14126) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cecbd5019aee2848"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnClose.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14126) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-08447715246ba027"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnClose.Invoke(Inno.Native.MiniAudio.MaVfs* pVFS, nint file)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14126) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-276ea4ce45a7bfab"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnClose(Inno.Native.MiniAudio.MaVfs* pVFS, nint file)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14126) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnContextEnumerateDevices</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnContextEnumerateDevices cabe64ba8d16a9271d0cb9e0583ae6b81a7b0533672d5ec3c1a973b2dce20147 -->
<!-- api: M:Inno.Native.MiniAudio.OnContextEnumerateDevices.#ctor(System.Object,System.IntPtr) 602e4e7d5ae1a4270239039ec9f9e44788af22f8c8ab6e197e86fbb7dfe9623f -->
<!-- api: M:Inno.Native.MiniAudio.OnContextEnumerateDevices.Invoke(Inno.Native.MiniAudio.MaContext*,System.Void*,System.Void*) cea481dc77c15607526e3bdb3789689926dd6bfd86b0a8982eb9e4d877943903 -->
<!-- api: M:Inno.Native.MiniAudio.OnContextEnumerateDevices.BeginInvoke(Inno.Native.MiniAudio.MaContext*,System.Void*,System.Void*,System.AsyncCallback,System.Object) 788d736191ac258bdc32b61da712d517253b84a25c773efe09d37e2929464758 -->
<!-- api: M:Inno.Native.MiniAudio.OnContextEnumerateDevices.EndInvoke(System.IAsyncResult) cba8a23fa656972cec9148e876b98a40d3f7ba054313a67b596ea0a248e6f7a5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2f6253e821223e2f"></a>[`public Inno.Native.MiniAudio.OnContextEnumerateDevices.OnContextEnumerateDevices(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14046) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8f7db99ce0bcbbc0"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnContextEnumerateDevices.BeginInvoke(Inno.Native.MiniAudio.MaContext* pContext, void* callback, void* pUserData, System.AsyncCallback __callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14046) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-83611782e823ce89"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnContextEnumerateDevices.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14046) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d698b0022068416c"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnContextEnumerateDevices.Invoke(Inno.Native.MiniAudio.MaContext* pContext, void* callback, void* pUserData)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14046) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7a667cd780d8f8ef"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnContextEnumerateDevices(Inno.Native.MiniAudio.MaContext* pContext, void* callback, void* pUserData)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14046) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnContextGetDeviceInfo</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnContextGetDeviceInfo f3d4b6904380297a4b24cfdd8e36b0247239cc611018bbfb814560e9c8c19e9a -->
<!-- api: M:Inno.Native.MiniAudio.OnContextGetDeviceInfo.#ctor(System.Object,System.IntPtr) 6e010552f5c3edac0cb67d2f23dd66a9fa401fdec5073ce2085cd97a37a7d6ae -->
<!-- api: M:Inno.Native.MiniAudio.OnContextGetDeviceInfo.Invoke(Inno.Native.MiniAudio.MaContext*,Inno.Native.MiniAudio.MaDeviceType,Inno.Native.MiniAudio.MaDeviceId*,Inno.Native.MiniAudio.MaDeviceInfo*) a52ab707601f5efbf381cec36da88c9caa7b13f1b8bee17429d0d7462a7b2ab2 -->
<!-- api: M:Inno.Native.MiniAudio.OnContextGetDeviceInfo.BeginInvoke(Inno.Native.MiniAudio.MaContext*,Inno.Native.MiniAudio.MaDeviceType,Inno.Native.MiniAudio.MaDeviceId*,Inno.Native.MiniAudio.MaDeviceInfo*,System.AsyncCallback,System.Object) bb0e85abb936aa11e25cadfb3c2c6c164f95d7c8367f2497857442e6d6408a1c -->
<!-- api: M:Inno.Native.MiniAudio.OnContextGetDeviceInfo.EndInvoke(System.IAsyncResult) 012f0399f22267c24beab597e46557445bd456cd00ee595dbb289b3ec8a4495f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3644be713281f0e7"></a>[`public Inno.Native.MiniAudio.OnContextGetDeviceInfo.OnContextGetDeviceInfo(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14051) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-cad59e06ec6cc201"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnContextGetDeviceInfo.BeginInvoke(Inno.Native.MiniAudio.MaContext* pContext, Inno.Native.MiniAudio.MaDeviceType deviceType, Inno.Native.MiniAudio.MaDeviceId* pDeviceID, Inno.Native.MiniAudio.MaDeviceInfo* pDeviceInfo, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14051) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ce3b29135a9d0a6e"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnContextGetDeviceInfo.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14051) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8c85883d772d0fa5"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnContextGetDeviceInfo.Invoke(Inno.Native.MiniAudio.MaContext* pContext, Inno.Native.MiniAudio.MaDeviceType deviceType, Inno.Native.MiniAudio.MaDeviceId* pDeviceID, Inno.Native.MiniAudio.MaDeviceInfo* pDeviceInfo)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14051) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-29b846337ca78cc4"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnContextGetDeviceInfo(Inno.Native.MiniAudio.MaContext* pContext, Inno.Native.MiniAudio.MaDeviceType deviceType, Inno.Native.MiniAudio.MaDeviceId* pDeviceID, Inno.Native.MiniAudio.MaDeviceInfo* pDeviceInfo)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14051) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnContextInit</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnContextInit c21942839869e2683750c11789ea0bc0310118e924009adcf8d709fb8db3c29c -->
<!-- api: M:Inno.Native.MiniAudio.OnContextInit.#ctor(System.Object,System.IntPtr) a3d38c5f7ba24dfec034c0d6bb3ada17bd4c94e9cabca7d638a600396e5adf4b -->
<!-- api: M:Inno.Native.MiniAudio.OnContextInit.Invoke(Inno.Native.MiniAudio.MaContext*,Inno.Native.MiniAudio.MaContextConfig*,Inno.Native.MiniAudio.MaBackendCallbacks*) cc88bb2d734f1318fe749f666b07e1e8c795c9853ef4b0783b428cb54f534d0d -->
<!-- api: M:Inno.Native.MiniAudio.OnContextInit.BeginInvoke(Inno.Native.MiniAudio.MaContext*,Inno.Native.MiniAudio.MaContextConfig*,Inno.Native.MiniAudio.MaBackendCallbacks*,System.AsyncCallback,System.Object) f439a82e6a50861a5a76a27daa1732a221d4e96448111bff83d85210be407ed3 -->
<!-- api: M:Inno.Native.MiniAudio.OnContextInit.EndInvoke(System.IAsyncResult) 4b7be8d6f70c2a5b8d55cf50142f01abc0569d91f1404e5d0d0fd8a5b5dea763 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-058786c6d3f3cb86"></a>[`public Inno.Native.MiniAudio.OnContextInit.OnContextInit(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14036) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e29ba0f89ecee4ac"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnContextInit.BeginInvoke(Inno.Native.MiniAudio.MaContext* pContext, Inno.Native.MiniAudio.MaContextConfig* pConfig, Inno.Native.MiniAudio.MaBackendCallbacks* pCallbacks, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14036) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1b4b742aafda9f08"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnContextInit.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14036) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d2028fcbdba74164"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnContextInit.Invoke(Inno.Native.MiniAudio.MaContext* pContext, Inno.Native.MiniAudio.MaContextConfig* pConfig, Inno.Native.MiniAudio.MaBackendCallbacks* pCallbacks)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14036) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d9e5064e094463d6"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnContextInit(Inno.Native.MiniAudio.MaContext* pContext, Inno.Native.MiniAudio.MaContextConfig* pConfig, Inno.Native.MiniAudio.MaBackendCallbacks* pCallbacks)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14036) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnContextUninit</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnContextUninit a89c1172bc9862ac0c672c9020b160e191e348186ab1e9dc21e25650b0933928 -->
<!-- api: M:Inno.Native.MiniAudio.OnContextUninit.#ctor(System.Object,System.IntPtr) b70528907f60554197336e824b431223cb938f672bca7efa6af4be56113e37c6 -->
<!-- api: M:Inno.Native.MiniAudio.OnContextUninit.Invoke(Inno.Native.MiniAudio.MaContext*) b32be6270ac75613d78aae7d525d65a496c20ba03791c623086830e405153dd3 -->
<!-- api: M:Inno.Native.MiniAudio.OnContextUninit.BeginInvoke(Inno.Native.MiniAudio.MaContext*,System.AsyncCallback,System.Object) a0be6f47397f394e1eef62178b89923de594661a7efaa136656a15637529531f -->
<!-- api: M:Inno.Native.MiniAudio.OnContextUninit.EndInvoke(System.IAsyncResult) efd81843a2a22712390b3a9c9a9c8efb3ea5b05dfca9e7e9b5d6694a5defa102 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3995ab98e14fdb67"></a>[`public Inno.Native.MiniAudio.OnContextUninit.OnContextUninit(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14041) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f857b869bbd87838"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnContextUninit.BeginInvoke(Inno.Native.MiniAudio.MaContext* pContext, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14041) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f35f26a1726c5a86"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnContextUninit.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14041) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-983fdd320d28c600"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnContextUninit.Invoke(Inno.Native.MiniAudio.MaContext* pContext)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14041) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-319da1625b55a5d0"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnContextUninit(Inno.Native.MiniAudio.MaContext* pContext)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14041) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnData</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnData 5ac9063f8cde544cc1b13c706fb980f8e8706a8db8ec6d4644b9b8973b4a90e5 -->
<!-- api: M:Inno.Native.MiniAudio.OnData.#ctor(System.Object,System.IntPtr) 46f2c4c03c56637b972def86ce7902825eafad459af2c54b7ee05548fea4b4aa -->
<!-- api: M:Inno.Native.MiniAudio.OnData.Invoke(Inno.Native.MiniAudio.MaDevice*,System.Void*,System.Void*,System.UInt32) 3e1083bbcc88c26b991e2f3113cbe42cf2104463d9cab8dc59ace3a8aeb26a69 -->
<!-- api: M:Inno.Native.MiniAudio.OnData.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,System.Void*,System.Void*,System.UInt32,System.AsyncCallback,System.Object) 2dbd1245e63a9d741ebac752ca826c4906863199d5afd3d6e2dde8ae4cb6c905 -->
<!-- api: M:Inno.Native.MiniAudio.OnData.EndInvoke(System.IAsyncResult) a8d791c793e209798d7a5bb6fff3fe301deea26176156cdf097316b96e73262d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-775fe4fe7eced568"></a>[`public Inno.Native.MiniAudio.OnData.OnData(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14006) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3fa595f26e592201"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnData.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, void* pOutput, void* pInput, uint frameCount, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14006) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-63510f364894eed0"></a>[`public virtual void Inno.Native.MiniAudio.OnData.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14006) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4445f1b7e43c2777"></a>[`public virtual void Inno.Native.MiniAudio.OnData.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice, void* pOutput, void* pInput, uint frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14006) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c949f7021a3a978b"></a>[`public delegate void Inno.Native.MiniAudio.OnData(Inno.Native.MiniAudio.MaDevice* pDevice, void* pOutput, void* pInput, uint frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14006) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnDeviceDataLoop</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnDeviceDataLoop 395daac67595ebb618c2ed84def4f18be23683f56ff419d51b76d24c8b5ebbe1 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceDataLoop.#ctor(System.Object,System.IntPtr) 8a54e1da1be88ebcc438caf92de8dfda8f77ca8e1c0e5bd246e5a97c5fe75eaa -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceDataLoop.Invoke(Inno.Native.MiniAudio.MaDevice*) f4e0570a7b02636d651417fed73312c7305aca0ad482eb45220d463c6129eac2 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceDataLoop.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,System.AsyncCallback,System.Object) ec3765e781e239f47ee164ca8a5db2dde30f863cf6bb1d54069daacf015400da -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceDataLoop.EndInvoke(System.IAsyncResult) 171505eeab795a642326e7456671bf79b4a4d7e99e0536f18b56aa7ac15f1d70 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e68ec32d39efbecf"></a>[`public Inno.Native.MiniAudio.OnDeviceDataLoop.OnDeviceDataLoop(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14086) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8b45344a8c8ad442"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnDeviceDataLoop.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14086) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f92791720a50bb87"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceDataLoop.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14086) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5385630b8cefd7a0"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceDataLoop.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14086) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a1871065184ac97f"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceDataLoop(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14086) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnDeviceDataLoopWakeup</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnDeviceDataLoopWakeup 6915537561f11ed851dcacfb754919be658eddd024fe4855e9d59ee4645e31c6 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceDataLoopWakeup.#ctor(System.Object,System.IntPtr) 26cefb082df79f81bde7dd78987261f815372c7a911c1b2949e12edb8d097df7 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceDataLoopWakeup.Invoke(Inno.Native.MiniAudio.MaDevice*) 58592c6e478eeb6f467302a0e235d36c7dd0c4b986911aed7904af16034b1159 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceDataLoopWakeup.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,System.AsyncCallback,System.Object) e0b93948f225ec15dcd97d6862c0c4581feec5e44ba5bdcb9e2bb593cbcf4bda -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceDataLoopWakeup.EndInvoke(System.IAsyncResult) 2d4596f63c9cad77da98f5953d436f8cc2280701c110b094b16069b918a35a0f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-622ee9f8ecc93fab"></a>[`public Inno.Native.MiniAudio.OnDeviceDataLoopWakeup.OnDeviceDataLoopWakeup(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14091) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-65d30b71d8c9cd13"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnDeviceDataLoopWakeup.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14091) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-97f1ff68da175343"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceDataLoopWakeup.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14091) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-86e99c3ac74e457b"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceDataLoopWakeup.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14091) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e7b7f4c8936d4c3d"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceDataLoopWakeup(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14091) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnDeviceGetInfo</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnDeviceGetInfo 3eacfba5e416a3ea8980625174cc6fa443acc6dfa2806ed5e0bcd8e0db837103 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceGetInfo.#ctor(System.Object,System.IntPtr) 1d6861eecf66e9715af81465cb3ea92a4a4b390e7711b7ac7b5f7d4cadcddc5d -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceGetInfo.Invoke(Inno.Native.MiniAudio.MaDevice*,Inno.Native.MiniAudio.MaDeviceType,Inno.Native.MiniAudio.MaDeviceInfo*) e8517e81fe6bc369ec41038a4a7de3ca850cc552f2f721a2163705904dee329e -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceGetInfo.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,Inno.Native.MiniAudio.MaDeviceType,Inno.Native.MiniAudio.MaDeviceInfo*,System.AsyncCallback,System.Object) ccf6e72f541c4abdb8e377441cf48465d774cd0b7c04bb38a4950e9d398b1cc7 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceGetInfo.EndInvoke(System.IAsyncResult) 61af8db46982ff579f32c9138b6def9e11a7a832c3a8300df2c7cdab17c50c4e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a49145fb4dc2dbe1"></a>[`public Inno.Native.MiniAudio.OnDeviceGetInfo.OnDeviceGetInfo(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14096) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-145c4531649cc065"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnDeviceGetInfo.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, Inno.Native.MiniAudio.MaDeviceType type, Inno.Native.MiniAudio.MaDeviceInfo* pDeviceInfo, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14096) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e98d350a1f454ce7"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceGetInfo.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14096) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9f881b5bc2d1daf3"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceGetInfo.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice, Inno.Native.MiniAudio.MaDeviceType type, Inno.Native.MiniAudio.MaDeviceInfo* pDeviceInfo)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14096) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ff6ccde2cabfac47"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceGetInfo(Inno.Native.MiniAudio.MaDevice* pDevice, Inno.Native.MiniAudio.MaDeviceType type, Inno.Native.MiniAudio.MaDeviceInfo* pDeviceInfo)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14096) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnDeviceInit</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnDeviceInit 43fc0113ddf3d13d8f2c3bb3b1d572aedb28d90b458d31c9ed32126b645d9cb8 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceInit.#ctor(System.Object,System.IntPtr) 07868bf6e9756fbe46bcce9ddfcf57602585126c50f45d2644bb82b9cf6f7af9 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceInit.Invoke(Inno.Native.MiniAudio.MaDevice*,Inno.Native.MiniAudio.MaDeviceConfig*,Inno.Native.MiniAudio.MaDeviceDescriptor*,Inno.Native.MiniAudio.MaDeviceDescriptor*) 893eb3e8b1f6399909076692c56a6130e90654bd49ef40da8377b97c3b10beac -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceInit.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,Inno.Native.MiniAudio.MaDeviceConfig*,Inno.Native.MiniAudio.MaDeviceDescriptor*,Inno.Native.MiniAudio.MaDeviceDescriptor*,System.AsyncCallback,System.Object) fa9e1155ebdc70295378da37f26103b0a99136a70f56c113e917f7f73ec1be14 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceInit.EndInvoke(System.IAsyncResult) c9c8f5793506b31935413289640717577796afc2e6505ebf45fb724831cc4057 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-49f4ca8e0b09a319"></a>[`public Inno.Native.MiniAudio.OnDeviceInit.OnDeviceInit(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14056) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fcec49b21356a31e"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnDeviceInit.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, Inno.Native.MiniAudio.MaDeviceConfig* pConfig, Inno.Native.MiniAudio.MaDeviceDescriptor* pDescriptorPlayback, Inno.Native.MiniAudio.MaDeviceDescriptor* pDescriptorCapture, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14056) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8b8df5a987876be9"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceInit.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14056) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f46ebd230f960b30"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceInit.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice, Inno.Native.MiniAudio.MaDeviceConfig* pConfig, Inno.Native.MiniAudio.MaDeviceDescriptor* pDescriptorPlayback, Inno.Native.MiniAudio.MaDeviceDescriptor* pDescriptorCapture)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14056) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4bd5fa57631c8f3f"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceInit(Inno.Native.MiniAudio.MaDevice* pDevice, Inno.Native.MiniAudio.MaDeviceConfig* pConfig, Inno.Native.MiniAudio.MaDeviceDescriptor* pDescriptorPlayback, Inno.Native.MiniAudio.MaDeviceDescriptor* pDescriptorCapture)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14056) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnDeviceRead</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnDeviceRead 8f924c23166371fa5902aa3d91465426420eb5774ef4a620ddd5e88c0e6fc75f -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceRead.#ctor(System.Object,System.IntPtr) bb4d0baf19fd9e300def48f12af0a05c7917faeada8197d54ad58369766e4ebb -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceRead.Invoke(Inno.Native.MiniAudio.MaDevice*,System.Void*,System.UInt32,System.UInt32*) bfb744250555e7cfa962371f779797ce4c5d752accb8f4fc3690b5aa07578381 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceRead.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,System.Void*,System.UInt32,System.UInt32*,System.AsyncCallback,System.Object) a4938632a81b7e787466de4b2e253d50209dff950691e614cdc5a0a5b56af506 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceRead.EndInvoke(System.IAsyncResult) 9fe65dc7e7a432f0971ef36276ee0f29d8192ed4e111c564bf0b3b9e9a687dc4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2e15e1557a13bec1"></a>[`public Inno.Native.MiniAudio.OnDeviceRead.OnDeviceRead(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14076) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-665c0dc792125fe4"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnDeviceRead.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, void* pFrames, uint frameCount, uint* pFramesRead, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14076) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-00b65f759148ebbd"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceRead.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14076) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2eb94ce2eb49dcd3"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceRead.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice, void* pFrames, uint frameCount, uint* pFramesRead)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14076) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dd878e2ef3e972d5"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceRead(Inno.Native.MiniAudio.MaDevice* pDevice, void* pFrames, uint frameCount, uint* pFramesRead)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14076) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnDeviceStart</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnDeviceStart 990fd04be3d2df51ff2b0b4d637e6b0d107a5f5cd48964d20e9723ad7315a165 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceStart.#ctor(System.Object,System.IntPtr) 6567f8401daa7fab99e601990602e462994e1f6f69ef47625984ddd6da5e5cb2 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceStart.Invoke(Inno.Native.MiniAudio.MaDevice*) d0f4dd7d58f35fbf2c4655a5587f0a672e5590baef58e2754c33f504932c7689 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceStart.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,System.AsyncCallback,System.Object) 2313623e39785af2eb4609fa9766fd01dbfe57007e0e8cc919aa64da3947c14c -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceStart.EndInvoke(System.IAsyncResult) d1bd6df64b7ebe0c014d4d1f546ea9d179a9d3ffd703f70296f0c84b4f87a939 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9c4761eb71bf79e3"></a>[`public Inno.Native.MiniAudio.OnDeviceStart.OnDeviceStart(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14066) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5943d93df0987fac"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnDeviceStart.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14066) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6e404a389bfc2ca7"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceStart.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14066) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e8a982a591139d19"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceStart.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14066) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-739cab20b5e36a52"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceStart(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14066) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnDeviceStop</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnDeviceStop e9133b5d34123d624129a27e461b78a872b4d1cdade72b95ef8789e29c9bc87b -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceStop.#ctor(System.Object,System.IntPtr) cce87502db44c21b8172f831d26cb374bdf913154d81f3cdae291d04514e6c68 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceStop.Invoke(Inno.Native.MiniAudio.MaDevice*) 57de6e2e545a40b57d36ba97a4dca3698e663330849566aaa38e4e54b4e1b939 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceStop.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,System.AsyncCallback,System.Object) bd08e439994110089c985c772d249a97d9dcd19dd7170b9fdc99fe280bf5c9be -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceStop.EndInvoke(System.IAsyncResult) bde3affa31b7eb74bc436a68037a75fe64bb8957539cd6c2e95ae032ea1db770 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a6898efaf70246fd"></a>[`public Inno.Native.MiniAudio.OnDeviceStop.OnDeviceStop(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14071) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b64530dde4d730e7"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnDeviceStop.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14071) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-79be23be3d53a971"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceStop.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14071) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2323b69b5ba76e10"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceStop.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14071) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e15f0797b15a3f73"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceStop(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14071) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnDeviceUninit</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnDeviceUninit c56c44bef1522bfd357568ba6fc58d0d88b1198dd4b2d1e051c9f535ca4cf1ac -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceUninit.#ctor(System.Object,System.IntPtr) dffa50b11f97648fb9ae2e95ec865dfdcac13288467b73c0726b3951a49a1df7 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceUninit.Invoke(Inno.Native.MiniAudio.MaDevice*) 0f3272928857221ba219433ff6be326a23395537c74e4b263417db872a2a2171 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceUninit.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,System.AsyncCallback,System.Object) 2e7984aacf30871541e1224b9516b6ed4c6598703bb88f53040aa3d0e1372307 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceUninit.EndInvoke(System.IAsyncResult) 5b6e47fd162427213426bf08a73e753b4822580ed4456de6fae87eed978e73bf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ec530c9e0ef7401a"></a>[`public Inno.Native.MiniAudio.OnDeviceUninit.OnDeviceUninit(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14061) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-66815a2bb06dbd69"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnDeviceUninit.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14061) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-62511e209dc24c23"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceUninit.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14061) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3b2694534d405cd9"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceUninit.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14061) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-879193d7aaeedef4"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceUninit(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14061) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnDeviceWrite</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnDeviceWrite a68daffaad8e52774cb138be00701b586c64214bee96a9fab7ec4f91854a5bd0 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceWrite.#ctor(System.Object,System.IntPtr) 83f976672fea89787a67567139caf0b6c672fbb33eaa731adfa6cec33b360285 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceWrite.Invoke(Inno.Native.MiniAudio.MaDevice*,System.Void*,System.UInt32,System.UInt32*) 4a5285705dd624b582537a88a9b7e7aa7a019c90e0d4facf27d8551cf698db91 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceWrite.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,System.Void*,System.UInt32,System.UInt32*,System.AsyncCallback,System.Object) 589401968f7154f5b78829e5a82adc08fcaeadefe924a55c55e64b1a15c2e9c2 -->
<!-- api: M:Inno.Native.MiniAudio.OnDeviceWrite.EndInvoke(System.IAsyncResult) 47f3a95e9aee5591c9a461c39166d70dfef3ae9e69ebe970f3cc0387f7694a52 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-cba7dbb75ea2bae9"></a>[`public Inno.Native.MiniAudio.OnDeviceWrite.OnDeviceWrite(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14081) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-864cc8598a47db79"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnDeviceWrite.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, void* pFrames, uint frameCount, uint* pFramesWritten, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14081) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ec2ea7dfddeeb548"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceWrite.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14081) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-60572b96648400d6"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceWrite.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice, void* pFrames, uint frameCount, uint* pFramesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14081) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6ad84fb8feb1bdaf"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnDeviceWrite(Inno.Native.MiniAudio.MaDevice* pDevice, void* pFrames, uint frameCount, uint* pFramesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14081) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnFree</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnFree c65db8bf27a833fdfff03b7a5fc4dac21e5b5cf7d23c52fc197a9176358d2d8f -->
<!-- api: M:Inno.Native.MiniAudio.OnFree.#ctor(System.Object,System.IntPtr) b6ba408e71a37c1126152e11feada8199fbccbe555d3e296fa2883dde4703846 -->
<!-- api: M:Inno.Native.MiniAudio.OnFree.Invoke(System.Void*,System.Void*) f3a7fbea61cfc1a908bc4b10dd366784285ce19ea988929065fd0496458535f3 -->
<!-- api: M:Inno.Native.MiniAudio.OnFree.BeginInvoke(System.Void*,System.Void*,System.AsyncCallback,System.Object) 96bd8b5ff87254861ac505c9d6bbd0155ac90b6f3b88f1fd6e42e0019d3f8da7 -->
<!-- api: M:Inno.Native.MiniAudio.OnFree.EndInvoke(System.IAsyncResult) 720eef72ced246d1072e256c07f2a8ea8ff9f8703a3fd93a0fb28cf8a2b2df3c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-80f6aa4bdef4f814"></a>[`public Inno.Native.MiniAudio.OnFree.OnFree(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13896) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-61de479e0d33f503"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnFree.BeginInvoke(void* p, void* pUserData, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13896) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a9cec6342ec54c66"></a>[`public virtual void Inno.Native.MiniAudio.OnFree.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13896) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-afe8b8c90f4bf142"></a>[`public virtual void Inno.Native.MiniAudio.OnFree.Invoke(void* p, void* pUserData)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13896) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8de3456ecc7f2cda"></a>[`public delegate void Inno.Native.MiniAudio.OnFree(void* p, void* pUserData)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13896) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnGetCursor</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnGetCursor e26d43bcc47bf1a228c2b20309a158500a121c86884f94aea7dee6e6055768a4 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetCursor.#ctor(System.Object,System.IntPtr) 8cd4d74b349997b781415abc8edb438548615bdc8aae762e1e33afbddefb3b46 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetCursor.Invoke(System.Void*,System.UInt64*) e37c08941a55e739c0dc6deb0f9daee00609d2fac7b224b548de04d7f2f94332 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetCursor.BeginInvoke(System.Void*,System.UInt64*,System.AsyncCallback,System.Object) c1e4e08016dac51fd60b8d9948ae93aca931fecd71d4fbf90d1a3450dc21b7d9 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetCursor.EndInvoke(System.IAsyncResult) 4caf3a7c3e2e867104e7570fc13328e41e85b1a1d8e75bd21fe0eeb41028554e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f10f09ffc1d3ec46"></a>[`public Inno.Native.MiniAudio.OnGetCursor.OnGetCursor(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13931) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-51b210bf3fefe8a7"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnGetCursor.BeginInvoke(void* pDataSource, ulong* pCursor, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13931) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9b27ae33b4ef4714"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetCursor.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13931) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe18baea3f68aef1"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetCursor.Invoke(void* pDataSource, ulong* pCursor)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13931) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4b262411f3e79cfc"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetCursor(void* pDataSource, ulong* pCursor)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13931) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnGetDataFormat</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnGetDataFormat f3a6c855550003e398afcd27e8b112e902df735d8b7cadba4fcf83209532759d -->
<!-- api: M:Inno.Native.MiniAudio.OnGetDataFormat.#ctor(System.Object,System.IntPtr) 969a2c027972eaca48cf4ef77e480aba8b5cfa3b0e31ff8d5c1f939e95483661 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetDataFormat.Invoke(System.Void*,Inno.Native.MiniAudio.MaFormat*,System.UInt32*,System.UInt32*,System.Byte*,System.UIntPtr) cde606e5aee190f5bd720e748e18be1d9a40805cca9795c0e4cc5e855eb03e4c -->
<!-- api: M:Inno.Native.MiniAudio.OnGetDataFormat.BeginInvoke(System.Void*,Inno.Native.MiniAudio.MaFormat*,System.UInt32*,System.UInt32*,System.Byte*,System.UIntPtr,System.AsyncCallback,System.Object) dfbdcfea3a1fda07a7fdcc694ad97be05d6ad301baa74f50cc0582eeb1e21acf -->
<!-- api: M:Inno.Native.MiniAudio.OnGetDataFormat.EndInvoke(System.IAsyncResult) 3a36cf69d9bea9f2ec25f11e5d40fd5e52bbda8b3bd45f1b4c58191b130123fe -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3e0baf880d4d2e59"></a>[`public Inno.Native.MiniAudio.OnGetDataFormat.OnGetDataFormat(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13926) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-48548ddca71da9c8"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnGetDataFormat.BeginInvoke(void* pDataSource, Inno.Native.MiniAudio.MaFormat* pFormat, uint* pChannels, uint* pSampleRate, byte* pChannelMap, nuint channelMapCap, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13926) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f434c47d578da6c7"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetDataFormat.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13926) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eab9fc6517d4e449"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetDataFormat.Invoke(void* pDataSource, Inno.Native.MiniAudio.MaFormat* pFormat, uint* pChannels, uint* pSampleRate, byte* pChannelMap, nuint channelMapCap)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13926) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-78cdc52f9c4b2e12"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetDataFormat(void* pDataSource, Inno.Native.MiniAudio.MaFormat* pFormat, uint* pChannels, uint* pSampleRate, byte* pChannelMap, nuint channelMapCap)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13926) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnGetExpectedOutputFrameCount</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnGetExpectedOutputFrameCount 6dbe3248e4795dfce5b2b7dc393f61445e5857b11641917e3a79c2e29fee6548 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetExpectedOutputFrameCount.#ctor(System.Object,System.IntPtr) 1e1b5ce85dcd2a481b0ba3f910e269ac6a268930d568f1daf27096ae7cf239a8 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetExpectedOutputFrameCount.Invoke(System.Void*,System.Void*,System.UInt64,System.UInt64*) 8e413a9a739f07e3b223a00449ef8e67b3779299d1ab4f7e43c05ab888915abd -->
<!-- api: M:Inno.Native.MiniAudio.OnGetExpectedOutputFrameCount.BeginInvoke(System.Void*,System.Void*,System.UInt64,System.UInt64*,System.AsyncCallback,System.Object) 996cea8410231b49fb7db4518f2d01b4b62fa3cd339e9f8a63a4c6517547604d -->
<!-- api: M:Inno.Native.MiniAudio.OnGetExpectedOutputFrameCount.EndInvoke(System.IAsyncResult) 50cfa345879c505b05da37d51726f1ef0fd1db99719dbf7e8820ccb85b89519e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9937501ad8cd7328"></a>[`public Inno.Native.MiniAudio.OnGetExpectedOutputFrameCount.OnGetExpectedOutputFrameCount(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13996) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-667ec1e5809e66ee"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnGetExpectedOutputFrameCount.BeginInvoke(void* pUserData, void* pBackend, ulong inputFrameCount, ulong* pOutputFrameCount, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13996) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-829725dcd836ed84"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetExpectedOutputFrameCount.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13996) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-93adf4ad8df76313"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetExpectedOutputFrameCount.Invoke(void* pUserData, void* pBackend, ulong inputFrameCount, ulong* pOutputFrameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13996) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aea252ea732bce74"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetExpectedOutputFrameCount(void* pUserData, void* pBackend, ulong inputFrameCount, ulong* pOutputFrameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13996) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnGetHeapSize</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnGetHeapSize 87c868f1c23d8d384f063799c7308a0c9588965a9380f995fa4fd6639d0e51b5 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetHeapSize.#ctor(System.Object,System.IntPtr) c0a3a318fe73e4e0067b0e2709c24a339bb6a7f32f31f1c20647a3bdce2ad9ad -->
<!-- api: M:Inno.Native.MiniAudio.OnGetHeapSize.Invoke(System.Void*,Inno.Native.MiniAudio.MaResamplerConfig*,System.UIntPtr*) bda98fd47930b2726098b81e956660d598eb61e7d9ab6b0ca8966313e2a1f711 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetHeapSize.BeginInvoke(System.Void*,Inno.Native.MiniAudio.MaResamplerConfig*,System.UIntPtr*,System.AsyncCallback,System.Object) fdf6678ffc13338de431171e96a734e8d92245340f5924da3c2cf5ddbb6d5f85 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetHeapSize.EndInvoke(System.IAsyncResult) 050b47a1eee06305b04da7d07d38a4b0cb38247e72a6219c519a3829d20baf77 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-bb7fcf95805f3aeb"></a>[`public Inno.Native.MiniAudio.OnGetHeapSize.OnGetHeapSize(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13956) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-79aaf0f40385bd53"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnGetHeapSize.BeginInvoke(void* pUserData, Inno.Native.MiniAudio.MaResamplerConfig* pConfig, nuint* pHeapSizeInBytes, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13956) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7c4ce5a071d46342"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetHeapSize.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13956) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-efed259400f74837"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetHeapSize.Invoke(void* pUserData, Inno.Native.MiniAudio.MaResamplerConfig* pConfig, nuint* pHeapSizeInBytes)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13956) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bd95f12b4cb21aa3"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetHeapSize(void* pUserData, Inno.Native.MiniAudio.MaResamplerConfig* pConfig, nuint* pHeapSizeInBytes)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13956) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnGetInputLatency</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnGetInputLatency a14483acc9439fcb59095a6bd6702acc9430b4f325aa62daf5b794e58538998c -->
<!-- api: M:Inno.Native.MiniAudio.OnGetInputLatency.#ctor(System.Object,System.IntPtr) d99da0a30925d504215fd4096a6c09132dd4dc16358bd91c93d2487eb318e2b3 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetInputLatency.Invoke(System.Void*,System.Void*) e6eb0ccf03e83333fb67e4a6ab361a7121fb4f380d5b8179ea16fa0d3079811a -->
<!-- api: M:Inno.Native.MiniAudio.OnGetInputLatency.BeginInvoke(System.Void*,System.Void*,System.AsyncCallback,System.Object) 47fda36cd9586d14b3086e83482da21f73d187196c028f286a37f8edbcebc6b3 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetInputLatency.EndInvoke(System.IAsyncResult) 11c7661e625a3bf9459d939461baedb87ef237f6823a126473473a06e95fdf1c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-62d05466e7e84ae3"></a>[`public Inno.Native.MiniAudio.OnGetInputLatency.OnGetInputLatency(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13981) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ce065b5ebe123adb"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnGetInputLatency.BeginInvoke(void* pUserData, void* pBackend, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13981) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4f74d9b3b4b5be50"></a>[`public virtual ulong Inno.Native.MiniAudio.OnGetInputLatency.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13981) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-79db9695486dc774"></a>[`public virtual ulong Inno.Native.MiniAudio.OnGetInputLatency.Invoke(void* pUserData, void* pBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13981) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1aee937468732670"></a>[`public delegate ulong Inno.Native.MiniAudio.OnGetInputLatency(void* pUserData, void* pBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13981) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnGetLength</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnGetLength ced0e70ca1fd444daa0e8fa84a6ebb8e5e7de7eaa30d4ae1164f5b417e46466c -->
<!-- api: M:Inno.Native.MiniAudio.OnGetLength.#ctor(System.Object,System.IntPtr) 41fa19a3815c06e172fc62d8cf9694301a73eb638425f47f87dcd12f91164439 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetLength.Invoke(System.Void*,System.UInt64*) 04945bd9876700247c3d299049cc15f149d1fab56c9a8cffd4c0efcd8597a775 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetLength.BeginInvoke(System.Void*,System.UInt64*,System.AsyncCallback,System.Object) 93e86e0ca37d8f180d41c2a7bd9a59ce8967f65cb7ae99ed3f9e2cbbe325c16b -->
<!-- api: M:Inno.Native.MiniAudio.OnGetLength.EndInvoke(System.IAsyncResult) 0096c4bedd3fcecb1a8cd78bb64bc7702e0a5318f17a91f4d1993bd3ce00ef1f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-24bc790de455aa90"></a>[`public Inno.Native.MiniAudio.OnGetLength.OnGetLength(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13936) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-91859a95b7f9dcb5"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnGetLength.BeginInvoke(void* pDataSource, ulong* pLength, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13936) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-09e1a8ee7700f8c2"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetLength.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13936) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ccd01727e982ca81"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetLength.Invoke(void* pDataSource, ulong* pLength)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13936) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-98a684100b8d93c5"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetLength(void* pDataSource, ulong* pLength)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13936) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnGetNext</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnGetNext d1fc2ee951417909dd024070b008050ba73a6171812e0470c7947850fe245d9c -->
<!-- api: M:Inno.Native.MiniAudio.OnGetNext.#ctor(System.Object,System.IntPtr) 7d5ee61ed445641d982b97696686685d5125c32472789a4718fcca64da9ebde2 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetNext.Invoke(System.Void*) 9f5c14bf60a3d46574b9bab09b9a8251709443aa60d247b8c2bee5e435d05ae1 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetNext.BeginInvoke(System.Void*,System.AsyncCallback,System.Object) 15ab0713e917210df8bad694fd427aeb0e91027b32e09ce7a7b020a101f7b096 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetNext.EndInvoke(System.IAsyncResult) b209c1ec58aae3466025035da8ae538b5d32486f6b140965e0abc3e8f8609373 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a32f5d178c0e622c"></a>[`public Inno.Native.MiniAudio.OnGetNext.OnGetNext(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13951) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4405e2ab0bb16495"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnGetNext.BeginInvoke(void* pDataSource, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13951) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4d5a9e6655433e24"></a>[`public virtual void* Inno.Native.MiniAudio.OnGetNext.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13951) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c0ede5d7a574da0b"></a>[`public virtual void* Inno.Native.MiniAudio.OnGetNext.Invoke(void* pDataSource)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13951) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dadc48882f9b1a70"></a>[`public delegate void* Inno.Native.MiniAudio.OnGetNext(void* pDataSource)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13951) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnGetOutputLatency</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnGetOutputLatency 4311a256365c862dc26b188ea9befb358f9eb3d9ccdae6ffd160751a21267d84 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetOutputLatency.#ctor(System.Object,System.IntPtr) d7547cc4fee72965f55cb4291c737b78da8a6385634efd4d6681cb034b064c94 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetOutputLatency.Invoke(System.Void*,System.Void*) 65696cd8e6b6f7147b295b8b508f641d790450b017f18bc2b6a1bad4d473225a -->
<!-- api: M:Inno.Native.MiniAudio.OnGetOutputLatency.BeginInvoke(System.Void*,System.Void*,System.AsyncCallback,System.Object) 80122240590c4c5ea899a3947c880653a6090aa57ebbc51407c890003dd554ad -->
<!-- api: M:Inno.Native.MiniAudio.OnGetOutputLatency.EndInvoke(System.IAsyncResult) 65ffa03f9cbf48072220713eb56e4802e16bbdcc6315ea7dfd2e4455fb38b97f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-536cfec357bce4f8"></a>[`public Inno.Native.MiniAudio.OnGetOutputLatency.OnGetOutputLatency(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13986) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-83dfc138e0a1391d"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnGetOutputLatency.BeginInvoke(void* pUserData, void* pBackend, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13986) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2549dbc4c244ee72"></a>[`public virtual ulong Inno.Native.MiniAudio.OnGetOutputLatency.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13986) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-499d73ba201704c4"></a>[`public virtual ulong Inno.Native.MiniAudio.OnGetOutputLatency.Invoke(void* pUserData, void* pBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13986) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c92c8d98b73f4144"></a>[`public delegate ulong Inno.Native.MiniAudio.OnGetOutputLatency(void* pUserData, void* pBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13986) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnGetRequiredInputFrameCount</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnGetRequiredInputFrameCount a4a46bdd64ec38559c38ba9a25153ecfef52607a0c02fa86587835717e26face -->
<!-- api: M:Inno.Native.MiniAudio.OnGetRequiredInputFrameCount.#ctor(System.Object,System.IntPtr) 6b676a472942c4d9bc6d9e8f3a94507a445c79a345581c4d88e377f0960a98e9 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetRequiredInputFrameCount.Invoke(System.Void*,System.Void*,System.UInt64,System.UInt64*) e6bc469e87c0dd33b79f24dc8f4dd691ec5463c12912d8816b1bb398dc9816cf -->
<!-- api: M:Inno.Native.MiniAudio.OnGetRequiredInputFrameCount.BeginInvoke(System.Void*,System.Void*,System.UInt64,System.UInt64*,System.AsyncCallback,System.Object) 330570bd33ed04c407d37017a78d85d475473ccce2b1b642edb3bfe9f619e200 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetRequiredInputFrameCount.EndInvoke(System.IAsyncResult) b8d1e01ccd5a39e8ae68f27fe15c63f8501eb039d03e0dbf983221af33208b2e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4a520b65ce230bb2"></a>[`public Inno.Native.MiniAudio.OnGetRequiredInputFrameCount.OnGetRequiredInputFrameCount(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13991) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0ae1c7fd8afc91e9"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnGetRequiredInputFrameCount.BeginInvoke(void* pUserData, void* pBackend, ulong outputFrameCount, ulong* pInputFrameCount, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13991) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-54bbd9d624f3277b"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetRequiredInputFrameCount.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13991) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5515bab7056a04fa"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetRequiredInputFrameCount.Invoke(void* pUserData, void* pBackend, ulong outputFrameCount, ulong* pInputFrameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13991) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4e7b48a2ccb67fb3"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetRequiredInputFrameCount(void* pUserData, void* pBackend, ulong outputFrameCount, ulong* pInputFrameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13991) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnGetRequiredInputFrameCount1</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnGetRequiredInputFrameCount1 05fdba4df1028a5217c1e04d8ee7118a1de4548ea7e0b1a8e485654b64545740 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetRequiredInputFrameCount1.#ctor(System.Object,System.IntPtr) 2118906ebd2c8d86b3eed368c79f49b24cc9e6c9e5d01a279a0a3680e32210a7 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetRequiredInputFrameCount1.Invoke(System.Void*,System.UInt32,System.UInt32*) 3cbec9a8793669d45805566741cf96305549109a76de65c92aac8f2eaf6b7a90 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetRequiredInputFrameCount1.BeginInvoke(System.Void*,System.UInt32,System.UInt32*,System.AsyncCallback,System.Object) 77cf0b2b25b2ed895acf6e262dc837b0da2e2f1701a3a9360b58650acee83c68 -->
<!-- api: M:Inno.Native.MiniAudio.OnGetRequiredInputFrameCount1.EndInvoke(System.IAsyncResult) 1f3f11027f080ab5a0698aadae4e5e59f0c8e9b781561170ed0ba3ac24bffa8c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-337fc26fd84dfadc"></a>[`public Inno.Native.MiniAudio.OnGetRequiredInputFrameCount1.OnGetRequiredInputFrameCount1(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14281) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-81db1592372e0328"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnGetRequiredInputFrameCount1.BeginInvoke(void* pNode, uint outputFrameCount, uint* pInputFrameCount, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14281) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-01a70460e63ddbb3"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetRequiredInputFrameCount1.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14281) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4a2f69c65cb114dc"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetRequiredInputFrameCount1.Invoke(void* pNode, uint outputFrameCount, uint* pInputFrameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14281) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3149f709db2ce73e"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnGetRequiredInputFrameCount1(void* pNode, uint outputFrameCount, uint* pInputFrameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14281) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnInfo</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnInfo b5bcb90b5ffd4e6ee2e815c9fbe2e582bd2be55c8cc885ee24265fd068f241b3 -->
<!-- api: M:Inno.Native.MiniAudio.OnInfo.#ctor(System.Object,System.IntPtr) a9ff98307c3945e5df83f20d0ea373e970aceea435dd1fd248c5684b8ced40e9 -->
<!-- api: M:Inno.Native.MiniAudio.OnInfo.Invoke(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,Inno.Native.MiniAudio.MaFileInfo*) cbeb8dda3bdde35b920fb2e2a0a85814f97525e6c9a927c79d3620e16c5d7191 -->
<!-- api: M:Inno.Native.MiniAudio.OnInfo.BeginInvoke(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,Inno.Native.MiniAudio.MaFileInfo*,System.AsyncCallback,System.Object) 7de12299e443e8c6c8b1f94c399b3aa12935c575acdf71342a1ab192f5330bc2 -->
<!-- api: M:Inno.Native.MiniAudio.OnInfo.EndInvoke(System.IAsyncResult) b8c612f84a93315050d0597bfa1ce0dd6c00a62e612b552a9cd9a0cc703d526d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ec34019b422ce35c"></a>[`public Inno.Native.MiniAudio.OnInfo.OnInfo(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14151) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4130c8e7199f9b52"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnInfo.BeginInvoke(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, Inno.Native.MiniAudio.MaFileInfo* pInfo, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14151) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0d52c9303d8474e4"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInfo.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14151) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c3cb304cb4ee67f0"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInfo.Invoke(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, Inno.Native.MiniAudio.MaFileInfo* pInfo)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14151) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-08b30263ff9a3ec8"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInfo(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, Inno.Native.MiniAudio.MaFileInfo* pInfo)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14151) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnInit</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnInit a3b38895fd86ca3d6018b1d7e856e38e78948687be3bef32d37c3adb14b493fa -->
<!-- api: M:Inno.Native.MiniAudio.OnInit.#ctor(System.Object,System.IntPtr) c8b246b0d9454cd16fb50abb040d6c2466b6428b421efc9867332bf04d48ce1e -->
<!-- api: M:Inno.Native.MiniAudio.OnInit.Invoke(System.Void*,Inno.Native.MiniAudio.MaResamplerConfig*,System.Void*,System.Void**) 737d6cc89bca507b952ffbe7ed6747c45e3a96f3918438bb1b80974b8a626623 -->
<!-- api: M:Inno.Native.MiniAudio.OnInit.BeginInvoke(System.Void*,Inno.Native.MiniAudio.MaResamplerConfig*,System.Void*,System.Void**,System.AsyncCallback,System.Object) e8126353fca64d4d39792254005ff9a4a41e132bcf2e4fcc8f4befc61e352035 -->
<!-- api: M:Inno.Native.MiniAudio.OnInit.EndInvoke(System.IAsyncResult) 266db6f8661ba18b6363febae1f8aa905903c08174bf93ee01cf4f4affbdd268 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2fd7aec4bc87bfe1"></a>[`public Inno.Native.MiniAudio.OnInit.OnInit(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13961) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-14f32e45da4459e6"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnInit.BeginInvoke(void* pUserData, Inno.Native.MiniAudio.MaResamplerConfig* pConfig, void* pHeap, void** ppBackend, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13961) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-98e04e867e30743d"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInit.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13961) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f83b467a45649fd5"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInit.Invoke(void* pUserData, Inno.Native.MiniAudio.MaResamplerConfig* pConfig, void* pHeap, void** ppBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13961) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f6a06e672b9dab6f"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInit(void* pUserData, Inno.Native.MiniAudio.MaResamplerConfig* pConfig, void* pHeap, void** ppBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13961) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnInit1</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnInit1 a85f22403d853a3cd25d1ecb2d6974e4954275da38629301129ecedf30c3d95f -->
<!-- api: M:Inno.Native.MiniAudio.OnInit1.#ctor(System.Object,System.IntPtr) e275bf4f96a4dba6faac03b17f29cceae49bc95b76b6a77ee77c78f3a8519d05 -->
<!-- api: M:Inno.Native.MiniAudio.OnInit1.Invoke(System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,Inno.Native.MiniAudio.MaDecodingBackendConfig*,Inno.Native.MiniAudio.MaAllocationCallbacks*,Inno.Native.MiniAudio.MaDataSource**) 47b6ef1c2d580ce3d55c1b7e43bd8cf87c5f87960cdd765f8e9fbcb0a88d1412 -->
<!-- api: M:Inno.Native.MiniAudio.OnInit1.BeginInvoke(System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,Inno.Native.MiniAudio.MaDecodingBackendConfig*,Inno.Native.MiniAudio.MaAllocationCallbacks*,Inno.Native.MiniAudio.MaDataSource**,System.AsyncCallback,System.Object) 95281157ff252cddbe1062498d27fa324b88e82d185f01f0a78904577921c394 -->
<!-- api: M:Inno.Native.MiniAudio.OnInit1.EndInvoke(System.IAsyncResult) ce7ed9e0def0b620bbf6d187a074c6ed9bf1d9ff525ade4b5893995e70f3989b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-505c61a640347ca2"></a>[`public Inno.Native.MiniAudio.OnInit1.OnInit1(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14171) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b92f28cfeab3c2e5"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnInit1.BeginInvoke(void* pUserData, void* onRead, void* onSeek, void* onTell, void* pReadSeekTellUserData, Inno.Native.MiniAudio.MaDecodingBackendConfig* pConfig, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, Inno.Native.MiniAudio.MaDataSource** ppBackend, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14171) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c625a563fc890db0"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInit1.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14171) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-940df75dd01b251f"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInit1.Invoke(void* pUserData, void* onRead, void* onSeek, void* onTell, void* pReadSeekTellUserData, Inno.Native.MiniAudio.MaDecodingBackendConfig* pConfig, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, Inno.Native.MiniAudio.MaDataSource** ppBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14171) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-032fd49627abe33a"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInit1(void* pUserData, void* onRead, void* onSeek, void* onTell, void* pReadSeekTellUserData, Inno.Native.MiniAudio.MaDecodingBackendConfig* pConfig, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, Inno.Native.MiniAudio.MaDataSource** ppBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14171) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnInit2</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnInit2 204fafbad3b12c7da286a4066f82a82d5f403e6eb0941ca63576fc45ca8ea03b -->
<!-- api: M:Inno.Native.MiniAudio.OnInit2.#ctor(System.Object,System.IntPtr) 70cc9b718b78a4cb4e9d1a884827fe81b29ef5ae9ff0ffb4585322ad31178f0d -->
<!-- api: M:Inno.Native.MiniAudio.OnInit2.Invoke(Inno.Native.MiniAudio.MaEncoder*) 45b657b3b2664e4a56d0bf4d5bd97461c31645f5bc3e281448f441c44d7813ae -->
<!-- api: M:Inno.Native.MiniAudio.OnInit2.BeginInvoke(Inno.Native.MiniAudio.MaEncoder*,System.AsyncCallback,System.Object) e49e14e5883fe1db0847c06232247c3ba669eeb16a347c0a60e05bad7c34636c -->
<!-- api: M:Inno.Native.MiniAudio.OnInit2.EndInvoke(System.IAsyncResult) 165df3d66bef57be43ce40b7661e300d074b9877e5ce8897fcc187a2e8d4a5a4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b16a966732b8b04e"></a>[`public Inno.Native.MiniAudio.OnInit2.OnInit2(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14261) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-af4dad192f2bdeca"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnInit2.BeginInvoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14261) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-109ae79981b15555"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInit2.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14261) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e94e23d872f845e1"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInit2.Invoke(Inno.Native.MiniAudio.MaEncoder* pEncoder)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14261) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2cc2c042bfec6d4d"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInit2(Inno.Native.MiniAudio.MaEncoder* pEncoder)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14261) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnInitFile</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnInitFile 4d06d9090444c4ae4c0c07c7a9acfa8a791ecf9ef06d3cd91b6cf7573d12d4b2 -->
<!-- api: M:Inno.Native.MiniAudio.OnInitFile.#ctor(System.Object,System.IntPtr) d916bee7f7663a674a4b5e3a2d505689f8757c7597ea653efabd6a340c025278 -->
<!-- api: M:Inno.Native.MiniAudio.OnInitFile.Invoke(System.Void*,System.Byte*,Inno.Native.MiniAudio.MaDecodingBackendConfig*,Inno.Native.MiniAudio.MaAllocationCallbacks*,Inno.Native.MiniAudio.MaDataSource**) 722951831ee0da2fbe04d3a7313f5655ad4f5f6b5b43e57ce4e577621c10db50 -->
<!-- api: M:Inno.Native.MiniAudio.OnInitFile.BeginInvoke(System.Void*,System.Byte*,Inno.Native.MiniAudio.MaDecodingBackendConfig*,Inno.Native.MiniAudio.MaAllocationCallbacks*,Inno.Native.MiniAudio.MaDataSource**,System.AsyncCallback,System.Object) ce28c118647979c958d1bc5b73ca5c4f97aa8672f7a27c45f143653d173e9cfe -->
<!-- api: M:Inno.Native.MiniAudio.OnInitFile.EndInvoke(System.IAsyncResult) f251fdb95624b9aa580a71f1cda750176773413fd963f0c442a680dad71ef07b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ba64761c7c85413f"></a>[`public Inno.Native.MiniAudio.OnInitFile.OnInitFile(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14176) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-06756d43e71434b9"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnInitFile.BeginInvoke(void* pUserData, byte* pFilePath, Inno.Native.MiniAudio.MaDecodingBackendConfig* pConfig, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, Inno.Native.MiniAudio.MaDataSource** ppBackend, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14176) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e91293c21ca44dbe"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInitFile.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14176) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b2853d8c666d83e7"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInitFile.Invoke(void* pUserData, byte* pFilePath, Inno.Native.MiniAudio.MaDecodingBackendConfig* pConfig, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, Inno.Native.MiniAudio.MaDataSource** ppBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14176) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aad1dc9adf3f654e"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInitFile(void* pUserData, byte* pFilePath, Inno.Native.MiniAudio.MaDecodingBackendConfig* pConfig, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, Inno.Native.MiniAudio.MaDataSource** ppBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14176) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnInitFileW</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnInitFileW 67ed4dcf137c193b8d99e33f329a2695cd2fd557ce308721b88e894fbce89b56 -->
<!-- api: M:Inno.Native.MiniAudio.OnInitFileW.#ctor(System.Object,System.IntPtr) cba59d7082b52656c82f3b8b83e629382a93b3178ec6c9bea4c1104fa751c57d -->
<!-- api: M:Inno.Native.MiniAudio.OnInitFileW.Invoke(System.Void*,System.UInt16*,Inno.Native.MiniAudio.MaDecodingBackendConfig*,Inno.Native.MiniAudio.MaAllocationCallbacks*,Inno.Native.MiniAudio.MaDataSource**) 612475381666163a07c53e713454c7929e2e4a196af382ddfde24040adb79648 -->
<!-- api: M:Inno.Native.MiniAudio.OnInitFileW.BeginInvoke(System.Void*,System.UInt16*,Inno.Native.MiniAudio.MaDecodingBackendConfig*,Inno.Native.MiniAudio.MaAllocationCallbacks*,Inno.Native.MiniAudio.MaDataSource**,System.AsyncCallback,System.Object) 951a301a39e0bf7f3a0356027130d30f2405ecad081566154480d0157e1bab5a -->
<!-- api: M:Inno.Native.MiniAudio.OnInitFileW.EndInvoke(System.IAsyncResult) bd7546a5ea59ec664979cccda9efb6ce92a7cac97fab8e88afd4b460ff632888 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-31fed481e8dbfcff"></a>[`public Inno.Native.MiniAudio.OnInitFileW.OnInitFileW(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14181) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2965bf6b67ae38b3"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnInitFileW.BeginInvoke(void* pUserData, ushort* pFilePath, Inno.Native.MiniAudio.MaDecodingBackendConfig* pConfig, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, Inno.Native.MiniAudio.MaDataSource** ppBackend, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14181) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0d32a4d60d9bc959"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInitFileW.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14181) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-71a74676a795558e"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInitFileW.Invoke(void* pUserData, ushort* pFilePath, Inno.Native.MiniAudio.MaDecodingBackendConfig* pConfig, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, Inno.Native.MiniAudio.MaDataSource** ppBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14181) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4849e3974ec2445d"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInitFileW(void* pUserData, ushort* pFilePath, Inno.Native.MiniAudio.MaDecodingBackendConfig* pConfig, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, Inno.Native.MiniAudio.MaDataSource** ppBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14181) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnInitMemory</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnInitMemory 59c68f1bff7f25571572f711d5d49e8c7c002bd6ea754c948e3bc30ba623da09 -->
<!-- api: M:Inno.Native.MiniAudio.OnInitMemory.#ctor(System.Object,System.IntPtr) 22872491f561aaaa78c1a218545e4720ea13c02c7753821b4a3ae92cec23ed41 -->
<!-- api: M:Inno.Native.MiniAudio.OnInitMemory.Invoke(System.Void*,System.Void*,System.UIntPtr,Inno.Native.MiniAudio.MaDecodingBackendConfig*,Inno.Native.MiniAudio.MaAllocationCallbacks*,Inno.Native.MiniAudio.MaDataSource**) 966834fa9263a245a53b026925c5b4cae2834d08dcf1b618362f687b0a4a6ede -->
<!-- api: M:Inno.Native.MiniAudio.OnInitMemory.BeginInvoke(System.Void*,System.Void*,System.UIntPtr,Inno.Native.MiniAudio.MaDecodingBackendConfig*,Inno.Native.MiniAudio.MaAllocationCallbacks*,Inno.Native.MiniAudio.MaDataSource**,System.AsyncCallback,System.Object) 9eadb2fce74b6ecc01adc12e493fd7a6ffb81a90debe814f478715935d516d85 -->
<!-- api: M:Inno.Native.MiniAudio.OnInitMemory.EndInvoke(System.IAsyncResult) 79570d7ba96ddce72c738a6df34d4a92e1730ac3f17aceef4116f35a4e3ebbbf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-15ee6ff3a14d15b7"></a>[`public Inno.Native.MiniAudio.OnInitMemory.OnInitMemory(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14186) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-79c7b32e6ead0e5f"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnInitMemory.BeginInvoke(void* pUserData, void* pData, nuint dataSize, Inno.Native.MiniAudio.MaDecodingBackendConfig* pConfig, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, Inno.Native.MiniAudio.MaDataSource** ppBackend, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14186) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2cf7052137658d35"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInitMemory.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14186) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-331298da8c2767da"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInitMemory.Invoke(void* pUserData, void* pData, nuint dataSize, Inno.Native.MiniAudio.MaDecodingBackendConfig* pConfig, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, Inno.Native.MiniAudio.MaDataSource** ppBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14186) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d4911267d6e5b370"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnInitMemory(void* pUserData, void* pData, nuint dataSize, Inno.Native.MiniAudio.MaDecodingBackendConfig* pConfig, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, Inno.Native.MiniAudio.MaDataSource** ppBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14186) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnLog</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnLog 9ce7317fd8af0a24c89a158aa1dc6cc666734aa73a2439ef8eeb6f9ad5cf2d8f -->
<!-- api: M:Inno.Native.MiniAudio.OnLog.#ctor(System.Object,System.IntPtr) 50b47ee1fc0b2ecf97c19dea58bf681b3eb4897485f9a91bb9bd123c5bba6cfa -->
<!-- api: M:Inno.Native.MiniAudio.OnLog.Invoke(System.Void*,System.UInt32,System.Byte*) 997511d2fafbfa8c1642a7a59711f2cbfe09c08352efa49c7e3c793e3e05874a -->
<!-- api: M:Inno.Native.MiniAudio.OnLog.BeginInvoke(System.Void*,System.UInt32,System.Byte*,System.AsyncCallback,System.Object) 5d6cbf2a3e5d1dff0da031782418099b617d40f710a0257c0dfc23398b8b36da -->
<!-- api: M:Inno.Native.MiniAudio.OnLog.EndInvoke(System.IAsyncResult) 854da6a6d73fcf25ddf960e1ab3fdf04e4a58237b54839471b3daa4277c6aca6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2497babdadfd3f6b"></a>[`public Inno.Native.MiniAudio.OnLog.OnLog(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13881) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ee3df4afd02cc8d9"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnLog.BeginInvoke(void* pUserData, uint level, byte* pMessage, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13881) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c76206b6309eba69"></a>[`public virtual void Inno.Native.MiniAudio.OnLog.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13881) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5bb1411e5f83cae9"></a>[`public virtual void Inno.Native.MiniAudio.OnLog.Invoke(void* pUserData, uint level, byte* pMessage)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13881) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-01ae9037646c3cc2"></a>[`public delegate void Inno.Native.MiniAudio.OnLog(void* pUserData, uint level, byte* pMessage)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13881) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnMalloc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnMalloc aeba488b5520d0190416e35e3db91e3f7465b97c8744b4c559fa51a760603105 -->
<!-- api: M:Inno.Native.MiniAudio.OnMalloc.#ctor(System.Object,System.IntPtr) 9061423af3c5e0a9bbe09aa24db9dbc22e457c29f1954736a5b8df8ca839d3d0 -->
<!-- api: M:Inno.Native.MiniAudio.OnMalloc.Invoke(System.UInt64,System.Void*) 1b5c8ecca3af1f973167e9b6fbe597d9ba213c412289c7427b12e97c0e4e58a1 -->
<!-- api: M:Inno.Native.MiniAudio.OnMalloc.BeginInvoke(System.UInt64,System.Void*,System.AsyncCallback,System.Object) a0504286f71fbee64a9ad9b81db1382ba5b8b35adcc4157068f0776c35443a73 -->
<!-- api: M:Inno.Native.MiniAudio.OnMalloc.EndInvoke(System.IAsyncResult) a29ddde06a8b8768b4ee14ab2ed70b667ec2fb4835e9709ebd3be1774d009596 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-823cb49e220f8501"></a>[`public Inno.Native.MiniAudio.OnMalloc.OnMalloc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13886) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bb6eabd3c56d7a0d"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnMalloc.BeginInvoke(ulong sz, void* pUserData, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13886) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f33d3d100308db66"></a>[`public virtual void* Inno.Native.MiniAudio.OnMalloc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13886) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fea365243f7cc1ce"></a>[`public virtual void* Inno.Native.MiniAudio.OnMalloc.Invoke(ulong sz, void* pUserData)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13886) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5fb22e599ca22fcc"></a>[`public delegate void* Inno.Native.MiniAudio.OnMalloc(ulong sz, void* pUserData)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13886) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnNotification</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnNotification 63b06f51de2479c352b4ebc6d0cb35d77c70fa6ab4040fdef87253cf493179ca -->
<!-- api: M:Inno.Native.MiniAudio.OnNotification.#ctor(System.Object,System.IntPtr) 60ee4cf92c2a9d3e9936a811f000a794faa71e3f2ab1ee841536c86516b570df -->
<!-- api: M:Inno.Native.MiniAudio.OnNotification.Invoke(Inno.Native.MiniAudio.MaDeviceNotification*) e661f0e9a361a6128a9601f1c9d01f3ca5eb3943670c4fc98cf240d0c91af847 -->
<!-- api: M:Inno.Native.MiniAudio.OnNotification.BeginInvoke(Inno.Native.MiniAudio.MaDeviceNotification*,System.AsyncCallback,System.Object) 13cc7941b5f064f3d784d71911a9237a467022786aef0da4c4c64e184219a7cd -->
<!-- api: M:Inno.Native.MiniAudio.OnNotification.EndInvoke(System.IAsyncResult) 8a7b813d2755c56b0b3cef2f019f9234990ea0c02f8021487af63dc0eef60703 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8cbbc9fcb6b284fa"></a>[`public Inno.Native.MiniAudio.OnNotification.OnNotification(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14011) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e3c60bba9d426e54"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnNotification.BeginInvoke(Inno.Native.MiniAudio.MaDeviceNotification* pNotification, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14011) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-84e2613b8c2c564c"></a>[`public virtual void Inno.Native.MiniAudio.OnNotification.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14011) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bafde29fcd6edff0"></a>[`public virtual void Inno.Native.MiniAudio.OnNotification.Invoke(Inno.Native.MiniAudio.MaDeviceNotification* pNotification)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14011) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-06a5fe27966c7f28"></a>[`public delegate void Inno.Native.MiniAudio.OnNotification(Inno.Native.MiniAudio.MaDeviceNotification* pNotification)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14011) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnOpen</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnOpen 7ce94086a362bd8e37140c8379fc53d4da89a117722019edbf3549e809a611b4 -->
<!-- api: M:Inno.Native.MiniAudio.OnOpen.#ctor(System.Object,System.IntPtr) 8c86e2e82154259c53b6eb5d11617822c5f4189d217a30f2cc3d5a90400da038 -->
<!-- api: M:Inno.Native.MiniAudio.OnOpen.Invoke(Inno.Native.MiniAudio.MaVfs*,System.Byte*,System.UInt32,System.IntPtr*) d7c03c32b64e1f62de5dd081b82f285d94027252285535572353442d6cddef65 -->
<!-- api: M:Inno.Native.MiniAudio.OnOpen.BeginInvoke(Inno.Native.MiniAudio.MaVfs*,System.Byte*,System.UInt32,System.IntPtr*,System.AsyncCallback,System.Object) 431be9405ef6a31e85c2050632666866c341397a3fe43ce0a4c4a1666ff35759 -->
<!-- api: M:Inno.Native.MiniAudio.OnOpen.EndInvoke(System.IAsyncResult) d5276de0491a945fe63999ee154a82cd883b53f3d847cf4b27cf4d6e17ce058f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3794556c3d0b34cd"></a>[`public Inno.Native.MiniAudio.OnOpen.OnOpen(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14116) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-cf9dba83020c0c76"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnOpen.BeginInvoke(Inno.Native.MiniAudio.MaVfs* pVFS, byte* pFilePath, uint openMode, nint* pFile, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14116) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0642c8bfafa3fbb2"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnOpen.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14116) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-61a781ac0edf2a3e"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnOpen.Invoke(Inno.Native.MiniAudio.MaVfs* pVFS, byte* pFilePath, uint openMode, nint* pFile)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14116) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-78a4c9d1e8fa0310"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnOpen(Inno.Native.MiniAudio.MaVfs* pVFS, byte* pFilePath, uint openMode, nint* pFile)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14116) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnOpenW</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnOpenW bf70a9ffd55bed1d1787fc8c052b481be61ae32cb2493c69fe738ff9be8107d5 -->
<!-- api: M:Inno.Native.MiniAudio.OnOpenW.#ctor(System.Object,System.IntPtr) 8e65220979541e2d88fba10e980ebda7aa69db895a6d350c958eb8e14dce6a98 -->
<!-- api: M:Inno.Native.MiniAudio.OnOpenW.Invoke(Inno.Native.MiniAudio.MaVfs*,System.UInt16*,System.UInt32,System.IntPtr*) 0316d9853f8eb0446af74d8e85b043ede39f3452ad6288d351765259d00c736b -->
<!-- api: M:Inno.Native.MiniAudio.OnOpenW.BeginInvoke(Inno.Native.MiniAudio.MaVfs*,System.UInt16*,System.UInt32,System.IntPtr*,System.AsyncCallback,System.Object) 24d91ba7c871061ba18124d9b26b0122012772d72aeb3b35d13dcf44f4b11c82 -->
<!-- api: M:Inno.Native.MiniAudio.OnOpenW.EndInvoke(System.IAsyncResult) 07920b97d4a0289e43fbcb72d2864a79e233b7c68b8532065c585fad3aaa42fc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-455b8dd50f7bc7f9"></a>[`public Inno.Native.MiniAudio.OnOpenW.OnOpenW(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14121) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-341e5d337588b9fa"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnOpenW.BeginInvoke(Inno.Native.MiniAudio.MaVfs* pVFS, ushort* pFilePath, uint openMode, nint* pFile, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14121) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f4f5ea49e992022f"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnOpenW.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14121) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ac824ce3962a7ffb"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnOpenW.Invoke(Inno.Native.MiniAudio.MaVfs* pVFS, ushort* pFilePath, uint openMode, nint* pFile)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14121) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f7fbf338073f131d"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnOpenW(Inno.Native.MiniAudio.MaVfs* pVFS, ushort* pFilePath, uint openMode, nint* pFile)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14121) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnProcess</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnProcess d2e5b3d0646f6e99785a1cd0793147cfe9254edec5a8aa7b7a181cd9ae090f5e -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess.#ctor(System.Object,System.IntPtr) 39f14e101e55b7913b1ee461dec1ac71211ce1a78349f0a82fc91fac10fbdcd7 -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess.Invoke(System.Void*,System.Void*,System.Void*,System.UInt64*,System.Void*,System.UInt64*) bcdb55075ba8541353c5eda1290369ee18b5bd82642ce776f85c6f8c3ea66316 -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess.BeginInvoke(System.Void*,System.Void*,System.Void*,System.UInt64*,System.Void*,System.UInt64*,System.AsyncCallback,System.Object) 4d51cbd5ab349b18fcd549e9b621946d7c16865fa81b8529ffa2b37169826c70 -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess.EndInvoke(System.IAsyncResult) fad7e6cb1480763f07a7669ad2b0a8a89c5e2c7d5149921a5462603da2250e18 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ab148f72daff5692"></a>[`public Inno.Native.MiniAudio.OnProcess.OnProcess(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13971) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7dc0d55ce25cb99f"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnProcess.BeginInvoke(void* pUserData, void* pBackend, void* pFramesIn, ulong* pFrameCountIn, void* pFramesOut, ulong* pFrameCountOut, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13971) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-77752c5d741e70f1"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnProcess.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13971) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7666e4f79945a51b"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnProcess.Invoke(void* pUserData, void* pBackend, void* pFramesIn, ulong* pFrameCountIn, void* pFramesOut, ulong* pFrameCountOut)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13971) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2e3811ca185b0dd9"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnProcess(void* pUserData, void* pBackend, void* pFramesIn, ulong* pFrameCountIn, void* pFramesOut, ulong* pFrameCountOut)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13971) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnProcess1</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnProcess1 977f3b79a753690b3ef9ff09fb21960a1b71b75840e4e8ba040fa65c9b58c16b -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess1.#ctor(System.Object,System.IntPtr) c6fb32787af3e63dc0c41399c755a6873cbb75f100c2bd29e5d6c391cecedb7e -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess1.Invoke(System.Void*,System.Single**,System.UInt32*,System.Single**,System.UInt32*) 8f7d141b9de805b625ce9f47253c5d3bf9b500bcdf4da0c7512bd6918d74b858 -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess1.BeginInvoke(System.Void*,System.Single**,System.UInt32*,System.Single**,System.UInt32*,System.AsyncCallback,System.Object) 57083aed2e0041091f9e374eef82d9787f0c56d2ecf51369ca1c2bc01508737e -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess1.EndInvoke(System.IAsyncResult) 591f3909dc12ca8409b5ea5ab39901f147d28fa5467e436b601a309a97f52e45 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-afa106c6356517a7"></a>[`public Inno.Native.MiniAudio.OnProcess1.OnProcess1(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14276) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-299dd472b0f46a4c"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnProcess1.BeginInvoke(void* pNode, float** ppFramesIn, uint* pFrameCountIn, float** ppFramesOut, uint* pFrameCountOut, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14276) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a3941714293b1660"></a>[`public virtual void Inno.Native.MiniAudio.OnProcess1.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14276) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8b104ff471784a3d"></a>[`public virtual void Inno.Native.MiniAudio.OnProcess1.Invoke(void* pNode, float** ppFramesIn, uint* pFrameCountIn, float** ppFramesOut, uint* pFrameCountOut)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14276) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-11412297b6ea6c98"></a>[`public delegate void Inno.Native.MiniAudio.OnProcess1(void* pNode, float** ppFramesIn, uint* pFrameCountIn, float** ppFramesOut, uint* pFrameCountOut)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14276) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnProcess2</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnProcess2 e39379aa82818469784f1fbb25fceca766a06ea732d3833078e08ff3c5862427 -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess2.#ctor(System.Object,System.IntPtr) 0f5fb57b8da1c718cb7a40d52a6c469b0a74b57d7360b006359e33f579adf034 -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess2.Invoke(System.Void*,System.Single*,System.UInt64) 405e7207a180c665dffc4475f3988aa4a67a80036b1665575ce4f6232cf52a47 -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess2.BeginInvoke(System.Void*,System.Single*,System.UInt64,System.AsyncCallback,System.Object) 9326d79178948c8592c4856858e2a5d19fbc8c938da84c22e857028fc1282f54 -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess2.EndInvoke(System.IAsyncResult) e529abb85dcac0f49694a8905f07d02771ef806e9a1f71ab993bd9bee58ae070 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-33aacb7f27a24dcd"></a>[`public Inno.Native.MiniAudio.OnProcess2.OnProcess2(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14301) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7491ed3b98f4af7a"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnProcess2.BeginInvoke(void* pUserData, float* pFramesOut, ulong frameCount, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14301) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-479e357851bbf422"></a>[`public virtual void Inno.Native.MiniAudio.OnProcess2.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14301) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8bc560ad49f55a27"></a>[`public virtual void Inno.Native.MiniAudio.OnProcess2.Invoke(void* pUserData, float* pFramesOut, ulong frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14301) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d24d003211a409d7"></a>[`public delegate void Inno.Native.MiniAudio.OnProcess2(void* pUserData, float* pFramesOut, ulong frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14301) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnProcess3</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnProcess3 8ba20a75bc7ca231e9ed99409fe1e8e6e77a7f7f93496cd2584e4698ac04078a -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess3.#ctor(System.Object,System.IntPtr) b68d2221721f3b1bc9e7f5bcdeb91ded0a9ca598f11ba3f4e3fda3115667f2a4 -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess3.Invoke(System.Void*,System.Single*,System.UInt64) d0d9a8ea3a3a522225870533e84b3586621da7508d8260f2af204d99fcad0ad1 -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess3.BeginInvoke(System.Void*,System.Single*,System.UInt64,System.AsyncCallback,System.Object) a6fa8fb167f9c8484d37c40df9be76a8f22334c0a0ee7f00e80a2df0c0542ab4 -->
<!-- api: M:Inno.Native.MiniAudio.OnProcess3.EndInvoke(System.IAsyncResult) 58ba714ba94144ab47cfacb2145d3abec46bfb92bd17f0a63d3a6a60f50d7aa1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-11939e5177ffe315"></a>[`public Inno.Native.MiniAudio.OnProcess3.OnProcess3(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14321) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3fc22a91d07b38e9"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnProcess3.BeginInvoke(void* pUserData, float* pFramesOut, ulong frameCount, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14321) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-45592bd11691d9a4"></a>[`public virtual void Inno.Native.MiniAudio.OnProcess3.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14321) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f28f70c5a9d09d99"></a>[`public virtual void Inno.Native.MiniAudio.OnProcess3.Invoke(void* pUserData, float* pFramesOut, ulong frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14321) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5d3451cd1c31674b"></a>[`public delegate void Inno.Native.MiniAudio.OnProcess3(void* pUserData, float* pFramesOut, ulong frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14321) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnRead</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnRead 47b58d274da70a07c4c14b299c4e6cfa1d2d29f219bf720c3ea94f8c9ff6d183 -->
<!-- api: M:Inno.Native.MiniAudio.OnRead.#ctor(System.Object,System.IntPtr) aba052c3c375a160a1d443d5c30a3914f671765fbeec1f37efd46289a93d649e -->
<!-- api: M:Inno.Native.MiniAudio.OnRead.Invoke(System.Void*,System.Void*,System.UInt64,System.UInt64*) 4d116482d0cc6af2f5c08c364160d8ca2a232b9e5e7d0c6a53d4b1392766368b -->
<!-- api: M:Inno.Native.MiniAudio.OnRead.BeginInvoke(System.Void*,System.Void*,System.UInt64,System.UInt64*,System.AsyncCallback,System.Object) a72266d58277c49a4973ee19afc7fb3a158ba0c261eb756be07b7c9ff9275be2 -->
<!-- api: M:Inno.Native.MiniAudio.OnRead.EndInvoke(System.IAsyncResult) 1ad17f36cf5ff3a903aa6916e5a9e6a765727af79a9b5c81caf27e3be8cf89e1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-246e10b2930f94d3"></a>[`public Inno.Native.MiniAudio.OnRead.OnRead(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13916) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a91e1b8d82bcb00c"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnRead.BeginInvoke(void* pDataSource, void* pFramesOut, ulong frameCount, ulong* pFramesRead, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13916) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9009d8303b09bec0"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnRead.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13916) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4cbc03acec962f0a"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnRead.Invoke(void* pDataSource, void* pFramesOut, ulong frameCount, ulong* pFramesRead)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13916) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-91fdf942625b31af"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnRead(void* pDataSource, void* pFramesOut, ulong frameCount, ulong* pFramesRead)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13916) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnRead1</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnRead1 1830f3ac188b14bc3a5882e2efa03e5abd3e4eacf11fcf92b5282215586ff276 -->
<!-- api: M:Inno.Native.MiniAudio.OnRead1.#ctor(System.Object,System.IntPtr) 1185c387b5e32274024ead5bb3d72b4e8bfc5bf748b8d6299097de302594c22d -->
<!-- api: M:Inno.Native.MiniAudio.OnRead1.Invoke(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,System.Void*,System.UIntPtr,System.UIntPtr*) fc87bf56deed01489c35c319313d0127b999632489fe92e4e7abad83abc51702 -->
<!-- api: M:Inno.Native.MiniAudio.OnRead1.BeginInvoke(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,System.Void*,System.UIntPtr,System.UIntPtr*,System.AsyncCallback,System.Object) 97ddd81bd01b7e776e3ef0be1278e3eae65cc8e67d8a678c7cf5d1d2ea763cb6 -->
<!-- api: M:Inno.Native.MiniAudio.OnRead1.EndInvoke(System.IAsyncResult) aaecdc1eb5149d386f957ba830b6209eb26d54611fc1f93e757e2a2622a71e7a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f541dc5c470f62a7"></a>[`public Inno.Native.MiniAudio.OnRead1.OnRead1(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14131) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2ebf2b70f427e764"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnRead1.BeginInvoke(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, void* pDst, nuint sizeInBytes, nuint* pBytesRead, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14131) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-967c8c8f6bf2d32c"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnRead1.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14131) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-03c0bbaa428bb197"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnRead1.Invoke(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, void* pDst, nuint sizeInBytes, nuint* pBytesRead)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14131) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d253b026bc3ddd0a"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnRead1(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, void* pDst, nuint sizeInBytes, nuint* pBytesRead)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14131) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnRead2</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnRead2 63d5f594292bc66dbca2f042d4826cd9ec002299b6a24496faeb5d383fc75c02 -->
<!-- api: M:Inno.Native.MiniAudio.OnRead2.#ctor(System.Object,System.IntPtr) d984c8b7479c5bc081252fa11a3f3a8f699d4d027ba6d3865da6277fc1850260 -->
<!-- api: M:Inno.Native.MiniAudio.OnRead2.Invoke(Inno.Native.MiniAudio.MaDecoder*,System.Void*,System.UIntPtr,System.UIntPtr*) cfe4a72428636e8a21a4d52d2b088513a798a135124fd49d68b4896495f0b1c4 -->
<!-- api: M:Inno.Native.MiniAudio.OnRead2.BeginInvoke(Inno.Native.MiniAudio.MaDecoder*,System.Void*,System.UIntPtr,System.UIntPtr*,System.AsyncCallback,System.Object) 17809884f53710294f1379f92a83bce1e14ba371f601d6094f4753af9ec5e7c2 -->
<!-- api: M:Inno.Native.MiniAudio.OnRead2.EndInvoke(System.IAsyncResult) 5b97f0817f08e79730be7d3438710d2d20703a4c23375f2678a965c0278facae -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-eb493e96a19ca1b6"></a>[`public Inno.Native.MiniAudio.OnRead2.OnRead2(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14211) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b31b18c29263180c"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnRead2.BeginInvoke(Inno.Native.MiniAudio.MaDecoder* pDecoder, void* pBufferOut, nuint bytesToRead, nuint* pBytesRead, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14211) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4a92568363ada9bf"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnRead2.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14211) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-31c13aae1a13132b"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnRead2.Invoke(Inno.Native.MiniAudio.MaDecoder* pDecoder, void* pBufferOut, nuint bytesToRead, nuint* pBytesRead)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14211) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d94d76e1f13cebcd"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnRead2(Inno.Native.MiniAudio.MaDecoder* pDecoder, void* pBufferOut, nuint bytesToRead, nuint* pBytesRead)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14211) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnRealloc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnRealloc c6e37115d5d242c9c208a6920b495f657feec9c73d8632db5f8e01346567b7b5 -->
<!-- api: M:Inno.Native.MiniAudio.OnRealloc.#ctor(System.Object,System.IntPtr) 360e55e5e54e0104582ca718c5786bc24517cd60c128bdf2c1bf74200e8a5f8d -->
<!-- api: M:Inno.Native.MiniAudio.OnRealloc.Invoke(System.Void*,System.UInt64,System.Void*) d37a2d195868e4acaf2e366d7cba62ee92ca7234b89ce670d3df17e6c68ae315 -->
<!-- api: M:Inno.Native.MiniAudio.OnRealloc.BeginInvoke(System.Void*,System.UInt64,System.Void*,System.AsyncCallback,System.Object) 4ac863637c698feba2110083dfe157bd2dd07901eaac33c0c74561ce5cb86058 -->
<!-- api: M:Inno.Native.MiniAudio.OnRealloc.EndInvoke(System.IAsyncResult) ac45bdc0d2b6d698295fef5139ab45f0fc6dbbd9207a604173094b9ac8b2f8d6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5ca9cea2f90922e2"></a>[`public Inno.Native.MiniAudio.OnRealloc.OnRealloc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13891) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-adaaff9401caa844"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnRealloc.BeginInvoke(void* p, ulong sz, void* pUserData, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13891) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-463ba0fe4192c111"></a>[`public virtual void* Inno.Native.MiniAudio.OnRealloc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13891) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4380c4f47ac18ad7"></a>[`public virtual void* Inno.Native.MiniAudio.OnRealloc.Invoke(void* p, ulong sz, void* pUserData)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13891) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b9b0c9b2a0d08645"></a>[`public delegate void* Inno.Native.MiniAudio.OnRealloc(void* p, ulong sz, void* pUserData)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13891) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnReset</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnReset 68204f479e0829b7a96da173b4c18e78cc9bfc30bdd042faab176d7d8199f82b -->
<!-- api: M:Inno.Native.MiniAudio.OnReset.#ctor(System.Object,System.IntPtr) 7165ababb39a1ea8d695fcd8a6a8ece2f0b81255e0903c6a606cce0fcff2da4b -->
<!-- api: M:Inno.Native.MiniAudio.OnReset.Invoke(System.Void*,System.Void*) 56a218500c3dca593296670d343fdc24c3e7f5b7519b8b9c4525fe88b76edbdb -->
<!-- api: M:Inno.Native.MiniAudio.OnReset.BeginInvoke(System.Void*,System.Void*,System.AsyncCallback,System.Object) 3087e957cbb570e7b55a4bff1faf1da553be6bdb6c296bcdc1f0ca35d4b6c52b -->
<!-- api: M:Inno.Native.MiniAudio.OnReset.EndInvoke(System.IAsyncResult) 771dad9ff62ca6366712201cd84384d96d735c94934623a9e03637ab543dfd27 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-68cc6ec751cd1d01"></a>[`public Inno.Native.MiniAudio.OnReset.OnReset(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14001) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-dbbc377cbd9773e8"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnReset.BeginInvoke(void* pUserData, void* pBackend, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14001) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c795d35d4646c619"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnReset.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14001) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d6b3176cee03b667"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnReset.Invoke(void* pUserData, void* pBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14001) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bc72d071da7554d9"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnReset(void* pUserData, void* pBackend)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14001) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnSeek</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnSeek 4a6181fefc6b930a6a3a52396f3f842a2b2e11b17843c04942e6d06f48cda3d3 -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek.#ctor(System.Object,System.IntPtr) 3eb2e52bfb75ee401d92d65f685a3541204e5a3e6c89a870f587c578d851c796 -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek.Invoke(System.Void*,System.UInt64) 8d9af5f495eeb592fc18c83bdc63f549a154ffa101b473c20707d3ab04998651 -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek.BeginInvoke(System.Void*,System.UInt64,System.AsyncCallback,System.Object) cae90916113f18b85c5918d1fb665055d71e68c4dc95feb39880da4c056ccaed -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek.EndInvoke(System.IAsyncResult) 0e4e95a6ea300c0d816762f42019195bcd024429f43f2dfb540c38a9eda4b201 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-90b835c19e2eac4e"></a>[`public Inno.Native.MiniAudio.OnSeek.OnSeek(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13921) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b5124d21eaad5231"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnSeek.BeginInvoke(void* pDataSource, ulong frameIndex, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13921) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ecda46d788fef2b8"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSeek.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13921) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-70ddc0ecdceea664"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSeek.Invoke(void* pDataSource, ulong frameIndex)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13921) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8c2998d7eab22af0"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSeek(void* pDataSource, ulong frameIndex)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13921) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnSeek1</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnSeek1 e0856e8faaf95cdc79109e0d80986c6f741b5be392b880a45180f11d85966d6f -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek1.#ctor(System.Object,System.IntPtr) b62a543cb768e44d695161913d2724cdbd70db6f573de7f9806020cba256cdb3 -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek1.Invoke(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,System.Int64,Inno.Native.MiniAudio.MaSeekOrigin) 90f566073e488a390ddf2843419198c802a991cb4258690cf7125dcd722bbbd5 -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek1.BeginInvoke(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,System.Int64,Inno.Native.MiniAudio.MaSeekOrigin,System.AsyncCallback,System.Object) 1466bf3cc089b00dda01a4bece9a445a50c9c759d2a09efb88a0a8c717673289 -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek1.EndInvoke(System.IAsyncResult) 7f1f3a7689c2af8877c1c63b70ce5ac5c2c9fc91f594b720e88b93065f6270a3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ab955027da46f681"></a>[`public Inno.Native.MiniAudio.OnSeek1.OnSeek1(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14141) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4aa48f4ab0e81b1c"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnSeek1.BeginInvoke(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14141) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-089674d08cb0297c"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSeek1.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14141) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b0803fc775943052"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSeek1.Invoke(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14141) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7d698b93b3b44efb"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSeek1(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14141) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnSeek2</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnSeek2 b7dd06a5b61b2cd9bb9c4f2d65a95c20b79560566ec0c91401e4bb01c9490963 -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek2.#ctor(System.Object,System.IntPtr) ed64cd30503f7d0c1bdee3c31e086272654b7056e864e2241868174e566c4b97 -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek2.Invoke(Inno.Native.MiniAudio.MaDecoder*,System.Int64,Inno.Native.MiniAudio.MaSeekOrigin) fc983351fa387436b36d9226ae0a6095db354bc25bb4236107a94589141f7a70 -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek2.BeginInvoke(Inno.Native.MiniAudio.MaDecoder*,System.Int64,Inno.Native.MiniAudio.MaSeekOrigin,System.AsyncCallback,System.Object) a1a2e43dcceb629f2be2d4cec2b177f8f986cdc7b8d840c17c906a7dcfa3936c -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek2.EndInvoke(System.IAsyncResult) 3fb6fa589285bacac96e857842f27d8e252ef337eb0ac22c0c00a4b1e0961fcf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-55d7c0a364bdbf9c"></a>[`public Inno.Native.MiniAudio.OnSeek2.OnSeek2(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14216) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3f7ff61f4c22c77d"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnSeek2.BeginInvoke(Inno.Native.MiniAudio.MaDecoder* pDecoder, long byteOffset, Inno.Native.MiniAudio.MaSeekOrigin origin, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14216) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8916dc491bc73945"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSeek2.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14216) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-55f53f9bc21a1fa1"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSeek2.Invoke(Inno.Native.MiniAudio.MaDecoder* pDecoder, long byteOffset, Inno.Native.MiniAudio.MaSeekOrigin origin)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14216) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-871ca74c6e3beda0"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSeek2(Inno.Native.MiniAudio.MaDecoder* pDecoder, long byteOffset, Inno.Native.MiniAudio.MaSeekOrigin origin)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14216) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnSeek3</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnSeek3 385bf70eb7ad8f44626809e4b455a179da488926398d6f5798ccfb7685c0a2fb -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek3.#ctor(System.Object,System.IntPtr) 34824a3df2412d242a60841a074f13e81efef4f1f1b507c3ffa4a3f639f1612a -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek3.Invoke(Inno.Native.MiniAudio.MaEncoder*,System.Int64,Inno.Native.MiniAudio.MaSeekOrigin) cead464e3b93676d387a78dbb52cd636893073da1e39a54b76dd473478d82a10 -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek3.BeginInvoke(Inno.Native.MiniAudio.MaEncoder*,System.Int64,Inno.Native.MiniAudio.MaSeekOrigin,System.AsyncCallback,System.Object) b8808c04dc27904ca6a1708ced8c0e8bfff12b5649776773445fb0be66ac1aff -->
<!-- api: M:Inno.Native.MiniAudio.OnSeek3.EndInvoke(System.IAsyncResult) 4482d88a722cf86f1bbf8ee53d883aabb8d8dd0dfc0fb8f9ea86a4ba47cf7790 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4b03caf5362dfdaf"></a>[`public Inno.Native.MiniAudio.OnSeek3.OnSeek3(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14256) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a69208d6cffc5709"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnSeek3.BeginInvoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14256) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-06e3c8b09df06128"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSeek3.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14256) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-17a95e301b13c17c"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSeek3.Invoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14256) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-57e8032bcac7ff22"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSeek3(Inno.Native.MiniAudio.MaEncoder* pEncoder, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14256) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnSetLooping</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnSetLooping 3d6bb811414bd25a35773dec67e6bb054f91336de9cbe3f75837e292c91b27ac -->
<!-- api: M:Inno.Native.MiniAudio.OnSetLooping.#ctor(System.Object,System.IntPtr) 405c40248c84eb17343f90721618f45dd4fe13a640df0bbc143bdfc140954a13 -->
<!-- api: M:Inno.Native.MiniAudio.OnSetLooping.Invoke(System.Void*,System.UInt32) a46cfeb21574d3c41d8c9f97c286500cd18119714fdd10219cc6272314d09518 -->
<!-- api: M:Inno.Native.MiniAudio.OnSetLooping.BeginInvoke(System.Void*,System.UInt32,System.AsyncCallback,System.Object) 9b26dc65a166ef4a3cb84dd5c5ceecee7d43956f45589673dd585aa1e424f374 -->
<!-- api: M:Inno.Native.MiniAudio.OnSetLooping.EndInvoke(System.IAsyncResult) 5d6ef08a2d5da4e0f735aa72110bac412b451f1101590c467f31b874b3296e3b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a0cf790d668c984a"></a>[`public Inno.Native.MiniAudio.OnSetLooping.OnSetLooping(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13941) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f1c6563133514410"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnSetLooping.BeginInvoke(void* pDataSource, uint isLooping, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13941) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-14af9553f89286e7"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSetLooping.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13941) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2992c547ff22be49"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSetLooping.Invoke(void* pDataSource, uint isLooping)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13941) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1c4dd1beb8befa41"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSetLooping(void* pDataSource, uint isLooping)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13941) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnSetRate</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnSetRate 1179ff0b017fefce9395b3bf7b4a13029d252b1846609c273507f502103cee9e -->
<!-- api: M:Inno.Native.MiniAudio.OnSetRate.#ctor(System.Object,System.IntPtr) a1d276e40700ea4f2a5f8fae99c0e29853df41063f6db529c1d5eda20a164bb9 -->
<!-- api: M:Inno.Native.MiniAudio.OnSetRate.Invoke(System.Void*,System.Void*,System.UInt32,System.UInt32) bf910ebb3122db387b7faf614980a049930698feccd40ccb08b98ff0e3eb90fd -->
<!-- api: M:Inno.Native.MiniAudio.OnSetRate.BeginInvoke(System.Void*,System.Void*,System.UInt32,System.UInt32,System.AsyncCallback,System.Object) 3d3b8e5362297a20969592d5e56b3f26222d5b67dc0e05a9b698ba9fb576ae14 -->
<!-- api: M:Inno.Native.MiniAudio.OnSetRate.EndInvoke(System.IAsyncResult) ed1cace59f4784a1a36675f0f0687c4737851bd330929e5cf62405a72290dc03 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-31d929c36f467215"></a>[`public Inno.Native.MiniAudio.OnSetRate.OnSetRate(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13976) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5f483edbaba8ccd1"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnSetRate.BeginInvoke(void* pUserData, void* pBackend, uint sampleRateIn, uint sampleRateOut, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13976) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ead88f8a30f6eca1"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSetRate.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13976) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3f497199f0aaaa7f"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSetRate.Invoke(void* pUserData, void* pBackend, uint sampleRateIn, uint sampleRateOut)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13976) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0e3dd47b2084ac69"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnSetRate(void* pUserData, void* pBackend, uint sampleRateIn, uint sampleRateOut)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13976) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnSignal</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnSignal 1bef4008fc55a835b535c8087a536cdf202403dc1f83a525133a210477fa73c7 -->
<!-- api: M:Inno.Native.MiniAudio.OnSignal.#ctor(System.Object,System.IntPtr) 3d6544b50139c05861ecac271a851f6141aacbde99ae9842e680cc99ee6f17fa -->
<!-- api: M:Inno.Native.MiniAudio.OnSignal.Invoke(Inno.Native.MiniAudio.MaAsyncNotification*) f73a2d651628e38687df927b2f5bdb9f9e4d397a92f8c9819f87df1ca6eb35b9 -->
<!-- api: M:Inno.Native.MiniAudio.OnSignal.BeginInvoke(Inno.Native.MiniAudio.MaAsyncNotification*,System.AsyncCallback,System.Object) 52e34bdefdc9a42d1f3e736939dd645ae590ccff0f9670c515d679f421540ea7 -->
<!-- api: M:Inno.Native.MiniAudio.OnSignal.EndInvoke(System.IAsyncResult) 47e7f9abe93ec4294115bd48e390fb1c86377d6a07c91c31177d955249dcb810 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e3afdd4c3ff547ee"></a>[`public Inno.Native.MiniAudio.OnSignal.OnSignal(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14101) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-adf13ed3916c905c"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnSignal.BeginInvoke(Inno.Native.MiniAudio.MaAsyncNotification* pNotification, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14101) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9e5d174e2bc794eb"></a>[`public virtual void Inno.Native.MiniAudio.OnSignal.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14101) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cb8f6f0dc9f7721b"></a>[`public virtual void Inno.Native.MiniAudio.OnSignal.Invoke(Inno.Native.MiniAudio.MaAsyncNotification* pNotification)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14101) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5eb32761e09d9f0b"></a>[`public delegate void Inno.Native.MiniAudio.OnSignal(Inno.Native.MiniAudio.MaAsyncNotification* pNotification)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14101) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnStop</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnStop 84259f0a9548d89c9136f6054d1656e9f5e6cf6ecae1dd8c448c4d53d92d21dc -->
<!-- api: M:Inno.Native.MiniAudio.OnStop.#ctor(System.Object,System.IntPtr) f829d1217ad7979f3e2f55ec3bfddb313caab16b9b79cefbb914aa97d0c305e9 -->
<!-- api: M:Inno.Native.MiniAudio.OnStop.Invoke(Inno.Native.MiniAudio.MaDevice*) 7da859cfe792a004f3f82c02d20c379728d50f30d2dd1f4fecbc9cafa8dc518c -->
<!-- api: M:Inno.Native.MiniAudio.OnStop.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,System.AsyncCallback,System.Object) aa5a785cf79c59184b9c8d395582add83592f14da8d6fce4b505ad9894598bc1 -->
<!-- api: M:Inno.Native.MiniAudio.OnStop.EndInvoke(System.IAsyncResult) d40057d337fe0653c772ab9629ef3f3571305f4e856d1c35f35d532d2edfc920 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4eeb9aca7252ff45"></a>[`public Inno.Native.MiniAudio.OnStop.OnStop(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14016) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0af0959b41bbdee8"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnStop.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14016) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-830d042c7a754823"></a>[`public virtual void Inno.Native.MiniAudio.OnStop.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14016) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-116043811aa8a22b"></a>[`public virtual void Inno.Native.MiniAudio.OnStop.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14016) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-594ed19310161407"></a>[`public delegate void Inno.Native.MiniAudio.OnStop(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14016) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnTell</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnTell 0a2748a80eb821b05b75fecab76fd37725e322d655d2eea851a48808ae359431 -->
<!-- api: M:Inno.Native.MiniAudio.OnTell.#ctor(System.Object,System.IntPtr) 943410185749dab0f1bf42c3dd5179e9c25173d9cffdbe6bcde37d91ba1d76e2 -->
<!-- api: M:Inno.Native.MiniAudio.OnTell.Invoke(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,System.Int64*) b01490899882f65d3f2c6cd6562a411502c99794d9e6ee5c458a4771eecebd9a -->
<!-- api: M:Inno.Native.MiniAudio.OnTell.BeginInvoke(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,System.Int64*,System.AsyncCallback,System.Object) 4cab0079261922caedde4c1023278900a954902097b7689cbc5364e6798ef48b -->
<!-- api: M:Inno.Native.MiniAudio.OnTell.EndInvoke(System.IAsyncResult) a1ad37ab72ecae6d38a17684f99b5609c36944a2da8700949f67884a88764dd5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d691d8729160b0c6"></a>[`public Inno.Native.MiniAudio.OnTell.OnTell(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14146) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-82cf6c5c8b924c02"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnTell.BeginInvoke(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, long* pCursor, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14146) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1762ab5d0ff85a57"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnTell.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14146) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-765fc89d0b9e55e7"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnTell.Invoke(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, long* pCursor)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14146) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5a95605dec9a3f76"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnTell(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, long* pCursor)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14146) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnTell1</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnTell1 9d31fb91571c9bc24f890999db06662b2d31a289f498d5f361df685e76d168ef -->
<!-- api: M:Inno.Native.MiniAudio.OnTell1.#ctor(System.Object,System.IntPtr) ed2ce85fba9a7b454386e5d2f253ded42445e62dcb48ca96ea633a970bff1ab3 -->
<!-- api: M:Inno.Native.MiniAudio.OnTell1.Invoke(Inno.Native.MiniAudio.MaDecoder*,System.Int64*) b393e62039d9fb5594a10859c60f880e12ae346f3721940083f1f49cc847527b -->
<!-- api: M:Inno.Native.MiniAudio.OnTell1.BeginInvoke(Inno.Native.MiniAudio.MaDecoder*,System.Int64*,System.AsyncCallback,System.Object) 4eab864309408c6bd18a109506531c994c63b543046e9bf7ac84de0120f57790 -->
<!-- api: M:Inno.Native.MiniAudio.OnTell1.EndInvoke(System.IAsyncResult) 532f5628231f4d10fb76918343cc7febf05c7ad8436c34a85aace11a3981d39e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0410aeafa100deff"></a>[`public Inno.Native.MiniAudio.OnTell1.OnTell1(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14221) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f7280a6e1f1bfe8a"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnTell1.BeginInvoke(Inno.Native.MiniAudio.MaDecoder* pDecoder, long* pCursor, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14221) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-49f16d21a4352697"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnTell1.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14221) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cf1a7ff000c41d45"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnTell1.Invoke(Inno.Native.MiniAudio.MaDecoder* pDecoder, long* pCursor)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14221) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0f60b13ac292b721"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnTell1(Inno.Native.MiniAudio.MaDecoder* pDecoder, long* pCursor)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14221) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnUninit</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnUninit 5faea375729aa324cc2aef0fe114d46d1ed3332c40abe5ef1fd414d42d199d25 -->
<!-- api: M:Inno.Native.MiniAudio.OnUninit.#ctor(System.Object,System.IntPtr) aa93554f39f5be6afc38efa82057ac088307568c5d8a480b9573e2ca6dfdadc3 -->
<!-- api: M:Inno.Native.MiniAudio.OnUninit.Invoke(System.Void*,System.Void*,Inno.Native.MiniAudio.MaAllocationCallbacks*) 223c022db7c1b17a395d0b8421009d5d13a3858c75e38526e255b16248617503 -->
<!-- api: M:Inno.Native.MiniAudio.OnUninit.BeginInvoke(System.Void*,System.Void*,Inno.Native.MiniAudio.MaAllocationCallbacks*,System.AsyncCallback,System.Object) badbbf4491b6c2f502967fb149a45aca52c1cf0e547bab12f8f222609b71f58d -->
<!-- api: M:Inno.Native.MiniAudio.OnUninit.EndInvoke(System.IAsyncResult) 4e64c7e76b25d6331c09deacaa574e28b7bd34e4ca65b7558bfe4425d53a147b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8897eb7a0334a507"></a>[`public Inno.Native.MiniAudio.OnUninit.OnUninit(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13966) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4bcfae33793e04b5"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnUninit.BeginInvoke(void* pUserData, void* pBackend, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13966) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3cbd4dcacc47b8de"></a>[`public virtual void Inno.Native.MiniAudio.OnUninit.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13966) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c46f896a17af6aee"></a>[`public virtual void Inno.Native.MiniAudio.OnUninit.Invoke(void* pUserData, void* pBackend, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13966) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e594f35f8bdc2380"></a>[`public delegate void Inno.Native.MiniAudio.OnUninit(void* pUserData, void* pBackend, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13966) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnUninit1</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnUninit1 902b72bfd4480192c54d5f71d0dab8f2686210c21d7ab78f69f91f9a21e3f4f1 -->
<!-- api: M:Inno.Native.MiniAudio.OnUninit1.#ctor(System.Object,System.IntPtr) b4a183c156614accd1f1a901976ba0c773d460d8bb66505c6f785d27dad9dff7 -->
<!-- api: M:Inno.Native.MiniAudio.OnUninit1.Invoke(System.Void*,Inno.Native.MiniAudio.MaDataSource*,Inno.Native.MiniAudio.MaAllocationCallbacks*) 4797251d44f406a0fe718269f9f113ef038e97efc036967bad19b58b63d09dbe -->
<!-- api: M:Inno.Native.MiniAudio.OnUninit1.BeginInvoke(System.Void*,Inno.Native.MiniAudio.MaDataSource*,Inno.Native.MiniAudio.MaAllocationCallbacks*,System.AsyncCallback,System.Object) 312518839394231798bf04dbc92ff2cc5395fc4be11b697e91daf85d115d7ceb -->
<!-- api: M:Inno.Native.MiniAudio.OnUninit1.EndInvoke(System.IAsyncResult) 8df440854408445a6c12214894f1cccf385d0b3ecdef4584e79585524c3fd444 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a0e99fb3abd4e9d4"></a>[`public Inno.Native.MiniAudio.OnUninit1.OnUninit1(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14191) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bb46015ba08e7373"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnUninit1.BeginInvoke(void* pUserData, Inno.Native.MiniAudio.MaDataSource* pBackend, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14191) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-549011c02d662ff6"></a>[`public virtual void Inno.Native.MiniAudio.OnUninit1.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14191) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fa29a2b5dd5dac77"></a>[`public virtual void Inno.Native.MiniAudio.OnUninit1.Invoke(void* pUserData, Inno.Native.MiniAudio.MaDataSource* pBackend, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14191) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4149ddc9c1ea687e"></a>[`public delegate void Inno.Native.MiniAudio.OnUninit1(void* pUserData, Inno.Native.MiniAudio.MaDataSource* pBackend, Inno.Native.MiniAudio.MaAllocationCallbacks* pAllocationCallbacks)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14191) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnUninit2</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnUninit2 d3985e8e2bd41f535f846f7443f2031172921823ed3c638a7a69316471aaad70 -->
<!-- api: M:Inno.Native.MiniAudio.OnUninit2.#ctor(System.Object,System.IntPtr) ef6e12f2ae302b49ba15bf67f80a14679f8706de532d8884647971edfa098a1f -->
<!-- api: M:Inno.Native.MiniAudio.OnUninit2.Invoke(Inno.Native.MiniAudio.MaEncoder*) 8730ebc2c4cf69f95182361b2ec3126fb121c21db6c08f8d1b0033487770f766 -->
<!-- api: M:Inno.Native.MiniAudio.OnUninit2.BeginInvoke(Inno.Native.MiniAudio.MaEncoder*,System.AsyncCallback,System.Object) 573439da437b190dff233dc35f2c12c32a9c4387a66a56a156998f523b728b84 -->
<!-- api: M:Inno.Native.MiniAudio.OnUninit2.EndInvoke(System.IAsyncResult) 9bd0b4621169374c6174a65599fe49c2b3bb1a08bceb807d30c70668b24ff447 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1751922b95c27722"></a>[`public Inno.Native.MiniAudio.OnUninit2.OnUninit2(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14266) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7dea0ef6b3ed1df3"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnUninit2.BeginInvoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14266) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a5d4aaa8cf93fd4f"></a>[`public virtual void Inno.Native.MiniAudio.OnUninit2.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14266) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-909622b5cc74582e"></a>[`public virtual void Inno.Native.MiniAudio.OnUninit2.Invoke(Inno.Native.MiniAudio.MaEncoder* pEncoder)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14266) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-259d297a51178278"></a>[`public delegate void Inno.Native.MiniAudio.OnUninit2(Inno.Native.MiniAudio.MaEncoder* pEncoder)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14266) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnWrite</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnWrite d33e61d34f22cab167e60c2875c61f0c02829a5e79b12b98bf85c2bc3c2dce1b -->
<!-- api: M:Inno.Native.MiniAudio.OnWrite.#ctor(System.Object,System.IntPtr) 920ef601b3a3964ad86a2263a37978d2c21c404e3eb1bb1ef324ee6e2e17eca0 -->
<!-- api: M:Inno.Native.MiniAudio.OnWrite.Invoke(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,System.Void*,System.UIntPtr,System.UIntPtr*) 8931aa59a1c4bb95d1d72b732c9b5910cc3267ddc4795c09bd40a448f4d546fc -->
<!-- api: M:Inno.Native.MiniAudio.OnWrite.BeginInvoke(Inno.Native.MiniAudio.MaVfs*,System.IntPtr,System.Void*,System.UIntPtr,System.UIntPtr*,System.AsyncCallback,System.Object) e90ec7df1f04b848cdeed9e89a86922e5c18a886a2d6ab91953ac06e0585a831 -->
<!-- api: M:Inno.Native.MiniAudio.OnWrite.EndInvoke(System.IAsyncResult) e204a6ae528a5ced6d56f3540487901594f0dad78735c1628c712cb03acc1506 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fd91ae184f9802f7"></a>[`public Inno.Native.MiniAudio.OnWrite.OnWrite(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14136) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8217dd38a37a30db"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnWrite.BeginInvoke(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, void* pSrc, nuint sizeInBytes, nuint* pBytesWritten, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14136) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4efcd72f2748fa26"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnWrite.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14136) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9e2b16db0dcd9c1a"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnWrite.Invoke(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, void* pSrc, nuint sizeInBytes, nuint* pBytesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14136) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ac750d712f0d25cc"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnWrite(Inno.Native.MiniAudio.MaVfs* pVFS, nint file, void* pSrc, nuint sizeInBytes, nuint* pBytesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14136) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnWrite1</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnWrite1 4ef6f34926b16979b4cd37d3ca2a72bd9a76a1017af8940a3a61af42a76473f6 -->
<!-- api: M:Inno.Native.MiniAudio.OnWrite1.#ctor(System.Object,System.IntPtr) 83baed80593326ecae6c382eea9cf6dad998fde6a0c9c824651d0c7f9887bd26 -->
<!-- api: M:Inno.Native.MiniAudio.OnWrite1.Invoke(Inno.Native.MiniAudio.MaEncoder*,System.Void*,System.UIntPtr,System.UIntPtr*) 922b2d2b8fb3a61417c6520ce13732cedf59d4c9ab270278eb4d14fe43d5c3e0 -->
<!-- api: M:Inno.Native.MiniAudio.OnWrite1.BeginInvoke(Inno.Native.MiniAudio.MaEncoder*,System.Void*,System.UIntPtr,System.UIntPtr*,System.AsyncCallback,System.Object) 743fa91ea74e2e05ed8ea06a5ea1fb2dc5f9bfd65a4389bd2fa84a4b815db770 -->
<!-- api: M:Inno.Native.MiniAudio.OnWrite1.EndInvoke(System.IAsyncResult) 57365dde4d6c245e2a8bcd0b0644c9c27a1889b8f9b4970e38bcc1cb2e021dc9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b49430b47d959180"></a>[`public Inno.Native.MiniAudio.OnWrite1.OnWrite1(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14251) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a3f3de45996dd765"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnWrite1.BeginInvoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, void* pBufferIn, nuint bytesToWrite, nuint* pBytesWritten, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14251) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9558873812c93da6"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnWrite1.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14251) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ef8a767505e8c7f3"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnWrite1.Invoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, void* pBufferIn, nuint bytesToWrite, nuint* pBytesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14251) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9c48dc2a239659c7"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnWrite1(Inno.Native.MiniAudio.MaEncoder* pEncoder, void* pBufferIn, nuint bytesToWrite, nuint* pBytesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14251) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.OnWritePCMFrames</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.OnWritePCMFrames 8990c680381fdc23804026845c14f2b6807fcab528c39a75b5caf9cec855681c -->
<!-- api: M:Inno.Native.MiniAudio.OnWritePCMFrames.#ctor(System.Object,System.IntPtr) 1ee7ef4d09553edc0259c1ea77c770a5380060d38e32fc9fcf422b84aca576fd -->
<!-- api: M:Inno.Native.MiniAudio.OnWritePCMFrames.Invoke(Inno.Native.MiniAudio.MaEncoder*,System.Void*,System.UInt64,System.UInt64*) a412c6e47f1022b3e2968d5daecae911314809a9331ea7da284b87faadd8dc46 -->
<!-- api: M:Inno.Native.MiniAudio.OnWritePCMFrames.BeginInvoke(Inno.Native.MiniAudio.MaEncoder*,System.Void*,System.UInt64,System.UInt64*,System.AsyncCallback,System.Object) dc318445f3454427665ee3afa7599f5e72d8251e87f3d437c63e0e54ca9ace78 -->
<!-- api: M:Inno.Native.MiniAudio.OnWritePCMFrames.EndInvoke(System.IAsyncResult) 5fddfde69b907780b0ccf0acf05139ebc0c7ac8798f14eafd5262dee36fc569e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0a00e7455c15ee31"></a>[`public Inno.Native.MiniAudio.OnWritePCMFrames.OnWritePCMFrames(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14271) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-35e11ff6c881b607"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.OnWritePCMFrames.BeginInvoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, void* pFramesIn, ulong frameCount, ulong* pFramesWritten, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14271) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-75957b4569d79b43"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnWritePCMFrames.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14271) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-910c8e8f7d503fcd"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnWritePCMFrames.Invoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, void* pFramesIn, ulong frameCount, ulong* pFramesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14271) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-41cde6df0fe244c1"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.OnWritePCMFrames(Inno.Native.MiniAudio.MaEncoder* pEncoder, void* pFramesIn, ulong frameCount, ulong* pFramesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14271) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.Proc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.Proc 7850ca7f981156330be8886cf7c45ea58ecb7ce052ea517e9c5c74f1076d8f94 -->
<!-- api: M:Inno.Native.MiniAudio.Proc.#ctor(System.Object,System.IntPtr) 0cc0584e3b3ff3d3e7d44b757e580a78c42cae6bfdd86bb8cc555aef83de6503 -->
<!-- api: M:Inno.Native.MiniAudio.Proc.Invoke(Inno.Native.MiniAudio.MaJob*) cca366283ca2d249b401eff9300761dd1436f54d2334a40c358dceb2d3ae0046 -->
<!-- api: M:Inno.Native.MiniAudio.Proc.BeginInvoke(Inno.Native.MiniAudio.MaJob*,System.AsyncCallback,System.Object) 091e1c2c5a6ea34b34020d49a43cb47de57dbc2ee031e01521a599dcb06456dd -->
<!-- api: M:Inno.Native.MiniAudio.Proc.EndInvoke(System.IAsyncResult) 5a120fe3bb74d1c3c8b669b9732a8e83f1dff29c6c1cd2657f16a11a45e0d85c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7448e05c15eccc54"></a>[`public Inno.Native.MiniAudio.Proc.Proc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14111) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-121b5afd23784175"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.Proc.BeginInvoke(Inno.Native.MiniAudio.MaJob* pJob, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14111) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-42895b3cc7a55362"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.Proc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14111) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-647c5d738ff513b8"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.Proc.Invoke(Inno.Native.MiniAudio.MaJob* pJob)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14111) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fd2e420d7672cda3"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.Proc(Inno.Native.MiniAudio.MaJob* pJob)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14111) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.StopCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.StopCallback 6d6ce99a5b52bcad5964b8a278abe242acc0d1511c9cd754a035f6ac734a9818 -->
<!-- api: M:Inno.Native.MiniAudio.StopCallback.#ctor(System.Object,System.IntPtr) 49cc25330dcb98056770b8ecfa1bbeee51c0ab67063e48d25129908cf7c0c6e8 -->
<!-- api: M:Inno.Native.MiniAudio.StopCallback.Invoke(Inno.Native.MiniAudio.MaDevice*) db51041e6967d68c4f1b386c880a302a253aa007cfdb0eefb6dee5b6bcd91687 -->
<!-- api: M:Inno.Native.MiniAudio.StopCallback.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,System.AsyncCallback,System.Object) ee0077da86400ba5689db7e43ea924109a715227f946fe3442cc41000bb1aa96 -->
<!-- api: M:Inno.Native.MiniAudio.StopCallback.EndInvoke(System.IAsyncResult) c0d207b83d93745a1705295b2e60373ec08eddfc95735321f2350e64b5f6ca62 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8f33d0859c52f684"></a>[`public Inno.Native.MiniAudio.StopCallback.StopCallback(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14031) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9527844cb42dc4a5"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.StopCallback.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14031) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6ec6184da9823c02"></a>[`public virtual void Inno.Native.MiniAudio.StopCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14031) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-720f66dc7c252d09"></a>[`public virtual void Inno.Native.MiniAudio.StopCallback.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14031) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a8165b97dd838c5f"></a>[`public delegate void Inno.Native.MiniAudio.StopCallback(Inno.Native.MiniAudio.MaDevice* pDevice)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14031) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
