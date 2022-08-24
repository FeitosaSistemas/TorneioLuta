using Microsoft.Extensions.DependencyInjection;
using TorneioLuta.Application;
using TorneioLuta.Interfaces.Application;
using TorneioLuta.Interfaces.Services;
using TorneioLuta.Services;

namespace TorneioLuta.IoC
{
    public static class NativeInjector
    {
        public static void RegisterNativeInjector (this IServiceCollection services)
        {
            #region Application

            services.AddScoped<ILutadoresApp, LutadoresApp>();
            services.AddScoped<ITorneioApp, TorneioApp>();

            #endregion

            #region External Services

            services.AddTransient<ILutadoresServices, LutadoresServices>();

            #endregion

            #region Repository

            #endregion
        }
    }
}
