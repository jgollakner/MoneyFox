﻿using GalaSoft.MvvmLight.Command;
using MoneyFox.Ui.Shared.ViewModels.Statistics;
using System.Collections.Generic;

namespace MoneyFox.ViewModels.Statistics
{
    public interface IStatisticSelectorViewModel
    {
        List<StatisticSelectorType> StatisticItems { get; }

        RelayCommand<StatisticSelectorType> GoToStatisticCommand { get; }
    }
}
