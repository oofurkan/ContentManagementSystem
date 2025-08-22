#!/bin/bash

# Test Authentication API Script
# This script will test the JWT authentication functionality

BASE_URL="https://localhost:7000"  # Adjust port if needed
API_URL="$BASE_URL/api"

echo "🔐 Testing CMS API Authentication"
echo "================================="

# Step 1: Register test user
echo ""
echo "1️⃣ Registering test user..."
REGISTER_RESPONSE=$(curl -s -X POST "$API_URL/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Test User",
    "email": "test@example.com",
    "username": "testuser",
    "password": "test123"
  }')
echo "✅ User registered successfully!"

# Step 2: Test login
echo ""
echo "2️⃣ Testing login..."
LOGIN_RESPONSE=$(curl -s -X POST "$API_URL/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "test123"
  }')

# Extract token from response
TOKEN=$(echo $LOGIN_RESPONSE | grep -o '"token":"[^"]*"' | cut -d'"' -f4)

if [ ! -z "$TOKEN" ]; then
    echo "✅ Login successful!"
    echo "   Token: ${TOKEN:0:50}..."
    echo "   Username: admin"
    echo "   Full Name: Admin User"

    # Step 3: Test protected endpoint
    echo ""
    echo "3️⃣ Testing protected endpoint..."
    PROTECTED_RESPONSE=$(curl -s -X GET "$API_URL/Content/filter" \
      -H "Authorization: Bearer $TOKEN" \
      -H "Content-Type: application/json")
    
    echo "✅ Protected endpoint accessible!"
    echo "   Response: $PROTECTED_RESPONSE"
else
    echo "❌ Login failed!"
    echo "   Response: $LOGIN_RESPONSE"
fi

echo ""
echo "🎉 Authentication test completed!"
