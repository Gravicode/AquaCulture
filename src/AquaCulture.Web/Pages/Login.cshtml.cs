using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AquaCulture.Web.Data;
using System.Linq;
using AquaCulture.Tools;

namespace AquaCulture.Web.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        AquaCultureDB db { set; get; }
        public string ReturnUrl { get; set; }
        public async Task<IActionResult>
            OnGetAsync(string paramUsername, string paramPassword)
        {
            if (db == null) db = new AquaCultureDB();
            string returnUrl = Url.Content("~/");
            try
            {
                // Clear the existing external cookie
                await HttpContext
                    .SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch { }
            bool isAuthenticate = false;
            var usr = db.UserProfiles.Where(x => x.Username == paramUsername).FirstOrDefault();
            if(usr != null)
            {
                var enc = new Encryption();
                var pass = enc.Decrypt(usr.Password);
                isAuthenticate = pass == paramPassword;
            }
            // In this example we just log the user in
            // (Always log the user in for this demo)
            if (isAuthenticate)
            {
                // *** !!! This is where you would validate the user !!! ***
                // In this example we just log the user in
                // (Always log the user in for this demo)
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, paramUsername),
                new Claim(ClaimTypes.Role, "Administrator"),
            };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    RedirectUri = this.Request.Host.Value
                };
                try
                {
                    await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            if (!isAuthenticate) returnUrl = "/auth/login?result=false";
            return LocalRedirect(returnUrl);
        }
    }
}
