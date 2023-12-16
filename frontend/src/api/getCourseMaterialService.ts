import { Addresses } from "./LocalHostAddresses";

export const getCourseMaterials = async (courseId: string, templateId: string) => {
    const response = await fetch(`${Addresses.MATERIALS}/by-course/${courseId}`);
    const response2 = await fetch(`${Addresses.MATERIALS}/by-template/${templateId}`);
    let data1 = [];
    let data2 = [];
    if(response.ok){
        data1 = await response.json();
    }
    if(response2.ok){
        data2 = await response2.json();
    }
    return [...data1, ...data2]; // Assuming the API returns an array of materials
};
