using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CadreApp.Context;
using CadreApp.Solder;
using CadreApp.TableEntities;

namespace CadreApp;

public partial class Self_Solder : Window
{
    private string? trip_ID;
    private MainWindow mainWindow;
    public ObservableCollection<soldier_full> returnSoldiersDataGrid { get; set; } = new ObservableCollection<soldier_full>();
    public Self_Solder(MainWindow mainWindow)
    {
        InitializeComponent();
        returnSoldiers.ItemsSource = returnSoldiersDataGrid;
        this.mainWindow = mainWindow;
    }
    
    
    private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var db = new MyDbContext();
            Console.WriteLine($"ObservableCollection Count: {returnSoldiersDataGrid.Count}");
            Console.WriteLine($"DataGrid Physical Grid Count: {returnSoldiers.Items.Count}");
            if (returnSoldiers.SelectedItem is soldier_full selectedSolder)
            {
                if (CacData.Text.Length >= 88)
                {
                    // Extract data from the barcode
                    var parsedData = CacData.Text;

                    // Now you can process `parsedData` as you need.
                    CACData cacInfo = ParseCACBarcode(parsedData);
                    var soldierId = cacInfo.DODID;
                    var soldierToRemove = returnSoldiersDataGrid.FirstOrDefault(s => s.ID == soldierId);
                    if(db.soldier_fulls.Any(s => s.ID == soldierId)){
                        if (soldierToRemove != null) returnSoldiersDataGrid.Remove(soldierToRemove);
                        Console.WriteLine($"ObservableCollection Count: {returnSoldiersDataGrid.Count}");
                        Console.WriteLine($"DataGrid Physical Grid Count: {returnSoldiers.Items.Count}");
                        CacData.Text = "";
                        if (returnSoldiers.Items.Count == 0)
                        {
                            var trip = db.trips.FirstOrDefault(t => t.ID == trip_ID);
                            // Console.WriteLine($"Trip ID: {trip.ID}");
                            trip.active = 0;
                            trip.time_in = DateTime.Now;
                            trip.cadrein_ID = 0;
                            db.SaveChanges();
                            mainWindow.TableData();
                            Close();
                        }
                    }
                }
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show($"An Exception has occured: {exception}");
        }
    }

    private void OnParseButtonClick(object sender, RoutedEventArgs e)
    {
        try
        {
            //Set DB Context Reference
            var db = new MyDbContext();
        
            //Load Trip from CAC Data
            var cacdata = ParseCACBarcode(CacData.Text);
        
            // Find Trip from DODID of Soldier
            trip_ID = db.solders_trips
                .Where(st => st.solders == cacdata.DODID)
                .OrderByDescending(trip => trip.trip_id)
                .Select(trip => trip.trip_id)
                .FirstOrDefault();

            if (trip_ID != null)
            {
                // Console.WriteLine($"Trip ID: {trip_ID}");
                if (IsTripActive(trip_ID) == false) {MessageBox.Show($"Soldier not actively signed out!"); return;}
                // Retrieve soldier IDs related to the trip
                var soldierIDsInTrip = db.solders_trips
                    .Where(soldier => soldier.trip_id == trip_ID)
                    .Select(soldier => soldier.solders)
                    .ToList();

                // Retrieve soldier information from soldier_fulls based on matching soldier IDs
                var solData = db.soldier_fulls
                    .Where(soldier => soldierIDsInTrip.Contains(soldier.ID))
                    .ToList();

                // Assuming soldiersDataGrid is your DataGrid instance
                foreach (var soldier in solData)
                {
                    returnSoldiersDataGrid.Add(soldier);
                }
            }
            TripButton.Visibility = Visibility.Collapsed; 
            CacData.Text = "";
        }
        catch (Exception exception)
        {
            MessageBox.Show($"An Exception has occured: {exception}");
        }
        
    }
    private bool IsTripActive(string tripId)
    {
        using (var db = new MyDbContext())
        {
            var trip = db.Trip_Actives.FirstOrDefault(t => t.ID == tripId);
            if (trip != null)
            {
                return true; // assuming 'active' is an integer where 1 means active and 0 means inactive.
            }
        }
        return false; // return false if trip is not found or is not active
    }
    
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
