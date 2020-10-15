Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports System.Net
Imports System.Text
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades


Public Class cResumen : Inherits cGeneracion

    Private dsEjecutivo As DataSet
    Private dsVariable As DataSet
    Private Const Reporte As String = "Reporte Ejecutivo de la Flota"

    Dim sCabecera As String
    'Dim IdEntidad As String


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


    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 201801105
    'PARA AGREGAR NUEVAS COLUMNAS
    'NOMBRE
    'SECUENCIA
    'HORAINICIO
    'HORAFIN
    'IDENTIDAD
    '******************************************************************************
    Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String, ByVal Nombre As String, ByVal Secuencia As String, ByVal HoraInicio As String, ByVal HoraFin As String, ByVal Usuario As String, ByVal IdEntidad As String)
        MyBase.New(TipoReporte, IdUsuario, Intervalo, Parametros, Nombre, Secuencia, HoraInicio, HoraFin, Usuario, IdEntidad)
    End Sub
    '******************************************************************************

    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 201801105
    'PARA AGREGAR NUEVAS COLUMNAS
    'NOMBRE
    'SECUENCIA
    'HORAINICIO
    'HORAFIN
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20200331
    'PARA AGREGAR NUEVA COLUMNA
    'USUARIO
    '******************************************************************************
    'IDENTIDAD
    'EVENTO
    '******************************************************************************
    Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String, ByVal Nombre As String, ByVal Secuencia As String, ByVal HoraInicio As String, ByVal HoraFin As String, ByVal Usuario As String, ByVal IdEntidad As String, ByVal Evento As String)
        MyBase.New(TipoReporte, IdUsuario, Intervalo, Parametros, Nombre, Secuencia, HoraInicio, HoraFin, Usuario, IdEntidad, Evento)
    End Sub
    '******************************************************************************

    Public Overrides Sub Generar(Datos As Object)
        Console.WriteLine(Datos.usuarioId.ToString() & ": Obteniendo Datos para el Reporte Resumen")

        Try
            DatoTipo = Datos.tipoReporte.Split(";")
            DatoActivos = Datos.activos.Split(";")
            DatoEmail = Datos.email.Split(";")

            Dim sh As ISheet = wk.CreateSheet("Flota")
            With sh
                .CreateRow(0).CreateCell(0).SetCellValue(Reporte)
                .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 14))
            End With

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
            'PARA GUARDAR LOS EMAILS A QUIENES LES LLEGARA EL REPORTE GENERADO
            'PARA GUARDAR EL NOMBRE DE ARCHIVO
            'PARA GUARDAR EL IDENTIDAD
            '******************************************************************************
            Me.Email = Datos.email
            'Me.Archivo = String.Format("{0}_{1}.pdf", Me.Tipo, DBS25.getUsuarioxID(IdUsuario))
            Me.Archivo = String.Format("{0}_{1}.pdf", Me.Tipo, Me.IdUsuario)
            'Me.IdEntidad = DBS25.getIdEntidadxSecuencia(Me.Secuencia)
            'Me.Usuario = DBS25.getUsuarioxID(Datos.IdUsuario)
            '******************************************************************************

            Dim correo As String

            'For Each correo In DatoEmail
            Dim negocio As New ActivoBL()
            Dim obj As New ActivoEN
            obj.baseDatosId = 25
            obj.usuarioId = CInt(IdUsuario)
            obj.entidadId = Me.IdEntidad

            Dim cantidad As Integer = negocio.ListarVehiculosActivos(obj)

            If cantidad > 0 Then
                GenerarReporte(Datos.usuarioId, Datos.intervalo, Datos.parametros, sh, "aaaaaa")
            Else
                GuardarLog(Me.Tipo, "SINVEHICULOS")
                Console.WriteLine(String.Format("El usuario :  {0} no cuenta con vehiculos activos", Me.Usuario))

            End If

            'Next

            wk = Nothing
            DatoEmail = Nothing
            DatoActivos = Nothing
            DatoTipo = Nothing

            'DBS25.Dispose()
            'DBS25 = Nothing
        Catch ex As Exception
            Console.WriteLine(ex.Message + " - Procedimiento Generar")
        End Try
    End Sub

    Private Function GenerarReporte(ByVal IdUsuario As Integer,
                               ByVal Intervalo As String,
                               ByVal Parametros As String,
                               ByVal Hoja As ISheet,
                               ByVal Correo As String
                               ) As String

        Dim err_mensaje As String = ""

        Try

            DbConnection = Nothing
            DbConnection = New SqlConnection(My.Settings.GEOSYSConnectionStringAux)

            If DbConnection.State = ConnectionState.Open Then
                DbConnection.Close()
            End If
            DbConnection.Open()

            DbDataAdapter = Nothing

            dsEjecutivo = Nothing
            dsEjecutivo = New DataSet()

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'SE COMENTO LINEA PARA QUE AL PRESENTARSE UN ERROR NO MANEJE EL ERROR Y NO ENVIE CORREO
            '******************************************************************************
            'Try
            '******************************************************************************

            Console.ForegroundColor = ConsoleColor.Magenta

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181218
            'MOSTRAR MENSAJE EN LA CONSOLA CON FECHAINICIO Y FECHAFIN
            '******************************************************************************
            'Console.WriteLine("Obteniendo Kilometraje de la Flota: ")
            Console.WriteLine(String.Format("Obteniendo Reporte Resumen Emblue: {0} {1}", Fecha2ANSI(Me.FechaInicial.ToString(), True), Fecha2ANSI(Me.FechaFinal.ToString(), True)))
            '******************************************************************************





            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
            'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
            'PARA LLAMAR AL PROCEDIMIENTO DE ACUERDO A LA VARIABLE PAIS
            'PARA UTILIZAR OTRO PROCEDIMIENTO ALMACENADO QUE DEVUELVE LOS VALORES DE LAS PLACAS EN FORMA HORIZONTAL
            '******************************************************************************

            Dim sql As String

            If (My.Settings.Pais = "PE") Then
                sql = String.Format("dbo.SP_PDF_ResumenEjecutivoIcaro '{0}','{1}','{2}','{3}'",
                                    Me.IdEntidad,
                               Fecha2ANSI(Me.FechaInicial.ToString(), False),
                               Fecha2ANSI(Me.FechaFinal.ToString(), False),
                               Me.IdUsuario
                               )
            Else
                sql = String.Format("[sp_maf_reporte_extranet]")
            End If


            DbDataAdapter = New SqlDataAdapter(sql, DbConnection)
            '******************************************************************************




            If My.Settings.Debug = True Then
                Console.WriteLine(DbDataAdapter.SelectCommand.CommandText)
            End If
            DbDataAdapter.SelectCommand.CommandTimeout = 3000000
            DbDataAdapter.Fill(dsEjecutivo, "Kilometraje")
            Console.WriteLine("Registros Obtenidos para la Flota " & dsEjecutivo.Tables(0).Rows.Count())
            dsEjecutivo.AcceptChanges()

            If dsEjecutivo.Tables(0).Rows.Count = 1 Then

                Dim objResumen As New ResumenEjecutivoUnoEN()

                For Each TmpRow As DataRow In dsEjecutivo.Tables(0).Rows

                    Try
                        objResumen.Placa = TmpRow("Placa")
                        If String.IsNullOrEmpty(objResumen.Placa) Then
                            objResumen.Placa = "S/P"
                        End If
                    Catch ex As Exception
                        objResumen.Placa = ""
                    End Try

                    Try
                        objResumen.Kilometraje = TmpRow("Distancia")
                        If String.IsNullOrEmpty(objResumen.Kilometraje) Then
                            objResumen.Kilometraje = "0 kms"
                        Else
                            objResumen.Kilometraje &= " kms."
                        End If
                    Catch ex As Exception
                        objResumen.Kilometraje = ""
                    End Try

                    Try
                        objResumen.Aceleracion = TmpRow("# Aceleraciones")
                        If String.IsNullOrEmpty(objResumen.Aceleracion) Then
                            objResumen.Aceleracion = "0"
                        End If
                    Catch ex As Exception
                        objResumen.Aceleracion = ""
                    End Try

                    Try
                        objResumen.EstadoBateria = TmpRow("EstadoBateria")
                        If String.IsNullOrEmpty(objResumen.EstadoBateria) Then
                            objResumen.EstadoBateria = ""
                        End If
                    Catch ex As Exception
                        objResumen.EstadoBateria = ""
                    End Try

                    Try
                        objResumen.Distancia = TmpRow("Distancia Promedio")
                        If String.IsNullOrEmpty(objResumen.Distancia) Then
                            objResumen.Distancia = "0 kms"
                        Else
                            objResumen.Distancia &= " kms."
                        End If
                    Catch ex As Exception
                        objResumen.Distancia = ""
                    End Try

                    Try
                        objResumen.VelocidadExceso = TmpRow("# Exc Velocidad")
                        If String.IsNullOrEmpty(objResumen.VelocidadExceso) Then
                            objResumen.VelocidadExceso = "0"
                        End If
                    Catch ex As Exception
                        objResumen.VelocidadExceso = ""
                    End Try

                    Try
                        objResumen.Frenado = TmpRow("# Frenados")
                        If String.IsNullOrEmpty(objResumen.Frenado) Then
                            objResumen.Frenado = "0"
                        End If
                    Catch ex As Exception
                        objResumen.Frenado = ""
                    End Try

                    Try
                        objResumen.Horas = TmpRow("# Uso Total")
                        If String.IsNullOrEmpty(objResumen.Horas) Then
                            objResumen.Horas = "0 hrs."
                        Else
                            objResumen.Horas &= " hrs."
                        End If
                    Catch ex As Exception
                        objResumen.Horas = ""
                    End Try

                    Try
                        objResumen.Trayectos = TmpRow("# Trayectos")
                        If String.IsNullOrEmpty(objResumen.Trayectos) Then
                            objResumen.Trayectos = "0"
                        End If
                    Catch ex As Exception
                        objResumen.Trayectos = ""
                    End Try

                    Try
                        objResumen.VelocidadPromedio = TmpRow("Velocidad Promedio")
                        If String.IsNullOrEmpty(objResumen.VelocidadPromedio) Then
                            objResumen.VelocidadPromedio = "0 km/h"
                        Else
                            objResumen.VelocidadPromedio &= " km/h"
                        End If
                    Catch ex As Exception
                        objResumen.VelocidadPromedio = ""
                    End Try
                    '******************************************************************************

                    objResumen.MesAnio = MonthName(FechaInicial.Month, False) & " " & FechaInicial.Year
                    objResumen.Dia = getIntervaloDias(FechaInicial.Month, Intervalo)
                    objResumen.AsuntoEmail = "Envio HM Movil Una Placa"
                    objResumen.ImagenBateria = ""

                    If objResumen.EstadoBateria <> "" Then
                        If CInt(objResumen.EstadoBateria) = 1 Then
                            objResumen.EstadoBateria = "Su batería se encuentra en óptimas condiciones."
                            objResumen.ImagenBateria = "http://www.huntermonitoreoperu.com/estilos/uno/img/bat_ok.png"
                        Else
                            objResumen.EstadoBateria = "Su batería no se encuentra en óptimas condiciones."
                            objResumen.ImagenBateria = "http://www.huntermonitoreoperu.com/estilos/uno/img/bat_nook.png"
                        End If
                    End If

                Next

                Dim AccessToken As String = My.Settings.TokenEmBlue
                Dim negocio As New ReporteProgramadoBL()

                If DatoEmail.Length = 0 Or My.Settings.Url_EmBlueBulk = "" Or AccessToken = "" Or Evento = "" Then
                    GuardarLog(Me.Tipo, "Error")
                Else
                    Dim resultado As String = negocio.enviarunomensual(DatoEmail, My.Settings.Url_EmBlueBulk, AccessToken, Me.Evento, objResumen)

                    Console.WriteLine(resultado)

                    If resultado.Contains("Events Tracked") Then
                        GuardarLog(Me.Tipo, "")
                    Else
                        GuardarLog(Me.Tipo, "Error")
                    End If

                End If

            ElseIf dsEjecutivo.Tables(0).Rows.Count > 1 Then



                Dim arrResumen As New List(Of ResumenEjecutivoUnoEN)

                For Each TmpRow As DataRow In dsEjecutivo.Tables(0).Rows

                    Dim objResumen As New ResumenEjecutivoUnoEN

                    Try
                        objResumen.Placa = TmpRow("Placa")

                        If String.IsNullOrEmpty(objResumen.Placa) Then
                            objResumen.Placa = "S/P"
                        End If
                    Catch ex As Exception
                        objResumen.Placa = ""
                    End Try

                    Try
                        objResumen.Kilometraje = TmpRow("Distancia")
                        If String.IsNullOrEmpty(objResumen.Kilometraje) Then
                            objResumen.Kilometraje = "0 kms"
                        Else
                            objResumen.Kilometraje &= " kms."
                        End If
                    Catch ex As Exception
                        objResumen.Kilometraje = ""
                    End Try

                    Try
                        objResumen.Aceleracion = TmpRow("# Aceleraciones")
                        If String.IsNullOrEmpty(objResumen.Aceleracion) Then
                            objResumen.Aceleracion = "0"
                        End If
                    Catch ex As Exception
                        objResumen.Aceleracion = ""
                    End Try

                    Try
                        objResumen.EstadoBateria = TmpRow("EstadoBateria")
                        If String.IsNullOrEmpty(objResumen.EstadoBateria) Then
                            objResumen.EstadoBateria = ""
                        End If
                    Catch ex As Exception
                        objResumen.EstadoBateria = ""
                    End Try

                    Try
                        objResumen.Distancia = TmpRow("Distancia Promedio")
                        If String.IsNullOrEmpty(objResumen.Distancia) Then
                            objResumen.Distancia = "0 kms"
                        Else
                            objResumen.Distancia &= " kms."
                        End If
                    Catch ex As Exception
                        objResumen.Distancia = ""
                    End Try

                    Try
                        objResumen.VelocidadExceso = TmpRow("# Exc Velocidad")
                        If String.IsNullOrEmpty(objResumen.VelocidadExceso) Then
                            objResumen.VelocidadExceso = "0"
                        End If
                    Catch ex As Exception
                        objResumen.VelocidadExceso = ""
                    End Try

                    Try
                        objResumen.Frenado = TmpRow("# Frenados")
                        If String.IsNullOrEmpty(objResumen.Frenado) Then
                            objResumen.Frenado = "0"
                        End If
                    Catch ex As Exception
                        objResumen.Frenado = ""
                    End Try

                    Try
                        objResumen.Horas = TmpRow("# Uso Total")
                        If String.IsNullOrEmpty(objResumen.Horas) Then
                            objResumen.Horas = "0 hrs."
                        Else
                            objResumen.Horas &= " hrs."
                        End If
                    Catch ex As Exception
                        objResumen.Horas = ""
                    End Try

                    Try
                        objResumen.Trayectos = TmpRow("# Trayectos")
                        If String.IsNullOrEmpty(objResumen.Trayectos) Then
                            objResumen.Trayectos = "0"
                        End If
                    Catch ex As Exception
                        objResumen.Trayectos = ""
                    End Try

                    Try
                        objResumen.VelocidadPromedio = TmpRow("Velocidad Promedio")
                        If String.IsNullOrEmpty(objResumen.VelocidadPromedio) Then
                            objResumen.VelocidadPromedio = "0 km/h"
                        Else
                            objResumen.VelocidadPromedio &= " km/h"
                        End If
                    Catch ex As Exception
                        objResumen.VelocidadPromedio = ""
                    End Try
                    '******************************************************************************

                    objResumen.MesAnio = MonthName(FechaInicial.Month, False) & " " & FechaInicial.Year
                    objResumen.Dia = getIntervaloDias(FechaInicial.Month, Intervalo)
                    objResumen.AsuntoEmail = "Envio HM Movil Varias Placas"
                    objResumen.ImagenBateria = ""

                    If objResumen.EstadoBateria <> "" Then
                        If CInt(objResumen.EstadoBateria) = 1 Then
                            objResumen.EstadoBateria = "Su batería se encuentra en óptimas condiciones."
                            objResumen.ImagenBateria = "http://www.huntermonitoreoperu.com/estilos/uno/img/bat_ok.png"
                        Else
                            objResumen.EstadoBateria = "Su batería no se encuentra en óptimas condiciones."
                            objResumen.ImagenBateria = "http://www.huntermonitoreoperu.com/estilos/uno/img/bat_nook.png"
                        End If
                    End If

                    arrResumen.Add(objResumen)

                Next

                Dim AccessToken As String = My.Settings.TokenEmBlue

                'Dim asunto As String = "Bienvenido a la familia Hunter - " & Me.Usuario & " -  Monitoreo GPS *Enviado desde EmBlue"

                Dim negocio As New ReporteProgramadoBL()

                If DatoEmail.Length = 0 Or My.Settings.Url_EmBlueBulk = "" Or AccessToken = "" Or Evento = "" Then
                    GuardarLog(Me.Tipo, "Error")
                Else
                    Dim resultado As String = negocio.enviarvariosmensual(DatoEmail, My.Settings.Url_EmBlueBulk, AccessToken, Me.Evento, arrResumen)
                    Console.WriteLine(resultado)
                    If resultado.Contains("Events Tracked") Then
                        GuardarLog(Me.Tipo, "")
                    Else
                        GuardarLog(Me.Tipo, "Error")
                    End If
                End If
            End If

            'dsEjecutivo = Nothing
        Catch ex As Exception
            Console.WriteLine(ex.Message + " - Procedimiento GenerarReporte - " + err_mensaje)
            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180920
            'PARA LLAMAR AL PROCESO GUARDARLOG UNA VEZ SE HAYA GENERADO UN ERROR
            '******************************************************************************
            GuardarLog(Tipo, "Error")
            '******************************************************************************
        End Try
    End Function

    Private Function GeneraHTML(ByVal urlGeneraHTML As String) As String
        Dim myReq As HttpWebRequest
        Dim myResp As HttpWebResponse

        Console.WriteLine(urlGeneraHTML)

        Try
            myReq = HttpWebRequest.Create(urlGeneraHTML)
            myReq.Method = "POST"
            myReq.ContentType = "application/json"
            myReq.Headers.Add("Authorization", "Basic ")
            myReq.Timeout = 300 * 60 * 1000
            Dim myData As String = "yourDataHere"
            myReq.GetRequestStream.Write(System.Text.Encoding.UTF8.GetBytes(myData), 0, System.Text.Encoding.UTF8.GetBytes(myData).Count)
            myResp = myReq.GetResponse
            Dim myreader As New System.IO.StreamReader(myResp.GetResponseStream)
            Dim myText As String
            myText = myreader.ReadToEnd

            'Console.WriteLine(myText)

            Return myText

        Catch ex As Exception
            Console.WriteLine(ex.Message + " - Procedimiento GeneraHTML")
            Throw ex
        End Try


    End Function


    Private Function GeneraPDF(ByVal urlGeneraPDF As String) As String
        Dim myReq As HttpWebRequest
        Dim myResp As HttpWebResponse

        Console.WriteLine(urlGeneraPDF)

        Try

            myReq = HttpWebRequest.Create(urlGeneraPDF)

            myReq.Method = "POST"
            myReq.ContentType = "application/json"
            myReq.Headers.Add("Authorization", "Basic ")
            myReq.Timeout = 300 * 60 * 1000
            Dim myData As String = "yourDataHere"
            myReq.GetRequestStream.Write(System.Text.Encoding.UTF8.GetBytes(myData), 0, System.Text.Encoding.UTF8.GetBytes(myData).Count)
            myResp = myReq.GetResponse
            Dim myreader As New System.IO.StreamReader(myResp.GetResponseStream)
            Dim myText As String
            myText = myreader.ReadToEnd

            'Console.Write(myText)

            Return myText

        Catch ex As Exception

            Console.WriteLine(ex.Message + " - Procedimiento GeneraPDF")

            Throw ex
        End Try

    End Function

    Private Function getIntervaloDias(ByVal NumeroMes As Integer, ByVal Intervalo As String) As String
        Dim intervaloDias As String

        If Intervalo = "1MA" Then
            Select Case NumeroMes
                Case 1
                    intervaloDias = "1 al 31"
                Case 2
                    intervaloDias = "1 al 29"
                Case 3
                    intervaloDias = "1 al 31"
                Case 4
                    intervaloDias = "1 al 30"
                Case 5
                    intervaloDias = "1 al 31"
                Case 6
                    intervaloDias = "1 al 30"
                Case 7
                    intervaloDias = "1 al 31"
                Case 8
                    intervaloDias = "1 al 31"
                Case 9
                    intervaloDias = "1 al 30"
                Case 10
                    intervaloDias = "1 al 31"
                Case 11
                    intervaloDias = "1 al 30"
                Case 12
                    intervaloDias = "1 al 31"
                Case Else
                    intervaloDias = "1 al 31"
            End Select
        Else
            intervaloDias = String.Format("{0} al {1}", FechaInicial.ToString("dd"), FechaFinal.ToString("dd"))
        End If

        Return intervaloDias

    End Function



End Class


