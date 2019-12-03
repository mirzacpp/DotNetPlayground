namespace Cleaners.Web.Services
{
    public interface IFoo
    {
        /// <summary>
        /// Used to resolve implementations
        /// </summary>
        string Name { get; }

        void Introduce();
    }
}