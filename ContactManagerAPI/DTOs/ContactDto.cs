using System.ComponentModel.DataAnnotations;

namespace ContactManagerAPI.DTOs;

public class ContactDto
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    public bool Married { get; set; }

    [Required, MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }
}