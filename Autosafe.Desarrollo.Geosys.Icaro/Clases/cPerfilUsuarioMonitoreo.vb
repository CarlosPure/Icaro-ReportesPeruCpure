Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades

Public Class cPerfilUsuarioMonitoreo : Inherits cGeneracion

    Private dsHistorico As DataSet
    Private Const Reporte As String = "Reporte de Perfil Usuario Monitoreo de la Flota"


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

        Console.WriteLine(Datos.usuarioId.ToString() & ": Obteniendo Datos para el Reporte Historico")

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

            GenerarReporte(Datos.usuarioId,
                              Datos.intervalo,
                              Datos.parametros)

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
                'Console.WriteLine(String.Format("Obteniendo Historico de:  {0} {1} {2}", tActivo.alias, Fecha2ANSI(Me.FechaInicial.ToString(), True), Fecha2ANSI(Me.FechaFinal.ToString(), True)))
                '******************************************************************************

                Dim sql As String

                If Intervalo Like "*H" Then
                    '******************************************************************************
                    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                    'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
                    '******************************************************************************
                    '' ''sql = String.Format("spActivoRecorridoRConsultar2 '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'",
                    '' ''       tActivo.vid,
                    '' ''       "CUS",
                    '' ''       "C",
                    '' ''       IdUsuario,
                    '' ''       "127005",
                    '' ''       Fecha2ANSI(Me.FechaInicial.ToString(), False),
                    '' ''       Fecha2ANSI(Me.FechaFinal.ToString(), False),
                    '' ''       0)

                    DbDataAdapter = New SqlDataAdapter(sql, DbConnection)
                    '******************************************************************************
                Else
                    '******************************************************************************
                    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                    'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
                    'PARA CAMBIAR EL NOMBRE DEL PROCEDIMIENTO LLAMADO
                    'PARA LLAMAR AL PROCEDIMIENTO DE ACUERDO A LA VARIABLE PAIS
                    '******************************************************************************

                    'EXEC [GEOSYS]..[sp_UPerfil_InformeFlota] '1405','20191201 00:00:00','20191231 23:59:59','PCF','4256','A'

                    If (My.Settings.Pais = "PE") Then
                        sql = String.Format("sp_UPerfil_InformeFlota_DESA_VICTOR '{0}','{1}','{2}','{3}','{4}','{5}'",
                            0,
                            Fecha2ANSI(Me.FechaInicial.ToString(), True) & " " & Me.HoraInicio,
                            Fecha2ANSI(Me.FechaFinal.ToString(), True) & " " & Me.HoraFin,
                           "PCF",
                           IdUsuario,
                           "A")
                    Else
                        'sql = String.Format("spActivoRecorridoRConsultar2 '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'",
                        '   tActivo.vid,
                        '   "CUS",
                        '   "C",
                        '   IdUsuario,
                        '   "127005",
                        '   Fecha2ANSI(Me.FechaInicial.ToString(), True) & " 00:00:00",
                        '   Fecha2ANSI(Me.FechaFinal.ToString(), True) & " 23:59:59",
                        '   0)
                    End If


                    DbDataAdapter = New SqlDataAdapter(sql, DbConnection)

                    '******************************************************************************
                End If



                Hoja = Nothing
               

                DbDataAdapter.SelectCommand.CommandTimeout = 3000000
                DbDataAdapter.Fill(dsHistorico, "Historico")
                dsHistorico.AcceptChanges()

                dsHistorico.AcceptChanges()

                Dim aaaaa As String

                aaaaa = ""


                Dim bbbbb As String

                bbbbb = ""


                aaaaa &= "<html xmlns='http://www.w3.org/1999/xhtml' lang='es'>"
                aaaaa &= "<head>"
                aaaaa &= "<meta charset='utf-8'>"
                aaaaa &= "<meta name='viewport' content='width=device-width, initial-scale=1'>"
                aaaaa &= "<title> Hunter - Estoy Seguro </title>"
                aaaaa &= "<link rel='stylesheet' type='text/css' href='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/theme.css'>"
                aaaaa &= "</head>"
                aaaaa &= "<body>"
                aaaaa &= "<!---- HEADER ----->"
                aaaaa &= "<div class='container '>"
                aaaaa &= "<div class='row'>"
                aaaaa &= "<div class='back-head'>"
                aaaaa &= "<div class='col-5'>"
                aaaaa &= "<div class='logo'>"
                aaaaa &= "<a href='https://www.hunterlojack.com/' target='_blank'> <img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/logo.png' alt='logo'> </a>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"
                aaaaa &= "<div class='col-5'>"
                aaaaa &= "<div class='title-head'>"
                aaaaa &= "<h2>Reportes Mensuales</h2>"
                aaaaa &= "<h3> <a href='https://www.huntermonitoreoperu.com/' target='_blank'> www.<strong>hunter</strong>monitoreoperu.com </a></h3>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"
                aaaaa &= "<!---- CIERRA - HEADER ----->"
                aaaaa &= "<!---- Background  ----->"
                aaaaa &= "<div class='container'>"
                aaaaa &= "<div class='row'>"
                aaaaa &= "<div class='back-image'>"
                aaaaa &= "<div class='col-10'>"
                aaaaa &= "<div class='title-back'>"
                aaaaa &= "<h4>HM Movil</h4>"
                aaaaa &= "<h3>Revisa aqui el</h3>"
                aaaaa &= "<h2>rendimiento de tus unidades</h2>"
                aaaaa &= "<div style='background-color: transparent; padding: 0; margin-bottom: 0;' class='icon-box-three'>"
                aaaaa &= "<div class='iconos-imag-two'>"
                aaaaa &= "<img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-app.png' alt=''>"
                aaaaa &= "</div>"
                aaaaa &= "<div class='tit-icon-two'>"
                aaaaa &= "<h2>HM MԖIL</h2>"
                aaaaa &= "<h4><a style='margin-right: 5px' href='https://apps.apple.com/pe/app/hunter-monitoreo-hmmovil/id665501817' target='_blank'><img  src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-app-apple.png' alt=''></a>"
                aaaaa &= "<a href='https://play.google.com/store/apps/details?id=com.aianbruf.hunter&hl=es_VE' target='_blank'><img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-app-android-white.png' alt=''></a></h4>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"

                aaaaa &= "</div>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"
                aaaaa &= "<!---- CIERRA - Background ----->"
                aaaaa &= "<!---- PRIMWER TITULO ----->"
                aaaaa &= "<div class='container'>"
                aaaaa &= "<div class='row'>"
                aaaaa &= "<div class='col-10'>"

                aaaaa &= "<div style='height: 100px;padding-bottom: 10px; padding-top: 10px;' class='icon-tit-home'>"
                aaaaa &= "<p>MES:</p>"
                aaaaa &= "<div class='icono-tit-home'>"
                aaaaa &= "<img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-010.png' alt=''>"
                aaaaa &= "</div>"
                aaaaa &= "<div class='tit-home'>"
                aaaaa &= "<h2>Diciembre 2019</h2>"
                aaaaa &= "<h4>Del 01 al 31</h4>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"

                aaaaa &= "</div>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"
                aaaaa &= "<!---- /PRIMWER TITULO ----->"




                For Each TmpRow As DataRow In dsHistorico.Tables(0).Rows




                    Contador += 1



                    aaaaa &= "<!----- SEGUNDA FILA DE ICONOS ---->"



                    aaaaa &= "<div class='container'>"
                    aaaaa &= "<div class='row'>"
                    aaaaa &= "<div class='secc-tres' style='padding-bottom: 10px; padding-top: 10px;'>"
                    aaaaa &= "<div class='col-3'>"
                    aaaaa &= "<div class='titulos-dos'>"
                    aaaaa &= "<h4>Placa: <span>" & TmpRow("Placa") & "</span></h4>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"

                    aaaaa &= "<div class='col-3'>"
                    aaaaa &= "<div class='titulos-dos-a'>"
                    aaaaa &= "<img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-016.png' alt=''> "
                    aaaaa &= "<p>Se recomienda revisar<br>el estado de su bater? </p>"
                    aaaaa &= "</div>	"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"



                    aaaaa &= "<!---- ICONOS - BOX  ----->"


                    aaaaa &= "<div class='container'>"
                    aaaaa &= "<div class='row'>"
                    aaaaa &= "<div class='back-color'>"
                    aaaaa &= "<div class='col-10'>"
                    aaaaa &= "<div class='contenedor'>"


                    aaaaa &= "<div class='col-25'>"
                    aaaaa &= "<div class='icon-box'>"
                    aaaaa &= "<div class='iconos-imag'>"
                    aaaaa &= "<img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-002.png' alt=''>"
                    aaaaa &= "</div>"
                    aaaaa &= "<div class='tit-icon'>"
                    aaaaa &= "<h4>KM RECORRIDOS</h4>"
                    aaaaa &= "<h2>" & TmpRow("Placa") & " km/h</h2>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"

                    aaaaa &= "<div class='col-25'>"
                    aaaaa &= "<div class='icon-box'>"
                    aaaaa &= "<div class='iconos-imag'>"
                    aaaaa &= "<img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-004.png' alt=''>"
                    aaaaa &= "</div>"
                    aaaaa &= "<div class='tit-icon'>"
                    aaaaa &= "<h4>DISTANCIA</h4>"
                    aaaaa &= "<h2>" & TmpRow("DistanciaRecorrida") & " km</h2>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"

                    aaaaa &= "<div class='col-25'>"
                    aaaaa &= "<div class='icon-box'>"
                    aaaaa &= "<div class='iconos-imag'>"
                    aaaaa &= "<img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-005.png' alt=''>"
                    aaaaa &= "</div>"
                    aaaaa &= "<div class='tit-icon'>"
                    aaaaa &= "<h4>HORAS TRABAJADAS</h4>"
                    aaaaa &= "<h2>" & TmpRow("HoraUso") & " hrs.</h2>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"

                    aaaaa &= "<div class='col-25'>"
                    aaaaa &= "<div class='icon-box'>"
                    aaaaa &= "<div class='iconos-imag'>"
                    aaaaa &= "<img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-008.png' alt=''>"
                    aaaaa &= "</div>"
                    aaaaa &= "<div class='tit-icon'>"
                    aaaaa &= "<h4>TRAYECTOS REALIZADOS</h4>"
                    aaaaa &= "<h2>" & TmpRow("Placa") & "</h2>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"


                    aaaaa &= "</div><!---- CONTENEDOR ----->"
                    aaaaa &= "</div> "

                    aaaaa &= "<div class='col-10'>"
                    aaaaa &= "<div class='contenedor'>"


                    aaaaa &= "<div class='col-25'>"
                    aaaaa &= "<div class='icon-box'>"
                    aaaaa &= "<div class='iconos-imag'>"
                    aaaaa &= "<img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-003.png' alt=''>"
                    aaaaa &= "</div>"
                    aaaaa &= "<div class='tit-icon'>"
                    aaaaa &= "<h4>ACELERACIONES BRUSCAS</h4>"
                    aaaaa &= "<h2>" & TmpRow("Cont_Aceleracion") & "</h2>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"

                    aaaaa &= "<div class='col-25'>"
                    aaaaa &= "<div class='icon-box'>"
                    aaaaa &= "<div class='iconos-imag'>"
                    aaaaa &= "<img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-005.png' alt=''>"
                    aaaaa &= "</div>"
                    aaaaa &= "<div class='tit-icon'>"
                    aaaaa &= "<h4>EXCESOS DE VELOCIDAD <span>(VECES SEG? REGLAMENTO)</span></h4>"
                    aaaaa &= "<h2 style='color: #ff0000;'>" & TmpRow("ExcesosVelocidad_Original") & "</h2>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"

                    aaaaa &= "<div class='col-25'>"
                    aaaaa &= "<div class='icon-box'>"
                    aaaaa &= "<div class='iconos-imag'>"
                    aaaaa &= "<img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-006.png' alt=''>"
                    aaaaa &= "</div>"
                    aaaaa &= "<div class='tit-icon'>"
                    aaaaa &= "<h4>FRENADAS BRUSCAS</h4>"
                    aaaaa &= "<h2 style='color: #ff0000;'>" & TmpRow("Cont_Frenado_Original") & "</h2>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"

                    aaaaa &= "<div class='col-25'>"
                    aaaaa &= "<div class='icon-box'>"
                    aaaaa &= "<div class='iconos-imag'>"
                    aaaaa &= "<img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-009.png' alt=''>"
                    aaaaa &= "</div>"
                    aaaaa &= "<div class='tit-icon'>"
                    aaaaa &= "<h4>VELOCIDAD PROMEDIO</h4>"
                    aaaaa &= "<h2>" & TmpRow("max_velocidad") & " km/h</h2>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"
                    aaaaa &= "</div>"


                    aaaaa &= "</div><!---- CONTENEDOR ----->"
                    aaaaa &= "</div> "

                    aaaaa &= "</div>"
                    aaaaa &= "</div> "
                    aaaaa &= "</div>"


                    aaaaa &= "<!----- /SEGUNDA FILA DE ICONOS ---->"


                    'dsHistorico.AcceptChanges()


                Next
                dsHistorico.AcceptChanges()


                aaaaa &= "<div class='container'>"
                aaaaa &= "<div class='row'>"
                aaaaa &= "<div class='col-10'>"

                aaaaa &= "<div class='icon-box-five'>"
                aaaaa &= "<div class='tit-icon-five'>"
                aaaaa &= "<h4>Revisa mayor detalle de tus unidades ingresando a:  <span class='clic-mano'><a href='http://www.huntermonitoreoperu.com/GeoV3.3/' target='_blank'>"
                aaaaa &= "<img src='http://www.huntermonitoreoperu.com:8080/hunterperu/perfilmonitoreo/img/icon-014.png' width='200px' alt=''></span>"
                aaaaa &= "</a></h4>						"
                aaaaa &= "</div>"
                aaaaa &= "</div>"


                aaaaa &= "</div>"
                aaaaa &= "<div class='borde-bottom'> </div>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"


                'aaaaa &= "<div class='container'>"
                'aaaaa &= "<div class='row'>"
                'aaaaa &= "<div class='col-10'>"
                'aaaaa &= "<div class='copy'>"
                'aaaaa &= "<h4>"
                'aaaaa &= "<span style='border: 0;'><a href='https://www.hunterlojack.com/' target='_blank'>www.hunterlojack.com</a></span>"
                'aaaaa &= "<span><a href='https://extranet.hunterlojack.com/' target='_blank'>Mi Cuenta Hunter</a></span>  "
                'aaaaa &= "<span><a href='https://www.hunterlojack.com/web/detalle-servicio.php?cat=1&id=43' target='_blank'>Conoce los beneficios de tu producto</a></span>"
                'aaaaa &= "<span><a href='#' target='_blank'>Deseo desuscribirme</a></span>"
                'aaaaa &= "</h4>"
                'aaaaa &= "</div>"
                'aaaaa &= "</div>"
                'aaaaa &= "</div>"
                'aaaaa &= "</div>"

                aaaaa &= "<div class='container'>"
                aaaaa &= "<div class='row'>"
                aaaaa &= "<div class='col-10'>"
                aaaaa &= "<div class='copy'>"
                aaaaa &= "<h4>"
                aaaaa &= "<span style='border: 0;'><a href='https://www.hunterlojack.com/' target='_blank'>www.hunterlojack.com</a></span>"
                aaaaa &= "<span><a href='https://extranet.hunterlojack.com/' target='_blank'>Mi Cuenta Hunter</a></span>  "
                aaaaa &= "<span><a href='https://www.hunterlojack.com/web/detalle-servicio.php?cat=1&id=43' target='_blank'>Conoce los beneficios de tu producto</a></span>"
                aaaaa &= "<span><a href='#' target='_blank'>Deseo desuscribirme</a></span>"
                aaaaa &= "</h4>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"
                aaaaa &= "</div>"





                aaaaa &= "</body>"


                aaaaa &= "</html>"

                aaaaa &= ""


                Dim nombreArchivo As String = String.Format("perfilmonitoreo_{0}_{1}.html",
                                                          Fecha2ANSI(FechaInicial.ToString(), True),
                                                          Fecha2ANSI(FechaFinal.ToString(), True))

                Dim ArchivoTEXT As String = nombreArchivo
                Dim fileTEXT As System.IO.StreamWriter
                fileTEXT = My.Computer.FileSystem.OpenTextFileWriter(ArchivoTEXT, False)
                fileTEXT.Write(aaaaa)
                fileTEXT.Close()




                Me.Archivo = nombreArchivo



                Console.WriteLine("Filas a Generar: " & dsHistorico.Tables(0).Rows.Count.ToString())
                Console.ForegroundColor = ConsoleColor.Cyan

                'NumHojas = 0
                'NumHojas = dsHistorico.Tables(0).Rows.Count / 65000
                'NumFila = 1
                'Dim tmpHoja As ISheet = Hoja
                'Pagina = 2

                'For Each oRow As DataRow In dsHistorico.Tables(0).Rows
                '    'Fila = tmpHoja.CreateRow(tmpHoja.LastRowNum + 1)
                '    'onsole.WriteLine(String.Format("Generando Fila {0} de {1}", tmpHoja.LastRowNum, dsHistorico.Tables(0).Rows.Count))





                'Next

                'For Each dColum As DataColumn In dsHistorico.Tables(0).Columns
                '    Hoja.AutoSizeColumn(dColum.Ordinal)
                'Next
                'Console.WriteLine("Registros Generados para " & tActivo.alias.ToUpper())
            Catch ex As Exception
                Console.Write(ex.Message)
            End Try
            'Next
            dsHistorico = Nothing



            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181002
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
                       Me.Archivo,
                       "Adjunto Email con el Reporte Solicitado",
                        Nothing,
                        Reporte,
                        Me.Tipo,
                        Me.IdUsuario)
            Catch ex As Exception
                ErroresEnvio &= "ERR_MAIL"
            End Try
            'End If
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
