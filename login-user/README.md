# Simple Login User Application

A straightforward full-stack authentication application built with .NET 8 Web API backend and React TypeScript frontend. This application provides simple user authentication functionality including login, signup, and profile management without JWT tokens or password hashing complexity.

## Features

- **User Registration**: Create new user accounts with email, password, and personal information
- **User Login**: Simple authentication with plain text passwords
- **Profile Management**: View and edit user profile information
- **Protected Routes**: Route protection based on authentication status
- **Context API**: React Context for global authentication state management
- **Modern UI**: Beautiful and responsive design with CSS animations
- **In-Memory Storage**: Simple data storage (resets on restart)
- **No Complexity**: No JWT tokens, no password hashing - perfect for learning!

## Technology Stack

### Backend (.NET 8 Web API)
- ASP.NET Core 8.0
- Entity Framework Core (In-Memory Database)
- Simple authentication (no JWT)
- Plain text passwords (no hashing)
- CORS middleware
- RESTful API design

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
│       │   └── SimpleAuthController.cs
│       ├── Data/
│       │   └── AppDbContext.cs
│       ├── Models/
│       │   └── User.cs
│       ├── Program.cs
│       └── appsettings.json
├── frontend/
│   ├── src/
│   │   ├── components/
│   │   │   ├── SimpleLogin.tsx
│   │   │   ├── SimpleRegister.tsx
│   │   │   ├── SimpleProfile.tsx
│   │   │   ├── SimpleProtectedRoute.tsx
│   │   │   ├── Auth.css
│   │   │   └── Profile.css
│   │   ├── contexts/
│   │   │   └── SimpleAuthContext.tsx
│   │   ├── App.tsx
│   │   └── App.css
│   └── package.json
└── README.md
```

## API Endpoints

### Authentication Endpoints

- `POST /api/simpleauth/register` - Register a new user
- `POST /api/simpleauth/login` - Login user
- `GET /api/simpleauth/users` - Get all users (for testing)
- `PUT /api/simpleauth/profile/{id}` - Update user profile

## Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js (v18 or later)
- npm or yarn

### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd login-user/backend/LoginUserAPI
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

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd login-user/frontend
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

1. User registers or logs in with plain text credentials
2. Backend stores user data in memory (no encryption)
3. Frontend stores user object in context state
4. Protected routes check for user presence in context
5. Logout clears the user from context state

## ⚠️ Important Notes

- **Not for production use** - This is a learning/demonstration application
- **No password security** - Passwords are stored in plain text
- **In-memory storage** - All data resets when the server restarts
- **No session persistence** - Authentication state is lost on page refresh
- **No input validation** - Minimal validation for simplicity

## Why Simple?

This application is designed for:
- **Learning purposes** - Understanding authentication concepts without complexity
- **Rapid prototyping** - Quick setup for testing ideas
- **Educational demos** - Teaching authentication flows
- **Development practice** - Focus on frontend/backend integration

## Security Features (Intentionally Missing)

For educational purposes, this app does NOT include:
- Password hashing (BCrypt, etc.)
- JWT token authentication
- Session management
- Input sanitization
- HTTPS enforcement
- Rate limiting
- Password complexity requirements

## Development Notes

- Uses Entity Framework In-Memory database
- All user data is lost when the server restarts
- API responses include full user objects
- No authentication middleware required
- CORS is configured for localhost:3000

## License

This project is open source and available under the MIT License.

---

**Perfect for learning authentication concepts without the complexity of production-level security!** 🎓
