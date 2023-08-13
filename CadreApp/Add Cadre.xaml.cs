using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CadreApp.Context;
using CadreApp.TableEntities;

namespace CadreApp;

public partial class Add_Cadre : Window
{
    public Add_Cadre()
    {
        InitializeComponent();
    }
    
    private void OnParseButtonClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var context = new MyDbContext();
            long id;
            //Parse ID to System
            if (long.TryParse(ID.Text, out long _id))
            {
                id = _id;
            }
            else
            {
                return;
            }

            var rank = dRank.Text;
            var last_name = lastName.Text;

            // Create a new product
            var newSoldier = new soldier()
            {
                ID = id,
                rank = rank,
                name_first = "Cadre",
                name_last = last_name,
                building = "N/A",
                company = dCompany.Text,
                platoon = dPlatoon.Text,
                phone_num = 0
            };

            // Add the new soldier to the context
            context.soldiers.Add(newSoldier);

            // Add Cadre to Soldier DB
            context.SaveChanges();
            // string pass = "ApexoftheDomain2023";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password.Password, 12);
            Console.WriteLine($"{hashedPassword}");
            var account = new account()
            {
                ID = id,
                name = $"{rank} {last_name}",
                password = hashedPassword
            };
            //Add the new Account to the context
            context.accounts.Add(account);
            // Add Cadre to Account DB
            context.SaveChanges();
            
            
            Close();
        } catch (Exception ex)
        {
            MessageBox.Show($"An Error Has Occured: {ex.Message}");
        }
    }
}