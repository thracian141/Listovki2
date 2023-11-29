<script lang="ts">
    import { browser } from "$app/environment";
    import type { Answer, Question } from "$lib/scripts/ExamManager";
    import { listAnswersByQuestionId } from "$lib/scripts/ExamManager";
    import type { QuestionMap, ListovkaInputModel } from "$lib/scripts/ListovkaModel";
    import { onMount } from "svelte";
    export let examSubmitted: boolean;
    export let question : Question;
    export let index : number;
    let answers : Answer[] | undefined = [];
    let answersSelected : boolean[] = [false, false, false, false];
    let newExam : ListovkaInputModel;
    let isLoading = true;
    let questionMap : QuestionMap | undefined;
    let correctAnswersCount = 0;

    onMount(async () => {
        newExam = JSON.parse(localStorage.getItem('newExam') || '{}');
        questionMap = newExam.questions.find((q: QuestionMap) => q.questionId === question.id);
        await fetchAnswers();
    })

    async function fetchAnswers() {
        isLoading = true;
        try {
            answers = await listAnswersByQuestionId(question.id);
            newExam = JSON.parse(localStorage.getItem('newExam') || '{}');
            if (newExam && newExam.questions) {
                const questionMap : QuestionMap | undefined = newExam.questions.find((q: QuestionMap) => q.questionId === question.id);
                if (questionMap && answers) {
                    answersSelected = answers.map(answer => questionMap.answers[answer.id] || false);
                }
            }
        } finally {
            isLoading = false;
            correctAnswersCount = answers?.filter(answer => answer.isCorrect).length || 0;
        }
    };
    $: {
        if (question) {
            fetchAnswers();
        }
    }

    function toggleAnswer(index : number) {
        if (isLoading) return;
        if (!question || !answers) return;
        if (!question.isMultipleChoice) {
            answersSelected = answersSelected.map((value, i) => i === index ? !answersSelected[index] : false);
        } else {
            answersSelected[index] = !answersSelected[index];
        }
        if (!isLoading) {
            questionMap = newExam.questions.find((q: QuestionMap) => q.questionId === question.id);
            if (questionMap) {
                questionMap.answers = answers.reduce((acc, answer, index) => {
                    acc[answer.id] = answersSelected[index];
                    return acc;
                }, {} as { [key: number]: boolean });
                localStorage.setItem('newExam', JSON.stringify(newExam));
            }
        }
    }
</script>

<div id="questionwrapper">
    <div style="width: 100%; height:15%; background-color:white; border-radius:1rem; margin-top:0.5rem; text-shadow:none; color:#000;
     display:flex; flex-direction:row; justify-content:center; align-items:center; overflow:hidden; position:relative;">
        <h2 style="margin: 0; line-height:100%;">{question.question}</h2>
        <h4 style="margin: 0; line-height:100%; position:absolute; left:1%; top:50%; transform:translateY(-50%);">№{index+1}</h4>
        <div style="height: 100%; display:flex; flex-direction:row; justify-content:center;align-items:center; gap:2rem; position:absolute; right:1%; top:50%; transform:translateY(-50%)">
            <h5 style="margin: 0; line-height:100%;">Точки: {question.points}</h5>
            <h5 style="margin: 0; line-height:100%;">Верни: {correctAnswersCount}</h5>
        </div>
    </div>
    <div id="mediaoutput" style="background-image:url({question.mediaURL});"></div>
    <div style="width:100%;height:100%; display:flex; justify-content:flex-end; align-items:center; flex-direction:column">
        <div class="answers">
            {#if answers}
                {#each answers as answer, index}
                    <button class="answer" class:correctAnswer={examSubmitted && answersSelected[index] && answer.isCorrect} 
                    class:wrongAnswer={examSubmitted && answersSelected[index] && !answer.isCorrect}
                     class:selectedAnswer={answersSelected[index] && !examSubmitted}
                    on:click={()=>toggleAnswer(index)}
                        id={String(answer.id)} name={String("answer" + index)} disabled={isLoading || examSubmitted}>{answer.text}</button>
                {/each}
            {/if}
        </div>
    </div>
</div>

<style>
    .wrongAnswer {
        background-color: #dc3545 !important; 
        color: white !important;
        border-color: white !important;
    }
    .correctAnswer {
        background-color: #198754 !important; 
        color: white !important;
        border-color: white !important;
    }
    .selectedAnswer {
        background-color: #0d6efd !important; 
        color: white !important;
        border-color: white !important;
    }
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
        box-shadow: 0 0 1px 1px #bfbfbfff;
        font-size: 1.3rem;
        padding:0.5rem;
        transition: box-shadow 0.1s ease-in-out;
    }
    .answer:hover {
        background-color: #f1f0f0;
        box-shadow: 0 0 5px 2px #0d6efd;
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