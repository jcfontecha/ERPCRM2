using MVVM_ERP.Models;
using MVVM_ERP.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_ERP.ViewModels
{
    class CRMViewModel : ERPViewModel
    {
        #region Properties

        private ObservableCollection<Client> clients;

        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChange("Clients");
            }
        }

        private Client selectedClient;

        public Client SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                OnPropertyChange("SelectedClient");
            }
        }

        private ICollectionView clientsView;

        public ICollectionView ClientsView
        {
            get { return clientsView; }
            set
            {
                clientsView = value;
                OnPropertyChange("ClientsView");
            }
        }

        #endregion

        #region Commands

        public ICommand ClearCommand { get; set; }

        #endregion

        #region Constructors

        public CRMViewModel()
        {
            ClearCommand = new RelayCommand(c => ClientsView.Filter = null, c => !IsLoading && SelectedClient != null);
        }

        #endregion


    }
}
