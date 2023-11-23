<script lang="ts">
    import Croppie from 'croppie';
    import 'croppie/croppie.css';
    import { onMount } from 'svelte';
    import * as ExamManager from '../../../lib/scripts/ExamManager';
    import Layout from '../../+layout.svelte';
    import { goto } from '$app/navigation';

    let mediaURLInput: HTMLInputElement;
    let croppie: Croppie | null = null;
    let croppedImageUrl: string | null = null;
    let isCroppieOpen = false;
    let originalFileName: string | null = null;

    function onFileChange(event : Event) {
        const target = event.target as HTMLInputElement;
        if (target && target.files && target.files[0]) {
            mediaURL = target.files[0];
            const file = target.files[0];
            originalFileName = file.name;
            const reader = new FileReader();
            reader.onload = function(e) {
                const el = document.getElementById('croppieModalContent');
                if (el && e.target) {
                    croppie = new Croppie(el, {
                        viewport: { width: 500, height: 250 },
                        boundary: { width: 500, height: 400 },
                    });
                    if (croppie) {
                        croppie.bind({ url: e.target.result as string });
                    }
                }
            };
            reader.readAsDataURL(file);
            isCroppieOpen = true;
        }
    }


    let croppedImage: File | null = null; // Add this line
    async function applyCrop() {
        if (croppie) {
            const croppedImageDataUrl = await croppie.result({ type: 'base64' });
            const blob = await (await fetch(croppedImageDataUrl)).blob();
            const file = new File([blob], `${originalFileName?.split('.').slice(0, -1).join('.')}_cropped.${originalFileName?.split('.').pop()}`, { type: 'image/png' });
            croppedImage = file; // Update this line
            isCroppieOpen = false;
        }
    }


    function closeCroppie() {
        if (croppie) {
            croppie.destroy();
            croppie = null;
            isCroppieOpen = false;
            mediaURLInput.value = '';
        }
    }

    let question : string = '';
    let category : string = '';
    let points : number = 0;
    let isMultipleChoice : boolean = false;
    let mediaURL: File | null = null;

    async function onSubmit(event: Event) {
        event.preventDefault();

        const questionModel: ExamManager.QuestionInputModel = {
            Id: 0,
            Question: question,
            Points: points,
            Category: category,
            IsMultipleChoice: isMultipleChoice,
            ImageFile: croppedImage 
        };

        let questionId : number | undefined = await ExamManager.createQuestion(questionModel);
        console.log("QUESTION ID IS: " + questionId);
        if (questionId) {
            for (let i = 0; i < 4; i++) {
                if (answers[i].length > 0) {
                    await ExamManager.addAnswer(answers[i], isCorrect[i], questionId);
                }
            }
            goto('/listovki');
        }
    }

    $: answer1Enabled = true;
    $: answer2Enabled = true;
    $: answer3Enabled = false;
    $: answer4Enabled = false;

    function toggleAnswer(currentState : boolean) {
        let enabledCount = [answer1Enabled, answer2Enabled, answer3Enabled, answer4Enabled].filter(v => v).length;
        // If the current state is enabled and there are only 2 enabled, prevent disabling
        if (currentState && enabledCount <= 2) {
            return true;
        }
        // Otherwise, toggle the state
        return !currentState;
    }

    let answer1 : string = '';
    let answer2 : string = '';
    let answer3 : string = '';
    let answer4 : string = '';
    let answers : string[] = [];
    $: {
        answer1Enabled ? answers[0] = answer1 : answers[0] = '';
        answer2Enabled ? answers[1] = answer2 : answers[1] = '';
        answer3Enabled ? answers[2] = answer3 : answers[2] = '';
        answer4Enabled ? answers[3] = answer4 : answers[3] = '';
    }
    
    let isCorrect = [false, false, false, false];
    $: {
        if (!isMultipleChoice) {
            isCorrect = [false, false, false, false];
        }
    }

    function toggleCorrect(index : number) {
        if (answers[index].length === 0) {
            return;
        }
        if (!isMultipleChoice) {
            isCorrect = isCorrect.map((val, i) => i === index ? !val : false);
        } else {
            let enabledCount = [answer1Enabled, answer2Enabled, answer3Enabled, answer4Enabled].filter(v => v).length;
            let correctCount = isCorrect.filter(v => v).length;
            if (isCorrect[index] || correctCount < enabledCount - 1) {
                isCorrect[index] = !isCorrect[index];
            }
        }
    }
