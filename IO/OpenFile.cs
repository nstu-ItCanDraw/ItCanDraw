using System.IO;

using Newtonsoft.Json;

namespace IO
{
    public static class OpenFile
    {
        public static object FromJSON(string filename)
        {
            string json_string;

            using (StreamReader reader = new StreamReader(filename))
            {
                json_string = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<object>(json_string);
        }
    }
}
