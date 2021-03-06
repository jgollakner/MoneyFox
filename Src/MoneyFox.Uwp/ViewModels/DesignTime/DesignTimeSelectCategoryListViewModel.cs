﻿using MoneyFox.Ui.Shared.ViewModels.Categories;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeSelectCategoryListViewModel : ISelectCategoryListViewModel
    {
        public CategoryViewModel SelectedCategory { get; } = new CategoryViewModel();
    }
}
