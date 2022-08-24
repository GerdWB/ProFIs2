namespace ProFiS2.WordAddIn
{
    using System.Xml.Linq;
    using Microsoft.Office.Interop.Word;
    using Model;

    internal static partial class Globals
    {
        public static ProfiS2WordData GetProfiS2Data(Document document)
        {
            try
            {
                var parts = document.CustomXMLParts.SelectByNamespace(ProfiS2WordData.Namespace.NamespaceName);
                var customXml = XElement.Parse(parts[1].XML);
                var profiS2Data = new ProfiS2WordData(customXml);
                return profiS2Data;
            }
            catch
            {
                return null;
            }
        }

        public static bool IsProFis2Document(Document document)
        {
            if (document == null)
            {
                return false;
            }

            try
            {
                var profis2Data = GetProfiS2Data(document);
                return profis2Data != null;
            }
            catch
            {
                return false;
            }
        }
    }
}