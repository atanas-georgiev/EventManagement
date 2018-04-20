using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EventManagement.UserManagement.Data;
using EventManagement.UserManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace EventManagement.UserManagement.Web.Pages.Users
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> userManager;

        public IndexModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.Users = new List<GridModel>();
        }

        public ICollection<GridModel> Users;

        public async Task OnGetAsync()
        {
            await this.userManager.Users.ForEachAsync(user =>
            {
                this.Users.Add(new GridModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                });
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OnPostDeleteAsync(string id)
        {
            var userToDelete = await this.userManager.FindByIdAsync(id);
            if (userToDelete != null)
            {
                await this.userManager.DeleteAsync(userToDelete);
            }
            this.Response.StatusCode = (int)HttpStatusCode.NoContent;
            return RedirectToPage("/Index");
        }
    }

    public class GridModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}