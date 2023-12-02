import UserTaskPriority from "../enum/UserTaskPriority";

export default interface UpdateUserTaskRequest {
    id: number,
    title?: string,
    description?: string,
    dueTimestamp?: number,
    statusId?: number,
    priority?: UserTaskPriority
}