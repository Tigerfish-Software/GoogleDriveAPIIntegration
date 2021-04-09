using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleDrive.Models
{
    [Table("TrainingFile")]
    public class TrainingFiles
    {
        [Key]
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
