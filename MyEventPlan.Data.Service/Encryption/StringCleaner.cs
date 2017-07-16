using System;

namespace MyEventPlan.Data.Service.Encryption
{
    public class StringCleaner
    {
            public string GetUntilOrEmpty(string text, string stopAt = "-")
            {
                if (!String.IsNullOrWhiteSpace(text))
                {
                    int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                    if (charLocation > 0)
                    {
                        return text.Substring(0, charLocation);
                    }
                }

                return String.Empty;
            }
        }
    
}
