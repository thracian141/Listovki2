<script lang="ts">
    import type {ListovkaResultModel} from "$lib/scripts/ListovkaModel";
    import { results } from "$lib/scripts/ExamManager";
    import { onMount } from "svelte";

    export let data;
    let listovka : ListovkaResultModel;

    onMount(async () => {
        const result : ListovkaResultModel | undefined = await results(parseInt(data.test));
        if (result) {
            listovka = result;
        } else {
            console.log('Data is undefined');
        }
    });

</script>

<div class="wrapper" style="height:90%; width:40%; padding:0; overflow:hidden; text-shadow:none">
    <div class="header">
        <h1>Резултати:</h1>
    </div>
    <div class="wrappersmall">
        {#if listovka}
            <div class="wraprow">
                <h3>Листовка | № {listovka.id}</h3>
                <p>Запомни номера, за намиране на листовката по-късно!</p>
            </div>
            <div class="wraprow">
                <p>Брой решени въпроси: {listovka.questionsNumber}</p>
            </div>
            <div class="wraprow">
                <p>Правилни отговори: {listovka.guessedQuestionsNumber}</p>
            </div>
            <div class="wraprow">
                <h5>Равнява се на процент: {listovka.percentageRight.toFixed(2)}%</h5>
            </div>
            <div class="wraprow">
                <p>на потребител: {listovka.userEmail}</p>
            </div>
            <div class="resultrow">
                <h2>Оценка:
                </h2>
                {#if listovka.percentageRight >= 89}
                    <h2 style="color:white; background-color:green; padding:0.4rem; border-radius:0.5rem;">Положителна</h2>
                {:else}
                    <h2 style="color:white; background-color:red; padding:0.4rem; border-radius:0.5rem;">Отрицателна</h2>
                {/if}
            </div>
        {/if}
    </div>
</div>

<style>
    .resultrow {
        display: flex;
        flex-direction: row;
        justify-content: left;
        width: 100%;
        flex-grow: 1;
        padding:1rem; padding-left: 3rem;
        gap:1rem;
    }
    .wraprow {
        display: flex;
        flex-direction: row;
        justify-content: left;
        gap:2rem;
        width: 100%;
        flex-grow: 1;
        padding:1rem; padding-left: 2rem;
        align-items: center;
    }
    .wraprow:nth-child(odd) {
        background-color: #dbdbdb;
    }
    .wraprow > p, h3, h5 {
        margin:0;
    }
    .wrappersmall {
        display: flex;
        flex-direction: column;
        background-color: white;
        border-radius: 1rem;
        flex-grow: 1;
        width: 97%;
        margin:1rem;
        box-shadow: 0 0 2px 2px rgba(0, 0, 0, 0.2);
        align-items: center;
        color:black;
        overflow: hidden;
    }
    .header {
        background-image: url("/listovkarezultat.png");
        height:45%;
        width:100%;
        background-repeat: no-repeat;
        background-size: cover;
        display: flex;
        flex-direction: column;
        justify-content: flex-end;
        align-items: center;
        background-position-y: 35%;
        padding-bottom: 2rem;
    }
</style>