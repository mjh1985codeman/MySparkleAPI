# 🎨 MySparkleHeart API  
**A secure, scalable ASP.NET Core Web API built for a local art-lessons business.**  
This API powers user registration, authentication, child enrollment, class scheduling, and more.  
Designed cleanly with Entity Framework Core, SQL Server, DTOs, and modern REST practices.


## 🚀 Tech Stack

**Backend:**  
- .NET 8 / ASP.NET Core Web API  
- Entity Framework Core  
- SQL Server  
- C# 12  

**Security:**  
- Password hashing (SHA256 → upgrading later to Identity)  
- JWT authentication (in progress)  
- Validation attributes  
- Clean DTO separation  

**Architecture:**  
- REST API Controllers  
- DTOs for request/response safety  
- EF Core Code-First migrations  

## 🧑‍🎨 Project Purpose

This API supports a real-world small business:

> **A local TN artist offering:**
> - group art classes  
> - private lessons  
> - commissioned paintings  
> - art prints  

Parents can create accounts, register their children for classes, and manage bookings.

## 🔐 Authentication 

- User Login endpoint  
- Password verification  
- JWT creation  
- JWT validation middleware  
- `[Authorize]` on protected routes  
- Only authenticated users can add children or enroll in classes  


## 🤝 Contributing

This project is actively developed by Michael Hodges as part of a real-world application.
External contributions are not currently open, but feedback is always welcome.

## 📜 License

This project is currently closed-source and proprietary to MySparkleHeart.

