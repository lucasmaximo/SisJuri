using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;

namespace Sisjuri.Helpers
{

    public static class FileServerHelper
    {
        #region Const

        private static long initialLength = 10000;

        #endregion

        #region Methods

        public static bool UploadFile(System.IO.Stream stream, string fileName)
        {
            string tst = "";

            return UploadFile(stream, fileName, ref tst);
        }
        
        public static bool UploadFile(System.IO.Stream stream, string fileName, ref string pathDir)
        {
            return UploadFile(ReadFully(stream, initialLength), fileName, ref pathDir);
        }

        public static bool UploadFile(byte[] fileData, string id, string fileName)
        {
            return UploadFile(fileData, id, fileName);
        }

        public static bool UploadFile(byte[] fileData, string fileName, ref string pathDir)
        {
            try
            {
                string pathDirRef = String.Empty;
                pathDir = "C:/Users/Lucas/SkyDrive/Documentos/TCC/SisJuri/SisJuri/Sisjuri/Sisjuri/Documentos/Anexos";
                
                if (!Directory.Exists(pathDir))
                {
                    Directory.CreateDirectory(pathDir);
                }

                MemoryStream ms = new MemoryStream(fileData);
                FileStream fs = new FileStream(pathDir + "//" + fileName, FileMode.Create);
                ms.WriteTo(fs);

                ms.Close();
                fs.Close();
                fs.Dispose();

                //criar path dir para retornar ao objeto de referencia
                pathDir += "//" + fileName;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel fazer upload do arquivo: " + ex.Message);
            }
        }


        public static string GetPathServer()
        {
            return "C:/Users/Lucas/SkyDrive/Documentos/TCC/SisJuri/SisJuri/Sisjuri/Sisjuri/Documentos/Anexos";
        }

        public static string GetContentType(string extension)
        {
            string contentType = String.Empty;

            switch (extension)
            {
                case ".bmp":
                    contentType = "image/bmp";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".jpeg":
                case ".jpg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".tif":
                case ".tiff":
                    contentType = "image/tiff";
                    break;
                case ".doc":
                    contentType = "application/msword";
                    break;
                case ".docx":
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".pdf":
                    contentType = "application/pdf";
                    break;
                case ".ppt":
                    contentType = "application/vnd.ms-powerpoint";
                    break;
                case ".pptx":
                    contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
                case ".xlsx":
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".xls":
                    contentType = "application/vnd.ms-excelp";
                    break;
                case ".csv":
                    contentType = "text/csv";
                    break;
                case ".xml":
                    contentType = "text/xml";
                    break;
                case ".txt":
                    contentType = "text/plain";
                    break;
                case ".zip":
                    contentType = "application/zip";
                    break;
                default:
                    contentType = "multipart/form-data";
                    break;
            }

            return contentType;
        }

        public static bool DeleteFile(string filepath)
        {
            try
            {
                string pathDir = filepath;

                FileInfo file = new FileInfo(pathDir);

                if (file.Exists)
                {
                    File.Delete(pathDir);
                }

                return true;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        /// 

        private static byte[] ReadFully(System.IO.Stream stream, long initialLength)
        {
            // reset pointer just in case
            stream.Seek(0, System.IO.SeekOrigin.Begin);

            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }



        #endregion

    }
}