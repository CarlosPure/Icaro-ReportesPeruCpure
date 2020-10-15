Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades

Public Class cTrayectosDetalle : Inherits cGeneracion

    Private dsTrayectos As DataSet
    Private dsFlota As DataSet
    Private Const Reporte As String = "Reporte de Trayectos de la Flota"

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

        Console.WriteLine(Datos.usuarioId.ToString() & ": Obteniendo Datos para el Reporte Detalle de Trayectos")

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

            Dim negocio As New ActivoBL()
            Dim obj As New ActivoEN
            obj.baseDatosId = 25
            obj.usuarioId = CInt(IdUsuario)
            obj.entidadId = "0"

            Dim cantidad As Integer = negocio.ListarVehiculosActivos(obj)

            If cantidad > 0 Then
                GenerarReporte(Datos.usuarioId,
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
            '    Dim file As FileStream = New FileStream(Me.Archivo, FileMode.Create)
            '    wk.Write(file)
            '    file.Close()
            '    file.Dispose()
            '    file = Nothing
            '    wk.Close()

            '    Try
            '        EnviarMail(Datos.Email,
            '               Archivo,
            '               "Adjunto Email con el Reporte Solicitado",
            '                wk.GetSheet(0),
            '                Reporte,
            '                "TRYD",
            '                Datos.idUsuario)
            '    Catch ex As Exception
            '        ErroresEnvio &= "ERR_MAIL"
            '    End Try
            'End If
            '******************************************************************************
            wk = Nothing
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
            Hoja.Header.Center = "Reporte Detalle Trayectos Flota"
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

            dsFlota = Nothing
            dsFlota = New DataSet()

            dsTrayectos = Nothing
            dsTrayectos = New DataSet()

            DbConnection = Nothing
            DbConnection = New SqlConnection(My.Settings.GEOSYSConnectionStringAux)

            If DbConnection.State = ConnectionState.Open Then
                DbConnection.Close()
            End If
            DbConnection.Open()

            DbDataAdapter = Nothing

            dsTrayectos = Nothing
            dsTrayectos = New DataSet()

            Dim tmpTrayectos As DataSet
            tmpTrayectos = Nothing
            tmpTrayectos = New DataSet()
            'Dim tActivos = DBS25.spActivosSinReportar(CInt(IdUsuario), "127002", "")





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
                'Console.WriteLine(String.Format("Registros Obtenidos para la Flota {0}", dsFlota.Tables(0).Rows.Count()))
                dsFlota.AcceptChanges()

            Catch ex As Exception
                Console.Write(ex.Message)
            End Try
            dsFlota.AcceptChanges()


            DbConnection = Nothing
            DbConnection = New SqlConnection(My.Settings.GEOSYSConnectionStringAux)


            'Dim nroActivos As Integer = tActivos.Count()

            'If (nroActivos > 0) Then
            If (dsFlota.Tables(0).Rows.Count > 0) Then


                'For Each tActivo In tActivos
                For Each tActivo In dsFlota.Tables(0).Rows
                    With tActivo
                        Try
                            tmpTrayectos.Clear()
                            tmpTrayectos.AcceptChanges()
                            Console.ForegroundColor = ConsoleColor.Magenta

                            '******************************************************************************
                            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                            'MOSTRAR MENSAJE EN LA CONSOLA CON PLACA, FECHAINICIO Y FECHAFIN
                            '******************************************************************************
                            'Console.WriteLine("Obteniendo Detalle Trayectos de: " & tActivo.Alias)
                            'Console.WriteLine(String.Format("Obteniendo Detalle Trayectos de:  {0} {1} {2}", tActivo.Alias, Fecha2ANSI(Me.FechaInicial.ToString(), True), Fecha2ANSI(Me.FechaFinal.ToString(), True)))
                            Console.WriteLine(String.Format("Obteniendo Detalle Trayectos de:  {0} {1} {2}", .Item("Alias"), Fecha2ANSI(Me.FechaInicial.ToString(), True), Fecha2ANSI(Me.FechaFinal.ToString(), True)))
                            '******************************************************************************

                            Dim sql As String

                            If Intervalo Like "*H" Then

                                '******************************************************************************
                                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                                'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
                                '******************************************************************************
                                sql = String.Format("sp_ReporteProgramadoTrayecto '{0}','{1}','{2}','{3}','{4}'",
                                            IdUsuario,
                                            Fecha2ANSI(FechaInicial.ToString(), False),
                                            Fecha2ANSI(FechaFinal.ToString(), False),
                                            "127002",
                                            .Item("VID"))

                                DbDataAdapter = New SqlDataAdapter(sql, DbConnection)
                                '******************************************************************************

                            Else

                                '******************************************************************************
                                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                                'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
                                'PARA LLAMAR AL PROCEDIMIENTO DE ACUERDO A LA VARIABLE PAIS
                                '******************************************************************************
                                If (My.Settings.Pais = "PE") Then
                                    sql = String.Format("sp_ReporteProgramadoTrayecto '{0}','{1}','{2}','{3}','{4}','{5}'",
                                            IdUsuario,
                                            Fecha2ANSI(FechaInicial.ToString(), True) & " " & Me.HoraInicio,
                                            Fecha2ANSI(FechaFinal.ToString(), True) & " " & Me.HoraFin,
                                            "127002",
                                            .Item("VID"),
                                            Me.Secuencia
                                            )
                                Else
                                    sql = String.Format("sp_ReporteProgramadoTrayecto '{0}','{1}','{2}','{3}','{4}','{5}'",
                                            IdUsuario,
                                            Fecha2ANSI(FechaInicial.ToString(), True) & " 00:00:00",
                                            Fecha2ANSI(FechaFinal.ToString(), True) & " 23:59:59",
                                            "127002",
                                            .Item("VID"),
                                            Me.Secuencia
                                            )
                                End If


                                DbDataAdapter = New SqlDataAdapter(sql, DbConnection)
                                '******************************************************************************

                            End If

                            If My.Settings.Debug = True Then
                                Console.WriteLine(DbDataAdapter.SelectCommand.CommandText)
                            End If

                            DbDataAdapter.SelectCommand.CommandTimeout = 3000000
                            DbDataAdapter.Fill(tmpTrayectos, "Trayectos")
                            tmpTrayectos.AcceptChanges()

                            Console.WriteLine(String.Format("Registros Obtenidos para {0} {1}", .Item("Alias"), tmpTrayectos.Tables(0).Rows.Count()))

                            dsTrayectos.Merge(tmpTrayectos)
                        Catch ex As Exception
                            Console.Write(ex.Message)
                        End Try
                    End With

                Next
                dsTrayectos.AcceptChanges()

                'Console.ForegroundColor = ConsoleColor.DarkGray
                'For Each drow As DataRow In dsTrayectos.Tables(0).Rows
                '    Dim Vid As String = ""

                '    Dim negocioActivo As New ActivoBL()
                '    Dim objActivo As New ActivoEN

                '    objActivo.baseDatosId = 25
                '    objActivo.alias = drow("Alias")

                '    objActivo = negocioActivo.GetVidPorPlaca(objActivo)


                '    'Vid = DBS25.GetVIDPlaca(drow("Alias"))'
                '    Vid = objActivo.vid


                'Next

                'dsTrayectos.AcceptChanges()



                If (dsTrayectos.Tables(0).Rows.Count > 0) Then



                    Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                    Console.ForegroundColor = ConsoleColor.DarkYellow
                    Console.WriteLine("Generando Archivo de " & dsTrayectos.Tables(0).Rows.Count.ToString() & " Filas")
                    For Each dColum As DataColumn In dsTrayectos.Tables(0).Columns
                        Celda = Fila.CreateCell(dColum.Ordinal)
                        Hoja.AutoSizeColumn(dColum.Ordinal)
                        Celda.CellStyle.Alignment = HorizontalAlignment.Center

                        Select Case dColum.ColumnName
                            Case "tHorasConduccion"
                                Celda.SetCellValue("TIEMPO CONDUCCION")
                            Case "tHorasRalenti"
                                Celda.SetCellValue("TIEMPO RALENTI")
                            Case "tTiempoTotal"
                                Celda.SetCellValue("TIEMPO TOTAL")
                            Case Else
                                Celda.SetCellValue(dColum.ColumnName.ToUpper())
                        End Select
                        Celda.CellStyle = stlNada
                    Next

                    For Each oRow As DataRow In dsTrayectos.Tables(0).Rows
                        Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                        Console.ForegroundColor = ConsoleColor.DarkGray
                        Console.WriteLine("Escribiendo Fila # " & Hoja.LastRowNum.ToString())
                        For Each dColum As DataColumn In dsTrayectos.Tables(0).Columns
                            Celda = Fila.CreateCell(dColum.Ordinal)
                            Celda.CellStyle.Alignment = HorizontalAlignment.Center
                            Celda.CellStyle = stlNormal

                            Celda.SetCellValue(oRow(dColum.ColumnName).ToString())
                        Next
                    Next

                    For Each dColum As DataColumn In dsTrayectos.Tables(0).Columns
                        Try
                            Hoja.AutoSizeColumn(dColum.Ordinal)
                        Catch ex As Exception

                        End Try
                    Next
                End If



                dsTrayectos = Nothing

                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180914
                'PARA LLAMAR AL PROCESO ENVIARMAIL UNA VEZ SE HAYA GENERADO EL ARCHIVO EXCEL
                '******************************************************************************
                If wk.NumberOfSheets > 0 Then
                    Dim file As FileStream = New FileStream(Me.Archivo, FileMode.Create)
                    wk.Write(file)
                    file.Close()
                    file.Dispose()
                    file = Nothing
                    wk.Close()

                    Try
                        EnviarMail(Me.Email,
                               Archivo,
                               "Adjunto Email con el Reporte Solicitado",
                                wk.GetSheet(0),
                                Reporte,
                                Me.Tipo,
                                Me.IdUsuario)
                    Catch ex As Exception
                        ErroresEnvio &= "ERR_MAIL"
                    End Try
                End If
                '******************************************************************************



            Else
                'If (dsTrayectos.Tables(0).Rows.Count > 0) Then


                Dim nombreArchivo As String = String.Format("{0}{1}.txt",
                                                         System.AppDomain.CurrentDomain.BaseDirectory,
                                                         Me.Usuario)

                Dim ArchivoTEXT As String = nombreArchivo

                Dim fileTEXT As System.IO.StreamWriter
                fileTEXT = My.Computer.FileSystem.OpenTextFileWriter(ArchivoTEXT, False)
                fileTEXT.Write("El usuario no cuenta con unidades en el periodo de tiempo indicado.")
                fileTEXT.Close()

                Me.Archivo = nombreArchivo

                Try
                    EnviarMail(Me.Email,
                           Me.Archivo,
                           "Adjunto Email con el Reporte Solicitado",
                            wk.GetSheet(0),
                            Reporte,
                            Me.Tipo,
                            Me.IdUsuario)
                Catch ex As Exception
                    ErroresEnvio &= "ERR_MAIL"
                End Try


                'End If

            End If






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
