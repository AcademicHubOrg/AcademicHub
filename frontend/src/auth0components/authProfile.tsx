import { useAuth0 } from "@auth0/auth0-react";
import LoginButton from "./auth0Login";
import {useEffect} from "react";
import {loginService} from "../api/loginService";
import { useMyContext} from "../MyContext";

const Profile = () => {
    const { user, isAuthenticated, isLoading} = useAuth0();
    const { updateJsonData } = useMyContext();
    const { jsonData } = useMyContext();

    useEffect(() => {
        if (!jsonData.loggedIn) {
            if (isAuthenticated && user && user.name && user.email) {
                loginService(user.name, user.email)
                    .catch(error => console.error('POST request failed: ', error));
                updateJsonData({
                    loggedIn: true
                })
            }
        }
    }, [isAuthenticated, user, jsonData.loggedIn, updateJsonData]);

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
            <p></p>
            <div>
                <h2>Your enrolled courses:</h2>
            </div>
        </div>
    );
};

export default Profile;
