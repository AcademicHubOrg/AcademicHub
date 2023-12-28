import { Addresses } from "./Addresses";
import { getUserInfo } from "./getUserService";

interface User {
    id: string;
    name: string;
    email: string;
}

export const getEnrollments = async (studentEmail: string) => {
    const user: User = await getUserInfo(studentEmail);
    const userId = user.id;
    const response = await fetch(`${Addresses.COURSESTREAMS}/getEnrolledCourses/` + userId);
    const data = await response.json();
    let courses = [];

    for (const enrollment of data.data) {
        const courseResponse = await fetch(`${Addresses.COURSESTREAMS}/` + enrollment.courseStreamId);
        const courseData = await courseResponse.json();
        courses.push(courseData);
    }
    return courses;
};
