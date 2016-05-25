using MVVM_ERP.Models;
using MVVM_ERP.ViewModels.Commands;
using MVVM_ERP.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVM_ERP.ViewModels
{
    class ERPViewModel : BaseViewModel
    {
        #region --Properties

        /// <summary>
        /// The context for the ERP Database
        /// </summary>
        public ERPContext context = new ERPContext();

        private bool isLoading;
        /// <summary>
        /// Returns weather or not the view is loading data.
        /// </summary>
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                isLoading = value;
                OnPropertyChange("IsLoading");
            }
        }

        /// <summary>
        /// Code to be run asynchronously 
        /// </summary>
        protected Action ToLoadAction { get; set; }

        /// <summary>
        /// Code to be run before starting to load the data. Usually UI preparation.
        /// </summary>
        protected Action BeforeLoadingAction { get; set; }

        /// <summary>
        /// Code to be run after loading is complete. Update UI to reflect changes here.
        /// </summary>
        protected Action AfterLoadingAction { get; set; }

        /// <summary>
        /// Code to be run before the context is saved
        /// </summary>
        protected Action BeforeSavingAction { get; set; }

        /// <summary>
        /// Code to be run before the view is dismissed
        /// </summary>
        protected Action BeforeClosingAction { get; set; }

        /// <summary>
        /// Action to be run by the AddNewItemCommand
        /// </summary>
        protected Action AddNewItemAction { get; set; }

        /// <summary>
        /// Action to be run by the EditSelectedItemCommand
        /// </summary>
        protected Action EditSelectedItemAction { get; set; }

        /// <summary>
        /// Action to be run by the DeleteSelectedItemCommand
        /// </summary>
        protected Action DeleteSelectedItemAction { get; set; }

        /// <summary>
        /// Should the user be able to add via the AddNewItemCommand?
        /// </summary>
        protected Predicate<object> ShouldAddNewItem { get; set; }

        /// <summary>
        /// Should the user be able to edit via the EditSelectedItemCommand?
        /// </summary>
        protected Predicate<object> ShouldEditSelectedItem { get; set; }

        /// <summary>
        /// Should the user be able to delete via the DeleteSelectedItemCommand?
        /// </summary>
        protected Predicate<object> ShouldDeleteSelectedItem { get; set; }

        private ICollectionView itemsView;
        /// <summary>
        /// Gets and sets the ItemsView to bind in the CollectionView
        /// </summary>
        public ICollectionView ItemsView
        {
            get { return itemsView; }
            set
            {
                itemsView = value;
                OnPropertyChange("ItemsView");
            }
        }

        private string searchString;
        /// <summary>
        /// String  to bind the search textbox to.
        /// </summary>
        public string SearchString
        {
            get { return searchString; }
            set
            {
                searchString = value;
                OnPropertyChange("SearchString");
            }
        }

        /// <summary>
        /// Predicate to filter search results.
        /// </summary>
        protected Predicate<object> SearchFilter { get; set; }

        #endregion // Properties

        #region --Commands

        /// <summary>
        /// Command to open up the window to edit countries.
        /// </summary>
        public virtual ICommand EditCountriesCommand { get; set; }
        
        /// <summary>
        /// Command to open up the window to edit countries.
        /// </summary>
        public virtual ICommand EditSchoolsCommand { get; set; }

        /// <summary>
        /// Command to save the changes in the context
        /// </summary>
        public virtual ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// Closes the current window
        /// </summary>
        public virtual ICommand CloseCommand { get; set; }

        /// <summary>
        /// Command to add a new item to a list.
        /// Uses the AddAction and ShouldAdd properties
        /// </summary>
        public virtual ICommand AddNewItemCommand { get; set; }

        /// <summary>
        /// Command to edit the selected item.
        /// This uses the EditAction and ShouldEdit properties.
        /// </summary>
        public virtual ICommand EditSelectedItemCommand { get; set; }

        /// <summary>
        /// Command to delete the selected item.
        /// Uses the DeleteAction and ShouldDelete properties
        /// </summary>
        public virtual ICommand DeleteSelectedItemCommand { get; set; }

        #endregion // Commands

        #region -- Constructors

        /// <summary>
        ///  Instantiates a new ERPViewModel class
        /// </summary>
        public ERPViewModel()
        {
            InitializeCommands();
        }

        public ERPViewModel(ERPContext c)
        {
            this.context = c;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            EditCountriesCommand = new EditCountriesCommand(this);

            EditSchoolsCommand = new RelayCommand(c =>
            {
                var window = new SchoolsView();
                window.Show();
            },
            c => !IsLoading);

            SaveChangesCommand = new RelayCommand(
                (x) => { SaveChanges(x as Window); },
                (x) => { return !IsLoading; }
            );

            CloseCommand = new RelayCommand(
                (x) => { CloseWindow(x as Window); },
                (x) => { return true; }
            );

            AddNewItemCommand = new RelayCommand(c => AddNewItemAction(), c => ShouldAddNewItem(c));
            EditSelectedItemCommand = new RelayCommand(c => EditSelectedItemAction(), c => ShouldEditSelectedItem(c));
            DeleteSelectedItemCommand = new RelayCommand(c => DeleteSelectedItemAction(), c => ShouldDeleteSelectedItem(c));

            PropertyChanged += OnSearchChanged;
        }


        #endregion

        private void OnSearchChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SearchString")
            {
                if (string.IsNullOrWhiteSpace(SearchString))
                {
                    if (ItemsView != null)
                        ItemsView.Filter = null;
                }
                else
                {
                    if (ItemsView != null && SearchFilter != null)
                        ItemsView.Filter = SearchFilter;
                }
            }
        }

        private void ClearSearch()
        {
            itemsView.Filter = null;
        }

        /// <summary>
        /// Instantiates and shows a new Edit Countries View
        /// </summary>
        public void ShowEditCountries()
        {
            var editCountriesWindow = new EditCountries();
            editCountriesWindow.Show();
        }

        /// <summary>
        /// Saves changes for the current context and closes the window instance
        /// </summary>
        /// <param name="window">Window instance to be closed</param>
        public void SaveChanges(Window window)
        {
            BeforeSavingAction?.Invoke();
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                MessageBox.Show(e.InnerException.InnerException.Message);

                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    var error = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    MessageBox.Show(error);
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        var error2 = string.Format("- Property: \"{0}\", Error: \"{1}\"",
                //            ve.PropertyName, ve.ErrorMessage);
                //        MessageBox.Show(error2);
                //    }
                //}
            }

            CloseWindow(window);
        }

        /// <summary>
        /// Closes the window. Usually recieved through a command.
        /// </summary>
        /// <param name="window">Window instance to be closed</param>
        public void CloseWindow(Window window)
        {
            BeforeClosingAction?.Invoke();
            window.Close();
        }

        #region Data Loading Methods

        /// <summary>
        /// Sets up the UI to reflect that loading is taking place
        /// </summary>
        private void BeforeLoadingSetup()
        {
            IsLoading = true;
            BeforeLoadingAction?.Invoke();
        }

        /// <summary>
        ///  Sets up the UI tu reflect that the loading is complete.
        /// </summary>
        private void AfterLoadingSetup()
        {
            IsLoading = false;
            AfterLoadingAction?.Invoke();
        }

        /// <summary>
        /// Sets up the worker and starts loading the data asynchronously.
        /// </summary>
        protected void LoadData()
        {
            // Para no cargar la información de la base de datos en el Designer
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }

            BeforeLoadingSetup();
            var worker = new BackgroundWorker();
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Loads the data asynchrounously
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Runs the work that was set.
            try
            {
                ToLoadAction();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error when loading the data.\n" + ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Reflects changes after the loading has completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Runs handler for after-loading mechanics.
            AfterLoadingSetup();
        }

        public void LoadData(Action toLoad, Action afterLoading)
        {
            ToLoadAction = toLoad;
            AfterLoadingAction = afterLoading;
            LoadData();
        }

        #endregion
    }
}
