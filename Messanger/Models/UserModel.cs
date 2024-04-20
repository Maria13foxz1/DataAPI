using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messanger.Models;

[Table("users", Schema = "public") ]
public class UserModel
{
    [Key] public int user_id { get; set; }
    public string user_name { get; set; }
    public string user_email { get; set; }
    public string password { get; set; }
}