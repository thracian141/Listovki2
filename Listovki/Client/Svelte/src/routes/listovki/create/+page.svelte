<script lang="ts">
    import Croppie from 'croppie';
    import 'croppie/croppie.css';
    import { onMount } from 'svelte';
    import * as ExamManager from '../../../lib/scripts/ExamManager';

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

        await ExamManager.createQuestion(questionModel);
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
            <label class="form-check-label" for="isMultipleChoice">С множествен избор</label>
        </div>
        <div style="display: flex; flex-direction:row; justify-content:space-between">
            <button type="submit" class="btn btn-success" style="width:16rem; height:3rem; font-size:1.2rem;">Създай</button>
            <button type="reset" class="btn btn-danger" style="width:16rem; height:3rem; font-size:1.2rem;">Изчисти</button>
        </div>
    </form>
</div>

<style>
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
        font-size:1.05rem;
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