using System;
using System.Dynamic;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Infrastructure.Dynamic;
using System.ComponentModel;

namespace Gem.Infrastructure.Tests
{
    [TestClass]
    public class RuntimeResolvingTests
    {

        #region Runtime Mapper Dummy Objects

        /// <summary>
        /// This is used to test the runtime mapper
        /// </summary>
        internal class Source
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string SourceSpecificProperty { get; set; }
        }

        /// <summary>
        /// This is used to test the runtime mapper
        /// </summary>
        internal class Destination
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string DestinationSpecificProperty { get; set; }
        }

        #endregion

        [TestMethod]
        [TestCategory("Expressions")]
        public void RuntimeMapperSuccessfulyResolvesObjects()
        {
            var sourceObject = new Source
            {
                Id = 1,
                Name = "source",
                SourceSpecificProperty = "SourceSpecificProperty"
            };

            var destinationObject = DynamicMapper.CreateMapper<Source, Destination>()
                                                 .Invoke(sourceObject);

            Assert.AreEqual(destinationObject.Id, sourceObject.Id);
            Assert.AreEqual(destinationObject.Name, sourceObject.Name);
            Assert.AreNotEqual(destinationObject.DestinationSpecificProperty, sourceObject.SourceSpecificProperty);
        }

        [TestMethod]
        public void AccessXMLByIndexTest()
        {
            var xml = XDocument.Parse(
            @"<School>
                <Classes>
                    <Class>
                        <Lecture>Expressions</Lecture>
                    </Class>
                    <Class>
                        <Lecture>DynamicProgramming</Lecture>
                    </Class>
                </Classes>
                <Professor>
                     <Name>George</Name>
                     <Lesson>Math</Lesson>
                </Professor>
             </School>");

            dynamic dynamicXML = new DynamicXElement(xml.Element("School"));

            string res = dynamicXML["Classes", 0]["Class", 1]["Lecture", 0].Value;

            Assert.AreEqual("DynamicProgramming", res);

        }

        #region PropertyNotifyExtensionsRaiseEvent Helper Objects

        public class PropertyChangedHelper : INotifyPropertyChanged, INotifyPropertyChanging
        {
            string name = string.Empty;
            public string Name
            {
                get
                {
                    return name;
                }
                set
                {
                    PropertyChanged.SetNotifyProperty(PropertyChanging, value, () => this.name,
                                  new Action<string>(newValue => this.name = newValue));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public event PropertyChangingEventHandler PropertyChanging;
        }

        #endregion

        [TestMethod]
        [TestCategory("Expressions")]
        public void PropertyNotifyExtensionsRaiseEvent()
        {
            bool changedEventRaised = false;
            bool changingEventRaised = false;
            var myobj = new PropertyChangedHelper();

            myobj.PropertyChanged += (sender, args) => changedEventRaised = true;
            myobj.PropertyChanging += (sender, args) => changingEventRaised = true;
            myobj.Name = "im trying to raise events";

            Assert.IsTrue(changedEventRaised);
            Assert.IsTrue(changingEventRaised);
        }
    }
}
