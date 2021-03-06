﻿using CommonServiceLocator;
using Microsoft.Services.Store.Engagement;
using MoneyFox.Ui.Shared.ViewModels.About;
using System;
using Windows.UI.Xaml;

namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class AboutView
    {
        private AboutViewModel ViewModel = ServiceLocator.Current.GetInstance<AboutViewModel>();

        public AboutView()
        {
            InitializeComponent();

            if(StoreServicesFeedbackLauncher.IsSupported())
            {
                FeedbackButton.Visibility = Visibility.Visible;
            }
        }

        private async void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            StoreServicesFeedbackLauncher launcher = StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }
    }
}
