<script lang='ts'> 
    import { onMount } from 'svelte';
    import * as ExamManager from '../../lib/scripts/ExamManager';

    let questions : ExamManager.Question[] | undefined = [];

    let sortDownIcon = '/sort-down.svg';
    let sortUpIcon = '/sort-up.svg';

    onMount(async () => {
        questions = await ExamManager.listQuestions();

        if (questions) {
            questions = await Promise.all(questions.map(async question => {
                const imageUrl = await ExamManager.getQuestionImage(question.id);
                return { ...question, mediaURL: imageUrl };
            }));
        }
    });
</script>
  
<div class="wrapper" style="width:85%; height:90%; text-shadow:none;">
    <a href="/listovki/create" class="btn btn-primary mb-3">Create new if admin</a>
    <h1>Въпроси</h1>
    <table class="bg-white text-dark">
        <thead>
          <tr>
            <th>
                <div><button class="btn btn-outline-info"><p>Номер</p><img src={sortDownIcon} alt="Sort"/></button></div>
            </th>
            <th>
                Текст
                <input type="text" class="form-control" placeholder="Търсене по текст...">
            </th>
            <th><div><button class="btn btn-outline-info"><p>Категория</p><img src={sortDownIcon} alt="Sort"/></button></div></th>
            <th><div><button class="btn btn-outline-info"><p>Номер</p><img src={sortDownIcon} alt="Sort"/></button></div></th>
            <th>Radio</th>
            <th>Снимка</th>
          </tr>
        </thead>
        <tbody>
            {#if questions}
                {#each questions as question}
                <tr>
                    <td>{question.id}</td>
                    <td>{question.question}</td>
                    <td>{question.category}</td>
                    <td>{question.points}</td>
                    <td>{!question.isMultipleChoice ? 'Да' : 'Не'}</td>
                    <td><img src={question.mediaURL} alt="question media"/></td>
                </tr>
                {/each}
            {:else}
                <p>Loading...</p>
            {/if}
        </tbody>
      </table>
</div>

<style>
    button {
        display: flex;
        flex-direction: row;
        gap:0.5rem;
        height:2.8rem;
    }
    button > img {
        height:2rem;
        filter: brightness(0) saturate(100%) invert(100%) sepia(94%) saturate(0%) hue-rotate(248deg) brightness(106%) contrast(106%);
    }
    button > p {
        color:white;
        font-size: 1.1rem;
        line-height: 2rem;
    }
    table {
        width: 100%;
        border-collapse: collapse;
        border-radius: 2rem;
        overflow: hidden;
    }
    th, td {
        text-align: center;
        padding: 8px;
        border: 1px solid #ddd;
    }
    td > img {
        width:14rem;
        height:7rem;
        object-fit: cover;
    }
    th {
        background-color: #0d6efd;
        color:white;
    }
    th > div {
        width: 100%;
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
    }
    input[type="text"] {
        margin-bottom: 10px;
        padding: 5px;
    }
    button {
        padding: 5px 10px;
    }
    td {
        font-size: 1.2rem;
    }
  </style>