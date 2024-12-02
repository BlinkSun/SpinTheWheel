Imports System.Globalization

Namespace Converters
    Public Class SegmentColorConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim index As Integer = value
            Dim colors As Brush() = {Brushes.LightBlue, Brushes.LightCoral, Brushes.LightGreen, Brushes.LightYellow}
            Return colors(index Mod colors.Length)
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function

    End Class

End Namespace