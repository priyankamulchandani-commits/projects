import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { SimpleAuthProvider } from './contexts/SimpleAuthContext';
import SimpleLogin from './components/SimpleLogin';
import SimpleRegister from './components/SimpleRegister';
import SimpleProfile from './components/SimpleProfile';
import SimpleProtectedRoute from './components/SimpleProtectedRoute';
import './App.css';

function App() {
  return (
    <SimpleAuthProvider>
      <Router>
        <div className="App">
          <Routes>
            <Route path="/login" element={<SimpleLogin />} />
            <Route path="/register" element={<SimpleRegister />} />
            <Route 
              path="/profile" 
              element={
                <SimpleProtectedRoute>
                  <SimpleProfile />
                </SimpleProtectedRoute>
              } 
            />
            <Route path="/" element={<Navigate to="/login" replace />} />
          </Routes>
        </div>
      </Router>
    </SimpleAuthProvider>
  );
}

export default App;
