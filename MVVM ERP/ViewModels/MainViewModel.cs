using MVVM_ERP.ViewModels.Commands;
using MVVM_ERP.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_ERP.ViewModels
{
    class MainViewModel : ERPViewModel
    {
        /// <summary>
        /// Command to launch and show the Clients Module
        /// </summary>
        public ICommand LaunchClientsModuleCommand { get; set; }

        /// <summary>
        /// Command to launch and show the Products Module
        /// </summary>
        public ICommand LaunchProductsModuleCommand { get; set; }

        /// <summary>
        /// Command to launch and show the Components Module
        /// </summary>
        public ICommand LaunchComponentsModuleCommand { get; set; }

        /// <summary>
        /// Command to launch and show the Components Module
        /// </summary>
        public ICommand LaunchEmployeesModuleCommand { get; set; }

        /// <summary>
        /// Command to launch and show the Components Module
        /// </summary>
        public ICommand LaunchExpensesModuleCommand { get; set; }

        /// <summary>
        /// Command to launch and show the Components Module
        /// </summary>
        public ICommand LaunchInventoryModuleCommand { get; set; }

        /// <summary>
        /// Command to launch and show the Components Module
        /// </summary>
        public ICommand LaunchSalesModuleCommand { get; set; }

        /// <summary>
        /// Command to launch and show the Components Module
        /// </summary>
        public ICommand LaunchCRMCommand { get; set; }

        /// <summary>
        /// Instantiates a new instance of the MainViewModel class and 
        /// sets up its commands.
        /// </summary>
        public MainViewModel()
        {
            LaunchClientsModuleCommand = new RelayCommand(
                (x) =>
                {
                    var window = new ClientsView();
                    window.Show();
                }
            );

            LaunchProductsModuleCommand = new RelayCommand(
               (x) =>
               {
                   var window = new ProductsView();
                   window.Show();
               }
           );

            LaunchComponentsModuleCommand = new RelayCommand(
               (x) =>
               {
                   var window = new ComponentsView();
                   window.Show();
               }
           );

            LaunchEmployeesModuleCommand = new RelayCommand(
               (x) =>
               {
                   var window = new EmployeesView();
                   window.Show();
               }
           );

            LaunchExpensesModuleCommand = new RelayCommand(
               (x) =>
               {
                   var window = new ExpensesView();
                   window.Show();
               }
           );

            LaunchInventoryModuleCommand = new RelayCommand(
               (x) =>
               {
                   var window = new InventoryView();
                   window.Show();
               }
           );

            LaunchSalesModuleCommand = new RelayCommand(
               (x) =>
               {
                   var window = new SalesView();
                   window.Show();
               }
           );

            LaunchCRMCommand = new RelayCommand(
               (x) =>
               {
                   var window = new CRMView();
                   window.Show();
               }
           );
        }
    }
}