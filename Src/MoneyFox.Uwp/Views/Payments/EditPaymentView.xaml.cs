﻿using MoneyFox.Ui.Shared.ViewModels.Payments;
using MoneyFox.Uwp.ViewModels.Payments;
using Windows.UI.Xaml.Navigation;

namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class EditPaymentView
    {
        private EditPaymentViewModel ViewModel => (EditPaymentViewModel) DataContext;

        public EditPaymentView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Subscribe();
            var vm = (PaymentViewModel)e.Parameter;
            ViewModel.InitializeCommand.Execute(vm.Id);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) => ViewModel.Unsubscribe();
    }
}
