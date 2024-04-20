using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messanger.Models;

[Table("messagemodel", Schema = "public") ]
public class MessageModel
{
    [Key] public int message_id { get; set; }
    [Column("message_time")]
    public DateTime message_time { get; set; }
    
    [Column("sender")]
    public int sender { get; set; }
    
    [Column("content")]
    public string content { get; set; }
}