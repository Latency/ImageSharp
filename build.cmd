@echo Off

if not "%GitVersion_NuGetVersion%" == "" (
    dotnet restore /p:packageversion=%GitVersion_NuGetVersion%
)ELSE ( 
	dotnet restore 
)

ECHO Building nuget packages
if not "%GitVersion_NuGetVersion%" == "" (
    dotnet build -c Release /p:packageversion=%GitVersion_NuGetVersion%
)ELSE ( 
    dotnet build -c Release
)
if not "%errorlevel%"=="0" goto failure

REM not needed as it'll be ran on appbeyor by codecov anyway
REM dotnet test ./tests/ImageSharp.Tests/ImageSharp.Tests.csproj

if not "%GitVersion_NuGetVersion%" == "" (
    dotnet pack ./src/ImageSharp/ -c Release --output ../../artifacts --no-build /p:packageversion=%GitVersion_NuGetVersion%
)ELSE ( 
    dotnet pack ./src/ImageSharp/ -c Release --output ../../artifacts --no-build
)

if not "%GitVersion_NuGetVersion%" == "" (
    dotnet pack ./src/ImageSharp.Drawing/ -c Release --output ../../artifacts --no-build /p:packageversion=%GitVersion_NuGetVersion%
)ELSE ( 
    dotnet pack ./src/ImageSharp.Drawing/ -c Release --output ../../artifacts --no-build
)

if not "%errorlevel%"=="0" goto failure


if not "%errorlevel%"=="0" goto failure

:success
ECHO successfully built project
REM exit 0
goto end

:failure
ECHO failed to build.
REM exit -1
goto end

:end