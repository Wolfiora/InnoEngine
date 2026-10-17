# Inno.Native.Sdl3 ABI · SDL → SDL

[项目与生命周期](../Inno.Native.Sdl3.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.Sdl3.SDL</code> · 361–540</summary>

<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PRIs64 cb8ade8091c93e9af91a3616eeb0093e7270a21a60c257ce8c7f0609931a15ea -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PRIu64 0c24266dee232b76e74d8fa1d1e2801eb08b042242642e0d16103888bf3640d9 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PRIx64 38bc205e6953a9fd129027650b4f5e6b3fa39ba9f5281a54596fcba4a6e27921 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PRIX64 7e2094bf77a6718e7ff3921a6ac1d10bf2f5c976f0c56b24b387cc1e2a2a1c24 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_COPYRIGHT_STRING 62aae1790a1a4252a4c312efe506ed0ba95a024eabd5b1f41d641bd062ba18b4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_CREATOR_STRING 1a7acccc9565926311c8f4dbe05e225b6e4de59266daec87c2a7795f3a4ea5a4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_IDENTIFIER_STRING 3bb4fde462145056b0c0ece7ed053ba0706c0e43727ba3ebe8a7ff44eea54848 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_NAME_STRING 7ef62d209953df6f6a3d736313acefcb2833220497f6a513f008ef66d7efc97d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_TYPE_STRING 394269c649fddf2769fb09a23df8f19821b86700ad01db6f5ca3908d0f09bead -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_URL_STRING 84223982fb5f200b9cc8d96a455c8994d241284fc7ac5046c02f54456c456111 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_VERSION_STRING 03398603ad09541e8be35617ad6611c31b8800c95f0a2a862bc82471f51ebcc4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_AUDIOSTREAM_AUTO_CLEANUP_BOOLEAN c3dd5374ea59325524c26180a35e06cdf1160ffb72613431afd3f13340407cee -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_DISPLAY_HDR_ENABLED_BOOLEAN 8b7549af6976e57d9a589024192aa02bf61a7a2e571bc2707538476e56b894ec -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_DISPLAY_KMSDRM_PANEL_ORIENTATION_NUMBER 6a9aff1ededbac5f654aaa57575da36dfe237d21c84921051b0b16a2930e8149 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_DISPLAY_WAYLAND_WL_OUTPUT_POINTER b6cb5bbfa8969ef6d66b3b293bfebfe7b8a1affa42077d52fbda1c9f54efa78b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_DISPLAY_WINDOWS_HMONITOR_POINTER 3ec853e107e4f0f4d19747367205b1e4f50081d546ecdb95f5e85a600f9d66f3 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_ACCEPT_STRING 103382885e0810814b3dc99a210c3251cb25da7aacf14309230280bf48c76186 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_CANCEL_STRING b59bb7f9455bf6aeae652aa6c0676c89e0e35044deb9855114a0f69f6abb4f90 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_FILTERS_POINTER e849d995097b00f94cc528cca8ce0a6aa48ffcb2683fa89c75be68e083336ed6 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_LOCATION_STRING 86e5b5d822fcf09ae3b1bfbb559c749d2a92b27745a2137e62ec10cc037b0eb6 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_MANY_BOOLEAN 58dff9f4750710f8dddc2968624fa6ae24011030c02cd4e9845dc7899d3560da -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_NFILTERS_NUMBER c9548db9210babffae77a4605c502323cce96eadaca3362ea7587637b59c8c2e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_TITLE_STRING 209d8d787e5cc12a1b9b7fa0a2056d4bddecec0d212352ad111addd529775330 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_WINDOW_POINTER 96feb69b2ed542a028a99bc95790857af4797545020eef2adbebf34267726064 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GAMEPAD_CAP_MONO_LED_BOOLEAN 99f57f0379b9fbf3dcff962f9e765291d8b490c9c12a4f9bb4b69ce4e2a20b89 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GAMEPAD_CAP_PLAYER_LED_BOOLEAN 03546ec5cece8e044a42ce3f945b75b98fc3ead85e4b0886dd204ff26e5239fa -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GAMEPAD_CAP_RGB_LED_BOOLEAN cb655bbbb093a5634d02bdd33e54b196c17fa6ce29e4f9fbed145622bf7645a5 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GAMEPAD_CAP_RUMBLE_BOOLEAN 8b348ade5482706171f0d6f08430c8106c934cab2bf483f0fe98520c81fbc4cd -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GAMEPAD_CAP_TRIGGER_RUMBLE_BOOLEAN 8921f75c5cd1c39a15eac93645759e156313e272ad02648ceeb326fb3c883fc1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GLOBAL_VIDEO_WAYLAND_WL_DISPLAY_POINTER 67aeb6d332e22ca22f71b205990c34c7f15a62e75c94fadaacbc430c3a2f5c9c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_BUFFER_CREATE_NAME_STRING 50d893cb8368620743bdd6fe5a7474690aed3cc1e3cf3af1517d50edc9e9dcac -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_COMPUTEPIPELINE_CREATE_NAME_STRING a89551e818dd4c2c5fd7d34fd5d7a7e977a5469f96aba6c9adb7ac3bad8127fe -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_D3D12_AGILITY_SDK_PATH_STRING 76caa2262187dff4bafde913d00b4cce90f4b531746d4701712cc329cad9e804 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_D3D12_AGILITY_SDK_VERSION_NUMBER 1099082c5e26d84ed42a9db4df2c359fd837fbf3ceffde177b5eb5160b66bad8 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_D3D12_ALLOW_FEWER_RESOURCE_SLOTS_BOOLEAN d54c3f3d58eff6da2ec39f65bb98a92ecc6c449d9a15ebd250ceff96c97fd295 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_D3D12_SEMANTIC_NAME_STRING b10587a863739c11189736d1b56227cb14a7b34612bd7046071731065b69909b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_DEBUGMODE_BOOLEAN 4efc33ed3e7fb9f9da012a144868333bfb76bf41e319d3389d30a2c937ba8f35 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_FEATURE_ANISOTROPY_BOOLEAN 1736305bf18cf270aff983fe8c9f97cfc44896fdc2a6b83fbc95e58f748992e5 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_FEATURE_CLIP_DISTANCE_BOOLEAN 1a07f8ba642a273be5b168a011ba9e2e67c6adf461cfbff125759369322747de -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_FEATURE_DEPTH_CLAMPING_BOOLEAN 5982f25c4e146ab30ea90fc87c2277a6524e502e734f46829c6d254a0621f9e4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_FEATURE_INDIRECT_DRAW_FIRST_INSTANCE_BOOLEAN b9fbbb39b52d8febe19670810974ad9543b170521e5b06d66a7c55f17315b7ed -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_METAL_ALLOW_MACFAMILY1_BOOLEAN 50d81368e81ad1ecd842ea1bfd74fd8fcd839565a8668bee5c4e4856da158174 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_NAME_STRING 7efcc689d3e815624d02e0b7f28425c69ee5e6ba5f90521b73d92359b13f6227 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_PREFERLOWPOWER_BOOLEAN 422e9796a626cb5dc06ad53598cf535ef1bc9ecfc3817bee46c6e816b9eed832 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_SHADERS_DXBC_BOOLEAN 9180e14e72b1392685f81364fe0e57974c406d5d685835a475622ccbf158e066 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_SHADERS_DXIL_BOOLEAN 7c3f850a40756dc16df3aa35c005a1252b3c3ae28049747b9674fd886f2ef003 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_SHADERS_METALLIB_BOOLEAN 2d21de452798706efd0a8af681bc30ec06bce500ef93c2a48b8bddff74110570 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_SHADERS_MSL_BOOLEAN 96713a92d6d124cd5e7f2c6c8d072d1fbbf81db13737dee893f65114c72c2c1a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_SHADERS_PRIVATE_BOOLEAN 6fc1486dbac95e6c0fd82caebc48521539e8678d0e6297603c2be97be5d4d8ac -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_SHADERS_SPIRV_BOOLEAN 554bc161ad7508ab2e527e194acdae6416f7150c7bab487a33542e87cde7b465 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_VERBOSE_BOOLEAN b86c2078fb49b780adbce5bf933bf2bdbd3769f78dbe5af8639d6fe4b447646b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_VULKAN_OPTIONS_POINTER 5e52d3234e9101f167960916808f96a19722c08b41f41f7b732666696361c6c5 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_VULKAN_REQUIRE_HARDWARE_ACCELERATION_BOOLEAN a73449decfd8d9b060ce58c81e04dfe54414e108b841c9e3ecd7c07090864246 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_APPLICATION_NAME_STRING 4fd7e7890c10bc3d87dda6a69e3c657582a05bbba805cc4a2fa7069f51d40858 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_APPLICATION_VERSION_NUMBER 0be4374771f67eafc48479bef041fa180485a1f35dc5f09bf77e8c7bf6791fb3 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_ENABLE_BOOLEAN 8a987dc91c5f3bbd88630ae10a2d5e24be815e52b2a7d7662fddd8152a711fc1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_ENGINE_NAME_STRING 0ccf0d5d956156eaf8124ce6f0311de8a9500972bcae106b5ecff212edc5075a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_ENGINE_VERSION_NUMBER e8c0de381cb57e4a0280dbc6af1c784f9d1913ec497a75007ec2fb87ad375593 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_EXTENSION_COUNT_NUMBER 9efe7fa74a4e5502776d9ee9269d28115b7d42931296b6ef121d55a1a0976a5b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_EXTENSION_NAMES_POINTER 22258d63b60da289d0ac5d3821f07c36202d91e4d4a37a5908fb888ce2bbd813 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_FORM_FACTOR_NUMBER e0cf21813c5b177433c5717899b8a4273c5a7fc731d735606cb20844a9450da3 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_INSTANCE_POINTER 3c7f7954d355e23b3526dfcba2dd15c42a345bb671e4da1c4ed15c50b823b2c4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_LAYER_COUNT_NUMBER 3edd2011592427f903be1750b26bde9032c58136b52e6b5bcdde8e043d8e0a5a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_LAYER_NAMES_POINTER a747215bbf6428b077fc931ec908139af199a3eb8fc134704b2d841e94609f51 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_SYSTEM_ID_POINTER 0aef8ceafa69ce5474931485db79aeec42816c40dc47a98162fed63e7907b21a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_VERSION_NUMBER d73b5249f6f938bad46f94a4a401fb02e989b1a55ba04d6b016a051cfa85be18 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_DRIVER_INFO_STRING 04e781e3cb34158a81d822a28ad3ae0b86407c54ccd940870cec4cf02e597433 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_DRIVER_NAME_STRING 66fa1663a74df1ebafdce8401915cecbd30714a6890e2d0f368fa577fd3f4388 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_DRIVER_VERSION_STRING 25b061e81a8c14df9e19bef99e32d281e2dce765e495ef3808f2919a6174d87f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_NAME_STRING 6506d6200f826d890cb3f9f67d8653257c3d586e9cf3cbda3a44b8f31662dc6c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_GRAPHICSPIPELINE_CREATE_NAME_STRING 2d537cd5d0dbc70c1271dc5f4b57e593718be7d14956d2be18795e8408452709 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_SAMPLER_CREATE_NAME_STRING 1782a8559ad8987f53bd38b928fb904ad3d0bfb742d6682df2b155d37e9456d5 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_SHADER_CREATE_NAME_STRING 762aa390ec99a581d8fce1fad8a1accad2d1f2c2562961cae574b5e4582fc394 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_A_FLOAT ab5f2363276f3277759d9a6e0b6fd53c962347dd6796371bbc07c968ccac333f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_B_FLOAT 0691e86bbba8d5b73bbdb8c3cf7efa75125e30e363f1b3498bfeabc5b06bb425 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_DEPTH_FLOAT a6d352a0625124ce97da70dd36c5c8042ad9c770a9e6574136e3283ff6922674 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_G_FLOAT f8197a8fef14116b4c05242b83a9da96dd529786d6e55c2303f1214834b6286c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_R_FLOAT fbad4238aa6e4d96091e1e8652ace0776cac01da20d151a148c5d5ae427f0c56 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_STENCIL_NUMBER 501f8f175ba215abcdee4a251546e86c27e0952fd4abcd75d9262d0d1c5f3c71 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_NAME_STRING f1ea3b0efdd7fedcdc17b958b1d8cc7ca6e66585e242bfe62b7cbd959c3ed873 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TRANSFERBUFFER_CREATE_NAME_STRING 388151679ca6a0c7eb34cbb25b5d64bee5407dc7dff29af48eb8bcb4f07f8629 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_HIDAPI_LIBUSB_DEVICE_HANDLE_POINTER 2ca9332418fd83a8182e4186fda8d4b6a5b95f75054167a374a7d025de0f32c1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_ANDROID_AASSET_POINTER 656c43eea5b805b6ac22c293f80f8b6b5b8310f03a1708c5c0409ebbe1c9f203 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_DYNAMIC_CHUNKSIZE_NUMBER 2f3945554025744efa3b87c93a30b6d31970e8abd8fcc03d179e8bb425cc656c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_DYNAMIC_MEMORY_POINTER a1ab2246a4808231ac7a813be9404cacf1b9b8b52239878ddb2699a23de9ee10 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_FILE_DESCRIPTOR_NUMBER 04b240a17266ec594a1f617e85f634868b9fddb70d66c972dbe540e294d4540f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_MEMORY_FREE_FUNC_POINTER 6a49ba81f8e2eabe37ccc8e5179a62de12da5c3bc42de75f5017c9a1be52f861 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_MEMORY_POINTER 0d8380f23ec3f6b5d60f99cec72fc8a68d2289f1ca43b2713800c82dc3c3ee33 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_MEMORY_SIZE_NUMBER 42f8bc6359b571160b0fc7cb40f1b759e7fc3130b3ea7bce932e217f295cb071 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_STDIO_FILE_POINTER f0ec9a926f3a3c328cec26b032a87d0f982280f26bb2ae886cbf4b40247b3f7e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_WINDOWS_HANDLE_POINTER 7fac48cc2bdf674394b39869692d03d50f973137dbc0f19fc276f0eb249f848a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_JOYSTICK_CAP_MONO_LED_BOOLEAN f8ac8bae78de954a94969702cfe74075bc6a7cf1d3de925abefee6e504cb97c0 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_JOYSTICK_CAP_PLAYER_LED_BOOLEAN 7604b67d6e6b63f3cc94ac06a73d7fa8a8aa8cda34bd0818bb9648955783a015 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_JOYSTICK_CAP_RGB_LED_BOOLEAN 7199172d313eee0294ac6503c7333934b0049897181272a93e1f159cf6e410df -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_JOYSTICK_CAP_RUMBLE_BOOLEAN 2677d4ca258e02a3a448971a5359a6864169cc84eadc20cf4ffce409c252fe22 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_JOYSTICK_CAP_TRIGGER_RUMBLE_BOOLEAN faf45c3cb37d43eb0c8c37d39c460dbe04648656b77fa550f1c39615feda8411 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_NAME_STRING e2f8d581ddb814f6b1ba1c084087d12b0787ce4413abc1be3e763e1e169f1512 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_BACKGROUND_BOOLEAN dd4a36d2adc34e4c0d75b8fe54451834ac79b4eb367f46652068e6b208ccabd2 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_ARGS_POINTER d631d13f62553dead50a88b7ef654381c0932a8a5a72272f1973372979fb5cb0 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_BACKGROUND_BOOLEAN 68f498b10cf92ffccdec1bafbc7ece8b3f2208a1d0806e86a702a3a37072322e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_CMDLINE_STRING 58246140bf12e40a4ee6d99bfc162a910831987340c37f42794e6afb3b8e05e6 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_ENVIRONMENT_POINTER c1ec32ec503133397292c6a54d6ecef3158f89be82abecd1a71070572461af37 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDERR_NUMBER a7c2daf5cd423cc1724a463cc9dd94e7868f43e2d7fed3fc663c1ae3859a8b00 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDERR_POINTER 5cb0fa27a1528e5a32209fdbb2322ff92e31cbce855a215b6c0795861632fd94 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDERR_TO_STDOUT_BOOLEAN f015d267603cbf73fa329d7c0235fc7484e899678e7d5ee83ad74bba0b1de0a4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDIN_NUMBER 0876dc7e61d2eafb86be81f3748194ccaf96b03eee2a2bf2c9f735fbd3481db0 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDIN_POINTER aab76ab8730115cf3317d858622e431588b767e8a895b831ec8fa352fc50c47a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDOUT_NUMBER dcff2654a8c1ac89b03541bdc5c93c3b32f9e88b8e9605dc51a1a006dee2632b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDOUT_POINTER 4162a57ac3017cca7ee908ff020f1463fc4ba1351f7b3aafc87e9dc7f4746955 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_WORKING_DIRECTORY_STRING c11e63496ee4ec6e464d1adfc6130780b2487d27e3a772ff3cbfa65f7c4fd05f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_PID_NUMBER 11e2247395a00d8c970bdfb47a14dfbce5c14a00ba808af2771933378f78417a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_STDERR_POINTER 76c93cbbeb4ef2c08857ada0d2d1707970291408111a9d0936c48227cb646050 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_STDIN_POINTER 5c57d9dfe01c7a6ce20ffec665bb93e5753f471b12ebf848a17f348b96f25358 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_STDOUT_POINTER 1b73b090ef617f0ea9befcfd08baa9930e55d7cce3bd9d66308165fd5ce86fd0 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_GPU_DEVICE_POINTER 7e2495a50c9bf7369ee83e145c14d6942c660f5bc739545a36c9c90103217e24 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_GPU_SHADERS_DXIL_BOOLEAN a70a420bc1fee962798fa559bde1f565a4fb0d875d12707dd471e191cce4ee8a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_GPU_SHADERS_MSL_BOOLEAN 4f70ed755415d5bf8d347313c50dc0e169e499ab212c6b4b01de484f5934d394 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_GPU_SHADERS_SPIRV_BOOLEAN 92f49b6822a506a0f7c674dc3e12eabe94bf6dc5f528d08de796e97c1e659953 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_NAME_STRING 418e5c74b25b2804d40c15afdb5df3a95b2cb63b889bde7af01ac9d61c8de522 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_OUTPUT_COLORSPACE_NUMBER 95461613765e96d141ee3ca81dbe05ccc227b51a782ff5c1f15f9454cb3f63bc -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_PRESENT_VSYNC_NUMBER a0e53e5680efb84cbc9cfe1e3d883554e7527b5729d35640b423ca49065d7590 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_SURFACE_POINTER 6e45a7233ffb12feab7efaff39501e5ac0ca2e3aa4ac2bd20fa7f9a65784b481 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_VULKAN_DEVICE_POINTER 647f08459427953c0656b1f204f4e6a1791b60219f109075b7a6d8156cff5ec7 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_VULKAN_GRAPHICS_QUEUE_FAMILY_INDEX_NUMBER 8a1a6e17802735c1ca8d8ba603e500593a41ec2b53da1b0cbc138a6faa0d34db -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_VULKAN_INSTANCE_POINTER c1cc0d204fdf57a8ad800d7493d52caccb79e8fc575671a89205d6fac96ea4b8 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_VULKAN_PHYSICAL_DEVICE_POINTER d5e1c21df7a2204ff64fd4a7de7d263d1ccdb6a05e6850d68d184e89ce3b3edb -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_VULKAN_PRESENT_QUEUE_FAMILY_INDEX_NUMBER 4ecca421024a814922ba108b4b08720127e85685b0e0ee2bb9310e991d663791 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_VULKAN_SURFACE_NUMBER 48294efc5446da373f3f67f6058a233b702c9a87aafac04d326fd87073b1bb8c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_WINDOW_POINTER 4aaf2a2b446887143370feff8ab77c67962e61217ec63421795fd5a67c391088 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_D3D11_DEVICE_POINTER 19b796ef13e55939b49a9b8e5f64961cfdb18461c63554d39f267d91a522dde3 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_D3D11_SWAPCHAIN_POINTER f530a8a759105c62dd6a685d01c10a91fb638e4d903ce739320079c4fc654a96 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_D3D12_COMMAND_QUEUE_POINTER 05118b2fb2b8342bc9760daccedfd92582e3d046f07ff92f2b59d62ad4902d66 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_D3D12_DEVICE_POINTER dd53e37ce9eb179e4518f414e99d407dc64dc19a3844cb542dcefcc33f8ce7d2 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_D3D12_SWAPCHAIN_POINTER 5e5d1975dd3f26330c238e52195a971eba275e3ee09e6831383c738a0159bab5 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_D3D9_DEVICE_POINTER 0b2e581c3779e5c2875140a0400b52cb7a02731aa9475d194332cfc3d2921f6c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_GPU_DEVICE_POINTER c9dd9eda480c2803a9bbfb25b29cc40c09c5135b03752efc79539cc1d8ed0acd -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_HDR_ENABLED_BOOLEAN 56bf99850b862fdf0ec86663c8d0740f75a64a33760c2ade0bcc726bf8b6a163 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_HDR_HEADROOM_FLOAT 7f7b78b4b4cc06e6b6ff7298bef62295f5d7e11fbe0846ab1273711b5a8d119d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_MAX_TEXTURE_SIZE_NUMBER 61dcc2edd0af7b47256379e2ff602b5018b339543dff132242bb9604ffbb39a4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_NAME_STRING 45b579fde71c04357ca963ac10db5f94ff18680e0532d2916d8e4642df6e24be -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_OUTPUT_COLORSPACE_NUMBER 9415c7ad30e457e8c4f2de6ffdcee2738eba9a3f610c90fc62a1f76d61e8d70b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_SDR_WHITE_POINT_FLOAT 72b47b58d34cd54fce2699c75ce895f2c677dc12c208fb98f7dfd2dd5d7726ed -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_SURFACE_POINTER 62aa0a73169536f64d2fbb47393ec7bab9a39f19876aa2f01365aaa2204c3394 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_TEXTURE_FORMATS_POINTER 621bacb3df304b6bca07d3a90e19fc73639caf558a07c7949086c52041997044 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_TEXTURE_WRAPPING_BOOLEAN 3671f6049b20bfbd627c285b2d5c29b57d11980aa9a34245bec963d8809fc925 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VSYNC_NUMBER d0b371ae4b40b328f78337c99c714033f0ab51cda9fb68fd19a78956db32572a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_DEVICE_POINTER 84bf135b3116c18690c65e17daca4703e6e05c12529dea7d79201569b73b2de5 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_GRAPHICS_QUEUE_FAMILY_INDEX_NUMBER daf5dbaeb0735e30bde241129f6e299d1cff327b7b7fcb48aa7b0f3c79e618b8 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_INSTANCE_POINTER 61a5ef13d1ef7171564ffc4ef60e127e676a9b373dfc3f10279894969cff90f0 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_PHYSICAL_DEVICE_POINTER 3c7afa8e865fd9db9fa31dde6f97c73f8ee9faf4a5cfa63201e1b34c9bd5268f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_PRESENT_QUEUE_FAMILY_INDEX_NUMBER 4bcc83e3043e1b5298bee72c151b340d512a64107f2e44607109fd13609ae2dc -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_SURFACE_NUMBER 2825a070f222ea5bf4449ebc165c908290e55a13219341d7858720c092ecd84d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_SWAPCHAIN_IMAGE_COUNT_NUMBER 26a0a03076505cff9d055863d363da99ba6cec146c25d20df825b68288fc9995 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_WINDOW_POINTER edd270cf86c0d682ba70933f772449f99f75f36379a8a3ffccfa2baa11542bd9 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_SURFACE_HDR_HEADROOM_FLOAT bbccc04cab121fc507ef7647db37fc10b32189abb63238314350ea31a075b209 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_SURFACE_HOTSPOT_X_NUMBER 9c6d804b3ea586bae284950bb42b9480fcdccd1e3f1694dce94be7cfe4b6babc -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_SURFACE_HOTSPOT_Y_NUMBER 12716b8135c18445f020ac43c11c6f7b7964530b671ebdd708b31a05086eb3b5 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_SURFACE_ROTATION_FLOAT 0a42a55117d0c823f44577d936d716e516aa03cf2808fafe364465affa0af8f7 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_SURFACE_SDR_WHITE_POINT_FLOAT 8a35be9a5a40eef1ca0dec42b6b8be2ccee31542e5e85dedad68cc1919b1a514 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_SURFACE_TONEMAP_OPERATOR_STRING 12d3224ca5bee94f1fd4e89eeb09f5b15de526a8a91715145218c58d26c28239 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTINPUT_ANDROID_INPUTTYPE_NUMBER 1930efc089cf4c0c69928c06cde08909318d82b6f06a2f074e6ae35f87ee02b0 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTINPUT_AUTOCORRECT_BOOLEAN d1f3dc1bcc66340acdee8b62868fd00378af0f91fa362a917856490d3e9ecafa -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTINPUT_CAPITALIZATION_NUMBER 786dd09d3ab4274abaa3b67b6e4957269ad99d4dc483ce132538708ffb51f7d1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTINPUT_MULTILINE_BOOLEAN dc9e485752de5fb9db7f211fcec5691716f9ea5a5b4827107d8767a0ecc17ce8 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTINPUT_TYPE_NUMBER 756820597bd44157b7fa8a11333b59cbd5b9dbf538c42275a1eb787b66ca8e4c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_ACCESS_NUMBER cf6e5851fd847ba1cde1b6277a46eaaaf08859aced91103e0541534be6ddf714 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_COLORSPACE_NUMBER a876b11b9d262ad00de023707b7c5cb5ddc8f768e35c9e16c3234c74069c693f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_ACCESS_NUMBER b9eb575339e89c1ce6dcf5c9ab284edee29ce1d5bca9a6d8dbac7f4c76797163 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_COLORSPACE_NUMBER 123d4cdea4c1f236f3fd41c416403e6b081fd0321bab547f110c61241474fa4c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_POINTER d4f99dd6937dea39c9140616fa8286fdb1b02b74a011eec452436dc83ec79247 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_U_POINTER 79cf33f829338385fbd6eb139b10a916cd3be1a00f2a975649b2292b417b82b2 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_V_POINTER 88de0dc0f509fcd3c0ca3b1c97a03d39c9fed00ca83bcbe2e8612031ce05d671 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_POINTER a5fc9e839d44af4eb82fb83ad084e04aa4c503543f93e4b53d2a58069a44da55 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_U_POINTER d8a97b2a5c95d8884e4249846c64fc5eed86f3c824ec7401c1db925951faf50c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_V_POINTER 76cc1896dd4b6e544857d26eb3d075396a2bec8a1b0bd3c23ce92a8b3685025d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_FORMAT_NUMBER ff89db785355c5e77eb02086c25d7d2db135c78949f8b2d6420295ca8def9eb0 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_POINTER f5532dde8bc9c1cd2b87278b16e0e82d3afc03eeef3aedf4a30cde8645831039 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_U_POINTER 7d08519da57c2c8debb4bcb33b01a2423d40b21d60987b642417d649895698da -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_UV_POINTER 399b0a2b12a7a8256449baa7530b777f626d2846675d512189a6dcf94f03a31e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_V_POINTER 4df9d607047c427b5732feef87e2bbf760a538aefc2dbf8935caf8876772af10 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5ab454a350923490"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PRIs64 = "I64d"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6947) | 当前常量或枚举值：`"I64d"`。 | — |
| <a id="api-883fea2b441904cd"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PRIu64 = "I64u"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6948) | 当前常量或枚举值：`"I64u"`。 | — |
| <a id="api-2eade04f320f4356"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PRIx64 = "I64x"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6949) | 当前常量或枚举值：`"I64x"`。 | — |
| <a id="api-f01409e3320da44b"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PRIX64 = "I64X"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6950) | 当前常量或枚举值：`"I64X"`。 | — |
| <a id="api-0cd309590bb15e68"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_COPYRIGHT_STRING = "SDL.app.metadata.copyright"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7739) | 当前常量或枚举值：`"SDL.app.metadata.copyright"`。 | — |
| <a id="api-aa83c908d4a9e978"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_CREATOR_STRING = "SDL.app.metadata.creator"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7738) | 当前常量或枚举值：`"SDL.app.metadata.creator"`。 | — |
| <a id="api-c6c706cdc00e3597"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_IDENTIFIER_STRING = "SDL.app.metadata.identifier"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7737) | 当前常量或枚举值：`"SDL.app.metadata.identifier"`。 | — |
| <a id="api-e112606ce234f5ae"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_NAME_STRING = "SDL.app.metadata.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7735) | 当前常量或枚举值：`"SDL.app.metadata.name"`。 | — |
| <a id="api-d88f1594333ab9ca"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_TYPE_STRING = "SDL.app.metadata.type"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7741) | 当前常量或枚举值：`"SDL.app.metadata.type"`。 | — |
| <a id="api-c3df13cc82cace7d"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_URL_STRING = "SDL.app.metadata.url"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7740) | 当前常量或枚举值：`"SDL.app.metadata.url"`。 | — |
| <a id="api-dc87b451e5fb8595"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_APP_METADATA_VERSION_STRING = "SDL.app.metadata.version"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7736) | 当前常量或枚举值：`"SDL.app.metadata.version"`。 | — |
| <a id="api-f518b39ae6bc30fd"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_AUDIOSTREAM_AUTO_CLEANUP_BOOLEAN = "SDL.audiostream.auto_cleanup"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6975) | 当前常量或枚举值：`"SDL.audiostream.auto_cleanup"`。 | — |
| <a id="api-aa8a5e049e282ba0"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_DISPLAY_HDR_ENABLED_BOOLEAN = "SDL.display.HDR_enabled"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7013) | 当前常量或枚举值：`"SDL.display.HDR_enabled"`。 | — |
| <a id="api-948e3a6f3897a179"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_DISPLAY_KMSDRM_PANEL_ORIENTATION_NUMBER = "SDL.display.KMSDRM.panel_orientation"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7014) | 当前常量或枚举值：`"SDL.display.KMSDRM.panel_orientation"`。 | — |
| <a id="api-75263a365a262709"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_DISPLAY_WAYLAND_WL_OUTPUT_POINTER = "SDL.display.wayland.wl_output"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7015) | 当前常量或枚举值：`"SDL.display.wayland.wl_output"`。 | — |
| <a id="api-63aa9c9f74e5d87a"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_DISPLAY_WINDOWS_HMONITOR_POINTER = "SDL.display.windows.hmonitor"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7016) | 当前常量或枚举值：`"SDL.display.windows.hmonitor"`。 | — |
| <a id="api-7eadb7ba45b3fa94"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_ACCEPT_STRING = "SDL.filedialog.accept"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7102) | 当前常量或枚举值：`"SDL.filedialog.accept"`。 | — |
| <a id="api-c89bebe460b1b952"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_CANCEL_STRING = "SDL.filedialog.cancel"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7103) | 当前常量或枚举值：`"SDL.filedialog.cancel"`。 | — |
| <a id="api-5ad551539d878e33"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_FILTERS_POINTER = "SDL.filedialog.filters"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7096) | 当前常量或枚举值：`"SDL.filedialog.filters"`。 | — |
| <a id="api-26245f041a361f5d"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_LOCATION_STRING = "SDL.filedialog.location"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7099) | 当前常量或枚举值：`"SDL.filedialog.location"`。 | — |
| <a id="api-5c620c84300617c0"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_MANY_BOOLEAN = "SDL.filedialog.many"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7100) | 当前常量或枚举值：`"SDL.filedialog.many"`。 | — |
| <a id="api-aeb3aabbd2337c6c"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_NFILTERS_NUMBER = "SDL.filedialog.nfilters"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7097) | 当前常量或枚举值：`"SDL.filedialog.nfilters"`。 | — |
| <a id="api-c3fa57e841c11fab"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_TITLE_STRING = "SDL.filedialog.title"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7101) | 当前常量或枚举值：`"SDL.filedialog.title"`。 | — |
| <a id="api-064cfadb0195ea3a"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_FILE_DIALOG_WINDOW_POINTER = "SDL.filedialog.window"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7098) | 当前常量或枚举值：`"SDL.filedialog.window"`。 | — |
| <a id="api-52485d1cb1b52de5"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GAMEPAD_CAP_MONO_LED_BOOLEAN = "SDL.joystick.cap.mono_led"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7120) | 当前常量或枚举值：`"SDL.joystick.cap.mono_led"`。 | — |
| <a id="api-9e0f600a264186de"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GAMEPAD_CAP_PLAYER_LED_BOOLEAN = "SDL.joystick.cap.player_led"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7122) | 当前常量或枚举值：`"SDL.joystick.cap.player_led"`。 | — |
| <a id="api-5bfc7d13ef878fee"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GAMEPAD_CAP_RGB_LED_BOOLEAN = "SDL.joystick.cap.rgb_led"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7121) | 当前常量或枚举值：`"SDL.joystick.cap.rgb_led"`。 | — |
| <a id="api-f2aceb650ee7819b"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GAMEPAD_CAP_RUMBLE_BOOLEAN = "SDL.joystick.cap.rumble"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7123) | 当前常量或枚举值：`"SDL.joystick.cap.rumble"`。 | — |
| <a id="api-ecdd2ce569337b61"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GAMEPAD_CAP_TRIGGER_RUMBLE_BOOLEAN = "SDL.joystick.cap.trigger_rumble"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7124) | 当前常量或枚举值：`"SDL.joystick.cap.trigger_rumble"`。 | — |
| <a id="api-c0b25a3ecc28a751"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GLOBAL_VIDEO_WAYLAND_WL_DISPLAY_POINTER = "SDL.video.wayland.wl_display"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6999) | 当前常量或枚举值：`"SDL.video.wayland.wl_display"`。 | — |
| <a id="api-73f171992bd33f10"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_BUFFER_CREATE_NAME_STRING = "SDL.gpu.buffer.create.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7455) | 当前常量或枚举值：`"SDL.gpu.buffer.create.name"`。 | — |
| <a id="api-ed2fa0ad0148d8a3"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_COMPUTEPIPELINE_CREATE_NAME_STRING = "SDL.gpu.computepipeline.create.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7444) | 当前常量或枚举值：`"SDL.gpu.computepipeline.create.name"`。 | — |
| <a id="api-01dbb62e54a2e402"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_D3D12_AGILITY_SDK_PATH_STRING = "SDL.gpu.device.create.d3d12.agility_sdk_path"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7423) | 当前常量或枚举值：`"SDL.gpu.device.create.d3d12.agility_sdk_path"`。 | — |
| <a id="api-5590f4ca26bf0e46"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_D3D12_AGILITY_SDK_VERSION_NUMBER = "SDL.gpu.device.create.d3d12.agility_sdk_version"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7422) | 当前常量或枚举值：`"SDL.gpu.device.create.d3d12.agility_sdk_version"`。 | — |
| <a id="api-e552538793838c3f"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_D3D12_ALLOW_FEWER_RESOURCE_SLOTS_BOOLEAN = "SDL.gpu.device.create.d3d12.allowtier1resourcebinding"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7420) | 当前常量或枚举值：`"SDL.gpu.device.create.d3d12.allowtier1resourcebinding"`。 | — |
| <a id="api-b2fa850dc41cdc9e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_D3D12_SEMANTIC_NAME_STRING = "SDL.gpu.device.create.d3d12.semantic"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7421) | 当前常量或枚举值：`"SDL.gpu.device.create.d3d12.semantic"`。 | — |
| <a id="api-302ec0425d39bf57"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_DEBUGMODE_BOOLEAN = "SDL.gpu.device.create.debugmode"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7406) | 当前常量或枚举值：`"SDL.gpu.device.create.debugmode"`。 | — |
| <a id="api-d02bb6118ad59e1b"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_FEATURE_ANISOTROPY_BOOLEAN = "SDL.gpu.device.create.feature.anisotropy"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7413) | 当前常量或枚举值：`"SDL.gpu.device.create.feature.anisotropy"`。 | — |
| <a id="api-9e3851b925ad9772"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_FEATURE_CLIP_DISTANCE_BOOLEAN = "SDL.gpu.device.create.feature.clip_distance"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7410) | 当前常量或枚举值：`"SDL.gpu.device.create.feature.clip_distance"`。 | — |
| <a id="api-35514db7348076b1"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_FEATURE_DEPTH_CLAMPING_BOOLEAN = "SDL.gpu.device.create.feature.depth_clamping"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7411) | 当前常量或枚举值：`"SDL.gpu.device.create.feature.depth_clamping"`。 | — |
| <a id="api-d421bf5592f5517f"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_FEATURE_INDIRECT_DRAW_FIRST_INSTANCE_BOOLEAN = "SDL.gpu.device.create.feature.indirect_draw_first_instance"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7412) | 当前常量或枚举值：`"SDL.gpu.device.create.feature.indirect_draw_first_instance"`。 | — |
| <a id="api-7e34e1b67a471edb"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_METAL_ALLOW_MACFAMILY1_BOOLEAN = "SDL.gpu.device.create.metal.allowmacfamily1"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7426) | 当前常量或枚举值：`"SDL.gpu.device.create.metal.allowmacfamily1"`。 | — |
| <a id="api-972f672173730a57"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_NAME_STRING = "SDL.gpu.device.create.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7409) | 当前常量或枚举值：`"SDL.gpu.device.create.name"`。 | — |
| <a id="api-e947dfe1af71a4e0"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_PREFERLOWPOWER_BOOLEAN = "SDL.gpu.device.create.preferlowpower"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7407) | 当前常量或枚举值：`"SDL.gpu.device.create.preferlowpower"`。 | — |
| <a id="api-a21273f90896d6f1"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_SHADERS_DXBC_BOOLEAN = "SDL.gpu.device.create.shaders.dxbc"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7416) | 当前常量或枚举值：`"SDL.gpu.device.create.shaders.dxbc"`。 | — |
| <a id="api-10cf4795fce6ac7b"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_SHADERS_DXIL_BOOLEAN = "SDL.gpu.device.create.shaders.dxil"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7417) | 当前常量或枚举值：`"SDL.gpu.device.create.shaders.dxil"`。 | — |
| <a id="api-904ee0c883231bfa"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_SHADERS_METALLIB_BOOLEAN = "SDL.gpu.device.create.shaders.metallib"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7419) | 当前常量或枚举值：`"SDL.gpu.device.create.shaders.metallib"`。 | — |
| <a id="api-dfb9fc9d531ff752"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_SHADERS_MSL_BOOLEAN = "SDL.gpu.device.create.shaders.msl"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7418) | 当前常量或枚举值：`"SDL.gpu.device.create.shaders.msl"`。 | — |
| <a id="api-f2a70c9e7dc7afd2"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_SHADERS_PRIVATE_BOOLEAN = "SDL.gpu.device.create.shaders.private"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7414) | 当前常量或枚举值：`"SDL.gpu.device.create.shaders.private"`。 | — |
| <a id="api-371ed2f3809e4138"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_SHADERS_SPIRV_BOOLEAN = "SDL.gpu.device.create.shaders.spirv"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7415) | 当前常量或枚举值：`"SDL.gpu.device.create.shaders.spirv"`。 | — |
| <a id="api-99324133a6f5dce2"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_VERBOSE_BOOLEAN = "SDL.gpu.device.create.verbose"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7408) | 当前常量或枚举值：`"SDL.gpu.device.create.verbose"`。 | — |
| <a id="api-efecce19dd283dc9"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_VULKAN_OPTIONS_POINTER = "SDL.gpu.device.create.vulkan.options"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7425) | 当前常量或枚举值：`"SDL.gpu.device.create.vulkan.options"`。 | — |
| <a id="api-ca05b39fe36bb06a"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_VULKAN_REQUIRE_HARDWARE_ACCELERATION_BOOLEAN = "SDL.gpu.device.create.vulkan.requirehardwareacceleration"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7424) | 当前常量或枚举值：`"SDL.gpu.device.create.vulkan.requirehardwareacceleration"`。 | — |
| <a id="api-bc497e36a2753c55"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_APPLICATION_NAME_STRING = "SDL.gpu.device.create.xr.application.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7436) | 当前常量或枚举值：`"SDL.gpu.device.create.xr.application.name"`。 | — |
| <a id="api-5b9f3cbeecaf35a2"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_APPLICATION_VERSION_NUMBER = "SDL.gpu.device.create.xr.application.version"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7437) | 当前常量或枚举值：`"SDL.gpu.device.create.xr.application.version"`。 | — |
| <a id="api-73666caa27147992"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_ENABLE_BOOLEAN = "SDL.gpu.device.create.xr.enable"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7427) | 当前常量或枚举值：`"SDL.gpu.device.create.xr.enable"`。 | — |
| <a id="api-48f7dfb9d2f9b185"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_ENGINE_NAME_STRING = "SDL.gpu.device.create.xr.engine.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7438) | 当前常量或枚举值：`"SDL.gpu.device.create.xr.engine.name"`。 | — |
| <a id="api-ec1a6f320a36e700"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_ENGINE_VERSION_NUMBER = "SDL.gpu.device.create.xr.engine.version"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7439) | 当前常量或枚举值：`"SDL.gpu.device.create.xr.engine.version"`。 | — |
| <a id="api-9baf76ac1d9d690d"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_EXTENSION_COUNT_NUMBER = "SDL.gpu.device.create.xr.extensions.count"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7432) | 当前常量或枚举值：`"SDL.gpu.device.create.xr.extensions.count"`。 | — |
| <a id="api-bce1d9a065f09b4e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_EXTENSION_NAMES_POINTER = "SDL.gpu.device.create.xr.extensions.names"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7433) | 当前常量或枚举值：`"SDL.gpu.device.create.xr.extensions.names"`。 | — |
| <a id="api-bb29b3c795b8db96"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_FORM_FACTOR_NUMBER = "SDL.gpu.device.create.xr.form_factor"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7431) | 当前常量或枚举值：`"SDL.gpu.device.create.xr.form_factor"`。 | — |
| <a id="api-acb077ff1e469c0e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_INSTANCE_POINTER = "SDL.gpu.device.create.xr.instance_out"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7428) | 当前常量或枚举值：`"SDL.gpu.device.create.xr.instance_out"`。 | — |
| <a id="api-56fe83350eeef71c"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_LAYER_COUNT_NUMBER = "SDL.gpu.device.create.xr.layers.count"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7434) | 当前常量或枚举值：`"SDL.gpu.device.create.xr.layers.count"`。 | — |
| <a id="api-c451ecd27662c7d6"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_LAYER_NAMES_POINTER = "SDL.gpu.device.create.xr.layers.names"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7435) | 当前常量或枚举值：`"SDL.gpu.device.create.xr.layers.names"`。 | — |
| <a id="api-ff658958dc8ed032"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_SYSTEM_ID_POINTER = "SDL.gpu.device.create.xr.system_id_out"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7429) | 当前常量或枚举值：`"SDL.gpu.device.create.xr.system_id_out"`。 | — |
| <a id="api-9baac2f8de5073d9"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_CREATE_XR_VERSION_NUMBER = "SDL.gpu.device.create.xr.version"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7430) | 当前常量或枚举值：`"SDL.gpu.device.create.xr.version"`。 | — |
| <a id="api-264c30ce5dee1458"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_DRIVER_INFO_STRING = "SDL.gpu.device.driver_info"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7443) | 当前常量或枚举值：`"SDL.gpu.device.driver_info"`。 | — |
| <a id="api-7386b7302b1afe09"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_DRIVER_NAME_STRING = "SDL.gpu.device.driver_name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7441) | 当前常量或枚举值：`"SDL.gpu.device.driver_name"`。 | — |
| <a id="api-e5e232b9b593cb2e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_DRIVER_VERSION_STRING = "SDL.gpu.device.driver_version"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7442) | 当前常量或枚举值：`"SDL.gpu.device.driver_version"`。 | — |
| <a id="api-0f03117f42c3e330"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_DEVICE_NAME_STRING = "SDL.gpu.device.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7440) | 当前常量或枚举值：`"SDL.gpu.device.name"`。 | — |
| <a id="api-970ea3d94743d8bf"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_GRAPHICSPIPELINE_CREATE_NAME_STRING = "SDL.gpu.graphicspipeline.create.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7445) | 当前常量或枚举值：`"SDL.gpu.graphicspipeline.create.name"`。 | — |
| <a id="api-6d8142fd62ea1bd7"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_SAMPLER_CREATE_NAME_STRING = "SDL.gpu.sampler.create.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7446) | 当前常量或枚举值：`"SDL.gpu.sampler.create.name"`。 | — |
| <a id="api-4074b55fca29ac58"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_SHADER_CREATE_NAME_STRING = "SDL.gpu.shader.create.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7447) | 当前常量或枚举值：`"SDL.gpu.shader.create.name"`。 | — |
| <a id="api-778574c9b8263c7f"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_A_FLOAT = "SDL.gpu.texture.create.d3d12.clear.a"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7451) | 当前常量或枚举值：`"SDL.gpu.texture.create.d3d12.clear.a"`。 | — |
| <a id="api-bf6a66e608046cd0"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_B_FLOAT = "SDL.gpu.texture.create.d3d12.clear.b"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7450) | 当前常量或枚举值：`"SDL.gpu.texture.create.d3d12.clear.b"`。 | — |
| <a id="api-42eb0b27305bf6e4"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_DEPTH_FLOAT = "SDL.gpu.texture.create.d3d12.clear.depth"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7452) | 当前常量或枚举值：`"SDL.gpu.texture.create.d3d12.clear.depth"`。 | — |
| <a id="api-832d81915d56f5fa"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_G_FLOAT = "SDL.gpu.texture.create.d3d12.clear.g"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7449) | 当前常量或枚举值：`"SDL.gpu.texture.create.d3d12.clear.g"`。 | — |
| <a id="api-ccf55e46a131e1ec"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_R_FLOAT = "SDL.gpu.texture.create.d3d12.clear.r"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7448) | 当前常量或枚举值：`"SDL.gpu.texture.create.d3d12.clear.r"`。 | — |
| <a id="api-03799577766ccdcb"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_D3D12_CLEAR_STENCIL_NUMBER = "SDL.gpu.texture.create.d3d12.clear.stencil"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7453) | 当前常量或枚举值：`"SDL.gpu.texture.create.d3d12.clear.stencil"`。 | — |
| <a id="api-49284efc23bb153a"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TEXTURE_CREATE_NAME_STRING = "SDL.gpu.texture.create.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7454) | 当前常量或枚举值：`"SDL.gpu.texture.create.name"`。 | — |
| <a id="api-30ba7035db268510"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_GPU_TRANSFERBUFFER_CREATE_NAME_STRING = "SDL.gpu.transferbuffer.create.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7456) | 当前常量或枚举值：`"SDL.gpu.transferbuffer.create.name"`。 | — |
| <a id="api-a4debe9cf1b9831e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_HIDAPI_LIBUSB_DEVICE_HANDLE_POINTER = "SDL.hidapi.libusb.device.handle"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7462) | 当前常量或枚举值：`"SDL.hidapi.libusb.device.handle"`。 | — |
| <a id="api-4958ecd7435cd15a"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_ANDROID_AASSET_POINTER = "SDL.iostream.android.aasset"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6968) | 当前常量或枚举值：`"SDL.iostream.android.aasset"`。 | — |
| <a id="api-5c16418aa3784851"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_DYNAMIC_CHUNKSIZE_NUMBER = "SDL.iostream.dynamic.chunksize"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6973) | 当前常量或枚举值：`"SDL.iostream.dynamic.chunksize"`。 | — |
| <a id="api-e85060b81343059e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_DYNAMIC_MEMORY_POINTER = "SDL.iostream.dynamic.memory"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6972) | 当前常量或枚举值：`"SDL.iostream.dynamic.memory"`。 | — |
| <a id="api-9c8c9944fb3fc05f"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_FILE_DESCRIPTOR_NUMBER = "SDL.iostream.file_descriptor"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6967) | 当前常量或枚举值：`"SDL.iostream.file_descriptor"`。 | — |
| <a id="api-bfd8d9e60ab1b7a0"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_MEMORY_FREE_FUNC_POINTER = "SDL.iostream.memory.free"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6971) | 当前常量或枚举值：`"SDL.iostream.memory.free"`。 | — |
| <a id="api-b77d318b39aaedd3"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_MEMORY_POINTER = "SDL.iostream.memory.base"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6969) | 当前常量或枚举值：`"SDL.iostream.memory.base"`。 | — |
| <a id="api-c1dc5774192af87d"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_MEMORY_SIZE_NUMBER = "SDL.iostream.memory.size"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6970) | 当前常量或枚举值：`"SDL.iostream.memory.size"`。 | — |
| <a id="api-3815c845c8884d43"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_STDIO_FILE_POINTER = "SDL.iostream.stdio.file"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6966) | 当前常量或枚举值：`"SDL.iostream.stdio.file"`。 | — |
| <a id="api-b2b7d2ceae8f3f9d"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_IOSTREAM_WINDOWS_HANDLE_POINTER = "SDL.iostream.windows.handle"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6965) | 当前常量或枚举值：`"SDL.iostream.windows.handle"`。 | — |
| <a id="api-c75a26219e102a1b"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_JOYSTICK_CAP_MONO_LED_BOOLEAN = "SDL.joystick.cap.mono_led"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7110) | 当前常量或枚举值：`"SDL.joystick.cap.mono_led"`。 | — |
| <a id="api-358dba7a221c54a1"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_JOYSTICK_CAP_PLAYER_LED_BOOLEAN = "SDL.joystick.cap.player_led"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7112) | 当前常量或枚举值：`"SDL.joystick.cap.player_led"`。 | — |
| <a id="api-a1adf9f3b66e9cd6"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_JOYSTICK_CAP_RGB_LED_BOOLEAN = "SDL.joystick.cap.rgb_led"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7111) | 当前常量或枚举值：`"SDL.joystick.cap.rgb_led"`。 | — |
| <a id="api-a0c9d2f31f653e93"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_JOYSTICK_CAP_RUMBLE_BOOLEAN = "SDL.joystick.cap.rumble"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7113) | 当前常量或枚举值：`"SDL.joystick.cap.rumble"`。 | — |
| <a id="api-e5ce26c652bc0f37"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_JOYSTICK_CAP_TRIGGER_RUMBLE_BOOLEAN = "SDL.joystick.cap.trigger_rumble"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7114) | 当前常量或枚举值：`"SDL.joystick.cap.trigger_rumble"`。 | — |
| <a id="api-31d614a6ce73c105"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_NAME_STRING = "SDL.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6960) | 当前常量或枚举值：`"SDL.name"`。 | — |
| <a id="api-d802e991c8c6b8c0"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_BACKGROUND_BOOLEAN = "SDL.process.background"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7765) | 当前常量或枚举值：`"SDL.process.background"`。 | — |
| <a id="api-65bd53884e3d6500"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_ARGS_POINTER = "SDL.process.create.args"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7749) | 当前常量或枚举值：`"SDL.process.create.args"`。 | — |
| <a id="api-79970dd05c25fa8b"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_BACKGROUND_BOOLEAN = "SDL.process.create.background"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7759) | 当前常量或枚举值：`"SDL.process.create.background"`。 | — |
| <a id="api-6a3c65364fc6db07"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_CMDLINE_STRING = "SDL.process.create.cmdline"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7760) | 当前常量或枚举值：`"SDL.process.create.cmdline"`。 | — |
| <a id="api-b9138d790e7de9b7"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_ENVIRONMENT_POINTER = "SDL.process.create.environment"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7750) | 当前常量或枚举值：`"SDL.process.create.environment"`。 | — |
| <a id="api-289c2a6490246043"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDERR_NUMBER = "SDL.process.create.stderr_option"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7756) | 当前常量或枚举值：`"SDL.process.create.stderr_option"`。 | — |
| <a id="api-ef41cbced2ac09bc"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDERR_POINTER = "SDL.process.create.stderr_source"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7757) | 当前常量或枚举值：`"SDL.process.create.stderr_source"`。 | — |
| <a id="api-fa71a28c318d5958"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDERR_TO_STDOUT_BOOLEAN = "SDL.process.create.stderr_to_stdout"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7758) | 当前常量或枚举值：`"SDL.process.create.stderr_to_stdout"`。 | — |
| <a id="api-a167006ad955978c"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDIN_NUMBER = "SDL.process.create.stdin_option"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7752) | 当前常量或枚举值：`"SDL.process.create.stdin_option"`。 | — |
| <a id="api-0059acb87e6860b4"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDIN_POINTER = "SDL.process.create.stdin_source"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7753) | 当前常量或枚举值：`"SDL.process.create.stdin_source"`。 | — |
| <a id="api-c9233a511a7ed1ec"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDOUT_NUMBER = "SDL.process.create.stdout_option"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7754) | 当前常量或枚举值：`"SDL.process.create.stdout_option"`。 | — |
| <a id="api-d943187b8f722e16"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_STDOUT_POINTER = "SDL.process.create.stdout_source"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7755) | 当前常量或枚举值：`"SDL.process.create.stdout_source"`。 | — |
| <a id="api-7dca8f0d668920e1"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_CREATE_WORKING_DIRECTORY_STRING = "SDL.process.create.working_directory"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7751) | 当前常量或枚举值：`"SDL.process.create.working_directory"`。 | — |
| <a id="api-96dd512670b4b838"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_PID_NUMBER = "SDL.process.pid"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7761) | 当前常量或枚举值：`"SDL.process.pid"`。 | — |
| <a id="api-3829d13c70703376"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_STDERR_POINTER = "SDL.process.stderr"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7764) | 当前常量或枚举值：`"SDL.process.stderr"`。 | — |
| <a id="api-33cd4fa52005ea34"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_STDIN_POINTER = "SDL.process.stdin"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7762) | 当前常量或枚举值：`"SDL.process.stdin"`。 | — |
| <a id="api-f19ad9749ac113c2"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_PROCESS_STDOUT_POINTER = "SDL.process.stdout"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7763) | 当前常量或枚举值：`"SDL.process.stdout"`。 | — |
| <a id="api-da8e9787c0c9df11"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_GPU_DEVICE_POINTER = "SDL.renderer.create.gpu.device"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7773) | 当前常量或枚举值：`"SDL.renderer.create.gpu.device"`。 | — |
| <a id="api-bcbe21753cbb5ae3"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_GPU_SHADERS_DXIL_BOOLEAN = "SDL.renderer.create.gpu.shaders_dxil"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7775) | 当前常量或枚举值：`"SDL.renderer.create.gpu.shaders_dxil"`。 | — |
| <a id="api-39481b26cf2e9144"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_GPU_SHADERS_MSL_BOOLEAN = "SDL.renderer.create.gpu.shaders_msl"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7776) | 当前常量或枚举值：`"SDL.renderer.create.gpu.shaders_msl"`。 | — |
| <a id="api-c71a66925b3288fa"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_GPU_SHADERS_SPIRV_BOOLEAN = "SDL.renderer.create.gpu.shaders_spirv"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7774) | 当前常量或枚举值：`"SDL.renderer.create.gpu.shaders_spirv"`。 | — |
| <a id="api-6808c969693129c4"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_NAME_STRING = "SDL.renderer.create.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7768) | 当前常量或枚举值：`"SDL.renderer.create.name"`。 | — |
| <a id="api-dc13d0e611decd2d"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_OUTPUT_COLORSPACE_NUMBER = "SDL.renderer.create.output_colorspace"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7771) | 当前常量或枚举值：`"SDL.renderer.create.output_colorspace"`。 | — |
| <a id="api-16fa3c6fc35abda8"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_PRESENT_VSYNC_NUMBER = "SDL.renderer.create.present_vsync"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7772) | 当前常量或枚举值：`"SDL.renderer.create.present_vsync"`。 | — |
| <a id="api-c00ae41fc28960be"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_SURFACE_POINTER = "SDL.renderer.create.surface"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7770) | 当前常量或枚举值：`"SDL.renderer.create.surface"`。 | — |
| <a id="api-ce5b88d42c7c7b37"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_VULKAN_DEVICE_POINTER = "SDL.renderer.create.vulkan.device"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7780) | 当前常量或枚举值：`"SDL.renderer.create.vulkan.device"`。 | — |
| <a id="api-4b428eb7f7884a6e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_VULKAN_GRAPHICS_QUEUE_FAMILY_INDEX_NUMBER = "SDL.renderer.create.vulkan.graphics_queue_family_index"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7781) | 当前常量或枚举值：`"SDL.renderer.create.vulkan.graphics_queue_family_index"`。 | — |
| <a id="api-cb056a105cef1f97"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_VULKAN_INSTANCE_POINTER = "SDL.renderer.create.vulkan.instance"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7777) | 当前常量或枚举值：`"SDL.renderer.create.vulkan.instance"`。 | — |
| <a id="api-f177977bdd1391be"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_VULKAN_PHYSICAL_DEVICE_POINTER = "SDL.renderer.create.vulkan.physical_device"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7779) | 当前常量或枚举值：`"SDL.renderer.create.vulkan.physical_device"`。 | — |
| <a id="api-d45201f86669e7e2"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_VULKAN_PRESENT_QUEUE_FAMILY_INDEX_NUMBER = "SDL.renderer.create.vulkan.present_queue_family_index"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7782) | 当前常量或枚举值：`"SDL.renderer.create.vulkan.present_queue_family_index"`。 | — |
| <a id="api-6aaa89f5080c513f"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_VULKAN_SURFACE_NUMBER = "SDL.renderer.create.vulkan.surface"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7778) | 当前常量或枚举值：`"SDL.renderer.create.vulkan.surface"`。 | — |
| <a id="api-ad65c400e53b9169"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_CREATE_WINDOW_POINTER = "SDL.renderer.create.window"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7769) | 当前常量或枚举值：`"SDL.renderer.create.window"`。 | — |
| <a id="api-8a16577c9a117215"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_D3D11_DEVICE_POINTER = "SDL.renderer.d3d11.device"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7795) | 当前常量或枚举值：`"SDL.renderer.d3d11.device"`。 | — |
| <a id="api-e96ad584d0ef70c6"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_D3D11_SWAPCHAIN_POINTER = "SDL.renderer.d3d11.swap_chain"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7796) | 当前常量或枚举值：`"SDL.renderer.d3d11.swap_chain"`。 | — |
| <a id="api-bfa7feaf55970469"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_D3D12_COMMAND_QUEUE_POINTER = "SDL.renderer.d3d12.command_queue"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7799) | 当前常量或枚举值：`"SDL.renderer.d3d12.command_queue"`。 | — |
| <a id="api-9b3c9315a926daa9"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_D3D12_DEVICE_POINTER = "SDL.renderer.d3d12.device"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7797) | 当前常量或枚举值：`"SDL.renderer.d3d12.device"`。 | — |
| <a id="api-5c03097882eb639e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_D3D12_SWAPCHAIN_POINTER = "SDL.renderer.d3d12.swap_chain"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7798) | 当前常量或枚举值：`"SDL.renderer.d3d12.swap_chain"`。 | — |
| <a id="api-d654c60e1c2faca3"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_D3D9_DEVICE_POINTER = "SDL.renderer.d3d9.device"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7794) | 当前常量或枚举值：`"SDL.renderer.d3d9.device"`。 | — |
| <a id="api-43894df9d6abbb93"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_GPU_DEVICE_POINTER = "SDL.renderer.gpu.device"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7807) | 当前常量或枚举值：`"SDL.renderer.gpu.device"`。 | — |
| <a id="api-eca36fc37db00fe6"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_HDR_ENABLED_BOOLEAN = "SDL.renderer.HDR_enabled"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7791) | 当前常量或枚举值：`"SDL.renderer.HDR_enabled"`。 | — |
| <a id="api-544ddc84558095c4"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_HDR_HEADROOM_FLOAT = "SDL.renderer.HDR_headroom"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7793) | 当前常量或枚举值：`"SDL.renderer.HDR_headroom"`。 | — |
| <a id="api-e6b3afac793d1a2c"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_MAX_TEXTURE_SIZE_NUMBER = "SDL.renderer.max_texture_size"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7787) | 当前常量或枚举值：`"SDL.renderer.max_texture_size"`。 | — |
| <a id="api-2a3ea2075eb11fbf"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_NAME_STRING = "SDL.renderer.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7783) | 当前常量或枚举值：`"SDL.renderer.name"`。 | — |
| <a id="api-fac18c99d1e08009"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_OUTPUT_COLORSPACE_NUMBER = "SDL.renderer.output_colorspace"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7790) | 当前常量或枚举值：`"SDL.renderer.output_colorspace"`。 | — |
| <a id="api-1cb9efb4ac738f3a"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_SDR_WHITE_POINT_FLOAT = "SDL.renderer.SDR_white_point"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7792) | 当前常量或枚举值：`"SDL.renderer.SDR_white_point"`。 | — |
| <a id="api-6a32a9cdab61d9ef"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_SURFACE_POINTER = "SDL.renderer.surface"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7785) | 当前常量或枚举值：`"SDL.renderer.surface"`。 | — |
| <a id="api-0f18bcd877f634ec"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_TEXTURE_FORMATS_POINTER = "SDL.renderer.texture_formats"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7788) | 当前常量或枚举值：`"SDL.renderer.texture_formats"`。 | — |
| <a id="api-5e3d591358f0a3f5"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_TEXTURE_WRAPPING_BOOLEAN = "SDL.renderer.texture_wrapping"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7789) | 当前常量或枚举值：`"SDL.renderer.texture_wrapping"`。 | — |
| <a id="api-2df97eec29a70e3f"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VSYNC_NUMBER = "SDL.renderer.vsync"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7786) | 当前常量或枚举值：`"SDL.renderer.vsync"`。 | — |
| <a id="api-be6851da7d133cf4"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_DEVICE_POINTER = "SDL.renderer.vulkan.device"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7803) | 当前常量或枚举值：`"SDL.renderer.vulkan.device"`。 | — |
| <a id="api-423ecf3942267dc3"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_GRAPHICS_QUEUE_FAMILY_INDEX_NUMBER = "SDL.renderer.vulkan.graphics_queue_family_index"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7804) | 当前常量或枚举值：`"SDL.renderer.vulkan.graphics_queue_family_index"`。 | — |
| <a id="api-59e0ca834bbcf310"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_INSTANCE_POINTER = "SDL.renderer.vulkan.instance"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7800) | 当前常量或枚举值：`"SDL.renderer.vulkan.instance"`。 | — |
| <a id="api-cd6f28343bf421ae"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_PHYSICAL_DEVICE_POINTER = "SDL.renderer.vulkan.physical_device"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7802) | 当前常量或枚举值：`"SDL.renderer.vulkan.physical_device"`。 | — |
| <a id="api-5db2cbf912792657"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_PRESENT_QUEUE_FAMILY_INDEX_NUMBER = "SDL.renderer.vulkan.present_queue_family_index"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7805) | 当前常量或枚举值：`"SDL.renderer.vulkan.present_queue_family_index"`。 | — |
| <a id="api-846ae49b7e837abb"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_SURFACE_NUMBER = "SDL.renderer.vulkan.surface"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7801) | 当前常量或枚举值：`"SDL.renderer.vulkan.surface"`。 | — |
| <a id="api-19655b30e066cd44"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_VULKAN_SWAPCHAIN_IMAGE_COUNT_NUMBER = "SDL.renderer.vulkan.swapchain_image_count"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7806) | 当前常量或枚举值：`"SDL.renderer.vulkan.swapchain_image_count"`。 | — |
| <a id="api-55b422966cbb9bc5"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_RENDERER_WINDOW_POINTER = "SDL.renderer.window"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7784) | 当前常量或枚举值：`"SDL.renderer.window"`。 | — |
| <a id="api-2940bcdd8fd66f2e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_SURFACE_HDR_HEADROOM_FLOAT = "SDL.surface.HDR_headroom"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6993) | 当前常量或枚举值：`"SDL.surface.HDR_headroom"`。 | — |
| <a id="api-a169d40320b591bc"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_SURFACE_HOTSPOT_X_NUMBER = "SDL.surface.hotspot.x"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6995) | 当前常量或枚举值：`"SDL.surface.hotspot.x"`。 | — |
| <a id="api-eb2e6d178abb2198"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_SURFACE_HOTSPOT_Y_NUMBER = "SDL.surface.hotspot.y"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6996) | 当前常量或枚举值：`"SDL.surface.hotspot.y"`。 | — |
| <a id="api-8d621288d75b8676"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_SURFACE_ROTATION_FLOAT = "SDL.surface.rotation"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6997) | 当前常量或枚举值：`"SDL.surface.rotation"`。 | — |
| <a id="api-728e3970850beae3"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_SURFACE_SDR_WHITE_POINT_FLOAT = "SDL.surface.SDR_white_point"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6992) | 当前常量或枚举值：`"SDL.surface.SDR_white_point"`。 | — |
| <a id="api-c46bf91ef638c525"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_SURFACE_TONEMAP_OPERATOR_STRING = "SDL.surface.tonemap"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6994) | 当前常量或枚举值：`"SDL.surface.tonemap"`。 | — |
| <a id="api-10e80b0b7d139ec7"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTINPUT_ANDROID_INPUTTYPE_NUMBER = "SDL.textinput.android.inputtype"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7399) | 当前常量或枚举值：`"SDL.textinput.android.inputtype"`。 | — |
| <a id="api-29c970636d8ec0b4"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTINPUT_AUTOCORRECT_BOOLEAN = "SDL.textinput.autocorrect"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7397) | 当前常量或枚举值：`"SDL.textinput.autocorrect"`。 | — |
| <a id="api-a6e8241d316575fc"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTINPUT_CAPITALIZATION_NUMBER = "SDL.textinput.capitalization"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7396) | 当前常量或枚举值：`"SDL.textinput.capitalization"`。 | — |
| <a id="api-f0c355f349b7ea39"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTINPUT_MULTILINE_BOOLEAN = "SDL.textinput.multiline"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7398) | 当前常量或枚举值：`"SDL.textinput.multiline"`。 | — |
| <a id="api-eaf8a563f130dcda"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTINPUT_TYPE_NUMBER = "SDL.textinput.type"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7395) | 当前常量或枚举值：`"SDL.textinput.type"`。 | — |
| <a id="api-0ac5461c9782bc11"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_ACCESS_NUMBER = "SDL.texture.access"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7839) | 当前常量或枚举值：`"SDL.texture.access"`。 | — |
| <a id="api-7fbecde5bee1fed5"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_COLORSPACE_NUMBER = "SDL.texture.colorspace"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7837) | 当前常量或枚举值：`"SDL.texture.colorspace"`。 | — |
| <a id="api-f2bf384e95b67995"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_ACCESS_NUMBER = "SDL.texture.create.access"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7810) | 当前常量或枚举值：`"SDL.texture.create.access"`。 | — |
| <a id="api-73c407b6fa518248"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_COLORSPACE_NUMBER = "SDL.texture.create.colorspace"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7808) | 当前常量或枚举值：`"SDL.texture.create.colorspace"`。 | — |
| <a id="api-0d41fe09bc74d196"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_POINTER = "SDL.texture.create.d3d11.texture"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7816) | 当前常量或枚举值：`"SDL.texture.create.d3d11.texture"`。 | — |
| <a id="api-42bf23875ee86e74"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_U_POINTER = "SDL.texture.create.d3d11.texture_u"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7817) | 当前常量或枚举值：`"SDL.texture.create.d3d11.texture_u"`。 | — |
| <a id="api-78ebc095c37b02e8"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_V_POINTER = "SDL.texture.create.d3d11.texture_v"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7818) | 当前常量或枚举值：`"SDL.texture.create.d3d11.texture_v"`。 | — |
| <a id="api-cad4a59accc832b3"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_POINTER = "SDL.texture.create.d3d12.texture"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7819) | 当前常量或枚举值：`"SDL.texture.create.d3d12.texture"`。 | — |
| <a id="api-59baa61c298be277"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_U_POINTER = "SDL.texture.create.d3d12.texture_u"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7820) | 当前常量或枚举值：`"SDL.texture.create.d3d12.texture_u"`。 | — |
| <a id="api-d13183571d248e3a"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_V_POINTER = "SDL.texture.create.d3d12.texture_v"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7821) | 当前常量或枚举值：`"SDL.texture.create.d3d12.texture_v"`。 | — |
| <a id="api-e7889ad9290ff84a"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_FORMAT_NUMBER = "SDL.texture.create.format"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7809) | 当前常量或枚举值：`"SDL.texture.create.format"`。 | — |
| <a id="api-dab82b1963aedca8"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_POINTER = "SDL.texture.create.gpu.texture"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7833) | 当前常量或枚举值：`"SDL.texture.create.gpu.texture"`。 | — |
| <a id="api-47f68a458a80610d"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_U_POINTER = "SDL.texture.create.gpu.texture_u"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7835) | 当前常量或枚举值：`"SDL.texture.create.gpu.texture_u"`。 | — |
| <a id="api-1130f1af058dfb92"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_UV_POINTER = "SDL.texture.create.gpu.texture_uv"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7834) | 当前常量或枚举值：`"SDL.texture.create.gpu.texture_uv"`。 | — |
| <a id="api-79bf9ed26ba38320"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_V_POINTER = "SDL.texture.create.gpu.texture_v"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7836) | 当前常量或枚举值：`"SDL.texture.create.gpu.texture_v"`。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDL</code> · 541–720</summary>

