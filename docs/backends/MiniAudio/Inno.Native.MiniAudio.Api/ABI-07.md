# Inno.Native.MiniAudio ABI · 07

[项目与生命周期](../Inno.Native.MiniAudio.md) · [ABI 索引](README.md)

<!-- api: F:Inno.Native.MiniAudio.MaFormat.ma_format_u8 b8b8d030a145362ff6857bc6d6d409c268ed51b8c0427952b79f484f28089b8d -->
<!-- api: F:Inno.Native.MiniAudio.MaFormat.ma_format_unknown a5744d048eefb322ef34142651192fe8ee3e7da4f6fbe9d0b7175a5ee7888d17 -->
<!-- api: F:Inno.Native.MiniAudio.MaGainer.Config 846c98cc65a76d046bd4ad801c84bb27fd16555a6cb85464c115705a27802698 -->
<!-- api: F:Inno.Native.MiniAudio.MaGainer.MasterVolume 2a8cd3a654804bb608ccb548e64b880b92fd53773c45425e2c8d453c11e14346 -->
<!-- api: F:Inno.Native.MiniAudio.MaGainer.OwnsHeap f2f60be742165b17985bd8413c58429e4157b765cf5bfdbf581a1c30230017ff -->
<!-- api: F:Inno.Native.MiniAudio.MaGainer.PHeap 1c9f0e96de9f3fdffd161ff44cc97b4d518a1df2f88513ae0cb92a66345084aa -->
<!-- api: F:Inno.Native.MiniAudio.MaGainer.PNewGains a227c59c70978f4930ea810b5491193dfd78c2bae73073f1efed2e0a4787a5d0 -->
<!-- api: F:Inno.Native.MiniAudio.MaGainer.POldGains 674f9d0c0c7aef9ad78b58a614530b6bbb05716428c37622c79e51ba77b5fc27 -->
<!-- api: F:Inno.Native.MiniAudio.MaGainer.T 25253ad555ff97be137612c21399e15bc6a73250ca4e4b9bacc980da48e2151f -->
<!-- api: F:Inno.Native.MiniAudio.MaGainerConfig.Channels 5a28e33f9b0a4ecdac05aff932aaf3811d25736fa4a118bc86d6741f40149bda -->
<!-- api: F:Inno.Native.MiniAudio.MaGainerConfig.SmoothTimeInFrames 4a8c02a94ed099139ba316f77aef918b06bf49e1c1344f14ba884e2be94be2e6 -->
<!-- api: F:Inno.Native.MiniAudio.MaHandedness.ma_handedness_left 50bfb16bc171a78389a7a1a0c5a461c332c3b6eda3d2586afca47bba0b1752cc -->
<!-- api: F:Inno.Native.MiniAudio.MaHandedness.ma_handedness_right 9d0f062d3e1de2107604729a01213e072fe770f9e4ec73d3e7c1797ff7916643 -->
<!-- api: F:Inno.Native.MiniAudio.MaHishelf2.Bq 9fb5e5ce75184f3fb6bcb566c701c3dd262071c002a2bb90650a6b7b3480bafd -->
<!-- api: F:Inno.Native.MiniAudio.MaHishelf2Config.Channels 16eb3e4e79a19089533606d0062a54fbcd87586eb6805c912771564ba16740a2 -->
<!-- api: F:Inno.Native.MiniAudio.MaHishelf2Config.Format 9fe9d0b97245fc3bfdceaaaa91b3cf3a722dc98546f47a95fe1a85c8bd50301c -->
<!-- api: F:Inno.Native.MiniAudio.MaHishelf2Config.Frequency 25e854b5ab6224eda294205ef5658d2fa5af1674d069e31c8648a423a0f3a7db -->
<!-- api: F:Inno.Native.MiniAudio.MaHishelf2Config.GainDb 6b27e31c8f757de9c153a83a7edf9b46a252fd39e2c3fc8770ebccfb8a141c91 -->
<!-- api: F:Inno.Native.MiniAudio.MaHishelf2Config.SampleRate 684268edfa634fdadeb6c8a7b937164592216a7fcf0cfb6def17e986ce183ec2 -->
<!-- api: F:Inno.Native.MiniAudio.MaHishelf2Config.ShelfSlope e33eb08a1cd1c6889030a980b51fd246f12a6e7b8fe685a8d191ecbad968b618 -->
<!-- api: F:Inno.Native.MiniAudio.MaHishelfNode.BaseNode 6ac2c2815ce85b28f16451be48aa123d6eb9f7da5bb2a04e402a92357cd21395 -->
<!-- api: F:Inno.Native.MiniAudio.MaHishelfNode.Hishelf 9ebe66b8abde20db3c6c40a5b869e5658e37d008312e85943afcece0b0661fd7 -->
<!-- api: F:Inno.Native.MiniAudio.MaHishelfNodeConfig.Hishelf ec37c608d15c3af7c0efcb04ecd0ceda95fd53295d7f16f82a12b92b2af177ad -->
<!-- api: F:Inno.Native.MiniAudio.MaHishelfNodeConfig.NodeConfig acd1f33f1c8b8be9add06831df075144ffb9fa7209dc8a5fe0bf1fc0f4a72517 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf.Channels f3251e2d4981409a08808873af7692a8956519bace65e745ee64515cf7aa01eb -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf.Format 7758d7f3c35667515b5e8d8007ca80d361671e5729c3b22b95fba59cadba4de2 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf.Hpf1Count 210ffbaffa72a7a01261878ddad042e4171838be32f77a01cfa45b6e20d57c84 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf.Hpf2Count 2c8e9026996aaa8f780e4b3557180c28647521d036c908aa39e7d6a3c0378718 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf.OwnsHeap c15cd8a57164a4aa2808d15cf0352d5be1cf1c9794755bd7030bdbb4707c7815 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf.PHeap a78e34d4d4b9e829c523200e5a32e15e0a83bc25574920a345a709013c6bb42b -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf.PHpf1 dd3de5d1a182f70b1d4cc71b896c1b186ff425693baa968fdd634ff6e2e71732 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf.PHpf2 7bc376142fd2080fbe3d7acb7ac28f5980f2b60fed720f8b38bbddd1bdf8aae4 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf.SampleRate bf1dfe0b0836f5b4f88595f20f36be55a4b60ee91960bee4163dc63fa27a0348 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf1.A 6c39a8d9d4b2929e2da7ea38f27219688f77e16a539188dc4a459ebefbdefdda -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf1.Channels 7c7dfcc8255b4c871bd5446f72ed77a3a3452ef3a99c1cc70556e78d6972f79c -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf1.Format e49e5af69cacdfcf31d9e8a1257a728ff2ee2547b9578ff6c52deca9e3badccd -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf1.OwnsHeap 3b23f93cdb7d28e22199256a138f28d705a3766bced5c200b3922b7494792f0b -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf1.PHeap 8fa30d459d1ecd05475f77436d1f5d3fcb96205885bafbff8def36d66f2450f4 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf1.PR1 a9829e1b1a7e85e2eb630776ccdc9a119d7698e25a9a459bd851b7952a1c6de1 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf1Config.Channels 6719789640bfd65ec7addac32dea14393564f3fa9addf8ee00913a71ce989b25 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf1Config.CutoffFrequency 96286d7e0b833c5868d76c771c55ff1511bd96474a5bdbc355cc20d6bf158232 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf1Config.Format 3407938ce518c6d81491a8f8102d2a04f760151c93efeec0d4b8f5a55e9cd26a -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf1Config.Q eb860332c340a91edb26e1500dca387fc435af56fd85aaad45ab6688f5595fe1 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf1Config.SampleRate 57eae20811f35247eb0babd0746e19c639596d46916bbdf6ca14d0f044c336ee -->
<!-- api: F:Inno.Native.MiniAudio.MaHpf2.Bq 09bf2094502063859cd0acba93ec5087f0037bfb199c41213c9a3b2bb57149fc -->
<!-- api: F:Inno.Native.MiniAudio.MaHpfConfig.Channels 26989d27b1f257a1668c0b24e4e3cf9b18ff9020b3ef9bcd27aa6953192e1b7d -->
<!-- api: F:Inno.Native.MiniAudio.MaHpfConfig.CutoffFrequency 520a31e7f2542451dfd43bed919e3576e89704e0a8df4d666145023463151792 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpfConfig.Format db28fac459e9af8b74dcb52292dca76a0a19a61977715f26aa8871be1ab15da8 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpfConfig.Order aeffb4a7f80f7ef80fa06424643ff5a5f4be9bcc3a498c76f072ecaf9d3d2a1f -->
<!-- api: F:Inno.Native.MiniAudio.MaHpfConfig.SampleRate fc36be1349c5a21b161710d0a6ddda804227e86576b618447f13c8e4f285aab2 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpfNode.BaseNode bedc58e24b07810b9f517efd549e9db75f6cbd1e02b6ef72fca4fc035f71a411 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpfNode.Hpf 2292e976b724135dc973e7545b82bc1ea732d5fef93d433c759a20996bfd39c8 -->
<!-- api: F:Inno.Native.MiniAudio.MaHpfNodeConfig.Hpf e470288452655b4ad5fff76557f8e9041389154ecd256a3b21a7cebecaf5a8ad -->
<!-- api: F:Inno.Native.MiniAudio.MaHpfNodeConfig.NodeConfig 068cbae73f9f0770751e1b6df8c146bc614cbf60e7511fd947abc2b0a0688bf3 -->
<!-- api: F:Inno.Native.MiniAudio.MaIMMNotificationClient.Counter 393498639bbf2298a2a971531deda06c63215860fe2b93945460f463e87c952a -->
<!-- api: F:Inno.Native.MiniAudio.MaIMMNotificationClient.LpVtbl 6939334ea8631fc146d28f346937079ef87db35598afe2f0e9b603d6010cd70f -->
<!-- api: F:Inno.Native.MiniAudio.MaIMMNotificationClient.PDevice 60dd1fc40ef4504b8cbbacfc0d3f114a796bd460e2d87774af4340db134124c2 -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_ambient 5c0effe2e153b3c300e988ae56ad6576c24efb08909d3ede75a844d7bff3dcd7 -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_default cc0a9187dcc96f650b35c126ba73c022be94163f155559259d128b906c3aa2bf -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_multi_route 88a1521dd5170d45269665baf18caca9f2362e5049d23bb5c57e319a30636102 -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_none e79a3b1ca4a7e6a28503ddd0477dbafdfa8ee7a053111116d4ea33d50df8693d -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_play_and_record 387df226f3ce7dbe0923ffc2718acc8953999d6dc8341ecdfee09cd02e7b628e -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_playback 643cfa91838757577f4c836984922e2d25988c0adc2f140cd416c4e6206f6437 -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_record be9517b62718f7e159b8679491d239cb4ee6fc00fb35604fe27482694e42f1c1 -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_solo_ambient 2072d3d4170b93de3f768ec6ba020e6d7e678317969cac0dad37fb0cb725b6b3 -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_allow_air_play 4a81e68de0aed3ce65ce32f2a28eb016ad220b61c75621a4845849195d81268c -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_allow_bluetooth 75b0b57a8186e559fabe95d6cb97a0afbe952f9040357613d8b7a2de134d2095 -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_allow_bluetooth_a2dp 9e395122ffa57ba5ef81e2480ba69f3883f7166d42d593220c9faf0f6243df56 -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_default_to_speaker 22b42855a3b04b353711144ce58d12130d27cedbc073c8b92a4a8d5db8be56a0 -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_duck_others 6a2cce8ffe75079a7b3838a33f0a83b950f155198137c3522d3c61cbe71e63f0 -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_interrupt_spoken_audio_and_mix_with_others 6bb774ff3f94886b69e4ea7d0179d044cf77556456915c6ac6dd2d478d441a10 -->
<!-- api: F:Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_mix_with_others 4f9730ec0391273f96d58c0e2197068dfa8facb79d62dff2435edda7dee28d9f -->
<!-- api: F:Inno.Native.MiniAudio.MaJob.Data 7bd4c63f412cfcbd4216e457bcb37ffd071a309eab6249b1df8f0c446d415612 -->
<!-- api: F:Inno.Native.MiniAudio.MaJob.Next 383c574cd359b9f005b0779c31bb989262cb3ef4dfa9553893305e84ba7945bf -->
<!-- api: F:Inno.Native.MiniAudio.MaJob.Order 1659b581ec0a38e040039f94ff069cdd12c2a19e1127bac3631c6b8b3c10ed5d -->
<!-- api: F:Inno.Native.MiniAudio.MaJob.Toc ed3e7144b39ec445bf0ee954b8295421d72e81be117a72f42efcd3acc7688680 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct20149c7d719fbcae.Code 865ec3cd48fe2ac0a1c3d61277b1664618bc58700e71698a0d2c92b47554d793 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct20149c7d719fbcae.Refcount 7150a7d64918c32eb752e0e71af886f662ed694200976e84189d651b1cb1c80c -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct20149c7d719fbcae.Slot 5d7e8df8cdc1c60e3f48ed86c9b00609217c00f356ccc6775bd1f1afaac6b5f4 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct21c26594439b048b.PDataBuffer 0e633e18f2a04ce07ad94a95bcd44aa1931f89bf2173036b808372c29d05fdd4 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct21c26594439b048b.PDoneFence 860ba833c1fc0905c4e22d5b7d1d549700ecbd29a76941b1852be042ec8bd6b2 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct21c26594439b048b.PDoneNotification 31b05b8a7840c4824b15ff1b2de5e16c7678170f4e8102396833c5476b15e260 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct36d3c431f4e90735.PDataStream 842e8d56f7f10a1ee08a6941cb9702aefae566d0112e2a8ddab71990a08420a8 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct36d3c431f4e90735.PDoneFence 452c6ec525f901ee18529d35f8a462bac58a44f36dacc0bbaa52e2f373aa75f5 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct36d3c431f4e90735.PDoneNotification 6b9478919aaf602b00fdef82d596ac4e7f2d82e4c9fdd000cf9f91b40052d907 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct74269070ad2e966f.PDataBufferNode 2d7076cc79ad3d67485f6ff2991003bff38edf2cbbefc69a3ee656bebabc1655 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct74269070ad2e966f.PDecoder 8b7510a9e35aa1c362eda7256e236d619a9653ab363a11f9924765601f0f6af4 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct74269070ad2e966f.PDoneFence ecf0a770dbfca86374afe940f11d6a5863e01aa65c356fff4d806c154c57cfa6 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct74269070ad2e966f.PDoneNotification 6435bf26e7e90b4e83c3a8971859ac1b4795cc753264711ad122f00e9e979b7d -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStruct74269070ad2e966f.PResourceManager f9412459c5a12aeeca05dd6ae5983c016cf5947261e81fee236ced7cda54f6b1 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.IsLooping e77dd8fb0491cea7fcc10732d5c991660309a3cdcb5b729d34214edb4bf89b0e -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.LoopPointBegInPcmFrames 3d634c1ac5ccbf7f4dbf4475d55df303364b427b5908655fadf053531e466711 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.LoopPointEndInPcmFrames c8429ce3a03c37cb5840582f6d7d2b0599490d86e4e3becbfcea68306070d062 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.PDataBuffer bfcb9a7b19d077884d9d34c8971c4a006fe521fac8b678f15c1de4629b183232 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.PDoneFence 05e34945ad502dd6ca6df1800b0f96ec212fb7bf8831a702fbdf9807071769a1 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.PDoneNotification a6163e571d349b2ba725a510d48ec038808fbea87a4ad8c5391df520ee6282c2 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.PInitFence e21044d61c79b15b32460f5a9da43420f62bc1f59ef64e0fa2bff4e0cce268ed -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.PInitNotification c3204b65dcf5ab3933b25d2b8618f90a4e32c768b241e6325eb8a14ff0f8b24b -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.RangeBegInPcmFrames f5d6b30f47a1ac3c07a09507d5381f9a6ae0ff9715ce7cf80b3eed8675f58a6d -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.RangeEndInPcmFrames 18eddaeb21a77a65414c6678cb972afd327ad91673fedbb707b557bf725edaa1 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.Flags fd23b292d167f7fa34c2d52e2923ea57841d26c409121c4cfa0245cf24a28d5a -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PDataBufferNode 06914be74b1d3000bbd9b0e91d949dd7a1e56bb9904fdeb9f00a0f72b3671079 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PDoneFence 912e178a3815939c1e2414d44b9d025a82ecdcdbd524e25d8f2e8b2d4c44dd6c -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PDoneNotification d1b0fc743de25e49081b5b98af845f6e2623f973d3468f0e5dca1c3cba1915dd -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PFilePath 8b5f3ff7422a6628a4ab2d02ef3681ae14cb752edd2cb3030fea84d46d0d378d -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PFilePathW 82c9f877cdaf42402eda2b50b1625e3adec42a93354ac19139f9a42d74c46e60 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PInitFence 7cf3f7bcab96f6723a4f78ff0315279b27b74f5bb2e572e3ef8f6b4ca3664a2b -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PInitNotification dddea2904f49e53c579b43169b97cd3962a4795e159e71ee8297d0e21a774045 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PResourceManager 656c13cdab807a1844240ff5d3d04ae4a0cc5b78ecf336915d9be26e95059b65 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAe6fc2fcd4157e5b.InitialSeekPoint 346654c8cdb3e15307ee31e7772e00691a5398f2f6f9e13c33a12bae1eeb0d6e -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAe6fc2fcd4157e5b.PDataStream 2e660d91d94c60959f2c980c6113ec7d9c63601f226191f4f9d9f3adddddbe1a -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAe6fc2fcd4157e5b.PFilePath 29ef52e82a2d56d3dd1e35ba14c5246a6a26491c60d60a7833af0fb3d8ab8ce1 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAe6fc2fcd4157e5b.PFilePathW e169e4660fc3b26cbfaaca9a57a4739ab535a165e6bd0e6305875d80bc52f89b -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAe6fc2fcd4157e5b.PInitFence b250edbc8993d62201fcea945567ea4bd19409ec6520d2bdb1602f75e40e7044 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructAe6fc2fcd4157e5b.PInitNotification 22432bb8dd8a531f48493d1f365fb7eb78e6ea542cde2a7750cc307e23af2d68 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructE3c4fc217309ab00.PDataBufferNode ce102d92f3e3e4a225750452f52a88a07cb72bbbf7ad6c1d926db37066c6b9ae -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructE3c4fc217309ab00.PDoneFence 2ea374fb39862dc5842877b9e532cfd454452443fd813311d3101e8017151f15 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructE3c4fc217309ab00.PDoneNotification 7aa24c08dc2a7187eef27d115fecfb3197166e50656114e6a4f5713fd8d344d9 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructE3c4fc217309ab00.PResourceManager 23df084803c584115a3fe27ff95baa5c8d16cb2a6411c63a98bc2314467651db -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructEee8605d2045b5ba.Data0 9f7defc39cd07d94e2e414b7af1f118e679e5d35e6677bc44f5aeee786f1ca5f -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructEee8605d2045b5ba.Data1 a4fadf17853573ddbbd3b8b7acfcde3a7d26b35cb3b7a69a7811979bc892dc61 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructEee8605d2045b5ba.Proc b87be6c3e6a4503394825fadfe414b6746b52bb06f07353da00808e66577bdee -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructF112c5486042b6f9.PDataStream 2225295ee04e9c200ba7139428a2b20dba5b1d32eefd76fde26cd6bb903f4577 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructF112c5486042b6f9.PageIndex 95b604859a15118a7a413ad41fd497d3ee98ab9f4f76afecf68d0b2f4da0be99 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructF6ac5bb0aa45cfd0.DeviceType 488745afc0261d944681e3323214987dabc9868a9fc66a366bb556ee28b75152 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructF6ac5bb0aa45cfd0.PDevice dfb8e5578b9cf6b09231f574d28cbc24aa1447b9de904a6c827f88c3af022da4 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructFb99d8299aee1e8c.FrameIndex 9261597cec25c9619063d6b9bfd1901dbf0c499e48c3720954b744019b775a9b -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousStructFb99d8299aee1e8c.PDataStream 9e2dbb23bd9ab8350fa24eeab40baa597867bf2797d8ba055a059a619c65acd1 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion0c22262d5e21db83.Custom 290015894b6664f300de549ec1069bd84cb9f10d6a499403caa47e1f403f1b6f -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion0c22262d5e21db83.Device dbb5ebfe2085bf6d84132f4f868ce2ea8d8fd76480427d19361ec0a763a192a5 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion0c22262d5e21db83.ResourceManager 2c3c5607afb3c88d6ccefec69181f8a6bec0b05ead8012f8eb5345d94c3512cc -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion68909ef0547c8fe7.Allocation 9b2b21ac0ca69f24f62fa2a132fb4d96b5efea02ab1f3241802d99fb84779faa -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion68909ef0547c8fe7.Breakup 05717c16b02ab60c0161b9d09f62f190bb94d8d68b7e5def2870a5bcf5eeb877 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.FreeDataBuffer 723d00254584db430bbb5d74f952ab6f6115f0eac049cea47ca6989ff1535511 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.FreeDataBufferNode bdb6ad5438286e32154a9bc4fdc7b83b0243a09f0aed5952b457997c1696721d -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.FreeDataStream 53c6da240c00d90eceffda4f90da7a55dfee6c3d05302d124b4f7b60262a6bcc -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.LoadDataBuffer 76b23c1866e83690a8dce764cdf09c3597a74fb72dfd93d90d12f14796b6e219 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.LoadDataBufferNode 41cd798af61c65d96d812765d07b429e075cce4dae1eeca6c23870ff16766899 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.LoadDataStream b35cd62e76831f5f47b12040a97168387c0fc186cb3d3653a7cd86c12b1e9358 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.PageDataBufferNode 4fda4fc4cc552b14180f3a6da5b8c69890774515346996006fe0aeeabc4f6c6b -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.PageDataStream bf0d4295022f9e57d7f8f54c9c73512ee42478bdf270365a87bf31c8577f8b87 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.SeekDataStream 1c71fd47fed01c431d60377b698daf5936bf5fa6ca4d03d7f9df05be9db9ba0d -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion73ca8060d9820b4c.Reroute d697931ce8df7507335d2daeb432c4ba852b6a5d9ba7bfae31ab35bd15075d08 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobAnonymousUnion85cb70704396c9bd.Aaudio 83c5994f3d47135027081be1dcaa9af2055152f73370949af6dd19901b84bc91 -->
<!-- api: F:Inno.Native.MiniAudio.MaJobQueue.Allocator 1b20f396347ae1924e52daa87d4724625fa55bcddec44c1e666371c2dcd9963d -->
<!-- api: F:Inno.Native.MiniAudio.MaJobQueue.Capacity 3959f2effbdedbe6628dcf5c3cd3bfd5c662618b3acda98db6725969b634f1dc -->
<!-- api: F:Inno.Native.MiniAudio.MaJobQueue.Flags 9612eb4acc6abfeb64464160f1d934e4fc0072bd4ad768f6ed3365c300fd53f6 -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-f28f2d825a5d0433"></a>[`Inno.Native.MiniAudio.MaFormat.ma_format_u8 = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20691) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e0528d03a36a71cc"></a>[`Inno.Native.MiniAudio.MaFormat.ma_format_unknown = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20690) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9c759bc819b86dfd"></a>[`public Inno.Native.MiniAudio.MaGainerConfig Inno.Native.MiniAudio.MaGainer.Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20710) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5afb539587b345ed"></a>[`public float Inno.Native.MiniAudio.MaGainer.MasterVolume`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20714) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-42f4d31dfc9b5842"></a>[`public uint Inno.Native.MiniAudio.MaGainer.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20722) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ad6250780c5d088c"></a>[`public void* Inno.Native.MiniAudio.MaGainer.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20720) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a19089fddeaedbd6"></a>[`public float* Inno.Native.MiniAudio.MaGainer.PNewGains`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20718) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0e568e221000b97d"></a>[`public float* Inno.Native.MiniAudio.MaGainer.POldGains`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20716) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-877d2dbe3f568da5"></a>[`public uint Inno.Native.MiniAudio.MaGainer.T`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20712) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-da5ce3aff79f5920"></a>[`public uint Inno.Native.MiniAudio.MaGainerConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20743) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c98a55a91f73363d"></a>[`public uint Inno.Native.MiniAudio.MaGainerConfig.SmoothTimeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20745) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-24e4e38b404cca60"></a>[`Inno.Native.MiniAudio.MaHandedness.ma_handedness_left = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20764) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c5ccc0c66bad6a4f"></a>[`Inno.Native.MiniAudio.MaHandedness.ma_handedness_right = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20763) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-4abcdc9f59602652"></a>[`public Inno.Native.MiniAudio.MaBiquad Inno.Native.MiniAudio.MaHishelf2.Bq`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20778) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7be1679691017132"></a>[`public uint Inno.Native.MiniAudio.MaHishelf2Config.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20801) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-fa527b1130d50a97"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaHishelf2Config.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20799) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bfaa08e87d2d203b"></a>[`public double Inno.Native.MiniAudio.MaHishelf2Config.Frequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20809) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a61b8c98d277c118"></a>[`public double Inno.Native.MiniAudio.MaHishelf2Config.GainDb`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20805) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bfe57b99a359c0c7"></a>[`public uint Inno.Native.MiniAudio.MaHishelf2Config.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20803) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f711c060c79647f6"></a>[`public double Inno.Native.MiniAudio.MaHishelf2Config.ShelfSlope`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20807) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3aff96e5735c29df"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaHishelfNode.BaseNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20830) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-26514cbda6b0aaa4"></a>[`public Inno.Native.MiniAudio.MaHishelf2 Inno.Native.MiniAudio.MaHishelfNode.Hishelf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20832) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-47938b1319cb5b40"></a>[`public Inno.Native.MiniAudio.MaHishelf2Config Inno.Native.MiniAudio.MaHishelfNodeConfig.Hishelf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20855) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0c51c1ccaa6f1112"></a>[`public Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaHishelfNodeConfig.NodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20853) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f2da4d144fc70188"></a>[`public uint Inno.Native.MiniAudio.MaHpf.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20878) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1461b79376bd44d7"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaHpf.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20876) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-77b399f7b14ed44a"></a>[`public uint Inno.Native.MiniAudio.MaHpf.Hpf1Count`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20882) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2634f8e76eff1cca"></a>[`public uint Inno.Native.MiniAudio.MaHpf.Hpf2Count`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20884) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-764d50d336b517d7"></a>[`public uint Inno.Native.MiniAudio.MaHpf.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20892) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-309fa9920c0ea962"></a>[`public void* Inno.Native.MiniAudio.MaHpf.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20890) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7df45e2cb2d42c3c"></a>[`public Inno.Native.MiniAudio.MaHpf1* Inno.Native.MiniAudio.MaHpf.PHpf1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20886) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bb676b7334041884"></a>[`public Inno.Native.MiniAudio.MaHpf2* Inno.Native.MiniAudio.MaHpf.PHpf2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20888) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-84f69d74e4033b35"></a>[`public uint Inno.Native.MiniAudio.MaHpf.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20880) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-612aa26d823559dc"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaHpf1.A`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20917) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-957e57551b8d1aef"></a>[`public uint Inno.Native.MiniAudio.MaHpf1.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20915) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7b5eeaadbb2a4c6b"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaHpf1.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20913) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-df120b09982f20f5"></a>[`public uint Inno.Native.MiniAudio.MaHpf1.OwnsHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20923) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e327c82bee294668"></a>[`public void* Inno.Native.MiniAudio.MaHpf1.PHeap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20921) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-463f678788caf9f3"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient* Inno.Native.MiniAudio.MaHpf1.PR1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20919) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e80c692afbf3decf"></a>[`public uint Inno.Native.MiniAudio.MaHpf1Config.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20946) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2cf4ac1c27b82e18"></a>[`public double Inno.Native.MiniAudio.MaHpf1Config.CutoffFrequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20950) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6d4e6ef74b0750e0"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaHpf1Config.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20944) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-121b42f17dfb516e"></a>[`public double Inno.Native.MiniAudio.MaHpf1Config.Q`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20952) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c9fdfed16e423459"></a>[`public uint Inno.Native.MiniAudio.MaHpf1Config.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20948) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ba651c9685e2533"></a>[`public Inno.Native.MiniAudio.MaBiquad Inno.Native.MiniAudio.MaHpf2.Bq`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20973) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f61859cb83310f17"></a>[`public uint Inno.Native.MiniAudio.MaHpfConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20996) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bb010a3edffddc2b"></a>[`public double Inno.Native.MiniAudio.MaHpfConfig.CutoffFrequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21000) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1143386ce9bbd6d7"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaHpfConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20994) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7f4efdccd99089e3"></a>[`public uint Inno.Native.MiniAudio.MaHpfConfig.Order`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21002) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-922409e378510209"></a>[`public uint Inno.Native.MiniAudio.MaHpfConfig.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L20998) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a73bbffa6225a01d"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaHpfNode.BaseNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21023) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-302885e08e82eb8b"></a>[`public Inno.Native.MiniAudio.MaHpf Inno.Native.MiniAudio.MaHpfNode.Hpf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21025) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f3a80cc452a66b45"></a>[`public Inno.Native.MiniAudio.MaHpfConfig Inno.Native.MiniAudio.MaHpfNodeConfig.Hpf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21048) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0c5ca9f1a7352ab5"></a>[`public Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaHpfNodeConfig.NodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21046) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9d103ed344a84229"></a>[`public uint Inno.Native.MiniAudio.MaIMMNotificationClient.Counter`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21071) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8575a1cec931cb29"></a>[`public void* Inno.Native.MiniAudio.MaIMMNotificationClient.LpVtbl`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21069) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ac101f7e59ef1473"></a>[`public Inno.Native.MiniAudio.MaDevice* Inno.Native.MiniAudio.MaIMMNotificationClient.PDevice`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21073) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-04bae11e5a21e777"></a>[`Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_ambient = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21093) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-73a5a3941a52be77"></a>[`Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21091) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ed2a20ae012645c"></a>[`Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_multi_route = 7`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21098) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2644270b1a729190"></a>[`Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_none = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21092) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-3bebb27851d50500"></a>[`Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_play_and_record = 6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21097) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b615e7cf808d49df"></a>[`Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_playback = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21095) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ed603873abe331f9"></a>[`Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_record = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21096) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bae458ba827347d7"></a>[`Inno.Native.MiniAudio.MaIosSessionCategory.ma_ios_session_category_solo_ambient = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21094) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-758efe2909ae05d8"></a>[`Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_allow_air_play = 64`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21115) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c467167c9bce5b84"></a>[`Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_allow_bluetooth = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21111) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-468db244223a58e8"></a>[`Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_allow_bluetooth_a2dp = 32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21114) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d75bbcb2c83b5b02"></a>[`Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_default_to_speaker = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21112) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0b557de89a72f2f0"></a>[`Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_duck_others = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21110) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-1e0bcfceab8de774"></a>[`Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_interrupt_spoken_audio_and_mix_with_others = 17`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21113) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5aef300a51032ce1"></a>[`Inno.Native.MiniAudio.MaIosSessionCategoryOption.ma_ios_session_category_option_mix_with_others = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21109) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a238a3053295135e"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousUnion0c22262d5e21db83 Inno.Native.MiniAudio.MaJob.Data`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21135) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-584fc6752efc6728"></a>[`public ulong Inno.Native.MiniAudio.MaJob.Next`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21131) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c040846e407da7f2"></a>[`public uint Inno.Native.MiniAudio.MaJob.Order`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21133) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-76af425d10a8f58a"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousUnion68909ef0547c8fe7 Inno.Native.MiniAudio.MaJob.Toc`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21129) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2af736173e2e8db1"></a>[`public ushort Inno.Native.MiniAudio.MaJobAnonymousStruct20149c7d719fbcae.Code`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21156) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c4199ab1ac888fa6"></a>[`public uint Inno.Native.MiniAudio.MaJobAnonymousStruct20149c7d719fbcae.Refcount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21160) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-cf3a8d51487ab025"></a>[`public ushort Inno.Native.MiniAudio.MaJobAnonymousStruct20149c7d719fbcae.Slot`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21158) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b57a38ad06623123"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStruct21c26594439b048b.PDataBuffer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21181) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ab8f3fa962b4b40"></a>[`public Inno.Native.MiniAudio.MaFence* Inno.Native.MiniAudio.MaJobAnonymousStruct21c26594439b048b.PDoneFence`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21185) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b17aa3c13c711aeb"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStruct21c26594439b048b.PDoneNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21183) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-50f11be40a7aed49"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStruct36d3c431f4e90735.PDataStream`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21206) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-38ed37e79ea3d78c"></a>[`public Inno.Native.MiniAudio.MaFence* Inno.Native.MiniAudio.MaJobAnonymousStruct36d3c431f4e90735.PDoneFence`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21210) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-549ef914d699aaca"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStruct36d3c431f4e90735.PDoneNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21208) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6e0da370bfb4b3f8"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStruct74269070ad2e966f.PDataBufferNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21233) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a46d93ebe4cb366a"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStruct74269070ad2e966f.PDecoder`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21235) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5428c60ab0ee21e4"></a>[`public Inno.Native.MiniAudio.MaFence* Inno.Native.MiniAudio.MaJobAnonymousStruct74269070ad2e966f.PDoneFence`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21239) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2482cd225b9e4a3d"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStruct74269070ad2e966f.PDoneNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21237) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9402714e960bc194"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStruct74269070ad2e966f.PResourceManager`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21231) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-e69dc9b8185a3d5d"></a>[`public uint Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.IsLooping`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21278) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8536c902ced6da1d"></a>[`public ulong Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.LoopPointBegInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21274) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0f52715b563c0f81"></a>[`public ulong Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.LoopPointEndInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21276) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0723855fe5444ee9"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.PDataBuffer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21260) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2f0da4a630dc84ae"></a>[`public Inno.Native.MiniAudio.MaFence* Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.PDoneFence`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21268) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-154a3e951e279759"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.PDoneNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21264) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5c3f6aad8e952286"></a>[`public Inno.Native.MiniAudio.MaFence* Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.PInitFence`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21266) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-23ed03ae3dc4b5d7"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.PInitNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21262) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-358c8e1004eb68ef"></a>[`public ulong Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.RangeBegInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21270) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-ec83a70f0d730ac1"></a>[`public ulong Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a.RangeEndInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21272) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9b95f02c2b10a5ca"></a>[`public uint Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.Flags`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21307) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-22dacee186ad46cb"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PDataBufferNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21301) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f149543ece388720"></a>[`public Inno.Native.MiniAudio.MaFence* Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PDoneFence`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21315) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f57194ec9a1b55f4"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PDoneNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21311) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-408e6bde8e1cf748"></a>[`public sbyte* Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PFilePath`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21303) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-47c02612a0c79d5f"></a>[`public ushort* Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PFilePathW`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21305) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-87d587328caabd4d"></a>[`public Inno.Native.MiniAudio.MaFence* Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PInitFence`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21313) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d21900b3df7c7e05"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PInitNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21309) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-64706765a28d9f8f"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150.PResourceManager`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21299) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c5211fba5e37ae01"></a>[`public ulong Inno.Native.MiniAudio.MaJobAnonymousStructAe6fc2fcd4157e5b.InitialSeekPoint`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21342) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-85941cdb47d40224"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructAe6fc2fcd4157e5b.PDataStream`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21336) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-b2861dbcb3fc8b23"></a>[`public sbyte* Inno.Native.MiniAudio.MaJobAnonymousStructAe6fc2fcd4157e5b.PFilePath`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21338) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-15ddb7d99f89f67e"></a>[`public ushort* Inno.Native.MiniAudio.MaJobAnonymousStructAe6fc2fcd4157e5b.PFilePathW`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21340) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-bcf1c919f9417d14"></a>[`public Inno.Native.MiniAudio.MaFence* Inno.Native.MiniAudio.MaJobAnonymousStructAe6fc2fcd4157e5b.PInitFence`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21346) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9088399fe0e7b353"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructAe6fc2fcd4157e5b.PInitNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21344) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-61b4929dda63d935"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructE3c4fc217309ab00.PDataBufferNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21369) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d62f26f40bbcd5e0"></a>[`public Inno.Native.MiniAudio.MaFence* Inno.Native.MiniAudio.MaJobAnonymousStructE3c4fc217309ab00.PDoneFence`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21373) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-9a9064f61dc14ea4"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructE3c4fc217309ab00.PDoneNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21371) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a7b665a12a976455"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructE3c4fc217309ab00.PResourceManager`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21367) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-f9b43f6bbfbbc58e"></a>[`public ulong Inno.Native.MiniAudio.MaJobAnonymousStructEee8605d2045b5ba.Data0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21396) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-c0e3da98c5a93b77"></a>[`public ulong Inno.Native.MiniAudio.MaJobAnonymousStructEee8605d2045b5ba.Data1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21398) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-56a84cc6f406d788"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.MiniAudio.MaJob*, Inno.Native.MiniAudio.MaResult> Inno.Native.MiniAudio.MaJobAnonymousStructEee8605d2045b5ba.Proc`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21394) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-6ea16af40e941ad7"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructF112c5486042b6f9.PDataStream`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21419) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0afcb67ae8ff1a09"></a>[`public uint Inno.Native.MiniAudio.MaJobAnonymousStructF112c5486042b6f9.PageIndex`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21421) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-139d1fe797700277"></a>[`public uint Inno.Native.MiniAudio.MaJobAnonymousStructF6ac5bb0aa45cfd0.DeviceType`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21444) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2bb0721e35cfe59b"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructF6ac5bb0aa45cfd0.PDevice`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21442) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2dfc7bd411ecfe19"></a>[`public ulong Inno.Native.MiniAudio.MaJobAnonymousStructFb99d8299aee1e8c.FrameIndex`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21467) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d5773891c40a7cb9"></a>[`public void* Inno.Native.MiniAudio.MaJobAnonymousStructFb99d8299aee1e8c.PDataStream`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21465) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7ffe75993c5ae356"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousStructEee8605d2045b5ba Inno.Native.MiniAudio.MaJobAnonymousUnion0c22262d5e21db83.Custom`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21488) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-0b6591be799ab798"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousUnion85cb70704396c9bd Inno.Native.MiniAudio.MaJobAnonymousUnion0c22262d5e21db83.Device`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21492) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-adfd18034c78b4c5"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0 Inno.Native.MiniAudio.MaJobAnonymousUnion0c22262d5e21db83.ResourceManager`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21490) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-5fb60c80f5fe7fe7"></a>[`public ulong Inno.Native.MiniAudio.MaJobAnonymousUnion68909ef0547c8fe7.Allocation`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21515) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-37e8facfb028c604"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousStruct20149c7d719fbcae Inno.Native.MiniAudio.MaJobAnonymousUnion68909ef0547c8fe7.Breakup`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21513) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2738395fc2af3151"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousStruct21c26594439b048b Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.FreeDataBuffer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21544) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-8e9721b3de90e1a0"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousStructE3c4fc217309ab00 Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.FreeDataBufferNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21538) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-47af140108632caa"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousStruct36d3c431f4e90735 Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.FreeDataStream`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21548) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2af27e091e549e2a"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousStructA0b0de7a870d2f7a Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.LoadDataBuffer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21542) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7101af61035d1877"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousStructAae8d31472e99150 Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.LoadDataBufferNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21536) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-316394a7aacfcf14"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousStructAe6fc2fcd4157e5b Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.LoadDataStream`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21546) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-2795822f841b1a16"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousStruct74269070ad2e966f Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.PageDataBufferNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21540) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-12048f0fee6c8de7"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousStructF112c5486042b6f9 Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.PageDataStream`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21550) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-a43003c519f36685"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousStructFb99d8299aee1e8c Inno.Native.MiniAudio.MaJobAnonymousUnion71082220c439abb0.SeekDataStream`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21552) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-376ee0c0a49284e7"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousStructF6ac5bb0aa45cfd0 Inno.Native.MiniAudio.MaJobAnonymousUnion73ca8060d9820b4c.Reroute`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21573) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-40d676205f897e95"></a>[`public Inno.Native.MiniAudio.MaJobAnonymousUnion73ca8060d9820b4c Inno.Native.MiniAudio.MaJobAnonymousUnion85cb70704396c9bd.Aaudio`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21594) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-d059d1d58232572c"></a>[`public Inno.Native.MiniAudio.MaSlotAllocator Inno.Native.MiniAudio.MaJobQueue.Allocator`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21634) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-caa77e2c7b7d3506"></a>[`public uint Inno.Native.MiniAudio.MaJobQueue.Capacity`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21626) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
| <a id="api-7fc2d4f61ff7b775"></a>[`public uint Inno.Native.MiniAudio.MaJobQueue.Flags`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L21624) | 当前生成的 ABI 声明；原生地址与回调只在所属 owner 声明的有效期内使用。 |
