using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace crudapp
{
    public class DatabaseHelper
    {
        // Path to the SQLite database
        private readonly string _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "students.db");

        public DatabaseHelper()
        {
            // Establish a connection to the database
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            // Command to create the Students table if it does not exist
            var command = connection.CreateCommand();
            command.CommandText =
            """
        CREATE TABLE IF NOT EXISTS Students (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Age INTEGER NOT NULL
        );
        """;
            command.ExecuteNonQuery(); // Execute the command
        }

        public void AddStudent(Student student)
        {
            // Open a connection to the database
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            // Prepare the INSERT command
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Students (Name, Age) VALUES (@Name, @Age)";
            // Bind parameters to avoid SQL injection
            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Age", student.Age);
            command.ExecuteNonQuery(); // Execute the command
        }

        public List<Student> GetAllStudents()
        {
            var students = new List<Student>(); // List to hold retrieved students

            // Open a connection to the database
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            // Prepare the SELECT command
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, Age FROM Students";

            // Execute the command and process the results
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                students.Add(new Student { Id = reader.GetInt32(0), Name = reader.GetString(1), Age = reader.GetInt32(2) });
            }

            return students; // Return the list of students
        }

        public void UpdateStudent(Student student)
        {
            // Open a connection to the database
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            // Prepare the UPDATE command
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Students SET Name = @Name, Age = @Age WHERE Id = @Id";
            // Bind parameters
            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Age", student.Age);
            command.Parameters.AddWithValue("@Id", student.Id);
            command.ExecuteNonQuery(); // Execute the command
        }

        public void DeleteStudent(int id)
        {
            // Open a connection to the database
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            // Prepare the DELETE command
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Students WHERE Id = @Id";
            // Bind the parameter
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery(); // Execute the command
        }
    }
}
