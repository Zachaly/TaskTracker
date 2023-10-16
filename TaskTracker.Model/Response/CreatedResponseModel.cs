namespace TaskTracker.Model.Response
{
    public class CreatedResponseModel : ResponseModel
    {
        public long? NewEntityId { get; set; }

        public CreatedResponseModel() : base("") { }

        public CreatedResponseModel(string error) : base(error) { }

        public CreatedResponseModel(long id) : base()
        {
            NewEntityId = id;
        }

        public CreatedResponseModel(IDictionary<string, string[]> validationErrors) : base(validationErrors) { }
    }
}
