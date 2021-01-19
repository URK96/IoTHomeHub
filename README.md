# IoTHomeHub
### Home Hub Framework for IoT Modules  
### 인하대학교 전자공학과 전자공학종합설계 프로젝트   
본 프로젝트는 전자공학종합설계 과목에서 진행된 자유주제 프로젝트입니다   

## 프로젝트 목표
다양한 IoT 기기들의 관리 및 모니터링을 하나의 기기로 할 수 있게 하며 외부 접속에 대한 기능을 개별 IoT 기기 대신 통합적으로 제공하는 하나의 Framework를 제작하는 것을 목표로 함

## 참여 인원   
* 최진혁 (URK96)
  * Hub Device Framework, Layout 제작
  * Hub Device Server 제작
  * Bluetooth Communication 제작
  * Hub Mobile App 제작
  * Simple DB Interface 제작
  
* 전덕원
  * Hub Device Layout 제작
  * IoT Module 제작
  * Test, Feedback 담당
  
* 이정인
  * IoT Module 제작
  * IoT Module 외형 제작
  * Test, Feedback 담당
  
* 김환웅
  * IoT Module 제작
  * IoT Module 외형 제작
  * Test, Feedback 담당

## 사용된 언어
Hub Device, Server, Mobile : C#   
IoT Module : C, C++

## Develop & Run Environment
* Hub Device
  * Linux Mint 20 Cinammon (Linux Kernal 5.10.4) (On Develop & Test)
  * Raspberry OS (Linux Kernal 5.4.51) (On Run & Demo)
  
* Mobile
  * Android (API 23+)
  * iOS (On Simulator) (iOS 8+)
  * Windows 10 (UWP)

