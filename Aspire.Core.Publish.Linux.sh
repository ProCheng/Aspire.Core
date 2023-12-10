
find .PublishFiles/ -type f -and ! -path '*/wwwroot/images/*' ! -name 'appsettings.*' |xargs rm -rf
dotnet build;
rm -rf /home/Aspire.Core/Aspire.Core.Api/bin/Debug/.PublishFiles;
dotnet publish -o /home/Aspire.Core/Aspire.Core.Api/bin/Debug/.PublishFiles;
rm -rf /home/Aspire.Core/Aspire.Core.Api/bin/Debug/.PublishFiles/WMAspire.db;
# cp -r /home/Aspire.Core/Aspire.Core.Api/bin/Debug/.PublishFiles ./;
awk 'BEGIN { cmd="cp -ri /home/Aspire.Core/Aspire.Core.Api/bin/Debug/.PublishFiles ./"; print "n" |cmd; }'
echo "Successfully!!!! ^ please see the file .PublishFiles";