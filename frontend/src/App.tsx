import React, { useEffect } from 'react';
import './App.css';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import CourseStreamListPage from './pages/CourseStreamListPage/CourseStreamListPage';
import AuthPage from './pages/AuthPage/AuthPage';
import AddCourseTemplatePage from "./pages/AddCourseTemplatePage/AddCourseTemplatePage";
import TopMenu from "./pages/TopMenu/TopMenu";
import CourseTemplateListPage from "./pages/CourseTemplateListPage/CourseTemplateListPage";
import CoursePage from "./pages/CoursePage/CoursePage";
import CreateCoursePage from "./pages/CreateCoursePage/CreateCoursePage";
import Profile from "./pages/ProfilePage/Profile";

function App() {
    useEffect(() => {
        const socket = new WebSocket('wss://academichub.net/ws');

        socket.addEventListener('open', (event) => {
            console.log('WebSocket connection opened');
        });

        socket.addEventListener('message', (event) => {
            console.log('Message from server ', event.data);
        });

        socket.addEventListener('error', (error) => {
            console.error('WebSocket error:', error);
        });

        socket.addEventListener('close', (event) => {
            console.log('WebSocket connection closed');
        });

        return () => {
            socket.close();
        };
    }, []);

    return (
        <Router>
            <TopMenu />
            <Routes>
                <Route path="/courselist" element={<CourseStreamListPage />} />
                <Route path="/auth" element={<AuthPage />} />
                <Route path="/template" element={<AddCourseTemplatePage />} />
                <Route path="/templatelist" element={<CourseTemplateListPage />} />
                <Route path="/course" element={<CoursePage />} />
                <Route path="/createCourse" element={<CreateCoursePage />} />
                <Route path="/profile" element={<Profile />} />
            </Routes>
        </Router>
    );
}

export default App;
