import React from 'react';

const AuthPage: React.FC = () => {
    const redirectToUrl = () => {
        window.location.href = 'http://localhost:5006/login'; // Replace with your specific URL
    };

    return (
        <button onClick={redirectToUrl}>google auth</button>
    );
};

export default AuthPage;
