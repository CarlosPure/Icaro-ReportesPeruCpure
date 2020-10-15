Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports System.Text
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades

Public Class cKilometraje : Inherits cGeneracion

    Private dsKilometraje As DataSet
    Private Const Reporte As String = "Reporte de Kilometrajes de la Flota"
    Private NCampos As New ArrayList()
    Private hConsumo As New Hashtable()
    Private ConsumoPromedio As Double = 0.0

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
        Console.WriteLine(Datos.usuarioId.ToString() & ": Obteniendo Datos para el Reporte Kilometraje")

        Try
            DatoTipo = Datos.tipoReporte.Split(";")
            DatoActivos = Datos.activos.Split(";")
            DatoEmail = Datos.email.Split(";")

            Dim sh As ISheet = wk.CreateSheet("Flota")
            With sh
                .CreateRow(0).CreateCell(0).SetCellValue(Reporte)
                .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 4))
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
            '    Dim file As FileStream

            '    file = New FileStream(Me.Archivo, FileMode.Create)
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
            '    '               Archivo,
            '    '               "Adjunto Email con el Reporte Solicitado",
            '    '                Nothing,
            '    '                Reporte,
            '    '                "ALR",
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
            '                "ALR",
            '                Datos.IdUsuario)
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

    Private Function GenerarReporte(ByVal IdUsuario As Integer,
                               ByVal Intervalo As String,
                               ByVal Parametros As String,
                               ByVal Hoja As ISheet) As String
        Try
            Hoja.Header.Center = "Reporte Kilometraje Flota"
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

            dsKilometraje = Nothing
            dsKilometraje = New DataSet()

            DbConnection = Nothing
            DbConnection = New SqlConnection(My.Settings.GEOSYSConnectionStringAux)

            If DbConnection.State = ConnectionState.Open Then
                DbConnection.Close()
            End If
            DbConnection.Open()

            DbDataAdapter = Nothing

            dsKilometraje = Nothing
            dsKilometraje = New DataSet()

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'SE COMENTO LINEA PARA QUE AL PRESENTARSE UN ERROR NO MANEJE EL ERROR Y NO ENVIE CORREO
            '******************************************************************************
            'Try
            '******************************************************************************

            Console.ForegroundColor = ConsoleColor.Magenta

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
            'MOSTRAR MENSAJE EN LA CONSOLA CON FECHAINICIO Y FECHAFIN
            '******************************************************************************
            'Console.WriteLine("Obteniendo Kilometraje de la Flota: ")
            Console.WriteLine(String.Format("Obteniendo Kilometraje de la Flota: {0} {1}", Fecha2ANSI(Me.FechaInicial.ToString(), True), Fecha2ANSI(Me.FechaFinal.ToString(), True)))
            '******************************************************************************





            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
            'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
            'PARA LLAMAR AL PROCEDIMIENTO DE ACUERDO A LA VARIABLE PAIS
            'PARA UTILIZAR OTRO PROCEDIMIENTO ALMACENADO QUE DEVUELVE LOS VALORES DE LAS PLACAS EN FORMA HORIZONTAL
            '******************************************************************************

            Dim sql As String

            If (My.Settings.Pais = "PE") Then

                Dim ParametroArray1 As String()
                Dim Parametros1 As String

                ParametroArray1 = Parametros.Split(";")

                If ParametroArray1.Count <= 1 Then
                    Parametros1 = Parametros
                Else
                    Parametros1 = Parametros.Split(";")(2)
                End If

                If Parametros1 = "CSV" Then

                    sql = String.Format("[spGenerarReporteKMSCSV] '{0}','{1}','{2}','{3}','{4}'",
                           IdUsuario,
                           Fecha2ANSI(FechaInicial.ToString(), True),
                           Fecha2ANSI(FechaFinal.ToString(), True),
                           Me.HoraInicio,
                           Me.HoraFin)
                Else
                    sql = String.Format("[spGenerarReporteMesKMS2_Vertical] '{0}','{1}','{2}','129001','{3}','{4}','1','False'",
                           IdUsuario,
                           Fecha2ANSI(FechaInicial.ToString(), True),
                           Fecha2ANSI(FechaFinal.ToString(), True),
                           Me.HoraInicio,
                           Me.HoraFin)
                End If

            Else
                sql = String.Format("[spGenerarReporteMesKMS_] '{0}','{1}','{2}','129001','00:00:00','23:59:59','1','False'",
                                                        IdUsuario,
                                                        Fecha2ANSI(FechaInicial.ToString(), True),
                                                        Fecha2ANSI(FechaFinal.ToString(), True))
            End If


            DbDataAdapter = New SqlDataAdapter(sql, DbConnection)
            '******************************************************************************


            If My.Settings.Debug = True Then
                Console.WriteLine(DbDataAdapter.SelectCommand.CommandText)
            End If
            DbDataAdapter.SelectCommand.CommandTimeout = 3000000
            DbDataAdapter.Fill(dsKilometraje, "Kilometraje")
            Console.WriteLine("Registros Obtenidos para la Flota " & dsKilometraje.Tables(0).Rows.Count())
            dsKilometraje.AcceptChanges()

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180914
            'SE COMENTO LINEA PARA QUE AL PRESENTARSE UN ERROR NO MANEJE EL ERROR Y NO ENVIE CORREO
            'TAMBIEN PARA COMENTAR LINEA DE CODIGO REPETIDA
            '******************************************************************************
            'Catch ex As Exception
            '    Console.Write(ex.Message)

            'End Try
            'dsKilometraje.AcceptChanges()
            '******************************************************************************


            Try
                dsKilometraje.Tables(0).Columns.Remove("FechaCompleta")
            Catch ex As Exception

            End Try
            dsKilometraje.AcceptChanges()

            NCampos.Clear()
            hConsumo.Clear()


            TotalRegistros = dsKilometraje.Tables(0).Rows.Count

            Dim ParametroArray As String()

            ParametroArray = Parametros.Split(";")

            If ParametroArray.Count <= 1 Then
                Parametros = Parametros
            Else
                Parametros = Parametros.Split(";")(2)
            End If

            Dim sb As New StringBuilder()

            If Parametros = "CSV" Then



                Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                For Each dColum As DataColumn In dsKilometraje.Tables(0).Columns
                    'Celda = Fila.CreateCell(dColum.Ordinal)
                    'Hoja.AutoSizeColumn(dColum.Ordinal)
                    'Celda.CellStyle.Alignment = HorizontalAlignment.Center

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
                            'Celda.SetCellValue(dColum.ColumnName.ToUpper())
                            sb.Append(dColum.ColumnName.ToUpper() & ";")
                    End Select

                    'Celda.CellStyle = stlNada
                Next

                sb.AppendLine()

                dsKilometraje.AcceptChanges()

                Console.ForegroundColor = ConsoleColor.DarkYellow
                Console.WriteLine("Generando Archivo de " & dsKilometraje.Tables(0).Rows.Count.ToString() & " Filas")
                For Each oRow As DataRow In dsKilometraje.Tables(0).Rows
                    Console.ForegroundColor = ConsoleColor.DarkGray
                    Console.WriteLine("Escribiendo Fila # " & Hoja.LastRowNum.ToString())
                    'Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                    For Each dColum As DataColumn In dsKilometraje.Tables(0).Columns
                        'Celda = Fila.CreateCell(dColum.Ordinal)
                        'Hoja.AutoSizeColumn(dColum.Ordinal)
                        'Celda.CellStyle.Alignment = HorizontalAlignment.Center
                        'Celda.CellStyle = stlNormal

                        'Celda.SetCellValue(oRow(dColum.ColumnName).ToString())
                        sb.Append(oRow(dColum.ColumnName).ToString() & ";")
                    Next
                    sb.AppendLine()
                Next
                Console.WriteLine("Fin de Generacion de Archivo CVS para Envio")


                If sb.Length > 0 Then
                    'Dim file As FileStream

                    'file = New FileStream(Me.Archivo, FileMode.Create)
                    'wk.Write(file)
                    'file.Close()
                    'file.Dispose()
                    'file = Nothing
                    'wk.Close()
                    'wk = Nothing

                    Me.Archivo = Me.Archivo.Replace(".xls", "") & ".csv"

                    Dim ArchivoTEXT As String = Me.Archivo
                    Dim fileTEXT As System.IO.StreamWriter
                    fileTEXT = My.Computer.FileSystem.OpenTextFileWriter(ArchivoTEXT, False)
                    fileTEXT.Write(sb.ToString())
                    fileTEXT.Close()


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

            Else

                If TotalRegistros < My.Settings.TotalRegistrosMaximo Then
                    Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                    For Each dColum As DataColumn In dsKilometraje.Tables(0).Columns
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
                    dsKilometraje.AcceptChanges()

                    Console.ForegroundColor = ConsoleColor.DarkYellow
                    Console.WriteLine("Generando Archivo de " & dsKilometraje.Tables(0).Rows.Count.ToString() & " Filas")
                    For Each oRow As DataRow In dsKilometraje.Tables(0).Rows
                        Console.ForegroundColor = ConsoleColor.DarkGray
                        Console.WriteLine("Escribiendo Fila # " & Hoja.LastRowNum.ToString())
                        Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                        For Each dColum As DataColumn In dsKilometraje.Tables(0).Columns
                            Celda = Fila.CreateCell(dColum.Ordinal)
                            Hoja.AutoSizeColumn(dColum.Ordinal)
                            Celda.CellStyle.Alignment = HorizontalAlignment.Center
                            Celda.CellStyle = stlNormal

                            Celda.SetCellValue(oRow(dColum.ColumnName).ToString())
                        Next
                    Next
                    Console.WriteLine("Fin de Generacion de Archivo XLS para Envio")
                Else
                    Dim vnxls As New fGrid(dsKilometraje, Me.Archivo, CDbl(Parametros))
                    vnxls.GenerarArchivo()
                End If
                dsKilometraje = Nothing

                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180914
                'PARA LLAMAR AL PROCESO ENVIARMAIL UNA VEZ SE HAYA GENERADO EL ARCHIVO EXCEL
                '******************************************************************************
                If wk.NumberOfSheets > 0 Then
                    Dim file As FileStream

                    file = New FileStream(Me.Archivo, FileMode.Create)
                    wk.Write(file)
                    file.Close()
                    file.Dispose()
                    file = Nothing
                    wk.Close()
                    wk = Nothing


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
