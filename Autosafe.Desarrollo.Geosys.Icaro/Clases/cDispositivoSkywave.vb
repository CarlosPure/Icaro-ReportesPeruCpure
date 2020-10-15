Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades

Public Class cDispositivoSkywave : Inherits cGeneracion

    Private dsHistorico As DataSet
    Private Const Reporte As String = "Reporte de Dispositivos Skywave"


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

        Console.WriteLine(Datos.usuarioId.ToString() & ": Obteniendo Datos para el Reporte de Dispostivos Skywave")

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

            'Dim cantidad As Integer = negocio.ListarVehiculosActivos(obj)

            'If cantidad > 0 Then
            GenerarReporte(Datos.usuarioId,
                              Datos.intervalo,
                              Datos.parametros)
            'Else

            '    GuardarLog(Me.Tipo, "SINVEHICULOS")
            '    Console.WriteLine(String.Format("El usuario :  {0} no cuenta con vehiculos activos", Me.Usuario))

            'End If

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
            'PARA LLAMAR AL PROCESO ENVIARMAIL UNA VEZ SE HAYA GENERADO EL ARCHIVO EXCEL
            '******************************************************************************
            'If wk.NumberOfSheets > 0 Then
            '    Dim file As FileStream = New FileStream(Me.Archivo, FileMode.Create)
            '    wk.Write(file)
            '    file.Close()
            '    file.Dispose()
            '    file = Nothing
            '    wk.Close()
            '    wk = Nothing

            '    'For eInd As Integer = 0 To DatoEmail.Length - 1
            '    '    Console.WriteLine(DatoEmail(eInd))
            '    '    Try
            '    '        EnviarMail(DatoEmail(eInd),
            '    '               file.Name,
            '    '               "Adjunto Email con el Reporte Solicitado",
            '    '                Nothing,
            '    '                Reporte,
            '    '                "HIS",
            '    '                Datos.IdUsuario)
            '    '    Catch ex As Exception
            '    '        ErroresEnvio &= "ERR_MAIL"
            '    '    End Try
            '    'Next



            '    Try
            '        EnviarMail(Datos.Email,
            '               Archivo,
            '               "Adjunto Email con el Reporte Solicitado",
            '                Nothing,
            '                Reporte,
            '                "HIS",
            '                Datos.IdUsuario)
            '    Catch ex As Exception
            '        ErroresEnvio &= "ERR_MAIL"
            '    End Try

            'End If
            '******************************************************************************



            Try
                GC.Collect()
            Catch ex As Exception

            End Try

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

            Dim Archivo As String = "HistoricoFlota.xls"
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

            'dsHistorico = Nothing
            'dsHistorico = New DataSet()

            DbConnection = Nothing
            DbConnection = New SqlConnection(My.Settings.GEOSYSConnectionStringAux)

            If DbConnection.State = ConnectionState.Open Then
                DbConnection.Close()
            End If
            DbConnection.Open()

            DbDataAdapter = Nothing

            dsHistorico = Nothing
            dsHistorico = New DataSet()

            'Dim tActivos = DBS25.spActivosSinReportar(CInt(IdUsuario), "127002", "")


            Dim negocio As New ActivoBL()
            Dim obj As New ActivoEN
            obj.baseDatosId = 25
            obj.usuarioId = CInt(IdUsuario)


            ''Dim lista As New List(Of ReporteProgramadoEN)()
            'Dim lista As List(Of ReporteProgramadoEN)()

            'lista = negocio.Listar(obj);

            'Dim lista As List(Of ActivoEN) = negocio.ListarActivosSinReportar(obj)

            'For Each ReporteProgramadoEN In lista

            'Next



            Dim Hoja As ISheet
            Dim Pagina As Integer = 1

            'For Each tActivo In lista
            Try
                dsHistorico = Nothing
                dsHistorico = New DataSet()
                Console.ForegroundColor = ConsoleColor.Magenta

                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                'MOSTRAR MENSAJE EN LA CONSOLA CON PLACA, FECHAINICIO Y FECHAFIN
                '******************************************************************************
                'Console.WriteLine("Obteniendo Historico de: " & tActivo.Alias)
                Console.WriteLine(String.Format("Obteniendo Reporte de Dispositivos Skywave de:  {0} {1} {2}", "Soporte Tecnico", Fecha2ANSI(Me.FechaInicial.ToString(), True), Fecha2ANSI(Me.FechaFinal.ToString(), True)))
                '******************************************************************************

                Dim sql As String

                If Intervalo Like "*H" Then
                    '******************************************************************************
                    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                    'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
                    '******************************************************************************
                    sql = String.Format("pruebaSKY'", ""
                               )

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
                        sql = String.Format("peru.spEnviaSkywaveCaducado", ""
                               )
                    Else
                        sql = String.Format("spActivoRecorridoRConsultar2 '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'",
                               "ALIAS",
                               "CUS",
                               "C",
                               IdUsuario,
                               "127005",
                               Fecha2ANSI(Me.FechaInicial.ToString(), True) & " 00:00:00",
                               Fecha2ANSI(Me.FechaFinal.ToString(), True) & " 23:59:59",
                               0)
                    End If


                    DbDataAdapter = New SqlDataAdapter(sql, DbConnection)

                    '******************************************************************************
                End If



                Hoja = Nothing
                Hoja = wk.CreateSheet("ALIAS")
                With Hoja
                    .CreateRow(0).CreateCell(0).SetCellValue(Reporte)
                    .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 14))
                End With

                Hoja.Header.Center = "Reporte Dispostivos Skywave"
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


                'Fila.CreateCell(0).SetCellValue(String.Format("Cliente: {0}", Me.Nombre))
                '******************************************************************************

                Fila = Hoja.CreateRow(4)

                DbDataAdapter.SelectCommand.CommandTimeout = 3000000
                DbDataAdapter.Fill(dsHistorico, "Historico")
                dsHistorico.AcceptChanges()

                dsHistorico.AcceptChanges()

                '    For Each TmpRow As DataRow In dsHistorico.Tables(0).Rows

                'Next
                '    dsHistorico.AcceptChanges()



                '******************************************************************************



                'For Ind As Integer = 0 To dsHistorico.Tables(0).Columns.Count - 1
                '    Try
                '        If dsHistorico.Tables(0).Columns(Ind).ColumnName.ToUpper() Like "*OBD*" Then
                '            dsHistorico.Tables(0).Columns.RemoveAt(Ind)
                '        End If
                '    Catch ex As Exception
                '        Console.WriteLine(ex.Message)
                '    End Try
                'Next

                dsHistorico.AcceptChanges()

                Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                For Each dColum As DataColumn In dsHistorico.Tables(0).Columns
                    Celda = Fila.CreateCell(dColum.Ordinal)
                    Hoja.AutoSizeColumn(dColum.Ordinal)
                    Celda.CellStyle.Alignment = HorizontalAlignment.Center

                    Select Case dColum.ColumnName.ToUpper()
                        Case "SPEED"
                            Celda.SetCellValue("VELOCIDAD")
                        Case "DATE_TIME"
                            Celda.SetCellValue("FECHA HORA")
                        Case "HEADING"
                            Celda.SetCellValue("RUMBO")
                        Case "DEVENTO"
                            Celda.SetCellValue("EVENTO")
                        Case "LATITUDE"
                            Celda.SetCellValue("LATITUD")
                        Case "LOOGITUDE"
                            Celda.SetCellValue("LONGITUD")
                        Case Else
                            Celda.SetCellValue(dColum.ColumnName.ToUpper())
                    End Select

                    Celda.CellStyle = stlNada
                Next
                dsHistorico.AcceptChanges()

                Console.WriteLine("Filas a Generar: " & dsHistorico.Tables(0).Rows.Count.ToString())
                Console.ForegroundColor = ConsoleColor.Cyan

                NumHojas = 0
                NumHojas = dsHistorico.Tables(0).Rows.Count / 65000
                NumFila = 1
                Dim tmpHoja As ISheet = Hoja
                Pagina = 2

                For Each oRow As DataRow In dsHistorico.Tables(0).Rows
                    Fila = tmpHoja.CreateRow(tmpHoja.LastRowNum + 1)
                    Console.WriteLine(String.Format("Generando Fila {0} de {1}", tmpHoja.LastRowNum, dsHistorico.Tables(0).Rows.Count))
                    If NumFila = MaxFilasArchivo Then
                        Try
                            tmpHoja = Nothing
                            tmpHoja = wk.CreateSheet(String.Format("{0} ({1})", "ALIAS", Pagina))
                            Pagina += 1
                            NumFila = 1
                            With tmpHoja
                                .CreateRow(0).CreateCell(0).SetCellValue(Reporte)
                                .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 14))
                            End With


                            tmpHoja.Header.Center = "Reporte Dispositivos Skywave"
                            Fila = tmpHoja.CreateRow(1)

                            '******************************************************************************
                            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181002
                            'PARA 
                            'PARA AGREGAR EL NOMBRE DEL USUARIO CORRESPONDIENTE AL ARCHIVO EXCEL
                            '******************************************************************************
                            If (My.Settings.Pais = "PE") Then
                                textoCelda = String.Format("Desde: {0} {1}", Fecha2ANSI(Me.FechaInicial.ToString(), True), Me.HoraInicio)
                            Else
                                textoCelda = String.Format("Desde: {0} 00:00:00", Fecha2ANSI(Me.FechaInicial.ToString(), True))
                            End If

                            Fila.CreateCell(0).SetCellValue(textoCelda)
                            Fila = tmpHoja.CreateRow(2)

                            If (My.Settings.Pais = "PE") Then
                                textoCelda = String.Format("Hasta: {0} {1}", Fecha2ANSI(Me.FechaFinal.ToString(), True), Me.HoraFin)
                            Else
                                textoCelda = String.Format("Hasta: {0} 23:59:59", Fecha2ANSI(Me.FechaFinal.ToString(), True))
                            End If

                            Fila.CreateCell(0).SetCellValue(textoCelda)
                            Fila = tmpHoja.CreateRow(3)
                            Fila = tmpHoja.CreateRow(4)

                            Fila = tmpHoja.CreateRow(tmpHoja.LastRowNum + 1)
                            Console.WriteLine(String.Format("Generando Fila {0} de {1}", tmpHoja.LastRowNum, dsHistorico.Tables(0).Rows.Count))

                            For Each dColum As DataColumn In dsHistorico.Tables(0).Columns
                                Celda = Fila.CreateCell(dColum.Ordinal)
                                tmpHoja.AutoSizeColumn(dColum.Ordinal)
                                Celda.CellStyle.Alignment = HorizontalAlignment.Center

                                Select Case dColum.ColumnName.ToUpper()
                                    Case "SPEED"
                                        Celda.SetCellValue("VELOCIDAD")
                                    Case "DATE_TIME"
                                        Celda.SetCellValue("FECHA HORA")
                                    Case "HEADING"
                                        Celda.SetCellValue("RUMBO")
                                    Case "DEVENTO"
                                        Celda.SetCellValue("EVENTO")
                                    Case Else
                                        Celda.SetCellValue(dColum.ColumnName.ToUpper())
                                End Select

                                Celda.CellStyle = stlNada
                            Next
                        Catch ex As Exception
                            Console.ForegroundColor = ConsoleColor.Red
                            Console.WriteLine(ex.Message)
                        End Try
                    End If

                    For Each dColum As DataColumn In dsHistorico.Tables(0).Columns
                        Celda = Fila.CreateCell(dColum.Ordinal)
                        Celda.CellStyle.Alignment = HorizontalAlignment.Center
                        Celda.CellStyle = stlNormal
                        Celda.SetCellValue(oRow(dColum.ColumnName).ToString())
                    Next

                    NumFila += 1
                Next

                'For Each dColum As DataColumn In dsHistorico.Tables(0).Columns
                '    Hoja.AutoSizeColumn(dColum.Ordinal)
                'Next
                Console.WriteLine("Registros Generados para " & "Soporte Tecnico")
            Catch ex As Exception
                Console.Write(ex.Message)
            End Try
            'Next
            dsHistorico = Nothing



            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181002
            'PARA LLAMAR AL PROCESO ENVIARMAIL UNA VEZ SE HAYA GENERADO EL ARCHIVO EXCEL
            '******************************************************************************
            If wk.NumberOfSheets > 0 Then
                Dim NOMBREARCHIVO2 = Me.Archivo
                Dim NOMBREARCHIVO = "Sky_" & Fecha2ANSI(Me.FechaInicial.ToString()) & "_" & Fecha2ANSI(Me.FechaFinal.ToString()) & ".xls"
                Dim file As FileStream = New FileStream(NOMBREARCHIVO, FileMode.Create)
                wk.Write(file)
                file.Close()
                file.Dispose()
                file = Nothing
                wk.Close()
                wk = Nothing

                'For eInd As Integer = 0 To DatoEmail.Length - 1
                '    Console.WriteLine(DatoEmail(eInd))
                '    Try
                '        EnviarMail(DatoEmail(eInd),
                '               file.Name,
                '               "Adjunto Email con el Reporte Solicitado",
                '                Nothing,
                '                Reporte,
                '                "HIS",
                '                Datos.IdUsuario)
                '    Catch ex As Exception
                '        ErroresEnvio &= "ERR_MAIL"
                '    End Try
                'Next


                Try
                    EnviarMail(Me.Email,
                           NOMBREARCHIVO,
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



End Class
