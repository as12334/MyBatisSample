using System;
using System.Web.UI;
using Data.Implements;


namespace Web
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var czUserService = new cz_usersService();
            czUserService.GetAllList();
            var accountService = new cz_usersService();
            accountService.GetAllList();
        }

    }
}