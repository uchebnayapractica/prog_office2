using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Office_1.DataLayer.Models;

public class Settings
{

    public int Id { get; set; }

    [Required]
    [Comment("Путь, в который будут выгружены выбранные запросы")]
    public string ExportPath { get; set; }
    
    [Required]
    [Comment("Путь, из которого будут загружены запросы")]
    public string ImportPath { get; set; }

}