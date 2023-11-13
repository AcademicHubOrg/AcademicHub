import React from 'react';

interface CourseProps {
    courseName: string;
}

const CourseInstance: React.FC<CourseProps> = ({ courseName }) => {
    return (
        <div>
            <h3>{courseName}</h3>
            <button onClick={() => alert(`Enrolled in ${courseName}`)}>Enroll</button>
        </div>
    );
};

export default CourseInstance;
