Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades

Public Class cComportamiento : Inherits cGeneracion

    Private dsComportamiento As DataSet
    Private dsComportamientoDetalle As DataSet

    Private Const Reporte As String = "Reporte de Comportamiento de la Flota"
    Private TotalBuenoR As Integer
    Private TotalRegularR As Integer
    Private TotalMaloR As Integer
    Private TotalBuenoU As Integer = 0
    Private TotalRegularU As Integer
    Private TotalMaloU As Integer
    Private TotalBuenoV As Integer = 0
    Private TotalRegularV As Integer
    Private TotalMaloV As Integer
    Private TotalBuenoRA As Integer
    Private TotalRegularRA As Integer
    Private TotalMaloRA As Integer
    Private TotalBuenoFB As Integer
    Private TotalRegularFB As Integer
    Private TotalMaloFB As Integer
    Private TotalBuenoAB As Integer
    Private TotalRegularAB As Integer
    Private TotalMaloAB As Integer

    Private Hoja As ISheet
    Private HojaGraficos As ISheet

    Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String)
        MyBase.New(TipoReporte, IdUsuario, Intervalo, Parametros)

    End Sub

    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
    'PARA AGREGAR NUEVAS COLUMNAS
    'NOMBRE
    'SECUENCIA
    'HORAINICIO
    'HORAFIN
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20200331
    'PARA AGREGAR NUEVA COLUMNA
    'USUARIO
    '******************************************************************************
    Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String, ByVal Nombre As String, ByVal Secuencia As String, ByVal HoraInicio As String, ByVal HoraFin As String, ByVal Usuario As String)
        MyBase.New(TipoReporte, IdUsuario, Intervalo, Parametros, Nombre, Secuencia, HoraInicio, HoraFin, Usuario)
    End Sub
    '******************************************************************************

    Public Overrides Sub Generar(Datos As Object)
        Console.WriteLine(Datos.usuarioId.ToString() & ": Obteniendo Datos para el Reporte Comportamiento de la Flota")

        Try

            Dim negocio As New ActivoBL()
            Dim obj As New ActivoEN
            obj.baseDatosId = 25
            obj.usuarioId = CInt(IdUsuario)
            obj.entidadId = "0"

            Dim cantidad As Integer = negocio.ListarVehiculosActivos(obj)

            If cantidad > 0 Then
                DatoTipo = Datos.tipoReporte.Split(";")
                DatoActivos = Datos.activos.Split(";")
                DatoEmail = Datos.email.Split(";")

                GenerarReporte(Datos.usuarioId,
                              Datos.intervalo,
                              Datos.parametros)

                If wk.NumberOfSheets > 0 Then
                    Dim file As FileStream = New FileStream(Me.Archivo, FileMode.Create)
                    wk.Write(file)
                    file.Close()
                    file.Dispose()
                    file = Nothing
                    wk.Close()
                    wk = Nothing

                    Try
                        EnviarMail(Datos.email,
                           Archivo,
                           "Adjunto Email con el Reporte Solicitado",
                            Hoja,
                            Reporte,
                            "COM",
                            Datos.usuarioId)
                    Catch ex As Exception
                        ErroresEnvio &= "ERR_MAIL"
                    End Try
                End If
            Else
                GuardarLog(Me.Tipo, "SINVEHICULOS")
                Console.WriteLine(String.Format("El usuario :  {0} no cuenta con vehiculos activos", Me.Usuario))

            End If



            DatoEmail = Nothing
            DatoActivos = Nothing
            DatoTipo = Nothing

            'DBS25.Dispose()
            'DBS25 = Nothing
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Function GenerarReporte(ByVal IdUsuario As Integer,
                               ByVal Intervalo As String,
                               ByVal Parametros As String) As String
        Try
            Dim Columnas As String = ""
            Dim Fila As IRow
            Dim Celda As ICell
            Dim Stl As ICellStyle = wk.CreateCellStyle()
            Dim stlNada As ICellStyle = wk.CreateCellStyle()
            Dim stlNormal As ICellStyle = wk.CreateCellStyle()
            Dim stlBueno As ICellStyle = wk.CreateCellStyle()
            Dim stlRegular As ICellStyle = wk.CreateCellStyle()
            Dim stlMalo As ICellStyle = wk.CreateCellStyle()

            Dim Total3050 As Double = 0
            Dim Total5185 As Double = 0
            Dim Total8600 As Double = 0
            Dim TotalAcel As Double = 0
            Dim TotalFren As Double = 0
            Dim TotalRal As Double = 0
            Dim TotalMin As Double = 0
            Dim TotalRural As Double = 0
            Dim TotalUrbana As Double = 0
            Dim TotalVelocidad As Double = 0

            Dim Reg3050 As Double = 0
            Dim Reg5185 As Double = 0
            Dim Reg8600 As Double = 0

            Dim RegRural As Integer = 0
            Dim RegUrbana As Double = 0
            Dim RegVelocidad As Double = 0
            Dim RegAcel As Double = 0
            Dim RegFren As Double = 0
            Dim RegRal As Double = 0
            Dim RegMin As Double = 0

            Dim FechaInicial As Date = Now()
            Dim FechaFinal As Date = Now()
            Dim dsTotalComportamiento As New dsComportamiento()

            cFinalBueno = 0
            cFinalMalo = 0
            cFinalRegular = 0

            With Stl
                .FillForegroundColor = NPOI.HSSF.Util.HSSFColor.SeaGreen.Index
                .FillPattern = FillPattern.SolidForeground
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
            End With

            With stlBueno
                .FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Green.Index
                .FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.White.Index
                .FillPattern = FillPattern.SolidForeground
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
            End With

            With stlRegular
                .FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightYellow.Index
                .FillPattern = FillPattern.SolidForeground
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
            End With

            With stlMalo
                .FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index
                .FillPattern = FillPattern.SolidForeground
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
            End With

            With stlNormal
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
            End With

            With stlNada
                .FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index
                .FillPattern = FillPattern.SolidForeground
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
            End With

            Dim Archivo As String = "CommportamientoFlota.xls"

            Select Case Intervalo
                Case "MC"
                    FechaInicial = FechaFinal.AddDays(-1 * Now.Date.Day)
                    FechaFinal = Now()
                Case "MA"
                    FechaInicial = FechaInicial.AddMonths(-1)
                    FechaFinal = FechaFinal.AddDays(-1)
                Case "DA"
                    FechaInicial = FechaInicial.AddDays(-1)
                    FechaFinal = FechaFinal.AddDays(-1)
                Case "SA"
                    FechaInicial = FechaInicial.AddDays(-7)
                    FechaFinal = FechaFinal.AddDays(-1)
                Case Else
                    If Intervalo Like "*M" Then
                        FechaInicial = FechaInicial.AddMonths(-1)
                        FechaFinal = FechaFinal.AddDays(-1)
                    Else
                        FechaInicial = FechaInicial.AddDays(0)
                        FechaFinal = FechaFinal.AddDays(0)
                    End If
            End Select

            dsComportamiento = Nothing
            dsComportamiento = New DataSet()

            DbConnection = Nothing
            DbConnection = New SqlConnection(My.Settings.GEOSYSConnectionStringAux)

            If DbConnection.State = ConnectionState.Open Then
                DbConnection.Close()
            End If
            DbConnection.Open()

            DbDataAdapter = Nothing

            dsComportamiento = Nothing
            dsComportamiento = New DataSet()

            Dim tmpVelocidad As DataSet
            tmpVelocidad = Nothing
            tmpVelocidad = New DataSet()


            Dim negocio As New ActivoBL()
            Dim obj As New ActivoEN
            obj.baseDatosId = 25
            obj.usuarioId = CInt(IdUsuario)

            Dim lista As List(Of ActivoEN) = negocio.ListarActivosSinReportar(obj)

            'Dim tActivos = DBS25.spActivosSinReportar(CInt(IdUsuario), "127002", "")
            'For Each tActivo In tActivos
            For Each tActivo In lista
                    Try
                        tmpVelocidad.Clear()
                        tmpVelocidad.AcceptChanges()
                        Console.ForegroundColor = ConsoleColor.Magenta
                        Console.WriteLine("Obteniendo Comportamiento de: " & tActivo.Alias)

                        DbDataAdapter = New SqlDataAdapter("exec spResumenComportamientoUnidades " & IdUsuario.ToString() & ",'" &
                                                                                         Fecha2ANSI(FechaInicial.ToString(), True) & " 00:00:00" & "','" &
                                                                                         Fecha2ANSI(FechaFinal.ToString(), True) & " 23:59:59" & "','" &
                                                                                         tActivo.VID.ToString() & "'", DbConnection)

                        DbDataAdapter.SelectCommand.CommandTimeout = 20000000
                        DbDataAdapter.Fill(dsComportamiento, "ComportamientoFlota")
                        dsComportamiento.AcceptChanges()

                        Total3050 = 0
                        Total5185 = 0
                        Total8600 = 0
                        TotalAcel = 0
                        TotalFren = 0
                        TotalRal = 0
                        TotalMin = 0
                        TotalRal = 0

                        TotalUrbana = 0
                        TotalRural = 0
                        TotalVelocidad = 0

                        RegRural = 0
                        RegVelocidad = 0
                        RegUrbana = 0

                        For Each dRow As DataRow In dsComportamiento.Tables(0).Rows
                            Try
                                Total3050 = 0
                                Total5185 = 0
                                Total8600 = 0
                                TotalAcel = 0
                                TotalFren = 0
                                TotalRal = 0
                                TotalMin = 0
                                TotalRal = 0

                                TotalUrbana = 0
                                TotalRural = 0
                                TotalVelocidad = 0

                                Total3050 += dRow("T3050")
                                Total5185 += dRow("T5185")
                                Total8600 += dRow("T8600")
                                TotalAcel += dRow("TAceleraciones")
                                TotalFren += dRow("TFrenadas")
                                TotalMin += dRow("TMinutos")
                                TotalRal += dRow("TRalenti")

                                Reg3050 = 0
                                Reg5185 = 0
                                Reg8600 = 0

                                Try
                                    Select Case dRow("DentroInicio")
                                        Case "R"
                                            RegRural += 1
                                            TotalRural += dRow("T5185")
                                        Case "U"
                                            RegUrbana += 1
                                            TotalUrbana += dRow("T3050")
                                        Case "V"
                                            RegVelocidad += 1
                                            TotalVelocidad += dRow("T8600")
                                    End Select
                                Catch ex As Exception

                                End Try
                            Catch ex As Exception

                            End Try
                        Next

                        If RegVelocidad = 0 Then
                            TotalVelocidad = -1
                        End If

                        If RegUrbana = 0 Then
                            TotalUrbana = -1
                        End If

                        If RegRural = 0 Then
                            TotalRural = -1
                        End If

                        Try
                            If IsNothing(dsTotalComportamiento.Flota.FindByVehiculos(tActivo.Alias)) Then
                                dsTotalComportamiento.Flota.AddFlotaRow(tActivo.Alias,
                                                TotalRural,
                                                TotalUrbana,
                                                TotalVelocidad,
                                                TotalRal,
                                                TotalFren,
                                                TotalAcel,
                                                "")
                            Else
                            dsTotalComportamiento.Flota.AddFlotaRow(tActivo.alias & " * " & tActivo.chasis.ToString(),
                                                                    TotalRural,
                                                                    TotalUrbana,
                                                                    TotalVelocidad,
                                                                    TotalRal,
                                                                    TotalFren,
                                                                    TotalAcel,
                                                                    "")
                        End If
                        Catch ex As Exception
                            Console.WriteLine(ex.Message)
                        End Try

                        Reg3050 = 0
                        Reg5185 = 0
                        Reg8600 = 0
                        RegAcel = 0
                        RegFren = 0
                        RegRal = 0
                        RegMin = 0
                        RegRal = 0
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try
                Next
                dsTotalComportamiento.AcceptChanges()

            Hoja = Nothing
            Hoja = wk.CreateSheet("Resumen")
            With Hoja
                .Header.Center = "Reporte Comportamiento Flota"
                .CreateRow(0).CreateCell(0).SetCellValue(Reporte)
                .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 8))
            End With

            Fila = Hoja.CreateRow(1)
            Fila.CreateCell(0).SetCellValue("Desde: " & Fecha2ANSI(FechaInicial.ToString(), True) & " 00:00:00")
            Fila = Hoja.CreateRow(2)
            Fila.CreateCell(0).SetCellValue("Hasta: " & Fecha2ANSI(FechaFinal.ToString(), True) & " 23:59:59")
            Hoja.CreateRow(3)
            Fila = Hoja.CreateRow(4)

            For Each dColum As DataColumn In dsTotalComportamiento.Tables(0).Columns
                Celda = Fila.CreateCell(dColum.Ordinal)
                Hoja.AutoSizeColumn(dColum.Ordinal)
                Celda.CellStyle.Alignment = HorizontalAlignment.Center
                Celda.SetCellValue(dColum.ColumnName.ToUpper())
                Celda.CellStyle = stlNada
            Next

            Dim ValorCelda As String = String.Empty
            Dim TotalBueno As Integer = 0
            Dim TotalRegular As Integer = 0
            Dim TotalMalo As Integer = 0
            Dim TotalFinal As Double = 0.0

            Dim tmpBueno As Integer = 0
            Dim tmpRegular As Integer = 0
            Dim tmpMalo As Integer = 0

            hsCalificacionIndividual.Clear()
            For Each oRow As DataRow In dsTotalComportamiento.Tables(0).Rows
                TotalFinal = 0.0
                Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                For Each dColum As DataColumn In dsTotalComportamiento.Tables(0).Columns
                    tmpBueno = 0
                    tmpRegular = 0
                    tmpMalo = 0

                    Celda = Fila.CreateCell(dColum.Ordinal)
                    Hoja.AutoSizeColumn(dColum.Ordinal)

                    If dColum.ColumnName = "Resultado Final" Then
                        If My.Settings.UsarTablaCalificacion Then
                            Dim Valor As String

                            For ic As Integer = 1 To dColum.Ordinal - 1
                                Try
                                    If Fila.Cells(ic).StringCellValue() Like "*Bueno*" Then
                                        tmpBueno += 1
                                        cFinalBueno += 1
                                        TotalFinal += 3.35
                                    End If

                                    If Fila.Cells(ic).StringCellValue() Like "*Regular*" Then
                                        tmpRegular += 1
                                        cFinalRegular += 1
                                        TotalFinal += 1.7
                                    End If

                                    If Fila.Cells(ic).StringCellValue() Like "*Malo*" Then
                                        tmpMalo += 1
                                        cFinalMalo += 1
                                        TotalFinal += 0
                                    End If
                                Catch ex As Exception
                                    TotalFinal += 0
                                End Try
                            Next

                            Try
                                If Not hsCalificacionIndividual.ContainsKey(oRow("Vehiculos")) Then
                                    hsCalificacionIndividual.Add(oRow("Vehiculos"), cFinalBueno.ToString() & "_" & cFinalRegular.ToString() & "_" & cFinalMalo.ToString())
                                End If
                            Catch ex As Exception

                            End Try

                            'Valor = DBS.getCalificacionCriterio(IdUsuario, "COM", 0, TotalBueno, 0)
                            'ValorCelda = Valor
                            'Celda.SetCellValue(Valor)

                            MyBase.CargarParametros()

                            Try
                                Valor = hsCalificacion(Round(TotalFinal, 2))

                                If Valor = "" Then
                                    Valor = "Bueno"
                                End If

                                ValorCelda = Valor
                            Catch ex As Exception
                                Valor = "Bueno"
                            End Try
                            Celda.SetCellValue(Valor)

                            Select Case Valor
                                Case "Bueno"
                                    Celda.CellStyle = stlBueno
                                Case "Regular"
                                    Celda.CellStyle = stlRegular
                                Case "Malo"
                                    Celda.CellStyle = stlMalo
                            End Select

                            Valor = Nothing
                        Else
                            Try
                                Select Case TotalBueno
                                    Case 0
                                        Celda.CellStyle = stlMalo
                                        ValorCelda = "Malo"
                                        Celda.SetCellValue("Malo")
                                    Case 1 To 2
                                        Celda.CellStyle = stlRegular
                                        ValorCelda = "Regular"
                                        Celda.SetCellValue("Regular")
                                    Case >= 3
                                        Celda.CellStyle = stlBueno
                                        ValorCelda = "Bueno"
                                        Celda.SetCellValue("Bueno")
                                End Select
                            Catch ex As Exception
                                Console.Write(ex.Message)
                            End Try
                        End If
                    Else
                        ValorCelda = oRow(dColum.ColumnName).ToString()
                        Celda.SetCellValue(ValorCelda)
                    End If

                    Select Case dColum.Ordinal
                        Case 1, 2, 3
                            If My.Settings.UsarTablaCalificacion Then
                                If CInt(ValorCelda) = -1 Then
                                    Celda.SetCellValue("Bueno (0)")
                                    Celda.CellStyle = stlBueno
                                    TotalBueno += 1
                                Else
                                    Dim Valor As String
                                    Valor = DBS25.getCalificacionCriterio(IdUsuario, "COM", dColum.Ordinal, CInt(ValorCelda), 1)
                                    Celda.SetCellValue(Valor)
                                    Valor = DBS25.getCalificacionCriterio(IdUsuario, "COM", dColum.Ordinal, CInt(ValorCelda), 0)
                                    Select Case Valor
                                        Case "Bueno"
                                            Celda.CellStyle = stlBueno
                                            TotalBueno += 1
                                        Case "Regular"
                                            Celda.CellStyle = stlRegular
                                        Case "Malo"
                                            Celda.CellStyle = stlMalo
                                    End Select

                                    Valor = Nothing
                                End If
                            Else
                                Select Case CInt(ValorCelda)
                                    Case -1
                                        Celda.CellStyle = stlBueno
                                        TotalBueno += 1
                                        Celda.SetCellValue("Bueno (" & ValorCelda & ")")
                                    Case 0 To 20
                                        Celda.CellStyle = stlBueno
                                        TotalBueno += 1
                                        Celda.SetCellValue("Bueno (" & ValorCelda & ")")
                                    Case 21 To 40
                                        Celda.CellStyle = stlRegular
                                        Celda.SetCellValue("Regular (" & ValorCelda & ")")
                                    Case > 41
                                        Celda.CellStyle = stlMalo
                                        Celda.SetCellValue("Malo (" & ValorCelda & ")")
                                End Select
                            End If

                            If My.Settings.UsarTablaCalificacion Then
                                Dim Valor As String
                                Valor = DBS25.getCalificacionCriterio(IdUsuario, "COM", dColum.Ordinal, CInt(ValorCelda), 0)
                                Select Case dColum.Ordinal
                                    Case 1
                                        Select Case Valor
                                            Case "Bueno"
                                                TotalBuenoR += 1
                                            Case "Regular"
                                                TotalRegularR += 1
                                            Case "Malo"
                                                TotalMaloR += 1
                                        End Select
                                    Case 2
                                        Select Case Valor
                                            Case "Bueno"
                                                TotalBuenoU += 1
                                            Case "Regular"
                                                TotalRegularU += 1
                                            Case "Malo"
                                                TotalMaloU += 1
                                        End Select
                                    Case 3
                                        Select Case Valor
                                            Case "Bueno"
                                                TotalBuenoV += 1
                                            Case "Regular"
                                                TotalRegularV += 1
                                            Case "Malo"
                                                TotalMaloV += 1
                                        End Select
                                End Select
                            Else
                                If dColum.Ordinal = 1 Then
                                    Select Case CInt(ValorCelda)
                                        Case 0 To 20
                                            TotalBuenoR += 1
                                        Case 21 To 40
                                            TotalRegularR += 1
                                        Case > 41
                                            TotalMaloR += 1
                                    End Select
                                End If

                                If dColum.Ordinal = 2 Then
                                    Select Case CInt(ValorCelda)
                                        Case 0 To 20
                                            TotalBuenoU += 1
                                        Case 21 To 40
                                            TotalRegularU += 1
                                        Case > 41
                                            TotalMaloU += 1
                                    End Select
                                End If

                                If dColum.Ordinal = 3 Then
                                    Select Case CInt(ValorCelda)
                                        Case 0 To 20
                                            TotalBuenoV += 1
                                        Case 21 To 40
                                            TotalRegularV += 1
                                        Case > 41
                                            TotalMaloV += 1
                                    End Select
                                End If
                            End If
                        Case 4, 5, 6
                            Select Case CInt(ValorCelda)
                                Case 0 To 3
                                    TotalBueno += 1
                                    Celda.CellStyle = stlBueno
                                    Celda.SetCellValue("Bueno (" & ValorCelda & ")")
                                Case 4 To 10
                                    Celda.CellStyle = stlRegular
                                    Celda.SetCellValue("Regular (" & ValorCelda & ")")
                                Case > 10
                                    Celda.CellStyle = stlMalo
                                    Celda.SetCellValue("Malo (" & ValorCelda & ")")
                            End Select

                            If dColum.Ordinal = 4 Then
                                Select Case CInt(ValorCelda)
                                    Case 0 To 3
                                        TotalBuenoRA += 1
                                    Case 4 To 10
                                        TotalRegularRA += 1
                                    Case > 10
                                        TotalMaloRA += 1
                                End Select
                            End If

                            If dColum.Ordinal = 5 Then
                                Select Case CInt(ValorCelda)
                                    Case 0 To 3
                                        TotalBuenoFB += 1
                                    Case 4 To 10
                                        TotalRegularFB += 1
                                    Case > 10
                                        TotalMaloFB += 1
                                End Select
                            End If

                            If dColum.Ordinal = 6 Then
                                Select Case CInt(ValorCelda)
                                    Case 0 To 3
                                        TotalBuenoAB += 1
                                    Case 4 To 10
                                        TotalRegularAB += 1
                                    Case > 10
                                        TotalMaloAB += 1
                                End Select
                            End If
                        Case Else
                            If dColum.ColumnName <> "Resultado Final" Then
                                Celda.CellStyle = stlNormal
                                If IsNumeric(ValorCelda) Then
                                    Select Case CInt(ValorCelda)
                                        Case 0 To 3
                                            TotalBueno += 1
                                            Celda.CellStyle = stlBueno
                                        Case 3 To 6
                                            Celda.CellStyle = stlRegular
                                        Case > 6
                                            Celda.CellStyle = stlMalo
                                    End Select
                                End If
                            End If
                            Celda.SetCellValue(ValorCelda)
                    End Select
                Next
            Next
            ValorCelda = Nothing

            Fila = Nothing
            Celda = Nothing

            Dim vnchart As fChart
            vnchart = New fChart("01", DBS25.getUsuarioxID(IdUsuario.ToString()), "Exc. Vel. Rural", TotalBuenoR, TotalRegularR, TotalMaloR)
            vnchart.Graficar()
            vnchart = Nothing
            vnchart = New fChart("02", DBS25.getUsuarioxID(IdUsuario.ToString()), "Zona Urbana", TotalBuenoU, TotalRegularU, TotalMaloU)
            vnchart.Graficar()
            vnchart = Nothing
            vnchart = New fChart("03", DBS25.getUsuarioxID(IdUsuario.ToString()), "Vias Alta Velocidad", TotalBuenoV, TotalRegularV, TotalMaloV)
            vnchart.Graficar()
            vnchart = Nothing
            vnchart = New fChart("04", DBS25.getUsuarioxID(IdUsuario.ToString()), "Ralenti", TotalBuenoRA, TotalRegularRA, TotalMaloRA)
            vnchart.Graficar()
            vnchart = Nothing
            vnchart = New fChart("05", DBS25.getUsuarioxID(IdUsuario.ToString()), "Frenadas Bruscas", TotalBuenoFB, TotalRegularFB, TotalMaloFB)
            vnchart.Graficar()
            vnchart = Nothing
            vnchart = New fChart("06", DBS25.getUsuarioxID(IdUsuario.ToString()), "Aceleraciones Bruscas", TotalBuenoAB, TotalRegularAB, TotalMaloAB)
            vnchart.Graficar()
            vnchart = Nothing

            HojaGraficos = Nothing
            HojaGraficos = wk.CreateSheet("Graficos")
            With HojaGraficos
                .CreateRow(0).CreateCell(0).SetCellValue("Graficos de Comportamiento de Flota GeoSyS")
                .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 21))
            End With

            Dim patriarch As HSSFPatriarch = DirectCast(HojaGraficos.CreateDrawingPatriarch(), HSSFPatriarch)
            'Grafico 1
            Dim anchor As HSSFClientAnchor
            'anchor = New HSSFClientAnchor(0, 0, 0, 255, 10, 1, 13, 14)
            anchor = New HSSFClientAnchor(0, 0, 0, 255, 0, 2, 13, 4)
            anchor.AnchorType = 2

            Dim picture As HSSFPicture = DirectCast(patriarch.CreatePicture(anchor,
                                                                            CargarImagenXLS(DBS25.getUsuarioxID(IdUsuario.ToString()) & "_01.jpg", wk)), HSSFPicture)
            picture.Resize()
            picture.LineStyle = HSSFPicture.LINESTYLE_DASHDOTGEL
            picture = Nothing

            'Grafico2
            Dim anchor2 As HSSFClientAnchor
            'anchor2 = New HSSFClientAnchor(0, 0, 0, 255, 15, 1, 18, 19)
            anchor2 = New HSSFClientAnchor(0, 0, 0, 255, 6, 2, 18, 10)
            anchor2.AnchorType = 2

            Dim picture2 As HSSFPicture = DirectCast(patriarch.CreatePicture(anchor2,
                                                                            CargarImagenXLS(DBS25.getUsuarioxID(IdUsuario.ToString()) & "_02.jpg", wk)), HSSFPicture)
            picture2.Resize()
            picture2.LineStyle = HSSFPicture.LINESTYLE_DASHDOTGEL
            picture2 = Nothing

            'Grafico3
            Dim anchor3 As HSSFClientAnchor
            'anchor3 = New HSSFClientAnchor(0, 0, 0, 255, 10, 18, 13, 22)
            anchor3 = New HSSFClientAnchor(0, 0, 0, 255, 12, 2, 18, 16)
            anchor3.AnchorType = 2

            Dim picture3 As HSSFPicture = DirectCast(patriarch.CreatePicture(anchor3,
                                                                            CargarImagenXLS(DBS25.getUsuarioxID(IdUsuario.ToString()) & "_03.jpg", wk)), HSSFPicture)
            picture3.Resize()
            picture3.LineStyle = HSSFPicture.LINESTYLE_DASHDOTGEL
            picture3 = Nothing
            '
            'Grafico4
            Dim anchor4 As HSSFClientAnchor
            'anchor4 = New HSSFClientAnchor(0, 0, 0, 255, 15, 18, 18, 22)
            anchor4 = New HSSFClientAnchor(0, 0, 0, 255, 0, 18, 4, 25)
            anchor4.AnchorType = 2

            Dim picture4 As HSSFPicture = DirectCast(patriarch.CreatePicture(anchor4,
                                                                            CargarImagenXLS(DBS25.getUsuarioxID(IdUsuario.ToString()) & "_04.jpg", wk)), HSSFPicture)
            picture4.Resize()
            picture4.LineStyle = HSSFPicture.LINESTYLE_DASHDOTGEL
            picture4 = Nothing

            'grafico5
            Dim anchor5 As HSSFClientAnchor
            'anchor5 = New HSSFClientAnchor(0, 0, 0, 255, 10, 37, 13, 41)
            anchor5 = New HSSFClientAnchor(0, 0, 0, 255, 6, 18, 10, 25)
            anchor5.AnchorType = 2

            Dim picture5 As HSSFPicture = DirectCast(patriarch.CreatePicture(anchor5,
                                                                            CargarImagenXLS(DBS25.getUsuarioxID(IdUsuario.ToString()) & "_05.jpg", wk)), HSSFPicture)
            picture5.Resize()
            picture5.LineStyle = HSSFPicture.LINESTYLE_DASHDOTGEL
            picture5 = Nothing

            'grafico6
            Dim anchor6 As HSSFClientAnchor
            'anchor6 = New HSSFClientAnchor(0, 0, 0, 255, 15, 37, 18, 41)
            anchor6 = New HSSFClientAnchor(0, 0, 0, 255, 12, 18, 16, 25)
            anchor6.AnchorType = 2

            Dim picture6 As HSSFPicture = DirectCast(patriarch.CreatePicture(anchor6,
                                                                            CargarImagenXLS(DBS25.getUsuarioxID(IdUsuario.ToString()) & "_06.jpg", wk)), HSSFPicture)
            picture6.Resize()
            picture6.LineStyle = HSSFPicture.LINESTYLE_DASHDOTGEL
            picture6 = Nothing

            'detalle de eventos

            'tActivos = Nothing
            lista = Nothing
            'tActivos = DBS25.spActivosSinReportar(CInt(IdUsuario), "127002", "")
            lista = negocio.ListarActivosSinReportar(obj)
            dsTotalComportamiento.Clear()

            'For Each tActivo In tActivos
            For Each tActivo In lista
                Try
                    Dim she As ISheet = wk.CreateSheet("Alertas " & tActivo.alias.Replace("[", "(").Replace("]", ")"))
                    With she
                        .CreateRow(0).CreateCell(0).SetCellValue("Detalle Alertas para " & tActivo.alias.Replace("[", "(").Replace("]", ")"))
                        .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7))
                    End With

                    dsComportamientoDetalle = Nothing
                    dsComportamientoDetalle = New DataSet()
                    DbDataAdapter = Nothing
                    DbDataAdapter = New SqlDataAdapter("exec spGenerarReporteConsolidadoDetalle " & IdUsuario.ToString() & ",'" &
                                                                                             Fecha2ANSI(FechaInicial.ToString(), True) & "','" &
                                                                                             Fecha2ANSI(FechaFinal.ToString(), True) & "','127001','00:00:00','23:59:59',1,'0','" & tActivo.alias.ToString() & "',0", DbConnection)

                    DbDataAdapter.SelectCommand.CommandTimeout = 20000000
                    DbDataAdapter.Fill(dsComportamientoDetalle, "Detalle" & tActivo.alias.ToString())
                    dsComportamientoDetalle.AcceptChanges()

                    Fila = Nothing
                    Celda = Nothing

                    If dsComportamientoDetalle.Tables.Count > 0 Then
                        Console.WriteLine("Detalle " & tActivo.alias & " " & dsComportamientoDetalle.Tables(0).Rows.Count.ToString())
                        Fila = she.CreateRow(she.LastRowNum + 3)
                        For Each dColum As DataColumn In dsComportamientoDetalle.Tables(0).Columns
                            Celda = Fila.CreateCell(dColum.Ordinal)
                            Celda.SetCellValue(dColum.ColumnName)
                            Celda.CellStyle = stlNada
                        Next
                        Fila = Nothing
                        Celda = Nothing

                        For Each oRow As DataRow In dsComportamientoDetalle.Tables(0).Rows
                            Fila = she.CreateRow(she.LastRowNum + 1)
                            For Each dColum As DataColumn In dsComportamientoDetalle.Tables(0).Columns
                                If dColum.ColumnName = "Evento" Then
                                    Select Case oRow(dColum.ColumnName).ToString()
                                        Case "90"
                                            oRow(dColum.ColumnName) = "EXCESO DE VELOCIDAD 100 KPH"
                                        Case "91"
                                            oRow(dColum.ColumnName) = "DISMINUCION VELOCIDAD 100 KPH"
                                        Case "110"
                                            oRow(dColum.ColumnName) = "ACELERACION BRUSCA"
                                        Case "111"
                                            oRow(dColum.ColumnName) = "FRENADO VIOLENTO"
                                        Case "112"
                                            oRow(dColum.ColumnName) = "GIRO VIOLENTO A LA IZQUIERDA"
                                        Case "113"
                                            oRow(dColum.ColumnName) = "GIRO VIOLENTO A LA DERECHA"
                                        Case "50"
                                            oRow(dColum.ColumnName) = "RALENTI"
                                        Case "62"
                                            oRow(dColum.ColumnName) = "ALARMA DE VEHICULO ACTIVADA"
                                        Case Else

                                    End Select
                                End If

                                Celda = Fila.CreateCell(dColum.Ordinal)
                                Celda.SetCellValue(oRow(dColum.ColumnName).ToString())
                                Celda.CellStyle = stlNormal
                            Next
                        Next
                    End If

                Catch ex As Exception

                End Try
            Next
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
            'PARA LLAMAR AL PROCESO GUARDARLOG UNA VEZ SE HAYA GENERADO UN ERROR
            '******************************************************************************
            GuardarLog(Tipo, ex.Message)
            '******************************************************************************
        End Try
    End Function

End Class
