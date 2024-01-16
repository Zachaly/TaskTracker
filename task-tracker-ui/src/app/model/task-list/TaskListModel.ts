import TaskStatusGroupModel from "../task-status-group/TaskStatusGroupModel";
import UserModel from "../user/UserModel";
import UserTaskModel from "../user-task/UserTaskModel";

export default interface TaskListModel {
    id: number,
    title: string,
    description?: string,
    color?: string
    creator: UserModel
    tasks?: UserTaskModel[],
    statusGroupId: number,
    statusGroup?: TaskStatusGroupModel,
    spaceId: number
}