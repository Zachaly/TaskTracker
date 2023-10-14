namespace TaskTracker.Model.Response
{
    public class DataResponseModel<TData> : ResponseModel
    {
        public TData? Data { get; } = default;

        public DataResponseModel(TData data) : base()
        {
            Data = data;
        }

        public DataResponseModel(string error) : base(error) { }
        public DataResponseModel(IDictionary<string, string[]> validationErrors) : base(validationErrors) { }
    }
}
