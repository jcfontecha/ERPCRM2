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
    class SalesViewModel : ERPViewModel
    {
        #region Properties

        private ObservableCollection<SaleOrder> saleOrders;

        public ObservableCollection<SaleOrder> SaleOrders
        {
            get { return saleOrders; }
            set
            {
                saleOrders = value;
                OnPropertyChange("SaleOrders");
            }
        }

        private SaleOrder selectedSaleOrder;

        public SaleOrder SelectedSaleOrder
        {
            get { return selectedSaleOrder; }
            set
            {
                selectedSaleOrder = value;
                OnPropertyChange("SelectedSaleOrder");
            }
        }

        #endregion

        #region Constructors

        public SalesViewModel()
        {
            ToLoadAction = () => { context.SaleOrders.Load(); };
            AfterLoadingAction = () =>
            {
                SaleOrders = context.SaleOrders.Local;
                ItemsView = CollectionViewSource.GetDefaultView(SaleOrders);
            };

            LoadData();

            SearchFilter = x =>
            {
                var sale = x as SaleOrder;

                if (sale.Employee.username.Contains(SearchString)) return true;
                if (sale.Invoice.Client.name.Contains(SearchString)) return true;

                return false;
            };
            

            // Setup UI behaviour through commands
            AddNewItemCommand = new RelayCommand(c => AddNewSaleOrder(), c => !IsLoading);
            EditSelectedItemCommand = new RelayCommand(c => EditSelectedSaleOrder(), c => !IsLoading && SelectedSaleOrder != null);
            DeleteSelectedItemCommand = new RelayCommand(c => DeleteSelectedSaleOrder(), c => !IsLoading && SelectedSaleOrder != null);
        }

        #endregion

        private void AddNewSaleOrder()
        {
            var viewModel = new EditSaleViewModel(context);
            PresentEditSaleView(viewModel);
        }

        private void DeleteSelectedSaleOrder()
        {
            var dialog = MessageBox.Show("Are you sure you want to remove the selected sale?",
                "Removing Sale Order", MessageBoxButton.OKCancel);

            if (dialog == MessageBoxResult.OK)
                context.SaleOrders.Remove(SelectedSaleOrder);
        }

        private void EditSelectedSaleOrder()
        {
            var viewModel = new EditSaleViewModel(context, SelectedSaleOrder);
            PresentEditSaleView(viewModel);
        }

        #region Presenter

        private void PresentEditSaleView(EditSaleViewModel viewModel)
        {
            var window = new EditSaleView();
            window.mainGrid.DataContext = viewModel;
            window.Show();
        }

        #endregion
    }
}
