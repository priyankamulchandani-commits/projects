import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { AuthProvider, useAuth } from '../contexts/AuthContext';
import axios from 'axios';

// Mock axios
jest.mock('axios');
const mockedAxios = axios as jest.Mocked<typeof axios>;

// Test component that uses the AuthContext
const TestComponent: React.FC = () => {
  const { user, login, register, logout, updateProfile, loading, error } = useAuth();

  return (
    <div>
      <div data-testid="user-info">
        {user ? `User: ${user.firstName} ${user.lastName}` : 'No user'}
      </div>
      <div data-testid="loading">{loading ? 'Loading' : 'Not loading'}</div>
      <div data-testid="error">{error || 'No error'}</div>
      <button
        data-testid="login-btn"
        onClick={() => login('test@example.com', 'password123')}
      >
        Login
      </button>
      <button
        data-testid="register-btn"
        onClick={() => register('test@example.com', 'password123', 'John', 'Doe')}
      >
        Register
      </button>
      <button data-testid="logout-btn" onClick={logout}>
        Logout
      </button>
      <button
        data-testid="update-btn"
        onClick={() => updateProfile('Jane', 'Smith')}
      >
        Update Profile
      </button>
    </div>
  );
};

const renderWithProvider = () => {
  return render(
    <AuthProvider>
      <TestComponent />
    </AuthProvider>
  );
};

