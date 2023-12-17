import { Addresses } from "./Addresses";

export const getCourseDetails = async (courseId: string) => {
    const response = await fetch(`${Addresses.COURSESTREAMS}/list`);
    const data = await response.json();
    return data.data[parseInt(courseId) - 1];
};

