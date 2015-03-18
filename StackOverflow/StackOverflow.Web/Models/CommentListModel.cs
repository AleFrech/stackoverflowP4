using System;

namespace StackOverflow.Web.Models
{
    public class CommentListModel
    {
        public int Votes { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
        public string OwnerName { get; set; }
        public Guid OwnerId { get; set; }
        public Guid FatherId { get; set; }
        public Guid CommentId { get; set; }

    }
}