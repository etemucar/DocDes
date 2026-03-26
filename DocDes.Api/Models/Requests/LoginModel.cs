using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DocDes.Api.Models;

public class LoginModel
{
    [Required]
    public string Identifier { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}