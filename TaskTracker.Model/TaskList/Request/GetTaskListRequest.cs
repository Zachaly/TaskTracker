﻿using TaskTracker.Model.Attribute;

namespace TaskTracker.Model.TaskList.Request
{
    public class GetTaskListRequest : PagedRequest
    {
        public long? CreatorId { get; set; }
        [Join]
        public bool? JoinTasks { get; set; }
        [Join]
        public bool? JoinStatusGroup { get; set; }
        public long? SpaceId { get; set; }
    }
}
