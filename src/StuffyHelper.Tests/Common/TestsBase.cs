using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace StuffyHelper.Tests.Common
{
    [UsesVerify]
    public abstract class TestsBase
    {
        protected readonly VerifySettings VerifySettings = new();

        protected readonly HttpContext HttpContext = CommonTestConstants.GetHttpContext();

        protected readonly CancellationToken CancellationToken = CommonTestConstants.GetCancellationToken();

        protected TestsBase()
        {
            VerifySettings.IgnoreMember<Exception>(x => x.InnerException);
            VerifySettings.IgnoreMember("Message");
            
            VerifySettings.UseDirectory($"{GetType().Name}/");
            VerifySettings.UseTypeName("_");
            VerifySettings.IgnoreStackTrace();
            VerifySettings.DisableDiff();

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        }
    }
}
