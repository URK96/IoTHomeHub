///////////////////////////////////////////////////////////////////////////////////////
//                                      Library                                      //
///////////////////////////////////////////////////////////////////////////////////////

#include <MQ135.h>
#include <BTCommand.h> // BT 관련 상수값 헤더파일
#include <SoftwareSerial.h> // BT Serial을 위한 라이브러리

///////////////////////////////////////////////////////////////////////////////////////
//                                 Pin Setting                                       //
///////////////////////////////////////////////////////////////////////////////////////

#define ANALOGPIN A0 //gas sensor pin
#define LEDGREEN 6  //green led pin
#define LEDYELLOW 5 //yellow led pin
#define LEDRED 7    //red led pin

MQ135 gasSensor = MQ135(ANALOGPIN); //library function
SoftwareSerial btSerial(2, 3);

///////////////////////////////////////////////////////////////////////////////////////
//                                 Init Value                                        //
///////////////////////////////////////////////////////////////////////////////////////

int level = 0;

///////////////////////////////////////////////////////////////////////////////////////
//                                 Setup Part                                        //
///////////////////////////////////////////////////////////////////////////////////////

void setup() {
  Serial.begin(9600);           //serial bit rate
  btSerial.begin(9600);
  
  pinMode(LEDGREEN, OUTPUT);  
  pinMode(LEDYELLOW, OUTPUT); 
  pinMode(LEDRED, OUTPUT);   

  btSerial.listen();
}

///////////////////////////////////////////////////////////////////////////////////////
//                                  Loop Part                                        //
///////////////////////////////////////////////////////////////////////////////////////

void loop() {
  int ppm = gasSensor.getPPM();   //측정된 CO2 ppm값
  Serial.println(ppm, DEC);       //serial monitor에 ppm 갱신
    //450ppm미만의 경우 Good : GREEN
    if( ppm < 450) {              
    digitalWrite(LEDGREEN,HIGH);
    digitalWrite(LEDYELLOW,LOW);
    digitalWrite(LEDRED,LOW);
    Serial.print("Good.\n");
    level = 0;
    }
    //450 ~ 1000ppm 경우 SoSo : YEllOW
    else if( ppm >= 450 && ppm < 1000 ) {
    digitalWrite(LEDGREEN,LOW);
    digitalWrite(LEDYELLOW,HIGH);
    digitalWrite(LEDRED,LOW);
    Serial.print("SoSo. But,Ventilation is recommended.\n"); 
    level = 1;
    }
    //1000 ~ 3000ppm 경우 환기 필요 : YEllOW
    else if( ppm >= 1000 && ppm < 3000 ) {
    digitalWrite(LEDGREEN,LOW);
    digitalWrite(LEDYELLOW,HIGH);
    digitalWrite(LEDRED,LOW);
    Serial.print("Need ventilation.\n");
    level = 2;
    }
    //3000ppm 이상의 경우 집 내부 확인 필요 : RED
    else if( ppm >= 3000) {
    digitalWrite(LEDGREEN,LOW);
    digitalWrite(LEDYELLOW,LOW);
    digitalWrite(LEDRED,HIGH);
    Serial.print("Check the house.\n");
    level = 3;
    }

  //////////////////////// BT Communication Part /////////////////////////////

  while (btSerial.available())
  {
    String command = btSerial.readString();

    command.trim();

    if (command.equals(CHECK_RESPONSE))
    {
      btSerial.println(SEND_RESPONSE);
    }
    else if (command.equals(REQUEST_TYPE))
    {
      btSerial.println(TYPE_GAS);
    }
    else if (command.equals(REQUEST_DATA))
    {
      String sep = ";";
      String data = level + sep + ppm;

      btSerial.println(data);
    }
  }

    
  delay(200);             // 100ms마다 ppm과 출력 갱신
}
