import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import CourseDetails from "./Components/CourseDetails";
import {Addresses} from "../../LocalHostAddresses";

interface Course {
    id: string;
    name: string;
}

interface Material {
    name: string;
    dataText: string;
}

const CoursePage: React.FC = () => {
    const [course, setCourse] = useState<Course | null>(null);
    const [materials, setMaterials] = useState<Material[]>([]);
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
        fetch(`https://localhost:5116/materials/by-course/${courseId}`)
            .then(response => response.json())
            .then(data => {
                console.log('Material Data:', data);
                setMaterials(data); // Assuming the API returns an array of materials
            })
            .catch(error => {
                console.error('Error fetching materials data: ', error);
            });
    }, [courseId]);// dependency array includes courseId



    return (
        <div>
            <h1>Course Details</h1>
            {course ? (
                <CourseDetails courseName={course.name} courseID={course.id}/>
            ) : (
                <p>Loading course data for course ID: {courseId}...</p>
            )}
            <div>
                <h2>Materials</h2>
                {materials.length > 0 ? (
                    materials.map(material => (
                        <div key={material.name}>
                            <h3>{material.name}</h3>
                            <p>{material.dataText}</p>
                        </div>
                    ))
                ) : (
                    <p>Loading materials...</p>
                )}
            </div>
        </div>
    );
};

export default CoursePage;