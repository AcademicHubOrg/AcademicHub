// @ts-ignore
import React, { useEffect, useState } from 'react';
import CourseDetails from "./Components/CourseDetails";
import { getCourseDetails } from '../../api/courseService';
import { getCourseMaterials } from '../../api/getCourseMaterialService';
import { useMyContext } from '../../MyContext';
import RichTextEditor from "../../pages/RichTextEditor";
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
    //deleted updateJsonData
    const { jsonData } = useMyContext();
    const courseId = jsonData.courseIDJSON;
    const templateId = jsonData.templateIDJSON; // take from context here

    // Fetch data from the backend
    useEffect(() => {
        getCourseDetails(courseId as string)
            .then(course => setCourse(course))
            .catch(error => console.error('Error fetching course data: ', error));

        getCourseMaterials(courseId as string, templateId as string)
            .then(materials => setMaterials(materials))
            .catch(error => console.error('Error fetching materials data: ', error));
    }, [courseId, templateId]);// dependency array includes courseId

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