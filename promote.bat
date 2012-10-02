set PATH=PATH;C:\Users\vitalii.biliienko\AppData\Local\GitHub\PortableGit_8810fd5c2c79c73adcc73fd0825f3b32fdb816e7\bin;
echo USERDOMAIN \ USERNAME
copy Version.cs D:\git\TestRepo >nul
cd D:\git\TestRepo
git.exe add Version.cs
git.exe commit -m "Version update"
git.exe push origin master