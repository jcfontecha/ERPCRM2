using MVVM_ERP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_ERP.ViewModels
{
    class EditEmployeeViewModel : ERPViewModel
    {
        #region Properties

        private Employee employee;
        /// <summary>
        /// Gets and sets the current Employee
        /// </summary>
        public Employee Employee
        {
            get { return employee; }
            set
            {
                employee = value;
                OnPropertyChange("Employee");
            }
        }


        private ObservableCollection<Country> countries;
        /// <summary>
        /// Returns the list of available countries.
        /// </summary>
        public ObservableCollection<Country> Countries
        {
            get { return countries; }
            set
            {
                countries = value;
                OnPropertyChange("Countries");
            }
        }

        #endregion

        #region Constructors

        public EditEmployeeViewModel(ERPContext c, Employee e) : base(c)
        {
            Employee = e;

            LoadCountries();
        }

        public EditEmployeeViewModel() { SetupNewEmployee(); }

        public EditEmployeeViewModel(ERPContext c) : base(c) { SetupNewEmployee(); }

        private void SetupNewEmployee()
        {
            Employee = new Employee();
            Employee.Address = new Address();

            LoadCountries();

            BeforeSavingAction = () =>
            {
                if (Employee != null)
                {
                    context.Employees.Add(Employee);
                }
            };
        }

        private void LoadCountries()
        {
            ToLoadAction = () => context.Countries.Load();
            AfterLoadingAction = () => Countries = context.Countries.Local;
            LoadData();
        }

        #endregion
    }
}
