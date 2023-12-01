import { Addresses } from './LocalHostAddresses';

export const addCourseStream = async (courseName: string, templateId: string) => {
    try {
        const response = await fetch(`${Addresses.COURSESTREAMS}/courseStreams/add`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                name: courseName,
                templateId: parseInt(templateId)
            })
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        console.log(data); // Process the response data as needed
        return data;
    } catch (error) {
        console.error('There was an error!', error);
        throw error;
    }
};