namespace ProjectContextExtractor
{
    internal sealed record Configuration
    {
        public string ProjectPath { get; set; }
        public string OutputDirectory { get; set; }
        public List<string> IgnoreList { get; set; }
        public List<string> AllowedExtensions { get; set; }
        public bool ReplyWithPromptFormat { get; set; }
        public string ForewordText { get; set; }
        public string ReplyWithPromptFormatText { get; set; }
        public string OpenAiApiKey { get; set; }
        public string PineconeApiKey { get; set; }
    }
}