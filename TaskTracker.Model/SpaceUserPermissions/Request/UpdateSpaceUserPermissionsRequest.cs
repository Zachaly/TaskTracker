namespace TaskTracker.Model.SpaceUserPermissions.Request
{
    public class UpdateSpaceUserPermissionsRequest
    {
        public long SpaceId { get; set; }
        public long UserId { get; set; }

        public bool CanAddUsers { get; set; }
        public bool CanRemoveUsers { get; set; }
        public bool CanChangePermissions { get; set; }
        public bool CanModifyLists { get; set; }
        public bool CanRemoveLists { get; set; }
        public bool CanModifyTasks { get; set; }
        public bool CanRemoveTasks { get; set; }
        public bool CanAssignTaskUsers { get; set; }
        public bool CanModifyStatusGroups { get; set; }
        public bool CanModifySpace { get; set; }
    }
}
