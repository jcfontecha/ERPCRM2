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
    class EditPurchaseViewModel : ERPViewModel
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

        private ObservableCollection<Component> components;
        /// <summary>
        /// Gets and sets the Components for binding
        /// </summary>
        public ObservableCollection<Component> Components
        {
            get { return components; }
            set
            {
                components = value;
                OnPropertyChange("Components");
            }
        }

        private Component selectedComponent;
        /// <summary>
        /// Gets and sets the SelectedComponent for binding
        /// </summary>
        public Component SelectedComponent
        {
            get { return selectedComponent; }
            set
            {
                selectedComponent = value;
                OnPropertyChange("SelectedComponent");
            }
        }

        private ObservableCollection<Vendor> vendors;
        /// <summary>
        /// Gets and sets the Vendors for binding
        /// </summary>
        public ObservableCollection<Vendor> Vendors
        {
            get { return vendors; }
            set
            {
                vendors = value;
                OnPropertyChange("Vendors");
            }
        }

        private int addNumber;
        /// <summary>
        /// Gets and sets the number of components to add for binding.
        /// </summary>
        public int AddNumber
        {
            get { return addNumber; }
            set
            {
                addNumber = value;
                OnPropertyChange("AddNumber");
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command to add components to the purchase.
        /// </summary>
        public ICommand AddComponentsCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the EditPurchaseViewModel class with a specific expense and context.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="e"></param>
        public EditPurchaseViewModel(ERPContext c, Expense e) : base(c)
        {
            Expense = e;

            AddComponentsCommand = new RelayCommand(x => AddComponents(), x => !IsLoading);

            LoadData(
                () =>
                {
                    context.Employees.Load();
                    context.Vendors.Load();
                    context.Components.Load();
                },
                () =>
                {
                    Employees = context.Employees.Local;
                    Vendors = context.Vendors.Local;
                    Components = context.Components.Local;
                });
        }

        /// <summary>
        /// Generic constructor
        /// </summary>
        public EditPurchaseViewModel() { }

        #endregion

        /// <summary>
        /// Adds the components required
        /// </summary>
        public void AddComponents()
        {
            for (int i = 0; i < AddNumber; i++)
            {
                Expense.StockComponents.Add(new StockComponent {
                    Component = SelectedComponent,
                    expirationDate = DateTime.Today,
                    Vendor = Vendors.First(),
                    warehouse = 1,
                    shelf = 1,
                    drawer = 1
                });
            }
        }
    }
}
