﻿using System;
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

public partial class Add_Soldier : Window
{
    public Add_Soldier()
    {
        InitializeComponent();
        // ProcessCACData(test_string);
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
        // var reader = new BarcodeReader();
        // var result = reader.Decode(CacData.Text);

        if (CacData.Text.Length >= 88)
        {
            // Extract data from the barcode
            var parsedData = CacData.Text;

            // Now you can process `parsedData` as you need.
            CACData cacInfo = ParseCACBarcode(parsedData);
            var ID = cacInfo.DODID;
            var Rank = cacInfo.Rank;
            var First_Name = cacInfo.FirstName;
            var Last_Name = cacInfo.LastName;
            var pNum = phoneNumber.Text;
            long phoneNum = Int64.Parse(Regex.Replace(pNum, @"[^\d]", ""));
            // Create a new product
            var newSoldier = new soldier()
            {
                ID = (long)ID,
                rank = Rank,
                name_first = First_Name,
                name_last = Last_Name,
                building = buildingNumber.Text,
                company = dCompany.Text,
                platoon = dPlatoon.Text,
                phone_num = phoneNum
            };

            // Add the new product to the context
            context.soldiers.Add(newSoldier);

            // Save changes to the database
            context.SaveChanges();
        }
        Close();
    }

    public CACData ParseCACBarcode(string barcodeData)
    {
        CACData cacData = new CACData();
        cacData.DODID = FromBase32(barcodeData.Substring(1, 7));
        cacData.FirstName = barcodeData.Substring(16, 20).Trim();
        cacData.LastName = barcodeData.Substring(37, 26).Trim();
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