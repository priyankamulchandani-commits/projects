import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Home = () => {
  const { isAuthenticated, user } = useAuth();

  if (isAuthenticated) {
    return (
      <div className="home-container">
        <div className="hero-section">
          <div className="hero-content">
            <h1>Welcome back, {user?.firstName}!</h1>
            <p>Great to see you again. Ready to continue where you left off?</p>
            <div className="hero-actions">
              <Link to="/dashboard" className="btn btn-primary">
                Go to Dashboard
              </Link>
              <Link to="/profile" className="btn btn-outline">
                View Profile
              </Link>
            </div>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="home-container">
      <div className="hero-section">
        <div className="hero-content">
          <h1>Welcome to MERN Auth</h1>
          <p>
            A complete authentication system built with MongoDB, Express, React, and Node.js.
            Secure, scalable, and ready for your next project.
          </p>
          <div className="hero-actions">
            <Link to="/signup" className="btn btn-primary">
              Get Started
            </Link>
            <Link to="/login" className="btn btn-outline">
              Sign In
            </Link>
          </div>
        </div>
        <div className="hero-image">
          <div className="feature-grid">
            <div className="feature-item">
              <div className="feature-icon">🔐</div>
              <h3>Secure Authentication</h3>
              <p>JWT-based authentication with bcrypt password hashing</p>
            </div>
            <div className="feature-item">
              <div className="feature-icon">👤</div>
              <h3>Profile Management</h3>
              <p>Complete user profile system with customizable fields</p>
            </div>
            <div className="feature-item">
              <div className="feature-icon">📱</div>
              <h3>Responsive Design</h3>
              <p>Works perfectly on desktop, tablet, and mobile devices</p>
            </div>
            <div className="feature-item">
              <div className="feature-icon">⚡</div>
              <h3>Fast & Efficient</h3>
              <p>Optimized performance with modern React patterns</p>
            </div>
          </div>
        </div>
      </div>

      <div className="features-section">
        <div className="container">
          <h2>Why Choose Our Auth System?</h2>
          <div className="features-grid">
            <div className="feature-card">
              <div className="feature-header">
                <div className="feature-icon large">🛡️</div>
                <h3>Enterprise Security</h3>
              </div>
              <p>
                Industry-standard security practices including password hashing,
                JWT tokens, and secure session management.
              </p>
              <ul>
                <li>Bcrypt password encryption</li>
                <li>JWT token authentication</li>
                <li>Session timeout protection</li>
                <li>Password strength validation</li>
              </ul>
            </div>

            <div className="feature-card">
              <div className="feature-header">
                <div className="feature-icon large">⚙️</div>
                <h3>Easy Integration</h3>
              </div>
              <p>
                Drop-in solution that can be easily integrated into any
                React application with minimal configuration.
              </p>
              <ul>
                <li>React Context API</li>
                <li>Protected routes</li>
                <li>Automatic token refresh</li>
                <li>Error boundary handling</li>
              </ul>
            </div>

            <div className="feature-card">
              <div className="feature-header">
                <div className="feature-icon large">🎨</div>
                <h3>Modern UI/UX</h3>
              </div>
              <p>
                Beautiful, responsive interface that provides an excellent
                user experience across all devices and screen sizes.
              </p>
              <ul>
                <li>Mobile-first design</li>
                <li>Accessibility features</li>
                <li>Loading states</li>
                <li>Error messaging</li>
              </ul>
            </div>
          </div>
        </div>
      </div>

      <div className="cta-section">
        <div className="container">
          <h2>Ready to Get Started?</h2>
          <p>Join thousands of developers who trust our authentication system.</p>
          <div className="cta-actions">
            <Link to="/signup" className="btn btn-primary large">
              Create Your Account
            </Link>
            <div className="cta-note">
              Already have an account? <Link to="/login">Sign in here</Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home;