namespace ProFiS2.WordAddIn.Model
{
    using System;
    using System.Xml.Linq;

    internal class CrmSettings
    {
        public static readonly XName CatIdElementName = ProfiS2Data.Namespace + "CatId";

        public static readonly XName DocKeyElementName = ProfiS2Data.Namespace + "DocKey";
        public static readonly XName DocumentNameElementName = ProfiS2Data.Namespace + "DocumentName";
        public static XName ElementName = ProfiS2Data.Namespace + "CrmSettings";
        public static readonly XName InstIdElementName = ProfiS2Data.Namespace + "InstId";
        public static readonly XName RestUrlElementName = ProfiS2Data.Namespace + "RestUrl";
        public static readonly XName TypeIdElementName = ProfiS2Data.Namespace + "TypeId";


        private readonly XElement _settingsElement;

        public CrmSettings(XElement settingsElement) => _settingsElement =
            settingsElement ?? throw new ArgumentNullException(nameof(settingsElement));


        // TODO: Logging for missing Elements
        public string DocKey => _settingsElement.Element(DocKeyElementName)?.Value;
        public string RestUrl => _settingsElement.Element(RestUrlElementName)?.Value;
        public string InstId => _settingsElement.Element(InstIdElementName)?.Value;
        public string TypeId => _settingsElement.Element(TypeIdElementName)?.Value;
        public string CatId => _settingsElement.Element(CatIdElementName)?.Value;
        public string DocumentName => _settingsElement.Element(DocumentNameElementName)?.Value;
    }
}