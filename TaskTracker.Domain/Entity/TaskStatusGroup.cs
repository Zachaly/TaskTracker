﻿namespace TaskTracker.Domain.Entity
{
    public class TaskStatusGroup : IEntity
    {
        public long Id { get; set; }
        public bool IsDefault { get; set; }
        public long? UserId { get; set; }
        public User? User { get; set; }
        public string Name { get; set; }
        public ICollection<UserTaskStatus> Statuses { get; set; }
        public ICollection<TaskList> Lists { get; set; }
        public ICollection<UserSpace> Spaces { get; set; }
    }
}
