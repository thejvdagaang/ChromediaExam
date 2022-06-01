using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamPlayground.Models {
    public class Article {
        public string? Title { get; set; }
        public string? Url { get; set; }
        public string? Author { get; set; }
        public int? Num_Comments { get; set; }
        public string? Story_ID { get; set; }
        public string? Story_Title { get; set; }
        public string? Story_Url { get; set; }
        public string? Parent_Id { get; set; }
        public string? Created_At { get; set; }
    }
}
