import { useAuth0 } from "@auth0/auth0-react";
import {loginService} from "../api/loginService";
import React, {useEffect} from "react";

const LoginButton = () => {
    const { loginWithRedirect, user, isAuthenticated } = useAuth0();

    useEffect(() => {
        if (isAuthenticated && user && user.name && user.email) {
            loginService(user.name, user.email)
                .catch(error => console.error('POST request failed: ', error));
        }
    }, [isAuthenticated, user]);

    return <button onClick={() => loginWithRedirect({
        appState: {
            returnTo: `${window.location.origin}${window.location.pathname}`
        }
    })}>Log In</button>;
};

export default LoginButton;

