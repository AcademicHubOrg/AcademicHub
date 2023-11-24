import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import CourseDetails from "./Components/CourseDetails";
import {Addresses} from "../../LocalHostAddresses";

interface Course {
    id: string;
    name: string;
    materials: string;
}

const CoursePage: React.FC = () => {
    const [course, setCourse] = useState<Course | null>(null);
    const { courseId } = useParams<{ courseId: string }>();

    // Fetch data from the backend
    useEffect(() => {
        fetch(`${Addresses.COURSESTREAMS}/courseStreams/list`)
            .then(response => response.json())
            .then(data => {
                console.log(data);
                setCourse(data.data[parseInt(courseId as string) - 1]);
            })
            .catch(error => {
                console.error('Error fetching data: ', error);
            });
    }, [courseId]); // dependency array includes courseId

    return (
        <div>
            <h1>Course Details</h1>
            {course ? (
                <CourseDetails courseName={course.name} courseID={course.id}/>
            ) : (
                <p>Loading course data for course ID: {courseId}...</p>
            )}
        </div>
    );
};

export default CoursePage;