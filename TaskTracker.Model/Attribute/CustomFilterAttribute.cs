using TaskTracker.Model.Enum;

namespace TaskTracker.Model.Attribute
{
    public class CustomFilterAttribute : System.Attribute
    {
        public ComparisonType ComparisonType { get; set; }
    }
}
