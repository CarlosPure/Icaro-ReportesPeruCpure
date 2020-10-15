Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades

Public Class cVelocidad : Inherits cGeneracion

    Private dsVelocidad As DataSet
    Private Const Reporte As String = "Reporte de Velocidad de la Flota"

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
        Console.WriteLine(String.Format("{0}: Obteniendo Datos para el Reporte de Velocidad", Datos.usuarioId))

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
                GenerarReporte(Datos.secuencia, Datos.usuarioId,
                              Datos.intervalo,
                              Datos.parametros,
                              sh)
            Else
                GuardarLog(Me.Tipo, "SINVEHICULOS")
                Console.WriteLine(String.Format("El usuario :  {0} no cuenta con vehiculos activos", Me.Usuario))

            End If



            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180914
            'PARA LLAMAR AL PROCESO ENVIARMAIL UNA VEZ SE HAYA GENERADO EL ARCHIVO EXCEL
            '******************************************************************************
            'If wk.NumberOfSheets > 0 Then
            '    Dim file As FileStream

            '    If TotalRegistros < 1000 Then
            '        file = New FileStream(Me.Archivo, FileMode.Create)
            '        wk.Write(file)
            '        file.Close()
            '        file.Dispose()
            '        file = Nothing
            '        wk.Close()
            '        wk = Nothing
            '    End If


            '    Try
            '        EnviarMail(Datos.Email,
            '               Archivo,
            '               "Adjunto Email con el Reporte Solicitado",
            '                Nothing,
            '                Reporte,
            '                "VEL",
            '                Datos.usuarioId)
            '    Catch ex As Exception
            '        ErroresEnvio &= "ERR_MAIL"
            '    End Try
            'End If

            '******************************************************************************

            DatoEmail = Nothing
            DatoActivos = Nothing
            DatoTipo = Nothing

            'DBS25.Dispose()
            'DBS25 = Nothing
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Function GenerarReporte(ByVal Secuencia As Integer,
                                   ByVal IdUsuario As Integer,
                               ByVal Intervalo As String,
                               ByVal Parametros As String,
                               ByVal Hoja As ISheet) As String
        Try
            Hoja.Header.Center = "Reporte Velocidad Flota"
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

            dsVelocidad = Nothing
            dsVelocidad = New DataSet()

            DbConnection = Nothing
            DbConnection = New SqlConnection(My.Settings.GEOSYSConnectionStringAux)

            If DbConnection.State = ConnectionState.Open Then
                DbConnection.Close()
            End If
            DbConnection.Open()

            DbDataAdapter = Nothing

            dsVelocidad = Nothing
            dsVelocidad = New DataSet()

            Dim tmpVelocidad As DataSet = Nothing
            tmpVelocidad = New DataSet()
            hsVehiculos.Clear()

            Dim iActivos As Integer = 0

            'Dim tActivos = DBS25.spActivosSinReportar(CInt(IdUsuario), "127002", "")

            Dim negocio As New ActivoBL()
            Dim obj As New ActivoEN
            obj.baseDatosId = 25
            obj.usuarioId = CInt(IdUsuario)


            Dim lista As List(Of ActivoEN) = negocio.ListarActivosSinReportar(obj)


            For Each tActivo In lista
                Try
                    tmpVelocidad.Clear()
                    tmpVelocidad.AcceptChanges()
                    Console.ForegroundColor = ConsoleColor.Magenta

                    '******************************************************************************
                    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                    'MOSTRAR MENSAJE EN LA CONSOLA CON PLACA, FECHAINICIO Y FECHAFIN
                    '******************************************************************************
                    'Console.WriteLine(String.Format("Obteniendo Velocidades de: {0}", tActivo.Alias))
                    Console.WriteLine(String.Format("Obteniendo Velocidades de:  {0} {1} {2}", tActivo.alias, Fecha2ANSI(Me.FechaInicial.ToString(), True), Fecha2ANSI(Me.FechaFinal.ToString(), True)))
                    '******************************************************************************

                    Dim sql As String

                    If Intervalo Like "*H" Then
                        '******************************************************************************
                        'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                        'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
                        '******************************************************************************
                        sql = String.Format("spReporteProgramadoVelocidad '{0}','{1}','{2}',{3},'{4}'",
                                    Secuencia,
                                    tActivo.vid,
                                    IdUsuario,
                                    Fecha2ANSI(Me.FechaInicial.ToString(), False),
                                    Fecha2ANSI(Me.FechaFinal.ToString(), False))

                        DbDataAdapter = New SqlDataAdapter(sql, DbConnection)
                        '******************************************************************************

                    Else
                        '******************************************************************************
                        'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                        'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
                        'PARA LLAMAR AL PROCEDIMIENTO DE ACUERDO A LA VARIABLE PAIS
                        '******************************************************************************
                        If (My.Settings.Pais = "PE") Then
                            sql = String.Format("spReporteProgramadoVelocidad '{0}','{1}','{2}','{3}','{4}'",
                                    Secuencia,
                                    tActivo.vid,
                                    IdUsuario,
                                    Fecha2ANSI(Me.FechaInicial.ToString(), True) & " " & Me.HoraInicio,
                                    Fecha2ANSI(Me.FechaFinal.ToString(), True) & " " & Me.HoraFin)
                        Else
                            sql = String.Format("spReporteProgramadoVelocidad '{0}','{1}','{2}','{3}','{4}'",
                                    Secuencia,
                                    tActivo.vid,
                                    IdUsuario,
                                    Fecha2ANSI(Me.FechaInicial.ToString(), True) & " 00:00:00",
                                    Fecha2ANSI(Me.FechaFinal.ToString(), True) & " 23:59:59")
                        End If


                        DbDataAdapter = New SqlDataAdapter(sql, DbConnection)
                        '******************************************************************************

                    End If

                    If My.Settings.Debug = True Then
                        Console.WriteLine(DbDataAdapter.SelectCommand.CommandText)
                    End If

                    hsVehiculos.Add(tActivo.vid, tActivo.alias)

                    DbDataAdapter.SelectCommand.CommandTimeout = 3000000
                    DbDataAdapter.Fill(tmpVelocidad, "Velocidad")
                    tmpVelocidad.AcceptChanges()

                    Console.WriteLine(String.Format("Registros Obtenidos para {0} {1}", tActivo.alias, tmpVelocidad.Tables(0).Rows.Count()))

                    dsVelocidad.Merge(tmpVelocidad)
                Catch ex As Exception
                    Console.Write(ex.Message)
                End Try

                iActivos = iActivos + 1
            Next
            dsVelocidad.AcceptChanges()


            If iActivos > 0 Then

                TotalRegistros = dsVelocidad.Tables(0).Rows.Count

                If TotalRegistros < My.Settings.TotalRegistrosMaximo Then
                    Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                    For Each dColum As DataColumn In dsVelocidad.Tables(0).Columns
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
                    dsVelocidad.AcceptChanges()

                    Console.ForegroundColor = ConsoleColor.DarkYellow
                    Console.WriteLine(String.Format("Generando Archivo de {0} Filas", dsVelocidad.Tables(0).Rows.Count))

                    If IsNumeric(Parametros) Then
                        Parametros = Parametros
                    Else
                        Parametros = Parametros.Split(";")(2)
                    End If

                    Dim cuentaVelocidadAlta As Integer = 0

                    For Each oRow As DataRow In dsVelocidad.Tables(0).Rows
                        If oRow("Speed") >= CDbl(Parametros) Then
                            Console.ForegroundColor = ConsoleColor.DarkGray
                            Console.WriteLine(String.Format("Escribiendo Fila # {0}", Hoja.LastRowNum))

                            cuentaVelocidadAlta = cuentaVelocidadAlta + 1

                            Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                            For Each dColum As DataColumn In dsVelocidad.Tables(0).Columns
                                Celda = Fila.CreateCell(dColum.Ordinal)
                                Celda.CellStyle.Alignment = HorizontalAlignment.Center
                                Celda.CellStyle = stlNormal

                                Celda.SetCellValue(oRow(dColum.ColumnName).ToString())
                            Next
                        End If

                        'cuando el reporte no cuenta con velocidades altas
                        
                    Next

                    If cuentaVelocidadAlta = 0 Then
                        Console.ForegroundColor = ConsoleColor.DarkGray
                        Console.WriteLine(String.Format("Escribiendo Fila # {0}  - ya que no se encontro velocidades altas", Hoja.LastRowNum))


                        Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                        Celda = Fila.CreateCell(0)
                        Celda.CellStyle.Alignment = HorizontalAlignment.Center
                        Celda.CellStyle = stlNormal

                        Celda.SetCellValue(String.Format("En el periodo del presente reporte no se alcanzo velocidades mayores a: ", Parametros))

                    End If

                    For Each dColum As DataColumn In dsVelocidad.Tables(0).Columns
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
                        vnxls = New fGrid(dsVelocidad, Archivo, CDbl(Parametros))
                    Else
                        vnxls = New fGrid(dsVelocidad, Archivo, CDbl(Parametros.Split(";")(2)))
                    End If

                    vnxls.GenerarArchivo()
                End If
                dsVelocidad = Nothing

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
                           Archivo,
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