<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_HDR_HEADROOM_FLOAT 3c83b3e03cf46051095fc1ad64e7df399a2ac09123fc4e415926e974f7605661 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_HEIGHT_NUMBER ad902a93c846c8361ac31ed618665db0219809b7092d1d54afcc70399c7b7581 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_METAL_PIXELBUFFER_POINTER c5983138f6c989d8802b002eefe08d0c12c1d6ffee714fa3c7419e0b50af790f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_NUMBER 4e954d9655bee51889c0424505b91b2897087aa8c3e6ff835d7fd54891cd65bc -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_U_NUMBER 35885c6b8c8dbc416a2901418aa24908dc4bc419edd42fb4300e2ef92c649037 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_UV_NUMBER eef78d69cb464027b809458e75879e3baea0cf52247f44668e708511aa762129 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_V_NUMBER cf284d75efbada939ae3f0171c4842bb1e01bfd403fbc65a5b4be186af3d8883 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_NUMBER 5ceaee27127d8a43e50c35bd2b2f493a8ec5972b6753a27dfcc82928ed28acdb -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_U_NUMBER c150177d7e949748cbebee5f7786e7eb8659eb11823be9b9afa2d1a92bef81c9 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_UV_NUMBER da99366933b1f1cfb644928e3a8396b7a469bc5756000eb212a883dd2b294edf -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_V_NUMBER dc40f07e7319998cfc5e4dd97d311258235c0f5439a156cedf844afabca36748 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_PALETTE_POINTER b5484829444ec774c25e00f626dad2e368f5eb93774b4a3d6c6ab43e49c1c30d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_SDR_WHITE_POINT_FLOAT 56ece19f121575a49ff78f5e960f96fa840c6731e71cf6c70638c0cc2ccdbe3e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_VULKAN_LAYOUT_NUMBER 576322efcf2e99fd67c69602c32f9ce48bb3f87ab54011963cd1c137c227b8be -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_VULKAN_TEXTURE_NUMBER bf65b4ec68a3216353f7843aad1c87ea814dc4df0ff9efb23d78cffe044ff16a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_WIDTH_NUMBER a47d569d3a29414caf17f063175273751f258bfc115b83347d25f22e828d3fe8 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_D3D11_TEXTURE_POINTER 881596abd0ea2f246677c3f01af374e03ab45044035fed73503346a0a51bd0c6 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_D3D11_TEXTURE_U_POINTER 3c11858465be6ea0b2e178d1b6fadb605f82208a893090580d26621a95776463 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_D3D11_TEXTURE_V_POINTER ea2f4831ef30aad2513a623d2d8bc81fb50668fab057ba6168fd1a84f6c861ac -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_D3D12_TEXTURE_POINTER 4652a8f3ac8b195bdb3665a89ad410024f8a8a3583e839709d78db09ad6c6a25 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_D3D12_TEXTURE_U_POINTER e43ee38b2319a26b4ab9a98c6d8081f4224165d48b3ee43c6f2f33243bb0c8fb -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_D3D12_TEXTURE_V_POINTER d519c524d52be69355676442a96019de2e9f5c670164bd089cca9cc4fcf94859 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_FORMAT_NUMBER 5cca8b5dc4daaddb7b3550a765eda12f82870fd09debdc3be5da8b0163b78acd -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_GPU_TEXTURE_POINTER 6caab59cefadbb3eb6568dafaf97b623a6295843a2deaf1b92086f2d3c964392 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_GPU_TEXTURE_U_POINTER 59d18ac38f3a57f575890788db3d35eb8493a4e52668d70705d1d1ac48783461 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_GPU_TEXTURE_UV_POINTER 134ba05b56bb3e37a19985291592dd7e30ee57a77c85ff80c83e2ceaadf78638 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_GPU_TEXTURE_V_POINTER d205c98e4c5dcb3117f558ff3ff5c7d84b4581bbcd146bf314fad57656463ae4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_HDR_HEADROOM_FLOAT 4697f6e60b9de6df11d1720c40808152eab499e11e24faed59ea1059ae80e717 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_HEIGHT_NUMBER 6908734b25e1bf2ee767d6999d80aefe703700b146206c1e3d65580ee7c5f9db -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEX_H_FLOAT 5f7fb99f9a4f5432c627f2e456b648df2e8972b3229372ac5e3c6d957cbdc569 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEX_W_FLOAT f739d0d078a7ed9c2971f72ec5227061ff7849603583d896238f2accc65cac66 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEXTURE_NUMBER b16dc8cd84becc907415966e44e598e1b61a9342d530d2d564b3f8d65475a87c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEXTURE_TARGET_NUMBER c61a72f0272a5aad5884be3d8780b2a5c3f03bb1288c47599aacedce7cad7c1b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEXTURE_U_NUMBER 218d9c5ac4df8d75d9b5b650a84e7f9f9d3f9366547c2ae8bde0d5328b542960 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEXTURE_UV_NUMBER a6e71fbd21e827fc4994786c3f22992c9ed81085d2f72ab090a1e1ecc48b59a8 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEXTURE_V_NUMBER 5a55b6bd1293e99c9462ae11b2eb4c8f14ec72c673ffb09885d188122ceaf6fe -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_NUMBER 3f114749ffd527529965459a40766fdcfe7695bb5e2b40b6c47992cdd86f7107 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_TARGET_NUMBER d9dad0d868ff5733e081c13d17d8ee30fb86c37b90ccf155ea4376dbf2e8e102 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_U_NUMBER 2502afdf7dc5cdc8d7d5f061f3962638bc283f3a0b077c931886e97d725c6667 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_UV_NUMBER 1be4c60cf208278702a2ea08de512f4e33907ebce7d1a73f86c08f6df059f10b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_V_NUMBER e86b090f129ff9f20e08d1cfe3fba677c8b6d6c7d86692df5f34b5297ab37409 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_SDR_WHITE_POINT_FLOAT dfa31b2a12eef3330c13caeaf6c464e280c0a6c097b57001db332b46fcd018a4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_VULKAN_TEXTURE_NUMBER e216dda70f467a76d6efdfa5db3d1ce201d14870f9ee87de80b0f6442e1cff1e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_WIDTH_NUMBER 8d635b230b3bed9df0e1ccc041fd933ca81a6a0a0409bcd20dcbdb362cece226 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_THREAD_CREATE_ENTRY_FUNCTION_POINTER 1129c81dd319a2b3b6889695e184787070cc06fd33edabe1a3df760191b4c8ed -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_THREAD_CREATE_NAME_STRING 21222addaa561f9ef9edd1b8943c21db5649d1388c08d81648906443bcae12a6 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_THREAD_CREATE_STACKSIZE_NUMBER 41f81c77dea98496faefaa59648daf2a11ea6bba4d72f0293b07344fe3f4a14c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_THREAD_CREATE_USERDATA_POINTER 36b7780237a5ef640d7e8d73102e9c01a57706807913a4de09553f126616f26f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TRAY_CREATE_ICON_POINTER 5075a8468867c0f93e30904a918743409822cd119cd1fd86c61abb49ee24e009 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TRAY_CREATE_LEFTCLICK_CALLBACK_POINTER e750ed52c1c52b1319b15946f142537967e339320049180b2bd4d6385cb16062 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TRAY_CREATE_MIDDLECLICK_CALLBACK_POINTER 46562605a9b41097ada46b47807e07901281eaf1745f213948dee1806fe54ca9 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TRAY_CREATE_RIGHTCLICK_CALLBACK_POINTER d9205292bdb17fe20dae9e8a0ed03e1ec46a61d9931cc2145abde3eee182a92b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TRAY_CREATE_TOOLTIP_STRING 1156c22227bd52b2a6706b3d111ee90384e5694e959ed65e288788b65f68b3ea -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_TRAY_CREATE_USERDATA_POINTER 6956ff053cd9890de97f126a8f7db07ce9043205522566e44297e26182b31151 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_ANDROID_SURFACE_POINTER 4ca88320bbf0fcb3c480bbedc1681895d70a8d5c297cff73dae5b7b9b64a2a96 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_ANDROID_WINDOW_POINTER a3b75d28adad0e38105a92a97d4683df361cf7581c453ca53ae6ac98aa2a5305 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_COCOA_METAL_VIEW_TAG_NUMBER 0e20275d50a5e7c3cd87a6e4f3476a31370bab691fd4771df5a7dbe466ea8e1f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_COCOA_WINDOW_POINTER 05aa9cc0d53cbc0c9bbcf045b721ff0b4b25be64d2107b5ba23ffcaf6e7fbbd5 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_ALWAYS_ON_TOP_BOOLEAN 4d9cd1204334f96ff3effbdf17fa721d672cc03d0bd9d3db3ff2b81ba3f7f5af -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_BORDERLESS_BOOLEAN 48ffd9a5d34f5481176ba2c9b252b507c54b9815ec4ce00b15308551f17ed263 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_COCOA_VIEW_POINTER 346e950809cee025807e401aeecd2f48781201c78777b0238d40db343c88dcba -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_COCOA_WINDOW_POINTER f9e6cf81e68fb1545ed88a1ca72a814f6dd88c0c312d396f6eba15c072670c0c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_CONSTRAIN_POPUP_BOOLEAN 3ec23c7094e8047638a47c7bdbdb7abca6f0122eaae0a3c764882f20d9c73e7e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_EMSCRIPTEN_CANVAS_ID_STRING 4cf368c2d39d4516798f41070527dd16c565ac0b55fdf92b00b61b7fc574dbfe -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_EMSCRIPTEN_KEYBOARD_ELEMENT_STRING 734d175ad9ae4da2331d3263829132c747fcb824c778e11d91d2c6182f922d7a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_EXTERNAL_GRAPHICS_CONTEXT_BOOLEAN 3e91cab34090ebe2814e87c8812b4e90b39c12a58fc7c1122cb81e71c021a0c3 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_FLAGS_NUMBER 0ce9e743d647057efbfe7822a34a0b88bedc85621a500ce398a4462d1c49638b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_FOCUSABLE_BOOLEAN 5241aa8e77f2a9841c3a8351f163371b17c0d984d37ebc6b7bc474fe4d7e6a8d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_FULLSCREEN_BOOLEAN cebe4224c4591977ee233765b1ebf346c4196e7f4edc68af55a3ac84aaebcc1d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_HEIGHT_NUMBER a72da8c0bb6222c3e08d0e4980b0446c9f15aecd7a4178f40d2663b2b0be9333 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_HIDDEN_BOOLEAN d74e65ec25378abf50b8c724b8c77250bcbb484a4d5598760b3d0f879a00f898 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_HIGH_PIXEL_DENSITY_BOOLEAN a876161e9ae8d65228f82cca5b3b924290f1043d973b6044e21470478bd3522f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_MAXIMIZED_BOOLEAN f3b8e51a22b519393858984a75c26d63b918636512de82e9a82e966f79a0e5fa -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_MENU_BOOLEAN e67bd9261b731d040e42acb8631a78653552e115ade3f6420a7d0a0a9f79bf06 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_METAL_BOOLEAN 03a46f15e11b4f8bb5fbc9665720b899537ad1b14aff34be692b0c97c4740cfa -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_MINIMIZED_BOOLEAN 04ac1d53d1455fa70be7c411e1e48578da1e6d01c94ea5d0503464a873a77737 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_MODAL_BOOLEAN 20e9c66696dd498a5a7277f8f43371c707ecf200e8260473cd83bae1e08191fe -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_MOUSE_GRABBED_BOOLEAN e58a0cbc0737912ae1d6313859893b6da39b84e66538f72163c42e38dbb6b08e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_OPENGL_BOOLEAN 7002be3c40c91d4b7bd9c5b3ed2bd81511ed1881c34be86bd7b97f0c52251058 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_PARENT_POINTER c4febee26f82e6d5ae9e60870c0ae5ddc5c16e6ecc34c1d418b1cf92a6176a13 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_RESIZABLE_BOOLEAN 569b35df76ee573b24f779cffe4663d8ba47ce5f042e9a84f91fa73c81a4c3de -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_TITLE_STRING e9c742e83ecd07149094eaa0f0239c281e9800c1e2a7dcdff62ba5f899bc61f1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_TOOLTIP_BOOLEAN e8ce2623d93dc9fc918f5a8d3c2b80cbffde8aeb4cab17a70f7dde840c9139a0 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_TRANSPARENT_BOOLEAN e897edf02c4d59ffb3ef30d195f4c92938c6c2e643a77f9dd0197b88f11f76d7 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_UTILITY_BOOLEAN cdd0166df059ca9f54c68dce764de818f6dcb6c294f921bd5317bb2df707ef6b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_VULKAN_BOOLEAN c7ffdf195e25768b58af00a1a87e90b4a4065e3fbf19aebaf44f69f28bd99b2a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WAYLAND_CREATE_EGL_WINDOW_BOOLEAN ed551b12c68cf9d3f49ce26e11e5a927be6474b1dd244e93910b9ae8a0f12ca6 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WAYLAND_SURFACE_ROLE_CUSTOM_BOOLEAN 48d22836dea23de62ac910490a707ef0d309b78691b15ec302cb0f64c849b5b0 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WAYLAND_WL_SURFACE_POINTER 4f611070f1e7509414918b44f9577c82f0136af992143208ba7e03f5a206e3c4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WIDTH_NUMBER 723e599e19d7fc72e67e2b4691cd755d0eca077a6c0643ed2bb808d9f0f2cc53 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WIN32_HWND_POINTER 318b701ed7ce2cee7819689881d7deebf517bebb21b811dcc2e240d7bb27377b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WIN32_PIXEL_FORMAT_HWND_POINTER c218b2d3b10bd5518837a56cda2f310322deea4099033e1531c935164e80e972 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WINDOWSCENE_POINTER e077cfc9d5409664c65379d8ca01eaa32c13f3b1dddd64400ada05e738fc2205 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_X_NUMBER 209a351e3f82de46f431f3eb9be4e2dd8bad40efc6f6658e2f5262ba320ac441 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_X11_WINDOW_NUMBER 59a5227f7eeb11b42fad392f95a96a8c4e7b302773ae578373759c4f72bf651d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_Y_NUMBER 81d6b9ef2ceedda4f9c5a97b861824e3ff649d3bf6d3af3b2edc49f5e433b930 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_EMSCRIPTEN_CANVAS_ID_STRING 04a993f83fea50d18f671aada97f09909827002a8040c8886391cd1e29fddda3 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_EMSCRIPTEN_KEYBOARD_ELEMENT_STRING ab79f0fb1154a1428f27c1f10799065920a7dce1c620fdc833b7e181b8d13746 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_HDR_ENABLED_BOOLEAN a3fcfb8de15add1e65c4ad2c22634f55e907f4772fbc1395c3f84aefb5d7403d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_HDR_HEADROOM_FLOAT cef58f57f7b95d1062952392a11cc5730380c041dc5bd4b21e352692569bcede -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_KMSDRM_DEVICE_INDEX_NUMBER 7cd970eeae39eaa501df76e3b2ea34e8808db03e2dc0d5c0c8776a417eade45d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_KMSDRM_DRM_FD_NUMBER 5c567808970e78e3514d6014c40643e8ad0d54f364b1a8d44eeeb2a14e90c604 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_KMSDRM_GBM_DEVICE_POINTER 3b1b0abf4d1faa1a69e31e54ffd2d386f8de8805305a2371fe3145cc9d6b7af0 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_OPENVR_OVERLAY_ID_NUMBER 7943d7d2b354a48a1024f93f73cd457feace2799037f3adaec25a60f5321bf25 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_QNX_SURFACE_POINTER d9fe3e3ca9daf1f636cb49e374d32b2240d6e6cb80b5d209eeb82372d3e3395f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_QNX_WINDOW_POINTER 615e8e28e391256e0a40ec9b0fa8b692416a6fa3bd0dbde823869a424fdb1dcb -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_SDR_WHITE_LEVEL_FLOAT 347cda30d9bf291051dee3e8c5712d0a83953011c7bfa5469f67377ad9bfff87 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_SHAPE_POINTER 3440735e2bec5585a4ac566e4e2e6215ceefbff2f18b5213f390e7ab35129eb6 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_UIKIT_METAL_VIEW_TAG_NUMBER 0ecc4a298da7b08e313ad0b5403588cbb21c3f84b81a5b4d2e0b081a0ceb6c6f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_UIKIT_OPENGL_FRAMEBUFFER_NUMBER 4631a08f92c387916ba5caedfc19ec7144fe17898df9aea60887ccb2a045c3e5 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_UIKIT_OPENGL_RENDERBUFFER_NUMBER 10586a082a797343d9404b6b550f1240a76ec7c908b44310260333529e057c1d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_UIKIT_OPENGL_RESOLVE_FRAMEBUFFER_NUMBER f3f51ffcb74fdb9b7edf7e868b3f6cf159c60d6ce577e4077ce5458a3ecf1c16 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_UIKIT_WINDOW_POINTER 59d9699c2b22cabe126c9ea69c68afd3fe7f7724b763fa3c8412f82d8965d12c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_VIVANTE_DISPLAY_POINTER 2cd8f2d1b2aba9dfa3966aceb7f069fa2cf58b45a9bdc25ec2699da44ba6fb5d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_VIVANTE_SURFACE_POINTER 119eb3bad3c56d0ed26734032b70687fc8327f3cfc671f3c3b484aaf78717e82 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_VIVANTE_WINDOW_POINTER a41ed31a11ad3ae1c46ea91484fb3ba5cb9b7c037c6715338720d1cf238dad23 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_DISPLAY_POINTER f08b6d2fa812ee49089cff1ce48c287d43cdf40e97f5c819d01e0678e5044392 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_EGL_WINDOW_POINTER 0dcbeddc76eaee86f67bb5edcfc2fe1b9d26525b4d39a534d334b0e52c7afaba -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_SURFACE_POINTER 26f27952400973b50088fdeab84f6493be4b3c7035a97deeaffdecddeeadd867 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_VIEWPORT_POINTER 5660a769f5a35543903d711aad5b65179a4fe7f7dc9d65b8c7a58945cc7b0694 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_XDG_POPUP_POINTER a41a21cafee1ed34bf37eefecc68a33a3f9165bad1b5aef1f01dcacd578a850c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_XDG_POSITIONER_POINTER 7b72f8bccfe7e213f1a8e30a9cdcfb99afff79f7da2f253c8f8995de0ed886c4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_XDG_SURFACE_POINTER afef60149fe06f552ae1eb9017710b1b062d057bf959e746bcd2b3e50f32aae3 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_XDG_TOPLEVEL_EXPORT_HANDLE_STRING 7f6d81280a8978a2b26b8a2b72dfc8f5228aa712d6a96211ba164d5ae931a7ea -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_XDG_TOPLEVEL_POINTER 1dec019932988ea1a5bc2411065154ffdf0c5ed96ca45a415655bc762ab7f546 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WIN32_HDC_POINTER 65be4e9d1ee4c6da706df2e3190f221533b4b61cc410f4654aca4074a15e7e49 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WIN32_HWND_POINTER ec0feac1440757935ca096529704ec7fd1674e334b8e900522c60e079f76da7d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WIN32_INSTANCE_POINTER b7fb9e9fa4b84c7b54559cfaaad7e3a77f473893927310a59b0feee08aa87ac9 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_X11_DISPLAY_POINTER f1f2ebf4abb5a1ac09bc1772c82a2b86523b243932b96699cc79e3dcddc0d31e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_X11_SCREEN_NUMBER 3bff6243d0fa67def469655a7c31b9126e3c4ff9f058bfb0beb3a1fd1b2760c4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_X11_WINDOW_NUMBER b7b7eb8917db3dee7efbea3774390f2cda0510c039582d4896fa04d21f4e7dfe -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_RENDERER_VSYNC_ADAPTIVE 8d877231ee7bd95c54f905d455da639b72e10aad526d1ab972aec0dcd08cd02a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_RENDERER_VSYNC_DISABLED 89ea1d277be25e136cc74fc260d4e4153aaf9ef4b898a32f116cdebf5be21d2e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_SOFTWARE_RENDERER 73ccf7dfa3212a3a6b26519580e19383f2d26aca97f8b25055b6254bc305d4be -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_STANDARD_GRAVITY e9cb732e9588a99fa1ffc22cfb70424c7e78e683c3ed34ec53fd12ff16ad6016 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_SURFACE_LOCK_NEEDED 1a935dc102b097c93ef5575746be759f7eb5167c1c501c1f431d8d05163f707f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_SURFACE_LOCKED 0bffcf355ae2799f65e7a55829cc860dc4ebbfa042ec14ad74c808c6064f30da -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_SURFACE_PREALLOCATED 2656709d6c07fa3cc3fce32566d910067e42de4f396b08bd221e637332b76954 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_SURFACE_SIMD_ALIGNED 11a3bb41db8742a31cb412a68eb14bd7bd4dfd5fd9a8c0fdcb33b906e669e779 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_TRAYENTRY_BUTTON a54929ca368b55ea4f5eb57616ace83f089dd13f57c79e669aef0312fd6aa888 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_TRAYENTRY_CHECKBOX c4699c39181fba2014a89854e8fdc8194961dcecc964f83fc00ce746459fa20c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_TRAYENTRY_CHECKED 5c9e99e3faee0dff5d3d12a0df66e9801887cf14c03cfe7eb178a2b8a16237e6 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_TRAYENTRY_DISABLED 0a782beff64825546ce82d83592af6b24ca5ac1eddf7b7648970e0f4186eb4bc -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_TRAYENTRY_SUBMENU 84cf3d544f30484dae07cdb8bdda15bc4a010273c264d68ecf8fc3d376101b27 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_US_PER_SECOND 7256dc630e9b10170380e0bea38e0533b2e1f5eddaa925acc9a1441f80045892 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_WINDOW_SURFACE_VSYNC_ADAPTIVE 01bfe85773c8ddc79805ac5e7fe886ff4538fae5f947d4b5de443eecf5838b95 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_WINDOW_SURFACE_VSYNC_DISABLED ed0df383b54816e82ef06f07688802136613f4b391e18379f1fbaa3331556349 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_WINDOWPOS_CENTERED_MASK 864ca0f65873b1ab5784c2f396b60fd10616e647617f97a5cc86d139748ef1e6 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDL_WINDOWPOS_UNDEFINED_MASK be80f41ddcbcf2487113e839e997b4c2402b25bb3c6565fdde1b00db6c181322 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_0 e8890ab5d897cee44cb7962954866a34cb4ee356d6f44254aaf4a95c8f2b4b0d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_1 17b04caaec8eb08a524a9bc6a2c59142bd94b67fbd0f7bd4d8ee91e4207fb871 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_2 c9e78050b681db26d94755523710766a4ff6146e8d032777021478d87629fcd2 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_3 f1b3a2bda3b4d4bb86a3b8bc387f9dc9a15356a75aed1d8fe9cedaafd688cf96 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_4 39f25a676e590724300bef43b0876f058094f0e92357c4d4847d4f860000c96a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_5 a0572aaee3141fea5b800a18b4048ae3fe979aea0ffa5e9ba30340b0518267eb -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_6 39c6733686967cee03e9462e0b4ef4f6702a8e6ccd21c19ed50e97d63a3313b4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_7 2e3d369b3ad23c869a3df13b9e2b03b8d79f87442e3ae743e13736ba33cdc5d9 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_8 48b317651f59afd12de673a864426b970294943ea061de2709754f66c0da1488 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_9 f76081a4a017e4b232a0e81ce6d3948493f75ffe18e54d2fff357e661d8f03b9 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_A 1915b05950bf5a00aafe04bcff8a79474ab50700bd2946d0f3c172fe83345f9c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_BACK 21e933bcf3207ad9f80ff3bc8d64733572e6dee35d534f8db313f6c71810ad2b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_BOOKMARKS cc790d59e25560b631dcac88ce9282460ed812b81a65777bd40260c59f7d01f8 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_CLOSE 29f0f4de90494a1629f65c5cae6a144d5da04da09d7edb8d60bfa8b9134a37de -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_EXIT 31f8a05774d350fcbf180d1b2d83f7bed976c1b7ff2c5c1c8aeda252d02b083d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_FORWARD 855ea3a1f1dcc6aaedf186438990e1203768ef9b1e84f11e41358c7a95c9dd7c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_HOME 93120e48f3610bc573ff3410bc7f5cd925f1723b2e71c2032e1b02b4097f178d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_NEW 199ef84801df2ef17ec3ca3acfaadf1563fd432cd1201b4a2779e83d2f17908e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_OPEN 4f5da4340bf9ef4a4e3e51dd21b4b097befe287892db9f00de34351f88d4f0dc -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_PRINT 5a63cba9c5000f309d0b968dfde68ca039b2c867490ca32997b3a7d4a7d551b1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_PROPERTIES 3cfbaea46bf624db6f0beae2bf5bbcd3f5f28ffc1f1bbb26ff306fef3136014e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_REFRESH cf663689bf22ce07022bbac42e2f50a56d3a83432b90356e6fc86250d855d0c2 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_SAVE 62732a25917b600a56cfa3d9b75dee9d5e83ae4c17b1489ea9c96171d1020aa1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_SEARCH 0830dc0b1329a72ad08704926f78015586bb68947952a1be414afe98a143396f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AC_STOP 4c5540f068a2e60a1eabdef6ebb9d129764551fc345ad5b4a92db0cc6d838338 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AGAIN 3e25e07cbc7eed47d0903f2a27a52fa7c9a19cb7d9bdec475cea752c6c70a808 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_ALTERASE b8beebaf2920f4127f3370dc85181378bfdde562ddc0235efb2453b1fe7d6c92 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AMPERSAND ccbdce573e1cb8731a961788a4cc2b3289449ed6e7c8f97fc2c7b00e288dd78d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_APOSTROPHE 01074e4784934755500ea0838ba6669e4610d950d06b32f0311b51eb8416f30f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_APPLICATION 1322a3d90a7ea5f7cc5c5d29dbb79c08e96ce3b1125e2857597a346b21b76dea -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_ASTERISK a1306f488bd29302e6010ffb151df4818fea588f4aa7778719fe4d5af23051e5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-969e1dc3c1c3200c"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_HDR_HEADROOM_FLOAT = "SDL.texture.create.HDR_headroom"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7815) | 当前常量或枚举值：`"SDL.texture.create.HDR_headroom"`。 | — |
| <a id="api-7c587f67f9bb2516"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_HEIGHT_NUMBER = "SDL.texture.create.height"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7812) | 当前常量或枚举值：`"SDL.texture.create.height"`。 | — |
| <a id="api-ec88a33e64f96469"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_METAL_PIXELBUFFER_POINTER = "SDL.texture.create.metal.pixelbuffer"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7822) | 当前常量或枚举值：`"SDL.texture.create.metal.pixelbuffer"`。 | — |
| <a id="api-e51ccbd123a3228e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_NUMBER = "SDL.texture.create.opengl.texture"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7823) | 当前常量或枚举值：`"SDL.texture.create.opengl.texture"`。 | — |
| <a id="api-9b3d085abbaf11f3"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_U_NUMBER = "SDL.texture.create.opengl.texture_u"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7825) | 当前常量或枚举值：`"SDL.texture.create.opengl.texture_u"`。 | — |
| <a id="api-b975b1a5fa2219a0"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_UV_NUMBER = "SDL.texture.create.opengl.texture_uv"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7824) | 当前常量或枚举值：`"SDL.texture.create.opengl.texture_uv"`。 | — |
| <a id="api-632589c3b9ef7789"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_V_NUMBER = "SDL.texture.create.opengl.texture_v"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7826) | 当前常量或枚举值：`"SDL.texture.create.opengl.texture_v"`。 | — |
| <a id="api-bd97ba058fed3295"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_NUMBER = "SDL.texture.create.opengles2.texture"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7827) | 当前常量或枚举值：`"SDL.texture.create.opengles2.texture"`。 | — |
| <a id="api-7e5645eacc040403"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_U_NUMBER = "SDL.texture.create.opengles2.texture_u"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7829) | 当前常量或枚举值：`"SDL.texture.create.opengles2.texture_u"`。 | — |
| <a id="api-6f2420148229ebcc"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_UV_NUMBER = "SDL.texture.create.opengles2.texture_uv"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7828) | 当前常量或枚举值：`"SDL.texture.create.opengles2.texture_uv"`。 | — |
| <a id="api-f5718c5a0f999d54"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_V_NUMBER = "SDL.texture.create.opengles2.texture_v"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7830) | 当前常量或枚举值：`"SDL.texture.create.opengles2.texture_v"`。 | — |
| <a id="api-e1cd6e65c242de9f"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_PALETTE_POINTER = "SDL.texture.create.palette"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7813) | 当前常量或枚举值：`"SDL.texture.create.palette"`。 | — |
| <a id="api-8eedbd9a4975c805"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_SDR_WHITE_POINT_FLOAT = "SDL.texture.create.SDR_white_point"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7814) | 当前常量或枚举值：`"SDL.texture.create.SDR_white_point"`。 | — |
| <a id="api-87a7b7476ae7f2f5"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_VULKAN_LAYOUT_NUMBER = "SDL.texture.create.vulkan.layout"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7832) | 当前常量或枚举值：`"SDL.texture.create.vulkan.layout"`。 | — |
| <a id="api-aa0eb170f6394b8d"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_VULKAN_TEXTURE_NUMBER = "SDL.texture.create.vulkan.texture"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7831) | 当前常量或枚举值：`"SDL.texture.create.vulkan.texture"`。 | — |
| <a id="api-120f895ff53c4612"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_CREATE_WIDTH_NUMBER = "SDL.texture.create.width"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7811) | 当前常量或枚举值：`"SDL.texture.create.width"`。 | — |
| <a id="api-814c0231201fd1b7"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_D3D11_TEXTURE_POINTER = "SDL.texture.d3d11.texture"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7844) | 当前常量或枚举值：`"SDL.texture.d3d11.texture"`。 | — |
| <a id="api-1a241375c82309ff"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_D3D11_TEXTURE_U_POINTER = "SDL.texture.d3d11.texture_u"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7845) | 当前常量或枚举值：`"SDL.texture.d3d11.texture_u"`。 | — |
| <a id="api-98cf0c7b053ed554"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_D3D11_TEXTURE_V_POINTER = "SDL.texture.d3d11.texture_v"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7846) | 当前常量或枚举值：`"SDL.texture.d3d11.texture_v"`。 | — |
| <a id="api-84b48a76d93d58b8"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_D3D12_TEXTURE_POINTER = "SDL.texture.d3d12.texture"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7847) | 当前常量或枚举值：`"SDL.texture.d3d12.texture"`。 | — |
| <a id="api-691b0871b35e6cf0"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_D3D12_TEXTURE_U_POINTER = "SDL.texture.d3d12.texture_u"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7848) | 当前常量或枚举值：`"SDL.texture.d3d12.texture_u"`。 | — |
| <a id="api-ee42c37b2847f242"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_D3D12_TEXTURE_V_POINTER = "SDL.texture.d3d12.texture_v"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7849) | 当前常量或枚举值：`"SDL.texture.d3d12.texture_v"`。 | — |
| <a id="api-4951da41e4462c7e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_FORMAT_NUMBER = "SDL.texture.format"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7838) | 当前常量或枚举值：`"SDL.texture.format"`。 | — |
| <a id="api-9080109c035f03a2"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_GPU_TEXTURE_POINTER = "SDL.texture.gpu.texture"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7863) | 当前常量或枚举值：`"SDL.texture.gpu.texture"`。 | — |
| <a id="api-7f84c4cb0a0a5685"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_GPU_TEXTURE_U_POINTER = "SDL.texture.gpu.texture_u"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7865) | 当前常量或枚举值：`"SDL.texture.gpu.texture_u"`。 | — |
| <a id="api-4de6ee4ba248e112"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_GPU_TEXTURE_UV_POINTER = "SDL.texture.gpu.texture_uv"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7864) | 当前常量或枚举值：`"SDL.texture.gpu.texture_uv"`。 | — |
| <a id="api-9b8d2855d20ae139"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_GPU_TEXTURE_V_POINTER = "SDL.texture.gpu.texture_v"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7866) | 当前常量或枚举值：`"SDL.texture.gpu.texture_v"`。 | — |
| <a id="api-4c30f9923347d8a5"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_HDR_HEADROOM_FLOAT = "SDL.texture.HDR_headroom"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7843) | 当前常量或枚举值：`"SDL.texture.HDR_headroom"`。 | — |
| <a id="api-5a6f14d2d8031a7c"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_HEIGHT_NUMBER = "SDL.texture.height"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7841) | 当前常量或枚举值：`"SDL.texture.height"`。 | — |
| <a id="api-cc19af3b68ea8695"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEX_H_FLOAT = "SDL.texture.opengl.tex_h"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7856) | 当前常量或枚举值：`"SDL.texture.opengl.tex_h"`。 | — |
| <a id="api-e9555dd422e027b7"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEX_W_FLOAT = "SDL.texture.opengl.tex_w"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7855) | 当前常量或枚举值：`"SDL.texture.opengl.tex_w"`。 | — |
| <a id="api-a7ccfa7a2874b0f0"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEXTURE_NUMBER = "SDL.texture.opengl.texture"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7850) | 当前常量或枚举值：`"SDL.texture.opengl.texture"`。 | — |
| <a id="api-e674b1611da4b337"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEXTURE_TARGET_NUMBER = "SDL.texture.opengl.target"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7854) | 当前常量或枚举值：`"SDL.texture.opengl.target"`。 | — |
| <a id="api-87333a5ed705fe93"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEXTURE_U_NUMBER = "SDL.texture.opengl.texture_u"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7852) | 当前常量或枚举值：`"SDL.texture.opengl.texture_u"`。 | — |
| <a id="api-4fc0d67764f38e64"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEXTURE_UV_NUMBER = "SDL.texture.opengl.texture_uv"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7851) | 当前常量或枚举值：`"SDL.texture.opengl.texture_uv"`。 | — |
| <a id="api-f62ff18575648bb6"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGL_TEXTURE_V_NUMBER = "SDL.texture.opengl.texture_v"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7853) | 当前常量或枚举值：`"SDL.texture.opengl.texture_v"`。 | — |
| <a id="api-a2c23340fff9aedc"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_NUMBER = "SDL.texture.opengles2.texture"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7857) | 当前常量或枚举值：`"SDL.texture.opengles2.texture"`。 | — |
| <a id="api-065ab0e382937ca8"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_TARGET_NUMBER = "SDL.texture.opengles2.target"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7861) | 当前常量或枚举值：`"SDL.texture.opengles2.target"`。 | — |
| <a id="api-4fa106c6d260d1f9"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_U_NUMBER = "SDL.texture.opengles2.texture_u"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7859) | 当前常量或枚举值：`"SDL.texture.opengles2.texture_u"`。 | — |
| <a id="api-907fd12495537ecf"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_UV_NUMBER = "SDL.texture.opengles2.texture_uv"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7858) | 当前常量或枚举值：`"SDL.texture.opengles2.texture_uv"`。 | — |
| <a id="api-3e9e49c9bcc23443"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_V_NUMBER = "SDL.texture.opengles2.texture_v"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7860) | 当前常量或枚举值：`"SDL.texture.opengles2.texture_v"`。 | — |
| <a id="api-3f80e6a0f4d253fe"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_SDR_WHITE_POINT_FLOAT = "SDL.texture.SDR_white_point"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7842) | 当前常量或枚举值：`"SDL.texture.SDR_white_point"`。 | — |
| <a id="api-57b7a0daac84ddde"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_VULKAN_TEXTURE_NUMBER = "SDL.texture.vulkan.texture"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7862) | 当前常量或枚举值：`"SDL.texture.vulkan.texture"`。 | — |
| <a id="api-2656c4d5228335f0"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TEXTURE_WIDTH_NUMBER = "SDL.texture.width"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7840) | 当前常量或枚举值：`"SDL.texture.width"`。 | — |
| <a id="api-37d0b91f1dd7be6f"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_THREAD_CREATE_ENTRY_FUNCTION_POINTER = "SDL.thread.create.entry_function"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6961) | 当前常量或枚举值：`"SDL.thread.create.entry_function"`。 | — |
| <a id="api-4e3e0aef3b104c03"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_THREAD_CREATE_NAME_STRING = "SDL.thread.create.name"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6962) | 当前常量或枚举值：`"SDL.thread.create.name"`。 | — |
| <a id="api-694428591a9e85f1"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_THREAD_CREATE_STACKSIZE_NUMBER = "SDL.thread.create.stacksize"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6964) | 当前常量或枚举值：`"SDL.thread.create.stacksize"`。 | — |
| <a id="api-6f4383fe5b7dcd35"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_THREAD_CREATE_USERDATA_POINTER = "SDL.thread.create.userdata"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6963) | 当前常量或枚举值：`"SDL.thread.create.userdata"`。 | — |
| <a id="api-9e19da68fee37528"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TRAY_CREATE_ICON_POINTER = "SDL.tray.create.icon"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7879) | 当前常量或枚举值：`"SDL.tray.create.icon"`。 | — |
| <a id="api-0a564153e843ec3d"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TRAY_CREATE_LEFTCLICK_CALLBACK_POINTER = "SDL.tray.create.leftclick_callback"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7882) | 当前常量或枚举值：`"SDL.tray.create.leftclick_callback"`。 | — |
| <a id="api-53f518f6b308adc4"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TRAY_CREATE_MIDDLECLICK_CALLBACK_POINTER = "SDL.tray.create.middleclick_callback"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7884) | 当前常量或枚举值：`"SDL.tray.create.middleclick_callback"`。 | — |
| <a id="api-7acc3bcdce21bb48"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TRAY_CREATE_RIGHTCLICK_CALLBACK_POINTER = "SDL.tray.create.rightclick_callback"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7883) | 当前常量或枚举值：`"SDL.tray.create.rightclick_callback"`。 | — |
| <a id="api-312cdc338ad264c1"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TRAY_CREATE_TOOLTIP_STRING = "SDL.tray.create.tooltip"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7880) | 当前常量或枚举值：`"SDL.tray.create.tooltip"`。 | — |
| <a id="api-572949d92a238bf8"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_TRAY_CREATE_USERDATA_POINTER = "SDL.tray.create.userdata"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7881) | 当前常量或枚举值：`"SDL.tray.create.userdata"`。 | — |
| <a id="api-d5bbb11c8cf938b6"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_ANDROID_SURFACE_POINTER = "SDL.window.android.surface"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7060) | 当前常量或枚举值：`"SDL.window.android.surface"`。 | — |
| <a id="api-474754317afae8f3"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_ANDROID_WINDOW_POINTER = "SDL.window.android.window"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7059) | 当前常量或枚举值：`"SDL.window.android.window"`。 | — |
| <a id="api-4a6ceb75ea3540db"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_COCOA_METAL_VIEW_TAG_NUMBER = "SDL.window.cocoa.metal_view_tag"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7070) | 当前常量或枚举值：`"SDL.window.cocoa.metal_view_tag"`。 | — |
| <a id="api-b5f00ec37a6ac40d"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_COCOA_WINDOW_POINTER = "SDL.window.cocoa.window"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7069) | 当前常量或枚举值：`"SDL.window.cocoa.window"`。 | — |
| <a id="api-dfed5cb2f0475c0c"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_ALWAYS_ON_TOP_BOOLEAN = "SDL.window.create.always_on_top"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7017) | 当前常量或枚举值：`"SDL.window.create.always_on_top"`。 | — |
| <a id="api-62a9df9da1006934"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_BORDERLESS_BOOLEAN = "SDL.window.create.borderless"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7018) | 当前常量或枚举值：`"SDL.window.create.borderless"`。 | — |
| <a id="api-9c313f792b32a5db"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_COCOA_VIEW_POINTER = "SDL.window.create.cocoa.view"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7045) | 当前常量或枚举值：`"SDL.window.create.cocoa.view"`。 | — |
| <a id="api-a254f482323d4eaa"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_COCOA_WINDOW_POINTER = "SDL.window.create.cocoa.window"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7044) | 当前常量或枚举值：`"SDL.window.create.cocoa.window"`。 | — |
| <a id="api-e52316dbbd2135dc"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_CONSTRAIN_POPUP_BOOLEAN = "SDL.window.create.constrain_popup"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7019) | 当前常量或枚举值：`"SDL.window.create.constrain_popup"`。 | — |
| <a id="api-02b01669f20996eb"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_EMSCRIPTEN_CANVAS_ID_STRING = "SDL.window.create.emscripten.canvas_id"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7053) | 当前常量或枚举值：`"SDL.window.create.emscripten.canvas_id"`。 | — |
| <a id="api-bdd31ba188afabc2"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_EMSCRIPTEN_KEYBOARD_ELEMENT_STRING = "SDL.window.create.emscripten.keyboard_element"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7054) | 当前常量或枚举值：`"SDL.window.create.emscripten.keyboard_element"`。 | — |
| <a id="api-11130b05302c6b2f"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_EXTERNAL_GRAPHICS_CONTEXT_BOOLEAN = "SDL.window.create.external_graphics_context"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7021) | 当前常量或枚举值：`"SDL.window.create.external_graphics_context"`。 | — |
| <a id="api-158d70f37541ffca"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_FLAGS_NUMBER = "SDL.window.create.flags"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7022) | 当前常量或枚举值：`"SDL.window.create.flags"`。 | — |
| <a id="api-e5650e276c629930"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_FOCUSABLE_BOOLEAN = "SDL.window.create.focusable"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7020) | 当前常量或枚举值：`"SDL.window.create.focusable"`。 | — |
| <a id="api-795abcea8c227261"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_FULLSCREEN_BOOLEAN = "SDL.window.create.fullscreen"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7023) | 当前常量或枚举值：`"SDL.window.create.fullscreen"`。 | — |
| <a id="api-cf219e581a99b138"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_HEIGHT_NUMBER = "SDL.window.create.height"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7024) | 当前常量或枚举值：`"SDL.window.create.height"`。 | — |
| <a id="api-297a23f5bd70caf2"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_HIDDEN_BOOLEAN = "SDL.window.create.hidden"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7025) | 当前常量或枚举值：`"SDL.window.create.hidden"`。 | — |
| <a id="api-271e7b347b0d6b41"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_HIGH_PIXEL_DENSITY_BOOLEAN = "SDL.window.create.high_pixel_density"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7026) | 当前常量或枚举值：`"SDL.window.create.high_pixel_density"`。 | — |
| <a id="api-a2ed412d115bf9e0"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_MAXIMIZED_BOOLEAN = "SDL.window.create.maximized"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7027) | 当前常量或枚举值：`"SDL.window.create.maximized"`。 | — |
| <a id="api-f3ee55ae3b4a9299"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_MENU_BOOLEAN = "SDL.window.create.menu"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7028) | 当前常量或枚举值：`"SDL.window.create.menu"`。 | — |
| <a id="api-7f8a50f991171206"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_METAL_BOOLEAN = "SDL.window.create.metal"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7029) | 当前常量或枚举值：`"SDL.window.create.metal"`。 | — |
| <a id="api-e835075e440b5485"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_MINIMIZED_BOOLEAN = "SDL.window.create.minimized"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7030) | 当前常量或枚举值：`"SDL.window.create.minimized"`。 | — |
| <a id="api-c4787f7bb563f223"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_MODAL_BOOLEAN = "SDL.window.create.modal"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7031) | 当前常量或枚举值：`"SDL.window.create.modal"`。 | — |
| <a id="api-3218ca34b928867c"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_MOUSE_GRABBED_BOOLEAN = "SDL.window.create.mouse_grabbed"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7032) | 当前常量或枚举值：`"SDL.window.create.mouse_grabbed"`。 | — |
| <a id="api-9485a06731219a76"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_OPENGL_BOOLEAN = "SDL.window.create.opengl"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7033) | 当前常量或枚举值：`"SDL.window.create.opengl"`。 | — |
| <a id="api-13ad3739a3dd2690"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_PARENT_POINTER = "SDL.window.create.parent"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7034) | 当前常量或枚举值：`"SDL.window.create.parent"`。 | — |
| <a id="api-e72793969007af12"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_RESIZABLE_BOOLEAN = "SDL.window.create.resizable"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7035) | 当前常量或枚举值：`"SDL.window.create.resizable"`。 | — |
| <a id="api-1de8a1073d35113a"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_TITLE_STRING = "SDL.window.create.title"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7036) | 当前常量或枚举值：`"SDL.window.create.title"`。 | — |
| <a id="api-65fc2461f34e1b5e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_TOOLTIP_BOOLEAN = "SDL.window.create.tooltip"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7038) | 当前常量或枚举值：`"SDL.window.create.tooltip"`。 | — |
| <a id="api-2f63a39818ee7ec7"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_TRANSPARENT_BOOLEAN = "SDL.window.create.transparent"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7037) | 当前常量或枚举值：`"SDL.window.create.transparent"`。 | — |
| <a id="api-95df40553c1d2777"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_UTILITY_BOOLEAN = "SDL.window.create.utility"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7039) | 当前常量或枚举值：`"SDL.window.create.utility"`。 | — |
| <a id="api-d5111c39eb8b39c6"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_VULKAN_BOOLEAN = "SDL.window.create.vulkan"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7040) | 当前常量或枚举值：`"SDL.window.create.vulkan"`。 | — |
| <a id="api-3931aee3f113cacc"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WAYLAND_CREATE_EGL_WINDOW_BOOLEAN = "SDL.window.create.wayland.create_egl_window"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7048) | 当前常量或枚举值：`"SDL.window.create.wayland.create_egl_window"`。 | — |
| <a id="api-759e71e937556d82"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WAYLAND_SURFACE_ROLE_CUSTOM_BOOLEAN = "SDL.window.create.wayland.surface_role_custom"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7047) | 当前常量或枚举值：`"SDL.window.create.wayland.surface_role_custom"`。 | — |
| <a id="api-4e03ef3d0e685690"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WAYLAND_WL_SURFACE_POINTER = "SDL.window.create.wayland.wl_surface"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7049) | 当前常量或枚举值：`"SDL.window.create.wayland.wl_surface"`。 | — |
| <a id="api-358c98c14e3b5ddc"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WIDTH_NUMBER = "SDL.window.create.width"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7041) | 当前常量或枚举值：`"SDL.window.create.width"`。 | — |
| <a id="api-9625c742a767021a"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WIN32_HWND_POINTER = "SDL.window.create.win32.hwnd"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7050) | 当前常量或枚举值：`"SDL.window.create.win32.hwnd"`。 | — |
| <a id="api-f525500fb026d43e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WIN32_PIXEL_FORMAT_HWND_POINTER = "SDL.window.create.win32.pixel_format_hwnd"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7051) | 当前常量或枚举值：`"SDL.window.create.win32.pixel_format_hwnd"`。 | — |
| <a id="api-3f2f1144ef2e448e"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_WINDOWSCENE_POINTER = "SDL.window.create.uikit.windowscene"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7046) | 当前常量或枚举值：`"SDL.window.create.uikit.windowscene"`。 | — |
| <a id="api-b2d55d6c20337041"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_X_NUMBER = "SDL.window.create.x"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7042) | 当前常量或枚举值：`"SDL.window.create.x"`。 | — |
| <a id="api-d35ca6051537306a"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_X11_WINDOW_NUMBER = "SDL.window.create.x11.window"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7052) | 当前常量或枚举值：`"SDL.window.create.x11.window"`。 | — |
| <a id="api-7bad3374da5f97f3"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_CREATE_Y_NUMBER = "SDL.window.create.y"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7043) | 当前常量或枚举值：`"SDL.window.create.y"`。 | — |
| <a id="api-9e54f293765c52d6"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_EMSCRIPTEN_CANVAS_ID_STRING = "SDL.window.emscripten.canvas_id"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7092) | 当前常量或枚举值：`"SDL.window.emscripten.canvas_id"`。 | — |
| <a id="api-ed41efbfbf7898ed"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_EMSCRIPTEN_KEYBOARD_ELEMENT_STRING = "SDL.window.emscripten.keyboard_element"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7093) | 当前常量或枚举值：`"SDL.window.emscripten.keyboard_element"`。 | — |
| <a id="api-4c7f4fdc50243d51"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_HDR_ENABLED_BOOLEAN = "SDL.window.HDR_enabled"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7056) | 当前常量或枚举值：`"SDL.window.HDR_enabled"`。 | — |
| <a id="api-3dbcdb93ee5af5c8"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_HDR_HEADROOM_FLOAT = "SDL.window.HDR_headroom"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7058) | 当前常量或枚举值：`"SDL.window.HDR_headroom"`。 | — |
| <a id="api-822c969ad9f22887"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_KMSDRM_DEVICE_INDEX_NUMBER = "SDL.window.kmsdrm.dev_index"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7066) | 当前常量或枚举值：`"SDL.window.kmsdrm.dev_index"`。 | — |
| <a id="api-a3be7018cb4eca51"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_KMSDRM_DRM_FD_NUMBER = "SDL.window.kmsdrm.drm_fd"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7067) | 当前常量或枚举值：`"SDL.window.kmsdrm.drm_fd"`。 | — |
| <a id="api-089c6679a016fde6"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_KMSDRM_GBM_DEVICE_POINTER = "SDL.window.kmsdrm.gbm_dev"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7068) | 当前常量或枚举值：`"SDL.window.kmsdrm.gbm_dev"`。 | — |
| <a id="api-80c7c802272e931a"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_OPENVR_OVERLAY_ID_NUMBER = "SDL.window.openvr.overlay_id"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7071) | 当前常量或枚举值：`"SDL.window.openvr.overlay_id"`。 | — |
| <a id="api-108a3ae1a0d0a657"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_QNX_SURFACE_POINTER = "SDL.window.qnx.surface"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7073) | 当前常量或枚举值：`"SDL.window.qnx.surface"`。 | — |
| <a id="api-a4dfb7dca9de73ad"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_QNX_WINDOW_POINTER = "SDL.window.qnx.window"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7072) | 当前常量或枚举值：`"SDL.window.qnx.window"`。 | — |
| <a id="api-74224c1b7639caab"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_SDR_WHITE_LEVEL_FLOAT = "SDL.window.SDR_white_level"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7057) | 当前常量或枚举值：`"SDL.window.SDR_white_level"`。 | — |
| <a id="api-5cc25125f5cda815"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_SHAPE_POINTER = "SDL.window.shape"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7055) | 当前常量或枚举值：`"SDL.window.shape"`。 | — |
| <a id="api-2c19044704b9e612"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_UIKIT_METAL_VIEW_TAG_NUMBER = "SDL.window.uikit.metal_view_tag"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7062) | 当前常量或枚举值：`"SDL.window.uikit.metal_view_tag"`。 | — |
| <a id="api-12e2e82f1271be2a"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_UIKIT_OPENGL_FRAMEBUFFER_NUMBER = "SDL.window.uikit.opengl.framebuffer"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7063) | 当前常量或枚举值：`"SDL.window.uikit.opengl.framebuffer"`。 | — |
| <a id="api-9c7f77855a7dc8d0"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_UIKIT_OPENGL_RENDERBUFFER_NUMBER = "SDL.window.uikit.opengl.renderbuffer"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7064) | 当前常量或枚举值：`"SDL.window.uikit.opengl.renderbuffer"`。 | — |
| <a id="api-e6df5c11e52ae96c"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_UIKIT_OPENGL_RESOLVE_FRAMEBUFFER_NUMBER = "SDL.window.uikit.opengl.resolve_framebuffer"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7065) | 当前常量或枚举值：`"SDL.window.uikit.opengl.resolve_framebuffer"`。 | — |
| <a id="api-3448e61f7f3f6bce"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_UIKIT_WINDOW_POINTER = "SDL.window.uikit.window"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7061) | 当前常量或枚举值：`"SDL.window.uikit.window"`。 | — |
| <a id="api-332e7ee760b2b040"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_VIVANTE_DISPLAY_POINTER = "SDL.window.vivante.display"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7074) | 当前常量或枚举值：`"SDL.window.vivante.display"`。 | — |
| <a id="api-fca24084c375b845"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_VIVANTE_SURFACE_POINTER = "SDL.window.vivante.surface"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7076) | 当前常量或枚举值：`"SDL.window.vivante.surface"`。 | — |
| <a id="api-25169eb932933040"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_VIVANTE_WINDOW_POINTER = "SDL.window.vivante.window"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7075) | 当前常量或枚举值：`"SDL.window.vivante.window"`。 | — |
| <a id="api-53ed801d1bf86c4b"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_DISPLAY_POINTER = "SDL.window.wayland.display"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7080) | 当前常量或枚举值：`"SDL.window.wayland.display"`。 | — |
| <a id="api-c3de2106a36e3bc6"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_EGL_WINDOW_POINTER = "SDL.window.wayland.egl_window"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7083) | 当前常量或枚举值：`"SDL.window.wayland.egl_window"`。 | — |
| <a id="api-5f59f81a0c1cd6b2"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_SURFACE_POINTER = "SDL.window.wayland.surface"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7081) | 当前常量或枚举值：`"SDL.window.wayland.surface"`。 | — |
| <a id="api-dc8af65a3565b3a9"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_VIEWPORT_POINTER = "SDL.window.wayland.viewport"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7082) | 当前常量或枚举值：`"SDL.window.wayland.viewport"`。 | — |
| <a id="api-7fbe4e8992ef0d70"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_XDG_POPUP_POINTER = "SDL.window.wayland.xdg_popup"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7087) | 当前常量或枚举值：`"SDL.window.wayland.xdg_popup"`。 | — |
| <a id="api-8a1a9a8dc39f75a7"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_XDG_POSITIONER_POINTER = "SDL.window.wayland.xdg_positioner"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7088) | 当前常量或枚举值：`"SDL.window.wayland.xdg_positioner"`。 | — |
| <a id="api-bc7ce4ce307496ad"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_XDG_SURFACE_POINTER = "SDL.window.wayland.xdg_surface"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7084) | 当前常量或枚举值：`"SDL.window.wayland.xdg_surface"`。 | — |
| <a id="api-08d3f33e94f6931d"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_XDG_TOPLEVEL_EXPORT_HANDLE_STRING = "SDL.window.wayland.xdg_toplevel_export_handle"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7086) | 当前常量或枚举值：`"SDL.window.wayland.xdg_toplevel_export_handle"`。 | — |
| <a id="api-957a12d370704c00"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WAYLAND_XDG_TOPLEVEL_POINTER = "SDL.window.wayland.xdg_toplevel"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7085) | 当前常量或枚举值：`"SDL.window.wayland.xdg_toplevel"`。 | — |
| <a id="api-003f73846411fdf8"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WIN32_HDC_POINTER = "SDL.window.win32.hdc"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7078) | 当前常量或枚举值：`"SDL.window.win32.hdc"`。 | — |
| <a id="api-52eb259187f0df5f"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WIN32_HWND_POINTER = "SDL.window.win32.hwnd"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7077) | 当前常量或枚举值：`"SDL.window.win32.hwnd"`。 | — |
| <a id="api-34734693b37f2392"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_WIN32_INSTANCE_POINTER = "SDL.window.win32.instance"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7079) | 当前常量或枚举值：`"SDL.window.win32.instance"`。 | — |
| <a id="api-a8bec53201e3debe"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_X11_DISPLAY_POINTER = "SDL.window.x11.display"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7089) | 当前常量或枚举值：`"SDL.window.x11.display"`。 | — |
| <a id="api-e93b442b07e7961d"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_X11_SCREEN_NUMBER = "SDL.window.x11.screen"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7090) | 当前常量或枚举值：`"SDL.window.x11.screen"`。 | — |
| <a id="api-af3362f571570704"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_PROP_WINDOW_X11_WINDOW_NUMBER = "SDL.window.x11.window"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7091) | 当前常量或枚举值：`"SDL.window.x11.window"`。 | — |
| <a id="api-bce449b1f8090083"></a>[`public const int Inno.Native.Sdl3.SDL.SDL_RENDERER_VSYNC_ADAPTIVE = -1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7868) | 当前常量或枚举值：`-1`。 | — |
| <a id="api-0a287bc683fcaea7"></a>[`public const int Inno.Native.Sdl3.SDL.SDL_RENDERER_VSYNC_DISABLED = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7867) | 当前常量或枚举值：`0`。 | — |
| <a id="api-0f62ae4abcee5e34"></a>[`public const string Inno.Native.Sdl3.SDL.SDL_SOFTWARE_RENDERER = "software"`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7766) | 当前常量或枚举值：`"software"`。 | — |
| <a id="api-edc7d65aade0afda"></a>[`public const float Inno.Native.Sdl3.SDL.SDL_STANDARD_GRAVITY = 9.80665`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7107) | 当前常量或枚举值：`9.80665`。 | — |
| <a id="api-8c10b58b9b37e780"></a>[`public const uint Inno.Native.Sdl3.SDL.SDL_SURFACE_LOCK_NEEDED = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6989) | 当前常量或枚举值：`2`。 | — |
| <a id="api-4cbfd58546a63725"></a>[`public const uint Inno.Native.Sdl3.SDL.SDL_SURFACE_LOCKED = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6990) | 当前常量或枚举值：`4`。 | — |
| <a id="api-929f9ce00d197a2a"></a>[`public const uint Inno.Native.Sdl3.SDL.SDL_SURFACE_PREALLOCATED = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6988) | 当前常量或枚举值：`1`。 | — |
| <a id="api-9efac82c5451ef68"></a>[`public const uint Inno.Native.Sdl3.SDL.SDL_SURFACE_SIMD_ALIGNED = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6991) | 当前常量或枚举值：`8`。 | — |
| <a id="api-45daee3cf48dc0c5"></a>[`public const uint Inno.Native.Sdl3.SDL.SDL_TRAYENTRY_BUTTON = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7874) | 当前常量或枚举值：`1`。 | — |
| <a id="api-cc78dd3ce2a1a4fd"></a>[`public const uint Inno.Native.Sdl3.SDL.SDL_TRAYENTRY_CHECKBOX = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7875) | 当前常量或枚举值：`2`。 | — |
| <a id="api-a3e3900638fd6ed8"></a>[`public const uint Inno.Native.Sdl3.SDL.SDL_TRAYENTRY_CHECKED = 1073741824`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7878) | 当前常量或枚举值：`1073741824`。 | — |
| <a id="api-281df2b959bfedac"></a>[`public const uint Inno.Native.Sdl3.SDL.SDL_TRAYENTRY_DISABLED = 2147483648`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7877) | 当前常量或枚举值：`2147483648`。 | — |
| <a id="api-2df0fc572cb07f99"></a>[`public const uint Inno.Native.Sdl3.SDL.SDL_TRAYENTRY_SUBMENU = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7876) | 当前常量或枚举值：`4`。 | — |
| <a id="api-7c07c55f860f8cac"></a>[`public const int Inno.Native.Sdl3.SDL.SDL_US_PER_SECOND = 1000000`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7871) | 当前常量或枚举值：`1000000`。 | — |
| <a id="api-8e77a470b6ecd6eb"></a>[`public const int Inno.Native.Sdl3.SDL.SDL_WINDOW_SURFACE_VSYNC_ADAPTIVE = -1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7095) | 当前常量或枚举值：`-1`。 | — |
| <a id="api-8a3ba51ab32e5883"></a>[`public const int Inno.Native.Sdl3.SDL.SDL_WINDOW_SURFACE_VSYNC_DISABLED = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7094) | 当前常量或枚举值：`0`。 | — |
| <a id="api-6e4b202a00bbd667"></a>[`public const uint Inno.Native.Sdl3.SDL.SDL_WINDOWPOS_CENTERED_MASK = 805240832`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7001) | 当前常量或枚举值：`805240832`。 | — |
| <a id="api-0d21795f8a537f78"></a>[`public const uint Inno.Native.Sdl3.SDL.SDL_WINDOWPOS_UNDEFINED_MASK = 536805376`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7000) | 当前常量或枚举值：`536805376`。 | — |
| <a id="api-c9c47ea42f2312a6"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_0 = 48`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7146) | 当前常量或枚举值：`48`。 | — |
| <a id="api-16e004eeccbe0665"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_1 = 49`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7147) | 当前常量或枚举值：`49`。 | — |
| <a id="api-a115dcca76a8d36d"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_2 = 50`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7148) | 当前常量或枚举值：`50`。 | — |
| <a id="api-d4a20487ed6ce9bd"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_3 = 51`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7149) | 当前常量或枚举值：`51`。 | — |
| <a id="api-de7253a9f879ac89"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_4 = 52`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7150) | 当前常量或枚举值：`52`。 | — |
| <a id="api-cf1e09aba30f6551"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_5 = 53`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7151) | 当前常量或枚举值：`53`。 | — |
| <a id="api-276846a23cc87192"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_6 = 54`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7152) | 当前常量或枚举值：`54`。 | — |
| <a id="api-b22887d48f59873c"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_7 = 55`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7153) | 当前常量或枚举值：`55`。 | — |
| <a id="api-f5370079afa20b1f"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_8 = 56`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7154) | 当前常量或枚举值：`56`。 | — |
| <a id="api-3edbe19034873e10"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_9 = 57`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7155) | 当前常量或枚举值：`57`。 | — |
| <a id="api-40b642c1218259e3"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_A = 97`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7169) | 当前常量或枚举值：`97`。 | — |
| <a id="api-83281d933cfbacd4"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_BACK = 1073742106`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7365) | 当前常量或枚举值：`1073742106`。 | — |
| <a id="api-c43326a62f40dcb9"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_BOOKMARKS = 1073742110`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7369) | 当前常量或枚举值：`1073742110`。 | — |
| <a id="api-819b85d2b388e46c"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_CLOSE = 1073742099`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7358) | 当前常量或枚举值：`1073742099`。 | — |
| <a id="api-e2842e4726308235"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_EXIT = 1073742100`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7359) | 当前常量或枚举值：`1073742100`。 | — |
| <a id="api-d18def022d7c6aa3"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_FORWARD = 1073742107`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7366) | 当前常量或枚举值：`1073742107`。 | — |
| <a id="api-0bf879a751cb506a"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_HOME = 1073742105`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7364) | 当前常量或枚举值：`1073742105`。 | — |
| <a id="api-1f4e073ff2e2c2f3"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_NEW = 1073742097`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7356) | 当前常量或枚举值：`1073742097`。 | — |
| <a id="api-a1984d5f844edf07"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_OPEN = 1073742098`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7357) | 当前常量或枚举值：`1073742098`。 | — |
| <a id="api-ef3d4fa45ccf8fa4"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_PRINT = 1073742102`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7361) | 当前常量或枚举值：`1073742102`。 | — |
| <a id="api-d5b7cc4ff4b4adc1"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_PROPERTIES = 1073742103`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7362) | 当前常量或枚举值：`1073742103`。 | — |
| <a id="api-cbbadcb29ff78bb3"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_REFRESH = 1073742109`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7368) | 当前常量或枚举值：`1073742109`。 | — |
| <a id="api-0707d0fd967be0cd"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_SAVE = 1073742101`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7360) | 当前常量或枚举值：`1073742101`。 | — |
| <a id="api-439837c80f8f1a80"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_SEARCH = 1073742104`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7363) | 当前常量或枚举值：`1073742104`。 | — |
| <a id="api-26a6aca7776ca81e"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AC_STOP = 1073742108`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7367) | 当前常量或枚举值：`1073742108`。 | — |
| <a id="api-0b3f0b9af36a5e5f"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AGAIN = 1073741945`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7263) | 当前常量或枚举值：`1073741945`。 | — |
| <a id="api-0a6848f95b5f2275"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_ALTERASE = 1073741977`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7274) | 当前常量或枚举值：`1073741977`。 | — |
| <a id="api-d78ab6c17e852593"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AMPERSAND = 38`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7136) | 当前常量或枚举值：`38`。 | — |
| <a id="api-183387c7a468ea5b"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_APOSTROPHE = 39`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7137) | 当前常量或枚举值：`39`。 | — |
| <a id="api-28219a454b99d526"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_APPLICATION = 1073741925`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7243) | 当前常量或枚举值：`1073741925`。 | — |
| <a id="api-fd1566979ff21124"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_ASTERISK = 42`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7140) | 当前常量或枚举值：`42`。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDL</code> · 721–900</summary>

