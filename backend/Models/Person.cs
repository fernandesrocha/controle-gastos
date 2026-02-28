using System.ComponentModel.DataAnnotations;

// Model para cadastro de pessoas, com deleção em cascata para transações associadas.
public class Person
{
    [Key]
    public int Id { get; set; } // Identificador único auto-gerado

    [Required(ErrorMessage = "O nome é obrigatório")]
    [MaxLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Name { get; set; } = null!; // Nome (max 200 caracteres)

    [Required(ErrorMessage = "A idade é obrigatória")]
    [Range(0, int.MaxValue, ErrorMessage = "A idade deve ser um valor positivo")]
    public int Age { get; set; } // Idade (não permite números negativos).

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>(); // Transações associadas
}