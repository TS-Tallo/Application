using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
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
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Threading;
using CadreApp.Context;
using CadreApp.Solder;
using CadreApp.TableEntities;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Path = System.IO.Path;

namespace CadreApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _refreshTimer;
        public ObservableCollection<Trip_Active> TripActives { get; set; } = new ObservableCollection<Trip_Active>();
        public ObservableCollection<soldier_full> prepSoldiersDataGrid { get; set; } = new ObservableCollection<soldier_full>();
        
        private readonly Config _config;
        public MainWindow()
        {
            
            var db = new MyDbContext();
            InitializeComponent();
            TableData();
            List<string?> userNames;
            userNames = db.accounts.Select(u => u.name).ToList();
            tripDataGrid.ItemsSource = TripActives;
            prepSoldiers.ItemsSource = prepSoldiersDataGrid;
            CadreBox.ItemsSource = userNames;
            _refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(60) };
            _refreshTimer.Tick += (sender, e) => TableData();
            _refreshTimer.Start();
            _config = ConfigurationManager.LoadConfig(); // Load configuration here
            logoImage.Source = new BitmapImage(new Uri(Path.GetFullPath(new Uri(_config.PhotoLocation).LocalPath)));
            Motto.Text = _config.Motto;
        }

        public void TableData()
        {
            using (var db = new MyDbContext())
            {
                var trips = db.Trip_Actives.ToList();
                TripActives.Clear();
                foreach (var trip in trips)
                {
                    TripActives.Add(trip);
                }
            }
        }
        
        ObservableCollection<soldier_full> s_full = new ObservableCollection<soldier_full>();
        private void OpenAddSolderWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Add_Soldier addSoldier = new Add_Soldier();
            addSoldier.ShowDialog(); // Use this so they enter data in.
        }
        private void OpenModifySolderWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Modify_Soldier modifySolder = new Modify_Soldier(this);
            modifySolder.ShowDialog(); // Use this so they enter data in.
        }
        public List<TProperty> ExtractColumn<TItem, TProperty>(DataGrid grid, Func<TItem, TProperty> propertySelector)
        {
            return grid.Items.OfType<TItem>().Select(propertySelector).ToList();
        }
       
        
        
        private void AddtoTripWindowButton_Click(object sender, RoutedEventArgs e)
        {
            
            var db = new MyDbContext();
            long DODID = 0;
            try
            {
                if (dodidAddTrip != null || CACAddTrip != null)
                {
                    if (CACAddTrip?.Text != "")
                    {
                        var cacdata = ParseCACBarcode(CACAddTrip.Text);
                        DODID = cacdata.DODID;
                        CACAddTrip.Text = "";
                    }

                    if (dodidAddTrip?.Text != "")
                    {
                        DODID = long.Parse(dodidAddTrip.Text);
                        dodidAddTrip.Text = "";
                    }

                    bool IsIdUnique(long id)
                    {
                        return prepSoldiersDataGrid.All(s => s.ID != id);
                    }

                    bool IsDODIDThere(long id)
                    {
                        return db.soldiers.Any(s => s.ID == id);
                    }

                    if (IsDODIDThere(DODID) == false)
                    {
                        MessageBox.Show($"DODID not found: {DODID}!");
                        return;
                    }

                    var soldier = db.soldier_fulls.FirstOrDefault(s => s.ID == DODID);
                    if (IsIdUnique(soldier.ID))
                    {
                        prepSoldiersDataGrid.Add(soldier);
                    }

                    dodidAddTrip.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An Exception has Occured: {ex}");
            }
        }
        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (password.Password == "Enter password")
            {
                password.Password = string.Empty;
            }
        }

        private void AddtoTripTableWindowButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new MyDbContext();
                string selectedCadre = CadreBox.SelectedItem?.ToString();
                if (selectedCadre == null) return;
                string enteredPass = password.Password;
                // string pass = "ApexoftheDomain2023";
                // string hashedPassword = BCrypt.Net.BCrypt.HashPassword(pass, 12);
                // Console.WriteLine($"{hashedPassword}");
                var user = db.accounts.SingleOrDefault(u => u.name == selectedCadre);
                var CadreID = user.ID;
                if (user == null)
                {
                    MessageBox.Show("User not found.");
                    return;
                }

                bool isSoldiersinTrip = prepSoldiers.Items.Count > 0;
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(enteredPass, user.password);
                if (isPasswordValid && isSoldiersinTrip)
                {
                    password.Password = "";

                    //Clear Password from Field
                    bool IsNotinTrip(long id)
                    {
                        return prepSoldiersDataGrid.All(s => s.ID == id);
                    }

                    var HighestID = db.trips.Max(trip => trip.ID);
                    int newID = 0;
                    if (HighestID != null)
                    {
                        int highInt;
                        highInt = int.Parse(HighestID);
                        if (highInt >= 0)
                        {
                            newID = highInt + 1;
                        }
                    }

                    if (addDestinations.Text == "")
                    {
                        MessageBox.Show("Missing a Destination!");
                        return;
                    }

                    var newTrip = new trip()
                    {
                        ID = newID.ToString(),
                        cadreout_ID = CadreID,
                        time_out = DateTime.Now,
                        active = 1,
                        destinations = addDestinations.Text
                        //... other properties
                    };
                    db.trips.Add(newTrip);
                    db.SaveChanges();
                    foreach (var soldier in prepSoldiersDataGrid)
                    {
                        var newSoldiersTrip = new solders_trip()
                        {
                            trip_id = newID.ToString(),
                            solders = soldier.ID
                        };
                        db.solders_trips.Add(newSoldiersTrip);
                    }

                    db.SaveChanges();
                    prepSoldiersDataGrid.Clear();
                    addDestinations.Text = "";
                    MessageBox.Show("Trip Added!");
                    TableData();
                }
                else
                {
                    MessageBox.Show("Incorrect password.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An Exception has Occured: {ex}");
            }
        }
        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (prepSoldiers.SelectedItem is soldier_full soldier)
            {
                prepSoldiersDataGrid.Remove(soldier);
            }
        }
        private void SignInRow(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new MyDbContext();
                foreach (var selectedtrip in tripDataGrid.SelectedItems)
                {
                    if (selectedtrip is Trip_Active trips)
                    {
                        string selectedCadre = CadreBox.SelectedItem?.ToString();
                        string enteredPass = password.Password;
                        var user = db.accounts.SingleOrDefault(u => u.name == selectedCadre);
                        if (user == null)
                        {
                            MessageBox.Show("Cadre not found.");
                            return;
                        }

                        if (BCrypt.Net.BCrypt.Verify(enteredPass, user.password))
                        {
                            var trip = db.trips.SingleOrDefault(t => t.ID == trips.ID);
                            trip.active = 0;
                            trip.time_in = DateTime.Now;
                            trip.cadrein_ID = user.ID;
                        }
                    }
                }

                password.Password = "";
                db.SaveChanges();
                TableData();
            } catch (Exception ex)
            {
                MessageBox.Show($"An Exception has Occured: {ex}");  
            }
        }

        private void OpenSettingsPanelWindowButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Set DB for Function
                var db = new MyDbContext();
                string? selectedCadre = CadreBox.SelectedItem?.ToString();
                if (selectedCadre == null) return;
                string enteredPass = password.Password;
                var user = db.accounts.SingleOrDefault(u => u.name == selectedCadre);
                
                //Check to make sure Cadre Selected
                if (user == null) {MessageBox.Show("User not found."); return;}
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(enteredPass, user.password);
                if (isPasswordValid)
                {
                    password.Password = "";
                    Settings_Panel settingsPanel = new Settings_Panel();
                    settingsPanel.ShowDialog(); // Use this so they enter data in.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An Exception has Occured: {ex}");
            }
        }

        private void OpenSelfSolderWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Self_Solder selfSolderPanel = new Self_Solder(this);
            selfSolderPanel.ShowDialog(); // Use this so they enter data in.
        }
        //CAC Data reference
        public CACData ParseCACBarcode(string barcodeData)
        {
            CACData cacData = new CACData();
            cacData.DODID = FromBase32(barcodeData.Substring(1, 7));
            cacData.FirstName = barcodeData.Substring(16, 20).Trim();
            cacData.LastName = barcodeData.Substring(36, 25).Trim();
            cacData.Rank = barcodeData.Substring(74, 6).Trim();
            return cacData;
        }
    
        // Define a character set for Base32. This can vary based on specific requirements.
        private static readonly char[] Base32Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUV".ToCharArray();
        public static int FromBase32(string value)
        {
            value = value.ToUpper();  // Convert to uppercase since our char set is in uppercase
            int result = 0;
        
            for (int i = 0; i < value.Length; i++)
            {
                int charValue = Array.IndexOf(Base32Chars, value[i]);
                if (charValue < 0)
                {
                    throw new ArgumentOutOfRangeException($"Character '{value[i]}' is not a valid Base32 character.");
                }

                result = result * 32 + charValue;
            }
            return result;
        }
    }

}