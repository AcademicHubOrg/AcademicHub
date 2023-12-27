import { Addresses } from "./Addresses";

export const deleteEssentialMaterial = async (materialId: string) => {
    const response = await fetch(`${Addresses.MATERIALS}/delete-essential/${materialId}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json'
        }
    });
    const data = response.json();
    if(response.ok){
        console.log('Material deleted successfully');
    }
    return data;
};