using PackageAnalyzer.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Services
{
    public class HtmlRenderer : IHtmlRenderer
    {
        private readonly IFileUtilities _fileUtilities;
        private readonly ILicenseMapper _licenseMapper;

        public HtmlRenderer(IFileUtilities fileUtilities, ILicenseMapper licenseMapper)
        {
            _fileUtilities = fileUtilities;
            _licenseMapper = licenseMapper;
        }


        public async Task<string> RenderToString(RenderProperties renderProperties, SolutionList solutionList, string titleText, string headerText)
        {
            return await Render(renderProperties, solutionList, titleText, headerText);
        }

        public async Task<string> Render(SolutionConfiguration solutionConfiguration, SolutionList solutionList)
        {
            var renderProperties = solutionConfiguration.RenderProperties;
            var outputPath = solutionConfiguration.OutputPath;
            var indexFileName = solutionConfiguration.IndexFileName;
            var titleText = !string.IsNullOrWhiteSpace(solutionConfiguration.PageTitle) ? solutionConfiguration.PageTitle : "Third party dependencies";
            var headerText = !string.IsNullOrWhiteSpace(solutionConfiguration.HeaderText) ? solutionConfiguration.HeaderText : "Third party dependencies";
            return await Render(renderProperties, solutionList, titleText, headerText, outputPath, indexFileName);
        }


        private async Task<string> Render(RenderProperties renderProperties, SolutionList solutionList, string titleText, string headerText, string outputPath = null, string indexFileName = null)
        {
            var createFile = !string.IsNullOrWhiteSpace(outputPath) && !string.IsNullOrWhiteSpace(indexFileName);
            
            var packageReferences = solutionList.GetPackageReferences(!renderProperties.IncludeDuplicates, !renderProperties.IncludePackageDependencies, "<br>");
            if (packageReferences.Any())
            {
                if (createFile && !Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }

                var tabPos = 0;

                string txt = string.Empty;
                RenderLine(ref txt, tabPos, "<html>");
                RenderLine(ref txt, ++tabPos, "<head>");
                RenderLine(ref txt, ++tabPos, $"<title>{titleText}</title>");

                if (renderProperties.PageAutomaticRefresh)
                {
                    RenderLine(ref txt, tabPos, "<meta http-equiv=\"refresh\" content=\"5\">");
                }

                RenderLine(ref txt, --tabPos, "</head>");
                RenderLine(ref txt, tabPos, "<body style=\"font-family: Verdana, Helvetica, sans-serif;font-size:8pt;\">");
                RenderLine(ref txt, ++tabPos, $"<h1>{headerText}</h1>");

                if (renderProperties.IncludeSolutionInformation)
                {
                    solutionList.ForEach(s =>
                    {
                        RenderLine(ref txt, tabPos, $"<b>{s.Name}</b><br>");
                        RenderLine(ref txt, tabPos, $"<ul>");

                        s.Projects.ForEach(p =>
                        {
                            RenderLine(ref txt, ++tabPos, $"<li>{p.Name}<br></li>");
                        });

                        RenderLine(ref txt, --tabPos, $"</ul>");
                        RenderLine(ref txt, tabPos, $"<br>");
                    });

                    RenderLine(ref txt, tabPos, $"<br>");
                }

                if (createFile)
                {
                    RenderLine(ref txt, tabPos, "<button onclick=\"createMarkdown();\">Markdown to clipboard</button>");
                }

                RenderLine(ref txt, tabPos, $"<table id=\"mainTable\" cellspacing=\"1\" cellpadding=\"3\" style=\"font-size:8pt;\">");

                txt += RenderTableHeaderRow(tabPos, renderProperties);
                txt += await RenderTableRows(tabPos, renderProperties, packageReferences, outputPath, createFile);

                RenderLine(ref txt, tabPos--, "</table>");

                if (createFile)
                {
                    txt += AddMarkdownScript(tabPos);
                    txt += "<textarea type=\"text\" id=\"hiddenInput\" style=\"position:absolute;left:-100px;top:-100px;width:10px;height:10px;\"></textarea>";
                }

                RenderLine(ref txt, tabPos--, "</body>");
                RenderLine(ref txt, tabPos, "</html>");

                if (createFile)
                {
                    var file = Path.Combine(outputPath, indexFileName);
                    await _fileUtilities.CreateFile(file, txt);
                }

                return txt;
            }

            return null;
        }

        private string AddMarkdownScript(int tabPos)
        {
            var txt = string.Empty;
            RenderLine(ref txt, tabPos, "<script>");

            //pretty ugly, but kinda works.
            var script =
                "function createMarkdown() {" +
                    "var table = document.getElementById('mainTable');" +
                    "var txt = ''," +
                    "headerAdded = false," +
                    "colTxt = ''," +
                    "headerTxt = '';" +
                    "for (var i = 0, row; row = table.rows[i]; i++)" +
                    "{" +
                        "colTxt = '';" +
                        "headerTxt = '';" +
                        "for (var j = 0, col; col = row.cells[j]; j++)" +
                        "{" +
                            "if (headerAdded == false)" +
                            "{" +
                                "if(headerTxt.length > 0) { headerTxt += '|';}" +
                                "headerTxt += '---';" +
                            "}" +

                            "if(colTxt.length > 0) { colTxt += '|';}" +
                            "colTxt += col.innerText.replace('|', '#');" +
                        "}" +
                        "txt += colTxt;" +

                        "if (headerAdded == false)" +
                        "{" +
                            "txt += '\\r\\n' + headerTxt;" +
                            "headerAdded = true;" +
                        "}" +

                        "txt += '\\r\\n';" +
                    "}" +

                    "var input = document.getElementById('hiddenInput');" +
                    "input.value = txt;" +
                    "input.select();" +
                    "document.execCommand(\"Copy\")" +
                "}";

            RenderLine(ref txt, tabPos, script);
            RenderLine(ref txt, tabPos, "</script>");
            return txt;
        }

        private async Task<string> RenderTableRows(int tabPos, RenderProperties renderProperties, ProjectReferenceList packageReferences, string outputPath, bool createFile)
        {
            var txt = string.Empty;
            var i = 1;
            var renderCount = renderProperties.Count;
            var nugetProperties = renderProperties.NugetProperties;
            var packageProperties = renderProperties.PackageProperties;
            var projectReferenceProperties = renderProperties.ProjectReferenceProperties;

            foreach (var packageReference in packageReferences)
            {
                RenderLine(ref txt, ++tabPos, i % 2 == 0 ? "<tr style=\"background-color:#e9e9e9\">" : "<tr>");
                tabPos++;

                if (renderCount) RenderColumnTag(ref txt, tabPos, i.ToString());

                var nugetValues = packageReference.NugetPackage;

                if (nugetProperties != null && nugetValues != null)
                {
                    if (nugetProperties.Id) RenderColumnTag(ref txt, tabPos, nugetValues.Id);
                    if (nugetProperties.Version) RenderColumnTag(ref txt, tabPos, nugetValues.Version);
                    if (nugetProperties.Description) RenderColumnTag(ref txt, tabPos, nugetValues.Description);
                    if (nugetProperties.LicenseUrl) RenderColumnTag(ref txt, tabPos, $"<a href=\"{nugetValues.LicenseUrl}\" target=\"blank\">{nugetValues.LicenseUrl}</a>");

                    if (nugetProperties.LicenseType)
                    {
                        var fallbackText = nugetProperties.LicenseUrl ? string.Empty : nugetValues.LicenseUrl;
                        var licenseType = await _licenseMapper.GetLicenseType(nugetValues.LicenseUrl, fallbackText);
                        RenderColumnTag(ref txt, tabPos, $"<a href=\"{nugetValues.LicenseUrl}\" target=\"blank\">{licenseType}</a>");
                    }

                    if (nugetProperties.Authors) RenderColumnTag(ref txt, tabPos, nugetValues.Authors);
                    if (nugetProperties.Owners) RenderColumnTag(ref txt, tabPos, nugetValues.Owners);
                    if (nugetProperties.ProjectUrl) RenderColumnTag(ref txt, tabPos, $"<a href=\"{nugetValues.ProjectUrl}\" target=\"blank\">{nugetValues.ProjectUrl}</a>");
                }

                var packageValues = packageReference.Package;

                if (packageProperties != null && packageValues != null)
                {
                    if (packageProperties.Id) RenderColumnTag(ref txt, tabPos, packageValues.Id);
                    if (packageProperties.Version) RenderColumnTag(ref txt, tabPos, packageValues.Version);
                    if (packageProperties.TargetFramework) RenderColumnTag(ref txt, tabPos, packageValues.TargetFramework);
                }

                if (projectReferenceProperties != null)
                {
                    if (projectReferenceProperties.Name) RenderColumnTag(ref txt, tabPos, packageReference.Name);
                    if (projectReferenceProperties.Version) RenderColumnTag(ref txt, tabPos, packageReference.Version);
                    if (projectReferenceProperties.Culture) RenderColumnTag(ref txt, tabPos, packageReference.Culture);
                    if (projectReferenceProperties.PublicKeyToken) RenderColumnTag(ref txt, tabPos, packageReference.PublicKeyToken);
                    if (projectReferenceProperties.ProcessorArchitecture) RenderColumnTag(ref txt, tabPos, packageReference.ProcessorArchitecture);
                    if (projectReferenceProperties.Location) RenderColumnTag(ref txt, tabPos, packageReference.Location);
                    if (projectReferenceProperties.Private) RenderColumnTag(ref txt, tabPos, packageReference.Private.ToString());
                    if (projectReferenceProperties.ProjectGuid) RenderColumnTag(ref txt, tabPos, packageReference.ProjectGuid);
                    if (projectReferenceProperties.LicenseFiles) RenderColumnTag(ref txt, tabPos, await RenderLicenseLinks(packageReference, outputPath, createFile));
                    if (projectReferenceProperties.ParentProjectName) RenderColumnTag(ref txt, tabPos, packageReference.ParentProjectName);
                    if (projectReferenceProperties.ParentProjectPath) RenderColumnTag(ref txt, tabPos, packageReference.ParentProjectPath);
                }

                tabPos--;
                RenderLine(ref txt, tabPos--, "</tr>");
                i++;
            }

            return txt;
        }

        private string RenderTableHeaderRow(int tabPos, RenderProperties renderProperties)
        {
            var txt = string.Empty;
            RenderLine(ref txt, ++tabPos, "<tr style=\"background-color:black;color:white;font-size:1em;text-align:left;\">");
            tabPos++;

            if (renderProperties.Count) RenderColumnHeaderTag(ref txt, tabPos, "#");

            var nugetProperties = renderProperties.NugetProperties;
            var packageProperties = renderProperties.PackageProperties;
            var projectReferenceProperties = renderProperties.ProjectReferenceProperties;

            if (nugetProperties != null)
            {
                if (nugetProperties.Id) RenderColumnHeaderTag(ref txt, tabPos, "Library");
                if (nugetProperties.Version) RenderColumnHeaderTag(ref txt, tabPos, "Version");
                if (nugetProperties.Description) RenderColumnHeaderTag(ref txt, tabPos, "Description");
                if (nugetProperties.LicenseUrl) RenderColumnHeaderTag(ref txt, tabPos, nugetProperties.LicenseType ? "License url" : "License");
                if (nugetProperties.LicenseType) RenderColumnHeaderTag(ref txt, tabPos, nugetProperties.LicenseUrl ? "License type" : "License");
                if (nugetProperties.Authors) RenderColumnHeaderTag(ref txt, tabPos, "Authors");
                if (nugetProperties.Owners) RenderColumnHeaderTag(ref txt, tabPos, "Owners");
                if (nugetProperties.ProjectUrl) RenderColumnHeaderTag(ref txt, tabPos, "Project Url");
            }

            if (packageProperties != null)
            {
                if (packageProperties.Id) RenderColumnHeaderTag(ref txt, tabPos, "Package name");
                if (packageProperties.Version) RenderColumnHeaderTag(ref txt, tabPos, "Package version");
                if (packageProperties.TargetFramework) RenderColumnHeaderTag(ref txt, tabPos, "Package target framework");
            }

            if (projectReferenceProperties != null)
            {
                if (projectReferenceProperties.Name) RenderColumnHeaderTag(ref txt, tabPos, "Reference name");
                if (projectReferenceProperties.Version) RenderColumnHeaderTag(ref txt, tabPos, "Reference version");
                if (projectReferenceProperties.Culture) RenderColumnHeaderTag(ref txt, tabPos, "Reference culture");
                if (projectReferenceProperties.PublicKeyToken) RenderColumnHeaderTag(ref txt, tabPos, "Reference public key token");
                if (projectReferenceProperties.ProcessorArchitecture) RenderColumnHeaderTag(ref txt, tabPos, "Reference processor architecture");
                if (projectReferenceProperties.Location) RenderColumnHeaderTag(ref txt, tabPos, "Reference location");
                if (projectReferenceProperties.Private) RenderColumnHeaderTag(ref txt, tabPos, "Reference is private");
                if (projectReferenceProperties.ProjectGuid) RenderColumnHeaderTag(ref txt, tabPos, "Reference project guid");
                if (projectReferenceProperties.LicenseFiles) RenderColumnHeaderTag(ref txt, tabPos, "Reference license files");
                if (projectReferenceProperties.ParentProjectName) RenderColumnHeaderTag(ref txt, tabPos, "Reference parent project name");
                if (projectReferenceProperties.ParentProjectPath) RenderColumnHeaderTag(ref txt, tabPos, "Reference parent project path");
            }

            tabPos--;
            RenderLine(ref txt, tabPos--, "</tr>");
            return txt;
        }

        private async Task<string> RenderLicenseLinks(ProjectReference projectReference, string basePath, bool createFile)
        {
            var retVal = string.Empty;
            var directory = string.Empty;
            if (projectReference.LicenseFiles?.Any() == true)
            {
                if (createFile)
                {
                    directory = Path.Combine(basePath, "licenses");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                }

                foreach (var license in projectReference.LicenseFiles)
                {
                    retVal += !string.IsNullOrWhiteSpace(retVal) ? "<br>" : string.Empty;

                    if (createFile)
                    {
                        var fileName = $"{Guid.NewGuid()}_{license.FileName}";
                        var fullPath = Path.Combine(directory, fileName);
                        await _fileUtilities.CreateFile(fullPath, license.Content);
                        retVal += $"<a href=\"licenses/{fileName}\" target=\"blank\">{license.FileName}</a>";
                    }
                }
            }

            return retVal;
        }

        private void RenderLine(ref string document, int numTabs, string txt)
        {
            document += $"{Tab(numTabs)}{txt}{Environment.NewLine}";
        }

        private string Tab(int num)
        {
            return new string('\t', num);
        }

        private void RenderColumnTag(ref string document, int numTabs, string text)
        {
            RenderLine(ref document, numTabs, $"<td>{text}</td>");
        }

        private void RenderColumnHeaderTag(ref string document, int numTabs, string text)
        {
            RenderLine(ref document, numTabs, $"<th>{text}</th>");
        }

    }
}
