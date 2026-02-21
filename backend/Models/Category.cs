using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public enum Purpose { Expense, Income, Both } // Enum para finalidade

public class Category
{
    [Key]
    public int Id { get; set; } // Identificador único auto-gerado

    [Required]
    [MaxLength(400)]
    public string Description { get; set; } = null!; // Descrição (max 400 caracteres)

    [Required]
    public Purpose Purpose { get; set; } // Finalidade: despesa/receita/ambas

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>(); // Transações associadas
}