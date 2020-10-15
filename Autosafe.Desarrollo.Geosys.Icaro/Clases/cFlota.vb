Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports Autosafe.Desarrollo.Geosys.Entidades
Imports Autosafe.Desarrollo.Geosys.Negocios

Public Class cFlota : Inherits cGeneracion

    Private dsFlota As DataSet
    Private Const Reporte As String = "Reporte de Estado de la Flota"
    Private contPorcDetenidasI As Double = 0.0
    Private contDetenidasI As Integer = 0
    Private unDetenidasI As String = ""
    Private contBateria As Integer = 0
    Private contVoltaje As Integer = 0
    Private contNivel As Integer = 0
    Private contPuntos As Integer = 0
    Private contActividad As Integer = 0
    Private contHorometro As Integer = 0
    Private contEA1 As Integer = 0
    Private contEA2 As Integer = 0
    Private contEA3 As Integer = 0
    Private contSA1 As Integer = 0
    Private contSA2 As Integer = 0
    Private contSA3 As Integer = 0
    Private contDriverID As Integer = 0
    Private contCE As Integer = 0
    Private contVelocidadOBD As Integer = 0
    Private contrpmOBD As Integer = 0
    Private contPosicionAceleradorOBD As Integer = 0
    Private contOdometroOBD As Integer = 0
    Private contOdometroViajeOBD As Integer = 0
    Private contNivelGasolinaOBD As Integer = 0
    Private contCombustibleRestanteOBD As Integer = 0
    Private contEngraneTransmisionOBD As Integer = 0
    Private contTemperaturaRefrigeranteOBD As Integer = 0
    Private contIndiceGasolinaOBD As Integer = 0
    Private contVoltajeAlimentacionOBD As Integer = 0
    Private contEstadoSeñalesGiroOBD As Integer = 0
    Private contGasolinaConsumidaPorViajeOBD As Integer = 0
    Private contIndicadoresOBD As Integer = 0

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
        Console.WriteLine(String.Format("{0}: Obteniendo Datos para el Reporte de Estado de la Flota", Datos.usuarioId))

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
                Dim sh As ISheet = wk.CreateSheet("EstadoFlota")
                With sh
                    .CreateRow(0).CreateCell(0).SetCellValue(Reporte)
                    .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 20))
                End With

                GenerarReporte(Datos.usuarioId,
                              Datos.intervalo,
                              Datos.parametros,
                              sh)

                If wk.NumberOfSheets > 0 Then
                    Dim file As FileStream

                    file = New FileStream(Me.Archivo, FileMode.Create)
                    wk.Write(file)
                    file.Close()
                    file.Dispose()
                    file = Nothing
                    wk.Close()

                    Try
                        EnviarMail(Datos.email,
                           Archivo,
                           "Adjunto Email con el Reporte Solicitado",
                            wk.GetSheet(0),
                            Reporte,
                            "FLO",
                            Datos.usuarioId)
                    Catch ex As Exception
                        ErroresEnvio &= "ERR_MAIL"
                    End Try
                End If
                wk = Nothing
            Else
                GuardarLog(Me.Tipo, "SINVEHICULOS")
                Console.WriteLine(String.Format("El usuario :  {0} no cuenta con vehiculos activos", Me.Usuario))

            End If



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
            Hoja.Header.Center = "Reporte Estado Flota"
            Fila = Hoja.CreateRow(1)
            Fila.CreateCell(0).SetCellValue("Fecha: " & Now.ToString())
            Fila = Hoja.CreateRow(2)
            'Fila.CreateCell(0).SetCellValue("Hasta: " & Fecha2ANSI(FechaFinal.ToString(), True) & " 23:59:59")
            Fila = Hoja.CreateRow(3)
            'Fila = Hoja.CreateRow(4)

            dsFlota = Nothing
            dsFlota = New DataSet()

            DbConnection = Nothing
            DbConnection = New SqlConnection(My.Settings.GEOSYSConnectionStringMain)

            If DbConnection.State = ConnectionState.Open Then
                DbConnection.Close()
            End If
            DbConnection.Open()

            DbDataAdapter = Nothing

            dsFlota = Nothing
            dsFlota = New DataSet()

            Try
                Console.ForegroundColor = ConsoleColor.Magenta
                Console.WriteLine("Obteniendo Estado de la Flota ")
                DbDataAdapter = New SqlDataAdapter(String.Format("[spActivosSinReportar] '{0}'",
                                                        IdUsuario.ToString()), DbConnection)
                If My.Settings.Debug = True Then
                    Console.WriteLine(DbDataAdapter.SelectCommand.CommandText)
                End If
                DbDataAdapter.SelectCommand.CommandTimeout = 3000000
                DbDataAdapter.Fill(dsFlota, "Flota")
                Console.WriteLine(String.Format("Registros Obtenidos para la Flota {0}", dsFlota.Tables(0).Rows.Count()))
                dsFlota.AcceptChanges()

            Catch ex As Exception
                Console.Write(ex.Message)
            End Try
            dsFlota.AcceptChanges()

            Try
                With dsFlota.Tables(0).Columns
                    .Remove("IdEntidad")
                    .Remove("CodSysHunter")
                    .Remove("IdUsuario")
                    .Remove("idActivo")
                    .Remove("CE")
                    .Remove("IndicadoresOBD")
                    .Remove("Icono")
                End With
            Catch ex As Exception

            End Try

            For Each tmpRows In dsFlota.Tables(0).Rows
                With tmpRows
                    Try
                        .Item("DiasSinReportar") = .Item("DiasSinReportar").ToString.Replace("<b>", "").Replace("</b>", "")
                    Catch ex As Exception
                        .Item("DiasSinReportar") = ""
                    End Try

                    Try
                        .Item("Velocidad") = Round(.Item("Velocidad") * 1.609344, 0)
                    Catch ex As Exception
                        .Item("Velocidad") = .Item("Velocidad")
                    End Try

                    Try
                        If .Item("Velocidad").ToString() <> "0" Then
                            .Item("DetenidoDesde") = ""
                        End If
                    Catch ex As Exception
                        .Item("DetenidoDesde") = ""
                    End Try

                    Try
                        .Item("DiasSinReportar") = DateDiff(DateInterval.Day, .Item("UltimoReporteServidor"), Now())
                    Catch ex As Exception
                        .Item("DiasSinReportar") = ""
                    End Try

                    Try
                        .Item("Rumbo") = GetCourse(.Item("Rumbo"))
                    Catch ex As Exception
                        .Item("Rumbo") = ""
                    End Try

                    Try
                        .Item("Horometro") = Round(CInt(.Item("Horometro")) / 3600, 0)
                    Catch ex As Exception

                    End Try


                    If .Item("Ignicion") Is DBNull.Value Then
                        .Item("Ignicion") = 3
                    End If

                    Try
                        If .Item("EstadoGPS") = "1" Then
                            .Item("EstadoGPS") = "Si"
                        Else
                            .Item("EstadoGPS") = "NO"
                        End If
                    Catch ex As Exception
                    End Try

                    Try
                        .Item("Calle") = .Item("Calle").ToString.Replace("Ã³", "ó").ToUpper().Replace("Ã¡", "Á").Replace("Ã­", "Í").Replace("Ã©", "É").Replace("Ã±", "Ñ").Replace("Âº", "º")
                    Catch ex As Exception
                    End Try

                    If .Item("NivelBateria") Is DBNull.Value Then
                        .Item("NivelBateria") = ""
                    Else
                        .Item("NivelBateria") = Round(CDbl(.Item("NivelBateria")), 0) & " %"
                        contNivel += 1
                    End If

                    If .Item("VoltajeBateria") Is DBNull.Value Then
                        .Item("VoltajeBateria") = ""
                    Else
                        .Item("VoltajeBateria") = Round(CDbl(.Item("VoltajeBateria")), 2).ToString() & " V."
                        contBateria += 1
                    End If

                    If .Item("VoltajeAlimentacion") Is DBNull.Value Then
                        .Item("VoltajeAlimentacion") = ""
                    Else
                        contVoltaje += 1
                        Try
                            .Item("VoltajeAlimentacion") = Round(CDbl(.Item("VoltajeAlimentacion")) * 1.0, 2).ToString() & " V."
                        Catch ex As Exception
                            .Item("VoltajeAlimentacion") = ""
                        End Try
                    End If

                    Select Case .Item("Ignicion")
                        Case "1"
                            .Item("Ignicion") = "ON"
                        Case "0"
                            .Item("Ignicion") = "OFF"
                        Case Else
                            .Item("Ignicion") = ""
                    End Select

                    If Not .Item("EA1") Is DBNull.Value Then
                        contEA1 += 1
                        Try
                            .Item("EA1") = DBS25.ConvertirValorES(CInt(.Item("IdActivo")),
                                                                           "EA1",
                                                                           CDbl(.Item("EA1")))
                        Catch ex As Exception
                            .Item("EA1") = ""
                        End Try
                    End If

                    If Not .Item("EA2") Is DBNull.Value Then
                        contEA2 += 1
                        Try
                            .Item("EA2") = DBS25.ConvertirValorES(CInt(.Item("IdActivo")),
                                                                             "EA2",
                                                                             CDbl(.Item("EA2")))
                        Catch ex As Exception
                        End Try
                    End If

                    If Not .Item("EA3") Is DBNull.Value Then
                        contEA3 += 1
                        Try
                            .Item("EA3") = DBS25.ConvertirValorES(CInt(.Item("IdActivo")),
                                                                             "EA3",
                                                                             CDbl(.Item("EA3")))
                        Catch ex As Exception
                        End Try
                    End If

                    'Pendiente
                    If Not .Item("SA1") Is DBNull.Value Then
                        contSA1 += 1
                        Try
                            .Item("SA1") = DBS25.ConvertirValorES(CInt(.Item("IdActivo")),
                                                                             "SA1",
                                                                             CDbl(.Item("SA1")))
                        Catch ex As Exception
                        End Try
                    End If

                    If Not .Item("SA2") Is DBNull.Value Then
                        contSA2 += 1
                        Try
                            .Item("SA2") = DBS25.ConvertirValorES(CInt(.Item("IdActivo")),
                                                                            "SA2",
                                                                            CDbl(.Item("SA2")))
                        Catch ex As Exception
                        End Try
                    End If

                    If Not .Item("SA3") Is DBNull.Value Then
                        contSA3 += 1
                        Try
                            .Item("SA3") = DBS25.ConvertirValorES(CInt(.Item("IdActivo")),
                                                                            "SA3",
                                                                            CDbl(.Item("SA3")))
                        Catch ex As Exception
                        End Try
                    End If

                    Try
                        .Item("IdActivo") = Contador
                    Catch ex As Exception

                    End Try

                    contDriverID = 1

                    If Not .Item("VelocidadOBD") Is DBNull.Value Then
                        contVelocidadOBD += 1
                    End If

                    If Not .Item("rpmOBD") Is DBNull.Value Then
                        contrpmOBD += 1
                    End If

                    If Not .Item("PosicionAceleradorOBD") Is DBNull.Value Then
                        contPosicionAceleradorOBD += 1
                    End If

                    If Not .Item("OdometroOBD") Is DBNull.Value Then
                        contOdometroOBD += 1
                    End If

                    If Not .Item("OdometroViajeOBD") Is DBNull.Value Then
                        contOdometroViajeOBD += 1
                    End If

                    If Not .Item("NivelGasolinaOBD") Is DBNull.Value Then
                        contNivelGasolinaOBD += 1
                    End If

                    If Not .Item("CombustibleRestanteOBD") Is DBNull.Value Then
                        contCombustibleRestanteOBD += 1
                    End If

                    If Not .Item("EngraneTransmisionOBD") Is DBNull.Value Then
                        contEngraneTransmisionOBD += 1
                    End If

                    If Not .Item("TemperaturaRefrigeranteOBD") Is DBNull.Value Then
                        contTemperaturaRefrigeranteOBD += 1
                    End If

                    If Not .Item("IndiceGasolinaOBD") Is DBNull.Value Then
                        contIndiceGasolinaOBD += 1
                    End If

                    If Not .Item("VoltajeAlimentacionOBD") Is DBNull.Value Then
                        contVoltajeAlimentacionOBD += 1
                    End If

                    If Not .Item("EstadoSeñalesGiroOBD") Is DBNull.Value Then
                        contEstadoSeñalesGiroOBD += 1
                    End If

                    If Not .Item("GasolinaConsumidaPorViajeOBD") Is DBNull.Value Then
                        contGasolinaConsumidaPorViajeOBD += 1
                    End If

                    Try
                        .Item("DEvento") = DBS25.getDescripcionEvento(.Item("VID"), .Item("Evento"))
                    Catch ex As Exception
                        .Item("DEvento") = String.Format("N/D ({0})",
                                                                     .Item("Evento"))
                    End Try


                    Try
                        .Item("Pto. Cercano") = DBS25.GetNearestPoint(CDbl(.Item("Latitud")),
                                                                                   CDbl(.Item("Longitud")),
                                                                                   IdUsuario)
                    Catch ex As Exception
                        .Item("Pto. Cercano") = ""
                    End Try

                    .AcceptChanges()
                End With
            Next

            Try
                dsFlota.Tables(0).Columns.Remove("VID")
            Catch ex As Exception

            End Try

            dsFlota.AcceptChanges()
            TotalRegistros = dsFlota.Tables(0).Rows.Count

            If TotalRegistros < My.Settings.TotalRegistrosMaximo Then
                Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                For Each dColum As DataColumn In dsFlota.Tables(0).Columns
                    Celda = Fila.CreateCell(dColum.Ordinal)
                    Hoja.AutoSizeColumn(dColum.Ordinal)
                    Celda.CellStyle.Alignment = HorizontalAlignment.Center
                    Celda.SetCellValue(dColum.ColumnName.ToUpper())
                    Celda.CellStyle = stlNada
                Next
                dsFlota.AcceptChanges()

                Console.ForegroundColor = ConsoleColor.DarkYellow
                Console.WriteLine(String.Format("Generando Archivo de {0} Filas", dsFlota.Tables(0).Rows.Count))
                For Each oRow As DataRow In dsFlota.Tables(0).Rows
                    Console.ForegroundColor = ConsoleColor.DarkGray
                    Console.WriteLine(String.Format("Escribiendo Fila # {0}", Hoja.LastRowNum))
                    Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                    For Each dColum As DataColumn In dsFlota.Tables(0).Columns
                        Celda = Fila.CreateCell(dColum.Ordinal)
                        Hoja.AutoSizeColumn(dColum.Ordinal)
                        Celda.CellStyle.Alignment = HorizontalAlignment.Center
                        Celda.CellStyle = stlNormal

                        Celda.SetCellValue(oRow(dColum.ColumnName).ToString())
                    Next
                Next
                Console.WriteLine("Fin de Generacion de Archivo para Envio")
            Else
                Dim vnxls As New fGrid(dsFlota, Archivo, CDbl(Parametros))
                vnxls.GenerarArchivo()
            End If
            dsFlota = Nothing
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
