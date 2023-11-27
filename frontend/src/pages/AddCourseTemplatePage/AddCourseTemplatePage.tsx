import React, { useState } from 'react';
import { addCourseTemplate } from '../../api/addTemplateService';

function AddCourseTemplatePage() {
    const [inputText, setInputText] = useState('');
    const [displayedText, setDisplayedText] = useState('');

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
