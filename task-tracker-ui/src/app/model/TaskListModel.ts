import UserModel from "./UserModel";

export default interface TaskListModel {
    id: number,
    title: string,
    description?: string,
    color?: string
    creator: UserModel
}