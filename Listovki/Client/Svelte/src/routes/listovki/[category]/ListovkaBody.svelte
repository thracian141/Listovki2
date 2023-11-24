<script lang="ts">
    import { createEventDispatcher, onDestroy, onMount } from 'svelte';
    import type {Question, Answer} from '../../../lib/scripts/ExamManager';
    import QuestionBody from './QuestionBody.svelte';
    export let question : Question;
    export let index : number;

    let timeleft = 40;
    let timerId: number;

    const dispatch = createEventDispatcher();

    function changeQuestion(direction: string) {
        dispatch('changeQuestion', { direction });
    }
    
    onMount(async () => {
        timerId = setInterval(() => {
            if (timeleft > 0) {
                timeleft--;
            } else {
                clearInterval(timerId);
            }
        }, 60000); // 60000 milliseconds = 1 minute
    });

    onDestroy(() => {
        clearInterval(timerId); // Clear the interval when the component is destroyed
    });

</script>


<div id="timerwrapper" style="text-shadow:none;" class="text-dark">
    <div id="timerbg" class="text-dark">
        <div id="timer" style="width: {((40-timeleft)/40)*100}%; text-align:end; padding-right:1rem; margin-right:1rem;" class="text-white">{40-timeleft} изминали мин.</div>
        <p style="line-height: 100%; margin:0;">{timeleft} оставащи мин.</p>
        <span class="tooltip">{timeleft} оставащи мин.</span>
    </div>
    <p style="line-height:100%; margin:0; margin-left:1rem;">Общо 40 мин.</p>
</div>
<QuestionBody question={question} index={index}/>
<div id="buttonswrapper">
    <button on:click={() => changeQuestion('previous')}>
        <img src="/double-chevron.png" style="transform:scaleX(-1);" alt="previous question"/>
    </button>
    <button on:click={() => changeQuestion('next')}>
        <img src="/double-chevron.png" alt="next question"/>
    </button>
</div>


<style>
    #timerwrapper {
        display: flex;
        width: 100%;
        height:15%;
        background-color: #ffffffee;
        border-radius: 0 0 1rem 1rem;
        justify-content: center;
        align-items: center;
    }
    #buttonswrapper {
        display: flex;
        flex-direction: row;
        width: 100%;
        height: 20%;
        background-color: #ffffffee;
        border-radius: 1rem 1rem 0 0;
        justify-content: center;
        align-items: center;
        gap:3%;
    }
    #buttonswrapper > button {
        background-color: white;
        width:40%;
        height: 70%;
        border-radius: 1.5rem;
        border: solid 1px #bfbfbfff;
        box-shadow: 1px 2px 3px 1px rgba(0, 0, 0, 0.15);
        transition: background-color 0.2s ease-in-out, border 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
    }
    #buttonswrapper > button:hover {
        border: solid 1px #bfbfbf88;
        background-color: #f1f0f0;
        box-shadow: 1px 2px 3px 1px rgba(0, 0, 0, 0.25);
    }
    button > img {
        filter: brightness(0) saturate(100%) invert(99%) sepia(1%) saturate(170%) hue-rotate(343deg) brightness(114%) contrast(86%);
        height:50%;
        transition: filter 0.2s ease-in-out;
    }
    button:hover > img {
        filter: brightness(0) saturate(100%) invert(100%) sepia(94%) saturate(0%) hue-rotate(248deg) brightness(106%) contrast(106%);
    }
    #timer {
        height:100%;
        background-color: #0d6efd;
        border-radius: 999px;
        box-shadow: 0px 0px 2px 2px rgba(0, 0, 0, 0.15);
        transition: width 1s ease-out;
        overflow: hidden;
    }
    #timerbg {
        position: relative;
        display: flex;
        align-items: center;
        width: 90%;
        height: 25%;
        background-color: #eaeaea;
        padding: 0.3rem;
        border-radius: 999px;
        box-shadow: 0px 0px 2px 2px rgba(0, 0, 0, 0.15);
    }
    .tooltip {
        visibility: hidden;
        width: 120px;
        background-color: #555;
        color: #fff;
        text-align: center;
        padding: 5px 0;
        border-radius: 6px;
        position: absolute;
        z-index: 50;
        bottom: 110%;
        left: 50%;
        margin-left: -60px;
        opacity: 0;
        transition: opacity 0.3s;
    }
    #timerbg:hover .tooltip {
        visibility: visible;
        opacity: 1;
    }
</style>
