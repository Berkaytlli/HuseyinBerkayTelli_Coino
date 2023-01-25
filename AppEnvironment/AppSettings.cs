
namespace AppEnvironment;
public class AppSettings
{
    public _ConnectionString ConnectionStrings { get; set; }
    public class _ConnectionString
    {
        public string DefaultConnection { get; set; }
    }
}