using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarkdownReader
{
    public class MarkdownResult
    {
        FileInfo _markdownFileInfo;

        Lazy<Regex> _questionRegex = new Lazy<Regex>(() => new Regex("(?<=#+\\s{0,2}?)([\\w\\s,\\.;:\\'\"!$% ()]+\\?)"));

        private string _currentQuestion;
        private string _currentAnswer = string.Empty;
        private bool _previousIsBullit;

        public string FileName { get; private set; }

        public string Name { get; private set; }

        public string Text { get; private set; }
        public List<string> Lines { get; private set; }

        public List<QuestionWithAnswer> QuestionWithAnswers { get; set; } = new List<QuestionWithAnswer>();
        public string KnowledgebaseUrl { get; private set; }

        public MarkdownResult(FileInfo markdownFileInfo, string knowledgebaseUrl)
        {
            _markdownFileInfo = markdownFileInfo;
            KnowledgebaseUrl = knowledgebaseUrl;
        }

        private async Task Analyze()
        {
            FileName = _markdownFileInfo.FullName;
            Name = _markdownFileInfo.Name.Replace(".md", "").ToLowerInvariant();
            Text = await FileExtensions.ReadAllTextAsync(_markdownFileInfo.FullName);

            Lines = Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            Lines.ForEach(l =>
            {
                l = l.Trim();

                if (!AddQuestion(l))
                {
                    AddAnswer(l);
                }
            });

            AddQuestionWithAnswer();
        }

        private bool AddQuestion(string line)
        {
            var match = _questionRegex.Value.Match(line);

            if (match.Success)
            {
                AddQuestionWithAnswer();

                _currentQuestion = match.Value.Trim();
                _currentAnswer = string.Empty;
            }

            return match.Success;
        }

        private void AddAnswer(string line)
        {
            if (!line.StartsWith("<a ") && !string.IsNullOrWhiteSpace(_currentQuestion))
            {
                var isBullit = line.StartsWith("* ");

                if(isBullit && !_previousIsBullit)
                {
                    if (!string.IsNullOrWhiteSpace(_currentAnswer))
                    {
                        _currentAnswer = _currentAnswer.Substring(0, _currentAnswer.Length - "\r\n".Length);
                    }

                    _currentAnswer += "\\n\r\n";
                }

                _currentAnswer += line + (isBullit ? "\\n" : "") + "\r\n";

                _previousIsBullit = isBullit;
            }
        }

        private void AddQuestionWithAnswer()
        {
            if (!string.IsNullOrWhiteSpace(_currentQuestion) && !string.IsNullOrWhiteSpace(_currentAnswer))
            {
                _currentAnswer += $"\\n\\nIf you need more help, then try my [Help center]({KnowledgebaseUrl.TrimEnd(new[] { '/' })}/{Name}) to learn more about what I can do.";

                QuestionWithAnswers.Add(new QuestionWithAnswer { Question = _currentQuestion, Answer = _currentAnswer.Trim() });
            }
        }


        public static async Task<MarkdownResult> Create(FileInfo markdownFileInfo, string knowledgebaseUrl)
        {
            var result = new MarkdownResult(markdownFileInfo, knowledgebaseUrl);
            await result.Analyze();
            return result;
        }
    }
}