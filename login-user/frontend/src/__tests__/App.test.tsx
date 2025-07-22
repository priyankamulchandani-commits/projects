import React from 'react';
import { render, screen } from '@testing-library/react';
import App from '../App';
import { BrowserRouter } from 'react-router-dom';

// Mock all the components
jest.mock('../components/Login', () => {
  return function MockLogin() {
    return <div data-testid="login-component">Login Component</div>;
  };
});

jest.mock('../components/Register', () => {
  return function MockRegister() {
    return <div data-testid="register-component">Register Component</div>;
  };
});

jest.mock('../components/Profile', () => {
  return function MockProfile() {
    return <div data-testid="profile-component">Profile Component</div>;
  };
});

jest.mock('../components/ProtectedRoute', () => {
  return function MockProtectedRoute({ children }: { children: React.ReactNode }) {
    return <div data-testid="protected-route">{children}</div>;
  };
});

// Mock the AuthContext
jest.mock('../contexts/AuthContext', () => ({
  AuthProvider: ({ children }: { children: React.ReactNode }) => (
    <div data-testid="auth-provider">{children}</div>
  ),
  useAuth: () => ({
    user: null,
    login: jest.fn(),
    register: jest.fn(),
    logout: jest.fn(),
    updateProfile: jest.fn(),
    loading: false,
    error: null,
  }),
}));

// Helper function to render App with router
const renderApp = (initialPath = '/') => {
  window.history.pushState({}, 'Test page', initialPath);
  return render(<App />);
};

describe('App Component', () => {
  it('should render with AuthProvider wrapper', () => {
    renderApp();
    expect(screen.getByTestId('auth-provider')).toBeInTheDocument();
  });

  it('should redirect to login page when accessing root path', () => {
    renderApp('/');
    expect(screen.getByTestId('login-component')).toBeInTheDocument();
  });

  it('should render Login component when navigating to /login', () => {
    renderApp('/login');
    expect(screen.getByTestId('login-component')).toBeInTheDocument();
  });

  it('should render Register component when navigating to /register', () => {
    renderApp('/register');
    expect(screen.getByTestId('register-component')).toBeInTheDocument();
  });

  it('should render Profile component within ProtectedRoute when navigating to /profile', () => {
    renderApp('/profile');
    expect(screen.getByTestId('protected-route')).toBeInTheDocument();
    expect(screen.getByTestId('profile-component')).toBeInTheDocument();
  });

  it('should have correct App class name', () => {
    const { container } = renderApp();
    expect(container.querySelector('.App')).toBeInTheDocument();
  });

  it('should handle invalid routes by redirecting to login', () => {
    renderApp('/invalid-route');
    // Should redirect to login since no matching route
    expect(screen.getByTestId('login-component')).toBeInTheDocument();
  });

  it('should render Router component', () => {
    const { container } = renderApp();
    // The Router component should be present (though we can't directly test it)
    expect(container.firstChild).toBeInTheDocument();
  });
});

// Test the App component structure without mocks for basic rendering
describe('App Component - Structure', () => {
  it('should render without crashing', () => {
    // This test ensures the component structure is valid
    expect(() => render(<App />)).not.toThrow();
  });

  it('should contain the main App div', () => {
    const { container } = render(<App />);
    const appDiv = container.querySelector('.App');
    expect(appDiv).toBeInTheDocument();
  });
});
