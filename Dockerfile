# escape=`
FROM hub.nexdev.net/base/rocketor/buildservice AS builder
ARG TRANSFORM_CONFIG
ARG ROCKETOR_API
ARG REDMINE_IDS

SHELL ["cmd", "/S", "/C"]

WORKDIR /build


# Copy files *.sln, *.csproj, packages.config to restore nuget packages
COPY . .

RUN nuget.exe restore Fanex.CustomerSetting.sln


RUN MSBuild.exe ./Core/Fanex.CustomerSetting.WebAPI/Fanex.CustomerSetting.WebAPI.csproj `
    /t:Build;WebPublish /m `
    /p:DebugType=None `
    /p:Configuration=Release `
    /p:WebPublishMethod=FileSystem `
    /p:TransformWebConfigEnabled=False `
    /p:LocalizedBuild=true `
    /p:AutoParameterizationWebConfigConnectionStrings=False `
    /p:MarkWebConfigAssistFilesAsExclude=false `
    /p:publishUrl="C:\Compiled" `
    /consoleloggerparameters:ErrorsOnly

WORKDIR C:/Compiled

# Change SP Version
RUN `
    if defined REDMINE_IDS `
    FOR %P IN ( `
        App_Data\CustomerSettingSpNames\APILoggerSpNames.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.Casino.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.NumberGames2.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType1.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType2.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType3.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType4.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType5.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType6.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType7.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType8.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType9.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType11.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType13.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType14.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType15.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType16.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType17.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType18.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.SettingType20.xml, `
        App_Data\CustomerSettingSpNames\Fanex.CustomerSetting.xml `
    ) `
    DO curl --fail -o C:\sp-result.xml --form file=@"%P" --form "redmineIds=%REDMINE_IDS%" -X POST %ROCKETOR_API% `
        && xcopy C:\sp-result.xml "%P" /Y

# Transform config
RUN echo "%TRANSFORM_CONFIG%" `
    && xcopy "Web.%TRANSFORM_CONFIG%.config" "Web.config" /Y `
    && xcopy "Web.ConnectionStrings.%TRANSFORM_CONFIG%.config" "Web.config" /Y 

FROM hub.nexdev.net/base/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019 AS base

WORKDIR C:/inetpub/wwwroot

COPY --from=builder C:/Compiled .

RUN dir ./bin/

ENTRYPOINT ["C:\\LogMonitor\\LogMonitor.exe", "C:\\ServiceMonitor.exe", "w3svc"]