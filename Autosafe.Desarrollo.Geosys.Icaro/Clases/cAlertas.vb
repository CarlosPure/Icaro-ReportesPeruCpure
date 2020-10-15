Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports System.IO
Imports System.Globalization
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades

Public Class cAlertas : Inherits cGeneracion

    Private dsAlertas As DataSet
    Private Const Reporte As String = "Reporte de Alertas de la Flota"


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
        Console.WriteLine(Datos.usuarioId.ToString() & ": Obteniendo Datos para el Reporte Reporte de Alertas")

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

                GenerarReporte(Datos.usuarioId,
                              Datos.intervalo,
                              Datos.parametros,
                              sh)

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
                              "ALR",
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
                               ByVal Parametros As String,
                               ByVal Hoja As ISheet) As String
        Try
            Hoja.Header.Center = "Reporte Alertas Flota"
            Fila = Hoja.CreateRow(1)
            Fila.CreateCell(0).SetCellValue(String.Format("Desde: {0} 00:00:00", Fecha2ANSI(FechaInicial.ToString(), True)))
            Fila = Hoja.CreateRow(2)
            Fila.CreateCell(0).SetCellValue(String.Format("Hasta: {0} 23:59:59", Fecha2ANSI(FechaFinal.ToString(), True)))
            Fila = Hoja.CreateRow(3)
            Fila = Hoja.CreateRow(4)

            dsAlertas = Nothing
            dsAlertas = New DataSet()

            DbConnection = Nothing
            DbConnection = New SqlConnection(My.Settings.GEOSYSConnectionStringAux)

            If DbConnection.State = ConnectionState.Open Then
                DbConnection.Close()
            End If
            DbConnection.Open()

            DbDataAdapter = Nothing

            dsAlertas = Nothing
            dsAlertas = New DataSet()

            Dim tmpAlertas As DataSet = New DataSet()
            hsVehiculos.Clear()

            Dim negocio As New ActivoBL()
            Dim obj As New ActivoEN
            obj.baseDatosId = 25
            obj.usuarioId = CInt(IdUsuario)

            Dim lista As List(Of ActivoEN) = negocio.ListarActivosSinReportar(obj)

            'Dim tActivos = DBS25.spActivosSinReportar(CInt(IdUsuario), "127002", "")

            'For Each tActivo In tActivos
            For Each tActivo In lista
                Try
                    tmpAlertas.Clear()
                    tmpAlertas.AcceptChanges()
                    Console.ForegroundColor = ConsoleColor.Magenta
                    Console.WriteLine("Obteniendo Alertas de: " & tActivo.alias)

                    Dim sql As String

                    If Intervalo Like "*H" Then
                        'DbDataAdapter = New SqlDataAdapter(String.Format("[spActivoRecorridoAConsultar2] '{0}','{1}','{2}','{3}','{4}'",
                        '            tActivo.VID,
                        '            IdUsuario,
                        '            0,
                        '            Fecha2ANSI(FechaInicial.ToString(), False),
                        '            Fecha2ANSI(FechaFinal.ToString(), False)), DbConnection)
                        sql = String.Format("[spActivoRecorridoAConsultar2] '{0}','{1}','{2}','{3}','{4}'",
                                    tActivo.vid,
                                    IdUsuario,
                                    0,
                                    Fecha2ANSI(FechaInicial.ToString(), False),
                                    Fecha2ANSI(FechaFinal.ToString(), False))

                    Else
                        sql = String.Format("[spActivoRecorridoAConsultar2] '{0}','{1}','{2}','{3}','{4}'",
                                    tActivo.vid,
                                    IdUsuario,
                                    0,
                                    Fecha2ANSI(FechaInicial.ToString(), True) & " 00:00:00",
                                    Fecha2ANSI(FechaFinal.ToString(), True) & " 23:59:59")

                    End If

                    DbDataAdapter = New SqlDataAdapter(sql, DbConnection)

                    If My.Settings.Debug = True Then
                        Console.WriteLine(DbDataAdapter.SelectCommand.CommandText)
                    End If

                    hsVehiculos.Add(tActivo.vid, tActivo.alias)

                    DbDataAdapter.SelectCommand.CommandTimeout = 3000000
                    DbDataAdapter.Fill(tmpAlertas, "Velocidad")
                    tmpAlertas.AcceptChanges()

                    Console.WriteLine(String.Format("Registros Obtenidos para {0} {1}", tActivo.alias, tmpAlertas.Tables(0).Rows.Count()))

                    dsAlertas.Merge(tmpAlertas)
                Catch ex As Exception
                    Console.Write(ex.Message)
                End Try
            Next
            dsAlertas.AcceptChanges()

            Try
                dsAlertas.Tables(0).Columns.Remove("DETENIDODESDE")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("ALTITUD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            For Each drow As DataRow In dsAlertas.Tables(0).Rows

                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
                'PARA AGREGAR NUEVAS COLUMNAS
                '******************************************************************************
                Try
                    drow("Date_Time") = DateAdd(DateInterval.Hour, -5, drow("Date_Time"))
                Catch ex As Exception
                    drow("Date_Time") = drow("Date_Time")
                End Try
                '******************************************************************************

                Try
                    If IsDBNull(drow("DEvento")) Then
                        If LastEvento = -1 Or (LastEvento <> drow("Input_Event")) Then

                            drow("DEvento") = DBS25.getDescripcionEvento(drow("ID"), CInt(drow("Input_Event")))
                            LastEvento = drow("Input_Event")
                            LastDEvento = drow("DEVento")
                        Else
                            drow("DEvento") = LastDEvento
                        End If
                    End If
                Catch ex As Exception
                    drow("DEvento") = String.Format("{0} ({1})",
                                                              "N/D",
                                                              drow("Input_Event"))
                End Try

                Try
                    'Console.WriteLine("Speed" + drow("Speed"))

                    Dim sCurrentCulture As String = System.Threading.Thread.CurrentThread.CurrentCulture.Name


                    Dim ci As New CultureInfo(sCurrentCulture)


                    ci = New CultureInfo(sCurrentCulture)
                    ci.NumberFormat.NumberDecimalSeparator = "."
                    System.Threading.Thread.CurrentThread.CurrentCulture = ci

                    Dim text As String = drow("Speed").ToString()
                    Dim value As Double
                    Double.TryParse(text, value)
                    drow("Speed") = Round(value * 1.609344, 2)
                Catch ex As Exception
                    drow("Speed") = 0
                End Try

                Try
                    drow("Heading") = GetCourse(drow("Heading"))
                Catch ex As Exception
                    drow("Heading") = ""
                End Try

                'Try
                '    drow("TDESDE") = tActivos
                'Catch ex As Exception
                '    Console.WriteLine(ex.Message)
                'End Try

                Try
                    drow("Kilometraje") = Math.Round(drow("Kilometraje") / 1000, 0)
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try

                Try
                    drow("VID") = hsVehiculos(drow("VID"))
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            Next
            dsAlertas.AcceptChanges()

            Try
                dsAlertas.Tables(0).Columns.Remove("Numero")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tLatitud")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tLongitud")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("Cod. Evento")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("GPS_Status")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tHorometro")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("Nivel Bateria")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tNivel Bateria")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tDate_Time")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tEstadoGPS")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("GPS_Status")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("Input_Event")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tEstado GPS")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("Horometro")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tHorometro")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("Nivel Bateria")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tNivel Bateria")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("Voltaje Bateria")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tVoltaje Bateria")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("Voltaje Alimentacion")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tVoltajeAlimentacion")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("EA1")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("NIVELBATERIA")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("VOLTAJEBATERIA")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("EA2")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("EA3")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tEA1")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tEA2")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tEA3")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("SA1")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("SA2")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("SA3")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tSA1")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tSA2")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("tSA3")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("CE")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("TNIVELBATERIA")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("IDACTIVO")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("DRIVERID")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("TKILOMETRAJE")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("ESTADOGPS")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("TVOLTAJEBATERIA")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("PTO. CERCANO")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("EVENTO")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("VELOCIDADOBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("RMPOBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("POSICIONACELERADOROBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("ODOMETROVIAJEOBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try


            Try
                dsAlertas.Tables(0).Columns.Remove("ODOMETROOBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("RPMOBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("NIVELGASOLINAOBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("COMBUSTIBLERESTANTEOBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("ENGRANETRANSMISIONOBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("TEMPERATURAREFRIGERANTEOBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("INDICEGASOLINAOBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("VOLTAJEALIMENTACIONOBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("ESTADOSEÑALESGIROOBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("GASOLINACONSUMIDAPORVIAJEOBD")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("EVENTO")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("TDESDE")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("ID")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsAlertas.Tables(0).Columns.Remove("OBSERVACIONES")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            'For Ind As Integer = 0 To dsAlertas.Tables(0).Columns.Count - 1
            '    Try
            '        If dsAlertas.Tables(0).Columns(Ind).ColumnName.ToUpper() Like "*OBD*" Then
            '            dsAlertas.Tables(0).Columns.RemoveAt(Ind)
            '        End If
            '    Catch ex As Exception
            '        Console.WriteLine(ex.Message)
            '    End Try
            'Next
            dsAlertas.AcceptChanges()
            TotalRegistros = dsAlertas.Tables(0).Rows.Count

            If TotalRegistros < 1000 Then
                Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                For Each dColum As DataColumn In dsAlertas.Tables(0).Columns
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
                dsAlertas.AcceptChanges()

                Console.ForegroundColor = ConsoleColor.DarkYellow
                Console.WriteLine(String.Format("Generando Archivo de {0} Filas", dsAlertas.Tables(0).Rows.Count))
                For Each oRow As DataRow In dsAlertas.Tables(0).Rows
                    Console.ForegroundColor = ConsoleColor.DarkGray
                    Console.WriteLine("Escribiendo Fila # " & Hoja.LastRowNum.ToString())
                    Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                    For Each dColum As DataColumn In dsAlertas.Tables(0).Columns
                        Celda = Fila.CreateCell(dColum.Ordinal)
                        'Hoja.AutoSizeColumn(dColum.Ordinal)
                        Celda.CellStyle.Alignment = HorizontalAlignment.Center
                        Celda.CellStyle = stlNormal

                        Celda.SetCellValue(oRow(dColum.ColumnName).ToString())
                    Next
                Next
                Console.WriteLine("Fin de Generacion de Archivo para Envio")
            Else
                Dim vnxls As New fGrid(dsAlertas, Archivo, CDbl(Parametros))
                vnxls.GenerarArchivo()
            End If
            dsAlertas = Nothing
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
