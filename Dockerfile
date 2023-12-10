#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#这种模式是直接在构建镜像的内部编译发布dotnet项目。
#注意下容器内输出端口是9291
#如果你想先手动dotnet build成可执行的二进制文件，然后再构建镜像，请看.Api层下的dockerfile。


#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Aspire.Core.Api/Aspire.Core.Api.csproj", "Aspire.Core.Api/"]
COPY ["Aspire.Core.Extensions/Aspire.Core.Extensions.csproj", "Aspire.Core.Extensions/"]
COPY ["Aspire.Core.EventBus/Aspire.Core.EventBus.csproj", "Aspire.Core.EventBus/"]
COPY ["Aspire.Core.Common/Aspire.Core.Common.csproj", "Aspire.Core.Common/"]
COPY ["Aspire.Core.Model/Aspire.Core.Model.csproj", "Aspire.Core.Model/"]
COPY ["Aspire.Core.Serilog.Es/Aspire.Core.Serilog.Es.csproj", "Aspire.Core.Serilog.Es/"]
COPY ["Ocelot.Provider.Nacos/Ocelot.Provider.Nacos.csproj", "Ocelot.Provider.Nacos/"]
COPY ["Aspire.Core.Services/Aspire.Core.Services.csproj", "Aspire.Core.Services/"]
COPY ["Aspire.Core.IServices/Aspire.Core.IServices.csproj", "Aspire.Core.IServices/"]
COPY ["Aspire.Core.Repository/Aspire.Core.Repository.csproj", "Aspire.Core.Repository/"]
COPY ["Aspire.Core.Tasks/Aspire.Core.Tasks.csproj", "Aspire.Core.Tasks/"]
COPY ["build", "build/"]
RUN dotnet restore "Aspire.Core.Api/Aspire.Core.Api.csproj"
COPY . .
WORKDIR "/src/Aspire.Core.Api"
RUN dotnet build "Aspire.Core.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Aspire.Core.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 9291 
ENTRYPOINT ["dotnet", "Aspire.Core.Api.dll"]
