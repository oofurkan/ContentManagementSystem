# Test Authentication API Script
# This script will test the JWT authentication functionality

$baseUrl = "https://localhost:7000"  # Adjust port if needed
$apiUrl = "$baseUrl/api"

Write-Host "🔐 Testing CMS API Authentication" -ForegroundColor Green
Write-Host "=================================" -ForegroundColor Green

# Step 1: Register test user
Write-Host "`n1️⃣ Registering test user..." -ForegroundColor Yellow
try {
    $registerData = @{
        fullName = "Test User"
        email = "test@example.com"
        username = "testuser"
        password = "test123"
    } | ConvertTo-Json

    $registerResponse = Invoke-RestMethod -Uri "$apiUrl/auth/register" -Method POST -Body $registerData -ContentType "application/json"
    Write-Host "✅ User registered successfully!" -ForegroundColor Green
} catch {
    Write-Host "ℹ️  $($_.Exception.Message)" -ForegroundColor Cyan
}

# Step 2: Test login
Write-Host "`n2️⃣ Testing login..." -ForegroundColor Yellow
try {
    $loginData = @{
        username = "testuser"
        password = "test123"
    } | ConvertTo-Json

    $loginResponse = Invoke-RestMethod -Uri "$apiUrl/auth/login" -Method POST -Body $loginData -ContentType "application/json"
    Write-Host "✅ Login successful!" -ForegroundColor Green
    Write-Host "   Token: $($loginResponse.token.Substring(0, 50))..." -ForegroundColor Gray
    Write-Host "   Username: $($loginResponse.username)" -ForegroundColor Gray
    Write-Host "   Full Name: $($loginResponse.fullName)" -ForegroundColor Gray

    $token = $loginResponse.token

    # Step 3: Test protected endpoint
    Write-Host "`n3️⃣ Testing protected endpoint..." -ForegroundColor Yellow
    $headers = @{
        "Authorization" = "Bearer $token"
        "Content-Type" = "application/json"
    }

    $protectedResponse = Invoke-RestMethod -Uri "$apiUrl/Content/filter" -Method GET -Headers $headers
    Write-Host "✅ Protected endpoint accessible!" -ForegroundColor Green
    Write-Host "   Response: $($protectedResponse | ConvertTo-Json -Depth 1)" -ForegroundColor Gray

} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseBody = $reader.ReadToEnd()
        Write-Host "   Response: $responseBody" -ForegroundColor Red
    }
}

Write-Host "`n🎉 Authentication test completed!" -ForegroundColor Green
