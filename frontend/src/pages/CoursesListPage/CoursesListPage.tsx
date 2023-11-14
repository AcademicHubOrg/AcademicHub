import React, { useEffect, useState } from 'react';

import CourseInstance from "./Components/CourseInstance";

// TypeScript interfaces for type checking
interface Course {
    id: string;
    name: string;
}

const CoursesList = () => {
    const [courses, setCourses] = useState<Course[]>([]);

    // Fetch data from the backend
    useEffect(() => {
        fetch('http://localhost:5237/courseStream/list')
            .then(response => response.json())
            .then(data => {
                // Assuming the data is an array of courses
                // console.log(data);
                setCourses(data);
            })
            .catch(error => {
                console.error('Error fetching data: ', error);
            });
    }, []);

    return (
        <div>
            <h1>Courses List</h1>
            <ul>
                {courses.map(course => (
                    <CourseInstance key={course.id} courseName={course.name} courseID={course.id}/>
                ))}
            </ul>
        </div>
    );
};

export default CoursesList;

