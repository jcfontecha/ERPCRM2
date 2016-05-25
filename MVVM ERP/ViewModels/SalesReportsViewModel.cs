using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_ERP.ViewModels
{
    class SalesReportsViewModel : ERPViewModel
    {
        #region Properties

        private DateTime fromDate;

        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChange("FromDate");
            }
        }

        private DateTime toDate = DateTime.Now;

        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChange("ToDate");
            }
        }

        private float netProfit;

        public float NetProfit
        {
            get { return netProfit; }
            set
            {
                netProfit = value;
                OnPropertyChange("NetProfit");
            }
        }

        private float profitBeforeTaxes;

        public float ProfitBeforeTaxes
        {
            get { return profitBeforeTaxes; }
            set
            {
                profitBeforeTaxes = value;
                OnPropertyChange("ProfitBeforeTaxes");
            }
        }

        private float taxes;

        public float Taxes
        {
            get { return taxes; }
            set
            {
                taxes = value;
                OnPropertyChange("Taxes");
            }
        }

        private string topSalesman;

        public string TopSalesman
        {
            get { return topSalesman; }
            set
            {
                topSalesman = value;
                OnPropertyChange("TopSalesman");
            }
        }

        private string mostSoldProduct;

        public string MostSoldProduct
        {
            get { return mostSoldProduct; }
            set
            {
                mostSoldProduct = value;
                OnPropertyChange("MostSoldProduct");
            }
        }

        private string mostPopularCountry;

        public string MostPopularCountry
        {
            get { return mostPopularCountry; }
            set
            {
                mostPopularCountry = value;
                OnPropertyChange("MostPopularCountry");
            }
        }

        #endregion

        #region Constructors

        public SalesReportsViewModel()
        {
            ToLoadAction = () =>
            {
                context.SaleOrders.Load();
            };
            AfterLoadingAction = () =>
            {
                Stats();
            };

            LoadData();

            PropertyChanged += DateChanges;
        }

        private void DateChanges(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FromDate" || e.PropertyName == "ToDate")
                if (!IsLoading) 
                    Stats();
        }

        private void Stats()
        {
            var netProfitQuery = from s in context.SaleOrders.Local
                                  where s.date > FromDate && s.date < ToDate
                                  select s.Invoice.finalPrice;

            if (netProfitQuery.Count() > 0)
                NetProfit = netProfitQuery.Sum();

            var taxQuery = from s in context.SaleOrders
                                 where s.date > FromDate && s.date < ToDate
                                 select s.Invoice.tax;

            if (taxQuery.Count() > 0)
                ProfitBeforeTaxes = NetProfit - taxQuery.Sum();

            if (taxQuery.Count() > 0)
                Taxes = taxQuery.Sum();

            var salesmanQuery = from s in context.SaleOrders
                                where s.date > FromDate && s.date < ToDate
                                group s by s.Employee into g
                                orderby g.Count()
                                select g.Key;

            if (salesmanQuery.Count() > 0)
                TopSalesman = salesmanQuery.First().username;

            var countryQuery = from s in context.SaleOrders
                                where s.date > FromDate && s.date < ToDate
                                group s by s.Invoice.Client.Address.Country into g
                                orderby g.Count()
                                select g.Key;

            if (countryQuery.Count() > 0)
                MostPopularCountry = countryQuery.First().name;
        }

        #endregion
    }
}
