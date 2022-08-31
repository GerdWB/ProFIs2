namespace ProFiS2.WordAddIn.Helper
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///     This class provides some static members which allow for the creation and deletion
    ///     of temporary files and folders.
    /// </summary>
    internal static class TempFileCollection
    {
        /// <summary>
        ///     Gets the path where new temporary files will be created.
        /// </summary>
        public static string TempPath { get; private set; }

        private static TempFiles _files;

        #region Constructors

        static TempFileCollection()
        {
            TempPath = Path.Combine(Path.GetTempPath(), "TempFileCollection");
            SetTempSubFolder(TempPath, true, true);
        }

        #endregion

        /// <summary>
        ///     Creates a name for a new temporary file, including the current path.
        /// </summary>
        /// <returns>the full path of the new temporary file</returns>
        public static string CreateFilename()
        {
            if (_files == null)
            {
                _files = new TempFiles();
            }

            return _files.CreateFilename(TempPath);
        }


        /// <summary>
        ///     Creates a name for a new temporary file, including the current path.
        /// </summary>
        /// <returns>the full path of the new temporary file</returns>
        public static string CreateFilename(Guid name, string extension = ".tmp")
        {
            if (_files == null)
            {
                _files = new TempFiles();
            }

            return _files.CreateFilename(TempPath, name, extension);
        }

        /// <summary>
        ///     Deletes all temporary files which have been created by the current process.
        /// </summary>
        public static void DeleteFiles()
        {
            _files?.DeleteFiles();
        }

        /// <summary>
        ///     Sets up a folder under the system's temp folder where new temporary files
        ///     will be created afterwards.
        /// </summary>
        /// <param name="name">the name of the sub-folder</param>
        /// <param name="create">determines whether or not to create the sub-folder</param>
        /// <param name="deleteFiles">
        ///     determines whether or not to delete files that already exist in the sub-folder
        /// </param>
        public static void SetTempSubFolder(string name, bool create, bool deleteFiles)
        {
            SetTempPath(Path.Combine(Path.GetTempPath(), name), create, deleteFiles);
        }

        private static void DeleteFile(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    var attribs = File.GetAttributes(filename);
                    if ((attribs & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        attribs -= FileAttributes.ReadOnly;
                        File.SetAttributes(filename, attribs);
                    }

                    File.Delete(filename);
                }
            }
            catch
            {
            }
        }

        private static void SetTempPath(string path, bool create, bool deleteFiles)
        {
            if (create && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (deleteFiles)
            {
                var folder = new DirectoryInfo(path);

                foreach (var subfolder in folder.GetDirectories())
                {
                    try
                    {
                        subfolder.Delete(true);
                    }
                    catch
                    {
                    }
                }

                foreach (var fileInfo in folder.GetFiles())
                {
                    DeleteFile(fileInfo.FullName);
                }
            }

            TempPath = path;
        }

        #region Nested type: TempFiles

        private class TempFiles : IDisposable
        {
            private readonly List<string> _files = new();

            #region IDisposable Members

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion

            public string CreateFilename(string tempPath)
            {
                if (_disposed)
                {
                    throw new InvalidOperationException("The object has already been disposed");
                }

                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                var filename = Path.Combine(tempPath, Guid.NewGuid() + ".tmp");
                if (!_files.Contains(filename))
                {
                    _files.Add(filename);
                }

                return filename;
            }

            public string CreateFilename(string tempPath, Guid name, string extension = ".tmp")
            {
                if (_disposed)
                {
                    throw new InvalidOperationException("The object has already been disposed");
                }

                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                var filename = Path.Combine(tempPath, name + extension);
                if (!_files.Contains(filename))
                {
                    _files.Add(filename);
                }

                return filename;
            }

            public void DeleteFiles()
            {
                foreach (var filename in _files)
                {
                    DeleteFile(filename);
                }
            }

            #region IDisposable implementation

            private bool _disposed;

            private void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        // free managed resources
                    }

                    // free unmanaged resources
                    DeleteFiles();
                    _disposed = true;
                }
            }

            ~TempFiles()
            {
                Dispose(false);
            }

            #endregion
        }

        #endregion
    }
}