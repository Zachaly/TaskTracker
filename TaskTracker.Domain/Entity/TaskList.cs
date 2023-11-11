﻿namespace TaskTracker.Domain.Entity
{
    public class TaskList : IEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public List<UserTask> Tasks { get; set; }
        public long CreatorId { get; set; }
        public User Creator { get; set; }
    }
}
