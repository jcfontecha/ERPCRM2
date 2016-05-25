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
    class EditStockProductViewModel : ERPViewModel
    {
        #region Properties

        private StockProduct stockProduct;
        /// <summary>
        /// Gets and sets the current Product
        /// </summary>
        public StockProduct StockProduct
        {
            get { return stockProduct; }
            set
            {
                stockProduct = value;
                OnPropertyChange("StockProduct");
            }
        }

        #endregion

        #region Constructors

        public EditStockProductViewModel(ERPContext c, StockProduct p) : base(c)
        {
            StockProduct = p;
        }

        public EditStockProductViewModel() { }


        #endregion

    }
}
