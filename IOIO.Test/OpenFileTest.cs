using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace IO.Test
{
    [TestClass()]
    public class OpenFileTest
    {
        [ExpectedException(typeof(JsonSerializationException))]
        [TestMethod()]
        public void openTestJSON()
        {
            string filename = @"..\..\..\TestJSON.json";
            //Logic.IDocument expected = Logic.DocumentFactory.CreateDocument("JSON", 100, 100);
            OpenFile.FromJSON(filename);
            //Logic.IDocument actual = OpenFile.FromJSON(filename);
            //Assert.IsInstanceOfType(OpenFile.FromJSON(filename), typeof(Logic.IDocument));
        }
    }
}
