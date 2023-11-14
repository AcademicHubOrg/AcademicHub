import React, { useState } from 'react';

function TemplatePage() {
    const [inputText, setInputText] = useState('');
    const [displayedText, setDisplayedText] = useState('');

    // Function to handle the POST request
    const sendPostRequest = async (courseTemplateName:string) => {
        try {
            const response = await fetch('http://localhost:5204/courseTemplate', { // Replace with your endpoint
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    name: courseTemplateName,
                    id: '12345'
                })
            });
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const data = await response.json();
            console.log(data); // Process the response data as needed
        } catch (error) {
            console.error('There was an error!', error);
        }
    };

    // Function to handle button click
    const handleButtonClick = () => {
        setDisplayedText(inputText);
        sendPostRequest(inputText); // Send the POST request
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

export default TemplatePage;
