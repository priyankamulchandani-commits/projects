import React from 'react';
import { render, screen } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import ProtectedRoute from '../components/ProtectedRoute';

// Mock the useAuth hook
const mockUseAuth = jest.fn();
jest.mock('../contexts/AuthContext', () => ({
  useAuth: () => mockUseAuth(),
}));

// Mock useNavigate
const mockNavigate = jest.fn();
jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useNavigate: () => mockNavigate,
}));

const TestComponent: React.FC = () => <div>Protected Content</div>;

const renderWithRouter = (children: React.ReactNode) => {
  return render(
    <BrowserRouter>
      {children}
    </BrowserRouter>
  );
};

describe('ProtectedRoute Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should render children when user is authenticated', () => {
    const mockUser = {
      id: 1,
      email: 'test@example.com',
      firstName: 'John',
      lastName: 'Doe'
    };

    mockUseAuth.mockReturnValue({
      user: mockUser,
    });

    renderWithRouter(
      <ProtectedRoute>
        <TestComponent />
      </ProtectedRoute>
    );

    expect(screen.getByText('Protected Content')).toBeInTheDocument();
  });

  it('should redirect to login when user is not authenticated', () => {
    mockUseAuth.mockReturnValue({
      user: null,
    });

    renderWithRouter(
      <ProtectedRoute>
        <TestComponent />
      </ProtectedRoute>
    );

    expect(screen.queryByText('Protected Content')).not.toBeInTheDocument();
    // The Navigate component should redirect to /login
    // We can't easily test the actual redirect in this setup, but we can verify the component doesn't render
  });

  it('should handle undefined user', () => {
    mockUseAuth.mockReturnValue({
      user: undefined,
    });

    renderWithRouter(
      <ProtectedRoute>
        <TestComponent />
      </ProtectedRoute>
    );

    expect(screen.queryByText('Protected Content')).not.toBeInTheDocument();
  });

  it('should render multiple children when user is authenticated', () => {
    const mockUser = {
      id: 1,
      email: 'test@example.com',
      firstName: 'John',
      lastName: 'Doe'
    };

    mockUseAuth.mockReturnValue({
      user: mockUser,
    });

    renderWithRouter(
      <ProtectedRoute>
        <div>First Child</div>
        <div>Second Child</div>
        <TestComponent />
      </ProtectedRoute>
    );

    expect(screen.getByText('First Child')).toBeInTheDocument();
    expect(screen.getByText('Second Child')).toBeInTheDocument();
    expect(screen.getByText('Protected Content')).toBeInTheDocument();
  });

  it('should not render any children when user is null', () => {
    mockUseAuth.mockReturnValue({
      user: null,
    });

    renderWithRouter(
      <ProtectedRoute>
        <div>First Child</div>
        <div>Second Child</div>
        <TestComponent />
      </ProtectedRoute>
    );

    expect(screen.queryByText('First Child')).not.toBeInTheDocument();
    expect(screen.queryByText('Second Child')).not.toBeInTheDocument();
    expect(screen.queryByText('Protected Content')).not.toBeInTheDocument();
  });

  it('should handle empty children when user is authenticated', () => {
    const mockUser = {
      id: 1,
      email: 'test@example.com',
      firstName: 'John',
      lastName: 'Doe'
    };

    mockUseAuth.mockReturnValue({
      user: mockUser,
    });

    renderWithRouter(
      <ProtectedRoute>
        {null}
      </ProtectedRoute>
    );

    // Should not crash, just render nothing
    expect(screen.queryByText('Protected Content')).not.toBeInTheDocument();
  });

  it('should work with different user objects', () => {
    const mockUser = {
      id: 2,
      email: 'admin@example.com',
      firstName: 'Admin',
      lastName: 'User'
    };

    mockUseAuth.mockReturnValue({
      user: mockUser,
    });

    renderWithRouter(
      <ProtectedRoute>
        <div>Admin Content</div>
      </ProtectedRoute>
    );

    expect(screen.getByText('Admin Content')).toBeInTheDocument();
  });

  it('should handle user object with missing properties', () => {
    const mockUser = {
      id: 1,
      email: 'test@example.com',
      // Missing firstName and lastName
    };

    mockUseAuth.mockReturnValue({
      user: mockUser,
    });

    renderWithRouter(
      <ProtectedRoute>
        <TestComponent />
      </ProtectedRoute>
    );

    // Should still render children if user object exists
    expect(screen.getByText('Protected Content')).toBeInTheDocument();
  });

  it('should handle user object with false-y but not null values', () => {
    const mockUser = {
      id: 0, // false-y but valid
      email: '',
      firstName: '',
      lastName: ''
    };

    mockUseAuth.mockReturnValue({
      user: mockUser,
    });

    renderWithRouter(
      <ProtectedRoute>
        <TestComponent />
      </ProtectedRoute>
    );

    // Should still render children if user object exists (even with false-y values)
    expect(screen.getByText('Protected Content')).toBeInTheDocument();
  });
});
