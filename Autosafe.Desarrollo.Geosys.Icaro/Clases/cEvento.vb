Imports System.Data.SqlClient
Imports System.Math
Imports System.IO
Imports System.IO.Packaging
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades

Public Class cEvento : Inherits cGeneracion

    Private ListadoArchivos As New ArrayList()
    Private dsHistorico As DataSet
    Private Const Reporte As String = "Reporte de Eventos de la Flota"
    Private tmpArchivo As String = ""

    Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String)
        MyBase.New(TipoReporte, IdUsuario, Intervalo, Parametros)

    End Sub

    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181112
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
        Console.WriteLine(String.Format("{0}: Obteniendo Datos para el Reporte Eventos", Datos.usuarioId))

        Try
            DatoTipo = Datos.tipoReporte.Split(";")
            DatoActivos = Datos.activos.Split(";")
            DatoEmail = Datos.email.Split(";")
            ListadoArchivos.Clear()

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
                    .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 14))
                End With

                'Me.Usuario = DBS25.getUsuarioxID(Datos.usuarioId)

                If Directory.Exists(String.Format("{0}Usuarios\{1}\{2}\Recorrido", System.AppDomain.CurrentDomain.BaseDirectory, DBS25.getUsuarioxID(Datos.usuarioId), Now.Date.ToShortDateString().Replace("/", ""))) Then
                    Try
                        Directory.Delete(String.Format("{0}Usuarios\{1}\{2}\Recorrido", System.AppDomain.CurrentDomain.BaseDirectory, DBS25.getUsuarioxID(Datos.usuarioId), Now.Date.ToShortDateString().Replace("/", "")), True)
                    Catch ex As Exception

                    End Try
                End If
                Directory.CreateDirectory(String.Format("{0}Usuarios\{1}\{2}\Recorrido", System.AppDomain.CurrentDomain.BaseDirectory, DBS25.getUsuarioxID(Datos.usuarioId), Now.Date.ToShortDateString().Replace("/", "")))



                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
                'PARA GUARDAR LOS EMAILS A QUIENES LES LLEGARA EL REPORTE GENERADO
                '******************************************************************************
                Me.Email = Datos.email
                '******************************************************************************

                GenerarReporte(Datos.usuarioId,
                              Datos.intervalo,
                              Datos.parametros,
                              sh)
            Else
                GuardarLog(Me.Tipo, "SINVEHICULOS")
                Console.WriteLine(String.Format("El usuario :  {0} no cuenta con vehiculos activos", Me.Usuario))

            End If



            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
            'PARA LLAMAR AL PROCESO ENVIARMAIL UNA VEZ SE HAYA GENERADO EL ARCHIVO EXCEL
            '******************************************************************************

            'Try
            '    EnviarMail(Datos.Email,
            '               "",
            '               "Adjunto Email con el Reporte Solicitado",
            '                Nothing,
            '                Reporte,
            '                Me.Tipo,
            '                Datos.IdUsuario,
            '                False,
            '                ListadoArchivos)
            'Catch ex As Exception
            '    ErroresEnvio &= "ERR_MAIL"
            'End Try
            '******************************************************************************

            Try
                wk.Close()
                wk = Nothing

                GC.Collect()
            Catch ex As Exception

            End Try

            DatoEmail = Nothing
            DatoActivos = Nothing
            DatoTipo = Nothing

            DBS25.Dispose()
            DBS25 = Nothing
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Function GenerarReporte(ByVal IdUsuario As Integer,
                               ByVal Intervalo As String,
                               ByVal Parametros As String,
                               ByVal Hoja As ISheet) As String
        Try
            Dim Columnas As String = ""
            Dim Archivo As String = "EventoFlota.xls"
            Dim dataO As ArrayList


            Dim tmpVelocidad As DataSet = Nothing

            tmpVelocidad = Nothing
            tmpVelocidad = New DataSet()

            dsHistorico = Nothing
            dsHistorico = New DataSet()

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

            Dim lista As List(Of ActivoEN) = negocio.ListarActivosSinReportar(obj)

            Dim Pagina As Integer = 1

            Dim iActivos As Integer = 0

            Dim nombreEvento As String = ""

            For Each tActivo In lista
                Try
                    dsHistorico = Nothing
                    dsHistorico = New DataSet()
                    Console.ForegroundColor = ConsoleColor.Magenta

                    '******************************************************************************
                    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                    'MOSTRAR MENSAJE EN LA CONSOLA CON PLACA, FECHAINICIO Y FECHAFIN
                    '******************************************************************************
                    'Console.WriteLine("Obteniendo Detalle Eventos de: " & tActivo.Alias)
                    Console.WriteLine(String.Format("Obteniendo Eventos de:  {0} {1} {2}", tActivo.alias, Fecha2ANSI(Me.FechaInicial.ToString(), True), Fecha2ANSI(Me.FechaFinal.ToString(), True)))
                    '******************************************************************************

                    Dim arrParametros As String()

                    arrParametros = Parametros.Split(";")

                    If arrParametros.Length > 1 Then
                        nombreEvento = Parametros.Split(";")(2)
                    Else
                        nombreEvento = Parametros
                    End If

                    Dim sql As String

                    If Intervalo Like "*H" Then

                        '******************************************************************************
                        'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                        'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
                        '******************************************************************************
                        sql = String.Format("spActivoRecorridoRConsultar2 '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'",
                               tActivo.vid,
                               "CUS",
                               "C",
                               IdUsuario,
                               "127005",
                               Fecha2ANSI(Me.FechaInicial.ToString(), False),
                               Fecha2ANSI(Me.FechaFinal.ToString(), False),
                               0)

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

                            sql = String.Format("spActivoRecorridoRConsultar2_Evento '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'",
                               tActivo.vid,
                               "CUS",
                               "C",
                               IdUsuario,
                               "127005",
                               Fecha2ANSI(Me.FechaInicial.ToString(), True) & " " & Me.HoraInicio,
                               Fecha2ANSI(Me.FechaFinal.ToString(), True) & " " & Me.HoraFin,
                               nombreEvento)

                        Else
                            sql = String.Format("spActivoRecorridoOConsultar4 '{0}','{1}','{2}'",
                               tActivo.vid,
                               Fecha2ANSI(Me.FechaInicial.ToString(), True) & " 00:00:00",
                               Fecha2ANSI(Me.FechaFinal.ToString(), True) & " 23:59:59")
                        End If

                        DbDataAdapter = New SqlDataAdapter(sql, DbConnection)
                        '******************************************************************************

                    End If

                    DbDataAdapter.SelectCommand.CommandTimeout = 3000000
                    DbDataAdapter.Fill(tmpVelocidad, "Historico")
                    tmpVelocidad.AcceptChanges()

                    dsHistorico.Merge(tmpVelocidad)

                    iActivos = iActivos + 1

                Catch ex As Exception
                    Console.Write(ex.Message)
                End Try
            Next

            dsHistorico.AcceptChanges()

            For Each TmpRow As DataRow In dsHistorico.Tables(0).Rows

                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                'PARA NO CONSULTAR A LA BASE DE DATOS AL OBTENER EL NOMBRE DE EVENTO
                'ESTO YA VIENE CON LA CONSULTA AL SP
                '******************************************************************************

                'TmpRow("DEvento") = DBS.getDescripcionEvento(TmpRow("vid"), TmpRow("Input_Event"))
                '******************************************************************************

                Try
                    TmpRow("Placa") = TmpRow("Placa")
                Catch ex As Exception
                    TmpRow("Placa") = TmpRow("Placa")
                End Try




                Try
                    'Select Case CType(Session("ConfigUsuario"), ConfigUsuario).UnidadVelocidad.ToString()
                    '    Case "Nm/H"
                    '        TmpRow("Speed") = MillasAMillasNauticas(TmpRow("Speed"))
                    '    Case "Km/H"
                    '        TmpRow("Speed") = MillasAKilometros(TmpRow("Speed"))
                    '    Case "Mi/H"
                    '        TmpRow("Speed") = TmpRow("Speed")
                    'End Select
                    'TmpRow("Speed") = MillasAKilometros(TmpRow("Speed"))
                    TmpRow("Speed") = TmpRow("Speed")
                Catch ex As Exception
                    TmpRow("Speed") = TmpRow("Speed")
                End Try

                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                'PARA MOSTRAR EL KILOMETRAJE SIN NINGUNA CONVERSION
                'ESTO YA VIENE CON LA CONSULTA AL SP
                '******************************************************************************
                'Try
                '    TmpRow("Kilometraje") = Math.Round(TmpRow("Kilometraje") / 1000, 0)
                'Catch ex As Exception

                'End Try
                '******************************************************************************




                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                ''PARA MODIFICAR EL VALOR ENVIADO EN EA1, EA2 Y EA3
                '******************************************************************************
                Try
                    TmpRow("EA1") = TmpRow("tEA1")
                Catch ex As Exception
                    TmpRow("EA1") = TmpRow("EA1")
                End Try

                Try
                    TmpRow("EA2") = TmpRow("tEA2")
                Catch ex As Exception
                    TmpRow("EA2") = TmpRow("EA2")
                End Try

                Try
                    TmpRow("EA3") = TmpRow("tEA3")
                Catch ex As Exception
                    TmpRow("EA3") = TmpRow("EA3")
                End Try
                '******************************************************************************


                Try
                    TmpRow("Heading") = GetCourse(TmpRow("Heading"))
                Catch ex As Exception
                    TmpRow("Heading") = TmpRow("Heading")
                End Try

                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
                'PARA AGREGAR NUEVAS COLUMNAS
                '******************************************************************************
                'Try
                '    TmpRow("Date_Time") = DateAdd(DateInterval.Hour, -5, TmpRow("Date_Time"))
                'Catch ex As Exception
                '    TmpRow("Date_Time") = TmpRow("Date_Time")
                'End Try


                Try
                    TmpRow("EstadoGPS") = GetGPS_Status(TmpRow("EstadoGPS"))
                Catch ex As Exception
                    TmpRow("EstadoGPS") = TmpRow("EstadoGPS")
                End Try
                '******************************************************************************

                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
                'PARA MODIFICAR EL VALOR ENVIADO EN PUNTO CERCANO
                '******************************************************************************

                Dim position As Integer
                position = InStr(TmpRow("Pto. Cercano1"), "*")
                position = position - 1

                Try
                    TmpRow("Pto. Cercano1") = TmpRow("Pto. Cercano1").Substring(0, position)
                Catch ex As Exception
                    TmpRow("Pto. Cercano1") = TmpRow("Pto. Cercano1")
                End Try
                '******************************************************************************

                TmpRow.AcceptChanges()
            Next
            dsHistorico.AcceptChanges()

            Try
                dsHistorico.Tables(0).Columns.Remove("Pto. Cercano")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("FechaHoraServidor")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("Etiqueta")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("Enlace")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("Script")
            Catch ex As Exception

            End Try


            Try
                dsHistorico.Tables(0).Columns.Remove("ScriptL")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("MostrarColumnaKilometraje")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("MostrarColumnabCE")
            Catch ex As Exception

            End Try


            Try
                dsHistorico.Tables(0).Columns.Remove("IdActivo")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("FechaHoraL")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("IndGrafico")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("Script2")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("Script3")
            Catch ex As Exception

            End Try







            Try
                dsHistorico.Tables(0).Columns.Remove("tDesde")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tKilometraje")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("Numero")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("VID")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tLatitud")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tLongitud")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("Cod. Evento")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("GPS_Status")
            Catch ex As Exception

            End Try
            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR NUEVAS COLUMNAS AL REPORTE
            '******************************************************************************
            'Try
            '    dsHistorico.Tables(0).Columns.Remove("tHorometro")
            'Catch ex As Exception

            'End Try
            '******************************************************************************
            Try
                dsHistorico.Tables(0).Columns.Remove("Nivel Bateria")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tNivel Bateria")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tDate_Time")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tEstadoGPS")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("GPS_Status")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("Input_Event")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tEstado GPS")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("Horometro")
            Catch ex As Exception

            End Try
            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR NUEVAS COLUMNAS AL REPORTE
            '******************************************************************************
            'Try
            '    dsHistorico.Tables(0).Columns.Remove("tHorometro")
            'Catch ex As Exception

            'End Try
            '******************************************************************************
            Try
                dsHistorico.Tables(0).Columns.Remove("Nivel Bateria")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tNivel Bateria")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("Voltaje Bateria")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tVoltaje Bateria")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("Voltaje Alimentacion")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tVoltajeAlimentacion")
            Catch ex As Exception

            End Try

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR NUEVAS COLUMNAS AL REPORTE
            '******************************************************************************
            'Try
            '    dsHistorico.Tables(0).Columns.Remove("EA1")
            'Catch ex As Exception

            'End Try

            'Try
            '    dsHistorico.Tables(0).Columns.Remove("EA2")
            'Catch ex As Exception

            'End Try

            'Try
            '    dsHistorico.Tables(0).Columns.Remove("EA3")
            'Catch ex As Exception

            'End Try
            '******************************************************************************
            Try
                dsHistorico.Tables(0).Columns.Remove("tEA1")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tEA2")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tEA3")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("SA1")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("SA2")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("SA3")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tSA1")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tSA2")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("tSA3")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("CE")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("TNIVELBATERIA")
            Catch ex As Exception

            End Try
            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR NUEVAS COLUMNAS AL REPORTE
            '******************************************************************************
            'Try
            '    dsHistorico.Tables(0).Columns.Remove("DRIVERID")
            'Catch ex As Exception

            'End Try
            '******************************************************************************

            Try
                dsHistorico.Tables(0).Columns.Remove("TVOLTAJEBATERIA")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("EVENTO")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("VELOCIDADOBD")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("RPMOBD")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("POSICIONACELERADOROBD")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("ODOMETROVIAJEOBD")
            Catch ex As Exception

            End Try


            Try
                dsHistorico.Tables(0).Columns.Remove("ODOMETROOBD")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("RPMOBD")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("NIVELGASOLINAOBD")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("COMBUSTIBLERESTANTEOBD")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("ENGRANETRANSMISIONOBD")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("TEMPERATURAREFRIGERANTEOBD")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("INDICEGASOLINAOBD")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("VOLTAJEALIMENTACIONOBD")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("ESTADOSEÑALESGIROOBD")
            Catch ex As Exception

            End Try

            Try
                dsHistorico.Tables(0).Columns.Remove("GASOLINACONSUMIDAPORVIAJEOBD")
            Catch ex As Exception

            End Try
            dsHistorico.AcceptChanges()

            Console.WriteLine("Filas a Generar: " & dsHistorico.Tables(0).Rows.Count.ToString())
            Console.ForegroundColor = ConsoleColor.Cyan

            If iActivos > 0 Then

                TotalRegistros = dsHistorico.Tables(0).Rows.Count

                If TotalRegistros < My.Settings.TotalRegistrosMaximo Then
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
                            Case "LATITUDE"
                                Celda.SetCellValue("LATITUD")
                            Case "LOOGITUDE"
                                Celda.SetCellValue("LONGITUD")
                            Case "DEVENTO"
                                Celda.SetCellValue("EVENTO")
                            Case "VID"
                                Celda.SetCellValue("PLACA")
                                '******************************************************************************
                                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180928
                                'PARA AGREGAR CAMPO ESTADOGPS AL REPORTE DE VELOCIDAD
                                '******************************************************************************
                            Case "EstadoGPS"
                                Celda.SetCellValue("ESTADO GPS")

                                '******************************************************************************
                            Case Else
                                Celda.SetCellValue(dColum.ColumnName.ToUpper())
                        End Select

                        Celda.CellStyle = stlNada
                    Next
                    dsHistorico.AcceptChanges()

                    Console.ForegroundColor = ConsoleColor.DarkYellow
                    Console.WriteLine(String.Format("Generando Archivo de {0} Filas", dsHistorico.Tables(0).Rows.Count))


                    For Each oRow As DataRow In dsHistorico.Tables(0).Rows
                        If oRow("DEvento") = nombreEvento Then
                            Console.ForegroundColor = ConsoleColor.DarkGray
                            Console.WriteLine(String.Format("Escribiendo Fila # {0}", Hoja.LastRowNum))
                            Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                            For Each dColum As DataColumn In dsHistorico.Tables(0).Columns
                                Celda = Fila.CreateCell(dColum.Ordinal)
                                Celda.CellStyle.Alignment = HorizontalAlignment.Center
                                Celda.CellStyle = stlNormal

                                Celda.SetCellValue(oRow(dColum.ColumnName).ToString())
                            Next
                        End If
                    Next

                    For Each dColum As DataColumn In dsHistorico.Tables(0).Columns
                        Try
                            Hoja.AutoSizeColumn(dColum.Ordinal)
                        Catch ex As Exception
                            Console.WriteLine(ex.Message)
                        End Try
                    Next

                    Console.WriteLine("Fin de Generacion de Archivo para Envio")
                Else
                    Dim vnxls As fGrid

                    If IsNumeric(Parametros) Then
                        vnxls = New fGrid(dsHistorico, Archivo, CDbl(Parametros))
                    Else
                        vnxls = New fGrid(dsHistorico, Archivo, CDbl(Parametros.Split(";")(2)))
                    End If

                    vnxls.GenerarArchivo()
                End If
                dsHistorico = Nothing





            End If


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



            dsHistorico = Nothing

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
            'PARA LLAMAR AL PROCESO ENVIARMAIL UNA VEZ SE HAYA GENERADO EL ARCHIVO EXCEL
            '******************************************************************************
            'Try
            '    EnviarMail(Me.Email,
            '               "",
            '               "Adjunto Email con el Reporte Solicitado",
            '                Nothing,
            '                Reporte,
            '                Me.Tipo,
            '                Me.IdUsuario,
            '                False,
            '                ListadoArchivos)
            'Catch ex As Exception
            '    ErroresEnvio &= "ERR_MAIL"
            'End Try
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
