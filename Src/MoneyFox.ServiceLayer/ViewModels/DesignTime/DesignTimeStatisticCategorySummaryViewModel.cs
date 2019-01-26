﻿using System.Globalization;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MoneyFox.ServiceLayer.ViewModels.Statistic;
using MvvmCross.ViewModels;

namespace MoneyFox.ServiceLayer.ViewModels.DesignTime
{
    public class DesignTimeStatisticCategorySummaryViewModel : IStatisticCategorySummaryViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

        public MvxObservableCollection<StatisticItem> CategorySummary => new MvxObservableCollection<StatisticItem>
        {
            new StatisticItem
            {
                Label = "Einkaufen",
                Value = 745,
                Percentage = 30
            },
            new StatisticItem
            {
                Label = "Beeeeer",
                Value = 666,
                Percentage = 70
            }
        };

        public bool HasData { get; } = true;
    }
}