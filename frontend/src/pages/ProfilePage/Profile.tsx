import React, { useState } from 'react';
import {useAuth0} from "@auth0/auth0-react";

function ProfilePage() {

    const [inputText1, setInputText1] = useState('');
    const [inputText2, setInputText2] = useState('');
    const [displayedText, setDisplayedText] = useState('');
    const { user, isAuthenticated, loginWithRedirect} = useAuth0();

    if (!isAuthenticated || !user) {
        loginWithRedirect()
        return <div>Not authenticated</div>;
    }
    const handleButtonClick = () => {
        setDisplayedText(`Name: ${inputText1}<br>Last name: ${inputText2}`);
    };

    return (
        <>
            <h1>Welcome to the Profile page</h1>

            <div>
                <label>Name:</label>
                <input
                    type="text"
                    value={inputText1}
                    onChange={(e) => setInputText1(e.target.value)}
                    placeholder="Name: "
                />
            </div>

            <div>
                <label>Last name:</label>
                <input
                    type="text"
                    value={inputText2}
                    onChange={(e) => setInputText2(e.target.value)}
                    placeholder="Last name:"
                />
            </div>

            {/* Button to display entered text */}
            <button onClick={handleButtonClick}>Display Text</button>

            {/* Display the entered text below the button */}
            {displayedText && <p dangerouslySetInnerHTML={{ __html: displayedText }}></p>}
        </>
    );
}

export default ProfilePage;
