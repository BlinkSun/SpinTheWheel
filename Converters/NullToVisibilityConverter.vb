﻿Imports System.Globalization

Namespace Converters

    ''' <summary>
    ''' Converts null to Visibility.Collapsed and non-null to Visibility.Visible.
    ''' </summary>
    Public Class NullToVisibilityConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            Return If(value Is Nothing, Visibility.Collapsed, Visibility.Visible)
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function

    End Class

End Namespace