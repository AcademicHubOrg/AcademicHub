import React from 'react';
import { useNavigate } from 'react-router-dom';
import { CSSProperties } from 'react';
import { useMyContext } from '../../../MyContext';
import {deleteTemplate} from "../../../api/deleteTemplate";

const cellStyle: CSSProperties = {
    border: '1px solid black',
    padding: '10px',
    textAlign: 'center',
};

interface TemplateProps {
    templateName: string;
    templateId: string;
    onDelete: (templateId: string) => void;
}

const TemplateInstance: React.FC<TemplateProps> = ({ templateName, templateId, onDelete }) => {
    // useEffect(()=>{console.log(courseName)})
    const navigate = useNavigate();
    const { updateJsonData } = useMyContext();
    const { jsonData } = useMyContext();
    const handleCreateClick = () => {
        updateJsonData({
            templateIDJSON: templateId,
            loggedIn: jsonData.loggedIn
        })
        navigate(`/createCourse`);
    }
    const handleMaterialsClick = () => {
        updateJsonData({
            templateIDJSON: templateId,
            loggedIn: jsonData.loggedIn
        })
        navigate(`/viewTemplateMaterials`);
    }
    const handleDeleteClick = () => {
        deleteTemplate(templateId);
        onDelete(templateId);
    }
    return (
        <tr>
            <td style={cellStyle}>{templateId}</td>
            <td style={cellStyle}>{templateName}</td>
            <td style={cellStyle}>
                <button onClick={handleMaterialsClick}>Materials</button>
            </td>
            <td style={cellStyle}>
                <button onClick={handleCreateClick}>New Stream</button>
            </td>
            <td style={cellStyle}>
                <button onClick={handleDeleteClick}>Delete Template</button>
            </td>
        </tr>
    );
};

export default TemplateInstance;
