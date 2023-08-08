using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using CadreApp.Context;
using CadreApp.Solder;
using CadreApp.TableEntities;
using Wiry.Base32;

namespace CadreApp;

public partial class Modify_Soldier : Window
{
    public Modify_Soldier()
    {
        InitializeComponent();
    }
    
    private void OnPhoneNumberPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Assuming a US phone number format like (555) 555-5555
        // This regex allows only numbers, space, '(', ')', and '-'
        var regex = new Regex("[^0-9()-]+");
        e.Handled = regex.IsMatch(e.Text);
    }
    
    private void OnParseButtonClick(object sender, RoutedEventArgs e)
    {
        var context = new MyDbContext();
        
        Close();
    }
}