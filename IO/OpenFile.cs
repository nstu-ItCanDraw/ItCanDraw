using System.IO;

using Newtonsoft.Json;

using Logic;

namespace IO
{
    public static class OpenFile
    {
        public static IDocument FromJSON(string filename)
        {
            FileValidator.CheckFileExists(filename);
            FileValidator.CheckExtension(filename, FileValidator.JSON_EXTENSION);

            string json_string;

            using (StreamReader reader = new StreamReader(filename))
            {
                json_string = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<IDocument>(json_string);
        }
    }
}
