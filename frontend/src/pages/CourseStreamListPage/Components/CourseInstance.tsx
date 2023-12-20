import React from 'react';
import { useNavigate } from 'react-router-dom';
import { CSSProperties } from 'react';
import { useMyContext } from '../../../MyContext';

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

const CourseInstance: React.FC<CourseProps> = ({ courseName, courseID, templateId}) => {
    // useEffect(()=>{console.log(courseName)})
    const navigate = useNavigate();
    const { updateJsonData } = useMyContext();
    const handleDetailsClick = () => {
        updateJsonData({
            courseIDJSON: courseID,
            templateIDJSON: templateId
        })
        navigate(`/course`); // write to context
    }
    const handleMaterialsClick = () => {
        updateJsonData({
            courseIDJSON: courseID
        })
        navigate(`/viewCourseMaterials`); // write to context
    }
    const handleDeleteClick = () => {
        updateJsonData({
            courseIDJSON: courseID
        })
        //call delete course stream endpoint
    }

    return (
        <tr>
            <td style={cellStyle}>{courseID}</td>
            <td style={cellStyle}>{templateId}</td>
            <td style={cellStyle}>{courseName}</td>
            <td style={cellStyle}>
                <button onClick={handleMaterialsClick}>Edit Materials</button>
            </td>
            <td style={cellStyle}>
                <button onClick={handleDetailsClick}>View Course</button>
            </td>
            <td style={cellStyle}>
                <button onClick={handleDeleteClick}>Delete</button>
            </td>
        </tr>
    );
};

export default CourseInstance;
