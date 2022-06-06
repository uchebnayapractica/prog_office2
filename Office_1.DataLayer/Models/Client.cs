using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Office_2.DataLayer.Models;

public class Client
{

    public int Id { get; set; }
    
    [Required]
    [Comment("ФИО Заявителя")]
    public string Name { get; set; }
    
    [Required]
    [Comment("Адрес")]
    public string Address { get; set; }

    public List<Request> Requests { get; set; }

}