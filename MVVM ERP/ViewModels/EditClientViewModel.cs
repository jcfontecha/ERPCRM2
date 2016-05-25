using MVVM_ERP.Models;
using MVVM_ERP.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_ERP.ViewModels
{
    class EditClientEventArgs : EventArgs
    {
        public Client Client { get; set; }
    }

    class EditClientViewModel : ERPViewModel
    {
        #region Properties

        private int idClient;

        private Client client = new Client();
        /// <summary>
        /// Returns the client that will be bounded to the View
        /// </summary>
        public Client Client
        {
            get { return client; }
            set
            {
                client = value;
                OnPropertyChange("Client");
            }
        }

        private ObservableCollection<School> schools;
        /// <summary>
        /// Returns the list of available schools
        /// </summary>
        public ObservableCollection<School> Schools
        {
            get { return schools; }
            set
            {
                schools = value;
                OnPropertyChange("Schools");
            }
        }

        private ObservableCollection<Country> countries;
        /// <summary>
        /// Returns the list of available countries.
        /// </summary>
        public ObservableCollection<Country> Countries
        {
            get { return countries; }
            set
            {
                countries = value;
                OnPropertyChange("Countries");
            }
        }


        #endregion // Properties

        #region Events

        public delegate void ClientEditedEventHandler(object source, EditClientEventArgs args);

        /// <summary>
        /// Notifies subscribers when a client has been edited.
        /// </summary>
        public event ClientEditedEventHandler ClientEdited;

        /// <summary>
        /// Raises the event when a client is edited.
        /// </summary>
        protected virtual void OnClientEdited()
        {
            ClientEdited?.Invoke(this, new EditClientEventArgs { Client = Client });
        }

        #endregion // Events

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EditClientViewModel class
        /// </summary>
        /// <param name="id">The ID of the specified Client.</param>
        /// <param name="c">The context to be used here.</param>
        public EditClientViewModel(int id, ERPContext c) : base(c) { SetupExistingClient(id); }

        /// <summary>
        /// Initializes a new instance of the EditClientViewModel class
        /// </summary>
        /// <param name="id">The ID of the specified Client.</param>
        public EditClientViewModel(int id) { SetupExistingClient(id); }

        /// <summary>
        /// Initializes a new instance of the EditClientViewModel class with the specified context.
        /// </summary>
        /// <param name="c"></param>
        public EditClientViewModel(ERPContext c) : base(c) { SetupNewClient(); }

        /// <summary>
        /// Creates a new instance of the EditClientViewModel ready to add a new Client.
        /// </summary>
        public EditClientViewModel() { SetupNewClient(); }

        #endregion

        /// <summary>
        /// Gets the specified client from the context and Loads it into the Client property
        /// </summary>
        /// <param name="id"></param>
        private void SetupExistingClient(int id)
        {
            idClient = id;

            ToLoadAction = () =>
            {
                context.Schools.Load();
                context.Countries.Load();
                Client = context.Clients.FirstOrDefault(c => c.idClient == id);
            };

            AfterLoadingAction = () =>
            {
                Schools = context.Schools.Local;
                Countries = context.Countries.Local;
            };

            LoadData();

            BeforeSavingAction = () => { OnClientEdited(); };
        }

        /// <summary>
        /// Creates a new Client and configures the BeforeSaving Action.
        /// </summary>
        private void SetupNewClient()
        {
            Client = new Client();
            Client.Address = new Address();

            ToLoadAction = () => {
                context.Schools.Load();
                context.Countries.Load();
            };

            AfterLoadingAction = () =>
            {
                Schools = context.Schools.Local;
                Countries = context.Countries.Local;
            };

            LoadData();

            // Saving the client. We also notify the event.
            BeforeSavingAction = () =>
            {
                context.Clients.Add(Client);
                OnClientEdited();
            };
        }
    }
}
