using System.ComponentModel.DataAnnotations;

public enum TransactionType { Expense, Income } // Enum para tipo

public class Transaction
{
    [Key]
    public int Id { get; set; } // Identificador único auto-gerado

    [Required]
    [MaxLength(400)]
    public string Description { get; set; } = null!; // Descrição (max 400 caracteres)

    [Required]
    [Range(0.01, double.MaxValue)] // Valor positivo > 0
    public decimal Value { get; set; }

    [Required]
    public TransactionType Type { get; set; } // Tipo: despesa/receita

    [Required]
    public int CategoryId { get; set; } // ID da categoria
    public virtual Category Category { get; set; } = null!;

    [Required]
    public int PersonId { get; set; } // ID da pessoa
    public virtual Person Person { get; set; } = null!;
}