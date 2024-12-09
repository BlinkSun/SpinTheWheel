Namespace Converters

    ''' <summary>
    ''' A value converter that divides a given numeric input by a specified divisor.
    ''' </summary>
    Public Class DivisionConverter
        Implements IValueConverter

        ''' <summary>
        ''' Gets or sets the divisor used in the division operation.
        ''' Default value is 1.
        ''' </summary>
        Public Property Divisor As Double = 1

        ''' <summary>
        ''' Converts a numeric value by dividing it by the specified divisor.
        ''' </summary>
        ''' <param name="value">The value to be converted.</param>
        ''' <param name="targetType">The target type of the conversion. This parameter is not used.</param>
        ''' <param name="parameter">Additional parameter for the conversion. This parameter is not used.</param>
        ''' <param name="culture">The culture to use in the conversion. This parameter is not used.</param>
        ''' <returns>The result of dividing the input value by the divisor, or <see cref="DependencyProperty.UnsetValue"/> if the input is not a valid number.</returns>
        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            Dim input As Double
            If Double.TryParse(value.ToString(), input) Then
                Return input / Divisor
            End If
            Return DependencyProperty.UnsetValue
        End Function

        ''' <summary>
        ''' Converts a value back to the original value. This method is not implemented.
        ''' </summary>
        ''' <param name="value">The value to be converted back. This parameter is not used.</param>
        ''' <param name="targetType">The target type of the conversion. This parameter is not used.</param>
        ''' <param name="parameter">Additional parameter for the conversion. This parameter is not used.</param>
        ''' <param name="culture">The culture to use in the conversion. This parameter is not used.</param>
        ''' <returns>Not supported.</returns>
        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException("ConvertBack is not implemented for DivisionConverter.")
        End Function
    End Class

End Namespace
