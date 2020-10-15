Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting

Public Class fChartResumen
    Private _Titulo As String = ""
    Private _Bueno As Integer = 0
    Private _Malo As Integer = 0
    Private _Regular As Integer = 0
    Private _Usuario As String = ""
    Private _Mes As Integer = 0

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        _Titulo = ""
        _Usuario = ""
        _Bueno = 0
        _Malo = 0
        _Regular = 0
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Sub New(ByVal Usuario As String,
            ByVal Titulo As String)

        ' This call is required by the designer.
        InitializeComponent()

        _Titulo = Titulo.ToUpper()
        _Usuario = Usuario.ToUpper()

        ' Add any initialization after the InitializeComponent() call.       
    End Sub

    Private Sub ChartResumen_Load(sender As Object, e As EventArgs)
        Try
            _Bueno = 10
            _Malo = 50
            _Regular = 10

            With oChart
                .Text = _Titulo
                .Titles(0).Text = _Titulo

                .Series(0).Points.Add(New DataPoint(1, _Regular))
                .Series(1).Points.Add(New DataPoint(1, _Regular))
                .Series(2).Points.Add(New DataPoint(1, _Bueno))

                .Series(0).Points.Add(New DataPoint(2, _Bueno))
                .Series(1).Points.Add(New DataPoint(2, _Regular))
                .Series(2).Points.Add(New DataPoint(2, _Bueno))


            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

    Public Function Graficar(ByVal Categoria As String,
                           ByVal Usuario As String,
                           ByVal Titulo As String,
                           ByVal Bueno As Integer,
                           ByVal Regular As Integer,
                           ByVal Malo As Integer,
                           ByVal Bueno1 As Integer,
                           ByVal Regular1 As Integer,
                           ByVal Malo1 As Integer,
                           ByVal Bueno2 As Integer,
                           ByVal Regular2 As Integer,
                           ByVal Regular3 As Integer) As Boolean

        Try
            With oChart
                .Text = Titulo
                .Titles(0).Text = Titulo

                .Series(0).Points.Add(New DataPoint(1, Malo))
                .Series(1).Points.Add(New DataPoint(1, Regular))
                .Series(2).Points.Add(New DataPoint(1, Bueno))
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

    Public Function AddSerie(ByVal Mes As Integer, ByVal Bueno As Integer, ByVal Regular As Integer, ByVal Malo As Integer)
        Try
            With oChart
                .Series(0).Points.Add(New DataPoint(Mes, Malo))
                .Series(1).Points.Add(New DataPoint(Mes, Regular))
                .Series(2).Points.Add(New DataPoint(Mes, Bueno))
            End With
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Function

    Public Function Graficar() As Boolean

        Try
            With oChart
                .Text = _Titulo
                .Titles(0).Text = _Titulo
            End With

            If File.Exists(_Usuario & "_Resumen.jpg") Then
                Try
                    File.Delete(_Usuario & "_Resumen.jpg")
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            End If

            Try
                oChart.SaveImage(_Usuario & "_Resumen.jpg", ChartImageFormat.Jpeg)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return True

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try
    End Function
End Class