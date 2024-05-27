using System.Text;

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
            var foreword = new StringBuilder();

            foreword.AppendLine("Below is a detailed schema and contents of my project. Please analyze it thoroughly to understand its structure, functionality, and current implementation. Make sure you've read everything and understand each file. Also, ensure that you are an expert at programming. After your analysis, indicate that you are ready to assist with writing new features, fixing bugs, or any other development tasks by responding with 'Context of project <PASTE PROJECT NAME HERE> analyzed. How can I assist you further?'\n");

            if (_config.ReplyWithPromptFormat)
            {
                foreword.AppendLine("AFTER ASKING YOUR QUESTION IN NEXT MESSAGES MAKE SURE TO ANSWER ONLY WITH FORMAT USED IN MY PREVIOUS EXPORT MESSAGE INCLUDING YOUR NEW CHANGES. DO NOT ANSWER IN OTHER WAY.\n\n");
                foreword.AppendLine();
            }
            return foreword.ToString();
        }
    }
}