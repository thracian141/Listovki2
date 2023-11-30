<script lang='ts'>
    import {getInfo} from '$lib/scripts/UserManager';
    import { onMount } from "svelte";

    let email = ''
    let fullname = ''
    let username = ''
    let birthdate = ''
    let egn = ''

    onMount(async () => {
        let dict: { [key: string]: string } | undefined = {};
        dict = await getInfo();
        if (dict != undefined) {
            email = dict["email"] as string;
            fullname = dict["fullName"] as string;
            username = dict["userName"] as string;
            birthdate = dict["birthDate"] as string;
            birthdate = birthdate.slice(8,10) + "/" + birthdate.slice(5,7) + "/" + birthdate.slice(0,4);
            egn = dict["egn"] as string;
        }
    });
</script>

<div class="wrapper" style="height:80%; width:40%; padding:0; overflow:hidden;">
    <div class="backgroundimg">
        <h1>Вашият профил:</h1>
    </div>
    <div class="smallwrapper">
        <div class="smallrow">
            <h5>Имена:</h5>
            <h5>{fullname}</h5>
        </div>
        <div class="smallrow">
            <h5>Потребителско име:</h5>
            <h5>{username}</h5>
        </div>
        <div class="smallrow">
            <h5>Електронна поща:</h5>
            <h5>{email}</h5>
        </div>
        <div class="smallrow">
            <h5>Дата на раждане:</h5>
            <h5>{birthdate}</h5>
        </div>
        <div class="smallrow">
            <h5>ЕГН:</h5>
            <h5>{egn}</h5>
        </div>
    </div>
</div>


<style>
    .smallrow {
        background-color: #ffffff;
        color:black;
        text-shadow: none;
        border-radius: 1rem;
        width: 80%;
        display: flex;
        flex-direction: row;
        justify-content: flex-start;
        align-items: center;
        width: 100%;
        height: 20%;
        box-shadow: 1px 2px 2px rgba(0, 0, 0, 0.2);
        border: rgba(83, 89, 194, 0.301) 1px solid;
        overflow: hidden;
    }
    .smallrow > h5 {
        margin: 0;
        padding: 0 1rem 0 1rem;
        height: 100%;
        font-size: 1.25rem;
        width: 14rem;
        display: flex;
        flex-direction: row;
        justify-content: flex-start;
        align-items: center;
    }
    .smallrow > h5:first-child {
        border-radius: 1rem 0 0 1rem;
        box-shadow: inset 1px 0px 2px 2px rgba(0, 0, 0, 0.1);
        padding-left: 1.2rem;
    }
    .smallrow > h5:last-child {
        flex-grow: 1;
        border-radius: 0 1rem 1rem 0;
        box-shadow: inset -1px 0px 2px 2px rgba(0, 0, 0, 0.1);
    }
    .smallwrapper {
        height:50%;
        width:100%;
        display: flex;
        flex-direction: column;
        gap:1rem;
        padding: 1rem;
    }
    .backgroundimg {
        display: flex;
        flex-direction: column;
        justify-content: flex-end;
        align-items: center;
        background-image: url("/accountinfobg.jpg");
        background-size: cover;
        background-position: center;
        background-repeat: no-repeat;
        width:100%;
        height:50%;
        padding-bottom: 2rem;
        text-shadow: 2px 2px 3px black;
    }
</style>