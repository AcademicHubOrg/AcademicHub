import { Addresses } from "./LocalHostAddresses";

export const getCourseDetails = async (courseId: string) => {
    const response = await fetch(`${Addresses.COURSESTREAMS}/courseStreams/list`);
    const data = await response.json();
    return data.data[parseInt(courseId) - 1];
};

