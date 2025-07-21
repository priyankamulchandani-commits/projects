import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Dashboard = () => {
  const { user } = useAuth();

  return (
    <div className="dashboard-container">
      <div className="dashboard-header">
        <h1>Welcome back, {user?.firstName}!</h1>
        <p>Here's what's happening with your account today.</p>
      </div>

      <div className="dashboard-stats">
        <div className="stat-card">
          <div className="stat-icon">👤</div>
          <div className="stat-content">
            <h3>Profile Status</h3>
            <p>Your profile is {user?.bio ? 'complete' : 'incomplete'}</p>
            {!user?.bio && (
              <Link to="/profile" className="stat-link">
                Complete Profile
              </Link>
            )}
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon">📧</div>
          <div className="stat-content">
            <h3>Email</h3>
            <p>{user?.email}</p>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon">🔐</div>
          <div className="stat-content">
            <h3>Account Security</h3>
            <p>Account is secure</p>
            <Link to="/profile" className="stat-link">
              Change Password
            </Link>
          </div>
        </div>
      </div>

      <div className="dashboard-actions">
        <h2>Quick Actions</h2>
        <div className="action-cards">
          <Link to="/profile" className="action-card">
            <div className="action-icon">⚙️</div>
            <h3>Edit Profile</h3>
            <p>Update your personal information and preferences</p>
          </Link>

          <div className="action-card">
            <div className="action-icon">📊</div>
            <h3>View Activity</h3>
            <p>Check your recent account activity and usage</p>
          </div>

          <div className="action-card">
            <div className="action-icon">🔔</div>
            <h3>Notifications</h3>
            <p>Manage your notification preferences</p>
          </div>
        </div>
      </div>

      <div className="dashboard-info">
        <h2>Account Information</h2>
        <div className="info-grid">
          <div className="info-item">
            <label>Username:</label>
            <span>{user?.username}</span>
          </div>
          <div className="info-item">
            <label>Full Name:</label>
            <span>{user?.firstName} {user?.lastName}</span>
          </div>
          <div className="info-item">
            <label>Email:</label>
            <span>{user?.email}</span>
          </div>
          <div className="info-item">
            <label>Member Since:</label>
            <span>{new Date(user?.createdAt).toLocaleDateString()}</span>
          </div>
          {user?.phone && (
            <div className="info-item">
              <label>Phone:</label>
              <span>{user.phone}</span>
            </div>
          )}
          {user?.bio && (
            <div className="info-item full-width">
              <label>Bio:</label>
              <span>{user.bio}</span>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Dashboard;