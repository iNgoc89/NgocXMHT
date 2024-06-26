using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NGOCXMHT_Identity.Models;
using NGOCXMHT_Identity.Models.Entities;

namespace NGOCXMHT_Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Employee> Employee { get; set; } = default!;
    public DbSet<MemberUnit> MemberUnit { get; set; } = default!;
}
