# İçerik Yönetim Sistemi (CMS) – .NET Core Projesi

Bu proje, kullanıcıların içerikleriyle etkileşim kurabileceği, içerik varyantlarını görebileceği, kategorilere ve dillere göre filtreleme yapabileceği bir içerik yönetim sistemidir. SOLID prensiplerine uygun, katmanlı mimari ile geliştirilmiştir.

## 🚀 Kullanılan Teknolojiler

- ASP.NET Core Web API (.NET 9)
- Entity Framework Core (SQL Server)
- Mapster (DTO ↔ Entity dönüşümleri)
- IMemoryCache (Caching)
- Swagger (API dokümantasyonu)
- Katmanlı Mimari (Domain, Application, Infrastructure, WebAPI)

## 🧱 Proje Mimarisi

CMS.sln
│
├── CMS.Domain → Entity tanımları (User, Content, Category, Variant)
├── CMS.Application → DTO’lar, servis arayüzleri, Mapster config
├── CMS.Infrastructure → DbContext, repository ve servis implementasyonları
├── CMS.WebAPI → API katmanı ve controller'lar


## 📦 Başlangıç

1. Gerekli NuGet paketlerini yükleyin:
   - `Microsoft.EntityFrameworkCore`
   - `Microsoft.EntityFrameworkCore.SqlServer`
   - `Microsoft.EntityFrameworkCore.Tools`
   - `Microsoft.EntityFrameworkCore.Design`
   - `Mapster`
   - `Swashbuckle.AspNetCore`

2. Migration oluşturun ve veritabanını hazırlayın:

```bash
Add-Migration InitialCreate
Update-Database

3. Projeyi çalıştırın:

dotnet run --project CMS.WebAPI

4. Swagger UI üzerinden test edin:
http://localhost:5000/swagger

Ana Özellikler
Kullanıcı oluşturma ve içerik ilişkilendirme

İçerik oluşturma (varyantlarıyla birlikte)

Kullanıcıya özel varyant sunumu (stateful)

Kategori bazlı içerik filtreleme

15 dakikalık cache sistemi

Swagger üzerinden test edilebilir API'ler

API Örnekleri

POST /api/user

{
  "fullName": "Ali Veli",
  "email": "ali@example.com"
}

 POST /api/content

 {
  "title": "Yapay Zeka",
  "description": "AI içerikleri",
  "language": "tr",
  "imageUrl": "https://example.com/image.jpg",
  "categoryId": "KATEGORI_ID",
  "userId": "KULLANICI_ID",
  "variants": ["AI nedir?", "Nasıl çalışır?"]
}


Geliştirici Notları
AppDbContext üzerinden veri işlemleri yapılır.

DTO dönüşümleri Mapster ile konfigure edilmiştir.

Repository Pattern uygulanmıştır.

Caching mekanizması IMemoryCache ile sağlanmıştır.

Frontent tarafı	react ile geliştirildi.
react-cms-frontend klasörü altında yer alıyor.

