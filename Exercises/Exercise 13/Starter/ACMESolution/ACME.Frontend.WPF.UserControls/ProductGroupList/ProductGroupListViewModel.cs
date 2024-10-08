﻿using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using ACME.Frontend.WPF.Core.Base;
using ACME.Frontend.WPF.Core.Interfaces;
using ACME.Frontend.WPF.UserControls.ProductList;
using ACME.Frontend.WPF.UserControls.ViewModels;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ACME.Frontend.WPF.UserControls.ProductGroupList;

public class ProductGroupListViewModel : BaseViewModel
{
    // TODO 4: Inject the IUnitOfWork here.
    private readonly IViewMediator _viewMediator;

    public ProductGroupListViewModel(IViewMediator viewMediator)
    {
        _viewMediator = viewMediator;
        SelectCommand = new RelayCommand<ProductGroupViewModel>(SelectItem);
    }

    private void SelectItem(ProductGroupViewModel? group)
    {
        if (group == null) return;
        _viewMediator.DataBag.ProductGroup = group;
        _viewMediator.Activate<ProductListView>();
    }

    public ICommand SelectCommand { get; }
    public ObservableCollection<ProductGroupViewModel> ProductGroups { get; private set; } = new ObservableCollection<ProductGroupViewModel>();
    public async Task LoadProductGroupsAsync()
    {
        // TODO 6: Retrieve the ProductGroupRepository.
        IProductGroupRepository repository = default;
        ProductGroups.Clear();
        // TODO 7: Call the GetAllAsync method from the repository and assign
        // the result to the variable groups
        // Test the program.
        IEnumerable<ProductGroup> groups = default;
        foreach(var item in groups.Select(pg=>new ProductGroupViewModel { Id = pg.Id, Name = pg.Name, Image = pg.Image }))
        {
            ProductGroups.Add(item);
        }
    }
}
