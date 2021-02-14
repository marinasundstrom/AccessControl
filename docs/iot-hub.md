# IoT Hub

The system uses IoT to connect devices, running AccessPoint service, to the AppService.

The communication is mainly as Commands and Event.

## AppService

You need to configure the Event Hub connection.

Viewing the IoT Hub, under "Built-in Endpoints" there is an endpoint that is compatible Event Hub. 

This is to be set as ```Events:Endpoint``` in the ```appsettings.json``` file .

## AccessPoint

In your hub, need to create a Device with Device Id "AccessPoint1". Once created you can get the endpoint.
