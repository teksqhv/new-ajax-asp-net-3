using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using aspnetWebApp.Models;

namespace aspnetWebApp.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> _useManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public RegisterModel(
        UserManager<ApplicationUser> UserManager,
        SignInManager<ApplicationUser> SignInManager)
    {
        _useManager = UserManager;
        _signInManager = SignInManager;
    }
    [BindProperty]
    public RegisterInput Input {get; set;} = new();
    public class RegisterInput
    {
        [Required]
        [EmailAddress]
        public string Email {get;set;} = "";
        [Required]
        [MinLength(6)]
        public string Password {get;set;} = "";
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword {get;set;}="";
    }
    public void OnGet()
    {

    }
    public async Task<IActionResult> OnPostAsync()
    {
        if(!ModelState.IsValid)
            return Page();
        var user = new ApplicationUser
        {
            UserName = Input.Email,
            Email = Input.Email
        };
        var result = await _useManager.CreateAsync(
            user,
            Input.Password
        );
        if(result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToPage("/Index");
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return Page();
    }

}