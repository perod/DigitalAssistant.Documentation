using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownReader
{
    public class MarkdownReader
    {
        private List<MarkdownResult> _markdownResults;

        public string BotName { get; private set; } = "Wanda";
        public string KnowledgebaseUrl { get; private set; } = "https://get-wanda-help.u4pp.com/";
        public string FileName { get; private set; } = "WandaFaq.txt";
        public List<QuestionWithAnswer> QuestionWithAnswers { get; set; } = new List<QuestionWithAnswer>();


        public async Task ReadMarkdownFiles(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            var markdownFiles = info.GetFiles("*.md");

            if (markdownFiles.Any())
            {
                var tasks = markdownFiles.Select(m => AnalyzeMarkdownFile(m)).ToArray();
                var results = new List<MarkdownResult>();

                await Task
                    .WhenAll(tasks)
                    .ContinueWith((markdownResults) =>
                    {
                        if (markdownResults.Status == TaskStatus.RanToCompletion)
                        {
                            if (markdownResults.Result.Any())
                            {
                                BuildKnowledgebase(markdownResults.Result.ToList());
                            }
                        }
                        else
                        {
                            throw new Exception("Cannot continue operation. The tasks are not completed.");
                        }
                    });
            }
            else
            {
                //Log it
            }
        }


        /*public async Task ReadMarkdownFiles(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            var markdownFiles = info.GetFiles("*.md");

            if (markdownFiles.Any())
            {
                var tasks = markdownFiles.Select(m => AnalyzeMarkdownFile(m)).ToArray();
                var results = new List<MarkdownResult>();

                await Task
                    .WhenAll(tasks)
                    .ContinueWith(async (markdownResults) =>
                    {
                        if (markdownResults.Status == TaskStatus.RanToCompletion)
                        {
                            if (markdownResults.Result.Any())
                            {
                                await BuildKnowledgebase(markdownResults.Result.ToList());
                            }
                        }
                        else
                        {
                            throw new Exception("Cannot continue operation. The tasks are not completed.");
                        }
                    });
            }
            else
            {
                //Log it
            }
        }*/


        private void BuildKnowledgebase(List<MarkdownResult> markdownResults)
        {
            _markdownResults = markdownResults;

            AddMainSummaryQuestion();
            AddTopicSummaryQuestions();
            AddTopicQuestions();
            //await StoreKnowledgebaseFile();
        }

        private void AddTopicQuestions()
        {
            _markdownResults.ForEach(m => {
                QuestionWithAnswers.AddRange(m.QuestionWithAnswers);
            });
        }

        private void AddMainSummaryQuestion()
        {
            var topics = string.Join(", ", _markdownResults.Select(m => m.Name).OrderBy(m => m));
            var answer = $"I can assist you with {topics}. If you need more help, then try my [Help center]({KnowledgebaseUrl}) to learn more about what I can do.";
            QuestionWithAnswers.Add(new QuestionWithAnswer
            {
                Question = "Help?",
                Answer = answer
            });
        }

        private void AddTopicSummaryQuestions()
        {
            _markdownResults.ForEach(m => {
                if (m.QuestionWithAnswers.Any())
                {
                    var mainQuestion = $"Can you help me with {m.Name}?";
                    var answer = $"For {m.Name} I have the following topics:\\n\r\n";

                    m.QuestionWithAnswers.ForEach(qa =>
                    {
                        var question = qa.Question.EndsWith("?") ? qa.Question.Substring(0, qa.Question.Length - 1) : qa.Question;
                        answer += $"*   {question}\\n\r\n";
                    });
                    
                    answer += $"\\n\\nIf you need more help, then try my [Help center]({KnowledgebaseUrl.TrimEnd(new[] { '/' })}/{m.Name}) to learn more about what I can do.";

                    QuestionWithAnswers.Add(new QuestionWithAnswer
                    {
                        Question = mainQuestion,
                        Answer = answer
                    });
                }
            });
        }

        private async Task StoreKnowledgebaseFile()
        {
            var qaText = string.Empty;

            QuestionWithAnswers.ForEach(qa =>
            {
                qaText += $"{qa.Question}\r\n";
                qaText += $"{qa.Answer}\r\n\r\n";
            });

            await FileExtensions.WriteAllTextAsync(FileName, qaText);
        }

        private Task<MarkdownResult> AnalyzeMarkdownFile(FileInfo markdownFileInfo)
        {
            return MarkdownResult.Create(markdownFileInfo, KnowledgebaseUrl);
        }
    }
}
