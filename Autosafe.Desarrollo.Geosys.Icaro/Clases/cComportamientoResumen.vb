Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports Autosafe.Desarrollo.Geosys.Entidades
Imports Autosafe.Desarrollo.Geosys.Negocios

Public Class cComportamientoResumen : Inherits cGeneracion
    Private dsComportamientoResumen As DataSet
    Private Const Reporte As String = "Reporte Evolucion Mensual de la Flota"
    Private Hoja As ISheet

    Private TotalBueno As Integer = 0
    Private TotalRegular As Integer = 0
    Private TotalMalo As Integer = 0
    Private TotalUnidad As Double = 0.0
    Private PorcUnidad As Double = 0.0

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
        Console.WriteLine(Datos.usuarioId.ToString() & ": Obteniendo Datos para el Reporte Comportamiento de la Flota")

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
                GenerarReporte(Datos.usuarioId,
                              Datos.intervalo,
                              Datos.parametros)

                If wk.NumberOfSheets > 0 Then
                    Dim file As FileStream = New FileStream(Me.Archivo, FileMode.Create)
                    wk.Write(file)
                    file.Close()
                    file.Dispose()
                    file = Nothing
                    wk.Close()
                    wk = Nothing

                    Try
                        EnviarMail(Datos.email,
                           Archivo,
                           "Adjunto Email con el Reporte Solicitado",
                            Hoja,
                            Reporte,
                            "ROM",
                            Datos.usuarioId)
                    Catch ex As Exception
                        ErroresEnvio &= "ERR_MAIL"
                    End Try
                End If
            Else
                GuardarLog(Me.Tipo, "SINVEHICULOS")
                Console.WriteLine(String.Format("El usuario :  {0} no cuenta con vehiculos activos", Me.Usuario))

            End If



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
            Dim stlBueno As ICellStyle = wk.CreateCellStyle()
            Dim stlRegular As ICellStyle = wk.CreateCellStyle()
            Dim stlMalo As ICellStyle = wk.CreateCellStyle()

            With Stl
                .FillForegroundColor = NPOI.HSSF.Util.HSSFColor.SeaGreen.Index
                .FillPattern = FillPattern.SolidForeground
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
            End With

            With stlBueno
                .FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Green.Index
                .FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.White.Index
                .FillPattern = FillPattern.SolidForeground
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
            End With

            With stlRegular
                .FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightYellow.Index
                .FillPattern = FillPattern.SolidForeground
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
            End With

            With stlMalo
                .FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index
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

            Dim Archivo As String = "CommportamientoFlota.xls"
            dsComportamientoResumen = Nothing
            dsComportamientoResumen = New DataSet()

            DbConnection = Nothing
            DbConnection = New SqlConnection(My.Settings.GEOSYSConnectionStringAux)

            If DbConnection.State = ConnectionState.Open Then
                DbConnection.Close()
            End If
            DbConnection.Open()
            DbDataAdapter = Nothing

            Console.ForegroundColor = ConsoleColor.Magenta
            Console.WriteLine("Obteniendo Resumen Evolucion Mensual de la Flota ")

            DbDataAdapter = New SqlDataAdapter("exec spReporteProgramadoResumen_Listar 'COM'," & IdUsuario.ToString() & "," & Now.Year.ToString() & "," & (Now.Month).ToString(), DbConnection)
            DbDataAdapter.SelectCommand.CommandTimeout = 20000000
            DbDataAdapter.Fill(dsComportamientoResumen, "ResumenComportamientoFlota")
            dsComportamientoResumen.AcceptChanges()

            Try
                dsComportamientoResumen.Tables(0).Columns.Remove("TipoReporte")
            Catch ex As Exception

            End Try
            dsComportamientoResumen.AcceptChanges()

            Hoja = Nothing
            Hoja = wk.CreateSheet("Evolucion Flota")
            With Hoja
                .Header.Center = "Reporte Evolucion Mensual de la Flota"
                .CreateRow(0).CreateCell(0).SetCellValue(Reporte)
                .AddMergedRegion(New NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7))
            End With

            Fila = Hoja.CreateRow(1)
            Fila.CreateCell(0).SetCellValue("Año: " & Now.Year.ToString())
            Fila = Hoja.CreateRow(2)
            Fila.CreateCell(0).SetCellValue("Mes: " & Now.Month)
            Hoja.CreateRow(3)
            Fila = Hoja.CreateRow(4)

            For Each dColum As DataColumn In dsComportamientoResumen.Tables(0).Columns
                Celda = Fila.CreateCell(dColum.Ordinal)
                Celda.CellStyle.Alignment = HorizontalAlignment.Center
                Celda.SetCellValue(dColum.ColumnName.ToUpper())
                Celda.CellStyle = stlNada
            Next

            For Each dColum As DataColumn In dsComportamientoResumen.Tables(0).Columns
                Hoja.AutoSizeColumn(dColum.Ordinal)
            Next

            Dim ValorCelda As String = String.Empty

            TotalBueno = 0
            TotalRegular = 0
            TotalMalo = 0

            Dim vnCharResumen As fChartResumen

            'vnCharResumen = New fChartResumen(DBS25.getUsuarioxID(IdUsuario.ToString()), "Resumen de Evolucion Mensual de la Flota")
            vnCharResumen = New fChartResumen(Me.Usuario, "Resumen de Evolucion Mensual de la Flota")
            For Each oRow As DataRow In dsComportamientoResumen.Tables(0).Rows
                Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
                PorcUnidad = 0.0
                TotalUnidad = 0.0

                If IsDBNull(oRow("# Buenos")) Then
                    oRow("# Buenos") = 0
                End If

                If IsDBNull(oRow("# Regulares")) Then
                    oRow("# Regulares") = 0
                End If

                If IsDBNull(oRow("# Malos")) Then
                    oRow("# Malos") = 0
                End If

                TotalUnidad = CInt(oRow("# Buenos")) + CInt(oRow("# Regulares")) + CInt(oRow("# Malos"))
                vnCharResumen.AddSerie(oRow("Mes"), oRow("# Buenos"), oRow("# Regulares"), oRow("# Malos"))

                Try
                    TotalBueno += oRow("# Buenos")
                Catch ex As Exception
                    TotalBueno += 0
                End Try

                Try
                    TotalRegular += oRow("# Regulaes")
                Catch ex As Exception
                    TotalRegular += 0
                End Try

                Try
                    TotalMalo += oRow("# Malos")
                Catch ex As Exception
                    TotalMalo += 0
                End Try

                Try
                    oRow("PorcBuenos") = Round((oRow("# Buenos") / TotalUnidad) * 100.0, 2)
                Catch ex As Exception
                    oRow("PorcBuenos") = 0.0
                End Try

                Try
                    oRow("PorcRegulares") = Round((oRow("# Regulares") / TotalUnidad) * 100.0, 2)
                Catch ex As Exception
                    oRow("PorcRegulares") = 0.0
                End Try

                Try
                    oRow("PorcMalos") = Round((oRow("# Malos") / TotalUnidad) * 100.0, 2)
                Catch ex As Exception
                    oRow("PorcMalos") = 0.0
                End Try
                oRow.AcceptChanges()

                For Each dColum As DataColumn In dsComportamientoResumen.Tables(0).Columns
                    Celda = Fila.CreateCell(dColum.Ordinal)
                    Hoja.AutoSizeColumn(dColum.Ordinal)

                    If dColum.ColumnName Like "Porc*" Then
                        Celda.SetCellValue(oRow(dColum.ColumnName).ToString() & " %")
                    Else
                        Celda.SetCellValue(oRow(dColum.ColumnName).ToString())
                    End If
                Next
            Next
            dsComportamientoResumen.AcceptChanges()

            Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)
            Fila = Hoja.CreateRow(Hoja.LastRowNum + 1)

            vnCharResumen.Graficar()

            Dim patriarch As HSSFPatriarch = DirectCast(Hoja.CreateDrawingPatriarch(), HSSFPatriarch)
            'Grafico 1
            Dim anchor As HSSFClientAnchor
            'anchor = New HSSFClientAnchor(0, 0, 0, 255, 10, 1, 13, 14)
            anchor = New HSSFClientAnchor(0, 0, 0, 255, 0, 10, 13, 16)
            anchor.AnchorType = 2

            Dim picture As HSSFPicture = DirectCast(patriarch.CreatePicture(anchor,
                                                                            CargarImagenXLS(Me.Usuario & "_Resumen.jpg", wk)), HSSFPicture)
            picture.Resize()
            picture.LineStyle = HSSFPicture.LINESTYLE_DASHDOTGEL
            picture = Nothing
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
