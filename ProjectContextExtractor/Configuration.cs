 namespace ProjectContextExtractor;

 internal sealed record Configuration
 {
     public string ProjectPath { get; set; }
     public string OutputPath { get; set; }
     public List<string> IgnoreList { get; set; }
     public List<string> AllowedExtensions { get; set; }
 }