using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackageAnalyzer.ConsoleApp
{
    public class ArgumentSet
    {
        public string BuildId { get; internal set; }
        public string OutputPath { get; internal set; }
        public string SolutionConfigurations { get; internal set; }
        public List<string> RootFolders { get; internal set; }
        public string StorageConnectionString { get; internal set; }
        public string StorageName { get; internal set; }

        public bool IsValid()
        {
            return SolutionConfigurations != null;
        }
        
        public static string GetArgumentMessage()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{Environment.NewLine}Utility for analyzing .Net solutions for third party dependencies.");
            sb.AppendLine($"Provide at least one argument, -solutionConfigurations. Arugments should come in");
            sb.AppendLine("format -key:value with space as argument separator. The following keys are valid:");
            sb.AppendLine();
            sb.AppendLine($"{Tab()}-solutionConfigurations{Tab(2)}Full path to json file holding solution configurations,");
            sb.AppendLine($"{Tab(5)}as defined by schema PackageAnalyzerSchema.json, avaliable");
            sb.AppendLine($"{Tab(5)}with project PackageAnalyzer.Core.");
            sb.AppendLine();
            sb.AppendLine($"{Tab()}-outputPath{Tab(3)}Full path to where to store the analyzer HTML result. Overrries property");
            sb.AppendLine($"{Tab(5)}'outPutPath' in json provided with argument -solutionConfigurations.");
            sb.AppendLine();
            sb.AppendLine($"{Tab()}-rootPaths{Tab(3)}Root path to solutions to examine. There should be a one-to-one mapping");
            sb.AppendLine($"{Tab(5)}between this number and the number of solution configuration entries");
            sb.AppendLine($"{Tab(5)}in the json file provided with argument -solutionConfigurations.");
            sb.AppendLine($"{Tab(5)}Overrides property 'rootFolder' in the corresponding solution configuration.");
            sb.AppendLine();
            sb.AppendLine($"{Tab()}-buildId{Tab(3)}If running within the context of VSTS, the current build identifier.");
            sb.AppendLine();
            sb.AppendLine($"{Tab()}-storageConnectionString{Tab(1)}Azure storage connection string for storing results.");
            sb.AppendLine();
            sb.AppendLine($"{Tab()}-storageName{Tab(3)}Name of storage compartment.");
            return sb.ToString();
        }

        private static string Tab(int num = 1)
        {
            return new string('\t', num);
        }

        public static ArgumentSet ParseArguments(string[] args)
        {
            var argumentSet = new ArgumentSet();
            foreach (var arg in args)
            {
                ParseArgument(ref argumentSet, arg);
            }

            return argumentSet;
        }
        
        private static void ParseArgument(ref ArgumentSet argumentSet, string argument)
        {
            if(argument.IndexOf("-solutionConfigurations:") == 0)
            {
                argumentSet.SolutionConfigurations = argument.Replace("-solutionConfigurations:", "");
            }
            else if(argument.IndexOf("-outputPath:") == 0)
            {
                argumentSet.OutputPath = argument.Replace("-outputPath:", "");
            }
            else if (argument.IndexOf("-rootPaths:") == 0)
            {
                argumentSet.RootFolders = argument.Replace("-rootPaths:", "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            else if (argument.IndexOf("-buildId:") == 0)
            {
                argumentSet.BuildId = argument.Replace("-buildId:", "");
            }
            else if (argument.IndexOf("-storageConnectionString:") == 0)
            {
                var value = argument.Replace("-storageConnectionString:", "").Trim();
                if (!string.IsNullOrWhiteSpace(value) && value.ToLowerInvariant().CompareTo("null") != 0)
                {
                    argumentSet.StorageConnectionString = value;
                }
            }
            else if (argument.IndexOf("-storageName:") == 0)
            {
                var value = argument.Replace("-storageName:", "").Trim();
                if (!string.IsNullOrWhiteSpace(value) && value.ToLowerInvariant().CompareTo("null") != 0)
                {
                    argumentSet.StorageName = value;
                }

            }
        }
    }
}
