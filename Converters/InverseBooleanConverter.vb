Imports System.Globalization

Namespace Converters

    ''' <summary>
    ''' Converts a boolean value to its inverse (True to False, False to True).
    ''' </summary>
    Public Class InverseBooleanConverter
        Implements IValueConverter

        ''' <summary>
        ''' Converts a boolean value to its inverse.
        ''' </summary>
        ''' <param name="value">The boolean value to invert.</param>
        ''' <param name="targetType">The target property type (ignored).</param>
        ''' <param name="parameter">Optional parameter (ignored).</param>
        ''' <param name="culture">The culture to use (ignored).</param>
        ''' <returns>The inverted boolean value.</returns>
        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            If TypeOf value Is Boolean Then
                Return Not CType(value, Boolean)
            End If
            Return DependencyProperty.UnsetValue
        End Function

        ''' <summary>
        ''' Converts back (not implemented for this converter).
        ''' </summary>
        ''' <param name="value">The value to convert back.</param>
        ''' <param name="targetType">The target property type (ignored).</param>
        ''' <param name="parameter">Optional parameter (ignored).</param>
        ''' <param name="culture">The culture to use (ignored).</param>
        ''' <returns>Not supported.</returns>
        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException("ConvertBack is not implemented for InverseBooleanConverter.")
        End Function

    End Class

End Namespace
