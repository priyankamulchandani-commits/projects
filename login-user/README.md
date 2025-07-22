# Login User Application

A clean full-stack authentication application built with .NET 8 Web API backend and React TypeScript frontend. This application provides user authentication functionality including login, signup, and profile management with educational focus.

## Features

- **User Registration**: Create new user accounts with email, password, and personal information
- **User Login**: Authentication with plain text passwords (educational purposes)
- **Profile Management**: View and edit user profile information
- **Protected Routes**: Route protection based on authentication status
- **Context API**: React Context for global authentication state management
- **Modern UI**: Beautiful and responsive design with CSS animations
- **In-Memory Storage**: Data storage (resets on restart - perfect for learning)
- **Educational Focus**: Clean code structure for learning authentication concepts

## Technology Stack

### Backend (.NET 8 Web API)
- ASP.NET Core 8.0
- Entity Framework Core (In-Memory Database)
- Plain text authentication (educational)
- CORS middleware
- RESTful API design
- Swagger documentation

### Frontend (React TypeScript)
- React 18 with TypeScript
- React Router DOM for navigation
- Context API for state management
- Axios for HTTP requests
- Modern CSS with animations
- Responsive design

## Project Structure

```
login-user/
├── backend/
│   └── LoginUserAPI/
│       ├── Controllers/
│       │   └── AuthController.cs
│       ├── Data/
│       │   └── AppDbContext.cs
│       ├── Models/
│       │   └── User.cs
│       ├── Program.cs
│       └── appsettings.json
├── frontend/
│   ├── src/
│   │   ├── components/
│   │   │   ├── Login.tsx
│   │   │   ├── Register.tsx
│   │   │   ├── Profile.tsx
│   │   │   ├── ProtectedRoute.tsx
│   │   │   ├── Auth.css
│   │   │   └── Profile.css
│   │   ├── contexts/
│   │   │   └── AuthContext.tsx
│   │   ├── App.tsx
│   │   └── App.css
│   └── package.json
├── start-app.sh
└── README.md
```

## API Endpoints

### Authentication Endpoints

- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login user
- `GET /api/auth/users` - Get all users (for testing)
- `PUT /api/auth/profile/{id}` - Update user profile

## Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js (v18 or later)
- npm or yarn

### Quick Start

Use the startup script for easy development:

```bash
./start-app.sh
```

This will start both backend and frontend servers automatically.

### Manual Setup

#### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd backend/LoginUserAPI
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the backend server:
   ```bash
   dotnet run --urls="http://localhost:5000"
   ```

   The API will be available at `http://localhost:5000`

#### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

   The React app will be available at `http://localhost:3000`

## Usage

1. **Register**: Create a new account by providing email, password, first name, and last name
2. **Login**: Sign in with your email and password
3. **Profile**: View and edit your profile information
4. **Logout**: Sign out to clear the authentication session

## Authentication Flow

1. User registers or logs in with credentials
2. Backend stores user data in memory (no encryption - educational purposes)
3. Frontend stores user object in context state
4. Protected routes check for user presence in context
5. Logout clears the user from context state

## API Testing

Test the API endpoints using curl:

```bash
# Register a user
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"password123","firstName":"John","lastName":"Doe"}'

# Login
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"password123"}'

# Get all users
curl http://localhost:5000/api/auth/users
```

## ⚠️ Educational Notes

This application is designed for learning purposes:
- **Not for production use** - No password encryption
- **In-memory storage** - All data resets when the server restarts
- **No session persistence** - Authentication state is lost on page refresh
- **Minimal validation** - Focus on core authentication concepts
- **No security features** - Plain text passwords for educational clarity

## Perfect For

- **Learning authentication concepts** without complexity
- **Understanding React Context API** usage
- **Full-stack development practice** with .NET and React
- **API development** and testing
- **Educational demonstrations** of authentication flows

## Development Features

- **Hot reload** - Both frontend and backend support hot reload
- **Swagger documentation** - Available at http://localhost:5000/swagger
- **CORS configured** - Frontend can communicate with backend
- **Error handling** - Proper error messages and validation
- **TypeScript support** - Full type safety in frontend

## Troubleshooting

### Connection Issues
- Ensure both servers are running on correct ports
- Check if ports 3000 and 5000 are available
- Use the startup script for automatic setup

### Build Issues
- Run `dotnet restore` in backend directory
- Run `npm install` in frontend directory
- Ensure .NET 8 SDK and Node.js are installed

### API Issues
- Check server logs at `backend/LoginUserAPI/server.log`
- Test API endpoints with curl
- Verify CORS configuration

## License

This project is open source and available under the MIT License.

---

**Perfect for learning authentication concepts with modern technologies!** 🎓
