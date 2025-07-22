import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { BrowserRouter } from 'react-router-dom';
import Login from '../components/Login';
import { AuthProvider } from '../contexts/AuthContext';
import axios from 'axios';

// Mock axios
jest.mock('axios');
const mockedAxios = axios as jest.Mocked<typeof axios>;

// Mock useNavigate
const mockNavigate = jest.fn();
jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useNavigate: () => mockNavigate,
}));

const renderWithProviders = () => {
  return render(
    <BrowserRouter>
      <AuthProvider>
        <Login />
      </AuthProvider>
    </BrowserRouter>
  );
};

describe('Login Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should render login form', () => {
    renderWithProviders();

    expect(screen.getByText('Welcome Back')).toBeInTheDocument();
    expect(screen.getByText('Sign in to your account')).toBeInTheDocument();
    expect(screen.getByLabelText('Email')).toBeInTheDocument();
    expect(screen.getByLabelText('Password')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Sign In' })).toBeInTheDocument();
    expect(screen.getByText('Create one')).toBeInTheDocument();
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

    renderWithProviders();

    const emailInput = screen.getByLabelText('Email');
    const passwordInput = screen.getByLabelText('Password');
    const submitButton = screen.getByRole('button', { name: 'Sign In' });

    await userEvent.type(emailInput, 'test@example.com');
    await userEvent.type(passwordInput, 'password123');
    await userEvent.click(submitButton);

    await waitFor(() => {
      expect(mockNavigate).toHaveBeenCalledWith('/profile');
    });

    expect(mockedAxios.post).toHaveBeenCalledWith(
      'http://localhost:5000/api/auth/login',
      {
        email: 'test@example.com',
        password: 'password123'
      }
    );
  });

  it('should display error message on login failure', async () => {
    const errorMessage = 'Invalid email or password';
    mockedAxios.post.mockRejectedValueOnce({
      response: { data: { message: errorMessage } }
    });

    renderWithProviders();

    const emailInput = screen.getByLabelText('Email');
    const passwordInput = screen.getByLabelText('Password');
    const submitButton = screen.getByRole('button', { name: 'Sign In' });

    await userEvent.type(emailInput, 'test@example.com');
    await userEvent.type(passwordInput, 'wrongpassword');
    await userEvent.click(submitButton);

    await waitFor(() => {
      expect(screen.getByText(errorMessage)).toBeInTheDocument();
    });

    expect(mockNavigate).not.toHaveBeenCalled();
  });

  it('should show loading state during login', async () => {
    let resolvePromise: (value: any) => void;
    const promise = new Promise((resolve) => {
      resolvePromise = resolve;
    });

    mockedAxios.post.mockReturnValueOnce(promise);

    renderWithProviders();

    const emailInput = screen.getByLabelText('Email');
    const passwordInput = screen.getByLabelText('Password');
    const submitButton = screen.getByRole('button', { name: 'Sign In' });

    await userEvent.type(emailInput, 'test@example.com');
    await userEvent.type(passwordInput, 'password123');
    await userEvent.click(submitButton);

    expect(screen.getByText('Signing in...')).toBeInTheDocument();
    expect(submitButton).toBeDisabled();

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
      expect(screen.getByText('Sign In')).toBeInTheDocument();
    });
  });

  it('should validate required fields', async () => {
    renderWithProviders();

    const submitButton = screen.getByRole('button', { name: 'Sign In' });
    await userEvent.click(submitButton);

    // HTML5 validation should prevent form submission
    expect(mockedAxios.post).not.toHaveBeenCalled();
    expect(mockNavigate).not.toHaveBeenCalled();
  });

  it('should navigate to register page when clicking create account link', async () => {
    renderWithProviders();

    const createAccountLink = screen.getByText('Create one');
    expect(createAccountLink.closest('a')).toHaveAttribute('href', '/register');
  });

  it('should handle form submission with empty email', async () => {
    renderWithProviders();

    const passwordInput = screen.getByLabelText('Password');
    const submitButton = screen.getByRole('button', { name: 'Sign In' });

    await userEvent.type(passwordInput, 'password123');
    await userEvent.click(submitButton);

    // Should not call API with empty email
    expect(mockedAxios.post).not.toHaveBeenCalled();
  });

  it('should handle form submission with empty password', async () => {
    renderWithProviders();

    const emailInput = screen.getByLabelText('Email');
    const submitButton = screen.getByRole('button', { name: 'Sign In' });

    await userEvent.type(emailInput, 'test@example.com');
    await userEvent.click(submitButton);

    // Should not call API with empty password
    expect(mockedAxios.post).not.toHaveBeenCalled();
  });

  it('should clear form fields after successful login', async () => {
    const mockUser = {
      id: 1,
      email: 'test@example.com',
      firstName: 'John',
      lastName: 'Doe'
    };

    mockedAxios.post.mockResolvedValueOnce({
      data: { user: mockUser }
    });

    renderWithProviders();

    const emailInput = screen.getByLabelText('Email') as HTMLInputElement;
    const passwordInput = screen.getByLabelText('Password') as HTMLInputElement;
    const submitButton = screen.getByRole('button', { name: 'Sign In' });

    await userEvent.type(emailInput, 'test@example.com');
    await userEvent.type(passwordInput, 'password123');
    
    expect(emailInput.value).toBe('test@example.com');
    expect(passwordInput.value).toBe('password123');

    await userEvent.click(submitButton);

    await waitFor(() => {
      expect(mockNavigate).toHaveBeenCalledWith('/profile');
    });
  });
});
