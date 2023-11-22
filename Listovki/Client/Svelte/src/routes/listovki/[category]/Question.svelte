<script lang="ts">
    import { createEventDispatcher, onDestroy, onMount } from 'svelte';
    import type {Question} from '../../../lib/scripts/ExamManager';
    export let question : Question;
    export let index : number;

    let timeleft = 40;
    let timerId: number;

    const dispatch = createEventDispatcher();

    function changeQuestion(direction: string) {
        dispatch('changeQuestion', { direction });
    }

    onMount(() => {
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
        <div id="timer" style="width: {40-timeleft}%; text-align:end; padding-right:1rem; margin-right:1rem;" class="text-white">{40-timeleft} изминали мин.</div>
        {timeleft} оставащи мин.
        <span class="tooltip">{timeleft} оставащи мин.</span>
    </div>
    <p style="line-height:100%; margin:0; margin-left:1rem;">Общо 40 мин.</p>
</div>
<div id="questionwrapper">
    <div style="width: 100%; height:15%; background-color:white; border-radius:1rem; margin-top:0.5rem; text-shadow:none; color:#000;
     display:flex; flex-direction:row; justify-content:center; align-items:center; overflow:hidden; position:relative;">
        <h2 style="margin: 0; line-height:100%;">{question.question}</h2>
        <h4 style="margin: 0; line-height:100%; position:absolute; left:1%; top:50%; transform:translateY(-50%);">№{index+1}</h4>
    </div>
    <div style="width:100%;height:85%; display:flex; justify-content:center; align-items:center;">
        <div style="display: flex; flex-direction:row;">
            <img style="max-height:200px" src={question.mediaURL} alt="question media"/>
            <div class="answers">
                {#if !question.isMultipleChoice}
                    <div class="answer">
                        <input type="radio" id="answer1" name="answer" value="answer1">
                        <label for="answer1"></label>
                    </div>
                    <div class="answer">
                        <input type="radio" id="answer2" name="answer" value="answer2">
                        <label for="answer2"></label>
                    </div>
                    <div class="answer">
                        <input type="radio" id="answer3" name="answer" value="answer3">
                        <label for="answer3"></label>
                    </div>
                    <div class="answer">
                        <input type="radio" id="answer4" name="answer" value="answer4">
                        <label for="answer4"></label>
                    </div>
                {:else}
                    <div class="answer">
                        <input type="checkbox" id="answer1" name="answer1" value="answer1">
                        <label for="answer1"></label>
                    </div>
                    <div class="answer">
                        <input type="checkbox" id="answer2" name="answer2" value="answer2">
                        <label for="answer2"></label>
                    </div>
                    <div class="answer">
                        <input type="checkbox" id="answer3" name="answer3" value="answer3">
                        <label for="answer3"></label>
                    </div>
                    <div class="answer">
                        <input type="checkbox" id="answer4" name="answer4" value="answer4">
                        <label for="answer4"></label>
                    </div>
                {/if}
            </div>
        </div>
    </div>
</div>
<div id="buttonswrapper">
    <button on:click={() => changeQuestion('previous')}>
        <img src="/double-chevron.png" style="transform:scaleX(-1);" alt="previous question"/>
    </button>
    <button on:click={() => changeQuestion('next')}>
        <img src="/double-chevron.png" alt="next question"/>
    </button>
</div>


<style>
    #questionwrapper {
        display: flex;
        flex-direction: column;
        align-items: center;
        overflow: hidden;
        width: 100%;
        height: 65%;
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
    #timerwrapper {
        display: flex;
        width: 100%;
        height: 15%;
        background-color: #ffffffee;
        border-radius: 0 0 1rem 1rem;
        justify-content: center;
        align-items: center;
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