</script>

{#if isCroppieOpen}
    <div id="croppieModal">
        <div id="croppieModalContent">
            <div style="display: flex; flex-direction:row; order:5;">
                <button class="btn btn-success" on:click={applyCrop}>Apply</button>
                <button class="btn btn-danger" on:click={closeCroppie}>Close</button>
            </div>
        </div>
    </div>
{/if}

<div class="wrapper" style="padding:0; overflow:hidden; height:45rem; width:40%;">
    <div id="gosling" class="mb-3">
        <h1 class="mb-3">Създай въпрос като Admin</h1>
    </div>
    <form method="post" enctype="multipart/form-data" on:submit={onSubmit}
      style="display: flex; flex-direction:column; align-items:center; text-shadow:none; width:100%;" class="text-dark">
        <div class="form-group mb-3">
            <label for="mediaURL">Снимка/Видео</label>
            <input class="form-control" name="mediaURL" type="file" bind:this={mediaURLInput} on:change={onFileChange}
              id="mediaURL" placeholder="Media URL" required />
        </div>
        <div class="form-floating mb-3">
            <input class="form-control" name="question" type="text" id="question" bind:value={question} placeholder="Question" required />
            <label for="question">Въпрос</label>
        </div>
        <div style="display: flex; flex-direction:row; width:70%; justify-content:center;">
            <div style="display: flex; flex-direction:column; width:50%; margin-right:0.3rem;">
                <div class="form-floating mb-3">
                    <select class="form-control" name="category" bind:value={category} id="category" required>
                        <option value="A">A</option>
                        <option value="B">B</option>
                        <option value="C">C</option>
                        <option value="D">D</option>
                    </select>
                    <label for="points">За категория</label>
                </div>
                <div class="form-floating mb-3">
                    <select class="form-control" name="points" bind:value={points} id="points" required>
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                    </select>
                    <label for="points">Точки</label>
                </div>
                <div class="form-check form-switch mb-3">
                    <input class="form-check-input" name="isMultipleChoice" bind:checked={isMultipleChoice} type="checkbox" id="isMultipleChoice" />
                    <label class="form-check-label" for="isMultipleChoice">Mножествен избор</label>
                </div>
            </div>
            <div style="display: flex; flex-direction:row; width:50%; height:75%; flex-wrap:wrap; position:relative; overflow:visible;">
                <div class="answer" style="width:50%; display:flex; flex-direction:row;">
                    <div style="display:flex; flex-direction:column; height:100%;">
                        <input name="answer1" disabled={!answer1Enabled} bind:value={answer1} class="form-control" placeholder="Отговор 1"/>
                        <button class="correct-button" class:enabled={isCorrect[0]} on:click={() => toggleCorrect(0)} type="button"></button>
                    </div>
                    <button type="button" on:click={() => {answer1Enabled = toggleAnswer(answer1Enabled)}} class:enabled={answer1Enabled}></button>
                </div>
                <div class="answer" style="width:50%; display:flex; flex-direction:row;">
                    <div style="display:flex; flex-direction:column; height:100%;">
                        <input name="answer2" disabled={!answer2Enabled} bind:value={answer2} class="form-control" placeholder="Отговор 2"/>
                        <button class="correct-button" class:enabled={isCorrect[1]} on:click={() => toggleCorrect(1)} type="button"></button>
                    </div>
                    <button type="button" on:click={() => {answer2Enabled = toggleAnswer(answer2Enabled)}} class:enabled={answer2Enabled}></button>
                </div>
                <div class="answer" style="width:50%; display:flex; flex-direction:row;">
                    <div style="display:flex; flex-direction:column; height:100%;">
                        <input name="answer3" disabled={!answer3Enabled} bind:value={answer3} class="form-control" placeholder="Отговор 3"/>
                        <button class="correct-button" class:enabled={isCorrect[2]} on:click={() => toggleCorrect(2)} type="button"></button>
                    </div>
                    <button type="button" on:click={() => {answer3Enabled = toggleAnswer(answer3Enabled)}} class:enabled={answer3Enabled}></button>
                </div>
                <div class="answer" style="width:50%; display:flex; flex-direction:row;">
                    <div style="display:flex; flex-direction:column; height:100%;">
                        <input name="anser4" disabled={!answer4Enabled} bind:value={answer4} class="form-control" placeholder="Отговор 4"/>
                        <button class="correct-button" class:enabled={isCorrect[3]} on:click={() => toggleCorrect(3)} type="button"></button>
                    </div>
                    <button type="button" on:click={() => {answer4Enabled = toggleAnswer(answer4Enabled)}} class:enabled={answer4Enabled}></button>
                </div>
                <p>Избери желания брой отговори и въведи текст в полетата им.</p>
            </div>
        </div>
        <div style="display: flex; flex-direction:row; justify-content:space-between">
            <button type="submit" class="btn btn-success" style="width:16rem; height:3rem; font-size:1.2rem;">Създай</button>
            <button type="reset" class="btn btn-danger" style="width:16rem; height:3rem; font-size:1.2rem;">Изчисти</button>
        </div>
    </form>
</div>

<style>
    .enabled:hover {
        background-color: #007bff !important;
        box-shadow: inset 0 0 5px 1px white !important;
    }
    .correct-button {
        outline: none;
        height:25%;
        border:1px solid rgb(206, 212, 218) !important;
        border-radius: 0 0 0.3rem 0.3rem;
        background-image: url('/check.svg');
        background-repeat: no-repeat;
        background-size: 1.5rem;
        background-position-y: 50%;
        background-position-x: center;
        transition: background-color 0.1s ease-in-out;
    }
    .correct-button:hover {
        background-color: rgb(228, 231, 233);
    }
    .answer {
        padding-right:0.2rem;
        height:4.6rem;
    }
    .answer > div > input {
        height:75%;
        border-radius: 0.3rem 0 0 0 !important;
    }
    .answer > button {
        background-color: white;
        border-radius: 0 0.3rem 0.3rem 0; 
        border:1px solid rgb(206, 212, 218);
        height:75%;
        transition: background-color 0.1s ease-in-out;
    }
    .enabled {
        background-color: #007bff !important;
        border-color: white !important;
        box-shadow: inset 0 0 3px 0.5px white !important;
    }
    .answer > button:hover {
        background-color: rgb(228, 231, 233);
    }
    #croppieModal {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        position: fixed;
        z-index: 1;
        left: 0;
        top: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0,0,0,0.4);
    }

    #croppieModalContent {
        display: flex;
        flex-direction: column;
        align-items: center;
        background-color: rgba(255, 255, 255, 1);
        border-radius: 1rem;
        padding: 1rem;
        border: 1px solid #888;
        width: 45rem;
        height: 30rem;
    }
    #gosling {
        background-image: url('/idrive2.jpg');
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: end;
        height: 100%;
        width: 100%;
        background-repeat: no-repeat;
        background-size: cover;
        background-position: center;
    }
    .form-check {
        display: flex;
        flex-direction: row;
        align-items: center;
    }
    .form-check-input {
        height:30px;
        width:60px;
    }
    .form-check-label {
        font-size:1rem;
        margin:none; margin-left: 0.5rem;
        line-height:30px;
    }
    form > div {
        width: 70%;
    }
    form > div > label {
        font-size: 1rem;
    }
</style>