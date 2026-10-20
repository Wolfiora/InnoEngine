# Inno.Native.Sdl3 ABI · 06

[项目与生命周期](../Inno.Native.Sdl3.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.Sdl3.SDLDateTime.Year e033fe256c750f5330afbdf89e7f9a2cb28e34a22d157bf88e0bd6ff81b815de -->
<!-- api: F:Inno.Native.Sdl3.SDLDialogFileFilter.Name 199d3900c35bb4589d4f5314ea29fb123ff36bc5ad7bdd2b129c44822c46c7ea -->
<!-- api: F:Inno.Native.Sdl3.SDLDialogFileFilter.Pattern cb34c7df938c3e257411fb11d5798e861bc483f09a358c6225df14fa2f7aa9ae -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayEvent.Data1 38677fff720204bba7e6b1d9e4a71b9abfe5542b60ad94b8d3b34b4701d53036 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayEvent.Data2 4e181f90a507d4377c28a35c1d53d3895fe6c91ebb99808d74736afc62556463 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayEvent.DisplayId cab617b89d35a4c1e355502cb9eae28826bf1f92df122a5d1dec4bd954f67dd4 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayEvent.Reserved 94220dcec2560a1100a3515ede5ade704b4af0b1ff4267505ccec61d7fcf21c3 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayEvent.Timestamp a245014cbd6cbda55af380ffce80bd3429fa044f222d3595cbb45d6d5b43b2c2 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayEvent.Type 02b9f33bcdf5865ce423bfbf65edcde92d21f891bfd17b452ec0e4a3c9ac7c69 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.DisplayId cac946d0fa9fd96fa786aabfb775d86d20c28bcccaf4b1281bc3b6ee6023895c -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.Format 5b80b19a138e8dd051dbf569bfe23428581b4125d3d5c71603cc74e0c7ad9711 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.H 7076cebe1b34dc38481fe8fe916e94f3763e4eeeb58540c2ae42e206cbdd217a -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.Internal eee3287d60ac3c045bc9c89f190489da221712ad9e471d0f562bf47f7dfa576a -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.PixelDensity 7f251c513cbd0afc46890cb59cfce77128dc8c878c92d80ef3f2529c83670ed6 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.RefreshRate 450e97d55afbfae068e8e8fa1dcc158f478be709558f698686847b19a910ca14 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.RefreshRateDenominator 4f99d7e151000b5b13970fb599cd1022395ec100ed3c7b6d81bdebd9ad76c84e -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.RefreshRateNumerator 98b916eed96e65b73c93711ef306a49cec32768d2272ae54595c1e836c61b161 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.W e68865e2fdf1ebffbd7e418b83c9541218bbe2c43978083b56f973e6cbd78da3 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayOrientation.SDL_ORIENTATION_LANDSCAPE db79d35c042ee8a7b6f8d72edef7618f041e4559ff461c18a14b612f1b661349 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayOrientation.SDL_ORIENTATION_LANDSCAPE_FLIPPED 77341305fa140bc698a49ede83c5b526944d7649b23c8f42885b11ada87f2321 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayOrientation.SDL_ORIENTATION_PORTRAIT e91aee34e4bf12c84f13c44d04026ec47cc36f83dee2477aa5210d5ddf142741 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayOrientation.SDL_ORIENTATION_PORTRAIT_FLIPPED 43b94eecc2173a6d09cf50f001d2801643235088c3ed8d040e52c0eca4351819 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayOrientation.SDL_ORIENTATION_UNKNOWN 1d404bfbff8ec1703ad016cf5e2dcc56329962320827ab4e7158a3724283b8f2 -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.Data 222a03d5b10823290e698df6e408e921220da0b13ee65aa606c83cab25c7138c -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.Reserved 765dd221695891849518c94b3746c89249b6a63d2753bbba0138e72c72e5f0ae -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.Source 0d264b9d2ef9cb34b5b056da03aad6d3e46592ae905b84606f2bf01165c6992f -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.Timestamp 4a06275ecbe7c65fd17c7e7dc0bf4917c02852af5f9ffb9b5c93eabb30c50a75 -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.Type 88aeea3f57c29bd3b209302db82af5eb77d2dc067941eb417fad332f6e24c708 -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.WindowId 4afc822a06ad6296a9962481d4f902ee3d295cd13148ee87c2cfed566f79f43a -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.X 27ad44ddb08c7df2f42017a3ec547ea1746256bdfda9869c59d9ed3d6a680779 -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.Y 83d979f59fd81ff6e6fb0f923651db49559ee887516b9729aa1562c5aa7d8c9d -->
<!-- api: F:Inno.Native.Sdl3.SDLEnumerationResult.SDL_ENUM_CONTINUE b6f0018360afb94ded35edbe7a8b4c7f09d7b5e5377d9f9e00947b3eb0efdcfd -->
<!-- api: F:Inno.Native.Sdl3.SDLEnumerationResult.SDL_ENUM_FAILURE 455513f4e93dc550f1f274f39e5d36de7407da0067b9084a7ab84c7940c9ecf1 -->
<!-- api: F:Inno.Native.Sdl3.SDLEnumerationResult.SDL_ENUM_SUCCESS 00d673025ab4a606f71f97a7362c7ebd5689c708dba978a613176d15914a77e2 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Adevice 0fdb1b7988d9878a1bfc67b02417110fa141615e09f541492ecde23c25f57789 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Button 459f8815e67a6214fcb733f7706b5544d3be9ebadde92d1e8a944cdd4e956f93 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Cdevice 2ceacaa567e4d4600d9955881aacd38fba14791a2333bef05a73d087f10e12b7 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Clipboard 24d669f935db8aaff77388ffe967d00dc4df7fa397b739486f4c0ae35fb51071 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Common 1a0809d1a51456d1e769281a86afe4ec571fe2186222adb044d8e0d0d810098a -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Display bd1a41f4ea90ec6a297118e059eeae9d8fa9deebbe7d2b29b6fbeca3659b27e4 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Drop 5f97cee59c910f90933dc25b5f1ea6d104b6709989396c6d1ab5cb91fcda72ce -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Edit 4c6c5ead7bfc5789bf3d1d4587c6e7785792b932f2260e57579d74fabecb39dc -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.EditCandidates f2f31ff00801982f7ba006d50419c30c0a9e24f21030879f0ffcf8ecafa75d1a -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Gaxis 09023029d9a81a50b4213a9d6c059591b2845a2bd66e10a730b8a4375a1a22f9 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Gbutton 7d482f4ac52d1a99e186d8e4da770bf56e1eb4c33c1bbfedf50ac48d15223457 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Gdevice 70aa440b6921720a21ad387151488e0673c6e494d77a4673c6e4b6f91e6a8cd6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Gsensor 28364d8beef88f5df30ef2f91117920146d89bf4cca35809f3167515de8662b1 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Gtouchpad 5ff7f01efad56fecede988048be677fcf4da89bea4f8e92ecd244840616c8bb8 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Jaxis 51b37ca7f4d3c37bcf427e997625343db50ec67c69a615d2f4b58676535f686d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Jball 69290df68d0d577979009aa54a159b2d1764fe135f230dfe12dfb8256dccf359 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Jbattery 0f99279957181f36a0fa868de8a091c77bc4c06ae23bbd4fed7ede6aa5632d69 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Jbutton 5bc2e8169c9491d1d54c663a7045b949fb97f22eb45a699f895aebf039f10e5c -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Jdevice a04c2506eb00dd9599aac97f77367ac877d47f8130fea74c2d855421eb9bc3f7 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Jhat 4f2f4768ad9e4bd8a748f84ef35ae2707d5c2edaa06682e66d96718c45cdc961 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Kdevice f3251f179ea267fda09290bc734f6962f84a4cb3246110f9f1a64f849250c64f -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Key ee57ee6c863c822edbe5aef1fabb57e148b6e9f26ca3d7ad40f144c0214c6a2e -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Mdevice e527f94c749b267c3b45fc2725b463cb0290cd38a76e0fcfb61bcce87497bb1f -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Motion 21236ac234f1703bda8c6fd5e0202f4844947f78d0d0df8210f4ef340acff27d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding 73751ea962bdc225c9f2de00b8dddcf887040212692c98bb94b4ed8f1b97eb0c -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Paxis bb750e6fbb81f482e76578dc4161f49bb15c544d479c54c19f52f89323f1a7a1 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Pbutton cd22f263585a02d0865fd4c99daed2fd0690dcfc7b2d6cbf1adbb9f478d6f5ec -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Pinch a4dddc9fa50f33669bc1696487b887120502c330d721f9beccd96d60f9b4064d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Pmotion a4c4f8a68a3aae2ee6df8db00bea77bae50b53bf1614b3bc2da47b562c2ed67c -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Pproximity b8aad60bde3da43e73f4e52a35c2a2f1add81dc9119d33f152e4dec88f92f831 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Ptouch 0763deca418882e48d325590c4b4d57c2a513d6ce18d6f354ef2eb5f80e0b989 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Quit a563948fd9ed59fd38c18a43e0937d7edd30bcc98c53f3b2bdf8c04f7d7c3e18 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Render 005c6472b365e6f384f03f87112f231c3ae825d4b526342d56ee51521656bea5 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Sensor 1f3118859d2c77a5d929a30e1af01680849826deb4e53f4f4c72d9a7dd645e45 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Text c94b94d438e054fa0dd3e79f9e4a2442d37227d292d9b43271a0b2011ad74e2d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Tfinger 19e9e508b1087b45433c603140874a1b0dc090a9ca09aeb7f1223c19c8e25621 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Type 6b77f804de865ae79146a25f8d87aa56c734f16af8291b5ed3c9091c0aac1a24 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.User cb8a411707019d9f1b2b5353fd30887ddc4a2466ebea4991abbe452b9b8890a3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Wheel 40a85765e289593052ad17e9fffa0f335803ce8f886b8992838853f48ed9d802 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Window e5e30717af46915fcd2d3a805201871e85870b40b43c3362a52c1a6daa25a1ad -->
<!-- api: F:Inno.Native.Sdl3.SDLEventAction.SDL_ADDEVENT 61aea4d8d398246c6e8f80770c25caf1a5146f64d797893625d246e5fac45b14 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventAction.SDL_GETEVENT d1076cc91b6cbf6034236cf62e6ead29f755c601fea55d878ac389850a20b561 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventAction.SDL_PEEKEVENT b7979fa2a701db4c8c2f071e5efde5de2a7b8f042b6fc30e53628e32cb3ec95a -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.AudioDeviceAdded a6218a2ad3b4a4f0276c23efc9efea3a286892a2a56d32323921abf62260ded6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.AudioDeviceFormatChanged c53891f38069f76f9541c7da59f1bbd7f970d5a06297cfdf81765e8376135ab1 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.AudioDeviceRemoved 8ae2f67e2de82f04bcce2b304ad7ecda622df1066008ab2e7d08a4d470b9941e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.CameraDeviceAdded fb6a09cf2d9c34e8ea346ba7e47568f8ccd7a2904f6ef7647a39c61517206872 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.CameraDeviceApproved 7f0953daed687bfc301ba8afa46ec15c1af4e146a661aa24fdbda0efa450813d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.CameraDeviceDenied 05cfe613c1e3ae076790ae05f7f5488540db83eeddea9c8aad2eb0ea0783a26a -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.CameraDeviceRemoved c990ed04ca1b5b947cc90db191f60f8890431ed0d42d134fb6557353dff1a817 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.ClipboardUpdate 671e3f41d1f5731435753f7a2bddace0843248405d5351a02cc773e1ad3bf7ca -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DidEnterBackground af178551ff7bec5943033dad650aa0fe4e4ed0f1306d85114d0d78737e66d9b6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DidEnterForeground 189d7f2905336dd00bc4ebc62037336ca23391a9205e0f3d15af57086597bfac -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayAdded efd45a971042e5f70a03995afab358be201a00f7e04a8e97bd607ae6b67ac2e0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayContentScaleChanged f51aa4a0fe65e078e35eeb5beb67bd8ce73ccbc9fea17311bb34767e49f2af7c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayCurrentModeChanged 85948ea1a396d2b73ae6ca466c7e31cd7b0b3821ce4c07f22bd5bbe4d126be10 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayDesktopModeChanged 5cef4688fd782f82d77af2634dc9ea8c3fb0677a5151dbddc75b3b3fc7fb1a32 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayFirst f06aac7ef0701f6d178289ec89b88bfe1ea98b35b8b60d9ad6ca4f67890297c8 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayLast 76f55ff9b24b1eb8d80fb4928a1638da03fbba2c66df93d7efdf8e313565bb0b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayMoved d8542fa4976378b1115ec7078305f3a439cc0315c41ad97210295e94d960c352 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayOrientation 11c58d414ac8527f165958228fc904e6b2265812cc5631e7a9ab90a8033a5743 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayRemoved 83b42e460b830eb382e3ff05e082057387c189ecf4de387b9149739c06181c6e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayUsableBoundsChanged 32033683ac00822987b8b1d84fe3f8d2cf6c6ee56c65e476c84f3077867e24d9 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DropBegin 1629bf4365573fb53e7528142818dc3fc8100cbe3b63399a5eb0a5e5ec5e2b8b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DropComplete b3f6c05db9114d2753c7f57c1fd40309220ac192b1a3f97d9d13411ff96873c0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DropFile 1e3da1ce7093c576fac2d5ae50dfec097fc427cd15a35a206abf601f9496dc5f -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DropPosition fcdb783cbdfab07003749b985aefd0acfbf0f4fe92b1b41c1aa444c4d3924740 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DropText 26f8d706f95bad7ab98cc5214ddc30ce7002db5825e3bc35adf1b40c81646532 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.EnumPadding 7536db2c1e9e930453e8a49bafcfa3f194ee0ce0cc75647f1eb4406aeb613dbe -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.FingerCanceled d2e09c89ad5e239213468c922e223e575b5386e958146a1e48422221e919543a -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.FingerDown 35456a0eeb0bbabead12904dcae6480367c3601acbf3c9a7796f4f3295d966a0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.FingerMotion 4bdaa0280312f70968d9d8756f89824c2c32960418cc7635359004e08b891926 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.FingerUp 269fa0b275586a041d4a5ad02bef56dd708b85f4bf18ee28292b1c254d85257d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.First ab2c33478a6f48d5d4967de53a8b7d0d9197a071ec253977cf196fc8f6290710 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadAdded 76e44bf7a469edef60ada480f6bd84acaa8b11e77b3e38a3307ddea836ed7784 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadAxisMotion 1d29445aee1257632e58f86bf3f01abf39bd044c589ff89e60c30f9900316d82 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadButtonDown c82632fb33bed1f889730df94e7dc01cf50823881faa3601d9697842fba8b162 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadButtonUp 08ba2c6eb95d74ac75af91c11b83b131127c51eba79fa9759220b698facc21a0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadRemapped bae6b69ea495de5bfcf2e526dba87d762e04c9255743ad2c486744ed09dfb798 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadRemoved cc8d9aa2b35e63dd3a95da613bcc7dd2c4d5c8270d3defd6678ac13e282db713 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadSensorUpdate 9db023a613bf544961388c9c18cb9bcfd79acdbaacf77db8c8bd8343094a3922 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadSteamHandleUpdated 688d127fcbb2cb08c2de3715eab20f47f72b36945f7d199e84f21f138477bf46 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadTouchpadDown e1ac0dad5659b2c911454cd7ef67a3f60684fb5f4b3020c57657611cad8a9e76 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadTouchpadMotion 2d918c5bcb6328626d44719bc7c09ec7753ff44b993330341b314aac6534d425 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadTouchpadUp ec82dfd38e93ab16b8d6c270d0bd87737ccf08ca1cd391c8ab71b50a83de6bb2 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadUpdateComplete cb8fcf73aa7d1f4b73b60e7b5841bbd46780f49403c8cfc3f305ed93769bb82d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickAdded 7e901fa8e5667eeebdba24fc64699b4ff0720636629abdf061b92b6fe3d70ea3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickAxisMotion 4499bf849860d7578794f04cfb265672932c1e8b895dec6325479ddd8c90d794 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickBallMotion 9dee5548c1978e2cf9b5826c0bef86c4b46c91cf60b17671b47164e3ee23d61b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickBatteryUpdated 2effc896a4eb3a7b69fe50db3bc378aa0796d6494743f9ebc8a2d01a7e61c8ff -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickButtonDown 2c5bf424e859978c648a9a4c122ee1c8f16b9d6bb9f7bb2175148fb43f98eb8a -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickButtonUp 8ba60240e9f90d055c50fd7bc3f8294c5b021f0875d8482f56bc086218a53272 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickHatMotion be748e5fec54455d283d6a61ad3e23148bd9a105bf3f6d5794793b95a1799547 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickRemoved 1556da65effd20064f3a143f1085d395187a832edc3ce62dd127514679e4bfc3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickUpdateComplete 6f36c0bb7e3e954ee6b2604b7b4cfcc1db0a50ed2e1b8a84e4afadb57f7e7fc5 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.KeyDown 391372a7f3f4bbb98210f6b3c6c15315e5b06011c4564f8360a93e414da38484 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.KeyUp ce66e74ac2912ba080448ed57bb8c12f3d8ebfe6361ffc35aca060aa61a880b3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.KeyboardAdded 5003d44f6035089f02b000c2d3eecc949cfcfee26bdcd69ca70dec2d10a47c1b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.KeyboardRemoved 7e194031891f91a3a36701ff469da7b6301371bd078ca5bd2cc19c365f6a9a22 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.KeymapChanged a7cbd633aef960b32e72eb616d5b39dbff045a591f1ac11533feff9179162ab2 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Last 128504c5eb0afd51c702b8169b10c9b655cde6984b8a8bd13670fe6209708f2b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.LocaleChanged b7ad54d000c5f0de928108a87695aef5431d8eaac9de5a75e8b9de84bf09e2b9 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.LowMemory 90b66d2c8891cf66239e05e7a7fa04d6bd449432e4220b65e112da4c5fbd70ec -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.MouseAdded e0d66e5ca86c4c785581d2bd241daa27f1a1553083f7ad55b9d9ddedbd690bb0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.MouseButtonDown 35d68d70549ad10e47e83a5f85a48347a5c3dd641e859f37157c5572efc6766f -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.MouseButtonUp f81bd6a740327821a6071e4a781c6acc581e001bdde84bcbb304a844db73c4fb -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.MouseMotion d77a8184a08cfa524f3d064b6f2622bcf25b29df6491213516c04a03a29d8e0f -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.MouseRemoved d41579bf380c8e544a18c7125f0dcc6c9cbfa117c63b2c6a2bb67b164aef7c1c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.MouseWheel 70bbc12298c9a66b3c0ddf1b45d554483cc5d41c0e1ece8a07915bb7f4c72b1c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenAxis 46df1318235193ebf655b003d1bb590240c17ca8c76fc6c15dc580e7bd4128e6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenButtonDown d2a9522d8bd0f9d1be0f11248155ac836cbf3dd1841f825aedb048cf91d0626d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenButtonUp 08d6c5c51399493396b472e3b36be16d4635116537ecc237b928a1c2ee908ea5 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenDown d54166fc14c578c5081958716dd5298366d3790bea96599db1613559b93d938d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenMotion 17a952b57a0da73b490f9875d1a90f7a0ca97800dc574d515f88eb0e6c842e6e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenProximityIn 0501f3bf77a6c5b8a67f9278ea43a4c00b316bf1698a76efd2a60d8291e61064 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenProximityOut bbcd0f7f5cccda31c2176c40ffebc285499c60f6f6160817636c4405b8ef7521 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenUp 8af9e6dc80b97276a5f39fbb1cac115d018347113841bd22ea669723a4abc5ed -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PinchBegin 35632f495f9390812649b6d49c916f919929f4fdc6366495dfebbda5dc4b7bef -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PinchEnd 203a58b6cb75341317793f70aafaf79f4038cac51613dba2ad85d6a301c58c3e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PinchUpdate 27b70a84c5a0ddadddb8c519c8ec216ce52e4c9dc5c96becc01e206d97e125ab -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PollSentinel acb76cb79929278869701836d5c298a7e5eb611c85124ab3d3edbed27b580f8f -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Private0 a622e8e7dbe2e58afb896f555c81424e5b0f5282b086ec60b783f855ad9a3ee3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Private1 4eda21694f1e490b2bc55b952340c3ad6066fdaa34b0fad5e6e987123731e42d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Private2 5e1b2c93b47ce2741b9a5d0a13feea4bea7c0a8a6870eb00513c766366cfee4e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Private3 0e336036e14dadc31dbea97a66b806012da65cd501a17ce33d557283395b0aba -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Quit bff14e11b5d4d2f0884c8733d45c07f7b9df57edd388a72058fdad71fd6f3f0c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.RenderDeviceLost eef88390962bdae7160639f5e65c4eaf86d4cdf0b04213372d8a510abf968880 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.RenderDeviceReset d42553dd578c0840759d52e7e189891a03b059a4f86ea1a0110e4f7e264e1b87 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.RenderTargetsReset 42996cca6d315ffa2f5e360cedf97272f43b62bf1565182e8e1212641cca8e5d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.ScreenKeyboardHidden 43e9e376eb9a28cd5de538128a77293f0506bca1e75777237fe9de4c13aa2ec3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.ScreenKeyboardShown 2e8bf5ff7f9fb187eb4c88bca22583199c3d61aad281c836ae1c3b585111cd70 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.SensorUpdate 2eb3d6c62f127148d5ea6d3b15c0440f678c3f1a5872dc0fd9db07055b2d4b8b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.SystemThemeChanged 3b60e310ebf51d9d38f7474fc6bc3cccc8980c6643f657ea7581eef814aef4fe -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Terminating a4ea908c3d30c0c2d93fc5afcabe081064433d0c2c41b40ad1b0d3ec21475969 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.TextEditing 9e31e2c4e3fe7751e7c1581e41f7b5aeb238641d1564a50c4787f05f25736481 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.TextEditingCandidates 65d98be5d1c3a3bc352eac42f23280affa83fc4bd9d50d5fb71009f5682c7d5a -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.TextInput 3230ff0ef48666424b605cc5038a697b45fb82bf4fa18df5c609eb0d434f3392 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.User b784fe1ed09f46c17b39ef9bd8af9a4cc4faf39ac4146fd385c43f1c91624b7f -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WillEnterBackground e549da04fe4a061070975fc28fa57d3980681ca6c76b4625de29b1b619b9c74b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WillEnterForeground 5002f97ccda0a453695033926d7dfa419e76c95ba302528c7c133c8c40a4c3cb -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowCloseRequested d0bd762f86017e9e5e9d3a762267f0843f3e2f387b36287fb0ab387696430dd0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowDestroyed 23c3a90d0f9a918273fc3067314a7469a79ed13f2eafc3364d505a60b3d61ca6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowDisplayChanged 317bb787f4e68fbaf55694b566fe511c886feafa35453f61c252772f565ccde8 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowDisplayScaleChanged 1f7f5ec23625d4ab9c17a0042a2190a940dbebce271f5fd9d51ed84ec289d33d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowEnterFullscreen 283647bff0605c894e1683ba88a527e788e33083748248caa5ff5d03eca46057 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowExposed 6f9419b488df029bc5a0ea35689b43ac86a3ac6eba672649240cebe81528c8b3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowFirst 065b6bfeb9be75ae97a85928092d5ab0b353a3b9159956282152c51eac2f9097 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowFocusGained 463ab29f1bec4bda5da0503bc8601543d07c50d7b23c068fc831152ec88d7ea1 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowFocusLost 2fb4f5971b97a62058fe7ae0e58cf204a039b7ebdc6f09b3ba8991fab5fd628c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowHdrStateChanged 61c2f7be1a9388f5a0688befa8e5397d7766de5a9bc4f63595606dcf2dfb313b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowHidden 2d3d8975c0332883fae3cfed28ce9eba7bbe64a301c3aa133287e1cf93139518 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowHitTest 8bd7bdc16ebf14fb4d87695bd75f70a69470c6b950ddcce67474b97599891244 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowIccprofChanged cac0ff912f71d66c3d50e0b6ef81370245f7e632c12e8a8bc90377e529ee170e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowLast 7a152767088144955136b47f67e2b434159b2330c6155f27248fd25b8ad278ac -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowLeaveFullscreen 0ae7db32d40c2cdee07de5ebc048c9da50ca5d4bb2fd1af5da9c50a8869e060b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowMaximized 655aab19611ae48b5881ffd10649dce324f1d740b2e1025385e46f30ff17d8e5 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowMetalViewResized 1a0e0abdd7bca9c03566ad0d9a6288475bc52f0b86949688d63ff5e829e2f2c9 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowMinimized 215a5e7fecacaf4faeb1791423d4f82e5dc7c67e471a36694898270643bc5d6c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowMouseEnter 6a750b76a953c3f3cb2a503938f3bdd7035c50c9c9c71e307e7ad90fd74d15ae -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowMouseLeave 0f74f13f8a3698b431f3d4ab5a5345e318bb5bd99cf4ad1e85f1f01127bb5a19 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowMoved c8dbbc49beb5274ee3e1b80f75aafed561052195d57db3faa5519accc06fdc06 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowOccluded 0ffd5000c9723ad0fc92c29e619643c85e0de41df229f24951a3b24f59dc180c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowPixelSizeChanged d997e4dc46cea0d5c1f2d886f46dbb120f87d724c1eadff77416ae73db135c48 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowResized da4ceeb7d67aeb0ce04a6127f0366f102b7f485e6ac6f56d3ba022b29621c72e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowRestored 98a3d80e18027d6f98f16937a9c11f9552afa186f80a3fe0545ed17dcf64fa99 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowSafeAreaChanged a4d7c3b874ea193c9977b50a62edc8f541ab1cdab843ae231e658f33f713f233 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowShown 3f5d8f7508c3427677db05ad5e414f4429b128247aa4258e9c8d6240f633bcc4 -->
<!-- api: F:Inno.Native.Sdl3.SDLFColor.A 135caa6d92d1fa5ed8b5303d196f2ead5ae04203dc5a78effb6cd44f346d9e10 -->
<!-- api: F:Inno.Native.Sdl3.SDLFColor.B 3e8300c1206003982c043c9a5314384f74d0591c138caf95e329cdf112811957 -->
<!-- api: F:Inno.Native.Sdl3.SDLFColor.G 2652fa3d2e590010f0305de3cf96fe5a5d220be660482505d377681e0f72f1ea -->
<!-- api: F:Inno.Native.Sdl3.SDLFColor.R 0d1306de4ea93789b93966bb2a3f8490d785fa41894aea453bc85bb2f725b1b1 -->
<!-- api: F:Inno.Native.Sdl3.SDLFPoint.X 1d3d02069ec61e9283b2aa9ea05fecaca7d24ffb3c612297fd74b703b3baf987 -->
<!-- api: F:Inno.Native.Sdl3.SDLFPoint.Y 857386a0a35d60b105bcdbf03004ad5e9995bff219cc69b0d88dda7c42b0130c -->
<!-- api: F:Inno.Native.Sdl3.SDLFRect.H 6be4183472f6f0802a7aa0236685045581556512a02b1866cf1e0f04f73677de -->
<!-- api: F:Inno.Native.Sdl3.SDLFRect.W 69c7047b47018a63333e7e3febd70d6a3bf297bd4c2c3d49bda0e503a7ccd6df -->
<!-- api: F:Inno.Native.Sdl3.SDLFRect.X d517913860b9651699308758844ca213f76714ac5e31c9e4d819a1ddc22fd80c -->
<!-- api: F:Inno.Native.Sdl3.SDLFRect.Y 9baf585f80041f6bc5270c6f3c2fb63013408825171f9b84aa509c9887af86f0 -->
<!-- api: F:Inno.Native.Sdl3.SDLFileDialogType.SDL_FILEDIALOG_OPENFILE 7a8ddd2e11e881e918703acd5ee1fb32710efae2d3ea3bd96c26d1e826b4dc1b -->
<!-- api: F:Inno.Native.Sdl3.SDLFileDialogType.SDL_FILEDIALOG_OPENFOLDER 3d437261e407bd9cd35a8f6365b9f093b454d4bc6d7fbad7e6a9ec3450b72815 -->
<!-- api: F:Inno.Native.Sdl3.SDLFileDialogType.SDL_FILEDIALOG_SAVEFILE fcbd5a8b3ab3b6ad61dabea1b37d93f410a22f658c74551fa7c446ffebeda379 -->
<!-- api: F:Inno.Native.Sdl3.SDLFinger.Id 6691b060b5f88e10ba1d8b1d4bd5497d7354d8a1888afecbae2b257b4fb6d4ce -->
<!-- api: F:Inno.Native.Sdl3.SDLFinger.Pressure 56a0549407f1cebf6c5c1715e6083b7e0de076fcb0343652144f8beb037b3d28 -->
<!-- api: F:Inno.Native.Sdl3.SDLFinger.X bf2c3f768e6e94f74875413db337323e26746666056ce35be39e11e1683e80d9 -->
<!-- api: F:Inno.Native.Sdl3.SDLFinger.Y 1967afefabe666d67b7de2661595b8c6db651b6ecd125e0de7fec1cf4f9eef27 -->
<!-- api: F:Inno.Native.Sdl3.SDLFlashOperation.SDL_FLASH_BRIEFLY 2dd87b0dd5d4a3d0d281cc412c420e3dd05f41d17009d23435d58f678ced180d -->
<!-- api: F:Inno.Native.Sdl3.SDLFlashOperation.SDL_FLASH_CANCEL d6c542a31a7ece52e580385b2d968acb140241ab2382ebfc8a3bf6bf8e139625 -->
<!-- api: F:Inno.Native.Sdl3.SDLFlashOperation.SDL_FLASH_UNTIL_FOCUSED 63cf647450a3f0d28271dd47539c95fd21a070ffd886f80dbfa2839cf42a8c90 -->
<!-- api: F:Inno.Native.Sdl3.SDLFlipMode.SDL_FLIP_HORIZONTAL c8f0041168a53dde58cd426de584e8083bf6f9ea976b9b31f4d0fb0a56feac23 -->
<!-- api: F:Inno.Native.Sdl3.SDLFlipMode.SDL_FLIP_HORIZONTAL_AND_VERTICAL c0ff17165738f41244fceccc67621b65392274f223348d87a26e751b62975123 -->
<!-- api: F:Inno.Native.Sdl3.SDLFlipMode.SDL_FLIP_NONE 3e6e1cffe600cd7a8dfea5afceda5018e72a4ca6932ccbb6e1990c694e5e039b -->
<!-- api: F:Inno.Native.Sdl3.SDLFlipMode.SDL_FLIP_VERTICAL 6ab25213da819e92314eeebc126943d26720fdb26a5a35e69f9cc4aaca0649dd -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_COUNT 419b1dd3fd7d0813c80b50bd59901710e70f19c5ec5fd7dd2b750a7ef90e7e34 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_DESKTOP 08cef14bcda3f76c09f0306dad801131a62cc379e7cde7963f32e2fa1508dd20 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_DOCUMENTS 9888b141938fc6e608dd8adc3c77403c716774a3ab7ea2dce3ed7db1f92c08cb -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_DOWNLOADS de07a5e8510fbdf6de3ccde4557c447ef7a1bfc422977a95de2ce37164dcc40a -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_HOME 1eb3b9a190d6055c665ec395e12e1f3f6e4f584e5b1578c1b1217657c1e56c5f -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_MUSIC abc62e1f34f78005733dfd0bed504edd92fd81418b230343717aba8523937ffb -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_PICTURES 82a9625a17e234a116e6bad1c4b11abc482e5b2631ebcf0576a766106a05d2a4 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_PUBLICSHARE 658987e090782ceb5d89d39e4821e6ea0222f3db2b066b50bcedef24b31436b1 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_SAVEDGAMES 29856cf5f48de82042f7f666f8e04537faec4b480fb72e749ab2a99de17b8a6b -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_SCREENSHOTS dfc5d6cb1bfdec76e5e1ce1ad4686ff8e5fb6be17fae50c8b7ca21fc2fcbec56 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_TEMPLATES 036375206c6c98a7e1605a0ae1b6567b87c5e330b5b988319997f1ff3d317ecc -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_VIDEOS 151712a6a010bb932e43daa7237d10fe85319623b069a287122f2ff533a7a7dd -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_ACCELERATED_VISUAL 975ad5d255862278cc7abf8bdf46f7ac66db6f6bd697711fa58cf25db08d16b9 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_ACCUM_ALPHA_SIZE a90789e437d9e9974aa285ceb3a3e893ae6dd47793b6047f05f4815d536ba733 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_ACCUM_BLUE_SIZE 8b306fc4e2786c08fbba9b8bd8d5ab0b0a997c8047224169202df25e3071078b -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_ACCUM_GREEN_SIZE b50277bd4607b409091aa9624c4a3d26d79079c6c21d0c978ed1361b249cb9d8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_ACCUM_RED_SIZE 8483405022d29044811b906ecb655ad9815e0c9852231d1389a714ba4489a941 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_ALPHA_SIZE afd0cca188d050f3cdc2c46c81f4945f8ec6b19085d990399f62a728c806d9cb -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_BLUE_SIZE 43c681376e46caf6f5a5e0a9a1f35637ad3d16acb59f19d00dab54146fafe90a -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_BUFFER_SIZE e23659789df65b094e5174c4411ecb4940ed5a5cbe734134df19c48c3c196117 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_FLAGS ec3be63b56836313795d7263511395d5e6161e60c7d816279d7071d0f451c748 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_MAJOR_VERSION c1b52b8ebcc2d5ae288643fd267790d26480c5f6455cec1dd5b19e1e8f4c6246 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_MINOR_VERSION c7e7135e55df6be66c30cb0cd3ae4b93aefb82f8087c2a28b4943bf7e5bb0f21 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_NO_ERROR 944b78dfb06fa57b2345a1405031b3b75e6e85d280df9f0e7af7688e65521fcc -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_PROFILE_MASK eb5f5015eae8ea521bee1e5dbfd8ed1a938b499e8c2a3db43525b69b0fdf572e -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_RELEASE_BEHAVIOR a1f1c8148200f9f0280c7677067615ec11bb61b4f3530958bfe8578bab17b0c5 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_RESET_NOTIFICATION 72ba74efc63eafe933141f542cab6d076c28bdf0745da3556b30249bfb756b9c -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_DEPTH_SIZE 95b3158776bb72b97e4cc8c9a31e9a9f718b2d02228047aae52fe0098b3fe1bd -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_DOUBLEBUFFER 88dc553611dfed27d74a1c890ac8b8cea9cca2e136ea822d3ed5318c7b3feca4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_EGL_PLATFORM 542ee1c1bc4e4e1b013c2cf2178a83646e69d166f710ad838ba9743859b07665 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_FLOATBUFFERS 8c7a8702f451482a901762a1abdc3efdf03f912561a3b703e7a9e72d2e6504f8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_FRAMEBUFFER_SRGB_CAPABLE e5115d28e95e25216f6512ab3e136e5a873d07c78a40325663380b783230454a -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_GREEN_SIZE 5603ff87bdda225bee66f59e05b70a2b74812708c2e6c3ce000e34a8cfc429c0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_MULTISAMPLEBUFFERS 117a83edc745ad10c525ef07e3accbe4a5d7eab575af4f10a1361ba60982976a -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_MULTISAMPLESAMPLES aa05fcd07e63ad9206502099eaad358fdf9bbf8cfee9febd82fd571fc518979e -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_RED_SIZE b8233bb3b43ad9bb4816fc25137b4ad3783695680ae46c106ae8cce2bf4e4a6e -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_RETAINED_BACKING d5c2aa5d84f66824cfd4f349a0ee9a115aefee5f2d0a9bc4edefb0ff3dc6d865 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_SHARE_WITH_CURRENT_CONTEXT 8bfb7293c3c0faadeb31c538ae77336f2a6ab81633108721b9bd21b298aa0cf5 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_STENCIL_SIZE 2cee782c445875f839422d608c851affce980136b831406cd8e38f64a933bcb6 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.SDL_GL_STEREO f02830723097578b33d68c99481e5b19bc72aa994648988e315c22edfce7b531 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_CONSTANT_COLOR 237f7124455f970c05771354c06ca9e937f619f1b9a7d717930c3db912548fa2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_DST_ALPHA a515f09ee3c47fb68add66fd18c536c38eacb22b59d9dfbdfd54f50351769f55 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_DST_COLOR 8e7754a19c49bf288a8ae36fc414220f00db71e0990ea3ffd514d6fb09504f1e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_INVALID 20c7eeb826b8bc092dd63093868c5541c8e0ac45aff92b69f2a5b8e415e46e72 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE c7c8b45d97c5c1a6747188376a0e37e6c5ab3f9c328be57a71e1e104c00220aa -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_CONSTANT_COLOR 469c7962f5697ce860206b5ceec41419f51c7e888386bf6b648685f07f7e2c4f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_DST_ALPHA 9d8f94154cc7a827bb966188f4c0c5f80ea9595a346d2be61974793d9d5f1131 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_DST_COLOR c0369371e7ded7a28362153e106915233bc9461d8ccca9265a4020202d6a27f7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_SRC_ALPHA 3f35e5fd1266dc0b200a49c5d8daef07c67620b75f60233508fb786ce22c88a8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_SRC_COLOR c4f10d82a8b07809db39c5ec8d80ac8028b9e2ee52f58057705b7aa6aae60feb -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_SRC_ALPHA 54bb7c9e16e5854e01e3acb9ff4f92df76b897792a58da3c80fd86db1bdd2f0f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_SRC_ALPHA_SATURATE 64499359a7f6b535c15a1b2c46cc5c4b9c9e0bb95b26410791e18888fdef15fb -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_SRC_COLOR 6df4e3d4c168be23b73cc022f4d9e885826f0eb3243de3052ed6ebc7247e5bc2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ZERO 48ec29837e9acb864a761d787e39dfb152f171984e27d30b15fff64cb080f9f6 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendOp.SDL_GPU_BLENDOP_ADD 517f5f643c0b3e0a431fd7eb07cdecdd6449acd72aa67f8eb7fb66174d18b7b8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendOp.SDL_GPU_BLENDOP_INVALID a4a0ae60a8dfd459f80ac1c52b2e7d2da2116e10f11c0b97b1b3b9002193e206 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendOp.SDL_GPU_BLENDOP_MAX 618d5c3af65ed8eb768636fc44a1b70aac0ae96daccc53d408e37fa01e9bbe85 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendOp.SDL_GPU_BLENDOP_MIN 28d886ce620a5e96a77b0c4326386dd5471d3d33d1a54c16d14f4d9350986057 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendOp.SDL_GPU_BLENDOP_REVERSE_SUBTRACT 01de48135266f2320968a11782eccb020f14a81b57081827f872cef30463e33a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendOp.SDL_GPU_BLENDOP_SUBTRACT af1de38aa4c2539714d750fa05ce9238e90563211c664934e85bf2f32082e8d8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.ClearColor 71f88b95228c2a266e9bfd6c96c22553710f9e437effe6956bd97213de4e7b22 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Cycle 3ba65142612bbac0aab46a9ce190215f305bb5fd9cf2c9d20e3fc156ef2bd168 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Destination f19564b596097298cc51be0d4ebe1c96f9c23e2463dbfce0612ed8c7f824002e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Filter 47d143330f096ce26b8503b4d6c91767ad1ffd3ff0a945e9f4e34dc0b43e9144 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.FlipMode ee8fcead0e79fb00db78680eb115cdf19fee91e8bf4870887628ce8de72ebffc -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.LoadOp 5e93a331cec493cb834263897d633a5208173e0484bd1b5261ae1a9fbca797b3 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Padding1 8da221ecc372431156f8f14cb9a0d46ecb46d8d450e2fccc7748bcaa22970fca -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Padding2 ab48213d20d461c60fd095e6a0ae421215eedd33c64ce91cd12b8af4c95f3a0c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Padding3 cf4ebe5322aad7f502134c0eabbc32babd331d4556cfe43635175da47e202e17 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Source 4b76d6610e21dcf8cf3b26823b1547f70b191649ff5239a7f22f6fe6aa573ccc -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.H c62e86db00691ffc5276c9759411e459c68dc051da3d81cf74d6b735caec1e3e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.LayerOrDepthPlane 0aa791756bbf4f30ecf14efdad1e44529dfdc34add2c430927149e778f0b2a17 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.MipLevel e6dd900e83facab8bae9376d3a668cf83f2975538dd6a8faf252a8ddc366854e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.Texture ec4afb0a25e65b035f1480f4b61bb6afed560adf61fcd3a8d6bdee5b852530ad -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.W 4683ff9b140bf8cd7ee2afa1387cbf08d1250b034e11e7c4572758ce942584db -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.X d6125dae92953ae70222328a5d0e1ee743c089bd5e9f7af44d7ff1b5ef388307 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.Y b2a26150821eb1f498377760a3bddb12fc5871d688e6b18b0f60740019381db7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferBinding.Buffer 6418fc1417353d18828b8829fbc57f14bcfab20031c895303b542c67fa65e60f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferBinding.Offset 6c7dc82f8a0549ea7b300875cd591784db647ea69df211de04c29f0551f2daff -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferCreateInfo.Props 79cfb80a6cd4c2e6b90e091d4f0dd217771139ff085fa6c767db242760a1e12b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferCreateInfo.Size 596ff9694b992b82f3f70bbcc857d4f86bf3e9cdb782a4ec894110f7338b1480 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferCreateInfo.Usage 1d48ce5ecab6b455e66792ec96109211180714ef4fdb06b552e7141917c5b7b8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferLocation.Buffer b038e83e7122e7a8dfe1f5203eada4299afcfc061fac44e98946f9fcd3867fe9 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferLocation.Offset 052c57d5e9f81a564f85a4b31775ef0663d7492709235e5e7485bdcff3435363 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferRegion.Buffer f36031c72ef3659ce80f22f6bb73f835d11b0e090e30282a59dcf4598290c27d -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferRegion.Offset 79acf5cf3352a9aa36b817c1d375018979195b47adbbf7f990f6526e7c5d9703 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferRegion.Size b249809490737aacd589e6ac3b116a2ff301a4801d459273a446558a542c2f6c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorComponentFlags.A c47588bf6e03e77215c12792a8b3f562f71b069c136f50e07a5c683e1cc610d7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorComponentFlags.B 4206c4d5128ec1b987d2ff3b383530b15414a4eaa6c3ce467477eb5d9a454fd3 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorComponentFlags.G 23feb06efc235a0c6d85a4f422660c85b32eb67b2ab21f51433e6f68d9efd712 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorComponentFlags.R 8d69ebff248089c0e7218c0b1e3d5c21b8674259ac7e7cf02ad7647a50d7b519 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.AlphaBlendOp 0ad10b0500b57b7cb8ca3de1349ca762087ccfa90a4793bbb9e93233ce90804c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.ColorBlendOp 6e63020ab65c33253c187b50ca92ae8959c64245548ad8df6590bd5a628b192b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.ColorWriteMask 65c59b4b67d0c237363194dc5349b5a10df45967559846e29797539b12dd3e34 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.DstAlphaBlendfactor 68d5c604adc54f07a21297e5fa5c74377eb67007563ec5db0c640ed90f0faccd -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.DstColorBlendfactor 7183c8f4ae3d2c4b2348aacf5250e8a1bd4a9e24c0f768cb4cf3ab36b8af9f63 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.EnableBlend ace5a8bc84ca089404ce0e1abd8273552463efc2285cbd2ee35f103a3da80e15 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.EnableColorWriteMask 149914a59c5055c59f98f568cad1a694a292fca56699646f9009c1b432260fa8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.Padding1 09cbafc91909c3a4f8e11604365f5587beda62a82bde3ec6bc2d675da663982c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.Padding2 9756e1d4374e37da84f04e330fa70f2a89010bdfcc943afb5a3c123d79403d7b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.SrcAlphaBlendfactor b1b78b3a0c5b7c921e0626d8371b484fcefef4bf2612c40732453a9445617546 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.SrcColorBlendfactor 00af5fcf8ac23417689bbccd20aab4a75412e2c29133d22b3abc891ca988fa1f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetDescription.BlendState faecfa66e70f725b4adc82dc83ff404b39300912b08c1923ba2911d64b3e1204 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetDescription.Format 398ccd4bad81936fd30507c039348a337b26eeec298424d8f9102c903ffbaaa0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.ClearColor 7b7c783c8927e3b7b46ba01e44f7a53f966f3583676654ac3db347f3e6c8d4ec -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.Cycle fa1aa6546e242f035a1cddd0499e3abdbe4252bfb6de3682a79e305375c5c2f4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.CycleResolveTexture 651777bce3c82969cae1db8eeecc8e7f3984af60436e2ba4af21af0decd2f839 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.LayerOrDepthPlane b5bf04832ae927e80eac764f83bc292236a904e9d133e4126c9e5ec5c7858aed -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.LoadOp bb98dba03de7d1a8f81f52f5298342982fc5eedc3aff4c09f847f40fe152752e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.MipLevel eb14f067621410e718a0f1dd201e206a6d75d0d2969db3a855c7821b9aca10af -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.Padding1 30aff1a07332e7b408d94efad926e3aceecd2c0ecd9b4e9e8691b6a6f57df3f0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.Padding2 041ffd230d645070cf812cd8acef663158640faec3607cd9c6a83ccfc831d935 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.ResolveLayer a378d2083f8750ae0fd7c81f6476afd432c7871584c40721b98f296232a3312f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.ResolveMipLevel b1b3014f2c9ffa313a33c588beb8c4e161402c4849d5a6001bc143f5c8c89831 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.ResolveTexture bc11c5ae576765e85083fe3d3c72d658e97ddef1c7c3b93317937214d05d38e1 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.StoreOp 4d2d4853977feea1d2cc6184777f5b4689500f555fb39a877b8814360fb4a106 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.Texture 1bff08ba558efd5f3c4a9cdc1d6090cb731791febbe11575c2066a6b46a8236e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_ALWAYS 413db6f5f15b4c446b9bc4c422c84e0c2071175c3248bb263f1409493f3e2d17 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_EQUAL 2c3ec7a6c3ad99f685acb1205abcab65cdc7bad70f159fbc68e05a0bce88afc8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_GREATER 6944f2de9b505bdaf3068caec47cc216efa86462dad5f83bb137803893efad60 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_GREATER_OR_EQUAL ddf259939afef3cb0a91c9af0e0771c5f2f6c21830f8cb26b23714856e27a3fe -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_INVALID 64cd68cecb6bf4301e87cc5a07250a62a78d2f43b7a642504b84da0b60fbc1b9 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_LESS f270c90bd1208b7a88e8e068c0dfc8d9209c12ab5c7bc2e7d314902c2a3016e7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_LESS_OR_EQUAL 3ab262270a0791165eadbbb226925992108d8d3e6d451742008cf1081a9a2561 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_NEVER d9ee0a1ed50d977b5db6e30f51775174c29da301549c7a31370ee5ccba9fb909 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_NOT_EQUAL 0ef62c1c84f906da445b1aba11565b6a587b2afeb3b7b51ab96a5214dec08b36 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Code fed0235a8744e436336afd2ec9d7fa1736312625b47276a4c2cc5913d937b833 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.CodeSize ef85a73fd4f3c1e16ea50dec7d2c25eb77f164e6fde3abb0b42af7991a1f8ce7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Entrypoint b749a31905a03131b75cb60f47a8237c7dbea355e8c96d0d371635f4b8cae6f8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Format 4c2ad89b731dad1f2552dde59bc741e19a558f45f0c585d6b9cac1f9fc6499d8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadonlyStorageBuffers cd8cb34b3fc1a43b66249c687ee037ea614b637e66cc621100cf2637870568fe -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadonlyStorageTextures 3b7f254d4a9063d4d4d842390d2f57922d007c71037cea24e075b3561e32d370 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadwriteStorageBuffers 25de7297b66158050b2ab1bcee456013db274f6694b26c71fa6af05fccec8ff1 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadwriteStorageTextures 42e433c065d8b81311626ed93724c73c35e490c7ecedd7c7215be444f8229c93 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumSamplers 2ff63fe62a427c08cb9459a55306fa80285a6a36061f87e9f5067b90589e3c88 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumUniformBuffers 3abd5f480f1fc09b97423be4c5bb65c04256ce2a49afd18c14a57a49227ddbce -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Props 859e07abc7bcac6b963b30dceabb72e01face61431cedd5cb190bff50801ee6b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.ThreadcountX c0f01b9054b48b4b31cf2b7d7425fb631c5844ba1965969d7d7712adee12fce0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.ThreadcountY 76344aac81a4f1edca122b73dd72c09d870bba924b65f8dd44887776f87da00e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.ThreadcountZ b80188c4e7f99791981208ebdb53b841ac801349a6d899e01349ef40bbbbcacc -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCubeMapFace.SDL_GPU_CUBEMAPFACE_NEGATIVEX 11f28ba6a48ee750f64a2b801f5b6efe421ec1d8a782560475bf883d1a616788 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCubeMapFace.SDL_GPU_CUBEMAPFACE_NEGATIVEY b244e2f0c8f9f6898496f9a10de43d8b8b3a321a45085fc453ed6805b128f9ca -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCubeMapFace.SDL_GPU_CUBEMAPFACE_NEGATIVEZ d0153990f673fa18c9339e1a152eaa99e6f9a4b80800bd1fb3ccb4e95626e447 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCubeMapFace.SDL_GPU_CUBEMAPFACE_POSITIVEX 7459dac9efe94df6c4a465df9192eba13a37f2f7bb63706030d26d594d37f30a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCubeMapFace.SDL_GPU_CUBEMAPFACE_POSITIVEY 3af96cb9d34201c57d5fb7e5a6f752b1880bb3cf5d31857f66d9910b6ceae732 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCubeMapFace.SDL_GPU_CUBEMAPFACE_POSITIVEZ 535d4c65e774505307d34d20a23a791a61e576e820ddf3de9de994d9d1f0dba0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCullMode.SDL_GPU_CULLMODE_BACK 47f13bb285c1409bb594915fa66e0d0997d036fc9c15d7ae9c76baae28f342c3 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCullMode.SDL_GPU_CULLMODE_FRONT 6144c7d27a1defc1fe79f1ccb56ce66dc194fba7be930c8bd2aefc74e848f91c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCullMode.SDL_GPU_CULLMODE_NONE 01e5eb3f819d514f50355b995bdd1709bbac3ce98bf493bcaa7f0220e2b49878 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.BackStencilState bf1c92f5b4ad80d8adebb51b721b0f3f50878c3760a896a0a9ef0a370f20ef99 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.CompareMask 1f31f9141b18e3b8cdac7c080148cdce32bddb3a74b17f43beb7073d512d10a9 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.CompareOp e23991a5cf33966cf22c22c79feb86426c599cb704bcfd04f3f4bb446eaf4c53 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.EnableDepthTest cc1b57f963459e25607c5e78c75bde570ff9b2504dd9cdc3a4d8553a3c180af8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.EnableDepthWrite 4b39363a5a8e05fb350d5ad3b9d7748c23090a2e87fedf699c130233b1d47042 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.EnableStencilTest c4847c9d41f6290229c7df263e942bcb28d03d3e0ba2d712fad7e47d0c9432d1 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.FrontStencilState 1f6b6c29c819331cef17f3ba260d23e95b0a84a43b893f73796269318262631b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.Padding1 9c786dc66e9dd83d10ca7ab66d7f7ca0d4a56f96f09882d70aed322d471bb1e0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.Padding2 6e5c69f9715f865c9ec7a5c6f8225c5d6abec1824251542ab958e9036c899d80 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.Padding3 e93bd4844115d7fb141d9ce748b2f233b844d3d27d8e3badbbb11aa552f0b264 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.WriteMask 31281812b316b9c251d4bc1bca28f19f88df6aaee4e4b6fbb123357b190bfac6 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.ClearDepth 14e22998a1554845d81748a40f0aa91b3d12abd0f4aea03f018c55522ccba426 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.ClearStencil b4816a76f980dce54eb83ebed723e88334cc2f46a194f9d53f2ebea4f4aea5e8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.Cycle 1376ffc573c15313b1c0d3f1aa6c6c375d2b2bae940b8536697f14f92ef2c536 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.Layer 17ae95a8ad15742c945870a4664951d06eee3acd79fb7c5cab67f9428dc52528 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.LoadOp 8b92d4d76218accf01c45f88c4501a633db8bbf29824ed96da97ac4a6521b97d -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.MipLevel d37ab47077f8da4a98881ccf51f2eda15bb395a27b7ab0b2934ebe24a4c9b421 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.StencilLoadOp 299e6e16a3545c990eb81fdc70654aa18806760cf6db09ad3881a77954f80d29 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.StencilStoreOp e98b6ff248ba369e601af032099fa937dbeae97157d8624a8b713149ed8a8b06 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.StoreOp fd7f11023584900024329f9aa7cb629a8011f991a95b4c71e676f311012432cf -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.Texture 0e2f86fef6c3ea3d2a98f3143653cc4fb79389891bfced207890af8e10a23d8e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUFillMode.SDL_GPU_FILLMODE_FILL 7a54df2d125e43cca4a70628232500d01aa5c76e3f43989f2137a1ed8dade211 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUFillMode.SDL_GPU_FILLMODE_LINE 6569e4809645f789319d4b8fff87b95742400fa4d6ce59ef72365e6f6d4fbe84 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUFilter.SDL_GPU_FILTER_LINEAR af8f84826584507d9912ab3db5e63a79d90f17fc7eca52c5336a51dd51988566 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUFilter.SDL_GPU_FILTER_NEAREST 557133f6625b62ead968e9c9deb5f30ce7e0605943aa356194d83942cd49b191 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUFrontFace.SDL_GPU_FRONTFACE_CLOCKWISE e719bd99a4de751064be89319971a86bed91ac9b716fac14e25974f031cd770a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUFrontFace.SDL_GPU_FRONTFACE_COUNTER_CLOCKWISE 930f2300fb341e81194b7c4d3c8102f5622068704c12b6ebd881fb8a423d26d2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.DepthStencilState 3fa460ad986c53db65499bee5271ce0b949e54a02af461aeed764d404aa7b37c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.FragmentShader 0f680718edda2d04d6b8b3a28f5ff59abfede0bdc834fe78c4dd4a66edd01bd3 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.MultisampleState 54c567058304d3d6f673ae6029364c635ca6db1f1b1bdbb2abed229fbc857a9a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.PrimitiveType 06ce54afb192ae3311631ed1fbe41b9f436b73cb245d41eb2024e1fb60834688 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.Props bff4bc9b99ab889f968e0a7fdf241e979066860b3ccbecd73771425d56d468cb -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.RasterizerState b1c8fd9f24b736b94a17ce8c623f08d60d8e0a53bb25dd40c6e62bd53c76a9ea -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.TargetInfo 570e664245e7b54cc457ed2a0d06266cb1282df33098436050502f58c2939c2b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.VertexInputState 75f0dd2b08831f4f2e3d2bfc956a0de17a3eb9876a704c43c53be5f5459dc813 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.VertexShader 2e00207fb724af60267467547530e942640e04ea5db56d18111234edf674e31b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.ColorTargetDescriptions 900cc2127586d4daf8f546aa1e552bf6f5d2ca9bf3964a54f907675aae694062 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.DepthStencilFormat 61bd4db139a7b8c58a1ccf1b8904e784eb0cd6c0ac5c2b788f521efa86a1f9cb -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.HasDepthStencilTarget 2f2994eeb03ea8e4a47fa2eed5ef96e3b72266d792c9cba5d840ffd97b709658 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.NumColorTargets 9b29700df84f9905be8163770f628d64c0f3bab1818f8a5e4a97f710a1f62aa2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.Padding1 8fabc1dae780daf428158a2ab62c9e1d4ccb2fcfd1c48a2b24f42ccbd6ac4b13 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.Padding2 116659d21330f1f31a0ecca20904fa96758edcee337ee79362d16d2f9fe7eaa4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.Padding3 2b65be0427623049f4c639b267520cdacd07c431bbff5d7c0b53062ae8457592 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexElementSize.SDL_GPU_INDEXELEMENTSIZE_16BIT f42d8df15e74d3e10b237c098173fffdbd11fa6a4202248fd383d5d67714b5c5 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexElementSize.SDL_GPU_INDEXELEMENTSIZE_32BIT be319d7526599f59a2c559c247e7328e1b92cd27737cf790875fa5f67025ac4d -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.FirstIndex e9280f97a44deb28bb5627d9f421e935edd186d7e551a91ce0514b23a7cfc5f8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.FirstInstance 7f1123871a885ab660e493cf2641d77a2f7269c0b79329578d514a1d0355f46f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.NumIndices 9177ad37b9fa31dee30501767973fb54d4339694954be2f09d493befdef7d12a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.NumInstances 2e1b032397336d62fff2ca75ffb71806becc830e266ad2763e938547d4e79a93 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.VertexOffset 0754f6968855caede7249874ccafbe8ae05c2612abe26c6d04a0b019f4478169 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.GroupcountX d19428a9a21278cbae8461839f157457bdbefe67539515ed87f52be79bac0a9e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.GroupcountY 76bcda7432660566b04ec75959a7560ae6a6f52797f57da0df6c38943b8882b4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.GroupcountZ 1ee8cd8a507bea3ab503f7b6b672f6fab4f7d7110e104e6b2a712543f81d506f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndirectDrawCommand.FirstInstance df4936f4be4442118f3b9ca6472581c2210285923315acff6ac79339e3abd459 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndirectDrawCommand.FirstVertex 24672df801395e4325d1655688be440759cd5d31195260cbdd67e5ee53cc02de -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndirectDrawCommand.NumInstances 7670e8c895a73f2ab702e3e333293ad5c00de2937dcf77bf15958c9e64badb9b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndirectDrawCommand.NumVertices 22dee59a4e2293279a0ff554e1c04a9cd38f98a29e04c9c9f62cde537988af53 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPULoadOp.SDL_GPU_LOADOP_CLEAR fc3eecede87f64298826df879582516c220c07cbd7a5111e20bfde91e61a0c0e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPULoadOp.SDL_GPU_LOADOP_DONT_CARE 2b1cf0d1235c22c65236f66b8058044eb30a54cda2464e3e0b76327fc1a284d4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPULoadOp.SDL_GPU_LOADOP_LOAD 95a4fb9fc6c967c9e3009fc9156c8404f0ccac7732425a5caef2db5eb6b8b023 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUMultisampleState.EnableAlphaToCoverage 4a91016c95b03e0e31c2d0c0251d9eef5dc6a121ac0c878968a1383ef149f6e4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUMultisampleState.EnableMask 44d1986e6b02129dc8c820ff3473cc878bf2d3b5e4979f53fe0cb327e04c2d60 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUMultisampleState.Padding2 f857bc73815eb35c8ecf7dbfa0e4f038bf2013a4e4543e368f1eec5c2f9c2f47 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUMultisampleState.Padding3 d69a25aad860e3cba6af93122d0fe49dfd0bc6c1ecaa551264bbcd0eb4738f03 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUMultisampleState.SampleCount 7f900adc8efa233087953777c9539c04f0b07ad0a63e5fac74937df3ea2619fa -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUMultisampleState.SampleMask fadd6ab51670f6f7edc7d6cfd8eaff2c253718e6a2da7c86f27d88549638bf7f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUPresentMode.SDL_GPU_PRESENTMODE_IMMEDIATE b86fc7172aa970536bdf7480f927483cc47221e93eb91f727b23709f524e6286 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUPresentMode.SDL_GPU_PRESENTMODE_MAILBOX d9208c0579e7588253ba996de17d70267fa7fc213ef7f232e2cda76645945e58 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUPresentMode.SDL_GPU_PRESENTMODE_VSYNC 937b048f3b381bf3a2757c97a33542b3d53a0b1093741ea111680403e6de2a54 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUPrimitiveType.SDL_GPU_PRIMITIVETYPE_LINELIST 3b36f03f023bd4c1ca01a2646530e14ab7fe86a8998b985439e03d1c6509fe45 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUPrimitiveType.SDL_GPU_PRIMITIVETYPE_LINESTRIP df8d830d1b170994223235d3f6eb17584b37594b070856e41238f5a2470b3cd5 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUPrimitiveType.SDL_GPU_PRIMITIVETYPE_POINTLIST 359160c715ca93fa47b7fe82e9320f830001aa17981c9c996d1580d58bd0d2d9 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUPrimitiveType.SDL_GPU_PRIMITIVETYPE_TRIANGLELIST 0f10664af735032df3ee8253cf24bcb60df78d6eb231ee42238027e1efe65145 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUPrimitiveType.SDL_GPU_PRIMITIVETYPE_TRIANGLESTRIP bd2b6185564a7347229fb1b61b18247a83d4b771b0f9090b511011f4bfb9d6a6 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPURasterizerState.CullMode 8e9084d6b4408b4b0ee69553be859797faf0f7e8cdb8379fa7abb9ac0f3e91c0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPURasterizerState.DepthBiasClamp a05614e2c844a7de547dccf0615dff41d36fc20de608b2a8e289b4bf3619a59f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPURasterizerState.DepthBiasConstantFactor c8abcee4d2801931dd0259ee5633b642116138060b638f95c0bfc567554e10de -->
<!-- api: F:Inno.Native.Sdl3.SDLGPURasterizerState.DepthBiasSlopeFactor f7f59c3cd26af0842d5deb4b71892e120049b6ac783172e6bc042c5f9b833ae8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPURasterizerState.EnableDepthBias c7fc4dd214061ecc86cf34fb324f7bc22b8d363cf349679dd83c54f1d7c966c7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPURasterizerState.EnableDepthClip 07898d9162c8713010668e34c16b12499934e4ab7aa1cb5f42587aa6e91a615d -->
<!-- api: F:Inno.Native.Sdl3.SDLGPURasterizerState.FillMode be851ebfa6984ecc133b422b0c954aa2e8d3e6b4bb16270ad69c2d4f6bf57d88 -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-02424b785dc33b39"></a>[`public int Inno.Native.Sdl3.SDLDateTime.Year`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L54958) | Year |
| <a id="api-ea1f9b8789ea6266"></a>[`public sbyte* Inno.Native.Sdl3.SDLDialogFileFilter.Name`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55076) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-308b443adb1886b7"></a>[`public sbyte* Inno.Native.Sdl3.SDLDialogFileFilter.Pattern`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55078) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e49f1599198a1f19"></a>[`public int Inno.Native.Sdl3.SDLDisplayEvent.Data1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55124) | event dependent data |
| <a id="api-7616902489eb5bd1"></a>[`public int Inno.Native.Sdl3.SDLDisplayEvent.Data2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55129) | event dependent data |
| <a id="api-c5e9e76b3ea210b0"></a>[`public uint Inno.Native.Sdl3.SDLDisplayEvent.DisplayId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55119) | The associated display |
| <a id="api-369efd7755b17a33"></a>[`public uint Inno.Native.Sdl3.SDLDisplayEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55109) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-be1bc23ff572df7f"></a>[`public ulong Inno.Native.Sdl3.SDLDisplayEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55114) | In nanoseconds, populated using SDL_GetTicksNS() |
| <a id="api-c355fc67be459874"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLDisplayEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55107) | SDL_EVENT_DISPLAY_* |
| <a id="api-4c01065e3dd32b5b"></a>[`public uint Inno.Native.Sdl3.SDLDisplayMode.DisplayId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55164) | the display this mode is associated with |
| <a id="api-bfc7e879cce3b789"></a>[`public Inno.Native.Sdl3.SDLPixelFormat Inno.Native.Sdl3.SDLDisplayMode.Format`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55169) | pixel format |
| <a id="api-cc91cf117b42dad8"></a>[`public int Inno.Native.Sdl3.SDLDisplayMode.H`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55179) | height |
| <a id="api-340bbf5dc692df16"></a>[`public Inno.Native.Sdl3.SDLDisplayModeData* Inno.Native.Sdl3.SDLDisplayMode.Internal`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55204) | Private |
| <a id="api-8297bbd0827fe520"></a>[`public float Inno.Native.Sdl3.SDLDisplayMode.PixelDensity`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55184) | scale converting size to pixels (e.g. a 1920x1080 mode with 2.0 scale would have 3840x2160 pixels) |
| <a id="api-dc66b95b2ec1678d"></a>[`public float Inno.Native.Sdl3.SDLDisplayMode.RefreshRate`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55189) | refresh rate (or 0.0f for unspecified) |
| <a id="api-481e6b28810b3fc7"></a>[`public int Inno.Native.Sdl3.SDLDisplayMode.RefreshRateDenominator`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55199) | precise refresh rate denominator |
| <a id="api-ccea2bdf939a1488"></a>[`public int Inno.Native.Sdl3.SDLDisplayMode.RefreshRateNumerator`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55194) | precise refresh rate numerator (or 0 for unspecified) |
| <a id="api-88766a65efee3b5f"></a>[`public int Inno.Native.Sdl3.SDLDisplayMode.W`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55174) | width |
| <a id="api-7e03792dcb3fbef3"></a>[`Inno.Native.Sdl3.SDLDisplayOrientation.SDL_ORIENTATION_LANDSCAPE = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55245) | The display is in landscape mode, with the right side up, relative to portrait mode<br>常量：`1` |
| <a id="api-3ae6983f6ee0f900"></a>[`Inno.Native.Sdl3.SDLDisplayOrientation.SDL_ORIENTATION_LANDSCAPE_FLIPPED = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55249) | The display is in landscape mode, with the left side up, relative to portrait mode<br>常量：`2` |
| <a id="api-b04b45b14ca87487"></a>[`Inno.Native.Sdl3.SDLDisplayOrientation.SDL_ORIENTATION_PORTRAIT = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55253) | The display is in portrait mode<br>常量：`3` |
| <a id="api-312069c85324562a"></a>[`Inno.Native.Sdl3.SDLDisplayOrientation.SDL_ORIENTATION_PORTRAIT_FLIPPED = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55257) | The display is in portrait mode, upside down<br>常量：`4` |
| <a id="api-d2dc368f15475259"></a>[`Inno.Native.Sdl3.SDLDisplayOrientation.SDL_ORIENTATION_UNKNOWN = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55241) | The display orientation can't be determined<br>常量：`0` |
| <a id="api-9676ad155146486f"></a>[`public sbyte* Inno.Native.Sdl3.SDLDropEvent.Data`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55312) | The text for SDL_EVENT_DROP_TEXT and the file name for SDL_EVENT_DROP_FILE, NULL for other events |
| <a id="api-c5f8084990830f4c"></a>[`public uint Inno.Native.Sdl3.SDLDropEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55282) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-af2dfef2109389f8"></a>[`public sbyte* Inno.Native.Sdl3.SDLDropEvent.Source`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55307) | The source app that sent this drop event, or NULL if that isn't available |
| <a id="api-31c9e83b409e9f50"></a>[`public ulong Inno.Native.Sdl3.SDLDropEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55287) | In nanoseconds, populated using SDL_GetTicksNS() |
| <a id="api-934e49c123b6687f"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLDropEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55280) | SDL_EVENT_DROP_BEGIN or SDL_EVENT_DROP_FILE or SDL_EVENT_DROP_TEXT or SDL_EVENT_DROP_COMPLETE or SDL_EVENT_DROP_POSITION |
| <a id="api-19b8df8684f59f6b"></a>[`public uint Inno.Native.Sdl3.SDLDropEvent.WindowId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55292) | The window that was dropped on, if any |
| <a id="api-a7a2e098d24a9386"></a>[`public float Inno.Native.Sdl3.SDLDropEvent.X`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55297) | X coordinate, relative to window (not on begin) |
| <a id="api-c2f48aec1525ac5d"></a>[`public float Inno.Native.Sdl3.SDLDropEvent.Y`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55302) | Y coordinate, relative to window (not on begin) |
| <a id="api-f531e7391483493f"></a>[`Inno.Native.Sdl3.SDLEnumerationResult.SDL_ENUM_CONTINUE = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55471) | Value that requests that enumeration continue.<br>常量：`0` |
| <a id="api-632a23e90cc520da"></a>[`Inno.Native.Sdl3.SDLEnumerationResult.SDL_ENUM_FAILURE = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55479) | Value that requests that enumeration stop, as a failure.<br>常量：`2` |
| <a id="api-fe983f84e4581402"></a>[`Inno.Native.Sdl3.SDLEnumerationResult.SDL_ENUM_SUCCESS = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55475) | Value that requests that enumeration stop, successfully.<br>常量：`1` |
| <a id="api-2f963fb977816b0d"></a>[`public Inno.Native.Sdl3.SDLAudioDeviceEvent Inno.Native.Sdl3.SDLEvent.Adevice`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55634) | Audio device event data |
| <a id="api-1afd958d8df1518e"></a>[`public Inno.Native.Sdl3.SDLMouseButtonEvent Inno.Native.Sdl3.SDLEvent.Button`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55569) | Mouse button event data |
| <a id="api-be1cec3c212e531d"></a>[`public Inno.Native.Sdl3.SDLCameraDeviceEvent Inno.Native.Sdl3.SDLEvent.Cdevice`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55639) | Camera device event data |
| <a id="api-04cb991a82fb4f0a"></a>[`public Inno.Native.Sdl3.SDLClipboardEvent Inno.Native.Sdl3.SDLEvent.Clipboard`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55704) | Clipboard event data |
| <a id="api-f733b4b74c9bf661"></a>[`public Inno.Native.Sdl3.SDLCommonEvent Inno.Native.Sdl3.SDLEvent.Common`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55519) | Common event data |
| <a id="api-93d5eb839eefd963"></a>[`public Inno.Native.Sdl3.SDLDisplayEvent Inno.Native.Sdl3.SDLEvent.Display`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55524) | Display event data |
| <a id="api-98588abf24f966b4"></a>[`public Inno.Native.Sdl3.SDLDropEvent Inno.Native.Sdl3.SDLEvent.Drop`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55699) | Drag and drop event data |
| <a id="api-acd6c9faec9fafff"></a>[`public Inno.Native.Sdl3.SDLTextEditingEvent Inno.Native.Sdl3.SDLEvent.Edit`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55544) | Text editing event data |
| <a id="api-e9aac588d5d4feb5"></a>[`public Inno.Native.Sdl3.SDLTextEditingCandidatesEvent Inno.Native.Sdl3.SDLEvent.EditCandidates`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55549) | Text editing candidates event data |
| <a id="api-47656e25cf89cf88"></a>[`public Inno.Native.Sdl3.SDLGamepadAxisEvent Inno.Native.Sdl3.SDLEvent.Gaxis`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55614) | Gamepad axis event data |
| <a id="api-7d7578d901f62060"></a>[`public Inno.Native.Sdl3.SDLGamepadButtonEvent Inno.Native.Sdl3.SDLEvent.Gbutton`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55619) | Gamepad button event data |
| <a id="api-8a8a7ba30437b98c"></a>[`public Inno.Native.Sdl3.SDLGamepadDeviceEvent Inno.Native.Sdl3.SDLEvent.Gdevice`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55609) | Gamepad device event data |
| <a id="api-cb489f545829693a"></a>[`public Inno.Native.Sdl3.SDLGamepadSensorEvent Inno.Native.Sdl3.SDLEvent.Gsensor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55629) | Gamepad sensor event data |
| <a id="api-94a813dca8eb298a"></a>[`public Inno.Native.Sdl3.SDLGamepadTouchpadEvent Inno.Native.Sdl3.SDLEvent.Gtouchpad`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55624) | Gamepad touchpad event data |
| <a id="api-44d8e7a11be1f225"></a>[`public Inno.Native.Sdl3.SDLJoyAxisEvent Inno.Native.Sdl3.SDLEvent.Jaxis`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55584) | Joystick axis event data |
| <a id="api-8fcd51768854f3c8"></a>[`public Inno.Native.Sdl3.SDLJoyBallEvent Inno.Native.Sdl3.SDLEvent.Jball`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55589) | Joystick ball event data |
| <a id="api-02c78141a105bb5c"></a>[`public Inno.Native.Sdl3.SDLJoyBatteryEvent Inno.Native.Sdl3.SDLEvent.Jbattery`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55604) | Joystick battery event data |
| <a id="api-29a030af118b2204"></a>[`public Inno.Native.Sdl3.SDLJoyButtonEvent Inno.Native.Sdl3.SDLEvent.Jbutton`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55599) | Joystick button event data |
| <a id="api-fd56ac2e160a2dc2"></a>[`public Inno.Native.Sdl3.SDLJoyDeviceEvent Inno.Native.Sdl3.SDLEvent.Jdevice`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55579) | Joystick device change event data |
| <a id="api-a6d650a41e59195f"></a>[`public Inno.Native.Sdl3.SDLJoyHatEvent Inno.Native.Sdl3.SDLEvent.Jhat`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55594) | Joystick hat event data |
| <a id="api-bda2e5d330ec8125"></a>[`public Inno.Native.Sdl3.SDLKeyboardDeviceEvent Inno.Native.Sdl3.SDLEvent.Kdevice`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55534) | Keyboard device change event data |
| <a id="api-7e3f31c4faef2653"></a>[`public Inno.Native.Sdl3.SDLKeyboardEvent Inno.Native.Sdl3.SDLEvent.Key`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55539) | Keyboard event data |
| <a id="api-3b1725a709a2691c"></a>[`public Inno.Native.Sdl3.SDLMouseDeviceEvent Inno.Native.Sdl3.SDLEvent.Mdevice`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55559) | Mouse device change event data |
| <a id="api-137b2fa77d66f1c1"></a>[`public Inno.Native.Sdl3.SDLMouseMotionEvent Inno.Native.Sdl3.SDLEvent.Motion`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55564) | Mouse motion event data |
| <a id="api-4c71808f62c1f834"></a>[`public Inno.Native.Sdl3.NativeArray_4db4b63b8985573d Inno.Native.Sdl3.SDLEvent.Padding`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55718) | This is necessary for ABI compatibility between Visual C++ and GCC. Visual C++ will respect the push pack pragma and use 52 bytes (size of SDL_TextEditingEvent, the largest structure for 32-bit and 64-bit architectures) for this union, and GCC will use the alignment of the largest datatype within the union, which is 8 bytes on 64-bit architectures. So... we'll add padding to force the size to be the same for both. On architectures where pointers are 16 bytes, this needs rounding up to the next multiple of 16, 64, and on architectures where pointers are even larger the size of SDL_UserEvent will dominate as being 3 pointers. |
| <a id="api-472567263bf9296e"></a>[`public Inno.Native.Sdl3.SDLPenAxisEvent Inno.Native.Sdl3.SDLEvent.Paxis`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55689) | Pen axis event data |
| <a id="api-31ef6393091b6e68"></a>[`public Inno.Native.Sdl3.SDLPenButtonEvent Inno.Native.Sdl3.SDLEvent.Pbutton`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55684) | Pen button event data |
| <a id="api-9c48bd354d5d44e7"></a>[`public Inno.Native.Sdl3.SDLPinchFingerEvent Inno.Native.Sdl3.SDLEvent.Pinch`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55664) | Pinch event data |
| <a id="api-7f3aa204fde4ad4c"></a>[`public Inno.Native.Sdl3.SDLPenMotionEvent Inno.Native.Sdl3.SDLEvent.Pmotion`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55679) | Pen motion event data |
| <a id="api-975d63536262798d"></a>[`public Inno.Native.Sdl3.SDLPenProximityEvent Inno.Native.Sdl3.SDLEvent.Pproximity`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55669) | Pen proximity event data |
| <a id="api-36dc28f033f74d03"></a>[`public Inno.Native.Sdl3.SDLPenTouchEvent Inno.Native.Sdl3.SDLEvent.Ptouch`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55674) | Pen tip touching event data |
| <a id="api-1417556ebc187076"></a>[`public Inno.Native.Sdl3.SDLQuitEvent Inno.Native.Sdl3.SDLEvent.Quit`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55649) | Quit request event data |
| <a id="api-5fb2d194309440cf"></a>[`public Inno.Native.Sdl3.SDLRenderEvent Inno.Native.Sdl3.SDLEvent.Render`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55694) | Render event data |
| <a id="api-e5e3591ee5e90eea"></a>[`public Inno.Native.Sdl3.SDLSensorEvent Inno.Native.Sdl3.SDLEvent.Sensor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55644) | Sensor event data |
| <a id="api-0a26940c9ddc57d7"></a>[`public Inno.Native.Sdl3.SDLTextInputEvent Inno.Native.Sdl3.SDLEvent.Text`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55554) | Text input event data |
| <a id="api-aff4104ca9b993a7"></a>[`public Inno.Native.Sdl3.SDLTouchFingerEvent Inno.Native.Sdl3.SDLEvent.Tfinger`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55659) | Touch finger event data |
| <a id="api-0b5f3633f58c4562"></a>[`public uint Inno.Native.Sdl3.SDLEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55514) | Event type, shared with all events, Uint32 to cover user events which are not in the SDL_EventType enumeration |
| <a id="api-67b6a948be9bd953"></a>[`public Inno.Native.Sdl3.SDLUserEvent Inno.Native.Sdl3.SDLEvent.User`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55654) | Custom event data |
| <a id="api-f6a80bfe5a16862b"></a>[`public Inno.Native.Sdl3.SDLMouseWheelEvent Inno.Native.Sdl3.SDLEvent.Wheel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55574) | Mouse wheel event data |
| <a id="api-d5e0eb8e3f534ec5"></a>[`public Inno.Native.Sdl3.SDLWindowEvent Inno.Native.Sdl3.SDLEvent.Window`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55529) | Window event data |
| <a id="api-cfafefe91b4a8d91"></a>[`Inno.Native.Sdl3.SDLEventAction.SDL_ADDEVENT = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55744) | Add events to the back of the queue.<br>常量：`0` |
| <a id="api-b6ce67e101edafb3"></a>[`Inno.Native.Sdl3.SDLEventAction.SDL_GETEVENT = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55752) | Retrieve/remove events from the front of the queue.<br>常量：`2` |
| <a id="api-14d50dab63968c52"></a>[`Inno.Native.Sdl3.SDLEventAction.SDL_PEEKEVENT = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55748) | Check but don't remove events from the queue front.<br>常量：`1` |
| <a id="api-ecf8282237adc6ff"></a>[`Inno.Native.Sdl3.SDLEventType.AudioDeviceAdded = 4352`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56191) | A new audio device is available<br>常量：`4352` |
| <a id="api-ecbb8d48c4d0f767"></a>[`Inno.Native.Sdl3.SDLEventType.AudioDeviceFormatChanged = 4354`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56199) | An audio device's format has been changed by the system.<br>常量：`4354` |
| <a id="api-a04ddaf63ceca86b"></a>[`Inno.Native.Sdl3.SDLEventType.AudioDeviceRemoved = 4353`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56195) | An audio device has been removed.<br>常量：`4353` |
| <a id="api-cd76647b1dacaa0b"></a>[`Inno.Native.Sdl3.SDLEventType.CameraDeviceAdded = 5120`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56239) | A new camera device is available<br>常量：`5120` |
| <a id="api-5f6f3227edcec59e"></a>[`Inno.Native.Sdl3.SDLEventType.CameraDeviceApproved = 5122`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56247) | A camera device has been approved for use by the user.<br>常量：`5122` |
| <a id="api-b8569e6e64f13037"></a>[`Inno.Native.Sdl3.SDLEventType.CameraDeviceDenied = 5123`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56251) | A camera device has been denied for use by the user.<br>常量：`5123` |
| <a id="api-7d09ecb20f5208d8"></a>[`Inno.Native.Sdl3.SDLEventType.CameraDeviceRemoved = 5121`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56243) | A camera device has been removed.<br>常量：`5121` |
| <a id="api-90066ec0c4756109"></a>[`Inno.Native.Sdl3.SDLEventType.ClipboardUpdate = 2304`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56167) | The clipboard changed<br>常量：`2304` |
| <a id="api-83f08b9c3fc4d550"></a>[`Inno.Native.Sdl3.SDLEventType.DidEnterBackground = 260`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55826) | The application did enter the background and may not get CPU for some time. This event must be handled in a callback set with SDL_AddEventWatch(). Called on iOS in applicationDidEnterBackground() Called on Android in onPause()<br>常量：`260` |
| <a id="api-c6aef52f1dbb256b"></a>[`Inno.Native.Sdl3.SDLEventType.DidEnterForeground = 262`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55838) | The application is now interactive. This event must be handled in a callback set with SDL_AddEventWatch(). Called on iOS in applicationDidBecomeActive() Called on Android in onResume()<br>常量：`262` |
| <a id="api-5273c4836fe5dd00"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayAdded = 338`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55854) | Display has been added to the system<br>常量：`338` |
| <a id="api-cd1584dc397c0721"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayContentScaleChanged = 343`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55874) | Display has changed content scale<br>常量：`343` |
| <a id="api-822974eb896adefe"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayCurrentModeChanged = 342`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55870) | Display has changed current mode<br>常量：`342` |
| <a id="api-570851b1dd70edd8"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayDesktopModeChanged = 341`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55866) | Display has changed desktop mode<br>常量：`341` |
| <a id="api-1cb2ae2b87e7c823"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayFirst = 337`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55879) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`337` |
| <a id="api-e538ad7f363ad431"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayLast = 344`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55880) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`344` |
| <a id="api-abd59f004bf48e76"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayMoved = 340`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55862) | Display has changed position<br>常量：`340` |
| <a id="api-b762cec5f6931e80"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayOrientation = 337`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55850) | Display orientation has changed to data1<br>常量：`337` |
| <a id="api-9d9b9b5fe42dfe84"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayRemoved = 339`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55858) | Display has been removed from the system<br>常量：`339` |
| <a id="api-68acec38a89986b7"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayUsableBoundsChanged = 344`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55878) | Display has changed usable bounds<br>常量：`344` |
| <a id="api-b2780a8faeead9ee"></a>[`Inno.Native.Sdl3.SDLEventType.DropBegin = 4098`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56179) | A new set of drops is beginning (NULL filename)<br>常量：`4098` |
| <a id="api-96bb6701db21e0a9"></a>[`Inno.Native.Sdl3.SDLEventType.DropComplete = 4099`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56183) | Current set of drops is now complete (NULL filename)<br>常量：`4099` |
| <a id="api-c5910e821d50e89c"></a>[`Inno.Native.Sdl3.SDLEventType.DropFile = 4096`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56171) | The system requests a file open<br>常量：`4096` |
| <a id="api-6cfd7913b5d0c411"></a>[`Inno.Native.Sdl3.SDLEventType.DropPosition = 4100`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56187) | Position while moving over the window<br>常量：`4100` |
| <a id="api-17a31061f8b2b3ce"></a>[`Inno.Native.Sdl3.SDLEventType.DropText = 4097`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56175) | text/plain drag-and-drop event<br>常量：`4097` |
| <a id="api-8743bb0b76649fad"></a>[`Inno.Native.Sdl3.SDLEventType.EnumPadding = 2147483647`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56296) | This just makes sure the enum is the size of Uint32<br>常量：`2147483647` |
| <a id="api-1f32f6209b38be46"></a>[`Inno.Native.Sdl3.SDLEventType.FingerCanceled = 1795`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56151) | Touch events<br>常量：`1795` |
| <a id="api-0e9894e849f533f0"></a>[`Inno.Native.Sdl3.SDLEventType.FingerDown = 1792`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56139) | Touch events<br>常量：`1792` |
| <a id="api-34925b751ea185f9"></a>[`Inno.Native.Sdl3.SDLEventType.FingerMotion = 1794`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56147) | Touch events<br>常量：`1794` |
| <a id="api-e9fc0df39a749d72"></a>[`Inno.Native.Sdl3.SDLEventType.FingerUp = 1793`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56143) | Touch events<br>常量：`1793` |
| <a id="api-40d03e6a9f65a08f"></a>[`Inno.Native.Sdl3.SDLEventType.First = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55798) | Unused (do not remove)<br>常量：`0` |
| <a id="api-8994b0b0d669b98a"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadAdded = 1619`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56103) | A new gamepad has been inserted into the system<br>常量：`1619` |
| <a id="api-8b84932e51fbb265"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadAxisMotion = 1616`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56091) | Gamepad axis motion<br>常量：`1616` |
| <a id="api-7bdc74dc9e4c7591"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadButtonDown = 1617`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56095) | Gamepad button pressed<br>常量：`1617` |
| <a id="api-22fa3ff5f23620a3"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadButtonUp = 1618`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56099) | Gamepad button released<br>常量：`1618` |
| <a id="api-7443e502de6616da"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadRemapped = 1621`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56111) | The gamepad mapping was updated<br>常量：`1621` |
| <a id="api-d562be19821c2726"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadRemoved = 1620`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56107) | A gamepad has been removed<br>常量：`1620` |
| <a id="api-831d1190f6aef5f7"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadSensorUpdate = 1625`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56127) | Gamepad sensor was updated<br>常量：`1625` |
| <a id="api-026b5c77851becce"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadSteamHandleUpdated = 1627`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56135) | Gamepad Steam handle has changed<br>常量：`1627` |
| <a id="api-f34ba3e47b4d2344"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadTouchpadDown = 1622`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56115) | Gamepad touchpad was touched<br>常量：`1622` |
| <a id="api-41f8f01cba6e0460"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadTouchpadMotion = 1623`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56119) | Gamepad touchpad finger was moved<br>常量：`1623` |
| <a id="api-907d7cfbad03d4ee"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadTouchpadUp = 1624`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56123) | Gamepad touchpad finger was lifted<br>常量：`1624` |
| <a id="api-b4663f675900e993"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadUpdateComplete = 1626`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56131) | Gamepad update is complete<br>常量：`1626` |
| <a id="api-056e864d069ba2a2"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickAdded = 1541`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56075) | A new joystick has been inserted into the system<br>常量：`1541` |
| <a id="api-0c31250194f1921e"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickAxisMotion = 1536`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56055) | Joystick axis motion<br>常量：`1536` |
| <a id="api-95532d8eefddfe2b"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickBallMotion = 1537`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56059) | Joystick trackball motion<br>常量：`1537` |
| <a id="api-79ab8d67806cfe6a"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickBatteryUpdated = 1543`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56083) | Joystick battery level change<br>常量：`1543` |
| <a id="api-adb42e34c93389e8"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickButtonDown = 1539`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56067) | Joystick button pressed<br>常量：`1539` |
| <a id="api-2155bbfcd242f1db"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickButtonUp = 1540`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56071) | Joystick button released<br>常量：`1540` |
| <a id="api-d7848ae06d993339"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickHatMotion = 1538`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56063) | Joystick hat position change<br>常量：`1538` |
| <a id="api-623ffdacc7f62e02"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickRemoved = 1542`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56079) | An opened joystick has been removed<br>常量：`1542` |
| <a id="api-536631e02c5197f9"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickUpdateComplete = 1544`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56087) | Joystick update is complete<br>常量：`1544` |
| <a id="api-9da460c20129edc2"></a>[`Inno.Native.Sdl3.SDLEventType.KeyDown = 768`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55990) | Key pressed<br>常量：`768` |
| <a id="api-39d37dfdf8622d5f"></a>[`Inno.Native.Sdl3.SDLEventType.KeyUp = 769`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55994) | Key released<br>常量：`769` |
| <a id="api-951e491f323fef6d"></a>[`Inno.Native.Sdl3.SDLEventType.KeyboardAdded = 773`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56011) | A new keyboard has been inserted into the system<br>常量：`773` |
| <a id="api-160c13f6ee896aa0"></a>[`Inno.Native.Sdl3.SDLEventType.KeyboardRemoved = 774`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56015) | A keyboard has been removed<br>常量：`774` |
| <a id="api-d710953d8ab82ec4"></a>[`Inno.Native.Sdl3.SDLEventType.KeymapChanged = 772`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56007) | Keymap changed due to a system event such as an input language or keyboard layout change.<br>常量：`772` |
| <a id="api-b7aa0b43855d2f88"></a>[`Inno.Native.Sdl3.SDLEventType.Last = 65535`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56292) | This last event is only for bounding internal arrays<br>常量：`65535` |
| <a id="api-208ab421deb0bce4"></a>[`Inno.Native.Sdl3.SDLEventType.LocaleChanged = 263`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55842) | The user's locale preferences have changed.<br>常量：`263` |
| <a id="api-ea927af28c0a6385"></a>[`Inno.Native.Sdl3.SDLEventType.LowMemory = 258`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55814) | The application is low on memory, free memory if possible. This event must be handled in a callback set with SDL_AddEventWatch(). Called on iOS in applicationDidReceiveMemoryWarning() Called on Android in onTrimMemory()<br>常量：`258` |
| <a id="api-75ffe3a7960f14fa"></a>[`Inno.Native.Sdl3.SDLEventType.MouseAdded = 1028`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56047) | A new mouse has been inserted into the system<br>常量：`1028` |
| <a id="api-2c4366cc21c43a65"></a>[`Inno.Native.Sdl3.SDLEventType.MouseButtonDown = 1025`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56035) | Mouse button pressed<br>常量：`1025` |
| <a id="api-9b020e4771c0f39e"></a>[`Inno.Native.Sdl3.SDLEventType.MouseButtonUp = 1026`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56039) | Mouse button released<br>常量：`1026` |
| <a id="api-e986bfec4174c856"></a>[`Inno.Native.Sdl3.SDLEventType.MouseMotion = 1024`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56031) | Mouse moved<br>常量：`1024` |
| <a id="api-8ba929e2db0726b6"></a>[`Inno.Native.Sdl3.SDLEventType.MouseRemoved = 1029`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56051) | A mouse has been removed<br>常量：`1029` |
| <a id="api-1338e002d02cb71d"></a>[`Inno.Native.Sdl3.SDLEventType.MouseWheel = 1027`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56043) | Mouse wheel motion<br>常量：`1027` |
| <a id="api-dc3c2af927cdc2b6"></a>[`Inno.Native.Sdl3.SDLEventType.PenAxis = 4871`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56235) | Pressure-sensitive pen angle/pressure/etc changed<br>常量：`4871` |
| <a id="api-7464e4efd3c0b7a8"></a>[`Inno.Native.Sdl3.SDLEventType.PenButtonDown = 4868`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56223) | Pressure-sensitive pen button pressed<br>常量：`4868` |
| <a id="api-b81e6204ba912ce2"></a>[`Inno.Native.Sdl3.SDLEventType.PenButtonUp = 4869`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56227) | Pressure-sensitive pen button released<br>常量：`4869` |
| <a id="api-a01f78390817466b"></a>[`Inno.Native.Sdl3.SDLEventType.PenDown = 4866`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56215) | Pressure-sensitive pen touched drawing surface<br>常量：`4866` |
| <a id="api-a65c68b64df074cd"></a>[`Inno.Native.Sdl3.SDLEventType.PenMotion = 4870`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56231) | Pressure-sensitive pen is moving on the tablet<br>常量：`4870` |
| <a id="api-7b9efe5043b82e8f"></a>[`Inno.Native.Sdl3.SDLEventType.PenProximityIn = 4864`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56207) | Pressure-sensitive pen has become available<br>常量：`4864` |
| <a id="api-8516bf890f871e11"></a>[`Inno.Native.Sdl3.SDLEventType.PenProximityOut = 4865`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56211) | Pressure-sensitive pen has become unavailable<br>常量：`4865` |
| <a id="api-854b294ec87962e2"></a>[`Inno.Native.Sdl3.SDLEventType.PenUp = 4867`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56219) | Pressure-sensitive pen stopped touching drawing surface<br>常量：`4867` |
| <a id="api-d64d08e5015f53f2"></a>[`Inno.Native.Sdl3.SDLEventType.PinchBegin = 1808`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56155) | Pinch gesture started<br>常量：`1808` |
| <a id="api-903fbb7275d83290"></a>[`Inno.Native.Sdl3.SDLEventType.PinchEnd = 1810`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56163) | Pinch gesture ended<br>常量：`1810` |
| <a id="api-971f4c7ec00deddd"></a>[`Inno.Native.Sdl3.SDLEventType.PinchUpdate = 1809`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56159) | Pinch gesture updated<br>常量：`1809` |
| <a id="api-d6a4e5236882dea3"></a>[`Inno.Native.Sdl3.SDLEventType.PollSentinel = 32512`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56283) | Signals the end of an event poll cycle<br>常量：`32512` |
| <a id="api-df0ba8fba690fd13"></a>[`Inno.Native.Sdl3.SDLEventType.Private0 = 16384`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56267) | Reserved events for private platforms<br>常量：`16384` |
| <a id="api-8d41bd9bf3e4ad85"></a>[`Inno.Native.Sdl3.SDLEventType.Private1 = 16385`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56271) | Reserved events for private platforms<br>常量：`16385` |
| <a id="api-93fa7e2d7e9a58f4"></a>[`Inno.Native.Sdl3.SDLEventType.Private2 = 16386`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56275) | Reserved events for private platforms<br>常量：`16386` |
| <a id="api-8b225911431cc75c"></a>[`Inno.Native.Sdl3.SDLEventType.Private3 = 16387`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56279) | Reserved events for private platforms<br>常量：`16387` |
| <a id="api-6bd870e14c8cb8ff"></a>[`Inno.Native.Sdl3.SDLEventType.Quit = 256`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55802) | User-requested quit<br>常量：`256` |
| <a id="api-84ba88462d742ee2"></a>[`Inno.Native.Sdl3.SDLEventType.RenderDeviceLost = 8194`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56263) | The device has been lost and can't be recovered.<br>常量：`8194` |
| <a id="api-2b91bbd463cbcf81"></a>[`Inno.Native.Sdl3.SDLEventType.RenderDeviceReset = 8193`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56259) | The device has been reset and all textures need to be recreated<br>常量：`8193` |
| <a id="api-5aafdc190d473699"></a>[`Inno.Native.Sdl3.SDLEventType.RenderTargetsReset = 8192`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56255) | The render targets have been reset and their contents need to be updated<br>常量：`8192` |
| <a id="api-ae3c4770c2d4a6eb"></a>[`Inno.Native.Sdl3.SDLEventType.ScreenKeyboardHidden = 777`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56027) | The on-screen keyboard has been hidden<br>常量：`777` |
| <a id="api-813e9537cbadbb06"></a>[`Inno.Native.Sdl3.SDLEventType.ScreenKeyboardShown = 776`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56023) | The on-screen keyboard has been shown<br>常量：`776` |
| <a id="api-931686f7f1008434"></a>[`Inno.Native.Sdl3.SDLEventType.SensorUpdate = 4608`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56203) | A sensor was updated<br>常量：`4608` |
| <a id="api-d75d4f2bb949dcfd"></a>[`Inno.Native.Sdl3.SDLEventType.SystemThemeChanged = 264`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55846) | The system theme changed<br>常量：`264` |
| <a id="api-b14d3f85bc465c81"></a>[`Inno.Native.Sdl3.SDLEventType.Terminating = 257`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55808) | The application is being terminated by the OS. This event must be handled in a callback set with SDL_AddEventWatch(). Called on iOS in applicationWillTerminate() Called on Android in onDestroy()<br>常量：`257` |
| <a id="api-be905e2b0e7a3e51"></a>[`Inno.Native.Sdl3.SDLEventType.TextEditing = 770`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55998) | Keyboard text editing (composition)<br>常量：`770` |
| <a id="api-677371bcc902b72c"></a>[`Inno.Native.Sdl3.SDLEventType.TextEditingCandidates = 775`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56019) | Keyboard text editing candidates<br>常量：`775` |
| <a id="api-fbab1764ee4955c8"></a>[`Inno.Native.Sdl3.SDLEventType.TextInput = 771`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56002) | Keyboard text input<br>常量：`771` |
| <a id="api-1482bfb4df17fbde"></a>[`Inno.Native.Sdl3.SDLEventType.User = 32768`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56288) | Events SDL_EVENT_USER through SDL_EVENT_LAST are for your use, and should be allocated with SDL_RegisterEvents()<br>常量：`32768` |
| <a id="api-fa7192bd71d32fcd"></a>[`Inno.Native.Sdl3.SDLEventType.WillEnterBackground = 259`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55820) | The application is about to enter the background. This event must be handled in a callback set with SDL_AddEventWatch(). Called on iOS in applicationWillResignActive() Called on Android in onPause()<br>常量：`259` |
| <a id="api-454c0470e91926d4"></a>[`Inno.Native.Sdl3.SDLEventType.WillEnterForeground = 261`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55832) | The application is about to enter the foreground. This event must be handled in a callback set with SDL_AddEventWatch(). Called on iOS in applicationWillEnterForeground() Called on Android in onResume()<br>常量：`261` |
| <a id="api-32bd4ea6dc6ba6d0"></a>[`Inno.Native.Sdl3.SDLEventType.WindowCloseRequested = 528`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55941) | The window manager requests that the window be closed<br>常量：`528` |
| <a id="api-0c5f179ae923d13a"></a>[`Inno.Native.Sdl3.SDLEventType.WindowDestroyed = 537`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55980) | The window with the associated ID is being or has been destroyed. If this message is being handled in an event watcher, the window handle is still valid and can still be used to retrieve any properties associated with the window. Otherwise, the handle has already been destroyed and all resources associated with it are invalid<br>常量：`537` |
| <a id="api-f4d6713dcb2abe14"></a>[`Inno.Native.Sdl3.SDLEventType.WindowDisplayChanged = 531`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55953) | Window has been moved to display data1<br>常量：`531` |
| <a id="api-acac75bfc3d5db85"></a>[`Inno.Native.Sdl3.SDLEventType.WindowDisplayScaleChanged = 532`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55957) | Window display scale has been changed<br>常量：`532` |
| <a id="api-c8cdd9e398b4b2b4"></a>[`Inno.Native.Sdl3.SDLEventType.WindowEnterFullscreen = 535`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55969) | The window has entered fullscreen mode<br>常量：`535` |
| <a id="api-7f550f36bd31363c"></a>[`Inno.Native.Sdl3.SDLEventType.WindowExposed = 516`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55893) | Window has been exposed and should be redrawn, and can be redrawn directly from event watchers for this event. data1 is 1 for live-resize expose events, 0 otherwise.<br>常量：`516` |
| <a id="api-5c27652ed76b9e46"></a>[`Inno.Native.Sdl3.SDLEventType.WindowFirst = 514`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55985) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`514` |
| <a id="api-90e2032c45fcf2c8"></a>[`Inno.Native.Sdl3.SDLEventType.WindowFocusGained = 526`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55933) | Window has gained keyboard focus<br>常量：`526` |
| <a id="api-dd6171f35e19bcc7"></a>[`Inno.Native.Sdl3.SDLEventType.WindowFocusLost = 527`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55937) | Window has lost keyboard focus<br>常量：`527` |
| <a id="api-ddf32f7d0ddf6b66"></a>[`Inno.Native.Sdl3.SDLEventType.WindowHdrStateChanged = 538`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55984) | Window HDR properties have changed<br>常量：`538` |
| <a id="api-d50c11157163efc2"></a>[`Inno.Native.Sdl3.SDLEventType.WindowHidden = 515`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55888) | Window has been hidden<br>常量：`515` |
| <a id="api-3908d25b6489f1a9"></a>[`Inno.Native.Sdl3.SDLEventType.WindowHitTest = 529`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55945) | Window had a hit test that wasn't SDL_HITTEST_NORMAL<br>常量：`529` |
| <a id="api-556a82e6ff0d7e19"></a>[`Inno.Native.Sdl3.SDLEventType.WindowIccprofChanged = 530`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55949) | The ICC profile of the window's display has changed<br>常量：`530` |
| <a id="api-9c0b67c8e56974bd"></a>[`Inno.Native.Sdl3.SDLEventType.WindowLast = 538`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55986) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`538` |
| <a id="api-31cba988c14a4110"></a>[`Inno.Native.Sdl3.SDLEventType.WindowLeaveFullscreen = 536`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55973) | The window has left fullscreen mode<br>常量：`536` |
| <a id="api-a6f31c9992b4b288"></a>[`Inno.Native.Sdl3.SDLEventType.WindowMaximized = 522`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55917) | Window has been maximized<br>常量：`522` |
| <a id="api-e852e13fc881c764"></a>[`Inno.Native.Sdl3.SDLEventType.WindowMetalViewResized = 520`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55909) | The pixel size of a Metal view associated with the window has changed<br>常量：`520` |
| <a id="api-878e5068d3a1328e"></a>[`Inno.Native.Sdl3.SDLEventType.WindowMinimized = 521`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55913) | Window has been minimized<br>常量：`521` |
| <a id="api-11d3d3732dc915f5"></a>[`Inno.Native.Sdl3.SDLEventType.WindowMouseEnter = 524`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55925) | Window has gained mouse focus<br>常量：`524` |
| <a id="api-40fc8d0ef4acfd2b"></a>[`Inno.Native.Sdl3.SDLEventType.WindowMouseLeave = 525`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55929) | Window has lost mouse focus<br>常量：`525` |
| <a id="api-08244a9dd273f18b"></a>[`Inno.Native.Sdl3.SDLEventType.WindowMoved = 517`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55897) | Window has been moved to data1, data2<br>常量：`517` |
| <a id="api-906b786c3bb83ee1"></a>[`Inno.Native.Sdl3.SDLEventType.WindowOccluded = 534`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55965) | The window has been occluded<br>常量：`534` |
| <a id="api-66efd37868e68a5e"></a>[`Inno.Native.Sdl3.SDLEventType.WindowPixelSizeChanged = 519`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55905) | The pixel size of the window has changed to data1xdata2<br>常量：`519` |
| <a id="api-36e3fcf5bd53a2c5"></a>[`Inno.Native.Sdl3.SDLEventType.WindowResized = 518`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55901) | Window has been resized to data1xdata2<br>常量：`518` |
| <a id="api-7e85a1dd57a9d107"></a>[`Inno.Native.Sdl3.SDLEventType.WindowRestored = 523`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55921) | Window has been restored to normal size and position<br>常量：`523` |
| <a id="api-bf2ab0a4926f3d78"></a>[`Inno.Native.Sdl3.SDLEventType.WindowSafeAreaChanged = 533`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55961) | The window safe area has been changed<br>常量：`533` |
| <a id="api-99636ec739cb48fb"></a>[`Inno.Native.Sdl3.SDLEventType.WindowShown = 514`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L55884) | Window has been shown<br>常量：`514` |
| <a id="api-d154b0c2abe45d63"></a>[`public float Inno.Native.Sdl3.SDLFColor.A`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56322) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-44a23f9321739fc1"></a>[`public float Inno.Native.Sdl3.SDLFColor.B`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56320) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-649bc0d0139e8dbd"></a>[`public float Inno.Native.Sdl3.SDLFColor.G`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56318) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2d5b800ca9b85105"></a>[`public float Inno.Native.Sdl3.SDLFColor.R`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56316) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-621f79a51d130cca"></a>[`public float Inno.Native.Sdl3.SDLFPoint.X`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56563) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-ec75870de64c748f"></a>[`public float Inno.Native.Sdl3.SDLFPoint.Y`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56565) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5000d22e3fb5b4ce"></a>[`public float Inno.Native.Sdl3.SDLFRect.H`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56610) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-60a02b76f6ca302c"></a>[`public float Inno.Native.Sdl3.SDLFRect.W`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56608) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-fcc22f88a20b0ed8"></a>[`public float Inno.Native.Sdl3.SDLFRect.X`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56604) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-adf11c4489ea2eba"></a>[`public float Inno.Native.Sdl3.SDLFRect.Y`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56606) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a9a518d765d49fa5"></a>[`Inno.Native.Sdl3.SDLFileDialogType.SDL_FILEDIALOG_OPENFILE = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56349) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-06cb232ae59fc2da"></a>[`Inno.Native.Sdl3.SDLFileDialogType.SDL_FILEDIALOG_OPENFOLDER = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56351) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-ae6caca1ee8dd172"></a>[`Inno.Native.Sdl3.SDLFileDialogType.SDL_FILEDIALOG_SAVEFILE = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56350) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-17429b7d3ddd1b54"></a>[`public ulong Inno.Native.Sdl3.SDLFinger.Id`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56378) | the finger ID |
| <a id="api-1354a166790862a9"></a>[`public float Inno.Native.Sdl3.SDLFinger.Pressure`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56393) | the quantity of pressure applied, normalized (0...1) |
| <a id="api-2e981bb160f5f79d"></a>[`public float Inno.Native.Sdl3.SDLFinger.X`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56383) | the x-axis location of the touch event, normalized (0...1) |
| <a id="api-32e35cf27eb5264a"></a>[`public float Inno.Native.Sdl3.SDLFinger.Y`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56388) | the y-axis location of the touch event, normalized (0...1) |
| <a id="api-9133e7c42067e1d9"></a>[`Inno.Native.Sdl3.SDLFlashOperation.SDL_FLASH_BRIEFLY = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56423) | Flash the window briefly to get attention<br>常量：`1` |
| <a id="api-6ec3728e29e1a8af"></a>[`Inno.Native.Sdl3.SDLFlashOperation.SDL_FLASH_CANCEL = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56419) | Cancel any window flash state<br>常量：`0` |
| <a id="api-b0125fe1d10efb08"></a>[`Inno.Native.Sdl3.SDLFlashOperation.SDL_FLASH_UNTIL_FOCUSED = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56427) | Flash the window until it gets focus<br>常量：`2` |
| <a id="api-ad5a89e9c5faa522"></a>[`Inno.Native.Sdl3.SDLFlipMode.SDL_FLIP_HORIZONTAL = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56450) | flip horizontally<br>常量：`1` |
| <a id="api-97e26748735ce15f"></a>[`Inno.Native.Sdl3.SDLFlipMode.SDL_FLIP_HORIZONTAL_AND_VERTICAL = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56458) | flip horizontally and vertically (not a diagonal flip)<br>常量：`3` |
| <a id="api-0d845d3fc6a74767"></a>[`Inno.Native.Sdl3.SDLFlipMode.SDL_FLIP_NONE = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56446) | Do not flip<br>常量：`0` |
| <a id="api-e5bfd66f72ce24e2"></a>[`Inno.Native.Sdl3.SDLFlipMode.SDL_FLIP_VERTICAL = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56454) | flip vertically<br>常量：`2` |
| <a id="api-8d4cbf2cc2b3448b"></a>[`Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_COUNT = 11`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56541) | Total number of types in this enum, not a folder type by itself.<br>常量：`11` |
| <a id="api-7fbe96f4cab7be19"></a>[`Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_DESKTOP = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56501) | The folder of files that are displayed on the desktop. Note that the existence of a desktop folder does not guarantee that the system does show icons on its desktop; certain GNU/Linux distros with a graphical environment may not have desktop icons.<br>常量：`1` |
| <a id="api-778dec6a4d157cd6"></a>[`Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_DOCUMENTS = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56505) | User document files, possibly application-specific. This is a good place to save a user's projects.<br>常量：`2` |
| <a id="api-e72145c8cb4e0e5d"></a>[`Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_DOWNLOADS = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56509) | Standard folder for user files downloaded from the internet.<br>常量：`3` |
| <a id="api-8003ae481ac3ad64"></a>[`Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_HOME = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56497) | The folder which contains all of the current user's data, preferences, and documents. It usually contains most of the other folders. If a requested folder does not exist, the home folder can be considered a safe fallback to store a user's documents.<br>常量：`0` |
| <a id="api-82d110dd0f196762"></a>[`Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_MUSIC = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56513) | Music files that can be played using a standard music player (mp3, ogg...).<br>常量：`4` |
| <a id="api-30b904e84f71ae16"></a>[`Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_PICTURES = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56517) | Image files that can be displayed using a standard viewer (png, jpg...).<br>常量：`5` |
| <a id="api-9aa8879a4d786334"></a>[`Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_PUBLICSHARE = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56521) | Files that are meant to be shared with other users on the same computer.<br>常量：`6` |
| <a id="api-7559eff2cb44c5cd"></a>[`Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_SAVEDGAMES = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56525) | Save files for games.<br>常量：`7` |
| <a id="api-ce3a760d146d868c"></a>[`Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_SCREENSHOTS = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56529) | Application screenshots.<br>常量：`8` |
| <a id="api-0d6c177d7758d0fd"></a>[`Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_TEMPLATES = 9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56533) | Template files to be used when the user requests the desktop environment to create a new file in a certain folder, such as "New Text File.txt". Any file in the Templates folder can be used as a starting point for a new file.<br>常量：`9` |
| <a id="api-6b5eb5f4327c479f"></a>[`Inno.Native.Sdl3.SDLFolder.SDL_FOLDER_VIDEOS = 10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L56537) | Video files that can be played using a standard video player (mp4, webm...).<br>常量：`10` |
| <a id="api-e72ab9733a03d07d"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_ACCELERATED_VISUAL = 15`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57409) | set to 1 to require hardware acceleration, set to 0 to force software rendering; defaults to allow either.<br>常量：`15` |
| <a id="api-2355f7b6ebc1ee43"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_ACCUM_ALPHA_SIZE = 11`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57393) | the minimum number of bits for the alpha channel of the accumulation buffer; defaults to 0.<br>常量：`11` |
| <a id="api-f9942e78dcd1db06"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_ACCUM_BLUE_SIZE = 10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57389) | the minimum number of bits for the blue channel of the accumulation buffer; defaults to 0.<br>常量：`10` |
| <a id="api-f071d4d501c6f34f"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_ACCUM_GREEN_SIZE = 9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57385) | the minimum number of bits for the green channel of the accumulation buffer; defaults to 0.<br>常量：`9` |
| <a id="api-7092c6342e6efd21"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_ACCUM_RED_SIZE = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57381) | the minimum number of bits for the red channel of the accumulation buffer; defaults to 0.<br>常量：`8` |
| <a id="api-d0e64a64cd9937c1"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_ALPHA_SIZE = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57361) | the minimum number of bits for the alpha channel of the color buffer; defaults to 8.<br>常量：`3` |
| <a id="api-d9561e42a2dc628f"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_BLUE_SIZE = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57357) | the minimum number of bits for the blue channel of the color buffer; defaults to 8.<br>常量：`2` |
| <a id="api-631a9623e518ad49"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_BUFFER_SIZE = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57365) | the minimum number of bits for frame buffer size; defaults to 0.<br>常量：`4` |
| <a id="api-f67b7f6bbb132860"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_FLAGS = 19`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57425) | some combination of 0 or more of elements of the SDL_GLContextFlag enumeration; defaults to 0.<br>常量：`19` |
| <a id="api-38d721c5f9095176"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_MAJOR_VERSION = 17`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57417) | OpenGL context major version.<br>常量：`17` |
| <a id="api-3fd5d6258da8fcf3"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_MINOR_VERSION = 18`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57421) | OpenGL context minor version.<br>常量：`18` |
| <a id="api-2ddf200a4d39d779"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_NO_ERROR = 25`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57446) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`25` |
| <a id="api-6ddb0a0360fab1de"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_PROFILE_MASK = 20`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57429) | type of GL context (Core, Compatibility, ES). See SDL_GLProfile; default value depends on platform.<br>常量：`20` |
| <a id="api-cf49f395aeb370dc"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_RELEASE_BEHAVIOR = 23`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57441) | sets context the release behavior. See SDL_GLContextReleaseFlag; defaults to FLUSH.<br>常量：`23` |
| <a id="api-582c7e20de979335"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_CONTEXT_RESET_NOTIFICATION = 24`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57445) | set context reset notification. See SDL_GLContextResetNotification; defaults to NO_NOTIFICATION.<br>常量：`24` |
| <a id="api-4ca0de7a357890f5"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_DEPTH_SIZE = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57373) | the minimum number of bits in the depth buffer; defaults to 16.<br>常量：`6` |
| <a id="api-8ca999ed20c2708d"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_DOUBLEBUFFER = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57369) | whether the output is single or double buffered; defaults to double buffering on.<br>常量：`5` |
| <a id="api-803ae0b8b39434aa"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_EGL_PLATFORM = 27`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57448) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`27` |
| <a id="api-99cabe11f83e919b"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_FLOATBUFFERS = 26`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57447) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`26` |
| <a id="api-16b11c8581fd2680"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_FRAMEBUFFER_SRGB_CAPABLE = 22`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57437) | requests sRGB capable visual; defaults to 0.<br>常量：`22` |
| <a id="api-13ffbfe0a4fc2b90"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_GREEN_SIZE = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57353) | the minimum number of bits for the green channel of the color buffer; defaults to 8.<br>常量：`1` |
| <a id="api-2d804244e58bcde2"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_MULTISAMPLEBUFFERS = 13`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57401) | the number of buffers used for multisample anti-aliasing; defaults to 0.<br>常量：`13` |
| <a id="api-ad8cf50c2200565b"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_MULTISAMPLESAMPLES = 14`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57405) | the number of samples used around the current pixel used for multisample anti-aliasing.<br>常量：`14` |
| <a id="api-0d5380dc09f1a496"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_RED_SIZE = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57349) | the minimum number of bits for the red channel of the color buffer; defaults to 8.<br>常量：`0` |
| <a id="api-85f25508d02b1566"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_RETAINED_BACKING = 16`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57413) | not used (deprecated).<br>常量：`16` |
| <a id="api-88f5089c885dd4e4"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_SHARE_WITH_CURRENT_CONTEXT = 21`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57433) | OpenGL context sharing; defaults to 0.<br>常量：`21` |
| <a id="api-6534ffe6c36e7924"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_STENCIL_SIZE = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57377) | the minimum number of bits in the stencil buffer; defaults to 0.<br>常量：`7` |
| <a id="api-d5a2206998ce1b6d"></a>[`Inno.Native.Sdl3.SDLGLAttr.SDL_GL_STEREO = 12`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57397) | whether the output is stereo 3D; defaults to off.<br>常量：`12` |
| <a id="api-e27cb1314a32d468"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_CONSTANT_COLOR = 11`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57524) | blend constant<br>常量：`11` |
| <a id="api-7cfa3e595eb83269"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_DST_ALPHA = 9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57516) | destination alpha<br>常量：`9` |
| <a id="api-bb8f18e3652f326d"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_DST_COLOR = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57500) | destination color<br>常量：`5` |
| <a id="api-75ccab5968686467"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_INVALID = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57480) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-1000322a632203d9"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57488) | 1<br>常量：`2` |
| <a id="api-8d65d7a24ffcd8f9"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_CONSTANT_COLOR = 12`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57528) | 1 - blend constant<br>常量：`12` |
| <a id="api-8d1281924942a4a5"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_DST_ALPHA = 10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57520) | 1 - destination alpha<br>常量：`10` |
| <a id="api-18e34c6cea22dcf3"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_DST_COLOR = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57504) | 1 - destination color<br>常量：`6` |
| <a id="api-63c19b2f6114d49e"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_SRC_ALPHA = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57512) | 1 - source alpha<br>常量：`8` |
| <a id="api-5d13ffc02d4af34e"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_SRC_COLOR = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57496) | 1 - source color<br>常量：`4` |
| <a id="api-2b6c94734ac3e3b0"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_SRC_ALPHA = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57508) | source alpha<br>常量：`7` |
| <a id="api-80dee3c65349624c"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_SRC_ALPHA_SATURATE = 13`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57532) | min(source alpha, 1 - destination alpha)<br>常量：`13` |
| <a id="api-4c48c8750304c38b"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_SRC_COLOR = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57492) | source color<br>常量：`3` |
| <a id="api-e2a44b6aec2b2471"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.SDL_GPU_BLENDFACTOR_ZERO = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57484) | 0<br>常量：`1` |
| <a id="api-5d4749c0cea393ab"></a>[`Inno.Native.Sdl3.SDLGPUBlendOp.SDL_GPU_BLENDOP_ADD = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57557) | (source * source_factor) + (destination * destination_factor)<br>常量：`1` |
| <a id="api-2af766271124c87b"></a>[`Inno.Native.Sdl3.SDLGPUBlendOp.SDL_GPU_BLENDOP_INVALID = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57553) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-72c0f301be096ac5"></a>[`Inno.Native.Sdl3.SDLGPUBlendOp.SDL_GPU_BLENDOP_MAX = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57573) | max(source, destination)<br>常量：`5` |
| <a id="api-620546b438df5a73"></a>[`Inno.Native.Sdl3.SDLGPUBlendOp.SDL_GPU_BLENDOP_MIN = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57569) | min(source, destination)<br>常量：`4` |
| <a id="api-0a32f731eab3f64d"></a>[`Inno.Native.Sdl3.SDLGPUBlendOp.SDL_GPU_BLENDOP_REVERSE_SUBTRACT = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57565) | (destination * destination_factor) - (source * source_factor)<br>常量：`3` |
| <a id="api-c3bdfd24ab266100"></a>[`Inno.Native.Sdl3.SDLGPUBlendOp.SDL_GPU_BLENDOP_SUBTRACT = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57561) | (source * source_factor) - (destination * destination_factor)<br>常量：`2` |
| <a id="api-ced264f119073f55"></a>[`public Inno.Native.Sdl3.SDLFColor Inno.Native.Sdl3.SDLGPUBlitInfo.ClearColor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57612) | The color to clear the destination region to before the blit. Ignored if load_op is not SDL_GPU_LOADOP_CLEAR. |
| <a id="api-612331c85dc2d651"></a>[`public byte Inno.Native.Sdl3.SDLGPUBlitInfo.Cycle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57627) | true cycles the destination texture if it is already bound. |
| <a id="api-0582b80b3a842409"></a>[`public Inno.Native.Sdl3.SDLGPUBlitRegion Inno.Native.Sdl3.SDLGPUBlitInfo.Destination`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57602) | The destination region for the blit. |
| <a id="api-e8f76ddff7113016"></a>[`public Inno.Native.Sdl3.SDLGPUFilter Inno.Native.Sdl3.SDLGPUBlitInfo.Filter`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57622) | The filter mode used when blitting. |
| <a id="api-4032df9a7cc17eab"></a>[`public Inno.Native.Sdl3.SDLFlipMode Inno.Native.Sdl3.SDLGPUBlitInfo.FlipMode`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57617) | The flip mode for the source region. |
| <a id="api-98c5ac9518c05df6"></a>[`public Inno.Native.Sdl3.SDLGPULoadOp Inno.Native.Sdl3.SDLGPUBlitInfo.LoadOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57607) | What is done with the contents of the destination before the blit. |
| <a id="api-68acb939abaee135"></a>[`public byte Inno.Native.Sdl3.SDLGPUBlitInfo.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57629) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-04416342950f72d3"></a>[`public byte Inno.Native.Sdl3.SDLGPUBlitInfo.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57631) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-370346dba7c62aa0"></a>[`public byte Inno.Native.Sdl3.SDLGPUBlitInfo.Padding3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57633) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-5df08f93df8fe0ce"></a>[`public Inno.Native.Sdl3.SDLGPUBlitRegion Inno.Native.Sdl3.SDLGPUBlitInfo.Source`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57597) | The source region for the blit. |
| <a id="api-dd40e02837e39b76"></a>[`public uint Inno.Native.Sdl3.SDLGPUBlitRegion.H`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57694) | The height of the region. |
| <a id="api-8b2e1bd02f9f6edd"></a>[`public uint Inno.Native.Sdl3.SDLGPUBlitRegion.LayerOrDepthPlane`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57674) | The layer index or depth plane of the region. This value is treated as a layer index on 2D array and cube textures, and as a depth plane on 3D textures. |
| <a id="api-4748f3f57bb178d1"></a>[`public uint Inno.Native.Sdl3.SDLGPUBlitRegion.MipLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57669) | The mip level index of the region. |
| <a id="api-3ad5b992ab427f16"></a>[`public Inno.Native.Sdl3.SDLGPUTexture* Inno.Native.Sdl3.SDLGPUBlitRegion.Texture`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57664) | The texture. |
| <a id="api-94cfbc6b8f6df79b"></a>[`public uint Inno.Native.Sdl3.SDLGPUBlitRegion.W`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57689) | The width of the region. |
| <a id="api-5c40df929d7f169b"></a>[`public uint Inno.Native.Sdl3.SDLGPUBlitRegion.X`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57679) | The left offset of the region. |
| <a id="api-0956c56464adf908"></a>[`public uint Inno.Native.Sdl3.SDLGPUBlitRegion.Y`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57684) | The top offset of the region. |
| <a id="api-928cbdd1d93557ae"></a>[`public Inno.Native.Sdl3.SDLGPUBuffer* Inno.Native.Sdl3.SDLGPUBufferBinding.Buffer`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57737) | The buffer to bind. Must have been created with SDL_GPU_BUFFERUSAGE_VERTEX for SDL_BindGPUVertexBuffers, or SDL_GPU_BUFFERUSAGE_INDEX for SDL_BindGPUIndexBuffer. |
| <a id="api-a4a85d71e9224986"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferBinding.Offset`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57742) | The starting byte of the data to bind in the buffer. |
| <a id="api-2611177a924db3a4"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferCreateInfo.Props`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57786) | A properties ID for extensions. Should be 0 if no extensions are needed. |
| <a id="api-86f4737412136484"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferCreateInfo.Size`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57781) | The size in bytes of the buffer. |
| <a id="api-8af81c004445463f"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferCreateInfo.Usage`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57776) | How the buffer is intended to be used by the client. |
| <a id="api-3e6fd62dff56660c"></a>[`public Inno.Native.Sdl3.SDLGPUBuffer* Inno.Native.Sdl3.SDLGPUBufferLocation.Buffer`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57818) | The buffer. |
| <a id="api-c7a66eea8a3acabb"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferLocation.Offset`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57823) | The starting byte within the buffer. |
| <a id="api-699957de57d271e2"></a>[`public Inno.Native.Sdl3.SDLGPUBuffer* Inno.Native.Sdl3.SDLGPUBufferRegion.Buffer`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57856) | The buffer. |
| <a id="api-4f7456bfaa1d2c18"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferRegion.Offset`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57861) | The starting byte within the buffer. |
| <a id="api-408bdb1dbd707586"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferRegion.Size`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57866) | The size in bytes of the region. |
| <a id="api-46957d2c145f1754"></a>[`Inno.Native.Sdl3.SDLGPUColorComponentFlags.A = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57888) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`8` |
| <a id="api-09fc2c00c150229c"></a>[`Inno.Native.Sdl3.SDLGPUColorComponentFlags.B = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57887) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-5f3a92b363979a49"></a>[`Inno.Native.Sdl3.SDLGPUColorComponentFlags.G = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57886) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-7862570e94d05022"></a>[`Inno.Native.Sdl3.SDLGPUColorComponentFlags.R = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57885) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-f4bb44dc79bee9fc"></a>[`public Inno.Native.Sdl3.SDLGPUBlendOp Inno.Native.Sdl3.SDLGPUColorTargetBlendState.AlphaBlendOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57940) | The blend operation for the alpha component. |
| <a id="api-e38c918e048ae0a7"></a>[`public Inno.Native.Sdl3.SDLGPUBlendOp Inno.Native.Sdl3.SDLGPUColorTargetBlendState.ColorBlendOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57925) | The blend operation for the RGB components. |
| <a id="api-e8aafa1f734cd4a4"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetBlendState.ColorWriteMask`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57945) | A bitmask specifying which of the RGBA components are enabled for writing. Writes to all channels if enable_color_write_mask is false. |
| <a id="api-31db2d9fa31a77bf"></a>[`public Inno.Native.Sdl3.SDLGPUBlendFactor Inno.Native.Sdl3.SDLGPUColorTargetBlendState.DstAlphaBlendfactor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57935) | The value to be multiplied by the destination alpha. |
| <a id="api-b086d3313e5ce169"></a>[`public Inno.Native.Sdl3.SDLGPUBlendFactor Inno.Native.Sdl3.SDLGPUColorTargetBlendState.DstColorBlendfactor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57920) | The value to be multiplied by the destination RGB value. |
| <a id="api-5c19dc688bab3c48"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetBlendState.EnableBlend`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57950) | Whether blending is enabled for the color target. |
| <a id="api-734b966b0b9d6026"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetBlendState.EnableColorWriteMask`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57955) | Whether the color write mask is enabled. |
| <a id="api-313e0adddc9889ea"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetBlendState.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57957) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-df34bc00c7382348"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetBlendState.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57959) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-195330b5a4ff2f4a"></a>[`public Inno.Native.Sdl3.SDLGPUBlendFactor Inno.Native.Sdl3.SDLGPUColorTargetBlendState.SrcAlphaBlendfactor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57930) | The value to be multiplied by the source alpha. |
| <a id="api-4ba4fe691436b881"></a>[`public Inno.Native.Sdl3.SDLGPUBlendFactor Inno.Native.Sdl3.SDLGPUColorTargetBlendState.SrcColorBlendfactor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57915) | The value to be multiplied by the source RGB value. |
| <a id="api-259b9b1ad5bfbee4"></a>[`public Inno.Native.Sdl3.SDLGPUColorTargetBlendState Inno.Native.Sdl3.SDLGPUColorTargetDescription.BlendState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57996) | The blend state to be used for the color target. |
| <a id="api-d00dac8038b57cd7"></a>[`public Inno.Native.Sdl3.SDLGPUTextureFormat Inno.Native.Sdl3.SDLGPUColorTargetDescription.Format`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L57991) | The pixel format of the texture to be used as a color target. |
| <a id="api-12c7a115050ae934"></a>[`public Inno.Native.Sdl3.SDLFColor Inno.Native.Sdl3.SDLGPUColorTargetInfo.ClearColor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58067) | The color to clear the color target to at the start of the render pass. Ignored if SDL_GPU_LOADOP_CLEAR is not used. |
| <a id="api-38b74adb16b633f9"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetInfo.Cycle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58097) | true cycles the texture if the texture is bound and load_op is not LOAD |
| <a id="api-61fb8395a36ffa53"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetInfo.CycleResolveTexture`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58102) | true cycles the resolve texture if the resolve texture is bound. Ignored if a RESOLVE* store_op is not used. |
| <a id="api-e76fc4279d73f233"></a>[`public uint Inno.Native.Sdl3.SDLGPUColorTargetInfo.LayerOrDepthPlane`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58062) | The layer index or depth plane to use as a color target. This value is treated as a layer index on 2D array and cube textures, and as a depth plane on 3D textures. |
| <a id="api-19afd85354d75bb4"></a>[`public Inno.Native.Sdl3.SDLGPULoadOp Inno.Native.Sdl3.SDLGPUColorTargetInfo.LoadOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58072) | What is done with the contents of the color target at the beginning of the render pass. |
| <a id="api-d3b3215c23bbe81c"></a>[`public uint Inno.Native.Sdl3.SDLGPUColorTargetInfo.MipLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58057) | The mip level to use as a color target. |
| <a id="api-11513ef3a254110a"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetInfo.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58104) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-1c6a67545fd5e8cf"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetInfo.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58106) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-2cac32bae676cc34"></a>[`public uint Inno.Native.Sdl3.SDLGPUColorTargetInfo.ResolveLayer`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58092) | The layer index of the resolve texture to use for the resolve operation. Ignored if a RESOLVE* store_op is not used. |
| <a id="api-5c711ddbe24f7e8c"></a>[`public uint Inno.Native.Sdl3.SDLGPUColorTargetInfo.ResolveMipLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58087) | The mip level of the resolve texture to use for the resolve operation. Ignored if a RESOLVE* store_op is not used. |
| <a id="api-c28a390efe102a39"></a>[`public Inno.Native.Sdl3.SDLGPUTexture* Inno.Native.Sdl3.SDLGPUColorTargetInfo.ResolveTexture`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58082) | The texture that will receive the results of a multisample resolve operation. Ignored if a RESOLVE* store_op is not used. |
| <a id="api-49a890b5cd4d2dd4"></a>[`public Inno.Native.Sdl3.SDLGPUStoreOp Inno.Native.Sdl3.SDLGPUColorTargetInfo.StoreOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58077) | What is done with the results of the render pass. |
| <a id="api-11164400bcfba14b"></a>[`public Inno.Native.Sdl3.SDLGPUTexture* Inno.Native.Sdl3.SDLGPUColorTargetInfo.Texture`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58052) | The texture that will be used as a color target by a render pass. |
| <a id="api-0eba0d2c200cea87"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_ALWAYS = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58178) | The comparison always evaluates true.<br>常量：`8` |
| <a id="api-f9353131988b5885"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_EQUAL = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58156) | The comparison evaluates reference == test.<br>常量：`3` |
| <a id="api-b24c8c088eaedddf"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_GREATER = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58166) | The comparison evaluates reference > test.<br>常量：`5` |
| <a id="api-e68b0f1f4739eb27"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_GREATER_OR_EQUAL = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58174) | The comparison evaluates reference >= test.<br>常量：`7` |
| <a id="api-2fee4cc13e10350b"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_INVALID = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58142) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-3a4f03b8b928ee59"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_LESS = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58152) | The comparison evaluates reference < test.<br>常量：`2` |
| <a id="api-dce9c9abd51d46e6"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_LESS_OR_EQUAL = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58162) | The comparison evaluates reference < = test.<br>常量：`4` |
| <a id="api-685eaa796e295ba8"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_NEVER = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58146) | The comparison always evaluates false.<br>常量：`1` |
| <a id="api-32b05b622374d2cd"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.SDL_GPU_COMPAREOP_NOT_EQUAL = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58170) | The comparison evaluates reference != test.<br>常量：`6` |
| <a id="api-ae4f6280f5f55c91"></a>[`public byte* Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Code`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58230) | A pointer to compute shader code. |
| <a id="api-95047c173a43c213"></a>[`public ulong Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.CodeSize`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58225) | The size in bytes of the compute shader code pointed to. |
| <a id="api-b20ddd55e6aa2734"></a>[`public sbyte* Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Entrypoint`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58235) | A pointer to a null-terminated UTF-8 string specifying the entry point function name for the shader. |
| <a id="api-0e9d469202517fb5"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Format`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58240) | The format of the compute shader code. |
| <a id="api-672d71b5b6340c9d"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadonlyStorageBuffers`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58255) | The number of readonly storage buffers defined in the shader. |
| <a id="api-5670b007f16c1836"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadonlyStorageTextures`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58250) | The number of readonly storage textures defined in the shader. |
| <a id="api-3ed72d94d5f410a5"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadwriteStorageBuffers`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58265) | The number of read-write storage buffers defined in the shader. |
| <a id="api-dc4fd09c38067b45"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadwriteStorageTextures`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58260) | The number of read-write storage textures defined in the shader. |
| <a id="api-fe37bb0e6089e0ec"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumSamplers`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58245) | The number of samplers defined in the shader. |
| <a id="api-e94e446e94c76734"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumUniformBuffers`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58270) | The number of uniform buffers defined in the shader. |
| <a id="api-0e9bc91184212ef6"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Props`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58290) | A properties ID for extensions. Should be 0 if no extensions are needed. |
| <a id="api-ff9944cf1fe5663c"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.ThreadcountX`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58275) | The number of threads in the X dimension. This should match the value in the shader. |
| <a id="api-9f75e9689e03c59a"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.ThreadcountY`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58280) | The number of threads in the Y dimension. This should match the value in the shader. |
| <a id="api-dfc315c66bdd8747"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.ThreadcountZ`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58285) | The number of threads in the Z dimension. This should match the value in the shader. |
| <a id="api-7be6e5a3ffe7af39"></a>[`Inno.Native.Sdl3.SDLGPUCubeMapFace.SDL_GPU_CUBEMAPFACE_NEGATIVEX = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58326) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-8600c1885f3deed0"></a>[`Inno.Native.Sdl3.SDLGPUCubeMapFace.SDL_GPU_CUBEMAPFACE_NEGATIVEY = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58328) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`3` |
| <a id="api-4fad971ee504ddfd"></a>[`Inno.Native.Sdl3.SDLGPUCubeMapFace.SDL_GPU_CUBEMAPFACE_NEGATIVEZ = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58330) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`5` |
| <a id="api-91ddefb0671a9409"></a>[`Inno.Native.Sdl3.SDLGPUCubeMapFace.SDL_GPU_CUBEMAPFACE_POSITIVEX = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58325) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-713ec06f9efe48ac"></a>[`Inno.Native.Sdl3.SDLGPUCubeMapFace.SDL_GPU_CUBEMAPFACE_POSITIVEY = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58327) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-fe07105d9241a240"></a>[`Inno.Native.Sdl3.SDLGPUCubeMapFace.SDL_GPU_CUBEMAPFACE_POSITIVEZ = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58329) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`4` |
| <a id="api-adaec576cfdc14a8"></a>[`Inno.Native.Sdl3.SDLGPUCullMode.SDL_GPU_CULLMODE_BACK = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58359) | Back-facing triangles are culled.<br>常量：`2` |
| <a id="api-899cb48d79b96aaa"></a>[`Inno.Native.Sdl3.SDLGPUCullMode.SDL_GPU_CULLMODE_FRONT = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58355) | Front-facing triangles are culled.<br>常量：`1` |
| <a id="api-4be6a62883d2c7c2"></a>[`Inno.Native.Sdl3.SDLGPUCullMode.SDL_GPU_CULLMODE_NONE = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58351) | No triangles are culled.<br>常量：`0` |
| <a id="api-9c974ae4731ac100"></a>[`public Inno.Native.Sdl3.SDLGPUStencilOpState Inno.Native.Sdl3.SDLGPUDepthStencilState.BackStencilState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58389) | The stencil op state for back-facing triangles. |
| <a id="api-4335843960ae8585"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.CompareMask`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58399) | Selects the bits of the stencil values participating in the stencil test. |
| <a id="api-bb024dc262910bde"></a>[`public Inno.Native.Sdl3.SDLGPUCompareOp Inno.Native.Sdl3.SDLGPUDepthStencilState.CompareOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58384) | The comparison operator used for depth testing. |
| <a id="api-725f56de512611bd"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.EnableDepthTest`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58409) | true enables the depth test. |
| <a id="api-13f03fe6b2ce3452"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.EnableDepthWrite`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58414) | true enables depth writes. Depth writes are always disabled when enable_depth_test is false. |
| <a id="api-3879406e4cefcc50"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.EnableStencilTest`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58419) | true enables the stencil test. |
| <a id="api-8ff589fda47eed30"></a>[`public Inno.Native.Sdl3.SDLGPUStencilOpState Inno.Native.Sdl3.SDLGPUDepthStencilState.FrontStencilState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58394) | The stencil op state for front-facing triangles. |
| <a id="api-09324ff27cd87935"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58421) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-e0f354a746691e5a"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58423) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-63e2fb1097c0657c"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.Padding3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58425) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-a6524f13ce6a9be7"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.WriteMask`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58404) | Selects the bits of the stencil values updated by the stencil test. |
| <a id="api-caec6c8e385a32ce"></a>[`public float Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.ClearDepth`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58491) | The value to clear the depth component to at the beginning of the render pass. Ignored if SDL_GPU_LOADOP_CLEAR is not used. |
| <a id="api-163250aa57456c1f"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.ClearStencil`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58521) | The value to clear the stencil component to at the beginning of the render pass. Ignored if SDL_GPU_LOADOP_CLEAR is not used. |
| <a id="api-43dd376013531022"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.Cycle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58516) | true cycles the texture if the texture is bound and any load ops are not LOAD |
| <a id="api-bca61a4ce50e305a"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.Layer`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58531) | The layer index to use as the depth stencil target. |
| <a id="api-be4ae3a3fe71c906"></a>[`public Inno.Native.Sdl3.SDLGPULoadOp Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.LoadOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58496) | What is done with the depth contents at the beginning of the render pass. |
| <a id="api-46c3d97e7a64750f"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.MipLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58526) | The mip level to use as the depth stencil target. |
| <a id="api-5a0df9c6ed686dcc"></a>[`public Inno.Native.Sdl3.SDLGPULoadOp Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.StencilLoadOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58506) | What is done with the stencil contents at the beginning of the render pass. |
| <a id="api-2f2bfcd70c1e12d8"></a>[`public Inno.Native.Sdl3.SDLGPUStoreOp Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.StencilStoreOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58511) | What is done with the stencil results of the render pass. |
| <a id="api-77a5852246713489"></a>[`public Inno.Native.Sdl3.SDLGPUStoreOp Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.StoreOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58501) | What is done with the depth results of the render pass. |
| <a id="api-54d6fe3a47440f3d"></a>[`public Inno.Native.Sdl3.SDLGPUTexture* Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.Texture`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58486) | The texture that will be used as the depth stencil target by the render pass. |
| <a id="api-e9b6f22eb1344c97"></a>[`Inno.Native.Sdl3.SDLGPUFillMode.SDL_GPU_FILLMODE_FILL = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58581) | Polygons will be rendered via rasterization.<br>常量：`0` |
| <a id="api-840607b391b8c90c"></a>[`Inno.Native.Sdl3.SDLGPUFillMode.SDL_GPU_FILLMODE_LINE = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58585) | Polygon edges will be drawn as line segments.<br>常量：`1` |
| <a id="api-8d77a713f448789c"></a>[`Inno.Native.Sdl3.SDLGPUFilter.SDL_GPU_FILTER_LINEAR = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58610) | Linear filtering.<br>常量：`1` |
| <a id="api-14f050a90447516f"></a>[`Inno.Native.Sdl3.SDLGPUFilter.SDL_GPU_FILTER_NEAREST = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58606) | Point filtering.<br>常量：`0` |
| <a id="api-cb3094de06ab4d4d"></a>[`Inno.Native.Sdl3.SDLGPUFrontFace.SDL_GPU_FRONTFACE_CLOCKWISE = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58636) | A triangle with clockwise vertex winding will be considered front-facing.<br>常量：`1` |
| <a id="api-4d81843b201a5ded"></a>[`Inno.Native.Sdl3.SDLGPUFrontFace.SDL_GPU_FRONTFACE_COUNTER_CLOCKWISE = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58632) | A triangle with counter-clockwise vertex winding will be considered front-facing.<br>常量：`0` |
| <a id="api-c6d5951ef93d04fe"></a>[`public Inno.Native.Sdl3.SDLGPUDepthStencilState Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.DepthStencilState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58708) | The depth-stencil state of the graphics pipeline. |
| <a id="api-257b0b445eda67ec"></a>[`public Inno.Native.Sdl3.SDLGPUShader* Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.FragmentShader`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58683) | The fragment shader used by the graphics pipeline. |
| <a id="api-9f31f824128f656d"></a>[`public Inno.Native.Sdl3.SDLGPUMultisampleState Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.MultisampleState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58703) | The multisample state of the graphics pipeline. |
| <a id="api-d9cf5840dd9fe140"></a>[`public Inno.Native.Sdl3.SDLGPUPrimitiveType Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.PrimitiveType`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58693) | The primitive topology of the graphics pipeline. |
| <a id="api-678417503d8a59f3"></a>[`public uint Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.Props`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58718) | A properties ID for extensions. Should be 0 if no extensions are needed. |
| <a id="api-a08e2189312d1013"></a>[`public Inno.Native.Sdl3.SDLGPURasterizerState Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.RasterizerState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58698) | The rasterizer state of the graphics pipeline. |
| <a id="api-05aaebcde82282ad"></a>[`public Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.TargetInfo`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58713) | Formats and blend modes for the render targets of the graphics pipeline. |
| <a id="api-0d900ef2e786d226"></a>[`public Inno.Native.Sdl3.SDLGPUVertexInputState Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.VertexInputState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58688) | The vertex layout of the graphics pipeline. |
| <a id="api-6b2db7963bf9728b"></a>[`public Inno.Native.Sdl3.SDLGPUShader* Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.VertexShader`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58678) | The vertex shader used by the graphics pipeline. |
| <a id="api-c78fd011e1167f27"></a>[`public Inno.Native.Sdl3.SDLGPUColorTargetDescription* Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.ColorTargetDescriptions`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58752) | A pointer to an array of color target descriptions. |
| <a id="api-e2204b325307818c"></a>[`public Inno.Native.Sdl3.SDLGPUTextureFormat Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.DepthStencilFormat`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58762) | The pixel format of the depth-stencil target. Ignored if has_depth_stencil_target is false. |
| <a id="api-2f41456f13b234a1"></a>[`public byte Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.HasDepthStencilTarget`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58767) | true specifies that the pipeline uses a depth-stencil target. |
| <a id="api-4f1bb746f2691bf6"></a>[`public uint Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.NumColorTargets`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58757) | The number of color target descriptions in the above array. |
| <a id="api-ee6173c73b2347f7"></a>[`public byte Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58769) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-35dd2c9bb79258ec"></a>[`public byte Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58771) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-efbf8e2361147910"></a>[`public byte Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.Padding3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58773) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-aea2c566aad87856"></a>[`Inno.Native.Sdl3.SDLGPUIndexElementSize.SDL_GPU_INDEXELEMENTSIZE_16BIT = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58858) | The index elements are 16-bit.<br>常量：`0` |
| <a id="api-e92efb7363c75f86"></a>[`Inno.Native.Sdl3.SDLGPUIndexElementSize.SDL_GPU_INDEXELEMENTSIZE_32BIT = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58862) | The index elements are 32-bit.<br>常量：`1` |
| <a id="api-ef7de4ec61df4c1b"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.FirstIndex`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58820) | The base index within the index buffer. |
| <a id="api-041027202ef52e59"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.FirstInstance`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58830) | The ID of the first instance to draw. |
| <a id="api-897c854a57dcea2b"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.NumIndices`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58810) | The number of indices to draw per instance. |
| <a id="api-0be3f9088b4565b9"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.NumInstances`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58815) | The number of instances to draw. |
| <a id="api-3b581729636447f9"></a>[`public int Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.VertexOffset`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58825) | The value added to the vertex index before indexing into the vertex buffer. |
| <a id="api-bbbb37d776ee3469"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.GroupcountX`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58886) | The number of local workgroups to dispatch in the X dimension. |
| <a id="api-edbb1b015ff68843"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.GroupcountY`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58891) | The number of local workgroups to dispatch in the Y dimension. |
| <a id="api-a628c2141805c09a"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.GroupcountZ`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58896) | The number of local workgroups to dispatch in the Z dimension. |
| <a id="api-a7250d7aa95d516d"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndirectDrawCommand.FirstInstance`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58948) | The ID of the first instance to draw. |
| <a id="api-129a80d819b10a11"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndirectDrawCommand.FirstVertex`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58943) | The index of the first vertex to draw. |
| <a id="api-068da33b7e4a5454"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndirectDrawCommand.NumInstances`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58938) | The number of instances to draw. |
| <a id="api-b6e490f8c581f962"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndirectDrawCommand.NumVertices`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58933) | The number of vertices to draw. |
| <a id="api-37e0924027bd697d"></a>[`Inno.Native.Sdl3.SDLGPULoadOp.SDL_GPU_LOADOP_CLEAR = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58981) | The contents of the texture will be cleared to a color.<br>常量：`1` |
| <a id="api-7bfdaa7fe68e5268"></a>[`Inno.Native.Sdl3.SDLGPULoadOp.SDL_GPU_LOADOP_DONT_CARE = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58985) | The previous contents of the texture need not be preserved. The contents will be undefined.<br>常量：`2` |
| <a id="api-1406e510e8bf4f55"></a>[`Inno.Native.Sdl3.SDLGPULoadOp.SDL_GPU_LOADOP_LOAD = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L58977) | The previous contents of the texture will be preserved.<br>常量：`0` |
| <a id="api-5d3a4509c0c25355"></a>[`public byte Inno.Native.Sdl3.SDLGPUMultisampleState.EnableAlphaToCoverage`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59025) | true enables the alpha-to-coverage feature. |
| <a id="api-1f6f71d5fe16e39c"></a>[`public byte Inno.Native.Sdl3.SDLGPUMultisampleState.EnableMask`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59020) | Reserved for future use. Must be set to false. |
| <a id="api-1f2005a9b26eb66c"></a>[`public byte Inno.Native.Sdl3.SDLGPUMultisampleState.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59027) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-cde7e40ca576d1ff"></a>[`public byte Inno.Native.Sdl3.SDLGPUMultisampleState.Padding3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59029) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。 |
| <a id="api-3530f146832a0986"></a>[`public Inno.Native.Sdl3.SDLGPUSampleCount Inno.Native.Sdl3.SDLGPUMultisampleState.SampleCount`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59010) | The number of samples to be used in rasterization. |
| <a id="api-09c013ade37b95fc"></a>[`public uint Inno.Native.Sdl3.SDLGPUMultisampleState.SampleMask`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59015) | Reserved for future use. Must be set to 0. |
| <a id="api-54ab190813a82cb5"></a>[`Inno.Native.Sdl3.SDLGPUPresentMode.SDL_GPU_PRESENTMODE_IMMEDIATE = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59070) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`1` |
| <a id="api-6876b5ec81d598a0"></a>[`Inno.Native.Sdl3.SDLGPUPresentMode.SDL_GPU_PRESENTMODE_MAILBOX = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59071) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`2` |
| <a id="api-2490fdaa9b87ae49"></a>[`Inno.Native.Sdl3.SDLGPUPresentMode.SDL_GPU_PRESENTMODE_VSYNC = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59069) | 当前生成的 ABI 声明；地址、缓冲区及回调的有效期由所属原生 owner 保证。<br>常量：`0` |
| <a id="api-22578c8297079224"></a>[`Inno.Native.Sdl3.SDLGPUPrimitiveType.SDL_GPU_PRIMITIVETYPE_LINELIST = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59111) | A series of separate lines.<br>常量：`2` |
| <a id="api-e57e80db36780710"></a>[`Inno.Native.Sdl3.SDLGPUPrimitiveType.SDL_GPU_PRIMITIVETYPE_LINESTRIP = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59115) | A series of connected lines.<br>常量：`3` |
| <a id="api-93fb5027ec470f0c"></a>[`Inno.Native.Sdl3.SDLGPUPrimitiveType.SDL_GPU_PRIMITIVETYPE_POINTLIST = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59119) | A series of separate points.<br>常量：`4` |
| <a id="api-73496cde91bf8016"></a>[`Inno.Native.Sdl3.SDLGPUPrimitiveType.SDL_GPU_PRIMITIVETYPE_TRIANGLELIST = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59103) | A series of separate triangles.<br>常量：`0` |
| <a id="api-205f700f73a87ed8"></a>[`Inno.Native.Sdl3.SDLGPUPrimitiveType.SDL_GPU_PRIMITIVETYPE_TRIANGLESTRIP = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59107) | A series of connected triangles.<br>常量：`1` |
| <a id="api-de17dfcf19b126e6"></a>[`public Inno.Native.Sdl3.SDLGPUCullMode Inno.Native.Sdl3.SDLGPURasterizerState.CullMode`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59155) | The facing direction in which triangles will be culled. |
| <a id="api-aed9bc9f39525493"></a>[`public float Inno.Native.Sdl3.SDLGPURasterizerState.DepthBiasClamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59170) | The maximum depth bias of a fragment. |
| <a id="api-185ce225504b478f"></a>[`public float Inno.Native.Sdl3.SDLGPURasterizerState.DepthBiasConstantFactor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59165) | A scalar factor controlling the depth value added to each fragment. |
| <a id="api-cb0bfad77376d80d"></a>[`public float Inno.Native.Sdl3.SDLGPURasterizerState.DepthBiasSlopeFactor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59175) | A scalar factor applied to a fragment's slope in depth calculations. |
| <a id="api-8c9057d18f87a79c"></a>[`public byte Inno.Native.Sdl3.SDLGPURasterizerState.EnableDepthBias`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59180) | true to bias fragment depth values. |
| <a id="api-309a95a6ef4d3b9b"></a>[`public byte Inno.Native.Sdl3.SDLGPURasterizerState.EnableDepthClip`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59185) | true to enable depth clip, false to enable depth clamp. |
| <a id="api-174b60bffb24810f"></a>[`public Inno.Native.Sdl3.SDLGPUFillMode Inno.Native.Sdl3.SDLGPURasterizerState.FillMode`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L59150) | Whether polygons will be filled in or drawn as lines. |
