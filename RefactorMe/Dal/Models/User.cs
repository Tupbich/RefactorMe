using System.ComponentModel.DataAnnotations;
using RefactorMe.Dal.Models.Abstract;

namespace RefactorMe.Dal.Models;

public class User : Entity
{
    [MaxLength(100)]
    public string Name { get; set; }
}