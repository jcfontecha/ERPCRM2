//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVVM_ERP.Models
{
    using System;
    using System.Collections.ObjectModel;
    
    public partial class School
    {
        public School()
        {
            this.Clients = new ObservableCollection<Client>();
        }
    
        public int idSchool { get; set; }
        public string name { get; set; }
        public Nullable<int> idCountry { get; set; }
    
        public virtual ObservableCollection<Client> Clients { get; set; }
        public virtual Country Country { get; set; }
    }
}
