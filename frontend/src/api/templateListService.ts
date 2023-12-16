import { Addresses } from "./LocalHostAddresses";

export const getCourseTemplates = async () => {
    const response = await fetch(`${Addresses.COURSETEMPLATES}/list`);
    const data = await response.json();
    return data.data; // Assuming the API returns an array of course templates
};
