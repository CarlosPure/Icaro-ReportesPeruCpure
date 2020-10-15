Imports System.Data.SqlClient
Imports System.Math
Imports System.IO
Imports System.IO.Packaging

'******************************************************************************
'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181115
'******************************************************************************
Imports NPOI.SS.UserModel
Imports NPOI.HSSF.UserModel
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades

Public Class cHistoricoMulti : Inherits cGeneracion

    Private ListadoArchivos As New ArrayList()
    Private dsHistorico As DataSet
    Private Const Reporte As String = "Reporte de Historico de la Flota"
    Private tmpArchivo As String = ""

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
        Console.WriteLine(String.Format("{0}: Obteniendo Datos para el Reporte Historico", Datos.usuarioId))

        Try
            DatoTipo = Datos.tipoReporte.Split(";")
            DatoActivos = Datos.activos.Split(";")
            DatoEmail = Datos.email.Split(";")
            ListadoArchivos.Clear()

            'Me.Usuario = DBS25.getUsuarioxID(Datos.IdUsuario)

            Dim negocio As New ActivoBL()
            Dim obj As New ActivoEN
            obj.baseDatosId = 25
            obj.usuarioId = CInt(IdUsuario)
            obj.entidadId = "0"

            Dim cantidad As Integer = negocio.ListarVehiculosActivos(obj)

            If cantidad > 0 Then
                Dim nombreDirectorio As String


                nombreDirectorio = String.Format("{0}Usuarios\{1}\{2}\Recorrido", System.AppDomain.CurrentDomain.BaseDirectory, Me.Usuario, Now.Date.ToShortDateString().Replace("/", ""))

                If Directory.Exists(nombreDirectorio) Then
                    Try
                        Directory.Delete(nombreDirectorio, True)
                    Catch ex As Exception

                    End Try
                End If
                Directory.CreateDirectory(nombreDirectorio)



                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
                'PARA GUARDAR LOS EMAILS A QUIENES LES LLEGARA EL REPORTE GENERADO
                '******************************************************************************
                Me.Email = Datos.email
                '******************************************************************************

                GenerarReporte(Datos.usuarioId,
                              Datos.intervalo,
                              Datos.parametros)
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
            Dim Archivo As String = "HistoricoFlotaMulti.xls"
            Dim dataO As ArrayList

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
            For Each tActivo In lista
                Try
                    dsHistorico = Nothing
                    dsHistorico = New DataSet()
                    Console.ForegroundColor = ConsoleColor.Magenta

                    '******************************************************************************
                    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                    'MOSTRAR MENSAJE EN LA CONSOLA CON PLACA, FECHAINICIO Y FECHAFIN
                    '******************************************************************************
                    'Console.WriteLine("Obteniendo Historico de: " & tActivo.Alias)
                    Console.WriteLine(String.Format("Obteniendo Historico de:  {0} {1} {2}", tActivo.alias, Fecha2ANSI(Me.FechaInicial.ToString(), True), Fecha2ANSI(Me.FechaFinal.ToString(), True)))
                    '******************************************************************************

                    Dim sql As String

                    If Intervalo Like "*H" Then
                        '******************************************************************************
                        'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                        'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
                        'PARA CAMBIAR EL NOMBRE DEL PROCEDIMIENTO LLAMADO
                        'PARA LLAMAR AL PROCEDIMIENTO DE ACUERDO A LA VARIABLE PAIS
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
                        '******************************************************************************

                        If (My.Settings.Pais = "PE") Then
                            sql = String.Format("spActivoRecorridoRConsultar2_E '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'",
                                tActivo.vid,
                               "CUS",
                               "C",
                               IdUsuario,
                               "127005",
                               String.Format("{0} {1}", Fecha2ANSI(Me.FechaInicial.ToString(), True), Me.HoraInicio),
                               String.Format("{0} {1}", Fecha2ANSI(Me.FechaFinal.ToString(), True), Me.HoraFin),
                               0)
                        Else
                            sql = String.Format("spActivoRecorridoRConsultar2 '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'",
                               tActivo.vid,
                               "CUS",
                               "C",
                               IdUsuario,
                               "127005",
                               String.Format("{0} 00:00:00", Fecha2ANSI(Me.FechaInicial.ToString(), True)),
                               String.Format("{0} 23:59:59", Fecha2ANSI(Me.FechaFinal.ToString(), True)),
                               0)
                        End If




                        DbDataAdapter = New SqlDataAdapter(String.Format(sql), DbConnection)

                        '******************************************************************************
                    End If

                    DbDataAdapter.SelectCommand.CommandTimeout = 3000000
                    DbDataAdapter.Fill(dsHistorico, "Historico")
                    dsHistorico.AcceptChanges()

                    dsHistorico.AcceptChanges()

                    For Each TmpRow As DataRow In dsHistorico.Tables(0).Rows

                        '******************************************************************************
                        'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                        'PARA NO CONSULTAR A LA BASE DE DATOS AL OBTENER EL NOMBRE DE EVENTO
                        'PARA MOSTRAR LA VELOCIDAD SIN NINGUNA CONVERSION
                        'PARA MOSTRAR EL KILOMETRAJE SIN NINGUNA CONVERSION
                        'ESTO YA VIENE CON LA CONSULTA AL SP
                        '******************************************************************************

                        'TmpRow("DEvento") = DBS.getDescripcionEvento(TmpRow("vid"), TmpRow("Input_Event"))




                        'Try
                        '    'Select Case CType(Session("ConfigUsuario"), ConfigUsuario).UnidadVelocidad.ToString()
                        '    '    Case "Nm/H"
                        '    '        TmpRow("Speed") = MillasAMillasNauticas(TmpRow("Speed"))
                        '    '    Case "Km/H"
                        '    '        TmpRow("Speed") = MillasAKilometros(TmpRow("Speed"))
                        '    '    Case "Mi/H"
                        '    '        TmpRow("Speed") = TmpRow("Speed")
                        '    'End Select
                        '    TmpRow("Speed") = MillasAKilometros(TmpRow("Speed"))
                        'Catch ex As Exception
                        '    TmpRow("Speed") = TmpRow("Speed")
                        'End Try

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
                    'SE MANTIENE LA COLUMNA THOROMETRO
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
                    'SE MANTIENEN COLUMNAS 
                    'EA1
                    'EA2
                    'EA3
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
                    'SE MANTIENE COLUMNA DRIVERID
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



                    '******************************************************************************
                    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180928
                    'PARA QUITAR NUEVAS COLUMNAS AL REPORTE
                    '******************************************************************************
                    Try
                        dsHistorico.Tables(0).Columns.Remove("CE")
                    Catch ex As Exception

                    End Try

                    Try
                        dsHistorico.Tables(0).Columns.Remove("FECHAHORASERVIDOR")
                    Catch ex As Exception

                    End Try
                    Try
                        dsHistorico.Tables(0).Columns.Remove("ETIQUETA")
                    Catch ex As Exception

                    End Try
                    Try
                        dsHistorico.Tables(0).Columns.Remove("ENLACE")
                    Catch ex As Exception

                    End Try
                    Try
                        dsHistorico.Tables(0).Columns.Remove("FECHAHORAL")
                    Catch ex As Exception

                    End Try
                    Try
                        dsHistorico.Tables(0).Columns.Remove("INDGRAFICO")
                    Catch ex As Exception

                    End Try
                    Try
                        dsHistorico.Tables(0).Columns.Remove("SCRIPT")
                    Catch ex As Exception

                    End Try
                    Try
                        dsHistorico.Tables(0).Columns.Remove("SCRIPT2")
                    Catch ex As Exception

                    End Try
                    Try
                        dsHistorico.Tables(0).Columns.Remove("SCRIPT3")
                    Catch ex As Exception

                    End Try
                    Try
                        dsHistorico.Tables(0).Columns.Remove("SCRIPTL")
                    Catch ex As Exception

                    End Try
                    Try
                        dsHistorico.Tables(0).Columns.Remove("MOSTRARCOLUMNAKILOMETRAJE")
                    Catch ex As Exception

                    End Try
                    Try
                        dsHistorico.Tables(0).Columns.Remove("MOSTRARCOLUMNABCE")
                    Catch ex As Exception

                    End Try
                    Try
                        dsHistorico.Tables(0).Columns.Remove("ALTITUD")
                    Catch ex As Exception

                    End Try
                    Try
                        dsHistorico.Tables(0).Columns.Remove("IDACTIVO")
                    Catch ex As Exception

                    End Try

                    Try
                        dsHistorico.Tables(0).Columns.Remove("Pto. Cercano")
                    Catch ex As Exception

                    End Try

                    '******************************************************************************

                    dsHistorico.AcceptChanges()

                    Console.WriteLine("Filas a Generar: " & dsHistorico.Tables(0).Rows.Count.ToString())
                    Console.ForegroundColor = ConsoleColor.Cyan

                    tmpArchivo = ""
                    Try
                        tmpArchivo = tActivo.alias.Split(" [")(0)
                    Catch ex As Exception
                        tmpArchivo = tActivo.alias
                    End Try

                    If tmpArchivo <> "" Then

                        '******************************************************************************
                        'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180930
                        'PARA AGREGAR FECHAINICIAL Y FECHAFINAL AL NOMBRE DE CADA ARCHIVO EXCEL
                        'PARA CREAR UN ARCHIVO EXCEL CUANDO EL VEHICULO PRESENTE REGISTROS
                        'PARA CREAR UN ARCHIVO DE TEXTO CUANDO EL VEHICULO NO PRESENTE REGISTROS
                        '******************************************************************************

                        Dim nombreArchivo As String

                        If dsHistorico.Tables(0).Rows.Count > 0 Then

                            'nombreArchivo = System.AppDomain.CurrentDomain.BaseDirectory & "Usuarios\" & Me.Usuario & "\" & Now.Date.ToShortDateString().Replace("/", "") & "\Recorrido\" & tmpArchivo & ".xls"
                            nombreArchivo = String.Format("{0}Usuarios\{1}\{2}\Recorrido\{3}.xls",
                                                          System.AppDomain.CurrentDomain.BaseDirectory,
                                                          Me.Usuario,
                                                          Now.Date.ToShortDateString().Replace("/", ""),
                                                          tmpArchivo,
                                                          Fecha2ANSI(FechaInicial.ToString(), True),
                                                          Fecha2ANSI(FechaFinal.ToString(), True))


                            '******************************************************************************
                            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181115
                            'PARA LLAMAR AL PROCESO GENERAREXCEL PARA DARLE FORMATO A CADA ARCHIVO EXCEL
                            '******************************************************************************

                            'Dim vn As New fHistorico(dsHistorico, nombreArchivo)

                            'vn.GenerarArchivo()
                            'vn.Dispose()
                            'vn = Nothing

                            GenerarExcel(tmpArchivo)

                            If wk.NumberOfSheets > 0 Then
                                Dim file As FileStream = New FileStream(nombreArchivo, FileMode.Create)
                                wk.Write(file)
                                file.Close()
                                file.Dispose()
                                file = Nothing
                                'wk.Close()
                                'wk = Nothing
                                wk.Clear()

                            End If
                            '******************************************************************************

                            ListadoArchivos.Add(nombreArchivo)


                        Else

                            'nombreArchivo = System.AppDomain.CurrentDomain.BaseDirectory & "Usuarios\" & Me.Usuario & "\" & Now.Date.ToShortDateString().Replace("/", "") & "\Recorrido\" & tmpArchivo & ".txt"
                            nombreArchivo = String.Format("{0}Usuarios\{1}\{2}\Recorrido\{3}.txt",
                                                          System.AppDomain.CurrentDomain.BaseDirectory,
                                                          Me.Usuario,
                                                          Now.Date.ToShortDateString().Replace("/", ""),
                                                          tmpArchivo,
                                                          Fecha2ANSI(FechaInicial.ToString(), True),
                                                          Fecha2ANSI(FechaFinal.ToString(), True))

                            Dim ArchivoTEXT As String = nombreArchivo
                            Dim fileTEXT As System.IO.StreamWriter
                            fileTEXT = My.Computer.FileSystem.OpenTextFileWriter(ArchivoTEXT, False)
                            fileTEXT.Write("El usuario no cuenta con reportes para este vehiculo en el periodo de tiempo indicado.")
                            fileTEXT.Close()

                            ListadoArchivos.Add(ArchivoTEXT)

                        End If
                        '******************************************************************************


                    End If

                    tmpArchivo = ""
                    Console.WriteLine("Registros Generados para " & tActivo.alias.ToUpper())
                Catch ex As Exception
                    Console.Write(ex.Message)
                End Try
            Next
            dsHistorico = Nothing

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
            'PARA LLAMAR AL PROCESO ENVIARMAIL UNA VEZ SE HAYA GENERADO EL ARCHIVO EXCEL
            '******************************************************************************
            Try
                EnviarMail(Me.Email,
                           "",
                           "Adjunto Email con el Reporte Solicitado",
                            Nothing,
                            Reporte,
                            Me.Tipo,
                            Me.IdUsuario,
                            False,
                            ListadoArchivos)
            Catch ex As Exception
                ErroresEnvio &= "ERR_MAIL"
            End Try
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

    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181115
    'PARA DARLE FORMATO A CADA ARCHIVO EXCEL
    '******************************************************************************
    Public Sub GenerarExcel(ByVal Placa As String)

        Dim Hoja As ISheet
        Dim Pagina As Integer = 1


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

        Hoja = Nothing
        Hoja = wk.CreateSheet(Placa.Replace("[", "(").Replace("]", ")"))
        With Hoja
            .CreateRow(0).CreateCell(0).SetCellValue(Reporte)
            .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 14))
        End With

        Hoja.Header.Center = "Reporte Historico Flota"
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
                    tmpHoja = wk.CreateSheet(String.Format("{0} ({1})", Placa.Replace("[", "(").Replace("]", ")"), Pagina))
                    Pagina += 1
                    NumFila = 1
                    With tmpHoja
                        .CreateRow(0).CreateCell(0).SetCellValue(Reporte)
                        .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 14))
                    End With

                    tmpHoja.Header.Center = "Reporte Historico Flota"
                    Fila = tmpHoja.CreateRow(1)
                    Fila.CreateCell(0).SetCellValue(String.Format("Desde: {0} 00:00:00", Fecha2ANSI(Me.FechaInicial.ToString(), True)))
                    Fila = tmpHoja.CreateRow(2)
                    Fila.CreateCell(0).SetCellValue(String.Format("Hasta: {0} 23:59:59", Fecha2ANSI(Me.FechaFinal.ToString(), True)))
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
        Console.WriteLine("Registros Generados para " & Placa.ToUpper())
    End Sub

End Class
