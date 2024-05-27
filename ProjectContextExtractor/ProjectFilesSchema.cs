namespace ProjectContextExtractor
{
    internal sealed class ProjectFilesSchema
    {
        private readonly Configuration _config;

        public ProjectFilesSchema(Configuration config)
        {
            _config = config;
        }

        public string GenerateSchema(string currentPath, string rootPath, string indent = "")
        {
            var directories = Directory.GetDirectories(currentPath);
            var files = Directory.GetFiles(currentPath);

            var schema = new StringWriter();

            schema.WriteLine($"{indent}{Path.GetFileName(currentPath)}");

            foreach (var file in files)
            {
                if (!IsAcceptedExtension(file, _config.AllowedExtensions) || ShouldIgnore(file, _config.IgnoreList))
                {
                    continue;
                }
                schema.WriteLine($"{indent}├── {Path.GetFileName(file)}");
            }

            for (int i = 0; i < directories.Length; i++)
            {
                var directory = directories[i];
                if (ShouldIgnore(directory, _config.IgnoreList))
                {
                    continue;
                }
                bool isLastDirectory = (i == directories.Length - 1);
                string directoryIndent = indent + (isLastDirectory ? "└── " : "├── ");
                schema.Write(GenerateSchema(directory, rootPath, directoryIndent + "    "));
            }

            return schema.ToString();
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
