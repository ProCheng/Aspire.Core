﻿dotnet restore
dotnet build 
cd Aspire.Core.Api

dotnet publish 
echo "Successfully!!!! ^ please see the file ."
cd bin/Debug/net7.0/publish/

#rm -f appsettings.json
#\cp -rf /var/jenkins_home/workspace/SecurityConfig/Aspire.Core/appsettings.json appsettings.json

#docker stop apkcontainer
#docker rm apkcontainer
#docker rmi laozhangisphi/apkimg

chmod 777 StopContainerImg.sh
./StopContainerImg.sh apkcontainer laozhangisphi/apkimg

docker build -t laozhangisphi/apkimg .
docker run --name=apkcontainer -d -v /data/Aspirecore/appsettings.json:/app/appsettings.json -v /data/Aspirecore/Log/:/app/Log -v /etc/localtime:/etc/localtime -it -p 9291:9291 laozhangisphi/apkimg