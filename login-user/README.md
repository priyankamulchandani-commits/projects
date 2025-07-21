# Login User Application

A full-stack authentication application built with .NET 8 Web API backend and React TypeScript frontend. This application provides complete user authentication functionality including login, signup, profile management, and logout with JWT authentication.

## Features

- **User Registration**: Create new user accounts with email, password, and personal information
- **User Login**: Secure authentication with JWT tokens
- **Profile Management**: View and edit user profile information
- **Protected Routes**: Route protection based on authentication status
- **Context API**: React Context for global authentication state management
- **Modern UI**: Beautiful and responsive design with CSS animations
- **Form Validation**: Client-side and server-side validation
- **Password Security**: BCrypt password hashing
- **CORS Support**: Cross-origin resource sharing configuration

## Technology Stack

### Backend (.NET 8 Web API)
- ASP.NET Core 8.0
- Entity Framework Core (In-Memory Database)
- JWT Authentication
- BCrypt for password hashing
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
│       │   └── AuthController.cs
│       ├── Data/
│       │   └── AppDbContext.cs
│       ├── DTOs/
│       │   └── AuthDTOs.cs
│       ├── Models/
│       │   └── User.cs
│       ├── Services/
│       │   └── JwtService.cs
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
└── README.md
```

## API Endpoints

### Authentication Endpoints

- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login user
- `GET /api/auth/profile` - Get user profile (protected)
- `PUT /api/auth/profile` - Update user profile (protected)

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
   dotnet run
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

1. User registers or logs in
2. Backend validates credentials and returns JWT token
3. Frontend stores token in localStorage
4. Token is included in subsequent API requests
5. Protected routes check for valid authentication
6. Logout clears the token and redirects to login

## Security Features

- Password hashing with BCrypt
- JWT token-based authentication
- Protected API endpoints
- Client-side route protection
- Input validation and sanitization
- CORS configuration for security

## Development Notes

- The application uses an in-memory database for simplicity
- JWT tokens expire after 7 days (configurable)
- All API responses include proper HTTP status codes
- Error handling is implemented on both client and server
- The UI is fully responsive and mobile-friendly

## License

This project is open source and available under the MIT License.