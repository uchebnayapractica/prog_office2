using System.ComponentModel;

namespace Office_1.DataLayer.Models;

public enum Status
{

    [Description("Создано")]
    Created,

    [Description("Рассматривается")]
    InReview,

    [Description("Рассмотрено")]
    Reviewed,

    [Description("Отклонено")]
    Declined,
    
    [Description("Завершено")]
    Completed

}