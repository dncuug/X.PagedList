using Example.DAL;

namespace Example.Website.Controllers;

public class Bootstrap41Controller : HomeController
{
    public Bootstrap41Controller(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}