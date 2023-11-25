import React, {CSSProperties, useEffect, useState} from 'react';

import CourseInstance from "./Components/CourseInstance";
import {Addresses} from "../../LocalHostAddresses";

// TypeScript interfaces for type checking
interface Course {
    id: string;
    name: string;
}

const tableStyle: CSSProperties = {
    width: '100%',      // Makes the table full-width
    border: '1px solid black',
    borderCollapse: 'collapse',
    fontSize: '1.2rem'  // Larger font size
};

const CoursesList = () => {
    const [courses, setCourses] = useState<Course[]>([]);

    // Fetch data from the backend
    useEffect(() => {
        fetch(`${Addresses.COURSESTREAMS}/courseStreams/list`)
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
            <h1>Courses List</h1>
            <table style={tableStyle}>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Details</th>
                    </tr>
                </thead>
                <tbody>
                    {courses.map(course => (
                        <CourseInstance key={course.id} courseName={course.name} courseID={course.id}/>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default CoursesList;

