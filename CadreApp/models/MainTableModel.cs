using CadreApp.Context;
using CadreApp.TableEntities;
using System.Collections.ObjectModel;

namespace CadreApp.models;

public class MainTableModel
{
    private readonly MyDbContext _dbContext;

    public MainTableModel()
    {
        _dbContext = new MyDbContext(); // Initialize your DbContext
        SoldiersData = new ObservableCollection<soldier>(_dbContext.soldiers);
        tripData = new ObservableCollection<trip>(_dbContext.trips);
        activeTrip = new ObservableCollection<Trip_Active>(_dbContext.Trip_Actives);
        accountData = new ObservableCollection<account>(_dbContext.accounts);
        solders_trip = new ObservableCollection<solders_trip>(_dbContext.solders_trips);
        TripLocations = new ObservableCollection<trip_location>(_dbContext.trip_locations);
    }

    public ObservableCollection<soldier> SoldiersData { get; set; }
    public ObservableCollection<trip> tripData { get; set; }
    public ObservableCollection<account> accountData { get; set; }
    public ObservableCollection<Trip_Active> activeTrip { get; set; }
    public ObservableCollection<solders_trip> solders_trip { get; set; }
    public ObservableCollection<trip_location> TripLocations { get; set; }
}