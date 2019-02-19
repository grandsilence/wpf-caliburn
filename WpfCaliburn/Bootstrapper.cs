using System.Windows;
using Caliburn.Micro;
using WpfCaliburn.ViewModels;

namespace WpfCaliburn
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            // TODO: Check if authenticated 
            //DisplayRootViewFor<ShellViewModel>();
            DisplayRootViewFor<LoginViewModel>();

            base.OnStartup(sender, e);
        }
    }
}