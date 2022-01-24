del /s /q .\packages\microlog.core
del /s /q .\packages\microlog.collector.client
del /s /q .\packages\microlog.provider.aspnetcore
nuget add ..\src\MicroLog.Core\bin\Debug\MicroLog.Core.1.0.0.nupkg -source .\packages
nuget add ..\src\MicroLog.Collector.Client\bin\Debug\MicroLog.Collector.Client.1.0.0.nupkg -source .\packages
nuget add ..\src\MicroLog.Provider.AspNetCore\bin\Debug\MicroLog.Provider.AspNetCore.1.0.0.nupkg -source .\packages