## Framework & Platform
* SmallDB
  * .NET Standard 2.0 [Github](https://github.com/dotnet/standard, ".NET Standard Github")
  * System.Data.DataSetExtensions (for LINQ Extension) [Github](https://github.com/microsoft/referencesource/tree/master/System.Data.DataSetExtensions, "System.Data.DataSetExtensions Github")

* Hub Device Program
  * .NET Core 3.1 [Github](https://github.com/dotnet/core, ".NET Core Github")
  * Avalonia UI Framework [Github](https://github.com/AvaloniaUI/Avalonia, "Avalonia Framework Github")
  * DotNet-BlueZ (by NuGet) [Github](https://github.com/hashtagchris/DotNet-BlueZ, "DotNet-BlueZ Github")
  * OpenWeather API [Github](https://github.com/swiftyspiffy/OpenWeatherMap-API-CSharp, "3rd-Party OpenWeather C# API Github")

* Hub Device Server
  * ASP.NET Core 3.1 (use Web API Template) [Github](https://github.com/dotnet/aspnetcore, "ASP.NET Core Github")

* Mobile App
  * Xamarin.Forms [Github](https://github.com/xamarin/Xamarin.Forms, "Xamarin.Forms Github")
  
* IoT Module
  * Arduino [Github](https://github.com/arduino/Arduino, "Arduino Github")
  
## H/W (with Testing)
* Hub Device
  * Raspberry Pi 4 (4GB)
  
* Mobile (Cross-Platform)
  * Android Devices
  * Windows 10 PC & Tablet

* IoT Module
  * Arduino Nano Compatible SBC [Product Detail](https://eduino.kr/product/detail.html?product_no=130&cate_no=134&display_group=1)
  * Arduino Uno R3 Compatible SBC [Product Detail](https://eduino.kr/product/detail.html?product_no=59&cate_no=134&display_group=1)
  * EBT-50 BT Module (Nordic nRF52810 Chipset) [Product Detail](https://eduino.kr/product/detail.html?product_no=618)
  
## Project Framework

<img src="https://github.com/URK96/IoTHomeHub/blob/master/GitImages/ProjectFramework.png" width="70%" height="30%" title="IoTHomeHub Project Framework"></img>


## Project Top Directory
* SmallDB
  * Project 전반에 사용되는 DB 수정 & 관리를 담당하는 Library (Project Reference로 각 프로젝트에서 참조됨)

* HubDevice
  * Hub Device에서 실행되는 프로그램 Source Code
  * Hub Device와 Hub Server로 나뉘어져 있음

* IoTMobileApp
  * Mobile Device에서 실행되는 Application Source Code
  * Cross-Platform 이므로 Windows, iOS, Android 지원
  
* IoTModule
  * 센서들과 부가기능이 포함되어 있는 IoT Sample Module Source Code
  * Project Framework 내에서 BT 통신 규격을 정의한 Header 파일도 포함
  
## Server Prepare Work
본 Project의 Server는 ASP.NET Core에 기본적으로 포함된 Kestrel 서버를 이용하지만 보안 구성을 위해 Reverse Proxy 구성을 Code에 적용하였으며 이를 위해 정상적으로 Server를 실행하려면 Apache 같은 중간 Server를 구성해야 합니다.   
Demo의 경우 Ubuntu 또는 Debian 기반의 Linux OS & Apache 서버를 기준으로 구성하였으며 하단 설명 부분도 해당 환경을 기준으로 합니다.   
그 외 서버나 Reverse Proxy 서버를 구성하지 않는 경우, [링크](https://docs.microsoft.com/ko-kr/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-5.0)를 참조해주세요.   

### 1. Apache 설치
아래 명령어를 입력해 Apache를 설치합니다. (apt 기준)   
```
sudo apt install apache2
```
Apache 설치 후에는 정상적인 활성화를 위해 재부팅을 권장합니다.

### 2. Hub Server 빌드 & 배포
Terminal에서 해당 프로젝트 폴더로 이동한 후 아래 명령어를 입력합니다.
```
dotnet clean
dotnet publish -c Release
```
해당 명령어 작업이 끝나면 IoTHubServer폴더 내부에 /bin/Release/netcoreapp3.1/publish 폴더가 생성되어 있습니다.
다른 위치에 폴더를 새로 만든 후 해당 경로의 내용물을 복사하시면 됩니다.

### 3. Apache 서버 구성
Apache 구성 경로인 /etc/apache2로 이동합니다. 해당 경로의 sites-available 폴더로 이동합니다.   
해당 경로에 iot.homehub.com.conf 파일을 아래 내용을 추가하여 생성합니다. 이 때, 파일 이름은 임의로 작성해도 무방하지만 코드 내용과 일관성을 위해 해당 파일 이름을 사용해 주세요.
```
<VirtualHost *:*>
    RequestHeader set "X-Forwarded-Proto" expr=%{REQUEST_SCHEME}
</VirtualHost>

<VirtualHost *:80>
    ProxyPreserveHost On
    ProxyPass / http://127.0.0.1:6002/
    ProxyPassReverse / http://127.0.0.1:6002/
    ServerName iot.homehub.com
    ServerAlias iot.homehub.com
    ErrorLog /home/urk96/logs/error-iot.homehub.com.log
    CustomLog /home/urk96/logs/access-iot.homehub.com.log common
</VirtualHost>
```
작성할 때, ProxyPass와 ProxyPassReverse의 포트 번호는 충돌하지 않는 범위 내에서 임의로 변경이 가능합니다. 단, 두 항목의 포트 번호는 동일해아 합니다.

### 4. Apache 모듈 활성화
Apache 설정 파일 및 Proxy Server 구성을 위해 Apache 모듈을 활성화해야 합니다. 아래 명령을 입력하여 활성화합니다.
```
sudo a2enmod headers proxy
```

### 5. Apache 서버 시작
Apache 설정이 모두 끝났으면 아래 명령어를 입력하여 구성 파일을 활성화하고 Apache 서버를 시작합니다.
```
sudo a2ensite iot.homehub.com.conf
sudo service apache2 restart
```

### 6. Kestrel 서버 구성
Linux System Service에 Kestrel 서버를 둥록하기 위해 /etc/systemd/system 경로로 이동합니다.   
해당 경로에 kestrel-iot.homehub.com.service 파일을 아래 내용을 포함하여 생성합니다.
```
[Unit]
Description=IoTHomeHub Web API

[Service]
WorkingDirectory=/home/urk96/HomeServer
ExecStart=/usr/bin/dotnet /home/urk96/HomeServer/IoTHubServer.dll --urls="http://127.0.0.1:6002"
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-example
User=urk96
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```
위 내용에서 Description 내용은 임의로 변경해도 무방합니다.   
WorkingDirectory는 Hub Server 배포 파일을 복사한 경로로 수정하셔야 정상적으로 동작합니다.   
ExecStart에는 .NET 설치 경로 (/usr/bin/dotnet)와 WorkingDirectory + IoTHubServer.dll 경로를 입력하시면 됩니다.   
또한 urls 인수에는 Apache 서버에서 설정한 Proxy 주소 & 포트를 입력해야 정상적으로 Reverse Proxy가 구성됩니다.   
User 항목은 Linux 계정 이름을 입력하시면 됩니다.

### 7. Kestrel 서버 등록 & 시작
Kestrel 구성 파일을 생성 후, 아래 명령어를 입력하여 Kestrel 서버를 Linux System Service에 등록하고 시작합니다.
```
sudo systemctl enable kestrel-iot.homehub.com.service
sudo systemctl start kestrel-iot.homehub.com.service
```
해당 명령어 실행 후 재부팅을 권장합니다.   
만약 서비스 상태를 보고 싶다면 아래 명령어를 입력하여 정보를 봅니다.
```
sudo systemctl status kestrel-iot.homehub.com.service
```
