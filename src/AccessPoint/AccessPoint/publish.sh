dotnet publish -r linux-arm /p:ShowLinkerSizeComparison=true 
pushd ./bin/Debug/net6.0/linux-arm/publish
scp -p raspberry * pi@raspberrypi.local:/home/pi/AccessPoint
popd