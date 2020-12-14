///////////////////////////////////////////////////////////////////////////////////////
//                                      Library                                      //
///////////////////////////////////////////////////////////////////////////////////////

#include <DHT.h> //DHT-22센서 라이브러리
#include <BTCommand.h> // BT 관련 상수값 헤더파일
#include <SoftwareSerial.h> // BT Serial을 위한 라이브러리

///////////////////////////////////////////////////////////////////////////////////////
//                                 Pin Setting                                       //
///////////////////////////////////////////////////////////////////////////////////////

int DHT_Data_Pin = 5; // DHT 데이터 핀

int Buzzer_Pin = 4; // 부저 핀 넘버
int BuzzerBUTTON = 8; // 부저 제어 버튼 핀

int FanCommon= 6;  // Fan(Temp, Dust) 공용 핀
int FanTemp = 7;   // 온도 조절 Fan 핀
int FanStatus = 0; // Fan 상태 (0=Off, 1=On)
int AutoMode = 1;

DHT dht(DHT_Data_Pin, DHT22); // DHT22 데이터 8번핀

SoftwareSerial btSerial(2, 3); // Rx=D2, Tx=D3로 설정한 BT Serial

////////////////////////////////////////////////////////////////////////////////////////
//                                 Initial Value                                      //
////////////////////////////////////////////////////////////////////////////////////////

////////////////////////////// <부저파트> //////////////////////////////

int Buzzer_State = 0; // 부저 상태 초기값(ON - 화재 시 작동)

int State2 = 0; // 부저 버튼 상태 변환 변수
int BuzzerBUTTONState; // 부저 버튼 상태 변수
int Count2 = 0; // 부저 버튼 눌린 횟수 카운터

int Criteria_Temp = 27; // 부저 & Fan 작동 온도 (27도)

//////////////////////////////////////////////////////////////////////////////////////////
//                                 Setup Part                                           //
//////////////////////////////////////////////////////////////////////////////////////////

void setup()
{
  Serial.begin(9600); //시리얼통신을 정의
  btSerial.begin(9600);
  dht.begin(); //DHT22센서의 사용시작을 정의해줍니다.

  pinMode(Buzzer_Pin, OUTPUT); // 부저를 출력으로 설정
  
  pinMode(BuzzerBUTTON,INPUT); // Buzzer ON/OFF 버튼을 입력으로 설정

  pinMode(FanCommon, OUTPUT); // Fan 공용핀(Temp, Dust)을 출력으로 설정
  pinMode(FanTemp, OUTPUT); // Fan 공용핀(Temp, Dust)을 출력으로 설정  

  btSerial.listen();
}

////////////////////////////////////////////////////////////////////////////////////////////
//                                  Loop Part                                             //
////////////////////////////////////////////////////////////////////////////////////////////

void loop() 
{
///////////////////////////// Temp & Humi Measurement //////////////////////////

  delay(10);   //측정하는 시간사이에 딜레이
  float h = dht.readHumidity();  //습도값
  float t = dht.readTemperature(); //온도값
  float hic = dht.computeHeatIndex(t, h, false); //체감온도

////////////////////////////// Buzzer Button Part //////////////////////////////////////

  BuzzerBUTTONState = digitalRead(BuzzerBUTTON); // 현재 LED 버튼 상태 입력

   if(BuzzerBUTTONState == HIGH){ // 부저버튼이 눌리지 않았을때
    if(State2==0){ // 상태가 0 이었다면
     delay(10);
     State2=1;     // 상태를 1로 바꿈
     }
  }
  if(BuzzerBUTTONState == LOW){ // 부저버튼이 눌리면
    if(State2==1){ // 상태가 1이라면
     Count2 +=1;   // count 값 증가(눌렸음을 표시)
     delay(10);   
     State2=0;     // 상태를 0으로 바꿈
     }
  }

   if(Count2%2==0){
      Buzzer_State = 1;// 부저 버튼이 짝수번 눌리면 작동

        Serial.print("Buzzerbutton: ");
        Serial.println(Buzzer_State);
  }
  else{
      Buzzer_State = 0; // 부저 버튼이 홀수번 눌리면 복구

      Serial.print("Buzzerbutton: ");
        Serial.println(Buzzer_State);
  }

//////////////////////// Buzzer & TempFan Runnig Part /////////////////////////////
   digitalWrite(FanCommon,HIGH); // Fan 공용 핀은 계속 HIGH 상태 출력(동작)

  if (AutoMode)
  {
    if(t>=Criteria_Temp&&Buzzer_State==1)
    {
      tone(Buzzer_Pin, 493); // 부저 동작
 
      digitalWrite(FanTemp,LOW); // Fan 온도 낮추기 위해 동작
      FanStatus = 1;

      delay(10); 
    }
    else
    {
      noTone(Buzzer_Pin); 

      digitalWrite(FanTemp,HIGH); // Fan 정지
      FanStatus = 0;

      delay(10);  
    }
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
      btSerial.println(TYPE_HT);
    }
    else if (command.equals(REQUEST_DATA))
    {
      String sep = ";";
      String data = t + sep + h + sep + FanStatus;

      btSerial.println(data);
    }
    else if (command.indexOf(COMMAND_FAN_TOGGLE) > 0)
    {
      if (FanStatus)
      {
        digitalWrite(FanTemp, HIGH);
        FanStatus = 0;
        AutoMode = 1;
      }
      else
      {
        digitalWrite(FanTemp, LOW);
        FanStatus = 1;
        AutoMode = 0;
      }
    }
  }

////////////////////////////////////////////////////////////////////////////////////////////
//                                       Serial Monitor                                   //
////////////////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////
  
  // 시리얼 모니터 함수 (온습도 확인용)
  Serial.print("Humidity: ");
  Serial.print(h); //습도가 출력
  Serial.print(" %\t");
  Serial.print("Temperature: ");
  Serial.print(t); //온도가 출력
  Serial.print(" *C ");
  Serial.print("Heat index: ");
  Serial.print(hic); //체감온도 출력
  Serial.println(" *C ");
}
