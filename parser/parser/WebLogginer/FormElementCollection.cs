using System.Collections.Generic;
using System.Text;

namespace parser.WebLogginer
{
    // Являє собою колекцію пари ключ значення з форми запиту
    public class FormElementCollection : Dictionary<string, string>
    {
        // Парсить HtmlDocument аби отримати всі input поля. 
        public FormElementCollection(HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            var inputs = htmlDoc.DocumentNode.Descendants("input");
            foreach (var element in inputs)
            {
                string name = element.GetAttributeValue("name", "undefined");
                string value = element.GetAttributeValue("value", "");

                if (!this.ContainsKey(name))
                {
                    if (!name.Equals("undefined"))
                    {
                        Add(name, value);
                    }
                }
            }
        }

        public string AssemblePostPayload()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var element in this)
            {
                string value = System.Uri.EscapeDataString(element.Value);
                sb.Append("&" + element.Key + "=" + value);
            }
            return sb.ToString().Substring(1);
        }
    }
}
