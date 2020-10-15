Imports System.IO

Public Class fFinancieraNoTransmisionGrid
    Private _Datos As DataSet
    Private _Archivo As String
    Private _Velocidad As Double

    Sub New(ByVal Datos As DataSet,
            ByVal Archivo As String,
            ByVal Velocidad As Double)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _Datos = Datos
        _Archivo = Archivo
        _Velocidad = Velocidad
    End Sub

    Public Sub GenerarArchivo()
        Try

            Dim vw As New DataView(_Datos.Tables(0))

            Try
                If File.Exists(_Archivo) Then
                    File.Delete(_Archivo)
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            If CInt(_Velocidad) > 0 Then
                With grdDatos
                    .DataSource = Nothing
                    .DataSource = vw

                    .ExportToXlsx(_Archivo)
                End With
            Else
                With grdDatos
                    .DataSource = Nothing
                    .DataSource = _Datos.Tables(0)

                    .ExportToXlsx(_Archivo)
                End With

            End If

            vw = Nothing
        Catch ex As Exception
            Console.WriteLine(ex.Message)
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
            Console.WriteLine(ex.Message)
        End Try
    End Sub
End Class