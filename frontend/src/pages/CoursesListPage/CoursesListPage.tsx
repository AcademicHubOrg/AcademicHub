import React from 'react';

import coursesData from './courses.json';
import CourseInstance from "./Components/CourseInstance";

function CoursesListPage() {
    return (
        <div>
            {coursesData.courses.map((course, index) => (
                <CourseInstance key={index} courseName={course.courseName} />
            ))}
        </div>
    );
}

export default CoursesListPage;
