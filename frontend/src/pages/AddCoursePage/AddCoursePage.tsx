import React, { useState } from 'react';

function AddCoursePage() {
    // State to store the entered text
    const [inputText, setInputText] = useState('');
    // State to store the displayed text
    const [displayedText, setDisplayedText] = useState('');

    // Function to handle button click
    const handleButtonClick = () => {
        setDisplayedText(inputText);
    };

    return (
        <>
            <h1>Welcome to the Add Course page</h1>

            <input
                type="text"
                value={inputText}
                onChange={(e) => setInputText(e.target.value)}
                placeholder="Enter text"
            />
            {/* Button to display entered text */}
            <button onClick={handleButtonClick}>Display Text</button>
            {/* Display the entered text below the button */}
            {displayedText && <p>Entered Text: {displayedText}</p>}
        </>
    );
}

export default AddCoursePage;
