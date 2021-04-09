using GoogleDrive.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleDrive.Models.DataManager
{
    public class TrainingFileManager : IDataRepository<TrainingFiles>
    {
        readonly ApplicationContext _applicationContext;

        public TrainingFileManager(ApplicationContext context)
        {
            _applicationContext = context;
        }

        public IEnumerable<TrainingFiles> GetAll()
        {
            return _applicationContext.TrainingFiles.ToList();            
        }



        public TrainingFiles Get(long id)
        {
            return _applicationContext.TrainingFiles
                  .FirstOrDefault(e => e.FileID == id);
        }
        public void Add(TrainingFiles entity)
        {
            bool FileExists = _applicationContext.TrainingFiles.Select(x => x.FileName == entity.FileName).FirstOrDefault();

            int count = _applicationContext.TrainingFiles.Count(x => x.FileName == entity.FileName);


            if (count == 0)
            {
                _applicationContext.TrainingFiles.Add(entity);
                _applicationContext.SaveChanges();
            }
           
        }
        public void Update(TrainingFiles file, TrainingFiles entity)
        {
            file.FileID = entity.FileID;
            file.FileName = entity.FileName;
            file.FilePath = entity.FilePath;            
            _applicationContext.SaveChanges();
        }
        public void Delete(TrainingFiles file)
        {
            _applicationContext.TrainingFiles.Remove(file);
            _applicationContext.SaveChanges();
        }




    }
}
