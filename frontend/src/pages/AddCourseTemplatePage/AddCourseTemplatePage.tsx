import React, { useState } from 'react';
import { addCourseTemplate } from '../../api/addTemplateService';
import {useAuth0} from "@auth0/auth0-react";
import LoginButton from "../../auth0components/auth0Login";

function AddCourseTemplatePage() {
    const [inputText, setInputText] = useState('');
    const [displayedText, setDisplayedText] = useState('');
    const { user, isAuthenticated, isLoading} = useAuth0();

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

    // Function to handle the POST request
    const handleButtonClick = () => {
        setDisplayedText(inputText);
        addCourseTemplate(inputText) // Send the POST request
            .catch(error => console.error('POST request failed: ', error));
    };

    return (
        <>
            <h1>Welcome to the Template page</h1>

            <input
                type="text"
                value={inputText}
                onChange={(e) => setInputText(e.target.value)}
                placeholder="Enter text"
            />
            <button onClick={handleButtonClick}>Register course template</button>
            {displayedText && <p>Registered following course template: {displayedText}</p>}
        </>
    );
}

export default AddCourseTemplatePage;
