using Microsoft.EntityFrameworkCore;

public class ExpensesDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configura chaves primárias auto-geradas
        modelBuilder.Entity<Person>().HasKey(p => p.Id);
        modelBuilder.Entity<Category>().HasKey(c => c.Id);
        modelBuilder.Entity<Transaction>().HasKey(t => t.Id);

        // Relação: Uma Pessoa para muitas Transações - Deleção em cascata
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Person)
            .WithMany(p => p.Transactions)
            .HasForeignKey(t => t.PersonId)
            .OnDelete(DeleteBehavior.Cascade); // Ao deletar pessoa, deleta transações

        // Relação: Uma Categoria para muitas Transações
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // Impede deleção de categoria se esta estiver em uso
    }
}