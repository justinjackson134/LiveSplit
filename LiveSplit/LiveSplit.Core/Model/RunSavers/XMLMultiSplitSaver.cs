using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using static LiveSplit.UI.SettingsHelper;

namespace LiveSplit.Model.RunSavers
{
    public class XMLMultiSplitSaver
    {
        public void Save(IRun run, Stream stream)
        {
            var document = new XmlDocument();

            XmlNode docNode = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            document.AppendChild(docNode);

            var parent = document.CreateElement("Run");
            parent.Attributes.Append(ToAttribute(document, "version", "1.7.0"));
            document.AppendChild(parent);

            var segmentElement = document.CreateElement("Segments");
            parent.AppendChild(segmentElement);

            foreach (var segment in run)
            {
                var splitElement = document.CreateElement("Segment");
                segmentElement.AppendChild(splitElement);

                CreateSetting(document, splitElement, "Name", segment.Name);
                CreateSetting(document, splitElement, "Icon", segment.Icon);
                CreateSetting(document, splitElement, "SplitTime", segment.SplitTime);
            }

            document.Save(stream);
        }
    }
}
