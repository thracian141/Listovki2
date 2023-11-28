<script lang="ts">
    import { createEventDispatcher, onDestroy, onMount } from 'svelte';
    import type {Question, Answer} from '../../../lib/scripts/ExamManager';
    import QuestionBody from './QuestionBody.svelte';
    import type { ListovkaInputModel } from '$lib/scripts/ListovkaModel';
    export let question : Question;
    export let index : number;
    export let examSubmitted : boolean;
    import {gradeExam} from '$lib/scripts/ExamManager';
    import { goto } from '$app/navigation';

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
        localStorage.removeItem('newExam');
    });

    async function gradeExamAndReturnId() {
        if (!examSubmitted) {
            examSubmitted = true;
            //set submitbutton's text to "Оценка"
            let submitbutton = document.getElementById('submitbutton');
            if (submitbutton) {
                submitbutton.innerHTML = 'Оценка';
            }
        } else {
            let exam : ListovkaInputModel = JSON.parse(localStorage.getItem('newExam') || '{}');
            let allQuestionsAnswered = true;
            exam.questions.forEach(question => {
                let questionAnswered = false;
                for (const answer in question.answers) {
                    if (question.answers[answer]) {
                        questionAnswered = true;
                        break;
                    }
                }
                if (!questionAnswered) {
                    allQuestionsAnswered = false;
                }
            });
            if (!allQuestionsAnswered) {
                alert('Моля, отговорете на всички въпроси преди да предадете теста.');
                return;
            }
            let listovkaId = await gradeExam(exam);
            console.log(listovkaId);
            goto(`/listovki/stats/${listovkaId}`);
        }
    }
</script>


<div id="timerwrapper" style="text-shadow:none;" class="text-dark">
    <div id="timerbg" class="text-dark">
        <div id="timer" style="width: {((40-timeleft)/40)*100}%; text-align:end; padding-right:1rem; margin-right:1rem;" class="text-white">{40-timeleft} изминали мин.</div>
        <p style="line-height: 100%; margin:0;">{timeleft} оставащи мин.</p>
        <span class="tooltip">{timeleft} оставащи мин.</span>
    </div>
    <p style="line-height:100%; margin:0; margin-left:1rem;">Общо 40 мин.</p>
</div>
<button id="submitbutton"
   on:click={gradeExamAndReturnId}>
    Предай<img src="/arrow-return-right.svg" alt="submit icon"/>
</button>
<QuestionBody question={question} index={index} examSubmitted={examSubmitted}/>
<div id="buttonswrapper">
    <button on:click={() => changeQuestion('previous')}>
        <img src="/double-chevron.png" style="transform:scaleX(-1);" alt="previous question"/>
    </button>
    <button on:click={() => changeQuestion('next')}>
        <img src="/double-chevron.png" alt="next question"/>
    </button>
</div>


<style>
    #submitbutton {
        position: absolute;
        top: 50%;
        right: 1rem;
        transform: translateY(-50%);
        background-color: #ffffff;
        height: 10rem;
        width: 5rem;
        text-align: center;
        background-color: #0d6efd;
        color:white;
        font-size: large;
        font-weight: bold;
        border-radius: 0.5rem;
        border:none;
        box-shadow: 1px 1px 3px 2px rgba(0, 0, 0, 0.2);
        z-index: 999;
        transition: box-shadow 0.1s ease-in-out, background-color 0.1s ease-in-out, color 0.1s ease-in-out, filter 0.1s ease-in-out;
    }
    #submitbutton:hover {
        cursor: pointer !important;
        box-shadow: 0px 0px 8px 8px rgba(255,255,255,0.7);
        background-color: #0d65ea;
        color: #e3e3e3;
    }
    #submitbutton > img {
        filter:  brightness(0) invert(1);
        width:70%;
    }
    #submitbutton:hover > img {
        filter:  brightness(0) invert(0.9);
    }
    #submitbutton:active{
        box-shadow: 0px 0px 2px 4px #0d6dfd8e;
    }
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
