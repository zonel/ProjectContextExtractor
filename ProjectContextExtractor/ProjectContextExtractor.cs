using System.Text;

namespace ProjectContextExtractor;

internal sealed class ProjectContextExtractor
{
    private readonly Configuration _config;

    public ProjectContextExtractor(Configuration config)
    {
        _config = config;
    }

    public void Execute()
    {
        var projectContext = new StringBuilder();

        TraverseDirectory(_config.ProjectPath, projectContext, _config.ProjectPath, _config.IgnoreList, _config.AllowedExtensions);

        File.WriteAllText(_config.OutputPath, projectContext.ToString());

        Console.WriteLine("Project context has been exported successfully.");
    }

    private void TraverseDirectory(string currentPath, StringBuilder projectContext, string rootPath, List<string> ignoreList, List<string> allowedExtensions)
    {
        var directories = Directory.GetDirectories(currentPath);
        var files = Directory.GetFiles(currentPath);

        var relativePath = GetRelativePath(currentPath, rootPath);
        projectContext.AppendLine($"Directory: {relativePath}");

        foreach (var file in files)
        {
            if (ShouldIgnore(file, ignoreList) || !IsAcceptedExtension(file, allowedExtensions)) continue;

            projectContext.AppendLine($"File: {GetRelativePath(file, rootPath)}");
            projectContext.AppendLine("Content:");
            projectContext.AppendLine(File.ReadAllText(file));
            projectContext.AppendLine(new string('-', 80));
        }

        foreach (var directory in directories)
        {
            if (ShouldIgnore(directory, ignoreList)) continue;

            TraverseDirectory(directory, projectContext, rootPath, ignoreList, allowedExtensions);
        }
    }

    private static string GetRelativePath(string fullPath, string rootPath)
    {
        return fullPath.Substring(rootPath.Length).TrimStart(Path.DirectorySeparatorChar);
    }

    private static bool ShouldIgnore(string path, List<string> ignoreList)
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

    private static bool IsAcceptedExtension(string filePath, List<string> allowedExtensions)
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