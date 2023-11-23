import React from 'react';
import { useNavigate } from 'react-router-dom';

interface CourseProps {
    courseName: string;
    courseID: string;
}

const CourseInstance: React.FC<CourseProps> = ({ courseName, courseID}) => {
    // useEffect(()=>{console.log(courseName)})
    const navigate = useNavigate();
    const handleDetailsClick = () => {
        navigate(`/course/${courseID}`);
    }
    return (
        <div>
            <h1>{courseName}</h1>
            <h1>{courseID}</h1>
            <button onClick={handleDetailsClick}>Details</button>
        </div>
    );
};

export default CourseInstance;
