import React, {useState} from 'react';
import { addEssentialMaterial } from '../../api/addMaterialService';
import { useMyContext } from '../../MyContext';

const AddEssentialMaterialPage: React.FC = () => {
    const { jsonData} = useMyContext();
    const templateId = jsonData.templateIDJSON;
    const [materialName, setMaterialName] = useState('');
    const [materialData, setMaterialData] = useState('');
    const [displayedText, setDisplayedText] = useState('');

    const handleButtonClick = () => {
        addEssentialMaterial(materialName, materialData, templateId as string) // Send the POST request
            .catch(error => console.error('POST request failed: ', error));
        setDisplayedText(`Course "${materialName}" created`);
    };

    return (
        <div>
            <h1>Create Material</h1>
            <input
                type="text"
                value={materialName}
                onChange={(e) => setMaterialName(e.target.value)}
                placeholder="Material Name"
            />
            <input
                type="text"
                value={materialData}
                onChange={(e) => setMaterialData(e.target.value)}
                placeholder="Material Data"
            />
            <button onClick={handleButtonClick}>Create</button>
            {displayedText &&<p>{displayedText}</p>}
        </div>

    );
};

export default AddEssentialMaterialPage;