using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EventManagement.UserManagement.Data;
using Microsoft.AspNetCore.Identity;
using EventManagement.UserManagement.Shared.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace EventManagement.UserManagement.Web.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly UserManager<User> userManager;
        
        public DeleteModel(UserManager<User> userManager)
        {                       
            this.userManager = userManager;
        }

        public async Task<ActionResult> OnPostAsync(string id)
        {
            var userToDelete = await this.userManager.FindByIdAsync(id);
            if (userToDelete != null)
            {
                await this.userManager.DeleteAsync(userToDelete);
                return StatusCode(204);
            }
            else
            {
                return NotFound();
            }
        }      
    }
}