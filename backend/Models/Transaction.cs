using System.ComponentModel.DataAnnotations;

public enum TransactionType { Expense, Income } // Enum para tipo

public class Transaction
{
    [Key]
    public int Id { get; set; } // Identificador único auto-gerado

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [MaxLength(400, ErrorMessage = "A descrição deve ter no máximo 400 caracteres")]
    public string Description { get; set; } = null!; // Descrição (max 400 caracteres)

    [Required(ErrorMessage = "O valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser positivo e maior que zero")] // Valor positivo > 0
    public decimal Value { get; set; }

    [Required(ErrorMessage = "O tipo é obrigatório")]
    public TransactionType Type { get; set; } // Tipo: despesa/receita

    [Required(ErrorMessage = "A categoria é obrigatória")]
    public int CategoryId { get; set; } // ID da categoria
    public virtual Category Category { get; set; } = null!;

    [Required(ErrorMessage = "A pessoa é obrigatória")]
    public int PersonId { get; set; } // ID da pessoa
    public virtual Person Person { get; set; } = null!;
}