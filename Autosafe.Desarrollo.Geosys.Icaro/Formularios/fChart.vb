Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting

Public Class fChart
    Private _Categoria As String = ""
    Private _Titulo As String = ""
    Private _Bueno As Integer = 0
    Private _Malo As Integer = 0
    Private _Regular As Integer = 0
    Private _Usuario As String
    Private _Mes As String = ""

    Sub New()
        InitializeComponent()
        _Titulo = ""
        _Usuario = ""
        _Categoria = ""
        _Bueno = 0
        _Malo = 0
        _Regular = 0
    End Sub

    Sub New(ByVal Categoria As String,
            ByVal Usuario As String,
            ByVal Titulo As String,
            ByVal Bueno As Integer,
            ByVal Regular As Integer,
            ByVal Malo As Integer)
        InitializeComponent()

        _Titulo = Titulo.ToUpper()
        _Usuario = Usuario.ToUpper()
        _Categoria = Categoria.ToUpper()
        _Bueno = Bueno
        _Malo = Malo
        _Regular = Regular
    End Sub

    Public Function Graficar(ByVal Categoria As String,
                        ByVal Usuario As String,
                        ByVal Titulo As String,
                        ByVal Bueno As Integer,
                        ByVal Regular As Integer,
                        ByVal Malo As Integer) As Boolean

        Try
            With oChart
                .Text = Titulo
                .Titles(0).Text = Titulo
                If Bueno = 0 Or Bueno = -1 Then
                    .Series(0).Points(0).IsEmpty = True
                Else
                    .Series(0).Points(0).YValues(0) = Bueno
                End If

                If Regular = 0 Or Regular = -1 Then
                    .Series(0).Points(1).IsEmpty = True
                Else
                    .Series(0).Points(1).YValues(0) = Regular
                End If

                If Malo = 0 Or Malo = -1 Then
                    .Series(0).Points(2).IsEmpty = True
                Else
                    .Series(0).Points(2).YValues(0) = Malo
                End If
            End With

            If File.Exists(String.Format("{0}_{1}.jpg", Usuario, Categoria)) Then
                Try
                    File.Delete(String.Format("{0}_{1}.jpg", Usuario, Categoria))
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            End If

            Try
                oChart.SaveImage(String.Format("{0}_{1}.jpg", Usuario, Categoria), ChartImageFormat.Jpeg)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Graficar() As Boolean

        Try
            With oChart
                .Text = _Titulo
                .Titles(0).Text = _Titulo
                If _Bueno = 0 Or _Bueno = -1 Then
                    .Series(0).Points(0).IsEmpty = True
                Else
                    .Series(0).Points(0).YValues(0) = _Bueno
                End If

                If _Regular = 0 Or _Regular = -1 Then
                    .Series(0).Points(1).IsEmpty = True
                Else
                    .Series(0).Points(1).YValues(0) = _Regular
                End If

                If _Malo = 0 Or _Malo = -1 Then
                    .Series(0).Points(2).IsEmpty = True
                Else
                    .Series(0).Points(2).YValues(0) = _Malo
                End If
            End With

            If File.Exists(String.Format("{0}_{1}.jpg", _Usuario, _Categoria)) Then
                Try
                    File.Delete(String.Format("{0}_{1}.jpg", _Usuario, _Categoria))
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            End If

            Try
                oChart.SaveImage(String.Format("{0}_{1}.jpg", _Usuario, _Categoria), ChartImageFormat.Jpeg)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function
End Class