using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Model.UserSpace.Request
{
    public class AddUserSpaceRequest
    {
        public long UserId { get; set; }
        public string Title { get; set; }
        public long StatusGroupId { get; set; }
    }
}
