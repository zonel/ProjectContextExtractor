namespace ProjectContextExtractor;

internal sealed record Configuration
{
    public string ProjectName { get; set; }
    public string ProjectPath { get; set; }
    public string OutputDirectory { get; set; }
    public List<string> IgnoreList { get; set; }
    public List<string> AllowedExtensions { get; set; }
}