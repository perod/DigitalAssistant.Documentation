using System.Runtime.Serialization;

namespace MarkdownReader
{
    [DataContract]
    public class QuestionWithAnswer
    {
        [DataMember(Name = "question")]
        public string Question { get; set; }
        [DataMember(Name = "answer")]
        public string Answer { get; set; }
    }
}
