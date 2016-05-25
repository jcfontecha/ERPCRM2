using MVVM_ERP.Models;
using MVVM_ERP.ViewModels.Commands;
using MVVM_ERP.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MVVM_ERP.ViewModels
{
    class ClientsViewModel : ERPViewModel
    {
        #region Properties

        private ObservableCollection<Client> clients;
        /// <summary>
        /// Returns the Clients ObservableCollection to bind in XAML.
        /// </summary>
        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChange("Clients");
            }
        }

        private int selectedClientIndex = -1;
        /// <summary>
        /// Returns the index of the Selected Client
        /// </summary>
        public int SelectedClientIndex
        {
            get { return selectedClientIndex; }
            set
            {
                selectedClientIndex = value;
                OnPropertyChange("SelectedClientIndex");
            }
        }

        private Client selectedClient;
        /// <summary>
        /// The currently selected Client.
        /// </summary>
        public Client SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                OnPropertyChange("SelectedClient");
            }
        }


        #endregion // Properties

        #region Commands

        /// <summary>
        /// Command to edit the selected Client
        /// </summary>
        public ICommand EditSelectedClientCommand { get; set; }

        /// <summary>
        /// Command to create a new Client
        /// </summary>
        public ICommand NewClientCommand { get; set; }

        /// <summary>
        /// Command to delete the selected Client.
        /// </summary>
        public ICommand DeleteClientCommand { get; set; }

        #endregion // Commands

        /// <summary>
        ///  Instantiates a new instance of the clas ClientsViewModel
        /// </summary>
        public ClientsViewModel()
        {
            InitializeCommands();

            ToLoadAction = () => { context.Clients.Load(); };
            AfterLoadingAction = () => 
            {
                Clients = context.Clients.Local;
                ItemsView = CollectionViewSource.GetDefaultView(Clients);
            };

            SearchFilter = c =>
            {
                var client = c as Client;

                if (client.name.Contains(SearchString))
                    return true;

                if (client.lastName.Contains(SearchString))
                    return true;

                if (client.email.Contains(SearchString))
                    return true;

                return false;
            };

            LoadData();
        }

        /// <summary>
        /// Intializes the commands in the class
        /// </summary>
        private void InitializeCommands()
        {
            EditSelectedClientCommand = new RelayCommand(EditClient, (x) => { return !IsLoading && SelectedClient != null; });
            NewClientCommand = new RelayCommand(NewClient, (x) => { return !IsLoading; });
            DeleteClientCommand = new RelayCommand(DeleteClient, (x) => { return SelectedClient != null; });
        }

        /// <summary>
        /// Creates and displays a new EditClientView window. Sets its Client ID as well.
        /// </summary>
        /// <param name="parameter"></param>
        private void EditClient(object parameter)
        {
            var id = SelectedClient.idClient;
            var editClientViewModel = new EditClientViewModel(id, context);
            editClientViewModel.ClientEdited += OnClientEdited;

            PresentEditClient(editClientViewModel);
        }

        /// <summary>
        /// Adds a new Client to the model and wires it to the EditClientViewModel.
        /// </summary>
        /// <param name="parameter"></param>
        private void NewClient(object parameter)
        {
            var editClientViewModel = new EditClientViewModel(context);
            editClientViewModel.ClientEdited += OnClientEdited;

            PresentEditClient(editClientViewModel);
        }

        /// <summary>
        /// Prompts the user and then deletes the Client from the context (Doesn't save)
        /// </summary>
        /// <param name="parameter"></param>
        private void DeleteClient(object parameter)
        {
            var dialog = MessageBox.Show("Are you sure you want to delete the selected client?", "Client deletion", MessageBoxButton.OKCancel);

            if (dialog == MessageBoxResult.OK)
                context.Clients.Remove(SelectedClient);
        }

        /// <summary>
        /// Recieves the edited Client and adds it to the Clients list in the context.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        private void OnClientEdited(object source, EditClientEventArgs args)
        {
            
        }

        #region Interaction

        /// <summary>
        /// Creates and presents a new instance of the EditClientView class with its View Model setup.
        /// </summary>
        /// <param name="viewModel"></param>
        private void PresentEditClient(EditClientViewModel viewModel)
        {
            var window = new EditClientView();
            window.mainGrid.DataContext = viewModel;
            window.Show();
        }

        #endregion
    }
}
