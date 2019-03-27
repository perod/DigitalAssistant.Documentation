using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownReader
{
    [DataContract]
    public class UpdateDto
    {
        public UpdateDto() { }
        public UpdateDto(List<QuestionWithAnswer> pairsToAdd = null, List<QuestionWithAnswer> pairsToDelete = null)
        {
            Add = new AddDto
            {
                QnaPairs = pairsToAdd != null ? pairsToAdd : new List<QuestionWithAnswer>(),
                Urls = new List<string>()
            };

            Delete = new DeleteDto
            {
                QnaPairs = pairsToDelete != null ? pairsToDelete : new List<QuestionWithAnswer>()
            };
        }

        [DataMember(Name = "add")]
        public AddDto Add
        {
            get;
            set;
        }

        [DataMember(Name = "delete")]
        public DeleteDto Delete
        {
            get;
            set;
        }
    }

    [DataContract]
    public class AddDto
    {
        [DataMember(Name = "qnaPairs")]
        public List<QuestionWithAnswer> QnaPairs
        {
            get;
            set;
        }

        [DataMember(Name = "urls")]
        public List<string> Urls
        {
            get;
            set;
        }
    }

    [DataContract]
    public class DeleteDto
    {
        [DataMember(Name = "qnaPairs")]
        public List<QuestionWithAnswer> QnaPairs
        {
            get;
            set;
        }
    }
}
