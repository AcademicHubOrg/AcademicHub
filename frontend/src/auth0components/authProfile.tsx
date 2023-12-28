import { useAuth0 } from "@auth0/auth0-react";
import LoginButton from "./auth0Login";
import React, {CSSProperties, useEffect, useState} from "react";
import {loginService} from "../api/loginService";
import { useMyContext} from "../MyContext";
import {getEnrollments} from "../api/getEnrollments";
import EnrolledCourse from "./EnrolledCourse";

const tableStyle: CSSProperties = {
    width: '100%',
    border: '1px solid black',
    borderCollapse: 'collapse',
    fontSize: '1.2rem'
};

interface Course {
    id: string;
    name: string;
    templateId: string;
}

const Profile = () => {
    const { user, isAuthenticated, isLoading} = useAuth0();
    const { updateJsonData } = useMyContext();
    const { jsonData } = useMyContext();
    const [courses, setCourses] = useState<Course[]>([]);

    useEffect(() => {
        if (!jsonData.loggedIn) {
            if (isAuthenticated && user && user.name && user.email) {
                loginService(user.name, user.email)
                    .catch(error => console.error('POST request failed: ', error));
                updateJsonData({
                    loggedIn: true
                })
            }
        }
        if (user?.email) {
            getEnrollments(user.email)
                .then(responseCourses => {
                    setCourses(responseCourses); // responseCourses should already be in the correct format
                })
                .catch(error => console.error('Error fetching courses data: ', error));
        }

    }, [isAuthenticated, user, jsonData.loggedIn, updateJsonData]);

    if (isLoading) {
        return <div>Loading ...</div>;
    }

    if (!isAuthenticated || !user) {
        return(
            <div>
                <p>You are not authenticated. Please login</p>
                <LoginButton></LoginButton>
            </div>);
    }

    return (
        <div>
            <img src={user.picture} alt={user.name} />
            <h2>{user.name}</h2>
            <p>{user.email}</p>
            <p></p>
            <div>
                <h2>Your enrolled courses:</h2>
                <table style={tableStyle}>
                    <thead>
                    <tr>
                        <th>ID</th>
                        <th>TemplateID</th>
                        <th>Name</th>
                        <th>Details</th>
                        <th>Unenroll</th>
                    </tr>
                    </thead>
                    <tbody>
                    {courses.map(course => (
                        <EnrolledCourse key={course.id} courseName={course.name} courseID={course.id} templateId={course.templateId}/>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default Profile;
