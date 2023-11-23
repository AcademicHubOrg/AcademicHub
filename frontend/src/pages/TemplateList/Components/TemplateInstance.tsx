import React from 'react';

interface TemplateProps {
    courseName: string;
    courseID: string;
}

const TemplateInstance: React.FC<TemplateProps> = ({ courseName, courseID }) => {
    // useEffect(()=>{console.log(courseName)})
    return (
        <div>
            <h1>{courseName}</h1>
            <h1>{courseID}</h1>
            <button onClick={() => alert(`Enrolled in ${courseName}`)}>Enroll</button>
        </div>
    );
};

export default TemplateInstance;
