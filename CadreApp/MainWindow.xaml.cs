using System;
using System.Collections.Generic;
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
using CadreApp.Context;
using CadreApp.models;
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
        public MainWindow()
        {
            InitializeComponent();
            TableData();
            
        }

        public void TableData()
        {
            MyDbContext context = new MyDbContext();
            var soldiersTable = context.soldiers;
            var soldiers_tripTable = context.solders_trips;
            var account = context.accounts;
            var activeTrips = context.trips.Where(trip => trip.active == 1).ToList();
            var activeIDs = activeTrips.Select(a => a.ID);
            
            tripDataGrid.ItemsSource = activeTrips;
        }
    }
}