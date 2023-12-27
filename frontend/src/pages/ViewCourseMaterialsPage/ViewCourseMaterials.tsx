import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getCourseStreamMaterials } from '../../api/getMaterialsService';
import { useMyContext } from '../../MyContext';
import {deleteMaterial} from "../../api/deleteMaterial";

interface Material {
    id: string
    name: string;
    dataText: string;
}

const ViewCourseMaterials: React.FC = () => {
    const [materials, setMaterials] = useState<Material[]>([]);
    const { jsonData } = useMyContext();
    const navigate = useNavigate();
    const courseId = jsonData.courseIDJSON;

    const removeMaterialFromList = (id: string) => {
        setMaterials(prevMaterials => prevMaterials.filter(material => material.id !== id));
    };

    const handleMaterialsClick = () => {
        navigate(`/addMaterial`);
    }
    const handleDeleteClick = (material: Material) => {
        return () => {
            if (window.confirm(`Are you sure you want to delete material: "${material.name}"?`)) {
                deleteMaterial(material.id)
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
        getCourseStreamMaterials(courseId as string)
            .then(materials => setMaterials(materials))
            .catch(error => console.error('Error fetching materials data: ', error));
    }, [courseId]);// dependency array includes courseId

    return (
        <div>
            <div>
                <h1>Materials for courseStream {courseId}</h1>
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
                <button onClick={handleMaterialsClick}>Add material</button>
            </div>
        </div>
    );
};

export default ViewCourseMaterials;