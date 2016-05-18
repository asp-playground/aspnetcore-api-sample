
## ติดตั้ง

ดาวโหลด .NET Core เวอร์ชั่นล่าสุด - https://github.com/dotnet/cli

1. .NET Core Installer
2. .NET Core SDK Installer

## Restore & Run

- cd เข้าไปใน `src\SampleApi`
- พิมพ์ `dotnet restore`
- พิมพ์ `dotnet run`
- เปิดเพจ `http://localhost:5000/contacts`

## โครงสร้างโปรเจค

```
├── SampleApi.sln
├── global.json
└── src
    └── SampleApi
        ├── Controllers
        │   ├── ContactsController.cs
        │   ├── CsvOnlyController.cs
        │   └── ItemsController.cs
        ├── Filters
        │   └── ContactSelfLinkFilter.cs
        ├── Formatters
        │   └── CsvMediaTypeFormatter.cs
        ├── LinkProvider.cs
        ├── Middleware
        │   └── TimerMiddleware.cs
        ├── Models
        │   ├── Contact.cs
        │   ├── ErrorData.cs
        │   ├── IContactRepository.cs
        │   └── InMemoryContactRepository.cs
        ├── ObjectResultExtensions.cs
        ├── Program.cs
        ├── Properties
        │   └── launchSettings.json
        ├── SampleApi.xproj
        ├── Startup.cs
        ├── appsettings.json
        ├── project.json
        ├── project.lock.json
        └── web.config
```

## Api ที่เกี่ยวข้อง

- ActionFilterAttribute - https://damienbod.com/2015/09/15/asp-net-5-action-filters
- ProducesAttribute - https://blogs.msdn.microsoft.com/webdev/2014/11/24/content-negotiation-in-mvc-6-or-how-can-i-just-write-json
- FormatFilter, ServiceFilter - http://www.strathweb.com


## NDC Oslo 2015 (upgrade to RC2)

Updated version of [NDc Oslo 2015 code samples](https://github.com/filipw/ndcoslo2015-demos).

Originally built as a migration project from ASP.NET Web API => ASP.NET 5 beta6. In this repo, upgraded to ASP.NET Core RC2.
