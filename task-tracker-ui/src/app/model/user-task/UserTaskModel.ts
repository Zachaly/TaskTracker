import UserModel from "../user/UserModel";
import UserTaskStatusModel from "../user-task-status/UserTaskStatusModel";
import UserTaskPriority from "../enum/UserTaskPriority";

export default interface UserTaskModel {
    id: number,
    title: string,
    description: string,
    creator: UserModel,
    dueTimestamp?: number,
    creationTimestamp: number,
    status: UserTaskStatusModel,
    priority?: UserTaskPriority
}