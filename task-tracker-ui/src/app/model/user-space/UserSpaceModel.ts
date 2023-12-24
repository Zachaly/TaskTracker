import TaskListModel from "../task-list/TaskListModel";
import TaskStatusGroupModel from "../task-status-group/TaskStatusGroupModel";
import UserModel from "../user/UserModel";

export default interface UserSpaceModel {
    id: number,
    title: string,
    statusGroup: TaskStatusGroupModel,
    lists: TaskListModel[],
    owner: UserModel
}