import React, { useEffect, useState } from 'react';
import { getTemplateMaterials } from '../../api/getMaterialsService';
import { useMyContext } from '../../MyContext';
import {useNavigate} from "react-router-dom";

interface Material {
    name: string;
    dataText: string;
}

const ViewTemplateMaterials: React.FC = () => {
    const [materials, setMaterials] = useState<Material[]>([]);
    //deleted updateJsonData
    const { jsonData } = useMyContext();
    const templateId = jsonData.templateIDJSON;
    const navigate = useNavigate();
    const handleMaterialsClick = () => {
        navigate(`/addEssentialMaterial`);
    }
    const handleDeleteClick = () => {
        //call delete material endpoint
    }
    // Fetch data from the backend
    useEffect(() => {
        getTemplateMaterials(templateId as string)
            .then(materials => setMaterials(materials))
            .catch(error => console.error('Error fetching materials data: ', error));
    }, [templateId]);// dependency array includes courseId

    return (
        <div>
            <div>
                <h1>Materials for template {templateId}</h1>
                {materials.length > 0 ? (
                    materials.map(material => (
                        <div key={material.name}>
                            <h3>{material.name}</h3>
                            <p>{material.dataText}</p>
                            <button onClick={handleDeleteClick}>Delete</button>
                        </div>
                    ))
                ) : (
                    <p>No materials</p>
                )}
            </div>
            <button onClick={handleMaterialsClick}>Add material</button>
        </div>
    );
};

export default ViewTemplateMaterials;