@echo off
setlocal
set XenkoSdkDir=%SiliconStudioXenkoDir%GamePackages\Xenko.1.9.3-beta\
set XenkoSdkBinDir=%XenkoSdkDir%Bin\Windows-Direct3D11\
"%XenkoSdkBinDir%SiliconStudio.Assets.CompilerApp.exe" --profile=Windows --platform=Windows --output-path="%~dp0obj\app_data" --build-path="%~dp0obj\build_app_data" --package-file="%~dp0Graphics.xkpkg"
"%XenkoSdkBinDir%SiliconStudio.Assets.CompilerApp.exe" --profile=Windows-OpenGL --platform=Windows --output-path="%~dp0obj\app_data" --build-path="%~dp0obj\build_app_data" --package-file="%~dp0Graphics.xkpkg"
"%XenkoSdkBinDir%SiliconStudio.Assets.CompilerApp.exe" --profile=Windows-OpenGLES --platform=Windows --output-path="%~dp0obj\app_data" --build-path="%~dp0obj\build_app_data" --package-file="%~dp0Graphics.xkpkg"
"%XenkoSdkBinDir%SiliconStudio.Assets.CompilerApp.exe" --profile=Windows-Vulkan --platform=Windows --output-path="%~dp0obj\app_data" --build-path="%~dp0obj\build_app_data" --package-file="%~dp0Graphics.xkpkg"