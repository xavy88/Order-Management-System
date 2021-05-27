using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.DataAccess.Data.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count()>0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                                
            }
            if (_db.Roles.Any(r=>r.Name==SD.Admin))
            {
                return;
            }
            _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Manager)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.User)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "111111111"

            }, "P@ssword1").GetAwaiter().GetResult();

            IdentityUser user = _db.Users.Where(u => u.Email == "admin@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(user, SD.Admin).GetAwaiter().GetResult();
        }
    }
}
