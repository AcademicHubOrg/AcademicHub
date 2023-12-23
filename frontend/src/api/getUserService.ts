import { Addresses } from './Addresses';
import {useAuth0} from "@auth0/auth0-react";

export const getUserInfo = async (studentEmail: string) => {
    const response = await fetch(`${Addresses.AUTH}/get-by-email/${studentEmail}`);
    let data = [];
    if(response.ok){
        data = await response.json();
    }
    return data;
};