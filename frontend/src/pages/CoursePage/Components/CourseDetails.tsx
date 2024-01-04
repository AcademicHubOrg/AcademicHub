import React, {useEffect, useState} from 'react';
import {useAuth0} from "@auth0/auth0-react";
import LoginButton from "../../../auth0components/auth0Login";
import {enrollCourseService} from "../../../api/enrollCourseService";
import {getStatusEnrollService} from "../../../api/statusEnrollService";
import {loginService} from "../../../api/loginService";
import {getEnrollments} from "../../../api/getEnrollments";
import {unenroll} from "../../../api/UnEnrollService";
interface CourseProps {
    courseName: string;
    courseID: string;
}
interface EnrollmentStatus {
    courseId: string;
    studentId: string;
    status: string;
    // Add other relevant properties as needed
}

const CourseDetails: React.FC<CourseProps> = ({ courseName, courseID}) => {
    const { isAuthenticated, isLoading, user} = useAuth0();
    const [enrollmentStatus, setEnrollmentStatus] = useState(false)

    useEffect(() => {
        const fetchEnrollmentStatus = async () => {
            if (user && user.email) {
                try {
                    const status = await getStatusEnrollService(courseID, user.email);
                    if (status.status === "true") {
                        setEnrollmentStatus(true);
                    } else {
                        setEnrollmentStatus(false);
                    }
                } catch (error) {
                    // Handle the case where there is an error fetching the enrollment status
                    console.error('There was an error!', error);
                    alert("There was an error fetching the enrollment status.");
                }
            } else {
                // Handle the case where user or user.email is undefined
                alert("User email is not available.");
            }
        };

        if (isAuthenticated && user && user.email) {
            fetchEnrollmentStatus();
        }
    }, [isAuthenticated, user, courseID]);

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
        if(!enrollmentStatus){
            if (user && user.email) {
                enrollCourseService(courseID, user.email);
                alert(`Enrolled in ${courseName}`);
            } else {
                // Handle the case where user or user.email is undefined
                alert("User email is not available.");
            }
        }
        else {
            if (user?.email) {
                if (window.confirm(`Are you sure you want to unenroll the course: ${courseName}?`)) {
                    unenroll(courseID, user.email)
                        .then(() => {

                        })
                        .catch(error => {
                            console.error('Error un enrolling the course stream:', error);
                        });
                }
            }
        }
    };

    return (
        <div>
            <h1>{courseName}</h1>
            <h1>{courseID}</h1>
            <button onClick={handleEnrollClick}>
                {enrollmentStatus ? 'Unenroll' : 'Enroll'}
            </button>
        </div>
    );
};

export default CourseDetails;