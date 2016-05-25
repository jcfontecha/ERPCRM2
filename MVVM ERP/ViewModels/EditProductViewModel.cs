using MVVM_ERP.Models;
using MVVM_ERP.ViewModels.Commands;
using MVVM_ERP.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_ERP.ViewModels
{
    class EditProductViewModel : ERPViewModel
    {
        #region Properties

        private Product product;
        /// <summary>
        /// Gets and sets the current Product
        /// </summary>
        public Product Product
        {
            get { return product; }
            set
            {
                product = value;
                OnPropertyChange("Product");
            }
        }

        private Component selectedComponent;
        /// <summary>
        /// Gets and sets the SelectedComponent for binding.
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

        public ICommand AddComponentCommand { get; set; }
        public ICommand RemoveComponentCommand { get; set; }

        #endregion

        #region Constructors

        public EditProductViewModel(ERPContext c, Product p) : base(c) {
            Product = p;
            CommonInitialization();
        }

        public EditProductViewModel() { SetupNewProduct(); }

        public EditProductViewModel(ERPContext c) : base(c) { SetupNewProduct(); }

        private void SetupNewProduct()
        {
            Product = new Product();

            BeforeSavingAction = () =>
            {
                if (Product != null)
                {
                    context.Products.Add(Product);
                }
            };

            CommonInitialization();
        }

        private void CommonInitialization()
        {
            AddComponentCommand = new RelayCommand(x => AddComponent(), x => !IsLoading);
            RemoveComponentCommand = new RelayCommand(x => RemoveComponent(), x => !IsLoading && SelectedComponent != null);
        }

        private void RemoveComponent()
        {
            Product.Components.Remove(SelectedComponent);
        }

        private void AddComponent()
        {
            var viewModel = new SelectComponentViewModel(context);
            viewModel.ComponentsSelected += OnComponentsSelected;
            PresentSelectComponentView(viewModel);
        }

        private void OnComponentsSelected(object sender, SelectComponentEventArgs e)
        {
            if (e.Component != null)
                Product.Components.Add(e.Component);
            
        }

        #endregion

        private void PresentSelectComponentView(SelectComponentViewModel viewModel)
        {
            var window = new SelectComponentView();
            window.mainGrid.DataContext = viewModel;
            window.Show();
        }
    }
}
