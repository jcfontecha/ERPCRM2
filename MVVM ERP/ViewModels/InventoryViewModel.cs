using MVVM_ERP.Models;
using MVVM_ERP.ViewModels.Commands;
using MVVM_ERP.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MVVM_ERP.ViewModels
{
    class InventoryViewModel : ERPViewModel
    {
        #region Properties

        private ObservableCollection<Product> products;
        /// <summary>
        /// Gets and sets the Products for binding
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
        /// Gets and sets the SelectedProduct for binding
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

        private StockProduct selectedStockProduct;
        /// <summary>
        /// Gets and sets the SelectedStockProduct for binding
        /// </summary>
        public StockProduct SelectedStockProduct
        {
            get { return selectedStockProduct; }
            set
            {
                selectedStockProduct = value;
                OnPropertyChange("SelectedStockProduct");
            }
        }

        private StockComponent selectedStockComponent;

        public StockComponent SelectedStockComponent
        {
            get { return selectedStockComponent; }
            set
            {
                selectedStockComponent = value;
                OnPropertyChange("SelectedStockComponent");
            }
        }


        private ICollectionView stockProductsView;

        public ICollectionView StockProductsView
        {
            get { return stockProductsView; }
            set
            {
                stockProductsView = value;
                OnPropertyChange("StockProductsView");
            }
        }

        private ICollectionView stockComponentsView;

        public ICollectionView StockComponentsView
        {
            get { return stockComponentsView; }
            set
            {
                stockComponentsView = value;
                OnPropertyChange("StockComponentsView");
            }
        }


        #endregion

        #region Commands

        /// <summary>
        /// Command to create a new product.
        /// </summary>
        public ICommand CreateNewProductCommand { get; set; }

        /// <summary>
        /// Command to edit the selected product
        /// </summary>
        public ICommand EditStockProductCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the InventoryViewModel class.
        /// </summary>
        public InventoryViewModel()
        {
            RefreshData();

            CreateNewProductCommand = new RelayCommand(c => AddNewProduct(), c => !IsLoading && SelectedProduct != null);
            EditStockProductCommand = new RelayCommand(c => EditStockProduct(), c => !IsLoading && SelectedStockProduct != null);

            PropertyChanged += OnComponentsSearch;
        }

        private void OnComponentsSearch(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SearchString")
            {
                if (string.IsNullOrWhiteSpace(SearchString))
                {
                    StockComponentsView.Filter = null;
                }
                else
                {
                    StockComponentsView.Filter = x =>
                    {
                        var sc = x as StockComponent;

                        if (sc.Component.key.Contains(SearchString)) return true;
                        if (sc.Component.description.Contains(SearchString)) return true;

                        return false;
                    };
                }
            }
        }

        private void RefreshData()
        {
            LoadData(() =>
            {
                context.StockComponents.Load();
                context.StockProducts.Load();
                context.Products.Load();
            }, () =>
            {
                Products = context.Products.Local;

                var query1 = from x in context.StockProducts.Local
                             where x.SaleOrder == null
                             select x;

                var query2 = from x in context.StockComponents.Local
                             where x.StockProduct == null
                             select x;
                
                StockProductsView = CollectionViewSource.GetDefaultView(query1);
                StockComponentsView = CollectionViewSource.GetDefaultView(query2);
            });
        }

        #endregion

        /// <summary>
        /// Usually called by the CreateNewProductCommand. Creates a new product.
        /// </summary>
        private void AddNewProduct()
        {
            var stockComponentsOut = new ObservableCollection<StockComponent>();

            var stockProduct = new StockProduct
            {
                Product = SelectedProduct,
                expirationDate = DateTime.Today
            };

            foreach (Models.Component comp in SelectedProduct.Components)
            {
                var query = from x in context.StockComponents.Local
                            where x.Component == comp && x.StockProduct == null
                            select x;

                if (query.Count() == 0)
                {
                    MessageBox.Show("There aren't enough materials to craft that wand. You're out of " + comp.description + "!");
                    return;
                }

                var sc = query.First();
                sc.StockProduct = stockProduct;
                stockComponentsOut.Add(sc);
            }

            stockProduct.StockComponents = stockComponentsOut;

            context.StockProducts.Local.Add(stockProduct);
            RefreshData();
        }

        private void EditStockProduct()
        {
            var viewModel = new EditStockProductViewModel(context, SelectedStockProduct);
            PresentEditStockProductViewModel(viewModel);
        }

        #region Presenter

        private void PresentEditStockProductViewModel(EditStockProductViewModel viewModel)
        {
            var window = new EditStockProductView();
            window.mainGrid.DataContext = viewModel;
            window.Show();
        }

        #endregion
    }
}
