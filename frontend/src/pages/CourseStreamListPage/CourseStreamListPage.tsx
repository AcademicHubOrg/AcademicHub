import React, { CSSProperties, useEffect, useState } from 'react';
import CourseInstance from "./Components/CourseInstance";
import { getCoursesList } from '../../api/coursesListService';
import { useAuth0 } from "@auth0/auth0-react";
import LoginButton from "../../auth0components/auth0Login";

// TypeScript interfaces for type checking
interface Course {
    id: string;
    name: string;
    templateId: string;
}

const tableStyle: CSSProperties = {
    width: '100%',
    border: '1px solid black',
    borderCollapse: 'collapse',
    fontSize: '1.2rem'
};

const CourseStreamListPage = () => {
    const [courses, setCourses] = useState<Course[]>([]);
    const { user, isAuthenticated, isLoading} = useAuth0();

    useEffect(() => {
        getCoursesList()
            .then(courses => setCourses(courses))
            .catch(error => console.error('Error fetching courses data: ', error));
    }, []);

    if (isLoading) {
        return <div>Loading ...</div>;
    }

    if (!isAuthenticated || !user) {
        return(
            <div>
                <p>You are not authenticated. Please login</p>
                <LoginButton></LoginButton>
            </div>);
    }

    return (
        <div>
            <h1>Courses List</h1>
            <table style={tableStyle}>
                <thead>
                <tr>
                    <th>ID</th>
                    <th>TemplateID</th>
                    <th>Name</th>
                    <th>Edit Materials</th>
                    <th>Details</th>
                    <th>Delete Stream</th>
                </tr>
                </thead>
                <tbody>
                {courses.map(course => (
                    <CourseInstance key={course.id} courseName={course.name} courseID={course.id} templateId={course.templateId}/>
                ))}
                </tbody>
            </table>
        </div>
    );
};

export default CourseStreamListPage;


