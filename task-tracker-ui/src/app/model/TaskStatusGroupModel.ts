import UserTaskStatusModel from "./UserTaskStatusModel"

export default interface TaskStatusGroupModel {
    id: number
    name: string
    isDefault: boolean
    statuses?: UserTaskStatusModel[]
}