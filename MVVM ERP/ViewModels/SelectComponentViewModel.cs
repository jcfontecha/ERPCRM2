using MVVM_ERP.Models;
using MVVM_ERP.ViewModels.Commands;
using System;
using System.Collections;
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
    class SelectComponentEventArgs
    {
        public Component Component;
    }

    class SelectComponentViewModel : ERPViewModel
    {
        #region Properties

        private ObservableCollection<Component> components;
        /// <summary>
        /// Gets and sets the components to bind.
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
        /// Gets and sets the SelectedComponents to be binded
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

        #endregion

        #region Commands

        /// <summary>
        /// Command to add the selected component and close the window.
        /// </summary>
        public ICommand AddComponentCommand { get; set; }

        #endregion

        #region Events

        public delegate void ComponentsSelectedEventHandler(object sender, SelectComponentEventArgs e);

        public event ComponentsSelectedEventHandler ComponentsSelected;

        public void OnComponentsSelected()
        {
            if (ComponentsSelected != null)
            {
                ComponentsSelected(this, new SelectComponentEventArgs
                {
                    Component = SelectedComponent
                });
            }
        }

        #endregion

        #region Constructors

        public SelectComponentViewModel(ERPContext c) : base(c)
        {
            CommonInitialization();
        }

        private void CommonInitialization()
        {
            LoadData(() =>
            {
                context.Components.Load();
            }, () =>
            {
                Components = context.Components.Local;
            });

            AddComponentCommand = new RelayCommand(x =>
            {
                OnComponentsSelected();
                (x as Window).Close();
            });
        }

        /// <summary>
        /// Default constructor. Usually only useful to the designer
        /// </summary>
        public SelectComponentViewModel() { }

        #endregion
    }
}