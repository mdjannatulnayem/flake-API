#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <WiFi.h>
#include <DHT.h>
#include <Adafruit_BMP280.h>
#include <TinyGPS++.h>
#include <SoftwareSerial.h>
#include "appConfiguration.h"
#define DHTPIN 23
#define DHTTYPE DHT11

int sRX = 4, sTX = 3;
uint32_t GPSBaud = 9600;
double humidity, temperature, pressure, latitude, longitude;
class DateTime{
public:
  int Y, M, D, h, m, s;
};
DateTime datetime;
char jsonPayload[128];

TinyGPSPlus gpsSensor;
DHT dhtSensor(DHTPIN, DHTTYPE);
Adafruit_BMP280 bmpSensor;
SoftwareSerial ss(sRX,sTX);

const char* location = _location;
const char* apiPostEndpoint = _endpoint;
const char* ssid = _ssid;
const char* password = _password;
const char* apiStaticAuthToken = _token;

void setup() 
{
//  Serial.begin(115200); //Using hardware serial!
//  while (!Serial) delay(100);
  dhtSensor.begin();
  unsigned status = bmpSensor.begin(0x76);
  if (!status) {
//    Serial.println("Could not find a valid BMP280 sensor, check wiring or try a different address!");
//    Serial.print("SensorID was: 0x");
//    Serial.println(bmpSensor.sensorID(),16);
//    Serial.print("ID of 0xFF probably means a bad address, a BMP 180 or BMP 085\n");
//    Serial.print("ID of 0x56-0x58 represents a BMP 280,\n");
//    Serial.print("ID of 0x60 represents a BME 280.\n");
//    Serial.print("ID of 0x61 represents a BME 680.\n");
    while (1) delay(10);
  }
  /* Default settings from datasheet. */
  bmpSensor.setSampling(Adafruit_BMP280::MODE_NORMAL,     /* Operating Mode. */
                  Adafruit_BMP280::SAMPLING_X2,     /* Temp. oversampling */
                  Adafruit_BMP280::SAMPLING_X16,    /* Pressure oversampling */
                  Adafruit_BMP280::FILTER_X16,      /* Filtering. */
                  Adafruit_BMP280::STANDBY_MS_500); /* Standby time. */
  
  if (millis() > 5000 && gpsSensor.charsProcessed() < 10)
  {
//    Serial.println("No GPS detected: check wiring.");
    while(true);
  }
  WiFi.begin(ssid,password);
//  Serial.print("Connecting to WiFi");
  
  while(WiFi.status() != WL_CONNECTED){
//    Serial.print(".");
    delay(500);
  }

//  Serial.println("\nConnected to the network");
//  Serial.print("IP address: ");
//  Serial.println(WiFi.localIP());
}

void loop() 
{
  readSensor(); // Take readings from all sensors!
  delay(2000); // 10 second delay between each call!
  if((WiFi.status() == WL_CONNECTED))
  {
    HTTPClient client;
    client.begin(String(apiPostEndpoint) + String(location));
    client.addHeader("authKey",String(_token));
    client.addHeader("Content-Type","application/json");
    const size_t capacity = JSON_OBJECT_SIZE(1);
    StaticJsonDocument<capacity> document;
    JsonObject object = document.to<JsonObject>();
    object["Time"] = String(datetime.Y) + String("-") + String(datetime.M) + String("-") + String(datetime.D) 
    + String("T") + String(datetime.h) + String(":") + String(datetime.m) + String(":") + String(datetime.s) + String("+GMT6:00");
    object["Temperature"] = String(temperature);
    object["Humidity"] = String(humidity);
    object["Pressure"] = String(pressure);
    object["Latitude"] = String(latitude);
    object["Longitude"] = String(longitude);
    serializeJson(document,jsonPayload);
    int statusCode = client.POST(String(jsonPayload));
    if(statusCode > 0){
//      String response = client.getString();
//      Serial.println("\nStatuscode:" + String(statusCode));
//      Serial.println(response);
      client.end(); // Closes the connection!
    }else{
//      Serial.println("\nStatuscode:" + String(statusCode));
    }
  }else{
//    Serial.println("Connection lost!");
  }
  
  delay(8000); // 10 second delay between each call!
}


void readSensor()
{
  humidity = dhtSensor.readHumidity();
  temperature = dhtSensor.readTemperature();
  pressure = bmpSensor.readPressure();
  if(gpsSensor.encode(ss.read())){
    if (gpsSensor.location.isValid())
    {
      latitude = gpsSensor.location.lat();
      longitude = gpsSensor.location.lng();
    }
    if (gpsSensor.date.isValid())
    {
      datetime.Y = gpsSensor.date.year();
      datetime.M = gpsSensor.date.month();
      datetime.D = gpsSensor.date.day();
    }
    if (gpsSensor.time.isValid())
    {
      datetime.h = gpsSensor.time.hour();
      datetime.m = gpsSensor.time.minute();
      datetime.s = gpsSensor.time.second();
    }
  }
//  if(Serial){
//    Serial.println();
//    Serial.print("Humidity: ");
//    Serial.print(humidity);
//    Serial.println(" %");
//    Serial.print("Temperature: ");
//    Serial.print(temperature);
//    Serial.println(" *C");
//    Serial.print("Pressure: ");
//    Serial.print(pressure);
//    Serial.println(" Pa");
//    Serial.print("Latitude: ");
//    Serial.println(latitude);
//    Serial.print("Longitude: ");
//    Serial.println(longitude);
//    Serial.print("Time: ");
//    Serial.print(datetime.Y);
//    Serial.print("/");
//    Serial.print(datetime.M);
//    Serial.print("/");
//    Serial.print(datetime.D);
//    Serial.print("T");
//    Serial.print(datetime.h);
//    Serial.print(":");
//    Serial.print(datetime.m);
//    Serial.print(":");
//    Serial.print(datetime.s);
//    Serial.println();
//  }
}
