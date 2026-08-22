using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SalesDemo.Models;

// Doi mau cua "the" so luong ton kho: do neu het hang (0), xanh neu con hang (>0)
public class StockColorConverter : IValueConverter
{
    public static readonly StockColorConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        int soLuong = value is int i ? i : 0;

        if (soLuong <= 0)
            return new SolidColorBrush(Color.Parse("#D32F2F"));   // do - het hang

        return new SolidColorBrush(Color.Parse("#2E7D32"));       // xanh - con hang
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}