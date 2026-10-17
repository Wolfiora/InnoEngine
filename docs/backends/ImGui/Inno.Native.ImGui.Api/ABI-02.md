# Inno.Native.ImGui ABI · ImDrawListPtr → ImFontAtlasBuilder

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImDrawListPtr</code> · 162 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImDrawListPtr 5e55db294aa31cfb6be6f7f45b21493262217eda28eeb574cfd0c81725091b76 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.#ctor(Inno.Native.ImGui.ImDrawList*) 76de37b1ddea6cd643c353064d054d0f8c983244c4bc9d556d1dbf97b0bb3803 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListPtr.Handle 11b027e0559d6920a21f1f23ee0a03b6b9fb31eec8385254702be942b7510006 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.IsNull 3b25484b0621a375076731bf28e1336918b688513bdedd97461b9869f9ba72dc -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.Null cc5f3751be35f7670be5f303447f0be03ab1854cb0a7e8c68648aed9e6cbbc6d -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.Item(System.Int32) a4cf14ba309edcdad4983b35e190c70ed1b728e1382fd22bfd794672c1289637 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.op_Implicit(Inno.Native.ImGui.ImDrawList*)~Inno.Native.ImGui.ImDrawListPtr b45152ebfa8830965084b50c06a8b8185da21d9ad7c9562fd27e06ad75125bf5 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.op_Implicit(Inno.Native.ImGui.ImDrawListPtr)~Inno.Native.ImGui.ImDrawList* 93427582cd671c976b750e4ba4f7d1bb8355d42722e4c1e7c654702b90dafa22 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.op_Equality(Inno.Native.ImGui.ImDrawListPtr,Inno.Native.ImGui.ImDrawListPtr) 35a7af003fe8c9496e7f6b0bb6266cad0d9a7f11a7eb71a38fbbb45cfc0d4565 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.op_Inequality(Inno.Native.ImGui.ImDrawListPtr,Inno.Native.ImGui.ImDrawListPtr) 81f2fcc9c67b017aa80fe6e61f990c00a4f1fa91bee28f3d07c1bcd7a483b3a1 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.Equals(Inno.Native.ImGui.ImDrawListPtr) c37be13c8a54d6dc5e9a8ef9cb93cb2c68998e3b16a11660edf5bb4fb9fff26f -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.Equals(System.Object) 3529e59a6b8fe13ffc48aadc77ee4a0d6767bb925a43bfcceca9a47552f60758 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.GetHashCode 59004dc2a84c2ef796bb8d91d33060437a7fbd0828ef7493ba3498f5a79a456d -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.CmdBuffer 20b013f53d305cbe7d7e1ee2282a3be8c86d4e1c6b90640847173ccaf205dd39 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.IdxBuffer c5cb1e2fcafafb9664139b1cba5035d906dff3fa8f1324446854fa70539a88e0 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.VtxBuffer dffa13c3b2b2601d6466443030328163465ef76703158f81d62d366320799fad -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.Flags 8786fc0f0d347dc2f280b55acd52857e7f6e5650405a36e3c9650ab05f81d6d1 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.VtxCurrentIdx 5439c78a00dc9ac41ffbecc35f768af4508908c2d078a6fdbe50829ea157df25 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.Data c0b241f8915113d8400e207ed4086ec8a17eb9c5a06411d52365a81398dfc933 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.VtxWritePtr 768fcc39f4105dcd6de0e1bfd7603c21044707bca473bf6cc784c9b8faddf556 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.IdxWritePtr a04dfe8884c58cdf08489855984a1200d9017b520273cca584455be27e4c483d -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.Path 364fd8d910f3b087257393ed3d69adeafd7c197a865474a9208d560e8aaaf7c7 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.CmdHeader 4c0373d54ae8f98138cbe06565d30bca665f058d62ba38a68dc02ba89a9ed89f -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.Splitter 7567366e797afc65a31ea943dbb63cb3c9b4fcfa7ba69c2c80de2671ae1d41d8 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.ClipRectStack 52d0d1dc3a8c15fd6de1011badc60ab97e6755f23acdaf5e25e62280c307d21b -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.TextureStack d4d7c2a6d80a883d94a74b3f5790f20c3dc552bc87739b13664f77879c42943e -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.CallbacksDataBuf 87329cd1342696890c0728990b7c557930aeb52e28e492b16a96a5c22db6929a -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.FringeScale fd19e0953180c10842223fa7dd4ea759aef012f7ebc160026a0cf1f49fbac844 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListPtr.OwnerName 8ffe7ab2afe23391647db4296258e5bf0cddb9c566c9ad3c04f21b5d14050859 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.Destroy 1d21570bf27e5296bfe05b44e80ae99fc0b74deacc01ddb25db29c32c9084cf3 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PushClipRect(System.Numerics.Vector2,System.Numerics.Vector2,System.Boolean) 41afdd5ce8430e8b8eeba79073ec762c23841b0223c372970f13b3d57489fea7 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PushClipRect(System.Numerics.Vector2,System.Numerics.Vector2) 7721354227e9572ba4f65dfc545ec32259b452ed4bec584d5f43fe51aaa95382 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PushClipRectFullScreen e635dd334df0b669fcb8271354a822470ac525a581dabd753e412ea424f7c37b -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PopClipRect 94b6b483e8a5d2f008cde26e814a4150bd9b5a171695cca0063937fa6ad06296 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PushTexture(Inno.Native.ImGui.ImTextureRef) 826519de895c41bc44775715946ff8307f690922bbb28648c607817f51ef9be2 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PopTexture 96a1fe1fb617bce2143230c7f753d29b74559686d0e76442940a8ed5de635753 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.GetClipRectMin 003b8c8021aa2e638cfb1d9cfb9262779391f937ab0f909a08232f91ff74355a -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.GetClipRectMax 662d252c4be21b36ec9325f48cc7be7b6fb1dc32d39f070af2b2ba8e12c96fda -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddLine(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single) 326215768c228e2323b6b38c8f602c010fa191cf65dc0588df2e21fdc4b5f14d -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddLine(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) b5c6312c60ac9e3a37dd232d23c1382d79db4ed854322280bc30be38844b7d10 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single,Inno.Native.ImGui.ImDrawFlags,System.Single) af829270d5844779a003fc69ff3db7bc159ca913fed6fda6ac7ad1ea4ad17a95 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 92283f97abbee1ce01135ab6a89e901f24a14fd7321a276c603a72fa213bbbfb -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single) 2b64b90541f42ddb7e8346d35b659f6dcc4c0b186d39b9ad409dd83234e928cf -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single,Inno.Native.ImGui.ImDrawFlags) 96407092be72b280ff3cdfc2c918c3bf2d24a09ff3afb3c86eb90e0d54d16431 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,Inno.Native.ImGui.ImDrawFlags) 68f1d861af48ede41d228ff514dc72eb320fbe96905a65903d14421108e60ae4 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single,System.Single) 5a65401ba3acbdd5edfce61c64866b0c0c3765efc2c73d5d5b0e3aae7c34b4eb -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,Inno.Native.ImGui.ImDrawFlags,System.Single) 9e7230d02ce08dfdc0b95f28299a7d33d6d96e91298ed77878a9e59cefe29ca0 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddRectFilled(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single,Inno.Native.ImGui.ImDrawFlags) 6be43676109615dbc96bbd9b51cc2981f9a06d4d5dd1b6dcd6e2735b1e8df242 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddRectFilled(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 9e99fce3c0f82093cb33e81c61849c78f91e3753791b84b98f4ffb7e598ecd2a -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddRectFilled(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single) 3750dee304fdf1f6d80f6c26e6a6ad1fe9705d1d3600d1002274a9853ac62153 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddRectFilled(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,Inno.Native.ImGui.ImDrawFlags) 314e127873521297ae04a8af36293fda9cf032aa507031285d021f7afe486610 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddRectFilledMultiColor(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.UInt32,System.UInt32,System.UInt32) a497f145a52a503cf6f7d2f2204fb83fe466a64def3b856a7f3a620305dce764 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddQuad(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single) fc577dbe56fd9d6556bca179ac25b4a67b524a99e2c741c044b2f59ed6e66912 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddQuad(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) f33b47611560f243e5aa261ed9e7397de3f7018e09ac94e1ecce9fd869fd6239 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddQuadFilled(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) e446480087b529d91f6b93da1f6003a69a932e8d2114c507e11ecadc130d535a -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddTriangle(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single) f3b9372524a680107651fd4003886c133f74361cb9dcb493b88bca0dd2f10576 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddTriangle(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 5d00bd6a9db5e201df9b158c5282c411a8b0b847446814fc2272a8f16f8c4266 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddTriangleFilled(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 13cc95afbc7c012e89d060ad43d0be74568e6f322bd57458cfc36025fb699724 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddCircle(System.Numerics.Vector2,System.Single,System.UInt32,System.Int32,System.Single) 7b41114e69e14f73b6d403d99eb459a20908246a4985929bd94bd8b86d272e95 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddCircle(System.Numerics.Vector2,System.Single,System.UInt32) f4fb00a3641e1bd92a414fee4457ad76c961c9e277a76e3182d9220509933bc7 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddCircle(System.Numerics.Vector2,System.Single,System.UInt32,System.Int32) 573fb493b892693760663fcfc1f6f1e17131b72ccb1717fbb05b3f72dd2687d5 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddCircle(System.Numerics.Vector2,System.Single,System.UInt32,System.Single) 9d637a21aa948793ec20958e9219d8e8044ec2de63e65d315de3b7e68f36c4de -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddCircleFilled(System.Numerics.Vector2,System.Single,System.UInt32,System.Int32) fa98cfbddcbecf057f09e8c98f37926fb38e0b29fef34fa5498e17a92cc75e8c -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddCircleFilled(System.Numerics.Vector2,System.Single,System.UInt32) 8436c941d65dadfdcb10f801d60fd62545a68c5a6185109094e4447341befa31 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddNgon(System.Numerics.Vector2,System.Single,System.UInt32,System.Int32,System.Single) 67704cc3d70dd07c73cea131e1de15fdfb464fe629a8c83daf1383118be4790e -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddNgon(System.Numerics.Vector2,System.Single,System.UInt32,System.Int32) 2b39c2de9d2a84c2e2655abda3faaa408d52573389bfb8537a87f4a89f89b58d -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddNgonFilled(System.Numerics.Vector2,System.Single,System.UInt32,System.Int32) 5966ad7b111501ecdfe365b753572f0d96954094957590995136e45f440bf3f6 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single,System.Int32,System.Single) 6d4e66b509b5481b87482d44f597a543b84892f7fc79fefb6bb43607373eb87d -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) d428017cd62eb9ab77d67b2ceeb3b0895e00079ae5eda6a9346cd696334557de -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single) d74500a4a4aa6ad6712949746ce28d4e11cbb25140aa24d3a7f165dadcac0c7d -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single,System.Int32) 2b4363df0e3081a6ad4615ae0139684eae949edd636eaab7efba1dcba6b93fcc -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Int32) f49f36b237874bb6b77ab558b6faa1b2808680c906ac16914cee6de4939b2b0a -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single,System.Single) a6cf9f9b2be3f3f8ffd1205aa9498a170d82a98b0cb949419a83b79d1344d35d -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Int32,System.Single) b3301bb505e3abdab25cd456c0d045293c94b0220844fa46d6a155f2fc448dc2 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddEllipseFilled(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single,System.Int32) 099017530623754685bcc4d7fb8e70e69c2a398082e57052099365cd47958139 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddEllipseFilled(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) d6e59212acaaa7885e1744b2db96a3ec2465fcceb98c386472979a75a943b41c -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddEllipseFilled(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single) 97b6603d6b4562af50534a84650d052db9338ce31213a0564f91ff2219b67f51 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddEllipseFilled(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Int32) 0f511df974e2994c673e735f1b3a30da06a43eb55b696e328a438d20666304a5 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddText(System.Numerics.Vector2,System.UInt32,System.String,System.String) 9a5f7c00f2a023b66013d7e81009336d8c6586a3312580073b1eaf58e4932819 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddText(System.Numerics.Vector2,System.UInt32,System.String) a97a132d942b460f613027b841dc072bce852d7c1623c0c2f4e4aace811fe1ba -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.String,System.String,System.Single,System.Numerics.Vector4*) 2e1d996499fa0d786cb6e49ea80e9f6d6365749ecad0105d7e0bf2c5314c9939 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.String) 0ef32887c7f9742db63ac104efc44dc3b9820d6e95d5420e5dc85ce05cc06973 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.String,System.String) 1f636a4b88ca121d3b14f39ece7702c45b0bcd047774182cc52b84076f2834cd -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.String,System.String,System.Single) e0157be9eb08c432917948feabf636788a0d168c725b63a36b8803194f87a5af -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.String,System.Single) ff419eba22f2405c06eafc43f6200506df40368bdf96ec8862990373ffe33bb8 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.String,System.Numerics.Vector4*) 4fb65d44bffbf7077790b4e3568e4cc5cfe444760a51ad716332c74e42726e14 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.String,System.String,System.Numerics.Vector4*) 658bd9393a8e9e523c837199b024b2e21d3594526d5e0d6122ab6e1c92f9aeb1 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.String,System.Single,System.Numerics.Vector4*) 279bc72771a078a9d4801c39cdc72a443a17a9ee74ae796cea903384a64350eb -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddBezierCubic(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single,System.Int32) 40b8b79c288da888efd2fb0b47ed045330f0c5c79e44b584734874ae490873e0 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddBezierCubic(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single) d19860c5dd46135c4fd9b196e023155f0128e9dcb7d40f2eb897f539981acc05 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddBezierQuadratic(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single,System.Int32) 81635c6a0d29bffcf6dedb03783f0003ab2dd0912d98c335dc09153fc6b76056 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddBezierQuadratic(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single) 1c0524cf96cda535e4d33ea561afd384bcb516efb3510728e6f273f944815009 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddPolyline(System.Numerics.Vector2*,System.Int32,System.UInt32,Inno.Native.ImGui.ImDrawFlags,System.Single) 75e0d8f4e7d27014e55a9a764f114bbf18c46357e22ad5f773ed8aa26888f607 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddConvexPolyFilled(System.Numerics.Vector2*,System.Int32,System.UInt32) bb15bd056b9fb35b2879e80b4ef62bb83eae72957a5af86ed5d348af3fd38577 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddConcavePolyFilled(System.Numerics.Vector2*,System.Int32,System.UInt32) 17dc4bfe866333f364a7232bf39c589da2e01930faf619f07100df84bb92f64b -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImage(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 8a6f91138fec6c45606f52ab170b6f9f24796a16583b3b6fc418a8d172e3b306 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImage(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2) 2bb5c502066f2849cb841b11781844be5fb76d6aea7b25bd545a8a37ce3ed866 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImage(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2) e87dc7988cfcac9f36b9e8681f6dff82976bd2f0e722930db37815942861f4af -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImage(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2) 2521731ec705ced04c098373209de7797a5dbb955c4c24509c372b0c596d6d03 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImage(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) e28d28df6247044707f3bb34426430f5dbf5a841d86e49fb64f2e12c52ad31c5 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImage(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 82e775ab0893d8abc933ffcb97bc3e49bdb56af2290f0504d9efd791e937c7eb -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 5128cbfecb0ac27b5f4bd0e1dfcfdef51a9bfcdf48397da502ced6b2c62ef0ff -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2) 9e7b61ce8b90eb5b0a2ce0785ec075d3faa01b80469021bca05c2749b6f8749b -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2) 361fe46ce864cff1a45e9e91c9a44815c0d3e4d36ff9fbb948e69edaf4946a09 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2) 20ded24548097fe815acfc7434ff798a7176e3dc5d2c2350ca4d50da5e30ffb3 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2) 03e168db47c7e0bc1c9302449403d72bac85da3f23ae03955667c1a6838fad7c -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2) b74c9022b120b1898d5698e265ba0b0f74aed53ccfd427977fcf5fae8795034c -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 5b99b386da8ffb6e3c76bf9993633d1f920376feaafcbcbbc667b6f498073142 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 0befacff64d39824af9221134b35e85835a46cd6c20097c87dbbd6bbcf729652 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 9a2a9742ae0e3d615fdb2a2f45c84f4e37b04dd7289b62bf3fe1cf3e30e87e99 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) bf89aefeda09ce838acc7ad02c64ccc282047b21e702ceea471e137c22ef9cf5 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImageRounded(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single,Inno.Native.ImGui.ImDrawFlags) 7199913d4fb01abe597b11f79436d6dccc08c9ec15dcf24a52d794bcd3ee2e25 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddImageRounded(Inno.Native.ImGui.ImTextureRef,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32,System.Single) 77f4799163a99939503098dfab3b87f7a5ca127c66946b788e7fc89213ed4690 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathClear 402eb5fdd824fb8fed97c1f44795e542e73072754beee444a0556a31f151b34d -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathLineTo(System.Numerics.Vector2) e0d2c341f20a9771f84a220361cf315a0014a97324af9336f96c7c40688c7920 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathLineToMergeDuplicate(System.Numerics.Vector2) c7de4dd7980b88e0e855c022ba0096652482e02bb5f2a8a58df40a0b641047f0 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathFillConvex(System.UInt32) 1a6835dec2a27dc0b46eea11d3cf5e5f5e7b1fa6cae50d179f487431ad8440ab -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathFillConcave(System.UInt32) 784328236019f78fbe3bc4c24611c595da0852c664a56f6d6357973255bd42e5 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathStroke(System.UInt32,Inno.Native.ImGui.ImDrawFlags,System.Single) fff309cae5ca05a014b36c382c08efef254b565b77366f54a32f26eb71317c99 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathStroke(System.UInt32) e5a5b2ef507d848574ef9edaaedeb3af3ff95ef2da0c77d4edb396812bf4b295 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathStroke(System.UInt32,Inno.Native.ImGui.ImDrawFlags) 83cf3c7eb0959c872fb8d28e2a5daca1f31cc3f43eea01f0edf4f9d640c84406 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathStroke(System.UInt32,System.Single) 9f41e1718e998445ef7fb7652a6b7f28568ece8bff24e71b58a72f62d92451c4 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathArcTo(System.Numerics.Vector2,System.Single,System.Single,System.Single,System.Int32) 8ad50fed5b1ba06e0a28096266342ee79aa65655729d96fb539985c46cafcdb7 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathArcTo(System.Numerics.Vector2,System.Single,System.Single,System.Single) f8187bd36ab146362b96021f4cf637a476c00d9dc43f34e58d0fa36f08a52212 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathArcToFast(System.Numerics.Vector2,System.Single,System.Int32,System.Int32) 0e50475333ffcc6549cb32d2326f0fcbed050b0b7e87915f8589a8200b000ef0 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathEllipticalArcTo(System.Numerics.Vector2,System.Numerics.Vector2,System.Single,System.Single,System.Single,System.Int32) b4cf86fde45fe75a18f1aa49e66e0aa64cfb8898776da68fc5f0476d1af5c373 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathEllipticalArcTo(System.Numerics.Vector2,System.Numerics.Vector2,System.Single,System.Single,System.Single) b5e103b8af4b8ed1c8b37ebb074dbe43675c51439857652ec09a8d7d87678b22 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathBezierCubicCurveTo(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Int32) c18274a03a9773cef210772d1967677f8883953857f28ffc544d95eede5dfb8c -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathBezierCubicCurveTo(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2) b15d3d5c485706a751723c4a0975b0236c2a2c43d129798b0816a8117a021961 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathBezierQuadraticCurveTo(System.Numerics.Vector2,System.Numerics.Vector2,System.Int32) 966061db22008904f6f1e45360cd70301601b7a2a73027d12b91f4aef44ad335 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathBezierQuadraticCurveTo(System.Numerics.Vector2,System.Numerics.Vector2) 0312b8d8300201e06fcc27209ee5685c0f001c0e67f33fa48cbf3e2d61ad3314 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathRect(System.Numerics.Vector2,System.Numerics.Vector2,System.Single,Inno.Native.ImGui.ImDrawFlags) 6557dfb054712dbc86c8aa1fc78b7306c892aba0b99b5f7561dc8e4b0f04b5a3 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathRect(System.Numerics.Vector2,System.Numerics.Vector2) d5ea0b49a0b7585b1e580c92021e03e7fc68fc23bc3de0b29e3836d62c12940a -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathRect(System.Numerics.Vector2,System.Numerics.Vector2,System.Single) b25cd6991eb03c618f9bb6025b9f2040e48e3ec50d890a38412baae57a6aa8fe -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PathRect(System.Numerics.Vector2,System.Numerics.Vector2,Inno.Native.ImGui.ImDrawFlags) 3e8fb78e41c787a396cf66d29df2b15f2ae7afda0324583cc6c8354d5057cb39 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddCallback(,System.Void*,System.UIntPtr) ce8ba89bce9a85cd3639ac0b5a50aec80c910da99898195dbe0e7ef9a41e0a70 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddCallback(,System.Void*) 9bf5634a87e04ff71deab95413ce49281374caaabf4b0d27aeef8a9d7a65ceb6 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.AddDrawCmd 9b9dafe91c350e284691add8465b15f6eea7542ac6a654745c66c435b5de3e67 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.CloneOutput f9c042c5c959f1395291f0c02edc21b9105ea83e36d158e36ca26f25ee143c8d -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.ChannelsSplit(System.Int32) d43a999f9419465816de084c867200acf9b9dbf4eda45cd1f6d37e548b46b65c -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.ChannelsMerge a9d60eaf705754a75be9df2ca4979236e40624f0187270d8011b10ccfc79a44c -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.ChannelsSetCurrent(System.Int32) fd38be92fe821018a0bda01f45a41a0e39a417988f8315395a6d4b2c6ade846b -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PrimReserve(System.Int32,System.Int32) ca64442b28dadb37cc18bd9262a3aca06cf18c8c728e03d7c4f685166f258e7d -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PrimUnreserve(System.Int32,System.Int32) 4796d90a75b7a92b3aeed3e2547d712d4b9d98be6a8dfe31e3a15caa6d79436e -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PrimRect(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 2f47e6e6b80edf5c6df32c3f45c34a5436e3ae89ff2d190b4434d99c6d1487bd -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PrimRectUV(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) d0d080571b10d482fe7d8a2a5660763a5234b0394db0d32f8a73151ae9c74610 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PrimQuadUV(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 2e0f5374f1d4aa40339ce2b670e7e13b4d545f423baf957ef4d731d89700d90f -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PrimWriteVtx(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) fd2af392caf8a22f3141fcd1d2ddbebf4a651b8389e8a1c8b86fae69e3a01453 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PrimWriteIdx(System.UInt16) df528d51ff84b0f07b779c14e5ee060a4cda9fbb6d25bc3065e153bae119eb07 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr.PrimVtx(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 15600d147430be1b0581eea6f4570c1470c4fbe2bda61392386a408881d8f260 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr._SetDrawListSharedData(Inno.Native.ImGui.ImDrawListSharedDataPtr) 697633afd16781ad322f3c5b1b70e51add04a132fc3643622604dafec3bc9174 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr._ResetForNewFrame 52263e8e0a2eb130f16be8344ed106eafae515ebe2b6ddf7dd3f778921411ea7 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr._ClearFreeMemory dfe386ff0fd95d3470a18c5b6b81855f9c72895f87c4ff6ecd662e5f0aea1df4 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr._PopUnusedDrawCmd 06f4c828807944f662a80b38a98b8492c749bffb6e053bcd6d6f807802d4207b -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr._TryMergeDrawCmds cfa47dd53b5ade20db72a492d77a5acd10f79364bbe1e0a86b7ddca0f129ed03 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr._OnChangedClipRect a3254ad00ee6e5227510c923655debb5e1dbfaf75eafcd348576f7d52dd8a680 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr._OnChangedTexture 26e35c3c6498f8996095ef1b6e383784efa24463c49df44bac4774f5ca9a8449 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr._OnChangedVtxOffset 581e5e2a3d9b3ccddbbbd11fddbf254470c0073edbadab9d6e029f3a830d95ad -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr._SetTexture(Inno.Native.ImGui.ImTextureRef) b6013570dd686329df6a44cfd0acd5cfe75cce60b4462d1f0ef1f378fae0448b -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr._CalcCircleAutoSegmentCount(System.Single) 704b0a196ff1f5a617e4a96ef1bfedfff4f8949792ed65c49f8228570206866b -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr._PathArcToFastEx(System.Numerics.Vector2,System.Single,System.Int32,System.Int32,System.Int32) f0775f36cc359f83b225cc0e4182066c62ecacace678695b68aad407d4a149dd -->
<!-- api: M:Inno.Native.ImGui.ImDrawListPtr._PathArcToN(System.Numerics.Vector2,System.Single,System.Single,System.Single,System.Int32) 6edd4a4af2cdf8c82def095abade931b15f1a3d464460c02b31f5301e972ab16 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2dfd33160cb73d45"></a>[`public Inno.Native.ImGui.ImDrawList* Inno.Native.ImGui.ImDrawListPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10959) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dceab37921e7a0a8"></a>[`public int Inno.Native.ImGui.ImDrawListPtr._CalcCircleAutoSegmentCount(float radius)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12033) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c71da685bdaee383"></a>[`public void Inno.Native.ImGui.ImDrawListPtr._ClearFreeMemory()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11977) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f4fbd5e48f2f9c1d"></a>[`public void Inno.Native.ImGui.ImDrawListPtr._OnChangedClipRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12001) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4accb07fd651e2d4"></a>[`public void Inno.Native.ImGui.ImDrawListPtr._OnChangedTexture()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12009) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-46cd8571b610a3da"></a>[`public void Inno.Native.ImGui.ImDrawListPtr._OnChangedVtxOffset()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12017) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-958404285ba1ee10"></a>[`public void Inno.Native.ImGui.ImDrawListPtr._PathArcToFastEx(System.Numerics.Vector2 center, float radius, int aMinSample, int aMaxSample, int aStep)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12041) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-244b97647e413d90"></a>[`public void Inno.Native.ImGui.ImDrawListPtr._PathArcToN(System.Numerics.Vector2 center, float radius, float aMin, float aMax, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12049) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6f6ed1ccd05315ce"></a>[`public void Inno.Native.ImGui.ImDrawListPtr._PopUnusedDrawCmd()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11985) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-102ab4eb11d73a00"></a>[`public void Inno.Native.ImGui.ImDrawListPtr._ResetForNewFrame()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11969) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-54942ab8d3d3afae"></a>[`public void Inno.Native.ImGui.ImDrawListPtr._SetDrawListSharedData(Inno.Native.ImGui.ImDrawListSharedDataPtr data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11961) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-881bc631cabca6ea"></a>[`public void Inno.Native.ImGui.ImDrawListPtr._SetTexture(Inno.Native.ImGui.ImTextureRef texRef)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12025) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3b72f9d5cb2cd2d3"></a>[`public void Inno.Native.ImGui.ImDrawListPtr._TryMergeDrawCmds()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11993) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6b22407c5d58b616"></a>[`public Inno.Native.ImGui.ImDrawListPtr.ImDrawListPtr(Inno.Native.ImGui.ImDrawList* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10954) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8d15db529d11e404"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddBezierCubic(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, uint col, float thickness, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11465) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-346ae249dc29193c"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddBezierCubic(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, uint col, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11473) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-059c4ffcd5195014"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddBezierQuadratic(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, uint col, float thickness, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11481) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d4737d4b48d21430"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddBezierQuadratic(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, uint col, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11489) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-41e6b9793a1a10ae"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddCallback(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImDrawList*, Inno.Native.ImGui.ImDrawCmd*, void> callback, void* userdata, nuint userdataSize)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11841) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-953ff9abc0881bb6"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddCallback(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImDrawList*, Inno.Native.ImGui.ImDrawCmd*, void> callback, void* userdata)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11849) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5d0df5fd7ea9d3ac"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddCircle(System.Numerics.Vector2 center, float radius, uint col, int numSegments, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11225) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-79a1400df5cdf61f"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddCircle(System.Numerics.Vector2 center, float radius, uint col, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11241) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e4b40d453893e0ef"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddCircle(System.Numerics.Vector2 center, float radius, uint col, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11249) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-af37a1d29ef5503a"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddCircle(System.Numerics.Vector2 center, float radius, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11233) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f2a99d73475fdafe"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddCircleFilled(System.Numerics.Vector2 center, float radius, uint col, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11257) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0c222a48495d9c6a"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddCircleFilled(System.Numerics.Vector2 center, float radius, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11265) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-978b64dcf05f7d45"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddConcavePolyFilled(System.Numerics.Vector2* points, int numPoints, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11513) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-593eb161b0088d56"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddConvexPolyFilled(System.Numerics.Vector2* points, int numPoints, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11505) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-176895b9442bed4e"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddDrawCmd()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11857) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-52d63982d7779f96"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2 center, System.Numerics.Vector2 radius, uint col, int numSegments, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11345) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d317ee2c5f8165f8"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2 center, System.Numerics.Vector2 radius, uint col, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11329) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-be96bf5a3f993ea4"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2 center, System.Numerics.Vector2 radius, uint col, float rot, int numSegments, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11297) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a0c5498ffd9694f8"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2 center, System.Numerics.Vector2 radius, uint col, float rot, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11321) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f7ced66c3755e593"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2 center, System.Numerics.Vector2 radius, uint col, float rot, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11337) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8ff0be7656ad0e9b"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2 center, System.Numerics.Vector2 radius, uint col, float rot)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11313) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b291d8e529c023bc"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddEllipse(System.Numerics.Vector2 center, System.Numerics.Vector2 radius, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11305) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-02c34ebaa4e47719"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddEllipseFilled(System.Numerics.Vector2 center, System.Numerics.Vector2 radius, uint col, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11377) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c7b68eb53a699aac"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddEllipseFilled(System.Numerics.Vector2 center, System.Numerics.Vector2 radius, uint col, float rot, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11353) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e257f9372851b71"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddEllipseFilled(System.Numerics.Vector2 center, System.Numerics.Vector2 radius, uint col, float rot)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11369) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b43b29cb0fb1512d"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddEllipseFilled(System.Numerics.Vector2 center, System.Numerics.Vector2 radius, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11361) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b388427edbbb0860"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImage(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, System.Numerics.Vector2 uvMin, System.Numerics.Vector2 uvMax, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11521) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f8487889e19803bf"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImage(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, System.Numerics.Vector2 uvMin, System.Numerics.Vector2 uvMax)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11545) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-445b70e8fb0ff3d4"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImage(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, System.Numerics.Vector2 uvMin, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11561) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-128afc1d8c69d7a6"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImage(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, System.Numerics.Vector2 uvMin)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11537) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2ad1d777ee3136e7"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImage(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11553) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-221a2b00649ba9ca"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImage(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11529) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-95092954a3d3f9b6"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, System.Numerics.Vector2 uv1, System.Numerics.Vector2 uv2, System.Numerics.Vector2 uv3, System.Numerics.Vector2 uv4, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11569) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b17e3b51c91f9483"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, System.Numerics.Vector2 uv1, System.Numerics.Vector2 uv2, System.Numerics.Vector2 uv3, System.Numerics.Vector2 uv4)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11609) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-97e7f9ee2039330e"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, System.Numerics.Vector2 uv1, System.Numerics.Vector2 uv2, System.Numerics.Vector2 uv3, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11641) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-72a7086550b66f46"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, System.Numerics.Vector2 uv1, System.Numerics.Vector2 uv2, System.Numerics.Vector2 uv3)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11601) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-879e309269f1b7a1"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, System.Numerics.Vector2 uv1, System.Numerics.Vector2 uv2, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11633) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2d0f8067c7e51dab"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, System.Numerics.Vector2 uv1, System.Numerics.Vector2 uv2)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11593) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b0e82b5b496f4758"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, System.Numerics.Vector2 uv1, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11625) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f6043a87dba6d5d4"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, System.Numerics.Vector2 uv1)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11585) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-382941f0f56fbd38"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11617) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-26459d2af14ab01b"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImageQuad(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11577) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dc5280d2cc534bec"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImageRounded(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, System.Numerics.Vector2 uvMin, System.Numerics.Vector2 uvMax, uint col, float rounding, Inno.Native.ImGui.ImDrawFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11649) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e6ebae0270c9935"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddImageRounded(Inno.Native.ImGui.ImTextureRef texRef, System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, System.Numerics.Vector2 uvMin, System.Numerics.Vector2 uvMax, uint col, float rounding)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11657) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-034b9da842a8ac84"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddLine(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, uint col, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11065) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1ba5f4dc90ceaaa9"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddLine(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11073) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5701e77c3ebddf36"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddNgon(System.Numerics.Vector2 center, float radius, uint col, int numSegments, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11273) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-416903bc5be35ca8"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddNgon(System.Numerics.Vector2 center, float radius, uint col, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11281) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7c0bc312818070c3"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddNgonFilled(System.Numerics.Vector2 center, float radius, uint col, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11289) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-21eba1b1a479332f"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddPolyline(System.Numerics.Vector2* points, int numPoints, uint col, Inno.Native.ImGui.ImDrawFlags flags, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11497) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-465e2a635aab3aae"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddQuad(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, uint col, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11177) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5d49b8a31fe096a6"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddQuad(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11185) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3e1cd7c22910cff3"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddQuadFilled(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11193) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-34a0894fd028ee93"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, uint col, Inno.Native.ImGui.ImDrawFlags flags, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11129) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8c0e36627e408517"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, uint col, Inno.Native.ImGui.ImDrawFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11113) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-73a5584e90de3f1c"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, uint col, float rounding, Inno.Native.ImGui.ImDrawFlags flags, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11081) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9b5bd1e21340a1d8"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, uint col, float rounding, Inno.Native.ImGui.ImDrawFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11105) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-90f7655317ab7ef0"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, uint col, float rounding, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11121) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2663d19ea41a2167"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, uint col, float rounding)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11097) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1db01bc35bbaa67d"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddRect(System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11089) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2cd599eb8641a91e"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddRectFilled(System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, uint col, Inno.Native.ImGui.ImDrawFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11161) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9a7a878af47381a5"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddRectFilled(System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, uint col, float rounding, Inno.Native.ImGui.ImDrawFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11137) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b773d20e0de404c6"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddRectFilled(System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, uint col, float rounding)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11153) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fb63b101a65cd478"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddRectFilled(System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11145) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e1f8d19f96553830"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddRectFilledMultiColor(System.Numerics.Vector2 pMin, System.Numerics.Vector2 pMax, uint colUprLeft, uint colUprRight, uint colBotRight, uint colBotLeft)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11169) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-18c179a7deb4d803"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr font, float fontSize, System.Numerics.Vector2 pos, uint col, string textBegin, System.Numerics.Vector4* cpuFineClipRect)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11441) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f5c2455fa36298ed"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr font, float fontSize, System.Numerics.Vector2 pos, uint col, string textBegin, float wrapWidth, System.Numerics.Vector4* cpuFineClipRect)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11457) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-27f528ac8e0757e0"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr font, float fontSize, System.Numerics.Vector2 pos, uint col, string textBegin, float wrapWidth)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11433) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-821b195c1cd64e31"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr font, float fontSize, System.Numerics.Vector2 pos, uint col, string textBegin, string? textEnd, System.Numerics.Vector4* cpuFineClipRect)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11449) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b0e8d5915efb9f54"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr font, float fontSize, System.Numerics.Vector2 pos, uint col, string textBegin, string? textEnd, float wrapWidth, System.Numerics.Vector4* cpuFineClipRect)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11401) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b49605a818d4227a"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr font, float fontSize, System.Numerics.Vector2 pos, uint col, string textBegin, string? textEnd, float wrapWidth)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11425) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ebb9ac366ed64b9d"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr font, float fontSize, System.Numerics.Vector2 pos, uint col, string textBegin, string? textEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11417) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6705c461a9eb8f1d"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddText(Inno.Native.ImGui.ImFontPtr font, float fontSize, System.Numerics.Vector2 pos, uint col, string textBegin)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11409) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cdb3ae05fb90a7f7"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddText(System.Numerics.Vector2 pos, uint col, string textBegin, string? textEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11385) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-11ba9779a222221e"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddText(System.Numerics.Vector2 pos, uint col, string textBegin)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11393) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-433bc64f6278320e"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddTriangle(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, uint col, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11201) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4211777846a58e43"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddTriangle(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11209) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-88fc1d97528944dd"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.AddTriangleFilled(System.Numerics.Vector2 p1, System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11217) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2ac45704b97f34ad"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.ChannelsMerge()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11881) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-691f4d96619957ca"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.ChannelsSetCurrent(int n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11889) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bd895029d25127cf"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.ChannelsSplit(int count)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11873) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5f447283ea079b31"></a>[`public Inno.Native.ImGui.ImDrawListPtr Inno.Native.ImGui.ImDrawListPtr.CloneOutput()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11865) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cf9c12ff0bd54b35"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10993) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e0cb5deee1018f40"></a>[`public bool Inno.Native.ImGui.ImDrawListPtr.Equals(Inno.Native.ImGui.ImDrawListPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10969) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-c3b2d7b912cf1373"></a>[`public override bool Inno.Native.ImGui.ImDrawListPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10970) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-8fa45597d1997a08"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListPtr.GetClipRectMax()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11057) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2724f22e49fae9f6"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListPtr.GetClipRectMin()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11049) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1c437e62371c3c09"></a>[`public override int Inno.Native.ImGui.ImDrawListPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10971) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-2b59b5068bc07759"></a>[`public static bool Inno.Native.ImGui.ImDrawListPtr.operator ==(Inno.Native.ImGui.ImDrawListPtr left, Inno.Native.ImGui.ImDrawListPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10967) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a3f28ee6078f5240"></a>[`public static Inno.Native.ImGui.ImDrawListPtr.implicit operator Inno.Native.ImGui.ImDrawListPtr(Inno.Native.ImGui.ImDrawList* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10965) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2cdabe6ebf01a9bd"></a>[`public static Inno.Native.ImGui.ImDrawListPtr.implicit operator Inno.Native.ImGui.ImDrawList*(Inno.Native.ImGui.ImDrawListPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10966) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-14ea6a9216b8e061"></a>[`public static bool Inno.Native.ImGui.ImDrawListPtr.operator !=(Inno.Native.ImGui.ImDrawListPtr left, Inno.Native.ImGui.ImDrawListPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10968) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-24f2b4c02655dab4"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathArcTo(System.Numerics.Vector2 center, float radius, float aMin, float aMax, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11737) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-197a0c4d6fec4914"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathArcTo(System.Numerics.Vector2 center, float radius, float aMin, float aMax)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11745) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7132f34e83fef580"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathArcToFast(System.Numerics.Vector2 center, float radius, int aMinOf12, int aMaxOf12)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11753) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b634a900a88cdc68"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathBezierCubicCurveTo(System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11777) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e9dface4a2471904"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathBezierCubicCurveTo(System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, System.Numerics.Vector2 p4)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11785) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-facb1a46df08435b"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathBezierQuadraticCurveTo(System.Numerics.Vector2 p2, System.Numerics.Vector2 p3, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11793) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-93490f97dfd465f4"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathBezierQuadraticCurveTo(System.Numerics.Vector2 p2, System.Numerics.Vector2 p3)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11801) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-538fa69ed7d21341"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathClear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11665) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-38ee40447d059590"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathEllipticalArcTo(System.Numerics.Vector2 center, System.Numerics.Vector2 radius, float rot, float aMin, float aMax, int numSegments)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11761) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-03cc06ad4199d2e4"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathEllipticalArcTo(System.Numerics.Vector2 center, System.Numerics.Vector2 radius, float rot, float aMin, float aMax)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11769) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-38b31fea2e3759a8"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathFillConcave(uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11697) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1b621c354a556642"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathFillConvex(uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11689) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a29f5ec654b2d6ea"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathLineTo(System.Numerics.Vector2 pos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11673) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e9f23f038589f0d8"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathLineToMergeDuplicate(System.Numerics.Vector2 pos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11681) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6d7dd0b011bbf61f"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathRect(System.Numerics.Vector2 rectMin, System.Numerics.Vector2 rectMax, Inno.Native.ImGui.ImDrawFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11833) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ae048cbf7ac0f58"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathRect(System.Numerics.Vector2 rectMin, System.Numerics.Vector2 rectMax, float rounding, Inno.Native.ImGui.ImDrawFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11809) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a037b0090cb2f47e"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathRect(System.Numerics.Vector2 rectMin, System.Numerics.Vector2 rectMax, float rounding)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11825) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b110c6586b1cf167"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathRect(System.Numerics.Vector2 rectMin, System.Numerics.Vector2 rectMax)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11817) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-584de46782b28e16"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathStroke(uint col, Inno.Native.ImGui.ImDrawFlags flags, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11705) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cc3a378a4db0db82"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathStroke(uint col, Inno.Native.ImGui.ImDrawFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11721) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e453af46f081b901"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathStroke(uint col, float thickness)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11729) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-43b6aa37c7a9df2d"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PathStroke(uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11713) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9ef79984b46ecb3d"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PopClipRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11025) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-11b51ce62bdb6402"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PopTexture()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11041) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f2fa75c6b7f15915"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PrimQuadUV(System.Numerics.Vector2 a, System.Numerics.Vector2 b, System.Numerics.Vector2 c, System.Numerics.Vector2 d, System.Numerics.Vector2 uvA, System.Numerics.Vector2 uvB, System.Numerics.Vector2 uvC, System.Numerics.Vector2 uvD, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11929) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-60ccd814c43c2654"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PrimRect(System.Numerics.Vector2 a, System.Numerics.Vector2 b, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11913) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fa777d715b980c2e"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PrimRectUV(System.Numerics.Vector2 a, System.Numerics.Vector2 b, System.Numerics.Vector2 uvA, System.Numerics.Vector2 uvB, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11921) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-db5f2ec7e3a5da46"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PrimReserve(int idxCount, int vtxCount)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11897) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5371208eb7728190"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PrimUnreserve(int idxCount, int vtxCount)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11905) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8a1b7f2200c216aa"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PrimVtx(System.Numerics.Vector2 pos, System.Numerics.Vector2 uv, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11953) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c600447c9542dd9a"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PrimWriteIdx(ushort idx)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11945) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7954883a39dc726d"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PrimWriteVtx(System.Numerics.Vector2 pos, System.Numerics.Vector2 uv, uint col)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11937) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0c675c6d4c277d9c"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PushClipRect(System.Numerics.Vector2 clipRectMin, System.Numerics.Vector2 clipRectMax, bool intersectWithCurrentClipRect)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11001) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c3c4084a7b1d7e31"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PushClipRect(System.Numerics.Vector2 clipRectMin, System.Numerics.Vector2 clipRectMax)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11009) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f90bb9ba4744da5d"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PushClipRectFullScreen()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11017) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8238e5dea1f8fa1c"></a>[`public void Inno.Native.ImGui.ImDrawListPtr.PushTexture(Inno.Native.ImGui.ImTextureRef texRef)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L11033) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-34afb8c41956b03a"></a>[`public ref Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImDrawListPtr.CallbacksDataBuf { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10986) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-85b5b062ee432ac2"></a>[`public ref Inno.Native.ImGui.ImVector<System.Numerics.Vector4> Inno.Native.ImGui.ImDrawListPtr.ClipRectStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10984) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-191f98c630a13df1"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawCmd> Inno.Native.ImGui.ImDrawListPtr.CmdBuffer { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10973) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8f7e9c376288523f"></a>[`public ref Inno.Native.ImGui.ImDrawCmdHeader Inno.Native.ImGui.ImDrawListPtr.CmdHeader { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10982) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-41a19361d4fa6141"></a>[`public ref Inno.Native.ImGui.ImDrawListSharedDataPtr Inno.Native.ImGui.ImDrawListPtr.Data { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10978) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f252c3456d545913"></a>[`public ref Inno.Native.ImGui.ImDrawListFlags Inno.Native.ImGui.ImDrawListPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10976) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ad00308600cd7582"></a>[`public ref float Inno.Native.ImGui.ImDrawListPtr.FringeScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10987) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1230c78970896172"></a>[`public ref Inno.Native.ImGui.ImVector<ushort> Inno.Native.ImGui.ImDrawListPtr.IdxBuffer { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10974) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-13ba94c760f40c76"></a>[`public ushort* Inno.Native.ImGui.ImDrawListPtr.IdxWritePtr { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10980) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9d140336f59806f3"></a>[`public bool Inno.Native.ImGui.ImDrawListPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10960) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-64b36933fa6a13d8"></a>[`public Inno.Native.ImGui.ImDrawList Inno.Native.ImGui.ImDrawListPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10963) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-08b50ae89d3275d7"></a>[`public static Inno.Native.ImGui.ImDrawListPtr Inno.Native.ImGui.ImDrawListPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10961) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f97c6413c22bb518"></a>[`public byte* Inno.Native.ImGui.ImDrawListPtr.OwnerName { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10988) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e83c8d638f9a89b8"></a>[`public ref Inno.Native.ImGui.ImVector<System.Numerics.Vector2> Inno.Native.ImGui.ImDrawListPtr.Path { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10981) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d11397e7e624a60f"></a>[`public ref Inno.Native.ImGui.ImDrawListSplitter Inno.Native.ImGui.ImDrawListPtr.Splitter { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10983) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c0f3d3a60e1b4bdb"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureRef> Inno.Native.ImGui.ImDrawListPtr.TextureStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10985) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0349f0e2ea896dc7"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawVert> Inno.Native.ImGui.ImDrawListPtr.VtxBuffer { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10975) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3a93d484b1215be4"></a>[`public ref uint Inno.Native.ImGui.ImDrawListPtr.VtxCurrentIdx { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10977) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dfe58a3d8b659507"></a>[`public ref Inno.Native.ImGui.ImDrawVertPtr Inno.Native.ImGui.ImDrawListPtr.VtxWritePtr { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10979) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3662637de890c243"></a>[`public struct Inno.Native.ImGui.ImDrawListPtr : System.IEquatable<Inno.Native.ImGui.ImDrawListPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10952) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImDrawListSharedData</code> · 133 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImDrawListSharedData 588426ecb5c761ad27c9c21bff597f4cef5b45808098c0182d6cc61101e1bb2e -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.TexUvWhitePixel 262f947827171fec6f60e9e130e8b4c6329bc5b680c2974673bc802bed00f476 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.TexUvLines e35ecae75cefb146750342d4aa5e663ff2f5e7b80b2eb3609d6cba2869ceb00a -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.FontAtlas ef3be8108c30c3edf6aa32267023b40e16fdace71dd30720f2f75aca33eb9a42 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.Font 1aa61d1e10dd19276413c8a094c777447c55c49476581a07aa2cb4a52e106912 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.FontSize 6988cb2315be316908084c58ca0f9d6deb83b4f5e157987254492d2cdc7b9ba4 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.FontScale 18e7df49b1bf40e478bd644040873af0aebe6e52a87f184e5852f9bd858e1683 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CurveTessellationTol 4cfa2e512473438281943fe7dbf81498060992c38b628c603b23cd36d14d5b98 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentMaxError 6d52d190eb24ab50dd9ba7560a6f901c53b851f8fa55ae12e3ff7690bd1341b1 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.InitialFringeScale 9f755bbebafa5c19e06cc70101cabe514fcd6a42d9e286466be3d3caf1672c88 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.InitialFlags 7f0dd0afc92f59ebb2869e2e701ab8ac4de488122516329f8ef5f8aa8116d15a -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ClipRectFullscreen 6d5d46e892f4da1dc96e5d61d808aed10a23a7f60853edfc4839e5b7b45a021e -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.TempBuffer b0e647f92e4d2696dbe8cc91b1eca83ea05e9804b4f326f29d69c8afe4681311 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.DrawLists 940126943b0ace4418447284ba2954e7aec8c0cf1329ec7190f397e9269c6bbe -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.Context 46ac9253761a623781fbf6f4861f2df67e4b24ef50db4caceefd58ec77226385 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_0 dd73430166241619037b03cd04b3bc87705e3d2e571aaf42860e668f22b86ff3 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_1 9cef473ddd8de5fb349fed91e5456aa4db8677cd27a6815581d8affe72f5fd3b -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_2 7d8dbbddfdca800ac72a7c8126f94fa04180550d171799e89d1bd96069f08191 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_3 7556b2d1430cc545ff5e6dba81dd93a0c97701512e6f4c8a7b8d6e4a84e15f68 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_4 123c694a9c7a3e5ee28ceca0e789609eb9d3fcfa0ca179027850bd22f0db5f9d -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_5 96a3110b6a3b505e848f18ca5cd3fef1547953de19d9ece330e4e1e7b346ae19 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_6 76b11fef4fd1e5cae6323e6564660b5901afcab8295f55971be7d2b382c53b62 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_7 e2c75a9d25c9f2176d7b681fb6d82e03240c8c53565350b0ee5f665a4381b424 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_8 e34172bf58dfef745128fbae310a2d077453c877303e9ece8df85cff7e82c601 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_9 4cedc5077ed09aada9f28917125b383ade43484f9c4a97a89aa34c5706d81103 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_10 900dd523d372971c9463043db2c0a14bc2036176fbe8b55151fccd86d0a75931 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_11 e8c5fea40cbc9862790ad1fd6719878be13e109038c9480eb6528eb4b4ea8eb2 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_12 ed3482d2fd213b5bd00b9e4f7fd251d2e384f51d9c56008017039d78a4cb8fbd -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_13 bac82620782ae8c63c24e52a2a4101ee659ecceebf9ecda89c233221b5527b86 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_14 27a790064f077041a3bb9586da20070b1bdb5a9fde643afe3427276ba34ba7c9 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_15 7dbe87b41d2aacb79987726f5686370863e2e92e5eb0e918691d3a288ffa60d9 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_16 35f926ea307f8dbffd24df24b8184413e67c18c2789cd16756652e9e172cf8f6 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_17 7f271602ca3102550d676bc86bc16ac31b67b5dff7da37ed73cc4917cbbf37c1 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_18 90e4a3d98a8e9998501a2d561e44c3b19cdcb248b44497100431c2be8b0f46a7 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_19 6feeda6a1067af10e9c6db8532880886c511206c5f9b22c8bd0e74dbab7dedc6 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_20 ae522573639fc4ab0197ad84dbfd97689a532454ef8d2efac0e1d94366da1753 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_21 5da22248a333a2fa50c3ea411ee25ee27b312885c705d54eab84736ee1c61258 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_22 c43b4d900ef78031613ae2c0f1089481369b5eedadf6b8c0140e2189f3838307 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_23 325a17b32ad3174fca8de1802fb993a93e9ad1ec6865cbade30b640ad8c38bf4 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_24 1cde4736c6df9cb68f75f61c1614f8b53fd906004df2a0aafba0f5c1d30ea5eb -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_25 b94d83bb0757ad94a3b36a3eb3d770537098771c7ce87a59c237da25fcdd4671 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_26 a73fbffb237d297e0971cb7a5a78a3d5d3686bccc0e2f1997c6fed60baceb6da -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_27 da1c922cf98c391f42d76e8d6de00975b5b08be3b55af35945a8c01e5bb11515 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_28 6b13d58e664c703be8a1965c5c8ed1cf6794d9f55328729807bf59bc427012ca -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_29 dc315c2d81508ab09ec4e44b3c7e72187683f51122c7915c9ad71c204cfeafc7 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_30 d9ba07c238e520c8fa2adee81e50acc2ece7b9e22e9cf15629b8b631dde094d4 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_31 201cfdef8b9bf84e5c7e050ec117e55b43c377e8abb8b7c3e502b31d735de864 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_32 388e0c7d11619a057a668cd7510bc8a64ba8966a51b0978b8ebcfa2b650eeaec -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_33 8e5c877ee298636c146f8c25e9a335b6e7de22b8b31a5aab3414ccdbe7b30eda -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_34 87505c890d83c5b7c00ca19de2f51c72102a0d090330c6dfd470ac18dbd847db -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_35 4e4c292af896f88a691840ec90671b8293f0cd2f4e4f5fc2eff6273158b168b8 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_36 a2342e5ad356b048aaafc1d0e791336176b9e83a2179304de6cc44762a7c4a22 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_37 5765a98e574c89ab511d6622f867ce148a44261d55587769d1d2bc3a24fade84 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_38 bd4a90888deaed1b69fb5ed0e2b178af9a3217f90e05ea19fdb3a61b668856b9 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_39 3965b5fe733730d860ca61d7204922b174c97d93e3bfd5bca8f741aaa16349e8 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_40 ef28cd24dcd26c6600687bb4a03a7a5e1b8971ddeb45f9acdff344829430b601 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_41 7a80ee2081166aba56f2a37d7c9b3d7aee9c7ea862bcc7657bad50ff8e0a9aeb -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_42 120d5e30d1d750e7acbfafa4a113c79e0fd2148f05360ceee907d74b0818f7c2 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_43 b0a7c43705d92e6e8cff6eb8e57e08d6a060409ddafb47a350b1ccc999c63b2a -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_44 ba430b19fbb4c314e3b93b766b5caeb9e4078c20003a60fa4f85e1ed0ee1c22a -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_45 25b7786a75918cbdb5e15aea791b21e9983e908beb4c6c54fae701e1f5940bd3 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_46 45f3403ecf967e312dcbfabe567352b404c129c7ab0e47eb7da962b1f41e19fc -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_47 b2e6efb6dc92a9d4027eb052321e9247588b8b90e7442117ff8f29f3e4beff81 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx 01327948589548e11732949ebb1a070d06cbb3c1d48fff8ac7d005c0543e075f -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.ArcFastRadiusCutoff 3e8bf9b90e3d36b16b0aed90390cb338b6c36cfce231c6c53c510e779e343534 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_0 bfbf1f2cbc50ca699ef55a4bdac3d39f57cc4cc5debcee56e020114a82845422 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_1 46e5d046a796940eec4a79e99bcad7e2171b0afc3fc1f1b0e2d352dd47479461 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_2 5ccab47a5a98d44f5da2fe6120ca04418e582b9b41ba134cc4d6b223ad8362d4 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_3 30b52da207cd148fe5e6a06a77580d9907eb0a04da29ca85d352bcd68955840e -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_4 ab93953fdc4dd13c509df06d9e2b60aa05787e1d05df32f70fc645b08dbddcfa -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_5 70975a301a58a1affc47143c2f7998355b0d3f89a94f8a7c117a8ef45148a9d8 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_6 0ef3af314e6aac8f6a854f8501bcb55fe3a7f22ec8342f0af301126a29a9cd3e -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_7 336381ff8f64bc6c1e89740af4365d56c76893ed9aa3af2a21c29e45425ec7b8 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_8 c401b0409119ce60459e290ca252b6d95903e09c0f9205b01afcf77fd719bf40 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_9 9f7a2d51efff22b7b89a4894d67a2bbd7fd180a50cf615ca5fdadc6e6d5a12ec -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_10 c042dc9d2682a0cedec781c35e8338efbf0f8f78988a895494d0cbf169a02e2a -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_11 73d1a58e55ff65366affb3636dd7e291487e4c1fc811b3e8529fdcbf2166dfe8 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_12 fdf982141020496fae808aa3e3b892439f4c496568026edc8a8f709825cfe6ac -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_13 3e27dcbedbc43aafb2424b1868a13318e40126a95048e477f267d62c15e6aeaf -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_14 8807bf54894086feeb6fe4119bfc0409ac2467804aac916b08d9d12c4a7a2570 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_15 5cd02f8e3c64ee9e618fb780c41ff6290e79e77d08f5786a9db515fe8daf3ddc -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_16 1e9503ad40a9cd492af95651de08c220bfac8f9ec8d0b6930b3650738b5617b6 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_17 85b9c4315c9a9a5f476b99ec130e5d57be8a7d8e87395536c3d989de2c4015f2 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_18 8c50dfb9445e39d56dda8e4383ff1eee94edfdf067239a7b42c581447a83b85d -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_19 c849be124aa1f9eb3abf710c73a655ec0ec474c8e47b7c093e545b4090884011 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_20 3485bc249fc3c48618f3349f9b1d50b7bb14bdbcd706f3101abf8fc32b9ff5b6 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_21 c787acb3bafdae634866037cdc870206a0a159493d2cecc5f7860df60e0d04f7 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_22 57d59943626092d8bb7857123d81d5662418aaef5b1af3f22cf770f6978f789d -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_23 7ae252b4a8b441a0520bd59b9f99a4bd562a2596a6a10c541ead237986094264 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_24 b6c1d0294427f0e294b2796c27cb54369c45babfdd7c75edd527884d40260a54 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_25 6f4f4177c031c14a30941cddf1cedf8ad0485106897089607f8fb3de3dd0da43 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_26 0ef8c7b90570153423eb96247089017ef15fa4872f58fc3d5297ffa64cf0146b -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_27 08894bc62b5e1fa877af8a6b50acf19ef87a7ee3489fb4492b5843e018eda6c3 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_28 b7f73dc6b13a8570061319cee1d678b8e64ca63c7118a8f0533a7169f0f66af4 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_29 941a58da63fb1c646863daaad5ae8b77a5c26098b822787928407b0b602cc6f8 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_30 759cc18927adcf20db463e8e16bdc073aea2aad40639fe71e6d080882fbd0741 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_31 945a186c4e37f6f6d7ff3365ef3ce3c39cb10491fab70da8877199270ca7f5ca -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_32 dd726873737bd1dd557f44c7894a70269f4073cc2e71269cf09b86db02b4cba5 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_33 1f76120b66fe16d9a5be0a196f1e656c952a510eb440452a5cd36f47c8cc575f -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_34 fe7e2388113199441e1cb77b4471f449b22361ad68feb629234d44dd9ab04d29 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_35 162521b9e6bce69ea919e9b750f346cde7e6c0cbb628a868e6355d7e829fe659 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_36 62fe91e64689f54a716bd5133d8ab43e399685a091681cc0a62888e860a6d740 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_37 26502e9155d16ba6404143631f92eb8eec98338551ff772e80020464736d0dff -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_38 dde3192681fbd24087dfe7f2c819a2ef850544c7d3b629d2f494ebe600822a15 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_39 7f17ca29f7458109787b1dcc92edd7792bf51e2848fc168d54dd6e2993d0aada -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_40 7f71e7b3605d54df2b250a433a41a40241234c4a6054bf68adede1ec02b9a810 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_41 57909a8cb2f9ba0773f8d684b96e6fb010f451025a7850f0a70113553409e36d -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_42 f660919db623465efde54d8085d5a34e088a8e335e3b62d006d80919208fe445 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_43 8071a2ec166377b42c4c1dd52001c753405fd18b670b8d517e53156fa9e2e88b -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_44 e4edb9cb7833222eb612e4d155520c437613577892adc1905dbf766f49058178 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_45 ab69aadfe109d2912632c092648a21dde9b5b5800216829a127707393fff25bc -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_46 3ea6905877c347021b6068ece6d1fed96f7d4b0b592362ef75b29c6eafe047c0 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_47 ab1bd32e0dbd79ec3eb8c95989c1d599748cc14a1ab02a128a4b668ca279d7df -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_48 67d633d36a0f38d23279d419a4abd121bd2a9e07d8eb6a1d464efed7567ffe96 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_49 2fd8ecdf1773cdbf2f9350d42f7f5b3f058152ba4f0990c4e667c93bf5630f73 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_50 79a3aa2b0df199c5b1350130faa05ade2df117a28b266579c4b290ef9031a1dc -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_51 071c48e725bd428b5ee604889d92c904993c14c56e1efc165ca705d3ffd7be35 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_52 0b9c23a92692c1f62b8cf285aab1e09c7181952745faeb501955be3c796edae0 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_53 777c55abd56a02dbb45c59d83f817c079288d1805df07eee47da6b3bd197cf3b -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_54 65a2319d52966d881d9c0fd6de432ed5956d1ba4460af2582d1b9e83bca2976c -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_55 5555928bcd8d314f6f781b50813eab8c115cf5333d97fea4654efa33887e4048 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_56 966b496d6c97c71ffc4a719b3be0725a093f458bae3e43b6343197f49e55fd91 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_57 001dd72c586d98f110f35f6ae19db1b7b8416c2912d16a1b4bafc2cd4aec6628 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_58 90f75267506ba50facd6b6b7e2ccc43f98214892ea3a7a38f753ada11064c28a -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_59 92e6dbaff4a27d6e70d55aa66600e8a00e5aeb66594beba72e247fe942866352 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_60 ae42ca528bffbbc44fa9bb29ce47ef06ef94abad76065d84da3c3eb58fe19322 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_61 cff0041d58aad2295fadc1e5462f277aa1a8b113f0e6f9d4040056c2e6eee741 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_62 3609d38b17b7dff9060f1e7a660f061a8e05c3a9021da94841cc2b665c0c1ddd -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_63 4562f9e65ae16f236208517c9b630cf5486f7e645dd348c9d4200c58c2eaa548 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts d6381bbe96404d88df12d9be61eaea1c2125bf45ca939f2380a7ab9292e7050a -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSharedData.#ctor(System.Numerics.Vector2,System.Numerics.Vector4*,Inno.Native.ImGui.ImFontAtlas*,Inno.Native.ImGui.ImFont*,System.Single,System.Single,System.Single,System.Single,System.Single,Inno.Native.ImGui.ImDrawListFlags,System.Numerics.Vector4,Inno.Native.ImGui.ImVector{System.Numerics.Vector2},Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImDrawListPtr},Inno.Native.ImGui.ImGuiContext*,System.Numerics.Vector2*,System.Single,System.Byte*) afd638ab10d70e6db345b0b55a6eaf468ab4a3b606b968d7c5f52d0c04a7ccee -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSharedData.Destroy 2a61524b0aeeea5ab6b13b61e4e6adc8e5997febf2cc5d7e60b2c40c59ed61c1 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSharedData.SetCircleTessellationMaxError(System.Single) 581f14ed47edab58da65ebd6028937c3560d30df16015104f9436d5471420e3e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-70d959d094e448a1"></a>[`public float Inno.Native.ImGui.ImDrawListSharedData.ArcFastRadiusCutoff`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9134) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-860b8924230763ae"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9084) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7d8d4900ce58c18e"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9085) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f97fd9da71ba5f6c"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9094) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-802ee376c69b8356"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9095) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58c2e6819cf96563"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9096) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-efd2e5fd104fca5f"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9097) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-96d9437a1513d275"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9098) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2b4c3007eb0bc227"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9099) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0db4143cb9be8315"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9100) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-864ad3d5df6c982e"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9101) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-134e2b482f8f791b"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9102) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a74f4eb1f51365f1"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9103) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f3aec76c61df4dc"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9086) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f8a9342e9b66448b"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9104) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df91e320d2798bb9"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9105) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-397c9306711c1e6a"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9106) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d791168c2f6c5f0d"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9107) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3401418f4358cbec"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9108) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a624fe296f4d95b"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9109) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e544775205401843"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9110) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-518ec75f5cd253ac"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9111) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-43e2ce8ed5af08b9"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9112) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-84cdd7b25d907743"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9113) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c815be130bae14f"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9087) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d723cda4d5e42488"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9114) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8318d5d4fe38c762"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9115) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-353912bf79a33c95"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9116) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d75757f6f5ecb5b4"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_33`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9117) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-17a634fc1de3c8a0"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_34`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9118) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f23eb29d1ef3efd3"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_35`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9119) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-43adb5e318bf6e33"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_36`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9120) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9e0716817a437dc1"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_37`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9121) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21e8d2270eaf08a2"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_38`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9122) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-980d3bdd51304910"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_39`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9123) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d3baa5558666520"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9088) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89c7a8c3286375b4"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_40`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9124) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-476e9bad227077c8"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_41`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9125) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eb626a568c9501fd"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_42`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9126) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0566643e7bacfce1"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_43`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9127) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-52795c38e13d6b8f"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_44`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9128) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f571c38109ed149"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_45`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9129) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0139b43ac364e1e6"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_46`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9130) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5bdf4ea2e8c7dcfe"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_47`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9131) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2381bf4b673dd5ae"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9089) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-97c5c71ea863313c"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9090) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6aa5304efebf0e27"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9091) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-188fff3f6648055b"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9092) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20f46fa32b1c2777"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9093) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cbeb58f09c7dc422"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9135) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a95e2c37f3b446c5"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9136) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f06273e143e2ed61"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9145) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fa019c0f7ecaa0cc"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9146) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1cb2c98cce61a944"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9147) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2cdf17697050e70c"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9148) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d3efc4f14874a134"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9149) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60b869a5bf2ef690"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9150) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2674fa301b9289b4"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9151) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4ab59bb83cdbe37d"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9152) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cdf8f74a5378fbf5"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9153) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-43ca28fd364a167f"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9154) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fc560d56019ebc7b"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9137) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ac60128d12cdab15"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9155) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-43c9b03fe0c86984"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9156) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6161e45f76069fc4"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9157) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e634005020154b76"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9158) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1a8255be3ae0a935"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9159) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-98f24d93d0710cf6"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9160) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89da214d7f6b8daa"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9161) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5ca852b6f3ab6588"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9162) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-021b10d496983258"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9163) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-257a8c92305157e4"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9164) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-36e65d9dd5f3f5e0"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9138) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-deb73b3a6cc80b71"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9165) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d9690ee0f8b27b1"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9166) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a5325a64d39a038c"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9167) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f89f6349b96142e5"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_33`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9168) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4ce7b3dc6608763d"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_34`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9169) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0238ee2e468e7626"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_35`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9170) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8f2582b150f4d339"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_36`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9171) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1b4f81acc2e5c79"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_37`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9172) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d25208f7407476b"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_38`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9173) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4c845afb43c63cec"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_39`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9174) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bfec67d918a52c2d"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9139) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9947819002d6e5eb"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_40`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9175) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6affa39c3a4d4542"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_41`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9176) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5975010049f73804"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_42`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9177) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-610310cce9e398e8"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_43`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9178) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-408b6d66675e9815"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_44`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9179) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2821af342ce3e23e"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_45`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9180) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-046c99ca5766e8cd"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_46`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9181) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-81d8d4e28c8f9f4c"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_47`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9182) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad058c63bf46b27f"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_48`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9183) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c1c147f498e5d663"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_49`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9184) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11355a20693c3e41"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9140) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d6e5d34eead1beb"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_50`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9185) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-18d2f82f45d8aca6"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_51`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9186) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-668c27d9d12fc9c1"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_52`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9187) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7394e81f00fc7b7c"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_53`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9188) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ba12bdfd321f122"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_54`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9189) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fbf33a8f561656ad"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_55`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9190) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f2b6eb558f5219b"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_56`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9191) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-16522b4aa570bdef"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_57`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9192) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-17ab3d418c201081"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_58`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9193) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-36912eb8a0a9d847"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_59`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9194) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b44c4e516beedfd4"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9141) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7577c1994ef858f"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_60`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9195) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a5a7471081354b6e"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_61`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9196) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cbf61e7147f1ef4b"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_62`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9197) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c57c6d45c8d685f2"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_63`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9198) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de5ed7cd79d638a1"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9142) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d64488bfc9ffb769"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9143) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8de88047f5a69500"></a>[`public byte Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9144) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b1ece1c7ed540868"></a>[`public float Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentMaxError`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9077) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-55f74dd87949fdf3"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImDrawListSharedData.ClipRectFullscreen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9080) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5ef8d61824e051fa"></a>[`public Inno.Native.ImGui.ImGuiContext* Inno.Native.ImGui.ImDrawListSharedData.Context`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9083) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d0bbfb3c415dc779"></a>[`public float Inno.Native.ImGui.ImDrawListSharedData.CurveTessellationTol`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9076) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85fc0832070db3d9"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawListPtr> Inno.Native.ImGui.ImDrawListSharedData.DrawLists`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9082) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e28f1de758901a49"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImDrawListSharedData.Font`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9073) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-990b844d157eae0f"></a>[`public Inno.Native.ImGui.ImFontAtlas* Inno.Native.ImGui.ImDrawListSharedData.FontAtlas`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9072) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-02afdfac2de93b9b"></a>[`public float Inno.Native.ImGui.ImDrawListSharedData.FontScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9075) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-00682ca2aa4e2596"></a>[`public float Inno.Native.ImGui.ImDrawListSharedData.FontSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9074) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58033bec04e705ca"></a>[`public Inno.Native.ImGui.ImDrawListFlags Inno.Native.ImGui.ImDrawListSharedData.InitialFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9079) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7bc704503688c95a"></a>[`public float Inno.Native.ImGui.ImDrawListSharedData.InitialFringeScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9078) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-33e6e747fc42d009"></a>[`public Inno.Native.ImGui.ImVector<System.Numerics.Vector2> Inno.Native.ImGui.ImDrawListSharedData.TempBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9081) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c032d7e5ed4aa9f8"></a>[`public System.Numerics.Vector4* Inno.Native.ImGui.ImDrawListSharedData.TexUvLines`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9071) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c70b1b12948e17fb"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedData.TexUvWhitePixel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9070) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2263f813e2e4cf34"></a>[`public Inno.Native.ImGui.ImDrawListSharedData.ImDrawListSharedData(System.Numerics.Vector2 texUvWhitePixel = default(System.Numerics.Vector2), System.Numerics.Vector4* texUvLines = null, Inno.Native.ImGui.ImFontAtlas* fontAtlas = null, Inno.Native.ImGui.ImFont* font = null, float fontSize = 0, float fontScale = 0, float curveTessellationTol = 0, float circleSegmentMaxError = 0, float initialFringeScale = 0, Inno.Native.ImGui.ImDrawListFlags initialFlags = Inno.Native.ImGui.ImDrawListFlags.None, System.Numerics.Vector4 clipRectFullscreen = default(System.Numerics.Vector4), Inno.Native.ImGui.ImVector<System.Numerics.Vector2> tempBuffer = default(Inno.Native.ImGui.ImVector<System.Numerics.Vector2>), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawListPtr> drawLists = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawListPtr>), Inno.Native.ImGui.ImGuiContext* context = null, System.Numerics.Vector2* arcFastVtx = null, float arcFastRadiusCutoff = 0, byte* circleSegmentCounts = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9201) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3fc42a83ee23df1b"></a>[`public void Inno.Native.ImGui.ImDrawListSharedData.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66377) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aa866c922671ab7a"></a>[`public void Inno.Native.ImGui.ImDrawListSharedData.SetCircleTessellationMaxError(float maxError)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66388) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-53949229e073e283"></a>[`public System.Span<System.Numerics.Vector2> Inno.Native.ImGui.ImDrawListSharedData.ArcFastVtx { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9132) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4b251cc411c9d977"></a>[`public System.Span<byte> Inno.Native.ImGui.ImDrawListSharedData.CircleSegmentCounts { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9199) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9b1b2f62e51a387b"></a>[`public struct Inno.Native.ImGui.ImDrawListSharedData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9068) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImDrawListSharedDataPtr</code> · 32 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImDrawListSharedDataPtr fde7841a6082d607cb18c48c74f9aa14f1abdc36d9cdc3687102a62427cbcddd -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSharedDataPtr.#ctor(Inno.Native.ImGui.ImDrawListSharedData*) 8ff3eafb03aaf8f71af2a0d7a5d9d1f2f084279da6d7a3290abad713a5c7902d -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSharedDataPtr.Handle 733dbbb83dfec759eda5c08d25a00d0ff32060d5811f907db83ec46cf7575ee2 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.IsNull acd4a910900d69582e7fd831f84438de8f5a24e97f9c44ae0ad15a246645527d -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.Null c80eeea8265b0402fb68b12e7dfde9618d8821a031f82397cacdd75063b79843 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.Item(System.Int32) d3c2e32cfba15b828b49d9c769cbdb685063454f9287a0f51694817d7723a9b8 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSharedDataPtr.op_Implicit(Inno.Native.ImGui.ImDrawListSharedData*)~Inno.Native.ImGui.ImDrawListSharedDataPtr 2527eab892f07cf697004015a14c514dd560b6eb3081617f54fcbe486fae8b65 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSharedDataPtr.op_Implicit(Inno.Native.ImGui.ImDrawListSharedDataPtr)~Inno.Native.ImGui.ImDrawListSharedData* ff522da291bc39e1db79bceaa02a08c6b705eb70067d73881860a3775d75f686 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSharedDataPtr.op_Equality(Inno.Native.ImGui.ImDrawListSharedDataPtr,Inno.Native.ImGui.ImDrawListSharedDataPtr) b9e4cd89dffce0482cb4f34a220b0d5c13f8bd4402decb252fc64c0722025f69 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSharedDataPtr.op_Inequality(Inno.Native.ImGui.ImDrawListSharedDataPtr,Inno.Native.ImGui.ImDrawListSharedDataPtr) 1326d3fbb31d1b9be6cef36eda0ccc0f11728b81f1e938d5a9d50aae86fb5f94 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSharedDataPtr.Equals(Inno.Native.ImGui.ImDrawListSharedDataPtr) cda5f3f12ba59c5e5e538b6fd732bc0a70e13b94019f096476c8ee0cf7519753 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSharedDataPtr.Equals(System.Object) f20fc85f1efd5765cb030a7c8ce4503d292923ae36fc084cf40ed0c7522c053a -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSharedDataPtr.GetHashCode e6065088bcbbefcc5181815b2dc2997fec745ae3003a1d0059961e0b9d97ca3c -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.TexUvWhitePixel 7d956c0eddbc3d47be6008aae3026192b5c4c212e56f374e700a11b98d8ada9b -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.TexUvLines b1d60459602d71da0747e8b9ff118233e9b829c1b354b93d7a4c18af21b3d24f -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.FontAtlas d8b2db76119ac787ac5b0aeb56005d6beb07ccecbc3415aac15716917e3b8736 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.Font b1046ac7288339ddf0458554eb6ce29fd30f580d9f40b796b4ae6e6112ea6b83 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.FontSize 1f4d187b850e67d514a8b893ff496d6ec8504807e8e0475bddbf52fbccde55b5 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.FontScale 6358aa38bde28d6d1dcff5d29a74cb7595ec9f58b51439b2121a226583090bc1 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.CurveTessellationTol 335307b25e9b0985d085a9ede576dd67e176ce47986adcfd0e82f6dd944b8ba8 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.CircleSegmentMaxError 3f29b936864a9b9a41f182a17fa2e01f40af7992bf2466aba47729c5ce74565f -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.InitialFringeScale 80385815ed84792133a44829b16a809c217c1d4961d3793005d71390857e6e2d -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.InitialFlags a0dfdf2587d745b4101c460a14f4bf0a134f32f576a8c9b3db4ca49260b725d7 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.ClipRectFullscreen 7c9a340aad8853a0e1b4ceb09391b717b98e9aed4d39ab0c43500db312db662c -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.TempBuffer f381b9e6ce63070ce42518e5213447b1ca28606570a7661197d7a3cf9ef8421c -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.DrawLists b2deb01f4f45733eb98b0ea0dea3ba686215562a2f972c8c311448221a00d277 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.Context 1c069ccbe9f77c051b68fdfc452e541e73d02029dfec5316a680619c8febb383 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.ArcFastVtx 8bb0fb2c702957f977c1da6479749cd79ab54717a20f5fcd8be3d0987356c22b -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.ArcFastRadiusCutoff 33a91ef51f9859f22d6e8c0962e3537394551e2566100ccc6ec4e41866f23c50 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSharedDataPtr.CircleSegmentCounts e66d259f26177388d478fb5cdef8df70d2efb373a0a33b860b36c1f715f5039d -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSharedDataPtr.Destroy 7ba2362e8282bce41b618a4182036f2c05130dd071f3867be0576d8bb394bed5 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSharedDataPtr.SetCircleTessellationMaxError(System.Single) 1b09e0e944e3985efd16e174084520055899b054c729f2eb7a2e1d3fcecfaa38 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-35b296dbbc46195e"></a>[`public Inno.Native.ImGui.ImDrawListSharedData* Inno.Native.ImGui.ImDrawListSharedDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12063) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d402bb17413b394"></a>[`public Inno.Native.ImGui.ImDrawListSharedDataPtr.ImDrawListSharedDataPtr(Inno.Native.ImGui.ImDrawListSharedData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12058) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0248abb7143e29d2"></a>[`public void Inno.Native.ImGui.ImDrawListSharedDataPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12098) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2be5e63fb12f122e"></a>[`public bool Inno.Native.ImGui.ImDrawListSharedDataPtr.Equals(Inno.Native.ImGui.ImDrawListSharedDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12073) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-3b3835f66fde390e"></a>[`public override bool Inno.Native.ImGui.ImDrawListSharedDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12074) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-ab856a1101991099"></a>[`public override int Inno.Native.ImGui.ImDrawListSharedDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12075) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-7d939bae73c05330"></a>[`public static bool Inno.Native.ImGui.ImDrawListSharedDataPtr.operator ==(Inno.Native.ImGui.ImDrawListSharedDataPtr left, Inno.Native.ImGui.ImDrawListSharedDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12071) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4f65a15f69626d37"></a>[`public static Inno.Native.ImGui.ImDrawListSharedDataPtr.implicit operator Inno.Native.ImGui.ImDrawListSharedDataPtr(Inno.Native.ImGui.ImDrawListSharedData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12069) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5cbbd0568c04d476"></a>[`public static Inno.Native.ImGui.ImDrawListSharedDataPtr.implicit operator Inno.Native.ImGui.ImDrawListSharedData*(Inno.Native.ImGui.ImDrawListSharedDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12070) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-32662ada1f36aad4"></a>[`public static bool Inno.Native.ImGui.ImDrawListSharedDataPtr.operator !=(Inno.Native.ImGui.ImDrawListSharedDataPtr left, Inno.Native.ImGui.ImDrawListSharedDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12072) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-70e1a9008f7dec9b"></a>[`public void Inno.Native.ImGui.ImDrawListSharedDataPtr.SetCircleTessellationMaxError(float maxError)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12106) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-629c949e192d9062"></a>[`public ref float Inno.Native.ImGui.ImDrawListSharedDataPtr.ArcFastRadiusCutoff { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12092) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-14bc53a125b37d18"></a>[`public System.Span<System.Numerics.Vector2> Inno.Native.ImGui.ImDrawListSharedDataPtr.ArcFastVtx { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12091) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-986b084d67093487"></a>[`public System.Span<byte> Inno.Native.ImGui.ImDrawListSharedDataPtr.CircleSegmentCounts { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12093) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d907425d02480899"></a>[`public ref float Inno.Native.ImGui.ImDrawListSharedDataPtr.CircleSegmentMaxError { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12084) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e552be64c6e25ce1"></a>[`public ref System.Numerics.Vector4 Inno.Native.ImGui.ImDrawListSharedDataPtr.ClipRectFullscreen { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12087) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b851308e94ae8914"></a>[`public ref Inno.Native.ImGui.ImGuiContextPtr Inno.Native.ImGui.ImDrawListSharedDataPtr.Context { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12090) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-681ced6e08747f65"></a>[`public ref float Inno.Native.ImGui.ImDrawListSharedDataPtr.CurveTessellationTol { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12083) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dc8900ac3ba58851"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawListPtr> Inno.Native.ImGui.ImDrawListSharedDataPtr.DrawLists { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12089) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f1d24b521097ba28"></a>[`public ref Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImDrawListSharedDataPtr.Font { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12080) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-438e248cdb69d157"></a>[`public ref Inno.Native.ImGui.ImFontAtlasPtr Inno.Native.ImGui.ImDrawListSharedDataPtr.FontAtlas { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12079) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d796389cfd6c733d"></a>[`public ref float Inno.Native.ImGui.ImDrawListSharedDataPtr.FontScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12082) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c7cca0c2bb151f3d"></a>[`public ref float Inno.Native.ImGui.ImDrawListSharedDataPtr.FontSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12081) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-267985446736a689"></a>[`public ref Inno.Native.ImGui.ImDrawListFlags Inno.Native.ImGui.ImDrawListSharedDataPtr.InitialFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12086) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6fb8f503122c56da"></a>[`public ref float Inno.Native.ImGui.ImDrawListSharedDataPtr.InitialFringeScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12085) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-41fd670ebce9c55c"></a>[`public bool Inno.Native.ImGui.ImDrawListSharedDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12064) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cdb72a57a89550bd"></a>[`public Inno.Native.ImGui.ImDrawListSharedData Inno.Native.ImGui.ImDrawListSharedDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12067) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-883690d8818d92c2"></a>[`public static Inno.Native.ImGui.ImDrawListSharedDataPtr Inno.Native.ImGui.ImDrawListSharedDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12065) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-757569d064bad87c"></a>[`public ref Inno.Native.ImGui.ImVector<System.Numerics.Vector2> Inno.Native.ImGui.ImDrawListSharedDataPtr.TempBuffer { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12088) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2c4fa7c6c6ce4465"></a>[`public System.Numerics.Vector4* Inno.Native.ImGui.ImDrawListSharedDataPtr.TexUvLines { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12078) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f9806b7e23b2cee5"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImDrawListSharedDataPtr.TexUvWhitePixel { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12077) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5d3f59d5738f2b04"></a>[`public struct Inno.Native.ImGui.ImDrawListSharedDataPtr : System.IEquatable<Inno.Native.ImGui.ImDrawListSharedDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12056) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImDrawListSplitter</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImDrawListSplitter babd776b3d8f0cbf6d2d3ff9b448e7b4255a55ad4edd762b7633d5c9066414e4 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSplitter.Current 87d8e83bdffd8bca61355e667caaa0a6ad73ff0dd6658742747dc308a66db424 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSplitter.Count f70c6add03bb4804cc4d7494c164dccd2d02ef271a6ac55b659ea5b882680a63 -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSplitter.Channels 43babf640e5876654834c5829088eb8f286fb446f18e074514e70e48c903907f -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitter.#ctor(System.Int32,System.Int32,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImDrawChannel}) c9efde2b3d9999bcbbc6b6445bfc47ddd5055c36c9fdb6532b131fe72eae2460 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitter.Destroy d2eee85424681858e7e55d21b7f483ff134102ef2576d053516443cf20f742ee -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitter.Clear 781cfa656ebef6aecb4d43f8bf24264ed8b8b49f2b299daa827c73be42af74da -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitter.ClearFreeMemory 5238d03d3545f4bb7b8bbdcb523d313e27b487bca63bd13ed02c2e075604a17d -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitter.Split(Inno.Native.ImGui.ImDrawList*,System.Int32) e35f418a06bf0f7fc0c0d2f9c102ca213e1229038b313e81aadb9457380aff2a -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitter.Merge(Inno.Native.ImGui.ImDrawList*) 204e773b338d7ef83fc831c4f80df59b65d18808ce294fe7638f386465dc7f31 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitter.SetCurrentChannel(Inno.Native.ImGui.ImDrawList*,System.Int32) a936b925180372e2a69fc3bf2262640b9a61c98d3d83fe49899130ec05978dad -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d679e8adcb67106a"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawChannel> Inno.Native.ImGui.ImDrawListSplitter.Channels`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4129) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c692b8bafdbca77d"></a>[`public int Inno.Native.ImGui.ImDrawListSplitter.Count`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4128) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-90e8e3244f6c66cd"></a>[`public int Inno.Native.ImGui.ImDrawListSplitter.Current`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4127) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-887122d6c721075d"></a>[`public Inno.Native.ImGui.ImDrawListSplitter.ImDrawListSplitter(int current = 0, int count = 0, Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawChannel> channels = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawChannel>))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4130) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9e544961b0039cfb"></a>[`public void Inno.Native.ImGui.ImDrawListSplitter.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64271) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e2e63f8bcb902e27"></a>[`public void Inno.Native.ImGui.ImDrawListSplitter.ClearFreeMemory()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64282) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dad826f632af85b8"></a>[`public void Inno.Native.ImGui.ImDrawListSplitter.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64260) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2a469a58ef207bb4"></a>[`public void Inno.Native.ImGui.ImDrawListSplitter.Merge(Inno.Native.ImGui.ImDrawList* drawList)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64304) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe6a0984be4dd56c"></a>[`public void Inno.Native.ImGui.ImDrawListSplitter.SetCurrentChannel(Inno.Native.ImGui.ImDrawList* drawList, int channelIdx)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64315) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f4798a8fad3cc52c"></a>[`public void Inno.Native.ImGui.ImDrawListSplitter.Split(Inno.Native.ImGui.ImDrawList* drawList, int count)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64293) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f5020eab5d972f0b"></a>[`public struct Inno.Native.ImGui.ImDrawListSplitter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4125) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImDrawListSplitterPtr</code> · 22 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImDrawListSplitterPtr c4d848b9bb6dae08310547badfc476814421313b3eed02b92760efca308eb91c -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.#ctor(Inno.Native.ImGui.ImDrawListSplitter*) a8dcbe45242c66c337af0a495451c15bcda4674ed04922517c6dd9a913d8a4ca -->
<!-- api: F:Inno.Native.ImGui.ImDrawListSplitterPtr.Handle 2424fae918f792847237360a13f244181718a6273f248d5fac8e7404331b7754 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSplitterPtr.IsNull 911a9e99db646990699f320fabb99232b44dcf8b0a937b6a577fb0bd92ceb6fd -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSplitterPtr.Null 5b2fff033400cbbee688b91ae4b5adcd4196c3ce6f527deb57a267652baaae14 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSplitterPtr.Item(System.Int32) 1d6c4c543c9af3658a6dfe2b4461cc29846fef8a41e23bf504b3a9f2d214abd6 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.op_Implicit(Inno.Native.ImGui.ImDrawListSplitter*)~Inno.Native.ImGui.ImDrawListSplitterPtr 0c789ccd485f36b940b5fb5de25df9dbd1a365fb908ffb3444f5802634b28549 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.op_Implicit(Inno.Native.ImGui.ImDrawListSplitterPtr)~Inno.Native.ImGui.ImDrawListSplitter* 2349382a4ecb3455404b9567fc108da93438cd51b574bd1752fc36cf5cb54efb -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.op_Equality(Inno.Native.ImGui.ImDrawListSplitterPtr,Inno.Native.ImGui.ImDrawListSplitterPtr) a17b1d2cfe3f98e2b35d0cf5af1e6903e10d2c6c0692c87637e771e784d49e6b -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.op_Inequality(Inno.Native.ImGui.ImDrawListSplitterPtr,Inno.Native.ImGui.ImDrawListSplitterPtr) 452929fd897bea3dedb5a4f2c9977de42dc0b53b2cb1828aa54e52859a026498 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.Equals(Inno.Native.ImGui.ImDrawListSplitterPtr) 6510ceec22573da76f6bbe048a2b33b87f38838a1a7f83898d06d0f9514afe6d -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.Equals(System.Object) b0738672f618c92277002776607f02754d69e9a955338d20c574581598234324 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.GetHashCode 715e2e4fe0f38c6cb10dcb99db8bb9876fa999300c8faa5b31b5b26c36dc7417 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSplitterPtr.Current 3b0d5505d9e20eb860f0c96939c319d23c03dbc868cfe1340fd8154902eab51c -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSplitterPtr.Count 62999f82b51b3000e3cb811d179dda6db1c05f4de425f1747c6bd803a3ea1ac2 -->
<!-- api: P:Inno.Native.ImGui.ImDrawListSplitterPtr.Channels 21d6f3cf085994dcc5cf1dc4a0c62f20f74a123ea4e8f0acf345b9b0920cc235 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.Destroy 1cc54df0de116adb640c7a57226a0fe23806c300363c3ef6d90129d2de1ba3ec -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.Clear 9df5628f8b08cb053f7bc22706f8dcc5575b8e018f3696c71a533741c12f16d1 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.ClearFreeMemory 7650c7d407facf595cd9b2a79ae1429924fbef2e7a25dbe861bb5ff85fd9ac52 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.Split(Inno.Native.ImGui.ImDrawListPtr,System.Int32) ea4308b8f684a523107225a09e8acfb320938baa10659bff66b36f28ea0151e4 -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.Merge(Inno.Native.ImGui.ImDrawListPtr) 47ecfe907a41714d854bbd5e6bd80de53b15a9ef7fee5f61913d0abb8819a6bd -->
<!-- api: M:Inno.Native.ImGui.ImDrawListSplitterPtr.SetCurrentChannel(Inno.Native.ImGui.ImDrawListPtr,System.Int32) 4bc586cf13db1e92db6283fce145b8ddc9361b577d4c392384795713d50abb22 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3755bec59cd3ab0f"></a>[`public Inno.Native.ImGui.ImDrawListSplitter* Inno.Native.ImGui.ImDrawListSplitterPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12120) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-134d91e2c34b00d3"></a>[`public Inno.Native.ImGui.ImDrawListSplitterPtr.ImDrawListSplitterPtr(Inno.Native.ImGui.ImDrawListSplitter* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12115) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7450901af7a396d6"></a>[`public void Inno.Native.ImGui.ImDrawListSplitterPtr.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12149) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-188e81918e5fbfcf"></a>[`public void Inno.Native.ImGui.ImDrawListSplitterPtr.ClearFreeMemory()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12157) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c62a9e0690e4a18a"></a>[`public void Inno.Native.ImGui.ImDrawListSplitterPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12141) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3b7a9a3b18954174"></a>[`public bool Inno.Native.ImGui.ImDrawListSplitterPtr.Equals(Inno.Native.ImGui.ImDrawListSplitterPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12130) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-689a432fd986fb3c"></a>[`public override bool Inno.Native.ImGui.ImDrawListSplitterPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12131) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-be86404f7f7dc6be"></a>[`public override int Inno.Native.ImGui.ImDrawListSplitterPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12132) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-ef5dd918f7fcd143"></a>[`public void Inno.Native.ImGui.ImDrawListSplitterPtr.Merge(Inno.Native.ImGui.ImDrawListPtr drawList)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12173) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dd1b846fbf61b28c"></a>[`public static bool Inno.Native.ImGui.ImDrawListSplitterPtr.operator ==(Inno.Native.ImGui.ImDrawListSplitterPtr left, Inno.Native.ImGui.ImDrawListSplitterPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12128) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5a30a75522c01ce3"></a>[`public static Inno.Native.ImGui.ImDrawListSplitterPtr.implicit operator Inno.Native.ImGui.ImDrawListSplitterPtr(Inno.Native.ImGui.ImDrawListSplitter* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12126) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4fcb2e63c8198089"></a>[`public static Inno.Native.ImGui.ImDrawListSplitterPtr.implicit operator Inno.Native.ImGui.ImDrawListSplitter*(Inno.Native.ImGui.ImDrawListSplitterPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12127) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-05a64a191667f425"></a>[`public static bool Inno.Native.ImGui.ImDrawListSplitterPtr.operator !=(Inno.Native.ImGui.ImDrawListSplitterPtr left, Inno.Native.ImGui.ImDrawListSplitterPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12129) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f1c81b0635e3d0a1"></a>[`public void Inno.Native.ImGui.ImDrawListSplitterPtr.SetCurrentChannel(Inno.Native.ImGui.ImDrawListPtr drawList, int channelIdx)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12181) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-06c4bf9f76694f1b"></a>[`public void Inno.Native.ImGui.ImDrawListSplitterPtr.Split(Inno.Native.ImGui.ImDrawListPtr drawList, int count)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12165) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-425064985d28857f"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawChannel> Inno.Native.ImGui.ImDrawListSplitterPtr.Channels { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12136) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-39bdd51b03286bff"></a>[`public ref int Inno.Native.ImGui.ImDrawListSplitterPtr.Count { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12135) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4bf90b5304e916d1"></a>[`public ref int Inno.Native.ImGui.ImDrawListSplitterPtr.Current { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12134) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e9308264f1779829"></a>[`public bool Inno.Native.ImGui.ImDrawListSplitterPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12121) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-284a891096f1b1f1"></a>[`public Inno.Native.ImGui.ImDrawListSplitter Inno.Native.ImGui.ImDrawListSplitterPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12124) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dc94ccd7d2f4d4ba"></a>[`public static Inno.Native.ImGui.ImDrawListSplitterPtr Inno.Native.ImGui.ImDrawListSplitterPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12122) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8be7b71361bc614f"></a>[`public struct Inno.Native.ImGui.ImDrawListSplitterPtr : System.IEquatable<Inno.Native.ImGui.ImDrawListSplitterPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12113) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImDrawTextFlags</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImDrawTextFlags 87fa73dc30952bd29b0a822f5bf8d42422dcb4721ae64ed23aed859ce24f3391 -->
<!-- api: F:Inno.Native.ImGui.ImDrawTextFlags.None cfd46bc7d68f49cf776916f02fc9e4c392e61cea6747dc221bce7b7b2d498613 -->
<!-- api: F:Inno.Native.ImGui.ImDrawTextFlags.CpuFineClip 364901fb497d84a240581626259dc962db4fc9cdd4ff4f91e40afab45f020004 -->
<!-- api: F:Inno.Native.ImGui.ImDrawTextFlags.WrapKeepBlanks 3dd997bc9ed9e52299950e2301cf17a650c4bfceb0b6020125e779655a104bf3 -->
<!-- api: F:Inno.Native.ImGui.ImDrawTextFlags.StopOnNewLine aa87926922aa05ee539e24539e4004002341fffcb9eaa81755475d4f575dfe1d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4f0a580f1d79bf67"></a>[`Inno.Native.ImGui.ImDrawTextFlags.CpuFineClip = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1194) | 当前常量或枚举值：`1`。 | — |
| <a id="api-a2cdcacdbb9f1acf"></a>[`Inno.Native.ImGui.ImDrawTextFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1193) | 当前常量或枚举值：`0`。 | — |
| <a id="api-84279497451aba60"></a>[`Inno.Native.ImGui.ImDrawTextFlags.StopOnNewLine = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1196) | 当前常量或枚举值：`4`。 | — |
| <a id="api-f47b677d2dae9d7c"></a>[`Inno.Native.ImGui.ImDrawTextFlags.WrapKeepBlanks = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1195) | 当前常量或枚举值：`2`。 | — |
| <a id="api-8a1d2dc9da7a1b63"></a>[`public enum Inno.Native.ImGui.ImDrawTextFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1191) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImDrawVert</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImDrawVert 13a8a1cbba6397c9980a02cf4df7143e1a1132bf88fe5b01ece51f37125f7445 -->
<!-- api: F:Inno.Native.ImGui.ImDrawVert.Pos 5bcebe1c79a2a2c66a6dc1d4aa778798c2bec637ca2122e572677e11cf8a27ba -->
<!-- api: F:Inno.Native.ImGui.ImDrawVert.Uv a0e560422480859b7d6d481035e7d28d2be79555e965debe4cc1afd59dcbb526 -->
<!-- api: F:Inno.Native.ImGui.ImDrawVert.Col 4b2f83bf9a3a011b27c432f1f8aa9642adaa14cfcd56a6cff249fd8f2e49634f -->
<!-- api: M:Inno.Native.ImGui.ImDrawVert.#ctor(System.Numerics.Vector2,System.Numerics.Vector2,System.UInt32) 4ce968ceaf3abb8c63162557e5676c15bf6818631accbd83d64057d02a2a4eba -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ababaa11667571ab"></a>[`public uint Inno.Native.ImGui.ImDrawVert.Col`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1783) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2ff55358637c6aad"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawVert.Pos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1781) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-abb5a110093083ea"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImDrawVert.Uv`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1782) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d880ef004ef8134"></a>[`public Inno.Native.ImGui.ImDrawVert.ImDrawVert(System.Numerics.Vector2 pos = default(System.Numerics.Vector2), System.Numerics.Vector2 uv = default(System.Numerics.Vector2), uint col = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1784) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8b95cff2cf2133e5"></a>[`public struct Inno.Native.ImGui.ImDrawVert`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1779) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImDrawVertPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImDrawVertPtr 26a70dab64bfcec4b0af855d644c86b70a3ca54f0b7883d14f0505d156585d0e -->
<!-- api: M:Inno.Native.ImGui.ImDrawVertPtr.#ctor(Inno.Native.ImGui.ImDrawVert*) 94dd9006f79a5a253d46356ec93a9dbd1d1ce9321b078861afd3ecf0ac15ab50 -->
<!-- api: F:Inno.Native.ImGui.ImDrawVertPtr.Handle 088e4ecce00e5459a31a457972f86cfb4858ed62d9dbc526e4ba2cdf3c5a3bb7 -->
<!-- api: P:Inno.Native.ImGui.ImDrawVertPtr.IsNull 4bb5d16c8f021e43201cbc379c417b494ac4887994cc0b632e11c38f5546d00f -->
<!-- api: P:Inno.Native.ImGui.ImDrawVertPtr.Null 31550e8536dbea13a4a52a00cad8192a2d782a3bca60542fe3d77efad0e80408 -->
<!-- api: P:Inno.Native.ImGui.ImDrawVertPtr.Item(System.Int32) bba1e60fed4be20edda9c8e970f16ce37fc4cf271ce28340945b9d27cd4b8a74 -->
<!-- api: M:Inno.Native.ImGui.ImDrawVertPtr.op_Implicit(Inno.Native.ImGui.ImDrawVert*)~Inno.Native.ImGui.ImDrawVertPtr 399c4ff0355e1b9f13a52853307df5e1313680da3633485830d7f7496b5f61a4 -->
<!-- api: M:Inno.Native.ImGui.ImDrawVertPtr.op_Implicit(Inno.Native.ImGui.ImDrawVertPtr)~Inno.Native.ImGui.ImDrawVert* 7e25914580c43ef907dc9461beb38f5c8d64543401d281eb12637c6dd38d24a4 -->
<!-- api: M:Inno.Native.ImGui.ImDrawVertPtr.op_Equality(Inno.Native.ImGui.ImDrawVertPtr,Inno.Native.ImGui.ImDrawVertPtr) e83b54d86ea60ecfe66110842b442d894b654ab2c1fe8cd9153e9c5c220ddbfd -->
<!-- api: M:Inno.Native.ImGui.ImDrawVertPtr.op_Inequality(Inno.Native.ImGui.ImDrawVertPtr,Inno.Native.ImGui.ImDrawVertPtr) e88a85faaacfee95faea7517802491b6e2d4d96592d77c32a27fa8255e2983d5 -->
<!-- api: M:Inno.Native.ImGui.ImDrawVertPtr.Equals(Inno.Native.ImGui.ImDrawVertPtr) 4687dd21a107492e50b5ad09426457646207bf5ff1e4f40562a3258bf6e5892c -->
<!-- api: M:Inno.Native.ImGui.ImDrawVertPtr.Equals(System.Object) fa2f6676886d7b588cc8976485f5dd94215b22dc4b7aaecb061da085343322e3 -->
<!-- api: M:Inno.Native.ImGui.ImDrawVertPtr.GetHashCode 14b8104224eb475a5ad91868f1c636514b4ef561036fb889051affc47722eda3 -->
<!-- api: P:Inno.Native.ImGui.ImDrawVertPtr.Pos 947bd2a8aa2550bae806e1b95df3f33699227c2ff91099462296af3163271712 -->
<!-- api: P:Inno.Native.ImGui.ImDrawVertPtr.Uv a8dcf606077412d4ac8bfc1f939f083274e25f4ca186768b0de65f6b3ee6f874 -->
<!-- api: P:Inno.Native.ImGui.ImDrawVertPtr.Col ce85a76eab4b31540a8ce8eef35ef30e9611a3d7a9d27d33e9bdff5b81d7ed3d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c375bfa8764b2f5e"></a>[`public Inno.Native.ImGui.ImDrawVert* Inno.Native.ImGui.ImDrawVertPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12195) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d639e1e722976843"></a>[`public Inno.Native.ImGui.ImDrawVertPtr.ImDrawVertPtr(Inno.Native.ImGui.ImDrawVert* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12190) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-36329c5e3c081c7f"></a>[`public bool Inno.Native.ImGui.ImDrawVertPtr.Equals(Inno.Native.ImGui.ImDrawVertPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12205) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-418b35151a422ebf"></a>[`public override bool Inno.Native.ImGui.ImDrawVertPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12206) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-2e95b9f8f83dc8cd"></a>[`public override int Inno.Native.ImGui.ImDrawVertPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12207) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-867634bdbcbf0b30"></a>[`public static bool Inno.Native.ImGui.ImDrawVertPtr.operator ==(Inno.Native.ImGui.ImDrawVertPtr left, Inno.Native.ImGui.ImDrawVertPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12203) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0083826261517190"></a>[`public static Inno.Native.ImGui.ImDrawVertPtr.implicit operator Inno.Native.ImGui.ImDrawVertPtr(Inno.Native.ImGui.ImDrawVert* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12201) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b37628976104e019"></a>[`public static Inno.Native.ImGui.ImDrawVertPtr.implicit operator Inno.Native.ImGui.ImDrawVert*(Inno.Native.ImGui.ImDrawVertPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12202) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-af3d116fa5ac8134"></a>[`public static bool Inno.Native.ImGui.ImDrawVertPtr.operator !=(Inno.Native.ImGui.ImDrawVertPtr left, Inno.Native.ImGui.ImDrawVertPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12204) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eb35a3e27178d2b6"></a>[`public ref uint Inno.Native.ImGui.ImDrawVertPtr.Col { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12211) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a1a73e10f7a291de"></a>[`public bool Inno.Native.ImGui.ImDrawVertPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12196) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-71095bb83dd2ae67"></a>[`public Inno.Native.ImGui.ImDrawVert Inno.Native.ImGui.ImDrawVertPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12199) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b2a8803c3a6aa6ae"></a>[`public static Inno.Native.ImGui.ImDrawVertPtr Inno.Native.ImGui.ImDrawVertPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12197) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c07319a5c99f49a5"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImDrawVertPtr.Pos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12209) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3ef87d87f6fb2111"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImDrawVertPtr.Uv { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12210) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a3446c0dcddfd29d"></a>[`public struct Inno.Native.ImGui.ImDrawVertPtr : System.IEquatable<Inno.Native.ImGui.ImDrawVertPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12188) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFont</code> · 26 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFont 2c015fabf7038bd279efa9a1f38e00431623fa39577ab1826a9b3bd9a01aa1b9 -->
<!-- api: F:Inno.Native.ImGui.ImFont.LastBaked 08343f617c8811be2134af8150d6983feb01628868825c3b9d5729392db05eb9 -->
<!-- api: F:Inno.Native.ImGui.ImFont.OwnerAtlas 53043cf662f318a13cb6e47cca4e05629ce33dd86d3b124d9b2dea76b2072f01 -->
<!-- api: F:Inno.Native.ImGui.ImFont.Flags a0ca8a139b7c2725009b99ac3ffab6456d3f6426a46d2af7b8d13d26399eafea -->
<!-- api: F:Inno.Native.ImGui.ImFont.CurrentRasterizerDensity 29663c197b9c3aa5592bbb0c3d67c8255bafbedf5bb5f25bbd32420b49e59815 -->
<!-- api: F:Inno.Native.ImGui.ImFont.FontId eebfeaf827bda24d21d0437f4897a00806a5271bdc1fdc6ec872f459c175daf0 -->
<!-- api: F:Inno.Native.ImGui.ImFont.LegacySize f7c587380baadc9a4b66565a687f297794030dfca78030d7e4eea85502edf06d -->
<!-- api: F:Inno.Native.ImGui.ImFont.Sources b02010c3ed43ca7338d1b220da76722180cb42d033796c4550013f395a3e87e4 -->
<!-- api: F:Inno.Native.ImGui.ImFont.EllipsisChar 1ad14b88cf6778700ced4e1b79017a137f908029ac089178570415281769c967 -->
<!-- api: F:Inno.Native.ImGui.ImFont.FallbackChar f6780ca8afca07767060e12de70980144e7a950076e891f1ee929671c6090074 -->
<!-- api: F:Inno.Native.ImGui.ImFont.Used8kPagesMap c07f78767e59cb1792edcf168492c1aa54e53828b52c9e03819a4483b47e089f -->
<!-- api: F:Inno.Native.ImGui.ImFont.EllipsisAutoBake 2fbe23bbd4e2ca899a230de2e104f48facd2a67f578f72cee6943b85077e8f75 -->
<!-- api: F:Inno.Native.ImGui.ImFont.RemapPairs eed325e0a40354be33bdb3115202f960f47aea04e828430d1ab62b5afeea334e -->
<!-- api: M:Inno.Native.ImGui.ImFont.#ctor(Inno.Native.ImGui.ImFontBaked*,Inno.Native.ImGui.ImFontAtlas*,Inno.Native.ImGui.ImFontFlags,System.Single,System.UInt32,System.Single,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImFontConfigPtr},System.UInt16,System.UInt16,System.Byte*,System.Byte,Inno.Native.ImGui.ImGuiStorage) 02b888c0fcd5163cded245d65526e72400988b271f71f490b59828cebdf55e9b -->
<!-- api: M:Inno.Native.ImGui.ImFont.Destroy 774e6c2a95b1f00df4e057ce4e0a48ac84dd096c41f2ccb6cd6aa686529d4469 -->
<!-- api: M:Inno.Native.ImGui.ImFont.IsGlyphInFont(System.UInt16) a4d0d8a8870409754a26bb1eafae454997a3be9808fb62a9f3bcad4ce8438df4 -->
<!-- api: M:Inno.Native.ImGui.ImFont.IsLoaded 9f917c61a0e161bd84598cff435022c81c4038ddfd80fa78dee054388d148c21 -->
<!-- api: M:Inno.Native.ImGui.ImFont.GetDebugName acdb8ec3761ac0b59c3185a2abdcd5e76a47eac2be2e17a3c99ba00ebd06dbb5 -->
<!-- api: M:Inno.Native.ImGui.ImFont.GetFontBaked(System.Single,System.Single) bbe67885597ab432f26c3bd362d32d59f67cde82d9b0eb7eaee99ae60b14435e -->
<!-- api: M:Inno.Native.ImGui.ImFont.CalcTextSizeA(System.Single,System.Single,System.Single,System.Byte*,System.Byte*,System.Byte**) f01543500100a6c2fcb0b1dfdca5011c862ec7f5f9a57a897b971ec764192444 -->
<!-- api: M:Inno.Native.ImGui.ImFont.CalcWordWrapPosition(System.Single,System.Byte*,System.Byte*,System.Single) 5a13cee8222441e87b6596c38beb221ee772a3b747a09b21831ad7443f90032e -->
<!-- api: M:Inno.Native.ImGui.ImFont.RenderChar(Inno.Native.ImGui.ImDrawList*,System.Single,System.Numerics.Vector2,System.UInt32,System.UInt16,System.Numerics.Vector4*) 5ed433e561a8013b7fd3d996feccbea72030531d29d314dae495ff241ec4a3df -->
<!-- api: M:Inno.Native.ImGui.ImFont.RenderText(Inno.Native.ImGui.ImDrawList*,System.Single,System.Numerics.Vector2,System.UInt32,System.Numerics.Vector4,System.Byte*,System.Byte*,System.Single,Inno.Native.ImGui.ImDrawTextFlags) e3ba7325ba086e94f8ccf7742d2ce298bf460a0e339f2c3173534daee37804a2 -->
<!-- api: M:Inno.Native.ImGui.ImFont.ClearOutputData 812f049e9ab445e1bec54fe1cea6673d2e27dd02942fc366c0ff9ecccb723dc5 -->
<!-- api: M:Inno.Native.ImGui.ImFont.AddRemapChar(System.UInt16,System.UInt16) 922572bc4f9101b9a8181e5261db33040c17f1d690c46ffec64567ffb8d6ad98 -->
<!-- api: M:Inno.Native.ImGui.ImFont.IsGlyphRangeUnused(System.UInt32,System.UInt32) 9a6f2873311e0026936e9e80683913f8da89af9780ad9de84900fa607bb7c727 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a0e1467f3f3d2595"></a>[`public float Inno.Native.ImGui.ImFont.CurrentRasterizerDensity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2144) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c37acf863225b28"></a>[`public byte Inno.Native.ImGui.ImFont.EllipsisAutoBake`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2151) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d8c439be98270dd5"></a>[`public ushort Inno.Native.ImGui.ImFont.EllipsisChar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2148) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ecd67340ae6c1e72"></a>[`public ushort Inno.Native.ImGui.ImFont.FallbackChar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2149) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bca918b10c7ee1c1"></a>[`public Inno.Native.ImGui.ImFontFlags Inno.Native.ImGui.ImFont.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2143) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e5f1ddad2108110"></a>[`public uint Inno.Native.ImGui.ImFont.FontId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2145) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8315c6bbcedcf3e8"></a>[`public Inno.Native.ImGui.ImFontBaked* Inno.Native.ImGui.ImFont.LastBaked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2141) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4241ac17fa103b32"></a>[`public float Inno.Native.ImGui.ImFont.LegacySize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2146) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5b738799a9e758f7"></a>[`public Inno.Native.ImGui.ImFontAtlas* Inno.Native.ImGui.ImFont.OwnerAtlas`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2142) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-848cdc7ef4d8f482"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImFont.RemapPairs`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2152) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2dd6b35db6796b6f"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontConfigPtr> Inno.Native.ImGui.ImFont.Sources`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2147) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a0122a3c43f0b90"></a>[`public byte Inno.Native.ImGui.ImFont.Used8kPagesMap`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2150) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1a82d696ea419b72"></a>[`public Inno.Native.ImGui.ImFont.ImFont(Inno.Native.ImGui.ImFontBaked* lastBaked = null, Inno.Native.ImGui.ImFontAtlas* ownerAtlas = null, Inno.Native.ImGui.ImFontFlags flags = Inno.Native.ImGui.ImFontFlags.None, float currentRasterizerDensity = 0, uint fontId = 0, float legacySize = 0, Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontConfigPtr> sources = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontConfigPtr>), ushort ellipsisChar = 0, ushort fallbackChar = 0, byte* used8kPagesMap = null, byte ellipsisAutoBake = 0, Inno.Native.ImGui.ImGuiStorage remapPairs = default(Inno.Native.ImGui.ImGuiStorage))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2153) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2f014e68761f824d"></a>[`public void Inno.Native.ImGui.ImFont.AddRemapChar(ushort fromCodepoint, ushort toCodepoint)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65819) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d9845d84a6cd0269"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImFont.CalcTextSizeA(float size, float maxWidth, float wrapWidth, byte* textBegin, byte* textEnd, byte** outRemaining)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65764) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fae33b0fa24d2f43"></a>[`public byte* Inno.Native.ImGui.ImFont.CalcWordWrapPosition(float size, byte* text, byte* textEnd, float wrapWidth)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65775) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ee28101cd64b501"></a>[`public void Inno.Native.ImGui.ImFont.ClearOutputData()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65808) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c1fea7498ed14ba3"></a>[`public void Inno.Native.ImGui.ImFont.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65709) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c54ed1089bd1e350"></a>[`public byte* Inno.Native.ImGui.ImFont.GetDebugName()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65742) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-653307ff83cd5cc6"></a>[`public Inno.Native.ImGui.ImFontBaked* Inno.Native.ImGui.ImFont.GetFontBaked(float fontSize, float density)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65753) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4c6e1730a5da3f26"></a>[`public byte Inno.Native.ImGui.ImFont.IsGlyphInFont(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65720) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8e662bc34e9ee62c"></a>[`public byte Inno.Native.ImGui.ImFont.IsGlyphRangeUnused(uint cBegin, uint cLast)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65830) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7088037fb59ebcba"></a>[`public byte Inno.Native.ImGui.ImFont.IsLoaded()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65731) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-343bacc08bae7cd3"></a>[`public void Inno.Native.ImGui.ImFont.RenderChar(Inno.Native.ImGui.ImDrawList* drawList, float size, System.Numerics.Vector2 pos, uint col, ushort c, System.Numerics.Vector4* cpuFineClip)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65786) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6498208fa9040244"></a>[`public void Inno.Native.ImGui.ImFont.RenderText(Inno.Native.ImGui.ImDrawList* drawList, float size, System.Numerics.Vector2 pos, uint col, System.Numerics.Vector4 clipRect, byte* textBegin, byte* textEnd, float wrapWidth, Inno.Native.ImGui.ImDrawTextFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65797) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a25260256b2cad49"></a>[`public struct Inno.Native.ImGui.ImFont`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2139) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontAtlas</code> · 85 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontAtlas 80686a37e913d62eebaaddf2b280ae4c981d469a2c9100866df3cc7a79ae594e -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.Flags 5078bc55341a1343864a4b17b5296fccd149bb210844622d4ccc5c0aeeeae63f -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexDesiredFormat 68609f953fe8d84a3c450282f2d0899707d51f5c266e35dac10b49b071a6a711 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexGlyphPadding 31e81a4d21282de6f992be04538b22c7115e6ccd8644ebfe95b60bb7d203ae37 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexMinWidth 118f140b621f3d91f54ce588a4cdb21f7e612cac8bfa67bd023aae4e2cae0af1 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexMinHeight 4a1b6fb9d0ad4ad8c8b62bb8fb219874708d1d2bfa7515af3558794a9aa2c52d -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexMaxWidth e4736b760b875f9fa7364124c71c2dc31eef53dfff2b7fa831272acabcaaffe7 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexMaxHeight f06ef3035e420ba7b687540c92663b3cf7550364f884a4c98ecfdc64cd00005a -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.UserData 1655f660941874ffa6a7eff345e1539bec0bc2b95717783087ba738214f9a185 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexRef 5433c5242cccce7b274ede23ae52d663ee39f0775d5e4008f58938d6f5347d1a -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexData 7d2ebaf016fd440e22e779068d2d1b566e92a848ffeeb3d4656afba938e5339a -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexList 6877bee98872b941545b84552bc9a04d825dc2141383120aa5f8b4e90798b859 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.Locked 4c81206b272b07f14dbdde664fdaf974376b35fdc8618bc63db2ab817f7aed39 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.RendererHasTextures 976ac194635735ce4883890ac3848fa71ec902bd92580f535f27dfd5acb6fd7a -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexIsBuilt e48fce51db941a21141217fd7506f5fa40c806f2ab4335e99c2120bd20718359 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexPixelsUseColors bdd1a2bdabcf0ed54a676dd9d1176b8f29eb8529ae0a28e7e4c0761a5de36598 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvScale b713cc87f0c55690ff54819efbee344eb8ad751490bae143dff38b6cc50d5d39 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvWhitePixel 49d22cfe0f92ab4e57694556bb04df93eb564d4e15bd5afb06f802d7c84db102 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.Fonts 8f23ab5bd314026a8fbe0e52ab2a464151a43fdeb8fee95748c9468657daf3fa -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.Sources 2b91156ea24c38bc4a8ffa36cc26c31cb2748f6719f4413723eb52fe90d9cb72 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_0 d653382fa9533b8a05a0802ee81b7d817504e870848ee4e44382d10e30390bd7 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_1 d8d702c4e9b029be840cd479550ea874636d590b2dc4b5f0c96cdf14677cd4dd -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_2 d186ffa81d338186263da0e8048d7329b417dbc8bb245477fafc6071736ad923 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_3 7496b2acb2b906183774d526226b0a2ae0f56fc577dfc195a9951dccba08ba29 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_4 43851b31220c31522e65432b46de9c55867a82edd70d2ec7bc1fcf32365562c9 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_5 4ee783cc81395ea2db8a51057449fd6a8aff1cc59f967d10bc023071683be053 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_6 8bbec84b761602933443f71f01e45539a15587ae3f31d42e69218f6ccd51d59e -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_7 8b398435d1ec82820bd29be2c32c221f4c847bfd2d8621cb347e576f9aeb287c -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_8 40748b6907928ee71b58042cb9e902cdb8ee3f3251053d9290580be85a6e8e63 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_9 d9a891169940d7f7b205e5dbab696d7a923beef49e6eeecc5147dfd0e50a1636 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_10 2acde958286380834526a89421d1c47989937c48eeab28f08948940d6ecc5858 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_11 1d70e3b2c8dbd478ccc5e28a092557190ce0fff90b59dca1246f6b492ae9e78f -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_12 a1801d90f734d323b958d2e7c6e12a31a3b228e4d708a4e5f24e2ed4e91c2e0c -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_13 da28100446883dbdcb224aadf8bc8949c07fa474d8638d616b0a0f6aa834ddab -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_14 e425731020f398c9286e48190af855698e56960617b52eda25834c7d296c6a6b -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_15 e3a8ca881260218c5ec89ad0ff1d44847638ec72bbb134070d51a6d25d233f38 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_16 c7bb13767601bfddd24376069bd5f449d02dc1ea03ccc142950c904df7bea49b -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_17 3aa6c52da4b4dfc67b3b98a000e26bbcc5613d5b51b55dc259132c28320c592d -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_18 3893aa2eaceccc2423e434b479c435a4ce7a5ca0a1b3f56e5721cbf963bc3788 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_19 495ca4aaf1622001d32f2e4f0ef53f0eb3bdae2d445084d983f2da5a1f252be9 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_20 50da43fdaea3a3dfc1a3c0da0e9530ae840cb261f1a337580d2e35e16ecf7fcf -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_21 ca37b152f578959a1d643690e565847d65db675197d89b1b9e65a8cf762b4387 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_22 27e33b2bcddee17e99e3f3abdb077324906abcf5974470d480fc26de8f062ed5 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_23 a4061bc3d7064e113d296301c58fd4dfcaca78fce5f3a7086193cdda8eb26b2d -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_24 ee0560fef0cb2901e503aaa1cd633b20fe52974a0e394fb7f5d79a181137e35f -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_25 1a7e7976795e967a039b59895323e8c51cb57aacb8a2a6e450237d463c324262 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_26 05376461bc0b4ef9d3e9154331994235b59bf74e806729b146aa8a0ff64071ee -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_27 8bb067ff7c8cb2172ce2b2f7b3f22b11a01abe8651179299dc64b771e8587e0d -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_28 3ccf35af9436e1b18d0905f4743ee1364d3151273bfe1dc7cac87df62470fb09 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_29 b97ed7f9d62fcd00e0bbd55549a8177ba0826d7b36f151604f9241dfb751031a -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_30 915db04d349ac1c39dd0d9cae6c82746353f3bdb1e35e1bfe7a88107bb978d94 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_31 90756f72915539eef570618274da082f9b983ac71e1950c6879f116ca45aed1d -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexUvLines_32 76bd96f274641c2d28176c164b1af6505963f64e661576bb3bd8ae9fa9fb605b -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlas.TexUvLines c4589be92a64ca4080c670304488c31d1382739248881d6c96b116a7a27014d6 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.TexNextUniqueID 3f91944bb9ef1b9b4abb448a5d1bc544147cea12b5cc3b70fdc3db44d96c78c3 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.FontNextUniqueID f15a810ac5e29da1ccaa6ac28bcad74b16d96232e43b9fb87e1d3929079374b3 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.DrawListSharedDatas 05f0ccf2cf06682b5bdfcdfb9c02d8531768ec2053108c8cf93e754017bc6ffb -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.Builder 03c6a1c758ae453bc97e34a5d203a7d14427c812d0d9b0bfc27708db7a8c4de1 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.FontLoader 63648859fafa3112ccc049a737ea947cbed6ebb38ad9390d0b0de5eee3050436 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.FontLoaderName 48db3314201f73698b4b7941023676f9e9eb28cb19467e78eaa4ec1c075924a2 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.FontLoaderData 2631182e334994cb85b1190abdc660435876c990e034aa81fb1c33f6cce81b2e -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.FontLoaderFlags b0114d2d37d696493738cc7f857f013c0946d9ce40c1035bcca08383fd988424 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.RefCount be0e219b7e74a3d0b5ac83ec5248df11515da00aa99047f99540a0cc78e4fc07 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlas.OwnerContext c7b4f26cf329d674893fbf23ccd51086c58c885b9d15b8974f0e8728da2cb2f7 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.#ctor(Inno.Native.ImGui.ImFontAtlasFlags,Inno.Native.ImGui.ImTextureFormat,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Void*,Inno.Native.ImGui.ImTextureRef,Inno.Native.ImGui.ImTextureData*,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImTextureDataPtr},System.Byte,System.Byte,System.Byte,System.Byte,System.Numerics.Vector2,System.Numerics.Vector2,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImFontPtr},Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImFontConfig},System.Numerics.Vector4*,System.Int32,System.Int32,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImDrawListSharedDataPtr},Inno.Native.ImGui.ImFontAtlasBuilder*,Inno.Native.ImGui.ImFontLoader*,System.Byte*,System.Void*,System.UInt32,System.Int32,Inno.Native.ImGui.ImGuiContext*) 50f245bf2b3dd3497dd6732028c775b5cf3d273b260429f98cc08f3540d73c1d -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.Destroy 2817ca047f83c1e7dd26ada9574db38d1279e1edef9c9d7008df6f70d6b4dd2f -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.AddFont(Inno.Native.ImGui.ImFontConfig*) 3f85462118350ad80973fc72058a754506c5ba563cd33b996807284c7f943b36 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.AddFontDefault(Inno.Native.ImGui.ImFontConfig*) 05ca337ba2725daea96a26f8b8b7d071acfb4d7f49ef86b07c816d46dff13683 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.AddFontDefaultVector(Inno.Native.ImGui.ImFontConfig*) a82e38662b8a1405b261ba47210ee57db30b8704fc5b5f7057bb8f150572b92a -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.AddFontDefaultBitmap(Inno.Native.ImGui.ImFontConfig*) 80b87a349627ce54ae918063cfc69440cecdffbd72850299990b9f856adcf925 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.AddFontFromFileTTF(System.Byte*,System.Single,Inno.Native.ImGui.ImFontConfig*,System.UInt16*) 698d356e9f3aece92f149f2639f47a1bac58a198f7d693893e4635d89cf184e7 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.AddFontFromMemoryTTF(System.Void*,System.Int32,System.Single,Inno.Native.ImGui.ImFontConfig*,System.UInt16*) d899b9bd1e48068770298d6468d7df6b0fe80f43b3a58cbe11ef48aada9a32e3 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.AddFontFromMemoryCompressedTTF(System.Void*,System.Int32,System.Single,Inno.Native.ImGui.ImFontConfig*,System.UInt16*) d10e8c28f7fd98290e5e58ef3a785abb72b41bbf524f59adffcbc71b73539fcc -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.AddFontFromMemoryCompressedBase85TTF(System.Byte*,System.Single,Inno.Native.ImGui.ImFontConfig*,System.UInt16*) ce7da6d7a5a1bd29d59dce5294474a9abff23e7472c5830a44d358d8cb77d98e -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.RemoveFont(Inno.Native.ImGui.ImFont*) b9b0b9b1de10f832a1e160b11eaed10f1c1046781bc26263d2b76f4da0edcfa8 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.Clear d8868444e216da1de6be44268f03cc93ceb1a53836bfe7ce035f01f725673bef -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.CompactCache c33c9a966755a8a11997fa15066cad5f6f44d4d464135727aa06c216e421cf07 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.SetFontLoader(Inno.Native.ImGui.ImFontLoader*) a1fade79ef7743a750305d242646d923fc814502324cecdd936f6bd8dc87b74f -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.ClearInputData 1b10428e8ce52f7216a87160de57506b27c34558f74ffc3a9749aa4a184c9818 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.ClearFonts bc9532303004d4959e67006c1a1f9fc713b6f51f0de4ceb3b9c146c84ab42303 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.ClearTexData c41a868e63adbfb5a34ef276e68cc4b15186339cdd6d4be79425bed694296e1e -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.GetGlyphRangesDefault 3fbb6ec319d2c3efe7e3e3eaf27577a12436462090b60e6ac7ae612f12943230 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.AddCustomRect(System.Int32,System.Int32,Inno.Native.ImGui.ImFontAtlasRect*) 9afb6933c76ff351ce4b78cada3b620e590e5b23738ea823b8afc5a0583310d1 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.RemoveCustomRect(System.Int32) 4a20b766e228cdd3095fde156f7090207ba5e124023d35bd304d4f2d1462d6d4 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlas.GetCustomRect(System.Int32,Inno.Native.ImGui.ImFontAtlasRect*) e233c233778aa2525335d5ffb9f678470d96f9a8a0b1215b7b10207f4ed1e174 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-de2901bcf77d614a"></a>[`public Inno.Native.ImGui.ImFontAtlasBuilder* Inno.Native.ImGui.ImFontAtlas.Builder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8986) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e84339005d84a743"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawListSharedDataPtr> Inno.Native.ImGui.ImFontAtlas.DrawListSharedDatas`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8985) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-78fd5a8afaa3ee20"></a>[`public Inno.Native.ImGui.ImFontAtlasFlags Inno.Native.ImGui.ImFontAtlas.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8929) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6b46e8ffd03f85a8"></a>[`public Inno.Native.ImGui.ImFontLoader* Inno.Native.ImGui.ImFontAtlas.FontLoader`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8987) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89bd886b7c764b9f"></a>[`public void* Inno.Native.ImGui.ImFontAtlas.FontLoaderData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8989) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-249104438b88ac58"></a>[`public uint Inno.Native.ImGui.ImFontAtlas.FontLoaderFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8990) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bddb020f43f017f0"></a>[`public byte* Inno.Native.ImGui.ImFontAtlas.FontLoaderName`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8988) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-074d72ba8ec8e1f0"></a>[`public int Inno.Native.ImGui.ImFontAtlas.FontNextUniqueID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8984) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba157f7dbf01aed3"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontPtr> Inno.Native.ImGui.ImFontAtlas.Fonts`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8946) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f55b1ce33d949351"></a>[`public byte Inno.Native.ImGui.ImFontAtlas.Locked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8940) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d8b52fdea076daa4"></a>[`public Inno.Native.ImGui.ImGuiContext* Inno.Native.ImGui.ImFontAtlas.OwnerContext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8992) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-42b323d7556da327"></a>[`public int Inno.Native.ImGui.ImFontAtlas.RefCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8991) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-687a405fc041a0c6"></a>[`public byte Inno.Native.ImGui.ImFontAtlas.RendererHasTextures`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8941) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f801dc9e34833b46"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontConfig> Inno.Native.ImGui.ImFontAtlas.Sources`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8947) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-47b7081a1c8e7181"></a>[`public Inno.Native.ImGui.ImTextureData* Inno.Native.ImGui.ImFontAtlas.TexData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8938) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b167617c55317b7b"></a>[`public Inno.Native.ImGui.ImTextureFormat Inno.Native.ImGui.ImFontAtlas.TexDesiredFormat`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8930) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7ea411791d36958"></a>[`public int Inno.Native.ImGui.ImFontAtlas.TexGlyphPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8931) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2dd1a7418202092e"></a>[`public byte Inno.Native.ImGui.ImFontAtlas.TexIsBuilt`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8942) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f42e0a7b5c55525"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureDataPtr> Inno.Native.ImGui.ImFontAtlas.TexList`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8939) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9a5e256cc8fd27eb"></a>[`public int Inno.Native.ImGui.ImFontAtlas.TexMaxHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8935) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89e6a9368686ed43"></a>[`public int Inno.Native.ImGui.ImFontAtlas.TexMaxWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8934) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-67c4b03d78dd6ec9"></a>[`public int Inno.Native.ImGui.ImFontAtlas.TexMinHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8933) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-395d4f3bae13de50"></a>[`public int Inno.Native.ImGui.ImFontAtlas.TexMinWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8932) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-44ef635e35ef88e7"></a>[`public int Inno.Native.ImGui.ImFontAtlas.TexNextUniqueID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8983) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a00649b4874841bb"></a>[`public byte Inno.Native.ImGui.ImFontAtlas.TexPixelsUseColors`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8943) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d3b3434f91d8a564"></a>[`public Inno.Native.ImGui.ImTextureRef Inno.Native.ImGui.ImFontAtlas.TexRef`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8937) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5031d308bf7661d4"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8948) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-97b98bbb34f5db9b"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8949) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d11cd17316b08652"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8958) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b08b84e8be6ab388"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8959) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cfd14450741512f9"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8960) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69f021c61ba20fd5"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8961) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a7219e4d893f7bd"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8962) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1dbcbcec03453fcc"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8963) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e932b279e2b5d961"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8964) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e4f4d39766ab7323"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8965) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6d7d596923f99725"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8966) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5f0913cead502bdc"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8967) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8fbb7295bd936a73"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8950) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8f283ce8175870e9"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8968) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-29cbd965028b6674"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8969) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d014a9919b79ee39"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8970) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1a1c430671757f8"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8971) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-688d05c309796439"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8972) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a9e1a81e6179a1d4"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8973) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7a9e73cd66ecb661"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8974) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1ec0fc62c0f92657"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8975) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af9a1e881edf5c3d"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8976) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d3d62102aacb97c"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8977) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8048de387b355d02"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8951) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-26925414c19caaf9"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8978) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eb76cb1444f7dfec"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8979) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a1b685ea5012bd4"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8980) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f790a07d39598897"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8952) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec0393c11df37227"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8953) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-43e875a8eeeaaafc"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8954) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf1ffcca2f39dae9"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8955) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-879d94666ced1dd3"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8956) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4affd92555f38599"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImFontAtlas.TexUvLines_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8957) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-65f5c22578222698"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImFontAtlas.TexUvScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8944) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8899bc72ce47d823"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImFontAtlas.TexUvWhitePixel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8945) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd767d91e5790458"></a>[`public void* Inno.Native.ImGui.ImFontAtlas.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8936) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-195d821e438e936d"></a>[`public Inno.Native.ImGui.ImFontAtlas.ImFontAtlas(Inno.Native.ImGui.ImFontAtlasFlags flags = Inno.Native.ImGui.ImFontAtlasFlags.None, Inno.Native.ImGui.ImTextureFormat texDesiredFormat = Inno.Native.ImGui.ImTextureFormat.Rgba32, int texGlyphPadding = 0, int texMinWidth = 0, int texMinHeight = 0, int texMaxWidth = 0, int texMaxHeight = 0, void* userData = null, Inno.Native.ImGui.ImTextureRef texRef = default(Inno.Native.ImGui.ImTextureRef), Inno.Native.ImGui.ImTextureData* texData = null, Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureDataPtr> texList = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureDataPtr>), byte locked = 0, byte rendererHasTextures = 0, byte texIsBuilt = 0, byte texPixelsUseColors = 0, System.Numerics.Vector2 texUvScale = default(System.Numerics.Vector2), System.Numerics.Vector2 texUvWhitePixel = default(System.Numerics.Vector2), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontPtr> fonts = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontPtr>), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontConfig> sources = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontConfig>), System.Numerics.Vector4* texUvLines = null, int texNextUniqueID = 0, int fontNextUniqueID = 0, Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawListSharedDataPtr> drawListSharedDatas = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawListSharedDataPtr>), Inno.Native.ImGui.ImFontAtlasBuilder* builder = null, Inno.Native.ImGui.ImFontLoader* fontLoader = null, byte* fontLoaderName = null, void* fontLoaderData = null, uint fontLoaderFlags = 0, int refCount = 0, Inno.Native.ImGui.ImGuiContext* ownerContext = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8993) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d96ecb92316ca4dc"></a>[`public int Inno.Native.ImGui.ImFontAtlas.AddCustomRect(int width, int height, Inno.Native.ImGui.ImFontAtlasRect* outR)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65604) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-025a6246f3ce9ab9"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontAtlas.AddFont(Inno.Native.ImGui.ImFontConfig* fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65428) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9a3a7e34d18b314b"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontAtlas.AddFontDefault(Inno.Native.ImGui.ImFontConfig* fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65439) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-580a64451d035ea9"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontAtlas.AddFontDefaultBitmap(Inno.Native.ImGui.ImFontConfig* fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65461) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aac277f7b804c407"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontAtlas.AddFontDefaultVector(Inno.Native.ImGui.ImFontConfig* fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65450) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ce5971264d0268c9"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontAtlas.AddFontFromFileTTF(byte* filename, float sizePixels, Inno.Native.ImGui.ImFontConfig* fontCfg, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65472) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e470a27f252bccd0"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontAtlas.AddFontFromMemoryCompressedBase85TTF(byte* compressedFontDatabase85, float sizePixels, Inno.Native.ImGui.ImFontConfig* fontCfg, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65505) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-793ce5d83b726679"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontAtlas.AddFontFromMemoryCompressedTTF(void* compressedFontData, int compressedFontDataSize, float sizePixels, Inno.Native.ImGui.ImFontConfig* fontCfg, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65494) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-33ed3a6307bd7420"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontAtlas.AddFontFromMemoryTTF(void* fontData, int fontDataSize, float sizePixels, Inno.Native.ImGui.ImFontConfig* fontCfg, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65483) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7b819bbef6e35a3d"></a>[`public void Inno.Native.ImGui.ImFontAtlas.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65527) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-56ba879c806f1509"></a>[`public void Inno.Native.ImGui.ImFontAtlas.ClearFonts()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65571) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f22978ea948404b9"></a>[`public void Inno.Native.ImGui.ImFontAtlas.ClearInputData()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65560) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-42180373798ac9ec"></a>[`public void Inno.Native.ImGui.ImFontAtlas.ClearTexData()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65582) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4b3e380812ff582b"></a>[`public void Inno.Native.ImGui.ImFontAtlas.CompactCache()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65538) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-81a4af968289793f"></a>[`public void Inno.Native.ImGui.ImFontAtlas.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65417) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-72f4c607a6b1d0f6"></a>[`public byte Inno.Native.ImGui.ImFontAtlas.GetCustomRect(int id, Inno.Native.ImGui.ImFontAtlasRect* outR)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65626) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-07ea2633235406b0"></a>[`public ushort* Inno.Native.ImGui.ImFontAtlas.GetGlyphRangesDefault()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65593) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d977f4a2d6045487"></a>[`public void Inno.Native.ImGui.ImFontAtlas.RemoveCustomRect(int id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65615) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8ca959a03958bde8"></a>[`public void Inno.Native.ImGui.ImFontAtlas.RemoveFont(Inno.Native.ImGui.ImFont* font)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65516) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6b6fc4e396db606e"></a>[`public void Inno.Native.ImGui.ImFontAtlas.SetFontLoader(Inno.Native.ImGui.ImFontLoader* fontLoader)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65549) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c1715705373809f7"></a>[`public System.Span<System.Numerics.Vector4> Inno.Native.ImGui.ImFontAtlas.TexUvLines { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8981) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-820100db5b513435"></a>[`public struct Inno.Native.ImGui.ImFontAtlas`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L8927) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontAtlasBuilder</code> · 23 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontAtlasBuilder 80e1fca23d4b2f3bea851e00155a48c116ab14dae3eac35151cefbfb08ab6d9f -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.PackContext 00699c02afffd6ea997ee906784854a8c46d28c327d41295fb23dafacec0d932 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.PackNodes 82dae1ed7f09c559e4a35762de8aec69226c40209c7947ae28048696fc362e3d -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.Rects b6fef3cfb4abf5be8ca8049aad791d93d5870505f16e837db604e3bbc5aca585 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.RectsIndex bcbc75fdf0af7105ac865493c8f3172cdd63f850794045ec34c839ebc02a250e -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.TempBuffer 6cf9b03b48c61f4d44ce0a9be71fc41987b47a7e7d9d57db0285610fc2afcb8c -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.RectsIndexFreeListStart 927eddeb609de0a03e70b43a3dd52ff297a8f007250274110476df8bc4dade17 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.RectsPackedCount 626bdbd24e25f43d8aba7d86e5c3aacb667ee23701a1a8694841e0ac019ced83 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.RectsPackedSurface 4165df6429c2172a97730d6642788b22701c1b566da0a5eca6cac96880b8ae8a -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.RectsDiscardedCount 7dce6371ac69bf0b214ebeb35ee42cd51904cdae05cf4de7eac4bff4eb6bf15f -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.RectsDiscardedSurface 1366686366f94123eaded6c5303423dfe4d0950aa3eb1c8dd4f72249bcaad97c -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.FrameCount fc5716eecec27e1d9fcc76e6f314d4ff32ee7ffcc2305ff1a5dd524bad5b7d3f -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.MaxRectSize 1d71c28c325e829664b05a8b1dbe4df629585b3966946eeadb1b01117038dee9 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.MaxRectBounds 627e5552074ec9cc0c796222a14c3fdb6ddc71357e68793f58b45b1eb06fb505 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.LockDisableResize 34b97c4736e1712266367b9d3bd760a696bfbc8a16a5cad92a3eb208cc5e9dbc -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.PreloadedAllGlyphsRanges 544b686626cc29f84a96463b529509a84b26665ca84f192300c2f1604e406e27 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.BakedPool fab1af88f7cf6744514b5ecd4e044639923dc6f9b3a1238ab994d4c6ec5cffcb -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.BakedMap e40e2d38520900c7d4a2421c0ecaae67270a1ec83de0e63e38bfe68432766b57 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.BakedDiscardedCount 54bdc8d1837d7bd74c796f8b9bb2fb7775d2472a3099e9049c8fdf4aac5c59ea -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.PackIdMouseCursors b2cd89fe606d850613808fa07ce684d9fea0d3bf50b4e533386a1a5bcdf867b9 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilder.PackIdLinesTexData af03ec8b488f535bac71aa66bd47311897d24fc019152edfb6a1b498c363bad2 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasBuilder.#ctor(Inno.Native.ImGui.StbrpContextOpaque,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.StbrpNode},Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImTextureRect},Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImFontAtlasRectEntry},Inno.Native.ImGui.ImVector{System.Byte},System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,Inno.Native.ImGui.ImVec2IC,Inno.Native.ImGui.ImVec2IC,System.Byte,System.Byte,Inno.Native.ImGui.ImStableVectorImFontBaked32,Inno.Native.ImGui.ImGuiStorage,System.Int32,System.Int32,System.Int32) ac88bba5a5be58abf524069b8f51fb92218e2bd1c283f49baae4043a19aaf61f -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasBuilder.Destroy c52da4297f797ad79bf26864241221d4f7468f10030bc878bf058d6429dcd0b9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-07c827fa1df1d0e7"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.BakedDiscardedCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2455) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3faf269dfd35bcec"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImFontAtlasBuilder.BakedMap`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2454) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf1881d7a18b8d2f"></a>[`public Inno.Native.ImGui.ImStableVectorImFontBaked32 Inno.Native.ImGui.ImFontAtlasBuilder.BakedPool`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2453) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d05f5b778937bf38"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.FrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2448) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4055ca66eec31054"></a>[`public byte Inno.Native.ImGui.ImFontAtlasBuilder.LockDisableResize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2451) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c39e7043d476c10c"></a>[`public Inno.Native.ImGui.ImVec2IC Inno.Native.ImGui.ImFontAtlasBuilder.MaxRectBounds`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2450) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3238120006c20c81"></a>[`public Inno.Native.ImGui.ImVec2IC Inno.Native.ImGui.ImFontAtlasBuilder.MaxRectSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2449) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5756f3300af7c602"></a>[`public Inno.Native.ImGui.StbrpContextOpaque Inno.Native.ImGui.ImFontAtlasBuilder.PackContext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2438) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01690bdb8728dd49"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.PackIdLinesTexData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2457) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-96490c8e8f3d9dac"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.PackIdMouseCursors`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2456) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-17d9fe15747236cd"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.StbrpNode> Inno.Native.ImGui.ImFontAtlasBuilder.PackNodes`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2439) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5b0e00ddef13d681"></a>[`public byte Inno.Native.ImGui.ImFontAtlasBuilder.PreloadedAllGlyphsRanges`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2452) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cab3df087ccd4cd8"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureRect> Inno.Native.ImGui.ImFontAtlasBuilder.Rects`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2440) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6e661214496fb7da"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.RectsDiscardedCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2446) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0b94b58ef6679ebc"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.RectsDiscardedSurface`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2447) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39be156881fa640b"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontAtlasRectEntry> Inno.Native.ImGui.ImFontAtlasBuilder.RectsIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2441) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-867057279c285ced"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.RectsIndexFreeListStart`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2443) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab76ffcf0c470133"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.RectsPackedCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2444) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-977c3854f60cd7ee"></a>[`public int Inno.Native.ImGui.ImFontAtlasBuilder.RectsPackedSurface`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2445) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-401799921c5db2ff"></a>[`public Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImFontAtlasBuilder.TempBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2442) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f831194241b3ef4f"></a>[`public Inno.Native.ImGui.ImFontAtlasBuilder.ImFontAtlasBuilder(Inno.Native.ImGui.StbrpContextOpaque packContext = default(Inno.Native.ImGui.StbrpContextOpaque), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.StbrpNode> packNodes = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.StbrpNode>), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureRect> rects = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureRect>), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontAtlasRectEntry> rectsIndex = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontAtlasRectEntry>), Inno.Native.ImGui.ImVector<byte> tempBuffer = default(Inno.Native.ImGui.ImVector<byte>), int rectsIndexFreeListStart = 0, int rectsPackedCount = 0, int rectsPackedSurface = 0, int rectsDiscardedCount = 0, int rectsDiscardedSurface = 0, int frameCount = 0, Inno.Native.ImGui.ImVec2IC maxRectSize = default(Inno.Native.ImGui.ImVec2IC), Inno.Native.ImGui.ImVec2IC maxRectBounds = default(Inno.Native.ImGui.ImVec2IC), byte lockDisableResize = 0, byte preloadedAllGlyphsRanges = 0, Inno.Native.ImGui.ImStableVectorImFontBaked32 bakedPool = default(Inno.Native.ImGui.ImStableVectorImFontBaked32), Inno.Native.ImGui.ImGuiStorage bakedMap = default(Inno.Native.ImGui.ImGuiStorage), int bakedDiscardedCount = 0, int packIdMouseCursors = 0, int packIdLinesTexData = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2458) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-982c4c116a146329"></a>[`public void Inno.Native.ImGui.ImFontAtlasBuilder.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67684) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2277e6ddb199d332"></a>[`public struct Inno.Native.ImGui.ImFontAtlasBuilder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2436) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
