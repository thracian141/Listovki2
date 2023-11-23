<script lang="ts">
    import type { Answer, Question } from "$lib/scripts/ExamManager";
    import { listAnswersByQuestionId } from "$lib/scripts/ExamManager";

    export let question : Question;
    export let index : number;
    let answers : Answer[] | undefined = [];

    const fetchAnswers = async () => {
        answers = await listAnswersByQuestionId(question.id);
    };

    $: {
        if (question){
            fetchAnswers();
        }
    }
</script>

<div id="questionwrapper">
    <div style="width: 100%; height:15%; background-color:white; border-radius:1rem; margin-top:0.5rem; text-shadow:none; color:#000;
     display:flex; flex-direction:row; justify-content:center; align-items:center; overflow:hidden; position:relative;">
        <h2 style="margin: 0; line-height:100%;">{question.question}</h2>
        <h4 style="margin: 0; line-height:100%; position:absolute; left:1%; top:50%; transform:translateY(-50%);">№{index+1}</h4>
    </div>
    <div id="mediaoutput" style="background-image:url({question.mediaURL});"></div>
    <div style="width:100%;height:100%; display:flex; justify-content:flex-end; align-items:center; flex-direction:column">
        <div class="answers">
            {#if answers}
                {#if !question.isMultipleChoice}
                    {#each answers as answer, index}
                        <button class="answer" id={String(answer.id)} name={String("answer" + index)}>{answer.text}</button>
                    {/each}
                {:else}
                    {#each answers as answer, index}
                        <button class="answer" id={String(answer.id)} name={String("answer" + index)}>{answer.text}</button>
                    {/each}
                {/if}
            {/if}
        </div>
    </div>
</div>

<style>
    #mediaoutput {
        position:absolute;
        top:15%;
        height:250px;
        width:500px;
        background-size:contain;
        background-repeat:no-repeat; 
        background-position:bottom;
        margin-top:0.5rem;
    }
    .answer {
        width:26rem;
        height:5rem;
        margin: 0;
        padding:0;
        background-color: white;
        border-radius: 1rem;
        border: 0.1rem solid #bfbfbfff;
        font-size: 1.3rem;
        padding:0.5rem;
    }
    .answers {
        display: flex;
        flex-direction: row;
        justify-content: center;
        flex-wrap: wrap;
        width: 60%;
        height:10rem;
        gap:1rem;
        margin-bottom: 2rem;
    }
    #questionwrapper {
        display: flex;
        flex-direction: column;
        align-items: center;
        overflow: hidden;
        width: 100%;
        height:65%;
        position: relative;
    }
</style>