using TaskTracker.Model.DocumentPage;
using TaskTracker.Model.User;

namespace TaskTracker.Model.Document
{
    public class DocumentModel : IModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long CreationTimestamp { get; set; }
        public UserModel Creator { get; set; }
        public IEnumerable<DocumentPageModel> Pages { get; set; }
    }
}
