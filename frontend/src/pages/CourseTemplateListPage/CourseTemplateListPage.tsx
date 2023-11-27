import React, { useEffect, useState } from 'react';

import TemplateInstance from "./Components/TemplateInstance";
import { getCourseTemplates } from '../../api/templateListService';

// TypeScript interfaces for type checking
interface CourseTemplate {
    id: string;
    name: string;
}

const CourseTemplateListPage = () => {
    const [courseTemplates, setCourses] = useState<CourseTemplate[]>([]);

    // Fetch data from the backend
    useEffect(() => {
        getCourseTemplates()
            .then(templates => setCourses(templates))
            .catch(error => console.error('Error fetching templates data: ', error));
    }, []);

    return (
        <div>
            <h1>Template List</h1>
            <ul>
                {courseTemplates.map(course => (
                    <TemplateInstance key={course.id} courseName={course.name} courseID={course.id}/>
                ))}
            </ul>
        </div>
    );
};

export default CourseTemplateListPage;

