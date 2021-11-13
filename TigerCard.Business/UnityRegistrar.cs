using TigerCard.Business.Interfaces;
using TigerCard.Business.Settings;

using Unity;

namespace TigerCard.Business
{
    public static class UnityRegistrar
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterSingleton<IFareComputationService, FareComputationService>();
            container.RegisterSingleton<IFareAggregator, FareAggregator>();
            container.RegisterSingleton<IFareCalculator, FareCalculator>();
            container.RegisterFactory<IFareSettings>(c => FareSettingsFactory.CreateDefaultSettings(), FactoryLifetime.Singleton);
            
        }
    }
}
