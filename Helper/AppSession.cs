

using DL.Entities;


namespace Helpers
{
    public partial class AppSession
    {
        public static string UsersUploads = "Uploads/UsersImages";
        public static User CurrentUser
        {
            get
            {
                var _user = WebHelpers.HttpContext.Session.Get<User>("CurrentUser");
                if (_user != null)
                {
                    try
                    {  return _user; }
                    catch { }
                }
                return null;
            }
            set
            {
                WebHelpers.HttpContext.Session.Set("CurrentUser", value);
            }
        }

        
    }
}
