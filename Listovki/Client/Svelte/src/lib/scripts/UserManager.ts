import { browser } from '$app/environment';
import { goto } from '$app/navigation';

export interface User {
    email: string;
    username: string;
    fullname: string;
    birthdate: string;
    egn: string;
    passedexams: number;
    failedexams: number;
    [key: string]: string | number;
}

export async function getUsername() {
    const cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    if (!cookie) {
        return;
    }

    const token = cookie.split('=')[1];
    const response = await fetch('https://localhost:5000/account/username', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    const data = await response.json();
    return data.userName;
}
export async function getEmail() {
    const cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    if (!cookie) {
        return;
    }

    const token = cookie.split('=')[1];
    const response = await fetch('https://localhost:5000/account/email', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    const data = await response.json();
    return data.email;
}

export async function isAdmin() {
    const cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    if (!cookie) {
        return;
    }

    const token = cookie.split('=')[1];
    const response = await fetch('https://localhost:5000/account/isadmin', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    const data = await response.json();
    return data.isAdmin;
}

export async function logout(event: Event) {
    event.preventDefault();
    
    document.cookie = 'token=;path=/;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    const response = await fetch('https://localhost:5000/authenticate/logout', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
    });
    if (response.ok) {
        location.reload();
        await goto('/');
    } else {
        console.error("Logout failed");
    }
}