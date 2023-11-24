export interface QuestionMap {
    questionId : number;
    answers: { [key: number]: boolean };
}

export interface ListovkaInputModel {
    userId : string;
    questions : QuestionMap[];
}
