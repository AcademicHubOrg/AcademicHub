import React from 'react';
import LoginButton from "../../components/auth0Login";
import Profile from "../../components/authProfile";
import LogoutButton from "../../components/auth0Logout";

const AuthPage: React.FC = () => {


    return (

        <div>
            <Profile></Profile>
            <LoginButton></LoginButton>
            <LogoutButton></LogoutButton>
        </div>
    );
};

export default AuthPage;
