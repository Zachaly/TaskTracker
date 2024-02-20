import UserModel from "../user/UserModel";

export default interface SpaceUserPermissionsModel {
    user: UserModel,
    spaceId: number,
    canAddUsers: boolean,
    canRemoveUsers: boolean,
    canChangePermissions: boolean,
    canModifyLists: boolean,
    canRemoveLists: boolean,
    canModifyTasks: boolean,
    canRemoveTasks: boolean,
    canAssignTaskUsers: boolean,
    canModifySpace: boolean
}