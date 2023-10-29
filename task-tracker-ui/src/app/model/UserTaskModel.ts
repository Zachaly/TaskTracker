import UserModel from "./UserModel";

export default interface UserTaskModel {
    id: number,
    title: string,
    description: string,
    creator: UserModel,
    dueTimestamp?: number,
    creationTimestamp: number
}