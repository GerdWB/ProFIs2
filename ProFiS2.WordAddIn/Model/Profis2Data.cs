namespace ProFiS2.WordAddIn.Model
{
    using System;
    using System.Xml.Linq;

    internal class ProfiS2Data
    {
        public static XNamespace Namespace = "http://schemas.profis2/sapupload";
        public static XName ElementName = Namespace + "profis2";


        public ProfiS2Data(XElement profiS2DataElement)
        {
            if (profiS2DataElement == null)
            {
                throw new ArgumentNullException(nameof(profiS2DataElement));
            }

            var Settings = new CrmSettings(profiS2DataElement.Element(CrmSettings.ElementName));
            var Infos = new Info(profiS2DataElement.Element(Info.ElementName));
        }

        public CrmSettings Settings { get; }
        public Info Infos { get; }
    }
}