<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_AT 63beb7e92e5aa6ba3e2defc8cd294b225e4d85d4ca83540c66b2accd7b11610b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_B a5dc4087f4cd84a1faadb5b1cbf3108879cb599f94e0f60db7e023cd2065a81a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_BACKSLASH 76dee70d9c6b8143ba0570bc25e306d0fe917be8b447d62acadd93bfa5b5e9c3 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_BACKSPACE 1a3a3b4fc3460973ddf8af12680aa394647604d738b0317c095501de57b3b640 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_C a9a5a3833b01c6815c840814ff64d08db6e0c1109e0f705bc40298026ffad866 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_CALL f36257621a0c6ad9e70fbf1b5d36d2dcad4e4e2dad84ead8dad3f0d8906d21ed -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_CANCEL a8b838030f7dd2142603db448afa40b7db0936b3dcca9c0635a3c7df1578ab7e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_CAPSLOCK 2225ec87bc0245ad1cf151fa7a7cb6f307001ceeeb8ee787444f29294576477c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_CARET 09228322cc124735073719c3ad04664367094df09cdfd30986ea699cc0c3cfdb -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_CHANNEL_DECREMENT 1e3ca52cf77aad238add1ad482ebfb74623257914cf252253e7bf53f2950636c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_CHANNEL_INCREMENT 091b89d41271423e979f7653afdf90a465bf0dec7b8440b2c58c357742a2bbc7 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_CLEAR bb74de05d774c98d2d7ea12ae44c426f74cb35119ce68ba38be85fa51b566ab7 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_CLEARAGAIN 88a070acbc0e627682ff86fa3f5dde38d6f51596c9dd3f898e70b05d55eb352b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_COLON 07f4cb6dcba3006087e86d15b223c0083b20e53a0d864b7bf9cf39ce889920d3 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_COMMA 18a1af37bf000d02c684943808d9e9daee5554b08240791c011f1f524dc74faa -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_COPY ca936baaac30c1e65e247940441622820fc39d69ffee123edf525f899b2b11a2 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_CRSEL c5a11243e453e97cd62e1b5dcd6d4fcc64f4f030e443c980fef279733de6a1be -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_CURRENCYSUBUNIT 857064fffdb0cd389198f94bf6ff5f35afce774b9bbbeabfb3357d79c8461fab -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_CURRENCYUNIT 2b2aa56ac749d74f32fe7206d79df2674cb71947df310ad21dc4ca440b1b4936 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_CUT 8f7bffc2319549a50696d6d67e882c2930cf06625e018291c6eb8f3766b3ae4a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_D 7d0c6a07ec3763cc167357cc94ca4121d1eab95abfd0ca8f48f0ab418c74eb4a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_DBLAPOSTROPHE 2bbb3c750b7524340d319f4aa8fc69e0b6db19c8a1ccdf3f67e99f650fe3f483 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_DECIMALSEPARATOR 018124a840d7636f11a391668c5f7fbc319a42d7f46462f5cebe1dddcd58c597 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_DELETE f493840c6a3d9b538cc8990ebffb16c87acbd232052dccf9eb5cf3cddefde71c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_DOLLAR 504b64ffaf8f6f53e34e7ffba0341e3207e0957d84786e374af0dcb44fc8d639 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_DOWN eeb26bf008aa3d441b24294ef3ce2b5035c08de7066b920b923d21f4951c2b79 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_E fe6f911e0d2a9d16e2170d1be1499153eeaad28d2331bf7fc3b73b7b1d154522 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_END 1e33f6f27552b61304d4b907111a0927fe36d33e5cec1cd24198a5e7c01f1adb -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_ENDCALL 6387fbddf80765e04db00a4eb37cbf5c1baf953c1330d43668ada952b673cd77 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_EQUALS 0515d354901de4476ee97650807c3c744804b01ba8bf85228d67bdec65233a88 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_ESCAPE 4ee79bb2c93d07cc037eed626962a5dbf919956510752092689008672b2825f8 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_EXCLAIM 9fd1b3ee49a0c3f1cf070964a5c64a1ca71f624bdcdf0ed5dbd8de30c38a1ea6 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_EXECUTE 956439ac2bb9ec161bc974c387a53c8ecb5121f1fe8ed034b7c9ffb661dd4ac5 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_EXSEL 482de9acdb7df2ce000ed897c188e283ec505e5d3a85bd48ea3077425731ec38 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F 34da3406a9c1dd7c636bdf271e57a375785867856e1130d0e506a81e5c5b41c5 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F1 55e2a9d5db1a85b3a1b312758c1974c9b8f21d10092c2c4698076ac7e914e469 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F10 6e5dac25dcf20a1ed9399989d444751a152c0821f86db35286c6b85ec0b993ff -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F11 4812cd92a0c68e9e2816e0328f88baf876bf3b0ac702ea367b7c3232523d5b8a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F12 7acb6adfc07663a5247f0656b49e2cf5b8387650f7468733026865699d36e2de -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F13 61c286352e75c92ebcb2d91d14735ee2a3b1962815b2e8dfa66fbc354e13d845 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F14 5b3ee50e19d69cd1f7c7b87fc6035acdc0dffdfb3900e341ccd7348c31a44372 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F15 47b31855e6f23919769612e3922eadd653ef5a051c56b1f470474b7abb318faf -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F16 d728740125ffa36e2dce86cdeea27a4eff6bf6b0039c4b79d935edbeaa7b906f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F17 f3d45d4d9bb04f1a05a72ceaee08eac61ada690c37983d6c42f2c60547628154 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F18 1ab84bc2c1d15f4e6bf8aca3941fcbcc28490533e942fc7a1bd58f70489d2f72 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F19 3a697e6a4ee42beafa51fe1c73d199a4ff93f631f3b8936ddbbc1aa80197ff6d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F2 223d72e744a4c74d22b6e0124cc07bbecf1c27c5bb789cf89d8a44c5501aff9d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F20 6c2cf5fce7aefd7c8e793acd4c394b7039b3bad53daa2d9f21efc248fbc3bbed -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F21 3b2a1d408d389b246a6f2d338c62ea31a81ecc674c53c014e467dac50c96e2ca -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F22 38c427ba21253176d007d5c50b3bb21965a58d0dee7635460f28c9e15c4303c0 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F23 d7840c067868d01052ef35729797f9c0e19ec169c205d94d9b59baf757770b53 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F24 caaee4164f9bc8a8f2b1ec6cd4726015c982adf066df72cb2d35c8736fd8c941 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F3 1f6c4ee48e7ff936ee1d77771acb18a168623845dd4d2ff83c54f985a6684060 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F4 17b7434e868f051e4a4234c2f66d60185f5255b4603aa271ab84c53508d63b98 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F5 5ff4dc810a0a483f051efb9017dbfc38247a72dc10f774f1f7daca67c61e0da9 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F6 3564af5a944208a137c88b21c2db473a45c71dcf9c6eb40d7b313a74c8c76017 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F7 02aef1d547710abe7567f45afd07de537e4041adaf91ef77829dba7a1584ddb1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F8 c8d12cba620f07c5fbbf9ea4b164542a6ca9bdd08a0a0dbc62cd897faf4bcf6a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_F9 e0f881df75bc8a521fa2f48d19dcfe170686b0fc5efc25b08ddd4933ad1f7a2d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_FIND 06bccbe3eb9fec3a9b9687ce8ba85d536df6fd762776bdeab4af03114629fa98 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_G 34cb51a672d015c379af801b16f56dfa3328f0ac6eee6fef005bfd899f93b99a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_GRAVE 83a0aa62493faf4938178631fc8bc72c916d97f3ac30a35c13849a58bab216fb -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_GREATER 7e51d67b5a51466ef79c1a2791a815b3e5fd409c5f46182c12292df1e049acbf -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_H f30154c2863f89867292d803027b476f043398a8ef1b7d45989c2670bf440620 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_HASH d7fb08837cf20d19b33d4e27dfd3c7399102f8cf0261b2fb0dd667b316a83080 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_HELP 59cae75d102062b516c250620310897dd65aeaa09d85796594e71b8e3fd02f9a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_HOME 7536cc1e62a528eb1127b450a4ba62c9ccf322f9bbc82d8ab7e73451f059d432 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_I 82068b5cc5652c5da5049e7aa314d4b52048aa0695981395fd98f57b544b519d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_INSERT 825a9ea6cee3a59885f3f8ba1534c6f70d9442f619cffd09ec75d77777abeb26 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_J ed351c3f2770a26a6bf7333d7ae88a22bfbe6c6b4ad8a55e9bc7131784e037d1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_K d6ac15effeaab4734caaefadc72528b4dca9e0dbddaeaa55ce2ae162386887f7 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_0 89af84b9ad28c97a91ca6dd2840722cbcddbfa8cc7fe9373b3edec45bcdfaad1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_00 a4c1f646f297accd94980a8b5b3cacc2be36c47e10260c42db1270ade101b5f1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_000 c5c1040f50c5202ba270534bfcba265e2e30966e9c01d8f7374162d227cb7e82 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_1 861acbefca8e531540858246a126d6cd94deae276d9586f278996f2d92b21021 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_2 f83f6fdb7157876b1d775e520d0cac3951d5cbae7b90044f003eae257d69d94e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_3 80ea7c3c755e87fda8787700d303f4b483b10cd562eed6ccf23693fc09234b7b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_4 b46c14d675daba766dfec1cdb240be100ef2a7e0ec05d8c7dd91f4914b1da227 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_5 1efd09e51cb7dbb74a11c6b9f228b1405bbba64565a008825a0ccfd067e0c3a8 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_6 2f82b582868a141829408406db4e0fd86633bf98b7e404badfc5977ecc404408 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_7 951d7e004a88560ea765a1b0150192a0c47b4a846bda8a2e19f5105255cdea2b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_8 f838a868d3d786257cc100bf3461039bad5e4b5fd29082a14fa5c638df563fc4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_9 7bf890414e2c743c44ec0a3d3a3c634707a0d0e3ccbad47192412427fd602c1c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_A 316d9548db3dd3d23e06ce0302d0bb5b487692b7573132c9678491750dd2de5c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_AMPERSAND f33e91899ff0802f5da46c9a2148a5dffebc4a678df8cfe90f59dd769b4ae0e8 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_AT 82c1513281e408f623c86b90ffecaf6a47a88461c5b8c0c8fbde96c57a69ed46 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_B 87e88253d9cbf3cc646bea8594073392a9cd0464842e27c7910d790908eb9fcc -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_BACKSPACE 527f08738c2ee0b863e4d855c341dbebf814fcd26638568e9886daba8356b9d6 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_BINARY 475420724f7a071ab13017957fb9fb38b1207712f1f2da028ee15dad6cb3da0d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_C e77d1e89d2fee303a5e251dcb42631f59463233ade2e8608fe56a50a32905604 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_CLEAR bdb134e9c7de95ec076b3d57c6c2fa353a7af1312a5c7bcd7e55c1b4f2a5593f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_CLEARENTRY 317fd53d48730a840db15d65cbf53a8455e6e874e55d43c13e223b289654065d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_COLON 4171943d0629536da5719fdd3a05bdda9eaa582cb2f8323e51461a2258dff760 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_COMMA 25faba5c519f4278c89af9070999bee95ba7f4fda2fc8abaea0997dbbbab2109 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_D 778915624e44ee72bb578125ff2dadaf4054919f500107afb93afeed6cc9e36a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_DBLAMPERSAND 62981d38ac07163a6cd4f3e1cbf419bce63a05aff251592e651ff67b6f073cae -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_DBLVERTICALBAR de7cd3b9ece3c2430788e14cb3f5bef32c2d10a39b6f39a284c07bc12800da84 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_DECIMAL cd701c0baf9f13ae17a2ce23dcac6e41c6fb10ad9734f97e9d055dbf54a75c1e -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_DIVIDE 39f59199fda85a5830bd980120cba05328a905862f164dcf4338f9d2f227f934 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_E 36162893822b9301193e50c89f334e5624ce13a0f6d2ad8c53904ceb718ee790 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_ENTER 8f3e7215b6f7aeccec7f02d1bd3a9fd8ec7fd4fc4e8f345fb476e77cecc9ddbc -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_EQUALS 00cb775be84c9049d543a88dc4973b1cd70fa136ea6a6073c3801994dec194f9 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_EQUALSAS400 4ea3a13b07049558964e546d5dd1743baf035b0c964d8ba695949ac2edea3225 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_EXCLAM 431f6b5137115310c22eecbbe3de781fee73ff5fcaee78062ec45c7f0384dd1b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_F a142b5957e0fa1d18b9dc29b86becf09017de986458c1e2615498d7085f30e2d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_GREATER 95f06f8485b9dde0074ada87ad628a3b54e6caa602ac75838c651ba7295ce296 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_HASH e535d6f2b1ec32a35629bab80d75682dc7778f818e33636248528403a95b4e9c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_HEXADECIMAL 5404b4da5060c3bb2fd9e042c5c3fcc7f362cca02179bf732f6cc8c1d7039385 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_LEFTBRACE 9a5b69cbbf006bbb2e2f8232c060239a4da59e5430ce2687e7e950127f63b1ab -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_LEFTPAREN 697d9f27b18723c755924eb25e970567e68f9c0d915a3ae42c792378f3783dde -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_LESS 52bb0e15d6da4a8ac162e2afb94763364ebdbf5d1816725c19e69df44b94c554 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_MEMADD dbdc1ff51f4d42cfb0d019b668c04555b58d3e49917280ab3f4d787566433231 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_MEMCLEAR ea534e69ed9847efa0427cb9b5fe985b200f11cdbf1da408e0524bbad032ede2 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_MEMDIVIDE f83f94fadedd02304ae276a352447968402a47c7d1ee252c40a4d1efc7c7b460 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_MEMMULTIPLY c3428d275ff50d0407ab9246d50485685ddd8260950eafc2ef87fce50e1c6e7b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_MEMRECALL 9916c1b896a229bd643c97c3ad6c6982ca20b778fc56c25783c5599d4e035781 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_MEMSTORE 39b4206741c247d256075b990f377cadc63e631b6a8b3faccdeade4699b443d0 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_MEMSUBTRACT 135f8dd848cab530643ea4a130170a6886551d2cbf7b64d8f72eeeef99b172da -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_MINUS 8ce50e59451ec8aa5cfff95ebe346e9189e470d14605571d99f2ce8332eb2b5d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_MULTIPLY 9b362f6c45faf386848fcebb8ecbe65419fe66284674e458b92213bc70bb0bf5 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_OCTAL 54cac16bc132c1a6c9446f3c9962f3d8236c6baa79752784362cb8eccf81e27d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_PERCENT d61454009392d381d3b8442ada05a16c3dfb91081fadc2365a8cb285ad100639 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_PERIOD 5d57ec0556ab3044318e38bb7cc614bd759a323f9eee841c10d0fce835e539dc -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_PLUS d8eb59c8d865cd527b22eef43baa6291e373bc115bfaa528e2ca63e4916a9b5a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_PLUSMINUS b30a2863f75298d8c30759c7ae710a47fec4afd5a504be83b618f4adce7403d4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_POWER 551f4a6165feb339f228c2110489f1060ce8c8448364839426c97ff4900fe9e1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_RIGHTBRACE 73aa81130312fdd81b21b62f1ca3cbf8ca50a27342ef3d1f5328b7db75866eb9 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_RIGHTPAREN bf93d2083d1bd22f053922690f5d92881bbf2aaed553427267f33bfed9262bda -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_SPACE 0417fd330b3184eb099c7563ee8abd57228ceeacdbcb56f575d527ac853c4104 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_TAB 28b7b1e8c3018c51101fc104eece47a9df356b3a675d9fb50b56b91038129a28 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_VERTICALBAR c660ced1b59248fe8e2f39643ceeb79de7fd8df53540c09fe63209ba0ce5901a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_KP_XOR 837b4334a4fbb5c05e8365bae50206b41551102757a8a38daf4c90d3c2a07dfb -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_L 8f7b590ad8efd413d992286224640c28b7e425911922fac1a39c1fa7422ef532 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_LALT 27297c83a997e588be877bc03c8d33b1f245a536f36f2d643a542b384031dba3 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_LCTRL 3faf80c24b732ff4d61f11b876a219b4cf6c2ac49305c9a894c66d9eab095b61 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_LEFT e75510a3070592769f5dd2299b224aa459bd73a8dcd68671e21afcb2481b7008 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_LEFT_TAB 011a8bc6b208f1e82a57abab65dbeb7b186f218df08ba7836540589d0232c932 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_LEFTBRACE 9fca2b21ce5f405dfcb4033713252713bd43447db502c5c5c68caff03680a791 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_LEFTBRACKET 458b6ae693f4b4a82bb906e1aee451bde50a9e01020c79baad13317eac79c7ff -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_LEFTPAREN 346830e5ba1b22b73a6f7637401868fa049fc2bb66993607f6452c826ef440be -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_LESS cfd64c3954bde0944c78cc5dda5f4c64b06220d93c5cf2bb5624f77304c71cbc -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_LEVEL5_SHIFT 562084d9eac18ad0225535fff9df5b833788c6fc340100be2b02a91c40dab409 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_LGUI 215a1be3b181e12bd32a3dfccd99cde2ae8b69520a7756d1a2d857ca30049498 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_LHYPER 1c067e35bd0f6cbf9712836a02bc281c85f7e0680f730e727bac03728599a943 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_LMETA 32112f3c797fb5ff7385392b189cd4139b2d03aff520fe68be140084f78f95ec -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_LSHIFT 189e3e1c52861cb118ae24242fc9273e20c9bcbbbc6982edd5f50257d3886065 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_M d59f9ba2b13ee415e5e7f7c5705889a44ff910655f04d3ee396781865d6c8cda -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MEDIA_EJECT 1921c69a490fcce38b3cc16da907643386e1e04a4e97ca2ae97a77652625e952 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MEDIA_FAST_FORWARD 078b74c6aa53862d6b79310f64c2e4da8d6a74b8d8df303c67d26bd9c7b0beec -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MEDIA_NEXT_TRACK 4b6d7c6edbe29d7fde19514fb46714cbbd893c0ef5652ebbc8145201b505375d -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MEDIA_PAUSE d0eac706d000bbc0c6dfdfc33b7f9dbab5a81be451faa8d63066d413891787d4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MEDIA_PLAY a6a063d9622d956654fddc7fa3e85aa87e8e7f5e055093570c3968a8a3468cac -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MEDIA_PLAY_PAUSE 7333784b6d82cc1591eb2cd7480cff80662d73a92c2b1e035401e781127744b4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MEDIA_PREVIOUS_TRACK 4759fca890795fa816d8b869443072207553a6b21d31c24221f52a410fbb4ca5 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MEDIA_RECORD ac0a1b30585dc547e416a576b3d7bd12976b5eb0a2d08f65571c523f279f770a -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MEDIA_REWIND 26873739390272507985e4b84603459ff871c9e40ba5c4bf191a3b276b7da805 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MEDIA_SELECT dc78a722c45ff6b54d6847636a0ebe4f727bc4e43e8b428062f525e98c6e440b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MEDIA_STOP 65b59e9411e96eedf4e94bdfbc8d8bda5d4fd34557810be9c4024c53aa628f8f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MENU 2d838aac5a6f4c63787a118a429cb0e4ecf9d7e3c0dad25aefcb02afbc8f535c -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MINUS a1076c814244785eb5c3017518643cf5471c7b49afd482fba71436403b606e6f -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MODE c05e17cf1ba45c858fcdefcd4069de8188a939375b5eabe13edf46a592a654b1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MULTI_KEY_COMPOSE 1b28b62ced333f87740d77c17ab73f1aaab2ba6454f4e1f7122bf5b7ddd10a90 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_MUTE 004a50471bfff3d1f054163912b91fa181f858580a3fbb10b28138cc76c7cd48 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_N 460bb965e324a36f5844944a77285f1f879bdf1563258b8172c91a7132c2c239 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_NUMLOCKCLEAR 105c53a3229ce3ae22c596aa5843ec9bbb49bb46686ca05cb5fd155861ddd92b -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_O 29106af28b7566dd7590d3e06662601ae14ef3cfad74219f8012b97f3d828834 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_OPER f16059eb4bf819204c04861a935766631c662f060f11e6ace3d446568a208689 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_OUT 268d5f655ec5037c01fedb32d52c66096b46050f197fd3b005d3f78c6bab9232 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_P 46f0c59dd69b864f765c0bd52a4e90e181b763ba7901abe19101eb6be4c8eeb0 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_PAGEDOWN c2470e99471424b8e9d220d18d352e83fe551fbdf3a3c9bce75cc97ff85ab428 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_PAGEUP c7116317cc762a15cfb214c9e52da6f6858f89571a4044f0f98281178c50febe -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_PASTE 842e37e3f2efb959a7f2bdbd9af74670c4ef6886815bf5d471a938f64e775bd6 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_PAUSE 0662e61034af60330f4a367c6371ecb0d6f695c3f123f9185b34c8d272273850 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_PERCENT d1dc079f8aa245f471782ec49a4fa4b84b032effbb5040634ff80b0fbb89a005 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_PERIOD 7d4554bd0d5e42d54847df7ed24cb3145c6ed04f92a36c17b67fe649791e7dc4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_PIPE 5f9bee4503858ba40d3d0803ee078d4127302c57922d1a72de146a74190f1057 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_PLUS 34137ae0c163cc2ab99304bc12aaed85396b49f42376642aa682b48f2458aff4 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_PLUSMINUS 3bbce9a42fde625ae2483c1911232933dcdc88001643342d32bb2b3a931dc240 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_POWER 2817dc583f3283c5dc3fcf4b41e3817f2f795646cad5d52341bb893d610be3b1 -->
<!-- api: F:Inno.Native.Sdl3.SDL.SDLK_PRINTSCREEN 3100ce3549cea8f15dc1760bf5dd87ef634335bc95c3267f0f875dda9ef0096e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9d0c710740ed98cf"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_AT = 64`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7162) | 当前常量或枚举值：`64`。 | — |
| <a id="api-62ba6e5833ddb1a2"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_B = 98`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7170) | 当前常量或枚举值：`98`。 | — |
| <a id="api-17a877d5fdf38ce1"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_BACKSLASH = 92`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7164) | 当前常量或枚举值：`92`。 | — |
| <a id="api-d067e324aaa9d697"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_BACKSPACE = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7128) | 当前常量或枚举值：`8`。 | — |
| <a id="api-06f37120ea356ef3"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_C = 99`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7171) | 当前常量或枚举值：`99`。 | — |
| <a id="api-095d5b11437daeae"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_CALL = 1073742113`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7372) | 当前常量或枚举值：`1073742113`。 | — |
| <a id="api-644a9a606d10c0d4"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_CANCEL = 1073741979`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7276) | 当前常量或枚举值：`1073741979`。 | — |
| <a id="api-5f9c5685992a6202"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_CAPSLOCK = 1073741881`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7201) | 当前常量或枚举值：`1073741881`。 | — |
| <a id="api-ebc692f83661f1e7"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_CARET = 94`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7166) | 当前常量或枚举值：`94`。 | — |
| <a id="api-fd973bce79140e02"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_CHANNEL_DECREMENT = 1073742085`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7344) | 当前常量或枚举值：`1073742085`。 | — |
| <a id="api-d42f6912f96bd2e4"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_CHANNEL_INCREMENT = 1073742084`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7343) | 当前常量或枚举值：`1073742084`。 | — |
| <a id="api-f3b2c09480eeff45"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_CLEAR = 1073741980`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7277) | 当前常量或枚举值：`1073741980`。 | — |
| <a id="api-dc8444ac1758efa2"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_CLEARAGAIN = 1073741986`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7283) | 当前常量或枚举值：`1073741986`。 | — |
| <a id="api-24caad6069034776"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_COLON = 58`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7156) | 当前常量或枚举值：`58`。 | — |
| <a id="api-f7feb278e686d59a"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_COMMA = 44`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7142) | 当前常量或枚举值：`44`。 | — |
| <a id="api-42519ad71fbac775"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_COPY = 1073741948`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7266) | 当前常量或枚举值：`1073741948`。 | — |
| <a id="api-7cd4d25dd24622d2"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_CRSEL = 1073741987`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7284) | 当前常量或枚举值：`1073741987`。 | — |
| <a id="api-091a23c827615eb9"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_CURRENCYSUBUNIT = 1073742005`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7291) | 当前常量或枚举值：`1073742005`。 | — |
| <a id="api-79c685bf656355bb"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_CURRENCYUNIT = 1073742004`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7290) | 当前常量或枚举值：`1073742004`。 | — |
| <a id="api-d84cfeb05fb3d46c"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_CUT = 1073741947`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7265) | 当前常量或枚举值：`1073741947`。 | — |
| <a id="api-5b1871057508c99f"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_D = 100`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7172) | 当前常量或枚举值：`100`。 | — |
| <a id="api-3348e7066600ed22"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_DBLAPOSTROPHE = 34`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7132) | 当前常量或枚举值：`34`。 | — |
| <a id="api-6e12814c34a5d022"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_DECIMALSEPARATOR = 1073742003`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7289) | 当前常量或枚举值：`1073742003`。 | — |
| <a id="api-b37e9702479f1944"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_DELETE = 127`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7199) | 当前常量或枚举值：`127`。 | — |
| <a id="api-92c0141d8936cea3"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_DOLLAR = 36`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7134) | 当前常量或枚举值：`36`。 | — |
| <a id="api-a193b586035433a0"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_DOWN = 1073741905`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7224) | 当前常量或枚举值：`1073741905`。 | — |
| <a id="api-df85c654f863db92"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_E = 101`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7173) | 当前常量或枚举值：`101`。 | — |
| <a id="api-aa4da88997f7a807"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_END = 1073741901`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7220) | 当前常量或枚举值：`1073741901`。 | — |
| <a id="api-a8bebf654d5cc6b9"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_ENDCALL = 1073742114`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7373) | 当前常量或枚举值：`1073742114`。 | — |
| <a id="api-05139e664c4430a2"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_EQUALS = 61`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7159) | 当前常量或枚举值：`61`。 | — |
| <a id="api-7676f037853f3ee8"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_ESCAPE = 27`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7127) | 当前常量或枚举值：`27`。 | — |
| <a id="api-923ea85961977e46"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_EXCLAIM = 33`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7131) | 当前常量或枚举值：`33`。 | — |
| <a id="api-3f94df7cb8ff6489"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_EXECUTE = 1073741940`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7258) | 当前常量或枚举值：`1073741940`。 | — |
| <a id="api-54a747bbb134d6fc"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_EXSEL = 1073741988`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7285) | 当前常量或枚举值：`1073741988`。 | — |
| <a id="api-d8bd3a220e6ffa33"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F = 102`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7174) | 当前常量或枚举值：`102`。 | — |
| <a id="api-0617b897b9f0aa9e"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F1 = 1073741882`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7202) | 当前常量或枚举值：`1073741882`。 | — |
| <a id="api-55dcaabe189d8170"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F10 = 1073741891`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7211) | 当前常量或枚举值：`1073741891`。 | — |
| <a id="api-c8edf2ad2056d509"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F11 = 1073741892`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7212) | 当前常量或枚举值：`1073741892`。 | — |
| <a id="api-120cce271a9249ef"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F12 = 1073741893`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7213) | 当前常量或枚举值：`1073741893`。 | — |
| <a id="api-c202cd7a30816112"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F13 = 1073741928`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7246) | 当前常量或枚举值：`1073741928`。 | — |
| <a id="api-090df0b23335c33e"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F14 = 1073741929`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7247) | 当前常量或枚举值：`1073741929`。 | — |
| <a id="api-c306420a9d7b59db"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F15 = 1073741930`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7248) | 当前常量或枚举值：`1073741930`。 | — |
| <a id="api-09e80ed54b865b32"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F16 = 1073741931`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7249) | 当前常量或枚举值：`1073741931`。 | — |
| <a id="api-b3302a408c5a9abf"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F17 = 1073741932`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7250) | 当前常量或枚举值：`1073741932`。 | — |
| <a id="api-6a63e8ca7f8ae342"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F18 = 1073741933`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7251) | 当前常量或枚举值：`1073741933`。 | — |
| <a id="api-03c1f26c098e306a"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F19 = 1073741934`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7252) | 当前常量或枚举值：`1073741934`。 | — |
| <a id="api-909aeb3270ae66d4"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F2 = 1073741883`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7203) | 当前常量或枚举值：`1073741883`。 | — |
| <a id="api-75b4dcc210a77c71"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F20 = 1073741935`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7253) | 当前常量或枚举值：`1073741935`。 | — |
| <a id="api-11d469c87ed119d8"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F21 = 1073741936`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7254) | 当前常量或枚举值：`1073741936`。 | — |
| <a id="api-cb0635b0d416ba0f"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F22 = 1073741937`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7255) | 当前常量或枚举值：`1073741937`。 | — |
| <a id="api-e7b24046c4444528"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F23 = 1073741938`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7256) | 当前常量或枚举值：`1073741938`。 | — |
| <a id="api-cf979876565bacfd"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F24 = 1073741939`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7257) | 当前常量或枚举值：`1073741939`。 | — |
| <a id="api-4dcef3dde442ba37"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F3 = 1073741884`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7204) | 当前常量或枚举值：`1073741884`。 | — |
| <a id="api-583b9a8ecf7c3506"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F4 = 1073741885`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7205) | 当前常量或枚举值：`1073741885`。 | — |
| <a id="api-c386700945653c92"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F5 = 1073741886`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7206) | 当前常量或枚举值：`1073741886`。 | — |
| <a id="api-ca94aa71228b66d1"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F6 = 1073741887`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7207) | 当前常量或枚举值：`1073741887`。 | — |
| <a id="api-22b635a687cea631"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F7 = 1073741888`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7208) | 当前常量或枚举值：`1073741888`。 | — |
| <a id="api-fbda3b7af5c5d322"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F8 = 1073741889`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7209) | 当前常量或枚举值：`1073741889`。 | — |
| <a id="api-70ca864a92806b61"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_F9 = 1073741890`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7210) | 当前常量或枚举值：`1073741890`。 | — |
| <a id="api-84f167b026501872"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_FIND = 1073741950`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7268) | 当前常量或枚举值：`1073741950`。 | — |
| <a id="api-5c4585ef85481524"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_G = 103`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7175) | 当前常量或枚举值：`103`。 | — |
| <a id="api-6e7fd7a4beea3012"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_GRAVE = 96`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7168) | 当前常量或枚举值：`96`。 | — |
| <a id="api-afa8fe79bc28d969"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_GREATER = 62`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7160) | 当前常量或枚举值：`62`。 | — |
| <a id="api-99a04a6aec636828"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_H = 104`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7176) | 当前常量或枚举值：`104`。 | — |
| <a id="api-0ff6e132bf2d1917"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_HASH = 35`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7133) | 当前常量或枚举值：`35`。 | — |
| <a id="api-9c1d3a6af944f3d9"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_HELP = 1073741941`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7259) | 当前常量或枚举值：`1073741941`。 | — |
| <a id="api-387b1faa1519db72"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_HOME = 1073741898`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7218) | 当前常量或枚举值：`1073741898`。 | — |
| <a id="api-0c5aebbd0c011805"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_I = 105`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7177) | 当前常量或枚举值：`105`。 | — |
| <a id="api-185f8071284bac5a"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_INSERT = 1073741897`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7217) | 当前常量或枚举值：`1073741897`。 | — |
| <a id="api-407a00026c67fe77"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_J = 106`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7178) | 当前常量或枚举值：`106`。 | — |
| <a id="api-bbcfd1ce11ac2c3b"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_K = 107`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7179) | 当前常量或枚举值：`107`。 | — |
| <a id="api-a38483c56b0b4304"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_0 = 1073741922`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7241) | 当前常量或枚举值：`1073741922`。 | — |
| <a id="api-ff5653e45505c79e"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_00 = 1073742000`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7286) | 当前常量或枚举值：`1073742000`。 | — |
| <a id="api-c2029280e8f42bec"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_000 = 1073742001`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7287) | 当前常量或枚举值：`1073742001`。 | — |
| <a id="api-b7545a9e3be25e5a"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_1 = 1073741913`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7232) | 当前常量或枚举值：`1073741913`。 | — |
| <a id="api-b0d15a5b86476c6d"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_2 = 1073741914`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7233) | 当前常量或枚举值：`1073741914`。 | — |
| <a id="api-b45e7b935ec3e34a"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_3 = 1073741915`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7234) | 当前常量或枚举值：`1073741915`。 | — |
| <a id="api-7a54e0bee95e1e3a"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_4 = 1073741916`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7235) | 当前常量或枚举值：`1073741916`。 | — |
| <a id="api-8c840a2d13b1dce8"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_5 = 1073741917`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7236) | 当前常量或枚举值：`1073741917`。 | — |
| <a id="api-bca620ab1ee041a7"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_6 = 1073741918`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7237) | 当前常量或枚举值：`1073741918`。 | — |
| <a id="api-3038ba6621985a48"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_7 = 1073741919`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7238) | 当前常量或枚举值：`1073741919`。 | — |
| <a id="api-fa409acaa58eea6f"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_8 = 1073741920`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7239) | 当前常量或枚举值：`1073741920`。 | — |
| <a id="api-a8c45820a7252a3c"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_9 = 1073741921`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7240) | 当前常量或枚举值：`1073741921`。 | — |
| <a id="api-ac2f0853562caba7"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_A = 1073742012`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7298) | 当前常量或枚举值：`1073742012`。 | — |
| <a id="api-747a7d8bca51d861"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_AMPERSAND = 1073742023`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7309) | 当前常量或枚举值：`1073742023`。 | — |
| <a id="api-be0ad978bdf2b02a"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_AT = 1073742030`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7316) | 当前常量或枚举值：`1073742030`。 | — |
| <a id="api-a876a651edf52966"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_B = 1073742013`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7299) | 当前常量或枚举值：`1073742013`。 | — |
| <a id="api-c6734e9270ed51d5"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_BACKSPACE = 1073742011`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7297) | 当前常量或枚举值：`1073742011`。 | — |
| <a id="api-dc271fcde05ce6d0"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_BINARY = 1073742042`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7328) | 当前常量或枚举值：`1073742042`。 | — |
| <a id="api-c52a3cfdd6414100"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_C = 1073742014`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7300) | 当前常量或枚举值：`1073742014`。 | — |
| <a id="api-3bf82e6114939693"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_CLEAR = 1073742040`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7326) | 当前常量或枚举值：`1073742040`。 | — |
| <a id="api-10bd26b481692d9b"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_CLEARENTRY = 1073742041`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7327) | 当前常量或枚举值：`1073742041`。 | — |
| <a id="api-d4dc495339634ed5"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_COLON = 1073742027`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7313) | 当前常量或枚举值：`1073742027`。 | — |
| <a id="api-9872dd9148274c50"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_COMMA = 1073741957`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7272) | 当前常量或枚举值：`1073741957`。 | — |
| <a id="api-cd2494a1bcb50909"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_D = 1073742015`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7301) | 当前常量或枚举值：`1073742015`。 | — |
| <a id="api-504cd23c26bb810a"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_DBLAMPERSAND = 1073742024`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7310) | 当前常量或枚举值：`1073742024`。 | — |
| <a id="api-fd4b445cb9bf69ed"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_DBLVERTICALBAR = 1073742026`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7312) | 当前常量或枚举值：`1073742026`。 | — |
| <a id="api-e665f2e21cfa8ef0"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_DECIMAL = 1073742044`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7330) | 当前常量或枚举值：`1073742044`。 | — |
| <a id="api-5e586724b30c9df9"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_DIVIDE = 1073741908`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7227) | 当前常量或枚举值：`1073741908`。 | — |
| <a id="api-889b9520c806c6ca"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_E = 1073742016`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7302) | 当前常量或枚举值：`1073742016`。 | — |
| <a id="api-7a70ce0d58c57b9a"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_ENTER = 1073741912`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7231) | 当前常量或枚举值：`1073741912`。 | — |
| <a id="api-63c61693fba5eee6"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_EQUALS = 1073741927`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7245) | 当前常量或枚举值：`1073741927`。 | — |
| <a id="api-3746229c5defae0b"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_EQUALSAS400 = 1073741958`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7273) | 当前常量或枚举值：`1073741958`。 | — |
| <a id="api-c7319a09ea865956"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_EXCLAM = 1073742031`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7317) | 当前常量或枚举值：`1073742031`。 | — |
| <a id="api-d9faa4cc327620d8"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_F = 1073742017`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7303) | 当前常量或枚举值：`1073742017`。 | — |
| <a id="api-e0615c6e6a363923"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_GREATER = 1073742022`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7308) | 当前常量或枚举值：`1073742022`。 | — |
| <a id="api-4073094db1b38a46"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_HASH = 1073742028`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7314) | 当前常量或枚举值：`1073742028`。 | — |
| <a id="api-127ff781da9d8afc"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_HEXADECIMAL = 1073742045`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7331) | 当前常量或枚举值：`1073742045`。 | — |
| <a id="api-596ab42504a2f462"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_LEFTBRACE = 1073742008`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7294) | 当前常量或枚举值：`1073742008`。 | — |
| <a id="api-6a99a872e7b73464"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_LEFTPAREN = 1073742006`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7292) | 当前常量或枚举值：`1073742006`。 | — |
| <a id="api-0d58105f812a3fc6"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_LESS = 1073742021`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7307) | 当前常量或枚举值：`1073742021`。 | — |
| <a id="api-7b011386cfeb99f7"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_MEMADD = 1073742035`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7321) | 当前常量或枚举值：`1073742035`。 | — |
| <a id="api-ad27630fbe25b4f6"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_MEMCLEAR = 1073742034`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7320) | 当前常量或枚举值：`1073742034`。 | — |
| <a id="api-e5a55997bbe2dbb1"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_MEMDIVIDE = 1073742038`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7324) | 当前常量或枚举值：`1073742038`。 | — |
| <a id="api-39ab973c42f373ae"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_MEMMULTIPLY = 1073742037`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7323) | 当前常量或枚举值：`1073742037`。 | — |
| <a id="api-b4782736870379c7"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_MEMRECALL = 1073742033`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7319) | 当前常量或枚举值：`1073742033`。 | — |
| <a id="api-704b31ddf6619b26"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_MEMSTORE = 1073742032`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7318) | 当前常量或枚举值：`1073742032`。 | — |
| <a id="api-2e32578f37ee7a2d"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_MEMSUBTRACT = 1073742036`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7322) | 当前常量或枚举值：`1073742036`。 | — |
| <a id="api-c161d714744a2930"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_MINUS = 1073741910`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7229) | 当前常量或枚举值：`1073741910`。 | — |
| <a id="api-97157622e2577143"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_MULTIPLY = 1073741909`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7228) | 当前常量或枚举值：`1073741909`。 | — |
| <a id="api-ebb4784973444e4b"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_OCTAL = 1073742043`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7329) | 当前常量或枚举值：`1073742043`。 | — |
| <a id="api-41fe63b503c9c521"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_PERCENT = 1073742020`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7306) | 当前常量或枚举值：`1073742020`。 | — |
| <a id="api-5d1fdb4991a73146"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_PERIOD = 1073741923`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7242) | 当前常量或枚举值：`1073741923`。 | — |
| <a id="api-40bd5946ed961501"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_PLUS = 1073741911`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7230) | 当前常量或枚举值：`1073741911`。 | — |
| <a id="api-4663f84a1ef85d4a"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_PLUSMINUS = 1073742039`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7325) | 当前常量或枚举值：`1073742039`。 | — |
| <a id="api-3883403b8270dbc5"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_POWER = 1073742019`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7305) | 当前常量或枚举值：`1073742019`。 | — |
| <a id="api-900b0d58e264e00b"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_RIGHTBRACE = 1073742009`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7295) | 当前常量或枚举值：`1073742009`。 | — |
| <a id="api-10eb55d572491e84"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_RIGHTPAREN = 1073742007`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7293) | 当前常量或枚举值：`1073742007`。 | — |
| <a id="api-9f2bd8595b81c61f"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_SPACE = 1073742029`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7315) | 当前常量或枚举值：`1073742029`。 | — |
| <a id="api-5660489f5bdd0d49"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_TAB = 1073742010`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7296) | 当前常量或枚举值：`1073742010`。 | — |
| <a id="api-dc437bf31a1302f4"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_VERTICALBAR = 1073742025`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7311) | 当前常量或枚举值：`1073742025`。 | — |
| <a id="api-efba2bec6562f465"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_KP_XOR = 1073742018`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7304) | 当前常量或枚举值：`1073742018`。 | — |
| <a id="api-38e0193368a90451"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_L = 108`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7180) | 当前常量或枚举值：`108`。 | — |
| <a id="api-507a2e3430093f4b"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_LALT = 1073742050`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7334) | 当前常量或枚举值：`1073742050`。 | — |
| <a id="api-6c9884b45304f5ad"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_LCTRL = 1073742048`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7332) | 当前常量或枚举值：`1073742048`。 | — |
| <a id="api-675964153c55db56"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_LEFT = 1073741904`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7223) | 当前常量或枚举值：`1073741904`。 | — |
| <a id="api-fc2c8ac51da2cdb3"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_LEFT_TAB = 536870913`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7374) | 当前常量或枚举值：`536870913`。 | — |
| <a id="api-e2c6ab0cfe6c572f"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_LEFTBRACE = 123`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7195) | 当前常量或枚举值：`123`。 | — |
| <a id="api-b7db85b9c22425d5"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_LEFTBRACKET = 91`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7163) | 当前常量或枚举值：`91`。 | — |
| <a id="api-50f1092573240a6d"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_LEFTPAREN = 40`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7138) | 当前常量或枚举值：`40`。 | — |
| <a id="api-27ada240c2d1d8e0"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_LESS = 60`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7158) | 当前常量或枚举值：`60`。 | — |
| <a id="api-ec2e563815c3899d"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_LEVEL5_SHIFT = 536870914`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7375) | 当前常量或枚举值：`536870914`。 | — |
| <a id="api-bb466901be484830"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_LGUI = 1073742051`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7335) | 当前常量或枚举值：`1073742051`。 | — |
| <a id="api-abdb4096f09efdf1"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_LHYPER = 536870918`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7379) | 当前常量或枚举值：`536870918`。 | — |
| <a id="api-5a0b86fd5cb34fbb"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_LMETA = 536870916`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7377) | 当前常量或枚举值：`536870916`。 | — |
| <a id="api-b45ea86df48ce585"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_LSHIFT = 1073742049`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7333) | 当前常量或枚举值：`1073742049`。 | — |
| <a id="api-f69b9727d05ed9ab"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_M = 109`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7181) | 当前常量或枚举值：`109`。 | — |
| <a id="api-c9ec70a1ea45f7d7"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MEDIA_EJECT = 1073742094`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7353) | 当前常量或枚举值：`1073742094`。 | — |
| <a id="api-b463670d106c445f"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MEDIA_FAST_FORWARD = 1073742089`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7348) | 当前常量或枚举值：`1073742089`。 | — |
| <a id="api-d8d5bf00733230a4"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MEDIA_NEXT_TRACK = 1073742091`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7350) | 当前常量或枚举值：`1073742091`。 | — |
| <a id="api-37addf6c449083d5"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MEDIA_PAUSE = 1073742087`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7346) | 当前常量或枚举值：`1073742087`。 | — |
| <a id="api-060ad7621b528ce9"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MEDIA_PLAY = 1073742086`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7345) | 当前常量或枚举值：`1073742086`。 | — |
| <a id="api-1b6a3d720d04cf5f"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MEDIA_PLAY_PAUSE = 1073742095`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7354) | 当前常量或枚举值：`1073742095`。 | — |
| <a id="api-bf722362755549f6"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MEDIA_PREVIOUS_TRACK = 1073742092`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7351) | 当前常量或枚举值：`1073742092`。 | — |
| <a id="api-8704c5d8733fde48"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MEDIA_RECORD = 1073742088`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7347) | 当前常量或枚举值：`1073742088`。 | — |
| <a id="api-e9aa3bda9d0e88e9"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MEDIA_REWIND = 1073742090`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7349) | 当前常量或枚举值：`1073742090`。 | — |
| <a id="api-3ec4eedf6a3a1291"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MEDIA_SELECT = 1073742096`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7355) | 当前常量或枚举值：`1073742096`。 | — |
| <a id="api-454a3f1b39ba9f20"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MEDIA_STOP = 1073742093`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7352) | 当前常量或枚举值：`1073742093`。 | — |
| <a id="api-00ca4bfebdebd679"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MENU = 1073741942`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7260) | 当前常量或枚举值：`1073741942`。 | — |
| <a id="api-7d3ba36c9daec090"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MINUS = 45`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7143) | 当前常量或枚举值：`45`。 | — |
| <a id="api-1b5c39bde78d7a53"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MODE = 1073742081`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7340) | 当前常量或枚举值：`1073742081`。 | — |
| <a id="api-f0211ec66185983b"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MULTI_KEY_COMPOSE = 536870915`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7376) | 当前常量或枚举值：`536870915`。 | — |
| <a id="api-56787c1cb46d695b"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_MUTE = 1073741951`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7269) | 当前常量或枚举值：`1073741951`。 | — |
| <a id="api-b8180133de755477"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_N = 110`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7182) | 当前常量或枚举值：`110`。 | — |
| <a id="api-9f21849e8fdacf12"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_NUMLOCKCLEAR = 1073741907`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7226) | 当前常量或枚举值：`1073741907`。 | — |
| <a id="api-4688abd2513f6941"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_O = 111`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7183) | 当前常量或枚举值：`111`。 | — |
| <a id="api-5674a82a624bb38e"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_OPER = 1073741985`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7282) | 当前常量或枚举值：`1073741985`。 | — |
| <a id="api-50e85e82e80009f9"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_OUT = 1073741984`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7281) | 当前常量或枚举值：`1073741984`。 | — |
| <a id="api-3c8bd1828f43851f"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_P = 112`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7184) | 当前常量或枚举值：`112`。 | — |
| <a id="api-f1f22c8dae04b70f"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_PAGEDOWN = 1073741902`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7221) | 当前常量或枚举值：`1073741902`。 | — |
| <a id="api-f0176b56fc201e38"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_PAGEUP = 1073741899`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7219) | 当前常量或枚举值：`1073741899`。 | — |
| <a id="api-7fac251052e7d261"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_PASTE = 1073741949`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7267) | 当前常量或枚举值：`1073741949`。 | — |
| <a id="api-bd53c2ae01c75b24"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_PAUSE = 1073741896`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7216) | 当前常量或枚举值：`1073741896`。 | — |
| <a id="api-14a3e18e7e6a6f8d"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_PERCENT = 37`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7135) | 当前常量或枚举值：`37`。 | — |
| <a id="api-b1602cb83a2b13ea"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_PERIOD = 46`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7144) | 当前常量或枚举值：`46`。 | — |
| <a id="api-3209d92ac9fc4313"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_PIPE = 124`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7196) | 当前常量或枚举值：`124`。 | — |
| <a id="api-bc6d3701d79b3b2a"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_PLUS = 43`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7141) | 当前常量或枚举值：`43`。 | — |
| <a id="api-28078692206a4975"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_PLUSMINUS = 177`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7200) | 当前常量或枚举值：`177`。 | — |
| <a id="api-95af8e799b8ea312"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_POWER = 1073741926`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7244) | 当前常量或枚举值：`1073741926`。 | — |
| <a id="api-d6ad8a451442df92"></a>[`public const uint Inno.Native.Sdl3.SDL.SDLK_PRINTSCREEN = 1073741894`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7214) | 当前常量或枚举值：`1073741894`。 | — |

</details>
