//-----------------------------------------------------------------------
// <copyright file="EmbeddedResource.cs">
//     Copyright (c) 2016 Adam Craven. All rights reserved.
// </copyright>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

namespace ChannelAdam.Core.Reflection
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Xml.Serialization;

    public static class EmbeddedResource
    {
        /// <summary>
        /// Gets the embedded resource from the given assembly as a stream.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        /// <remarks>Ensure that you dispose of the stream appropriately!</remarks>
        public static Stream GetAsStream(Assembly assembly, string resourceName)
        {
            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null) throw new System.IO.FileNotFoundException($"Cannot find the embedded resource '{resourceName}' in assembly '{assembly.FullName}'.");

            return stream;
        }

        /// <summary>
        /// Gets the string contents of the embedded resource.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        public static string GetAsString(Assembly assembly, string resourceName)
        {
            using (var stream = GetAsStream(assembly, resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Deserialises the given type from the embedded XML resource.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly">The assembly that is storing the embedded resource.</param>
        /// <param name="xmlResourceName">Name of the embedded XML resource.</param>
        /// <returns></returns>
        public static T DeserialiseFromXmlResource<T>(Assembly assembly, string xmlResourceName)
        {
            using (var stream = GetAsStream(assembly, xmlResourceName))
            {
                var xmlSerialiser = new XmlSerializer(typeof(T));
                xmlSerialiser.UnknownAttribute += xmlSerialiser_UnknownAttribute;
                xmlSerialiser.UnknownElement += xmlSerialiser_UnknownElement;
                xmlSerialiser.UnknownNode += xmlSerialiser_UnknownNode;
                xmlSerialiser.UnreferencedObject += xmlSerialiser_UnreferencedObject;

                return (T)xmlSerialiser.Deserialize(stream);
            }
        }


        /// <summary>
        /// Deserialises the given type from the embedded XML resource.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly">The assembly that is storing the embedded resource.</param>
        /// <param name="xmlResourceName">Name of the XML embedded resource.</param>
        /// <param name="xmlAttributeSerialisationOverrides">XML attribute overrides for deserialisation.</param>
        /// <returns></returns>
        public static T DeserialiseFromXmlResource<T>(Assembly assembly, string xmlResourceName, XmlAttributeOverrides xmlAttributeSerialisationOverrides)
        {
            using (var stream = GetAsStream(assembly, xmlResourceName))
            {
                var xmlSerialiser = new XmlSerializer(typeof(T), xmlAttributeSerialisationOverrides);
                xmlSerialiser.UnknownAttribute += xmlSerialiser_UnknownAttribute;
                xmlSerialiser.UnknownElement += xmlSerialiser_UnknownElement;
                xmlSerialiser.UnknownNode += xmlSerialiser_UnknownNode;
                xmlSerialiser.UnreferencedObject += xmlSerialiser_UnreferencedObject;

                return (T)xmlSerialiser.Deserialize(stream);
            }
        }

        private static void xmlSerialiser_UnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {
            Trace.WriteLine($"XmlSerialiser Error - Unreferenced Object - Id:{e.UnreferencedId}");
        }

        private static void xmlSerialiser_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Trace.WriteLine($"XmlSerialiser Error - Unknown Node - LineNumber:{e.LineNumber}, LinePosition:{e.LinePosition}, Namespace:'{e.NamespaceURI}', Name:'{e.Name}', Text:'{e.Text}'");
        }

        private static void xmlSerialiser_UnknownElement(object sender, XmlElementEventArgs e)
        {
            Trace.WriteLine($"XmlSerialiser Error - Unknown Element - LineNumber:{e.LineNumber}, LinePosition:{e.LinePosition}, Namespace:'{e.Element.NamespaceURI}', Name:'{e.Element.Name}', Expected:'{e.ExpectedElements}'");
        }

        private static void xmlSerialiser_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            Trace.WriteLine($"XmlSerialiser Error - Unknown Attribute - LineNumber:{e.LineNumber}, LinePosition:{e.LinePosition}, Namespace:'{e.Attr.NamespaceURI}', Name:'{e.Attr.Name}', Expected:'{e.ExpectedAttributes}'");
        }
    }
}
