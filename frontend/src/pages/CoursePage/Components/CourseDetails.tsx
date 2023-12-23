import React from 'react';
import {useAuth0} from "@auth0/auth0-react";
import LoginButton from "../../../auth0components/auth0Login";
import {enrollCourseService} from "../../../api/enrollCourseService";

interface CourseProps {
    courseName: string;
    courseID: string;
}

const CourseDetails: React.FC<CourseProps> = ({ courseName, courseID}) => {
    const { isAuthenticated, isLoading, user} = useAuth0();

    if (isLoading) {
        return <div>Loading ...</div>;
    }

    if (!isAuthenticated) {
        return(
            <div>
                <p>You are not authenticated. Please login</p>
                <LoginButton></LoginButton>
            </div>);
    }

    const handleEnrollClick = () => {
        if (user && user.email) {
            enrollCourseService(courseID, user.email);
            alert(`Enrolled in ${courseName}`);
        } else {
            // Handle the case where user or user.email is undefined
            alert("User email is not available.");
        }
    };

    return (
        <div>
            <h1>{courseName}</h1>
            <h1>{courseID}</h1>
            <button onClick={handleEnrollClick}>Enroll</button>
        </div>
    );
};

export default CourseDetails;