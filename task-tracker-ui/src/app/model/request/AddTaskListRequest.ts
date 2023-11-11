export default interface AddTaskListRequest {
    creatorId: number,
    color?: string
    description?: string,
    title: string
}