import { Addresses } from "./LocalHostAddresses";

export const getCourseMaterials = async (courseId: string) => {
    const response = await fetch(`${Addresses.MATERIALS}/materials/by-course/${courseId}`);
    const data = await response.json();
    return data; // Assuming the API returns an array of materials
};
