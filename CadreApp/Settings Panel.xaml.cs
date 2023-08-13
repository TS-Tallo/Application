using System.IO;
using System.Windows;
using CadreApp.Context;
using Newtonsoft.Json;

namespace CadreApp;

public partial class Settings_Panel : Window
{
    
    public Settings_Panel()
    {
        InitializeComponent();
        
    }
    private void OpenAddCadreWindowButton_Click(object sender, RoutedEventArgs e)
    {
        Add_Cadre addCadrePanel = new Add_Cadre();
        addCadrePanel.ShowDialog(); // Use this so they enter data in.
    }
    
    private void SavePhotoLocationButton_Click(object sender, RoutedEventArgs e)
    {
        string configFile = "config.json";
        string json = File.ReadAllText(configFile);
        Config? config = JsonConvert.DeserializeObject<Config>(json);
        if (config != null)
        {
            config.PhotoLocation = PhotoLocationTextBox.Text;
            string updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(configFile, updatedJson);
        }
        
        //Update textbox to empty
        PhotoLocationTextBox.Text = "";
    }
    
    private void SaveConnectionStringButton_Click(object sender, RoutedEventArgs e)
    {
        string configFile = "config.json";
        string json = File.ReadAllText(configFile);
        Config? config = JsonConvert.DeserializeObject<Config>(json);
        if (config != null)
        {
            config.ConnectionString = ConnectionStringTextBox.Text;
            string updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(configFile, updatedJson);
        }
        
        //Update textbox to empty
        ConnectionStringTextBox.Text = "";
    }
}