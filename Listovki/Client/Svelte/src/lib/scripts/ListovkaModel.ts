export interface QuestionMap {
    questionId : number;
    answers: { [key: number]: boolean };
}

export interface ListovkaInputModel {
    userId : string;
    questions : QuestionMap[];
}

export interface ListovkaResultModel 
{
    id : number;
    percentageRight : number;
    questionsNumber : number;
    guessedQuestionsNumber : number;
    userEmail : string;
    user : string;
}
