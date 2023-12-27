import { Addresses } from "./Addresses";

export const deleteTemplate = async (templateId: string) => {
    const response = await fetch(`${Addresses.COURSETEMPLATES}/delete/${templateId}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json'
        }
    });
    const data = response.json();
    if(response.ok){
        console.log('Template deleted successfully');
    }
    return data;
};