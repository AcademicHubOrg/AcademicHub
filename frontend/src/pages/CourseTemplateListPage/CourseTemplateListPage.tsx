import React, {CSSProperties, useEffect, useState} from 'react';

import TemplateInstance from "./Components/TemplateInstance";
import { getCourseTemplates } from '../../api/templateListService';
import {useAuth0} from "@auth0/auth0-react";
import LoginButton from "../../auth0components/auth0Login";

const tableStyle: CSSProperties = {
    width: '100%',      // Makes the table full-width
    border: '1px solid black',
    borderCollapse: 'collapse',
    fontSize: '1.2rem'  // Larger font size
};

// TypeScript interfaces for type checking
interface CourseTemplate {
    id: string;
    name: string;
}

const CourseTemplateListPage = () => {
    const [courseTemplates, setCourses] = useState<CourseTemplate[]>([]);
    const { user, isAuthenticated, isLoading} = useAuth0();

    const removeTemplateFromList = (templateId: string) => {
        setCourses(prevCourses => prevCourses.filter(template => template.id !== templateId));
    };
    // Fetch data from the backend
    useEffect(() => {
        getCourseTemplates()
            .then(templates => setCourses(templates))
            .catch(error => console.error('Error fetching templates data: ', error));
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
            <h1>Template List</h1>
            <ul>
            </ul>
            <table style={tableStyle}>
                <thead>
                <tr>
                    <th>TemplateID</th>
                    <th>Name</th>
                    <th>Edit Materials</th>
                    <th>Create Stream</th>
                    <th>Delete template</th>
                </tr>
                </thead>
                <tbody>
                {courseTemplates.map(course => (
                    <TemplateInstance key={course.id} templateName={course.name} templateId={course.id} onDelete={removeTemplateFromList}/>
                ))}
                </tbody>
            </table>
        </div>
    );
};

export default CourseTemplateListPage;

