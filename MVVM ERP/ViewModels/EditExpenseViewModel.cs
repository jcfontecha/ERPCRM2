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

namespace MVVM_ERP.ViewModels
{
    class EditExpenseViewModel : ERPViewModel
    {
        #region Properties

        private Expense expense;
        /// <summary>
        /// Gets and sets the current Expense
        /// </summary>
        public Expense Expense
        {
            get { return expense; }
            set
            {
                expense = value;
                OnPropertyChange("Expense");
            }
        }


        private ObservableCollection<Employee> employees;
        /// <summary>
        /// Returns the list of available countries.
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

        #endregion

        #region Commands

        public ICommand EditPurchaseCommand { get; set; }

        #endregion

        #region Constructors

        public EditExpenseViewModel(ERPContext c, Expense e) : base(c)
        {
            Expense = e;
            LoadData(LoadRelevantData, AssignRelevantData);

            EditPurchaseCommand = new RelayCommand(x => LaunchEditPurchaseModule(), x => !IsLoading);
        }

        public EditExpenseViewModel() { SetupNewExpense(); }

        public EditExpenseViewModel(ERPContext c) : base(c) { SetupNewExpense(); }

        private void SetupNewExpense()
        {
            Expense = new Expense();
            Expense.date = DateTime.Now;

            LoadData(LoadRelevantData, AssignRelevantData);

            EditPurchaseCommand = new RelayCommand(x => LaunchEditPurchaseModule(), x => !IsLoading);

            BeforeSavingAction = () =>
            {
                if (Expense != null)
                {
                    context.Expenses.Add(Expense);
                }
            };
        }

        private void LoadRelevantData()
        {
            context.Employees.Load();
            context.Expenses.Load();
        }

        private void AssignRelevantData()
        {
            Employees = context.Employees.Local;
        }

        #endregion

        private void LaunchEditPurchaseModule()
        {
            var viewModel = new EditPurchaseViewModel(context, Expense);
            PresentEditExpenseView(viewModel);
        }

        #region Presenter

        private void PresentEditExpenseView(EditPurchaseViewModel viewModel)
        {
            var window = new EditPurchaseView();
            window.mainGrid.DataContext = viewModel;
            window.Show();
        }

        #endregion
    }
}
