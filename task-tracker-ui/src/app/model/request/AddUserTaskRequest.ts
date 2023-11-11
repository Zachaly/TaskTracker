export default interface AddUserTaskRequest {
    creatorId: number,
    title: string,
    description: string,
    dueTimestamp?: number
    listId?: number
}