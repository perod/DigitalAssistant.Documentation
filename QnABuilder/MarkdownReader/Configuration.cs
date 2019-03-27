using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownReader
{
    public class Configuration
    {
        public string QnABaseUrl { get; private set; } = ConfigurationManager.AppSettings["qna:base-url"];
        public string QnAKnowledgebaseKey { get; private set; } = ConfigurationManager.AppSettings["qna:knowledgebase-key"];
        public string QnAKSubscriptionKey { get; private set; } = ConfigurationManager.AppSettings["qna:subscription-key"];
    }
}
