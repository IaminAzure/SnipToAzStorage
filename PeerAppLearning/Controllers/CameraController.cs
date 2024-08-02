using Microsoft.AspNetCore.Mvc;
using PeerApp.Azlayer.Interface;

namespace PeerAppLearning.Controllers
{
    public class CameraController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IAzStorageOperations _azStorageOperations;

        public CameraController(IWebHostEnvironment hostingEnvironment, IAzStorageOperations azStorageOperations)
        {
            _environment = hostingEnvironment;
            this._azStorageOperations = azStorageOperations;
        }

        public IActionResult Capture()
        {
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> Capture(string name)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            // Getting Filename
                            var fileName = file.FileName;
                            // Unique filename "Guid"
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                            // Getting Extension
                            var fileExtension = Path.GetExtension(fileName);
                            // Concating filename + fileExtension (unique filename)
                            var newFileName = string.Concat(myUniqueFileName, fileExtension);
                            //  Generating Path to store photo 
                            var filepath = Path.Combine(_environment.WebRootPath, "CameraPhotos") + $@"\{newFileName}";

                            if (!string.IsNullOrEmpty(filepath))
                            {
                                // Storing Image in Folder
                                //StoreInFolder(file, filepath);
                               await  _azStorageOperations.BlobUploadAsync(file, newFileName);
                            }

                        }
                    }
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }
    }
}
