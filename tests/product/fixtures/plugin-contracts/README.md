# Installed plugin product fixture

`Controller.cs.txt` is the single runtime script used by the Editor reload regression and the published Player. `Tone.editor.cs.txt` imports the square-wave sample and the game mixer. The game template contains a serialized startup scene and a project setting selecting that mixer. Work in a fresh scratch directory so that product caches remain outside this fixture.

From the engine root, set `$scratch` to an empty absolute path and `$support` to the local support-pack store. The example uses PowerShell and the Windows x64 product.

```powershell
$fixture = Resolve-Path tests/product/fixtures/plugin-contracts
$engine = (Get-Location).Path
$sdk = (Get-Command dotnet).Source
New-Item -ItemType Directory -Force $scratch | Out-Null
Copy-Item "$fixture/ControllerFixture","$fixture/Game" $scratch -Recurse
Copy-Item "$fixture/Controller.cs.txt" "$scratch/ControllerFixture/Assets/Controller.cs"
Copy-Item "$fixture/Tone.editor.cs.txt" "$scratch/ControllerFixture/Assets/Tone.editor.cs"
New-Item -ItemType Directory "$scratch/Game/Plugins" | Out-Null
& $sdk run -c Release --project entry/InnoEngine.Build -- plugin --engine-root $engine --dotnet $sdk --tools-target windows-x64 --project "$scratch/ControllerFixture" --display-name "Controller Fixture" --output "$scratch/Game/Plugins/Controller.iplugin"
& $sdk run -c Release --project entry/InnoEngine.Editor -- "$scratch/Game" --window-visible false --smoke-frames 600
& $sdk run -c Release --project entry/InnoEngine.Build -- game --engine-root $engine --dotnet $sdk --tools-target windows-x64 --target windows-x64 --deployment nativeaot --project "$scratch/Game" --support-packs $support --startup-scene Startup.iscene --output "$scratch/Published"
```

Run the resulting Player executable from the published game directory with `--window-visible false --smoke-frames 600`. The fixture prints `Controller fixture input connected.` after registering its controller, `Controller fixture processed audible PCM.` on the first nonzero block processed by its mixer effect, and `Controller fixture playback complete.` after the matching voice reports natural completion. The Player's own `INNO-SMOKE` line records completion of the bounded frame run. The [native offline regression](../../../backends/MiniAudio/runtime/Inno.Adapter.Audio.MiniAudio.Native.Tests/MiniAudioDeviceTests.cs) independently verifies the exact samples.
