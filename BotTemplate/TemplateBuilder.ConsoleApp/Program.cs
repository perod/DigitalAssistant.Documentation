using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TemplateBuilder.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var location = Assembly.GetExecutingAssembly().Location;

                var fileInfo = new FileInfo(location);
                var rootFolder = $"{fileInfo.DirectoryName.TrimEnd('\\')}\\..\\..\\..\\";

                var templateGenerator = new Generator();
                templateGenerator.Generate(new Template
                {
                    SourceProjects = new List<string>
                    {
                        $"{rootFolder}SampleChatbot\\SampleChatbot.csproj",
                        $"{rootFolder}SampleChatbot.Tests\\SampleChatbot.Tests.csproj",
                        $"{rootFolder}SampleChatbot.Emulator\\SampleChatbot.Emulator.csproj",
                        $"{rootFolder}SampleChatbot.Webjob\\SampleChatbot.Webjob.csproj"
                    },
                    DestinationProject = $"{rootFolder}U4.Chatbot.ProjectTemplate\\U4.Chatbot.ProjectTemplate.csproj",
                    RootNamespace = "SampleChatbot"
                }).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occured. Error: {ex.Message}. Press any key to exit.");
                Console.ReadKey();

            }
        }
    }
}
