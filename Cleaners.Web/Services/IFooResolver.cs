namespace Cleaners.Web.Services
{
    public interface IFooResolver
    {
        IFoo GetInstance(string name);
    }
}