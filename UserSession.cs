public static class UserSession
{
    public static int UserId = 0;
    public static string Email = "";
    public static string Username = "";

    public static bool IsLoggedIn()
    {
        return UserId > 0;
    }

    public static void Logout()
    {
        UserId = 0;
        Email = "";
        Username = "";
    }
}
