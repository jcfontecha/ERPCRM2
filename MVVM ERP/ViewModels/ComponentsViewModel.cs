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

namespace MVVM_ERP.ViewModels
{
    class ComponentsViewModel : ERPViewModel
    {
        #region Properties

        private ObservableCollection<Component> components;
        /// <summary>
        /// Gets and sets the Components list.
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
        /// Gets and sets the currently selected component.
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

        #endregion // Properties

        #region Constructors

        public ComponentsViewModel()
        {
            ToLoadAction = () => context.Components.Load();
            AfterLoadingAction = () => Components = context.Components.Local;
            LoadData();

            SearchFilter = c =>
            {
                var comp = c as Component;

                if (comp.key.Contains(SearchString) || comp.description.Contains(SearchString))
                    return true;

                return false;
            };

            AddNewItemCommand = new RelayCommand(c => AddNewComponent(), c => !IsLoading);
            EditSelectedItemCommand = new RelayCommand(c => EditSelectedComponent(), c => !IsLoading && SelectedComponent != null);
            DeleteSelectedItemCommand = new RelayCommand(c => DeleteSelectedComponent(), c => !IsLoading && SelectedComponent != null);
        }

        #endregion

        private void AddNewComponent()
        {
            var viewModel = new EditComponentViewModel(context);
            PresentEditComponentView(viewModel);
        }

        private void EditSelectedComponent()
        {
            var viewModel = new EditComponentViewModel(context, SelectedComponent);
            PresentEditComponentView(viewModel);
        }

        private void DeleteSelectedComponent()
        {
            var dialog = MessageBox.Show("Are you sure you want to delete the selected component?", "Delete confirmation", MessageBoxButton.OKCancel);

            if (dialog == MessageBoxResult.OK)
                context.Components.Remove(SelectedComponent);
        }

        #region Presentation

        private void PresentEditComponentView(EditComponentViewModel viewModel)
        {
            var window = new EditComponentView();
            window.mainGrid.DataContext = viewModel;

            window.Show();
        }

        #endregion
    }
}
