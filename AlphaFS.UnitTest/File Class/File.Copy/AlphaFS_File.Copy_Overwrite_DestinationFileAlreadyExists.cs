using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_Copy_Overwrite_DestinationFileAlreadyExists_LocalAndNetwork_Success()
      {
         AlphaFS_File_Copy_Overwrite_DestinationFileAlreadyExists(false);
         AlphaFS_File_Copy_Overwrite_DestinationFileAlreadyExists(true);
      }

      
      private void AlphaFS_File_Copy_Overwrite_DestinationFileAlreadyExists(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var srcFile = UnitTestConstants.CreateFile(tempRoot.Directory.FullName);
            var dstFile = tempRoot.RandomFileFullPath;

            Console.WriteLine("Src File Path: [{0}]", srcFile);
            Console.WriteLine("Dst File Path: [{0}]", dstFile);


            System.IO.File.Copy(srcFile.FullName, dstFile);


            var gotException = false;

            try
            {
               Alphaleonis.Win32.Filesystem.File.Copy(srcFile.FullName, dstFile);
            }
            catch (Exception ex)
            {
               var exType = ex.GetType();

               gotException = exType == typeof(Alphaleonis.Win32.Filesystem.AlreadyExistsException);

               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exType.Name, ex.Message);
            }


            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");


            Alphaleonis.Win32.Filesystem.File.Copy(srcFile.FullName, dstFile, true);


            Assert.IsTrue(System.IO.File.Exists(srcFile.FullName), "The file does not exists, but is expected to.");

            Assert.IsTrue(System.IO.File.Exists(dstFile), "The file does not exists, but is expected to.");
         }


         Console.WriteLine();
      }
   }
}
