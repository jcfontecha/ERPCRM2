using MVVM_ERP.Models;
using MVVM_ERP.ViewModels.Commands;
using MVVM_ERP.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MVVM_ERP.ViewModels
{
    class ProductsViewModel : ERPViewModel
    {
        #region Properties

        private ObservableCollection<Product> products;
        /// <summary>
        /// Returns the Products to list
        /// </summary>
        public ObservableCollection<Product> Products
        {
            get { return products; }
            set
            {
                products = value;
                OnPropertyChange("Products");
            }
        }

        private Product selectedProduct;
        /// <summary>
        /// Returns and sets the selected product. Ready to be binded.
        /// </summary>
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChange("SelectedProduct");
            }
        }

        #endregion // Properties

        #region Constructors

        public ProductsViewModel()
        {
            // Load the data
            ToLoadAction = () => { context.Products.Load(); };
            AfterLoadingAction = () =>
            {
                Products = context.Products.Local;
                ItemsView = CollectionViewSource.GetDefaultView(Products);
            };

            SearchFilter = c =>
            {
                var prod = c as Product;

                if (prod.description.Contains(SearchString))
                    return true;

                if (prod.key.Contains(SearchString))
                    return true;

                return false;
            };

            LoadData();

            // Setup UI behaviour through commands
            AddNewItemCommand = new RelayCommand(c => AddNewProduct(), c => !IsLoading);
            EditSelectedItemCommand = new RelayCommand(c => EditSelectedProduct(), c => !IsLoading && SelectedProduct != null);
            DeleteSelectedItemCommand = new RelayCommand(c => DeleteSelectedProduct(), c => !IsLoading && SelectedProduct != null);
        }

        #endregion // Constructors

        private void DeleteSelectedProduct()
        {
            var dialog = MessageBox.Show("Are you sure you want to delete the selected products?", "Confirm Deletion", MessageBoxButton.OKCancel);

            if (dialog == MessageBoxResult.OK)
                context.Products.Remove(SelectedProduct);
        }

        private void EditSelectedProduct()
        {
            var viewModel = new EditProductViewModel(context, SelectedProduct);
            PresentEditProductView(viewModel);
        }

        private void AddNewProduct()
        {
            var viewModel = new EditProductViewModel(context);
            PresentEditProductView(viewModel);
        }

        #region Presenter

        private void PresentEditProductView(EditProductViewModel viewModel)
        {
            var window = new EditProductView();
            window.mainGrid.DataContext = viewModel;
            window.Show();
        }


        #endregion
    }
}
