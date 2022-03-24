using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace IO.Test
{
    [TestClass()]
    public class FileValidatorTest
    {
        [ExpectedException(typeof(DirectoryNotFoundException))]
        [TestMethod()]
        public void parentDirectoryNotFound()
        {
            string filename = "FileValidatorrrrr.cs";
            FileValidator.CheckParentDirectory(filename);
        }

        [ExpectedException(typeof(FileNotFoundException))]
        [TestMethod()]
        public void fileNotExists()
        {
            string filename = "FileValidatorrrrrr.cs";
            FileValidator.CheckFileExists(filename);
        }

        [ExpectedException(typeof(BadFileExtensionError))]
        [TestMethod()]
        public void ExtentionNotFound()
        {
            string filename = "FileValidatorrrrrr";
            FileValidator.CheckExtension(filename, FileValidator.SVG_EXTENSION);
        }
    }
}
