import React, {CSSProperties, useEffect, useState} from 'react';

import TemplateInstance from "./Components/TemplateInstance";
import { getCourseTemplates } from '../../api/templateListService';
import RichTextEditor from "../../pages/RichTextEditor";

// Example using RichTextEditor:
/*const YourPageComponent: React.FC = () => {
    const handleFileUpload = (fileContent: string) => {
        // Do something with the file content, e.g., save it or display it
        console.log('File content:', fileContent);
    };

    return (
        <div>
            <h1>Your Page</h1>
            <RichTextEditor onFileUpload={handleFileUpload} />
        </div>
    );
};*/

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
            </ul>
            <table style={tableStyle}>
                <thead>
                <tr>
                    <th>TemplateID</th>
                    <th>Name</th>
                    <th>Create</th>
                </tr>
                </thead>
                <tbody>
                {courseTemplates.map(course => (
                    <TemplateInstance key={course.id} templateName={course.name} templateId={course.id}/>
                ))}
                </tbody>
            </table>
        </div>
    );
};

export default CourseTemplateListPage;

