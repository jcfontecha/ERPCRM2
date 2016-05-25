using MVVM_ERP.Models;
using MVVM_ERP.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVM_ERP.ViewModels
{
    class EmailViewModel : ERPViewModel
    {
        private Client client;

        public Client Client
        {
            get { return client; }
            set
            {
                client = value;
                OnPropertyChange("Client");
            }
        }

        private string emailBody;

        public string EmailBody
        {
            get { return emailBody; }
            set
            {
                emailBody = value;
                OnPropertyChange("EmailBody");
            }
        }

        public ICommand SendEmailCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public EmailViewModel()
        {
            CommonInitialization();
        }

        public EmailViewModel(ERPContext c) : base(c)
        {
            CommonInitialization();
        }

        private void CommonInitialization()
        {
            SendEmailCommand = new RelayCommand(c => MessageBox.Show("Se debería enviar un correo"));
            CancelCommand = new RelayCommand(c => (c as Window).Close());
        }

    }
}
