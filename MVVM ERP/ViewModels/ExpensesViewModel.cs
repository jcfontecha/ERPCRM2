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
    class ExpensesViewModel : ERPViewModel
    {
        #region Properties

        private ObservableCollection<Expense> expenses;
        /// <summary>
        /// Returns the Expenses to list
        /// </summary>
        public ObservableCollection<Expense> Expenses
        {
            get { return expenses; }
            set
            {
                expenses = value;
                OnPropertyChange("Expenses");
            }
        }

        private Expense selectedExpense;
        /// <summary>
        /// Returns and sets the selected Expense. Ready to be binded.
        /// </summary>
        public Expense SelectedExpense
        {
            get { return selectedExpense; }
            set
            {
                selectedExpense = value;
                OnPropertyChange("SelectedExpense");
            }
        }

        #endregion // Properties

        #region Constructors

        public ExpensesViewModel()
        {
            // Load the data
            ToLoadAction = () => { context.Expenses.Load(); };
            AfterLoadingAction = () =>
            {
                Expenses = context.Expenses.Local;
                ItemsView = CollectionViewSource.GetDefaultView(Expenses);
            };
            LoadData();

            SearchFilter = x =>
            {
                var exp = x as Expense;

                if (exp.concept.Contains(SearchString)) return true;
                if (exp.Employee.username.Contains(SearchString)) return true;

                return false;
            };

            // Setup UI behaviour through commands
            AddNewItemCommand = new RelayCommand(c => AddNewExpense(), c => !IsLoading);
            EditSelectedItemCommand = new RelayCommand(c => EditSelectedExpense(), c => !IsLoading && SelectedExpense != null);
            DeleteSelectedItemCommand = new RelayCommand(c => DeleteSelectedExpense(), c => !IsLoading && SelectedExpense != null);
        }

        #endregion // Constructors

        private void DeleteSelectedExpense()
        {
            var dialog = MessageBox.Show("Are you sure you want to delete the selected Expenses?", "Confirm Deletion", MessageBoxButton.OKCancel);

            if (dialog == MessageBoxResult.OK)
                context.Expenses.Remove(SelectedExpense);
        }

        private void EditSelectedExpense()
        {
            var viewModel = new EditExpenseViewModel(context, SelectedExpense);
            PresentEditExpenseView(viewModel);
        }

        private void AddNewExpense()
        {
            var viewModel = new EditExpenseViewModel(context);
            PresentEditExpenseView(viewModel);
        }

        #region Presenter

        private void PresentEditExpenseView(EditExpenseViewModel viewModel)
        {
            var window = new EditExpenseView();
            window.mainGrid.DataContext = viewModel;
            window.Show();
        }

        #endregion
    }
}
