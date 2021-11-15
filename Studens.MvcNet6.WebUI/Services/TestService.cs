using Studens.Commons.DependencyInjection;

namespace Studens.MvcNet6.WebUI.Services
{
    [ScopeDependency]
    public class TestService : ITestService
    {
        public string GetMessage()
        {
            return "Message is message";
        }
    }

    //[ScopeDependency]
    //public class TestService2 : ITestService
    //{
    //    public string GetMessage()
    //    {
    //        return "Message is message";
    //    }
    //}

    //[TransientDependency]
    //public class TestService3 : ITestService
    //{
    //    public string GetMessage()
    //    {
    //        return "Message is message";
    //    }
    //}
}