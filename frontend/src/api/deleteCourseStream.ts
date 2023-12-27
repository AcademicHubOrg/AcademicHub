import { Addresses } from "./Addresses";

export const deleteCourseStream = async (courseId: string) => {
    const response = await fetch(`${Addresses.COURSESTREAMS}/delete/${courseId}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json'
        }
    });
    const data = response.json();
    if(response.ok){
        console.log('Course stream deleted successfully');
    }
    return data;
};