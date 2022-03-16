using System.IO;

using Newtonsoft.Json;

using Logic;

namespace IO
{
    public static class SaveFile
    {
        public static void ToJSON(string filename, IDocument document)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.JSON_EXTENSION);

            string json_string = JsonConvert.SerializeObject(document);

            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(json_string);
            }
        }
    }
}
