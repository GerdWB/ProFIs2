namespace ProFiS2.WordAddIn.Helper
{
    using System;
    using System.IO;
    using System.Threading;
    using Microsoft.Office.Interop.Word;
    using Application = System.Windows.Forms.Application;

    public static class WordHelper
    {
        internal static GetBinaryResult GetDocxBinary(Document document)
        {
            if (document == null)
            {
                return new GetBinaryResult(null, false, null);
            }

            var fullName = document.FullName;
            byte[] bytes = null;
            try
            {
                if (string.IsNullOrWhiteSpace(document.Path))
                {
                    // document is not saved to disk yet
                    fullName = string.IsNullOrWhiteSpace(document.Name)
                        ? TempFileCollection.CreateFilename()
                        : Path.Combine(TempFileCollection.TempPath, document.Name);

                    document.SaveAs(fullName, WdSaveFormat.wdFormatDocumentDefault, AddToRecentFiles: false);
                }
                else
                {
                    document.Save();
                }

                document.Close();
                Application.DoEvents();
                Thread.Sleep(125);
                Application.DoEvents();
                bytes = FileHelper.ReadBinaryFile(fullName);

                return new GetBinaryResult(null, true, bytes);
            }
            catch (Exception e)
            {
                return new GetBinaryResult(fullName, false, bytes, e);
            }
        }

        internal record GetBinaryResult(string FullName, bool Success, byte[] Docx, Exception Exception = null);
    }
}