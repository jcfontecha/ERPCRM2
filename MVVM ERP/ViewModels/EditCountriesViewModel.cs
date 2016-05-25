using MVVM_ERP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_ERP.ViewModels
{
    class EditCountriesViewModel : ERPViewModel
    {
        private ObservableCollection<Country> countries;
        /// <summary>
        /// Returns the countries list
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

        /// <summary>
        /// Initializes a new EditCountriesViewModel instance
        /// </summary>
        public EditCountriesViewModel()
        {
            ToLoadAction = () => { context.Countries.Load(); };
            AfterLoadingAction = () => { Countries = context.Countries.Local; };
            LoadData();
        }
    }
}
