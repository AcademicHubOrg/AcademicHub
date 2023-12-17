import React from 'react';
import {Addresses} from '../../api/Addresses';
import RichTextEditor from "../../pages/RichTextEditor";
const AuthPage: React.FC = () => {
    const redirectToUrl = () => {
        window.location.href = `${Addresses.AUTH}/login`; // Replace with your specific URL
    };

    return (
        <button onClick={redirectToUrl}>google auth</button>
    );
};

export default AuthPage;
