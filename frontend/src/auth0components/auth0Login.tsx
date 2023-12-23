import { useAuth0 } from "@auth0/auth0-react";
import {loginService} from "../api/loginService";
import React, {useEffect} from "react";

const LoginButton = () => {
    const {loginWithRedirect} = useAuth0();

    return <button onClick={() => loginWithRedirect({
        appState: {
            returnTo: `${window.location.origin}${window.location.pathname}`
        }
    })}>Log In</button>;
};

export default LoginButton;

