Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades

Public Class cResumenUbicacionGeografica : Inherits cGeneracion

    Private dsCalidadManejo As DataSet
    Private Const Reporte As String = "Reporte de Resumen de Ubicacion Geografica de la Flota"
    Private NCampos As New ArrayList()
    Private hConsumo As New Hashtable()
    Private ConsumoPromedio As Double = 0.0

    Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String)
        MyBase.New(TipoReporte, IdUsuario, Intervalo, Parametros)

    End Sub

    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20190711
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
        Console.WriteLine(Datos.usuarioId.ToString() & ": Obteniendo Datos para el Reporte de Resumen de Ubicacion Geografica")

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
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20190711
            'PARA GUARDAR LOS EMAILS A QUIENES LES LLEGARA EL REPORTE GENERADO
            '******************************************************************************
            Me.Email = Datos.email
            Me.IdEntidad = Datos.entidadId
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
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20190711
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
            Hoja.Header.Center = "Reporte Resumen de Ubicacion Geografica Flota"
            Fila = Hoja.CreateRow(1)
            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20190711
            'PARA 
            'PARA AGREGAR LA HORAINICIO Y HORAFIN EN LA CABECERA DEL REPORTE
            'PARA AGREGAR EL NOMBRE DEL USUARIO CORRESPONDIENTE AL ARCHIVO EXCEL
            '******************************************************************************

            Dim textoCelda As String

            'If (My.Settings.Pais = "PE") Then
            'textoCelda = String.Format("Desde: {0} {1}", Fecha2ANSI(Me.FechaInicial.ToString(), True), Me.HoraInicio)
            ' Else
            'extoCelda = String.Format("Desde: {0} 00:00:00", Fecha2ANSI(Me.FechaInicial.ToString(), True))
            'End If

            'Fila.CreateCell(0).SetCellValue(textoCelda)
            'Fila = Hoja.CreateRow(2)

            If (My.Settings.Pais = "PE") Then
                textoCelda = String.Format("Fecha: {0} {1}", Fecha2ANSI(Me.FechaFinal.ToString(), True), Me.HoraFin)
            Else
                textoCelda = String.Format("Hasta: {0} 23:59:59", Fecha2ANSI(Me.FechaFinal.ToString(), True))
            End If

            Fila.CreateCell(0).SetCellValue(textoCelda)
            Fila = Hoja.CreateRow(3)


            Fila.CreateCell(0).SetCellValue(String.Format("Cliente: {0}", Me.Nombre))
            '******************************************************************************

            Fila = Hoja.CreateRow(4)

            dsCalidadManejo = Nothing
            dsCalidadManejo = New DataSet()

            DbConnection = Nothing
            DbConnection = New SqlConnection(My.Settings.GEOSYSConnectionStringAux)

            If DbConnection.State = ConnectionState.Open Then
                DbConnection.Close()
            End If
            DbConnection.Open()

            DbDataAdapter = Nothing

            dsCalidadManejo = Nothing
            dsCalidadManejo = New DataSet()

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20190711
            'SE COMENTO LINEA PARA QUE AL PRESENTARSE UN ERROR NO MANEJE EL ERROR Y NO ENVIE CORREO
            '******************************************************************************
            'Try
            '******************************************************************************

            Console.ForegroundColor = ConsoleColor.Magenta

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20190711
            'MOSTRAR MENSAJE EN LA CONSOLA CON FECHAINICIO Y FECHAFIN
            '******************************************************************************
            'Console.WriteLine("Obteniendo Kilometraje de la Flota: ")
            Console.WriteLine(String.Format("Obteniendo Resumen de Ubicacion Geografica de la Flota: {0} {1}", Fecha2ANSI(Me.FechaInicial.ToString(), True), Fecha2ANSI(Me.FechaFinal.ToString(), True)))
            '******************************************************************************





            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20190711
            'GUARDAR EN UNA VARIABLE EL LLAMADO AL PROCEDIMIENTO ALMACENADO
            'PARA LLAMAR AL PROCEDIMIENTO DE ACUERDO A LA VARIABLE PAIS
            'PARA UTILIZAR OTRO PROCEDIMIENTO ALMACENADO QUE DEVUELVE LOS VALORES DE LAS PLACAS EN FORMA HORIZONTAL
            '******************************************************************************

            Dim sql As String

            If (My.Settings.Pais = "PE") Then
                sql = String.Format("spResumenUbicacionFlota '{0}','{1}'",
                           Me.IdUsuario,
                           Me.HoraFin
                           )
            Else
                sql = String.Format("SP_PDF_ResumenEjecutivo '{0}','{1}','{2}'",
                           Me.IdEntidad,
                           Fecha2ANSI(FechaInicial.ToString(), True),
                           Fecha2ANSI(FechaFinal.ToString(), True)
                           )
            End If


            DbDataAdapter = New SqlDataAdapter(sql, DbConnection)
            '******************************************************************************


            If My.Settings.Debug = True Then
                Console.WriteLine(DbDataAdapter.SelectCommand.CommandText)
            End If
            DbDataAdapter.SelectCommand.CommandTimeout = 3000000

            DbDataAdapter.Fill(dsCalidadManejo, "Kilometraje")
            Console.WriteLine("Registros Obtenidos para la Flota " & dsCalidadManejo.Tables(0).Rows.Count())
            dsCalidadManejo.AcceptChanges()

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20190711
            'SE COMENTO LINEA PARA QUE AL PRESENTARSE UN ERROR NO MANEJE EL ERROR Y NO ENVIE CORREO
            'TAMBIEN PARA COMENTAR LINEA DE CODIGO REPETIDA
            '******************************************************************************
            'Catch ex As Exception
            '    Console.Write(ex.Message)

            'End Try
            'dsCalidadManejo.AcceptChanges()
            '******************************************************************************

            For Each TmpRow As DataRow In dsCalidadManejo.Tables(0).Rows


                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180823
                'PARA NO CONSULTAR A LA BASE DE DATOS AL OBTENER EL NOMBRE DE EVENTO
                'PARA MOSTRAR LA VELOCIDAD SIN NINGUNA CONVERSION
                'PARA MOSTRAR EL KILOMETRAJE SIN NINGUNA CONVERSION
                'ESTO YA VIENE CON LA CONSULTA AL SP
                '******************************************************************************
                Try
                    'TmpRow("Vid") = String.Format("<a href='http://maps.google.com/maps?q={0},{1}'>{2}</a>", TmpRow("Lat").ToString(), TmpRow("Lon").ToString(), TmpRow("FechaHora").ToString())
                    TmpRow("Vid") = String.Format("http://maps.google.com/maps?q={0},{1}*{2}", TmpRow("Lat").ToString(), TmpRow("Lon").ToString(), TmpRow("FechaHora").ToString())

                Catch ex As Exception
                    TmpRow("Vid") = TmpRow("Vid")
                End Try
                '******************************************************************************




                TmpRow.AcceptChanges()
            Next
            dsCalidadManejo.AcceptChanges()


            Try
                dsCalidadManejo.Tables(0).Columns.Remove("Usuario")
            Catch ex As Exception

            End Try

            Try
                dsCalidadManejo.Tables(0).Columns.Remove("Lat")
            Catch ex As Exception

            End Try

            Try
                dsCalidadManejo.Tables(0).Columns.Remove("Lon")
            Catch ex As Exception

            End Try

            Try
                dsCalidadManejo.Tables(0).Columns.Remove("FechaHora")
            Catch ex As Exception

            End Try

            Try
                dsCalidadManejo.Tables(0).Columns.Remove("Cuenta")
            Catch ex As Exception

            End Try


            Try
                dsCalidadManejo.Tables(0).Columns.Remove("Ultima Lectura")
            Catch ex As Exception

            End Try

            Try
                dsCalidadManejo.Tables(0).Columns.Remove("Lectura")
            Catch ex As Exception

            End Try




            dsCalidadManejo.AcceptChanges()

            NCampos.Clear()
            hConsumo.Clear()
            'For Each dCol As DataColumn In dsCalidadManejo.Tables(0).Columns
            '    ConsumoPromedio = 0.0

            '    Try
            '        ConsumoPromedio = DBS25.getFactorConsumoPlaca(dCol.ColumnName)
            '    Catch ex As Exception
            '        ConsumoPromedio = 0.0
            '    End Try

            '    If dCol.ColumnName <> "FechaCompleta" And
            '            dCol.ColumnName <> "Año" And
            '            dCol.ColumnName <> "Mes" And
            '            dCol.ColumnName <> "Dia" And
            '            ConsumoPromedio > 0.0 Then

            '        If Not NCampos.Contains(dCol.ColumnName) Then
            '            NCampos.Add(dCol.ColumnName & "")
            '            hConsumo.Add(dCol.ColumnName & "", ConsumoPromedio)
            '        Else
            '            Console.ForegroundColor = ConsoleColor.Magenta
            '            Console.WriteLine("Placa duplicada: " & dCol.ColumnName)
            '        End If
            '    End If
            'Next
            'dsCalidadManejo.AcceptChanges()

            'If NCampos.Count > 0 Then
            '    For Each TmpRow As DataRow In dsCalidadManejo.Tables(0).Rows
            '        For Ind As Integer = 0 To NCampos.Count - 1
            '            Try
            '                TmpRow(NCampos(Ind)) &= " ; " & Math.Round(CDbl(TmpRow(NCampos(Ind).ToString())) / CDbl(hConsumo(NCampos(Ind))), 1) & " gls"
            '            Catch ex As Exception
            '                TmpRow(NCampos(Ind)) = "0.0"
            '            End Try
            '        Next
            '    Next
            'End If
            'dsCalidadManejo.AcceptChanges()

            TotalRegistros = dsCalidadManejo.Tables(0).Rows.Count

            If TotalRegistros < My.Settings.TotalRegistrosMaximo Then
                Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                For Each dColum As DataColumn In dsCalidadManejo.Tables(0).Columns
                    Celda = Fila.CreateCell(dColum.Ordinal)
                    Hoja.AutoSizeColumn(dColum.Ordinal)
                    Celda.CellStyle.Alignment = HorizontalAlignment.Center




                    Select Case dColum.ColumnName.ToUpper()
                        Case "USUARIO"

                            Celda.SetCellValue("USUARIO")
                        Case "ALIAS"
                            Celda.SetCellValue("PLACA")
                        Case "VID"
                            Celda.SetCellValue("FECHA HORA")
                        Case "IGNICION"
                            Celda.SetCellValue("IGNICION")
                        Case "NOMBREUBIGEO"
                            Celda.SetCellValue("NOMBRE UBIGEO")
                        Case "CODUBI"
                            Celda.SetCellValue("CODIGO UBIGEO")
                        Case "MARCA"
                            Celda.SetCellValue("MARCA")
                        Case "MODELO"
                            Celda.SetCellValue("MODELO")
                        Case Else
                            Celda.SetCellValue(dColum.ColumnName.ToUpper())
                    End Select

                    Celda.CellStyle = stlNada
                Next
                dsCalidadManejo.AcceptChanges()

                Console.ForegroundColor = ConsoleColor.DarkYellow
                Console.WriteLine("Generando Archivo de " & dsCalidadManejo.Tables(0).Rows.Count.ToString() & " Filas")
                For Each oRow As DataRow In dsCalidadManejo.Tables(0).Rows
                    Console.ForegroundColor = ConsoleColor.DarkGray
                    Console.WriteLine("Escribiendo Fila # " & Hoja.LastRowNum.ToString())
                    Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                    For Each dColum As DataColumn In dsCalidadManejo.Tables(0).Columns
                        Celda = Fila.CreateCell(dColum.Ordinal)
                        Hoja.AutoSizeColumn(dColum.Ordinal)
                        Celda.CellStyle.Alignment = HorizontalAlignment.Center






                        Select Case dColum.ColumnName.ToUpper()
                            Case "VID"

                                Dim words As String() = oRow(dColum.ColumnName).ToString().Split(New Char() {"*"c})
                                Dim url As String = words(0)
                                Dim fecha_hora As String = words(1)

                                If words.Count = 2 Then
                                    url = words(0)
                                    fecha_hora = words(1)
                                Else
                                    url = words(0)
                                    fecha_hora = "Ver Ubicacion"
                                End If


                                Dim creationHelper As ICreationHelper = wk.GetCreationHelper()
                                Dim link As IHyperlink = creationHelper.CreateHyperlink(HyperlinkType.Url)
                                link.Address = url

                                Celda.CellStyle = stlLink
                                Celda.Hyperlink = link

                                Celda.Hyperlink.Address = url
                                Celda.SetCellValue(fecha_hora)
                            Case Else
                                Celda.CellStyle = stlNormal
                                Celda.SetCellValue(oRow(dColum.ColumnName).ToString())
                        End Select




                    Next
                Next
                Console.WriteLine("Fin de Generacion de Archivo para Envio")
            Else
                Dim vnxls As New fGrid(dsCalidadManejo, Me.Archivo, CDbl(Parametros))
                vnxls.GenerarArchivo()
            End If
            dsCalidadManejo = Nothing

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20190711
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

                'For eInd As Integer = 0 To DatoEmail.Length - 1
                '    Console.WriteLine(DatoEmail(eInd))
                '    Try
                '        EnviarMail(DatoEmail(eInd),
                '               Archivo,
                '               "Adjunto Email con el Reporte Solicitado",
                '                Nothing,
                '                Reporte,
                '                "ALR",
                '                Datos.IdUsuario)
                '    Catch ex As Exception
                '        ErroresEnvio &= "ERR_MAIL"
                '    End Try
                'Next



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
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20190711
            'PARA LLAMAR AL PROCESO GUARDARLOG UNA VEZ SE HAYA GENERADO UN ERROR
            '******************************************************************************
            GuardarLog(Tipo, ex.Message)
            '******************************************************************************
        End Try
    End Function
End Class
