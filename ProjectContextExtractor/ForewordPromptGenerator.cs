using System.Text;

namespace ProjectContextExtractor
{
    internal sealed class ForewordPromptGenerator(Configuration config)
    {
        public string GenerateForeword()
        {
            var foreword = new StringBuilder();

            foreword.AppendLine(config.ForewordText);

            if (!config.ReplyWithPromptFormat) return foreword.ToString();
            
            foreword.AppendLine(config.ReplyWithPromptFormatText);
            foreword.AppendLine();
            
            return foreword.ToString();
        }
    }
}