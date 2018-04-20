using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EventManagement.UserManagement.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EventManagement.UserManagement.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.UserManagement.Web.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly UserManager<User> userManager;

        private readonly RoleManager<IdentityRole> roleManager;

        [BindProperty]
        public EditUserModel EditUser { get; set; }

        [BindProperty]
        public IList<EditRoleModel> Roles { get; }

        public EditModel(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.Roles = new List<EditRoleModel>();
        }

        public async Task OnGetAsync(string id)
        {
            var dbuser = await this.userManager.FindByIdAsync(id);
            var dbuserRoles = await this.userManager.GetRolesAsync(dbuser);

            this.EditUser = new EditUserModel()
            {
                Id = dbuser.Id,
                UserName = dbuser.UserName,
                FirstName = dbuser.FirstName,
                LastName = dbuser.LastName,
                Email = dbuser.Email
            };

            foreach (var role in this.roleManager.Roles)
            {
                this.Roles.Add(new EditRoleModel()
                {
                    Name = role.Name,
                    IsSelected = dbuserRoles.Contains(role.Name)
                });
            }
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (this.ModelState.IsValid)
            {
                var dbuser = await this.userManager.FindByIdAsync(this.EditUser.Id);
                var dbuserRoles = await this.userManager.GetRolesAsync(dbuser);
                dbuser.FirstName = this.EditUser.FirstName;
                dbuser.LastName = this.EditUser.LastName;
                dbuser.Email = this.EditUser.Email;

                await this.userManager.UpdateAsync(dbuser);

                foreach (var role in this.Roles)
                {
                    if (role.IsSelected && !dbuserRoles.Contains(role.Name))
                    {
                        await this.userManager.AddToRoleAsync(dbuser, role.Name);
                    }

                    if (!role.IsSelected && dbuserRoles.Contains(role.Name))
                    {
                        await this.userManager.RemoveFromRoleAsync(dbuser, role.Name);
                    }
                }

                return this.Redirect("/Users/Index");
            }

            return this.Page();
        }

        public class EditUserModel
        {
            public string Id { get; set; }

            public string UserName { get; set; }

            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public class EditRoleModel
        {
            public string Name { get; set; }

            public bool IsSelected { get; set; }
        }
    }
}