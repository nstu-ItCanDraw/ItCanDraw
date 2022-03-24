using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IO.Test
{
    [TestClass()]
    public class SaveFileTest
    {
        [TestMethod()]
        public void testSaveToJSON()
        {
            string filename = @"..\..\..\Test1.json";
            Logic.IDocument document = Logic.DocumentFactory.CreateDocument("JSON", 100, 100);

            SaveFile.ToJSON(filename, document);

        }
    }
}
