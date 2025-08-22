# JWT Authentication Setup for CMS API

This document provides step-by-step instructions for implementing JWT-based authentication in your ASP.NET Core Web API.

## 🔧 **Prerequisites**

- ASP.NET Core 6+ project
- Entity Framework Core
- SQL Server or LocalDB

## 📦 **NuGet Packages Added**

The following packages have been added to `CMS.WebAPI.csproj`:

```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.6" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.4.0" />
```

## 🚀 **Setup Instructions**

### 1. **Database Migration**

Run the following commands to update your database schema:

```bash
# Add migration for User entity changes
dotnet ef migrations add AddUserAuthenticationFields --project CMS.Infrastructure --startup-project CMS.WebAPI

# Update database
dotnet ef database update --project CMS.Infrastructure --startup-project CMS.WebAPI
```

### 2. **Configuration**

The JWT settings are configured in `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyHere12345678901234567890",
    "Issuer": "CMS_API",
    "Audience": "CMS_Users",
    "ExpirationHours": "24"
  }
}
```

**⚠️ Important:** Change the `SecretKey` in production to a secure, randomly generated key.

### 3. **Default User**

A default user is automatically created when the application starts:

- **Username:** `admin`
- **Password:** `admin123`
- **Email:** `admin@cms.com`

## 🔐 **API Endpoints**

### **Public Endpoints (No Authentication Required)**
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `POST /api/User` - Create new user
- `GET /api/Content/{contentId}/user/{userId}` - Get content for user

### **Protected Endpoints (JWT Token Required)**
- `GET /api/Content/filter` - Get filtered contents
- `POST /api/Content` - Create new content
- `POST /api/Content/category` - Create content category
- `GET /api/User/{userId}/contents` - Get user's contents

## 🔑 **Authentication Flow**

### 1. **Login**
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "fullName": "Admin User"
}
```

### 2. **Using JWT Token**
Include the token in the Authorization header for protected endpoints:

```http
GET /api/Content/filter
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 🛠️ **Swagger Integration**

The Swagger UI now includes:

1. **Authorize Button** - Click to enter JWT token
2. **Padlock Icons** - Indicates protected endpoints
3. **Bearer Token Support** - Automatic token inclusion in requests

### **Using Swagger with JWT:**

1. Open Swagger UI at `/swagger`
2. Click the **"Authorize"** button
3. Enter your JWT token (without "Bearer " prefix)
4. Click **"Authorize"**
5. All protected endpoints will now include the token automatically

## 🔧 **CORS Configuration**

CORS is configured to allow requests from your React frontend:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});
```

## 🧪 **Testing Authentication**

### **Option 1: Using the Test Scripts**

I've created test scripts to help you verify the authentication:

**PowerShell (Windows):**
```powershell
.\test-auth.ps1
```

**Bash (Linux/Mac):**
```bash
chmod +x test-auth.sh
./test-auth.sh
```

### **Option 2: Manual Testing**

1. **Register New User:**
```bash
curl -X POST "https://localhost:7000/api/auth/register" \
     -H "Content-Type: application/json" \
     -d '{
       "fullName": "Test User",
       "email": "test@example.com",
       "username": "testuser",
       "password": "test123"
     }'
```

2. **Login:**
```bash
curl -X POST "https://localhost:7000/api/auth/login" \
     -H "Content-Type: application/json" \
     -d '{"username":"testuser","password":"test123"}'
```

3. **Use the token:**
```bash
curl -X GET "https://localhost:7000/api/Content/filter" \
     -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"
```

### **Option 3: Using Swagger UI**

1. Open `https://localhost:7000/swagger`
2. First, call `POST /api/auth/register` to create a new user
3. Then call `POST /api/auth/login` with the credentials
4. Copy the token and click "Authorize" in Swagger
5. Test protected endpoints

### **Using the React Frontend:**

The React frontend is already configured to:
- Store JWT tokens in localStorage
- Automatically include tokens in API requests
- Handle 401 responses by redirecting to login

## 🔒 **Security Considerations**

1. **Secret Key:** Use a strong, randomly generated secret key in production
2. **Token Expiration:** Configure appropriate token expiration times
3. **HTTPS:** Always use HTTPS in production
4. **Password Hashing:** Passwords are hashed using SHA256 (consider using bcrypt for production)
5. **Token Storage:** Store tokens securely on the client side

## 🚨 **Production Checklist**

- [ ] Change JWT secret key
- [ ] Configure HTTPS
- [ ] Set appropriate token expiration
- [ ] Implement password reset functionality
- [ ] Add rate limiting
- [ ] Configure proper CORS origins
- [ ] Set up logging for authentication events
- [ ] Implement refresh token mechanism (if needed)

## 📝 **Troubleshooting**

### **Common Issues:**

1. **401 Unauthorized:**
   - Check if the token is valid and not expired
   - Verify the token format: `Bearer <token>`
   - Ensure the token was generated with the correct secret key

2. **CORS Errors:**
   - Verify the frontend URL is included in CORS policy
   - Check if `AllowCredentials()` is configured

3. **Database Issues:**
   - Run database migrations
   - Check connection string
   - Verify User table has the new authentication fields

4. **Login Fails with Default Credentials:**
   - The default user might not exist in the database
   - Use the register endpoint to create a new user: `POST /api/auth/register`
   - Or manually create a user using the `POST /api/User` endpoint with the new `UserCreateDto`

5. **Port Issues:**
   - The API might be running on a different port (check the console output)
   - Update the port in test scripts if needed
   - Default ports are usually 7000 (HTTPS) and 5000 (HTTP)

### **Debug Information:**

Enable detailed logging in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore.Authentication": "Debug"
    }
  }
}
```

## 📚 **Additional Resources**

- [JWT Authentication in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn)
- [Swagger with JWT](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle)
- [CORS in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/cors)
