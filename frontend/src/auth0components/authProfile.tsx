import { useAuth0 } from "@auth0/auth0-react";
import React, {useEffect} from "react";
import LogoutButton from "./auth0Logout";
import {loginService} from "../api/loginService";
import LoginButton from "./auth0Login";

const Profile = () => {
    const { user, isAuthenticated, isLoading, loginWithRedirect} = useAuth0();

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

    return (
        <div>
            <img src={user.picture} alt={user.name} />
            <h2>{user.name}</h2>
            <p>{user.email}</p>
        </div>
    );
};

export default Profile;
