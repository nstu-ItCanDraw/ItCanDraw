using System.IO;

using Newtonsoft.Json;

namespace IO
{
    public static class SaveFile
    {
        public static void ToJSON(string filename, object document)
        {
            string json_string = JsonConvert.SerializeObject(document);

            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(json_string);
            }
        }
    }
}
