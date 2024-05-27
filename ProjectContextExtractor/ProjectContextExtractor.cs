using System.Text;

namespace ProjectContextExtractor
{
    internal sealed class ProjectContextExtractor
    {
        private readonly Configuration _config;
        private readonly ProjectFilesSchema _schemaGenerator;
        private readonly ForewordPromptGenerator _forewordGenerator;

        public ProjectContextExtractor(Configuration config, ProjectFilesSchema schemaGenerator, ForewordPromptGenerator forewordGenerator)
        {
            _config = config;
            _schemaGenerator = schemaGenerator;
            _forewordGenerator = forewordGenerator;
        }

        public void Execute()
        {
            var projectContext = new StringBuilder();

            projectContext.AppendLine(_forewordGenerator.GenerateForeword());

            projectContext.AppendLine("Project files schema:");
            projectContext.AppendLine(_schemaGenerator.GenerateSchema(_config.ProjectPath, _config.ProjectPath));

            TraverseDirectory(_config.ProjectPath, projectContext, _config.ProjectPath, _config.IgnoreList, _config.AllowedExtensions);

            var dateTime = DateTime.Now.ToString("yyyyMMddHHmm");
            var outputFileName = $"-export-{dateTime}.txt";
            var outputPath = Path.Combine(_config.OutputDirectory, outputFileName);

            File.WriteAllText(outputPath, projectContext.ToString());

            Console.WriteLine($"Project context has been exported successfully to {outputPath}");
        }

        private void TraverseDirectory(string currentPath, StringBuilder projectContext, string rootPath, List<string> ignoreList, List<string> allowedExtensions)
        {
            var directories = Directory.GetDirectories(currentPath);
            var files = Directory.GetFiles(currentPath);

            foreach (var file in files)
            {
                if (ShouldIgnore(file, ignoreList) || !IsAcceptedExtension(file, allowedExtensions)) continue;

                projectContext.AppendLine();
                projectContext.AppendLine($"File: {GetRelativePath(file, rootPath)}");
                projectContext.AppendLine(new string('-', 80));
                projectContext.AppendLine(File.ReadAllText(file));
                projectContext.AppendLine(new string('-', 80));
            }

            foreach (var directory in directories)
            {
                if (ShouldIgnore(directory, ignoreList)) continue;

                TraverseDirectory(directory, projectContext, rootPath, ignoreList, allowedExtensions);
            }
        }

        private string GetRelativePath(string fullPath, string rootPath)
        {
            return fullPath.Substring(rootPath.Length).TrimStart(Path.DirectorySeparatorChar);
        }

        private bool ShouldIgnore(string path, List<string> ignoreList)
        {
            foreach (var ignore in ignoreList)
            {
                if (path.Contains(ignore))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsAcceptedExtension(string filePath, List<string> allowedExtensions)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            foreach (var allowedExtension in allowedExtensions)
            {
                if (extension == allowedExtension)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
