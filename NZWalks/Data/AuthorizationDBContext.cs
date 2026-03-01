using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace NZWalks.Data
{
    public class AuthorizationDBContext : IdentityDbContext<IdentityUser>
    {
        public AuthorizationDBContext(DbContextOptions<AuthorizationDBContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerID = "fd5ba5a6-eae2-475b-80b8-4305c30dbd62";
            var writerId = "6d01ef39-da95-4ecf-8b8b-347760f20032";

            var roles = new List<IdentityRole>
            {
                new IdentityRole {
                Id = readerID,
                Name = "reader",
                NormalizedName = "READER",
                ConcurrencyStamp = readerID
                }
               ,
                new IdentityRole {
                Id = writerId,
                Name = "writer",
                NormalizedName = "WRITER",
                ConcurrencyStamp = writerId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
