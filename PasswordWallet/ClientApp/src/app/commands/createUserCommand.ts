export interface UpdateUserCommand {
    login: string
    password: string
    isPasswordKeptAsHash: boolean
}