using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace ElsevierMaterials.Exporter.Models
{

    public enum XMLEncoding { 
        None = 0,
        UTF8 = 1,
        UTF16 = 2
    
    }

    public static class XDocumentExtensions {
        public static string ToStringWithDeclaration(this XDocument doc, XMLEncoding encoding = XMLEncoding.UTF8)
        {
            if (doc == null) {
                throw new ArgumentNullException("doc");
            }
            StringBuilder builder = new StringBuilder();
          
                using (TextWriter writer = new UtfStringWriter(builder, encoding))
                {
                    doc.Save(writer);
                }          
         

            return builder.ToString();
        }
    }




    public class UtfStringWriter : StringWriter {
        private StringBuilder builder;
        private XMLEncoding encoding;
        public UtfStringWriter(StringBuilder builder, XMLEncoding encoding):base(builder) {
            this.builder = builder;
            this.encoding = encoding;
        }
        public override Encoding Encoding { get {


            return encoding == XMLEncoding.UTF16? Encoding.Unicode : Encoding.UTF8;
        }
        }
    }




}