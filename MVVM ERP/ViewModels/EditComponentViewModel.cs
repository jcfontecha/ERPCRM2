using MVVM_ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_ERP.ViewModels
{
    class EditComponentViewModel : ERPViewModel
    {
        #region Properties

        private Component component;
        /// <summary>
        /// Gets or sets the Component
        /// </summary>
        public Component Component
        {
            get { return component; }
            set
            {
                component = value;
                OnPropertyChange("Component");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EditComponentViewModel class with a specified context for a new Component
        /// </summary>
        /// <param name="c"></param>
        public EditComponentViewModel(ERPContext c) : base(c) { SetupNewComponent(); }

        /// <summary>
        /// Initializes a new instance of the EditComponentViewModel class for a new Component.
        /// </summary>
        public EditComponentViewModel() { SetupNewComponent(); }

        /// <summary>
        /// Sets up a new instance of the EditComponentViewModel with the specified context and Component.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="component"></param>
        public EditComponentViewModel(ERPContext c, Component component) : base(c) { Component = component; }

        #endregion

        /// <summary>
        /// Sets up the ViewModel to add a new component.
        /// </summary>
        private void SetupNewComponent()
        {
            Component = new Component();

            BeforeSavingAction = () => context.Components.Add(Component);
        }
    }


}
