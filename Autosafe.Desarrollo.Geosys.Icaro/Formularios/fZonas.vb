Imports System.IO
Public Class fZonas
    Private _Datos As DataSet
    Private _Archivo As String

    Sub New(ByVal Datos As DataSet,
       ByVal Archivo As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _Datos = Datos
        _Archivo = Archivo
    End Sub

    Public Sub GenerarArchivo()
        Try
            Try
                If File.Exists(_Archivo) Then
                    File.Delete(_Archivo)
                End If
            Catch ex As Exception

            End Try

            With grdDatos
                .DataSource = Nothing
                .DataSource = _Datos.Tables(0)

                .ExportToXlsx(_Archivo)
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub fGrid_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim FormatoNumeros As New Globalization.NumberFormatInfo
            Dim FormatoFecha As New Globalization.DateTimeFormatInfo

            With FormatoNumeros
                .CurrencyDecimalDigits = 2
                .CurrencyDecimalSeparator = "."
                .CurrencyGroupSeparator = ","
            End With

            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat = FormatoNumeros
        Catch ex As Exception

        End Try
    End Sub
End Class