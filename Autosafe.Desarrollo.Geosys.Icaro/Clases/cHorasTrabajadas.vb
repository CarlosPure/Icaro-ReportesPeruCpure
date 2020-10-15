Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades

Public Class cHorasTrabajadas : Inherits cGeneracion

    Private dsHorasTrabajadas As DataSet
    Private Const Reporte As String = "Reporte de Horas Trabajadas de la Flota"
    Private FactorConsumo As Double = 0.0
    Private FactorConsumoHoras As Double = 0.0
    Private FactorHoras As Boolean = False

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
        Console.WriteLine(String.Format("{0}: Obteniendo Datos para el Reporte de Horas Trabajadas", Datos.usuarioId))

        Try
            DatoTipo = Datos.tipoReporte.Split(";")
            DatoActivos = Datos.activos.Split(";")
            DatoEmail = Datos.email.Split(";")

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
            'PARA GUARDAR LOS EMAILS A QUIENES LES LLEGARA EL REPORTE GENERADO
            '******************************************************************************
            Me.Email = Datos.email
            '******************************************************************************
            Dim negocio As New ActivoBL()
            Dim obj As New ActivoEN
            obj.baseDatosId = 25
            obj.usuarioId = CInt(IdUsuario)
            obj.entidadId = "0"

            Dim cantidad As Integer = negocio.ListarVehiculosActivos(obj)

            If cantidad > 0 Then
                GenerarReporte(Datos.usuarioId,
                              Datos.intervalo,
                              Datos.parametros)
            Else
                GuardarLog(Me.Tipo, "SINVEHICULOS")
                Console.WriteLine(String.Format("El usuario :  {0} no cuenta con vehiculos activos", Me.Usuario))

            End If


            'If wk.NumberOfSheets > 0 Then
            '    Dim file As FileStream = New FileStream(Me.Archivo, FileMode.Create)
            '    wk.Write(file)
            '    file.Close()
            '    file.Dispose()
            '    file = Nothing
            '    wk.Close()

            '    'For eInd As Integer = 0 To DatoEmail.Length - 1
            '    '    Console.WriteLine(DatoEmail(eInd))
            '    '    Try
            '    '        EnviarMail(DatoEmail(eInd),
            '    '               file.Name,
            '    '               "Adjunto Email con el Reporte Solicitado",
            '    '                wk.GetSheet(0),
            '    '                Reporte,
            '    '                "HTR",
            '    '                Datos.IdUsuario)
            '    '    Catch ex As Exception
            '    '        ErroresEnvio &= "ERR_MAIL"
            '    '    End Try
            '    'Next

            '    Try
            '        EnviarMail(Datos.Email,
            '               Archivo,
            '               "Adjunto Email con el Reporte Solicitado",
            '                wk.GetSheet(0),
            '                Reporte,
            '                "HTR",
            '                Datos.IdUsuario)
            '    Catch ex As Exception
            '        ErroresEnvio &= "ERR_MAIL"
            '    End Try

            'End If

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

            With Stl
                .FillForegroundColor = NPOI.HSSF.Util.HSSFColor.SeaGreen.Index
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

            Dim Archivo As String = "HorasFlota.xls"
            Dim dataO As ArrayList

            'Select Case Intervalo
            '    Case "MC"
            '        FechaInicial = FechaFinal.AddDays(-1 * Now.Date.Day)
            '        FechaFinal = Now()
            '    Case "MA"
            '        FechaInicial = FechaInicial.AddMonths(-1)
            '        FechaFinal = FechaFinal.AddDays(-1)
            '    Case "DA"
            '        FechaInicial = FechaInicial.AddDays(-1)
            '        FechaFinal = FechaFinal.AddDays(-1)
            '    Case "SA"
            '        FechaInicial = FechaInicial.AddDays(-7)
            '        FechaFinal = FechaFinal.AddDays(-1)
            '    Case Else
            '        If Intervalo Like "*M" Then
            '            FechaInicial = FechaInicial.AddMonths(-1)
            '            FechaFinal = FechaFinal.AddDays(-1)
            '        Else
            '            FechaInicial = FechaInicial.AddDays(0)
            '            FechaFinal = FechaFinal.AddDays(0)
            '        End If
            'End Select

            dsHorasTrabajadas = Nothing
            dsHorasTrabajadas = New DataSet()

            DbConnection = Nothing
            DbConnection = New SqlConnection(My.Settings.GEOSYSConnectionStringAux)

            If DbConnection.State = ConnectionState.Open Then
                DbConnection.Close()
            End If
            DbConnection.Open()

            DbDataAdapter = Nothing

            dsHorasTrabajadas = Nothing
            'dsHorasTrabajadas = New DataSet()

            Dim tmpHoras As DataSet
            tmpHoras = Nothing
            tmpHoras = New DataSet()

            'Dim tActivos = DBS25.spActivosSinReportar(CInt(IdUsuario), "127002", "")

            Dim negocio As New ActivoBL()
            Dim obj As New ActivoEN
            obj.baseDatosId = 25
            obj.usuarioId = CInt(IdUsuario)


            Dim lista As List(Of ActivoEN) = negocio.ListarActivosSinReportar(obj)


            Dim Hoja As ISheet
            For Each tActivo In lista
                Try

                    dsHorasTrabajadas = New DataSet()

                    FactorConsumo = 0.0
                    FactorConsumoHoras = 0.0

                    Try
                        FactorConsumo = DBS25.getFactorConsumo(tActivo.vid)
                    Catch ex As Exception
                        FactorConsumo = 0
                    End Try

                    If FactorConsumo = 0 Then
                        Try
                            FactorConsumoHoras = DBS25.getFactorConsumoHoras(tActivo.vid)
                        Catch ex As Exception
                            FactorConsumoHoras = 0.0
                        End Try
                    End If

                    If FactorConsumoHoras > 0 Then
                        FactorHoras = True
                    Else
                        FactorHoras = False
                    End If

                    tmpHoras.Clear()
                    tmpHoras.AcceptChanges()
                    Console.ForegroundColor = ConsoleColor.Magenta

                    '******************************************************************************
                    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                    'MOSTRAR MENSAJE EN LA CONSOLA CON PLACA, FECHAINICIO Y FECHAFIN
                    '******************************************************************************
                    'Console.WriteLine("Obteniendo Horas Trabajadas de: " & tActivo.Alias)
                    Console.WriteLine(String.Format("Obteniendo Horas Trabajadas de:  {0} {1} {2}", tActivo.alias, Fecha2ANSI(Me.FechaInicial.ToString(), True), Fecha2ANSI(Me.FechaFinal.ToString(), True)))
                    '******************************************************************************

                    Dim sql As String

                    If Intervalo Like "*H" Then

                        '******************************************************************************
                        'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                        'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
                        '******************************************************************************
                        sql = String.Format("spActivoRecorridoOConsultar4 '{0}','{1}','{2}'",
                               tActivo.vid,
                               Fecha2ANSI(Me.FechaInicial.ToString(), False),
                               Fecha2ANSI(Me.FechaFinal.ToString(), False))

                        DbDataAdapter = New SqlDataAdapter(sql, DbConnection)
                        '******************************************************************************

                    Else

                        '******************************************************************************
                        'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                        'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
                        'PARA CAMBIAR EL NOMBRE DEL PROCEDIMIENTO LLAMADO
                        'PARA LLAMAR AL PROCEDIMIENTO DE ACUERDO A LA VARIABLE PAIS
                        '******************************************************************************
                        If (My.Settings.Pais = "PE") Then
                            sql = String.Format("spActivoRecorridoOConsultar4_Resumen '{0}','{1}','{2}'",
                               tActivo.vid,
                               Fecha2ANSI(Me.FechaInicial.ToString(), True) & " " & Me.HoraInicio,
                               Fecha2ANSI(Me.FechaFinal.ToString(), True) & " " & Me.HoraFin)
                        Else
                            sql = String.Format("spActivoRecorridoOConsultar4 '{0}','{1}','{2}'",
                               tActivo.vid,
                               Fecha2ANSI(Me.FechaInicial.ToString(), True) & " 00:00:00",
                               Fecha2ANSI(Me.FechaFinal.ToString(), True) & " 23:59:59")
                        End If

                        DbDataAdapter = New SqlDataAdapter(sql, DbConnection)
                        '******************************************************************************

                    End If

                    Hoja = Nothing
                    Hoja = wk.CreateSheet(tActivo.alias.Replace("[", "(").Replace("]", ")"))
                    With Hoja
                        .CreateRow(0).CreateCell(0).SetCellValue(Reporte)
                        .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 5))
                    End With

                    Hoja.Header.Center = "Reporte de Horas Trabajadas"

                    Fila = Hoja.CreateRow(1)
                    '******************************************************************************
                    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181002
                    'PARA 
                    'PARA AGREGAR LA HORAINICIO Y HORAFIN EN LA CABECERA DEL REPORTE
                    'PARA AGREGAR EL NOMBRE DEL USUARIO CORRESPONDIENTE AL ARCHIVO EXCEL
                    '******************************************************************************

                    Dim textoCelda As String

                    If (My.Settings.Pais = "PE") Then
                        textoCelda = String.Format("Desde: {0} {1}", Fecha2ANSI(Me.FechaInicial.ToString(), True), Me.HoraInicio)
                    Else
                        textoCelda = String.Format("Desde: {0} 00:00:00", Fecha2ANSI(Me.FechaInicial.ToString(), True))
                    End If

                    Fila.CreateCell(0).SetCellValue(textoCelda)
                    Fila = Hoja.CreateRow(2)

                    If (My.Settings.Pais = "PE") Then
                        textoCelda = String.Format("Hasta: {0} {1}", Fecha2ANSI(Me.FechaFinal.ToString(), True), Me.HoraFin)
                    Else
                        textoCelda = String.Format("Hasta: {0} 23:59:59", Fecha2ANSI(Me.FechaFinal.ToString(), True))
                    End If

                    Fila.CreateCell(0).SetCellValue(textoCelda)
                    Fila = Hoja.CreateRow(3)


                    Fila.CreateCell(0).SetCellValue(String.Format("Cliente: {0}", Me.Nombre))
                    '******************************************************************************

                    Fila = Hoja.CreateRow(4)

                    DbDataAdapter.SelectCommand.CommandTimeout = 3000000
                    DbDataAdapter.Fill(dsHorasTrabajadas, "Historico")
                    dsHorasTrabajadas.AcceptChanges()

                    For Each TmpRow As DataRow In dsHorasTrabajadas.Tables(0).Rows
                        Try
                            If FactorHoras Then
                                If FactorConsumo > 0 Then
                                    TmpRow("Consumo") = Math.Round((TmpRow("Horas") * FactorConsumo), 1)
                                Else
                                    TmpRow("Consumo") = 0
                                End If
                            Else
                                If FactorConsumo > 0 Then
                                    TmpRow("Consumo") = Math.Round((TmpRow("KmsRecorridos") / FactorConsumo), 1)
                                Else
                                    TmpRow("Consumo") = 0
                                End If
                            End If
                        Catch ex As Exception
                            TmpRow("Consumo") = 0.0
                        End Try

                        TmpRow.AcceptChanges()
                    Next
                    dsHorasTrabajadas.AcceptChanges()

                    Try
                        dsHorasTrabajadas.Tables(0).Columns.Remove("Ignicion")
                    Catch ex As Exception

                    End Try

                    Try
                        dsHorasTrabajadas.Tables(0).Columns.Remove("Ver")
                    Catch ex As Exception

                    End Try

                    Try
                        dsHorasTrabajadas.Tables(0).Columns.Remove("Minutos")
                    Catch ex As Exception

                    End Try

                    Try
                        dsHorasTrabajadas.Tables(0).Columns.Remove("EA2")
                    Catch ex As Exception

                    End Try

                    Try
                        dsHorasTrabajadas.Tables(0).Columns.Remove("GasolinaConsumidaPorViajeOBD")
                    Catch ex As Exception

                    End Try

                    Try
                        dsHorasTrabajadas.Tables(0).Columns.Remove("CombustibleRestanteOBD")
                    Catch ex As Exception

                    End Try


                    Try
                        dsHorasTrabajadas.Tables(0).Columns.Remove("NivelGasolinaOBD")
                    Catch ex As Exception

                    End Try

                    Try
                        dsHorasTrabajadas.Tables(0).Columns.Remove("OdometroViajeOBD")
                    Catch ex As Exception

                    End Try

                    Try
                        dsHorasTrabajadas.Tables(0).Columns.Remove("OdometroOBD")
                    Catch ex As Exception

                    End Try

                    Try
                        dsHorasTrabajadas.Tables(0).Columns.Remove("Secuencia")
                    Catch ex As Exception

                    End Try

                    Try
                        dsHorasTrabajadas.Tables(0).Columns.Remove("Minutos1")
                    Catch ex As Exception

                    End Try

                    dsHorasTrabajadas.AcceptChanges()

                    Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                    For Each dColum As DataColumn In dsHorasTrabajadas.Tables(0).Columns
                        Celda = Fila.CreateCell(dColum.Ordinal)
                        Hoja.AutoSizeColumn(dColum.Ordinal)
                        Celda.CellStyle.Alignment = HorizontalAlignment.Center
                        Celda.SetCellValue(dColum.ColumnName.ToUpper())
                        Celda.CellStyle = stlNada
                    Next
                    dsHorasTrabajadas.AcceptChanges()

                    For Each oRow As DataRow In dsHorasTrabajadas.Tables(0).Rows
                        Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                        Console.ForegroundColor = ConsoleColor.Cyan
                        Console.WriteLine(String.Format("Generando Fila {0} de {1}", Hoja.LastRowNum, dsHorasTrabajadas.Tables(0).Rows.Count))
                        For Each dColum As DataColumn In dsHorasTrabajadas.Tables(0).Columns
                            Celda = Fila.CreateCell(dColum.Ordinal)
                            Celda.CellStyle.Alignment = HorizontalAlignment.Center
                            Celda.CellStyle = stlNormal

                            Celda.SetCellValue(oRow(dColum.ColumnName).ToString())
                        Next
                    Next
                    Console.WriteLine("Registros Generados para " & tActivo.alias.ToUpper())

                    dsHorasTrabajadas = Nothing
                Catch ex As Exception
                    Console.Write(ex.Message)
                End Try
            Next
            dsHorasTrabajadas = Nothing


            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180914
            'PARA LLAMAR AL PROCESO ENVIARMAIL UNA VEZ SE HAYA GENERADO EL ARCHIVO EXCEL
            '******************************************************************************

            If wk.NumberOfSheets > 0 Then
                Dim file As FileStream

                If TotalRegistros < My.Settings.TotalRegistrosMaximo Then
                    file = New FileStream(Me.Archivo, FileMode.Create)
                    wk.Write(file)
                    file.Close()
                    file.Dispose()
                    file = Nothing
                    wk.Close()
                    wk = Nothing
                End If


                Try
                    EnviarMail(Me.Email,
                           Me.Archivo,
                           "Adjunto Email con el Reporte Solicitado",
                            Nothing,
                            Reporte,
                            Me.Tipo,
                            Me.IdUsuario)
                Catch ex As Exception
                    ErroresEnvio &= "ERR_MAIL"
                End Try
            End If

            '******************************************************************************



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

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
