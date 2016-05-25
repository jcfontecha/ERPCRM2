using MVVM_ERP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using MVVM_ERP.ViewModels.Commands;
using MVVM_ERP.Views;

namespace MVVM_ERP.ViewModels
{
    class CountryViewModel : CRMViewModel
    {
        #region Properties

        private ObservableCollection<Country> countries;

        public ObservableCollection<Country> Countries
        {
            get { return countries; }
            set
            {
                countries = value;
                OnPropertyChange("Countries");
            }
        }

        private Country selectedCountry;

        public Country SelectedCountry
        {
            get { return selectedCountry; }
            set
            {
                selectedCountry = value;
                OnPropertyChange("SelectedCountry");
            }
        }

        #endregion

        #region Commands

        public ICommand EmailAdvertisementCommand { get; set; }

        #endregion

        #region Constructors

        public CountryViewModel() : base()
        {
            LoadData(() =>
            {
                context.Countries.Load();
                context.Clients.Load();
            }, () =>
            {
                Clients = context.Clients.Local;
                ClientsView = CollectionViewSource.GetDefaultView(Clients);

                Countries = context.Countries.Local;
            });

            PropertyChanged += RefreshData;

            EmailAdvertisementCommand = new RelayCommand(c => EmailAd(), c => !IsLoading && SelectedClient != null);
        }

        #endregion


        private void RefreshData(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedCountry")
            {
                if (Clients == null)
                    return;

                ClientsView.Filter = c =>
                {
                    var client = c as Client;

                    if (client != null && client.birthDate.HasValue)
                        return client.Address.idCountry == SelectedCountry.idCountry;

                    return false;
                };
            }

        }

        private void EmailAd()
        {
            var viewModel = new EmailViewModel(context);
            viewModel.Client = SelectedClient;
            viewModel.EmailBody = "Hola " + SelectedClient.name + "!";
            viewModel.EmailBody += "\nVen a Ollivander's a ver las novedades!";
            viewModel.EmailBody += "\nLos siguientes productos son especialmente populares en " + SelectedClient.Address.Country.name;
            viewModel.EmailBody += "\n\n";
            var productos = GetTopProductsString(SelectedClient.Address.Country);
            viewModel.EmailBody += productos;
            viewModel.EmailBody += "\n Atentamente,\n Ollivander's.";
            PresentEmailView(viewModel);
        }

        private string GetTopProductsString(Country c)
        {
            var query = from sp in context.StockProducts
                        where sp.SaleOrder != null && sp.SaleOrder.Invoice.Client.Address.idCountry == c.idCountry
                        group sp by sp.Product into g
                        orderby g.Count()
                        select new { Product = g.Key, Count = g.Count() };

            var top3 = query.Take(3);

            var list = "";
            foreach (var product in top3)
            {
                list += product.Product.description + "\n";
            }

            return list;
        }

        #region Presenter

        private void PresentEmailView(EmailViewModel viewModel)
        {
            var window = new EmailView();
            window.mainGrid.DataContext = viewModel;
            window.Show();
        }

        #endregion

    }
}
