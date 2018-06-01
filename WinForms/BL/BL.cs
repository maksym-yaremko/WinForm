using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WindowsForms.Models;

namespace WindowsForms.BL
{
    public class BL
    {
        public static void SerializeList(List<Rectangle> rectangles, string path)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Rectangle>));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, rectangles);
            }
        }

        public static List<Rectangle> DeserializeList(string path)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Rectangle>));
            List<Rectangle> rectangles = null;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                rectangles = (List<Rectangle>)formatter.Deserialize(fs);
            }

            if (rectangles == null)
            {
                throw new ApplicationException(string.Format("cannot deserialize file {0}", path));
            }
            
            return rectangles;
        }
    }
}
