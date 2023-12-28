import React from 'react';
import { useNavigate } from 'react-router-dom';
import { CSSProperties } from 'react';
import { useMyContext } from '../MyContext';

const cellStyle: CSSProperties = {
    border: '1px solid black',
    padding: '10px',
    textAlign: 'center',
};

interface CourseProps {
    courseName: string;
    courseID: string;
    templateId: string;
}

const EnrolledCourse: React.FC<CourseProps> = ({ courseName, courseID, templateId}) => {
    const navigate = useNavigate();
    const { updateJsonData } = useMyContext();
    const { jsonData } = useMyContext();

    const handleDetailsClick = () => {
        updateJsonData({
            courseIDJSON: courseID,
            templateIDJSON: templateId,
            loggedIn: jsonData.loggedIn
        })
        navigate(`/course`); // write to context
    }

    const handleUnenrollClick = () => {
        //write logic here in the future
    }

    return (
        <tr>
            <td style={cellStyle}>{courseID}</td>
            <td style={cellStyle}>{templateId}</td>
            <td style={cellStyle}>{courseName}</td>
            <td style={cellStyle}>
                <button onClick={handleDetailsClick}>View Course</button>
            </td>
            <td style={cellStyle}>
                <button onClick={handleUnenrollClick}>Unenroll</button>
            </td>
        </tr>
    );
};

export default EnrolledCourse;