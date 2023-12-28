import { Addresses } from "./Addresses";

export const getCourseDetails = async (courseId: string) => {
    const courseResponse = await fetch(`${Addresses.COURSESTREAMS}/` + courseId);
    const courseData = await courseResponse.json();
    return courseData;
};

