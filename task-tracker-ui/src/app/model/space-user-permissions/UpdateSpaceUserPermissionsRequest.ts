export default interface UpdateSpaceUserPermissionsRequest {
    userId: number,
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