#include <SoftwareSerial.h>

SoftwareSerial btSerial(2, 3);

void InitializeBT();
void SendResponse(String response);
String GetRequest();