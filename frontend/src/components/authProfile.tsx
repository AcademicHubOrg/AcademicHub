import { useAuth0 } from "@auth0/auth0-react";
import React, {useEffect} from "react";
import LogoutButton from "./auth0Logout";
import {loginService} from "../api/loginService";

const Profile = () => {
    const { user, isAuthenticated, isLoading, loginWithRedirect} = useAuth0();

    if (isLoading) {
        return <div>Loading ...</div>;
    }

    if (!isAuthenticated || !user) {
        loginWithRedirect()
        return <div>Not authenticated</div>;
    }

    return (
        <div>
            <img src={user.picture} alt={user.name} />
            <h2>{user.name}</h2>
            <p>{user.email}</p>
            <LogoutButton></LogoutButton>
        </div>
    );
};

export default Profile;
