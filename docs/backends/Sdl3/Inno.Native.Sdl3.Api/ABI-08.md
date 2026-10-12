# Inno.Native.Sdl3 ABI · SDLEvent → SDLGamepadType

[项目与生命周期](../Inno.Native.Sdl3.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.Sdl3.SDLEvent</code> · 169 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLEvent fd23cae8d85f46848a7dca528d18f70e9ff7c994e8a4a3ab8ad8d3a22178268d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Type 6b77f804de865ae79146a25f8d87aa56c734f16af8291b5ed3c9091c0aac1a24 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Common 1a0809d1a51456d1e769281a86afe4ec571fe2186222adb044d8e0d0d810098a -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Display bd1a41f4ea90ec6a297118e059eeae9d8fa9deebbe7d2b29b6fbeca3659b27e4 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Window e5e30717af46915fcd2d3a805201871e85870b40b43c3362a52c1a6daa25a1ad -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Kdevice f3251f179ea267fda09290bc734f6962f84a4cb3246110f9f1a64f849250c64f -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Key ee57ee6c863c822edbe5aef1fabb57e148b6e9f26ca3d7ad40f144c0214c6a2e -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Edit 4c6c5ead7bfc5789bf3d1d4587c6e7785792b932f2260e57579d74fabecb39dc -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.EditCandidates f2f31ff00801982f7ba006d50419c30c0a9e24f21030879f0ffcf8ecafa75d1a -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Text c94b94d438e054fa0dd3e79f9e4a2442d37227d292d9b43271a0b2011ad74e2d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Mdevice e527f94c749b267c3b45fc2725b463cb0290cd38a76e0fcfb61bcce87497bb1f -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Motion 21236ac234f1703bda8c6fd5e0202f4844947f78d0d0df8210f4ef340acff27d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Button 459f8815e67a6214fcb733f7706b5544d3be9ebadde92d1e8a944cdd4e956f93 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Wheel 40a85765e289593052ad17e9fffa0f335803ce8f886b8992838853f48ed9d802 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Jdevice a04c2506eb00dd9599aac97f77367ac877d47f8130fea74c2d855421eb9bc3f7 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Jaxis 51b37ca7f4d3c37bcf427e997625343db50ec67c69a615d2f4b58676535f686d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Jball 69290df68d0d577979009aa54a159b2d1764fe135f230dfe12dfb8256dccf359 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Jhat 4f2f4768ad9e4bd8a748f84ef35ae2707d5c2edaa06682e66d96718c45cdc961 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Jbutton 5bc2e8169c9491d1d54c663a7045b949fb97f22eb45a699f895aebf039f10e5c -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Jbattery 0f99279957181f36a0fa868de8a091c77bc4c06ae23bbd4fed7ede6aa5632d69 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Gdevice 70aa440b6921720a21ad387151488e0673c6e494d77a4673c6e4b6f91e6a8cd6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Gaxis 09023029d9a81a50b4213a9d6c059591b2845a2bd66e10a730b8a4375a1a22f9 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Gbutton 7d482f4ac52d1a99e186d8e4da770bf56e1eb4c33c1bbfedf50ac48d15223457 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Gtouchpad 5ff7f01efad56fecede988048be677fcf4da89bea4f8e92ecd244840616c8bb8 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Gsensor 28364d8beef88f5df30ef2f91117920146d89bf4cca35809f3167515de8662b1 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Adevice 0fdb1b7988d9878a1bfc67b02417110fa141615e09f541492ecde23c25f57789 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Cdevice 2ceacaa567e4d4600d9955881aacd38fba14791a2333bef05a73d087f10e12b7 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Sensor 1f3118859d2c77a5d929a30e1af01680849826deb4e53f4f4c72d9a7dd645e45 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Quit a563948fd9ed59fd38c18a43e0937d7edd30bcc98c53f3b2bdf8c04f7d7c3e18 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.User cb8a411707019d9f1b2b5353fd30887ddc4a2466ebea4991abbe452b9b8890a3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Tfinger 19e9e508b1087b45433c603140874a1b0dc090a9ca09aeb7f1223c19c8e25621 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Pinch a4dddc9fa50f33669bc1696487b887120502c330d721f9beccd96d60f9b4064d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Pproximity b8aad60bde3da43e73f4e52a35c2a2f1add81dc9119d33f152e4dec88f92f831 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Ptouch 0763deca418882e48d325590c4b4d57c2a513d6ce18d6f354ef2eb5f80e0b989 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Pmotion a4c4f8a68a3aae2ee6df8db00bea77bae50b53bf1614b3bc2da47b562c2ed67c -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Pbutton cd22f263585a02d0865fd4c99daed2fd0690dcfc7b2d6cbf1adbb9f478d6f5ec -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Paxis bb750e6fbb81f482e76578dc4161f49bb15c544d479c54c19f52f89323f1a7a1 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Render 005c6472b365e6f384f03f87112f231c3ae825d4b526342d56ee51521656bea5 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Drop 5f97cee59c910f90933dc25b5f1ea6d104b6709989396c6d1ab5cb91fcda72ce -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Clipboard 24d669f935db8aaff77388ffe967d00dc4df7fa397b739486f4c0ae35fb51071 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_0 c81c417fca12e4f0acebd1df21437627faea7595fe21135e08cc0dcbb097dee7 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_1 4783048d5e1d81062b2771741952c521c7a5ee4dc538d0c501e1ad09efac8a85 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_2 1f98781e671c084d3c0e52aca160394eecfe54c24fc14d0307189a2e1694f158 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_3 e25bef9ee9ddbb52e6ebe8b22bbcf2d519f8a1c842f807586b614b9cb5877aef -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_4 c0d252049be37d400139f1e14a92df6ad06f1eb16bc7dc837e9e389fc4c1c8e1 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_5 86f5817b6f4a8542d87f10b8d0c60b5315ce020c90d1a02445805f246d099d89 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_6 679019063a3027e7eba3dc213884545b580aaeb65aa0b6ee9d42a63269bafef6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_7 3b518a19d5d4731c2c3beb2a3509a74e52a56b134e743af91b846b123f7ce3e3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_8 159ef9ac669edcbd050777f06178187845e52168769b7f6d38a2e5d9594d8884 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_9 17aae8766eaa51c21d4aa7ff2786e6aafd8033e70db66127f161715f1eba566f -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_10 d4d57c18d6fe7d330c4b0c9397c9fc73fed1b9e54de3b69c30421ce1c9d5c60b -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_11 a1f91b2bca63b0b7df416ad9b9db18397aafbb72d048fcd9cedd710d3cb281ee -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_12 16e443bc772be4ed465c204b9570f80c7209360211ef014bb52a5133f84f5777 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_13 c36117c0aede31814080a1bc0be9ca1efa9db7066b20312a91856b3c4a213a0d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_14 89f600a16e9ca7b9b9a1e435645c8a91a36d36e83eae967c78ec5a0c33a851da -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_15 6f82529130eb89c59739f956832d952e8b89fac77ef665f7c6493b464eeeac36 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_16 2512430696e17af5109b42c3fb21c41988f4bec5dcb3576d5367ac99ac1bb641 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_17 17383c55a943ee03ab592695249021b3991d97367ae1c60bb5180171b40fd9ce -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_18 914aa44bb44f959defe2a0567a640625966786c4570762fd83ef8b8737084f5c -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_19 5cd426439e770c1f1753dd22d8916528f9571fdec2d6f7186ba7050f839cf49b -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_20 c3682b6fef13eb8242f9a5ef2a5c7e075f55b380c9e882e06025984412ccf099 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_21 8959c2225cec953cd1efab3d3ab5adb07b0f80069d2f059cfeaa815d9d551691 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_22 91e4df175e6b954e8aab165e4c0b6593d0eb6a88648e8ee98f5a509d90b652fe -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_23 b1306407843f74519c6c2faef41b247945d2d18d958e1d2eb78462fc77e44749 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_24 bead35e677e26858a87b0fc154bf71ddd51ae70f05024b1f17d49a2c6097899c -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_25 021938488c0245b10ff9d88c32bb745fa55a278c8e9d0f6cee194e95ed58324f -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_26 a33cb2a2a1b7b16ab07f4546f76c3c44aa8ea751409fc56fb1b7aae95f97c00d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_27 de0b5b268085ad3ddd8c3758c5d46371e18430900ae97c4bcb49cf13a26dcc16 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_28 37d39e9b1a51156a1851bba2f140aa1367cadfb9d47ea2af46ec142abab85538 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_29 00140f7e04c4a816f3c1533dc15b1e2fe02f1646e4d6631af87354f0bb0ef71a -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_30 6d0a39a87ed2bba9558f1239fe2c3a88f9e8f8b115753921d8216a6304128579 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_31 1e7cb36f0764a8317880ca0f6e2b7b85af216ff0efe71370f90ed7532d577dbd -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_32 221332da4e685a0be44fb59d9b9f2d0877e4034feedc471db063a8b9f413d813 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_33 8164518d7ce5ed2daed190b95404a6cc15f793ad4c2aec0a396a44da2527774b -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_34 eea3cbd44f49d07a1a2841730a0e068aceb992f6f908baded325ad045a29954b -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_35 cd3aa00c9e7a087a37f10aec6b050bd3ea2e288bfb915cc6c13371e3c50be9ae -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_36 72513e1cf47086610d3ba69579e541bf88484d7defd65d17bf81879346a0537d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_37 c6eb6f9ea5c9f3e1516b7e9a75c9b3ba026d61368f344f03377427bf47d86b66 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_38 5e6dfa9ff291998ee64e8fda129196af63a30f65514f665691a118525ca5f6f6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_39 fdefc5b7223b506a469be2c9df9a43ba33b2e51b7992d34939dc6ab34f6f3883 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_40 e3a95c3d39f8b46fa357c86338488d57f378e48ed30f32c620b4963aaa0ec695 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_41 567b1a2f008716932385a89c5fcc1f6556f696939e62d41143fff6470b348db5 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_42 a92bddcfa4f8fde484c3b8e14edc02f3d85af6d29ae1a28540fb1e4115182ae1 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_43 76d834cdac7f74491bda4f28f59d974999eaaebe23704c122b31538aa645dc7f -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_44 515cd8e8a76d1729ec988a549e08c61cad07f30375e3291d65a12a5c7fa330d4 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_45 37e06fbab157d94aa21df6b348a669168a5a96139f7fb2f8d2f8016428085025 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_46 3cc02c7e4a66f26acbd63636744e6d58d80be757bee05974e6c1a646b94fa7fc -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_47 d3ac8a04d8b3895f999aba634959240133623374e6b3b14f6cfdfb170115172a -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_48 b3924035af605699d9e697fda8533a3b0875b145ce2c4ead0fd339550c05f399 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_49 ae5975d690811d81e268c604a4e26e68cca0d1c0c52deb71b8293911f9f514a7 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_50 2625b714e6c519cfb0ea76e8722a293d46ebf599ca5cbea9e7bca33ed2bdaf99 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_51 fb8dcb14c9e3bdf90faa53f2f99d000a7abf1f5f26148f9e4b0d01f16360ed57 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_52 7fb7f78f5f6d3d379029c1b6a9bfd4ff307cff673d64fc6df478d7e19b4dc6e1 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_53 0c606f7926d2fe23378698aeba9584be74c0344808dad2dcb0fa25579e5c9602 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_54 250ad5ba1a2531b43a6649428ded52b3f24936ab640c2129d2b15cfee152c3ee -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_55 001c526c733678b7a2ff1522ec123cbf0a9a54ef0c15ce1bbe7869cac0063226 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_56 c9e679c45927f47da1bb6c90056c2a4343a35539f2c06fd6298f1c9bc0587aaf -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_57 6cf42fa5b76ea05eb6d979b9dc7373e66193d97f952e728bfc942366e0cc1d14 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_58 b0d9ec7ece0748e9deeab816e1cb65f8dc3e56b138528803587bbba357e6f448 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_59 c34f634b60e4c1f0b046db63e6a22ed9b5e8ffe0a573b8aedb3cc24b80780097 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_60 e6dbbd22a31590d80229954e02e476fdbef9d21fadf7f3abaad06884725f5a28 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_61 213635850fd2d5403ac3d9ad5966f14eba4e036ad59941fad5c24a4c290ef290 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_62 a04d9c64ecb0dbc6157914a702edd63c0c46dbce41f80281f9021e608da3b752 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_63 ef1ca249c1327eff448a7e5faf0f0ad2c5c6801b74c34eb22ca38202dabccfc2 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_64 e92dc1b2e3024951bb1cb7016343040ef26abad1f9a2e278ff183fa617ef3404 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_65 717a46b39f35442f8dd7e325e527fecb6c0deea3eef82a0c92ae525262c01fb0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_66 c07211c400aa86695a9887ccceee7b4fb0af4c445403946d8e87dfd65e6937b5 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_67 96e1e8196cf7e5d3814dec7037d800a35eacf6fbec99c1d00fc286287f3952cd -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_68 7499187bd27e8d14590434ad4a3506f44e21657aa0282fa23a48e3cde78032ee -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_69 1a85e7195b379b254a00d52eb46a7d8e78b063bf13f901879123bd71c9fd6ad3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_70 bbfb39fa08054ca9cb21706f7769ec7460a5bc23ca19effa644b24708be7d0da -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_71 29fdce0f6c5581825691456ddb92145100297bbff05277cbaf08fa86bb77c012 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_72 1be172338f80c9971818f5415407fdd8b598c0ca7fdfed441686e05e9d2cfe99 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_73 cc23dd030777841b0331b00345b4090d52bb2fa6af22cbbc737e2b0b4f50cd91 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_74 f7fe4a158dcdcb1a28010bf101e75dfb6980fbc73f1e862007101258156ac459 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_75 880a2750459c60e5bb8183a032562761156834d02bcf3af753478bc65d2fad33 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_76 64b59477e57dceea35825e7c9daa0c0b7e664eac42904625bcab033a879431b6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_77 3150ff3e8c97df620c34d87adef1f98c49faefe3a402bb196a5527ece658df17 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_78 b8b416785051a837f49c4431b0684b68961301c82641179ac43ee4227420029a -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_79 1591287e501c4e512def7ab25aed45b94ac73ba89c9f42c049256fb1c8c7b87f -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_80 a91c4bab7e13932ebe7d819850b7902f658018406cca576c37c35ddb519f2737 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_81 6b2b1f0c99b4ddfcf3e31ed35d8baa9fa223f725739159472ffa2c646430d02e -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_82 f158f1dd4431b554003fa84648fd00a4b24d22432da29761cd24abb96280da94 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_83 a9a21155323aaf8b2368de39114279c63f4ef6cdb56a004f9ea9437e52dc45a6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_84 70b54b133fe8dfa4e25c3633fd4696d3e2500e11364bb3bca7f0e92a60a1ab29 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_85 e8fc4afaf15522956210030431b5caeaa08e4e545a78361d5865514db6d55bd7 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_86 236a88134f0e84caab7e49d93043832269501e6f171fc5a7b87f0be2f11ecbe9 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_87 b10275b453c4f093cca0f6046331e5adad2a9748296edf40c0efb97d5ac25590 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_88 a51eafcfa87fa13e3a9eaed908b680208d9588b1c6db3cc5ff8212df3a8ab95d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_89 2898de4f70727176d1ce0fec6ea801b778509f4920884baafe70f30ed2abe50b -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_90 964f32a6a8d3d27ccef3ed04532b09fae6c49657e75a3dfc7a45874d984b5c47 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_91 a0f115d2052d25b00557380fbc308fe174e11a00e41a9efa801aee9c5354068b -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_92 c1eb7a65ad984cda2116169370eef084c00c69dd8ad314cf76282db7e48348ca -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_93 b66b9eaeeb9ad246e75b28657a6bb32df02eba1c2910e24b69bf2f327322aab9 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_94 46381e634eedf69aa14cca0ea1b96d2da32bbf6be381b97f0bb06e144e132479 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_95 94a0322e3287fef246dbab2fb32f387738f3f2d839f7c3a7062f1fcdf9963e8a -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_96 2f1561e80f3696fc1564d008b139d4edcb7c04a47e4a253da89aedf971d6d678 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_97 d5265162185d0a591c3e707d99cf9bf930b987fc95894d08064dcd13c93a6717 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_98 62c30c084a3358b45bc41fe5b71d7f4bc00235e1df277ec9a1ff49dee175a2f9 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_99 4644818e56c7f2c4b6619eb7e2ef4178786ea8eb8592cc1f6b9c0cc775c89030 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_100 fc970fcab3ad7fab799647fb19a06e1f427798b4de3aaf3f20f0aa4936a8c00b -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_101 6638c68c86221fbbdb2ab974544d283d6e677c0017f292d8fc959af38d250fcb -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_102 9af59361321b3ab9ebaaf249650dbbc06d46ccdf92ebebb9e2aa3fa68968d364 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_103 aff3463336d329df162f1fa5c40482a83b9dc6b305c495e00e4e0fa3301d3e43 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_104 5c66da2dbc6a3d32a1c8276dc8009a07c0ee390d4cc36557bdb7752a1b54d7c0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_105 a4bfb25c29d7693d6b647228f5d5cfd752f13637204d040ef146e66583bbf9a9 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_106 8e594f84c407b48d45bfe767b641780ccf8e79c02d5f74335fbb3a9e960498d5 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_107 e119709f26a7626f2adc2aeb08e790ec3722047759d87a6da3ef85e8f6db202f -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_108 f67da45ade677e4613b080f25052f94f6f6a1331dfc7ebed9fe6fe3c1e4ad153 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_109 81668d2d0fdd735c809cf0e6d6431d25fa1219eca99815d24bc355bee0c7f9e5 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_110 ec6e1433d7b38c2b1d0e1331d7df05c43590ed04ed8b04dc8587048400078c63 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_111 fef789b5e5d6a64fec88072c553d4a24dfd43526154983b5d8f89de5fddaafc4 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_112 5ab02d89bbe70315f61d73f6306472564e56f6c4e7ca1ee9e03644c1faa36f2b -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_113 2adf96be02916a7e2315a05f2b3db7fd8f21ae7ccd8151abc8710d7328e2f57d -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_114 149859b17dc6dd524cd7ae24a7481df32f02758654b03acca0e23ccecb547730 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_115 937fba087ff25aec4f3fbeaf81c406b9bff21c8e9a55694ae877bb10b89227a8 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_116 4378f257f927d4acdb34c34677600357dd145e774f21e1ea61fb73156c9c2dbf -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_117 f3859a03d7e76e412d8511cfa54a114d8ddf4386df84e309a30b4bd40242299e -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_118 f1edc25da8f1331d3aef1090af0cba70f968a7110ae095c9b7edcc21fe571eb0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_119 2522b36caa9c21216f637ba61dfa01a2b10fef0769b447509ba73c4cdcb707fa -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_120 3872a903a9c4cfaf6b533edd406ca47bebbf9a589b5d4084843893d7e5df4223 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_121 bb0e2755274b82acfd2ae309af21eda754606783d81b446fd58209cb47d9ac0b -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_122 805bebcab69f710c58aceb1a391045c6374e32b693bca862393c81c79eeb38a9 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_123 0f39e6e780ab2c1ba29fff324a9318d4116ce21c4988dcc8f90da4a81cb48813 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_124 35757bb4c92a9c986973282bc7aa1297697d03f268d160b15f75e8766519d994 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_125 f88758a600f217952796cfe0fc7bbfd4e35f7a76921622fa0b530bc22a7c842f -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_126 ee20d791af51f7d63ede9b18917f05a226cb73f76924a5909049df43c5089c06 -->
<!-- api: F:Inno.Native.Sdl3.SDLEvent.Padding_127 ab4405b5c56f6b2cc842552fc6ee7ead05e0cfe03f0f1d51b6dd17b4b02e8923 -->
<!-- api: P:Inno.Native.Sdl3.SDLEvent.Padding 6991877feb60f5d0a88ac9f524d66990671dd0a90d63be5e869afb617ed2f211 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2f963fb977816b0d"></a>[`public Inno.Native.Sdl3.SDLAudioDeviceEvent Inno.Native.Sdl3.SDLEvent.Adevice`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4124) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1afd958d8df1518e"></a>[`public Inno.Native.Sdl3.SDLMouseButtonEvent Inno.Native.Sdl3.SDLEvent.Button`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4098) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be1cec3c212e531d"></a>[`public Inno.Native.Sdl3.SDLCameraDeviceEvent Inno.Native.Sdl3.SDLEvent.Cdevice`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4126) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-04cb991a82fb4f0a"></a>[`public Inno.Native.Sdl3.SDLClipboardEvent Inno.Native.Sdl3.SDLEvent.Clipboard`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4152) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f733b4b74c9bf661"></a>[`public Inno.Native.Sdl3.SDLCommonEvent Inno.Native.Sdl3.SDLEvent.Common`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4078) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-93d5eb839eefd963"></a>[`public Inno.Native.Sdl3.SDLDisplayEvent Inno.Native.Sdl3.SDLEvent.Display`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4080) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-98588abf24f966b4"></a>[`public Inno.Native.Sdl3.SDLDropEvent Inno.Native.Sdl3.SDLEvent.Drop`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4150) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-acd6c9faec9fafff"></a>[`public Inno.Native.Sdl3.SDLTextEditingEvent Inno.Native.Sdl3.SDLEvent.Edit`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4088) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e9aac588d5d4feb5"></a>[`public Inno.Native.Sdl3.SDLTextEditingCandidatesEvent Inno.Native.Sdl3.SDLEvent.EditCandidates`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4090) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-47656e25cf89cf88"></a>[`public Inno.Native.Sdl3.SDLGamepadAxisEvent Inno.Native.Sdl3.SDLEvent.Gaxis`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4116) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7d7578d901f62060"></a>[`public Inno.Native.Sdl3.SDLGamepadButtonEvent Inno.Native.Sdl3.SDLEvent.Gbutton`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4118) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8a8a7ba30437b98c"></a>[`public Inno.Native.Sdl3.SDLGamepadDeviceEvent Inno.Native.Sdl3.SDLEvent.Gdevice`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4114) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cb489f545829693a"></a>[`public Inno.Native.Sdl3.SDLGamepadSensorEvent Inno.Native.Sdl3.SDLEvent.Gsensor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4122) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-94a813dca8eb298a"></a>[`public Inno.Native.Sdl3.SDLGamepadTouchpadEvent Inno.Native.Sdl3.SDLEvent.Gtouchpad`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4120) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-44d8e7a11be1f225"></a>[`public Inno.Native.Sdl3.SDLJoyAxisEvent Inno.Native.Sdl3.SDLEvent.Jaxis`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4104) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8fcd51768854f3c8"></a>[`public Inno.Native.Sdl3.SDLJoyBallEvent Inno.Native.Sdl3.SDLEvent.Jball`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4106) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-02c78141a105bb5c"></a>[`public Inno.Native.Sdl3.SDLJoyBatteryEvent Inno.Native.Sdl3.SDLEvent.Jbattery`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4112) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-29a030af118b2204"></a>[`public Inno.Native.Sdl3.SDLJoyButtonEvent Inno.Native.Sdl3.SDLEvent.Jbutton`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4110) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fd56ac2e160a2dc2"></a>[`public Inno.Native.Sdl3.SDLJoyDeviceEvent Inno.Native.Sdl3.SDLEvent.Jdevice`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4102) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6d650a41e59195f"></a>[`public Inno.Native.Sdl3.SDLJoyHatEvent Inno.Native.Sdl3.SDLEvent.Jhat`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4108) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bda2e5d330ec8125"></a>[`public Inno.Native.Sdl3.SDLKeyboardDeviceEvent Inno.Native.Sdl3.SDLEvent.Kdevice`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4084) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7e3f31c4faef2653"></a>[`public Inno.Native.Sdl3.SDLKeyboardEvent Inno.Native.Sdl3.SDLEvent.Key`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4086) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b1725a709a2691c"></a>[`public Inno.Native.Sdl3.SDLMouseDeviceEvent Inno.Native.Sdl3.SDLEvent.Mdevice`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4094) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-137b2fa77d66f1c1"></a>[`public Inno.Native.Sdl3.SDLMouseMotionEvent Inno.Native.Sdl3.SDLEvent.Motion`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4096) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f16c326a95b92edf"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4154) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0dd06291b2acaf5b"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4156) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0cd7c4fcf0a35e2e"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4174) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d3d0889e93bf737c"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_100`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4354) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf050b7ddd8c9eba"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_101`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4356) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f5934afd04c0d3f"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_102`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4358) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aeddf0e281ddbdd5"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_103`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4360) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-49546d799a12cc2f"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_104`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4362) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-547e4da691f332f6"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_105`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4364) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf221b99aa7e278b"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_106`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4366) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-636e3aaf779887e4"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_107`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4368) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ba61426436d3890"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_108`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4370) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-edde0829c67cf7e2"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_109`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4372) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6af489f280a5a6d8"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_11`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4176) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af5696e9fcbdd899"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_110`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4374) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5d971563baf29cc2"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_111`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4376) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0369dd9cd4f1372b"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_112`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4378) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e9967f41314d2fc4"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_113`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4380) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a181748d64c32a6"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_114`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4382) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b1eb0df121695a5"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_115`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4384) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a69a9a4e395bc01d"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_116`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4386) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f91672fea65412ed"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_117`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4388) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-16efa8784093dbb4"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_118`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4390) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c69cfcb3819423fb"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_119`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4392) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13ea356acf9873e8"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_12`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4178) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-702bd6f8db7ddf3b"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_120`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4394) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-576b921ce8fe46b7"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_121`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4396) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9414bd5a8f29412d"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_122`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4398) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-72acc8009af3566b"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_123`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4400) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4672cc32055d5ec2"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_124`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4402) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-edbdb56f3fcf7460"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_125`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4404) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-95f687ad342def8d"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_126`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4406) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-499b2d643f654add"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_127`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4408) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-961c1bf2f4eb0263"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_13`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4180) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0952d74b34d702d0"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_14`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4182) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-57f588086366f1d2"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_15`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4184) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-211a9c2cfd578347"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_16`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4186) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4bd3d84f56ad23fe"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_17`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4188) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6fe5ecd9c89f253e"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_18`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4190) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0be860be7bfa2264"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_19`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4192) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eb7fe345a80ce639"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4158) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-919cb25956dc7405"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_20`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4194) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb35add27284724c"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_21`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4196) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5445bc824f9580e1"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_22`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4198) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1055e35848e674bd"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_23`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4200) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f3e19c9e0077da1"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_24`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4202) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8fb2ec04d8f62d58"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_25`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4204) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-db924393f89c10d6"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_26`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4206) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-404b56e05e41d352"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_27`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4208) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1ee31ce758eb745f"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_28`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4210) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9293d230cc459aa6"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_29`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4212) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab6ede500f37640d"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4160) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-30b654c735488a88"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_30`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4214) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e89f1c33a53da0a5"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_31`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4216) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-02eeafd6c7216b46"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_32`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4218) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fbfb7503743ac552"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_33`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4220) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13cd626d822107aa"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_34`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4222) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-75f99fe5febbf9a8"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_35`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4224) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89eaf3d8647beb07"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_36`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4226) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fff39db2b71726aa"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_37`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4228) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69336ca98a3284d1"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_38`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4230) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4aa29cc3ae8e8f03"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_39`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4232) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-781f622e8b042ebf"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4162) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-06de23e2b73550a7"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_40`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4234) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7974d77132af173"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_41`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4236) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-55ea1a9b7cb01037"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_42`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4238) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-402e28e5fa279ae6"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_43`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4240) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-84b9fd62de8ff34b"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_44`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4242) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4b373fee291e1f85"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_45`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4244) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8e5e2de48cba73a7"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_46`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4246) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c984b2a3cc2b88f5"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_47`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4248) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d26a20e68445d62"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_48`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4250) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-327572d28b1632a0"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_49`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4252) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e71ea067158e6b07"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4164) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4b6ba7a1021904e"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_50`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4254) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f0a078b0de2df97"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_51`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4256) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60dd49ce0f0839ad"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_52`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4258) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-06e7bd5c7fb298de"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_53`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4260) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e46aac8b187f538"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_54`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4262) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-821db7ff54f65471"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_55`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4264) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df710d15355fcd40"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_56`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4266) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-73958be94927ccab"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_57`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4268) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-80ebd0c571ce693f"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_58`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4270) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-218baf6ee45b0b64"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_59`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4272) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cb1a35f91d57b3ad"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4166) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ac944a87304600da"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_60`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4274) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c8643df300651437"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_61`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4276) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f17dd23cfc3b58b"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_62`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4278) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4d479460ee1408ac"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_63`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4280) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8aa152612b3e7c6c"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_64`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4282) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9371fe7737132825"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_65`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4284) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9e04b02a69f8da54"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_66`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4286) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f4fafbe983946712"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_67`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4288) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-48f13b233087c437"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_68`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4290) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-32b77043ddd20392"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_69`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4292) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64a438c913038390"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4168) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-976a1d5e219207ad"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_70`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4294) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f48ea16e7798f06d"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_71`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4296) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a34f0daf52268b4"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_72`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4298) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-932cc333510c7ef5"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_73`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4300) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-969d9147339d8de3"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_74`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4302) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e2bed8d633a7b728"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_75`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4304) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35e264386eef58e5"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_76`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4306) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7273f059723fdd1e"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_77`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4308) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ee5de1e3fd1ce19"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_78`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4310) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-51e42a8e21c96b87"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_79`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4312) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-00ee6a6d5f6f49e7"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4170) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ffa9c3f1b81f6618"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_80`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4314) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c2c8991271433b4"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_81`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4316) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb9cf5b475016f73"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_82`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4318) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c02e1a8c55ce4ed5"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_83`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4320) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-59ba2673e702b4ff"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_84`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4322) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-75fc601c51f9ab9b"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_85`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4324) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4d0e93044a0bcb6d"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_86`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4326) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7a478eb0e800d368"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_87`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4328) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5d50517287e3d16e"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_88`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4330) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a4c12bf321b5fa31"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_89`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4332) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2b8bd42bec1b412c"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4172) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-18922e30225b344c"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_90`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4334) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f3b477d806c329c1"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_91`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4336) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6349f7b0cd94da2"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_92`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4338) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4c27682331b0cbc2"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_93`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4340) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b6471b83fc537f7c"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_94`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4342) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f86cb6f68949b60"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_95`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4344) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ac04a74f9b2bb91e"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_96`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4346) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8d05d4452f53e71d"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_97`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4348) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e3aa603535853cb"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_98`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4350) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f91534cd6be3ffef"></a>[`public byte Inno.Native.Sdl3.SDLEvent.Padding_99`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4352) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-472567263bf9296e"></a>[`public Inno.Native.Sdl3.SDLPenAxisEvent Inno.Native.Sdl3.SDLEvent.Paxis`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4146) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-31ef6393091b6e68"></a>[`public Inno.Native.Sdl3.SDLPenButtonEvent Inno.Native.Sdl3.SDLEvent.Pbutton`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4144) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c48bd354d5d44e7"></a>[`public Inno.Native.Sdl3.SDLPinchFingerEvent Inno.Native.Sdl3.SDLEvent.Pinch`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4136) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f3aa204fde4ad4c"></a>[`public Inno.Native.Sdl3.SDLPenMotionEvent Inno.Native.Sdl3.SDLEvent.Pmotion`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4142) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-975d63536262798d"></a>[`public Inno.Native.Sdl3.SDLPenProximityEvent Inno.Native.Sdl3.SDLEvent.Pproximity`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4138) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-36dc28f033f74d03"></a>[`public Inno.Native.Sdl3.SDLPenTouchEvent Inno.Native.Sdl3.SDLEvent.Ptouch`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4140) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1417556ebc187076"></a>[`public Inno.Native.Sdl3.SDLQuitEvent Inno.Native.Sdl3.SDLEvent.Quit`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4130) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5fb2d194309440cf"></a>[`public Inno.Native.Sdl3.SDLRenderEvent Inno.Native.Sdl3.SDLEvent.Render`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4148) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e5e3591ee5e90eea"></a>[`public Inno.Native.Sdl3.SDLSensorEvent Inno.Native.Sdl3.SDLEvent.Sensor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4128) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a26940c9ddc57d7"></a>[`public Inno.Native.Sdl3.SDLTextInputEvent Inno.Native.Sdl3.SDLEvent.Text`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4092) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aff4104ca9b993a7"></a>[`public Inno.Native.Sdl3.SDLTouchFingerEvent Inno.Native.Sdl3.SDLEvent.Tfinger`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4134) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0b5f3633f58c4562"></a>[`public uint Inno.Native.Sdl3.SDLEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4076) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-67b6a948be9bd953"></a>[`public Inno.Native.Sdl3.SDLUserEvent Inno.Native.Sdl3.SDLEvent.User`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4132) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6a80bfe5a16862b"></a>[`public Inno.Native.Sdl3.SDLMouseWheelEvent Inno.Native.Sdl3.SDLEvent.Wheel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4100) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d5e0eb8e3f534ec5"></a>[`public Inno.Native.Sdl3.SDLWindowEvent Inno.Native.Sdl3.SDLEvent.Window`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4082) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d4d7bd2bfb14f3c"></a>[`public System.Span<byte> Inno.Native.Sdl3.SDLEvent.Padding { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4409) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6f4a26e735d41cf9"></a>[`public struct Inno.Native.Sdl3.SDLEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4073) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLEventAction</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLEventAction 5f96038ec130656983b617694abbc29ee9e6d9692cb22c32aee8e4e641ad7820 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventAction.Addevent 039c126f43bc8b9e5ac57a14fa36597740291d8a09064d84b2a1661e7111319c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventAction.Peekevent e122cdbc4b4dcf5301cf0632bb85eddb0c05dcd5989fe920d2d51ba0fbbd6b71 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventAction.Getevent e1595f8c3dedd8e7c45ecf8885bbfe14a0b274504b8a4cee53981afb3b3dba98 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-611e159a64504bca"></a>[`Inno.Native.Sdl3.SDLEventAction.Addevent = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1297) | 当前常量或枚举值：`0`。 | — |
| <a id="api-33075e41ad63c095"></a>[`Inno.Native.Sdl3.SDLEventAction.Getevent = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1299) | 当前常量或枚举值：`2`。 | — |
| <a id="api-11d23cd178c6e85d"></a>[`Inno.Native.Sdl3.SDLEventAction.Peekevent = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1298) | 当前常量或枚举值：`1`。 | — |
| <a id="api-385625b9d16bb537"></a>[`public enum Inno.Native.Sdl3.SDLEventAction : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1295) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLEventFilter</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLEventFilter 59e230b22a21173c3789cc866b688e68de9e9d1adf8e2dfe31db4bcd919179fe -->
<!-- api: M:Inno.Native.Sdl3.SDLEventFilter.#ctor(System.Object,System.IntPtr) f59fb8284cc50ea79da9372d628b4fd668492d1c198bd78dc2d0ad69408124db -->
<!-- api: M:Inno.Native.Sdl3.SDLEventFilter.Invoke(System.Void*,Inno.Native.Sdl3.SDLEvent*) a71c7679347a24cfc554254297452bc74f661ce24504a4ab0cff74f4824ae103 -->
<!-- api: M:Inno.Native.Sdl3.SDLEventFilter.BeginInvoke(System.Void*,Inno.Native.Sdl3.SDLEvent*,System.AsyncCallback,System.Object) fade4c721fa113b4651d581bec3da572d22f5c2c98e23cf4e0d87a4784ca1346 -->
<!-- api: M:Inno.Native.Sdl3.SDLEventFilter.EndInvoke(System.IAsyncResult) 3589533bdddde78c77ead33f1dcc3e93649303f3ec9e0ee2c17d0a1ac4906f1d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ea38ef4e2608300b"></a>[`public Inno.Native.Sdl3.SDLEventFilter.SDLEventFilter(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6875) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-cfeb0075d8990f07"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLEventFilter.BeginInvoke(void* userdata, Inno.Native.Sdl3.SDLEvent* evnt, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6875) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9a76608b626c3e8c"></a>[`public virtual byte Inno.Native.Sdl3.SDLEventFilter.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6875) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-69d4c136dfe36a73"></a>[`public virtual byte Inno.Native.Sdl3.SDLEventFilter.Invoke(void* userdata, Inno.Native.Sdl3.SDLEvent* evnt)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6875) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-39267be22a4730be"></a>[`public delegate byte Inno.Native.Sdl3.SDLEventFilter(void* userdata, Inno.Native.Sdl3.SDLEvent* evnt)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6875) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLEventPtr</code> · 53 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLEventPtr 0df5e91ea884c73df9725930c0cc900dc5f42d5bacf14bb42e6db215b98c205c -->
<!-- api: M:Inno.Native.Sdl3.SDLEventPtr.#ctor(Inno.Native.Sdl3.SDLEvent*) 7622672567a2c77768f4909166e686ceffeb5f2a06e893db696bed8d62e223c0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventPtr.Handle a91eb26d1a0f66e8e93060a15db0eccf6a0dd4d0c466c5ccb6aeb76591b7c984 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.IsNull 39c1b5deb2be458430b9673a77206546e273d189b04965e45bb463a507064cbd -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Null 8f5636cda6691ea287c639a3308d3eecaaa7e8d4edbf6b87810a25bf0a9f976e -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Item(System.Int32) 5569dc14993d748fc001ab032f67cd168d8240689d9db449bcd2e0532f451c8e -->
<!-- api: M:Inno.Native.Sdl3.SDLEventPtr.op_Implicit(Inno.Native.Sdl3.SDLEvent*)~Inno.Native.Sdl3.SDLEventPtr f98d7ba54b5eb68e77a8500c8d772aab5c070451736964eea7fef6020350a39d -->
<!-- api: M:Inno.Native.Sdl3.SDLEventPtr.op_Implicit(Inno.Native.Sdl3.SDLEventPtr)~Inno.Native.Sdl3.SDLEvent* 157207d33edea45b0eb3cda5f481bdd6bb69bdb3e744cb32dba4a7813a55d6b5 -->
<!-- api: M:Inno.Native.Sdl3.SDLEventPtr.op_Equality(Inno.Native.Sdl3.SDLEventPtr,Inno.Native.Sdl3.SDLEventPtr) b38b5d695cedef5626b0abf9135d5ed6ddd1cd45cc9f5e380632a08bfa8e329e -->
<!-- api: M:Inno.Native.Sdl3.SDLEventPtr.op_Inequality(Inno.Native.Sdl3.SDLEventPtr,Inno.Native.Sdl3.SDLEventPtr) 25f8803f4513c2482fb4841d519dc05567d1cd9ba27e93f821e7b5cf64699c97 -->
<!-- api: M:Inno.Native.Sdl3.SDLEventPtr.Equals(Inno.Native.Sdl3.SDLEventPtr) 6f5878bcbc88c555807b2ea6fd2c4b54524e05b5ae8f91ceb0f88714ecea491e -->
<!-- api: M:Inno.Native.Sdl3.SDLEventPtr.Equals(System.Object) 4e78bc17fb5c6006d3cf1f20f236f248bd7f78fd8029e274eacbae24c400d90b -->
<!-- api: M:Inno.Native.Sdl3.SDLEventPtr.GetHashCode 5dd9cf9986f12a75ea1fbe48a7d2dc7460043058f942b886d4e97ea0cd8a8d42 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Type 13d0e09dba5023a3b2d45b4de11a61f1c81e429d6fa1ee43cac08bd7e6623f1b -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Common 36f03a0329cd32d51a9d0fa3d253cf8bb2a4058c8288e9b55c062dc55242ddc6 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Display df0538301f5c02452de9334cee3cb50d2867f2123b6190379bbbc9d929b06956 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Window 50a1ef69f7329113d15f353b6df409e4dba5a952cd5869078587a0f1f11d0836 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Kdevice 0ede321b4d3cf2c856554f0cd0553e81ff2429126db285e6fae4cfed47eb3de1 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Key 1a6161a5c88fd95e92db0c2b6777e607d0482a8306e4b0ecc1ed56cd9fa55ad7 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Edit 3be257ff32a8032fff2cd62cdc14e3c4c014aefc19f7ef9f28ef4c9348ca0faf -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.EditCandidates 07c868c38c83b1aaa90a615f459aba4e2f10aa7b31e8ea18619ff110e9d02120 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Text c8940cc4fa42235204bdebf86a9bfc92d4bdb266ce23456eca0929f15833d710 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Mdevice 21926b601b9f3147a494e4a25524cb83acbd3653b69c9da2f0bbfb8dec903198 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Motion 4b926e3cba3b86bced086a62a7a7281e264562d6c8a9037ae1fcccaca2f00dcc -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Button fbd0685403daf4751959ef4130d8ec1b951831a9a1243d37e221760097240e1f -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Wheel dc45423840b88a836b0ae416b9a9ab3bc5fff602ce0812687533325771f4a790 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Jdevice e01df6bacf8bac3be943852a73151807036f7e2229957de75fcf8cae7e196e95 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Jaxis 13089b2079fdfd18ad0bbde6520d4d8b27628639623b6dd86af55f9619b9cda2 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Jball 92646cfbf2e64789e4c50bd4f49fcea55e0d05e3d9ee9a36e9430d4ec371f1e3 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Jhat 933baf5d3a4393203452782b6c27a3bc262e1ef5fa2b21acf4cea9123c481b03 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Jbutton 966c414aabd1908d745b7dd6fa3d32add327ae3c96d9282467f43e5ccb39b9f4 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Jbattery 9132173f3ba022459fad753252e74b02d4b1d3faa60c29f9f13cf67d42b19e7d -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Gdevice 5b9b6bb0c28d693f1e77301bca1551d7974c42598d7ae911f7c42bc6e2a06d61 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Gaxis bb11a0be06a9923c22b63351bedd546b284291ab390f6316bb8e447d23ffd113 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Gbutton 4a74ab12bfbb13835b1df682c79462bf4b36ebf4f492ed9588b62765ed8ca875 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Gtouchpad c4ec650561540eb2df06ab5495efcca5ec14c5e5d51ffa1aac28ff2201b1939a -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Gsensor 3bdd0fb1d7119ade4d4434423635adc7a96c2b851264fc436489bdc9ea44f48b -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Adevice 58ca050ce0f3a00c3764f16f0e9a22fb6c848b124db14ca153a5032e86b5d14b -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Cdevice 8a0490f3e56f7a31f9473807d42cd3c2219f225dfec1e70017ca381e65e0ca06 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Sensor 6b20d51c1a17d6bef05c9d4d3ea2b3bdf3357edfb4986a9c4cc3401c9ef2495d -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Quit 964f26e1bd5b99e0be71ba935f1ed7ea63cc7c7313ae775ad76379f1d1f86f86 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.User 7a9ae4d8d358d0fc40e434a470cc5c9dceab9b585eabceac4e23571ccb578063 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Tfinger 846c89425772df8d53e2d9bced9f08c886b5063647f67a636bd820be270e72c6 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Pinch 4592bf3410e80b852a975c08929ef4de4cf175c6d390639590f208036d6c6010 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Pproximity 0d992c7fc9f0e3da76b72a802426adde7803d1e277def517f9c1133854ff8650 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Ptouch b233303200388c9d169a504bf26666c18bdfc83b660ccceae0c6438b2ca97114 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Pmotion 827054810a2c71ee1db691f3c65bfcf9208c31d11f65a65a5cbcc85fbcad747b -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Pbutton 6f45835b9ffce80847f97edc3772cdaf3ce1a3ced0a002429882c27690621fda -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Paxis ddc0b34fed5b5ecbfaa9a36fcfb55f44141a486257ee29cba29ba43059ba80af -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Render 4ea639f0b364c1ef777e31c4ce9dfe6b185c857003d156c38a71a1cd1e1207d2 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Drop de18969de325bf58540c354eb2babd1adb8282dbef153f6c2d6f304854f24219 -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Clipboard a8d614c153637bfa95309b80c28b41cd8fcbb3a1edcd86f4f7592f8e679bea2e -->
<!-- api: P:Inno.Native.Sdl3.SDLEventPtr.Padding 255b743ae223bc752cc3a1e65fc8aaedfc62c5ecfe3cfdc173c1e6e6ee98ff76 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0fd83368163ccf62"></a>[`public Inno.Native.Sdl3.SDLEvent* Inno.Native.Sdl3.SDLEventPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8262) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e2d0ad15209b462"></a>[`public Inno.Native.Sdl3.SDLEventPtr.SDLEventPtr(Inno.Native.Sdl3.SDLEvent* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8257) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0fde065fee42cca6"></a>[`public bool Inno.Native.Sdl3.SDLEventPtr.Equals(Inno.Native.Sdl3.SDLEventPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8272) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-96625f0e22a9ebab"></a>[`public override bool Inno.Native.Sdl3.SDLEventPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8273) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-1a32f3cb187eeda5"></a>[`public override int Inno.Native.Sdl3.SDLEventPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8274) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-3ba8ecd20dba6d49"></a>[`public static bool Inno.Native.Sdl3.SDLEventPtr.operator ==(Inno.Native.Sdl3.SDLEventPtr left, Inno.Native.Sdl3.SDLEventPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8270) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c1e75ca3649cf1f0"></a>[`public static Inno.Native.Sdl3.SDLEventPtr.implicit operator Inno.Native.Sdl3.SDLEventPtr(Inno.Native.Sdl3.SDLEvent* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8268) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d2c3b6cbffa52cde"></a>[`public static Inno.Native.Sdl3.SDLEventPtr.implicit operator Inno.Native.Sdl3.SDLEvent*(Inno.Native.Sdl3.SDLEventPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8269) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6f6b2b84d916b214"></a>[`public static bool Inno.Native.Sdl3.SDLEventPtr.operator !=(Inno.Native.Sdl3.SDLEventPtr left, Inno.Native.Sdl3.SDLEventPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8271) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8af81324d8faac1b"></a>[`public ref Inno.Native.Sdl3.SDLAudioDeviceEvent Inno.Native.Sdl3.SDLEventPtr.Adevice { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8300) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a4986a88d2dcd92a"></a>[`public ref Inno.Native.Sdl3.SDLMouseButtonEvent Inno.Native.Sdl3.SDLEventPtr.Button { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8287) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-56598db2b4ee212e"></a>[`public ref Inno.Native.Sdl3.SDLCameraDeviceEvent Inno.Native.Sdl3.SDLEventPtr.Cdevice { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8301) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9e0605a1383a6b6d"></a>[`public ref Inno.Native.Sdl3.SDLClipboardEvent Inno.Native.Sdl3.SDLEventPtr.Clipboard { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8314) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-54f166a219d5fdfa"></a>[`public ref Inno.Native.Sdl3.SDLCommonEvent Inno.Native.Sdl3.SDLEventPtr.Common { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8277) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-80f60c5146a3b41d"></a>[`public ref Inno.Native.Sdl3.SDLDisplayEvent Inno.Native.Sdl3.SDLEventPtr.Display { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8278) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0fc3fd33510b4812"></a>[`public ref Inno.Native.Sdl3.SDLDropEvent Inno.Native.Sdl3.SDLEventPtr.Drop { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8313) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-030fe89c8cc373e8"></a>[`public ref Inno.Native.Sdl3.SDLTextEditingEvent Inno.Native.Sdl3.SDLEventPtr.Edit { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8282) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cb86eb2658cc2646"></a>[`public ref Inno.Native.Sdl3.SDLTextEditingCandidatesEvent Inno.Native.Sdl3.SDLEventPtr.EditCandidates { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8283) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d7477012d0743fe7"></a>[`public ref Inno.Native.Sdl3.SDLGamepadAxisEvent Inno.Native.Sdl3.SDLEventPtr.Gaxis { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8296) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-40c694997b6fbdf9"></a>[`public ref Inno.Native.Sdl3.SDLGamepadButtonEvent Inno.Native.Sdl3.SDLEventPtr.Gbutton { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8297) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6c4fc357254754c7"></a>[`public ref Inno.Native.Sdl3.SDLGamepadDeviceEvent Inno.Native.Sdl3.SDLEventPtr.Gdevice { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8295) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-27d6526c01762128"></a>[`public ref Inno.Native.Sdl3.SDLGamepadSensorEvent Inno.Native.Sdl3.SDLEventPtr.Gsensor { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8299) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-929e2d8fbe3eef5c"></a>[`public ref Inno.Native.Sdl3.SDLGamepadTouchpadEvent Inno.Native.Sdl3.SDLEventPtr.Gtouchpad { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8298) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-30d1a8e241ed6915"></a>[`public bool Inno.Native.Sdl3.SDLEventPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8263) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0a483a212991208f"></a>[`public Inno.Native.Sdl3.SDLEvent Inno.Native.Sdl3.SDLEventPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8266) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-36ce82d719386ed0"></a>[`public ref Inno.Native.Sdl3.SDLJoyAxisEvent Inno.Native.Sdl3.SDLEventPtr.Jaxis { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8290) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a600514bb3d3824b"></a>[`public ref Inno.Native.Sdl3.SDLJoyBallEvent Inno.Native.Sdl3.SDLEventPtr.Jball { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8291) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3d427596ebc0e183"></a>[`public ref Inno.Native.Sdl3.SDLJoyBatteryEvent Inno.Native.Sdl3.SDLEventPtr.Jbattery { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8294) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3c152a5b10e53c5a"></a>[`public ref Inno.Native.Sdl3.SDLJoyButtonEvent Inno.Native.Sdl3.SDLEventPtr.Jbutton { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8293) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5632dac5acd7792f"></a>[`public ref Inno.Native.Sdl3.SDLJoyDeviceEvent Inno.Native.Sdl3.SDLEventPtr.Jdevice { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8289) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-64dfb0237342101c"></a>[`public ref Inno.Native.Sdl3.SDLJoyHatEvent Inno.Native.Sdl3.SDLEventPtr.Jhat { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8292) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3cc4f11a7c173c60"></a>[`public ref Inno.Native.Sdl3.SDLKeyboardDeviceEvent Inno.Native.Sdl3.SDLEventPtr.Kdevice { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8280) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1cdde553ab74f42e"></a>[`public ref Inno.Native.Sdl3.SDLKeyboardEvent Inno.Native.Sdl3.SDLEventPtr.Key { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8281) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dffd89a5e5c7ebc6"></a>[`public ref Inno.Native.Sdl3.SDLMouseDeviceEvent Inno.Native.Sdl3.SDLEventPtr.Mdevice { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8285) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b7cb5c72f950ea9e"></a>[`public ref Inno.Native.Sdl3.SDLMouseMotionEvent Inno.Native.Sdl3.SDLEventPtr.Motion { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8286) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-43f86c216460602a"></a>[`public static Inno.Native.Sdl3.SDLEventPtr Inno.Native.Sdl3.SDLEventPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8264) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-374e067634bdf5db"></a>[`public System.Span<byte> Inno.Native.Sdl3.SDLEventPtr.Padding { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8315) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-deaa2ed80e82f83a"></a>[`public ref Inno.Native.Sdl3.SDLPenAxisEvent Inno.Native.Sdl3.SDLEventPtr.Paxis { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8311) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aeb3406d16fbc7ad"></a>[`public ref Inno.Native.Sdl3.SDLPenButtonEvent Inno.Native.Sdl3.SDLEventPtr.Pbutton { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8310) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-371e5c35441840a1"></a>[`public ref Inno.Native.Sdl3.SDLPinchFingerEvent Inno.Native.Sdl3.SDLEventPtr.Pinch { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8306) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9419d8c61789ce0e"></a>[`public ref Inno.Native.Sdl3.SDLPenMotionEvent Inno.Native.Sdl3.SDLEventPtr.Pmotion { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8309) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-657bdc399c980573"></a>[`public ref Inno.Native.Sdl3.SDLPenProximityEvent Inno.Native.Sdl3.SDLEventPtr.Pproximity { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8307) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-89f9e75a68217a7f"></a>[`public ref Inno.Native.Sdl3.SDLPenTouchEvent Inno.Native.Sdl3.SDLEventPtr.Ptouch { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8308) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-96bb283851582ed6"></a>[`public ref Inno.Native.Sdl3.SDLQuitEvent Inno.Native.Sdl3.SDLEventPtr.Quit { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8303) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-35b48fe4a26fd329"></a>[`public ref Inno.Native.Sdl3.SDLRenderEvent Inno.Native.Sdl3.SDLEventPtr.Render { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8312) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-592da65e53ec01a0"></a>[`public ref Inno.Native.Sdl3.SDLSensorEvent Inno.Native.Sdl3.SDLEventPtr.Sensor { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8302) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d3fa0605ea7b85dd"></a>[`public ref Inno.Native.Sdl3.SDLTextInputEvent Inno.Native.Sdl3.SDLEventPtr.Text { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8284) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4d9c3bf449d38f5f"></a>[`public ref Inno.Native.Sdl3.SDLTouchFingerEvent Inno.Native.Sdl3.SDLEventPtr.Tfinger { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8305) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-867dae7b4a6658f8"></a>[`public ref uint Inno.Native.Sdl3.SDLEventPtr.Type { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8276) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0955695d349e1c7b"></a>[`public ref Inno.Native.Sdl3.SDLUserEvent Inno.Native.Sdl3.SDLEventPtr.User { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8304) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4b0c232c2f1d9fe3"></a>[`public ref Inno.Native.Sdl3.SDLMouseWheelEvent Inno.Native.Sdl3.SDLEventPtr.Wheel { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8288) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8919198c51a21785"></a>[`public ref Inno.Native.Sdl3.SDLWindowEvent Inno.Native.Sdl3.SDLEventPtr.Window { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8279) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8d70aa2debff4ab2"></a>[`public struct Inno.Native.Sdl3.SDLEventPtr : System.IEquatable<Inno.Native.Sdl3.SDLEventPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8255) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLEventType</code> · 125 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLEventType 41541825d696775328c4ac34e7adf8677315dddca205dd653aa1d20c7a96fe7b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.First ab2c33478a6f48d5d4967de53a8b7d0d9197a071ec253977cf196fc8f6290710 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Quit bff14e11b5d4d2f0884c8733d45c07f7b9df57edd388a72058fdad71fd6f3f0c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Terminating a4ea908c3d30c0c2d93fc5afcabe081064433d0c2c41b40ad1b0d3ec21475969 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.LowMemory 90b66d2c8891cf66239e05e7a7fa04d6bd449432e4220b65e112da4c5fbd70ec -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WillEnterBackground e549da04fe4a061070975fc28fa57d3980681ca6c76b4625de29b1b619b9c74b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DidEnterBackground af178551ff7bec5943033dad650aa0fe4e4ed0f1306d85114d0d78737e66d9b6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WillEnterForeground 5002f97ccda0a453695033926d7dfa419e76c95ba302528c7c133c8c40a4c3cb -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DidEnterForeground 189d7f2905336dd00bc4ebc62037336ca23391a9205e0f3d15af57086597bfac -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.LocaleChanged b7ad54d000c5f0de928108a87695aef5431d8eaac9de5a75e8b9de84bf09e2b9 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.SystemThemeChanged 3b60e310ebf51d9d38f7474fc6bc3cccc8980c6643f657ea7581eef814aef4fe -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayOrientation 11c58d414ac8527f165958228fc904e6b2265812cc5631e7a9ab90a8033a5743 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayAdded efd45a971042e5f70a03995afab358be201a00f7e04a8e97bd607ae6b67ac2e0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayRemoved 83b42e460b830eb382e3ff05e082057387c189ecf4de387b9149739c06181c6e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayMoved d8542fa4976378b1115ec7078305f3a439cc0315c41ad97210295e94d960c352 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayDesktopModeChanged 5cef4688fd782f82d77af2634dc9ea8c3fb0677a5151dbddc75b3b3fc7fb1a32 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayCurrentModeChanged 85948ea1a396d2b73ae6ca466c7e31cd7b0b3821ce4c07f22bd5bbe4d126be10 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayContentScaleChanged f51aa4a0fe65e078e35eeb5beb67bd8ce73ccbc9fea17311bb34767e49f2af7c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayUsableBoundsChanged 32033683ac00822987b8b1d84fe3f8d2cf6c6ee56c65e476c84f3077867e24d9 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayFirst f06aac7ef0701f6d178289ec89b88bfe1ea98b35b8b60d9ad6ca4f67890297c8 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DisplayLast 76f55ff9b24b1eb8d80fb4928a1638da03fbba2c66df93d7efdf8e313565bb0b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowShown 3f5d8f7508c3427677db05ad5e414f4429b128247aa4258e9c8d6240f633bcc4 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowHidden 2d3d8975c0332883fae3cfed28ce9eba7bbe64a301c3aa133287e1cf93139518 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowExposed 6f9419b488df029bc5a0ea35689b43ac86a3ac6eba672649240cebe81528c8b3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowMoved c8dbbc49beb5274ee3e1b80f75aafed561052195d57db3faa5519accc06fdc06 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowResized da4ceeb7d67aeb0ce04a6127f0366f102b7f485e6ac6f56d3ba022b29621c72e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowPixelSizeChanged d997e4dc46cea0d5c1f2d886f46dbb120f87d724c1eadff77416ae73db135c48 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowMetalViewResized 1a0e0abdd7bca9c03566ad0d9a6288475bc52f0b86949688d63ff5e829e2f2c9 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowMinimized 215a5e7fecacaf4faeb1791423d4f82e5dc7c67e471a36694898270643bc5d6c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowMaximized 655aab19611ae48b5881ffd10649dce324f1d740b2e1025385e46f30ff17d8e5 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowRestored 98a3d80e18027d6f98f16937a9c11f9552afa186f80a3fe0545ed17dcf64fa99 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowMouseEnter 6a750b76a953c3f3cb2a503938f3bdd7035c50c9c9c71e307e7ad90fd74d15ae -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowMouseLeave 0f74f13f8a3698b431f3d4ab5a5345e318bb5bd99cf4ad1e85f1f01127bb5a19 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowFocusGained 463ab29f1bec4bda5da0503bc8601543d07c50d7b23c068fc831152ec88d7ea1 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowFocusLost 2fb4f5971b97a62058fe7ae0e58cf204a039b7ebdc6f09b3ba8991fab5fd628c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowCloseRequested d0bd762f86017e9e5e9d3a762267f0843f3e2f387b36287fb0ab387696430dd0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowHitTest 8bd7bdc16ebf14fb4d87695bd75f70a69470c6b950ddcce67474b97599891244 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowIccprofChanged cac0ff912f71d66c3d50e0b6ef81370245f7e632c12e8a8bc90377e529ee170e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowDisplayChanged 317bb787f4e68fbaf55694b566fe511c886feafa35453f61c252772f565ccde8 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowDisplayScaleChanged 1f7f5ec23625d4ab9c17a0042a2190a940dbebce271f5fd9d51ed84ec289d33d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowSafeAreaChanged a4d7c3b874ea193c9977b50a62edc8f541ab1cdab843ae231e658f33f713f233 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowOccluded 0ffd5000c9723ad0fc92c29e619643c85e0de41df229f24951a3b24f59dc180c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowEnterFullscreen 283647bff0605c894e1683ba88a527e788e33083748248caa5ff5d03eca46057 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowLeaveFullscreen 0ae7db32d40c2cdee07de5ebc048c9da50ca5d4bb2fd1af5da9c50a8869e060b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowDestroyed 23c3a90d0f9a918273fc3067314a7469a79ed13f2eafc3364d505a60b3d61ca6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowHdrStateChanged 61c2f7be1a9388f5a0688befa8e5397d7766de5a9bc4f63595606dcf2dfb313b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowFirst 065b6bfeb9be75ae97a85928092d5ab0b353a3b9159956282152c51eac2f9097 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.WindowLast 7a152767088144955136b47f67e2b434159b2330c6155f27248fd25b8ad278ac -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.KeyDown 391372a7f3f4bbb98210f6b3c6c15315e5b06011c4564f8360a93e414da38484 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.KeyUp ce66e74ac2912ba080448ed57bb8c12f3d8ebfe6361ffc35aca060aa61a880b3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.TextEditing 9e31e2c4e3fe7751e7c1581e41f7b5aeb238641d1564a50c4787f05f25736481 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.TextInput 3230ff0ef48666424b605cc5038a697b45fb82bf4fa18df5c609eb0d434f3392 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.KeymapChanged a7cbd633aef960b32e72eb616d5b39dbff045a591f1ac11533feff9179162ab2 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.KeyboardAdded 5003d44f6035089f02b000c2d3eecc949cfcfee26bdcd69ca70dec2d10a47c1b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.KeyboardRemoved 7e194031891f91a3a36701ff469da7b6301371bd078ca5bd2cc19c365f6a9a22 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.TextEditingCandidates 65d98be5d1c3a3bc352eac42f23280affa83fc4bd9d50d5fb71009f5682c7d5a -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.ScreenKeyboardShown 2e8bf5ff7f9fb187eb4c88bca22583199c3d61aad281c836ae1c3b585111cd70 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.ScreenKeyboardHidden 43e9e376eb9a28cd5de538128a77293f0506bca1e75777237fe9de4c13aa2ec3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.MouseMotion d77a8184a08cfa524f3d064b6f2622bcf25b29df6491213516c04a03a29d8e0f -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.MouseButtonDown 35d68d70549ad10e47e83a5f85a48347a5c3dd641e859f37157c5572efc6766f -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.MouseButtonUp f81bd6a740327821a6071e4a781c6acc581e001bdde84bcbb304a844db73c4fb -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.MouseWheel 70bbc12298c9a66b3c0ddf1b45d554483cc5d41c0e1ece8a07915bb7f4c72b1c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.MouseAdded e0d66e5ca86c4c785581d2bd241daa27f1a1553083f7ad55b9d9ddedbd690bb0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.MouseRemoved d41579bf380c8e544a18c7125f0dcc6c9cbfa117c63b2c6a2bb67b164aef7c1c -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickAxisMotion 4499bf849860d7578794f04cfb265672932c1e8b895dec6325479ddd8c90d794 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickBallMotion 9dee5548c1978e2cf9b5826c0bef86c4b46c91cf60b17671b47164e3ee23d61b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickHatMotion be748e5fec54455d283d6a61ad3e23148bd9a105bf3f6d5794793b95a1799547 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickButtonDown 2c5bf424e859978c648a9a4c122ee1c8f16b9d6bb9f7bb2175148fb43f98eb8a -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickButtonUp 8ba60240e9f90d055c50fd7bc3f8294c5b021f0875d8482f56bc086218a53272 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickAdded 7e901fa8e5667eeebdba24fc64699b4ff0720636629abdf061b92b6fe3d70ea3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickRemoved 1556da65effd20064f3a143f1085d395187a832edc3ce62dd127514679e4bfc3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickBatteryUpdated 2effc896a4eb3a7b69fe50db3bc378aa0796d6494743f9ebc8a2d01a7e61c8ff -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.JoystickUpdateComplete 6f36c0bb7e3e954ee6b2604b7b4cfcc1db0a50ed2e1b8a84e4afadb57f7e7fc5 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadAxisMotion 1d29445aee1257632e58f86bf3f01abf39bd044c589ff89e60c30f9900316d82 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadButtonDown c82632fb33bed1f889730df94e7dc01cf50823881faa3601d9697842fba8b162 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadButtonUp 08ba2c6eb95d74ac75af91c11b83b131127c51eba79fa9759220b698facc21a0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadAdded 76e44bf7a469edef60ada480f6bd84acaa8b11e77b3e38a3307ddea836ed7784 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadRemoved cc8d9aa2b35e63dd3a95da613bcc7dd2c4d5c8270d3defd6678ac13e282db713 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadRemapped bae6b69ea495de5bfcf2e526dba87d762e04c9255743ad2c486744ed09dfb798 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadTouchpadDown e1ac0dad5659b2c911454cd7ef67a3f60684fb5f4b3020c57657611cad8a9e76 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadTouchpadMotion 2d918c5bcb6328626d44719bc7c09ec7753ff44b993330341b314aac6534d425 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadTouchpadUp ec82dfd38e93ab16b8d6c270d0bd87737ccf08ca1cd391c8ab71b50a83de6bb2 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadSensorUpdate 9db023a613bf544961388c9c18cb9bcfd79acdbaacf77db8c8bd8343094a3922 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadUpdateComplete cb8fcf73aa7d1f4b73b60e7b5841bbd46780f49403c8cfc3f305ed93769bb82d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.GamepadSteamHandleUpdated 688d127fcbb2cb08c2de3715eab20f47f72b36945f7d199e84f21f138477bf46 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.FingerDown 35456a0eeb0bbabead12904dcae6480367c3601acbf3c9a7796f4f3295d966a0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.FingerUp 269fa0b275586a041d4a5ad02bef56dd708b85f4bf18ee28292b1c254d85257d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.FingerMotion 4bdaa0280312f70968d9d8756f89824c2c32960418cc7635359004e08b891926 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.FingerCanceled d2e09c89ad5e239213468c922e223e575b5386e958146a1e48422221e919543a -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PinchBegin 35632f495f9390812649b6d49c916f919929f4fdc6366495dfebbda5dc4b7bef -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PinchUpdate 27b70a84c5a0ddadddb8c519c8ec216ce52e4c9dc5c96becc01e206d97e125ab -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PinchEnd 203a58b6cb75341317793f70aafaf79f4038cac51613dba2ad85d6a301c58c3e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.ClipboardUpdate 671e3f41d1f5731435753f7a2bddace0843248405d5351a02cc773e1ad3bf7ca -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DropFile 1e3da1ce7093c576fac2d5ae50dfec097fc427cd15a35a206abf601f9496dc5f -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DropText 26f8d706f95bad7ab98cc5214ddc30ce7002db5825e3bc35adf1b40c81646532 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DropBegin 1629bf4365573fb53e7528142818dc3fc8100cbe3b63399a5eb0a5e5ec5e2b8b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DropComplete b3f6c05db9114d2753c7f57c1fd40309220ac192b1a3f97d9d13411ff96873c0 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.DropPosition fcdb783cbdfab07003749b985aefd0acfbf0f4fe92b1b41c1aa444c4d3924740 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.AudioDeviceAdded a6218a2ad3b4a4f0276c23efc9efea3a286892a2a56d32323921abf62260ded6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.AudioDeviceRemoved 8ae2f67e2de82f04bcce2b304ad7ecda622df1066008ab2e7d08a4d470b9941e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.AudioDeviceFormatChanged c53891f38069f76f9541c7da59f1bbd7f970d5a06297cfdf81765e8376135ab1 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.SensorUpdate 2eb3d6c62f127148d5ea6d3b15c0440f678c3f1a5872dc0fd9db07055b2d4b8b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenProximityIn 0501f3bf77a6c5b8a67f9278ea43a4c00b316bf1698a76efd2a60d8291e61064 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenProximityOut bbcd0f7f5cccda31c2176c40ffebc285499c60f6f6160817636c4405b8ef7521 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenDown d54166fc14c578c5081958716dd5298366d3790bea96599db1613559b93d938d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenUp 8af9e6dc80b97276a5f39fbb1cac115d018347113841bd22ea669723a4abc5ed -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenButtonDown d2a9522d8bd0f9d1be0f11248155ac836cbf3dd1841f825aedb048cf91d0626d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenButtonUp 08d6c5c51399493396b472e3b36be16d4635116537ecc237b928a1c2ee908ea5 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenMotion 17a952b57a0da73b490f9875d1a90f7a0ca97800dc574d515f88eb0e6c842e6e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PenAxis 46df1318235193ebf655b003d1bb590240c17ca8c76fc6c15dc580e7bd4128e6 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.CameraDeviceAdded fb6a09cf2d9c34e8ea346ba7e47568f8ccd7a2904f6ef7647a39c61517206872 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.CameraDeviceRemoved c990ed04ca1b5b947cc90db191f60f8890431ed0d42d134fb6557353dff1a817 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.CameraDeviceApproved 7f0953daed687bfc301ba8afa46ec15c1af4e146a661aa24fdbda0efa450813d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.CameraDeviceDenied 05cfe613c1e3ae076790ae05f7f5488540db83eeddea9c8aad2eb0ea0783a26a -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.RenderTargetsReset 42996cca6d315ffa2f5e360cedf97272f43b62bf1565182e8e1212641cca8e5d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.RenderDeviceReset d42553dd578c0840759d52e7e189891a03b059a4f86ea1a0110e4f7e264e1b87 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.RenderDeviceLost eef88390962bdae7160639f5e65c4eaf86d4cdf0b04213372d8a510abf968880 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Private0 a622e8e7dbe2e58afb896f555c81424e5b0f5282b086ec60b783f855ad9a3ee3 -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Private1 4eda21694f1e490b2bc55b952340c3ad6066fdaa34b0fad5e6e987123731e42d -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Private2 5e1b2c93b47ce2741b9a5d0a13feea4bea7c0a8a6870eb00513c766366cfee4e -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Private3 0e336036e14dadc31dbea97a66b806012da65cd501a17ce33d557283395b0aba -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.PollSentinel acb76cb79929278869701836d5c298a7e5eb611c85124ab3d3edbed27b580f8f -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.User b784fe1ed09f46c17b39ef9bd8af9a4cc4faf39ac4146fd385c43f1c91624b7f -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.Last 128504c5eb0afd51c702b8169b10c9b655cde6984b8a8bd13670fe6209708f2b -->
<!-- api: F:Inno.Native.Sdl3.SDLEventType.EnumPadding 7536db2c1e9e930453e8a49bafcfa3f194ee0ce0cc75647f1eb4406aeb613dbe -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ecf8282237adc6ff"></a>[`Inno.Native.Sdl3.SDLEventType.AudioDeviceAdded = 4352`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1263) | 当前常量或枚举值：`4352`。 | — |
| <a id="api-ecbb8d48c4d0f767"></a>[`Inno.Native.Sdl3.SDLEventType.AudioDeviceFormatChanged = 4354`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1265) | 当前常量或枚举值：`4354`。 | — |
| <a id="api-a04ddaf63ceca86b"></a>[`Inno.Native.Sdl3.SDLEventType.AudioDeviceRemoved = 4353`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1264) | 当前常量或枚举值：`4353`。 | — |
| <a id="api-cd76647b1dacaa0b"></a>[`Inno.Native.Sdl3.SDLEventType.CameraDeviceAdded = 5120`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1275) | 当前常量或枚举值：`5120`。 | — |
| <a id="api-5f6f3227edcec59e"></a>[`Inno.Native.Sdl3.SDLEventType.CameraDeviceApproved = 5122`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1277) | 当前常量或枚举值：`5122`。 | — |
| <a id="api-b8569e6e64f13037"></a>[`Inno.Native.Sdl3.SDLEventType.CameraDeviceDenied = 5123`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1278) | 当前常量或枚举值：`5123`。 | — |
| <a id="api-7d09ecb20f5208d8"></a>[`Inno.Native.Sdl3.SDLEventType.CameraDeviceRemoved = 5121`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1276) | 当前常量或枚举值：`5121`。 | — |
| <a id="api-90066ec0c4756109"></a>[`Inno.Native.Sdl3.SDLEventType.ClipboardUpdate = 2304`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1257) | 当前常量或枚举值：`2304`。 | — |
| <a id="api-83f08b9c3fc4d550"></a>[`Inno.Native.Sdl3.SDLEventType.DidEnterBackground = 260`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1171) | 当前常量或枚举值：`260`。 | — |
| <a id="api-c6aef52f1dbb256b"></a>[`Inno.Native.Sdl3.SDLEventType.DidEnterForeground = 262`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1173) | 当前常量或枚举值：`262`。 | — |
| <a id="api-5273c4836fe5dd00"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayAdded = 338`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1177) | 当前常量或枚举值：`338`。 | — |
| <a id="api-cd1584dc397c0721"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayContentScaleChanged = 343`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1182) | 当前常量或枚举值：`343`。 | — |
| <a id="api-822974eb896adefe"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayCurrentModeChanged = 342`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1181) | 当前常量或枚举值：`342`。 | — |
| <a id="api-570851b1dd70edd8"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayDesktopModeChanged = 341`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1180) | 当前常量或枚举值：`341`。 | — |
| <a id="api-1cb2ae2b87e7c823"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayFirst = 337`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1184) | 当前常量或枚举值：`337`。 | — |
| <a id="api-e538ad7f363ad431"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayLast = 344`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1185) | 当前常量或枚举值：`344`。 | — |
| <a id="api-abd59f004bf48e76"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayMoved = 340`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1179) | 当前常量或枚举值：`340`。 | — |
| <a id="api-b762cec5f6931e80"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayOrientation = 337`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1176) | 当前常量或枚举值：`337`。 | — |
| <a id="api-9d9b9b5fe42dfe84"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayRemoved = 339`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1178) | 当前常量或枚举值：`339`。 | — |
| <a id="api-68acec38a89986b7"></a>[`Inno.Native.Sdl3.SDLEventType.DisplayUsableBoundsChanged = 344`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1183) | 当前常量或枚举值：`344`。 | — |
| <a id="api-b2780a8faeead9ee"></a>[`Inno.Native.Sdl3.SDLEventType.DropBegin = 4098`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1260) | 当前常量或枚举值：`4098`。 | — |
| <a id="api-96bb6701db21e0a9"></a>[`Inno.Native.Sdl3.SDLEventType.DropComplete = 4099`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1261) | 当前常量或枚举值：`4099`。 | — |
| <a id="api-c5910e821d50e89c"></a>[`Inno.Native.Sdl3.SDLEventType.DropFile = 4096`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1258) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-6cfd7913b5d0c411"></a>[`Inno.Native.Sdl3.SDLEventType.DropPosition = 4100`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1262) | 当前常量或枚举值：`4100`。 | — |
| <a id="api-17a31061f8b2b3ce"></a>[`Inno.Native.Sdl3.SDLEventType.DropText = 4097`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1259) | 当前常量或枚举值：`4097`。 | — |
| <a id="api-8743bb0b76649fad"></a>[`Inno.Native.Sdl3.SDLEventType.EnumPadding = 2147483647`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1289) | 当前常量或枚举值：`2147483647`。 | — |
| <a id="api-1f32f6209b38be46"></a>[`Inno.Native.Sdl3.SDLEventType.FingerCanceled = 1795`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1253) | 当前常量或枚举值：`1795`。 | — |
| <a id="api-0e9894e849f533f0"></a>[`Inno.Native.Sdl3.SDLEventType.FingerDown = 1792`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1250) | 当前常量或枚举值：`1792`。 | — |
| <a id="api-34925b751ea185f9"></a>[`Inno.Native.Sdl3.SDLEventType.FingerMotion = 1794`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1252) | 当前常量或枚举值：`1794`。 | — |
| <a id="api-e9fc0df39a749d72"></a>[`Inno.Native.Sdl3.SDLEventType.FingerUp = 1793`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1251) | 当前常量或枚举值：`1793`。 | — |
| <a id="api-40d03e6a9f65a08f"></a>[`Inno.Native.Sdl3.SDLEventType.First = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1166) | 当前常量或枚举值：`0`。 | — |
| <a id="api-8994b0b0d669b98a"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadAdded = 1619`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1241) | 当前常量或枚举值：`1619`。 | — |
| <a id="api-8b84932e51fbb265"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadAxisMotion = 1616`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1238) | 当前常量或枚举值：`1616`。 | — |
| <a id="api-7bdc74dc9e4c7591"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadButtonDown = 1617`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1239) | 当前常量或枚举值：`1617`。 | — |
| <a id="api-22fa3ff5f23620a3"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadButtonUp = 1618`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1240) | 当前常量或枚举值：`1618`。 | — |
| <a id="api-7443e502de6616da"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadRemapped = 1621`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1243) | 当前常量或枚举值：`1621`。 | — |
| <a id="api-d562be19821c2726"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadRemoved = 1620`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1242) | 当前常量或枚举值：`1620`。 | — |
| <a id="api-831d1190f6aef5f7"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadSensorUpdate = 1625`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1247) | 当前常量或枚举值：`1625`。 | — |
| <a id="api-026b5c77851becce"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadSteamHandleUpdated = 1627`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1249) | 当前常量或枚举值：`1627`。 | — |
| <a id="api-f34ba3e47b4d2344"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadTouchpadDown = 1622`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1244) | 当前常量或枚举值：`1622`。 | — |
| <a id="api-41f8f01cba6e0460"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadTouchpadMotion = 1623`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1245) | 当前常量或枚举值：`1623`。 | — |
| <a id="api-907d7cfbad03d4ee"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadTouchpadUp = 1624`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1246) | 当前常量或枚举值：`1624`。 | — |
| <a id="api-b4663f675900e993"></a>[`Inno.Native.Sdl3.SDLEventType.GamepadUpdateComplete = 1626`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1248) | 当前常量或枚举值：`1626`。 | — |
| <a id="api-056e864d069ba2a2"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickAdded = 1541`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1234) | 当前常量或枚举值：`1541`。 | — |
| <a id="api-0c31250194f1921e"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickAxisMotion = 1536`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1229) | 当前常量或枚举值：`1536`。 | — |
| <a id="api-95532d8eefddfe2b"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickBallMotion = 1537`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1230) | 当前常量或枚举值：`1537`。 | — |
| <a id="api-79ab8d67806cfe6a"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickBatteryUpdated = 1543`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1236) | 当前常量或枚举值：`1543`。 | — |
| <a id="api-adb42e34c93389e8"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickButtonDown = 1539`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1232) | 当前常量或枚举值：`1539`。 | — |
| <a id="api-2155bbfcd242f1db"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickButtonUp = 1540`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1233) | 当前常量或枚举值：`1540`。 | — |
| <a id="api-d7848ae06d993339"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickHatMotion = 1538`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1231) | 当前常量或枚举值：`1538`。 | — |
| <a id="api-623ffdacc7f62e02"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickRemoved = 1542`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1235) | 当前常量或枚举值：`1542`。 | — |
| <a id="api-536631e02c5197f9"></a>[`Inno.Native.Sdl3.SDLEventType.JoystickUpdateComplete = 1544`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1237) | 当前常量或枚举值：`1544`。 | — |
| <a id="api-951e491f323fef6d"></a>[`Inno.Native.Sdl3.SDLEventType.KeyboardAdded = 773`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1218) | 当前常量或枚举值：`773`。 | — |
| <a id="api-160c13f6ee896aa0"></a>[`Inno.Native.Sdl3.SDLEventType.KeyboardRemoved = 774`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1219) | 当前常量或枚举值：`774`。 | — |
| <a id="api-9da460c20129edc2"></a>[`Inno.Native.Sdl3.SDLEventType.KeyDown = 768`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1213) | 当前常量或枚举值：`768`。 | — |
| <a id="api-d710953d8ab82ec4"></a>[`Inno.Native.Sdl3.SDLEventType.KeymapChanged = 772`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1217) | 当前常量或枚举值：`772`。 | — |
| <a id="api-39d37dfdf8622d5f"></a>[`Inno.Native.Sdl3.SDLEventType.KeyUp = 769`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1214) | 当前常量或枚举值：`769`。 | — |
| <a id="api-b7aa0b43855d2f88"></a>[`Inno.Native.Sdl3.SDLEventType.Last = 65535`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1288) | 当前常量或枚举值：`65535`。 | — |
| <a id="api-208ab421deb0bce4"></a>[`Inno.Native.Sdl3.SDLEventType.LocaleChanged = 263`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1174) | 当前常量或枚举值：`263`。 | — |
| <a id="api-ea927af28c0a6385"></a>[`Inno.Native.Sdl3.SDLEventType.LowMemory = 258`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1169) | 当前常量或枚举值：`258`。 | — |
| <a id="api-75ffe3a7960f14fa"></a>[`Inno.Native.Sdl3.SDLEventType.MouseAdded = 1028`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1227) | 当前常量或枚举值：`1028`。 | — |
| <a id="api-2c4366cc21c43a65"></a>[`Inno.Native.Sdl3.SDLEventType.MouseButtonDown = 1025`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1224) | 当前常量或枚举值：`1025`。 | — |
| <a id="api-9b020e4771c0f39e"></a>[`Inno.Native.Sdl3.SDLEventType.MouseButtonUp = 1026`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1225) | 当前常量或枚举值：`1026`。 | — |
| <a id="api-e986bfec4174c856"></a>[`Inno.Native.Sdl3.SDLEventType.MouseMotion = 1024`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1223) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-8ba929e2db0726b6"></a>[`Inno.Native.Sdl3.SDLEventType.MouseRemoved = 1029`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1228) | 当前常量或枚举值：`1029`。 | — |
| <a id="api-1338e002d02cb71d"></a>[`Inno.Native.Sdl3.SDLEventType.MouseWheel = 1027`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1226) | 当前常量或枚举值：`1027`。 | — |
| <a id="api-dc3c2af927cdc2b6"></a>[`Inno.Native.Sdl3.SDLEventType.PenAxis = 4871`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1274) | 当前常量或枚举值：`4871`。 | — |
| <a id="api-7464e4efd3c0b7a8"></a>[`Inno.Native.Sdl3.SDLEventType.PenButtonDown = 4868`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1271) | 当前常量或枚举值：`4868`。 | — |
| <a id="api-b81e6204ba912ce2"></a>[`Inno.Native.Sdl3.SDLEventType.PenButtonUp = 4869`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1272) | 当前常量或枚举值：`4869`。 | — |
| <a id="api-a01f78390817466b"></a>[`Inno.Native.Sdl3.SDLEventType.PenDown = 4866`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1269) | 当前常量或枚举值：`4866`。 | — |
| <a id="api-a65c68b64df074cd"></a>[`Inno.Native.Sdl3.SDLEventType.PenMotion = 4870`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1273) | 当前常量或枚举值：`4870`。 | — |
| <a id="api-7b9efe5043b82e8f"></a>[`Inno.Native.Sdl3.SDLEventType.PenProximityIn = 4864`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1267) | 当前常量或枚举值：`4864`。 | — |
| <a id="api-8516bf890f871e11"></a>[`Inno.Native.Sdl3.SDLEventType.PenProximityOut = 4865`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1268) | 当前常量或枚举值：`4865`。 | — |
| <a id="api-854b294ec87962e2"></a>[`Inno.Native.Sdl3.SDLEventType.PenUp = 4867`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1270) | 当前常量或枚举值：`4867`。 | — |
| <a id="api-d64d08e5015f53f2"></a>[`Inno.Native.Sdl3.SDLEventType.PinchBegin = 1808`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1254) | 当前常量或枚举值：`1808`。 | — |
| <a id="api-903fbb7275d83290"></a>[`Inno.Native.Sdl3.SDLEventType.PinchEnd = 1810`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1256) | 当前常量或枚举值：`1810`。 | — |
| <a id="api-971f4c7ec00deddd"></a>[`Inno.Native.Sdl3.SDLEventType.PinchUpdate = 1809`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1255) | 当前常量或枚举值：`1809`。 | — |
| <a id="api-d6a4e5236882dea3"></a>[`Inno.Native.Sdl3.SDLEventType.PollSentinel = 32512`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1286) | 当前常量或枚举值：`32512`。 | — |
| <a id="api-df0ba8fba690fd13"></a>[`Inno.Native.Sdl3.SDLEventType.Private0 = 16384`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1282) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-8d41bd9bf3e4ad85"></a>[`Inno.Native.Sdl3.SDLEventType.Private1 = 16385`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1283) | 当前常量或枚举值：`16385`。 | — |
| <a id="api-93fa7e2d7e9a58f4"></a>[`Inno.Native.Sdl3.SDLEventType.Private2 = 16386`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1284) | 当前常量或枚举值：`16386`。 | — |
| <a id="api-8b225911431cc75c"></a>[`Inno.Native.Sdl3.SDLEventType.Private3 = 16387`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1285) | 当前常量或枚举值：`16387`。 | — |
| <a id="api-6bd870e14c8cb8ff"></a>[`Inno.Native.Sdl3.SDLEventType.Quit = 256`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1167) | 当前常量或枚举值：`256`。 | — |
| <a id="api-84ba88462d742ee2"></a>[`Inno.Native.Sdl3.SDLEventType.RenderDeviceLost = 8194`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1281) | 当前常量或枚举值：`8194`。 | — |
| <a id="api-2b91bbd463cbcf81"></a>[`Inno.Native.Sdl3.SDLEventType.RenderDeviceReset = 8193`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1280) | 当前常量或枚举值：`8193`。 | — |
| <a id="api-5aafdc190d473699"></a>[`Inno.Native.Sdl3.SDLEventType.RenderTargetsReset = 8192`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1279) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-ae3c4770c2d4a6eb"></a>[`Inno.Native.Sdl3.SDLEventType.ScreenKeyboardHidden = 777`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1222) | 当前常量或枚举值：`777`。 | — |
| <a id="api-813e9537cbadbb06"></a>[`Inno.Native.Sdl3.SDLEventType.ScreenKeyboardShown = 776`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1221) | 当前常量或枚举值：`776`。 | — |
| <a id="api-931686f7f1008434"></a>[`Inno.Native.Sdl3.SDLEventType.SensorUpdate = 4608`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1266) | 当前常量或枚举值：`4608`。 | — |
| <a id="api-d75d4f2bb949dcfd"></a>[`Inno.Native.Sdl3.SDLEventType.SystemThemeChanged = 264`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1175) | 当前常量或枚举值：`264`。 | — |
| <a id="api-b14d3f85bc465c81"></a>[`Inno.Native.Sdl3.SDLEventType.Terminating = 257`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1168) | 当前常量或枚举值：`257`。 | — |
| <a id="api-be905e2b0e7a3e51"></a>[`Inno.Native.Sdl3.SDLEventType.TextEditing = 770`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1215) | 当前常量或枚举值：`770`。 | — |
| <a id="api-677371bcc902b72c"></a>[`Inno.Native.Sdl3.SDLEventType.TextEditingCandidates = 775`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1220) | 当前常量或枚举值：`775`。 | — |
| <a id="api-fbab1764ee4955c8"></a>[`Inno.Native.Sdl3.SDLEventType.TextInput = 771`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1216) | 当前常量或枚举值：`771`。 | — |
| <a id="api-1482bfb4df17fbde"></a>[`Inno.Native.Sdl3.SDLEventType.User = 32768`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1287) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-fa7192bd71d32fcd"></a>[`Inno.Native.Sdl3.SDLEventType.WillEnterBackground = 259`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1170) | 当前常量或枚举值：`259`。 | — |
| <a id="api-454c0470e91926d4"></a>[`Inno.Native.Sdl3.SDLEventType.WillEnterForeground = 261`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1172) | 当前常量或枚举值：`261`。 | — |
| <a id="api-32bd4ea6dc6ba6d0"></a>[`Inno.Native.Sdl3.SDLEventType.WindowCloseRequested = 528`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1200) | 当前常量或枚举值：`528`。 | — |
| <a id="api-0c5f179ae923d13a"></a>[`Inno.Native.Sdl3.SDLEventType.WindowDestroyed = 537`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1209) | 当前常量或枚举值：`537`。 | — |
| <a id="api-f4d6713dcb2abe14"></a>[`Inno.Native.Sdl3.SDLEventType.WindowDisplayChanged = 531`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1203) | 当前常量或枚举值：`531`。 | — |
| <a id="api-acac75bfc3d5db85"></a>[`Inno.Native.Sdl3.SDLEventType.WindowDisplayScaleChanged = 532`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1204) | 当前常量或枚举值：`532`。 | — |
| <a id="api-c8cdd9e398b4b2b4"></a>[`Inno.Native.Sdl3.SDLEventType.WindowEnterFullscreen = 535`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1207) | 当前常量或枚举值：`535`。 | — |
| <a id="api-7f550f36bd31363c"></a>[`Inno.Native.Sdl3.SDLEventType.WindowExposed = 516`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1188) | 当前常量或枚举值：`516`。 | — |
| <a id="api-5c27652ed76b9e46"></a>[`Inno.Native.Sdl3.SDLEventType.WindowFirst = 514`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1211) | 当前常量或枚举值：`514`。 | — |
| <a id="api-90e2032c45fcf2c8"></a>[`Inno.Native.Sdl3.SDLEventType.WindowFocusGained = 526`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1198) | 当前常量或枚举值：`526`。 | — |
| <a id="api-dd6171f35e19bcc7"></a>[`Inno.Native.Sdl3.SDLEventType.WindowFocusLost = 527`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1199) | 当前常量或枚举值：`527`。 | — |
| <a id="api-ddf32f7d0ddf6b66"></a>[`Inno.Native.Sdl3.SDLEventType.WindowHdrStateChanged = 538`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1210) | 当前常量或枚举值：`538`。 | — |
| <a id="api-d50c11157163efc2"></a>[`Inno.Native.Sdl3.SDLEventType.WindowHidden = 515`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1187) | 当前常量或枚举值：`515`。 | — |
| <a id="api-3908d25b6489f1a9"></a>[`Inno.Native.Sdl3.SDLEventType.WindowHitTest = 529`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1201) | 当前常量或枚举值：`529`。 | — |
| <a id="api-556a82e6ff0d7e19"></a>[`Inno.Native.Sdl3.SDLEventType.WindowIccprofChanged = 530`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1202) | 当前常量或枚举值：`530`。 | — |
| <a id="api-9c0b67c8e56974bd"></a>[`Inno.Native.Sdl3.SDLEventType.WindowLast = 538`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1212) | 当前常量或枚举值：`538`。 | — |
| <a id="api-31cba988c14a4110"></a>[`Inno.Native.Sdl3.SDLEventType.WindowLeaveFullscreen = 536`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1208) | 当前常量或枚举值：`536`。 | — |
| <a id="api-a6f31c9992b4b288"></a>[`Inno.Native.Sdl3.SDLEventType.WindowMaximized = 522`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1194) | 当前常量或枚举值：`522`。 | — |
| <a id="api-e852e13fc881c764"></a>[`Inno.Native.Sdl3.SDLEventType.WindowMetalViewResized = 520`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1192) | 当前常量或枚举值：`520`。 | — |
| <a id="api-878e5068d3a1328e"></a>[`Inno.Native.Sdl3.SDLEventType.WindowMinimized = 521`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1193) | 当前常量或枚举值：`521`。 | — |
| <a id="api-11d3d3732dc915f5"></a>[`Inno.Native.Sdl3.SDLEventType.WindowMouseEnter = 524`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1196) | 当前常量或枚举值：`524`。 | — |
| <a id="api-40fc8d0ef4acfd2b"></a>[`Inno.Native.Sdl3.SDLEventType.WindowMouseLeave = 525`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1197) | 当前常量或枚举值：`525`。 | — |
| <a id="api-08244a9dd273f18b"></a>[`Inno.Native.Sdl3.SDLEventType.WindowMoved = 517`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1189) | 当前常量或枚举值：`517`。 | — |
| <a id="api-906b786c3bb83ee1"></a>[`Inno.Native.Sdl3.SDLEventType.WindowOccluded = 534`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1206) | 当前常量或枚举值：`534`。 | — |
| <a id="api-66efd37868e68a5e"></a>[`Inno.Native.Sdl3.SDLEventType.WindowPixelSizeChanged = 519`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1191) | 当前常量或枚举值：`519`。 | — |
| <a id="api-36e3fcf5bd53a2c5"></a>[`Inno.Native.Sdl3.SDLEventType.WindowResized = 518`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1190) | 当前常量或枚举值：`518`。 | — |
| <a id="api-7e85a1dd57a9d107"></a>[`Inno.Native.Sdl3.SDLEventType.WindowRestored = 523`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1195) | 当前常量或枚举值：`523`。 | — |
| <a id="api-bf2ab0a4926f3d78"></a>[`Inno.Native.Sdl3.SDLEventType.WindowSafeAreaChanged = 533`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1205) | 当前常量或枚举值：`533`。 | — |
| <a id="api-99636ec739cb48fb"></a>[`Inno.Native.Sdl3.SDLEventType.WindowShown = 514`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1186) | 当前常量或枚举值：`514`。 | — |
| <a id="api-26886c92e85bc48d"></a>[`public enum Inno.Native.Sdl3.SDLEventType : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1164) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFColor</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFColor 971953fc663c907b3856459e32e575c78b0cd9fc2fca25be57dcc2f7b60d2c43 -->
<!-- api: F:Inno.Native.Sdl3.SDLFColor.R 0d1306de4ea93789b93966bb2a3f8490d785fa41894aea453bc85bb2f725b1b1 -->
<!-- api: F:Inno.Native.Sdl3.SDLFColor.G 2652fa3d2e590010f0305de3cf96fe5a5d220be660482505d377681e0f72f1ea -->
<!-- api: F:Inno.Native.Sdl3.SDLFColor.B 3e8300c1206003982c043c9a5314384f74d0591c138caf95e329cdf112811957 -->
<!-- api: F:Inno.Native.Sdl3.SDLFColor.A 135caa6d92d1fa5ed8b5303d196f2ead5ae04203dc5a78effb6cd44f346d9e10 -->
<!-- api: M:Inno.Native.Sdl3.SDLFColor.#ctor(System.Single,System.Single,System.Single,System.Single) e5e85d07f55c88ebd45c4398f36ca56043c8551543facf98777a6199561cb1bc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d154b0c2abe45d63"></a>[`public float Inno.Native.Sdl3.SDLFColor.A`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2347) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-44a23f9321739fc1"></a>[`public float Inno.Native.Sdl3.SDLFColor.B`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2346) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-649bc0d0139e8dbd"></a>[`public float Inno.Native.Sdl3.SDLFColor.G`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2345) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d5b800ca9b85105"></a>[`public float Inno.Native.Sdl3.SDLFColor.R`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2344) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a8877860cc635943"></a>[`public Inno.Native.Sdl3.SDLFColor.SDLFColor(float r = 0, float g = 0, float b = 0, float a = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2348) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ff6e39fe33f1a4b7"></a>[`public struct Inno.Native.Sdl3.SDLFColor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2342) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFColorPtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFColorPtr d9ec2785dc1ecbae814c4a46749eb46606547f43d721eccea27d21a844c2b459 -->
<!-- api: M:Inno.Native.Sdl3.SDLFColorPtr.#ctor(Inno.Native.Sdl3.SDLFColor*) c3379f4998e51046440016992bf4de37119f4a0b2dff9b8a437d9c799b2d55d1 -->
<!-- api: F:Inno.Native.Sdl3.SDLFColorPtr.Handle 447a42d5eb2e7684b6437698345fe77f98f28040aa7c1b43d5e0e4ddde0e2aa0 -->
<!-- api: P:Inno.Native.Sdl3.SDLFColorPtr.IsNull cdd574b2aa7c82e2943171f73bba57055c67cf12c0930162db71c06a7829c81a -->
<!-- api: P:Inno.Native.Sdl3.SDLFColorPtr.Null 3a82bf13cb25d763e15fdcea089eda27714a021bfc4c4acff9a942c0cea93167 -->
<!-- api: P:Inno.Native.Sdl3.SDLFColorPtr.Item(System.Int32) 58ea58414372190be64908e509397d83abeb5cea17edc9769a3314d5bec5d74e -->
<!-- api: M:Inno.Native.Sdl3.SDLFColorPtr.op_Implicit(Inno.Native.Sdl3.SDLFColor*)~Inno.Native.Sdl3.SDLFColorPtr 630d15f7cf9a96ba0384b0ab099cb453035445e7e4fe2d08153f28fc2f6e09d4 -->
<!-- api: M:Inno.Native.Sdl3.SDLFColorPtr.op_Implicit(Inno.Native.Sdl3.SDLFColorPtr)~Inno.Native.Sdl3.SDLFColor* f705b293bac106e40188898dc9860ddf81718d68aef1f0de6f89586d56f3c9f9 -->
<!-- api: M:Inno.Native.Sdl3.SDLFColorPtr.op_Equality(Inno.Native.Sdl3.SDLFColorPtr,Inno.Native.Sdl3.SDLFColorPtr) ba7bf0af10d3df1fe6f92e33c387d187e3bbd41e05e8926512d0200801dc0610 -->
<!-- api: M:Inno.Native.Sdl3.SDLFColorPtr.op_Inequality(Inno.Native.Sdl3.SDLFColorPtr,Inno.Native.Sdl3.SDLFColorPtr) f88f16c3358a00226a04ef81acfbedf0bd5eacd3d3c767dfa7bb36c584f99e98 -->
<!-- api: M:Inno.Native.Sdl3.SDLFColorPtr.Equals(Inno.Native.Sdl3.SDLFColorPtr) e365d604ea7b69be5aa198e68cfffb00379631393fca1b82bf0ab0be40f11af0 -->
<!-- api: M:Inno.Native.Sdl3.SDLFColorPtr.Equals(System.Object) 4ff2ffb05192066c62a185d5365fa1c4bed6d718af7a886c78123d6ea7437423 -->
<!-- api: M:Inno.Native.Sdl3.SDLFColorPtr.GetHashCode e0ed90e9f4ff25e079ba37f5af93d9673ba8cb42df9fe48e250c83399c23e776 -->
<!-- api: P:Inno.Native.Sdl3.SDLFColorPtr.R fbb7504abce28c342c8dc526e90ca9a6a3d64102bbb9ba6917d065fbb2582349 -->
<!-- api: P:Inno.Native.Sdl3.SDLFColorPtr.G 1f34e7964399b5f678e5177a6664bd30eba39e7633eb6e70b668154a4e6269a6 -->
<!-- api: P:Inno.Native.Sdl3.SDLFColorPtr.B 47189556a06e2f90e13301bb33b3aac1ec44d5ab2417578d7fe59ee28d726efb -->
<!-- api: P:Inno.Native.Sdl3.SDLFColorPtr.A c587f7fe98a63321284206e3aed59bc477121b423836e30b531d8144dc8b7474 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-39f0cc6a5f7edf80"></a>[`public Inno.Native.Sdl3.SDLFColor* Inno.Native.Sdl3.SDLFColorPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8326) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f74c631deeaa568"></a>[`public Inno.Native.Sdl3.SDLFColorPtr.SDLFColorPtr(Inno.Native.Sdl3.SDLFColor* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8321) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-397e550d77895c64"></a>[`public bool Inno.Native.Sdl3.SDLFColorPtr.Equals(Inno.Native.Sdl3.SDLFColorPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8336) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-0d8951de69fcb31a"></a>[`public override bool Inno.Native.Sdl3.SDLFColorPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8337) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-b55370db9730e931"></a>[`public override int Inno.Native.Sdl3.SDLFColorPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8338) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-367894fff9b5a0ec"></a>[`public static bool Inno.Native.Sdl3.SDLFColorPtr.operator ==(Inno.Native.Sdl3.SDLFColorPtr left, Inno.Native.Sdl3.SDLFColorPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8334) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-74c6a75cacf6814b"></a>[`public static Inno.Native.Sdl3.SDLFColorPtr.implicit operator Inno.Native.Sdl3.SDLFColorPtr(Inno.Native.Sdl3.SDLFColor* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8332) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-59b330b4737efc73"></a>[`public static Inno.Native.Sdl3.SDLFColorPtr.implicit operator Inno.Native.Sdl3.SDLFColor*(Inno.Native.Sdl3.SDLFColorPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8333) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-32e9dd37f5a3f5f2"></a>[`public static bool Inno.Native.Sdl3.SDLFColorPtr.operator !=(Inno.Native.Sdl3.SDLFColorPtr left, Inno.Native.Sdl3.SDLFColorPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8335) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-43ad6c0ee1b4f8c9"></a>[`public ref float Inno.Native.Sdl3.SDLFColorPtr.A { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8343) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-123340b922ac2f02"></a>[`public ref float Inno.Native.Sdl3.SDLFColorPtr.B { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8342) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0ba30aa5c3c68201"></a>[`public ref float Inno.Native.Sdl3.SDLFColorPtr.G { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8341) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0e788dce2e577cca"></a>[`public bool Inno.Native.Sdl3.SDLFColorPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8327) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8b9fb982d7252dd0"></a>[`public Inno.Native.Sdl3.SDLFColor Inno.Native.Sdl3.SDLFColorPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8330) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1d6750e93174c8ee"></a>[`public static Inno.Native.Sdl3.SDLFColorPtr Inno.Native.Sdl3.SDLFColorPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8328) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4ef56a9822c77b9e"></a>[`public ref float Inno.Native.Sdl3.SDLFColorPtr.R { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8340) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6c75df1de4ea9f22"></a>[`public struct Inno.Native.Sdl3.SDLFColorPtr : System.IEquatable<Inno.Native.Sdl3.SDLFColorPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8319) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFileDialogType</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFileDialogType d4ed12231c97ccf6c437bdaeb44b7b112300468dadb2d94d7803180e0bcc452c -->
<!-- api: F:Inno.Native.Sdl3.SDLFileDialogType.FiledialogOpenfile 9bd477b30d10cd740c5609d0f60e84dac85e28def335830d4c6056652f0e3601 -->
<!-- api: F:Inno.Native.Sdl3.SDLFileDialogType.FiledialogSavefile 1877fa3387a3688e93493ba6a252271e95a2c324e7d4f99d1bcc9a55f51f3df4 -->
<!-- api: F:Inno.Native.Sdl3.SDLFileDialogType.FiledialogOpenfolder 6ff734ef7fb9311dfadb5b2a79aaf3703a412b143941425e78c2dfcf07b30bbc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ce8fc67df8a1f13b"></a>[`Inno.Native.Sdl3.SDLFileDialogType.FiledialogOpenfile = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L644) | 当前常量或枚举值：`0`。 | — |
| <a id="api-0281bda346d7eeb9"></a>[`Inno.Native.Sdl3.SDLFileDialogType.FiledialogOpenfolder = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L646) | 当前常量或枚举值：`2`。 | — |
| <a id="api-f977bf7e7b895b3f"></a>[`Inno.Native.Sdl3.SDLFileDialogType.FiledialogSavefile = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L645) | 当前常量或枚举值：`1`。 | — |
| <a id="api-1edd8277df6c32dd"></a>[`public enum Inno.Native.Sdl3.SDLFileDialogType : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L642) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFinger</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFinger 1613f3f431fffa3e1449ad33bc0af8e6fb654a325c9e0e7a8e9c90e9f0b22ddf -->
<!-- api: F:Inno.Native.Sdl3.SDLFinger.Id 6691b060b5f88e10ba1d8b1d4bd5497d7354d8a1888afecbae2b257b4fb6d4ce -->
<!-- api: F:Inno.Native.Sdl3.SDLFinger.X bf2c3f768e6e94f74875413db337323e26746666056ce35be39e11e1683e80d9 -->
<!-- api: F:Inno.Native.Sdl3.SDLFinger.Y 1967afefabe666d67b7de2661595b8c6db651b6ecd125e0de7fec1cf4f9eef27 -->
<!-- api: F:Inno.Native.Sdl3.SDLFinger.Pressure 56a0549407f1cebf6c5c1715e6083b7e0de076fcb0343652144f8beb037b3d28 -->
<!-- api: M:Inno.Native.Sdl3.SDLFinger.#ctor(System.UInt64,System.Single,System.Single,System.Single) 91bdf3fa206a0db647487fe40f658da1a8ded74d1a1c87408f46169b610662d7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-17429b7d3ddd1b54"></a>[`public ulong Inno.Native.Sdl3.SDLFinger.Id`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3054) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1354a166790862a9"></a>[`public float Inno.Native.Sdl3.SDLFinger.Pressure`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3057) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e981bb160f5f79d"></a>[`public float Inno.Native.Sdl3.SDLFinger.X`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3055) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-32e35cf27eb5264a"></a>[`public float Inno.Native.Sdl3.SDLFinger.Y`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3056) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5d9e9ce31dd41016"></a>[`public Inno.Native.Sdl3.SDLFinger.SDLFinger(ulong id = 0, float x = 0, float y = 0, float pressure = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3058) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-dc5c6f0500ccb6a1"></a>[`public struct Inno.Native.Sdl3.SDLFinger`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3052) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFingerPtrPtr</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFingerPtrPtr 726cd20d6a8503ee9effe851a663f72f1d6abb7095befac56c0a07fc07d5275e -->
<!-- api: M:Inno.Native.Sdl3.SDLFingerPtrPtr.#ctor(Inno.Native.Sdl3.SDLFinger**) 0e745c1fa80ae583186bb7cd999cdd667a388696e59328c74234c2f83786c363 -->
<!-- api: F:Inno.Native.Sdl3.SDLFingerPtrPtr.Handle f4d7faf5d91e549860a55af8b970a5ff2d135d3d5b070531ea4d4f9e86371784 -->
<!-- api: P:Inno.Native.Sdl3.SDLFingerPtrPtr.IsNull fe2996918fae79c3ca5c92e061fa1c411b4958c01724a5094a059a4d254b9cd8 -->
<!-- api: P:Inno.Native.Sdl3.SDLFingerPtrPtr.Null 56daad18890b9dad00461d88c294533748c9d96ea84fd14add2b9aadde250e63 -->
<!-- api: P:Inno.Native.Sdl3.SDLFingerPtrPtr.Item(System.Int32) 6fb039522406426fe0a8cc789e97a1eab17e9351825f2c5e0cd1d43f07541157 -->
<!-- api: M:Inno.Native.Sdl3.SDLFingerPtrPtr.op_Implicit(Inno.Native.Sdl3.SDLFinger**)~Inno.Native.Sdl3.SDLFingerPtrPtr 40eb186fd6f460b191ab8f795dd86c0b3af11ca180317e0654b0e122681fa8a2 -->
<!-- api: M:Inno.Native.Sdl3.SDLFingerPtrPtr.op_Implicit(Inno.Native.Sdl3.SDLFingerPtrPtr)~Inno.Native.Sdl3.SDLFinger** 3178ccf0b34fabeabe6f8e217280c3ef470bd4083a0b65d853f7b8188bbc69d3 -->
<!-- api: M:Inno.Native.Sdl3.SDLFingerPtrPtr.op_Equality(Inno.Native.Sdl3.SDLFingerPtrPtr,Inno.Native.Sdl3.SDLFingerPtrPtr) b266d1834c36404b0fe4541ff051160d42ba6a81817bfc69fe4615c7543bd8b1 -->
<!-- api: M:Inno.Native.Sdl3.SDLFingerPtrPtr.op_Inequality(Inno.Native.Sdl3.SDLFingerPtrPtr,Inno.Native.Sdl3.SDLFingerPtrPtr) 7e90d7acc1fe7853ed63d1cc64f81326e0346c3b8e5a198ae137ec2917ce0776 -->
<!-- api: M:Inno.Native.Sdl3.SDLFingerPtrPtr.Equals(Inno.Native.Sdl3.SDLFingerPtrPtr) a2f8cfa69330a704a8c2f1add9a982de5f52d9cea8a06e9cd0de8ae2deac69c6 -->
<!-- api: M:Inno.Native.Sdl3.SDLFingerPtrPtr.Equals(System.Object) 0652fd5d827eaef9b4d3f10784ae5eaf5a52dc0fa61865ab84977f962924eaa8 -->
<!-- api: M:Inno.Native.Sdl3.SDLFingerPtrPtr.GetHashCode 1afcc079683274b12270637cebba86e312c2da10312a4e9388d0242415fa636e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-92c2528c674c7593"></a>[`public Inno.Native.Sdl3.SDLFinger** Inno.Native.Sdl3.SDLFingerPtrPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8408) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ff194300f39bf746"></a>[`public Inno.Native.Sdl3.SDLFingerPtrPtr.SDLFingerPtrPtr(Inno.Native.Sdl3.SDLFinger** handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8403) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3e96eaa741a9316d"></a>[`public bool Inno.Native.Sdl3.SDLFingerPtrPtr.Equals(Inno.Native.Sdl3.SDLFingerPtrPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8418) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-bc3c8dc7ec00f28a"></a>[`public override bool Inno.Native.Sdl3.SDLFingerPtrPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8419) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-2a5926716126bbcf"></a>[`public override int Inno.Native.Sdl3.SDLFingerPtrPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8420) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-baf9162e2de57936"></a>[`public static bool Inno.Native.Sdl3.SDLFingerPtrPtr.operator ==(Inno.Native.Sdl3.SDLFingerPtrPtr left, Inno.Native.Sdl3.SDLFingerPtrPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8416) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5bb7236aef1470f2"></a>[`public static Inno.Native.Sdl3.SDLFingerPtrPtr.implicit operator Inno.Native.Sdl3.SDLFingerPtrPtr(Inno.Native.Sdl3.SDLFinger** handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8414) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-45c312e22923e56a"></a>[`public static Inno.Native.Sdl3.SDLFingerPtrPtr.implicit operator Inno.Native.Sdl3.SDLFinger**(Inno.Native.Sdl3.SDLFingerPtrPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8415) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d8dff3266f5a4593"></a>[`public static bool Inno.Native.Sdl3.SDLFingerPtrPtr.operator !=(Inno.Native.Sdl3.SDLFingerPtrPtr left, Inno.Native.Sdl3.SDLFingerPtrPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8417) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-24e2196cd4bc5829"></a>[`public bool Inno.Native.Sdl3.SDLFingerPtrPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8409) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f35a3eb3a89aade4"></a>[`public Inno.Native.Sdl3.SDLFinger* Inno.Native.Sdl3.SDLFingerPtrPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8412) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8bcc180754eb3edd"></a>[`public static Inno.Native.Sdl3.SDLFingerPtrPtr Inno.Native.Sdl3.SDLFingerPtrPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8410) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6e09f6650f8c9eb9"></a>[`public struct Inno.Native.Sdl3.SDLFingerPtrPtr : System.IEquatable<Inno.Native.Sdl3.SDLFingerPtrPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8401) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFlashOperation</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFlashOperation 11dcd79a4e9e3f4d192e587f2672f8aa91c1fc68748b7b82af3f7ba60128bccf -->
<!-- api: F:Inno.Native.Sdl3.SDLFlashOperation.Cancel a5d0368dc7b749d09c474a238283a1233cbb73ecb6b2263047e3eeebad7af7cd -->
<!-- api: F:Inno.Native.Sdl3.SDLFlashOperation.Briefly 2f9214d0524e10c78a1947f134519e297de6afce84f36c7d945ec5d452f9b3e5 -->
<!-- api: F:Inno.Native.Sdl3.SDLFlashOperation.UntilFocused 0d58fc54fef3698e87a7a48b9b51424ba530062099c15b572b83ee79727883da -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4ad0e085e6dd0f01"></a>[`Inno.Native.Sdl3.SDLFlashOperation.Briefly = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L570) | 当前常量或枚举值：`1`。 | — |
| <a id="api-8ff623ce5824ff27"></a>[`Inno.Native.Sdl3.SDLFlashOperation.Cancel = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L569) | 当前常量或枚举值：`0`。 | — |
| <a id="api-b002c5ddbb3e271d"></a>[`Inno.Native.Sdl3.SDLFlashOperation.UntilFocused = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L571) | 当前常量或枚举值：`2`。 | — |
| <a id="api-cacf1d54850f149c"></a>[`public enum Inno.Native.Sdl3.SDLFlashOperation : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L567) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFlipMode</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFlipMode 050f1733527e091668db4a2b563549491bd977d5070c6688ca630f1e81d5589e -->
<!-- api: F:Inno.Native.Sdl3.SDLFlipMode.None bfb1676df45288da3d67890cc79ad7313ce3606fdeeeffca845e564cd7302bcc -->
<!-- api: F:Inno.Native.Sdl3.SDLFlipMode.Horizontal b2b42a2e3ff3c4a1442cba52375a32d70cbbfe7eac17a5b1a1672c51a2e02c43 -->
<!-- api: F:Inno.Native.Sdl3.SDLFlipMode.Vertical bba94b3f69d9bc04b70303fea026649f1a077d4cbfe9a8ad77f29578420e2f07 -->
<!-- api: F:Inno.Native.Sdl3.SDLFlipMode.HorizontalAndVertical dd994392406fac36ab3c86635b49c44e80e65c6a6cf604148a8243e8cc68c623 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f90a5ad2ecef5162"></a>[`Inno.Native.Sdl3.SDLFlipMode.Horizontal = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L517) | 当前常量或枚举值：`1`。 | — |
| <a id="api-9c5e602ff507cc7f"></a>[`Inno.Native.Sdl3.SDLFlipMode.HorizontalAndVertical = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L519) | 当前常量或枚举值：`3`。 | — |
| <a id="api-7d921922969440d7"></a>[`Inno.Native.Sdl3.SDLFlipMode.None = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L516) | 当前常量或枚举值：`0`。 | — |
| <a id="api-a2d6a1e268881160"></a>[`Inno.Native.Sdl3.SDLFlipMode.Vertical = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L518) | 当前常量或枚举值：`2`。 | — |
| <a id="api-5b54416d01689b5c"></a>[`public enum Inno.Native.Sdl3.SDLFlipMode : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L514) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFolder</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFolder e9423c19656199478436195ce1ae8f1b4fde598e6ba9f27ad3386c21dc37ebe2 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.Home c1f7107158cc582ea97353b285bebe9aa1a8837e5a8b5abf407fd112959cae51 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.Desktop 4ac7ace322bed5f2fc065a5d88e3f4971fe20ac4f3ca341bfdc0a2a21e858723 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.Documents 913f38c7d978a3a286a1f37b74e825e1e1bd04d84b27b0dbe750c29a700618e3 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.Downloads 5938f77e5b49b2d1802fe6285c1e91cfcd464cc9367398a77992ffd144b12244 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.Music 5604fad051166dbf674ad5f5a4a82cf76ce54d7f12b91117128937e853eb05d6 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.Pictures 4e2c2fcf27af744a0ed44bf49131a94b61dd0be46d348cef247862b2b36bf9b2 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.Publicshare 596d6865b06e8835ca3a1461c51241d068276f029cf4f9bd3b14254a2cf206f1 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.Savedgames 868762ab702d271a2a77a0bc7b81a7cd3434e5137748519ee3b128e4ef7c4d52 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.Screenshots 64b2c9a3ad2838089e54bba6434728a76f4c84ed650e85b789bc456a1e89d03c -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.Templates a72705f3c85b47ad1a701546b45ad30e72c2765ed5b2d61554ac343348457128 -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.Videos 7c02008673db717310576507f2755c5cf36740e04b300e9b007ecf098108d8ac -->
<!-- api: F:Inno.Native.Sdl3.SDLFolder.Count f8b013bdfacafd5cbc614c01d7dba22b69251436469382b5b924f2559195739d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-36311aaa41f7e580"></a>[`Inno.Native.Sdl3.SDLFolder.Count = 11`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1318) | 当前常量或枚举值：`11`。 | — |
| <a id="api-445a7490d4e5037c"></a>[`Inno.Native.Sdl3.SDLFolder.Desktop = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1308) | 当前常量或枚举值：`1`。 | — |
| <a id="api-9f80a1616ed6dcd2"></a>[`Inno.Native.Sdl3.SDLFolder.Documents = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1309) | 当前常量或枚举值：`2`。 | — |
| <a id="api-29c7b85befbd8613"></a>[`Inno.Native.Sdl3.SDLFolder.Downloads = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1310) | 当前常量或枚举值：`3`。 | — |
| <a id="api-d0f6796d528ff4ff"></a>[`Inno.Native.Sdl3.SDLFolder.Home = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1307) | 当前常量或枚举值：`0`。 | — |
| <a id="api-f40ded86b09b7b28"></a>[`Inno.Native.Sdl3.SDLFolder.Music = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1311) | 当前常量或枚举值：`4`。 | — |
| <a id="api-1642afadb40f10e3"></a>[`Inno.Native.Sdl3.SDLFolder.Pictures = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1312) | 当前常量或枚举值：`5`。 | — |
| <a id="api-5a73f75ff3ed1902"></a>[`Inno.Native.Sdl3.SDLFolder.Publicshare = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1313) | 当前常量或枚举值：`6`。 | — |
| <a id="api-797e254c4079e0c0"></a>[`Inno.Native.Sdl3.SDLFolder.Savedgames = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1314) | 当前常量或枚举值：`7`。 | — |
| <a id="api-280f374ff4639738"></a>[`Inno.Native.Sdl3.SDLFolder.Screenshots = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1315) | 当前常量或枚举值：`8`。 | — |
| <a id="api-6e5afbc8651813c4"></a>[`Inno.Native.Sdl3.SDLFolder.Templates = 9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1316) | 当前常量或枚举值：`9`。 | — |
| <a id="api-b92e800222a4e99e"></a>[`Inno.Native.Sdl3.SDLFolder.Videos = 10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1317) | 当前常量或枚举值：`10`。 | — |
| <a id="api-d40c9a584a0eea68"></a>[`public enum Inno.Native.Sdl3.SDLFolder : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1305) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFPoint</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFPoint e88b06700b23785132ed04fbe9b55c99c3ed127f463e9fb0e37a09b116f0a1e3 -->
<!-- api: F:Inno.Native.Sdl3.SDLFPoint.X 1d3d02069ec61e9283b2aa9ea05fecaca7d24ffb3c612297fd74b703b3baf987 -->
<!-- api: F:Inno.Native.Sdl3.SDLFPoint.Y 857386a0a35d60b105bcdbf03004ad5e9995bff219cc69b0d88dda7c42b0130c -->
<!-- api: M:Inno.Native.Sdl3.SDLFPoint.#ctor(System.Single,System.Single) 4c9c0554adb869e3665a4e2ba1a51579b1c21e55505b90cb357406607541ba36 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-621f79a51d130cca"></a>[`public float Inno.Native.Sdl3.SDLFPoint.X`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2448) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec75870de64c748f"></a>[`public float Inno.Native.Sdl3.SDLFPoint.Y`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2449) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6ab9eb7b5b64a351"></a>[`public Inno.Native.Sdl3.SDLFPoint.SDLFPoint(float x = 0, float y = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2450) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a246332e4cd9730e"></a>[`public struct Inno.Native.Sdl3.SDLFPoint`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2446) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFPointPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFPointPtr 402688096d479aee5300d6b3b26558ba93bdcaa873d522c8fc92108f3fd3cb87 -->
<!-- api: M:Inno.Native.Sdl3.SDLFPointPtr.#ctor(Inno.Native.Sdl3.SDLFPoint*) 4a28a43d952e68f6350b590c7988bcc89d08e36a9c6fba7855d7247a4a9207e3 -->
<!-- api: F:Inno.Native.Sdl3.SDLFPointPtr.Handle b769f2e1091c4ed203b8833ca19bbd8d9bc13d46140b1b3f531be4c6f61c7150 -->
<!-- api: P:Inno.Native.Sdl3.SDLFPointPtr.IsNull 35b9af3e4d6a4fbeb517e5cfa9fdd3f3a5a8ba812ed26a95761024ee61cc8354 -->
<!-- api: P:Inno.Native.Sdl3.SDLFPointPtr.Null 091aea9c004726a08bd9b2823a8a2d8140dd49f78fd58c80f2c2fdf1fc7cb654 -->
<!-- api: P:Inno.Native.Sdl3.SDLFPointPtr.Item(System.Int32) 2cfa4bae203ed835f15a6d419ab9e04a9658af4f53da796d49aae4344eece36e -->
<!-- api: M:Inno.Native.Sdl3.SDLFPointPtr.op_Implicit(Inno.Native.Sdl3.SDLFPoint*)~Inno.Native.Sdl3.SDLFPointPtr 76a35e7aa0bb1be951e8a1c97c8e7e5f9e2745b900bb3b3d20fab98933339c61 -->
<!-- api: M:Inno.Native.Sdl3.SDLFPointPtr.op_Implicit(Inno.Native.Sdl3.SDLFPointPtr)~Inno.Native.Sdl3.SDLFPoint* b7d4298c0ad2f01f7772ad5015edb4d195bb09432b268de70467b8734a3533ab -->
<!-- api: M:Inno.Native.Sdl3.SDLFPointPtr.op_Equality(Inno.Native.Sdl3.SDLFPointPtr,Inno.Native.Sdl3.SDLFPointPtr) 9d8e5bedf3cb9ded420cc8ebed66859043a2f107a6436278c06c41aef70286e0 -->
<!-- api: M:Inno.Native.Sdl3.SDLFPointPtr.op_Inequality(Inno.Native.Sdl3.SDLFPointPtr,Inno.Native.Sdl3.SDLFPointPtr) 4b5b55dbbb85325db16316b0e9f680c34ec7e091bdbb702287a64e6d06bb6bf9 -->
<!-- api: M:Inno.Native.Sdl3.SDLFPointPtr.Equals(Inno.Native.Sdl3.SDLFPointPtr) 8e2ee1ae15cb2378765ca843dc68ed79c038b271c8409ec1d2cd4edd4f727e8b -->
<!-- api: M:Inno.Native.Sdl3.SDLFPointPtr.Equals(System.Object) 30b7be672de7ded30b0997c63c946367884bc0aad68654dc887b729efee6cca8 -->
<!-- api: M:Inno.Native.Sdl3.SDLFPointPtr.GetHashCode be7febef419db18c22ccd85254c8954cee8d631ef1ee9e9c0ebd0a4bd3e812f5 -->
<!-- api: P:Inno.Native.Sdl3.SDLFPointPtr.X 91823ca38e268a39e8d40ec81f68bcd68e4b08edcb9299f31dddb2265e0ba22b -->
<!-- api: P:Inno.Native.Sdl3.SDLFPointPtr.Y 6c7f173ce3f877d0a8f1b9a7e1917c5250f9813ab30ced16063874c6c92b461a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-70830d69b73e715b"></a>[`public Inno.Native.Sdl3.SDLFPoint* Inno.Native.Sdl3.SDLFPointPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8354) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e534675eef5f1f6e"></a>[`public Inno.Native.Sdl3.SDLFPointPtr.SDLFPointPtr(Inno.Native.Sdl3.SDLFPoint* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8349) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-65b04a44f2cfa62c"></a>[`public bool Inno.Native.Sdl3.SDLFPointPtr.Equals(Inno.Native.Sdl3.SDLFPointPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8364) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-6f34f3f1ea156faa"></a>[`public override bool Inno.Native.Sdl3.SDLFPointPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8365) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-71d6516de0c0a3c9"></a>[`public override int Inno.Native.Sdl3.SDLFPointPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8366) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-d7c428cd2d341f9a"></a>[`public static bool Inno.Native.Sdl3.SDLFPointPtr.operator ==(Inno.Native.Sdl3.SDLFPointPtr left, Inno.Native.Sdl3.SDLFPointPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8362) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-951f4a2e51a9e146"></a>[`public static Inno.Native.Sdl3.SDLFPointPtr.implicit operator Inno.Native.Sdl3.SDLFPointPtr(Inno.Native.Sdl3.SDLFPoint* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8360) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9bad630883b00f19"></a>[`public static Inno.Native.Sdl3.SDLFPointPtr.implicit operator Inno.Native.Sdl3.SDLFPoint*(Inno.Native.Sdl3.SDLFPointPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8361) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-da394cf083b57b4d"></a>[`public static bool Inno.Native.Sdl3.SDLFPointPtr.operator !=(Inno.Native.Sdl3.SDLFPointPtr left, Inno.Native.Sdl3.SDLFPointPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8363) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-766faf04cb851568"></a>[`public bool Inno.Native.Sdl3.SDLFPointPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8355) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-deacf9080e356cfc"></a>[`public Inno.Native.Sdl3.SDLFPoint Inno.Native.Sdl3.SDLFPointPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8358) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b526b69c67133fde"></a>[`public static Inno.Native.Sdl3.SDLFPointPtr Inno.Native.Sdl3.SDLFPointPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8356) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c3125a2a9be9d7db"></a>[`public ref float Inno.Native.Sdl3.SDLFPointPtr.X { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8368) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-50668c9106eeb109"></a>[`public ref float Inno.Native.Sdl3.SDLFPointPtr.Y { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8369) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7188689f3c29fb10"></a>[`public struct Inno.Native.Sdl3.SDLFPointPtr : System.IEquatable<Inno.Native.Sdl3.SDLFPointPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8347) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFRect</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFRect d4e98cb233358c9f220013bbc33ba15004331edbb3ebd121df3f3a6c8a4bd14e -->
<!-- api: F:Inno.Native.Sdl3.SDLFRect.X d517913860b9651699308758844ca213f76714ac5e31c9e4d819a1ddc22fd80c -->
<!-- api: F:Inno.Native.Sdl3.SDLFRect.Y 9baf585f80041f6bc5270c6f3c2fb63013408825171f9b84aa509c9887af86f0 -->
<!-- api: F:Inno.Native.Sdl3.SDLFRect.W 69c7047b47018a63333e7e3febd70d6a3bf297bd4c2c3d49bda0e503a7ccd6df -->
<!-- api: F:Inno.Native.Sdl3.SDLFRect.H 6be4183472f6f0802a7aa0236685045581556512a02b1866cf1e0f04f73677de -->
<!-- api: M:Inno.Native.Sdl3.SDLFRect.#ctor(System.Single,System.Single,System.Single,System.Single) 519f1b32c36619df7d5e490390dcbcfa16ac98d2cdf3f2fcc103e21481745cad -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5000d22e3fb5b4ce"></a>[`public float Inno.Native.Sdl3.SDLFRect.H`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2485) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60a02b76f6ca302c"></a>[`public float Inno.Native.Sdl3.SDLFRect.W`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2484) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fcc22f88a20b0ed8"></a>[`public float Inno.Native.Sdl3.SDLFRect.X`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2482) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-adf11c4489ea2eba"></a>[`public float Inno.Native.Sdl3.SDLFRect.Y`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2483) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f9f3634323594af1"></a>[`public Inno.Native.Sdl3.SDLFRect.SDLFRect(float x = 0, float y = 0, float w = 0, float h = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2486) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-99c76ef6bb0e7248"></a>[`public struct Inno.Native.Sdl3.SDLFRect`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2480) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFRectPtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFRectPtr db78c139678a8e546274a0e9fcddc4d211002eaac37009b1e5c9f553de545702 -->
<!-- api: M:Inno.Native.Sdl3.SDLFRectPtr.#ctor(Inno.Native.Sdl3.SDLFRect*) 775fe72427754c0a8f045bfe5ba176c652ee78806ae6a7f61736f5bad61a04b5 -->
<!-- api: F:Inno.Native.Sdl3.SDLFRectPtr.Handle da58906167f0a9a8b578ef819c09bb1e409bcdbb14d75b77e4dd1483ebb1fc66 -->
<!-- api: P:Inno.Native.Sdl3.SDLFRectPtr.IsNull bbcbf5aed6e0c7d0588c2a8e3392e85360d960892aad040f0429e6fa5b344023 -->
<!-- api: P:Inno.Native.Sdl3.SDLFRectPtr.Null 746b488722fa97259bcb1c8106fe5e5b88f37c918aa49642ac8c3678d1e57579 -->
<!-- api: P:Inno.Native.Sdl3.SDLFRectPtr.Item(System.Int32) 63658502be8b55f779c85bf8ddd1577621df7afe97ab8248d1a217ebdeb59cf6 -->
<!-- api: M:Inno.Native.Sdl3.SDLFRectPtr.op_Implicit(Inno.Native.Sdl3.SDLFRect*)~Inno.Native.Sdl3.SDLFRectPtr 22d8e6343ff4d5ff00284e2207935074ca37aaa15a8b0f8696cef743dfc009b1 -->
<!-- api: M:Inno.Native.Sdl3.SDLFRectPtr.op_Implicit(Inno.Native.Sdl3.SDLFRectPtr)~Inno.Native.Sdl3.SDLFRect* fe243a5176027d4c1490707f7174a7c51ee4d1820e0ca0c8a257d7ba1c13a4dd -->
<!-- api: M:Inno.Native.Sdl3.SDLFRectPtr.op_Equality(Inno.Native.Sdl3.SDLFRectPtr,Inno.Native.Sdl3.SDLFRectPtr) b1e46068daeb4ef0deb0ff4fe270ca1697f2d66ec14c510ac693d324dffb32fa -->
<!-- api: M:Inno.Native.Sdl3.SDLFRectPtr.op_Inequality(Inno.Native.Sdl3.SDLFRectPtr,Inno.Native.Sdl3.SDLFRectPtr) bba5a0f524d4c4e01a21d2ad88fcb8065af9d01ce044b40e2fd681ec6586a84c -->
<!-- api: M:Inno.Native.Sdl3.SDLFRectPtr.Equals(Inno.Native.Sdl3.SDLFRectPtr) eeb1e725864897666f81be80944710e67f3f72f7263faef01aed087cb6904984 -->
<!-- api: M:Inno.Native.Sdl3.SDLFRectPtr.Equals(System.Object) ddb7377f9aaf32f3dd061ee0cab5bc4526609af1b0e942221949fa407f83c0c9 -->
<!-- api: M:Inno.Native.Sdl3.SDLFRectPtr.GetHashCode 952eb5a4dd7b86326431337c378f741f7e64c40634e14161a46f344b7759c4ca -->
<!-- api: P:Inno.Native.Sdl3.SDLFRectPtr.X 69246758a700a2ac81c276aa8c26e5be0c45bb2047593705265724d8c1cc7bf4 -->
<!-- api: P:Inno.Native.Sdl3.SDLFRectPtr.Y 5a25bc7952281b901ae59a3efaa5bb2609cc6796583a32b640b8f5509be15c18 -->
<!-- api: P:Inno.Native.Sdl3.SDLFRectPtr.W 6bdae901f18a40a9462ae3c24b4528908f5b1498f2fca04b2be8109b39580a21 -->
<!-- api: P:Inno.Native.Sdl3.SDLFRectPtr.H 110cec6d6e994b86eb1d77ccdbafe67996594e746ea0d286175acdb526c7e210 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6c8e7ce8d143bccb"></a>[`public Inno.Native.Sdl3.SDLFRect* Inno.Native.Sdl3.SDLFRectPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8380) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc0fc7ec78a02698"></a>[`public Inno.Native.Sdl3.SDLFRectPtr.SDLFRectPtr(Inno.Native.Sdl3.SDLFRect* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8375) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fe891ec5ad6fae77"></a>[`public bool Inno.Native.Sdl3.SDLFRectPtr.Equals(Inno.Native.Sdl3.SDLFRectPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8390) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-97f1fb562996e1b6"></a>[`public override bool Inno.Native.Sdl3.SDLFRectPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8391) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-c783708ae34f235a"></a>[`public override int Inno.Native.Sdl3.SDLFRectPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8392) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-2867b9a1ccea5354"></a>[`public static bool Inno.Native.Sdl3.SDLFRectPtr.operator ==(Inno.Native.Sdl3.SDLFRectPtr left, Inno.Native.Sdl3.SDLFRectPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8388) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-caa48698ca32a48c"></a>[`public static Inno.Native.Sdl3.SDLFRectPtr.implicit operator Inno.Native.Sdl3.SDLFRectPtr(Inno.Native.Sdl3.SDLFRect* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8386) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-09aa4b2d6dfe9dd4"></a>[`public static Inno.Native.Sdl3.SDLFRectPtr.implicit operator Inno.Native.Sdl3.SDLFRect*(Inno.Native.Sdl3.SDLFRectPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8387) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-27efe0aac09be489"></a>[`public static bool Inno.Native.Sdl3.SDLFRectPtr.operator !=(Inno.Native.Sdl3.SDLFRectPtr left, Inno.Native.Sdl3.SDLFRectPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8389) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-76719889d4b01df6"></a>[`public ref float Inno.Native.Sdl3.SDLFRectPtr.H { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8397) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-abfeedfb857ecbc7"></a>[`public bool Inno.Native.Sdl3.SDLFRectPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8381) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1c88e78962af94b3"></a>[`public Inno.Native.Sdl3.SDLFRect Inno.Native.Sdl3.SDLFRectPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8384) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5fc051820ebdac00"></a>[`public static Inno.Native.Sdl3.SDLFRectPtr Inno.Native.Sdl3.SDLFRectPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8382) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-54276e239c2f3477"></a>[`public ref float Inno.Native.Sdl3.SDLFRectPtr.W { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8396) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f882f91a59d97141"></a>[`public ref float Inno.Native.Sdl3.SDLFRectPtr.X { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8394) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-12a3cec67face85f"></a>[`public ref float Inno.Native.Sdl3.SDLFRectPtr.Y { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8395) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cbbd93428e4b240a"></a>[`public struct Inno.Native.Sdl3.SDLFRectPtr : System.IEquatable<Inno.Native.Sdl3.SDLFRectPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8373) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFreeFunc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFreeFunc 40bdbdd5805f60a8cad34cece56add7c07d51ab47a8dc2e6420eb308056bf5c0 -->
<!-- api: M:Inno.Native.Sdl3.SDLFreeFunc.#ctor(System.Object,System.IntPtr) 5efee16756b807d8fb7cbaac9716ef7d754b3d4c61155ac230a3bada81a261f2 -->
<!-- api: M:Inno.Native.Sdl3.SDLFreeFunc.Invoke(System.Void*) 3c5a91c9f617c05a2ffdbe2bad8a2daacd039bf67b84c112ec2b3799aa57ef7d -->
<!-- api: M:Inno.Native.Sdl3.SDLFreeFunc.BeginInvoke(System.Void*,System.AsyncCallback,System.Object) 6f85a0f3ce9e5d2c3834e630eb48d3edc6c54009aba5ffb36db10285a559a388 -->
<!-- api: M:Inno.Native.Sdl3.SDLFreeFunc.EndInvoke(System.IAsyncResult) 16a0dcec08e5789c3c0bc60b7ad8ed8c8edc29187c30d109d521e25675ee99cc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-082d7fc867196fac"></a>[`public Inno.Native.Sdl3.SDLFreeFunc.SDLFreeFunc(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6780) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4cfe78db70442f36"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLFreeFunc.BeginInvoke(void* mem, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6780) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2bbc156349b48d12"></a>[`public virtual void Inno.Native.Sdl3.SDLFreeFunc.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6780) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c4d9b269b2363eaa"></a>[`public virtual void Inno.Native.Sdl3.SDLFreeFunc.Invoke(void* mem)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6780) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-005733603ebbeb04"></a>[`public delegate void Inno.Native.Sdl3.SDLFreeFunc(void* mem)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6780) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLFunctionPointer</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLFunctionPointer 9ee4b4321208e623ed60616c28035a7cf55271a5c54976de909d6abbac14ec86 -->
<!-- api: M:Inno.Native.Sdl3.SDLFunctionPointer.#ctor(System.Object,System.IntPtr) d67e2c1782d687f84bac704c1c6f8994455392ebd15a87c75718cb4aa69839ee -->
<!-- api: M:Inno.Native.Sdl3.SDLFunctionPointer.Invoke 6e6ab72b0f068d34881639399b4d0032ad2f3e7158281df39e671d6dc5179c33 -->
<!-- api: M:Inno.Native.Sdl3.SDLFunctionPointer.BeginInvoke(System.AsyncCallback,System.Object) 6273d18d2814fc7856e148a67a5be87a546e09b51c57c2c699dd74027494e8e3 -->
<!-- api: M:Inno.Native.Sdl3.SDLFunctionPointer.EndInvoke(System.IAsyncResult) 17f43dd03ade8f590a02d3873683ab76f5d13d99663de03255ff44315251f2fa -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-862f537f09e16d27"></a>[`public Inno.Native.Sdl3.SDLFunctionPointer.SDLFunctionPointer(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6795) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-39cb8661f5ab0b49"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLFunctionPointer.BeginInvoke(System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6795) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-769c3f40aa2c1702"></a>[`public virtual void Inno.Native.Sdl3.SDLFunctionPointer.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6795) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4509aac0dde9a705"></a>[`public virtual void Inno.Native.Sdl3.SDLFunctionPointer.Invoke()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6795) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8dd8a54777a38a45"></a>[`public delegate void Inno.Native.Sdl3.SDLFunctionPointer()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6795) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepad</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepad a9a7dcd858dd68edf66a77f100e9769d88341d5d454cdc31284175af74edb282 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepad.#ctor(System.IntPtr) 5ce78b0e1facc44b0494af292742ce51ced36846613605019e560f01451fcd87 -->
<!-- api: P:Inno.Native.Sdl3.SDLGamepad.Handle 03c31fac9845d9638203a51533945f18ef6aada4c06de8ac1ff5ba2d74779a2b -->
<!-- api: P:Inno.Native.Sdl3.SDLGamepad.IsNull 274d5dc80695976e93184b2f2c741be2bbded1034750ce85ea4e6412f2463101 -->
<!-- api: P:Inno.Native.Sdl3.SDLGamepad.Null bb7ad4a494c47a79e3c3da74a60aecb6dffc5f54f6aadb89bb4a13602978c6d5 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepad.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLGamepad 7b3212cbe356c8cbabeac0ebcf5412e3713efccae18e60fd6026106520c23faa -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepad.op_Equality(Inno.Native.Sdl3.SDLGamepad,Inno.Native.Sdl3.SDLGamepad) b2608531a43d8a29427802929ce5a651723cf79b4f09045eec4254643014d878 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepad.op_Inequality(Inno.Native.Sdl3.SDLGamepad,Inno.Native.Sdl3.SDLGamepad) 50a56dd05d5caf5d919ef6571975b9bd5ee8834de4d49ea747a795430f0abf3b -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepad.Equals(Inno.Native.Sdl3.SDLGamepad) 5699b1fe9fb19a19a75c3961b3575a4f2bbf7f0a21832176dc5af1b9cc5328e1 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepad.Equals(System.Object) 711c2e1a0f0a5b795a94d1e11dcbcece39d864592e323e25d8c2dd0a532a18da -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepad.GetHashCode 80530f366fe58ac077594ed4bfdcd1936e0dd5e619fb6119f79d5c12f51812fc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b97c4b598c2e6f3d"></a>[`public Inno.Native.Sdl3.SDLGamepad.SDLGamepad(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2897) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3208439d33c5cb50"></a>[`public bool Inno.Native.Sdl3.SDLGamepad.Equals(Inno.Native.Sdl3.SDLGamepad other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2909) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-b37ae71dac55a468"></a>[`public override bool Inno.Native.Sdl3.SDLGamepad.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2910) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-6a09114d334029a0"></a>[`public override int Inno.Native.Sdl3.SDLGamepad.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2911) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-7f14d9ab9f065f9b"></a>[`public static bool Inno.Native.Sdl3.SDLGamepad.operator ==(Inno.Native.Sdl3.SDLGamepad left, Inno.Native.Sdl3.SDLGamepad right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2907) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-74f9edfcd517360f"></a>[`public static Inno.Native.Sdl3.SDLGamepad.implicit operator Inno.Native.Sdl3.SDLGamepad(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2906) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b84ec6df69c2e0a5"></a>[`public static bool Inno.Native.Sdl3.SDLGamepad.operator !=(Inno.Native.Sdl3.SDLGamepad left, Inno.Native.Sdl3.SDLGamepad right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2908) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ddbab371e2ad48f2"></a>[`public nint Inno.Native.Sdl3.SDLGamepad.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2902) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8bdeb24485f62e26"></a>[`public bool Inno.Native.Sdl3.SDLGamepad.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2903) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7bb0273b0286d286"></a>[`public static Inno.Native.Sdl3.SDLGamepad Inno.Native.Sdl3.SDLGamepad.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2904) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-af93354a20e4f9ae"></a>[`public readonly struct Inno.Native.Sdl3.SDLGamepad : System.IEquatable<Inno.Native.Sdl3.SDLGamepad>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2895) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadAxis</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadAxis 5b8085643220006511c1a33d30ecf5982d903af9d2ee9ba464082aacca925e2e -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxis.Invalid c8412e6f3c53e814d23b23312963e77c064237e69badf016c196982ed5240c70 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxis.Leftx 0e456e4e52dce1ff9c1db586e8cea61a98e38ecbd325be9fb2541c05859c2178 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxis.Lefty 0145772c851f68c07b168247678f1d34d9d20d9ee71b1acdba62b2d72597a22e -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxis.Rightx d8923414384d1d30f4cf411d45b5e1a4d6fb1cfda6d8721b07fbdf04d6cf9c9f -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxis.Righty 85592e4c4e0b16bace53c2a6d9f02736fbf8e671daaffbb9c569b9e9530264c2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxis.LeftTrigger 66b68055b3af8d4ed9adb553f5e509c810c355c490ffcc132d0a88d58b1ea0cc -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxis.RightTrigger ea6aa153e08a15fcc244cc0c0aac1645e74a65fe9f19e61c83692462ff9ec260 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxis.Count 7b9456c584440bee14c85eac4dd2668577b7efcb2b5c6fe6533758c6a881fd0c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9c086e834a40392f"></a>[`Inno.Native.Sdl3.SDLGamepadAxis.Count = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L790) | 当前常量或枚举值：`6`。 | — |
| <a id="api-56ad80166d9b9552"></a>[`Inno.Native.Sdl3.SDLGamepadAxis.Invalid = -1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L783) | 当前常量或枚举值：`-1`。 | — |
| <a id="api-82bb930719eaa06e"></a>[`Inno.Native.Sdl3.SDLGamepadAxis.LeftTrigger = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L788) | 当前常量或枚举值：`4`。 | — |
| <a id="api-02ba45673744226e"></a>[`Inno.Native.Sdl3.SDLGamepadAxis.Leftx = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L784) | 当前常量或枚举值：`0`。 | — |
| <a id="api-9ad5e7b848939743"></a>[`Inno.Native.Sdl3.SDLGamepadAxis.Lefty = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L785) | 当前常量或枚举值：`1`。 | — |
| <a id="api-01b82717057f7992"></a>[`Inno.Native.Sdl3.SDLGamepadAxis.RightTrigger = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L789) | 当前常量或枚举值：`5`。 | — |
| <a id="api-acffe2cae3cab7f2"></a>[`Inno.Native.Sdl3.SDLGamepadAxis.Rightx = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L786) | 当前常量或枚举值：`2`。 | — |
| <a id="api-01f4d156ae9370fe"></a>[`Inno.Native.Sdl3.SDLGamepadAxis.Righty = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L787) | 当前常量或枚举值：`3`。 | — |
| <a id="api-3410a9aea3b55103"></a>[`public enum Inno.Native.Sdl3.SDLGamepadAxis : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L781) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadAxisEvent</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadAxisEvent dc83139d2b090c75e4411baebbd6c940a1113eae37b3e65e5f47514c02253daf -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxisEvent.Type 43ecbd5e55e3d5e2345a5310aa41a1635715cd38eb7b481fd6aa06281ecaaf73 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxisEvent.Reserved b63dca2d43a83f5cf838b67e11b97f7b252fbe4bf3e1ed9fb3646f456d22a986 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxisEvent.Timestamp 127f595529c054ebd199ff17ef5fe6ef5e3c676dd0f67ed6789d6c3ed6e953d7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxisEvent.Which 251ebb2934a50e91b10224bcde5179f13067406578eb82df7a67de4b08663b54 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxisEvent.Axis 4ea00d5ac03bb5c429970e41d5c3c083573329c9adc1f6af059735ffe19f9d0e -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxisEvent.Padding1 48428802870d75456e1be0eadd98dbf8a0ab33a424ee67498d0dd76b082df671 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxisEvent.Padding2 7ff35da7a8c58615924d12f8591b35cda6ebc0b7ca465903c06ab936af202a1d -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxisEvent.Padding3 e7131d029dea4d49a70e5d139a0ce34524b3fc351bb6ee9d5b0b3705b14b9640 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxisEvent.Value aa8fda1e0fc4a00b2a965a5e5d44c6650bacc31f5bf8061fdb18fdfbe95c17c3 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadAxisEvent.Padding4 15b292ecf6bda0ca1f325e84be791ba321c103bb3001195bde1844d2edbd5fd0 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadAxisEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt32,System.Byte,System.Byte,System.Byte,System.Byte,System.Int16,System.UInt16) 8f63d7ff81ce0c4af281963b61c5907291150140d7f56049d896940802c19cc3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-671c4c89417dafbd"></a>[`public byte Inno.Native.Sdl3.SDLGamepadAxisEvent.Axis`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3547) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bec0a3c7468e5b75"></a>[`public byte Inno.Native.Sdl3.SDLGamepadAxisEvent.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3548) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ab8d361b6a05e54"></a>[`public byte Inno.Native.Sdl3.SDLGamepadAxisEvent.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3549) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf49aa1b5b906cc3"></a>[`public byte Inno.Native.Sdl3.SDLGamepadAxisEvent.Padding3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3550) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5d29b04881735db"></a>[`public ushort Inno.Native.Sdl3.SDLGamepadAxisEvent.Padding4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3552) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dcbb73108dfe6f82"></a>[`public uint Inno.Native.Sdl3.SDLGamepadAxisEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3544) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed5f404b2a8a8c71"></a>[`public ulong Inno.Native.Sdl3.SDLGamepadAxisEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3545) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ea8acdacd76a26f"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLGamepadAxisEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3543) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aebcd11b5ef52730"></a>[`public short Inno.Native.Sdl3.SDLGamepadAxisEvent.Value`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3551) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-632f12432b21d6cb"></a>[`public uint Inno.Native.Sdl3.SDLGamepadAxisEvent.Which`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3546) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-71ee3e1b1827dec5"></a>[`public Inno.Native.Sdl3.SDLGamepadAxisEvent.SDLGamepadAxisEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, uint which = 0, byte axis = 0, byte padding1 = 0, byte padding2 = 0, byte padding3 = 0, short value = 0, ushort padding4 = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3553) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-91c6426d39428dca"></a>[`public struct Inno.Native.Sdl3.SDLGamepadAxisEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3541) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadBinding</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadBinding 2a2c05fba54b2822306eb18b7e88714ad476d28bd9dc8fd718ffb1bd552a7038 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.InputType 4e0f7ca1372c25d3c0575f447daf52fa73dde76df73aac7f4edeb86b6bfbe5a0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.Input ee9bdc61c828a564150930f19c02ed360c4d230d4d63e5478910a1804f2e405b -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.OutputType babc7dcd6d71afb83bab030f2931c4a6036ae3de6352ecabd1f4dfbcdfa812bd -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.Output 2a8645c561642a1db19ddda7f51f1630fa908e9b6a9de2e016d0583d13193ce9 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadBinding.#ctor(Inno.Native.Sdl3.SDLGamepadBindingType,Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous,Inno.Native.Sdl3.SDLGamepadBindingType,Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous) c0766579790636f8732140d9a5b1720e4c7fbddff2fd79850b6b6debbea2990d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-355e92352314d833"></a>[`public Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous Inno.Native.Sdl3.SDLGamepadBinding.Input`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2997) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e881c998d697b645"></a>[`public Inno.Native.Sdl3.SDLGamepadBindingType Inno.Native.Sdl3.SDLGamepadBinding.InputType`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2996) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d21a35e3d5c09d3b"></a>[`public Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous Inno.Native.Sdl3.SDLGamepadBinding.Output`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2999) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f81d5cb15af78bc1"></a>[`public Inno.Native.Sdl3.SDLGamepadBindingType Inno.Native.Sdl3.SDLGamepadBinding.OutputType`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2998) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e2193270f5d50b37"></a>[`public Inno.Native.Sdl3.SDLGamepadBinding.SDLGamepadBinding(Inno.Native.Sdl3.SDLGamepadBindingType input_type = Inno.Native.Sdl3.SDLGamepadBindingType.BindtypeNone, Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous input = default(Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous), Inno.Native.Sdl3.SDLGamepadBindingType output_type = Inno.Native.Sdl3.SDLGamepadBindingType.BindtypeNone, Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous output = default(Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous))`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3000) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b816d36ef2eb5f0a"></a>[`public struct Inno.Native.Sdl3.SDLGamepadBinding`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2919) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous 195ef31b353fa8b349b81c5297e258611f03bec98d23aa097e4fb0267d44fc2f -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.Button 8db5937e16afe014ad07fb95febfa8b895c5f2d9f08d57e15e18ccb1169ab853 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.Axis 9f4812311bc0d49e6b65955c95f04cec6f2d59681bef2a22eae0f897f0d5f273 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.Hat 607c66f23d42e9aa8828e9c7fd71d4299d942cf14359a847afb5d9799f1b3035 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ddc7009fbc517b80"></a>[`public Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.AxisAnonymous Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.Axis`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2962) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ef097d3d9a65a74e"></a>[`public int Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.Button`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2960) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20fb04c96df632a9"></a>[`public Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.HatAnonymous Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.Hat`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2964) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e1bcf11980814ec"></a>[`public struct Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2925) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.AxisAnonymous</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.AxisAnonymous ac9f6f1fe9d0fea707aac8f3a866a85426f2cf0f6b126d5166134945f761311d -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.AxisAnonymous.Axis e404857085f2fc54c20c7f9322f675958d5d10f8d5f268e65c9ef9bcfa62b635 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.AxisAnonymous.AxisMin 8d698ac4d3fc51c2b9bf15953f488217ee91214cb2213dfd771da5bfb30ccc8b -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.AxisAnonymous.AxisMax 78b6740c831f4bdaced0ad362d3a158f8fa39b25f8eea657da2ec018ea4918b8 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.AxisAnonymous.#ctor(System.Int32,System.Int32,System.Int32) 53e9b5f97a34032ec14b07aaed6a0e8151c6ebff8c69631aba88ed07ddfa248c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b7d4d5f98eb1d22b"></a>[`public int Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.AxisAnonymous.Axis`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2933) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f0a2b19fb934bd32"></a>[`public int Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.AxisAnonymous.AxisMax`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2935) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-853e3ff214d1040a"></a>[`public int Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.AxisAnonymous.AxisMin`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2934) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c504a510bc3154d1"></a>[`public Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.AxisAnonymous.AxisAnonymous(int axis = 0, int axis_min = 0, int axis_max = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2936) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-40fcbbc4e1d8937b"></a>[`public struct Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.AxisAnonymous`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2931) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.HatAnonymous</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.HatAnonymous 552c668506f03c28618bb2a8058022512b74c09a40b875f0fa784b495eef0eb2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.HatAnonymous.Hat 29eafed40a35a8b12d2d73f6fb3b9aac10dac1f890edf824addbe159f8ee87ea -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.HatAnonymous.HatMask 17fa1294be8b24b43dfdb99199463350ac08234ef2de431d6f12d2793c0bc29c -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.HatAnonymous.#ctor(System.Int32,System.Int32) ef7e932110dce9b22f43ba3a643dcfc7a4ed05146e9157b336721fa699e9f3b7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4231c7253dd1ce35"></a>[`public int Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.HatAnonymous.Hat`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2950) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fff1c83c5119bc93"></a>[`public int Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.HatAnonymous.HatMask`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2951) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccc503fbbd5dce79"></a>[`public Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.HatAnonymous.HatAnonymous(int hat = 0, int hat_mask = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2952) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-044726568e4e8772"></a>[`public struct Inno.Native.Sdl3.SDLGamepadBinding.InputAnonymous.HatAnonymous`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2948) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous b77b2b6eb928eb45ae23952ab5d307aeccf65ba78d68f4a0a7c3dd426831f401 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.Button a80efef672821524ebea18afbcc0e15d0a48f7c985bd22232e244b074181d756 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.Axis a46ab13f8c20f475415ec682f9957867f4abe9f25fa7977ddfbafce14ad25a71 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-518add600c547b8d"></a>[`public Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.AxisAnonymous Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.Axis`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2993) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5be0253828a92cab"></a>[`public Inno.Native.Sdl3.SDLGamepadButton Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.Button`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2991) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-56723941d124265b"></a>[`public struct Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2971) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.AxisAnonymous</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.AxisAnonymous dc3f29a226871824259e31668f032437873cf2e9263cd70c818b00ae284be855 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.AxisAnonymous.Axis ace5bf18ceb1b9745c7909a218ba75ee8d5cd8e6027bff01dd1dd069558d9082 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.AxisAnonymous.AxisMin 92a985030b0cdc825c78820f1a1de024d22b79faa76678d239160a68b7bbe2cf -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.AxisAnonymous.AxisMax 6b9fd2a2a06fa6f919720a2509ed2af71c9519801f7a21e26813a47c52fcfe08 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.AxisAnonymous.#ctor(Inno.Native.Sdl3.SDLGamepadAxis,System.Int32,System.Int32) 2ba72dd5a3146b48cab1831efa4b830f766cdbc16d68aa1e7536a80d800f2cd9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6d339efd09c9a8ef"></a>[`public Inno.Native.Sdl3.SDLGamepadAxis Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.AxisAnonymous.Axis`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2979) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20257f2c5f18ace5"></a>[`public int Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.AxisAnonymous.AxisMax`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2981) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d8b7a51fa04cf7fc"></a>[`public int Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.AxisAnonymous.AxisMin`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2980) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1cb31e9710573058"></a>[`public Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.AxisAnonymous.AxisAnonymous(Inno.Native.Sdl3.SDLGamepadAxis axis = Inno.Native.Sdl3.SDLGamepadAxis.Leftx, int axis_min = 0, int axis_max = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2982) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d54fc88d8dbed991"></a>[`public struct Inno.Native.Sdl3.SDLGamepadBinding.OutputAnonymous.AxisAnonymous`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2977) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadBindingPtrPtr</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadBindingPtrPtr 9deaf7a52c6c1dd6053ee012e039bcd20236ac4f9f4da2c3c6bc10c18383effb -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.#ctor(Inno.Native.Sdl3.SDLGamepadBinding**) d824451d321326b6a3b79e526bea5cb49c0413aedc24fc01c9501fd801cc2d97 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.Handle 1a8289a056248c2d24cd7939ff7672c9c2c814a0d487479abd4975959e8447e7 -->
<!-- api: P:Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.IsNull 5679fac5eb3a07af2a45e21bbfeec3dc1c2e9c0813d245d1728e4c140317bf12 -->
<!-- api: P:Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.Null e370eb69b276739d6fb392f4b734c622e87805812c6de894dce894c6a1e2e4bd -->
<!-- api: P:Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.Item(System.Int32) 245f68f057622164f6cd990dff1674ce3196dac9d1c3e20c17fbf60a4550282a -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.op_Implicit(Inno.Native.Sdl3.SDLGamepadBinding**)~Inno.Native.Sdl3.SDLGamepadBindingPtrPtr 68135eea1545d4d0a169ff6ced9fb6573b158825b5e76c3b7eec09c9ab57708f -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.op_Implicit(Inno.Native.Sdl3.SDLGamepadBindingPtrPtr)~Inno.Native.Sdl3.SDLGamepadBinding** 475ffd54f5f17e39f99e556c8f50d4852fb39d7eaa6ff9daa4b18fe225ea39e2 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.op_Equality(Inno.Native.Sdl3.SDLGamepadBindingPtrPtr,Inno.Native.Sdl3.SDLGamepadBindingPtrPtr) f5f1a354874b7ad942862a0dde34acd37751710a89e33a92fad75027a2836b56 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.op_Inequality(Inno.Native.Sdl3.SDLGamepadBindingPtrPtr,Inno.Native.Sdl3.SDLGamepadBindingPtrPtr) c43ca2c794803a4728bc2fa1d6749d8d6687e6730e2b3c4d66c8c422970e3dc1 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.Equals(Inno.Native.Sdl3.SDLGamepadBindingPtrPtr) ab8f814ea0982f5d83fe200e439e9942b163d619951760997d53afb0c069805e -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.Equals(System.Object) 6178c686083624a642ce63969f1987d01ff400d51633cd6ea08926c92920f303 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.GetHashCode 96a7606cffb7cb6b10d76abab6a684a28f6227aa47552e5683ca14dd8edc3ebc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-404bddaa1686eb60"></a>[`public Inno.Native.Sdl3.SDLGamepadBinding** Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9195) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11350aa269732bf5"></a>[`public Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.SDLGamepadBindingPtrPtr(Inno.Native.Sdl3.SDLGamepadBinding** handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9190) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1a372176ad700c0b"></a>[`public bool Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.Equals(Inno.Native.Sdl3.SDLGamepadBindingPtrPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9205) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-cd03464fefcfdc0a"></a>[`public override bool Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9206) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-2f262e8eb0dd7924"></a>[`public override int Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9207) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-c260e04d5017de0f"></a>[`public static bool Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.operator ==(Inno.Native.Sdl3.SDLGamepadBindingPtrPtr left, Inno.Native.Sdl3.SDLGamepadBindingPtrPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9203) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2837bb73d37f6f17"></a>[`public static Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.implicit operator Inno.Native.Sdl3.SDLGamepadBindingPtrPtr(Inno.Native.Sdl3.SDLGamepadBinding** handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9201) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bbf86e31681873e2"></a>[`public static Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.implicit operator Inno.Native.Sdl3.SDLGamepadBinding**(Inno.Native.Sdl3.SDLGamepadBindingPtrPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9202) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c781a2434fc27066"></a>[`public static bool Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.operator !=(Inno.Native.Sdl3.SDLGamepadBindingPtrPtr left, Inno.Native.Sdl3.SDLGamepadBindingPtrPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9204) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-daa8642295436256"></a>[`public bool Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9196) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ad73b4c740b96519"></a>[`public Inno.Native.Sdl3.SDLGamepadBinding* Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9199) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ed777b6678e71306"></a>[`public static Inno.Native.Sdl3.SDLGamepadBindingPtrPtr Inno.Native.Sdl3.SDLGamepadBindingPtrPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9197) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-88af15e87df6fde3"></a>[`public struct Inno.Native.Sdl3.SDLGamepadBindingPtrPtr : System.IEquatable<Inno.Native.Sdl3.SDLGamepadBindingPtrPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9188) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadBindingType</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadBindingType bd44f1fb18713c5e2598f9957e7943fb10fcf14a405bab7ee131b3398162d1af -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBindingType.BindtypeNone 364e91558e4ed90af94bbbde333b524a6fac867bc9ba476e9c7f59396f01447a -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBindingType.BindtypeButton 4b2ae63c756618cd48b565c5ba44ae6dc155bab1ae95d9977d6e60e993993466 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBindingType.BindtypeAxis a7c02b56de8ada7b3efd462b16810db9796ec437b7b119a14f5518ec0f79136e -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadBindingType.BindtypeHat b920fa004bd0efde3c7fadb4c036cc9d5e35e662d357210a934c13590926fa9d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ff67f611f85b2ceb"></a>[`Inno.Native.Sdl3.SDLGamepadBindingType.BindtypeAxis = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L800) | 当前常量或枚举值：`2`。 | — |
| <a id="api-855b0a447633c568"></a>[`Inno.Native.Sdl3.SDLGamepadBindingType.BindtypeButton = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L799) | 当前常量或枚举值：`1`。 | — |
| <a id="api-6458e82c1a340816"></a>[`Inno.Native.Sdl3.SDLGamepadBindingType.BindtypeHat = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L801) | 当前常量或枚举值：`3`。 | — |
| <a id="api-0d5d2731c413a020"></a>[`Inno.Native.Sdl3.SDLGamepadBindingType.BindtypeNone = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L798) | 当前常量或枚举值：`0`。 | — |
| <a id="api-23dec6365cc02d5b"></a>[`public enum Inno.Native.Sdl3.SDLGamepadBindingType : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L796) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadButton</code> · 29 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadButton 5b3de208193ef976555d288c48adc5250b3100cc459acc1a1b40d703f5b634d5 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.Invalid de9b75349868c88c7ba8a7d5c42353d96f4af9e72e2d362aa71f7d67e2903249 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.South aa75fcc89ca2c25efd9f5694e5288e5049b555d7d9a4e63ad3c593249ae0c4b6 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.East c3d6b8be0a736c5ba48d12c5ebaa84f646d6903b5d30cd29d6d0534470c4a995 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.West 1e451a312f6984db392440bccde3e7e6d02dc99fbb0527be6ba651214c564ddd -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.North e88e8b64128951b26a362a5a199cfcdc1aaee4a5698be9b439c5495df13b8956 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.Back 003ff05c7f3c4c87fe976099d143808c23ab23d176068051365321b60b066d8c -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.Guide 8f9c70d1b5ed49a658c75f1ee0edab5c65d25e515ea407a945dc48b559b9fb02 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.Start f3a73b302daf755f071dc738e69615d2d4cf8be8ca5b371aab5fb4c8261d435c -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.LeftStick 8b0e90f3bef54f409c9b83426e85a218338e65ac3b1cedc3d34287ac1de77daa -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.RightStick 093db865588792d885330359bc37f9eb5ec0450a60dbc90ab1133357b8d535ff -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.LeftShoulder 74a0abf472694ab96c0f92c36b5dda45cf45764aa860530773d0ef0fc7cd9857 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.RightShoulder f3984d325606ba9b824bd08228ffce545d9f365e1f07e5a306ae69d7267b7595 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.DpadUp cff1c129306f9c73f7afb8e658867c0759008b8a7d69f699a1d97e603bd37071 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.DpadDown 3e32990569a95c216ca129b68098e5a880fa4ebfc37b4645b58816f5b2ea902f -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.DpadLeft c634f90c55751b0ebc54f78c9f6dab6cac0b7961c46cc265fde06e56ff50492e -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.DpadRight 86f70e1c3f60195f7b07b47631f1c13a3e7e106b8d68bd6f6a78719b02d058fd -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.Misc1 61368041408be07e8721678431aa7ac15acdf27c62076b01dec9894487532ce6 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.RightPaddle1 5077bddbaae4107853e53454d8cb70b5b294afce3b025d6964529de64cbd77cf -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.LeftPaddle1 a32acfb6aa96a26093d51de27957e8ba4ea853e9512feb0ae4dabe0a897c1b04 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.RightPaddle2 84dd43dc0d43b812e7a4b068c87fcb764ebcc376f6ff791c69d3f8bab6f52673 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.LeftPaddle2 f859b84de7c7f3f98a6a91bbb554fb890ed0e3f3bfbb54fc2c6656071c85328a -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.Touchpad 9dde032797e0f6e24c3406e7fb0153270c78ffdf9f1ee1c94c1a524ded25a19a -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.Misc2 70095b64abee77693122fb0230a5cf432ec2087676329e8e940dadc013e3d0bf -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.Misc3 cfb20675c039596d1d67f64d28a0ab8281245deaf21bf60529f04c76327e0091 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.Misc4 f13e06ef517a41f8d1ce7a33b555d2ad53d287dfda332608d317eccbbe16f8f2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.Misc5 3d82780b4b87afb8f96f76dcf7e2d2e4e8e2bcdce2b2679a33f5c8abbc0e9395 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.Misc6 258eb7338197217c956ca049474b200529c5b80603d011a9b8f5f3204784347c -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButton.Count aded961b7610865e830be97839be5d12670ed3380cdad7d43aea7b6db8c31dd8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8aef3e2620a9cefe"></a>[`Inno.Native.Sdl3.SDLGamepadButton.Back = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L737) | 当前常量或枚举值：`4`。 | — |
| <a id="api-21cbdf5d4367acea"></a>[`Inno.Native.Sdl3.SDLGamepadButton.Count = 26`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L759) | 当前常量或枚举值：`26`。 | — |
| <a id="api-4e99ace5268c5dce"></a>[`Inno.Native.Sdl3.SDLGamepadButton.DpadDown = 12`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L745) | 当前常量或枚举值：`12`。 | — |
| <a id="api-4d4cb2350bef0d4e"></a>[`Inno.Native.Sdl3.SDLGamepadButton.DpadLeft = 13`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L746) | 当前常量或枚举值：`13`。 | — |
| <a id="api-47b1613eff364ec4"></a>[`Inno.Native.Sdl3.SDLGamepadButton.DpadRight = 14`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L747) | 当前常量或枚举值：`14`。 | — |
| <a id="api-dfafc84761bf3ff5"></a>[`Inno.Native.Sdl3.SDLGamepadButton.DpadUp = 11`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L744) | 当前常量或枚举值：`11`。 | — |
| <a id="api-596ca779d87116c8"></a>[`Inno.Native.Sdl3.SDLGamepadButton.East = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L734) | 当前常量或枚举值：`1`。 | — |
| <a id="api-6c48af5af8cc6acb"></a>[`Inno.Native.Sdl3.SDLGamepadButton.Guide = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L738) | 当前常量或枚举值：`5`。 | — |
| <a id="api-5d1e42924a383a6f"></a>[`Inno.Native.Sdl3.SDLGamepadButton.Invalid = -1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L732) | 当前常量或枚举值：`-1`。 | — |
| <a id="api-185edac774894688"></a>[`Inno.Native.Sdl3.SDLGamepadButton.LeftPaddle1 = 17`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L750) | 当前常量或枚举值：`17`。 | — |
| <a id="api-e16dbc33384ce705"></a>[`Inno.Native.Sdl3.SDLGamepadButton.LeftPaddle2 = 19`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L752) | 当前常量或枚举值：`19`。 | — |
| <a id="api-fd3fdacb0b74a0ff"></a>[`Inno.Native.Sdl3.SDLGamepadButton.LeftShoulder = 9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L742) | 当前常量或枚举值：`9`。 | — |
| <a id="api-3d941d1708602287"></a>[`Inno.Native.Sdl3.SDLGamepadButton.LeftStick = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L740) | 当前常量或枚举值：`7`。 | — |
| <a id="api-7d426859771798c8"></a>[`Inno.Native.Sdl3.SDLGamepadButton.Misc1 = 15`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L748) | 当前常量或枚举值：`15`。 | — |
| <a id="api-5625b7ae1810a46e"></a>[`Inno.Native.Sdl3.SDLGamepadButton.Misc2 = 21`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L754) | 当前常量或枚举值：`21`。 | — |
| <a id="api-2383c90a92789866"></a>[`Inno.Native.Sdl3.SDLGamepadButton.Misc3 = 22`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L755) | 当前常量或枚举值：`22`。 | — |
| <a id="api-ae2bebaa0f89eb5f"></a>[`Inno.Native.Sdl3.SDLGamepadButton.Misc4 = 23`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L756) | 当前常量或枚举值：`23`。 | — |
| <a id="api-384f1b00bdbabd1f"></a>[`Inno.Native.Sdl3.SDLGamepadButton.Misc5 = 24`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L757) | 当前常量或枚举值：`24`。 | — |
| <a id="api-6c6bc79b40200995"></a>[`Inno.Native.Sdl3.SDLGamepadButton.Misc6 = 25`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L758) | 当前常量或枚举值：`25`。 | — |
| <a id="api-9c862944773b752d"></a>[`Inno.Native.Sdl3.SDLGamepadButton.North = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L736) | 当前常量或枚举值：`3`。 | — |
| <a id="api-28cc6e1a2ed210c4"></a>[`Inno.Native.Sdl3.SDLGamepadButton.RightPaddle1 = 16`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L749) | 当前常量或枚举值：`16`。 | — |
| <a id="api-a5ddaacffa686706"></a>[`Inno.Native.Sdl3.SDLGamepadButton.RightPaddle2 = 18`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L751) | 当前常量或枚举值：`18`。 | — |
| <a id="api-5df05f9e8fa36784"></a>[`Inno.Native.Sdl3.SDLGamepadButton.RightShoulder = 10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L743) | 当前常量或枚举值：`10`。 | — |
| <a id="api-27a01da9ea57e142"></a>[`Inno.Native.Sdl3.SDLGamepadButton.RightStick = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L741) | 当前常量或枚举值：`8`。 | — |
| <a id="api-dde17ae9a514e2d1"></a>[`Inno.Native.Sdl3.SDLGamepadButton.South = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L733) | 当前常量或枚举值：`0`。 | — |
| <a id="api-5eb72f50df69160e"></a>[`Inno.Native.Sdl3.SDLGamepadButton.Start = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L739) | 当前常量或枚举值：`6`。 | — |
| <a id="api-9a1ade2eef656cc3"></a>[`Inno.Native.Sdl3.SDLGamepadButton.Touchpad = 20`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L753) | 当前常量或枚举值：`20`。 | — |
| <a id="api-5f249c93d45402fc"></a>[`Inno.Native.Sdl3.SDLGamepadButton.West = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L735) | 当前常量或枚举值：`2`。 | — |
| <a id="api-885ed9b7f805a63c"></a>[`public enum Inno.Native.Sdl3.SDLGamepadButton : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L730) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadButtonEvent</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadButtonEvent 53117d1a338f11c0c5b2c015fcfc6b1305785d82bbb43f2069921d5b77e6b8f0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonEvent.Type 46c09a609d8969471eb8c68bd699fc2c2c6bf863814830c8f405088e584ea795 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonEvent.Reserved 80ea0efeff4e71cff009cce165db2a46215662df66441a9d034e440ab4a11d55 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonEvent.Timestamp 79cbcef91ce4604365dd17eb426dbd6cdc9c3e34444552940db890373a6ad5c9 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonEvent.Which ca69d41e7208c7686d63818b967a574c1695fbaf1b7104279f683e6765aca14c -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonEvent.Button 89647ec20b4f9d15e22563fe5cb87a29710b4d55105054b1404163965179798a -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonEvent.Down 135d93c54436c0daedefdbc0554d6a2f43182976baca8051f973c27850b17e20 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonEvent.Padding1 fe97cbce5f21a8f8f7408da72b2ecad651f05b7dfe8ebbb1550452a5246aef37 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonEvent.Padding2 6e6251d732907ce8f425b6c6392d46c50248ffd16863f19e7ddc0fd0e8d91496 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadButtonEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt32,System.Byte,System.Byte,System.Byte,System.Byte) 5e707a6280e16b5cc1a080c7f1de07c3b6de7a958fc1945d303df67c57cd3687 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-59bfe08d6ba9fa46"></a>[`public byte Inno.Native.Sdl3.SDLGamepadButtonEvent.Button`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3578) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb8cb5768040f631"></a>[`public byte Inno.Native.Sdl3.SDLGamepadButtonEvent.Down`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3579) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a5b70e9d83f9f0b5"></a>[`public byte Inno.Native.Sdl3.SDLGamepadButtonEvent.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3580) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ad462ed20736451"></a>[`public byte Inno.Native.Sdl3.SDLGamepadButtonEvent.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3581) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5eb614561a463ad9"></a>[`public uint Inno.Native.Sdl3.SDLGamepadButtonEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3575) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f9fef6dacff03f18"></a>[`public ulong Inno.Native.Sdl3.SDLGamepadButtonEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3576) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e94a8854a9fb89ce"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLGamepadButtonEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3574) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27f3a8daabf66519"></a>[`public uint Inno.Native.Sdl3.SDLGamepadButtonEvent.Which`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3577) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-700c66f8fe126f14"></a>[`public Inno.Native.Sdl3.SDLGamepadButtonEvent.SDLGamepadButtonEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, uint which = 0, byte button = 0, byte down = 0, byte padding1 = 0, byte padding2 = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3582) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-26d835eee3f4b775"></a>[`public struct Inno.Native.Sdl3.SDLGamepadButtonEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3572) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadButtonLabel</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadButtonLabel 53d9bd163f7c3d3927b3b380d3d1a875b76a928ac47877364ff01cdc83bbd428 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonLabel.Unknown 8aafdc753a6d3a430ecd67e62336fe362dfd482e8fdc800c81d21914190586e3 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonLabel.A fbcf08f67973a8ba072d217b330b381b26eae254d908c27a25e48559fc77738a -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonLabel.B 0fad905869db973f8ec44019e0e53611ec8fabab64a673564f08c3b27e1d1586 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonLabel.X 60779cc7876270e1304fcdb5c73e71e95ccb267c3c1263617994b6df5b24f732 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonLabel.Y ad3239596fa5f23601f4ef711af3201195b254776730168c73d91943560c00b2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonLabel.Cross 316bad72718b97c32fffdb96c5b90f6fd054ba254e9e8d8e996dfa58e6179226 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonLabel.Circle 249785d07bf5c5ebcd2e90a4bd99cbca575443c95eb583ee909a1173b571e329 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonLabel.Square 86e6e0b606d18480f74084d2cecad03519681df67c854990c14077dc8756dc3c -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadButtonLabel.Triangle aa6bd0f9d038504040bf01a53b6ef5af86037c62384c38848e77a21700d22af9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-684b74575233d97c"></a>[`Inno.Native.Sdl3.SDLGamepadButtonLabel.A = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L768) | 当前常量或枚举值：`1`。 | — |
| <a id="api-25171ecd5ba9420e"></a>[`Inno.Native.Sdl3.SDLGamepadButtonLabel.B = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L769) | 当前常量或枚举值：`2`。 | — |
| <a id="api-250dd9401923ca7c"></a>[`Inno.Native.Sdl3.SDLGamepadButtonLabel.Circle = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L773) | 当前常量或枚举值：`6`。 | — |
| <a id="api-a6978192e6ee2121"></a>[`Inno.Native.Sdl3.SDLGamepadButtonLabel.Cross = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L772) | 当前常量或枚举值：`5`。 | — |
| <a id="api-96a7c8d37417c22e"></a>[`Inno.Native.Sdl3.SDLGamepadButtonLabel.Square = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L774) | 当前常量或枚举值：`7`。 | — |
| <a id="api-8e1abfd022b8bd69"></a>[`Inno.Native.Sdl3.SDLGamepadButtonLabel.Triangle = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L775) | 当前常量或枚举值：`8`。 | — |
| <a id="api-65a7106b5ce9ee5b"></a>[`Inno.Native.Sdl3.SDLGamepadButtonLabel.Unknown = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L767) | 当前常量或枚举值：`0`。 | — |
| <a id="api-0f2fcd8f002fa6ec"></a>[`Inno.Native.Sdl3.SDLGamepadButtonLabel.X = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L770) | 当前常量或枚举值：`3`。 | — |
| <a id="api-d335030a41ca4430"></a>[`Inno.Native.Sdl3.SDLGamepadButtonLabel.Y = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L771) | 当前常量或枚举值：`4`。 | — |
| <a id="api-cf342f96cc089cbf"></a>[`public enum Inno.Native.Sdl3.SDLGamepadButtonLabel : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L765) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadDeviceEvent</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadDeviceEvent 1df81ec2e0d0b06718429e759b7d8a4a37e7fc96f55966940f2c41c7edf3b28a -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadDeviceEvent.Type 8dc5e7f9d4e66906f586de7dbf0324e1db5d823684c3c2daae2f143dd2f20c5c -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadDeviceEvent.Reserved 8b1f5afb70a932f36fb6e4df1d1920a55b7f70030e63de481fda030ba01b9c20 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadDeviceEvent.Timestamp 365d95c5ce0b030c0c89a207a21b8cbc685136bd9553fc560315cd1fbb80eaf7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadDeviceEvent.Which 3e88f98a6a034bde22b8d6a28efc055f4b65427e8b11d15fdfc04638b9d46b86 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadDeviceEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt32) 1898c430ce4de158c37e7ac79ed4be5fda612ad0761f01134c8342a2121fdbc2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-69fd499e690d0383"></a>[`public uint Inno.Native.Sdl3.SDLGamepadDeviceEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3602) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21c9cb200986352d"></a>[`public ulong Inno.Native.Sdl3.SDLGamepadDeviceEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3603) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54ba7d6b969d8619"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLGamepadDeviceEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3601) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e80d5b1a5ffb5109"></a>[`public uint Inno.Native.Sdl3.SDLGamepadDeviceEvent.Which`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3604) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9838a3f24fd128e9"></a>[`public Inno.Native.Sdl3.SDLGamepadDeviceEvent.SDLGamepadDeviceEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, uint which = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3605) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-121edb4fe9c8617a"></a>[`public struct Inno.Native.Sdl3.SDLGamepadDeviceEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3599) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadSensorEvent</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadSensorEvent 1fd2d0965b24126efe839a55c01da19bbdd3c307d6a40844da0657c0e6f8fbe0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadSensorEvent.Type 58cdb6cd4cd95bbdea464e67f839424adb42e87fd937311097690add64127564 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadSensorEvent.Reserved dbb79cb11b932f9c1ceb9ecf42359f1ade029a19454a1fd740b0e828b14ecf53 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadSensorEvent.Timestamp 9aadbcf3bd0ba73a2c5fd302d578a4a7d06c5f04438af89da0e75f0437fe94a3 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadSensorEvent.Which 895b968b16fb1502f5602269e37e638a7393f91c2e21de8a9877c2a6de601602 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadSensorEvent.Sensor 9c67d0540db7692f0789cba7674706015aeed54e77d10ce45b886abc878557f8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadSensorEvent.Data_0 1b17bdcd8dde2d13d8438f9f61334a200545560d1e37cd40004a576cc1f8c1ca -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadSensorEvent.Data_1 31462994fc9794240d5781ac26076da4c27d5f30cbb0e499006a5c3a45ba2be6 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadSensorEvent.Data_2 be449a5ffc93dd2fbd52a9b3e3ae9d45114f75d8d6de460c811912230c78d377 -->
<!-- api: P:Inno.Native.Sdl3.SDLGamepadSensorEvent.Data c41cad3215230ec241bf342d6498cf849b6ed9f5e22dd95228b45073cd148b8d -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadSensorEvent.SensorTimestamp bd502360cb07cc60aa51155fb436e1c7a73331d5ecf5cd1a84d9ff6f7925ff89 -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadSensorEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt32,System.Int32,System.Single*,System.UInt64) 619c1f3518076853cdf34f65b9446f076f29bd6078485b43004685c6e7714705 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-78d42b5ba0313be8"></a>[`public float Inno.Native.Sdl3.SDLGamepadSensorEvent.Data_0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3654) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-56d6fbb8cd761c23"></a>[`public float Inno.Native.Sdl3.SDLGamepadSensorEvent.Data_1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3655) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-541effbbe2fe34d0"></a>[`public float Inno.Native.Sdl3.SDLGamepadSensorEvent.Data_2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3656) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11a9ce319d3b0f5a"></a>[`public uint Inno.Native.Sdl3.SDLGamepadSensorEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3650) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-adcd0df04d555c11"></a>[`public int Inno.Native.Sdl3.SDLGamepadSensorEvent.Sensor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3653) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5a123ec7869075e"></a>[`public ulong Inno.Native.Sdl3.SDLGamepadSensorEvent.SensorTimestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3659) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8621d1865bea6363"></a>[`public ulong Inno.Native.Sdl3.SDLGamepadSensorEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3651) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb4e14fb07d2e25c"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLGamepadSensorEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3649) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df6e4fcd33dd66e7"></a>[`public uint Inno.Native.Sdl3.SDLGamepadSensorEvent.Which`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3652) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a53d7667cd975708"></a>[`public Inno.Native.Sdl3.SDLGamepadSensorEvent.SDLGamepadSensorEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, uint which = 0, int sensor = 0, float* data = null, ulong sensor_timestamp = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3660) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8aca05eff71c7e6f"></a>[`public System.Span<float> Inno.Native.Sdl3.SDLGamepadSensorEvent.Data { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3657) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-09a0cb5b566e2d82"></a>[`public struct Inno.Native.Sdl3.SDLGamepadSensorEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3647) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadTouchpadEvent</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadTouchpadEvent b875c3476d28eeb2c146d383913d81445046cec5b3a6a26d0451fbffae57c8aa -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Type 54c4017cad2c2c74af960ccda78d96dfa0993d0d1367d1823cf68d7f7ebc9dea -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Reserved cbd922c48740ed0306f2b4099d21909c60175a9afe9df09a158c5aeac27f5a56 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Timestamp 023637d89db34113c015052d80818b8bc1405730c4f539701a54439c1ef4e304 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Which 1e48dad1c1a8ead51b92dcd10b5a1a49e8b25d90900427183d1aa4e7b119d822 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Touchpad 01ddab5b5fb9f02fe95025b735ac13247cbefebee42923df4e5da970d4b800d9 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Finger decb3c1d19950c7624ba9a29da88c0668b65e06513158245e5a962cca4cd8641 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadTouchpadEvent.X ce9cc652a68542d622fc18a5eb8029da5d0b9235a6389c637eb250e88fef756f -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Y 8963c530667342ecc6b4815d79ce49fd3cbf462b620c446cc60c8b5f936598f1 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Pressure 85d747141d47a97b0621e5420b479adad88a1070f3f833278540f721f5cf637f -->
<!-- api: M:Inno.Native.Sdl3.SDLGamepadTouchpadEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt32,System.Int32,System.Int32,System.Single,System.Single,System.Single) 2c0f327d3b5cd5de4588d67abc522a7ee80e05fd76b9a4075df7c3734c95ebf9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8f67e2e0a7e587ff"></a>[`public int Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Finger`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3625) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c6db000ce4da7577"></a>[`public float Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Pressure`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3628) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-efe87ac5f5b93e64"></a>[`public uint Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3621) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-87d930d57899ed78"></a>[`public ulong Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3622) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1022dbcec8136f01"></a>[`public int Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Touchpad`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3624) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-447e732352f10f77"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3620) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf7dbcde8dec34c2"></a>[`public uint Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Which`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3623) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2f0984d8f0c8f72"></a>[`public float Inno.Native.Sdl3.SDLGamepadTouchpadEvent.X`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3626) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9264d15cdff46ace"></a>[`public float Inno.Native.Sdl3.SDLGamepadTouchpadEvent.Y`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3627) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0718050723f6532f"></a>[`public Inno.Native.Sdl3.SDLGamepadTouchpadEvent.SDLGamepadTouchpadEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, uint which = 0, int touchpad = 0, int finger = 0, float x = 0, float y = 0, float pressure = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3629) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-61b356a687c30e11"></a>[`public struct Inno.Native.Sdl3.SDLGamepadTouchpadEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3618) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGamepadType</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGamepadType b4ba0ec697b1f6d8abd848da07bbee3e28593fb44e516bd09679e4f4dac309c5 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadType.Unknown dc8891fbd7dcde22746f465cf63c83bdf5f96ac7f1b422d33f15f5505e94998a -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadType.Standard 02728383e8bdea8a7991fa4e0e5012e4570c2da28bb1c8f874bffcb2c85711c4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadType.Xbox360 86858821de092631f94ddf5224f27610d7c675045414f44ee3db7c860fd97e30 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadType.Xboxone 9e2d0c5afb65c97036936aa2efaae8c123847a43ad35bc3ebd13c9c5dd3aa64b -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadType.Ps3 985bb50bb5b822d9bc9845022dec5cc2477c571cdf7008c13ab833e9375c0908 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadType.Ps4 4ab1160a97df8048ad9c834b7f1ddc695d30f2119eae65a865526476e80f9c6c -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadType.Ps5 21435b4a4b05e7629f1c127b51f003148f287d2c9d3c3c00f84daf58211db22b -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadType.NintendoSwitchPro 067f42247286d0342de680889556bbc35e44dcb732872f42b501bf27cbffb394 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadType.NintendoSwitchJoyconLeft 40027c15afd8aca86f3313287cb180c48dc3cd45b8817f9be349e4c4b28552e7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadType.NintendoSwitchJoyconRight eb4c883ce733bd941602e3de61301f61e8a9b3d6225a1a5802a87908940c85a7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadType.NintendoSwitchJoyconPair 47cdf12ca1a3f0f6a7a28f5a2b3a057cca6ea4aa9119c27f8e159c2afd2fb3aa -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadType.Gamecube 230b4d30e32bba364f77440acae425b626e24d33edbe7c946ec7ebd0bfb6d40e -->
<!-- api: F:Inno.Native.Sdl3.SDLGamepadType.Count b5abd3db4d4b7603c3a0ad398e5f1d1754bf0d65292812ab7cd42357af510cdc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f149062fbf7aab2e"></a>[`Inno.Native.Sdl3.SDLGamepadType.Count = 12`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L724) | 当前常量或枚举值：`12`。 | — |
| <a id="api-11ffb37d16207e51"></a>[`Inno.Native.Sdl3.SDLGamepadType.Gamecube = 11`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L723) | 当前常量或枚举值：`11`。 | — |
| <a id="api-2e0bdb8af0108388"></a>[`Inno.Native.Sdl3.SDLGamepadType.NintendoSwitchJoyconLeft = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L720) | 当前常量或枚举值：`8`。 | — |
| <a id="api-02c1c45b2c78daf9"></a>[`Inno.Native.Sdl3.SDLGamepadType.NintendoSwitchJoyconPair = 10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L722) | 当前常量或枚举值：`10`。 | — |
| <a id="api-6bfdfe45522a6ff8"></a>[`Inno.Native.Sdl3.SDLGamepadType.NintendoSwitchJoyconRight = 9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L721) | 当前常量或枚举值：`9`。 | — |
| <a id="api-de7ff5fd0613e7b5"></a>[`Inno.Native.Sdl3.SDLGamepadType.NintendoSwitchPro = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L719) | 当前常量或枚举值：`7`。 | — |
| <a id="api-f0c8121c565dbe5a"></a>[`Inno.Native.Sdl3.SDLGamepadType.Ps3 = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L716) | 当前常量或枚举值：`4`。 | — |
| <a id="api-81747f101e4510a1"></a>[`Inno.Native.Sdl3.SDLGamepadType.Ps4 = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L717) | 当前常量或枚举值：`5`。 | — |
| <a id="api-2dcd2dcfece0ee31"></a>[`Inno.Native.Sdl3.SDLGamepadType.Ps5 = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L718) | 当前常量或枚举值：`6`。 | — |
| <a id="api-08347cf8302fb7b5"></a>[`Inno.Native.Sdl3.SDLGamepadType.Standard = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L713) | 当前常量或枚举值：`1`。 | — |
| <a id="api-a7c1ea8b528cce4c"></a>[`Inno.Native.Sdl3.SDLGamepadType.Unknown = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L712) | 当前常量或枚举值：`0`。 | — |
| <a id="api-8eea8c7a641aac86"></a>[`Inno.Native.Sdl3.SDLGamepadType.Xbox360 = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L714) | 当前常量或枚举值：`2`。 | — |
| <a id="api-50cc2b2b31a377c2"></a>[`Inno.Native.Sdl3.SDLGamepadType.Xboxone = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L715) | 当前常量或枚举值：`3`。 | — |
| <a id="api-ce6cfcfa6dc70b61"></a>[`public enum Inno.Native.Sdl3.SDLGamepadType : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L710) | Native 取值集合；当前取值见本类型成员。 | — |

</details>
