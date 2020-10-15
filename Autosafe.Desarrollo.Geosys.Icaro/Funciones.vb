Imports System.Threading
Imports System.IO

Imports System.Data.SqlClient

Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades

Module Funciones
    Public hsCalificacion As New Hashtable()
    Private hsEnvios As Thread

    Private NumEnvios As Integer
    Private ErroresEnvio As String
    Public Archivo As String

    Private DatoTipo As String()
    Private DatoActivos As String()
    Private DatoEmail As String()
    Private Usuario As String
    Private ColumnasReporte As String()
    Private SecuenciaLog As Integer = 0

    'Private DBS As ThorDataContext



    Public Sub Main()


        Console.WriteLine("Inicio de Operaciones" & Now.ToString())
        Console.Title = "En Ejecucion desde las " & Now.ToString()
        Dim hsEnvios As New Thread(AddressOf ConsultarEnvios)
        Dim hsProcesa As New Thread(AddressOf ProcesarRegistros)

        If Not Directory.Exists(AppDomain.CurrentDomain.BaseDirectory & "Usuario") Then
            Try
                Directory.CreateDirectory("Usuarios")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        End If

        'DBS = New ThorDataContext()
        'DBS.CommandTimeout = 3000000

        hsEnvios.Start()
        'hsProcesa.Start()
    End Sub

    Private Sub ConsultarEnvios()
        Dim vid As String = ""

        While True

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180828
            'PARA VERIFICAR QUE LA HORA ACTUAL ESTE DENTRO DEL RANGO DE HORAS DE ATENCION DE LOS REPORTES PROGRAMADOS
            '******************************************************************************
            If CheckTimeRange(Now.TimeOfDay, TimeSpan.Parse(My.Settings.HoraInicio), TimeSpan.Parse(My.Settings.HoraFin)) Then

                Try



                    Console.ForegroundColor = ConsoleColor.White
                    Console.WriteLine("Verificación de Emails a Enviar " & Now.ToString())
                    NumEnvios = 0

                    'CONSULTA A LA BASE DE PRODUCCION 172.16.1.217
                    'DBS = New ThorDataContext()
                    'DBS.CommandTimeout = 3000000

                    'ErroresEnvio = ""

                    ''CONSULTA LOS REPORTES PENDIENTES DE ENVIO
                    'Dim Envios = DBS.spReporteProgramadoListar(0)

                    '        AplicativoHadesBL negocio = new AplicativoHadesBL();
                    'MonitoreoHadesEN obj = new MonitoreoHadesEN();

                    'string servidorLocal = "";
                    'servidorLocal = ConfigurationManager.AppSettings["ServidorLocal"];

                    'List<Entidades.MonitoreoHadesEN> listaAplicativoHades = negocio.ListarPorCriterios(servidorLocal);

                    'foreach (Entidades.MonitoreoHadesEN item in listaAplicativoHades)
                    '{


                    Dim negocio As New ReporteProgramadoBL()
                    Dim obj As New ReporteProgramadoEN
                    obj.baseDatosId = 217
                    obj.usuarioId = 0


                    ''Dim lista As New List(Of ReporteProgramadoEN)()
                    'Dim lista As List(Of ReporteProgramadoEN)()

                    'lista = negocio.Listar(obj);

                    Dim lista As List(Of ReporteProgramadoEN) = negocio.Listar(obj)

                    For Each ReporteProgramadoEN In lista

                        With ReporteProgramadoEN
                            DatoTipo = .tipoReporte.Split("")
                            DatoActivos = .activos.Split("")
                            DatoEmail = .email.Split("")

                            If .horas = 0 Or .forzar Then

                                For Ind As Integer = 0 To DatoTipo.Length - 1

                                    Try
                                        Console.ForegroundColor = ConsoleColor.Green
                                        Console.WriteLine(String.Format("{0} {1}", DatoTipo(Ind), .usuarioId))
                                        Console.WriteLine("")

                                        If .activos Like "*Todos*" Then
                                            Try
                                                Select Case .tipoReporte
                                                    '******************************************************************************
                                                    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180828
                                                    'PARA AGREGAR LOS VALORES:
                                                    'NOMBRE
                                                    'SECUENCIA A TODOS LOS CONSTRUCTORES DE LAS CLASES
                                                    'HORAINICIO
                                                    'HORA FIN
                                                    'A TODOS LOS CONSTRUCTORES DE LAS CLASES
                                                    'ESTO DE ACUERDO A LA VARIABLE MY.SETTINGS.PAIS = PE
                                                    '******************************************************************************
                                                    Case "VEL"
                                                        'Dim oVelocidad As cVelocidad = New cVelocidad(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oVelocidad As cVelocidad
                                                        If (My.Settings.Pais = "PE") Then
                                                            oVelocidad = New cVelocidad(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oVelocidad = New cVelocidad(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsVelocidad As Thread = New Thread(AddressOf oVelocidad.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Velocidad para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsVelocidad.Start(ReporteProgramadoEN)
                                                    Case "KMS"
                                                        'Dim oKms As cKilometraje = New cKilometraje(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oKms As cKilometraje
                                                        If (My.Settings.Pais = "PE") Then
                                                            oKms = New cKilometraje(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oKms = New cKilometraje(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsKilometraje As Thread = New Thread(AddressOf oKms.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Kilometraje para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsKilometraje.Start(ReporteProgramadoEN)
                                                    Case "HIS"
                                                        'Dim oHis As cHistorico = New cHistorico(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oHis As cHistorico
                                                        If (My.Settings.Pais = "PE") Then
                                                            oHis = New cHistorico(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oHis = New cHistorico(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsHistorico As Thread = New Thread(AddressOf oHis.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Historico para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsHistorico.Start(ReporteProgramadoEN)
                                                    Case "HISM"
                                                        'Dim oHisM As cHistoricoMulti = New cHistoricoMulti(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oHisM As cHistoricoMulti
                                                        If (My.Settings.Pais = "PE") Then
                                                            oHisM = New cHistoricoMulti(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oHisM = New cHistoricoMulti(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsHistoricoMulti As Thread = New Thread(AddressOf oHisM.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Historico para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsHistoricoMulti.Start(ReporteProgramadoEN)
                                                    Case "TRY"
                                                        'Dim oTry As cTrayectos = New cTrayectos(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oTry As cTrayectos
                                                        If (My.Settings.Pais = "PE") Then
                                                            oTry = New cTrayectos(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oTry = New cTrayectos(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsTrayectos As Thread = New Thread(AddressOf oTry.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Trayectos para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsTrayectos.Start(ReporteProgramadoEN)
                                                    Case "COM"
                                                        'Dim oComp As cComportamiento = New cComportamiento(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oComp As cComportamiento
                                                        If (My.Settings.Pais = "PE") Then
                                                            oComp = New cComportamiento(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oComp = New cComportamiento(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsComportamiento As Thread = New Thread(AddressOf oComp.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Comportamiento para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsComportamiento.Start(ReporteProgramadoEN)
                                                    Case "ROM"
                                                        'Dim oRom As cComportamientoResumen = New cComportamientoResumen(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oRom As cComportamientoResumen
                                                        If (My.Settings.Pais = "PE") Then
                                                            oRom = New cComportamientoResumen(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oRom = New cComportamientoResumen(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsResumenComportamiento As Thread = New Thread(AddressOf oRom.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte de Resumen de Comportamiento para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsResumenComportamiento.Start(ReporteProgramadoEN)
                                                    Case "ALR"
                                                        'Dim oAlertas As cAlertas = New cAlertas(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oAlertas As cAlertas
                                                        If (My.Settings.Pais = "PE") Then
                                                            oAlertas = New cAlertas(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oAlertas = New cAlertas(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsAlertas As Thread = New Thread(AddressOf oAlertas.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte de Alertas para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsAlertas.Start(ReporteProgramadoEN)
                                                    Case "HTR"
                                                        'Dim oHoras As cHorasTrabajadas = New cHorasTrabajadas(.TipoReporte, .usuarioId, .Intervalo, .Parametros, .Nombre, .Secuencia)

                                                        Dim oHoras As cHorasTrabajadas
                                                        If (My.Settings.Pais = "PE") Then
                                                            oHoras = New cHorasTrabajadas(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oHoras = New cHorasTrabajadas(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsHoras As Thread = New Thread(AddressOf oHoras.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte de Horas Trabajadas para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsHoras.Start(ReporteProgramadoEN)
                                                    Case "FLO"
                                                        'Dim oFlota As cFlota = New cFlota(.TipoReporte, .usuarioId, .Intervalo, .Parametros)
                                                        Dim oFlota As cFlota = New cFlota(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Dim hsFlota As Thread = New Thread(AddressOf oFlota.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Estado  de la Flota para  {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsFlota.Start(ReporteProgramadoEN)
                                                    Case "RZN"
                                                        'Dim oZona As cZonas = New cZonas(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oZona As cZonas
                                                        If (My.Settings.Pais = "PE") Then
                                                            oZona = New cZonas(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oZona = New cZonas(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsZona As Thread = New Thread(AddressOf oZona.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte de Zonas para  {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsZona.Start(ReporteProgramadoEN)
                                                        '******************************************************************************


                                                        'Case "CMP"

                                                        'Case "RES"


                                                        '******************************************************************************
                                                        'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180828
                                                        'PARA AGREGAR NUEVO TIPO DE REPORTE: 
                                                        'TRAYECTO DETALLE
                                                        'EJECUTIVO
                                                        'EVENTO
                                                        'HORAS TRABAJADAS
                                                        'PROTOCOLO DE MANEJO
                                                        'MAF EXTRANET
                                                        '******************************************************************************
                                                    Case "TRYD"
                                                        'Dim oTry As cTrayectosDetalle = New cTrayectosDetalle(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oTryDetalle As cTrayectosDetalle
                                                        If (My.Settings.Pais = "PE") Then
                                                            oTryDetalle = New cTrayectosDetalle(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oTryDetalle = New cTrayectosDetalle(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsTrayectosDetalle As Thread = New Thread(AddressOf oTryDetalle.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Detalle de Trayectos para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsTrayectosDetalle.Start(ReporteProgramadoEN)

                                                    Case "EJE"
                                                        'Dim oTry As cEjecutivo = New cEjecutivo(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oEje As cEjecutivo
                                                        If (My.Settings.Pais = "PE") Then
                                                            oEje = New cEjecutivo(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario, .entidadId)
                                                        Else
                                                            oEje = New cEjecutivo(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hscEjecutivo As Thread = New Thread(AddressOf oEje.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Ejecutivo para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hscEjecutivo.Start(ReporteProgramadoEN)

                                                    Case "PER"
                                                        'Dim oTry As cPerfil = New cPerfil(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oEje As cPerfil
                                                        If (My.Settings.Pais = "PE") Then
                                                            oEje = New cPerfil(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario, .entidadId)
                                                        Else
                                                            oEje = New cPerfil(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hscEjecutivo As Thread = New Thread(AddressOf oEje.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Ejecutivo para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hscEjecutivo.Start(ReporteProgramadoEN)

                                                    Case "EVE"
                                                        'Dim oTry As cEvento = New cEvento(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oEve As cEvento
                                                        If (My.Settings.Pais = "PE") Then
                                                            oEve = New cEvento(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oEve = New cEvento(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hscEvento As Thread = New Thread(AddressOf oEve.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Evento para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hscEvento.Start(ReporteProgramadoEN)

                                                    Case "HTRD"
                                                        'Dim oHorasDetalle As cHorasTrabajadasDetalle = New cHorasTrabajadasDetalle(.TipoReporte, .usuarioId, .Intervalo, .Parametros)

                                                        Dim oHorasDetalle As cHorasTrabajadasDetalle
                                                        If (My.Settings.Pais = "PE") Then
                                                            oHorasDetalle = New cHorasTrabajadasDetalle(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oHorasDetalle = New cHorasTrabajadasDetalle(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsHorasDetalle As Thread = New Thread(AddressOf oHorasDetalle.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Detalle de Horas Trabajadas para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsHorasDetalle.Start(ReporteProgramadoEN)

                                                    Case "PRM"

                                                        Dim oProtocolo As cProtocoloManejo
                                                        If (My.Settings.Pais = "PE") Then
                                                            oProtocolo = New cProtocoloManejo(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario, .entidadId)
                                                        Else
                                                            oProtocolo = New cProtocoloManejo(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsProtocolo As Thread = New Thread(AddressOf oProtocolo.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Protocolo Manejo para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsProtocolo.Start(ReporteProgramadoEN)

                                                    Case "MGR"

                                                        Dim oGestion As cGestionRiesgo
                                                        If (My.Settings.Pais = "PE") Then
                                                            oGestion = New cGestionRiesgo(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oGestion = New cGestionRiesgo(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsProtocolo As Thread = New Thread(AddressOf oGestion.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Gestion de Riesgos para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsProtocolo.Start(ReporteProgramadoEN)

                                                    Case "RCM"

                                                        Dim oGestion As cCalidadManejo
                                                        If (My.Settings.Pais = "PE") Then
                                                            oGestion = New cCalidadManejo(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario, .entidadId)
                                                        Else
                                                            oGestion = New cCalidadManejo(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsProtocolo As Thread = New Thread(AddressOf oGestion.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Resumen Calidad de Manejo para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsProtocolo.Start(ReporteProgramadoEN)

                                                    Case "CTR"

                                                        Dim oCuadro As cCuadroTrabajo
                                                        If (My.Settings.Pais = "PE") Then
                                                            oCuadro = New cCuadroTrabajo(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oCuadro = New cCuadroTrabajo(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsCuadro As Thread = New Thread(AddressOf oCuadro.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Cuadro de Trabajo para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsCuadro.Start(ReporteProgramadoEN)

                                                    Case "FNT"

                                                        Dim oNoTransmision As cFinancieraNoTransmision
                                                        If (My.Settings.Pais = "PE") Then
                                                            oNoTransmision = New cFinancieraNoTransmision(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oNoTransmision = New cFinancieraNoTransmision(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsNoTransmision As Thread = New Thread(AddressOf oNoTransmision.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Financiera No Transmision para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsNoTransmision.Start(ReporteProgramadoEN)

                                                    Case "RUG"

                                                        Dim oResumenUbicacion As cResumenUbicacionGeografica
                                                        If (My.Settings.Pais = "PE") Then
                                                            oResumenUbicacion = New cResumenUbicacionGeografica(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oResumenUbicacion = New cResumenUbicacionGeografica(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsResumenUbicacion As Thread = New Thread(AddressOf oResumenUbicacion.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Resumen Ubicacion Geografica para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsResumenUbicacion.Start(ReporteProgramadoEN)

                                                    Case "DUG"

                                                        Dim oDetalleUbicacion As cDetalleUbicacionGeografica
                                                        If (My.Settings.Pais = "PE") Then
                                                            oDetalleUbicacion = New cDetalleUbicacionGeografica(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oDetalleUbicacion = New cDetalleUbicacionGeografica(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsDetalleUbicacion As Thread = New Thread(AddressOf oDetalleUbicacion.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Detalle Ubicacion Geografica para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsDetalleUbicacion.Start(ReporteProgramadoEN)

                                                    Case "MGR2"

                                                        Dim oGestion As cGestionRiesgo2
                                                        If (My.Settings.Pais = "PE") Then
                                                            oGestion = New cGestionRiesgo2(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oGestion = New cGestionRiesgo2(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsProtocolo As Thread = New Thread(AddressOf oGestion.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Reporte Gestion de Riesgos2 para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsProtocolo.Start(ReporteProgramadoEN)

                                                    'Case "AAA"

                                                    '    Dim oPerfil As cPerfilUsuarioMonitoreo
                                                    '    If (My.Settings.Pais = "PE") Then
                                                    '        oPerfil = New cPerfilUsuarioMonitoreo(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                    '    Else
                                                    '        oPerfil = New cPerfilUsuarioMonitoreo(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                    '    End If

                                                    '    Dim hsProtocolo As Thread = New Thread(AddressOf oPerfil.Generar)
                                                    '    Console.ForegroundColor = ConsoleColor.Yellow
                                                    '    Console.WriteLine(String.Format("Generando Reporte Perfil Usuario Monitoreo para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                    '    hsProtocolo.Start(ReporteProgramadoEN)
                                                    Case "CRE"

                                                        Dim oCredencial As cCredenciales
                                                        If (My.Settings.Pais = "PE") Then
                                                            oCredencial = New cCredenciales(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario, .entidadId, .evento)
                                                        Else
                                                            oCredencial = New cCredenciales(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsProtocolo As Thread = New Thread(AddressOf oCredencial.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Envio de Credenciales para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsProtocolo.Start(ReporteProgramadoEN)
                                                    Case "RES"

                                                        Dim oResumen As cResumen
                                                        If (My.Settings.Pais = "PE") Then
                                                            oResumen = New cResumen(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario, .entidadId, .evento)
                                                        Else
                                                            oResumen = New cResumen(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsResumen As Thread = New Thread(AddressOf oResumen.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Envio de Resumen Emblue para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsResumen.Start(ReporteProgramadoEN)
                                                    Case "SKY"

                                                        Dim oResumen As cDispositivoSkywave
                                                        If (My.Settings.Pais = "PE") Then
                                                            oResumen = New cDispositivoSkywave(.tipoReporte, .usuarioId, .intervalo, .parametros, .nombre, .secuencia, .horaInicio, .horaFin, .usuario)
                                                        Else
                                                            oResumen = New cDispositivoSkywave(.tipoReporte, .usuarioId, .intervalo, .parametros)
                                                        End If

                                                        Dim hsResumen As Thread = New Thread(AddressOf oResumen.Generar)
                                                        Console.ForegroundColor = ConsoleColor.Yellow
                                                        Console.WriteLine(String.Format("Generando Envio de Resumen SKY para {0} a las {1} Intervalo {2}", .usuarioId, Now, .intervalo))
                                                        hsResumen.Start(ReporteProgramadoEN)

                                                        '******************************************************************************

                                                End Select

                                            Catch xe As Exception
                                                Console.ForegroundColor = ConsoleColor.Red
                                                Console.WriteLine(xe.Message.ToUpper())
                                            End Try
                                        Else

                                        End If
                                    Catch ex As Exception
                                    End Try

                                Next
                            Else
                                If .horas <= 30 And ReporteProgramadoEN.horas > 0 Then
                                    Console.ForegroundColor = ConsoleColor.Green
                                    Console.WriteLine(String.Format("Faltan {0} minutos para el Siguiente Envio a {1}", ReporteProgramadoEN.horas, ReporteProgramadoEN.email))
                                    Console.ForegroundColor = ConsoleColor.Gray
                                End If
                            End If
                        End With


                        Try

                            If ReporteProgramadoEN.forzar Then

                                obj = New ReporteProgramadoEN
                                obj.baseDatosId = 217
                                obj.secuencia = ReporteProgramadoEN.secuencia
                                obj.usuarioId = ReporteProgramadoEN.usuarioId
                                obj.tipoReporte = ReporteProgramadoEN.tipoReporte

                                Dim actualizar As Boolean = negocio.ActualizarForzarFin(obj)

                                'DBS.spReporteProgramadoForzarFin(ReporteProgramadoEN.secuencia)

                                '******************************************************************************
                                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
                                'PARA PROCESAR UN REPORTE PENDIENTE DE ENVIAR
                                'EN LA TABLA LOG REPORTEPROGRAMADOPENDIENTE
                                'VERIFICA QUE LA VARIABLE MY.SETTINGS.ReporteProgramadoPendiente_GuardarLog = 1
                                '******************************************************************************

                                If ReporteProgramadoEN.secuencia > -999999 Then
                                    If My.Settings.ReporteProgramadoPendiente_GuardarLog = "1" Then
                                        'DBS.spReporteProgramadoPendienteForzarFin(ReporteProgramadoEN.usuarioId, ReporteProgramadoEN.tipoReporte, ReporteProgramadoEN.secuencia, ReporteProgramadoEN.intervalo, ReporteProgramadoEN.parametros)

                                        Dim negocioPendiente As New ReporteProgramadoPendienteBL()
                                        Dim objPendiente As New ReporteProgramadoPendienteEN

                                        objPendiente.baseDatosId = 217
                                        objPendiente.usuarioId = ReporteProgramadoEN.usuarioId
                                        objPendiente.tipoReporte = ReporteProgramadoEN.tipoReporte
                                        objPendiente.secuencia = ReporteProgramadoEN.secuencia
                                        objPendiente.intervalo = ReporteProgramadoEN.intervalo
                                        objPendiente.parametros = ReporteProgramadoEN.parametros

                                        actualizar = negocioPendiente.ActualizarForzarFin(objPendiente)


                                    End If
                                End If
                                '******************************************************************************
                            End If

                        Catch ex As Exception
                            Console.WriteLine(ex.Message)


                        End Try



                    Next



                    Select Case Now.Minute
                        Case 57, 29
                            GC.Collect()
                        Case 0
                            Console.Clear()
                    End Select

                Catch ex As Exception

                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine(String.Format("Se produjo el siguiente error {0} a las {1}", ex.Message, Now))
                    Console.WriteLine(String.Format("Se produjo el siguiente error {0} a las {1}", ex.StackTrace(), Now))

                Finally
                    'DBS.Dispose()
                    'DBS = Nothing
                    Thread.Sleep(My.MySettings.Default.Espera)
                    'Finally
                    '    Thread.Sleep(My.MySettings.Default.Espera)

                End Try
            Else
                Console.ForegroundColor = ConsoleColor.White
                Console.WriteLine("No esta dentro del horario de atencion " & Now.ToString())
                Thread.Sleep(My.MySettings.Default.Espera)
            End If


        End While
    End Sub

    Private Sub ProcesarRegistros()
        Try
            While True

                Console.ForegroundColor = ConsoleColor.Cyan
                Console.WriteLine(Now.ToString() & " Iniciando carga de datos para reportes mensuales")
                'DBS = New ThorDataContext()
                'DBS.CommandTimeout = 10000000

                'Dim reportes = DBS.spReporteProgramadoListar_Procesar

                'For Each reporte In reportes
                '    Try
                '        Console.ForegroundColor = ConsoleColor.Gray
                '        Console.WriteLine(Now.ToString() & " Iniciando carga de datos para la secuencia {0} del tipo {1} ", reporte.Secuencia, reporte.TipoReporte)

                '        DBS.spProcesarReporteProgramado(reporte.Secuencia, reporte.usuarioId, reporte.Intervalo, reporte.TipoReporte)

                '        Console.WriteLine(Now.ToString() & " Registros Procesados para la secuencia {0} del tipo {1} ", reporte.Secuencia, reporte.TipoReporte)
                '    Catch ex As Exception
                '        Console.WriteLine(ex.Message)
                '    End Try
                'Next

                Console.ForegroundColor = ConsoleColor.Cyan
                Console.WriteLine(Now.ToString() & " Registros procesados para los reportes mensuales...")

                Thread.Sleep(My.MySettings.Default.EsperaLoadData)

            End While
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(ex.Message.ToUpper())
        Finally
            'DBS.Dispose()
            'DBS = Nothing
        End Try
    End Sub

    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180828
    'PARA VERIFICAR QUE LA HORA ACTUAL ESTE DENTRO DEL RANGO DE HORAS DE ATENCION DE LOS REPORTES PROGRAMADOS
    '******************************************************************************
    Private Function CheckTimeRange(myDate As TimeSpan, minTime As TimeSpan, maxTime As TimeSpan) As Boolean
        If minTime > maxTime Then
            Return myDate >= minTime OrElse myDate < maxTime
        Else
            Return myDate >= minTime AndAlso myDate < maxTime
        End If
    End Function

End Module