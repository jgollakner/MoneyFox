﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

#nullable enable
namespace MoneyFox.Uwp.Services
{
    public partial class NavigationService : INavigationService
    {
        static private readonly ConcurrentDictionary<Type, Type> viewModelMap = new ConcurrentDictionary<Type, Type>();

        static NavigationService()
        {
            MainViewId = ApplicationView.GetForCurrentView().Id;
        }

        static public int MainViewId { get; }

        static public void Register<TViewModel, TView>() where TView : Page
        {
            if(!viewModelMap.TryAdd(typeof(TViewModel), typeof(TView)))
            {
                throw new InvalidOperationException($"ViewModel already registered '{typeof(TViewModel).FullName}'");
            }
        }

        static public Type GetView<TViewModel>()
        {
            return GetView(typeof(TViewModel));
        }

        static public Type GetView(Type viewModel)
        {
            if(viewModelMap.TryGetValue(viewModel, out Type view))
            {
                return view;
            }
            throw new InvalidOperationException($"View not registered for ViewModel '{viewModel.FullName}'");
        }

        static public Type GetViewModel(Type view)
        {
            var type = viewModelMap.Where(r => r.Value == view).Select(r => r.Key).FirstOrDefault();
            if(type == null)
            {
                throw new InvalidOperationException($"View not registered for ViewModel '{view.FullName}'");
            }
            return type;
        }

        public bool IsMainView => CoreApplication.GetCurrentView().IsMain;

        public Frame Frame { get; private set; }

        public bool CanGoBack => Frame.CanGoBack;

        public bool GoBack()
        {
            if(CanGoBack)
            {
                Frame.GoBack();
                return true;
            }

            return false;
        }

        public bool GoForward()
        {
            if(Frame.CanGoForward)
            {
                Frame.GoForward();
                return true;
            }
            return false;
        }

        public void Initialize(object frame)
        {
            Frame = (Frame)frame;
        }

        public bool Navigate<TViewModel>(object parameter = null)
        {
            return Navigate(typeof(TViewModel), parameter);
        }

        public bool Navigate(Type viewModelType, object parameter = null)
        {
            if(Frame == null)
            {
                throw new InvalidOperationException("Navigation frame not initialized.");
            }
            return Frame.Navigate(GetView(viewModelType), parameter);
        }
    }
}
