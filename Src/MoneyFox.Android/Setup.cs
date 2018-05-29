using MoneyFox.Business;
using MoneyFox.Droid.Manager;
using MoneyFox.Droid.OneDriveAuth;
using MoneyFox.Droid.Services;
using MoneyFox.Droid.Src;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Core;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace MoneyFox.Droid
{
    public class Setup : MvxFormsAndroidSetup<CoreApp, App>
    {
        public Setup()
        {
            Strings.Culture = new Localize().GetCurrentCultureInfo();
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.ConstructAndRegisterSingleton<IConnectivity, ConnectivityImplementation>();
            Mvx.ConstructAndRegisterSingleton<IDialogService, DialogService>();
            Mvx.ConstructAndRegisterSingleton<IModifyDialogService, ModifyDialogService>();
            Mvx.ConstructAndRegisterSingleton<IOneDriveAuthenticator, OneDriveAuthenticator>();
            Mvx.ConstructAndRegisterSingleton<IProtectedData, ProtectedData>();
            Mvx.ConstructAndRegisterSingleton<INotificationService, NotificationService>();
            Mvx.ConstructAndRegisterSingleton<ITileManager, TileManager>();
            Mvx.ConstructAndRegisterSingleton<IAppInformation, DroidAppInformation>();
            Mvx.ConstructAndRegisterSingleton<IStoreOperations, PlayStoreOperations>();
            Mvx.ConstructAndRegisterSingleton<ISettings, Settings>();
            Mvx.ConstructAndRegisterSingleton<IBackgroundTaskManager, BackgroundTaskManager>();

            DependencyRegistrator.RegisterDependencies();
        }

        //public override MvxLogProviderType GetDefaultLogProviderType()
        //    => MvxLogProviderType.Serilog;

        //protected override IMvxLogProvider CreateLogProvider()
        //{
        //    Log.Logger = new LoggerConfiguration()
        //                 .MinimumLevel.Debug()
        //                 .WriteTo.AndroidLog()
        //                 .CreateLogger();
        //    return base.CreateLogProvider();
        //}

        protected override void InitializeLastChance()
        {
            Mvx.ConstructAndRegisterSingleton<IBackgroundTaskManager, BackgroundTaskManager>();
            base.InitializeLastChance();
        }
    }
}