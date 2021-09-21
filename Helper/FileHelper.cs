using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Helpers
{
    public interface IFileHelper
    {

    }
   public class FileHelper:IFileHelper
    {
      public static string FileUpload(IFormFile File,IHostingEnvironment hostingEnvironment,string UploadFolder)
        {
          
            string UniqeFileName = null;
            if (File!=null)
            {
               var path= Path.Combine("wwwroot", UploadFolder);
                UniqeFileName = Guid.NewGuid().ToString() + "-" + File.FileName;
                string FilePath = Path.Combine(path, UniqeFileName);
                File.CopyTo(new FileStream(FilePath, FileMode.Create));
                return UniqeFileName;
            }
            return UniqeFileName;
        }

    }
}
