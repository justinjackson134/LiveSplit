using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using static LiveSplit.UI.SettingsHelper;

namespace LiveSplit.Model.RunSavers
{
    public class XMLMultiSplitSaver
    {
        public void Save(LiveSplitState state, Stream stream)
        {
            IRun run = state.Run;

            var document = new XmlDocument();

            XmlNode docNode = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            document.AppendChild(docNode);

            var parent = document.CreateElement("Run");
            parent.Attributes.Append(ToAttribute(document, "version", "1.7.0"));
            document.AppendChild(parent);

            CreateSetting(document, parent, "GameIcon", run.GameIcon);
            CreateSetting(document, parent, "GameName", run.GameName);
            CreateSetting(document, parent, "CategoryName", run.CategoryName);

            var metadata = document.CreateElement("Metadata"); var runElement = document.CreateElement("Run");
            runElement.Attributes.Append(ToAttribute(document, "id", run.Metadata.RunID));
            metadata.AppendChild(runElement);

            var platform = ToElement(document, "Platform", run.Metadata.PlatformName);
            platform.Attributes.Append(ToAttribute(document, "usesEmulator", run.Metadata.UsesEmulator));
            metadata.AppendChild(platform);

            CreateSetting(document, metadata, "Region", run.Metadata.RegionName);

            var variables = document.CreateElement("Variables");
            foreach (var variable in run.Metadata.VariableValueNames)
            {
                var variableElement = ToElement(document, "Variable", variable.Value);
                variableElement.Attributes.Append(ToAttribute(document, "name", variable.Key));
                variables.AppendChild(variableElement);
            }
            metadata.AppendChild(variables);
            parent.AppendChild(metadata);

            CreateSetting(document, parent, "Offset", run.Offset);

            var segmentElement = document.CreateElement("Segments");
            parent.AppendChild(segmentElement);

            //foreach (var segment in run)
            //{
            //    var splitElement = document.CreateElement("Segment");
            //    segmentElement.AppendChild(splitElement);

            //    CreateSetting(document, splitElement, "Name", segment.Name);
            //    CreateSetting(document, splitElement, "Icon", segment.Icon);
            //    CreateSetting(document, splitElement, "SplitTime", segment.SplitTime);
            //}
                        
            for (int i = 0; i < run.Count; i++)
            {
                if (run[i].SplitTime.ToString() != " | ")
                {
                    var splitElement = document.CreateElement("Segment");
                    segmentElement.AppendChild(splitElement);

                    CreateSetting(document, splitElement, "Name", run[i].Name);
                    CreateSetting(document, splitElement, "Icon", run[i].Icon);
                    CreateSetting(document, splitElement, "SplitTime", run[i].SplitTime);                    
                }
            }

            var lastSplitElement = document.CreateElement("Segment");
            segmentElement.AppendChild(lastSplitElement);

            CreateSetting(document, lastSplitElement, "Name", state.CurrentSplit.Name);
            CreateSetting(document, lastSplitElement, "Icon", state.CurrentSplit.Icon);
            CreateSetting(document, lastSplitElement, "SplitTime", state.CurrentTime);

            var autoSplitterSettings = document.CreateElement("AutoSplitterSettings");
            if (run.IsAutoSplitterActive())
                autoSplitterSettings.InnerXml = run.AutoSplitter.Component.GetSettings(document).InnerXml;
            parent.AppendChild(autoSplitterSettings);

            document.Save(stream);
        }
    }
}
