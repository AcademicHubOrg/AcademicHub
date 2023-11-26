import React from 'react';

interface CourseProps {
    courseName: string;
    courseID: string;
}

const CourseDetails: React.FC<CourseProps> = ({ courseName, courseID}) => {
    return (
        <div>
            <h1>{courseName}</h1>
            <h1>{courseID}</h1>
            <button onClick={() => alert(`Enrolled in ${courseName}`)}>Enroll</button>
        </div>
    );
};

export default CourseDetails;