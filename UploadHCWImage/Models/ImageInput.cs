using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadHCWImage.Models
{
    public class ImageInput
    {

        public string content { get; set; }
        public string path { get; set; }

        public string bucketName { get; set; }

     
    }
}
