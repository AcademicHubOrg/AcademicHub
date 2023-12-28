import { Addresses } from './Addresses';
import {getUserInfo} from "./getUserService";

interface User {
    id: string;
    name: string;
    email: string;
}

export const enrollCourseService = async (courseStreamId: string, studentEmail: string) => {
    try {
        const user: User = await getUserInfo(studentEmail);
        const studentId = parseInt(user.id);
        const streamId = parseInt(courseStreamId);

        const url = new URL(`${Addresses.COURSESTREAMS}/enroll`);
        url.searchParams.append('studentId', studentId.toString());
        url.searchParams.append('courseStreamId', streamId.toString());

        const response = await fetch(url.toString(), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        // Assuming the response is JSON
        const data = await response.json();
        console.log(data); // Process the response data as needed
        return data;
    } catch (error) {
        console.error('There was an error!', error);
        throw error;
    }
};
