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
using System.Windows.Input;
using System.ComponentModel;

namespace MVVM_ERP.ViewModels
{
    class EditSaleViewModel : ERPViewModel
    {
        #region Properties

        private SaleOrder saleOrder;
        /// <summary>
        /// Gets and sets the current SaleOrder
        /// </summary>
        public SaleOrder SaleOrder
        {
            get { return saleOrder; }
            set
            {
                saleOrder = value;
                OnPropertyChange("SaleOrder");
            }
        }

        private StockProduct selectedStockProduct;
        /// <summary>
        /// Gets and sets the SelectedStockProduct for binding.
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

        private StockProduct selectedSaleStockProduct;
        /// <summary>
        /// Gets and sets the SelectedStockProduct for binding.
        /// </summary>
        public StockProduct SelectedSaleStockProduct
        {
            get { return selectedSaleStockProduct; }
            set
            {
                selectedSaleStockProduct = value;
                OnPropertyChange("SelectedSaleStockProduct");
            }
        }

        private ObservableCollection<Product> products;

        public ObservableCollection<Product> Products
        {
            get { return products; }
            set
            {
                products = value;
                OnPropertyChange("Products");
            }
        }

        private ObservableCollection<StockProduct> stockProducts = new ObservableCollection<StockProduct>();

        public ObservableCollection<StockProduct> StockProducts
        {
            get { return stockProducts; }
            set
            {
                stockProducts = value;
                OnPropertyChange("StockProducts");
            }
        }

        private ObservableCollection<StockProduct> saleStockProducts = new ObservableCollection<StockProduct>();

        public ObservableCollection<StockProduct> SaleStockProducts
        {
            get { return saleStockProducts; }
            set
            {
                saleStockProducts = value;
                OnPropertyChange("SaleStockProducts");
            }
        }

        private ObservableCollection<Employee> employees;

        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChange("Employees");
            }
        }

        private ObservableCollection<Client> clients;

        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChange("Clients");
            }
        }


        private Product selectedProduct;

        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChange("SelectedProduct");
            }
        }

        private float price = 0;

        public float Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChange("Price");
            }
        }

        private float discount = 0;

        public float Discount
        {
            get { return discount; }
            set
            {
                discount = value;
                OnPropertyChange("Discount");
            }
        }

        private float tax = 0;

        public float Tax
        {
            get { return tax; }
            set
            {
                tax = value;
                OnPropertyChange("Tax");
            }
        }

        private float total = 0;

        public float Total
        {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChange("Total");
            }
        }

        #endregion

        #region Commands

        public ICommand AddStockProductCommand { get; set; }
        public ICommand RemoveStockProductCommand { get; set; }

        #endregion

        #region Constructors

        public EditSaleViewModel(ERPContext c, SaleOrder so) : base(c)
        {
            SaleOrder = so;

            // Before we save, we add the selected stock products.
            BeforeSavingAction = () =>
            {
                SaleOrder.StockProducts = SaleStockProducts;

                SaleOrder.price = Price;
                SaleOrder.discount = Discount;
                SaleOrder.Invoice.tax = Tax;
                SaleOrder.Invoice.finalPrice = Total;
            };

            CommonInitialization();
        }

        public EditSaleViewModel() { SetupNewSaleOrder(); }

        public EditSaleViewModel(ERPContext c) : base(c) { SetupNewSaleOrder(); }

        private void SetupNewSaleOrder()
        {
            SaleOrder = new SaleOrder();
            SaleOrder.date = DateTime.Now;
            SaleOrder.Invoice = new Invoice();

            BeforeSavingAction = () =>
            {
                // Before we save, we add the selected stock products.
                SaleOrder.StockProducts = SaleStockProducts;

                SaleOrder.price = Price;
                SaleOrder.discount = Discount;
                SaleOrder.Invoice.tax = Tax;
                SaleOrder.Invoice.finalPrice = Total;

                if (SaleOrder != null)
                    context.SaleOrders.Add(SaleOrder);
            };

            CommonInitialization();
        }

        private void CommonInitialization()
        {
            // Load data
            ToLoadAction = () =>
            {
                context.Employees.Load();
                context.Products.Load();
                context.StockProducts.Load();
                context.Clients.Load();
            };

            AfterLoadingAction = () =>
            {
                Employees = context.Employees.Local;
                Products = context.Products.Local;
                Clients = context.Clients.Local;

                foreach (StockProduct sp in context.StockProducts.Local)
                {

                    if (SaleOrder.StockProducts.Contains(sp))
                        SaleStockProducts.Add(sp);
                    else if (sp.SaleOrder == null)
                        StockProducts.Add(sp);
                }
            };

            LoadData();

            // Setup commands
            AddStockProductCommand = new RelayCommand(x => AddStockProduct(),
                x => !IsLoading && SelectedStockProduct != null);
            RemoveStockProductCommand = new RelayCommand(x => RemoveStockProduct(),
                x => !IsLoading && SelectedSaleStockProduct != null);

            PropertyChanged += HandlePropertyChange;
        }

        private void RemoveStockProduct()
        {
            StockProducts.Add(SelectedSaleStockProduct);

            Price -= SelectedSaleStockProduct.Product.unitPrice.Value;

            SaleStockProducts.Remove(SelectedSaleStockProduct);

        }

        private void AddStockProduct()
        {
            SaleStockProducts.Add(SelectedStockProduct);

            Price += SelectedStockProduct.Product.unitPrice.Value;

            StockProducts.Remove(SelectedStockProduct);
        }

        private void HandlePropertyChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Price" || e.PropertyName == "Discount")
            {
                Tax = Price * 0.1f;
                Total = Price * (1 - Discount) + Tax;
            }
        }

        #endregion
    }
}