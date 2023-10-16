import UserModel from "./UserModel";

export default interface LoginResponse {
    userData: UserModel,
    accessToken: string,
    refreshToken: string
}