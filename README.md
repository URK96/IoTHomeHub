# IoTHomeHub
### Home Hub for IoT Modules  
### 인하대학교 전자공학과 전자공학종합설계 프로젝트   
본 프로젝트는 전자공학종합설계 과목에서 진행된 자유주제 프로젝트입니다   

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

## Framework & Platform
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
  * iOS

* IoT Module
  * Arduino Nano Compatible SBC [Product Detail](https://eduino.kr/product/detail.html?product_no=130&cate_no=134&display_group=1)
  * Arduino Uno R3 Compatible SBC [Product Detail](https://eduino.kr/product/detail.html?product_no=59&cate_no=134&display_group=1)
  * EBT-50 BT Module (Nordic nRF52810 Chipset) [Product Detail](https://eduino.kr/product/detail.html?product_no=618)
  
## Project Framework

<img src="https://github.com/URK96/IoTHomeHub/blob/master/GitImages/ProjectFramework.png" width="70%" height="30%" title="IoTHomeHub Project Framework"></img>
