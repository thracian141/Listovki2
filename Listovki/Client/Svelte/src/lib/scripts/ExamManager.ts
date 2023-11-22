import { goto } from "$app/navigation";

export interface QuestionInputModel {
    Id: number;
    Question: string;
    Points: number;
    Category: string;
    IsMultipleChoice: boolean;
    ImageFile: File | null;
}

export interface Question {
    id: number;
    question: string;
    category: string;
    points: number;
    isMultipleChoice: boolean;
    mediaURL?: string;
}

export async function getQuestionImage(questionId : number) {
    const cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    if (!cookie) {
        return;
    }

    const token = cookie.split('=')[1];
    const response = await fetch(`https://localhost:5000/exam/getQuestionImg?questionId=${questionId}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        },
    });

    const blob = await response.blob();
    const url = URL.createObjectURL(blob);
    return url;
}

export async function createQuestion(inputModel : QuestionInputModel) {
    const cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    if (!cookie) {
        console.log('Token not found');
        return;
    }

    const token = cookie.split('=')[1];
    
    const formData = new FormData();
    if (inputModel.ImageFile) {
        formData.append('ImageFile', inputModel.ImageFile);
    }
    formData.append('Id', inputModel.Id.toString());
    formData.append('Question', inputModel.Question);
    formData.append('Points', inputModel.Points.toString());
    formData.append('Category', inputModel.Category);
    formData.append('IsMultipleChoice', inputModel.IsMultipleChoice.toString());

    formData.forEach((value, key) => {
        console.log(key + ', ' + value);
    });
    const response = await fetch('https://localhost:5000/exam/addQuestion', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`
        },
        body: formData
    });
    console.log(response);

    if (response.ok) {
        goto('/listovki');
    } else {
        alert('Error');
    }
}


export async function listQuestions() {
    const cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    if (!cookie) {
        return;
    }

    const token = cookie.split('=')[1];
    const response = await fetch('https://localhost:5000/exam/listQuestions', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        },
    });
    const data = await response.json();

    const questions: Question[] = data.questions.map((question: Question) => ({
        id: question.id,
        question: question.question,
        category: question.category,
        points: question.points,
        isMultipleChoice: question.isMultipleChoice,
        mediaURL: question.mediaURL
    }));

    console.log(questions);
    return questions;
}