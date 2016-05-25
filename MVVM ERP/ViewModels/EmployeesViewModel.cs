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
using System.Windows.Input;

namespace MVVM_ERP.ViewModels
{
    class EmployeesViewModel : ERPViewModel
    {
        #region Properties

        private ObservableCollection<Employee> employees;
        /// <summary>
        /// Returns the Employees to list
        /// </summary>
        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChange("Employees");
            }
        }

        private Employee selectedEmployee;
        /// <summary>
        /// Returns and sets the selected Employee. Ready to be binded.
        /// </summary>
        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                OnPropertyChange("SelectedEmployee");
            }
        }

        #endregion // Properties

        #region Constructors

        public EmployeesViewModel()
        {
            // Load the data
            ToLoadAction = () => { context.Employees.Load(); };
            AfterLoadingAction = () => { Employees = context.Employees.Local; };
            LoadData();

            // Setup UI behaviour through commands
            AddNewItemCommand = new RelayCommand(c => AddNewEmployee(), c => !IsLoading);
            EditSelectedItemCommand = new RelayCommand(c => EditSelectedEmployee(), c => !IsLoading && SelectedEmployee != null);
            DeleteSelectedItemCommand = new RelayCommand(c => DeleteSelectedEmployee(), c => !IsLoading && SelectedEmployee != null);
        }

        #endregion // Constructors

        private void DeleteSelectedEmployee()
        {
            var dialog = MessageBox.Show("Are you sure you want to delete the selected Employees?", "Confirm Deletion", MessageBoxButton.OKCancel);

            if (dialog == MessageBoxResult.OK)
                context.Employees.Remove(SelectedEmployee);
        }

        private void EditSelectedEmployee()
        {
            var viewModel = new EditEmployeeViewModel(context, SelectedEmployee);
            PresentEditEmployeeView(viewModel);
        }

        private void AddNewEmployee()
        {
            var viewModel = new EditEmployeeViewModel(context);
            PresentEditEmployeeView(viewModel);
        }

        #region Presenter

        private void PresentEditEmployeeView(EditEmployeeViewModel viewModel)
        {
            var window = new EditEmployeeView();
            window.mainGrid.DataContext = viewModel;
            window.Show();
        }

        #endregion
    }
}
