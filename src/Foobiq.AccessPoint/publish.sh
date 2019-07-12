dotnet publish -r linux-arm /p:ShowLinkerSizeComparison=true 
pushd ./bin/Debug/netcoreapp2.2/linux-arm/publish
scp -p raspberry * pi@raspberrypi.local:/home/pi/AccessPoint
popd