#!/bin/bash

echo "🚀 Starting Login User Application..."

# Kill any existing processes
pkill -f dotnet 2>/dev/null || true
pkill -f "npm start" 2>/dev/null || true

echo "📦 Starting Backend Server..."
cd backend/LoginUserAPI
nohup dotnet run --urls="http://localhost:5000" > server.log 2>&1 &
BACKEND_PID=$!

echo "⏳ Waiting for backend to start..."
sleep 5

# Test if backend is running
if curl -s http://localhost:5000/api/auth/users > /dev/null; then
    echo "✅ Backend is running on http://localhost:5000"
else
    echo "❌ Backend failed to start"
    exit 1
fi

echo "🎨 Starting Frontend Server..."
cd ../../frontend
npm start &
FRONTEND_PID=$!

echo "📊 Application Status:"
echo "   Backend:  http://localhost:5000"
echo "   Frontend: http://localhost:3000"
echo "   API Test: http://localhost:5000/api/auth/users"
echo ""
echo "🔧 To stop the application:"
echo "   kill $BACKEND_PID $FRONTEND_PID"
echo ""
echo "📝 Server logs available at: backend/LoginUserAPI/server.log"

wait $FRONTEND_PID
