namespace TaskTracker.Model.Response
{
    public class ResponseModel
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public IDictionary<string, string[]>? ValidationErrors { get; }

        public ResponseModel()
        {
            IsSuccess = true;
        }

        public ResponseModel(string error)
        {
            IsSuccess = false;
            Error = error;
        }

        public ResponseModel(IDictionary<string, string[]> validationErrors) : this("Validation failed")
        {
            ValidationErrors = validationErrors;
        }
    }
}
