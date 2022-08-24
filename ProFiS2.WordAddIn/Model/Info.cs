namespace ProFiS2.WordAddIn.Model
{
    using System;
    using System.Xml.Linq;

    internal class Info
    {
        public static readonly XName DateElementName = ProfiS2WordData.Namespace + "Date";
        public static XName ElementName = ProfiS2WordData.Namespace + "Info";
        private readonly XElement _infoElement;

        public Info(XElement infoElement) =>
            _infoElement = infoElement ?? throw new ArgumentNullException(nameof(infoElement));

        // TODO: Logging for missing Elements
        public string Date => _infoElement.Element(DateElementName)?.Value;
    }
}