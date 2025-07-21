import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Navbar = () => {
  const { isAuthenticated, user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = async () => {
    await logout();
    navigate('/');
  };

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <Link to="/" className="navbar-brand">
          MERN Auth
        </Link>
        
        <div className="navbar-links">
          {isAuthenticated ? (
            <>
              <span className="navbar-welcome">
                Welcome, {user?.firstName || user?.username}!
              </span>
              <Link to="/profile" className="navbar-link">
                Profile
              </Link>
              <button onClick={handleLogout} className="navbar-button logout-btn">
                Logout
              </button>
            </>
          ) : (
            <>
              <Link to="/login" className="navbar-link">
                Login
              </Link>
              <Link to="/signup" className="navbar-button signup-btn">
                Sign Up
              </Link>
            </>
          )}
        </div>
      </div>
    </nav>
  );
};

export default Navbar;