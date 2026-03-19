using Microsoft.EntityFrameworkCore;
using DocDes.Core.Model;
using System.Linq.Expressions;


namespace DocDes.Data;

public class DocDesDbContext : DbContext
{ 
    public DocDesDbContext(DbContextOptions<DocDesDbContext> options) : base(options)
    {
    }


    public DbSet<Address> DocDes { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ContactMedium> ContactMediums { get; set; }
    public DbSet<Individual> Individuals { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<LocalizableFields> LocalizableFields { get; set; }
    public DbSet<Localization> Localizations { get; set; }
    public DbSet<Neighborhood> Neighborhoods { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationLanguageRel> OrganizationLanguageRels { get; set; }
    public DbSet<Party> Parties { get; set; }
    public DbSet<PartyRole> PartyRoles { get; set; }
    public DbSet<PartyRoleAccount> PartyRoleAccounts { get; set; }
    public DbSet<PartyRoleType> PartyRoleTypes { get; set; }
    public DbSet<RelatedParty> RelatedParties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Önce varsa özel konfigürasyonları uygula
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocDesDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            var idProperty = entityType.FindProperty("Id");

            if (idProperty != null && !string.IsNullOrEmpty(tableName))
            {
                idProperty.SetColumnName($"{tableName}_id");
            }

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var propertyMethodInfo = typeof(EF).GetMethod("Property")?.MakeGenericMethod(typeof(int));
            var isDeletedProperty = Expression.Call(null, propertyMethodInfo!, parameter, Expression.Constant("IsDeleted"));
            var compareExpression = Expression.NotEqual(isDeletedProperty, Expression.Constant(1));
            var lambda = Expression.Lambda(compareExpression, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }       
    }
}