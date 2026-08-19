using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SalesDemo.Models;

// Doi mau cua "the" so luong ton kho: do neu sap het, cam neu it, xanh neu con nhieu
public class StockColorConverter : IValueConverter
{
    public static readonly StockColorConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        int soLuong = value is int i ? i : 0;
        if (soLuong <= 5) return new SolidColorBrush(Color.Parse("#D32F2F"));   // do - sap het
        if (soLuong <= 20) return new SolidColorBrush(Color.Parse("#F57C00")); // cam - it
        return new SolidColorBrush(Color.Parse("#2E7D32"));                    // xanh - con nhieu
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}