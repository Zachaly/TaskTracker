namespace TaskTracker.Model.Response
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
        public IDictionary<string, string[]>? ValidationErrors { get; set; }

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
