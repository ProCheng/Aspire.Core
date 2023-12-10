color B

del  .PublishFiles\*.*   /s /q

dotnet restore

dotnet build

cd Aspire.Core.Api

dotnet publish -o ..\Aspire.Core.Api\bin\Debug\net7.0\

md ..\.PublishFiles

xcopy ..\Aspire.Core.Api\bin\Debug\net7.0\*.* ..\.PublishFiles\ /s /e 

echo "Successfully!!!! ^ please see the file .PublishFiles"

cmd