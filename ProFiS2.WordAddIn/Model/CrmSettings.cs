namespace ProFiS2.WordAddIn.Model
{
    using System;
    using System.Xml.Linq;

    internal class CrmSettings
    {
        public static readonly XName CatIdElementName = ProfiS2WordData.Namespace + "CatId";

        public static readonly XName DocKeyElementName = ProfiS2WordData.Namespace + "DocKey";
        public static readonly XName DocumentNameElementName = ProfiS2WordData.Namespace + "DocumentName";
        public static XName ElementName = ProfiS2WordData.Namespace + "CrmSettings";
        public static readonly XName InstIdElementName = ProfiS2WordData.Namespace + "InstId";
        public static readonly XName RestUrlElementName = ProfiS2WordData.Namespace + "RestUrl";
        public static readonly XName TypeIdElementName = ProfiS2WordData.Namespace + "TypeId";


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