Namespace Converters

    Public Class DivisionConverter
        Implements IValueConverter

        Public Property Divisor As Double = 1

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            Dim input As Double
            If Double.TryParse(value.ToString(), input) Then
                Return input / Divisor
            End If
            Return DependencyProperty.UnsetValue
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace