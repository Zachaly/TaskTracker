export default interface ChangeUserPasswordRequest {
    userId: number,
    currentPassword: string,
    newPassword: string
}