describe('AuthContext', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should render without user initially', () => {
    renderWithProvider();
    expect(screen.getByTestId('user-info')).toHaveTextContent('No user');
    expect(screen.getByTestId('loading')).toHaveTextContent('Not loading');
    expect(screen.getByTestId('error')).toHaveTextContent('No error');
  });

  it('should handle successful login', async () => {
    const mockUser = {
      id: 1,
      email: 'test@example.com',
      firstName: 'John',
      lastName: 'Doe'
    };

    mockedAxios.post.mockResolvedValueOnce({
      data: { user: mockUser }
    });

    renderWithProvider();

    const loginBtn = screen.getByTestId('login-btn');
    await userEvent.click(loginBtn);

    await waitFor(() => {
      expect(screen.getByTestId('user-info')).toHaveTextContent('User: John Doe');
    });

    expect(mockedAxios.post).toHaveBeenCalledWith(
      'http://localhost:5000/api/auth/login',
      {
        email: 'test@example.com',
        password: 'password123'
      }
    );
  });

  it('should handle login error', async () => {
    const errorMessage = 'Invalid credentials';
    mockedAxios.post.mockRejectedValueOnce({
      response: { data: { message: errorMessage } }
    });

    renderWithProvider();

    const loginBtn = screen.getByTestId('login-btn');
    await userEvent.click(loginBtn);

    await waitFor(() => {
      expect(screen.getByTestId('error')).toHaveTextContent(errorMessage);
    });
  });

  it('should handle successful registration', async () => {
    const mockUser = {
      id: 1,
      email: 'test@example.com',
      firstName: 'John',
      lastName: 'Doe'
    };

    mockedAxios.post.mockResolvedValueOnce({
      data: { user: mockUser }
    });

    renderWithProvider();

    const registerBtn = screen.getByTestId('register-btn');
    await userEvent.click(registerBtn);

    await waitFor(() => {
      expect(screen.getByTestId('user-info')).toHaveTextContent('User: John Doe');
    });

    expect(mockedAxios.post).toHaveBeenCalledWith(
      'http://localhost:5000/api/auth/register',
      {
        email: 'test@example.com',
        password: 'password123',
        firstName: 'John',
        lastName: 'Doe'
      }
    );
  });

  it('should handle registration error', async () => {
    const errorMessage = 'User already exists';
    mockedAxios.post.mockRejectedValueOnce({
      response: { data: { message: errorMessage } }
    });

    renderWithProvider();

    const registerBtn = screen.getByTestId('register-btn');
    await userEvent.click(registerBtn);

    await waitFor(() => {
      expect(screen.getByTestId('error')).toHaveTextContent(errorMessage);
    });
  });

  it('should handle logout', async () => {
    const mockUser = {
      id: 1,
      email: 'test@example.com',
      firstName: 'John',
      lastName: 'Doe'
    };

    mockedAxios.post.mockResolvedValueOnce({
      data: { user: mockUser }
    });

    renderWithProvider();

    // First login
    const loginBtn = screen.getByTestId('login-btn');
    await userEvent.click(loginBtn);

    await waitFor(() => {
      expect(screen.getByTestId('user-info')).toHaveTextContent('User: John Doe');
    });

    // Then logout
    const logoutBtn = screen.getByTestId('logout-btn');
    await userEvent.click(logoutBtn);

    expect(screen.getByTestId('user-info')).toHaveTextContent('No user');
    expect(screen.getByTestId('error')).toHaveTextContent('No error');
  });

  it('should handle profile update', async () => {
    const mockUser = {
      id: 1,
      email: 'test@example.com',
      firstName: 'John',
      lastName: 'Doe'
    };

    const updatedUser = {
      id: 1,
      email: 'test@example.com',
      firstName: 'Jane',
      lastName: 'Smith'
    };

    // Mock login first
    mockedAxios.post.mockResolvedValueOnce({
      data: { user: mockUser }
    });

    // Mock profile update
    mockedAxios.put.mockResolvedValueOnce({
      data: { user: updatedUser }
    });

    renderWithProvider();

    // First login
    const loginBtn = screen.getByTestId('login-btn');
    await userEvent.click(loginBtn);

    await waitFor(() => {
      expect(screen.getByTestId('user-info')).toHaveTextContent('User: John Doe');
    });

    // Then update profile
    const updateBtn = screen.getByTestId('update-btn');
    await userEvent.click(updateBtn);

    await waitFor(() => {
      expect(screen.getByTestId('user-info')).toHaveTextContent('User: Jane Smith');
    });

    expect(mockedAxios.put).toHaveBeenCalledWith(
      'http://localhost:5000/api/auth/profile/1',
      {
        firstName: 'Jane',
        lastName: 'Smith'
      }
    );
  });

  it('should handle profile update error', async () => {
    const mockUser = {
      id: 1,
      email: 'test@example.com',
      firstName: 'John',
      lastName: 'Doe'
    };

    const errorMessage = 'Update failed';

    // Mock login first
    mockedAxios.post.mockResolvedValueOnce({
      data: { user: mockUser }
    });

    // Mock profile update error
    mockedAxios.put.mockRejectedValueOnce({
      response: { data: { message: errorMessage } }
    });

    renderWithProvider();

    // First login
    const loginBtn = screen.getByTestId('login-btn');
    await userEvent.click(loginBtn);

    await waitFor(() => {
      expect(screen.getByTestId('user-info')).toHaveTextContent('User: John Doe');
    });

    // Then try to update profile
    const updateBtn = screen.getByTestId('update-btn');
    await userEvent.click(updateBtn);

    await waitFor(() => {
      expect(screen.getByTestId('error')).toHaveTextContent(errorMessage);
    });
  });

  it('should handle update profile when no user is logged in', async () => {
    renderWithProvider();

    const updateBtn = screen.getByTestId('update-btn');
    await userEvent.click(updateBtn);

    // Should not make API call when no user is logged in
    expect(mockedAxios.put).not.toHaveBeenCalled();
  });

  it('should handle login error without response data', async () => {
    mockedAxios.post.mockRejectedValueOnce(new Error('Network error'));

    renderWithProvider();

    const loginBtn = screen.getByTestId('login-btn');
    await userEvent.click(loginBtn);

    await waitFor(() => {
      expect(screen.getByTestId('error')).toHaveTextContent('Login failed');
    });
  });

  it('should handle registration error without response data', async () => {
    mockedAxios.post.mockRejectedValueOnce(new Error('Network error'));

    renderWithProvider();

    const registerBtn = screen.getByTestId('register-btn');
    await userEvent.click(registerBtn);

    await waitFor(() => {
      expect(screen.getByTestId('error')).toHaveTextContent('Registration failed');
    });
  });

  it('should handle profile update error without response data', async () => {
    const mockUser = {
      id: 1,
      email: 'test@example.com',
      firstName: 'John',
      lastName: 'Doe'
    };

    // Mock login first
    mockedAxios.post.mockResolvedValueOnce({
      data: { user: mockUser }
    });

    // Mock profile update error
    mockedAxios.put.mockRejectedValueOnce(new Error('Network error'));

    renderWithProvider();

    // First login
    const loginBtn = screen.getByTestId('login-btn');
    await userEvent.click(loginBtn);

    await waitFor(() => {
      expect(screen.getByTestId('user-info')).toHaveTextContent('User: John Doe');
    });

    // Then try to update profile
    const updateBtn = screen.getByTestId('update-btn');
    await userEvent.click(updateBtn);

    await waitFor(() => {
      expect(screen.getByTestId('error')).toHaveTextContent('Profile update failed');
    });
  });

  it('should show loading state during login', async () => {
    let resolvePromise: (value: any) => void;
    const promise = new Promise((resolve) => {
      resolvePromise = resolve;
    });

    mockedAxios.post.mockReturnValueOnce(promise);

    renderWithProvider();

    const loginBtn = screen.getByTestId('login-btn');
    await userEvent.click(loginBtn);

    // Should show loading state
    expect(screen.getByTestId('loading')).toHaveTextContent('Loading');

    // Resolve the promise
    resolvePromise!({
      data: {
        user: {
          id: 1,
          email: 'test@example.com',
          firstName: 'John',
          lastName: 'Doe'
        }
      }
    });

    await waitFor(() => {
      expect(screen.getByTestId('loading')).toHaveTextContent('Not loading');
    });
  });
});

// Test useAuth hook outside of provider
describe('useAuth hook', () => {
  it('should throw error when used outside AuthProvider', () => {
    // Mock console.error to avoid noise in test output
    const consoleSpy = jest.spyOn(console, 'error').mockImplementation();

    const TestComponentOutsideProvider: React.FC = () => {
      useAuth();
      return <div>Test</div>;
    };

    expect(() => {
      render(<TestComponentOutsideProvider />);
    }).toThrow('useAuth must be used within an AuthProvider');

    consoleSpy.mockRestore();
  });
});
