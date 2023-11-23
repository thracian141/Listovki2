<script lang='ts'>
    import ListovkaBody from './ListovkaBody.svelte';
    import { onMount } from 'svelte';
    import { fade, slide, fly } from 'svelte/transition';
    export let data;
    import * as ExamManager from '../../../lib/scripts/ExamManager';

    let navRender = false;
    let buttonsRender = false;
    let paragraphsRender = false;
    let bodyRender = false;

    let questions : ExamManager.Question[] | undefined = [];
    let selectedQuestion : ExamManager.Question = {
        id: 0,
        question: '',
        category: '',
        points: 0,
        isMultipleChoice: false,
        mediaURL: ''
    };
    let selectedIndex : number = 0;

    let count = [1]
    for (let i: number = 1; i < 40; i++) {
        count.push(i)
    }

    onMount(async () => {
        navRender = true;
        questions = await ExamManager.getExamByCategory(data.test);
        buttonsRender = true;
        if (questions) {
            bodyRender = true;
            questions = await Promise.all(questions.map(async question => {
                const imageUrl = await ExamManager.getQuestionImage(question.id);
                return { ...question, mediaURL: imageUrl };
            }));
            if (questions.length > 0) {
                selectedQuestion = questions[0];
            }
        }
        paragraphsRender = true;
    });

    function selectQuestion(index : number) {
        if (questions) {
            selectedQuestion = { ...selectedQuestion, ...questions[index] }
        }
        selectedIndex = index;
    }

    function handleChangeQuestion(event : CustomEvent) {
        if (event.detail.direction === 'previous' && selectedIndex > 0) {
            selectedIndex--;
        } else if (event.detail.direction === 'next' && questions && selectedIndex < questions.length - 1) {
            selectedIndex++;
        }
        selectQuestion(selectedIndex);
    }
</script>

{#if navRender}
    <div class="questionNav" in:fade={{delay:1300,duration:500}}>
        {#each count as x, index}
            {#if questions && questions[index]}
                <button on:click={() => selectQuestion(index)} class="bg-white questionEl" in:slide={{delay:(2000+50*index) , duration: 100}} style="background-color:#f5e0e0"> 
                    {#if paragraphsRender}
                        <p style="margin: none;" in:fly={{delay:(3200+10*index+50*index), duration:200}}>{index+1}</p>
                        <img src={questions[index].mediaURL} in:fly={{delay:(3200+10*index+50*index), duration:200}} alt="">
                    {/if}
                </button>
            {:else}
                {#if buttonsRender}
                    <button type="button" class="bg-white questionEl" in:slide={{delay:(2000+50*index) , duration: 100}} style="background-color:#f5e0e0"> 
                        <p style="margin: none;" in:fly={{delay:(3200+10*index+50*index), duration:200}}>{index+1}</p>
                        <img src="/favicon.png" in:fly={{delay:(3200+10*index+50*index), duration:200}} alt="">
                    </button>
                {/if}
            {/if}
        {/each}
    </div>
{/if}

{#if bodyRender}
    <div in:slide={{delay:5000,duration:200}} class="wrapper" 
    style="border-radius:0; margin:0; margin-left:0.4rem; flex-grow:1; height: 100%;
     padding:0 1rem 0 1rem !important; ">
        <ListovkaBody question={selectedQuestion} index={selectedIndex} on:changeQuestion={handleChangeQuestion} />
    </div>
{/if}

<style>
    .questionNav {
        padding-top:0.25rem;
        display:flex;
        flex-direction: row;
        height:100%;
        width:16rem; 
        flex-wrap:wrap;
        align-items: flex-start;
        justify-content: center;
        background-color: rgba(242, 242, 242, 0.9);
        border-right: 1px solid #ababab;
        outline: 5px solid rgb(191, 235, 212);
        box-shadow: 7px 0px 2px white;
    }

    .questionEl {
        display:flex;
        flex-direction: row;
        align-items: center;
        justify-content: space-between;
        width:7rem;
        height:2.4rem;
        border:none;
        border-radius: 0.4rem;
        border: 1px solid #cacaca;
        box-shadow: 1px 1px 1px rgba(0,0,0,0.25);
        transition: background-color 0.3s ease-in-out;
    }
    .questionEl:hover {
        background-color: #f7f7f7 !important;
        transition:none;
    }
    .questionEl:active {
        background-color: rgb(254, 255, 255) !important;
        box-shadow: inset 1px 1px 2px rgba(0,0,0,0.25);
    }
    .questionEl > img {
        height:2rem;
        border-radius: 0.3rem;
        width:auto;
        object-fit: cover;
        box-shadow: 1px 1px 2px rgba(0,0,0,0.2);
        border: 1px solid #888888;
    }
    .questionEl:nth-child(odd) {
        margin-right: 0.2rem;
    }
</style>