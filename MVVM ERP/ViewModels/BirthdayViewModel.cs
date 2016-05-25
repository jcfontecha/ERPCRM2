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
    class BirthdayViewModel : CRMViewModel
    {
        #region Properties

        private DateTime selectedDate = DateTime.Now;

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                OnPropertyChange("SelectedDate");
            }
        }

        #endregion

        #region Commands

        public ICommand EmailCongratulationsCommand { get; set; }

        #endregion

        #region Constructors

        public BirthdayViewModel() : base()
        {
            LoadData(() => context.Clients.Load(), () =>
            {
                Clients = context.Clients.Local;
                ClientsView = CollectionViewSource.GetDefaultView(Clients);
            });

            PropertyChanged += RefreshData;

            EmailCongratulationsCommand = new RelayCommand(c => EmailCongrats(), c => !IsLoading && SelectedClient != null);
        }

        #endregion

        private void RefreshData(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDate")
            {
                if (Clients == null)
                    return;

                ClientsView.Filter = c =>
                {
                    var client = c as Client;

                    if (client != null && client.birthDate.HasValue)
                        return client.birthDate.Value.Day == SelectedDate.Day
                                && client.birthDate.Value.Month == SelectedDate.Month;

                    return false;
                };
            }
        }

        private void EmailCongrats()
        {
            var viewModel = new EmailViewModel(context);
            viewModel.Client = SelectedClient;
            viewModel.EmailBody = "Feliz cumpleaños " + SelectedClient.name + "!";
            viewModel.EmailBody += "\nVen a festejar con nosotros.";
            viewModel.EmailBody += "\nRecuerda comprar muchas varitas.";
            viewModel.EmailBody += "\n\n Atentamente,\n Ollivander's.";

            PresentEmailView(viewModel);
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