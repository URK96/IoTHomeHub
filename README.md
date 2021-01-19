# IoTHomeHub
### Home Hub Framework for IoT Modules  
### 인하대학교 전자공학과 전자공학종합설계 프로젝트   
본 프로젝트는 전자공학종합설계 과목에서 진행된 자유주제 프로젝트입니다   

## Project Objective
다양한 IoT 기기들의 관리 및 모니터링을 하나의 기기로 할 수 있게 하며 외부 접속에 대한 기능을 개별 IoT 기기 대신 통합적으로 제공하는 하나의 Framework를 제작하는 것을 목표로 함

## Contributor  
* 최진혁 (URK96)
  * Hub Device Framework, Layout 제작
  * Hub Device Server 제작
  * Bluetooth Communication 제작
  * Hub Mobile App 제작
  * Simple DB Interface 제작
  
* 전덕원 (DeokWon1213)
  * Hub Device Layout 제작
  * IoT Module 제작
  * Test, Feedback 담당
  
* 이정인 (fpdlxms95)
  * IoT Module 제작
  * IoT Module 외형 제작
  * Test, Feedback 담당
  
* 김환웅
  * IoT Module 제작
  * IoT Module 외형 제작
  * Test, Feedback 담당

## Programming Languages
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
  * .NET Standard 2.0 [Github](https://github.com/dotnet/standard ".NET Standard Github")
  * System.Data.DataSetExtensions (for LINQ Extension) [Github](https://github.com/microsoft/referencesource/tree/master/System.Data.DataSetExtensions "System.Data.DataSetExtensions Github")

* Hub Device Program
  * .NET Core 3.1 [Github](https://github.com/dotnet/core ".NET Core Github")
  * Avalonia UI Framework [Github](https://github.com/AvaloniaUI/Avalonia "Avalonia Framework Github")
  * DotNet-BlueZ (Download on NuGet) [Github](https://github.com/hashtagchris/DotNet-BlueZ "DotNet-BlueZ Github")
  * OpenWeather API [Github](https://github.com/swiftyspiffy/OpenWeatherMap-API-CSharp "3rd-Party OpenWeather C# API Github")

* Hub Device Server
  * ASP.NET Core 3.1 (use Web API Template) [Github](https://github.com/dotnet/aspnetcore "ASP.NET Core Github")

* Mobile App
  * Xamarin.Forms [Github](https://github.com/xamarin/Xamarin.Forms "Xamarin.Forms Github")
  
* IoT Module
  * Arduino [Github](https://github.com/arduino/Arduino "Arduino Github")
  
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
  
## Project Usage
본 Project를 동작시키기 위해서는 환경을 구성해야 합니다. 자세한 내용은 Wiki 항목의 Usage 카테고리 내의 페이지를 참조하세요.   
[Wiki 바로가기](https://github.com/URK96/IoTHomeHub/wiki "Wiki")
