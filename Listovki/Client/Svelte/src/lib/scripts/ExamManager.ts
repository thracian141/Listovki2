import { goto } from "$app/navigation";
import type { ListovkaInputModel, ListovkaResultModel } from "./ListovkaModel";

export interface QuestionInputModel {
    Id: number;
    Question: string;
    Points: number;
    Category: string;
    IsMultipleChoice: boolean;
    ImageFile: File | null;
}

export interface AnswerInputModel {
    Id: number;
    Text: string;
    IsCorrect: boolean;
    ExamQuestionId: number;
}

export interface Answer {
    id: number;
    text: string;
    isCorrect: boolean;
    examQuestionId: number;
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

    const id = await response.text();
    console.log(id);
    return Number(id);
}

export async function addAnswer(text : string, isCorrect : boolean, questionId : number) {
    let inputModel : AnswerInputModel = {
        Id: 0,
        Text: text,
        IsCorrect: isCorrect,
        ExamQuestionId: questionId
    };

    const cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    if (!cookie) {
        console.log('Token not found');
        return;
    }

    const token = cookie.split('=')[1];

    const response = await fetch('https://localhost:5000/exam/addAnswer', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(inputModel)
    });
}

export async function listAnswersByQuestionId(questionId : number) {
    const cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    if (!cookie) {
        return;
    }

    const token = cookie.split('=')[1];
    const response = await fetch(`https://localhost:5000/exam/listAnswersByQuestionId?questionId=${questionId}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        },
    });
    const data = await response.json();
    const answers: Answer[] = data.answers.map((answer: Answer) => ({
        id: answer.id,
        text: answer.text,
        isCorrect: answer.isCorrect,
        examQuestionId: answer.examQuestionId
    }));
    return answers;
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

    return questions;
}

export async function getExamByCategory(category: string) {
    const cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    if (!cookie) {
        return;
    }

    const token = cookie.split('=')[1];
    const response = await fetch(`https://localhost:5000/exam/getExamByCategory?category=${category}`, {
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

    return questions;
}

export async function gradeExam(exam : ListovkaInputModel) {
    console.log(exam);
    const cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    if (!cookie) {
        return;
    }
    const token = cookie.split('=')[1];
    const response = await fetch('https://localhost:5000/exam/gradeExam', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(exam.questions)
    });
    const data = await response.text();

    return Number(data);
}

export async function results(listovkaId : number) {
    const cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    if (!cookie) {
        return;
    }
    const token = cookie.split('=')[1];
    const response = await fetch(`https://localhost:5000/exam/results?listovkaId=${listovkaId}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
        }
    });
    const data = await response.json();
    const listovka : ListovkaResultModel = data.listovka;
    console.log(data);
    console.log(listovka);
    return listovka;
}