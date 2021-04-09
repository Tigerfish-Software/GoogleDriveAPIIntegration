using GoogleDrive.Models;
using GoogleDrive.Models.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Drive;
using GoogleDrive;
using Google.Apis.Drive.v3;
using System.IO;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Cloud.Storage.V1;


namespace GoogleDrive.Controllers
{
    [Route("api/TrainingFiles")]
    [ApiController]
    public class TrainingFilesController : ControllerBase
    {
        private readonly IDataRepository<TrainingFiles> _dataRepository;

        public TrainingFilesController(IDataRepository<TrainingFiles> dataRepository)
        {
            _dataRepository = dataRepository;
        }
        // GET: api/Employee
        [HttpGet]
        public IActionResult Get(string folderID)
        {
           
            //get list of google drive files
            //GetDriveFiles();
            
            var files2 = GetDriveFiles2(folderID);


            AddFilesToDB();
            IEnumerable<TrainingFiles> files = _dataRepository.GetAll();


            return Ok(files);
        }

        private void AddFilesToDB()
        {
            //gets local files.
            //TODO: need to check if Directory exists
            string[] fileArray = Directory.GetFiles(@"d:\Testing\");
            foreach (var item in fileArray)
            {
                string fileName;
                string filePath;

                filePath = item;
                fileName = Path.GetFileName(filePath);

                //save to DB
                TrainingFiles fileToAdd = new TrainingFiles();
                fileToAdd.FileName = fileName;
                fileToAdd.FilePath = filePath;

                //if (!System.IO.File.Exists(filePath))
                //{
                    _dataRepository.Add(fileToAdd);
                //}

               

            }
        }



        public static List<Google.Apis.Drive.v3.Data.File> GetDriveFiles()
        {
            Google.Apis.Drive.v3.DriveService service = GetGoogleService("service");

            // Define parameters of request.    
            Google.Apis.Drive.v3.FilesResource.ListRequest FileListRequest = service.Files.List();
            // for getting folders only.    
            //FileListRequest.Q = "mimeType='application/vnd.google-apps.folder'";    
            FileListRequest.Fields = "nextPageToken, files(*)";

            // List files.    
            IList<Google.Apis.Drive.v3.Data.File> files = FileListRequest.Execute().Files;
            List<Google.Apis.Drive.v3.Data.File> FileList = new List<Google.Apis.Drive.v3.Data.File>();


            // For getting only folders    
            // files = files.Where(x => x.MimeType == "application/vnd.google-apps.folder").ToList();    


            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Google.Apis.Drive.v3.Data.File File = new Google.Apis.Drive.v3.Data.File
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Size = file.Size,
                        Version = file.Version,
                        CreatedTime = file.CreatedTime,
                        Parents = file.Parents,
                        MimeType = file.MimeType
                    };
                    FileList.Add(File);
                }
            }
            return FileList;
        }

        public static List<Google.Apis.Drive.v3.Data.File> GetDriveFiles2(string folderID)
        {
            Google.Apis.Drive.v3.DriveService service = GetGoogleService("service");

            Google.Apis.Drive.v3.FilesResource.ListRequest listRequest = service.Files.List();
            //listRequest.Q = "'" + folderID + "' in parents";
            listRequest.Q = "mimeType='application/vnd.google-apps.folder'";
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            var counter = files.Count();



            return files.ToList();
        }


       

        public static void CreateFolderOnDrive(string Folder_Name)
        {
            Google.Apis.Drive.v3.DriveService service = GetGoogleService("service");

            Google.Apis.Drive.v3.Data.File FileMetaData = new
            Google.Apis.Drive.v3.Data.File();
            FileMetaData.Name = Folder_Name;
            FileMetaData.MimeType = "application/vnd.google-apps.folder";

            Google.Apis.Drive.v3.FilesResource.CreateRequest request;

            request = service.Files.Create(FileMetaData);
            request.Fields = "id";
            var file = request.Execute();
        }


        public static Google.Apis.Drive.v3.DriveService GetGoogleService(string authType)
        {
            Google.Apis.Drive.v3.DriveService service = null;

            if (authType == "service")
            {
                // Load the Service account credentials and define the scope of its access.
                var credential = GoogleCredential.FromFile("mydriveapiproject-310118-90ab749df59c.json")
                                .CreateScoped(DriveService.ScopeConstants.Drive);

                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });
            }
            else if (authType == "apikey")
            {
                Google.Apis.Services.BaseClientService.Initializer bcs = new Google.Apis.Services.BaseClientService.Initializer();
                bcs.ApiKey = "AIzaSyCOes0NQs2UyWdD3AcrsX60fQ1xl5h9Xk0";
                bcs.ApplicationName = "MyDriveAPIProjectKey";

                service = new DriveService(bcs);
            }

            return service;
        }











        // GET: api/Employee/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(long id)
        {
            TrainingFiles file = _dataRepository.Get(id);
            if (file == null)
            {
                return NotFound("The record couldn't be found.");
            }
            return Ok(file);
        }
        // POST: api/Employee
        [HttpPost]
        public IActionResult Post([FromBody] TrainingFiles file)
        {
            if (file == null)
            {
                return BadRequest("File is null.");
            }
            _dataRepository.Add(file);
            return CreatedAtRoute(
                  "Get",
                  new { Id = file.FileID },
                  file);
        }
        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] TrainingFiles file)
        {
            if (file == null)
            {
                return BadRequest("File is null.");
            }
            TrainingFiles fileToUpdate = _dataRepository.Get(id);
            if (fileToUpdate == null)
            {
                return NotFound("The file record couldn't be found.");
            }
            _dataRepository.Update(fileToUpdate, file);
            return NoContent();
        }
        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            TrainingFiles file = _dataRepository.Get(id);
            if (file == null)
            {
                return NotFound("The file record couldn't be found.");
            }
            _dataRepository.Delete(file);
            return NoContent();
        }


    }
}
