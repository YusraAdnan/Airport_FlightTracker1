using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;


namespace Airport_FlightTracker1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string filePath;
        public MainWindow()
        {
            InitializeComponent();

            /* sets the path to create the text file in the application domain on load of the app */
            filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Flight_Details.txt");
            
        }
        /* A thread is like a worker inside your program that executes instructions one after another 
           Every program has atleast 1 thread (called the main thread) that runs the application 
           We can have multiple threads working at the same time, allowing our program to do several tasks simultaneously 
           Using multiple threads helps increase performance and keeps apps responsive */
        private void btnBoarded_Click(object sender, RoutedEventArgs e) //Synchronous method writes to file 
        {
            string name = txtPassengerName.Text.Trim();
            string seat = txtSeatNumber.Text.Trim();
            string flightInfoLine = $"{name},''{seat}, ''{DateTime.Now:yyyy-MM-dd HH:mm:ss}, Boarded";

            try
            {
                // This is synchronous and will block the UI thread and write to the file created
                File.AppendAllText(filePath, flightInfoLine + Environment.NewLine);

                Thread.Sleep(5000); // simulates the action when it takes long for stuff to be written in real life programs
                MessageBox.Show("Boarded info logged.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error writing to file:" + ex.Message);
            }
        }

        //Async Method Demonstrates how the UI thread is still accessible even when a process is pending
        private async void btnBoardedAsync_Click(object sender, RoutedEventArgs e)
        {
            string name = txtPassengerName.Text.Trim();
            string seat = txtSeatNumber.Text.Trim();

            string line = $"{name},{seat},{DateTime.Now:yyyy-MM-dd HH:mm:ss}, Boarded";

            try
            {
                /* * In real world when the writing to the file takes long the thread on which UI is working on will not freeze. 
                  */

                await Task.Delay(5000);

                // Asynchronously write without blocking UI thread
                /* "await" means process is awaiting response from the other thread that is doing the other task (writing to the file)
                   * But during the time you waiting the UI task is not jammed. */
                await File.AppendAllTextAsync(filePath, line + Environment.NewLine);
                MessageBox.Show("Boarded info logged.");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing to file: {ex.Message}");
            }
        }

    }
}