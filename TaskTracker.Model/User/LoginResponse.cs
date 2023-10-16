namespace TaskTracker.Model.User
{
    public class LoginResponse
    {
        public UserModel? UserData { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
