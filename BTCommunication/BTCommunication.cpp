#include "BTCommunication.h"

void InitializeBT()
{
    btSerial.begin(9600);
}

void SendResponse(String response)
{
    btSerial.write(response + "\r\n");
}

String GetRequest()
{
    if (btSerial.available())
    {
        return btSerial.readString();
    }
}