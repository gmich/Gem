using System.Dynamic;
using System.Linq;
using System.Xml.Linq;

namespace Gem.Infrastructure.Dynamic
{
    /// <summary>
    /// Accesses xml members using indexers or objects
    /// </summary>
    /// <example>
    /// dynamic xml = new DynamicXElement(myXml);
    /// dynamic obj = xml["People",2];
    /// </example>
    /// <remarks>Only supports [string, int] indexers</remarks>
    public class DynamicXElement : DynamicObject
    {
        private readonly XElement xmlSource;

        public DynamicXElement(XElement source)
        {
            xmlSource = source;
        }

        #region DynamicObject Members

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = new DynamicXElement(null);

            if (binder.Name == "Value")
            {
                result = (xmlSource != null) ?
                xmlSource.Value : "";
                return true;
            }

            if (xmlSource != null)
            {
                result = new DynamicXElement(xmlSource.Element(XName.Get(binder.Name)));
            }

            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder,object[] indexes,
                                                               out object result)
        {
            result = null;
            if (indexes.Length != 2)
            {
                return false;
            }
            if (!(indexes[0] is string))
            {
                return false;
            }
            if (!(indexes[1] is int))
            {
                return false;
            }
            var allNodes = xmlSource.Elements(indexes[0].ToString());

            int index = (int)indexes[1];
            if (index < allNodes.Count())
            {
                result = new DynamicXElement(allNodes.ElementAt(index));
            }
            else
            {
                result = new DynamicXElement(null);
            }

            return true;
        }

        #endregion

        public override string ToString()
        {
            return (xmlSource != null) ?
            xmlSource.ToString() : string.Empty;
        }
    }
}
