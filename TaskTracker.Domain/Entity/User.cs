﻿namespace TaskTracker.Domain.Entity
{
    public class User : IEntity
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? RefreshToken { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}
