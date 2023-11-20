import React from 'react';

const AuthPage: React.FC = () => {
    const redirectToUrl = () => {
        window.location.href = 'https://localhost:44339/login'; // Replace with your specific URL
    };

    return (
        <button onClick={redirectToUrl}>google auth</button>
    );
};

export default AuthPage;
