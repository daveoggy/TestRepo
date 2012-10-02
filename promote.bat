set PATH=PATH;C:\Users\vitalii.biliienko\AppData\Local\GitHub\PortableGit_8810fd5c2c79c73adcc73fd0825f3b32fdb816e7\bin;
echo %USERDOMAIN%\%USERNAME%
@if not exist "%HOME%" @set HOME=%HOMEDRIVE%%HOMEPATH%
@if not exist "%HOME%" @set HOME=%USERPROFILE%
copy Version.cs D:\git\TestRepo >nul
cd D:\git\TestRepo
call git.exe add Version.cs
call git.exe commit -m "Version update"
call git.exe push origin master