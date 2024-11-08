using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDBConnection.Model;

namespace WPFDBConnection.DataAccess
{

    //Need this class to update, add, delete, retrieve student info
    //from the backend database
    //This class will depend on my Student models -> Student.cs (map directly to the table)
    //Also depend on StudentRecord.cs which is the glue between the view and the model
    //to indicate the changes of properties that happened in the views (front-end UI)
    public class StudentRepository
    {
        private StudentDBOneEntities studentContext = null;


        //Constructor to point to the database in question and
        //to open the database connection
        //The variable "StudentDBOneEntities" comes from the Connection string in App.config
        public StudentRepository()
        {
            studentContext = new StudentDBOneEntities(); //this one should same like connectionString name
        }
        
        
        //References the Student.cs from my Model folder
        //This function fetches a particular student by their id as the parameter
        //The id parameter is passed from the front-end view.
        public Student Get(int id) //get details on particular students.
        {
            //This is same as saying "select * from Students where ID = 'id'"
            //The find (id) is like saying where ID='id'
            return studentContext.Students.Find(id); // return the student info
        }

        // If i want to retrieve the list of all students 
        //the use the following function
        //Returns all students from the db
        //Converts this into a list -> which will be passed to the view.
        //by data binding with the view model class.

        public List<Student> GetAll()
        {
            return studentContext.Students.ToList(); //LINQ QUERY
        }

        //Insert a new student object as a new record inside the student table 
        //"INSERT INTO QUERY" 
        public void AddStudent(Student student)
        {
            //The student object comes from the front-end form
            //eg: student.name = name value from the form's name field
            //eg: student.contect = contact value from the form's name field
            //Student student ={name: "YJ", contact:240247824, address: "Symonds street"}
            if (student != null)
            {
                //The LINQ Add() function maps to the "insert into" query
                studentContext.Students.Add(student);//LINQ -> The add() function of your list 
                studentContext.SaveChanges(); //make sure using the savechange, otherwise student will not save the change.
            }
        }

        //Updating/editing the record of a particular student

        public void UpdateStudent(Student student)
        {
            //The student parameter is the updated student object from the front-end
            //but this object is yet to be updated in the database
            //For this, i need to get the corresponding record from the database
            //Pull that record as the studentFind object in my code
            //If that studentFind object os in my database, I update it 
            //by passing the values of the student object which is coming from my front-end 
            //then I save the changes
            var studentFind = this.Get(student.ID);
            if (studentFind != null)
            {
                studentFind.Name = student.Name;
                studentFind.Contact = student.Contact;
                studentFind.Age = student.Age;
                studentFind.Address = student.Address;
                studentContext.SaveChanges();
            }
        }

        //Deleting a particular student 
        //The id of the student os passed from the front-end view
        //We just check if a student with this id still exists in the database table
        //If so, then delete it using LINQ'S Remove() function of the list and specify the id 

        public void RemoveStudent(int id)
        {
            var studObj = studentContext.Students.Find(id);
            if (studObj != null)
            {
                //Same as saying delete from students where id = id
                studentContext.Students.Remove(studObj);
                studentContext.SaveChanges();
            }
        }
    }
}
