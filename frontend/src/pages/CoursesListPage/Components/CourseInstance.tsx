import React from 'react';

interface CourseProps {
    courseName: string;
    courseID: string;
}

const CourseInstance: React.FC<CourseProps> = ({ courseName, courseID }) => {
    // useEffect(()=>{console.log(courseName)})
    return (
        <div>
            <h1>{courseName}</h1>
            <h1>{courseID}</h1>
            <button onClick={() => alert(`Enrolled in ${courseName}`)}>Enroll</button>
        </div>
    );
};

export default CourseInstance;
