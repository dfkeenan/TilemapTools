@echo off
setlocal
set XenkoSdkDir=%SiliconStudioXenkoDir%GamePackages\Xenko.2.0.4.1\
set XenkoSdkBinDir=%XenkoSdkDir%Bin\Windows\
"%XenkoSdkBinDir%SiliconStudio.Assets.CompilerApp.exe" --profile=Windows --platform=Windows --output-path="%~dp0obj\app_data" --build-path="%~dp0obj\build_app_data" --package-file="Graphics.xkpkg"
"%XenkoSdkBinDir%SiliconStudio.Assets.CompilerApp.exe" --profile=Windows-OpenGL --platform=Windows --output-path="%~dp0obj\app_data" --build-path="%~dp0obj\build_app_data" --package-file="Graphics.xkpkg"
"%XenkoSdkBinDir%SiliconStudio.Assets.CompilerApp.exe" --profile=Windows-OpenGLES --platform=Windows --output-path="%~dp0obj\app_data" --build-path="%~dp0obj\build_app_data" --package-file="Graphics.xkpkg"
"%XenkoSdkBinDir%SiliconStudio.Assets.CompilerApp.exe" --profile=Windows-Vulkan --platform=Windows --output-path="%~dp0obj\app_data" --build-path="%~dp0obj\build_app_data" --package-file="Graphics.xkpkg"