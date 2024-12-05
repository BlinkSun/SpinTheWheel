Imports System.Globalization

Namespace Converters

    ''' <summary>
    ''' Converts null to Visibility.Collapsed and non-null to Visibility.Visible.
    ''' </summary>
    Public Class NullToVisibilityConverter
        Implements IValueConverter

        ''' <summary>
        ''' Converts a value to its Visibility.
        ''' </summary>
        ''' <param name="value">The value to convert.</param>
        ''' <param name="targetType">The target property type (ignored).</param>
        ''' <param name="parameter">Optional parameter (ignored).</param>
        ''' <param name="culture">The culture to use (ignored).</param>
        ''' <returns>The Visibility value.</returns>
        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            Return If(value Is Nothing, Visibility.Collapsed, Visibility.Visible)
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
            Throw New NotImplementedException("ConvertBack is not implemented for NullToVisibilityConverter.")
        End Function

    End Class

    ''' <summary>
    ''' Converts null to Visibility.Collapsed and non-null to Visibility.Visible.
    ''' </summary>
    Public Class InverseBooleanToVisibilityConverter
        Implements IValueConverter

        ''' <summary>
        ''' Converts a value to its Visibility.
        ''' </summary>
        ''' <param name="value">The value to convert.</param>
        ''' <param name="targetType">The target property type (ignored).</param>
        ''' <param name="parameter">Optional parameter (ignored).</param>
        ''' <param name="culture">The culture to use (ignored).</param>
        ''' <returns>The Visibility value.</returns>
        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            If TypeOf value Is Boolean Then
                Return If(CType(value, Boolean), Visibility.Collapsed, Visibility.Visible)
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
            Throw New NotImplementedException("ConvertBack is not implemented for NullToVisibilityConverter.")
        End Function

    End Class

End Namespace