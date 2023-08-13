using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CadreApp.Context;
using CadreApp.Solder;
using CadreApp.TableEntities;
using Wiry.Base32;

namespace CadreApp;

public partial class Modify_Soldier : Window
{
    private MainWindow mainWindow;
    public Modify_Soldier(MainWindow mainWindow)
    {
        InitializeComponent();
        this.mainWindow = mainWindow;
    }
    
    private void OnPhoneNumberPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Assuming a US phone number format like (555) 555-5555
        // This regex allows only numbers, space, '(', ')', and '-'
        var regex = new Regex("[^0-9()-]+");
        e.Handled = regex.IsMatch(e.Text);
    }
    private void OnLookupButtonClick(object sender, RoutedEventArgs e)
    {
        var context = new MyDbContext();
        var ID = lookupSoldier.Text;
        dRank.SelectedIndex = 0; 
        dCompany.SelectedIndex = 0;
        dPlatoon.SelectedIndex = 0;
        if (ID != ""){
            var DODID = long.Parse(ID);
            var soldier = context.soldiers.Find(DODID);
            lastName.Text = soldier.name_last;
            firstName.Text = soldier.name_first;
            phoneNumber.Text = soldier.phone_num.ToString();
            buildingNumber.Text = soldier.building;
            if (soldier.rank != null) SelectRank(soldier.rank);
            if (soldier.platoon != null) SelectPlatoon(soldier.platoon);
            if (soldier.company != null) SelectCompany(soldier.company);
        }
    }
    private void OnUpdateButtonClick(object sender, RoutedEventArgs e)
    {
        var context = new MyDbContext();
        var DODID = long.Parse(lookupSoldier.Text);
        var soldierToUpdate = context.soldiers.FirstOrDefault(s => s.ID == DODID);

        if (soldierToUpdate != null)
        {
            // Update properties of the fetched soldier record
            soldierToUpdate.rank = dRank.Text;
            soldierToUpdate.name_first = firstName.Text;
            soldierToUpdate.name_last = lastName.Text;
            soldierToUpdate.building = buildingNumber.Text;
            soldierToUpdate.company = dCompany.Text;
            soldierToUpdate.platoon = dPlatoon.Text;
            soldierToUpdate.phone_num = Int64.Parse(Regex.Replace(phoneNumber.Text, @"[^\d]", ""));
            
            // Save changes to the database
            context.SaveChanges();
        }
        mainWindow.TableData();
        Close();
    }
    
    private void SelectRank(string rankToSelect)
    {
        foreach (ComboBoxItem item in dRank.Items)
        {
            if (item.Content.ToString() == rankToSelect)
            {
                dRank.SelectedItem = item;  // Set the found item as the selected item.
                return;
            }
        }

        // Optionally handle the case where the rank was not found.
        MessageBox.Show($"The rank '{rankToSelect}' was not found in the ComboBox.");
    }
    
    private void SelectPlatoon(string pltToSelect)
    {
        foreach (ComboBoxItem item in dPlatoon.Items)
        {
            if (item.Content.ToString() == pltToSelect)
            {
                dPlatoon.SelectedItem = item;  // Set the found item as the selected item.
                return;
            }
        }

        // Optionally handle the case where the rank was not found.
        MessageBox.Show($"The rank '{pltToSelect}' was not found in the ComboBox.");
    }
    
    private void SelectCompany(string coToSelect)
    {
        foreach (ComboBoxItem item in dCompany.Items)
        {
            if (item.Content.ToString() == coToSelect)
            {
                dCompany.SelectedItem = item;  // Set the found item as the selected item.
                return;
            }
        }

        // Optionally handle the case where the rank was not found.
        MessageBox.Show($"The rank '{coToSelect}' was not found in the ComboBox.");
    }
}