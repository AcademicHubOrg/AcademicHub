import { Addresses } from './Addresses';
import { getUserInfo } from "./getUserService";

interface EnrollmentStatus {
    courseId: string;
    studentId: string;
    status: string;
    // Add other relevant properties as needed
}

interface User {
    id: string;
    name: string;
    email: string;
}

export const getStatusEnrollService = async (courseStreamId: string, studentEmail: string): Promise<EnrollmentStatus> => {
    try {
        const user: User = await getUserInfo(studentEmail);
        const studentId = parseInt(user.id);
        const streamId = parseInt(courseStreamId);

        const url = new URL(`${Addresses.COURSESTREAMS}/isEnrolled/` + studentId + '/' + streamId);


        const response = await fetch(url.toString(), {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
                // Add any additional headers as needed
            },
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        // Assuming the response is JSON
        const data: EnrollmentStatus = await response.json();
        console.log(data); // Process the response data as needed
        return data;
    } catch (error) {
        console.error('There was an error!', error);
        throw error;
    }
};
