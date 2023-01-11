export interface CreateUserCommand {
    login: string;
    password: string;
    isPasswordKeptAsHash: boolean;
}