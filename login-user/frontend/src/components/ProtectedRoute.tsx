import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

interface SimpleProtectedRouteProps {
  children: React.ReactNode;
}

const SimpleProtectedRoute: React.FC<SimpleProtectedRouteProps> = ({ children }) => {
  const { user } = useAuth();
  
  if (!user) {
    return <Navigate to="/login" replace />;
  }
  
  return <>{children}</>;
};

export default SimpleProtectedRoute;
