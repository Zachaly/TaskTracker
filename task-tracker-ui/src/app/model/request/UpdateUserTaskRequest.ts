export default interface UpdateUserTaskRequest {
    id: number,
    title?: string,
    description?: string,
    dueTimestamp?: number
}