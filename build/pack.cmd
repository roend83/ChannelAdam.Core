SET msbuild="C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"

%msbuild% ..\src\ChannelAdam.Core\ChannelAdam.Core.csproj /t:Rebuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;OutDir=bin\Release\net40
%msbuild% ..\src\ChannelAdam.Core\ChannelAdam.Core.csproj /t:Rebuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;OutDir=bin\Release\net45

..\tools\nuget\nuget.exe pack ..\src\ChannelAdam.Core\ChannelAdam.Core.nuspec -Symbols

pause
