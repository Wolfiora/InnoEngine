# Inno.Native.Sdl3 ABI · SDL → SDLAtomicInt

[项目与生命周期](../Inno.Native.Sdl3.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.Sdl3.SDL</code> · 2161–2340</summary>

<!-- api: M:Inno.Native.Sdl3.SDL.SetenvUnsafe(System.String,System.String,System.Int32) 78dd2eae4cf5b23a37f922f648cc94b2db8149967765f0f014aad78c8c969689 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetErrorV(System.Byte*,System.Byte*) 7ee41003d989e2224592d889315bdaeef59ac55dff8787d64f0756c14a169203 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetEventEnabled(System.UInt32,System.Boolean) 925fa069cb945700b1ad32ce96220dc5c474d6df7e47d194b5b9c6be0389ebb4 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetEventEnabled(System.UInt32,System.Byte) dc2d21ad2f9de4f9edc06fa9c25cd7016046ff023fe573ae3da41786ef87de25 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetEventFilter(,System.Void*) 6adbcc8797a3385e0083173f177912ce08035ba9a21e45cb01406a0c5f67e0ab -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetFloatProperty(System.UInt32,System.Byte*,System.Single) 89dbb92c24964f5ee983ad72128a38555d10b6761b4881b47de508e37ea73f6f -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetFloatProperty(System.UInt32,System.String,System.Single) 6e10ee0400d2eac8c42859d866b00b6922596006e1b968539052ac6942aaa136 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGamepadEventsEnabled(System.Boolean) bc069dc40f07760e4784b39d76000c73eea1f6240c394dd5da00e116d1204470 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGamepadEventsEnabled(System.Byte) af8f59dee8a5ec98dbe1c28f9d36ad1c3f5fc8781ee9a5983e4c77d94ad2d457 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGamepadLED(Inno.Native.Sdl3.SDLGamepad,System.Byte,System.Byte,System.Byte) 96cb631cba1b847f8c35a793cebfd0edf9f2f20b2218f1c26e87d1cb16876a11 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGamepadMapping(System.UInt32,System.Byte*) 262a4b8fdbd2cc64598d65e401fc2d01b3f239851c2ec72f09b768f02295160e -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGamepadMapping(System.UInt32,System.String) 4c5944a945862098ab202ca33fc96672a9c1ac53e06a012e8a4b2a7a01e804a8 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGamepadPlayerIndex(Inno.Native.Sdl3.SDLGamepad,System.Int32) 44e2f374d9f3ad01a1c5ef461376aaa6724e91588cadcac54344e2aaa9a728ac -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGamepadSensorEnabled(Inno.Native.Sdl3.SDLGamepad,Inno.Native.Sdl3.SDLSensorType,System.Boolean) 1dc36a295d54d0868163fd1ed41027692de8b61c564e3bba9059983e12f93fca -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGamepadSensorEnabled(Inno.Native.Sdl3.SDLGamepad,Inno.Native.Sdl3.SDLSensorType,System.Byte) 7b8a0c977d6cdf14606ecf96a9fbf6637ab8c332bdd2667a9f4276464fd5ad4a -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPUAllowedFramesInFlight(Inno.Native.Sdl3.SDLGPUDevice,System.UInt32) 05d7e1193d2f03b3296b763711874f2e2031be56e202b18ca90388663066159a -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPUBlendConstants(Inno.Native.Sdl3.SDLGPURenderPass,Inno.Native.Sdl3.SDLFColor) 96d9d2983f0a06a5837c127405842ceb695f4a3155d745b8538eb66049693d5a -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPUBufferName(Inno.Native.Sdl3.SDLGPUDevice,Inno.Native.Sdl3.SDLGPUBuffer,System.Byte*) f44bb305f67852baea5ca09d2d1e48b3a65e9ab39a5e42bc093db91015c686ae -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPURenderState(Inno.Native.Sdl3.SDLRenderer,Inno.Native.Sdl3.SDLGPURenderState) 871532f85fe9fc0367bfcdf69af861b2dd7e350e514b370019feed62898ecd5a -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPURenderStateFragmentUniforms(Inno.Native.Sdl3.SDLGPURenderState,System.UInt32,System.Void*,System.UInt32) 568a9a3e0752fdb5698d83417af8677b3c7133aec84e3fc1a9bacbd71cb70a62 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPURenderStateSamplerBindings(Inno.Native.Sdl3.SDLGPURenderState,System.Int32,Inno.Native.Sdl3.SDLGPUTextureSamplerBinding@) 0786eab1cb32f22a6ebcc669e2d5659dbbc6828152d766c0fb78dd008c4520ca -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPURenderStateSamplerBindings(Inno.Native.Sdl3.SDLGPURenderState,System.Int32,Inno.Native.Sdl3.SDLGPUTextureSamplerBindingPtr) 35517521aa50e591d5275ad313b5f0152f066650fcae000b385338cbbd43ec51 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPURenderStateStorageBuffers(Inno.Native.Sdl3.SDLGPURenderState,System.Int32,Inno.Native.Sdl3.SDLGPUBuffer*) ead36c07b23ef4e20e1145a1c935fb59f034ba231e4fa85a12f45ac21ec672d4 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPURenderStateStorageTextures(Inno.Native.Sdl3.SDLGPURenderState,System.Int32,Inno.Native.Sdl3.SDLGPUTexture*) 093e77a6c82a8e5e22a87564fc5b2bfff454f95e42baf2a30c91e5331d9c213f -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPUScissor(Inno.Native.Sdl3.SDLGPURenderPass,Inno.Native.Sdl3.SDLRect@) c0b659e518af8d9b69cf13241b00d7fecafd01d2334d03675312f42bbf5a1c30 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPUScissor(Inno.Native.Sdl3.SDLGPURenderPass,Inno.Native.Sdl3.SDLRectPtr) a0fa628e84b6bd16ce1be9b1991514b785598768491d97042f5ba0669389fb63 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPUStencilReference(Inno.Native.Sdl3.SDLGPURenderPass,System.Byte) e70eb3beb62a498fc6ba79dfacb905727528b4ca4b8e25f4bf76f5015439d766 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPUSwapchainParameters(Inno.Native.Sdl3.SDLGPUDevice,Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLGPUSwapchainComposition,Inno.Native.Sdl3.SDLGPUPresentMode) 8341622a52b668a00d41c5e34093b52284d72b6f983e364d4495310c56ad4ff9 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPUTextureName(Inno.Native.Sdl3.SDLGPUDevice,Inno.Native.Sdl3.SDLGPUTexture,System.Byte*) 229715b62587a3db0e5805c721c7cce68e2ac049adf29511b50c6275cbbefea0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPUTextureName(Inno.Native.Sdl3.SDLGPUDevice,Inno.Native.Sdl3.SDLGPUTexture,System.String) 6387ad23cd817449bccb50a9367992fd1a522ca671555caac11d4d8de0adbc07 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPUViewport(Inno.Native.Sdl3.SDLGPURenderPass,Inno.Native.Sdl3.SDLGPUViewport@) 8bf853206464d71576579574d2bcd563cf8c8dee260ee76c4a063a4b738934ff -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetGPUViewport(Inno.Native.Sdl3.SDLGPURenderPass,Inno.Native.Sdl3.SDLGPUViewportPtr) 25ec53f32361a3769c361fd2b899f24e59a83e2ed3cb7f0ef1512f9b723e083c -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetHapticAutocenter(Inno.Native.Sdl3.SDLHaptic,System.Int32) a1922e2d3df141543164303168db865fcd16d4a7ef600c06ac87be9c4f7a3c38 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetHapticGain(Inno.Native.Sdl3.SDLHaptic,System.Int32) 7f04abfd5d2f175bcfc4d53ea47969e6750128f22eb50e9a0ae6afb5d26a22ae -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetHint(System.Byte*,System.Byte*) 6bee5a7b75dba1bf560789f1ee6d00fd1a39d604376d9e8a1d4680dd5faa50eb -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetHint(System.String,System.String) f2372d03ce5e74f4d4c74bbf831c5ef611d7fa95af339f0e1d7d8f41e2ebbb6f -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetHintWithPriority(System.Byte*,System.Byte*,Inno.Native.Sdl3.SDLHintPriority) 5c86b00c89aeb03966557c57cdd18507085263ea5a64865efa430d51272ad4d0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetHintWithPriority(System.String,System.String,Inno.Native.Sdl3.SDLHintPriority) 141db19b48c60f1f1662bed31610711e5f09c6b5e74c3bee563056e79dba509a -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetInitialized(Inno.Native.Sdl3.SDLInitState@,System.Byte) c4194179f23ab3547986ab4e9e26867691031fed0cc851cece6b8a59474c3bb3 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetInitialized(Inno.Native.Sdl3.SDLInitStatePtr,System.Boolean) 28ab2224beba26a33aeb90638266ce14d2866a527ec5b38e1fefedd579d233b4 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetInitialized(Inno.Native.Sdl3.SDLInitStatePtr,System.Byte) 11629a0c981962845801eae7cc4ec71fb55cb36a50a2ce219ec0db4aad092f7a -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetJoystickEventsEnabled(System.Boolean) db198a447336eda6957c97413daee49e4af28bd9a614d8e3b45a66b0e60fe383 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetJoystickEventsEnabled(System.Byte) 3e10b436489f2c354eeeb21bcb6d2a2c050c3d75076108bda8388e58fd53f351 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetJoystickLED(Inno.Native.Sdl3.SDLJoystick,System.Byte,System.Byte,System.Byte) d6bff3431fb5dbb05646c378a5fd27fa1e7732ca782824a600b3d38e61b5118c -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetJoystickPlayerIndex(Inno.Native.Sdl3.SDLJoystick,System.Int32) 1b8e93a3d0e3c28bb63fa099548f8a4f1bca96ae026643d42403375fbc6e4773 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetJoystickVirtualAxis(Inno.Native.Sdl3.SDLJoystick,System.Int32,System.Int16) b5e0d9531383ee7b47c8ad8270d77a61b4c9ecb24282f6a5d2d1ad7c5cb1dc6b -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetJoystickVirtualBall(Inno.Native.Sdl3.SDLJoystick,System.Int32,System.Int16,System.Int16) 59e9f520f348532e796b46936fa1fed8d46c1f64dc72ca9737c07ea9190566d0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetJoystickVirtualButton(Inno.Native.Sdl3.SDLJoystick,System.Int32,System.Boolean) 335eabdf9f00ad2f0dfc7b7231cc4a925c9ad46decd22ea9b42e48ee3a8db02a -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetJoystickVirtualButton(Inno.Native.Sdl3.SDLJoystick,System.Int32,System.Byte) d471f82bb0019b6cb4c0e19b44fdb36892fc3a15b404817ea3bb6dbd5395e019 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetJoystickVirtualHat(Inno.Native.Sdl3.SDLJoystick,System.Int32,System.Byte) 8807d0eca487494cc6157b7ff435bc02c50414217f926d0f70b70ca2a7d5b83c -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetJoystickVirtualTouchpad(Inno.Native.Sdl3.SDLJoystick,System.Int32,System.Int32,System.Boolean,System.Single,System.Single,System.Single) 40079336ce9496a3454c78fe8d7e438f02784c0b3d9c353ac9f30464b867b33f -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetJoystickVirtualTouchpad(Inno.Native.Sdl3.SDLJoystick,System.Int32,System.Int32,System.Byte,System.Single,System.Single,System.Single) ae6e2983780da679553eddaafbdc859eaee7df71220a28e060ad24ae9989b6d0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetLogOutputFunction(,System.Void*) 33142ffa81b0da6ec298c1b023965896a27d1cdb75142f8aa24bcaef8911db00 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetLogPriorities(Inno.Native.Sdl3.SDLLogPriority) c4be79507134f3170be4c823111109eb15a13407e4f427c4aa9a6c09806cb0ce -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetLogPriority(System.Int32,Inno.Native.Sdl3.SDLLogPriority) 06c723aada93ec22c9cac65247a9ac89e5319b8d4199350aa18c10f15b68ba19 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetLogPriorityPrefix(Inno.Native.Sdl3.SDLLogPriority,System.Byte*) aa1bf4a3650ebfb66e6a7c193fa836ead3a6c0e73d1d811c252d9cb6422be2f8 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetLogPriorityPrefix(Inno.Native.Sdl3.SDLLogPriority,System.String) 7815690d7e6f7b2e59f6393d104c47c059eb152708bcd48e9e97230b9a8097a8 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetMemoryFunctions(,,,) 29db0940566ee36877243cc0edc0a6713ae19e741f883440b0110c07f0e3abfb -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetModState(Inno.Native.Sdl3.SDLKeymod) 1a460de9af2bc476e870905eac817dd8c891f910cf895785913e8f6a838a0185 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetNumberProperty(System.UInt32,System.Byte*,System.Int64) 12d3a65822acffffe3d100c7393e4d68115a4123ddf8d427a3467a013d7cd704 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetNumberProperty(System.UInt32,System.String,System.Int64) 618209eec64b91f0ffe42aca08af42b7fd01aea53929b09bc9f935a68eaca796 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetPaletteColors(Inno.Native.Sdl3.SDLPalette@,Inno.Native.Sdl3.SDLColor@,System.Int32,System.Int32) eebc433c8ac09fe7cf184bf8caba0842f65d26a10fce0c3cc6aa10f14768c4f1 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetPaletteColors(Inno.Native.Sdl3.SDLPalettePtr,Inno.Native.Sdl3.SDLColorPtr,System.Int32,System.Int32) ee9ba784840c0e1c7c9b7cce87ce6bfd2b3eb91d5107d66e818efe50f7acd120 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetPointerProperty(System.UInt32,System.Byte*,System.Void*) bcdcc13bf437fe63eb475f0b03222db7ea66cf2983f3104add9b63a734c54bc9 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetPointerProperty(System.UInt32,System.String,System.Void*) 2ca9714e485fceea9ec5d5c6209d77d257b2c6e0e0bb965802b2e9be87d7e857 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetPointerPropertyWithCleanup(System.UInt32,System.Byte*,System.Void*,,System.Void*) 59fcb7ee898e1fbaf0399deab680880798dbdad79183ae7d977c0ff6ccf6e4a8 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetPrimarySelectionText(System.Byte*) 241097e5c3b252cc302f70281e3173d4f9bb9569a825ac3b88d53886131858bd -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetPrimarySelectionText(System.String) 2fc68601af7c98e5a4ae8b9b2c773612ecf2a21ccaebaeeb7d0d680089f7d800 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRelativeMouseTransform(,System.Void*) 1835aa4eab36eb223d1b6d3d1a12ea5a4b3eb0a9a37ecb9329dd875a745de0b2 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderClipRect(Inno.Native.Sdl3.SDLRenderer,Inno.Native.Sdl3.SDLRect@) c92dfb12eda461b526fbed0978b8a67578d783d4085b0f6918c299463f9858e1 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderClipRect(Inno.Native.Sdl3.SDLRenderer,Inno.Native.Sdl3.SDLRectPtr) fb20dd913dab69c5e2a7cc78e71bda2ade0f33f29fbec5db84be7e95d316db15 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderColorScale(Inno.Native.Sdl3.SDLRenderer,System.Single) 2fe2f45116e082e7e872b264e392a0586225841e12662499199c84c0c6daca3f -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderDrawBlendMode(Inno.Native.Sdl3.SDLRenderer,System.UInt32) 0aa1fbf711e5dd9e7a103acb22e9cb79ca36634be19f3946a38866bdb54a4663 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderDrawColor(Inno.Native.Sdl3.SDLRenderer,System.Byte,System.Byte,System.Byte,System.Byte) c7e903ff2c2bcfd7217ce556024281431d008499b49617928cdae62cd4d2c899 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderDrawColorFloat(Inno.Native.Sdl3.SDLRenderer,System.Single,System.Single,System.Single,System.Single) d30a9d9f889dc9ccc7da5ada195a5e37b2983a6fd31d7de3798e623dc100a5c9 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderLogicalPresentation(Inno.Native.Sdl3.SDLRenderer,System.Int32,System.Int32,Inno.Native.Sdl3.SDLRendererLogicalPresentation) d3a64e7c3c88f04b1b48d2472177c56b96397b7f166a8730e759e1824e91bddc -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderScale(Inno.Native.Sdl3.SDLRenderer,System.Single,System.Single) 957c6440bef851f30cb4deca2a353c10d8c95cf4cebc8d441e4e48583146c92c -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderTarget(Inno.Native.Sdl3.SDLRenderer,Inno.Native.Sdl3.SDLTexture@) 02f278f2af918093aa53de4c124115c7bc9b12d66c46dc74835602219b3e4b21 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderTarget(Inno.Native.Sdl3.SDLRenderer,Inno.Native.Sdl3.SDLTexturePtr) c55d563355fd97c02a5904d7c082042bca00b1e3b3956f0b6d9f4f834ff41fd7 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderTextureAddressMode(Inno.Native.Sdl3.SDLRenderer,Inno.Native.Sdl3.SDLTextureAddressMode,Inno.Native.Sdl3.SDLTextureAddressMode) bbead4486fcd5264b49146cf9ad108e5e028daede8c90122284c3d52c7831206 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderViewport(Inno.Native.Sdl3.SDLRenderer,Inno.Native.Sdl3.SDLRect@) 712f16c20cfe57b4edda338525a269f0b0587119df1200d5c59a734c15885902 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderViewport(Inno.Native.Sdl3.SDLRenderer,Inno.Native.Sdl3.SDLRectPtr) 3843db326f7d5968cc1de1581c04858a464dad216a7d650f8bc74b983ecaa90f -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetRenderVSync(Inno.Native.Sdl3.SDLRenderer,System.Int32) 9d5f6d6cfb2ee123e06953f5b173ede6f1e078f48bbaa25b37f254495c913f09 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetScancodeName(Inno.Native.Sdl3.SDLScancode,System.Byte*) b10cb73ce5a93565732e9212d0a07e7d98261dfffa09464714a2f1f7df2be6cc -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetScancodeName(Inno.Native.Sdl3.SDLScancode,System.String) b732371f4c0b97a06bca753c263ab343a949c4ecf5123e8083bf486ba49a292e -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetStringProperty(System.UInt32,System.Byte*,System.Byte*) 3db39508d132061c02ea25769cd031652252102d159f85b8d44406a100bb62b0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetStringProperty(System.UInt32,System.String,System.String) bd7271759fd24d65a1f6da2c4b9476850f492378b955066d7a18d0aed6e61863 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceAlphaMod(Inno.Native.Sdl3.SDLSurface@,System.Byte) 39099f0b064bd4a96ffeb8476086fc57f79257a902b9a792eeda01d77bd71392 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceAlphaMod(Inno.Native.Sdl3.SDLSurfacePtr,System.Byte) 8262e5997ada8c85a4f754d6aa0ceded0d4f3306e2855c4b2049d3f2da0bb49b -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceBlendMode(Inno.Native.Sdl3.SDLSurface@,System.UInt32) 01be346dadd056460e5d3fdbea0847e14e22d9fcd05d896a41bdfde3140daacf -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceBlendMode(Inno.Native.Sdl3.SDLSurfacePtr,System.UInt32) c80599c73759de743f1d0826d7b6e7bccabee3b98033a98ba177fe91d23be7c7 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceClipRect(Inno.Native.Sdl3.SDLSurface@,Inno.Native.Sdl3.SDLRect@) 6390ff805310de4d08958bda8b2f7cd7dfced131c5e91a67eafdf18ab895790c -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceClipRect(Inno.Native.Sdl3.SDLSurfacePtr,Inno.Native.Sdl3.SDLRectPtr) 4fffd3307d3d2922935acba733748bb2cfad67807af4e5c229ae3ed71e9bb99e -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceColorKey(Inno.Native.Sdl3.SDLSurface@,System.Byte,System.UInt32) 1fb08916c7fa3a339ad72f104252dbb151c5058a616df9b4f52ee93b26f84896 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceColorKey(Inno.Native.Sdl3.SDLSurfacePtr,System.Boolean,System.UInt32) 3e95330c7bd882f9dccc9993150078a16e4539138e674c6b724f02c7264128f4 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceColorKey(Inno.Native.Sdl3.SDLSurfacePtr,System.Byte,System.UInt32) fbd9332f850cd462a174cffaf06a2bd25b341c9d308cdda3e343b567deea476b -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceColorMod(Inno.Native.Sdl3.SDLSurface@,System.Byte,System.Byte,System.Byte) acc933136fd3f21c56263f2695a030f7889bb03757bee76bcc342adfbce4e2a0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceColorMod(Inno.Native.Sdl3.SDLSurfacePtr,System.Byte,System.Byte,System.Byte) 0c567847fd6c1f6be6a052a615c4a76459c9daed87daa351036b784e5a3b8ff4 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceColorspace(Inno.Native.Sdl3.SDLSurface@,Inno.Native.Sdl3.SDLColorspace) 09a35d4af0ff70e5ab35cde2fefdc561f6bc9d2038f9726a82d033333d0985bb -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceColorspace(Inno.Native.Sdl3.SDLSurfacePtr,Inno.Native.Sdl3.SDLColorspace) d98a6817ffa407a5fffdc7d85991f514a4113777c67c21e472bbefae21b61e48 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfacePalette(Inno.Native.Sdl3.SDLSurface@,Inno.Native.Sdl3.SDLPalette@) 6e63ff0593925204d376acb9aab3630f54c2e60649ae87340cdd1fca91efec2c -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfacePalette(Inno.Native.Sdl3.SDLSurfacePtr,Inno.Native.Sdl3.SDLPalettePtr) 36d8b2b189a5f59d29e6d59142312732848d6a2003a2f5b596f8800f7d4542ed -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceRLE(Inno.Native.Sdl3.SDLSurface@,System.Byte) eed8a08646b49643b553bda8502ba35cf8f1ecf9feda03161ef117dea777ba64 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceRLE(Inno.Native.Sdl3.SDLSurfacePtr,System.Boolean) dfa38566933234be60cbda4b58f200675c9ffb004cc739ee545c84f759b615a7 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetSurfaceRLE(Inno.Native.Sdl3.SDLSurfacePtr,System.Byte) 04f05dbab9f1b63691d281dc9bf735b4716a510361bfc9ea06f3b1b5088508f2 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextInputArea(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLRect@,System.Int32) d45f8eb917b55abdd93e9f8462c05e28d995b2626aff2d57ac7fc4883783bcfa -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextInputArea(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLRectPtr,System.Int32) 79e73158dbe9d34a7764444d5161dfede0adc33de1e7f5575b06be48055b8afc -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextureAlphaMod(Inno.Native.Sdl3.SDLTexture@,System.Byte) f8373557325f987c1813b59bd9365aaa77f23aaadf12b61c8261a5ca7ae74a5d -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextureAlphaMod(Inno.Native.Sdl3.SDLTexturePtr,System.Byte) b3ef368b964401820faebb896dcd6c8a28ff0ee3d6fd5468d6f46aa0e6f9cec9 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextureAlphaModFloat(Inno.Native.Sdl3.SDLTexture@,System.Single) 2a1fad0176e2a8644b9db83b715ea14d285ddfef9b1d57931bd69eb4b6c48bf4 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextureAlphaModFloat(Inno.Native.Sdl3.SDLTexturePtr,System.Single) 3481579d1f610ace3e1f8a48c002949e71fd3f5448711720e88456fd93b47f86 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextureBlendMode(Inno.Native.Sdl3.SDLTexture@,System.UInt32) ea37769d1d5a7c401a1ebb2dc5fd931dbcfcb36d690ba21aeadfc1cec2d62088 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextureBlendMode(Inno.Native.Sdl3.SDLTexturePtr,System.UInt32) e65fcddfea74f985aebabe078f4e9da028a44c295365f518cd5d58a93f043629 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextureColorMod(Inno.Native.Sdl3.SDLTexture@,System.Byte,System.Byte,System.Byte) 6404885a625335d821e5dd2fbee0c0200c42121ab88bf38a3d6af451d3a15c9f -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextureColorMod(Inno.Native.Sdl3.SDLTexturePtr,System.Byte,System.Byte,System.Byte) 2a4c975d5208824d069f0a2a1fc0b16b33a37b7a645ffd45702b38ba9f874782 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextureColorModFloat(Inno.Native.Sdl3.SDLTexture@,System.Single,System.Single,System.Single) ead39fad71b9b550481747e621a6117974aa3bf9cdd89d813287f0ba55544c72 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextureColorModFloat(Inno.Native.Sdl3.SDLTexturePtr,System.Single,System.Single,System.Single) eeab657e4aa4defca0d0fd96719e5afd5f25b5e2b1f0c6fa3ec7d68d8a9bbddd -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTexturePalette(Inno.Native.Sdl3.SDLTexture@,Inno.Native.Sdl3.SDLPalette@) 087533f8f1f5d66affc9a9309e9137bf1bbb73c6224f68b0e94f3056d93be182 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTexturePalette(Inno.Native.Sdl3.SDLTexturePtr,Inno.Native.Sdl3.SDLPalettePtr) 6825b0476fd7b001bba671736d6af93c0cb1283cc62a7f5788d1900c105a5dd1 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextureScaleMode(Inno.Native.Sdl3.SDLTexture@,Inno.Native.Sdl3.SDLScaleMode) bf2e6dfd0d0767c88605ca60f04732c6c685c1f16b7bfd1bf546a5d5eea2f54f -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTextureScaleMode(Inno.Native.Sdl3.SDLTexturePtr,Inno.Native.Sdl3.SDLScaleMode) a0ef5632bf18e9ab268e981f992ba574b6a5836880f2c9284e40d8f58b135fd4 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTLS(Inno.Native.Sdl3.SDLAtomicInt*,System.Void*,) 74642c0b075e1040a18de3c7227d47fc839abc8c3d4b7c9f56199b01b3159419 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTrayEntryCallback(Inno.Native.Sdl3.SDLTrayEntry,,System.Void*) 0393dedecdda6a7dc2f2a06016faebe937ad8270feee89ca732cadc5ce8ae77b -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTrayEntryChecked(Inno.Native.Sdl3.SDLTrayEntry,System.Boolean) 055ff1d5cc494b9dc868fb1e2b27ffcf99613d9fe88f7223cd88b6372e5d4ec8 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTrayEntryChecked(Inno.Native.Sdl3.SDLTrayEntry,System.Byte) d556f6424d80888dbf0f57987b32ef64aa12503a8e7b516830f1f355e12a546a -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTrayEntryEnabled(Inno.Native.Sdl3.SDLTrayEntry,System.Boolean) aef42776ab8fa59a04fe687da8c413775baec65648001b02d42b9cbe50816ba0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTrayEntryEnabled(Inno.Native.Sdl3.SDLTrayEntry,System.Byte) 90a5e6d6ec6b282a10dfe58b4a2aabb25bfa11f8fe53327c1e66ebb4b72d2d71 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTrayEntryLabel(Inno.Native.Sdl3.SDLTrayEntry,System.Byte*) 242d2881157fc7c7996f81a9084b4e26a189428c4c86aaf9a96f201ae712f7f8 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTrayEntryLabel(Inno.Native.Sdl3.SDLTrayEntry,System.String) a3195215cac3c01769635ff66d122d08fe288207de244be23db7ed575d5f9e65 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTrayIcon(Inno.Native.Sdl3.SDLTray,Inno.Native.Sdl3.SDLSurface@) 0a6e6b417ab26cda21b88bf7df13dc7940e4a52ec5adcf21fe3753ab6749439b -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTrayIcon(Inno.Native.Sdl3.SDLTray,Inno.Native.Sdl3.SDLSurfacePtr) ff078505954555eb388e14a82500ad91d894c1cbabe4218ba5fbe920616d8b2f -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTrayTooltip(Inno.Native.Sdl3.SDLTray,System.Byte*) 04e396cda15b3673f76af4c57508b3e125b902f936e3ec6e0935a04f8f5589ce -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetTrayTooltip(Inno.Native.Sdl3.SDLTray,System.String) f9f5a926aae643c41db23318cc58aa90c40200159029854d95fd8741ff4402cc -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowAlwaysOnTop(Inno.Native.Sdl3.SDLWindow,System.Boolean) 0f76d45b43e07956833a5ba90abd16439a0be792339d5824f3cb1d5da8ac45cc -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowAlwaysOnTop(Inno.Native.Sdl3.SDLWindow,System.Byte) e8dfcf7d66d2e4ac1b31768a9e29c26c60cfd2513aaa2a103797c63ec5bd5702 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowAspectRatio(Inno.Native.Sdl3.SDLWindow,System.Single,System.Single) d5aa4fff558191f7acb101427b80f797a3b69c05ac382eb734f4828fa90c785a -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowBordered(Inno.Native.Sdl3.SDLWindow,System.Boolean) fa95efb64faf86dc070e1ba8a7c634bfd4934fd7d691759d8ea769847271f98b -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowBordered(Inno.Native.Sdl3.SDLWindow,System.Byte) 7eab0a21335ad9354f7ba08c31d3f0e84670b9bd7abd399181527172fe847641 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowFillDocument(Inno.Native.Sdl3.SDLWindow,System.Boolean) 98994122dbc927777b1bcee10aeb8fbb32d314afb94d7c6d0c5211aaa8d1be8a -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowFillDocument(Inno.Native.Sdl3.SDLWindow,System.Byte) 6e4b881e138764f3d4d3f17f973b268458999108355a594f78c9304996e06db3 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowFocusable(Inno.Native.Sdl3.SDLWindow,System.Boolean) b1619b24749543b94c9414b943a6a618b66251599b6c84a041551f178a6fc1e4 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowFocusable(Inno.Native.Sdl3.SDLWindow,System.Byte) f51cecd4dc297a7b5b764075284ecdae18338bfea275e0b377c1cd2f0e278b93 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowFullscreen(Inno.Native.Sdl3.SDLWindow,System.Boolean) 8b5305277c16b88669b64910c1794480f70e34ca4bbe96a6f1fab22de2cff3dc -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowFullscreen(Inno.Native.Sdl3.SDLWindow,System.Byte) 45cfa1d06b4fa3413a3888a4da0a95c2e1a75d376d791fc342e18e33530f1b54 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowFullscreenMode(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLDisplayMode@) 9a88ab840f6c19c5d0c9cf74ab122996476f27caa94a32a5a55268f527aacab6 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowFullscreenMode(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLDisplayModePtr) ae45ae4f23ff8b342ca3ec1bcb5fae6fa1f6853b735224d6f9edeba6b0a5c331 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowHitTest(Inno.Native.Sdl3.SDLWindow,,System.Void*) 6c118fc317372b39371b56fc3f2a9785b15887aa0f880213f671f4b696577910 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowIcon(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLSurface@) f656821cd3a0cc5b8204d3a73f1dc3c4ddf202f2a8459ea91f661f76ede195e3 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowIcon(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLSurfacePtr) 13e539f044e54266552f87bea71bd7ae224aa0d94d017147aacef7d3ec8465d0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowKeyboardGrab(Inno.Native.Sdl3.SDLWindow,System.Boolean) 622e5b656bf65c86b36a937403e17b00c56b57c54414f44bb27166c252d1308a -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowKeyboardGrab(Inno.Native.Sdl3.SDLWindow,System.Byte) 789a46c012c200c9ad572780e4123ec273c0b7b3ec6ec74ce20897e6758da8f0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowMaximumSize(Inno.Native.Sdl3.SDLWindow,System.Int32,System.Int32) 0b0fdb73fca41cc65a237a4192cda2575fa7494b88e9cff03e3df6feabae458c -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowMinimumSize(Inno.Native.Sdl3.SDLWindow,System.Int32,System.Int32) 0cd2cffef50ed3b51782dea7cf7c115d81620331ab2181fa0bb516f0e5073913 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowModal(Inno.Native.Sdl3.SDLWindow,System.Boolean) 08d167efe6fffdb639155e6a3a1d0fa4fbd13408ffe063fb9ab57d397b4492a8 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowModal(Inno.Native.Sdl3.SDLWindow,System.Byte) c39331b212e2e875d0c88b786a99a964141591ef91047358cf074a7378e4ecb3 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowMouseGrab(Inno.Native.Sdl3.SDLWindow,System.Boolean) 64b9b3673d9bc1c8601e600ef27949a93d49c16cb002741e1f5b8e8d3875014f -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowMouseGrab(Inno.Native.Sdl3.SDLWindow,System.Byte) e2c9fddb55397e33532bcf96b29d8753c51d751aad28a796f73c184ea41b92f8 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowMouseRect(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLRect@) e3662f4c50a083f980d4014871a1571458dfef627870d205c226c71be2ba435d -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowMouseRect(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLRectPtr) a22ba22e11095f87cb222bd6537f06dad398ff6e7d9284bb52c2d96794302402 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowOpacity(Inno.Native.Sdl3.SDLWindow,System.Single) 74b8cd2581d0a4a166a929e59bec374311fc8c0c0b0b048030442a779d3e77c2 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowParent(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLWindow) 8fe1e518d3c4cfbd583d0ca15e163ec74f5a6077ed7df3cab4214b86fda4d609 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowPosition(Inno.Native.Sdl3.SDLWindow,System.Int32,System.Int32) 52214bd0dbd171b489b56f17e16b42d519ef32876e01a9c0b0aeec3cc1619c8c -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowProgressState(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLProgressState) 953d0a4fa87bec7637e32565e5e2167945535f272517057788b790e3cd1915e6 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowProgressValue(Inno.Native.Sdl3.SDLWindow,System.Single) 779e7459b08ac8a06b1ac2032bb4448ab73e6aadcc6dce74de7b5102491e80ab -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowRelativeMouseMode(Inno.Native.Sdl3.SDLWindow,System.Boolean) 70a09e6698938640c03887e9c33b463f2748bc857328c67850bccf2d2a90a2a0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowRelativeMouseMode(Inno.Native.Sdl3.SDLWindow,System.Byte) 3ff521c4f1d3a10fe9a145acc619f4669395e52c25eab667c9832bfaf4b3b61b -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowResizable(Inno.Native.Sdl3.SDLWindow,System.Boolean) 6e2468ee382732eb353cb10d39d40808e87a9ec321d7b3db602f5b1c4bb49fe6 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowResizable(Inno.Native.Sdl3.SDLWindow,System.Byte) 353befc42980e791bddc76ce99e1fff6b652d1df7740608969ed1f2a2a8dd056 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowShape(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLSurface@) d265170bf9610c3d06ec9ebda4552633b041103db83bfdc3045b368434a7b338 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowShape(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLSurfacePtr) 9b0820ded785c1f7c17d983097b386c04968c6115bf46186c509372f571e3482 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowSize(Inno.Native.Sdl3.SDLWindow,System.Int32,System.Int32) 1e1e0448ce43d730074d50b469433de0deb2d8491b89f3d5abfa074bb0053775 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowsMessageHook(,System.Void*) eee7f82874ad45b4728c5d202d5435e86914b61eeab3202bff3e87b477dd8d0d -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowSurfaceVSync(Inno.Native.Sdl3.SDLWindow,System.Int32) 9f42829df0149d4418e83d88616a9e10b00fb53014432a1f5f850a72cd56d9e7 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowTitle(Inno.Native.Sdl3.SDLWindow,System.Byte*) e6a3095194417f11ef6e04592b2c7970bdebc5ef6b9d2d6715a813771727354f -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetWindowTitle(Inno.Native.Sdl3.SDLWindow,System.String) b96392c9041177d24ddb7f05c260c00cd2fe457c12ce1e482f383bb9601a1032 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SetX11EventHook(,System.Void*) 06e417af3fc3680ad2849a014df35435a6d8c09b5ec3f3ecd08acb53f6fcf08e -->
<!-- api: M:Inno.Native.Sdl3.SDL.ShouldInit(Inno.Native.Sdl3.SDLInitState@) a7dfa9031eaa76b8c40ca1b11fdfd77633c66c942c698355bf56d62945bf5506 -->
<!-- api: M:Inno.Native.Sdl3.SDL.ShouldInit(Inno.Native.Sdl3.SDLInitStatePtr) 80fcd4fe0324d562a92de0c8f7e54ba4f780354cee494136e213f3b8127d7545 -->
<!-- api: M:Inno.Native.Sdl3.SDL.ShouldQuit(Inno.Native.Sdl3.SDLInitState@) 957556ed3731c9da8f7044a7ec0e3ea3067020ba672989db716625ffc01bfc65 -->
<!-- api: M:Inno.Native.Sdl3.SDL.ShouldQuit(Inno.Native.Sdl3.SDLInitStatePtr) efc58cbd8e03ae102ad6061915901dd9116d4356fc3ed42140e0ecc1a092d954 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dacf86211543014b"></a>[`public static int Inno.Native.Sdl3.SDL.SetenvUnsafe(string name, string value, int overwrite)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23209) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f0f97171656b344c"></a>[`public static byte Inno.Native.Sdl3.SDL.SetErrorV(byte* fmt, byte* ap)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24963) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f1b2af1052c779cb"></a>[`public static void Inno.Native.Sdl3.SDL.SetEventEnabled(uint type, bool enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31820) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9bc3b348eb9dfbbb"></a>[`public static void Inno.Native.Sdl3.SDL.SetEventEnabled(uint type, byte enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31814) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-46b1e5b3b686a097"></a>[`public static void Inno.Native.Sdl3.SDL.SetEventFilter(delegate* unmanaged[Cdecl]<void*, Inno.Native.Sdl3.SDLEvent*, byte> filter, void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31782) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-43d69353ed763fe1"></a>[`public static byte Inno.Native.Sdl3.SDL.SetFloatProperty(uint props, byte* name, float value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25108) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ca809cbc88586f6"></a>[`public static bool Inno.Native.Sdl3.SDL.SetFloatProperty(uint props, string name, float value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25115) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-63892d937228cc3d"></a>[`public static void Inno.Native.Sdl3.SDL.SetGamepadEventsEnabled(bool enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30866) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e14a2384dda9f20e"></a>[`public static void Inno.Native.Sdl3.SDL.SetGamepadEventsEnabled(byte enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30860) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-764a2666e6f124c9"></a>[`public static bool Inno.Native.Sdl3.SDL.SetGamepadLED(Inno.Native.Sdl3.SDLGamepad gamepad, byte red, byte green, byte blue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31120) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7829aa0d18b271f5"></a>[`public static byte Inno.Native.Sdl3.SDL.SetGamepadMapping(uint instanceId, byte* mapping)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30589) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-84e3242be1e1c648"></a>[`public static bool Inno.Native.Sdl3.SDL.SetGamepadMapping(uint instanceId, string mapping)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30596) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a6e3a89a562d9a9f"></a>[`public static bool Inno.Native.Sdl3.SDL.SetGamepadPlayerIndex(Inno.Native.Sdl3.SDLGamepad gamepad, int playerIndex)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30774) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-42d52e777a075d21"></a>[`public static bool Inno.Native.Sdl3.SDL.SetGamepadSensorEnabled(Inno.Native.Sdl3.SDLGamepad gamepad, Inno.Native.Sdl3.SDLSensorType type, bool enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31078) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-67a3c94a641febc8"></a>[`public static byte Inno.Native.Sdl3.SDL.SetGamepadSensorEnabled(Inno.Native.Sdl3.SDLGamepad gamepad, Inno.Native.Sdl3.SDLSensorType type, byte enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31071) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9bf054c7f186c67d"></a>[`public static bool Inno.Native.Sdl3.SDL.SetGPUAllowedFramesInFlight(Inno.Native.Sdl3.SDLGPUDevice device, uint allowedFramesInFlight)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32713) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6cb2bbdce0242a7e"></a>[`public static void Inno.Native.Sdl3.SDL.SetGPUBlendConstants(Inno.Native.Sdl3.SDLGPURenderPass renderPass, Inno.Native.Sdl3.SDLFColor blendConstants)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32325) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-40de6ef4146b7377"></a>[`public static void Inno.Native.Sdl3.SDL.SetGPUBufferName(Inno.Native.Sdl3.SDLGPUDevice device, Inno.Native.Sdl3.SDLGPUBuffer buffer, byte* text)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32169) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-71281a25c84a2cf9"></a>[`public static bool Inno.Native.Sdl3.SDL.SetGPURenderState(Inno.Native.Sdl3.SDLRenderer renderer, Inno.Native.Sdl3.SDLGPURenderState state)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35341) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-944e1f6b824ea3b6"></a>[`public static byte Inno.Native.Sdl3.SDL.SetGPURenderStateFragmentUniforms(Inno.Native.Sdl3.SDLGPURenderState state, uint slotIndex, void* data, uint length)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35334) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c548e3131aee58cf"></a>[`public static bool Inno.Native.Sdl3.SDL.SetGPURenderStateSamplerBindings(Inno.Native.Sdl3.SDLGPURenderState state, int numSamplerBindings, ref Inno.Native.Sdl3.SDLGPUTextureSamplerBinding samplerBindings)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35303) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5b2dc6a628bc1eb4"></a>[`public static bool Inno.Native.Sdl3.SDL.SetGPURenderStateSamplerBindings(Inno.Native.Sdl3.SDLGPURenderState state, int numSamplerBindings, Inno.Native.Sdl3.SDLGPUTextureSamplerBindingPtr samplerBindings)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35313) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-26fa125296ef1e5d"></a>[`public static byte Inno.Native.Sdl3.SDL.SetGPURenderStateStorageBuffers(Inno.Native.Sdl3.SDLGPURenderState state, int numStorageBuffers, Inno.Native.Sdl3.SDLGPUBuffer* storageBuffers)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35327) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3360dc4a1cc679ed"></a>[`public static bool Inno.Native.Sdl3.SDL.SetGPURenderStateStorageTextures(Inno.Native.Sdl3.SDLGPURenderState state, int numStorageTextures, Inno.Native.Sdl3.SDLGPUTexture* storageTextures)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35320) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fff4fc872f7f289a"></a>[`public static void Inno.Native.Sdl3.SDL.SetGPUScissor(Inno.Native.Sdl3.SDLGPURenderPass renderPass, ref Inno.Native.Sdl3.SDLRect scissor)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32316) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ccd56fcdbf35e907"></a>[`public static void Inno.Native.Sdl3.SDL.SetGPUScissor(Inno.Native.Sdl3.SDLGPURenderPass renderPass, Inno.Native.Sdl3.SDLRectPtr scissor)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32310) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-13d254f733d8809e"></a>[`public static void Inno.Native.Sdl3.SDL.SetGPUStencilReference(Inno.Native.Sdl3.SDLGPURenderPass renderPass, byte reference)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32331) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-03c5a9169377cf4d"></a>[`public static bool Inno.Native.Sdl3.SDL.SetGPUSwapchainParameters(Inno.Native.Sdl3.SDLGPUDevice device, Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLGPUSwapchainComposition swapchainComposition, Inno.Native.Sdl3.SDLGPUPresentMode presentMode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32706) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-15dd309aa6be1fc8"></a>[`public static void Inno.Native.Sdl3.SDL.SetGPUTextureName(Inno.Native.Sdl3.SDLGPUDevice device, Inno.Native.Sdl3.SDLGPUTexture texture, byte* text)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32175) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-98cebbec0a1bc5e8"></a>[`public static void Inno.Native.Sdl3.SDL.SetGPUTextureName(Inno.Native.Sdl3.SDLGPUDevice device, Inno.Native.Sdl3.SDLGPUTexture texture, string text)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32181) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0e31dbbfd9406edf"></a>[`public static void Inno.Native.Sdl3.SDL.SetGPUViewport(Inno.Native.Sdl3.SDLGPURenderPass renderPass, ref Inno.Native.Sdl3.SDLGPUViewport viewport)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32301) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-be6f0e968fc99f92"></a>[`public static void Inno.Native.Sdl3.SDL.SetGPUViewport(Inno.Native.Sdl3.SDLGPURenderPass renderPass, Inno.Native.Sdl3.SDLGPUViewportPtr viewport)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32295) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b6803139d05aa919"></a>[`public static bool Inno.Native.Sdl3.SDL.SetHapticAutocenter(Inno.Native.Sdl3.SDLHaptic haptic, int autocenter)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33024) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4e6ad3ceb506cac3"></a>[`public static bool Inno.Native.Sdl3.SDL.SetHapticGain(Inno.Native.Sdl3.SDLHaptic haptic, int gain)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33017) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2312a7b3318a9bc1"></a>[`public static byte Inno.Native.Sdl3.SDL.SetHint(byte* name, byte* value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33327) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5d26d17b788e8589"></a>[`public static bool Inno.Native.Sdl3.SDL.SetHint(string name, string value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33334) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f75b29407b2d0c81"></a>[`public static byte Inno.Native.Sdl3.SDL.SetHintWithPriority(byte* name, byte* value, Inno.Native.Sdl3.SDLHintPriority priority)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33299) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-987d33de965ad426"></a>[`public static bool Inno.Native.Sdl3.SDL.SetHintWithPriority(string name, string value, Inno.Native.Sdl3.SDLHintPriority priority)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33306) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-31a5f52ba9dc8463"></a>[`public static void Inno.Native.Sdl3.SDL.SetInitialized(ref Inno.Native.Sdl3.SDLInitState state, byte initialized)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25631) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2cd1d04a84c0eaec"></a>[`public static void Inno.Native.Sdl3.SDL.SetInitialized(Inno.Native.Sdl3.SDLInitStatePtr state, bool initialized)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25640) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f6f4287e57f61b28"></a>[`public static void Inno.Native.Sdl3.SDL.SetInitialized(Inno.Native.Sdl3.SDLInitStatePtr state, byte initialized)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25625) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d4edd7f1e0c44f20"></a>[`public static void Inno.Native.Sdl3.SDL.SetJoystickEventsEnabled(bool enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30368) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-017fe42c2fe990ef"></a>[`public static void Inno.Native.Sdl3.SDL.SetJoystickEventsEnabled(byte enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30362) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-71183865e1bc1277"></a>[`public static bool Inno.Native.Sdl3.SDL.SetJoystickLED(Inno.Native.Sdl3.SDLJoystick joystick, byte red, byte green, byte blue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30459) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-68ce266cb03edcb8"></a>[`public static bool Inno.Native.Sdl3.SDL.SetJoystickPlayerIndex(Inno.Native.Sdl3.SDLJoystick joystick, int playerIndex)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30240) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fde698519b9235f1"></a>[`public static bool Inno.Native.Sdl3.SDL.SetJoystickVirtualAxis(Inno.Native.Sdl3.SDLJoystick joystick, int axis, short value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30156) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-30894468a47b8b7f"></a>[`public static bool Inno.Native.Sdl3.SDL.SetJoystickVirtualBall(Inno.Native.Sdl3.SDLJoystick joystick, int ball, short xrel, short yrel)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30163) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d89533033f87e133"></a>[`public static bool Inno.Native.Sdl3.SDL.SetJoystickVirtualButton(Inno.Native.Sdl3.SDLJoystick joystick, int button, bool down)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30177) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1dcaeabc6da73ff8"></a>[`public static byte Inno.Native.Sdl3.SDL.SetJoystickVirtualButton(Inno.Native.Sdl3.SDLJoystick joystick, int button, byte down)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30170) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a729439390531735"></a>[`public static bool Inno.Native.Sdl3.SDL.SetJoystickVirtualHat(Inno.Native.Sdl3.SDLJoystick joystick, int hat, byte value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30184) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f46f8f634205f1e7"></a>[`public static bool Inno.Native.Sdl3.SDL.SetJoystickVirtualTouchpad(Inno.Native.Sdl3.SDLJoystick joystick, int touchpad, int finger, bool down, float x, float y, float pressure)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30198) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ff9d305e5f3ed043"></a>[`public static byte Inno.Native.Sdl3.SDL.SetJoystickVirtualTouchpad(Inno.Native.Sdl3.SDLJoystick joystick, int touchpad, int finger, byte down, float x, float y, float pressure)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30191) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-81ce89484a720cfe"></a>[`public static void Inno.Native.Sdl3.SDL.SetLogOutputFunction(delegate* unmanaged[Cdecl]<void*, int, Inno.Native.Sdl3.SDLLogPriority, byte*, void> callback, void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33654) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9b079573f58105c7"></a>[`public static void Inno.Native.Sdl3.SDL.SetLogPriorities(Inno.Native.Sdl3.SDLLogPriority priority)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33586) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-603191afcb2264c3"></a>[`public static void Inno.Native.Sdl3.SDL.SetLogPriority(int category, Inno.Native.Sdl3.SDLLogPriority priority)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33592) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bda16688dbd39559"></a>[`public static byte Inno.Native.Sdl3.SDL.SetLogPriorityPrefix(Inno.Native.Sdl3.SDLLogPriority priority, byte* prefix)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33611) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c695b1d99d22a072"></a>[`public static bool Inno.Native.Sdl3.SDL.SetLogPriorityPrefix(Inno.Native.Sdl3.SDLLogPriority priority, string prefix)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33618) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d347a962ede646fe"></a>[`public static byte Inno.Native.Sdl3.SDL.SetMemoryFunctions(delegate* unmanaged[Cdecl]<ulong, void*> mallocFunc, delegate* unmanaged[Cdecl]<ulong, ulong, void*> callocFunc, delegate* unmanaged[Cdecl]<void*, ulong, void*> reallocFunc, delegate* unmanaged[Cdecl]<void*, void> freeFunc)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23075) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e2b081b90c927acd"></a>[`public static void Inno.Native.Sdl3.SDL.SetModState(Inno.Native.Sdl3.SDLKeymod modstate)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31202) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-27c7a1e7f1c972d8"></a>[`public static byte Inno.Native.Sdl3.SDL.SetNumberProperty(uint props, byte* name, long value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25084) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1f3dc745df550ec2"></a>[`public static bool Inno.Native.Sdl3.SDL.SetNumberProperty(uint props, string name, long value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25091) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ac30d80cfae51e82"></a>[`public static bool Inno.Native.Sdl3.SDL.SetPaletteColors(ref Inno.Native.Sdl3.SDLPalette palette, ref Inno.Native.Sdl3.SDLColor colors, int firstcolor, int ncolors)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26704) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9d7b436314550f56"></a>[`public static bool Inno.Native.Sdl3.SDL.SetPaletteColors(Inno.Native.Sdl3.SDLPalettePtr palette, Inno.Native.Sdl3.SDLColorPtr colors, int firstcolor, int ncolors)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26717) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b0eed1f618d89976"></a>[`public static byte Inno.Native.Sdl3.SDL.SetPointerProperty(uint props, byte* name, void* value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25032) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a3726ef06e8e7c59"></a>[`public static bool Inno.Native.Sdl3.SDL.SetPointerProperty(uint props, string name, void* value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25039) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6e1513b05957ee3d"></a>[`public static byte Inno.Native.Sdl3.SDL.SetPointerPropertyWithCleanup(uint props, byte* name, void* value, delegate* unmanaged[Cdecl]<void*, void*, void> cleanup, void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25025) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e8bf71c11e5169d5"></a>[`public static byte Inno.Native.Sdl3.SDL.SetPrimarySelectionText(byte* text)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L28424) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c67acfbb68446d9f"></a>[`public static bool Inno.Native.Sdl3.SDL.SetPrimarySelectionText(string text)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L28431) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9e483dcf1433dfe6"></a>[`public static byte Inno.Native.Sdl3.SDL.SetRelativeMouseTransform(delegate* unmanaged[Cdecl]<void*, ulong, nint, uint, float*, float*, void> callback, void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31512) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1f18ef0ac8542e02"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderClipRect(Inno.Native.Sdl3.SDLRenderer renderer, ref Inno.Native.Sdl3.SDLRect rect)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34642) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-92b386de34bc67ae"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderClipRect(Inno.Native.Sdl3.SDLRenderer renderer, Inno.Native.Sdl3.SDLRectPtr rect)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34652) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f648a07a00e386ba"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderColorScale(Inno.Native.Sdl3.SDLRenderer renderer, float scale)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34776) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-63cb211c76f96207"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderDrawBlendMode(Inno.Native.Sdl3.SDLRenderer renderer, uint blendMode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34800) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8c6036db093734f6"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderDrawColor(Inno.Native.Sdl3.SDLRenderer renderer, byte r, byte g, byte b, byte a)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34710) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7baaef379040058d"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderDrawColorFloat(Inno.Native.Sdl3.SDLRenderer renderer, float r, float g, float b, float a)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34717) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c940dde9b993c1fa"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderLogicalPresentation(Inno.Native.Sdl3.SDLRenderer renderer, int w, int h, Inno.Native.Sdl3.SDLRendererLogicalPresentation mode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34480) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-686b17e4ce3c458a"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderScale(Inno.Native.Sdl3.SDLRenderer renderer, float scaleX, float scaleY)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34683) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6a0a5f7cc47376c6"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderTarget(Inno.Native.Sdl3.SDLRenderer renderer, ref Inno.Native.Sdl3.SDLTexture texture)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34456) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c8bf65acfc638859"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderTarget(Inno.Native.Sdl3.SDLRenderer renderer, Inno.Native.Sdl3.SDLTexturePtr texture)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34466) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8fc5172e35fc88ed"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderTextureAddressMode(Inno.Native.Sdl3.SDLRenderer renderer, Inno.Native.Sdl3.SDLTextureAddressMode uMode, Inno.Native.Sdl3.SDLTextureAddressMode vMode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35134) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b5a8d09d8dc12b48"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderViewport(Inno.Native.Sdl3.SDLRenderer renderer, ref Inno.Native.Sdl3.SDLRect rect)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34584) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4186b2281014b90e"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderViewport(Inno.Native.Sdl3.SDLRenderer renderer, Inno.Native.Sdl3.SDLRectPtr rect)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34594) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-17da2a0c2c16ab9f"></a>[`public static bool Inno.Native.Sdl3.SDL.SetRenderVSync(Inno.Native.Sdl3.SDLRenderer renderer, int vsync)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35224) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0a27e4b8927dcaf7"></a>[`public static byte Inno.Native.Sdl3.SDL.SetScancodeName(Inno.Native.Sdl3.SDLScancode scancode, byte* name)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31237) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8d32952ad27a52c0"></a>[`public static bool Inno.Native.Sdl3.SDL.SetScancodeName(Inno.Native.Sdl3.SDLScancode scancode, string name)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31244) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a34df1a7b04bb721"></a>[`public static byte Inno.Native.Sdl3.SDL.SetStringProperty(uint props, byte* name, byte* value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25056) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d2df7aa9b2471a5b"></a>[`public static bool Inno.Native.Sdl3.SDL.SetStringProperty(uint props, string name, string value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25063) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9be6ccadd2e6df21"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceAlphaMod(ref Inno.Native.Sdl3.SDLSurface surface, byte alpha)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27620) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-07734ebf3b96763d"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceAlphaMod(Inno.Native.Sdl3.SDLSurfacePtr surface, byte alpha)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27630) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-747c43926daf3675"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceBlendMode(ref Inno.Native.Sdl3.SDLSurface surface, uint blendMode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27664) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-47eefb6e18b411a7"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceBlendMode(Inno.Native.Sdl3.SDLSurfacePtr surface, uint blendMode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27674) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2756dc1c4e2f59d8"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceClipRect(ref Inno.Native.Sdl3.SDLSurface surface, ref Inno.Native.Sdl3.SDLRect rect)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27708) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-15530ad6d0edc8ee"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceClipRect(Inno.Native.Sdl3.SDLSurfacePtr surface, Inno.Native.Sdl3.SDLRectPtr rect)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27721) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c98006eb39c31668"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceColorKey(ref Inno.Native.Sdl3.SDLSurface surface, byte enabled, uint key)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27509) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3695cc12854225f5"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceColorKey(Inno.Native.Sdl3.SDLSurfacePtr surface, bool enabled, uint key)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27519) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5eb172b7c3ef66b5"></a>[`public static byte Inno.Native.Sdl3.SDL.SetSurfaceColorKey(Inno.Native.Sdl3.SDLSurfacePtr surface, byte enabled, uint key)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27502) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-271657416c15b8a0"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceColorMod(ref Inno.Native.Sdl3.SDLSurface surface, byte r, byte g, byte b)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27570) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6c68a89e3de80cc1"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceColorMod(Inno.Native.Sdl3.SDLSurfacePtr surface, byte r, byte g, byte b)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27580) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1646f85b288d04e3"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceColorspace(ref Inno.Native.Sdl3.SDLSurface surface, Inno.Native.Sdl3.SDLColorspace colorspace)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27144) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-67d99404a59fd119"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceColorspace(Inno.Native.Sdl3.SDLSurfacePtr surface, Inno.Native.Sdl3.SDLColorspace colorspace)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27154) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b7cf62f38ad9fcab"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfacePalette(ref Inno.Native.Sdl3.SDLSurface surface, ref Inno.Native.Sdl3.SDLPalette palette)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27185) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1a228aa4b2f3852b"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfacePalette(Inno.Native.Sdl3.SDLSurfacePtr surface, Inno.Native.Sdl3.SDLPalettePtr palette)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27198) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7f0acd62253e3125"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceRLE(ref Inno.Native.Sdl3.SDLSurface surface, byte enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27468) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f9967053203bb4c7"></a>[`public static bool Inno.Native.Sdl3.SDL.SetSurfaceRLE(Inno.Native.Sdl3.SDLSurfacePtr surface, bool enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27478) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-784fb97b8452ab3a"></a>[`public static byte Inno.Native.Sdl3.SDL.SetSurfaceRLE(Inno.Native.Sdl3.SDLSurfacePtr surface, byte enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27461) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a58c7fc5dbcadf25"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextInputArea(Inno.Native.Sdl3.SDLWindow window, ref Inno.Native.Sdl3.SDLRect rect, int cursor)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31356) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-160d5ffb02f44e0b"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextInputArea(Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLRectPtr rect, int cursor)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31366) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-492c70d33bf127c3"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextureAlphaMod(ref Inno.Native.Sdl3.SDLTexture texture, byte alpha)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34168) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-82733853ab93cc10"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextureAlphaMod(Inno.Native.Sdl3.SDLTexturePtr texture, byte alpha)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34178) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2a46962cc126a4c6"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextureAlphaModFloat(ref Inno.Native.Sdl3.SDLTexture texture, float alpha)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34185) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-295c03f197dc9126"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextureAlphaModFloat(Inno.Native.Sdl3.SDLTexturePtr texture, float alpha)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34195) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f15808bdb0a37376"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextureBlendMode(ref Inno.Native.Sdl3.SDLTexture texture, uint blendMode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34256) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-19a764c8e9675b94"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextureBlendMode(Inno.Native.Sdl3.SDLTexturePtr texture, uint blendMode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34266) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a4ef33b45724437e"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextureColorMod(ref Inno.Native.Sdl3.SDLTexture texture, byte r, byte g, byte b)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34068) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2063ffa7bd5affd3"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextureColorMod(Inno.Native.Sdl3.SDLTexturePtr texture, byte r, byte g, byte b)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34078) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9d1d3f517577763e"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextureColorModFloat(ref Inno.Native.Sdl3.SDLTexture texture, float r, float g, float b)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34085) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8fb5b6ab1b1f70e4"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextureColorModFloat(Inno.Native.Sdl3.SDLTexturePtr texture, float r, float g, float b)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34095) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c1f31cad422e7973"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTexturePalette(ref Inno.Native.Sdl3.SDLTexture texture, ref Inno.Native.Sdl3.SDLPalette palette)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34041) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d37bc035fcda0b81"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTexturePalette(Inno.Native.Sdl3.SDLTexturePtr texture, Inno.Native.Sdl3.SDLPalettePtr palette)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34054) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a1418279fa2a149b"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextureScaleMode(ref Inno.Native.Sdl3.SDLTexture texture, Inno.Native.Sdl3.SDLScaleMode scaleMode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34300) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-612e5dafe042a322"></a>[`public static bool Inno.Native.Sdl3.SDL.SetTextureScaleMode(Inno.Native.Sdl3.SDLTexturePtr texture, Inno.Native.Sdl3.SDLScaleMode scaleMode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34310) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bf7575b164e6822d"></a>[`public static byte Inno.Native.Sdl3.SDL.SetTLS(Inno.Native.Sdl3.SDLAtomicInt* id, void* value, delegate* unmanaged[Cdecl]<void*, void> destructor)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25417) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0c84ad92a4907b36"></a>[`public static void Inno.Native.Sdl3.SDL.SetTrayEntryCallback(Inno.Native.Sdl3.SDLTrayEntry entry, delegate* unmanaged[Cdecl]<void*, nint, void> callback, void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L36080) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8a2dfb2039e5d183"></a>[`public static void Inno.Native.Sdl3.SDL.SetTrayEntryChecked(Inno.Native.Sdl3.SDLTrayEntry entry, bool @checked)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L36048) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0d99261eff7c84f4"></a>[`public static void Inno.Native.Sdl3.SDL.SetTrayEntryChecked(Inno.Native.Sdl3.SDLTrayEntry entry, byte @checked)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L36042) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-15c40dd2e9aa7b1b"></a>[`public static void Inno.Native.Sdl3.SDL.SetTrayEntryEnabled(Inno.Native.Sdl3.SDLTrayEntry entry, bool enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L36067) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-190d65077cf16d5d"></a>[`public static void Inno.Native.Sdl3.SDL.SetTrayEntryEnabled(Inno.Native.Sdl3.SDLTrayEntry entry, byte enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L36061) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9c05ec791bc201a3"></a>[`public static void Inno.Native.Sdl3.SDL.SetTrayEntryLabel(Inno.Native.Sdl3.SDLTrayEntry entry, byte* label)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L36013) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-10ffd7fa54d598ba"></a>[`public static void Inno.Native.Sdl3.SDL.SetTrayEntryLabel(Inno.Native.Sdl3.SDLTrayEntry entry, string label)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L36019) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c6315338e9acb81e"></a>[`public static void Inno.Native.Sdl3.SDL.SetTrayIcon(Inno.Native.Sdl3.SDLTray tray, ref Inno.Native.Sdl3.SDLSurface icon)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35934) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9237c24f4e1bea59"></a>[`public static void Inno.Native.Sdl3.SDL.SetTrayIcon(Inno.Native.Sdl3.SDLTray tray, Inno.Native.Sdl3.SDLSurfacePtr icon)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35928) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5a007faead78b936"></a>[`public static void Inno.Native.Sdl3.SDL.SetTrayTooltip(Inno.Native.Sdl3.SDLTray tray, byte* tooltip)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35943) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d9d38a017e62ad2f"></a>[`public static void Inno.Native.Sdl3.SDL.SetTrayTooltip(Inno.Native.Sdl3.SDLTray tray, string tooltip)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35949) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0e6cecc8f3090307"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowAlwaysOnTop(Inno.Native.Sdl3.SDLWindow window, bool onTop)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29256) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-418229bf97596482"></a>[`public static byte Inno.Native.Sdl3.SDL.SetWindowAlwaysOnTop(Inno.Native.Sdl3.SDLWindow window, byte onTop)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29249) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-79ed13bc081b7e0c"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowAspectRatio(Inno.Native.Sdl3.SDLWindow window, float minAspect, float maxAspect)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29094) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7c1bda1dc52dd0c9"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowBordered(Inno.Native.Sdl3.SDLWindow window, bool bordered)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29228) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-142b147778d0cb14"></a>[`public static byte Inno.Native.Sdl3.SDL.SetWindowBordered(Inno.Native.Sdl3.SDLWindow window, byte bordered)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29221) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6bce9b0ba8bdaf7c"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowFillDocument(Inno.Native.Sdl3.SDLWindow window, bool fill)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29270) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1551b62b5cb1ac45"></a>[`public static byte Inno.Native.Sdl3.SDL.SetWindowFillDocument(Inno.Native.Sdl3.SDLWindow window, byte fill)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29263) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-36f5bb999a45b337"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowFocusable(Inno.Native.Sdl3.SDLWindow window, bool focusable)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29524) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d9bfb17f9287e61d"></a>[`public static byte Inno.Native.Sdl3.SDL.SetWindowFocusable(Inno.Native.Sdl3.SDLWindow window, byte focusable)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29517) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8588d480394593dc"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowFullscreen(Inno.Native.Sdl3.SDLWindow window, bool fullscreen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29326) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c42c7a954cf738f1"></a>[`public static byte Inno.Native.Sdl3.SDL.SetWindowFullscreen(Inno.Native.Sdl3.SDLWindow window, byte fullscreen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29319) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-775ab260d3fe1387"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowFullscreenMode(Inno.Native.Sdl3.SDLWindow window, ref Inno.Native.Sdl3.SDLDisplayMode mode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L28858) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e5aeb65a0935753d"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowFullscreenMode(Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLDisplayModePtr mode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L28868) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4bc8023dbb98aa9f"></a>[`public static byte Inno.Native.Sdl3.SDL.SetWindowHitTest(Inno.Native.Sdl3.SDLWindow window, delegate* unmanaged[Cdecl]<nint, Inno.Native.Sdl3.SDLPoint*, void*, Inno.Native.Sdl3.SDLHitTestResult> callback, void* callbackData)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29538) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f3b57a2ea8b0df93"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowIcon(Inno.Native.Sdl3.SDLWindow window, ref Inno.Native.Sdl3.SDLSurface icon)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29006) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-91943348589fbaf0"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowIcon(Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLSurfacePtr icon)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29016) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-edc7195afeee423d"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowKeyboardGrab(Inno.Native.Sdl3.SDLWindow window, bool grabbed)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29416) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cc5de6ab0ce0a3a9"></a>[`public static byte Inno.Native.Sdl3.SDL.SetWindowKeyboardGrab(Inno.Native.Sdl3.SDLWindow window, byte grabbed)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29409) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b702fda3606a9314"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowMaximumSize(Inno.Native.Sdl3.SDLWindow window, int maxW, int maxH)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29194) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-688bd272cc501ed8"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowMinimumSize(Inno.Native.Sdl3.SDLWindow window, int minW, int minH)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29167) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1a2dcec1f3f38667"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowModal(Inno.Native.Sdl3.SDLWindow window, bool modal)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29510) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-99f4f190984fc79e"></a>[`public static byte Inno.Native.Sdl3.SDL.SetWindowModal(Inno.Native.Sdl3.SDLWindow window, byte modal)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29503) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ada07dd56ddef012"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowMouseGrab(Inno.Native.Sdl3.SDLWindow window, bool grabbed)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29430) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-404bb630575c5214"></a>[`public static byte Inno.Native.Sdl3.SDL.SetWindowMouseGrab(Inno.Native.Sdl3.SDLWindow window, byte grabbed)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29423) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-df9993c894c0f086"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowMouseRect(Inno.Native.Sdl3.SDLWindow window, ref Inno.Native.Sdl3.SDLRect rect)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29458) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-853b273c6f0a4bdb"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowMouseRect(Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLRectPtr rect)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29468) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c69fea8fc8c64feb"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowOpacity(Inno.Native.Sdl3.SDLWindow window, float opacity)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29482) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a1556536eaab74ab"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowParent(Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLWindow parent)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29496) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-824c2f8a3c165d0b"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowPosition(Inno.Native.Sdl3.SDLWindow window, int x, int y)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29023) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bc96b708f8ab6f63"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowProgressState(Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLProgressState state)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29569) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3639ed03b2d3a614"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowProgressValue(Inno.Native.Sdl3.SDLWindow window, float value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29583) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-56dcaa907ccd856f"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowRelativeMouseMode(Inno.Native.Sdl3.SDLWindow window, bool enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31526) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b7c3c5c465f670cf"></a>[`public static byte Inno.Native.Sdl3.SDL.SetWindowRelativeMouseMode(Inno.Native.Sdl3.SDLWindow window, byte enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31519) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f721410d90fb672d"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowResizable(Inno.Native.Sdl3.SDLWindow window, bool resizable)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29242) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9d92dae4e2ac5b9c"></a>[`public static byte Inno.Native.Sdl3.SDL.SetWindowResizable(Inno.Native.Sdl3.SDLWindow window, byte resizable)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29235) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0385fe47222de69e"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowShape(Inno.Native.Sdl3.SDLWindow window, ref Inno.Native.Sdl3.SDLSurface shape)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29545) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-08e2aeb253fa45ec"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowShape(Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLSurfacePtr shape)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29555) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-62255df3eafb0e52"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowSize(Inno.Native.Sdl3.SDLWindow window, int w, int h)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29050) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cfd15eaf8a8c141c"></a>[`public static void Inno.Native.Sdl3.SDL.SetWindowsMessageHook(delegate* unmanaged[Cdecl]<void*, nint, byte> callback, void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35630) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-441ff9bc4c68c380"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowSurfaceVSync(Inno.Native.Sdl3.SDLWindow window, int vsync)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29354) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1741612b1cd3d54e"></a>[`public static byte Inno.Native.Sdl3.SDL.SetWindowTitle(Inno.Native.Sdl3.SDLWindow window, byte* title)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L28975) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-be2fb7e568220c4b"></a>[`public static bool Inno.Native.Sdl3.SDL.SetWindowTitle(Inno.Native.Sdl3.SDLWindow window, string title)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L28982) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e4089f34f0612dce"></a>[`public static void Inno.Native.Sdl3.SDL.SetX11EventHook(delegate* unmanaged[Cdecl]<void*, nint, byte> callback, void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35650) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cd48684937cf0c75"></a>[`public static bool Inno.Native.Sdl3.SDL.ShouldInit(ref Inno.Native.Sdl3.SDLInitState state)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25591) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f47dc9bd70dd97c8"></a>[`public static bool Inno.Native.Sdl3.SDL.ShouldInit(Inno.Native.Sdl3.SDLInitStatePtr state)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25601) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fd814e0c7d2fbde7"></a>[`public static bool Inno.Native.Sdl3.SDL.ShouldQuit(ref Inno.Native.Sdl3.SDLInitState state)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25608) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-300e042f3867aed6"></a>[`public static bool Inno.Native.Sdl3.SDL.ShouldQuit(Inno.Native.Sdl3.SDLInitStatePtr state)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25618) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDL</code> · 2341–2520</summary>

<!-- api: M:Inno.Native.Sdl3.SDL.ShowCursor c9db4214e418488c5e7dcb9e12eaaa827b45e3de2f64b739ed2920da802fc02b -->
<!-- api: M:Inno.Native.Sdl3.SDL.ShowFileDialogWithProperties(Inno.Native.Sdl3.SDLFileDialogType,,System.Void*,System.UInt32) 7e52d5091e39244f55e668216276d8da4599b4c215cd7bd27a07abc8f7b5558b -->
<!-- api: M:Inno.Native.Sdl3.SDL.ShowMessageBox(Inno.Native.Sdl3.SDLMessageBoxData*,System.Int32*) 0a3e02853c88ab24dbbf04874e79dedf1ed934b9179d7a02b1d8f3aadf0b813b -->
<!-- api: M:Inno.Native.Sdl3.SDL.ShowOpenFileDialog(,System.Void*,Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLDialogFileFilter*,System.Int32,System.Byte*,System.Byte) 072f50ca585957a911e0363d4db4739c8a420ece6816e872f433d85fa31488e3 -->
<!-- api: M:Inno.Native.Sdl3.SDL.ShowOpenFolderDialog(,System.Void*,Inno.Native.Sdl3.SDLWindow,System.Byte*,System.Byte) fac2d68f129c43a3b60c5641e81d44fc226daf5f536c71a2354a7e640d7e02ff -->
<!-- api: M:Inno.Native.Sdl3.SDL.ShowSaveFileDialog(,System.Void*,Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLDialogFileFilter*,System.Int32,System.Byte*) aeebd072705d6fed16800e06b7043a85f02daaaa9d5f357be8a9c61cba582b4c -->
<!-- api: M:Inno.Native.Sdl3.SDL.ShowSimpleMessageBox(Inno.Native.Sdl3.SDLMessageBoxFlags,System.Byte*,System.Byte*,Inno.Native.Sdl3.SDLWindow) fe7b4d7146233fc6dccb24f44425323934cb743f7aa3c2f920c9dba2bd279ccb -->
<!-- api: M:Inno.Native.Sdl3.SDL.ShowSimpleMessageBox(Inno.Native.Sdl3.SDLMessageBoxFlags,System.String,System.String,Inno.Native.Sdl3.SDLWindow) 1d99d739612ba16d4388222e6268897a97d4b5b1d1e70e12d8a39228d1422f03 -->
<!-- api: M:Inno.Native.Sdl3.SDL.ShowWindow(Inno.Native.Sdl3.SDLWindow) 817a9f8fc48b951e703d61d849838911b8f03a88b373c4ca310c4d07305d8fd5 -->
<!-- api: M:Inno.Native.Sdl3.SDL.ShowWindowSystemMenu(Inno.Native.Sdl3.SDLWindow,System.Int32,System.Int32) 7a3974189c75ba984464898a9d5592251ba1d17b70fc01ad1145821b92645d1a -->
<!-- api: M:Inno.Native.Sdl3.SDL.SignalAsyncIOQueue(Inno.Native.Sdl3.SDLAsyncIOQueue) 612125ad8e2fd257e4f404514a671c927133c4bc814d3a390cffe73c10715918 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SignalCondition(Inno.Native.Sdl3.SDLCondition) e8ff7935b0974bd2cf2391ee7076a11cd7653ed1cbe07fa5e4c227b936b8ab71 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SignalSemaphore(Inno.Native.Sdl3.SDLSemaphore) 3cd63ed02ede9c0d2751368ed7e7924273a755b4711346413a571fdf9a738ffe -->
<!-- api: M:Inno.Native.Sdl3.SDL.Sin(System.Double) 1c4ec805c9acbd5aef44cdf581ed3b47e362528081caedcc68119f1e72f3c87b -->
<!-- api: M:Inno.Native.Sdl3.SDL.Sinf(System.Single) 7c88e97acec789d0ac3439ceda19b9b60d3edfc11f0b09f6ad4ac39d84503790 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Sqrt(System.Double) 9bd562963265ed4bd07a6ab133c07ef97320bf5aaf48a2e00808e145cd5adbb5 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Sqrtf(System.Single) 1675470375aaf7895d9a184162de721aeffb62e9bb9267f7fbd060aa9aef0add -->
<!-- api: M:Inno.Native.Sdl3.SDL.Srand(System.UInt64) 89e384cc607c89ed2a7eb6c7db5adbbd3d95eff791b00597269852f63d10ac44 -->
<!-- api: M:Inno.Native.Sdl3.SDL.StartTextInput(Inno.Native.Sdl3.SDLWindow) 3a0745824419fe8ea0260147c465b198ae25e84ad08eba0a7ba3ad6ddca1aed0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.StartTextInputWithProperties(Inno.Native.Sdl3.SDLWindow,System.UInt32) ea2f27fc8d653fbb7718d7ac55c3f4b6ca560495366793a0d21fb4e1a6160e8f -->
<!-- api: M:Inno.Native.Sdl3.SDL.StepBackUTF8(System.Byte*,System.Byte**) a7722490f2e5217f1db085af323df1053053865a406156169b05fa534fbc6c4c -->
<!-- api: M:Inno.Native.Sdl3.SDL.StepBackUTF8(System.String,System.Byte**) 41b1d8d26990c606821e726fe89efed0716b8b2cf275003fe5ccd5da555145df -->
<!-- api: M:Inno.Native.Sdl3.SDL.StepUTF8(System.Byte**,System.UInt64@) 78f86ef727d331e1fb2e9c1f00065d50d9e6d4ceabc9843e53987313c6c2fdd0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.StepUTF8(System.Byte**,System.UInt64*) d0fdeade7af454e287affc483c59ca157dd0c1d8117b525ae8837214cb608b2b -->
<!-- api: M:Inno.Native.Sdl3.SDL.StopHapticEffect(Inno.Native.Sdl3.SDLHaptic,System.Int32) 204b706d20c9dc36a1bbf50b2ac4a22da00f700486ba3e3b291a7133f9c3501b -->
<!-- api: M:Inno.Native.Sdl3.SDL.StopHapticEffects(Inno.Native.Sdl3.SDLHaptic) 17d9f885ac6ab57d18b4666ee0b34f580951f0d7388b6f10e6739ea3b933c436 -->
<!-- api: M:Inno.Native.Sdl3.SDL.StopHapticRumble(Inno.Native.Sdl3.SDLHaptic) 78dd8f86248a8e13cd9b4662187c5d1e896dcd4b570c55b692824c7995f98c8e -->
<!-- api: M:Inno.Native.Sdl3.SDL.StopTextInput(Inno.Native.Sdl3.SDLWindow) 4bd984d8479e1980831dce5bfb900b665a9baf95dc58c5e843fef3001579c65b -->
<!-- api: M:Inno.Native.Sdl3.SDL.StorageReady(Inno.Native.Sdl3.SDLStorage) da9c50149da44b9721a6445473907d2ec11dc8840d5d468d1cdffefc3d317865 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strcasecmp(System.Byte*,System.Byte*) afb0255c5635f1b59c85d65b753c6193f5817761ff83f9d29a52aa1e1c16f290 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strcasecmp(System.String,System.String) f8817b3b26b71cf78ef2cc20601139aa998252dfee51f556b4b4b37c396c28d0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strcasestr(System.Byte*,System.Byte*) d3a8c54a0b26915d01d6f150e53fe06a54fc5f4522bba52b3d5e5fe1c01d999c -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strchr(System.Byte*,System.Int32) 09340b39ad5cab309dcfd43ae7b46e2abbd78e5bb89ad0bdad9ad5c8b388b149 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strcmp(System.Byte*,System.Byte*) 674b9336093d714d085d9dcad78abfdff2e6e6590e354d90d78352790fda17e4 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strcmp(System.String,System.String) 99ad6bbacbb7dedaa65dfebec033529160ef39a1b035bf83e13ec0cc23c97f45 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strdup(System.Byte*) 05c00c19d6e4983a3e9bc44fea773ca61316e9419c80615198fa5934bae50623 -->
<!-- api: M:Inno.Native.Sdl3.SDL.StretchSurface(Inno.Native.Sdl3.SDLSurface@,Inno.Native.Sdl3.SDLRect@,Inno.Native.Sdl3.SDLSurface@,Inno.Native.Sdl3.SDLRect@,Inno.Native.Sdl3.SDLScaleMode) 26aa3ba9e227bf76ed3a20dcbe4bc506fbe102e39181e8f9d8f2b38dc2b862ed -->
<!-- api: M:Inno.Native.Sdl3.SDL.StretchSurface(Inno.Native.Sdl3.SDLSurfacePtr,Inno.Native.Sdl3.SDLRectPtr,Inno.Native.Sdl3.SDLSurfacePtr,Inno.Native.Sdl3.SDLRectPtr,Inno.Native.Sdl3.SDLScaleMode) b15a7f3a06c03e10d9b267644b658576385954ae9412c8b7da7d6dd336cba370 -->
<!-- api: M:Inno.Native.Sdl3.SDL.StringToGUID(System.Byte*) ebcd35c16e8e7481595befc9b85dd244a524062af3f7544ae59f46c5bb0333a2 -->
<!-- api: M:Inno.Native.Sdl3.SDL.StringToGUID(System.String) 653369ee5f1b67ef7d1676fcf879fe347d60875dd7a074f6284ac1662dd020a3 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strlcat(System.Byte*,System.Byte*,System.UInt64) 9a1454d4b67655942e66fa76c96ccf3d59c9a298e33472bec5252128d344f10a -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strlcpy(System.Byte*,System.Byte*,System.UInt64) b4db98c9ab2f44717fd7dfe3f371d40aeff061b42a1ac8c94c9b27c2cb0720c5 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strlen(System.Byte*) d1dbf6b4bfe85636c1413ffae1c04db2b6cc016bddb8cb8b7c88bcdacf08d653 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strlen(System.String) 872f575da59172640d83fa59318009d026c1880704af5ce6b3d386150dbcd006 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strlwr(System.Byte*) c5955c73b938401d876b221786f9a2a9ccaf267763525f0674a57e5dad9e498c -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strncasecmp(System.Byte*,System.Byte*,System.UInt64) e06b345523e8774ab4c64f022196d73fe0e500cf924e78eb70349a751df5d634 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strncasecmp(System.String,System.String,System.UInt64) 09a43463dafc4eb2e4b00841a7f8211b69b0602567b500b4c68c5ee110900c19 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strncmp(System.Byte*,System.Byte*,System.UInt64) b7cbe175929eaddffe7c342c280da373fd07a4a238b7df90d730454bbfec2e7e -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strncmp(System.String,System.String,System.UInt64) dc5d8b8e567752e1c9bb7032fe9e22dc94391d80771dfe7e7a6ddadbf3be9d9e -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strndup(System.Byte*,System.UInt64) c5bfeb35721055560d636f7c967db0c50c52be12b49729981b286e6ed8b571aa -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strnlen(System.Byte*,System.UInt64) d738ff4cde4d2b3528a7bf756b2b96ad64415e0fcea47a5155ead46b4689a434 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strnlen(System.String,System.UInt64) fad2d409098b53ebccd9d327bc6130a7c9b6e31860d3458deca333dab270dcb5 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strnstr(System.Byte*,System.Byte*,System.UInt64) 9a4cd240d978b8ed3d02702e8ecf5316924db9307c3f4fe119babcccb95134f6 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strpbrk(System.Byte*,System.Byte*) 120fdf6356befe3be1e428e428735fde693cf66b0e5c74187b0305b5700712b3 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strrchr(System.Byte*,System.Int32) d6fb67f8b7f92f46642f4726a1824a44187e7c6102d95b74e675791c15b538b6 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strrev(System.Byte*) 279e5fd9a467f2d15847d8e7a7513434db5ba7bb68be72547b739dd0b0c29807 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strstr(System.Byte*,System.Byte*) 054380dc6f56d9fa12f157f657864fa5eb008457c63c089cf97045150a9b0cc7 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strtod(System.Byte*,System.Byte**) 106bbc00da9729a2b7148564ac9b8b46acba74e5e95e510fa3b38b8f067cae3d -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strtod(System.String,System.Byte**) 0cf4295b9773312f4cfd0a3851425f80cf61e35627c5deaeb3e545f5b6b323c9 -->
<!-- api: M:Inno.Native.Sdl3.SDL.StrtokR(System.Byte*,System.Byte*,System.Byte**) b88178caf7a29db8a2e00474f6937c191ea9dd4e55db65e557f477320800e2eb -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strtol(System.Byte*,System.Byte**,System.Int32) e358f5a72cdcb192522adc694ff006ebf4eba76dfd195c027f90f5bb47028ca3 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strtol(System.String,System.Byte**,System.Int32) cf3857c6f0094b90a2a91deecd691579a6521bcee94e90394bac463a92da96cd -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strtoll(System.Byte*,System.Byte**,System.Int32) 3c1ef8ad06d9eb8d7b4a60d31a98350fa0c8c75483fffbbbc2d09f2b74275b8c -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strtoll(System.String,System.Byte**,System.Int32) 8f5f2259949469013163f4fb500acd0697dc02bb2e12b5c8eac1012e2ed17956 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strtoul(System.Byte*,System.Byte**,System.Int32) b8123f466f62c1c2c93425f1e4285737e943d3bd8fe1e8ceea947a032cc7fa97 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strtoul(System.String,System.Byte**,System.Int32) 0f1e34154ac2b896f48a65f2360e38a218b27b97afb27388eec89b5729bdccfc -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strtoull(System.Byte*,System.Byte**,System.Int32) 29c36253ad0f13a8eb2f254cdaf3a49683e2b6183a31bbcc9d214f5ce8baeb3a -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strtoull(System.String,System.Byte**,System.Int32) 1315de8b8d3e5e4afdfae9616bf131518cb77bbdc1ac7facc2f908e9395a347e -->
<!-- api: M:Inno.Native.Sdl3.SDL.Strupr(System.Byte*) 9536b6b30630f95497f17deeef4116d9f408a52acd5b396f615fbd7b5fa0672f -->
<!-- api: M:Inno.Native.Sdl3.SDL.SubmitGPUCommandBuffer(Inno.Native.Sdl3.SDLGPUCommandBuffer) c8338937ec1b85f5d8fa143222f443952c1d883e78754d486e862300956913d7 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SubmitGPUCommandBufferAndAcquireFence(Inno.Native.Sdl3.SDLGPUCommandBuffer) afb9ffc6a03aeae5884e9480723399f4b6459fdfbabe3f80ff5d8e192edfd310 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SurfaceHasAlternateImages(Inno.Native.Sdl3.SDLSurface@) c1fa95ef7e9feea00499ff73f8ead3d5f55bafb4816386af6cfc8ff0f37ac886 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SurfaceHasAlternateImages(Inno.Native.Sdl3.SDLSurfacePtr) fd8b6e50fee221ca33533e68989fdc7dc746cb5d4857b1060994f7dffc30cc7e -->
<!-- api: M:Inno.Native.Sdl3.SDL.SurfaceHasColorKey(Inno.Native.Sdl3.SDLSurface@) f47dce57b2cc7db80e54747eeaa2dd99bada75d875e2b8b7c06bc98b7c3a395c -->
<!-- api: M:Inno.Native.Sdl3.SDL.SurfaceHasColorKey(Inno.Native.Sdl3.SDLSurfacePtr) e2883af843a0d37dcf4537c13e4f87d7d633dfc4cd352fc7096ef527f63d33f7 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SurfaceHasRLE(Inno.Native.Sdl3.SDLSurface@) 7088b05816e13e9d7ed9af795f99fa055bb8a0678994e5a922c143cf5a14c3e3 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SurfaceHasRLE(Inno.Native.Sdl3.SDLSurfacePtr) 6a9f9fefc81ea014bc8d8531539c10536c57bb21f9efe68d09fbf504355f5f32 -->
<!-- api: M:Inno.Native.Sdl3.SDL.SyncWindow(Inno.Native.Sdl3.SDLWindow) 0fa968abf5f92e8c9d5622419091729065ee610c535c1dd1750726b8eee762e4 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Tan(System.Double) c178ba1cc6e8ffeaa88ca0405910e68ae2785b671ae363fcb93d13b945ef7982 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Tanf(System.Single) 55e06465c204e104e31d13e145c96658aeb1e5e6659a80a7a51757c10858f6ca -->
<!-- api: M:Inno.Native.Sdl3.SDL.TellIO(Inno.Native.Sdl3.SDLIOStream) 6541f6e34c636e52f19f423d7003048c801bcdf93f156cf46929d2c7823114e1 -->
<!-- api: M:Inno.Native.Sdl3.SDL.TextInputActive(Inno.Native.Sdl3.SDLWindow) 7d87ae322c892b6e8c6626e70421399c32b74cbff0f88292537690e34a744d3b -->
<!-- api: M:Inno.Native.Sdl3.SDL.TimeFromWindows(System.UInt32,System.UInt32) 2cfb526413d53f7039f63b4bda32853e04e70342f8b9154d514f2c328b9024b0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.TimeToDateTime(System.Int64,Inno.Native.Sdl3.SDLDateTime@,System.Byte) ead6fe62be96c4c329cd12d1b9fbc23a59ec5a5650008b0ea9ae1f5d995913c5 -->
<!-- api: M:Inno.Native.Sdl3.SDL.TimeToDateTime(System.Int64,Inno.Native.Sdl3.SDLDateTimePtr,System.Boolean) 77971843df452e921323a156dd806b762df45acf4a031a3c24079487202dfd02 -->
<!-- api: M:Inno.Native.Sdl3.SDL.TimeToDateTime(System.Int64,Inno.Native.Sdl3.SDLDateTimePtr,System.Byte) e61583614eda6c8a6b07cba191f580c1fd019cd9c1b34f3c52c4ebc88e710fe7 -->
<!-- api: M:Inno.Native.Sdl3.SDL.TimeToWindows(System.Int64,System.UInt32@,System.UInt32@) 6f82f6de4696cc43fd6c899fa9d55365048ae3727986071854f381156394af34 -->
<!-- api: M:Inno.Native.Sdl3.SDL.TimeToWindows(System.Int64,System.UInt32*,System.UInt32*) e3c8f9ee939080f80702d40e1a06c7e4755b23d398e03cd3146e0183500fbf60 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Tolower(System.Int32) c7cbe873ee050ac8f1bd06366187eb30f6c2f682cf68252dbb4032e7647082a9 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Toupper(System.Int32) d1adc2372c0efd83f801e3c9ca1bcdd1f8f5ad26fbd987f0ce8215648216a609 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Trunc(System.Double) c081e30c8cf13747164e5381b9ce3ecc76c19cdab383fb054f702a29026f5aaa -->
<!-- api: M:Inno.Native.Sdl3.SDL.Truncf(System.Single) 2ddaaf8f6b8f7943cf84d7d7ecf4b29cf4a84b4928daf252ea02211f802b3e97 -->
<!-- api: M:Inno.Native.Sdl3.SDL.TryLockJoysticks a34861981717ce2d0d484af138c340a6116fc6a42d8be4de9c7cb7130e77b858 -->
<!-- api: M:Inno.Native.Sdl3.SDL.TryLockMutex(Inno.Native.Sdl3.SDLMutex) 32bc2c362d609f3101eae0c5b50ffc34286613b915e8c5c5ff726a1e73d59e17 -->
<!-- api: M:Inno.Native.Sdl3.SDL.TryLockRWLockForReading(Inno.Native.Sdl3.SDLRWLock) 8dc24fc6f58173066becb27c85e5cbce91c9020d4fef1f37c6539cf251fb1bfd -->
<!-- api: M:Inno.Native.Sdl3.SDL.TryLockRWLockForWriting(Inno.Native.Sdl3.SDLRWLock) 0ddf1ad66dfb6a02fe1b2621531eb0a93b3f0491ece4f2cb962012fa1b0d7238 -->
<!-- api: M:Inno.Native.Sdl3.SDL.TryLockSpinlock(System.Int32@) 3dbb691d071048247b244cbe546a467e6da7eb24d7a84f46a022abe934413b5e -->
<!-- api: M:Inno.Native.Sdl3.SDL.TryLockSpinlock(System.Int32*) 3b8113cab8af2d83639664c8a96de0578a53af1e381a0e3a770b4400b225ed4d -->
<!-- api: M:Inno.Native.Sdl3.SDL.TryWaitSemaphore(Inno.Native.Sdl3.SDLSemaphore) 5ab1c1ff806b7b80af436d81433df4880dcf71bade98d09a240c4de7a6333479 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UCS4ToUTF8(System.UInt32,System.Byte*) c54c82b3d80952c9e8b29ea2ab587175dfde921df46dae874846858cf5773e56 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Uitoa(System.UInt32,System.Byte*,System.Int32) 674cc3e881172ad59e16c77fbfb951885a2f6483f187230dcf23a08c928e34c9 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Ulltoa(System.UInt64,System.Byte*,System.Int32) d2d9afa352995a4aed7b3eece53181d730cfb39e3d6c075c250af4b425958be8 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Ultoa(System.UInt32,System.Byte*,System.Int32) 26c87ec6f7a013b5ebdfb3e0b52c1596fcc1a353b2f42e9c57cdc8b40e87d33f -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnbindAudioStream(Inno.Native.Sdl3.SDLAudioStream) 3042e887f29f2e4d28eb57c5b50a36a5a3e02172e11608e12e708d8c3c91afcc -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnbindAudioStreams(Inno.Native.Sdl3.SDLAudioStream*,System.Int32) 0a6cac833a93f0c0cf2319052688b93018709e247262c0119e5e7482a65d765e -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnloadObject(Inno.Native.Sdl3.SDLSharedObject) 81ccc10e3b5414cbe54d48c86cc3951d70f3ac0ac79442da1022380e5af459ff -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnlockAudioStream(Inno.Native.Sdl3.SDLAudioStream) 328cb17e9bfe76012e889b5c7574abe42e575382b6cb6f69a788067970b0491c -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnlockJoysticks d8b40a41412a058542fad3c90e2bf1e804a927a5f8800e4574cae4a9cfd26076 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnlockMutex(Inno.Native.Sdl3.SDLMutex) 12c16f9848cd23747e79a2020554fa5578b7c786af4e344e5d904969d2818341 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnlockProperties(System.UInt32) e1e102432d5ee129b3cabe8c150154448a912968f6a497229ac0c35535519fc2 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnlockRWLock(Inno.Native.Sdl3.SDLRWLock) 3267625fbf88e5a0d1f3e9d25843b23683b5ec0741e61c066882064185a71a96 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnlockSpinlock(System.Int32@) 9e5fc2ae79102755583b4a632279f625caa759c4865e15ac9b3cc83039dfb509 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnlockSpinlock(System.Int32*) 23170cb12ad7d6de47f341ee97cc719cd9e906c740921cad6d944a7a776d91a6 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnlockSurface(Inno.Native.Sdl3.SDLSurface@) 0fcc27e6a7ce4ccc6a82f0f7dd6a26100eb6ec8108ebfbf49128e2ba1b9381ee -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnlockSurface(Inno.Native.Sdl3.SDLSurfacePtr) 6ef8a17a4fdbe026f207affe3e06c0fa8821ce4f31957120376267a2be0b29cd -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnlockTexture(Inno.Native.Sdl3.SDLTexture@) 4653c8c33eb1c96baea7046343540224a9f58b7f2b07bb1f0efd1e231d5bf782 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnlockTexture(Inno.Native.Sdl3.SDLTexturePtr) a1031c3981638c4348995737542154d09c00afd0629929340c9eb7f06af4bc7e -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnmapGPUTransferBuffer(Inno.Native.Sdl3.SDLGPUDevice,Inno.Native.Sdl3.SDLGPUTransferBuffer) 776fd3fed1993a680b0df95824f142f508ca745acd9fbd06f90813d4bf005672 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnsetEnvironmentVariable(Inno.Native.Sdl3.SDLEnvironment,System.Byte*) 53ad7055734a3a987aa95028d4b0088ac1565a6b665bc9719aeb220dbc1762ba -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnsetEnvironmentVariable(Inno.Native.Sdl3.SDLEnvironment,System.String) a10c25ba82790d713efaae1d61a74bff39b41ec109b0050bc13f412163ff31aa -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnsetenvUnsafe(System.Byte*) 9f2ddb278211f17f46005b0b8dfcaf5f872477393274132d1274305bf9ba4089 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UnsetenvUnsafe(System.String) 0910dfdc1af541346d3b868cca1580d733fc6fc858c6294967c4408a3d3a8f2a -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateGamepads e282f0f0fa8a9101c05d7db8b0e326057814c0eece0a1192b790240a6d428e22 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateHapticEffect(Inno.Native.Sdl3.SDLHaptic,System.Int32,Inno.Native.Sdl3.SDLHapticEffect*) 5bc39e05d5fcda20040636b95f0f35fa495b9f2784b5968519de53e838f5e862 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateJoysticks 07d13bdddd8ff3c0e350a60e1c3b8a0a160e8cdebeb3911b3a79fe7bfa0553c8 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateNVTexture(Inno.Native.Sdl3.SDLTexture@,Inno.Native.Sdl3.SDLRect@,System.Byte*,System.Int32,System.Byte*,System.Int32) 7b17edfebd81fde339d8d0c17eafff96a5bf0c019d91043aa086134e1a2b9f59 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateNVTexture(Inno.Native.Sdl3.SDLTexturePtr,Inno.Native.Sdl3.SDLRectPtr,System.Byte*,System.Int32,System.Byte*,System.Int32) 5fd1e03c66b117338a46505ce80ffaa8b67f63800bf5f561bdbc8f4f0f836025 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateSensors 277eb95d875e075fb11d8cd8a810df6b653190cf381314f56a0de7dfc831b771 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateTexture(Inno.Native.Sdl3.SDLTexture@,Inno.Native.Sdl3.SDLRect@,System.Void*,System.Int32) a244712b2a705769d7a88a2dd3a898faf308de8dd98253e8e60db3b0ac6e84ec -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateTexture(Inno.Native.Sdl3.SDLTexturePtr,Inno.Native.Sdl3.SDLRectPtr,System.Void*,System.Int32) 9665502010d2e36673aa4615fd3bf2734820c7aa4996e8b9e9b9baf697adeeb5 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateTrays f339fcab9940b6b9adb403f5d6155c5d2634247589cf0ac92ef468085a6985bc -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateWindowSurface(Inno.Native.Sdl3.SDLWindow) 16b75adf68c79b8054d69c9457a7616dcf6d12b221f33ece9e5e283ea509d297 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateWindowSurfaceRects(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLRect@,System.Int32) ff0fe8299ed216b543052904a4082937b4f8c6bbc3fc0091cd68e12098b89635 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateWindowSurfaceRects(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLRectPtr,System.Int32) fa129021f53978f461916b3640b13d23e10694c500ae70a6704c22df125a1ea7 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateYUVTexture(Inno.Native.Sdl3.SDLTexture@,Inno.Native.Sdl3.SDLRect@,System.Byte*,System.Int32,System.Byte*,System.Int32,System.Byte*,System.Int32) ea67f60a95005311f9fd18e86ef8f9acef5ce55918d8b388a800ca18bf1ef9db -->
<!-- api: M:Inno.Native.Sdl3.SDL.UpdateYUVTexture(Inno.Native.Sdl3.SDLTexturePtr,Inno.Native.Sdl3.SDLRectPtr,System.Byte*,System.Int32,System.Byte*,System.Int32,System.Byte*,System.Int32) 9229a815015ee964cf826816f91918d4223ad984f05012fa93c8517874bc50bd -->
<!-- api: M:Inno.Native.Sdl3.SDL.UploadToGPUBuffer(Inno.Native.Sdl3.SDLGPUCopyPass,Inno.Native.Sdl3.SDLGPUTransferBufferLocation@,Inno.Native.Sdl3.SDLGPUBufferRegion@,System.Byte) 5d12c34f59465cb13a8124bad805f6ec06770356b08b5b954ae5a17be35b9ce1 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UploadToGPUBuffer(Inno.Native.Sdl3.SDLGPUCopyPass,Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr,Inno.Native.Sdl3.SDLGPUBufferRegionPtr,System.Boolean) 0dc97a3bc6cd8774292c23cb6266b8fe36cfa4dc77b5ffb66c919817d3cbb9d7 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UploadToGPUBuffer(Inno.Native.Sdl3.SDLGPUCopyPass,Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr,Inno.Native.Sdl3.SDLGPUBufferRegionPtr,System.Byte) 2ed5ab7e8cc12d3deb147f5e9d864c79751e7dd71d04ef7fa9354faf62b2858c -->
<!-- api: M:Inno.Native.Sdl3.SDL.UploadToGPUTexture(Inno.Native.Sdl3.SDLGPUCopyPass,Inno.Native.Sdl3.SDLGPUTextureTransferInfo@,Inno.Native.Sdl3.SDLGPUTextureRegion@,System.Byte) 77a78525a69ae7f08b42d86f5a2e266ee4362dd4de934f6806a1fe3e43ab3ae0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.UploadToGPUTexture(Inno.Native.Sdl3.SDLGPUCopyPass,Inno.Native.Sdl3.SDLGPUTextureTransferInfoPtr,Inno.Native.Sdl3.SDLGPUTextureRegionPtr,System.Boolean) c5baf7313e26ac48f60bdd0bb2436ba0602ab87264b7108ef3b6a4c72328844f -->
<!-- api: M:Inno.Native.Sdl3.SDL.UploadToGPUTexture(Inno.Native.Sdl3.SDLGPUCopyPass,Inno.Native.Sdl3.SDLGPUTextureTransferInfoPtr,Inno.Native.Sdl3.SDLGPUTextureRegionPtr,System.Byte) 81d3741b7253bf3e460238d625506e19bf4d7d48d619e24b33799adef5edfb3d -->
<!-- api: M:Inno.Native.Sdl3.SDL.Utf8Strlcpy(System.Byte*,System.Byte*,System.UInt64) ed20eabc05f3e2d648c236c18878edce98e4665dcdeac34fa8469db47fdafb38 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Utf8Strlen(System.Byte*) 6ac092dfba326266f80437b0dd9f442ef201a31041fd4064d2541d79cb277bfa -->
<!-- api: M:Inno.Native.Sdl3.SDL.Utf8Strlen(System.String) 42ba6270008bec05f20ed1f51d988d622506e5e73b603249784e52b150e7d4fa -->
<!-- api: M:Inno.Native.Sdl3.SDL.Utf8Strnlen(System.Byte*,System.UInt64) 8fd4fc4e19eac39eb46b9b9b159faf8216f9b9ad1595285eeb3a7a5b1ed93389 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Utf8Strnlen(System.String,System.UInt64) b63de211a23227adeb6dda2b78d433e761f4388505362a010de68d2a284b2afc -->
<!-- api: M:Inno.Native.Sdl3.SDL.Vasprintf(System.Byte**,System.Byte*,System.Byte*) bff767c64bdfd55bba244f147430927aacf7c77a41aa8503a14b9c1449c25ef1 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Vsnprintf(System.Byte*,System.UInt64,System.Byte*,System.Byte*) d790509739862bacdeecc4da9e228183a2e5367be5c4051e634b368948729a4b -->
<!-- api: M:Inno.Native.Sdl3.SDL.Vsscanf(System.Byte*,System.Byte*,System.Byte*) 02d7d7d2fc7153b6111c53dbecf7847fd92e67239dc49e40cc8cf3fd0358824f -->
<!-- api: M:Inno.Native.Sdl3.SDL.Vswprintf(System.UInt16*,System.UInt64,System.UInt16*,System.Byte*) 64776a00484303348d4e9a70feb39d883a372bc2d96632873e3ffdc4c1f67a28 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitAndAcquireGPUSwapchainTexture(Inno.Native.Sdl3.SDLGPUCommandBuffer,Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLGPUTexture*,System.UInt32*,System.UInt32*) 1b21beec3f2f2d01266018760733828640e9154c579d35d48cd66e3e215d46f4 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitAsyncIOResult(Inno.Native.Sdl3.SDLAsyncIOQueue,Inno.Native.Sdl3.SDLAsyncIOOutcome@,System.Int32) 5f3480f0c89f6156fbad2befbe1146b9dbaec8be3e37d556e861ff524eb0c002 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitAsyncIOResult(Inno.Native.Sdl3.SDLAsyncIOQueue,Inno.Native.Sdl3.SDLAsyncIOOutcomePtr,System.Int32) 3a84e9ad28940721ea2ed865f8936566332516e18175ed05324d2c84fc1a3ef8 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitCondition(Inno.Native.Sdl3.SDLCondition,Inno.Native.Sdl3.SDLMutex) 621cb91732bfe6c5d976c753b6350b019345c5755f3b9e460de25afc49c42f88 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitConditionTimeout(Inno.Native.Sdl3.SDLCondition,Inno.Native.Sdl3.SDLMutex,System.Int32) b88561bca40f6aa3ea69f7373cb8d1ffbec44f60d2e8a93d355ee3c3cce274e5 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitEvent(Inno.Native.Sdl3.SDLEvent@) 3ebcec76e25714d672d22fe4850bc1cdefcc7dddbfec6c42c83434177502721b -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitEvent(Inno.Native.Sdl3.SDLEventPtr) 39af043023d03e510b8ad101d945783d8cd04c66eaf5b55a8a45b2b6221554ec -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitEventTimeout(Inno.Native.Sdl3.SDLEvent@,System.Int32) 73c710e83c401386da98566205d63251351290c5892df92fc658de8c90079a33 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitEventTimeout(Inno.Native.Sdl3.SDLEventPtr,System.Int32) 24b04fd0a0162e23cb40f25a704d47022d694fd0b2f903731d0538e7d6a999bd -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitForGPUFences(Inno.Native.Sdl3.SDLGPUDevice,System.Boolean,Inno.Native.Sdl3.SDLGPUFence*,System.UInt32) 67aaa3e3ce13b942227c967052d20c6f2d18544ba7d625cf0959df81e9757df5 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitForGPUFences(Inno.Native.Sdl3.SDLGPUDevice,System.Byte,Inno.Native.Sdl3.SDLGPUFence*,System.UInt32) 54a2e72bbc5f53d2a9bf44d3fd29c9299f088f7d3b3a8980c8fb570069d82424 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitForGPUIdle(Inno.Native.Sdl3.SDLGPUDevice) 37ba2b472a9f48a9cdc5d0bd3b57756a68e172757d4e569e5a029e748d8a49af -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitForGPUSwapchain(Inno.Native.Sdl3.SDLGPUDevice,Inno.Native.Sdl3.SDLWindow) edf2c660fb1206b4470bc6a6dfaa20f3c964a14b70361c600b21e14c67bfbba7 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitProcess(Inno.Native.Sdl3.SDLProcess,System.Boolean,System.Int32@) 0bcc61d4c3c269a095e69e925309fb21ed376c22ceeabfc032ea3fa0085a181d -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitProcess(Inno.Native.Sdl3.SDLProcess,System.Byte,System.Int32*) b0cd25a475ed47516473d55bf466820733bd5c93041f81ebfe358a31caff427a -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitSemaphore(Inno.Native.Sdl3.SDLSemaphore) 639771d22cfc5c8a16e15a5f42cd010db6072c018380b627eb0abd587c2f9bb3 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitSemaphoreTimeout(Inno.Native.Sdl3.SDLSemaphore,System.Int32) fd59be9a37658f6b786f27b804690924c9dbb43758ed539bd70faece04e05f6c -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitThread(Inno.Native.Sdl3.SDLThread,System.Int32@) ad59abd2866150fae2bcdddeb3eccd6b4ce07fe08e6674f7f6b2ef06b36eda0a -->
<!-- api: M:Inno.Native.Sdl3.SDL.WaitThread(Inno.Native.Sdl3.SDLThread,System.Int32*) 93f29be2a08efc9438a0e68a17a77b23a745b922ab326558d9cea043294bfe6b -->
<!-- api: M:Inno.Native.Sdl3.SDL.WarpMouseGlobal(System.Single,System.Single) 359d83a604921a6fb579b8a539cc07d455351be0ddacb0a64f7c41246d9410e9 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WarpMouseInWindow(Inno.Native.Sdl3.SDLWindow,System.Single,System.Single) 6de4f4997cc83998dc829f50309f38ead2334a2f8af7c8c7f949e5c3adcf9834 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WasInit(Inno.Native.Sdl3.SDLInitFlags) e61efaff1c793e3826c81f39ec2a4bbbbe09bac4688aef65c6b4c884efb91883 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Wcscasecmp(System.UInt16*,System.UInt16*) 64db809c1ade20f18275434149701bc929c7de27c1ffc2482882b65ed9f9f417 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Wcscmp(System.UInt16*,System.UInt16*) 6462bbb40d5fcde43f9885feba461fc8c7d86c4a2e8dafcc6cccd7da98a3c68f -->
<!-- api: M:Inno.Native.Sdl3.SDL.Wcsdup(System.UInt16*) 6a32fcf0f468af937e1974445a883e18a26ab7dcd49458ed70c0eb6e5b317c9a -->
<!-- api: M:Inno.Native.Sdl3.SDL.Wcslcat(System.UInt16@,System.UInt16*,System.UInt64) 2c526f23b1b65700409ccd8c9edf5cb377868435c80e1ef47729aa810a959b73 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Wcslcat(System.UInt16*,System.UInt16*,System.UInt64) c94869f1f2a670146129486565b55d5053c0273b275a9a72655f92a00a01fd47 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Wcslcpy(System.UInt16@,System.UInt16*,System.UInt64) f9cbe60c1bc85372c2666df6d7c36e4948e33a0c8f6ed22f995aca1d65d59b07 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Wcslcpy(System.UInt16*,System.UInt16*,System.UInt64) fe8e9f3136a0d1f764c5d21d52f38cd33412a680a5209e0a0358943d4536eea4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fd2126d0ecd99501"></a>[`public static bool Inno.Native.Sdl3.SDL.ShowCursor()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31609) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a2c78d3444d2b4e1"></a>[`public static void Inno.Native.Sdl3.SDL.ShowFileDialogWithProperties(Inno.Native.Sdl3.SDLFileDialogType type, delegate* unmanaged[Cdecl]<void*, byte**, int, void> callback, void* userdata, uint props)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29865) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1be5ec4809df8ab7"></a>[`public static byte Inno.Native.Sdl3.SDL.ShowMessageBox(Inno.Native.Sdl3.SDLMessageBoxData* messageboxdata, int* buttonid)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33660) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3a3c88ec56951aeb"></a>[`public static void Inno.Native.Sdl3.SDL.ShowOpenFileDialog(delegate* unmanaged[Cdecl]<void*, byte**, int, void> callback, void* userdata, Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLDialogFileFilter* filters, int nfilters, byte* defaultLocation, byte allowMany)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29847) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-576289192c9bb36e"></a>[`public static void Inno.Native.Sdl3.SDL.ShowOpenFolderDialog(delegate* unmanaged[Cdecl]<void*, byte**, int, void> callback, void* userdata, Inno.Native.Sdl3.SDLWindow window, byte* defaultLocation, byte allowMany)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29859) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-33afea370cc679cf"></a>[`public static void Inno.Native.Sdl3.SDL.ShowSaveFileDialog(delegate* unmanaged[Cdecl]<void*, byte**, int, void> callback, void* userdata, Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLDialogFileFilter* filters, int nfilters, byte* defaultLocation)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29853) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-df63ada3afc25219"></a>[`public static byte Inno.Native.Sdl3.SDL.ShowSimpleMessageBox(Inno.Native.Sdl3.SDLMessageBoxFlags flags, byte* title, byte* message, Inno.Native.Sdl3.SDLWindow window)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33667) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c0659324c8be2138"></a>[`public static bool Inno.Native.Sdl3.SDL.ShowSimpleMessageBox(Inno.Native.Sdl3.SDLMessageBoxFlags flags, string title, string message, Inno.Native.Sdl3.SDLWindow window)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33674) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-18129fc70ae74ce3"></a>[`public static bool Inno.Native.Sdl3.SDL.ShowWindow(Inno.Native.Sdl3.SDLWindow window)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29277) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2e221a0b44a4461f"></a>[`public static bool Inno.Native.Sdl3.SDL.ShowWindowSystemMenu(Inno.Native.Sdl3.SDLWindow window, int x, int y)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29531) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b872b131e45d5abe"></a>[`public static void Inno.Native.Sdl3.SDL.SignalAsyncIOQueue(Inno.Native.Sdl3.SDLAsyncIOQueue queue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24734) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f02a4a4d152b365e"></a>[`public static void Inno.Native.Sdl3.SDL.SignalCondition(Inno.Native.Sdl3.SDLCondition cond)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25566) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d2b5853f9e0be4fa"></a>[`public static void Inno.Native.Sdl3.SDL.SignalSemaphore(Inno.Native.Sdl3.SDLSemaphore sem)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25540) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-47fc776f38206a1f"></a>[`public static double Inno.Native.Sdl3.SDL.Sin(double x)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24510) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bd679655b8e4b029"></a>[`public static float Inno.Native.Sdl3.SDL.Sinf(float x)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24517) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-584a4f8dddfe9bb5"></a>[`public static double Inno.Native.Sdl3.SDL.Sqrt(double x)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24524) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c8bdc407e7b66708"></a>[`public static float Inno.Native.Sdl3.SDL.Sqrtf(float x)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24531) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8917fa53c023c4cb"></a>[`public static void Inno.Native.Sdl3.SDL.Srand(ulong seed)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24123) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-da2fe8745192522a"></a>[`public static bool Inno.Native.Sdl3.SDL.StartTextInput(Inno.Native.Sdl3.SDLWindow window)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31321) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d04aad6697fdcc6d"></a>[`public static bool Inno.Native.Sdl3.SDL.StartTextInputWithProperties(Inno.Native.Sdl3.SDLWindow window, uint props)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31328) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5b3e25db843165b6"></a>[`public static uint Inno.Native.Sdl3.SDL.StepBackUTF8(byte* start, byte** pstr)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24065) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9d987d7fe8f470bc"></a>[`public static uint Inno.Native.Sdl3.SDL.StepBackUTF8(string start, byte** pstr)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24072) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2895888c1688b3ed"></a>[`public static uint Inno.Native.Sdl3.SDL.StepUTF8(byte** pstr, ref ulong pslen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24056) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-992882eae34a30bc"></a>[`public static uint Inno.Native.Sdl3.SDL.StepUTF8(byte** pstr, ulong* pslen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24049) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-28bf48e0b9e1e0f0"></a>[`public static bool Inno.Native.Sdl3.SDL.StopHapticEffect(Inno.Native.Sdl3.SDLHaptic haptic, int effect)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32997) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ae3b11048d01109"></a>[`public static bool Inno.Native.Sdl3.SDL.StopHapticEffects(Inno.Native.Sdl3.SDLHaptic haptic)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33045) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ed03eb7fc80d3d16"></a>[`public static bool Inno.Native.Sdl3.SDL.StopHapticRumble(Inno.Native.Sdl3.SDLHaptic haptic)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33073) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bdf9686a992a6f1a"></a>[`public static bool Inno.Native.Sdl3.SDL.StopTextInput(Inno.Native.Sdl3.SDLWindow window)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31342) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-16268d3e528a16b4"></a>[`public static bool Inno.Native.Sdl3.SDL.StorageReady(Inno.Native.Sdl3.SDLStorage storage)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35389) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ba9be1d11006bdef"></a>[`public static int Inno.Native.Sdl3.SDL.Strcasecmp(byte* str1, byte* str2)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23988) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-10305788fa8fdcda"></a>[`public static int Inno.Native.Sdl3.SDL.Strcasecmp(string str1, string str2)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23995) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7dc699c2da309eaf"></a>[`public static byte* Inno.Native.Sdl3.SDL.Strcasestr(byte* haystack, byte* needle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23671) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8d398fb247659bb6"></a>[`public static byte* Inno.Native.Sdl3.SDL.Strchr(byte* str, int c)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23643) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6dfbbd8013898284"></a>[`public static int Inno.Native.Sdl3.SDL.Strcmp(byte* str1, byte* str2)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23934) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9d517d631daf9fd1"></a>[`public static int Inno.Native.Sdl3.SDL.Strcmp(string str1, string str2)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23941) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-768dc9817850b73b"></a>[`public static byte* Inno.Native.Sdl3.SDL.Strdup(byte* str)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23608) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4baa3cc49436c4f1"></a>[`public static bool Inno.Native.Sdl3.SDL.StretchSurface(ref Inno.Native.Sdl3.SDLSurface src, ref Inno.Native.Sdl3.SDLRect srcrect, ref Inno.Native.Sdl3.SDLSurface dst, ref Inno.Native.Sdl3.SDLRect dstrect, Inno.Native.Sdl3.SDLScaleMode scaleMode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L28013) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-71e315f917d91649"></a>[`public static bool Inno.Native.Sdl3.SDL.StretchSurface(Inno.Native.Sdl3.SDLSurfacePtr src, Inno.Native.Sdl3.SDLRectPtr srcrect, Inno.Native.Sdl3.SDLSurfacePtr dst, Inno.Native.Sdl3.SDLRectPtr dstrect, Inno.Native.Sdl3.SDLScaleMode scaleMode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L28032) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-71a9ad2bbfb054c9"></a>[`public static Inno.Native.Sdl3.SdlGuid Inno.Native.Sdl3.SDL.StringToGUID(byte* pchGUID)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29877) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-638ddd453cf4fcc4"></a>[`public static Inno.Native.Sdl3.SdlGuid Inno.Native.Sdl3.SDL.StringToGUID(string pchGUID)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29884) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-401f6aa3a218790e"></a>[`public static ulong Inno.Native.Sdl3.SDL.Strlcat(byte* dst, byte* src, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23601) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9aa206a21e9ac94a"></a>[`public static ulong Inno.Native.Sdl3.SDL.Strlcpy(byte* dst, byte* src, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23587) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-61f011c98441cadc"></a>[`public static ulong Inno.Native.Sdl3.SDL.Strlen(byte* str)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23541) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ea44b49477b28e61"></a>[`public static ulong Inno.Native.Sdl3.SDL.Strlen(string str)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23548) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e29cda8c9541fa7"></a>[`public static byte* Inno.Native.Sdl3.SDL.Strlwr(byte* str)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23636) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-42b2e7c9ca17191d"></a>[`public static int Inno.Native.Sdl3.SDL.Strncasecmp(byte* str1, byte* str2, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24015) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b93e4250d0657782"></a>[`public static int Inno.Native.Sdl3.SDL.Strncasecmp(string str1, string str2, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24022) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4aeb2e8486cd4299"></a>[`public static int Inno.Native.Sdl3.SDL.Strncmp(byte* str1, byte* str2, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23961) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a02d3c0c2fdd28e0"></a>[`public static int Inno.Native.Sdl3.SDL.Strncmp(string str1, string str2, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23968) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ca004ff54b4ab59c"></a>[`public static byte* Inno.Native.Sdl3.SDL.Strndup(byte* str, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23615) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8edec2f131ed8ba4"></a>[`public static ulong Inno.Native.Sdl3.SDL.Strnlen(byte* str, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23564) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cc75e25eedfb5472"></a>[`public static ulong Inno.Native.Sdl3.SDL.Strnlen(string str, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23571) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3b17cca886162b33"></a>[`public static byte* Inno.Native.Sdl3.SDL.Strnstr(byte* haystack, byte* needle, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23664) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a248cd0570f36257"></a>[`public static byte* Inno.Native.Sdl3.SDL.Strpbrk(byte* str, byte* breakset)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24042) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4625e488082e0bbe"></a>[`public static byte* Inno.Native.Sdl3.SDL.Strrchr(byte* str, int c)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23650) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-68ce1c4ea1d82f29"></a>[`public static byte* Inno.Native.Sdl3.SDL.Strrev(byte* str)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23622) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fcb59269120915cc"></a>[`public static byte* Inno.Native.Sdl3.SDL.Strstr(byte* haystack, byte* needle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23657) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-46adab4897dc529b"></a>[`public static double Inno.Native.Sdl3.SDL.Strtod(byte* str, byte** endp)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23911) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1f7f950e4f370bf8"></a>[`public static double Inno.Native.Sdl3.SDL.Strtod(string str, byte** endp)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23918) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-af7ee90a30482e64"></a>[`public static byte* Inno.Native.Sdl3.SDL.StrtokR(byte* str, byte* delim, byte** saveptr)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23678) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6ed3b834d3e1cc0a"></a>[`public static int Inno.Native.Sdl3.SDL.Strtol(byte* str, byte** endp, int baseValue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23819) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-69729b6ff75ce6cf"></a>[`public static int Inno.Native.Sdl3.SDL.Strtol(string str, byte** endp, int baseValue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23826) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c5cd938b0314adec"></a>[`public static long Inno.Native.Sdl3.SDL.Strtoll(byte* str, byte** endp, int baseValue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23865) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-00f65aee0ceb947a"></a>[`public static long Inno.Native.Sdl3.SDL.Strtoll(string str, byte** endp, int baseValue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23872) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-06c3ccc2fe8e139f"></a>[`public static uint Inno.Native.Sdl3.SDL.Strtoul(byte* str, byte** endp, int baseValue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23842) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d5439e40fb52265c"></a>[`public static uint Inno.Native.Sdl3.SDL.Strtoul(string str, byte** endp, int baseValue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23849) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-59e95b9252cf5154"></a>[`public static ulong Inno.Native.Sdl3.SDL.Strtoull(byte* str, byte** endp, int baseValue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23888) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-20f0441bfe981708"></a>[`public static ulong Inno.Native.Sdl3.SDL.Strtoull(string str, byte** endp, int baseValue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23895) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-72afb7759c904654"></a>[`public static byte* Inno.Native.Sdl3.SDL.Strupr(byte* str)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23629) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-13536b4a2719d84d"></a>[`public static byte Inno.Native.Sdl3.SDL.SubmitGPUCommandBuffer(Inno.Native.Sdl3.SDLGPUCommandBuffer commandBuffer)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32748) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-de253575145358b3"></a>[`public static Inno.Native.Sdl3.SDLGPUFence Inno.Native.Sdl3.SDL.SubmitGPUCommandBufferAndAcquireFence(Inno.Native.Sdl3.SDLGPUCommandBuffer commandBuffer)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32755) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ee15b3e8fb04fc08"></a>[`public static bool Inno.Native.Sdl3.SDL.SurfaceHasAlternateImages(ref Inno.Native.Sdl3.SDLSurface surface)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27232) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-57670f91b89808da"></a>[`public static bool Inno.Native.Sdl3.SDL.SurfaceHasAlternateImages(Inno.Native.Sdl3.SDLSurfacePtr surface)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27242) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6071c76d19277eab"></a>[`public static bool Inno.Native.Sdl3.SDL.SurfaceHasColorKey(ref Inno.Native.Sdl3.SDLSurface surface)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27526) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2f56d9dff55cc115"></a>[`public static bool Inno.Native.Sdl3.SDL.SurfaceHasColorKey(Inno.Native.Sdl3.SDLSurfacePtr surface)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27536) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-13f6ced3a849f7af"></a>[`public static bool Inno.Native.Sdl3.SDL.SurfaceHasRLE(ref Inno.Native.Sdl3.SDLSurface surface)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27485) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-82ef7910bd5b0006"></a>[`public static bool Inno.Native.Sdl3.SDL.SurfaceHasRLE(Inno.Native.Sdl3.SDLSurfacePtr surface)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27495) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fc2f2c21c50fd8ee"></a>[`public static bool Inno.Native.Sdl3.SDL.SyncWindow(Inno.Native.Sdl3.SDLWindow window)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29333) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e044dd1eae469c1"></a>[`public static double Inno.Native.Sdl3.SDL.Tan(double x)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24538) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe41b67641762b97"></a>[`public static float Inno.Native.Sdl3.SDL.Tanf(float x)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24545) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ad0882f54b83118e"></a>[`public static long Inno.Native.Sdl3.SDL.TellIO(Inno.Native.Sdl3.SDLIOStream context)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25716) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f220ad7f98539d6d"></a>[`public static bool Inno.Native.Sdl3.SDL.TextInputActive(Inno.Native.Sdl3.SDLWindow window)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31335) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-24651176033a059e"></a>[`public static long Inno.Native.Sdl3.SDL.TimeFromWindows(uint dwLowDateTime, uint dwHighDateTime)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35819) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-06ae6ced22888a90"></a>[`public static bool Inno.Native.Sdl3.SDL.TimeToDateTime(long ticks, ref Inno.Native.Sdl3.SDLDateTime dt, byte localTime)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35757) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-26d869c833657821"></a>[`public static bool Inno.Native.Sdl3.SDL.TimeToDateTime(long ticks, Inno.Native.Sdl3.SDLDateTimePtr dt, bool localTime)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35767) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8a5b8efd21e26b4f"></a>[`public static byte Inno.Native.Sdl3.SDL.TimeToDateTime(long ticks, Inno.Native.Sdl3.SDLDateTimePtr dt, byte localTime)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35750) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6cde3bf0f510fe7f"></a>[`public static void Inno.Native.Sdl3.SDL.TimeToWindows(long ticks, ref uint dwLowDateTime, ref uint dwHighDateTime)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35807) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-38dd46c8b387f7c0"></a>[`public static void Inno.Native.Sdl3.SDL.TimeToWindows(long ticks, uint* dwLowDateTime, uint* dwHighDateTime)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35801) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6954dfa5e8ad549b"></a>[`public static int Inno.Native.Sdl3.SDL.Tolower(int x)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23376) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-17374aed4afcf4ee"></a>[`public static int Inno.Native.Sdl3.SDL.Toupper(int x)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23369) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-565805694952d09e"></a>[`public static double Inno.Native.Sdl3.SDL.Trunc(double x)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24338) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d9cf5320c39801e6"></a>[`public static float Inno.Native.Sdl3.SDL.Truncf(float x)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24345) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8c3a07ced1d3151b"></a>[`public static bool Inno.Native.Sdl3.SDL.TryLockJoysticks()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30021) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-60487a300fe88ab4"></a>[`public static bool Inno.Native.Sdl3.SDL.TryLockMutex(Inno.Native.Sdl3.SDLMutex mutex)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25443) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-03a711f6070100dc"></a>[`public static bool Inno.Native.Sdl3.SDL.TryLockRWLockForReading(Inno.Native.Sdl3.SDLRWLock rwlock)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25481) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b159dbca38ab9979"></a>[`public static bool Inno.Native.Sdl3.SDL.TryLockRWLockForWriting(Inno.Native.Sdl3.SDLRWLock rwlock)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25488) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-658665926a30d9f4"></a>[`public static bool Inno.Native.Sdl3.SDL.TryLockSpinlock(ref int lock0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24754) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e6b3500f80a7db7a"></a>[`public static byte Inno.Native.Sdl3.SDL.TryLockSpinlock(int* lock0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24747) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7cb7c87015b0358c"></a>[`public static bool Inno.Native.Sdl3.SDL.TryWaitSemaphore(Inno.Native.Sdl3.SDLSemaphore sem)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25526) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9b2f8f685419bf51"></a>[`public static byte* Inno.Native.Sdl3.SDL.UCS4ToUTF8(uint codepoint, byte* dst)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24088) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-855a594ae2c03d5b"></a>[`public static byte* Inno.Native.Sdl3.SDL.Uitoa(uint value, byte* str, int radix)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23738) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9782cd1a31bd90d8"></a>[`public static byte* Inno.Native.Sdl3.SDL.Ulltoa(ulong value, byte* str, int radix)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23766) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8ac4305590cae18d"></a>[`public static byte* Inno.Native.Sdl3.SDL.Ultoa(uint value, byte* str, int radix)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23752) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-976f8e97aa3b0b9d"></a>[`public static void Inno.Native.Sdl3.SDL.UnbindAudioStream(Inno.Native.Sdl3.SDLAudioStream stream)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26290) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-46d6fcbf60002767"></a>[`public static void Inno.Native.Sdl3.SDL.UnbindAudioStreams(Inno.Native.Sdl3.SDLAudioStream* streams, int numStreams)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26284) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7a4f0fe245c61bd0"></a>[`public static void Inno.Native.Sdl3.SDL.UnloadObject(Inno.Native.Sdl3.SDLSharedObject handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33573) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-135f71da22db20f9"></a>[`public static bool Inno.Native.Sdl3.SDL.UnlockAudioStream(Inno.Native.Sdl3.SDLAudioStream stream)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26497) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9f9566bf8566048d"></a>[`public static void Inno.Native.Sdl3.SDL.UnlockJoysticks()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30028) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ab6de9dbc4f51ba4"></a>[`public static void Inno.Native.Sdl3.SDL.UnlockMutex(Inno.Native.Sdl3.SDLMutex mutex)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25450) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fcb886c92ede1b6f"></a>[`public static void Inno.Native.Sdl3.SDL.UnlockProperties(uint props)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25019) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-29826bcfed3750ac"></a>[`public static void Inno.Native.Sdl3.SDL.UnlockRWLock(Inno.Native.Sdl3.SDLRWLock rwlock)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25495) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4ef0e5182abbc398"></a>[`public static void Inno.Native.Sdl3.SDL.UnlockSpinlock(ref int lock0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24785) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5f37b8ea2dd9b623"></a>[`public static void Inno.Native.Sdl3.SDL.UnlockSpinlock(int* lock0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24779) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a6e52865db29717f"></a>[`public static void Inno.Native.Sdl3.SDL.UnlockSurface(ref Inno.Native.Sdl3.SDLSurface surface)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27294) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-72b7eab6e1025bc8"></a>[`public static void Inno.Native.Sdl3.SDL.UnlockSurface(Inno.Native.Sdl3.SDLSurfacePtr surface)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L27288) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-26f77002fbc02474"></a>[`public static void Inno.Native.Sdl3.SDL.UnlockTexture(ref Inno.Native.Sdl3.SDLTexture texture)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34447) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7cee721975f3eb92"></a>[`public static void Inno.Native.Sdl3.SDL.UnlockTexture(Inno.Native.Sdl3.SDLTexturePtr texture)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34441) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-500676c8faf2cf80"></a>[`public static void Inno.Native.Sdl3.SDL.UnmapGPUTransferBuffer(Inno.Native.Sdl3.SDLGPUDevice device, Inno.Native.Sdl3.SDLGPUTransferBuffer transferBuffer)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32516) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ecd6337ac0553cc2"></a>[`public static byte Inno.Native.Sdl3.SDL.UnsetEnvironmentVariable(Inno.Native.Sdl3.SDLEnvironment env, byte* name)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23158) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3503392217418674"></a>[`public static bool Inno.Native.Sdl3.SDL.UnsetEnvironmentVariable(Inno.Native.Sdl3.SDLEnvironment env, string name)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23165) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-678ca4f232fe2339"></a>[`public static int Inno.Native.Sdl3.SDL.UnsetenvUnsafe(byte* name)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23229) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-427fd1d51c9ea53d"></a>[`public static int Inno.Native.Sdl3.SDL.UnsetenvUnsafe(string name)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23236) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-03e85fa597b6371b"></a>[`public static void Inno.Native.Sdl3.SDL.UpdateGamepads()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30886) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e20fc236a6d1d156"></a>[`public static byte Inno.Native.Sdl3.SDL.UpdateHapticEffect(Inno.Native.Sdl3.SDLHaptic haptic, int effect, Inno.Native.Sdl3.SDLHapticEffect* data)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32983) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9fcb176726ab02a6"></a>[`public static void Inno.Native.Sdl3.SDL.UpdateJoysticks()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30381) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6278f08714efea31"></a>[`public static bool Inno.Native.Sdl3.SDL.UpdateNVTexture(ref Inno.Native.Sdl3.SDLTexture texture, ref Inno.Native.Sdl3.SDLRect rect, byte* yplane, int ypitch, byte* uVplane, int uVpitch)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34384) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4684c16b4d248c18"></a>[`public static bool Inno.Native.Sdl3.SDL.UpdateNVTexture(Inno.Native.Sdl3.SDLTexturePtr texture, Inno.Native.Sdl3.SDLRectPtr rect, byte* yplane, int ypitch, byte* uVplane, int uVpitch)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34397) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1308451496c4f02c"></a>[`public static void Inno.Native.Sdl3.SDL.UpdateSensors()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L30009) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ac14ad0ec5f341b7"></a>[`public static bool Inno.Native.Sdl3.SDL.UpdateTexture(ref Inno.Native.Sdl3.SDLTexture texture, ref Inno.Native.Sdl3.SDLRect rect, void* pixels, int pitch)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34344) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ba0d1c4f16c855f"></a>[`public static bool Inno.Native.Sdl3.SDL.UpdateTexture(Inno.Native.Sdl3.SDLTexturePtr texture, Inno.Native.Sdl3.SDLRectPtr rect, void* pixels, int pitch)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34357) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dc2b430874a3bd97"></a>[`public static void Inno.Native.Sdl3.SDL.UpdateTrays()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L36119) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-38ad1951149143a7"></a>[`public static bool Inno.Native.Sdl3.SDL.UpdateWindowSurface(Inno.Native.Sdl3.SDLWindow window)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29378) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a5dcc1a101e2ae3e"></a>[`public static bool Inno.Native.Sdl3.SDL.UpdateWindowSurfaceRects(Inno.Native.Sdl3.SDLWindow window, ref Inno.Native.Sdl3.SDLRect rects, int numrects)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29385) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d8353a730de5d704"></a>[`public static bool Inno.Native.Sdl3.SDL.UpdateWindowSurfaceRects(Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLRectPtr rects, int numrects)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29395) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f0c9817e51547f49"></a>[`public static bool Inno.Native.Sdl3.SDL.UpdateYUVTexture(ref Inno.Native.Sdl3.SDLTexture texture, ref Inno.Native.Sdl3.SDLRect rect, byte* yplane, int ypitch, byte* uplane, int upitch, byte* vplane, int vpitch)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34364) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c2733f28aca901de"></a>[`public static bool Inno.Native.Sdl3.SDL.UpdateYUVTexture(Inno.Native.Sdl3.SDLTexturePtr texture, Inno.Native.Sdl3.SDLRectPtr rect, byte* yplane, int ypitch, byte* uplane, int upitch, byte* vplane, int vpitch)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L34377) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ba1a6dce6b0d3c5f"></a>[`public static void Inno.Native.Sdl3.SDL.UploadToGPUBuffer(Inno.Native.Sdl3.SDLGPUCopyPass copyPass, ref Inno.Native.Sdl3.SDLGPUTransferBufferLocation source, ref Inno.Native.Sdl3.SDLGPUBufferRegion destination, byte cycle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32559) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-33eb73def7558752"></a>[`public static void Inno.Native.Sdl3.SDL.UploadToGPUBuffer(Inno.Native.Sdl3.SDLGPUCopyPass copyPass, Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr source, Inno.Native.Sdl3.SDLGPUBufferRegionPtr destination, bool cycle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32571) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2e15d6c6f0a84640"></a>[`public static void Inno.Native.Sdl3.SDL.UploadToGPUBuffer(Inno.Native.Sdl3.SDLGPUCopyPass copyPass, Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr source, Inno.Native.Sdl3.SDLGPUBufferRegionPtr destination, byte cycle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32553) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b83f811795627ea1"></a>[`public static void Inno.Native.Sdl3.SDL.UploadToGPUTexture(Inno.Native.Sdl3.SDLGPUCopyPass copyPass, ref Inno.Native.Sdl3.SDLGPUTextureTransferInfo source, ref Inno.Native.Sdl3.SDLGPUTextureRegion destination, byte cycle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32535) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-64973755a4f0491c"></a>[`public static void Inno.Native.Sdl3.SDL.UploadToGPUTexture(Inno.Native.Sdl3.SDLGPUCopyPass copyPass, Inno.Native.Sdl3.SDLGPUTextureTransferInfoPtr source, Inno.Native.Sdl3.SDLGPUTextureRegionPtr destination, bool cycle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32547) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-287e6d93fb394144"></a>[`public static void Inno.Native.Sdl3.SDL.UploadToGPUTexture(Inno.Native.Sdl3.SDLGPUCopyPass copyPass, Inno.Native.Sdl3.SDLGPUTextureTransferInfoPtr source, Inno.Native.Sdl3.SDLGPUTextureRegionPtr destination, byte cycle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32529) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-53675b0d7ae5c774"></a>[`public static ulong Inno.Native.Sdl3.SDL.Utf8Strlcpy(byte* dst, byte* src, ulong dstBytes)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23594) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f7b2212dd5558f3a"></a>[`public static ulong Inno.Native.Sdl3.SDL.Utf8Strlen(byte* str)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23685) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3997030e1cb13629"></a>[`public static ulong Inno.Native.Sdl3.SDL.Utf8Strlen(string str)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23692) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e88092e63610319d"></a>[`public static ulong Inno.Native.Sdl3.SDL.Utf8Strnlen(byte* str, ulong bytes)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23708) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ade72f22fec58d62"></a>[`public static ulong Inno.Native.Sdl3.SDL.Utf8Strnlen(string str, ulong bytes)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23715) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7f0ac5b23498df6f"></a>[`public static int Inno.Native.Sdl3.SDL.Vasprintf(byte** strp, byte* fmt, byte* ap)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24116) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f953175c9b388d7c"></a>[`public static int Inno.Native.Sdl3.SDL.Vsnprintf(byte* text, ulong maxlen, byte* fmt, byte* ap)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24102) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f546ecd89548502f"></a>[`public static int Inno.Native.Sdl3.SDL.Vsscanf(byte* text, byte* fmt, byte* ap)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24095) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9cd8996880820399"></a>[`public static int Inno.Native.Sdl3.SDL.Vswprintf(ushort* text, ulong maxlen, ushort* fmt, byte* ap)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24109) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-adacbedb6bcbdd97"></a>[`public static byte Inno.Native.Sdl3.SDL.WaitAndAcquireGPUSwapchainTexture(Inno.Native.Sdl3.SDLGPUCommandBuffer commandBuffer, Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLGPUTexture* swapchainTexture, uint* swapchainTextureWidth, uint* swapchainTextureHeight)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32741) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e6d9691054453a2"></a>[`public static bool Inno.Native.Sdl3.SDL.WaitAsyncIOResult(Inno.Native.Sdl3.SDLAsyncIOQueue queue, ref Inno.Native.Sdl3.SDLAsyncIOOutcome outcome, int timeoutMS)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24717) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3a1e076bc27196fd"></a>[`public static bool Inno.Native.Sdl3.SDL.WaitAsyncIOResult(Inno.Native.Sdl3.SDLAsyncIOQueue queue, Inno.Native.Sdl3.SDLAsyncIOOutcomePtr outcome, int timeoutMS)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24727) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b0ee5588c5ba7c7c"></a>[`public static void Inno.Native.Sdl3.SDL.WaitCondition(Inno.Native.Sdl3.SDLCondition cond, Inno.Native.Sdl3.SDLMutex mutex)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25578) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2d6c0aad29678020"></a>[`public static bool Inno.Native.Sdl3.SDL.WaitConditionTimeout(Inno.Native.Sdl3.SDLCondition cond, Inno.Native.Sdl3.SDLMutex mutex, int timeoutMS)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25584) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-feccc00f0642112f"></a>[`public static bool Inno.Native.Sdl3.SDL.WaitEvent(ref Inno.Native.Sdl3.SDLEvent evnt)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31731) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe317cffc0c9fa1d"></a>[`public static bool Inno.Native.Sdl3.SDL.WaitEvent(Inno.Native.Sdl3.SDLEventPtr evnt)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31741) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e81eae7c95666506"></a>[`public static bool Inno.Native.Sdl3.SDL.WaitEventTimeout(ref Inno.Native.Sdl3.SDLEvent evnt, int timeoutMS)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31748) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ddeb6dc04ef0413"></a>[`public static bool Inno.Native.Sdl3.SDL.WaitEventTimeout(Inno.Native.Sdl3.SDLEventPtr evnt, int timeoutMS)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31758) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f558e2347f67eb6c"></a>[`public static bool Inno.Native.Sdl3.SDL.WaitForGPUFences(Inno.Native.Sdl3.SDLGPUDevice device, bool waitAll, Inno.Native.Sdl3.SDLGPUFence* fences, uint numFences)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32783) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-124b4602eefdcc9b"></a>[`public static byte Inno.Native.Sdl3.SDL.WaitForGPUFences(Inno.Native.Sdl3.SDLGPUDevice device, byte waitAll, Inno.Native.Sdl3.SDLGPUFence* fences, uint numFences)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32776) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4b848848494d9e77"></a>[`public static bool Inno.Native.Sdl3.SDL.WaitForGPUIdle(Inno.Native.Sdl3.SDLGPUDevice device)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32769) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7a10f1a958daa2b2"></a>[`public static bool Inno.Native.Sdl3.SDL.WaitForGPUSwapchain(Inno.Native.Sdl3.SDLGPUDevice device, Inno.Native.Sdl3.SDLWindow window)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32734) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-084b1919ca641428"></a>[`public static bool Inno.Native.Sdl3.SDL.WaitProcess(Inno.Native.Sdl3.SDLProcess process, bool block, ref int exitcode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33809) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-84585f052164b8d1"></a>[`public static byte Inno.Native.Sdl3.SDL.WaitProcess(Inno.Native.Sdl3.SDLProcess process, byte block, int* exitcode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33802) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-81c05f0684301176"></a>[`public static void Inno.Native.Sdl3.SDL.WaitSemaphore(Inno.Native.Sdl3.SDLSemaphore sem)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25520) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d14fc943f8ebc674"></a>[`public static bool Inno.Native.Sdl3.SDL.WaitSemaphoreTimeout(Inno.Native.Sdl3.SDLSemaphore sem, int timeoutMS)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25533) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6872e5dcd24a739c"></a>[`public static void Inno.Native.Sdl3.SDL.WaitThread(Inno.Native.Sdl3.SDLThread thread, ref int status)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25388) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-967ae80a9d491648"></a>[`public static void Inno.Native.Sdl3.SDL.WaitThread(Inno.Native.Sdl3.SDLThread thread, int* status)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25382) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1cc490f187df6693"></a>[`public static bool Inno.Native.Sdl3.SDL.WarpMouseGlobal(float x, float y)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31505) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0222abf3731c42f9"></a>[`public static void Inno.Native.Sdl3.SDL.WarpMouseInWindow(Inno.Native.Sdl3.SDLWindow window, float x, float y)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L31499) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-794854658b52057a"></a>[`public static Inno.Native.Sdl3.SDLInitFlags Inno.Native.Sdl3.SDL.WasInit(Inno.Native.Sdl3.SDLInitFlags flags)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L33449) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b4e8bf43a8827e8e"></a>[`public static int Inno.Native.Sdl3.SDL.Wcscasecmp(ushort* str1, ushort* str2)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23520) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-18a52895ce587cc6"></a>[`public static int Inno.Native.Sdl3.SDL.Wcscmp(ushort* str1, ushort* str2)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23506) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-363c0534970f0445"></a>[`public static ushort* Inno.Native.Sdl3.SDL.Wcsdup(ushort* wstr)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23485) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d78a23bbb5b24983"></a>[`public static ulong Inno.Native.Sdl3.SDL.Wcslcat(ref ushort dst, ushort* src, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23476) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a906df2761145ba8"></a>[`public static ulong Inno.Native.Sdl3.SDL.Wcslcat(ushort* dst, ushort* src, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23469) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-61df67f084045a0e"></a>[`public static ulong Inno.Native.Sdl3.SDL.Wcslcpy(ref ushort dst, ushort* src, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23460) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b71e7873d0b4fda3"></a>[`public static ulong Inno.Native.Sdl3.SDL.Wcslcpy(ushort* dst, ushort* src, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23453) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDL</code> · 2521–2552</summary>

<!-- api: M:Inno.Native.Sdl3.SDL.Wcslen(System.UInt16*) 656b7f8acbb99b60345ca02f42f917a9acbbef5931e026153337a49c0de99d9f -->
<!-- api: M:Inno.Native.Sdl3.SDL.Wcsncasecmp(System.UInt16*,System.UInt16*,System.UInt64) cd88217503632dd9885ef3e14ca8476dd749ade13bdf55fdafb047ec080ae846 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Wcsncmp(System.UInt16*,System.UInt16*,System.UInt64) a1bfd72fd2604388c0f61612a9f2fb2dfee08dc9a3eb01fb0c2133561e622759 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Wcsnlen(System.UInt16*,System.UInt64) 45e2a7a9d0970f9b8d5d987431d8abc27e68a17abe5c53d73c5d9add378e9262 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Wcsnstr(System.UInt16*,System.UInt16*,System.UInt64) cbbd147b0801b0323f70b1f68aac38c6b6610d892d3362b9aeae573deea6bb1b -->
<!-- api: M:Inno.Native.Sdl3.SDL.Wcsstr(System.UInt16*,System.UInt16*) b9be8da840e05ec859c169ebe400008992eb6ce385354f8df5b95b1d50c673e1 -->
<!-- api: M:Inno.Native.Sdl3.SDL.Wcstol(System.UInt16*,System.UInt16**,System.Int32) 86757fa11e87687f973efacae918283058a731dfe12b8f0144a24e283d3d9114 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WindowHasSurface(Inno.Native.Sdl3.SDLWindow) 61d83bf6c86c22e481f29ebc9a6fc950abcfc1111faf07ff5ac4e89b89e34cfe -->
<!-- api: M:Inno.Native.Sdl3.SDL.WindowSupportsGPUPresentMode(Inno.Native.Sdl3.SDLGPUDevice,Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLGPUPresentMode) 2f8fb9f3c194eddd7c05d849971da77e8c92e38c79bf7870cdacbec39f48f849 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WindowSupportsGPUSwapchainComposition(Inno.Native.Sdl3.SDLGPUDevice,Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLGPUSwapchainComposition) 5b9b8645192f1b45862a5c50b726622f51b91b1df228022934a2f8a2e08443f1 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteAsyncIO(Inno.Native.Sdl3.SDLAsyncIO,System.Void*,System.UInt64,System.UInt64,Inno.Native.Sdl3.SDLAsyncIOQueue,System.Void*) 9988e2e894c83ee4d762e945c091777f9ff6a67a3dab04e44a83513cbab2ee10 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteIO(Inno.Native.Sdl3.SDLIOStream,System.Void*,System.UInt64) 02f3ec30369945ede5e2ac1cd92c322ec1088c41470f35bce39747669902a822 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteS16BE(Inno.Native.Sdl3.SDLIOStream,System.Int16) 5aeb2dddd89c1dc13b2b667aa3771ddbbb8e92b84d934d13a6b559d111d32f5a -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteS16LE(Inno.Native.Sdl3.SDLIOStream,System.Int16) 29e7b6bff7f336e8d355e0087990b0031887924b721f218c1dd9476214b7efec -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteS32BE(Inno.Native.Sdl3.SDLIOStream,System.Int32) e4a2d226f24a9c0d79f433e7e48350c6582afa945d3cb74502a164a91972e25d -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteS32LE(Inno.Native.Sdl3.SDLIOStream,System.Int32) 9258960886d6ecc27d5ea8856e67de2c48544692719f45fc40e9a0448a0f78ad -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteS64BE(Inno.Native.Sdl3.SDLIOStream,System.Int64) 005f4569ff0a6ab2817c73df3a46629b7bafa61bf25e5a9e28a6fd256466bfc0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteS64LE(Inno.Native.Sdl3.SDLIOStream,System.Int64) 85d68c2c2d5fe253efaa4c4061d04a5d4508dc50d1bca3a966b8d11eeebbf771 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteS8(Inno.Native.Sdl3.SDLIOStream,System.Byte) 7c33a08e3867e7bd2cc5083f0a9b6f078375f87ec918939fd615490c12a4e3bf -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteStorageFile(Inno.Native.Sdl3.SDLStorage,System.Byte*,System.Void*,System.UInt64) 25bbe01b8685bf73933a78b5f873f2435f097e541aff25475af03391a66b6b97 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteStorageFile(Inno.Native.Sdl3.SDLStorage,System.String,System.Void*,System.UInt64) 53f189c62f8191fee9b7d49f5c36b31f4a6ce61c56151037aa93cbfee8ba411b -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteSurfacePixel(Inno.Native.Sdl3.SDLSurface@,System.Int32,System.Int32,System.Byte,System.Byte,System.Byte,System.Byte) b1113b577af92c917b88da2fe4981266010bdb287b71ae116c76739955594327 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteSurfacePixel(Inno.Native.Sdl3.SDLSurfacePtr,System.Int32,System.Int32,System.Byte,System.Byte,System.Byte,System.Byte) 81af5e15aafb098e0d63bf69c1933491ddf4649a09de50d60d1a9866a04851b0 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteSurfacePixelFloat(Inno.Native.Sdl3.SDLSurface@,System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single) df9d39d9c69f9f52e5ef031077c075410b1f59d27c446d8709a8af2d558d20f7 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteSurfacePixelFloat(Inno.Native.Sdl3.SDLSurfacePtr,System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single) 8607bfda8aaf4ba411b38ce23f4a87b48aad1d3e5a00cf6c1c03afac93c48106 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteU16BE(Inno.Native.Sdl3.SDLIOStream,System.UInt16) 88556402c68b1cfe47b015b757ec43828a026c281dcc6635b48ebbc77c5f6b71 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteU16LE(Inno.Native.Sdl3.SDLIOStream,System.UInt16) e48030402a71857174cefb00c14d05380ec007a6ba7657460297edf07c9688b7 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteU32BE(Inno.Native.Sdl3.SDLIOStream,System.UInt32) e59228ec042c9fef7eb1510abc681e898737d2690078f44bcbbef37a1060872f -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteU32LE(Inno.Native.Sdl3.SDLIOStream,System.UInt32) 7e89723ee09cec4170779fea995421b7f03a4e75545931548d8eb1b06ebd8f09 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteU64BE(Inno.Native.Sdl3.SDLIOStream,System.UInt64) 89d06869a306f62935f0b91f89b1beac3b31a63c9e2c0078083588f182e57ea2 -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteU64LE(Inno.Native.Sdl3.SDLIOStream,System.UInt64) fd87a30927f1d14ea09b5a85130ee5b78f2ba785fddb237a3ddd5c03182ec36c -->
<!-- api: M:Inno.Native.Sdl3.SDL.WriteU8(Inno.Native.Sdl3.SDLIOStream,System.Byte) 24f1055c63d1e783546ecf903cfc0d146538e538c466400bf42c7dd5adfadc8b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a547e6cc16c9116c"></a>[`public static ulong Inno.Native.Sdl3.SDL.Wcslen(ushort* wstr)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23439) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-891e33d701a83d87"></a>[`public static int Inno.Native.Sdl3.SDL.Wcsncasecmp(ushort* str1, ushort* str2, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23527) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fd1b50d7445c63f3"></a>[`public static int Inno.Native.Sdl3.SDL.Wcsncmp(ushort* str1, ushort* str2, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23513) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7600eeaef7d8bd25"></a>[`public static ulong Inno.Native.Sdl3.SDL.Wcsnlen(ushort* wstr, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23446) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eec7aea2f9db991a"></a>[`public static ushort* Inno.Native.Sdl3.SDL.Wcsnstr(ushort* haystack, ushort* needle, ulong maxlen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23499) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6d599f220cc1cc29"></a>[`public static ushort* Inno.Native.Sdl3.SDL.Wcsstr(ushort* haystack, ushort* needle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23492) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f5042c4591f8fb93"></a>[`public static int Inno.Native.Sdl3.SDL.Wcstol(ushort* str, ushort** endp, int baseValue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L23534) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ac10dea4c769e548"></a>[`public static bool Inno.Native.Sdl3.SDL.WindowHasSurface(Inno.Native.Sdl3.SDLWindow window)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L29340) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c22bc6ae9a9c01e2"></a>[`public static bool Inno.Native.Sdl3.SDL.WindowSupportsGPUPresentMode(Inno.Native.Sdl3.SDLGPUDevice device, Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLGPUPresentMode presentMode)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32686) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ac0eaec8a7d732d4"></a>[`public static bool Inno.Native.Sdl3.SDL.WindowSupportsGPUSwapchainComposition(Inno.Native.Sdl3.SDLGPUDevice device, Inno.Native.Sdl3.SDLWindow window, Inno.Native.Sdl3.SDLGPUSwapchainComposition swapchainComposition)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L32679) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4cb490889e930296"></a>[`public static byte Inno.Native.Sdl3.SDL.WriteAsyncIO(Inno.Native.Sdl3.SDLAsyncIO asyncio, void* ptr, ulong offset, ulong size, Inno.Native.Sdl3.SDLAsyncIOQueue queue, void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L24673) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6638a60c36f32d9a"></a>[`public static ulong Inno.Native.Sdl3.SDL.WriteIO(Inno.Native.Sdl3.SDLIOStream context, void* ptr, ulong size)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L25730) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-03a6020b9a5f3615"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteS16BE(Inno.Native.Sdl3.SDLIOStream dst, short value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26059) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bea1a5de79bcd063"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteS16LE(Inno.Native.Sdl3.SDLIOStream dst, short value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26045) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d17355232fd41274"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteS32BE(Inno.Native.Sdl3.SDLIOStream dst, int value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26087) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-87b996252d9b491f"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteS32LE(Inno.Native.Sdl3.SDLIOStream dst, int value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26073) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9aab41879c613d2e"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteS64BE(Inno.Native.Sdl3.SDLIOStream dst, long value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26115) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-364b57a4753d4d72"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteS64LE(Inno.Native.Sdl3.SDLIOStream dst, long value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26101) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5cfb6408de82a112"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteS8(Inno.Native.Sdl3.SDLIOStream dst, byte value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26031) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cfe80bb38544dbc3"></a>[`public static byte Inno.Native.Sdl3.SDL.WriteStorageFile(Inno.Native.Sdl3.SDLStorage storage, byte* path, void* source, ulong length)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35447) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7b923d054f5a7287"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteStorageFile(Inno.Native.Sdl3.SDLStorage storage, string path, void* source, ulong length)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L35454) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fbfb9dfdc87ec444"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteSurfacePixel(ref Inno.Native.Sdl3.SDLSurface surface, int x, int y, byte r, byte g, byte b, byte a)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L28223) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e8199673df80af4a"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteSurfacePixel(Inno.Native.Sdl3.SDLSurfacePtr surface, int x, int y, byte r, byte g, byte b, byte a)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L28233) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-caee5f2892f77dbc"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteSurfacePixelFloat(ref Inno.Native.Sdl3.SDLSurface surface, int x, int y, float r, float g, float b, float a)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L28240) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-48c1bf95e7393027"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteSurfacePixelFloat(Inno.Native.Sdl3.SDLSurfacePtr surface, int x, int y, float r, float g, float b, float a)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L28250) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-251a67edb4c0b443"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteU16BE(Inno.Native.Sdl3.SDLIOStream dst, ushort value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26052) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e9e8d9b0baa86539"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteU16LE(Inno.Native.Sdl3.SDLIOStream dst, ushort value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26038) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7351f091c8d6c875"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteU32BE(Inno.Native.Sdl3.SDLIOStream dst, uint value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26080) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4e09be35688fd3f0"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteU32LE(Inno.Native.Sdl3.SDLIOStream dst, uint value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26066) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-76df90e69ebf5cf7"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteU64BE(Inno.Native.Sdl3.SDLIOStream dst, ulong value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26108) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e055569325d06188"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteU64LE(Inno.Native.Sdl3.SDLIOStream dst, ulong value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26094) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-32e8a11808be89bd"></a>[`public static bool Inno.Native.Sdl3.SDL.WriteU8(Inno.Native.Sdl3.SDLIOStream dst, byte value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L26024) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAlignmentTest</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAlignmentTest ecc2ae5f090e7a86308b733e26056acddbedeb85e962735889a17cf69b8c3214 -->
<!-- api: F:Inno.Native.Sdl3.SDLAlignmentTest.A 213d3cbfcca5fe2cc0159945d5797611f05b45c8e9dd22cfd685cd27be1c6d54 -->
<!-- api: F:Inno.Native.Sdl3.SDLAlignmentTest.B f266a3bdbaa7e5948545fb5b1b2a73c4c7ee2616af059ffb962c1a122436c25f -->
<!-- api: M:Inno.Native.Sdl3.SDLAlignmentTest.#ctor(System.Byte,System.Void*) 261ded5a3b4ad1aacf3fcbcd1ff1c425974224c214a380128036ddb7c05f1e37 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d83ae94a809e30ca"></a>[`public byte Inno.Native.Sdl3.SDLAlignmentTest.A`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1909) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e5fe4a9f87421333"></a>[`public void* Inno.Native.Sdl3.SDLAlignmentTest.B`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1910) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-068db5b31011ee83"></a>[`public Inno.Native.Sdl3.SDLAlignmentTest.SDLAlignmentTest(byte a = 0, void* b = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1911) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f6ea3c6ad97be77d"></a>[`public struct Inno.Native.Sdl3.SDLAlignmentTest`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1907) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAppEventFunc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAppEventFunc 6be925af9f23a4dd94694a191d060555d0a1bde3345278801d1a499ca1ea480a -->
<!-- api: M:Inno.Native.Sdl3.SDLAppEventFunc.#ctor(System.Object,System.IntPtr) 53e6c4b2a18f628cff6e71bf72683a12a2f7fd0ea0a3b446dc866e5e18aabd2d -->
<!-- api: M:Inno.Native.Sdl3.SDLAppEventFunc.Invoke(System.Void*,Inno.Native.Sdl3.SDLEvent*) c0e21bfd31667a266a0cecee65cc4ac5e5069f49c6f528821399ad962300a14a -->
<!-- api: M:Inno.Native.Sdl3.SDLAppEventFunc.BeginInvoke(System.Void*,Inno.Native.Sdl3.SDLEvent*,System.AsyncCallback,System.Object) 6e88287b7c4d97d7b3a8f942d6b2c818dc4a9ce5d5fa7ed203f15a58d3844adb -->
<!-- api: M:Inno.Native.Sdl3.SDLAppEventFunc.EndInvoke(System.IAsyncResult) dc3f9ad47255b648d134f12fa375018a8ce6dddb374c4dc9594c03b1ac7ce294 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b3bc8167cef95862"></a>[`public Inno.Native.Sdl3.SDLAppEventFunc.SDLAppEventFunc(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6895) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ff26a0b8b10c5f12"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLAppEventFunc.BeginInvoke(void* appstate, Inno.Native.Sdl3.SDLEvent* evnt, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6895) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aec6cfc338300df5"></a>[`public virtual Inno.Native.Sdl3.SDLAppResult Inno.Native.Sdl3.SDLAppEventFunc.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6895) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ea1cc6e527ca6c4a"></a>[`public virtual Inno.Native.Sdl3.SDLAppResult Inno.Native.Sdl3.SDLAppEventFunc.Invoke(void* appstate, Inno.Native.Sdl3.SDLEvent* evnt)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6895) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-488135540ed44cc1"></a>[`public delegate Inno.Native.Sdl3.SDLAppResult Inno.Native.Sdl3.SDLAppEventFunc(void* appstate, Inno.Native.Sdl3.SDLEvent* evnt)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6895) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAppInitFunc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAppInitFunc 0fc05fbabac372e9863facac27a2aee7cf86a0c3b401ac139ba814adf93ea630 -->
<!-- api: M:Inno.Native.Sdl3.SDLAppInitFunc.#ctor(System.Object,System.IntPtr) 63b6686bab0d185efd2f01496137b5321a0fcd0b6832688c1b3cc4aca999826a -->
<!-- api: M:Inno.Native.Sdl3.SDLAppInitFunc.Invoke(System.Void**,System.Int32,System.Byte**) 1ebda78a3c7c3869d7799ad40d44adb44f366802c3e659b6b0c337f97061a231 -->
<!-- api: M:Inno.Native.Sdl3.SDLAppInitFunc.BeginInvoke(System.Void**,System.Int32,System.Byte**,System.AsyncCallback,System.Object) 1e2449c3464635325a2ede04faa374673e37284a1dbf1863ad410a6a2605508f -->
<!-- api: M:Inno.Native.Sdl3.SDLAppInitFunc.EndInvoke(System.IAsyncResult) b710479dfd1c0f2dd2064fd33dc1a45385b30b124534e85ac3612be5552a139d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-eac60bf995989ccc"></a>[`public Inno.Native.Sdl3.SDLAppInitFunc.SDLAppInitFunc(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6885) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a43d26c714feadb5"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLAppInitFunc.BeginInvoke(void** appstate, int argc, byte** argv, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6885) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a9304ff7f4698e44"></a>[`public virtual Inno.Native.Sdl3.SDLAppResult Inno.Native.Sdl3.SDLAppInitFunc.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6885) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-50d0a74dad3935e7"></a>[`public virtual Inno.Native.Sdl3.SDLAppResult Inno.Native.Sdl3.SDLAppInitFunc.Invoke(void** appstate, int argc, byte** argv)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6885) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e81f9d2d655cd41a"></a>[`public delegate Inno.Native.Sdl3.SDLAppResult Inno.Native.Sdl3.SDLAppInitFunc(void** appstate, int argc, byte** argv)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6885) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAppIterateFunc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAppIterateFunc 9e943d554da9c3909a949044135fb75185a745a5afa1cad3c8965d6e1e361c42 -->
<!-- api: M:Inno.Native.Sdl3.SDLAppIterateFunc.#ctor(System.Object,System.IntPtr) 0540eeb5bc0f6c30bdbbee7723a7ac0e3a65e8b3c5038558c746dbd3c1dfcd4b -->
<!-- api: M:Inno.Native.Sdl3.SDLAppIterateFunc.Invoke(System.Void*) cfd0d587cb9a118cc890bb7035a94d19cd07bac5b11eaaf1ecd937f24ea62751 -->
<!-- api: M:Inno.Native.Sdl3.SDLAppIterateFunc.BeginInvoke(System.Void*,System.AsyncCallback,System.Object) c1e65ac2651865d7a74944fe410a32c49d6aba951eedcd6a81c62da4dfe469d8 -->
<!-- api: M:Inno.Native.Sdl3.SDLAppIterateFunc.EndInvoke(System.IAsyncResult) 17f3aa39a5bbcc8cf05af81120feeac8e301c6c5770a7a13045655701e7b736f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c8afed3be80aa88f"></a>[`public Inno.Native.Sdl3.SDLAppIterateFunc.SDLAppIterateFunc(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6890) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4712edc0549535a3"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLAppIterateFunc.BeginInvoke(void* appstate, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6890) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-83673c38dc1355c7"></a>[`public virtual Inno.Native.Sdl3.SDLAppResult Inno.Native.Sdl3.SDLAppIterateFunc.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6890) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6357c6d527f45e0d"></a>[`public virtual Inno.Native.Sdl3.SDLAppResult Inno.Native.Sdl3.SDLAppIterateFunc.Invoke(void* appstate)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6890) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cae7e24799fbb744"></a>[`public delegate Inno.Native.Sdl3.SDLAppResult Inno.Native.Sdl3.SDLAppIterateFunc(void* appstate)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6890) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAppQuitFunc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAppQuitFunc 9a725e38bf80185ed9497cf934d36d80fff873f861ea169c865ea4ec42d8f52f -->
<!-- api: M:Inno.Native.Sdl3.SDLAppQuitFunc.#ctor(System.Object,System.IntPtr) cc4f4b08063c5e60d0925947041533f54f1e12df44e4b55c57c7487d2fc3bcd5 -->
<!-- api: M:Inno.Native.Sdl3.SDLAppQuitFunc.Invoke(System.Void*,Inno.Native.Sdl3.SDLAppResult) 078372993469d67e939173cd3c9cadfe70eaeb0a8e08d1bf024eb9c2c74d33fe -->
<!-- api: M:Inno.Native.Sdl3.SDLAppQuitFunc.BeginInvoke(System.Void*,Inno.Native.Sdl3.SDLAppResult,System.AsyncCallback,System.Object) d0e4a7d3deda3f7783b3f57bbdc9be9b6dc20d276b53af8825350004337591d1 -->
<!-- api: M:Inno.Native.Sdl3.SDLAppQuitFunc.EndInvoke(System.IAsyncResult) 91741eb996268a3b1da04e19844aa1441008f24f9f667a018f5a573ddbb205eb -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-bc48f06d2ecb710b"></a>[`public Inno.Native.Sdl3.SDLAppQuitFunc.SDLAppQuitFunc(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6900) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f113306ad830457f"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLAppQuitFunc.BeginInvoke(void* appstate, Inno.Native.Sdl3.SDLAppResult result, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6900) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b8d18313b04bc6a6"></a>[`public virtual void Inno.Native.Sdl3.SDLAppQuitFunc.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6900) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3e6efdef4fb68e88"></a>[`public virtual void Inno.Native.Sdl3.SDLAppQuitFunc.Invoke(void* appstate, Inno.Native.Sdl3.SDLAppResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6900) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-521fef9af632eedf"></a>[`public delegate void Inno.Native.Sdl3.SDLAppQuitFunc(void* appstate, Inno.Native.Sdl3.SDLAppResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6900) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAppResult</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAppResult d46bf1e417b0058bb77eca00f8477add9bb3888b3b6eadd3b551dbf68a69fcff -->
<!-- api: F:Inno.Native.Sdl3.SDLAppResult.Continue 2e7fce1fccc2c3dde3e575255004b62d4c310e4fc47e3160731039c96185e160 -->
<!-- api: F:Inno.Native.Sdl3.SDLAppResult.Success 3e63c72251769060357046bf229730e5bce7732a468a748a60d9ef49c5ac258f -->
<!-- api: F:Inno.Native.Sdl3.SDLAppResult.Failure abf7a0eddd593d80a00f97d1e4dca5f56cfb117f7cbbbd265c45012b256419e3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d4a7ea909a6f4de8"></a>[`Inno.Native.Sdl3.SDLAppResult.Continue = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1767) | 当前常量或枚举值：`0`。 | — |
| <a id="api-008125c0397d59c5"></a>[`Inno.Native.Sdl3.SDLAppResult.Failure = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1769) | 当前常量或枚举值：`2`。 | — |
| <a id="api-7e76e5dcef55a459"></a>[`Inno.Native.Sdl3.SDLAppResult.Success = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1768) | 当前常量或枚举值：`1`。 | — |
| <a id="api-1f52d40045b5650a"></a>[`public enum Inno.Native.Sdl3.SDLAppResult : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1765) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLArrayOrder</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLArrayOrder e0496a807596987ced946a21f01fd95df74cbe416ab35950ca10da2667a17ce7 -->
<!-- api: F:Inno.Native.Sdl3.SDLArrayOrder.ArrayorderNone 04c86f01fd1701fbdc688b68003c410664ad1059cb97531230773102a9979a43 -->
<!-- api: F:Inno.Native.Sdl3.SDLArrayOrder.ArrayorderRgb 4efedd24ce746493fca1c970d8bba6b57b662d0ca6c55842d89030a119f85458 -->
<!-- api: F:Inno.Native.Sdl3.SDLArrayOrder.ArrayorderRgba 3f3e9282abed4770393cc5cc179f6d76bc028905c5eede6e8c06b0a3e56c1353 -->
<!-- api: F:Inno.Native.Sdl3.SDLArrayOrder.ArrayorderArgb 57ce8aab4fb39cc76f6ab808bacc2fe273dbb2e6a7dd3e13c427cddd86922e26 -->
<!-- api: F:Inno.Native.Sdl3.SDLArrayOrder.ArrayorderBgr 91203d9bf3f1287ffa74b129f7690b961897a7b1dc2202b7bb362988e30f0cfb -->
<!-- api: F:Inno.Native.Sdl3.SDLArrayOrder.ArrayorderBgra 426b3616f1ade98c7eac337652e75e2bd8a391db14b578cd7289ea66d1ca412b -->
<!-- api: F:Inno.Native.Sdl3.SDLArrayOrder.ArrayorderAbgr 08b4c377471f403b36287cede6b38d5f978790e19caae9e58047d0b0412ea882 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b5511acb032f44ee"></a>[`Inno.Native.Sdl3.SDLArrayOrder.ArrayorderAbgr = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L281) | 当前常量或枚举值：`6`。 | — |
| <a id="api-a407355a01f20330"></a>[`Inno.Native.Sdl3.SDLArrayOrder.ArrayorderArgb = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L278) | 当前常量或枚举值：`3`。 | — |
| <a id="api-c16c942785c84de5"></a>[`Inno.Native.Sdl3.SDLArrayOrder.ArrayorderBgr = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L279) | 当前常量或枚举值：`4`。 | — |
| <a id="api-9eb37524d403d439"></a>[`Inno.Native.Sdl3.SDLArrayOrder.ArrayorderBgra = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L280) | 当前常量或枚举值：`5`。 | — |
| <a id="api-d3a26ff863e6f621"></a>[`Inno.Native.Sdl3.SDLArrayOrder.ArrayorderNone = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L275) | 当前常量或枚举值：`0`。 | — |
| <a id="api-34bd2ac76d5caaf5"></a>[`Inno.Native.Sdl3.SDLArrayOrder.ArrayorderRgb = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L276) | 当前常量或枚举值：`1`。 | — |
| <a id="api-2b8b8be8fb951ff8"></a>[`Inno.Native.Sdl3.SDLArrayOrder.ArrayorderRgba = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L277) | 当前常量或枚举值：`2`。 | — |
| <a id="api-a322d67f6f58f2f6"></a>[`public enum Inno.Native.Sdl3.SDLArrayOrder : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L273) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAssertData</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAssertData 7da5960bf0129d3ea9bd5a30ac8d1a906589b6f35ab5a6370116cacc0aeeac75 -->
<!-- api: F:Inno.Native.Sdl3.SDLAssertData.AlwaysIgnore bdca691f27755402bdb51d18f2ada590fdbac05209965e0dbfd89a17b66afb36 -->
<!-- api: F:Inno.Native.Sdl3.SDLAssertData.TriggerCount 9935c7fd5e9951d7c257cedb6ef23da461e1a4259add297da083e1ef0731279b -->
<!-- api: F:Inno.Native.Sdl3.SDLAssertData.Condition a0a3e33ce370ca7b08ae2826bf2b424f9f5e727d07379dbd868a2ee6ea745423 -->
<!-- api: F:Inno.Native.Sdl3.SDLAssertData.Filename 1f9ab358f241611d070c671dee6ade00342fd6e70350d8cfa3bf51bd52a49ab8 -->
<!-- api: F:Inno.Native.Sdl3.SDLAssertData.Linenum 5cab97aab78a0d4f9187c946f038211504b9950a732d8f47d9297730e52c8429 -->
<!-- api: F:Inno.Native.Sdl3.SDLAssertData.Function 2fd7db1ca7fe575b2dae627d219c74ac67b0444069dab2f7672531c20adff5b3 -->
<!-- api: F:Inno.Native.Sdl3.SDLAssertData.Next 3161785874b4d1f678cd7e778c6821ec013ef47cd5c016e02c0ba14e2bb991e6 -->
<!-- api: M:Inno.Native.Sdl3.SDLAssertData.#ctor(System.Byte,System.UInt32,System.Byte*,System.Byte*,System.Int32,System.Byte*,Inno.Native.Sdl3.SDLAssertData*) 5c7cea58aa8a6f5d6c7437ef5d4e843e92100c4b10708323392f84b705ea461c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-22a0e8cbf1b16055"></a>[`public byte Inno.Native.Sdl3.SDLAssertData.AlwaysIgnore`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1972) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aca65e8209c4c622"></a>[`public byte* Inno.Native.Sdl3.SDLAssertData.Condition`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1974) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5dd662a926bebeba"></a>[`public byte* Inno.Native.Sdl3.SDLAssertData.Filename`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1975) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-81c2906eec12330f"></a>[`public byte* Inno.Native.Sdl3.SDLAssertData.Function`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1977) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-806b00431b9103ed"></a>[`public int Inno.Native.Sdl3.SDLAssertData.Linenum`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1976) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-633f59b4ee6de769"></a>[`public Inno.Native.Sdl3.SDLAssertData* Inno.Native.Sdl3.SDLAssertData.Next`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1978) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-555d097f22f38085"></a>[`public uint Inno.Native.Sdl3.SDLAssertData.TriggerCount`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1973) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4cf0ed0b951235d"></a>[`public Inno.Native.Sdl3.SDLAssertData.SDLAssertData(byte always_ignore = 0, uint trigger_count = 0, byte* condition = null, byte* filename = null, int linenum = 0, byte* function = null, Inno.Native.Sdl3.SDLAssertData* next = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1979) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-55320fa7066f474c"></a>[`public struct Inno.Native.Sdl3.SDLAssertData`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1970) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAssertDataPtr</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAssertDataPtr 78e8557380481243141eebddc2e4ccd6d7cf6c5faeecb61f2ad963a41c5cbc2b -->
<!-- api: M:Inno.Native.Sdl3.SDLAssertDataPtr.#ctor(Inno.Native.Sdl3.SDLAssertData*) 2fb4ae2af325ec64bda36480152cd76ee1d5ab8208e177ba52d70ef38ce9fd72 -->
<!-- api: F:Inno.Native.Sdl3.SDLAssertDataPtr.Handle 6a09026e50973498fdd5633d2acfa15e83b1ccaa28cfe4695894bc16acf1257b -->
<!-- api: P:Inno.Native.Sdl3.SDLAssertDataPtr.IsNull 92f51d2d1b4a8bab2af679aad7e1b3617fc17764b38f3793645a8a1b7e0fba3b -->
<!-- api: P:Inno.Native.Sdl3.SDLAssertDataPtr.Null 4a5ebfcf268b800dd70c8c68fcaa68b91404c1c1c76f3550a5f37d45602b374d -->
<!-- api: P:Inno.Native.Sdl3.SDLAssertDataPtr.Item(System.Int32) f1ccf711529fcf6e5d225529ff3b94ae36c66d2871c58c130ab6609d3a2f824d -->
<!-- api: M:Inno.Native.Sdl3.SDLAssertDataPtr.op_Implicit(Inno.Native.Sdl3.SDLAssertData*)~Inno.Native.Sdl3.SDLAssertDataPtr 41f541ff55c152c07d2a65695ad8ed512a47387fb3acd974ba4cd75fd288c4c6 -->
<!-- api: M:Inno.Native.Sdl3.SDLAssertDataPtr.op_Implicit(Inno.Native.Sdl3.SDLAssertDataPtr)~Inno.Native.Sdl3.SDLAssertData* f102285721a1012588a3f423d06e60fd971441240ad97a70c1ba4d13157e9738 -->
<!-- api: M:Inno.Native.Sdl3.SDLAssertDataPtr.op_Equality(Inno.Native.Sdl3.SDLAssertDataPtr,Inno.Native.Sdl3.SDLAssertDataPtr) a984fe5c2f0b858d1d237bc5527d27594493dcbd2c460208982847c52b735d2c -->
<!-- api: M:Inno.Native.Sdl3.SDLAssertDataPtr.op_Inequality(Inno.Native.Sdl3.SDLAssertDataPtr,Inno.Native.Sdl3.SDLAssertDataPtr) 97bcc7a5953c630783573baac675ec5ea6b935a856482d8d8c059d2fa03107da -->
<!-- api: M:Inno.Native.Sdl3.SDLAssertDataPtr.Equals(Inno.Native.Sdl3.SDLAssertDataPtr) fe526f06eb9f47481a0b558cdc3319de4a56da82d671aec523511c8f66762b67 -->
<!-- api: M:Inno.Native.Sdl3.SDLAssertDataPtr.Equals(System.Object) 64293295f922b0d3117735a633b0e0cbf9a55e83af0e51f713361e0bdf322620 -->
<!-- api: M:Inno.Native.Sdl3.SDLAssertDataPtr.GetHashCode 01d4d741563eef393ea1e62900245a1e3da63e5411e9345cc74034d0ac79b797 -->
<!-- api: P:Inno.Native.Sdl3.SDLAssertDataPtr.AlwaysIgnore 6bff8e8412af3dcdcaddb7fe6c99cefa642eac3a6bfbb3de8813ef96194572c8 -->
<!-- api: P:Inno.Native.Sdl3.SDLAssertDataPtr.TriggerCount 5baf60971240caf7bd25ee0adcfbe996dfdef316c2adc3a54506371489b08f24 -->
<!-- api: P:Inno.Native.Sdl3.SDLAssertDataPtr.Condition 5d2a2aa2368011437f16c3610b0b9b0392a3a68bc642638210a006c89e6ea724 -->
<!-- api: P:Inno.Native.Sdl3.SDLAssertDataPtr.Filename 2dcb1574bf62e33647205bc729815f4026325e7f2a626ec44b4db4d9cb093c2b -->
<!-- api: P:Inno.Native.Sdl3.SDLAssertDataPtr.Linenum dc29bdfa775f77aa6a44904d1bfea852d08d526e593a872cf9be4a38117635bb -->
<!-- api: P:Inno.Native.Sdl3.SDLAssertDataPtr.Function d77e454ab9bfa7ed33624cc9361d8929c4f69181b9d1f14d062a8ddf5d5ebd82 -->
<!-- api: P:Inno.Native.Sdl3.SDLAssertDataPtr.Next 436bf86f5712e90252b5c9a469a16164543a8a40caf6e15cbb5bf9c3867fe488 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c1d7399eb777f844"></a>[`public Inno.Native.Sdl3.SDLAssertData* Inno.Native.Sdl3.SDLAssertDataPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7898) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bade8b0930f27cce"></a>[`public Inno.Native.Sdl3.SDLAssertDataPtr.SDLAssertDataPtr(Inno.Native.Sdl3.SDLAssertData* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7893) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-97810a9ce206f5ae"></a>[`public bool Inno.Native.Sdl3.SDLAssertDataPtr.Equals(Inno.Native.Sdl3.SDLAssertDataPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7908) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-1ab3e3d7a860a229"></a>[`public override bool Inno.Native.Sdl3.SDLAssertDataPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7909) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-4531e55f89691bf5"></a>[`public override int Inno.Native.Sdl3.SDLAssertDataPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7910) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-a9bedc34da94a4d2"></a>[`public static bool Inno.Native.Sdl3.SDLAssertDataPtr.operator ==(Inno.Native.Sdl3.SDLAssertDataPtr left, Inno.Native.Sdl3.SDLAssertDataPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7906) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-12c4e6f42506d014"></a>[`public static Inno.Native.Sdl3.SDLAssertDataPtr.implicit operator Inno.Native.Sdl3.SDLAssertDataPtr(Inno.Native.Sdl3.SDLAssertData* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7904) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-40becd2e830e8273"></a>[`public static Inno.Native.Sdl3.SDLAssertDataPtr.implicit operator Inno.Native.Sdl3.SDLAssertData*(Inno.Native.Sdl3.SDLAssertDataPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7905) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-45dad4a08ad644c2"></a>[`public static bool Inno.Native.Sdl3.SDLAssertDataPtr.operator !=(Inno.Native.Sdl3.SDLAssertDataPtr left, Inno.Native.Sdl3.SDLAssertDataPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7907) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1722e145e4c44b46"></a>[`public bool Inno.Native.Sdl3.SDLAssertDataPtr.AlwaysIgnore { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7912) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-be1308bd0b4ee5a3"></a>[`public byte* Inno.Native.Sdl3.SDLAssertDataPtr.Condition { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7914) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bdc36e98fe3b24fa"></a>[`public byte* Inno.Native.Sdl3.SDLAssertDataPtr.Filename { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7915) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e6f5ff28ac483731"></a>[`public byte* Inno.Native.Sdl3.SDLAssertDataPtr.Function { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7917) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0e191a77c6d1bdf6"></a>[`public bool Inno.Native.Sdl3.SDLAssertDataPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7899) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-858680a62b6eb05b"></a>[`public Inno.Native.Sdl3.SDLAssertData Inno.Native.Sdl3.SDLAssertDataPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7902) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fc3995659177c272"></a>[`public ref int Inno.Native.Sdl3.SDLAssertDataPtr.Linenum { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7916) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9179d74f11084399"></a>[`public ref Inno.Native.Sdl3.SDLAssertDataPtr Inno.Native.Sdl3.SDLAssertDataPtr.Next { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7918) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e5fd3ebd7b1b8677"></a>[`public static Inno.Native.Sdl3.SDLAssertDataPtr Inno.Native.Sdl3.SDLAssertDataPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7900) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-26177145795c6b73"></a>[`public ref uint Inno.Native.Sdl3.SDLAssertDataPtr.TriggerCount { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7913) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-09486cbe9d8a1973"></a>[`public struct Inno.Native.Sdl3.SDLAssertDataPtr : System.IEquatable<Inno.Native.Sdl3.SDLAssertDataPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7891) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAssertionHandler</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAssertionHandler 0b1c8c7806c4129653720d031ddab538ce1ac483b05148d0ccf1f2d81d78eb41 -->
<!-- api: M:Inno.Native.Sdl3.SDLAssertionHandler.#ctor(System.Object,System.IntPtr) 79d6f8993d347b6dddf17ae0ca840d20cdf549a7a7025b7ab70a5d03b58bfd27 -->
<!-- api: M:Inno.Native.Sdl3.SDLAssertionHandler.Invoke(Inno.Native.Sdl3.SDLAssertData*,System.Void*) 9db42d1419bb6cb74ea6d2b1b379a59a34d000e01a05726a45787466b2352170 -->
<!-- api: M:Inno.Native.Sdl3.SDLAssertionHandler.BeginInvoke(Inno.Native.Sdl3.SDLAssertData*,System.Void*,System.AsyncCallback,System.Object) 7d2a551c0e5d2cf50e5cc8ce6662fc0a0ed5ae98329328af8ab0bf13e97e2c44 -->
<!-- api: M:Inno.Native.Sdl3.SDLAssertionHandler.EndInvoke(System.IAsyncResult) 62bb9b7322eae9a77998ef7582975f4f70bd99774792b7d62ab47040ac306171 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1c9560694749e6c4"></a>[`public Inno.Native.Sdl3.SDLAssertionHandler.SDLAssertionHandler(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6800) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-dbc2b45e6f8baadc"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLAssertionHandler.BeginInvoke(Inno.Native.Sdl3.SDLAssertData* data, void* userdata, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6800) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6373bce717442a22"></a>[`public virtual Inno.Native.Sdl3.SDLAssertState Inno.Native.Sdl3.SDLAssertionHandler.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6800) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a91988fa05e72f2b"></a>[`public virtual Inno.Native.Sdl3.SDLAssertState Inno.Native.Sdl3.SDLAssertionHandler.Invoke(Inno.Native.Sdl3.SDLAssertData* data, void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6800) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e7664fdbc56e869f"></a>[`public delegate Inno.Native.Sdl3.SDLAssertState Inno.Native.Sdl3.SDLAssertionHandler(Inno.Native.Sdl3.SDLAssertData* data, void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6800) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAssertState</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAssertState 9e4bccdbf8d774e7b64d82b3f68998f0945c1339ec841496e7e2c03c46a4de5f -->
<!-- api: F:Inno.Native.Sdl3.SDLAssertState.AssertionRetry 1ba3e72bdb1746fa23d0286167ef2decafa0976bf8a569c327c208d5d617f7fe -->
<!-- api: F:Inno.Native.Sdl3.SDLAssertState.AssertionBreak 575b831371bf53eb388b4747c838a1bf50100ad4276effb9fa68dc11598ac288 -->
<!-- api: F:Inno.Native.Sdl3.SDLAssertState.AssertionAbort 9bbbadc2fcb49118ded237579bae98ffde2210d23bedea69d9dfc32461a08723 -->
<!-- api: F:Inno.Native.Sdl3.SDLAssertState.AssertionIgnore f11b254bd2ffe595cdf13b9ad1bfa5835cd721042e87673fc956001addb3b5d4 -->
<!-- api: F:Inno.Native.Sdl3.SDLAssertState.AssertionAlwaysIgnore d86c80f074e4c7eecee6c63b850b7489f50483988dcdb70e48dbc45fb0217eac -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0dc870f70cb7c590"></a>[`Inno.Native.Sdl3.SDLAssertState.AssertionAbort = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L82) | 当前常量或枚举值：`2`。 | — |
| <a id="api-e9c5e7d7abaae82c"></a>[`Inno.Native.Sdl3.SDLAssertState.AssertionAlwaysIgnore = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L84) | 当前常量或枚举值：`4`。 | — |
| <a id="api-c82d97605741e1df"></a>[`Inno.Native.Sdl3.SDLAssertState.AssertionBreak = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L81) | 当前常量或枚举值：`1`。 | — |
| <a id="api-266d903ad0a4f894"></a>[`Inno.Native.Sdl3.SDLAssertState.AssertionIgnore = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L83) | 当前常量或枚举值：`3`。 | — |
| <a id="api-459bc846f0a663e9"></a>[`Inno.Native.Sdl3.SDLAssertState.AssertionRetry = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L80) | 当前常量或枚举值：`0`。 | — |
| <a id="api-ab14ce6c82eb219a"></a>[`public enum Inno.Native.Sdl3.SDLAssertState : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L78) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAsyncIO</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAsyncIO d60a9d2f51612e7c5de8eae8b883c25d0fdc777d9b4dfcfc3335be65511fada0 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIO.#ctor(System.IntPtr) af5625a9d3ff4285ee0c35e2556098240d182a71353300826f5c1ff3680231c9 -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIO.Handle c3ec4935721fed5584a4f97446f42f1aa2830144be42d71b9ac2a801c42a7bc9 -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIO.IsNull a40b92a4ce03b3832df8aa1bd0fe33fde5350d5c29fa0843af4eb7077f9149c5 -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIO.Null d663e05f60546c8bf747b08f16a7e9aca263d561ab6164fc50ec01bee5847b0a -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIO.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLAsyncIO 96c05dad709cf868c310dfb565fb6bd701bce1794774dd946b1f6ba2aedb35e2 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIO.op_Equality(Inno.Native.Sdl3.SDLAsyncIO,Inno.Native.Sdl3.SDLAsyncIO) 1648a4c4874f428832108ae22d26b1cd84c2aff65526ce7c4a11bb3fc0742235 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIO.op_Inequality(Inno.Native.Sdl3.SDLAsyncIO,Inno.Native.Sdl3.SDLAsyncIO) e62b1071a563e8e763f82b132845529bd8416085103cd62704b274431bcbdaf6 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIO.Equals(Inno.Native.Sdl3.SDLAsyncIO) dea027fbd1cb18ad7260a2a21e0198b902f935c1d9df8742ab27cb9df7da1d15 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIO.Equals(System.Object) d0a99f9bedfdb0e91105b58604732ae67c1c09be892076b35abec4dac5a141ec -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIO.GetHashCode 1df7f48157ca409e5ede3f3b0d585a095dc91e4fa3bd6f2bb212e4cef0945efa -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6dcc6210f86e056b"></a>[`public Inno.Native.Sdl3.SDLAsyncIO.SDLAsyncIO(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1997) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-61e301b18a7a9c73"></a>[`public bool Inno.Native.Sdl3.SDLAsyncIO.Equals(Inno.Native.Sdl3.SDLAsyncIO other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2009) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-4ded67094f6c33e0"></a>[`public override bool Inno.Native.Sdl3.SDLAsyncIO.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2010) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-a854c033380a1d6a"></a>[`public override int Inno.Native.Sdl3.SDLAsyncIO.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2011) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-7ab096d5a6373b33"></a>[`public static bool Inno.Native.Sdl3.SDLAsyncIO.operator ==(Inno.Native.Sdl3.SDLAsyncIO left, Inno.Native.Sdl3.SDLAsyncIO right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2007) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b92d1abd95f9cfa7"></a>[`public static Inno.Native.Sdl3.SDLAsyncIO.implicit operator Inno.Native.Sdl3.SDLAsyncIO(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2006) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-75a7912aba9113ad"></a>[`public static bool Inno.Native.Sdl3.SDLAsyncIO.operator !=(Inno.Native.Sdl3.SDLAsyncIO left, Inno.Native.Sdl3.SDLAsyncIO right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2008) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-37158c7a370b5958"></a>[`public nint Inno.Native.Sdl3.SDLAsyncIO.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2002) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6ccbe5e1442de054"></a>[`public bool Inno.Native.Sdl3.SDLAsyncIO.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2003) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-31b9de2c1265c692"></a>[`public static Inno.Native.Sdl3.SDLAsyncIO Inno.Native.Sdl3.SDLAsyncIO.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2004) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-022a1611378a8f9d"></a>[`public readonly struct Inno.Native.Sdl3.SDLAsyncIO : System.IEquatable<Inno.Native.Sdl3.SDLAsyncIO>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1995) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAsyncIOOutcome</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAsyncIOOutcome 6fb892c31ca7dfc7e52ad0e008aa5f9f665e6116f2aee193216384702fdfcd3f -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOOutcome.Asyncio 68004c23a3ac6b93dc131faa53475f5fba64e81e2cb078e2d2b78ccfe4b22c4d -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOOutcome.Type 634e1acde700cd1e8e6388f23e99c354731561f0f6662c0b58dbe23732e7455e -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOOutcome.Result f2db173c83e7100fa13143b87aa2b9af458129074582b2363f68a5d573810cfb -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOOutcome.Buffer b40dd64a5e5c8fee5fbe80e207470d790f9a621e5a342b5f5bfba9c51fec72ca -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOOutcome.Offset 07fafae3251b7cc6d83dce4e0eb282723286f9ef1626734386705405eabab989 -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOOutcome.BytesRequested 72af8f6942363f6824a93c4559f11527d2a4eb2a1836699d7c7a5d0c8d492209 -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOOutcome.BytesTransferred 33ddb466f6dc7e85cadf052cb19c15c6e035eccefeb9d47b031784e7a1c36228 -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOOutcome.Userdata 1d9695da7d54a193352761da8a104cc98f448879e1b97d2c7112632181f27ae6 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOOutcome.#ctor(Inno.Native.Sdl3.SDLAsyncIO,Inno.Native.Sdl3.SDLAsyncIOTaskType,Inno.Native.Sdl3.SDLAsyncIOResult,System.Void*,System.UInt64,System.UInt64,System.UInt64,System.Void*) 4f3d973baf6a3ce2e597093a71dae56170cbcf4305b0025c90a9bf7b0191dd82 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4c98925710389c61"></a>[`public Inno.Native.Sdl3.SDLAsyncIO Inno.Native.Sdl3.SDLAsyncIOOutcome.Asyncio`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2021) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-75a26e5b740d67d3"></a>[`public void* Inno.Native.Sdl3.SDLAsyncIOOutcome.Buffer`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2024) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cacc00bf1e34063f"></a>[`public ulong Inno.Native.Sdl3.SDLAsyncIOOutcome.BytesRequested`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2026) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-77201a1088521d1b"></a>[`public ulong Inno.Native.Sdl3.SDLAsyncIOOutcome.BytesTransferred`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2027) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1940ff48df7892bc"></a>[`public ulong Inno.Native.Sdl3.SDLAsyncIOOutcome.Offset`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2025) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-14e74823fc602b7c"></a>[`public Inno.Native.Sdl3.SDLAsyncIOResult Inno.Native.Sdl3.SDLAsyncIOOutcome.Result`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2023) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a1fd442db3208e00"></a>[`public Inno.Native.Sdl3.SDLAsyncIOTaskType Inno.Native.Sdl3.SDLAsyncIOOutcome.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2022) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-590b9eb588944d23"></a>[`public void* Inno.Native.Sdl3.SDLAsyncIOOutcome.Userdata`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2028) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-83c459ffc98b3e5f"></a>[`public Inno.Native.Sdl3.SDLAsyncIOOutcome.SDLAsyncIOOutcome(Inno.Native.Sdl3.SDLAsyncIO asyncio = default(Inno.Native.Sdl3.SDLAsyncIO), Inno.Native.Sdl3.SDLAsyncIOTaskType type = Inno.Native.Sdl3.SDLAsyncIOTaskType.AsyncioTaskRead, Inno.Native.Sdl3.SDLAsyncIOResult result = Inno.Native.Sdl3.SDLAsyncIOResult.AsyncioComplete, void* buffer = null, ulong offset = 0, ulong bytes_requested = 0, ulong bytes_transferred = 0, void* userdata = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2029) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f51f5043f194c366"></a>[`public struct Inno.Native.Sdl3.SDLAsyncIOOutcome`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2019) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAsyncIOOutcomePtr</code> · 21 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr 89be092c80a564ee2d13e4b414f94cded3ea6fbb5b28b632a405e439a9f742fb -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.#ctor(Inno.Native.Sdl3.SDLAsyncIOOutcome*) bb2bd6065e655efb3e4f7b61b69b5961e6eeaf14a98789575414d33101b0bdce -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Handle a24e5dd6cec81f3012e4263646be5edb3a45ec183f285b27b1152c35b80aacef -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.IsNull b73c4f449849cf5f5a44710638037454f06dbcec41db1cd8231207f35367f4e0 -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Null bab5c9665ef163ff1099a769afd743e51ef62043ad5cdfca4f46b9f9c2e7136d -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Item(System.Int32) 96a541040bdb41a28ab336b8b2bacaef05d788d320387ac28e65cffa04ec4774 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.op_Implicit(Inno.Native.Sdl3.SDLAsyncIOOutcome*)~Inno.Native.Sdl3.SDLAsyncIOOutcomePtr 0c82ba511c646f221c57dca38249722ee4e6efa65f7ce8a740632cde779856c3 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.op_Implicit(Inno.Native.Sdl3.SDLAsyncIOOutcomePtr)~Inno.Native.Sdl3.SDLAsyncIOOutcome* 137c76c3d2e69ccad57283f95069516ee4cfafaf62d27ba7c598c125eea4d2ad -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.op_Equality(Inno.Native.Sdl3.SDLAsyncIOOutcomePtr,Inno.Native.Sdl3.SDLAsyncIOOutcomePtr) cb17bea6a15c12667ff593ca6b77ff3e225d257dd615f7d0563543ae60118fc5 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.op_Inequality(Inno.Native.Sdl3.SDLAsyncIOOutcomePtr,Inno.Native.Sdl3.SDLAsyncIOOutcomePtr) fed7e31640d40aed4b2dc2a8f49fbb94eb1c31c92fbeffe31d59b17d5d98620b -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Equals(Inno.Native.Sdl3.SDLAsyncIOOutcomePtr) 321584854b0c30f42647e8ee626f8cc4c7e84e6981bbacfa3251b6c279537c58 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Equals(System.Object) 09b14c9168b45ec54eb4623510fe55fdb62a87e4d249d4d30c675e49e4655091 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.GetHashCode 7370ac3d33b1ff1adb91ce95891486ae02b7ab15a6aa6c4e0ddc6ddfdd8d7623 -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Asyncio 05c21c13718de86b129bcbc4dca206d7797eced235a99752e6d0d32a94f27bbd -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Type d1417be361f0c363c123b42475c5d4114d75654379bc8ce64a94beea4813def5 -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Result c350deeeadefd07ec901d92c9c743b98f042abf4c047395219243bc2ec1d95e6 -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Buffer e2b88bde8c696d9f37577c42f4459c561bf00ee90dffc4b4286fb4e0e379e0b9 -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Offset 6956539358ae94a9dd95bddea6869c35d39f55cfe441611710506f3f40708a86 -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.BytesRequested 1e5b4f94acaa3b8278a9c53acf3684d0834ea8b4d4019b1c5ec4226f4f561575 -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.BytesTransferred 195807435ed3075be9f537559c1c5dbbd81ee8315f412bd111b3165674d28d5e -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Userdata 8c952a358dddc513dcbd64a32fe44949d5794e6866ffad5604fb072dd1d0ceae -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-61cba2f8ff208c56"></a>[`public Inno.Native.Sdl3.SDLAsyncIOOutcome* Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7929) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f7fd49992545c29"></a>[`public Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.SDLAsyncIOOutcomePtr(Inno.Native.Sdl3.SDLAsyncIOOutcome* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7924) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-caf1dbc362e3aa9b"></a>[`public bool Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Equals(Inno.Native.Sdl3.SDLAsyncIOOutcomePtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7939) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-ee9ad34b70ef9994"></a>[`public override bool Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7940) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-22c34f0923ab2ea9"></a>[`public override int Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7941) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-51fe42ce72e27ab0"></a>[`public static bool Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.operator ==(Inno.Native.Sdl3.SDLAsyncIOOutcomePtr left, Inno.Native.Sdl3.SDLAsyncIOOutcomePtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7937) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6ddb0eebf88990ba"></a>[`public static Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.implicit operator Inno.Native.Sdl3.SDLAsyncIOOutcomePtr(Inno.Native.Sdl3.SDLAsyncIOOutcome* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7935) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-678bfec3308066b8"></a>[`public static Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.implicit operator Inno.Native.Sdl3.SDLAsyncIOOutcome*(Inno.Native.Sdl3.SDLAsyncIOOutcomePtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7936) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e6025786ed1ca329"></a>[`public static bool Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.operator !=(Inno.Native.Sdl3.SDLAsyncIOOutcomePtr left, Inno.Native.Sdl3.SDLAsyncIOOutcomePtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7938) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8f9337be10c507d0"></a>[`public ref Inno.Native.Sdl3.SDLAsyncIO Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Asyncio { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7943) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d9163492b1ebb323"></a>[`public void* Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Buffer { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7946) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a094f2932244ead7"></a>[`public ref ulong Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.BytesRequested { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7948) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-187f346572ededcf"></a>[`public ref ulong Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.BytesTransferred { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7949) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1d0cae7abf47b581"></a>[`public bool Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7930) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c16781588a386217"></a>[`public Inno.Native.Sdl3.SDLAsyncIOOutcome Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7933) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9377beb5d92f5b53"></a>[`public static Inno.Native.Sdl3.SDLAsyncIOOutcomePtr Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7931) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f205b7481847b574"></a>[`public ref ulong Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Offset { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7947) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7a904734d7973f89"></a>[`public ref Inno.Native.Sdl3.SDLAsyncIOResult Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Result { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7945) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bcea10535bf0aafe"></a>[`public ref Inno.Native.Sdl3.SDLAsyncIOTaskType Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Type { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7944) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e05be3d830ef2658"></a>[`public void* Inno.Native.Sdl3.SDLAsyncIOOutcomePtr.Userdata { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7950) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ac7bff9a34161455"></a>[`public struct Inno.Native.Sdl3.SDLAsyncIOOutcomePtr : System.IEquatable<Inno.Native.Sdl3.SDLAsyncIOOutcomePtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7922) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAsyncIOQueue</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAsyncIOQueue a6babdd928db082dde209a54e144f882122e4b4a57e775b3e92fea7ae7a9a15c -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOQueue.#ctor(System.IntPtr) 8e939d9f9a3eaa6c23cfe2a8324213f45ca59eac96ed4ccb82599f64371b0da0 -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOQueue.Handle 0237fc362788eb993c1d159c5bc283f04bfeed405feea6a9133873399d9f7919 -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOQueue.IsNull dd0ae40daaac34ad9bcf0e1da491712abf41f73d870e925a65bbe584a6c79c46 -->
<!-- api: P:Inno.Native.Sdl3.SDLAsyncIOQueue.Null 0d1fa084f83cb229b9cd9272d316b91e57136491c2290b0098bd8a7d0c7cdf08 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOQueue.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLAsyncIOQueue 796925b050ed6099263edaf362c686839f62b31873c32012c90f6fb1310b1bf1 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOQueue.op_Equality(Inno.Native.Sdl3.SDLAsyncIOQueue,Inno.Native.Sdl3.SDLAsyncIOQueue) 4477fe36984feb0fa463d2c364f39c105898a813e60c7e50b5fc0987d6847e67 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOQueue.op_Inequality(Inno.Native.Sdl3.SDLAsyncIOQueue,Inno.Native.Sdl3.SDLAsyncIOQueue) 5689c102f8265b254689a3e9245f7cb32a07b84eb6907923155e6fba191814cf -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOQueue.Equals(Inno.Native.Sdl3.SDLAsyncIOQueue) fe82390fb8531391ca0dbcd82ebd6b23b6ff1417894b77085e9766f22013a238 -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOQueue.Equals(System.Object) bfbde98962fa246ff2b0b6ffee711754fe962948f95a0ea4baad0a32e234866f -->
<!-- api: M:Inno.Native.Sdl3.SDLAsyncIOQueue.GetHashCode 514f7ab7b4efd30a2fb6d94822f9c0c8460bc1598160f2c971cbb79fa9e429d8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-eb638c969c66c095"></a>[`public Inno.Native.Sdl3.SDLAsyncIOQueue.SDLAsyncIOQueue(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2048) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f6487e540e745c7f"></a>[`public bool Inno.Native.Sdl3.SDLAsyncIOQueue.Equals(Inno.Native.Sdl3.SDLAsyncIOQueue other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2060) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-81b5d914c8ba2b9a"></a>[`public override bool Inno.Native.Sdl3.SDLAsyncIOQueue.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2061) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-a53fc710466fef8f"></a>[`public override int Inno.Native.Sdl3.SDLAsyncIOQueue.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2062) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-de40ceac5ba7a5d7"></a>[`public static bool Inno.Native.Sdl3.SDLAsyncIOQueue.operator ==(Inno.Native.Sdl3.SDLAsyncIOQueue left, Inno.Native.Sdl3.SDLAsyncIOQueue right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2058) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8103310f075b71e7"></a>[`public static Inno.Native.Sdl3.SDLAsyncIOQueue.implicit operator Inno.Native.Sdl3.SDLAsyncIOQueue(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2057) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-86e16bdd0e01675a"></a>[`public static bool Inno.Native.Sdl3.SDLAsyncIOQueue.operator !=(Inno.Native.Sdl3.SDLAsyncIOQueue left, Inno.Native.Sdl3.SDLAsyncIOQueue right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2059) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ce8a73439a9e6430"></a>[`public nint Inno.Native.Sdl3.SDLAsyncIOQueue.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2053) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f1eb312d30bf0cec"></a>[`public bool Inno.Native.Sdl3.SDLAsyncIOQueue.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2054) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b1e83b7be9b34c41"></a>[`public static Inno.Native.Sdl3.SDLAsyncIOQueue Inno.Native.Sdl3.SDLAsyncIOQueue.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2055) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1ff6ab8b2b134798"></a>[`public readonly struct Inno.Native.Sdl3.SDLAsyncIOQueue : System.IEquatable<Inno.Native.Sdl3.SDLAsyncIOQueue>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2046) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAsyncIOResult</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAsyncIOResult 7e0aa4d86b200b21825f750a729a42ef28bed81c9ef661ae93b14d78b036e098 -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOResult.AsyncioComplete 5538610403bdfff1cec1eced42fa133fde925e5308da6e72d7817a334f356cd7 -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOResult.AsyncioFailure 0cdd32d46b645ae84ed88c56727d86b0d8a42082a243eb341da1c4c615128e1a -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOResult.AsyncioCanceled 25153c61aea3240473d350c7d3b2a029228b6b7ea225fcb28fed970fc48d26a1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c25a236b79ce28b6"></a>[`Inno.Native.Sdl3.SDLAsyncIOResult.AsyncioCanceled = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L104) | 当前常量或枚举值：`2`。 | — |
| <a id="api-b0802d5fa1035c88"></a>[`Inno.Native.Sdl3.SDLAsyncIOResult.AsyncioComplete = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L102) | 当前常量或枚举值：`0`。 | — |
| <a id="api-b6826538d43f4775"></a>[`Inno.Native.Sdl3.SDLAsyncIOResult.AsyncioFailure = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L103) | 当前常量或枚举值：`1`。 | — |
| <a id="api-99a7b2028c9bad21"></a>[`public enum Inno.Native.Sdl3.SDLAsyncIOResult : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L100) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAsyncIOTaskType</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAsyncIOTaskType cda214186512462e0ff543eafff3de3b4ab7072d229aaddbafeb3b7163440e87 -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOTaskType.AsyncioTaskRead 4e7871fa0ce2ab3fca200e562b9d16e1afe113c81373213baff01913b8ff87a8 -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOTaskType.AsyncioTaskWrite 3e4a00f6a08dd53c454c67ba46c8ea714f2e041766424ac06687fc9a2958ad92 -->
<!-- api: F:Inno.Native.Sdl3.SDLAsyncIOTaskType.AsyncioTaskClose 448db2c5fa90828acf973f06ba9a55f7af785aae0b5465198be3c2cf35b01775 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-60e5a71ff6a576b6"></a>[`Inno.Native.Sdl3.SDLAsyncIOTaskType.AsyncioTaskClose = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L94) | 当前常量或枚举值：`2`。 | — |
| <a id="api-73f24306593d2a53"></a>[`Inno.Native.Sdl3.SDLAsyncIOTaskType.AsyncioTaskRead = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L92) | 当前常量或枚举值：`0`。 | — |
| <a id="api-fdfa12f5bfa97929"></a>[`Inno.Native.Sdl3.SDLAsyncIOTaskType.AsyncioTaskWrite = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L93) | 当前常量或枚举值：`1`。 | — |
| <a id="api-0b24a6e6ea633b04"></a>[`public enum Inno.Native.Sdl3.SDLAsyncIOTaskType : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L90) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAtomicInt</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAtomicInt 1aa51ba02878de62e031b881fa10196f8a44ee3b4ae087413471190b24cb22fe -->
<!-- api: F:Inno.Native.Sdl3.SDLAtomicInt.Value e5daa6b2683bb5ac8ae18033d5ac1088fc889a9e1d5ba7fd440ad2d2042f5113 -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicInt.#ctor(System.Int32) 053d93522bb133cbc81091303732fd94e0f673bb86a7022700f3023fb7d23f7f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-832d88bd6ee3dc99"></a>[`public int Inno.Native.Sdl3.SDLAtomicInt.Value`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2072) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae39e536266ee5bb"></a>[`public Inno.Native.Sdl3.SDLAtomicInt.SDLAtomicInt(int value = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2073) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-48683a270017f5cf"></a>[`public struct Inno.Native.Sdl3.SDLAtomicInt`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2070) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
