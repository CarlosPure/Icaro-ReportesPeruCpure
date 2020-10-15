Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports System.IO
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades

Public Class cZonas : Inherits cGeneracion

    Private dsZonas As DataSet
    Private Const Reporte As String = "Reporte de Zonas de la Flota"

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
        Console.WriteLine(Datos.usuarioId.ToString() & ": Obteniendo Datos para el Reporte Reporte de Zonas")

        Try
            DatoTipo = Datos.tipoReporte.Split(";")
            DatoActivos = Datos.activos.Split(";")
            DatoEmail = Datos.email.Split(";")

            Dim negocio As New ActivoBL()
            Dim obj As New ActivoEN
            obj.baseDatosId = 25
            obj.usuarioId = CInt(IdUsuario)
            obj.entidadId = "0"

            Dim cantidad As Integer = negocio.ListarVehiculosActivos(obj)

            If cantidad > 0 Then
                Dim sh As ISheet = wk.CreateSheet("Flota")
                With sh
                    .CreateRow(0).CreateCell(0).SetCellValue(Reporte)
                    .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 8))
                End With

                'Console.WriteLine("{0}", Datos.Secuencia)

                GenerarReporte(Datos.secuencia, Datos.usuarioId, Datos.intervalo, Datos.parametros, sh)

                If wk.NumberOfSheets > 0 Then
                    Dim file As FileStream
                    If TotalRegistros < 1000 Then
                        file = New FileStream(Me.Archivo, FileMode.Create)
                        wk.Write(file)
                        file.Close()
                        file.Dispose()
                        file = Nothing
                        wk.Close()
                        wk = Nothing
                    End If

                    Try
                        EnviarMail(Datos.email,
                                  Archivo,
                                  "Adjunto Email con el Reporte Solicitado",
                                  Nothing,
                                  Reporte,
                                  "RZN",
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

    Private Function GenerarReporte(ByVal Secuencia As Integer, ByVal IdUsuario As Integer,
                               ByVal Intervalo As String, ByVal Parametros As String,
                               ByVal Hoja As ISheet) As String
        Try
            Hoja.Header.Center = "Reporte Zonas Flota"
            Fila = Hoja.CreateRow(1)
            Fila.CreateCell(0).SetCellValue(String.Format("Desde: {0} 00:00:00", Fecha2ANSI(FechaInicial.ToString(), True)))
            Fila = Hoja.CreateRow(2)
            Fila.CreateCell(0).SetCellValue(String.Format("Hasta: {0} 23:59:59", Fecha2ANSI(FechaFinal.ToString(), True)))
            Fila = Hoja.CreateRow(3)
            Fila = Hoja.CreateRow(4)

            dsZonas = Nothing
            dsZonas = New DataSet()

            DbConnection = Nothing
            DbConnection = New SqlConnection(My.Settings.GEOSYSConnectionStringAux)

            If DbConnection.State = ConnectionState.Open Then
                DbConnection.Close()
            End If
            DbConnection.Open()

            DbDataAdapter = Nothing

            dsZonas = Nothing
            dsZonas = New DataSet()

            Dim tmpZonas As DataSet = New DataSet()
            hsVehiculos.Clear()
            Dim tActivos = DBS25.spActivosSinReportar(CInt(IdUsuario), "127002", "")
            For Each tActivo In tActivos
                Try
                    tmpZonas.Clear()
                    tmpZonas.AcceptChanges()
                    Console.ForegroundColor = ConsoleColor.Magenta
                    Console.WriteLine("Obteniendo Zonas de: " & tActivo.Alias)

                    '[spActivoRecorridoZConsultar] '1023020417','CUS','RZN',20661,'::1','21/2/2018 00:00:00','21/2/2018 11:00:00',0

                    'adp = New SqlDataAdapter("[spActivoRecorridoZConsultar] '" & VID & "','" _
                    '                                                     & "CUS" & "','" _
                    '                                                     & Tipo & "'," _
                    '                                                     & IdUsuario.ToString() & ",'" _
                    '                                                     & IP & "','" _
                    '                                                     & Desde & "','" _
                    '                                                     & Hasta & "'," _
                    '                                                     & SoloPunto.ToString(),
                    '                                                        objConexion)

                    If Intervalo Like "*H" Then
                        DbDataAdapter = New SqlDataAdapter(String.Format("[spReporteProgramadoZona] '{0}','{1}','{2}','{3}','{4}'",
                                    Secuencia,
                                    tActivo.VID,
                                    IdUsuario,
                                    Fecha2ANSI(FechaInicial.ToString(), False),
                                    Fecha2ANSI(FechaFinal.ToString(), False)), DbConnection)
                    Else
                        DbDataAdapter = New SqlDataAdapter(String.Format("[spReporteProgramadoZona] '{0}','{1}','{2}','{3}','{4}'",
                                    Secuencia,
                                    tActivo.VID,
                                    IdUsuario,
                                    Fecha2ANSI(FechaInicial.ToString(), True) & " 00:00:00",
                                    Fecha2ANSI(FechaFinal.ToString(), True) & " 23:59:59"), DbConnection)
                    End If

                    If My.Settings.Debug = True Then
                        Console.WriteLine(DbDataAdapter.SelectCommand.CommandText)
                    End If

                    hsVehiculos.Add(tActivo.VID, tActivo.Alias)

                    DbDataAdapter.SelectCommand.CommandTimeout = 3000000
                    DbDataAdapter.Fill(tmpZonas, "Zonas")
                    tmpZonas.AcceptChanges()

                    Console.WriteLine(String.Format("Registros Obtenidos para {0} {1}", tActivo.Alias, tmpZonas.Tables(0).Rows.Count()))

                    dsZonas.Merge(tmpZonas)
                Catch ex As Exception
                    Console.Write(ex.Message)
                End Try
            Next
            dsZonas.AcceptChanges()

            Console.WriteLine(dsZonas.Tables(0).Rows.Count.ToString() & " Filas Totales")


            'For Ind As Integer = 0 To dsZonas.Tables(0).Columns.Count - 1
            '    Try
            '        If dsZonas.Tables(0).Columns(Ind).ColumnName.ToUpper() Like "*OBD*" Then
            '            dsZonas.Tables(0).Columns.RemoveAt(Ind)
            '        End If
            '    Catch ex As Exception
            '        Console.WriteLine(ex.Message)
            '    End Try
            'Next
            'dsZonas.AcceptChanges()

            TotalRegistros = dsZonas.Tables(0).Rows.Count

            If TotalRegistros < 1000 Then
                Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                For Each dColum As DataColumn In dsZonas.Tables(0).Columns
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
                        Case "LATITUDE"
                            Celda.SetCellValue("LATITUD")
                        Case "LOOGITUDE"
                            Celda.SetCellValue("LONGITUD")
                        Case "DEVENTO"
                            Celda.SetCellValue("EVENTO")
                        Case "VID"
                            Celda.SetCellValue("PLACA")
                        Case Else
                            Celda.SetCellValue(dColum.ColumnName.ToUpper())
                    End Select

                    Celda.CellStyle = stlNada
                Next
                dsZonas.AcceptChanges()

                Console.ForegroundColor = ConsoleColor.DarkYellow
                Console.WriteLine(String.Format("Generando Archivo de {0} Filas", dsZonas.Tables(0).Rows.Count))
                For Each oRow As DataRow In dsZonas.Tables(0).Rows
                    Console.ForegroundColor = ConsoleColor.DarkGray
                    Console.WriteLine("Escribiendo Fila # " & Hoja.LastRowNum.ToString())
                    Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                    For Each dColum As DataColumn In dsZonas.Tables(0).Columns
                        Celda = Fila.CreateCell(dColum.Ordinal)
                        'Hoja.AutoSizeColumn(dColum.Ordinal)
                        Celda.CellStyle.Alignment = HorizontalAlignment.Center
                        Celda.CellStyle = stlNormal

                        Celda.SetCellValue(oRow(dColum.ColumnName).ToString())
                    Next
                Next
                Console.WriteLine("Fin de Generacion de Archivo para Envio")
            Else
                Dim vnxls As New fZonas(dsZonas, Archivo)
                vnxls.GenerarArchivo()
            End If
            dsZonas = Nothing
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
