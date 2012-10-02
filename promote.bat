set PATH=PATH;C:\Users\vitalii.biliienko\AppData\Local\GitHub\PortableGit_8810fd5c2c79c73adcc73fd0825f3b32fdb816e7\bin;
echo %USERDOMAIN%\%USERNAME%
copy Version.cs D:\git\TestRepo >nul
cd D:\git\TestRepo
call git.cmd add Version.cs
call git.cmd commit -m "Version update"
call git.cmd push origin master