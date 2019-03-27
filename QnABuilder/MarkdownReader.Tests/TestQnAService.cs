using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownReader.Tests
{
    [TestClass]
    [Ignore]
    public class TestQnAService
    {

        [TestMethod]
        public async Task QnaService_Get()
        {
            var service = new QnAService();
            var tsvData = await service.Get();
        }

        [TestMethod]
        public async Task QnaService_Update()
        {
            var service = new QnAService();
            var existingData = await service.Get();
            await service.Update(
                pairsToDelete: existingData, 
                pairsToAdd: new List<QuestionWithAnswer>
                {
                    new QuestionWithAnswer
                    {
                        Question = "Hello",
                        Answer = "Hello, how can I be of assistance?"
                    },
                    new QuestionWithAnswer
                    {
                        Question = "What is Oslo?",
                        Answer = "A city"
                    }
                });
        }


        //Use this to replace the qna app.
        [TestMethod]
        public async Task QnaService_Publish()
        {
            //Read contets of existing markdown files
            var reader = new MarkdownReader();
            await reader.ReadMarkdownFiles(@"C:\VS2015\pp-git\DigitalAssistant.Documentation\Helpcenter\docs");
            if (reader.QuestionWithAnswers.Any())
            {
                //Sdk on top of QnAMaker - V2.0 Rest api
                var service = new QnAService();

                //Get all existing QnA pairs
                var existingData = await service.Get();

                //Remove all existing entries, and add the ones generated from the markdown files
                await service.Update(
                    pairsToDelete: existingData,
                    pairsToAdd: reader.QuestionWithAnswers
                );

                //Republish
                var published = await service.Publish();
            }
        }
    }
}
