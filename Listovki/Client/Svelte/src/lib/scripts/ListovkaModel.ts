export interface QuestionMap {
    questionId : number;
    answers: { [key: number]: boolean };
}

export interface ListovkaInputModel {
    userId : string;
    category: string,
    questions : QuestionMap[];
}

export interface ListovkaResultModel 
{
    id : number;
    percentageRight : number;
    questionsNumber : number;
    guessedQuestionsNumber : number;
    category: string,
    userEmail : string;
    user : string;
}
