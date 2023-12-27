import React from 'react';
import { useNavigate } from 'react-router-dom';
import { CSSProperties } from 'react';
import { useMyContext } from '../../../MyContext';
import {deleteCourseStream} from "../../../api/deleteCourseStream";

const cellStyle: CSSProperties = {
    border: '1px solid black',
    padding: '10px',
    textAlign: 'center',
};

interface CourseProps {
    courseName: string;
    courseID: string;
    templateId: string;
    onDelete: (courseId: string) => void;
}

const CourseInstance: React.FC<CourseProps> = ({ courseName, courseID, templateId, onDelete}) => {
    // useEffect(()=>{console.log(courseName)})
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

    const handleMaterialsClick = () => {
        updateJsonData({
            courseIDJSON: courseID,
            loggedIn: jsonData.loggedIn
        })
        navigate(`/viewCourseMaterials`); // write to context
    }

    const handleDeleteClick = () => {
        if (window.confirm(`Are you sure you want to delete the course: ${courseName}?`)) {
            deleteCourseStream(courseID)
                .then(() => {
                    onDelete(courseID);
                })
                .catch(error => {
                    console.error('Error deleting the course stream:', error);
                });
        }
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
