import React from 'react';
import logo from './logo.svg';
import './App.css';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import CoursesListPage from './pages/CoursesListPage/CoursesListPage';
import AuthPage from './pages/AuthPage/AuthPage';

function App() {
  return (
      <Router>
        <Routes>
          <Route path="/courselist" element={<CoursesListPage />} />
          <Route path="/auth" element={<AuthPage />} />
        </Routes>
      </Router>
  );
}

export default App;
