import { Addresses } from "./Addresses";

export const getCoursesList = async () => {
    const response = await fetch(`${Addresses.COURSESTREAMS}/list`);
    const data = await response.json();
    return data.data; // Assuming the API returns an array of courses
};
