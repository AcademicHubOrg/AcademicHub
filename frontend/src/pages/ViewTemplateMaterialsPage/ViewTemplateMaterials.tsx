import React, { useEffect, useState } from 'react';
import { getTemplateMaterials } from '../../api/getMaterialsService';
import { useMyContext } from '../../MyContext';
import {useNavigate} from "react-router-dom";
import {deleteEssentialMaterial} from "../../api/deleteEssentialMaterial";

interface Material {
    id: string;
    name: string;
    dataText: string;
}

const ViewTemplateMaterials: React.FC = () => {
    const [materials, setMaterials] = useState<Material[]>([]);
    //deleted updateJsonData
    const { jsonData } = useMyContext();
    const templateId = jsonData.templateIDJSON;
    const navigate = useNavigate();

    const removeMaterialFromList = (id: string) => {
        setMaterials(prevMaterials => prevMaterials.filter(material => material.id !== id));
    };

    const handleMaterialsClick = () => {
        navigate(`/addEssentialMaterial`);
    }
    const handleDeleteClick = (material: Material) => {
        return () => {
            if (window.confirm(`Are you sure you want to delete material: "${material.name}"?`)) {
                deleteEssentialMaterial(material.id)
                    .then(() => {
                        removeMaterialFromList(material.id);
                    })
                    .catch(error => {
                        console.error('Error deleting the material:', error);
                    });
            }
        };
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
                            <button onClick={handleDeleteClick(material)}>Delete</button>
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