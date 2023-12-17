import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getCourseDetails } from '../../api/courseService';
import { getCourseStreamMaterials } from '../../api/getMaterialsService';
import { useMyContext } from '../../MyContext';

interface Material {
    name: string;
    dataText: string;
}

const ViewCourseMaterials: React.FC = () => {
    const [materials, setMaterials] = useState<Material[]>([]);
    //deleted updateJsonData
    const { jsonData } = useMyContext();
    const navigate = useNavigate();
    const courseId = jsonData.courseIDJSON;
    const handleMaterialsClick = () => {
        navigate(`/addMaterial`);
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
                            <button>Delete</button>
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