import React, { useEffect, useState } from 'react';

import TemplateInstance from "./Components/TemplateInstance";
import {Addresses} from "../../LocalHostAddresses";

// TypeScript interfaces for type checking
interface CourseTemplate {
    id: string;
    name: string;
}

const TemplateList = () => {
    const [courseTemplates, setCourses] = useState<CourseTemplate[]>([]);

    // Fetch data from the backend
    useEffect(() => {
        fetch(`${Addresses.COURSETEMPLATES}/courseTemplates/list`)
            .then(response => response.json())
            .then(data => {
                // Assuming the data is an array of courses
                // console.log(data);
                setCourses(data.data);
            })
            .catch(error => {
                console.error('Error fetching data: ', error);
            });
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

export default TemplateList;

