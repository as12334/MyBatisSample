using System;
using System.Web.UI;
using Data.Implements;


namespace Web
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var czUserService = new Cz_userService();
            czUserService.GetAllList();
            var accountService = new AccountService();
            accountService.GetAllList();
        }

    }
}