<script lang="ts">
    import logo from '$lib/img/lol.svg';
    import { onMount } from 'svelte';
    import * as UserManager from '../lib/scripts/UserManager';

    let isAuthenticated : boolean = false;
    let isAdminPromise : Promise<boolean>;
    let usernamePromise : Promise<string>;

    onMount(async () => {
        const cookies = document.cookie.split(';');
        isAuthenticated = cookies.some((item) => item.trim().startsWith('token='));
        if (isAuthenticated) {
            usernamePromise = await UserManager.getUsername();
            isAdminPromise = await UserManager.isAdmin();
        }
    });
</script>

<nav class="navbar navbar-expand-lg navbar-light bg-light" style="padding-left: 1rem; padding-right: 1rem; z-index:999;">
    <a class="navbar-brand" href="/" style="display: flex; flex-direction:row; align-items:center">
        <img src={logo} height="50" class="d-inline-block align-top" alt="">
        <p style="margin:0; font-size:1.8rem">истовки!</p>
    </a>
    <div class="collapse navbar-collapse justify-content-between" id="navbarNav">
        <ul class="navbar-nav">
            <li class="nav-item">
                <a class="nav-link" href="/">Начало</a>
            </li>
            {#await isAdminPromise then isAdmin}
                {#if isAdmin}
                    <li class="nav-item">
                        <a class="nav-link" href="/listovki">Листовки*</a>
                    </li>
                {/if}
            {/await}
            <li class="navbar-nav">
                <a class="nav-link" href="/listovki/stats">Статистика</a>
            </li>
        </ul>
        <ul class="navbar-nav">
            {#if !isAuthenticated}
                <li class="nav-item">
                    <a class="nav-link" href="/auth/register">Регистрация</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/auth/login">Вход</a>
                </li>
            {:else}
                {#await usernamePromise then username}
                    <li class="nav-item">
                        <a class="nav-link" href="/listovki/stats">Здравейте, {username}!</a>
                    </li>
                {/await}
                <li class="nav-item">
                    <a class="nav-link" href="/auth/logout" on:click={UserManager.logout}>Изход</a>
                </li>
            {/if}
        </ul>
    </div>
</nav>

<style>
    nav {
        box-shadow: 0px 4px 15px -3px rgba(0,0,0,0.12);
        border-bottom: 1px solid rgba(0,0,0,0.3);
        transition: box-shadow 0.2s ease-in-out;
    }
    nav:hover {
        box-shadow: 0px 4px 15px -3px rgba(0,0,0,0.25);
    }
    ul > li > a {
        color: darkgrey;
        transition:color 0.2 ease-in-out;
    }
    ul > li > a:hover {
        color:black;
    }
    .nav-link {
        font-size: 1.2rem !important;
    }
    @font-face {
        font-family: 'lolFont';
        src: url('../lib/fonts/BeaufortforLOL-Bold.woff2'),
             url('../lib/fonts/BeaufortforLOL-Bold.ttf');
    }
    nav > a > p {
        font-family: 'lolFont';
    }
</style>