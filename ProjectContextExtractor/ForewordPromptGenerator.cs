namespace ProjectContextExtractor
{
    internal sealed class ForewordPromptGenerator
    {
        private readonly Configuration _config;

        public ForewordPromptGenerator(Configuration config)
        {
            _config = config;
        }

        public string GenerateForeword()
        {
            return $"Below is a detailed schema and contents of my project. Please analyze it thoroughly to understand its structure, functionality, and current implementation. Make sure you've read everything and understand each file. Also, ensure that you are an expert at programming. After your analysis, indicate that you are ready to assist with writing new features, fixing bugs, or any other development tasks by responding with 'Context of project {_config.ProjectName} analyzed. How can I assist you further?'\n\n";
        }
    }
}