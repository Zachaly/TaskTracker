import UserModel from "./UserModel";
import UserTaskStatusModel from "./UserTaskStatusModel";

export default interface UserTaskModel {
    id: number,
    title: string,
    description: string,
    creator: UserModel,
    dueTimestamp?: number,
    creationTimestamp: number,
    status: UserTaskStatusModel
}