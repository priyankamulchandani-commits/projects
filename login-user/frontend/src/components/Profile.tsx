import React, { useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import './Profile.css';

const SimpleProfile: React.FC = () => {
  const { user, updateProfile, logout, loading, error } = useAuth();
  const [isEditing, setIsEditing] = useState(false);
  const [firstName, setFirstName] = useState(user?.firstName || '');
  const [lastName, setLastName] = useState(user?.lastName || '');
  const [successMessage, setSuccessMessage] = useState('');

  const handleSave = async () => {
    try {
      await updateProfile(firstName, lastName);
      setIsEditing(false);
      setSuccessMessage('Profile updated successfully!');
      setTimeout(() => setSuccessMessage(''), 3000);
    } catch (error) {
      // Error is handled by the context
    }
  };

  const handleCancel = () => {
    setFirstName(user?.firstName || '');
    setLastName(user?.lastName || '');
    setIsEditing(false);
  };

  if (!user) {
    return <div>Loading...</div>;
  }

  return (
    <div className="profile-container">
      <div className="profile-card">
        <div className="profile-header">
          <div className="profile-avatar">
            <span>{user.firstName[0]}{user.lastName[0]}</span>
          </div>
          <div className="profile-info">
            <h1>{user.firstName} {user.lastName}</h1>
            <p className="profile-email">{user.email}</p>
            <p className="profile-date">Simple Auth - No JWT!</p>
          </div>
          <button onClick={logout} className="logout-button">
            Logout
          </button>
        </div>

        <div className="profile-content">
          <div className="profile-section">
            <div className="section-header">
              <h2>Personal Information</h2>
              {!isEditing && (
                <button onClick={() => setIsEditing(true)} className="edit-button">
                  Edit Profile
                </button>
              )}
            </div>

            {successMessage && (
              <div className="success-message">{successMessage}</div>
            )}

            {error && (
              <div className="error-message">{error}</div>
            )}

            {isEditing ? (
              <div className="edit-form">
                <div className="form-row">
                  <div className="form-group">
                    <label htmlFor="firstName">First Name</label>
                    <input
                      id="firstName"
                      type="text"
                      value={firstName}
                      onChange={(e) => setFirstName(e.target.value)}
                      required
                    />
                  </div>

                  <div className="form-group">
                    <label htmlFor="lastName">Last Name</label>
                    <input
                      id="lastName"
                      type="text"
                      value={lastName}
                      onChange={(e) => setLastName(e.target.value)}
                      required
                    />
                  </div>
                </div>

                <div className="form-actions">
                  <button 
                    onClick={handleSave} 
                    disabled={loading}
                    className="save-button"
                  >
                    {loading ? 'Saving...' : 'Save Changes'}
                  </button>
                  <button onClick={handleCancel} className="cancel-button">
                    Cancel
                  </button>
                </div>
              </div>
            ) : (
              <div className="profile-details">
                <div className="detail-item">
                  <label>First Name</label>
                  <span>{user.firstName}</span>
                </div>
                <div className="detail-item">
                  <label>Last Name</label>
                  <span>{user.lastName}</span>
                </div>
                <div className="detail-item">
                  <label>Email</label>
                  <span>{user.email}</span>
                </div>
                <div className="detail-item">
                  <label>User ID</label>
                  <span>#{user.id}</span>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default SimpleProfile;
