using System.Collections.ObjectModel;

namespace crudapp
{
    public partial class MainPage : ContentPage
    {
        // int count = 0;
        private readonly DatabaseHelper _databaseHelper; // Database helper for CRUD operations
        private ObservableCollection<Student> _students; // Collection to hold students and update UI


        public MainPage()
        {
            InitializeComponent();
            _databaseHelper = new DatabaseHelper(); // Initialize the database helper
            LoadStudents(); // Load students from the database
        }

        private void LoadStudents()
        {
            // Retrieve students from the database and update the collection
            _students = new ObservableCollection<Student>(_databaseHelper.GetAllStudents());
            studentsCollectionView.ItemsSource = _students; // Bind the collection to the UI
        }

        private void OnAddStudentClicked(object sender, EventArgs e)
        {
            // Get the input values
            var name = nameEntry.Text;
            var age = int.TryParse(ageEntry.Text, out var parsedAge) ? parsedAge : 0; // Basic validation

            // Check for valid input
            if (!string.IsNullOrWhiteSpace(name) && age > 0)
            {
                var student = new Student { Name = name, Age = age };
                _databaseHelper.AddStudent(student); // Add the student to the database
                LoadStudents(); // Reload the student list

                // Clear the input fields
                nameEntry.Text = string.Empty;
                ageEntry.Text = string.Empty;
            }
        }

        private async void OnEditStudentInvoked(object sender, EventArgs e)
        {
            var swipeItem = sender as SwipeItem;
            var student = swipeItem.BindingContext as Student;

            // Prompt the user for new values
            var newName = await DisplayPromptAsync("Edit Student", "Enter new name:", initialValue: student.Name);
            var newAge = await DisplayPromptAsync("Edit Student", "Enter new age:", initialValue: student.Age.ToString(), keyboard: Keyboard.Numeric);

            // Update the student if valid input was provided
            if (!string.IsNullOrWhiteSpace(newName) && int.TryParse(newAge, out var age))
            {
                _databaseHelper.UpdateStudent(new Student { Id = student.Id, Name = newName, Age = age });
                LoadStudents(); // Reload the student list
            }
        }

        private void OnDeleteStudentInvoked(object sender, EventArgs e)
        {
            var swipeItem = sender as SwipeItem;
            var student = swipeItem.BindingContext as Student;

            _databaseHelper.DeleteStudent(student.Id); // Delete the student from the database
            LoadStudents(); // Reload the student list
        }
    }
}