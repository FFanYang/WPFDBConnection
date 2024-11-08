using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFDBConnection.DataAccess;
using WPFDBConnection.Model;


//Sinece this a viewmodel class, it depends on the correspoding view (UI page)
//It also need to map to our corresponding models and database query code
//The view-model passes data which is binded from the UI all the way to the backend models
namespace WPFDBConnection.ViewModel
{

    public class StudentViewModel
    {
        //These commands must use the RelayCommand
        //The RelayCommand implements the ICommand interface
        private ICommand _saveCommand;
        private ICommand _resetCommand;
        private ICommand _editCommand;
        private ICommand _deleteCommand;
        private StudentRepository _repository;
        private Student _studentEntity = null;
        public StudentRecord StudentRecord { get; set; }
        public StudentDBOneEntities StudentEntities { get; set; }

        //When the user clicks on the cancel button in the view
        //the binded ResetCommand gets trigerred
        //so, in this case we need the RelayCommand to specify
        //which function inthe bag end must be executed
        //In this case, its the ResetData() function

        //Constructor for the StudentViewModel Class
        //Default constructor with no parameter
        //I set the model table i.e. _studentEntity to point to my student.sc class 
        //_repository obejct to the StudentRepository class so I can call the DB functionality
        //StudentRecord initialization from the StudentRecord class
        //Link this constructor of my viewmodel to the relevant Model and Dataaccess Classes
        public StudentViewModel()
        {
            _studentEntity = new Student();
            _repository = new StudentRepository();
            StudentRecord = new StudentRecord();
            GetAll();
        }

        public ICommand ResetCommand //cancle button will reset command in the mainwindow front-end
        {
            get
            {
                if (_resetCommand == null)
                    _resetCommand = new RelayCommand(param => ResetData(), null);

                return _resetCommand;
            }
        }
        //If the user presses the Cancel button
        //We reset the form binded variables of the particular student record
        //To a 0 if its an integer or an empty string
        public void ResetData() 
        {
            StudentRecord.Name = string.Empty;
            StudentRecord.Id = 0;
            StudentRecord.Address = string.Empty;
            StudentRecord.Contact = string.Empty;
            StudentRecord.Age = 0;
        }
        
        //When the user click save button in the UI page
        //the binded SaveCommand gets triggered
        //so, we need to use the RelayCommand to execute a function in the back-end
        //In this case we write a function called as "SaveData() to be execute"
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(param => SaveData(), null);

                return _saveCommand;
            }
        }
        public void SaveData()
        {
            //StudentRecord is the values of the form data from my front-end
            //This gets saved in the StudentRecord object
            //Then I need to update my Student.cs model with the new data
            //-studentenetity.name is an object of my student.sc model class
            //It just resets the variabled of my model to the ones from the form
            //But its yet to save it to the database
            if (StudentRecord != null)
            {
                _studentEntity.Name = StudentRecord.Name;
                _studentEntity.Age = StudentRecord.Age;
                _studentEntity.Address = StudentRecord.Address;
                _studentEntity.Contact = StudentRecord.Contact;

                try
                {
                    //if this is a new student record which does not have an ID
                    //I.e it does not exist in the database
                    //Then we need to insert that record into the database
                    //So we call the StudentRepository class "AddStudentFunction()"
                    //And pass this new model data there for execution.
                    if (StudentRecord.Id <= 0)
                    {
                        _repository.AddStudent(_studentEntity);
                        MessageBox.Show("New record successfully saved.");
                    }

                    //But if the student exist already in the database, they have an ID
                    //In other word we are updating an existing student and then
                    //clicking the Save button(firing the save command)
                    //Then we need to update the record
                    else
                    {
                        _studentEntity.ID = StudentRecord.Id;
                        _repository.UpdateStudent(_studentEntity);
                        MessageBox.Show("Record successfully updated.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured while saving. " + ex.InnerException);
                }
                finally
                {
                    GetAll();
                    ResetData();
                }
            }
        }

        //When the student decides to select  particular record from the grid-view in the UI front-end,this commandgets fired from the select button
        //Accordingly, the RelayCommand Triggers the Editdata function() to execute
        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(param => EditData((int)param), null);

                return _editCommand;
            }
        }

        public void EditData(int id)
        {
            //The model variable pulls the student record from the database as per the id 
            //by calling the StudentRepository's class Get() function
            var model = _repository.Get(id);
            //Then it display the values  in this model variable
            //on to the form variable
            //Note we are updating anything yet
            //We just select the student to be update/edit
            //and then click save button (SaveCommand)
            StudentRecord.Id = model.ID;
            StudentRecord.Name = model.Name;
            StudentRecord.Age = (int)model.Age;
            StudentRecord.Address = model.Address;
            StudentRecord.Contact = model.Contact;

        }

        //When the user click on the "Delete" Button of a particular student record in the 
        //UI data-grid
        //Then the deleteCommand gets triggered
        //For this, we need the RelayCommand to execute the DeleteStudent() function
        //This DeleteStudent function takes 1 parameter i.e the ID of the particular student recoprd we need to  delete.
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(param => DeleteStudent((int)param), null);

                return _deleteCommand;
            }
        }

        public void DeleteStudent(int id) //delete function
        {
            if (MessageBox.Show("Confirm delete of this record?", "Student", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.RemoveStudent(id);
                    MessageBox.Show("Record successfully deleted.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured while saving. " + ex.InnerException);
                }
                finally
                {
                    GetAll();
                }
            }
        }

        //This GetAll() function's job is to fetch all my StudentRecords from the
        //database using the StuentRepositor Class' GetAll() function which returns a list
        //Then I add the elements of this returned list into
        //The observable Collection called as Student Record
        //Meaning I convert this backend list to a list format which is compatible to be 
        //Displayed on the front-end as a data grid 
        public void GetAll()
        {
            StudentRecord.StudentRecords = new ObservableCollection<StudentRecord>();
            _repository.GetAll().ForEach(data => StudentRecord.StudentRecords.Add(new StudentRecord()
            {
                Id = data.ID,
                Name = data.Name,
                Address = data.Address,
                Age = Convert.ToInt32(data.Age),
                Contact = data.Contact
            }));
        }
    }
}
