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
import ViewCourseMaterials from "./pages/ViewCourseMaterialsPage/ViewCourseMaterials";
import ViewTemplateMaterials from "./pages/ViewTemplateMaterialsPage/ViewTemplateMaterials";
import AddMaterialPage from "./pages/AddMaterialPage/AddMaterialPage";
import AddEssentialMaterialPage from "./pages/AddEssentialMaterialPage/AddEssentialMaterialPage";

function App() {
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
                <Route path="/viewCourseMaterials" element={<ViewCourseMaterials />} />
                <Route path="/viewTemplateMaterials" element={<ViewTemplateMaterials />} />
                <Route path="/addMaterial" element={<AddMaterialPage />} />
                <Route path="/addEssentialMaterial" element={<AddEssentialMaterialPage />} />
            </Routes>
        </Router>
    );
}

export default App;
