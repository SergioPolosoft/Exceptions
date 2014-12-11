namespace Security
{
    public interface IApplicationContext
    {
        User GetCurrentUser();
    }
}