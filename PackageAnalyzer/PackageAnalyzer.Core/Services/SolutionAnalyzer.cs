using PackageAnalyzer.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Services
{
    public class SolutionAnalyzer : ISolutionAnalyzer
    {
        //Regex without double \\: Project\(\"\{\w{8}-\w{4}-\w{4}-\w{4}-\w{12}\}\"\)\s\=\s\"([\w\.]+)\",\s\"([\w\.\\]+)\",\s\"\{\w{8}-\w{4}-\w{4}-\w{4}-\w{12}\}\"
        private const string projectFormat12Pattern = "Project\\(\"\\{\\w{8}-\\w{4}-\\w{4}-\\w{4}-\\w{12}\\}\"\\)\\s\\=\\s\"([\\w\\.]+)\",\\s\"([\\w\\.\\\\]+)\",\\s\"\\{\\w{8}-\\w{4}-\\w{4}-\\w{4}-\\w{12}\\}\"";
        private Lazy<Regex> projectFormat12Regex = new Lazy<Regex>(() => new Regex(projectFormat12Pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));
        private readonly IFileUtilities _fileUtilities;
        private readonly IProjectAnalyzer _projectAnalyzer;

        public SolutionAnalyzer(IProjectAnalyzer projectAnalyzer, IFileUtilities fileUtilities)
        {
            _fileUtilities = fileUtilities;
            _projectAnalyzer = projectAnalyzer;
        }

        public async Task<Solution> AnalyzeSolution(string solutionFile, List<string> projectReferenceNamesToIgnore = null, List<string> projectNamesToIgnore = null)
        {
            var fileInfo = new FileInfo(solutionFile);
            Solution solution = null;

            if (fileInfo.Exists)
            {
                Console.WriteLine($"Examining solution '{solutionFile}' for dependencies");
                solution = new Solution
                {
                    Name = fileInfo.Name,
                    Path = fileInfo.DirectoryName
                };

                var projects = await GetProjectPaths(solutionFile);

                var debugInfo = $"For solution {solutionFile}, the following projects are examined:{Environment.NewLine}";

                projectNamesToIgnore = projectNamesToIgnore == null ? new List<string>() : projectNamesToIgnore;
                projectReferenceNamesToIgnore = projectReferenceNamesToIgnore == null ? new List<string>() : projectReferenceNamesToIgnore;

                if (projects.Any())
                {
                    var tasks = new List<Task<Project>>();

                    projects.ForEach(p =>
                    {
                        if (p.EndsWith("csproj") && !projectNamesToIgnore.Any(i => p.IndexOf(i) >= 0))
                        {
                            debugInfo += $"\t{p}{Environment.NewLine}";
                            tasks.Add(_projectAnalyzer.AnalyzeProject(p, projectReferenceNamesToIgnore));
                        }
                    });

                    if(tasks.Any())
                    {
                        Console.WriteLine(debugInfo);
                    }

                    var projectResults = await Task.WhenAll(tasks);
                    var projectList = new ProjectList();
                    projectList.AddRange(projectResults);

                    solution.Projects = projectList;
                }
            }
            else
            {
                Console.WriteLine($"Could not locate solution '{solutionFile}'");
            }

            return solution;
        }

        private async Task<List<string>> GetProjectPaths(string solutionFile)
        {
            var fileInfo = new FileInfo(solutionFile);
            var projectPaths = new List<string>();

            if(fileInfo.Exists)
            {
                string txt = string.Empty;

                txt = await _fileUtilities.ReadFile(fileInfo);
                
                if(!string.IsNullOrWhiteSpace(txt))
                {
                    var lines = txt.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    if (lines.Length == 1)
                    {
                        lines = txt.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    }

                    lines.ToList().ForEach(l =>
                    {
                        ExamineLineAsProjectFormat12(l, fileInfo.DirectoryName, ref projectPaths);
                    });
                }
            }
            return projectPaths;
        }

        private void ExamineLineAsProjectFormat12(string lineToExamine, string basePath, ref List<string> projectPaths)
        {
            var match = projectFormat12Regex.Value.Match(lineToExamine);

            if (match.Success && match.Groups.Count == 3)
            {
                var projectPath = match.Groups[2].Value;

                var projectFullPath = Path.Combine(basePath, projectPath);

                var projectFileInfo = new FileInfo(projectFullPath);

                if (projectFileInfo.Exists)
                {
                    projectPaths.Add(projectFileInfo.FullName);
                }
            }
        }
    }
}
