import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Profile from '../components/Profile';
import { AuthProvider } from '../contexts/AuthContext';
import axios from 'axios';

// Mock axios
jest.mock('axios');
const mockedAxios = axios as jest.Mocked<typeof axios>;

// Mock AuthContext with a logged-in user
const mockUser = {
  id: 1,
  email: 'test@example.com',
  firstName: 'John',
  lastName: 'Doe'
};

// Create a custom AuthProvider with pre-logged user
const AuthProviderWithUser: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  return (
    <AuthProvider>
      {children}
    </AuthProvider>
  );
};

// Mock the useAuth hook to return a logged-in user
jest.mock('../contexts/AuthContext', () => ({
  ...jest.requireActual('../contexts/AuthContext'),
  useAuth: () => ({
    user: mockUser,
    updateProfile: jest.fn(),
    logout: jest.fn(),
    loading: false,
    error: null,
  }),
}));

describe('Profile Component', () => {
  const mockUpdateProfile = jest.fn();
  const mockLogout = jest.fn();

  beforeEach(() => {
    jest.clearAllMocks();
    // Reset the mock implementation
    require('../contexts/AuthContext').useAuth.mockReturnValue({
      user: mockUser,
      updateProfile: mockUpdateProfile,
      logout: mockLogout,
      loading: false,
      error: null,
    });
  });

  it('should render user profile information', () => {
    render(<Profile />);

    expect(screen.getByText('JD')).toBeInTheDocument(); // Avatar initials
    expect(screen.getByText('John Doe')).toBeInTheDocument();
    expect(screen.getByText('test@example.com')).toBeInTheDocument();
    expect(screen.getByText('User Profile')).toBeInTheDocument();
    expect(screen.getByText('Personal Information')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Logout' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Edit Profile' })).toBeInTheDocument();
  });

  it('should display user details in view mode', () => {
    render(<Profile />);

    expect(screen.getByText('First Name')).toBeInTheDocument();
    expect(screen.getByText('Last Name')).toBeInTheDocument();
    expect(screen.getByText('Email')).toBeInTheDocument();
    expect(screen.getByText('User ID')).toBeInTheDocument();
    expect(screen.getByText('John')).toBeInTheDocument();
    expect(screen.getByText('Doe')).toBeInTheDocument();
    expect(screen.getByText('#1')).toBeInTheDocument();
  });

  it('should enter edit mode when clicking Edit Profile button', async () => {
    render(<Profile />);

    const editButton = screen.getByRole('button', { name: 'Edit Profile' });
    await userEvent.click(editButton);

    expect(screen.getByDisplayValue('John')).toBeInTheDocument();
    expect(screen.getByDisplayValue('Doe')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Save Changes' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Cancel' })).toBeInTheDocument();
    expect(screen.queryByRole('button', { name: 'Edit Profile' })).not.toBeInTheDocument();
  });

  it('should handle successful profile update', async () => {
    mockUpdateProfile.mockResolvedValueOnce(undefined);

    render(<Profile />);

    // Enter edit mode
    const editButton = screen.getByRole('button', { name: 'Edit Profile' });
    await userEvent.click(editButton);

    // Update fields
    const firstNameInput = screen.getByDisplayValue('John');
    const lastNameInput = screen.getByDisplayValue('Doe');
    
    await userEvent.clear(firstNameInput);
    await userEvent.type(firstNameInput, 'Jane');
    await userEvent.clear(lastNameInput);
    await userEvent.type(lastNameInput, 'Smith');

    // Save changes
    const saveButton = screen.getByRole('button', { name: 'Save Changes' });
    await userEvent.click(saveButton);

    await waitFor(() => {
      expect(mockUpdateProfile).toHaveBeenCalledWith('Jane', 'Smith');
    });

    // Should show success message
    await waitFor(() => {
      expect(screen.getByText('Profile updated successfully!')).toBeInTheDocument();
    });
  });

  it('should handle profile update error', async () => {
    const errorMessage = 'Update failed';
    mockUpdateProfile.mockRejectedValueOnce(new Error(errorMessage));

    // Mock useAuth to return error state
    require('../contexts/AuthContext').useAuth.mockReturnValue({
      user: mockUser,
      updateProfile: mockUpdateProfile,
      logout: mockLogout,
      loading: false,
      error: errorMessage,
    });

    render(<Profile />);

    expect(screen.getByText(errorMessage)).toBeInTheDocument();
  });

  it('should cancel edit mode and revert changes', async () => {
    render(<Profile />);

    // Enter edit mode
    const editButton = screen.getByRole('button', { name: 'Edit Profile' });
    await userEvent.click(editButton);

    // Make changes
    const firstNameInput = screen.getByDisplayValue('John');
    await userEvent.clear(firstNameInput);
    await userEvent.type(firstNameInput, 'Changed');

    // Cancel changes
    const cancelButton = screen.getByRole('button', { name: 'Cancel' });
    await userEvent.click(cancelButton);

    // Should revert to original values and exit edit mode
    expect(screen.queryByDisplayValue('Changed')).not.toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Edit Profile' })).toBeInTheDocument();
    expect(screen.queryByRole('button', { name: 'Save Changes' })).not.toBeInTheDocument();
  });

  it('should show loading state during profile update', async () => {
    // Mock useAuth to return loading state
    require('../contexts/AuthContext').useAuth.mockReturnValue({
      user: mockUser,
      updateProfile: mockUpdateProfile,
      logout: mockLogout,
      loading: true,
      error: null,
    });

    render(<Profile />);

    // Enter edit mode
    const editButton = screen.getByRole('button', { name: 'Edit Profile' });
    await userEvent.click(editButton);

    expect(screen.getByText('Saving...')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Saving...' })).toBeDisabled();
  });

  it('should handle logout', async () => {
    render(<Profile />);

    const logoutButton = screen.getByRole('button', { name: 'Logout' });
    await userEvent.click(logoutButton);

    expect(mockLogout).toHaveBeenCalled();
  });

  it('should hide success message after 3 seconds', async () => {
    jest.useFakeTimers();
    mockUpdateProfile.mockResolvedValueOnce(undefined);

    render(<Profile />);

    // Enter edit mode and save
    const editButton = screen.getByRole('button', { name: 'Edit Profile' });
    await userEvent.click(editButton);

    const saveButton = screen.getByRole('button', { name: 'Save Changes' });
    await userEvent.click(saveButton);

    await waitFor(() => {
      expect(screen.getByText('Profile updated successfully!')).toBeInTheDocument();
    });

    // Fast-forward 3 seconds
    jest.advanceTimersByTime(3000);

    await waitFor(() => {
      expect(screen.queryByText('Profile updated successfully!')).not.toBeInTheDocument();
    });

    jest.useRealTimers();
  });

  it('should render loading state when user is null', () => {
    // Mock useAuth to return null user
    require('../contexts/AuthContext').useAuth.mockReturnValue({
      user: null,
      updateProfile: mockUpdateProfile,
      logout: mockLogout,
      loading: false,
      error: null,
    });

    render(<Profile />);

    expect(screen.getByText('Loading...')).toBeInTheDocument();
  });

  it('should update first name only when last name is empty', async () => {
    mockUpdateProfile.mockResolvedValueOnce(undefined);

    render(<Profile />);

    // Enter edit mode
    const editButton = screen.getByRole('button', { name: 'Edit Profile' });
    await userEvent.click(editButton);

    // Update only first name
    const firstNameInput = screen.getByDisplayValue('John');
    const lastNameInput = screen.getByDisplayValue('Doe');
    
    await userEvent.clear(firstNameInput);
    await userEvent.type(firstNameInput, 'Jane');
    await userEvent.clear(lastNameInput);

    // Save changes
    const saveButton = screen.getByRole('button', { name: 'Save Changes' });
    await userEvent.click(saveButton);

    await waitFor(() => {
      expect(mockUpdateProfile).toHaveBeenCalledWith('Jane', '');
    });
  });

  it('should update last name only when first name is empty', async () => {
    mockUpdateProfile.mockResolvedValueOnce(undefined);

    render(<Profile />);

    // Enter edit mode
    const editButton = screen.getByRole('button', { name: 'Edit Profile' });
    await userEvent.click(editButton);

    // Update only last name
    const firstNameInput = screen.getByDisplayValue('John');
    const lastNameInput = screen.getByDisplayValue('Doe');
    
    await userEvent.clear(firstNameInput);
    await userEvent.clear(lastNameInput);
    await userEvent.type(lastNameInput, 'Smith');

    // Save changes
    const saveButton = screen.getByRole('button', { name: 'Save Changes' });
    await userEvent.click(saveButton);

    await waitFor(() => {
      expect(mockUpdateProfile).toHaveBeenCalledWith('', 'Smith');
    });
  });
});
