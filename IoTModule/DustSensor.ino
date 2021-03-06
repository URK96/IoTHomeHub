///////////////////////////////////////////////////////////////////////////////////////
//                                      Library                                      //
///////////////////////////////////////////////////////////////////////////////////////

#include <BTCommand.h> // BT 관련 상수값 헤더파일
#include <SoftwareSerial.h> // BT Serial을 위한 라이브러리

///////////////////////////////////////////////////////////////////////////////////////
//                                 Pin Setting                                       //
///////////////////////////////////////////////////////////////////////////////////////

int dustout = A0; // 미세먼지 센서 V0 핀
int v_led=5; // 미세먼지 센서 V_LED 핀

int FanCommon= 7;  // Fan(Temp, Dust) 공용 핀
int FanDust = 6;    // 미세먼지 조절 Fan 핀
int FanStatus = 0; // Fan 동작 상태
int AutoMode = 1;

SoftwareSerial btSerial(2, 3);

///////////////////////////////////////////////////////////////////////////////////////
//                                 Initial Value                                     //
///////////////////////////////////////////////////////////////////////////////////////

//////////////////////////// <미세먼지 파트> ////////////////////////////
// 미세 먼지 센서 초기값
#define no_dust 0.1 // 미세 먼지 없을 때 초기 V 값 0.35 / 공기청정기 위 등에서 먼지를 가라앉힌 후 voltage값 개별적으로 측정 필요

// 센서로 읽은 값 변수 선언
float vo_value=0;

// 센서로 읽은 값을 전압으로 측정 변수
float sensor_voltage=0;

// 실제 미세 먼지 밀도 변수
float dust_density=0;
int dust_level = 0;

// 기준으로 삼을 미세먼지 값
int dust_tmp=80; // 미세먼지 농도 80㎍이상이면 나쁨 단계

///////////////////////////////////////////////////////////////////////////////////////
//                                 Setup Part                                        //
///////////////////////////////////////////////////////////////////////////////////////

void setup()
{
  Serial.begin(9600); //PC모니터를 이용하기 위하여, 시리얼통신을 정의해줍니다.
  btSerial.begin(9600);
  pinMode(v_led,OUTPUT); // 적외선 led 출력으로 설정(미세먼지센서)

  pinMode(FanCommon, OUTPUT); // Fan 공용핀(Temp, Dust)을 출력으로 설정
  pinMode(FanDust, OUTPUT); // Fan 공용핀(Temp, Dust)을 출력으로 설정

  digitalWrite(FanDust,HIGH);

  btSerial.listen();
}

///////////////////////////////////////////////////////////////////////////////////////
//                                  Loop Part                                        //
///////////////////////////////////////////////////////////////////////////////////////

void loop() 
{

///////////////////////////// Dust Sensor Measurement ///////////////////////////
 
 digitalWrite(v_led,LOW); // 적외선 LED ON
 delayMicroseconds(280); // 280us동안 딜레이
 vo_value=analogRead(dustout); // 데이터를 읽음
 delayMicroseconds(40); // 320us - 280us
 digitalWrite(v_led,HIGH); // 적외선 LED OFF
 delayMicroseconds(9680); // 10ms(주기) -320us(펄스 폭) 한 값

 sensor_voltage=get_voltage(vo_value);
 dust_density=get_dust_density(sensor_voltage);

 float sum = 0;

 for (int i = 0; i < 10; ++i)
 {
   digitalWrite(v_led,LOW); // 적외선 LED ON
   delayMicroseconds(280); // 280us동안 딜레이
   vo_value=analogRead(dustout); // 데이터를 읽음
   delayMicroseconds(40); // 320us - 280us
   digitalWrite(v_led,HIGH); // 적외선 LED OFF
   delayMicroseconds(9680); // 10ms(주기) -320us(펄스 폭) 한 값

   sensor_voltage=get_voltage(vo_value);
   sum+=get_dust_density(sensor_voltage);
 }

 dust_density = sum / 10;

 if(dust_density<0)
 {
  //dust_density=0;
  dust_level = 0;
 }
 else if (dust_density <= 30)
 {
  dust_level = 0;
 }
 else if (dust_density <= 80)
 {
  dust_level = 1;
 }
 else if (dust_density <= 150)
 {
  dust_level = 2;
 }
 else
 {
  dust_level = 3;
 }

 delay(1000);

////////////////////////////// Fan Setting //////////////////////////////////////
  digitalWrite(FanCommon,HIGH); // Fan 공용 핀은 계속 HIGH 상태 출력(동작)

  delay(10);

///////////////////////// DustFan Running Part////////////////////////////////////////////

if (AutoMode)
{
  if(dust_density>=dust_tmp)
  {
      digitalWrite(FanDust,LOW); // Fan 미세먼지 제거 위해 동작
      FanStatus = 1;

      delay(10);
  }
  else
  {
    digitalWrite(FanDust,HIGH); // Fan 정지
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
      btSerial.println(TYPE_DUST);
    }
    else if (command.equals(REQUEST_DATA))
    {
      String sep = ";";
      String data = dust_level + sep + dust_density + sep + FanStatus;

      btSerial.println(data);
    }
    else if (command.indexOf(COMMAND_FAN_TOGGLE) > 0)
    {
      if (FanStatus)
      {
        digitalWrite(FanDust,HIGH);
        FanStatus = 0;
        AutoMode = 1;
      }
      else
      {
        digitalWrite(FanDust,LOW);
        FanStatus = 1;
        AutoMode = 0;
      }
    }
  }

////////////////////////////////////////////////////////////////////////////////////////////
//                                          Serial Monitor                                //
////////////////////////////////////////////////////////////////////////////////////////////

// 시리얼 모니터 함수 (미세먼지 확인용)
  Serial.print("value = ");
  Serial.println(vo_value);
  Serial.print("Voltage = ");
  Serial.print(sensor_voltage);
  Serial.println(" [V]");
  Serial.print("Dust Density = ");
  Serial.print(dust_density);
  Serial.println(" [ug/m^3]");
  delay(10);
 
}


/////////////////////////////////////////////////////////////////////////////////////////////
//                       Particularmatter Measurement Function Part            //
////////////////////////////////////////////////////////////////////////////////////////////

 float get_voltage(float value)
{
 // 아날로그 값을 전압 값으로 바꿈
 float V = value * (5.0 / 1024); 
 return V;
}

float get_dust_density(float voltage)
{
 // 데이터 시트에 있는 미세 먼지 농도(ug) 공식 기준
 float dust = (voltage - no_dust) / 0.005;
 return dust;
}
////////////////////////////////////////////////////////////////////////////////////////////
