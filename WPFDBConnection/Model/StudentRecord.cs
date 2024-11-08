using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDBConnection.ViewModel;

namespace WPFDBConnection.Model
{
    public class StudentRecord : ViewModelBase
    {
        private int _id; //corresponding with student.cs
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private int _age;
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
                OnPropertyChanged("Age");
            }
        }

        private string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        private string _contact;
        public string Contact
        {
            get
            {
                return _contact;
            }
            set
            {
                _contact = value;
                OnPropertyChanged("Contact");
            }
        }
        //Later part of the code below is for display and manipulating
        //the data grid which retrieves the list of students from the database
        //and displays it on the front end view
        private ObservableCollection<StudentRecord> _studentRecords;
        public ObservableCollection<StudentRecord> StudentRecords 
        {
            get
            {
                return _studentRecords;
            }
            set
            {
                _studentRecords = value;
                OnPropertyChanged("StudentRecords");
            }
        }

        private void StudentModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("StudentRecords");
        }
    }
}
