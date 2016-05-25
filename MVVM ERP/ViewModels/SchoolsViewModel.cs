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
    class SchoolsViewModel : ERPViewModel
    {
        private ObservableCollection<School> schools;
        /// <summary>
        /// Returns the countries list
        /// </summary>
        public ObservableCollection<School> Schools
        {
            get { return schools; }
            set
            {
                schools = value;
                OnPropertyChange("Schools");
            }
        }

        /// <summary>
        /// Initializes a new SchoolsViewModel instance
        /// </summary>
        public SchoolsViewModel()
        {
            ToLoadAction = () => { context.Schools.Load(); };
            AfterLoadingAction = () => { Schools = context.Schools.Local; };
            LoadData();
        }
    }
}
