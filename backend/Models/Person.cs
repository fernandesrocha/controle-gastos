using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Person
{
    [Key]
    public int Id { get; set; } // Identificador único auto-gerado

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } // Nome (max 200 caracteres)

    [Required]
    [Range(0, int.MaxValue)]
    public int Age { get; set; } // Idade (não permite números negativos).

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>(); // Transações associadas
}