using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using CadreApp.Solder;
using Wiry.Base32;

namespace CadreApp;

public partial class Add_Soldier : Window
{
    public Add_Soldier()
    {
        InitializeComponent();
        var test_string =
            "M1G412AJ01T3C2QOJoseph               Wacha                     B54FUS AA00PVT   ME012BCN7BDPETOU78K";
        ProcessCACData(test_string);
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
        // var reader = new BarcodeReader();
        // var result = reader.Decode(CacData.Text);

        if (CacData != null)
        {
            // Extract data from the barcode
            var parsedData = CacData.Text;

            // Now you can process `parsedData` as you need.
            ProcessCACData(parsedData);
        }
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
    
    private void ProcessCACData(string rawData)
    {
        CACData cacInfo = ParseCACBarcode(rawData);

        // Now you have the parsed data in cacInfo which you can use as needed.
        Console.WriteLine($"First Name: {cacInfo.FirstName}");
        Console.WriteLine($"Last Name: {cacInfo.LastName}");
        Console.WriteLine($"Rank: {cacInfo.Rank}");
        Console.WriteLine($"DODID: {cacInfo.DODID}");
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