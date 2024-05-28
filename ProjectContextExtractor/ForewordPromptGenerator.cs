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

            foreword.AppendLine(_config.ForewordText);

            if (_config.ReplyWithPromptFormat)
            {
                foreword.AppendLine(_config.ReplyWithPromptFormatText);
                foreword.AppendLine();
            }
            return foreword.ToString();
        }
    }
}