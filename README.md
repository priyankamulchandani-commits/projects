# MERN Authentication App

A complete authentication system built with MongoDB, Express.js, React, and Node.js. This application provides secure user registration, login, profile management, and logout functionality with modern UI/UX design.

## Features

### 🔐 Authentication
- **User Registration**: Secure signup with email and username validation
- **User Login**: Login with email or username
- **JWT Authentication**: Secure token-based authentication
- **Password Security**: Bcrypt hashing for password protection
- **Auto Logout**: Automatic logout on token expiration

### 👤 Profile Management
- **Complete Profile System**: Manage personal information, contact details, and address
- **Password Change**: Secure password update functionality
- **Account Deletion**: Users can delete their accounts
- **Profile Validation**: Comprehensive form validation

### 🎨 User Interface
- **Modern Design**: Beautiful, responsive interface
- **Mobile-First**: Optimized for all device sizes
- **Loading States**: Enhanced user experience with loading indicators
- **Error Handling**: User-friendly error messages
- **Form Validation**: Real-time validation with helpful feedback

### 🛡️ Security Features
- **Protected Routes**: Route-level authentication
- **CORS Configuration**: Cross-origin resource sharing setup
- **Input Validation**: Server-side and client-side validation
- **Error Boundaries**: Graceful error handling

## Tech Stack

### Backend
- **Node.js**: Runtime environment
- **Express.js**: Web framework
- **MongoDB**: Database
- **Mongoose**: ODM for MongoDB
- **JWT**: JSON Web Tokens for authentication
- **bcryptjs**: Password hashing
- **express-validator**: Input validation
- **cors**: Cross-origin resource sharing
- **dotenv**: Environment variables

### Frontend
- **React**: Frontend library
- **React Router**: Client-side routing
- **Context API**: State management
- **Axios**: HTTP client
- **Modern CSS**: Responsive design with gradients and animations

## Getting Started

### Prerequisites
- Node.js (v14 or higher)
- MongoDB (local installation or MongoDB Atlas)
- npm or yarn

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd mern-auth-app
   ```

2. **Install backend dependencies**
   ```bash
   npm install
   ```

3. **Install frontend dependencies**
   ```bash
   cd client
   npm install
   cd ..
   ```

4. **Environment Setup**
   Create a `.env` file in the root directory:
   ```env
   NODE_ENV=development
   PORT=5000
   MONGODB_URI=mongodb://localhost:27017/mern-auth
   JWT_SECRET=your-super-secret-jwt-key-change-this-in-production
   CLIENT_URL=http://localhost:3000
   ```

5. **Start MongoDB**
   Make sure MongoDB is running on your system.

6. **Run the application**
   
   **Development mode (both frontend and backend):**
   ```bash
   npm run dev
   ```
   
   **Or run separately:**
   
   **Backend only:**
   ```bash
   npm run server
   ```
   
   **Frontend only:**
   ```bash
   npm run client
   ```

7. **Access the application**
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:5000

## API Endpoints

### Authentication Routes
- `POST /api/auth/signup` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/logout` - User logout
- `GET /api/auth/me` - Get current user
- `POST /api/auth/verify-token` - Verify JWT token

### User Routes
- `GET /api/user/profile` - Get user profile
- `PUT /api/user/profile` - Update user profile
- `PUT /api/user/change-password` - Change password
- `DELETE /api/user/profile` - Delete user account
- `GET /api/user/users` - Get all users (paginated)

## Project Structure

```
mern-auth-app/
├── client/                 # React frontend
│   ├── public/
│   ├── src/
│   │   ├── components/     # React components
│   │   ├── context/        # React Context API
│   │   ├── utils/          # Utility functions
│   │   ├── App.js          # Main App component
│   │   ├── App.css         # Global styles
│   │   └── index.js        # Entry point
│   └── package.json
├── server/                 # Express backend
│   ├── models/             # Mongoose models
│   ├── routes/             # Express routes
│   ├── middleware/         # Custom middleware
│   └── server.js           # Server entry point
├── .env                    # Environment variables
├── package.json            # Backend dependencies
└── README.md
```

## Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `NODE_ENV` | Environment mode | development |
| `PORT` | Server port | 5000 |
| `MONGODB_URI` | MongoDB connection string | mongodb://localhost:27017/mern-auth |
| `JWT_SECRET` | JWT signing secret | (required) |
| `CLIENT_URL` | Frontend URL for CORS | http://localhost:3000 |

## Usage

### User Registration
1. Navigate to `/signup`
2. Fill in the registration form
3. Submit to create account and auto-login

### User Login
1. Navigate to `/login`
2. Enter email/username and password
3. Submit to login and redirect to dashboard

### Profile Management
1. Access profile via `/profile` (protected route)
2. Update personal information in the "Profile Information" tab
3. Change password in the "Change Password" tab
4. Delete account in the "Danger Zone" tab

### Dashboard
- View account overview
- Quick actions for profile management
- Account information display

## Features in Detail

### Authentication Flow
1. User registers with email, username, password, and personal details
2. Password is hashed using bcrypt before storage
3. JWT token is generated upon successful login
4. Token is stored in localStorage and sent with API requests
5. Protected routes verify token validity
6. Automatic logout on token expiration

### Profile Management
- **Personal Information**: First name, last name, username, email, bio, phone, date of birth
- **Address Information**: Street, city, state, ZIP code, country
- **Security**: Password change with current password verification
- **Account Control**: Account deletion with confirmation

### Security Measures
- Password hashing with bcrypt (12 rounds)
- JWT token expiration (7 days)
- Protected API routes with authentication middleware
- Input validation on both client and server
- CORS configuration for API security
- Secure HTTP headers

## Development

### Available Scripts
- `npm start` - Start production server
- `npm run dev` - Start development with concurrent frontend/backend
- `npm run server` - Start backend development server
- `npm run client` - Start frontend development server
- `npm run build` - Build frontend for production
- `npm run install-all` - Install all dependencies

### Adding New Features
1. Backend: Add routes in `/server/routes/`
2. Frontend: Add components in `/client/src/components/`
3. Update API calls in `/client/src/utils/api.js`
4. Add routing in `/client/src/App.js`

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License.

## Support

For support, please open an issue in the repository or contact the development team.

---

**Note**: Remember to change the JWT secret and use environment-specific configurations for production deployment.