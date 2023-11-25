import React from 'react';
import { useNavigate } from 'react-router-dom';
import { CSSProperties } from 'react';

const cellStyle: CSSProperties = {
    border: '1px solid black',
    padding: '10px',
    textAlign: 'center',
};

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
        <tr>
            <td style={cellStyle}>{courseID}</td>
            <td style={cellStyle}>{courseName}</td>
            <td style={cellStyle}>
                <button onClick={handleDetailsClick}>Details</button>
            </td>
        </tr>
    );
};

export default CourseInstance;
