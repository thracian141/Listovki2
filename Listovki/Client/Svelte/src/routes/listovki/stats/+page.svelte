<script lang="ts">
    import { getUsername } from "$lib/scripts/UserManager";
    import { onMount } from "svelte";
    import {stats} from '$lib/scripts/ExamManager';

    let username : string = 'test';
    let listovkiCount : number = 10;
    let verniListovki : number = 0.5; //percentage 50%

    let aCatListovki : number = 3;
    let bCatListovki : number = 4;
    let cCatListovki : number = 2;
    let dCatListovki : number = 1;

    let aCatVerniListovki : number = 1;
    let bCatVerniListovki : number = 3;
    let cCatVerniListovki : number = 1;
    let dCatVerniListovki : number = 1;

    let lastExamId : number = 0;

    onMount(async () => {
        let dict: { [key: string]: number } | undefined = {};
        dict = await stats();
        if (dict != undefined) {
            listovkiCount = dict["listovkiCount"];
            verniListovki = dict["verniListovki"];
            aCatListovki = dict["aCatListovki"];
            bCatListovki = dict["bCatListovki"];
            cCatListovki = dict["cCatListovki"];
            dCatListovki = dict["dCatListovki"];
            aCatVerniListovki = dict["aCatVerniListovki"];
            bCatVerniListovki = dict["bCatVerniListovki"];
            cCatVerniListovki = dict["cCatVerniListovki"];
            dCatVerniListovki = dict["dCatVerniListovki"];
            lastExamId = dict["lastExamId"];
        }

        username = await getUsername();
    })
</script>

<div class="wrapper" style="width:60%; height:90%; text-shadow: none; font-family:'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif">
    <div class="smallwrapper" style="width:100%; height:6%; margin-bottom:0;">
        <h2 style="margin-bottom: 0;">Статистика на потребител {username}:</h2>
    </div>
    <div class="smallwrapper" style="width:100%; height:30%; margin-bottom:1rem;">
        <h1><span style="font-size:4rem;text-decoration: underline; text-decoration-style:dashed;">{listovkiCount}</span> решени листовки</h1>
        <div class="bar" style="background-color: #dc3545;">
            <div class="bar-fill" style="background-color: #28a745; width:{verniListovki*100}%"></div>
            <p style="margin: 0; line-height:1.5rem; position:absolute; left:2%; top:50%; transform:translateY(-50%);">{verniListovki*100}% верни</p>
            <p style="margin: 0; line-height:1.5rem; position:absolute; right:2%; top:50%; transform:translateY(-50%);">{100-(verniListovki*100)}% грешни</p>
        </div>
        <div class="bar" style="background-color: #ffffff;">
            <div class="bar-fill" style="background-color: #dc3545; width:{(aCatListovki/listovkiCount)*100}%">A кат. - {(aCatListovki/listovkiCount)*100}%</div>
            <div class="bar-fill" style="background-color: #6f42c1; width:{(bCatListovki/listovkiCount)*100}%">B кат. - {(bCatListovki/listovkiCount)*100}%</div>
            <div class="bar-fill" style="background-color: #17a2b8; width:{(cCatListovki/listovkiCount)*100}%">C кат. - {(cCatListovki/listovkiCount)*100}%</div>
            <div class="bar-fill" style="background-color: #28a745; width:{(dCatListovki/listovkiCount)*100}%">D кат. - {(dCatListovki/listovkiCount)*100}%</div>
        </div>
    </div>
    <div class="smallwrapper" style="width:100%; height:54%;">
        <h2>По категории</h2>
        <div style="display: flex; width:100%; height:100%; flex-direction:row; margin-bottom:1rem; gap:1rem;">
            <div class="verysmallwrapper">
                <h3>A категория</h3>
                <h4><span style="font-size:1.8rem;text-decoration: underline; text-decoration-style:dashed;">{aCatListovki}</span> решени листовки</h4>
                <div class="bar" style="background-color: #dc3545;">
                    {#if aCatListovki != 0}
                    <div class="bar-fill" style="background-color: #28a745; width:{((aCatVerniListovki/aCatListovki)*100).toFixed(1)}%">{((aCatVerniListovki/aCatListovki)*100).toFixed(1)}% верни</div>
                    {/if}
                </div>
            </div>
            <div class="verysmallwrapper">
                <h3>B категория</h3>
                <h4><span style="font-size:1.8rem;text-decoration: underline; text-decoration-style:dashed;">{bCatListovki}</span> решени листовки</h4>
                <div class="bar" style="background-color: #dc3545;">
                    {#if bCatListovki != 0}
                    <div class="bar-fill" style="background-color: #28a745; width:{((bCatVerniListovki/bCatListovki)*100).toFixed(1)}%">{((bCatVerniListovki/bCatListovki)*100).toFixed(1)}% верни</div>
                    {/if}
                </div>
            </div>
        </div>
        <div style="display: flex; width:100%; height:100%; flex-direction:row; gap:1rem;">
            <div class="verysmallwrapper">
                <h3>C категория</h3>
                <h4><span style="font-size:1.8rem;text-decoration: underline; text-decoration-style:dashed;">{cCatListovki}</span> решени листовки</h4>
                <div class="bar" style="background-color: #dc3545;">
                    {#if cCatListovki != 0}
                    <div class="bar-fill" style="background-color: #28a745; width:{((cCatVerniListovki/aCatListovki)*100).toFixed(1)}%">{((cCatVerniListovki/aCatListovki)*100).toFixed(1)}% верни</div>
                    {/if}
                </div>
            </div>
            <div class="verysmallwrapper">
                <h3>D категория</h3>
                <h4><span style="font-size:1.8rem;text-decoration: underline; text-decoration-style:dashed;">{dCatListovki}</span> решени листовки</h4>
                <div class="bar" style="background-color: #dc3545;">
                    {#if dCatListovki != 0}
                    <div class="bar-fill" style="background-color: #28a745; width:{((dCatVerniListovki/aCatListovki)*100).toFixed(1)}%">{((dCatVerniListovki/aCatListovki)*100).toFixed(1)}% верни</div>
                    {/if}
                </div>
            </div>
        </div>
    </div>
    <div class="smallwrapper" style="width:100%; height:8%; flex-direction:row; justify-content:center; margin-top:0.5rem;"> 
        <h2>Номер на последна листовка: <a href="/listovki/stats/{lastExamId}" style="text-shadow: none; text-decoration:none;">№ {lastExamId}</a></h2>
    </div>
</div>

<style>
    .verysmallwrapper > h4 {
        margin-bottom: 2rem;
    }
    .verysmallwrapper {
        background-color: #ffffff44;
        display: flex;
        flex-direction: column;
        border-radius: 1rem;
        align-items: center;
        color:white;
        text-shadow: 0px 0px 3px black;
        width:50%;
        flex-grow: 1;
    }
    .bar-fill {
        height:100%;
        display: flex;
        justify-content: center;
        align-items: center;
        text-align: center;
    }
    .bar {
        width: 100%;
        height: 1.5rem;
        border-radius: 999px;
        overflow: hidden;
        position: relative;
        text-shadow: none;
        display: flex;
        flex-direction: row;
    }
    .smallwrapper {
        background-color: #ffffff44;
        display: flex;
        flex-direction: column;
        border-radius: 1rem;
        align-items: center;
        color:white;
        text-shadow: 0px 0px 3px black;
        justify-content: space-between;
    }
</style>