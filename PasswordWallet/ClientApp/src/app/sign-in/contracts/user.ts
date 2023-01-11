export interface User {
    id: number;
    login: string;
    lastSigninDateTime: Date;
    wasSuccessfulSignin: boolean;
    sessionKey: string;
    addressIP: string;
}