import UserModel from "../user/UserModel";
import UserTaskStatusModel from "../user-task-status/UserTaskStatusModel";
import UserTaskPriority from "../enum/UserTaskPriority";
import TaskFileAttachmentModel from "../task-file-attachment/TaskFileAttachmentModel";

export default interface UserTaskModel {
    id: number,
    title: string,
    description: string,
    creator: UserModel,
    dueTimestamp?: number,
    creationTimestamp: number,
    status: UserTaskStatusModel,
    priority?: UserTaskPriority
    assignedUsers: UserModel[],
    listId: number,
    attachments: TaskFileAttachmentModel[]
}