import { Addresses } from "./Addresses";

export const deleteTemplate = async (templateId: string) => {
    const response = await fetch(`${Addresses.COURSETEMPLATES}/delete/${templateId}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json'
        }
    });
    return await response.json();
};