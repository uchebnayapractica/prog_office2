using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Office_2.DataLayer.Models;

public class Request
{
    
    public int Id { get; set; }

    [Required] 
    [Comment("Заявитель")]
    public Client Client { get; set; }

    [Required]
    [Comment("ФИО руководителя")]
    public string DirectorName { get; set; }
    
    [Required]
    [Comment("Тематика")]
    public string Subject { get; set; }
    
    [Required]
    [Comment("Содержание")]
    public string Content { get; set; }
    
    [Required]
    [Comment("Резолюция")]
    public string Resolution { get; set; }
    
    [Required]
    [Comment("Статус")]
    public Status Status { get; set; }
    
    [NotMapped]
    public string StatusDescription => Status.GetDescription() ?? Status.ToString();

    [Comment("Примечание")]
    public string Remark { get; set; }

}