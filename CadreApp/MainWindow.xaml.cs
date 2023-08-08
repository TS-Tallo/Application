using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Timers;
using System.Windows.Threading;
using CadreApp.Context;
using CadreApp.TableEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CadreApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _refreshTimer;
        public ObservableCollection<Trip_Active> TripActives { get; set; } = new ObservableCollection<Trip_Active>();

        public MainWindow()
        {
            InitializeComponent();
            TableData();
            tripDataGrid.ItemsSource = TripActives;

            _refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            _refreshTimer.Tick += (sender, e) => TableData();
            _refreshTimer.Start();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _refreshTimer.Stop();
        }

        public void TableData()
        {
            using (var db = new MyDbContext())
            {
                var trips = db.Trip_Actives.ToList();

                // ... (Here you might compare and only update if there are changes)

                TripActives.Clear();
                foreach (var trip in trips)
                {
                    TripActives.Add(trip);
                }
            }
        }

        public void AddNewEntry(Trip_Active newTrip)
        {
            using (var db = new MyDbContext())
            {
                db.Trip_Actives.Add(newTrip);
                db.SaveChanges();
            }
            TripActives.Add(newTrip);
        }

        public void UpdateEntry(Trip_Active updatedTrip)
        {
            using (var db = new MyDbContext())
            {
                db.Trip_Actives.Update(updatedTrip);
                db.SaveChanges();
            }

            // Assuming Trip_Active has a suitable Equals() override
            var existing = TripActives.FirstOrDefault(t => t.Equals(updatedTrip));
            if (existing != null)
            {
                int index = TripActives.IndexOf(existing);
                TripActives[index] = updatedTrip; // Refresh the view item
            }
        }

        public void DeleteEntry(Trip_Active tripToDelete)
        {
            using (var db = new MyDbContext())
            {
                db.Trip_Actives.Remove(tripToDelete);
                db.SaveChanges();
            }
            TripActives.Remove(tripToDelete);
        }
        
        private void OpenAddSolderWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Add_Soldier addSoldier = new Add_Soldier();
            addSoldier.ShowDialog(); // Use this so they enter data in.
        }
    }
}