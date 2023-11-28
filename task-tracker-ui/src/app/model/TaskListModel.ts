import TaskStatusGroupModel from "./TaskStatusGroupModel";
import UserModel from "./UserModel";
import UserTaskModel from "./UserTaskModel";

export default interface TaskListModel {
    id: number,
    title: string,
    description?: string,
    color?: string
    creator: UserModel
    tasks?: UserTaskModel[],
    statusGroupId: number,
    statusGroup?: TaskStatusGroupModel
}