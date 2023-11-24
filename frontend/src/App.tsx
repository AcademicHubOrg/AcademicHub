import React from 'react';
import './App.css';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import CoursesListPage from './pages/CoursesListPage/CoursesListPage';
import AuthPage from './pages/AuthPage/AuthPage';
import AddCoursePage from './pages/AddCoursePage/AddCoursePage';
import AddTemplatePage from "./pages/AddTemplate/TemplatePage";
import TopMenu from "./pages/TopMenu/TopMenu";
import TemplateList from "./pages/TemplateList/TemplateListPage";
import CoursePage from "./pages/CoursePage/CoursePage";

function App() {
  return (
      <Router>
          <TopMenu/>
        <Routes>
            <Route path="/courselist" element={<CoursesListPage />} />
            <Route path="/auth" element={<AuthPage />} />
            <Route path="/addcourse" element={<AddCoursePage />} />
            <Route path="/template" element={<AddTemplatePage />} />
            <Route path="/templatelist" element={<TemplateList />} />
            <Route path="/course/:courseId" element={<CoursePage/>} />
        </Routes>
      </Router>
  );
}

export default App;
