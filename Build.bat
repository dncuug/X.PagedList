@ECHO OFF
%SYSTEMROOT%\Microsoft.Net\Framework\v3.5\msbuild.exe "./src/PagedList.sln" /t:rebuild /p:Configuration=Debug
PAUSE