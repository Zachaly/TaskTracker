import UserTaskStatusModel from "../user-task-status/UserTaskStatusModel"

export default interface TaskStatusGroupModel {
    id: number
    name: string
    isDefault: boolean
    statuses?: UserTaskStatusModel[]
}