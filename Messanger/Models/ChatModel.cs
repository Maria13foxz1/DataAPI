using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SQLite;

namespace Messanger.Models;
[Table("name_info", Schema = "public")]
public class NameInfoModel
{
    [Column("id")] 
    [Key] public int Id { get; set; }
        
    [Column("name")] 
    public string Name { get; set; }
        
    [Column("gender")] 
    public string Gender { get; set; }
    
    [Column("gender_probability")] 
    public float GenderProbability { get; set; }
        
    [Column("age")] 
    public int Age { get; set; }

    [Column("nationality_data")] 
    public string NationalityData { get; set; } 
}       