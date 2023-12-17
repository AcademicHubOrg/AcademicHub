import { Addresses } from './Addresses';

export const addMaterial = async (materialName: string, materialData: string, courseId: string) => {
    try {
        const response = await fetch(`${Addresses.MATERIALS}/add`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                name: materialName,
                dataText: materialData,
                courseId: courseId
            })
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        console.log(data); // Process the response data as needed
        return data;
    } catch (error) {
        console.error('There was an error!', error);
        throw error;
    }
};

export const addEssentialMaterial = async (materialName: string, materialData: string, templateId: string) => {
    try {
        const response = await fetch(`${Addresses.MATERIALS}/add-essential`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                name: materialName,
                dataText: materialData,
                courseId: templateId
            })
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        console.log(data); // Process the response data as needed
        return data;
    } catch (error) {
        console.error('There was an error!', error);
        throw error;
    }
};