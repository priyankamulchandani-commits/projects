#!/bin/bash

echo "Testing servers..."

# Test frontend
echo "Testing frontend on port 3000..."
if curl -s http://localhost:3000 > /dev/null; then
    echo "✅ Frontend is running on http://localhost:3000"
else
    echo "❌ Frontend is not accessible"
fi

# Test backend
echo "Testing backend on port 5000..."
if curl -s http://localhost:5000/api/auth/profile > /dev/null 2>&1; then
    echo "✅ Backend is running on http://localhost:5000"
else
    echo "❌ Backend is not accessible (this is expected without authentication)"
    # Try a simple health check
    if curl -s -f http://localhost:5000/swagger > /dev/null 2>&1 || curl -s -f http://localhost:5000 > /dev/null 2>&1; then
        echo "✅ Backend server is responding"
    else
        echo "❌ Backend server is not responding"
    fi
fi

echo ""
echo "Application URLs:"
echo "Frontend: http://localhost:3000"
echo "Backend API: http://localhost:5000"
echo ""
echo "To start the application:"
echo "1. Backend: cd backend/LoginUserAPI && dotnet run --urls='http://localhost:5000'"
echo "2. Frontend: cd frontend && npm start"