///////////////////////////////////////////////////////////////////////////////////////
//                                      Library                                      //
///////////////////////////////////////////////////////////////////////////////////////

#include <BTCommand.h> // BT 관련 상수값 헤더파일
#include <SoftwareSerial.h> // BT Serial을 위한 라이브러리

///////////////////////////////////////////////////////////////////////////////////////
//                                 Pin Setting                                       //
///////////////////////////////////////////////////////////////////////////////////////

int LightSensor_Pin = A0; // 조도센서(CDS) 핀
int LED = 4;        // LED 핀 
int LEDBUTTON = 5; // LED 제어 버튼 핀

SoftwareSerial btSerial(2, 3);

///////////////////////////////////////////////////////////////////////////////////////
//                                 Initial Value                                     //
///////////////////////////////////////////////////////////////////////////////////////

////////////////////////////// <LED 파트> /////////////////////////////

int LEDStatus = 0;
int AutoLED = 1;
int State1 = 0; // LED 버튼 상태 변환 변수
int LEDBUTTONState; // LED 버튼 상태 변수
int Count1 = 0; // LED 버튼 눌린 횟수 카운터

///////////////////////////////////////////////////////////////////////////////////////
//                                 Setup Part                                        //
///////////////////////////////////////////////////////////////////////////////////////

void setup()
{
  Serial.begin(9600); //PC모니터를 이용하기 위하여, 시리얼통신을 정의해줍니다..
  btSerial.begin(9600);

  pinMode(LED,OUTPUT); // LED를 출력으로 설정
  pinMode(LEDBUTTON,INPUT_PULLUP); // LED ON/OFF 버튼을 입력으로 설정

  btSerial.listen();
}


///////////////////////////////////////////////////////////////////////////////////////
//                                  Loop Part                                        //
///////////////////////////////////////////////////////////////////////////////////////

void loop() 
{
/////////////////////////// LED Setting //////////////////////

  LEDBUTTONState = digitalRead(LEDBUTTON); // 현재 LED 버튼 상태 입력
  
  int Light=analogRead(LightSensor_Pin); // 현재 조도 상태 입력

  delay(10);


///////////////////////////////// LED Button Part /////////////////////////////////

  if(LEDBUTTONState == HIGH){ // LED버튼이 눌리지 않았을때
    if(State1==1)
    { // 상태가 0 이었다면
     delay(10);
     State1=0;     // 상태를 1로 바꿈
    }
  }
  if(LEDBUTTONState == LOW){ // LED버튼이 눌리면
    if(State1==1)
    { // 상태가 1이라면
     delay(10);   
     //State1=0;     // 상태를 0으로 바꿈
    }
  }

  if (State1)
  {
    if(LEDStatus)
    {
        digitalWrite(LED, LOW); // LED 버튼이 짝수번 눌리면 OFF
        AutoLED = 1;
        LEDStatus = 0;
    }
    else
    {
        digitalWrite(LED, HIGH); // LED 버튼이 홀수번 눌리면 ON
        AutoLED = 0;
        LEDStatus = 1;
    }

    State1 = 0;
  }

  if (AutoLED)
  {
    if (Light < 150)
    {
      digitalWrite(LED, HIGH);
      LEDStatus = 1;
    }
    else
    {
      digitalWrite(LED, LOW);
      LEDStatus = 0;
    }
  }
//////////////////////// BT Communication Part /////////////////////////////

  while (btSerial.available())
  {
    String command = btSerial.readString();

    command.trim();

    Serial.println(command);

    if (command.equals(CHECK_RESPONSE))
    {
      btSerial.println(SEND_RESPONSE);
    }
    else if (command.equals(REQUEST_TYPE))
    {
      btSerial.println(TYPE_LIGHT);
    }
    else if (command.equals(REQUEST_DATA))
    {
      String sep = ";";
      String data = Light + sep + LEDStatus;

      btSerial.println(data);
    }
    else if (command.indexOf("LED") > 0)
    {
      Serial.println("command input");
      
      if (LEDStatus)
      {
        digitalWrite(LED, LOW); // LED 버튼이 짝수번 눌리면 OFF
        AutoLED = 1;
        LEDStatus = 0;
      }
      else
      {
        digitalWrite(LED, HIGH); // LED 버튼이 짝수번 눌리면 ON
        AutoLED = 0;
        LEDStatus = 1;
      }
    }
  }

//////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////////////////
//                                          Serial Monitor                                //
////////////////////////////////////////////////////////////////////////////////////////////

  //시리얼 모니터 함수 (조도 확인용)
  //Serial.print("Light: ");
  //Serial.println(Light);
}
