import { Addresses } from "./Addresses";

export const deleteMaterial = async (materialId: string) => {
    const response = await fetch(`${Addresses.MATERIALS}/delete/${materialId}`, {
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