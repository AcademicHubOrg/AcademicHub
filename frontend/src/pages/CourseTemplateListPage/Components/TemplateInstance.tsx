import React from 'react';
import { useNavigate } from 'react-router-dom';
import { CSSProperties } from 'react';
import { useMyContext } from '../../../MyContext';


const cellStyle: CSSProperties = {
    border: '1px solid black',
    padding: '10px',
    textAlign: 'center',
};

interface TemplateProps {
    templateName: string;
    templateId: string;
}

const TemplateInstance: React.FC<TemplateProps> = ({ templateName, templateId }) => {
    // useEffect(()=>{console.log(courseName)})
    const navigate = useNavigate();
    const { updateJsonData } = useMyContext();
    const handleCreateClick = () => {
        updateJsonData({
            templateIDJSON: templateId
        })
        navigate(`/createCourse`);
    }
    return (
        <tr>
            <td style={cellStyle}>{templateId}</td>
            <td style={cellStyle}>{templateName}</td>
            <td style={cellStyle}>
                <button onClick={handleCreateClick}>Create course</button>
            </td>
        </tr>
    );
};

export default TemplateInstance;
