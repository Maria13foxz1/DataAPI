using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messanger.Models;

[Table("chatmodel", Schema = "public") ]
public class ChatModel
{
    [Key] public int chat_id { get; set; }
    [Column("users_id")]
    public List<int> users { get; set; }
    [Column("messages_id")]
    public List<int> messages { get; set; }
}