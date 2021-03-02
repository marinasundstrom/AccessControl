dotnet publish -r linux-arm /p:ShowLinkerSizeComparison=true 
pushd ./bin/Debug/net5.0/linux-arm/publish
scp -r -p raspberry * pi@raspberrypi.local:/home/pi/AccessPoint
popd