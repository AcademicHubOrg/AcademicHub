import { Addresses } from "./Addresses";

export const deleteCourseStream = async (courseId: string) => {
    const response = await fetch(`${Addresses.COURSESTREAMS}/delete/${courseId}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json'
        }
    });
    return await response.json();
};