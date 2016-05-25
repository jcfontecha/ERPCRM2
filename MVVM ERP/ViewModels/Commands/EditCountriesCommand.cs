using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_ERP.ViewModels.Commands
{
    class EditCountriesCommand : ICommand
    {
        /// <summary>
        /// Property for the View Model responsible for this command.
        /// </summary>
        public ERPViewModel viewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the EditCountriesCommand class
        /// </summary>
        public EditCountriesCommand(ERPViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return !viewModel.IsLoading;
        }

        public void Execute(object parameter)
        {
            viewModel.ShowEditCountries();
        }

        #endregion
    }
}
