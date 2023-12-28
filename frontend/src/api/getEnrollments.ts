import { Addresses } from "./Addresses";
import { getUserInfo } from "./getUserService";
import {getCourseDetails} from "./courseService";

interface User {
    id: string;
    name: string;
    email: string;
}

interface Course {
    id: string;
    name: string;
    templateId: string;
}

export const getEnrollments = async (studentEmail: string) => {
    const user: User = await getUserInfo(studentEmail);
    const userId = user.id;
    const response = await fetch(`${Addresses.COURSESTREAMS}/getEnrolledCourses/` + userId);
    const data = await response.json();
    let courses: Course[] = [];

    for (const enrollment of data.data) {
        const courseData = await getCourseDetails(enrollment.courseStreamId);
        courses.push(courseData);
    }
    return courses;
};

