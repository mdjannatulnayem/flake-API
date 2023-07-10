Hello fellow developers!

This is my semester project where I created a weather API (REST) using ASP.NET Core in latest .NET 7 (not using minimal APIs for this).

I prefer calling it Flake❄️ Weather Service.

The API provides several endpoints that can be invoked to get environmental data such as temperature, humidity, pressure etc.

Mind it that these data are collected using dedicated hardware.

NodeMCU ESP-32S boaad => collects weather data and send to server through HTTP

DHT22 sensor => collects humidity and temperature

BME 280 sensor => collects atmospheric pressure and temperature

Neo-6M GPS sensor => GPS device

Mutiple units of these devices are installed at specific locations to gather weather data.Currently it doesn't predict weather conditions but that should be added soon in near future.

Stay tuned ✨
