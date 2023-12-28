import { Addresses } from "./Addresses";
import {getUserInfo} from "./getUserService";

interface User {
    id: string; // Assuming this is actually a string representation of an integer
    name: string;
    email: string;
}

export const getEnrollments = async (studentEmail: string) => {
    const user: User = await getUserInfo(studentEmail);
    const userId = user.id;
    const response = await fetch(`${Addresses.COURSESTREAMS}/checkEnrollements` + userId);
    const data = await response.json();
    return data.data; // Assuming the API returns an array of courses
};