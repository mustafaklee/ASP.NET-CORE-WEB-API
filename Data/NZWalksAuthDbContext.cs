using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyFirstApiProject.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void  OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "61C0EDCD-D7FB-4829-92CE-5C87FC6F0D7E";
            var writerRoleId = "12CDEDEW-D7FB-4829-92CE-5C87FC6F0D7E";

            var roles = new List<IdentityRole> {

            new IdentityRole
            {
                Id = readerRoleId,
                ConcurrencyStamp=readerRoleId,
                Name="Reader",
                NormalizedName="Reader".ToUpper()
            },
            new IdentityRole
            {
                Id = writerRoleId,
                ConcurrencyStamp=writerRoleId,
                Name="Writer",
                NormalizedName="Writer".ToUpper()
            }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
