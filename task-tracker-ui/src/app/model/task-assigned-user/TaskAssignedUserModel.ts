import UserTaskModel from "../user-task/UserTaskModel";
import UserModel from "../user/UserModel";

export default interface TaskAssignedUserModel {
    task: UserTaskModel,
    user: UserModel
}