Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.Packaging
Imports System.Net.Mail
Imports NPOI.HPSF
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel
Imports System.Math
Imports System.Text.RegularExpressions
Imports Microsoft.SqlServer.Types
Imports Autosafe.Desarrollo.Geosys.Negocios
Imports Autosafe.Desarrollo.Geosys.Entidades
Imports DevExpress.PivotGrid.Internal.ThinClientDataSource

Public MustInherit Class cGeneracion
    Public DbConnection As SqlConnection
    Public DbDataAdapter As SqlDataAdapter
    Public DbCommand As SqlCommand
    Public Property ColumnasReporte As String()
    Public Property DEvento As String = ""
    Public Property TotalRegistros As Integer = 0
    Public Property Archivo As String = ""
    Public Property ArchivoZIP As String = ""
    Public Property Contador As Integer = 1
    Public Property hsVehiculos As New Hashtable()
    Public Property LastEvento As Integer = -1
    Public Property LastDEvento As String = ""
    Public Property FechaInicial As Date = Now()
    Public Property FechaFinal As Date = Now()

    Public Property cFinalBueno As Integer = 0
    Public Property cFinalRegular As Integer = 0
    Public Property cFinalMalo As Integer = 0
    Public Const MaxFilasArchivo As Integer = 65000
    Public Property NumHojas As Integer = 0
    Public Property NumFila As Integer = 0

    Protected wk As HSSFWorkbook = New HSSFWorkbook()
    Protected dsi As DocumentSummaryInformation = PropertySetFactory.CreateDocumentSummaryInformation()
    Protected dsu As SummaryInformation = PropertySetFactory.CreateSummaryInformation()
    Protected pal As HSSFPalette = wk.GetCustomPalette()
    Protected DBS As ThorDataContext = New ThorDataContext()

    Protected DBS25 As ThorDataContext25 = New ThorDataContext25()

    Protected stlTitulo As ICellStyle = wk.CreateCellStyle()
    Protected Fuente As IFont = wk.CreateFont()

    Public Property ErroresEnvio As String

    Public CadenaConexion As String = My.Settings.GEOSYSConnectionStringMain

    Protected IdUsuario As Integer
    Protected Secuencia As Integer
    Protected Usuario As String
    Protected Email As String
    Protected Tipo As String
    Protected Parametros As String
    Protected HoraEnvio As String
    Protected Intervalo As String
    Protected Activos As String
    Protected Velocidad As String
    Protected Guardar As Boolean

    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
    'PARA AGREGAR EL NOMBRE DE USUARIO AL REPORTE
    'PARA AGREGAR FECHAHORAINICIAL AL REPORTE
    'PARA AGREGAR FECHAHORAFINAL AL REPORTE
    'PARA AGREGAR HORAINICIO AL REPORTE
    'PARA AGREGAR HORAFIN AL REPORTE
    'PARA AGREGAR IDENTIDAD AL REPORTE
    '******************************************************************************
    Protected Nombre As String
    Public Property FechaHoraInicial As DateTime = Now()
    Public Property FechaHoraFinal As DateTime = Now()
    Protected HoraInicio As String
    Protected HoraFin As String
    Protected IdEntidad As String
    Protected Evento As String
    '******************************************************************************

    Protected DatoTipo As String()
    Protected DatoActivos As String()
    Protected DatoEmail As String()

    Public MustOverride Sub Generar(ByVal Datos As Object)
    Public Const EarthRadius As Double = 6371

    Protected Columnas As String = ""
    Protected Fila As IRow
    Protected Celda As ICell
    Protected Stl As ICellStyle = wk.CreateCellStyle()
    Protected stlNada As ICellStyle = wk.CreateCellStyle()
    Protected stlNormal As ICellStyle = wk.CreateCellStyle()

    Protected stlLink As ICellStyle = wk.CreateCellStyle()
    Protected fontLink As IFont = wk.CreateFont()


    Protected TituloIntervalo As String = ""
    Protected EntidadUsuario As String = ""

    Public Structure GeoCoordinate
        Dim Latitude As Double
        Dim Longitude As Double
    End Structure

    Public Structure sCalificacionIndividual
        Dim TotalBueno As Integer
        Dim TotalRegular As Integer
        Dim TotalMalo As Integer
    End Structure

    Public Property CalificacionIndividual As sCalificacionIndividual

    Public Property hsCalificacionIndividual As New Hashtable()

    Public Property oZip As Package
    Public Property FileSize As Double = 0

    Sub New()
        MyBase.New()
    End Sub



    Public Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String, Optional ByVal Guardar As Boolean = False)
        Try

            dsi.Company = "Carseg"

            wk.DocumentSummaryInformation = dsi
            wk.SummaryInformation = dsu

            Me.Tipo = TipoReporte
            Me.IdUsuario = IdUsuario
            Me.Intervalo = Intervalo
            Me.Parametros = Parametros
            Me.Guardar = Guardar


            Dim negocioUsuario As New UsuarioBL()
            Dim objUsuario As New UsuarioEN

            objUsuario.baseDatosId = 25
            objUsuario.usuarioId = IdUsuario

            objUsuario = negocioUsuario.GetUsuarioPorId(objUsuario)

            'Archivo = String.Format("{0}_{1}.xls", TipoReporte, DBS25.getUsuarioxID(IdUsuario))
            Archivo = String.Format("{0}_{1}.xls", TipoReporte, objUsuario.usuario)




            Try
                File.Delete(Archivo)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Email = ""
            HoraEnvio = ""
            Activos = ""

            DatoActivos = Nothing
            DatoTipo = Nothing
            DatoEmail = Nothing

            ColumnasReporte = Nothing
            ColumnasReporte = My.Settings.ColumnasReporte.Split(";")

            wk.Clear()

            dsi.Company = My.Settings.Company
            wk.DocumentSummaryInformation = dsi

            dsu.Subject = "Reportes Programados GeoSyS"
            wk.SummaryInformation = dsu

            With Fuente
                .IsBold = True
                .FontName = "Verdana"
                .FontHeight = 12
            End With

            With stlTitulo
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
                .SetFont(Fuente)
            End With

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

            With fontLink
                .Underline = FontUnderlineType.Single
                .Color = FontColor.Red
            End With

            With stlLink
                .SetFont(fontLink)
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


            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181102
            'PARA PROCESAR LA FECHAINICIO Y FECHAFIN DEL REPORTE DE ACUERDO AL INTERVALO
            'PARA GUARDAR UN REGISTRO LOG EN LA TABLA REPORTEPROGRAMADOPENDIENTE
            '******************************************************************************
            Call procesarIntervalo(Me.Intervalo)

            '******************************************************************************

        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181002
    'PARA AGREGAR NOMBRE AL REPORTE
    'PARA AGREGAR EL NUMERO DE SECUENCIA AL REPORTE
    '******************************************************************************

    Public Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String _
                   , ByVal Nombre As String _
                   , ByVal Secuencia As String _
                   , Optional ByVal Guardar As Boolean = False)

        Try

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR NOMBRE COMPANIA
            '******************************************************************************
            'dsi.Company = "Carseg"
            dsi.Company = My.Settings.Company
            '******************************************************************************


            wk.DocumentSummaryInformation = dsi
            wk.SummaryInformation = dsu

            Me.Tipo = TipoReporte
            Me.IdUsuario = IdUsuario
            Me.Intervalo = Intervalo
            Me.Parametros = Parametros
            Me.Guardar = Guardar


            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR EL NOMBRE DE USUARIO AL REPORTE
            'PARA GUARDAR EL NUMERO DE SECUENCIA DEL REPORTE
            'PARA GUARDAR FECHAINICIO DEL REPORTE
            'PARA GUARDAR FECHAFIN DEL REPORTE
            '******************************************************************************
            Me.Nombre = Nombre
            Me.Secuencia = Secuencia
            Me.HoraInicio = HoraInicio
            Me.HoraFin = HoraFin
            '******************************************************************************


            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181113
            'PARA MODIFICAR EL NOMBRE DEL ARCHIVO 
            'PARA BORRAR EL ARCHIVO SI EXISTIERA
            '******************************************************************************
            'Archivo = String.Format("{0}_{1}.xls", TipoReporte, DBS.getUsuarioxID(IdUsuario))

            'Try
            '    File.Delete(Archivo)
            'Catch ex As Exception
            '    Console.WriteLine(ex.Message)
            'End Try

            Email = ""
            HoraEnvio = ""
            Activos = ""

            DatoActivos = Nothing
            DatoTipo = Nothing
            DatoEmail = Nothing

            ColumnasReporte = Nothing
            ColumnasReporte = My.Settings.ColumnasReporte.Split(";")

            wk.Clear()

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR NOMBRE COMPANIA
            '******************************************************************************
            'dsi.Company = "Carseg"
            dsi.Company = My.Settings.Company
            '******************************************************************************

            wk.DocumentSummaryInformation = dsi

            dsu.Subject = "Reportes Programados GeoSyS"
            wk.SummaryInformation = dsu

            With Fuente
                .IsBold = True
                .FontName = "Verdana"
                .FontHeight = 12
            End With

            With stlTitulo
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
                .SetFont(Fuente)
            End With

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




            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181102
            'PARA PROCESAR LA FECHAINICIO Y FECHAFIN DEL REPORTE DE ACUERDO AL INTERVALO
            'PARA GUARDAR UN REGISTRO LOG EN LA TABLA REPORTEPROGRAMADOPENDIENTE
            '******************************************************************************
            Call procesarIntervalo(Me.Intervalo)

            '******************************************************************************

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181113
            'PARA GUARDAR EL NOMBRE DE ARCHIVO DE LA SIGUIENTE MANERA
            'TIPOREPORTE_USUARIO_SECUENCIA_FECHAINICIAL_FECHAFINAL.XLS
            'PARA BORRAR EN CASO EXISTIERA
            '******************************************************************************

            Dim negocioUsuario As New UsuarioBL()
            Dim objUsuario As New UsuarioEN

            objUsuario.baseDatosId = 25
            objUsuario.usuarioId = IdUsuario

            objUsuario = negocioUsuario.GetUsuarioPorId(objUsuario)

            'Archivo = String.Format("{0}_{1}_{2}_{3}_{4}.xls", TipoReporte, DBS25.getUsuarioxID(IdUsuario), Me.Secuencia, Fecha2ANSI(FechaInicial.ToString(), True), Fecha2ANSI(FechaFinal.ToString(), True))
            Archivo = String.Format("{0}_{1}_{2}_{3}_{4}.xls", TipoReporte, objUsuario.usuario, Me.Secuencia, Fecha2ANSI(FechaInicial.ToString(), True), Fecha2ANSI(FechaFinal.ToString(), True))

            Try
                File.Delete(Archivo)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            '******************************************************************************




        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub


    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181002
    'PARA AGREGAR NOMBRE AL REPORTE
    'PARA AGREGAR EL NUMERO DE SECUENCIA AL REPORTE
    'PARA AGREGAR HORADEINICIO AL REPORTE
    'PARA AGREGAR HORADEFIN AL REPORTE
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 202100331
    'PARA AGREGAR USUARIO AL REPORTE
    '******************************************************************************

    Public Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String _
                   , ByVal Nombre As String _
                   , ByVal Secuencia As String _
                   , ByVal HoraInicio As String _
                   , ByVal HoraFin As String _
                   , ByVal Usuario As String _
                   , Optional ByVal Guardar As Boolean = False)

        Try

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR NOMBRE COMPANIA
            '******************************************************************************
            'dsi.Company = "Carseg"
            dsi.Company = My.Settings.Company
            '******************************************************************************


            wk.DocumentSummaryInformation = dsi
            wk.SummaryInformation = dsu

            Me.Tipo = TipoReporte
            Me.IdUsuario = IdUsuario
            Me.Intervalo = Intervalo
            Me.Parametros = Parametros
            Me.Guardar = Guardar


            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR EL NOMBRE DE USUARIO AL REPORTE
            'PARA GUARDAR EL NUMERO DE SECUENCIA DEL REPORTE
            'PARA GUARDAR FECHAINICIO DEL REPORTE
            'PARA GUARDAR FECHAFIN DEL REPORTE
            '******************************************************************************
            Me.Nombre = Nombre
            Me.Secuencia = Secuencia
            Me.HoraInicio = HoraInicio
            Me.HoraFin = HoraFin
            '******************************************************************************


            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181113
            'PARA MODIFICAR EL NOMBRE DEL ARCHIVO 
            'PARA BORRAR EL ARCHIVO SI EXISTIERA
            '******************************************************************************
            'Archivo = String.Format("{0}_{1}.xls", TipoReporte, DBS.getUsuarioxID(IdUsuario))

            'Try
            '    File.Delete(Archivo)
            'Catch ex As Exception
            '    Console.WriteLine(ex.Message)
            'End Try

            Email = ""
            HoraEnvio = ""
            Activos = ""

            DatoActivos = Nothing
            DatoTipo = Nothing
            DatoEmail = Nothing

            ColumnasReporte = Nothing
            ColumnasReporte = My.Settings.ColumnasReporte.Split(";")

            wk.Clear()

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR NOMBRE COMPANIA
            '******************************************************************************
            'dsi.Company = "Carseg"
            dsi.Company = My.Settings.Company
            '******************************************************************************

            wk.DocumentSummaryInformation = dsi

            dsu.Subject = "Reportes Programados GeoSyS"
            wk.SummaryInformation = dsu

            With Fuente
                .IsBold = True
                .FontName = "Verdana"
                .FontHeight = 12
            End With

            With stlTitulo
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
                .SetFont(Fuente)
            End With

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


            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181113
            'PARA GUARDAR EL NOMBRE DE ARCHIVO DE LA SIGUIENTE MANERA
            'TIPOREPORTE_USUARIO_SECUENCIA_FECHAINICIAL_FECHAFINAL.XLS
            'PARA BORRAR EN CASO EXISTIERA
            '******************************************************************************
            'Dim negocioUsuario As New UsuarioBL()
            'Dim objUsuario As New UsuarioEN

            'objUsuario.baseDatosId = 25
            'objUsuario.usuarioId = IdUsuario

            'objUsuario = negocioUsuario.GetUsuarioPorId(objUsuario)


            'Archivo = String.Format("{0}_{1}_{2}_{3}_{4}.xls", TipoReporte, DBS25.getUsuarioxID(IdUsuario), Me.Secuencia, Fecha2ANSI(FechaInicial.ToString(), True), Fecha2ANSI(FechaFinal.ToString(), True))
            'Archivo = String.Format("{0}_{1}_{2}_{3}_{4}.xls", TipoReporte, objUsuario.usuario, Me.Secuencia, Fecha2ANSI(FechaInicial.ToString(), True), Fecha2ANSI(FechaFinal.ToString(), True))

            'Dim fechaParametro As String
            'fechaParametro = Me.Parametros.Replace("/", "")
            'fechaParametro = fechaParametro.Replace(";", "_")

            Me.Usuario = Usuario

            'Archivo = String.Format("{0}_{1}_{2}_{3}.xls", TipoReporte, objUsuario.usuario, Me.Secuencia, fechaParametro)
            Archivo = String.Format("{0}_{1}_{2}_{3}_{4}.xls", TipoReporte, Me.Usuario, Me.Secuencia, Fecha2ANSI(FechaInicial.ToString(), True), Fecha2ANSI(FechaFinal.ToString(), True))



            Try
                File.Delete(Archivo)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            '******************************************************************************




            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181226
            'PARA PROCESAR LA FECHAINICIO Y FECHAFIN DEL REPORTE DE ACUERDO AL INTERVALO
            'PARA GUARDAR UN REGISTRO LOG EN LA TABLA REPORTEPROGRAMADOPENDIENTE
            'PARA RESOLVER BUG DE LLAMADA DOBLE AL MISMO PROCEDIMIENTO procesarIntervalo
            '******************************************************************************
            Call procesarIntervalo(Me.Intervalo)

            '******************************************************************************






        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181002
    'PARA AGREGAR NOMBRE AL REPORTE
    'PARA AGREGAR EL NUMERO DE SECUENCIA AL REPORTE
    'PARA AGREGAR HORADEINICIO AL REPORTE
    'PARA AGREGAR HORADEFIN AL REPORTE
    'PARA AGREGAR IDENTIDAD AL REPORTE
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 202100331
    'PARA AGREGAR USUARIO AL REPORTE
    '******************************************************************************

    Public Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String _
                   , ByVal Nombre As String _
                   , ByVal Secuencia As String _
                   , ByVal HoraInicio As String _
                   , ByVal HoraFin As String _
                   , ByVal Usuario As String _
                   , ByVal IdEntidad As String _
                   , Optional ByVal Guardar As Boolean = False)

        Try

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR NOMBRE COMPANIA
            '******************************************************************************
            'dsi.Company = "Carseg"
            dsi.Company = My.Settings.Company
            '******************************************************************************


            wk.DocumentSummaryInformation = dsi
            wk.SummaryInformation = dsu

            Me.Tipo = TipoReporte
            Me.IdUsuario = IdUsuario
            Me.Intervalo = Intervalo
            Me.Parametros = Parametros
            Me.Guardar = Guardar
            'Me.IdEntidad = IdEntidad


            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR EL NOMBRE DE USUARIO AL REPORTE
            'PARA GUARDAR EL NUMERO DE SECUENCIA DEL REPORTE
            'PARA GUARDAR FECHAINICIO DEL REPORTE
            'PARA GUARDAR FECHAFIN DEL REPORTE
            'PARA GUARDAR IDENTIDAD DEL REPORTE
            'PARA GUARDAR IDUSUARIO DEL REPORTE
            '******************************************************************************
            Me.Nombre = Nombre
            Me.Secuencia = Secuencia
            Me.HoraInicio = HoraInicio
            Me.HoraFin = HoraFin
            Me.IdEntidad = IdEntidad
            Me.IdUsuario = IdUsuario
            '******************************************************************************


            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181113
            'PARA MODIFICAR EL NOMBRE DEL ARCHIVO 
            'PARA BORRAR EL ARCHIVO SI EXISTIERA
            '******************************************************************************
            'Archivo = String.Format("{0}_{1}.xls", TipoReporte, DBS.getUsuarioxID(IdUsuario))

            'Try
            '    File.Delete(Archivo)
            'Catch ex As Exception
            '    Console.WriteLine(ex.Message)
            'End Try

            Email = ""
            HoraEnvio = ""
            Activos = ""

            DatoActivos = Nothing
            DatoTipo = Nothing
            DatoEmail = Nothing

            ColumnasReporte = Nothing
            ColumnasReporte = My.Settings.ColumnasReporte.Split(";")

            wk.Clear()

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR NOMBRE COMPANIA
            '******************************************************************************
            'dsi.Company = "Carseg"
            dsi.Company = My.Settings.Company
            '******************************************************************************

            wk.DocumentSummaryInformation = dsi

            dsu.Subject = "Reportes Programados GeoSyS"
            wk.SummaryInformation = dsu

            With Fuente
                .IsBold = True
                .FontName = "Verdana"
                .FontHeight = 12
            End With

            With stlTitulo
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
                .SetFont(Fuente)
            End With

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




            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181102
            'PARA PROCESAR LA FECHAINICIO Y FECHAFIN DEL REPORTE DE ACUERDO AL INTERVALO
            'PARA GUARDAR UN REGISTRO LOG EN LA TABLA REPORTEPROGRAMADOPENDIENTE
            '******************************************************************************
            Call procesarIntervalo(Me.Intervalo)

            '******************************************************************************

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181113
            'PARA GUARDAR EL NOMBRE DE ARCHIVO DE LA SIGUIENTE MANERA
            'TIPOREPORTE_USUARIO_SECUENCIA_FECHAINICIAL_FECHAFINAL.XLS
            'PARA BORRAR EN CASO EXISTIERA
            '******************************************************************************

            'Dim negocioUsuario As New UsuarioBL()
            'Dim objUsuario As New UsuarioEN

            'objUsuario.baseDatosId = 25
            'objUsuario.usuarioId = IdUsuario

            'objUsuario = negocioUsuario.GetUsuarioPorId(objUsuario)

            Me.Usuario = Usuario


            'Archivo = String.Format("{0}_{1}_{2}_{3}_{4}.xls", TipoReporte, DBS25.getUsuarioxID(IdUsuario), Me.Secuencia, Fecha2ANSI(FechaInicial.ToString(), True), Fecha2ANSI(FechaFinal.ToString(), True))
            Archivo = String.Format("{0}_{1}_{2}_{3}_{4}.xls", TipoReporte, Me.IdUsuario, Me.Secuencia, Fecha2ANSI(FechaInicial.ToString(), True), Fecha2ANSI(FechaFinal.ToString(), True))

            Try
                File.Delete(Archivo)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            '******************************************************************************


            Me.IdEntidad = IdEntidad

        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub


    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181002
    'PARA AGREGAR NOMBRE AL REPORTE
    'PARA AGREGAR EL NUMERO DE SECUENCIA AL REPORTE
    'PARA AGREGAR HORADEINICIO AL REPORTE
    'PARA AGREGAR HORADEFIN AL REPORTE
    'PARA AGREGAR IDENTIDAD AL REPORTE
    'PARA AGREGAR EVENTO AL REPORTE
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 202100331
    'PARA AGREGAR USUARIO AL REPORTE
    '******************************************************************************

    Public Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String _
                   , ByVal Nombre As String _
                   , ByVal Secuencia As String _
                   , ByVal HoraInicio As String _
                   , ByVal HoraFin As String _
                   , ByVal Usuario As String _
                   , ByVal IdEntidad As String _
                   , ByVal Evento As String _
                   , Optional ByVal Guardar As Boolean = False)

        Try

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR NOMBRE COMPANIA
            '******************************************************************************
            'dsi.Company = "Carseg"
            dsi.Company = My.Settings.Company
            '******************************************************************************


            wk.DocumentSummaryInformation = dsi
            wk.SummaryInformation = dsu

            Me.Tipo = TipoReporte
            Me.IdUsuario = IdUsuario
            Me.Intervalo = Intervalo
            Me.Parametros = Parametros
            Me.Guardar = Guardar
            Me.IdEntidad = IdEntidad



            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR EL NOMBRE DE USUARIO AL REPORTE
            'PARA GUARDAR EL NUMERO DE SECUENCIA DEL REPORTE
            'PARA GUARDAR FECHAINICIO DEL REPORTE
            'PARA GUARDAR FECHAFIN DEL REPORTE
            'PARA GUARDAR IDENTIDAD DEL REPORTE
            'PARA GUARDAR IDUSUARIO DEL REPORTE
            '******************************************************************************
            Me.Nombre = Nombre
            Me.Secuencia = Secuencia
            Me.HoraInicio = HoraInicio
            Me.HoraFin = HoraFin
            Me.IdEntidad = IdEntidad
            Me.IdUsuario = IdUsuario
            Me.Evento = Evento
            '******************************************************************************


            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181113
            'PARA MODIFICAR EL NOMBRE DEL ARCHIVO 
            'PARA BORRAR EL ARCHIVO SI EXISTIERA
            '******************************************************************************
            'Archivo = String.Format("{0}_{1}.xls", TipoReporte, DBS.getUsuarioxID(IdUsuario))

            'Try
            '    File.Delete(Archivo)
            'Catch ex As Exception
            '    Console.WriteLine(ex.Message)
            'End Try

            Email = ""
            HoraEnvio = ""
            Activos = ""

            DatoActivos = Nothing
            DatoTipo = Nothing
            DatoEmail = Nothing

            ColumnasReporte = Nothing
            ColumnasReporte = My.Settings.ColumnasReporte.Split(";")

            wk.Clear()

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA AGREGAR NOMBRE COMPANIA
            '******************************************************************************
            'dsi.Company = "Carseg"
            dsi.Company = My.Settings.Company
            '******************************************************************************

            wk.DocumentSummaryInformation = dsi

            dsu.Subject = "Reportes Programados GeoSyS"
            wk.SummaryInformation = dsu

            With Fuente
                .IsBold = True
                .FontName = "Verdana"
                .FontHeight = 12
            End With

            With stlTitulo
                .Alignment = HorizontalAlignment.Center
                .BorderBottom = BorderStyle.Thin
                .BorderRight = BorderStyle.Thin
                .BorderTop = BorderStyle.Thin
                .BorderLeft = BorderStyle.Thin
                .SetFont(Fuente)
            End With

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




            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181102
            'PARA PROCESAR LA FECHAINICIO Y FECHAFIN DEL REPORTE DE ACUERDO AL INTERVALO
            'PARA GUARDAR UN REGISTRO LOG EN LA TABLA REPORTEPROGRAMADOPENDIENTE
            '******************************************************************************
            Call procesarIntervalo(Me.Intervalo)

            '******************************************************************************

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181113
            'PARA GUARDAR EL NOMBRE DE ARCHIVO DE LA SIGUIENTE MANERA
            'TIPOREPORTE_USUARIO_SECUENCIA_FECHAINICIAL_FECHAFINAL.XLS
            'PARA BORRAR EN CASO EXISTIERA
            '******************************************************************************

            'Dim negocioUsuario As New UsuarioBL()
            'Dim objUsuario As New UsuarioEN

            'objUsuario.baseDatosId = 25
            'objUsuario.usuarioId = IdUsuario

            'objUsuario = negocioUsuario.GetUsuarioPorId(objUsuario)

            Me.Usuario = Usuario


            'Archivo = String.Format("{0}_{1}_{2}_{3}_{4}.xls", TipoReporte, DBS25.getUsuarioxID(IdUsuario), Me.Secuencia, Fecha2ANSI(FechaInicial.ToString(), True), Fecha2ANSI(FechaFinal.ToString(), True))
            Archivo = String.Format("{0}_{1}_{2}_{3}_{4}.xls", TipoReporte, Me.IdUsuario, Me.Secuencia, Fecha2ANSI(FechaInicial.ToString(), True), Fecha2ANSI(FechaFinal.ToString(), True))

            Try
                File.Delete(Archivo)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            '******************************************************************************




        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Public Sub procesarIntervalo(ByVal Intervalo As String)

        Me.TituloIntervalo = ""

        Select Case Intervalo
            Case "MC"
                Me.FechaInicial = FechaFinal.AddDays(-1 * Now.Date.Day)
                Me.FechaFinal = Now()
                Me.TituloIntervalo = "Mes actual"
            Case "MA"
                Me.FechaInicial = FechaInicial.AddMonths(-1)
                Me.FechaFinal = FechaFinal.AddDays(-1)
                Me.TituloIntervalo = "Mes anterior"
            Case "DA"
                Me.FechaInicial = FechaInicial.AddDays(-1)
                Me.FechaFinal = FechaFinal.AddDays(-1)
                Me.TituloIntervalo = "Dia anterior"
            Case "2DA"
                Me.FechaInicial = FechaInicial.AddDays(-2)
                Me.FechaFinal = FechaFinal.AddDays(-2)
                Me.TituloIntervalo = "2 Dias anteriores"
            Case "3DA"
                Me.FechaInicial = FechaInicial.AddDays(-3)
                Me.FechaFinal = FechaFinal.AddDays(-3)
                Me.TituloIntervalo = "3 Dias anteriores"
            Case "ES"
                Me.FechaInicial = FechaInicial.AddDays(-7)
                Me.FechaFinal = FechaFinal.AddDays(0)
                Me.TituloIntervalo = "Semana 7 Dias Incluyendo el Actual"
            Case "SA"
                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181102
                'PARA SETEAR
                'FECHAINICIAL = AL PRIMER DIA DE LA SEMANA ANTERIOR
                'FECHAFINAL = AL ULTIMO DIA DE LA SEMANA ANTERIOR
                '******************************************************************************
                'Me.FechaInicial = FechaInicial.AddDays(-7)
                'Me.FechaFinal = FechaFinal.AddDays(-1)
                'Me.TituloIntervalo = "Semanal"
                Me.FechaInicial = DateTime.Now.AddDays(-DateTime.Now.DayOfWeek - 6)
                Me.FechaFinal = DateTime.Now.AddDays(-DateTime.Now.DayOfWeek)
                Me.TituloIntervalo = "Semanal desde el primer dia de la semana"
                '******************************************************************************
            Case "DS"
                Me.FechaInicial = FechaInicial.AddDays(-15)
                Me.FechaFinal = FechaFinal.AddDays(-1)
                Me.TituloIntervalo = "Quincenal"
            Case "CUS"
                Me.FechaInicial = Parametros.Split(";")(0)
                Me.FechaFinal = Parametros.Split(";")(1)
                Me.TituloIntervalo = "Custom"
            Case "24H"
                Me.FechaInicial = FechaInicial.AddHours(-24)
                Me.FechaFinal = Now()
                Me.TituloIntervalo = "Ultimas 24 horas"
            Case "12H"
                Me.FechaInicial = FechaInicial.AddHours(-12)
                Me.FechaFinal = Now()
                Me.TituloIntervalo = "Ultimas 12 horas"
            Case "6H"
                Me.FechaInicial = FechaInicial.AddHours(-6)
                Me.FechaFinal = Now()
                Me.TituloIntervalo = "Ultimas 6 horas"
            Case "3H"
                Me.FechaInicial = FechaInicial.AddHours(-3)
                Me.FechaFinal = Now()
                Me.TituloIntervalo = "Ultimas 3 horas"
            Case "2H"
                Me.FechaInicial = FechaInicial.AddHours(-2)
                Me.FechaFinal = Now()
                Me.TituloIntervalo = "Ultimas 2 horas"
            Case "1H"
                Me.FechaInicial = FechaInicial.AddHours(-1)
                Me.FechaFinal = Now()
                Me.TituloIntervalo = "Ultima hora"
            Case Else
                If Intervalo Like "*M" Then
                    Me.FechaInicial = FechaInicial.AddMonths(-1)
                    Me.FechaFinal = FechaFinal.AddDays(-1)
                    Me.TituloIntervalo = "Mensual"
                ElseIf Intervalo Like "*D" Then
                    Me.FechaInicial = FechaInicial.AddDays(-7)
                    Me.FechaFinal = FechaFinal.AddDays(-1)
                    Me.TituloIntervalo = "Hace 1 Semana"
                ElseIf Intervalo Like "*DS" Then
                    Me.FechaInicial = FechaInicial.AddDays(-15)
                    Me.FechaFinal = FechaFinal.AddDays(-1)
                    Me.TituloIntervalo = "Hace 2 Semanas"
                Else

                    '******************************************************************************
                    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180904
                    'PARA SETEAR:

                    'FECHAINICIAL = AL PRIMER DIA DEL MES ANTERIOR
                    'FECHAFINAL = AL ULTIMO DIA DEL MES ANTERIOR
                    If Intervalo Like "*MA" Then
                        Me.FechaInicial = DateAdd("m", -1, DateSerial(Year(Today), Month(Today), 1))
                        Me.FechaFinal = DateAdd("m", 0, DateSerial(Year(Today), Month(Today), 0))
                        Me.TituloIntervalo = "Mensual desde el primer dia del mes"

                        'FECHAINICIAL = AL PRIMER DIA DE LA SEMANA ANTERIOR
                        'FECHAFINAL = AL ULTIMO DIA DE LA SEMANA ANTERIOR
                    ElseIf Intervalo Like "*SA" Then
                        Me.FechaInicial = DateTime.Now.AddDays(-DateTime.Now.DayOfWeek - 6)
                        Me.FechaFinal = DateTime.Now.AddDays(-DateTime.Now.DayOfWeek)
                        Me.TituloIntervalo = "Semanal desde el primer dia de la semana"
                    ElseIf Intervalo Like "*QA" Then

                        Dim sDia As String
                        sDia = Intervalo.Substring(0, Intervalo.Length - "QA".Length)

                        Dim iDia As Integer = Integer.Parse(sDia)

                        'FECHAINICIAL = AL DECIMO SEXTO DIA DEL MES ANTERIOR
                        'FECHAFINAL = AL ULTIMO DIA DEL MES ANTERIOR
                        If (iDia <= 15) Then

                            Me.FechaInicial = DateAdd("m", -1, DateSerial(Year(Today), Month(Today), 1))
                            Me.FechaInicial = FechaInicial.AddDays(15)
                            Me.FechaFinal = DateAdd("m", 0, DateSerial(Year(Today), Month(Today), 0))

                            Me.TituloIntervalo = "Quincenal desde el quinceavo dia del mes anterior"

                            'FECHAINICIAL = AL PRIMER DIA DEL MES ACTUAL
                            'FECHAFINAL = AL DECIMO QUINTO DIA DEL MES ACTUAL
                        ElseIf (iDia > 15) Then

                            Me.FechaInicial = DateAdd("m", 0, DateSerial(Year(Today), Month(Today), 1))
                            Me.FechaFinal = FechaInicial.AddDays(14)
                            Me.TituloIntervalo = "Quincenal desde el primer dia del mes actual"
                        End If
                        '******************************************************************************
                    Else
                        Me.FechaInicial = FechaInicial.AddDays(0)
                        Me.FechaFinal = FechaFinal.AddDays(0)
                        Me.TituloIntervalo = "Dia de hoy"
                    End If

                End If

        End Select

        '******************************************************************************
        'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180904
        'PARA GUARDAR UN REGISTRO LOG EN LA TABLA REPORTEPROGRAMADOPENDIENTE
        'VERIFICA QUE LA VARIABLE MY.SETTINGS.ReporteProgramadoPendiente_GuardarLog = 1
        '******************************************************************************
        If Me.Secuencia > -999999 Then


            If My.Settings.ReporteProgramadoPendiente_GuardarLog = "1" Then

                'INDICA QUE EL REPORTE SE ACABA DE GENERAR Y ESTA PENDIENTE SU ENVIO
                ErroresEnvio = "PENDIENTE"

                'DBS.spReporteProgradolog_IngresarPendiente(IdUsuario, Me.Tipo, Secuencia, Me.FechaInicial.ToString(), Me.FechaFinal.ToString(), ErroresEnvio, Me.Parametros)


                Dim negocioPendiente As New ReporteProgramadoPendienteBL()
                Dim objPendiente As New ReporteProgramadoPendienteEN

                objPendiente.baseDatosId = 217
                objPendiente.usuarioId = IdUsuario
                objPendiente.tipoReporte = Tipo
                objPendiente.secuencia = Secuencia
                objPendiente.fechaInicial = Me.FechaInicial.ToString()
                objPendiente.fechaFinal = Me.FechaFinal.ToString()
                objPendiente.errorEnvio = ErroresEnvio
                objPendiente.parametros = Me.Parametros

                Dim insertar As Boolean = negocioPendiente.Insertar(objPendiente)



                ErroresEnvio = ""
            End If
        End If


        '******************************************************************************

    End Sub

    Public Sub CargarParametros()
        Try
            Dim dsReporteCOMP As New dsCalificacionFinal()

            hsCalificacion.Clear()

            With dsReporteCOMP.ReporteConsolidadoCalificacion
                .AddReporteConsolidadoCalificacionRow(0, 0, 0, 0, 0, 0, 0, "Malo")
                hsCalificacion.Add(0, "Malo")

                .AddReporteConsolidadoCalificacionRow(1.7, 0, 0, 0, 0, 0, 1.7, "Malo")
                hsCalificacion.Add(1.7, "Malo")

                .AddReporteConsolidadoCalificacionRow(3.35, 0, 0, 0, 0, 0, 3.35, "Malo")
                hsCalificacion.Add(3.35, "Malo")

                .AddReporteConsolidadoCalificacionRow(1.7, 1.7, 0, 0, 0, 0, 3.4, "Malo")
                hsCalificacion.Add(3.4, "Malo")

                .AddReporteConsolidadoCalificacionRow(1.7, 0, 0, 0, 0, 3.35, 5.05, "Malo")
                hsCalificacion.Add(5.05, "Malo")

                .AddReporteConsolidadoCalificacionRow(1.7, 1.7, 1.7, 0, 0, 0, 5.1, "Malo")
                hsCalificacion.Add(5.1, "Malo")

                .AddReporteConsolidadoCalificacionRow(3.35, 3.35, 0, 0, 0, 0, 6.7, "Malo")
                hsCalificacion.Add(6.7, "Malo")

                .AddReporteConsolidadoCalificacionRow(1.7, 1.7, 0, 0, 0, 3.35, 6.75, "Malo")
                hsCalificacion.Add(6.75, "Malo")

                .AddReporteConsolidadoCalificacionRow(1.7, 1.7, 1.7, 1.7, 0, 0, 6.8, "Malo")
                hsCalificacion.Add(6.8, "Malo")

                .AddReporteConsolidadoCalificacionRow(1.7, 0, 0, 0, 3.35, 3.35, 8.4, "Malo")
                hsCalificacion.Add(8.4, "Malo")

                .AddReporteConsolidadoCalificacionRow(1.7, 1.7, 1.7, 0, 0, 3.35, 8.45, "Regular")
                hsCalificacion.Add(8.45, "Regular")

                .AddReporteConsolidadoCalificacionRow(1.7, 1.7, 1.7, 1.7, 1.7, 0, 8.5, "Regular")
                hsCalificacion.Add(8.5, "Regular")

                .AddReporteConsolidadoCalificacionRow(3.35, 3.35, 3.35, 0, 0, 0, 10.05, "Regular")
                hsCalificacion.Add(10.05, "Regular")

                .AddReporteConsolidadoCalificacionRow(1.7, 1.7, 0, 0, 3.35, 3.35, 10.1, "Regular")
                hsCalificacion.Add(10.1, "Regular")

                .AddReporteConsolidadoCalificacionRow(1.7, 1.7, 1.7, 1.7, 0, 3.35, 10.15, "Regular")
                hsCalificacion.Add(10.15, "Regular")

                .AddReporteConsolidadoCalificacionRow(1.7, 1.7, 1.7, 1.7, 1.7, 1.7, 10.2, "Regular")
                hsCalificacion.Add(10.2, "Regular")

                .AddReporteConsolidadoCalificacionRow(1.7, 0, 0, 3.35, 3.35, 3.35, 11.75, "Regular")
                hsCalificacion.Add(11.75, "Regular")

                .AddReporteConsolidadoCalificacionRow(1.7, 1.7, 1.7, 0, 3.35, 3.35, 11.8, "Regular")
                hsCalificacion.Add(11.8, "Regular")

                .AddReporteConsolidadoCalificacionRow(3.35, 1.7, 1.7, 1.7, 1.7, 1.7, 11.85, "Regular")
                hsCalificacion.Add(11.85, "Regular")

                .AddReporteConsolidadoCalificacionRow(3.35, 3.35, 3.35, 3.35, 0, 0, 13.4, "Regular")
                hsCalificacion.Add(13.4, "Regular")

                .AddReporteConsolidadoCalificacionRow(1.7, 1.7, 0, 3.35, 3.35, 3.35, 13.45, "Regular")
                hsCalificacion.Add(13.45, "Regular")

                .AddReporteConsolidadoCalificacionRow(3.35, 3.35, 1.7, 1.7, 1.7, 1.7, 13.5, "Regular")
                hsCalificacion.Add(13.5, "Regular")

                .AddReporteConsolidadoCalificacionRow(1.7, 0, 3.35, 3.35, 3.35, 3.35, 15.1, "Bueno")
                hsCalificacion.Add(15.1, "Bueno")

                .AddReporteConsolidadoCalificacionRow(3.35, 3.35, 3.35, 1.7, 1.7, 1.7, 15.15, "Bueno")
                hsCalificacion.Add(15.15, "Bueno")

                .AddReporteConsolidadoCalificacionRow(3.35, 3.35, 3.35, 3.35, 3.35, 0, 16.75, "Bueno")
                hsCalificacion.Add(16.75, "Bueno")

                .AddReporteConsolidadoCalificacionRow(3.35, 3.35, 3.35, 3.35, 1.7, 1.7, 16.8, "Bueno")
                hsCalificacion.Add(16.8, "Bueno")

                .AddReporteConsolidadoCalificacionRow(3.35, 3.35, 3.35, 3.35, 3.35, 1.7, 18.45, "Bueno")
                hsCalificacion.Add(18.45, "Bueno")

                .AddReporteConsolidadoCalificacionRow(3.35, 3.35, 3.35, 3.35, 3.35, 3.35, 20.1, "Bueno")
                hsCalificacion.Add(20.1, "Bueno")

            End With
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Public Function CargarImagenXLS(path As String, wk As HSSFWorkbook) As Integer
        Dim file As New FileStream(path, FileMode.Open, FileAccess.Read)
        Dim buffer As Byte() = New Byte(file.Length - 1) {}
        file.Read(buffer, 0, CInt(file.Length))

        Return wk.AddPicture(buffer, PictureType.JPEG)
    End Function

    Public Function ConvertDataTableToHTML(dt As DataTable) As String
        Dim html As String = "<table id='tresumen' border=1 cellpadding='5' cellspacing='3' width='1024' align='center' font='Verdana' font-size='x-small'>"
        'add header row
        html += "<tr>"
        For i As Integer = 0 To dt.Columns.Count - 1
            html += String.Format("<td bgcolor='gray'><b>{0}</b></td>", dt.Columns(i).ColumnName)
        Next
        html += "</tr>"
        'add rows
        For i As Integer = 0 To dt.Rows.Count - 1
            If i Mod 2 = 0 Then
                html += "<tr bgcolor='lightgray'>"
            Else
                html += "<tr>"
            End If

            For j As Integer = 0 To dt.Columns.Count - 1
                html += String.Format("<td>{0}</td>", dt.Rows(i)(j))
            Next
            html += "</tr>"
        Next
        html += "</table>"
        Return html
    End Function

    Public Function ANSI2Fecha(ByVal Fecha As String) As DateTime
        Dim tmpFecha() As String = Split(Fecha, " ")
        Dim nFecha As DateTime

        Dim Año As String = tmpFecha(0).Substring(0, 4)
        Dim mes As String = tmpFecha(0).Substring(4, 2)
        Dim Dia As String = tmpFecha(0).Substring(6, 2)

        nFecha = New DateTime(Año,
                     mes,
                     Dia,
                     tmpFecha(1).Split(":")(0),
                     tmpFecha(1).Split(":")(1),
                     tmpFecha(1).Split(":")(2))

        Año = Nothing
        mes = Nothing
        Dia = Nothing

        tmpFecha = Nothing

        Return nFecha
    End Function

    Public Function Fecha2ANSI(ByVal Fecha As DateTime,
                          Optional ByVal SoloFecha As Boolean = True) As String
        Dim tmpFecha As String = ""

        Try
            With Fecha
                tmpFecha = .Year.ToString()

                If .Month.ToString.Length = 1 Then
                    tmpFecha &= "0" & .Month.ToString()
                Else
                    tmpFecha &= .Month.ToString()
                End If

                If .Day.ToString.Length = 1 Then
                    tmpFecha &= String.Format("0{0} ",
                                              .Day)
                Else
                    tmpFecha &= .Day.ToString() & " "
                End If

                If Not SoloFecha Then
                    If .Hour.ToString.Length = 1 Then
                        tmpFecha &= String.Format("0{0}:",
                                                  .Hour)
                    Else
                        tmpFecha &= .Hour.ToString() & ":"
                    End If

                    If .Minute.ToString.Length = 1 Then
                        tmpFecha &= String.Format("0{0}:",
                                                  .Minute)
                    Else
                        tmpFecha &= .Minute.ToString() & ":"
                    End If

                    If .Second.ToString.Length = 1 Then
                        tmpFecha &= "0" & .Second.ToString()
                    Else
                        tmpFecha &= .Second.ToString()
                    End If
                End If

                If SoloFecha Then
                    tmpFecha = tmpFecha.Trim()
                End If

                Return tmpFecha
            End With
        Catch ex As Exception
            Return "null"
        End Try
    End Function

    Public Sub EnviarMail(ByVal Email As String,
                                 ByVal Archivo As String,
                                 ByVal Msj As String,
                                 ByVal Hoja As ISheet,
                                 ByVal TituloMail As String,
                                 ByVal Tipo As String,
                                 ByVal IdUsuario As Integer,
                                 Optional ByVal Guardar As Boolean = False,
                                 Optional ByVal Archivos As ArrayList = Nothing,
                                 Optional ByVal Comprimir As Boolean = True)
        Try
            Dim correo As New MailMessage()
            Dim Cuerpo As String = ""
            Dim Adjunto As Attachment

            Dim dtResumen As New DataTable()
            Dim tmpFila As IRow
            Dim ind As Integer = 0
            Dim Columnas As String = ""
            Dim Resultados As String = ""
            Dim tmpArchivo As String() = Nothing
            HoraEnvio = Now.ToShortTimeString.Replace(":", "")

            'Dim negocioUsuario As New UsuarioBL()
            'Dim objUsuario As New UsuarioEN

            'objUsuario.baseDatosId = 25
            'objUsuario.usuarioId = IdUsuario

            'objUsuario = negocioUsuario.GetUsuarioPorId(objUsuario)

            ''Me.Usuario = DBS25.getUsuarioxID(IdUsuario)
            'Me.Usuario = objUsuario.usuario

            If Not IsNothing(Hoja) Then
                tmpFila = Hoja.GetRow(4)
                For Each Celda As ICell In tmpFila.Cells
                    Try
                        dtResumen.Columns.Add(Celda.ToString())
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try
                Next

                For indr As Integer = 0 To Hoja.PhysicalNumberOfRows
                    Try
                        If indr > 4 Then
                            Dim d As DataRow = dtResumen.NewRow()
                            d.ItemArray = Hoja.GetRow(indr).ToArray()

                            If Not IsDBNull(d(0)) Then
                                Try
                                    dtResumen.Rows.Add(d)
                                Catch ex As Exception
                                    dtResumen.Rows.Add(d)
                                End Try
                            End If
                            d = Nothing
                        End If
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try
                Next
            End If

            correo.From = New MailAddress(My.Settings.EmailDesde)

            If Email Like "*;*" Then
                Dim tmpEmails As String() = Email.ToString.Split(";")

                For Inde As Integer = 0 To tmpEmails.Length - 1
                    Try
                        If tmpEmails(Inde) <> "" And ValidarEmail(tmpEmails(Inde).Replace("$", "")) Then
                            Console.ForegroundColor = ConsoleColor.Yellow
                            Console.WriteLine("Correo Agregado: " & tmpEmails(Inde).Replace("$", ""))

                            If (tmpEmails(Inde).ToString Like "*$*") Then
                                correo.Bcc.Add(tmpEmails(Inde).Replace("$", ""))
                            Else
                                correo.To.Add(tmpEmails(Inde))
                            End If
                        End If
                    Catch ex As Exception
                        Console.WriteLine("no se Puede Agregar Email")
                    End Try
                Next

                tmpEmails = Nothing
            Else
                correo.To.Add(Email)
            End If

            correo.Subject = TituloMail

            correo.Priority = Net.Mail.MailPriority.High
            FileSize = 0

            Dim nombreArchivo As String
            Dim nombreDirectorio As String

            Try


                nombreDirectorio = String.Format("{0}\{1}\{2}",
                                                 My.Settings.RutaDescargas,
                                                 Usuario,
                                                 Now.Date.ToShortDateString().Replace("/", ""))

                If Not Directory.Exists(nombreDirectorio) Then
                    Directory.CreateDirectory(nombreDirectorio)
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            'CUANDO SE ENVIE UN NOMBRE DE ARCHIVO
            If Archivo <> "" Then

                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
                'PARA INCLUIR EN EL NOMBRE DE ARCHIVO LOS ARCHIVOS
                'FORMATO XLS
                'FORMATO PDF ( REPORTES EJECUTIVOS )
                '******************************************************************************
                'ArchivoZIP = Archivo.Replace("xls", "zip")

                Dim positionExtension As Integer

                positionExtension = InStr(Archivo, ".xls")
                If positionExtension > 0 Then
                    'ArchivoZIP = Archivo.Replace(".xls", "_") & Me.Secuencia & "_" & FechaInicial.ToString().Substring(0, 10).Replace("/", "-") & "_" & FechaFinal.ToString().Substring(0, 10).Replace("/", "-") & ".zip"
                    ArchivoZIP = Archivo.Replace(".xls", "") & ".zip"
                End If

                positionExtension = InStr(Archivo, ".pdf")
                If positionExtension > 0 Then
                    ArchivoZIP = Archivo.Replace(".pdf", "") & ".zip"
                End If

                positionExtension = InStr(Archivo, ".csv")
                If positionExtension > 0 Then
                    ArchivoZIP = Archivo.Replace(".csv", "") & ".zip"
                End If

                '******************************************************************************


                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20190208
                'PARA VERIFICAR SI LOS ARCHIVOS ADJUNTOS SE COMPRIMIRAN O NO
                '******************************************************************************
                If Comprimir = True Then
                    Dim uriArchivoZip As String = Archivo.Replace(" ", "_")
                    Dim zipUri As String = String.Concat("/", IO.Path.GetFileName(uriArchivoZip))
                    Dim partUri As New Uri(zipUri, UriKind.Relative)
                    Dim contentType As String = Net.Mime.MediaTypeNames.Application.Zip

                    Try
                        If File.Exists(ArchivoZIP) Then
                            Try
                                File.Delete(ArchivoZIP)
                            Catch ex As Exception
                                Console.WriteLine(ex.Message)
                            End Try
                        End If

                        oZip = ZipPackage.Open(ArchivoZIP, FileMode.OpenOrCreate, FileAccess.ReadWrite)

                        Dim pkgPart As PackagePart = oZip.CreatePart(partUri, contentType, CompressionOption.Maximum)
                        Dim bites As Byte() = File.ReadAllBytes(Archivo)
                        pkgPart.GetStream().Write(bites, 0, bites.Length)

                        oZip.Close()

                        pkgPart = Nothing
                        bites = Nothing

                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try

                    Try
                        FileSize = Round(My.Computer.FileSystem.GetFileInfo(ArchivoZIP).Length / 1024000, 0)
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try

                    oZip = Nothing
                    uriArchivoZip = Nothing
                    zipUri = Nothing
                    partUri = Nothing
                    contentType = Nothing

                    If FileSize <= 9 Then
                        Try
                            Adjunto = Nothing
                            Adjunto = New Attachment(ArchivoZIP)
                            correo.Attachments.Add(Adjunto)
                        Catch ex As Exception
                            Adjunto = Nothing
                            Adjunto = New Attachment(Archivo)
                            correo.Attachments.Add(Adjunto)
                        End Try
                    Else
                        tmpArchivo = Nothing

                        Try
                            tmpArchivo = ArchivoZIP.Split("\")

                            nombreArchivo = String.Format("{0}{1}\{2}\{3}",
                                                          My.Settings.RutaDescargas,
                                                          Usuario,
                                                          Now.Date.ToShortDateString().Replace("/", ""),
                                                          tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", String.Format("_{0}.zip", HoraEnvio)))

                            If File.Exists(nombreArchivo) Then
                                Try
                                    File.Delete(nombreArchivo)
                                Catch ex As Exception
                                    Console.WriteLine(ex.Message)
                                End Try
                            End If

                            nombreArchivo = String.Format("{0}{1}\{2}\{3}",
                                                          My.MySettings.Default.RutaDescargas,
                                                          Usuario, Now.Date.ToShortDateString().Replace("/", ""),
                                                          tmpArchivo(tmpArchivo.Length - 1).Replace(".zip", String.Format("_{0}.zip", HoraEnvio)))

                            File.Copy(ArchivoZIP, nombreArchivo, True)

                        Catch ex As Exception
                            tmpArchivo = Archivo.Split("\")

                            nombreArchivo = String.Format("{0}{1}\{2}\{3}",
                                                          My.MySettings.Default.RutaDescargas,
                                                          Usuario, Now.Date.ToShortDateString().Replace("/", ""),
                                                          tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", String.Format("_{0}.xls", HoraEnvio)))

                            File.Copy(Archivo, nombreArchivo, True)
                            Console.Write(ex.Message)
                        End Try
                    End If

                Else

                    Try
                        FileSize = Round(My.Computer.FileSystem.GetFileInfo(Archivo).Length / 1024000, 0)
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try


                    If FileSize <= 9 Then
                        Try
                            Adjunto = Nothing
                            Adjunto = New Attachment(Archivo)
                            correo.Attachments.Add(Adjunto)
                        Catch ex As Exception
                            Adjunto = Nothing
                            Adjunto = New Attachment(Archivo)
                            correo.Attachments.Add(Adjunto)
                        End Try
                    Else
                        tmpArchivo = Nothing

                        Try
                            tmpArchivo = Archivo.Split("\")

                            nombreArchivo = String.Format("{0}{1}\{2}\{3}",
                                                          My.Settings.RutaDescargas,
                                                          Usuario,
                                                          Now.Date.ToShortDateString().Replace("/", ""),
                                                          tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", String.Format("_{0}.zip", HoraEnvio)))

                            If File.Exists(nombreArchivo) Then
                                Try
                                    File.Delete(nombreArchivo)
                                Catch ex As Exception
                                    Console.WriteLine(ex.Message)
                                End Try
                            End If

                            nombreArchivo = String.Format("{0}{1}\{2}\{3}",
                                                          My.MySettings.Default.RutaDescargas,
                                                          Usuario, Now.Date.ToShortDateString().Replace("/", ""),
                                                          tmpArchivo(tmpArchivo.Length - 1).Replace(".zip", String.Format("_{0}.zip", HoraEnvio)))

                            File.Copy(Archivo, nombreArchivo, True)

                        Catch ex As Exception
                            tmpArchivo = Archivo.Split("\")

                            nombreArchivo = String.Format("{0}{1}\{2}\{3}",
                                                          My.MySettings.Default.RutaDescargas,
                                                          Usuario, Now.Date.ToShortDateString().Replace("/", ""),
                                                          tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", String.Format("_{0}.xls", HoraEnvio)))

                            File.Copy(Archivo, nombreArchivo, True)
                            Console.Write(ex.Message)
                        End Try
                    End If



                End If
                '******************************************************************************


                'CUANDO SE ENVIE MULTIPLES ARCHIVOS EXCEL
            ElseIf Not IsNothing(Archivos) Then
                Try
                    oZip = Nothing
                    '******************************************************************************
                    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
                    'PARA INCLUIR EN EL NOMBRE DE ARCHIVO LA FECHAINICIAL Y LA FECHA FINAL DEL REPORTE
                    '******************************************************************************
                    'ArchivoZIP = String.Format("{0}Usuarios\{1}\{2}\Recorrido\Flota.zip", AppDomain.CurrentDomain.BaseDirectory, Me.Usuario, Now.Date.ToShortDateString().Replace("/", ""))

                    nombreArchivo = String.Format("{0}Usuarios\{1}\{2}\Recorrido\Flota_{3}_{4}_{5}_{6}.zip",
                                               AppDomain.CurrentDomain.BaseDirectory,
                                               Me.Usuario,
                                               Now.Date.ToShortDateString().Replace("/", ""),
                                               Me.Usuario,
                                               Me.Secuencia,
                                               FechaInicial.ToString().Substring(0, 10).Replace("/", ""),
                                               FechaFinal.ToString().Substring(0, 10).Replace("/", ""))

                    ArchivoZIP = nombreArchivo
                    '******************************************************************************

                    If File.Exists(ArchivoZIP) Then
                        Try
                            File.Delete(ArchivoZIP)
                        Catch ex As Exception
                            Console.WriteLine(ex.Message)
                        End Try
                    End If

                    oZip = ZipPackage.Open(ArchivoZIP, FileMode.OpenOrCreate, FileAccess.ReadWrite)

                    'AGREGANDO EN ARCHIVO COMPRIMIDO LA LISTA DE ARCHIVO
                    If Archivos.Count > 0 Then

                        For JL As Integer = 0 To Archivos.Count - 1
                            Try

                                Dim uriArchivoZip As String = Archivos(JL).Replace(" ", "_")
                                Dim zipUri As String = String.Concat("/", IO.Path.GetFileName(uriArchivoZip))
                                Dim partUri As New Uri(zipUri, UriKind.Relative)
                                Dim contentType As String = Net.Mime.MediaTypeNames.Application.Zip

                                Try
                                    Dim pkgPart As PackagePart = oZip.CreatePart(partUri, contentType, CompressionOption.Maximum)
                                    Dim bites As Byte() = File.ReadAllBytes(Archivos(JL))
                                    pkgPart.GetStream().Write(bites, 0, bites.Length)
                                    pkgPart = Nothing
                                    bites = Nothing
                                Catch ex As Exception
                                    Console.WriteLine(ex.Message)
                                End Try
                            Catch ex As Exception
                                Console.WriteLine(ex.Message)
                            End Try
                        Next

                    Else
                        'CREAR ARCHIVO DE TEXTO CON MENSAJE QUE NO EXISTE EL RECORRIDO
                        Dim ArchivoTEXT As String = String.Format("{0}Usuarios\{1}\{2}\Recorrido\Flota_{3}_{4}_{5}_{6}_{7}.txt",
                                                                  AppDomain.CurrentDomain.BaseDirectory,
                                                                  Me.Usuario,
                                                                  Now.Date.ToShortDateString().Replace("/", ""),
                                                                  Me.Tipo,
                                                                  Me.Usuario,
                                                                  Me.Secuencia,
                                                                  Fecha2ANSI(FechaInicial.ToString(), True),
                                                                  Fecha2ANSI(FechaFinal.ToString(), True))



                        Dim fileTEXT As System.IO.StreamWriter
                        fileTEXT = My.Computer.FileSystem.OpenTextFileWriter(ArchivoTEXT, False)
                        fileTEXT.Write("El usuario no cuenta con vehiculos activos para generar el reporte.")
                        fileTEXT.Close()

                        Try
                            Dim uriArchivoZip As String = ArchivoTEXT
                            Dim zipUri As String = String.Concat("/", IO.Path.GetFileName(uriArchivoZip))
                            Dim partUri As New Uri(zipUri, UriKind.Relative)
                            Dim contentType As String = Net.Mime.MediaTypeNames.Application.Zip

                            Try
                                Dim pkgPart As PackagePart = oZip.CreatePart(partUri, contentType, CompressionOption.Maximum)
                                Dim bites As Byte() = File.ReadAllBytes(ArchivoTEXT)
                                pkgPart.GetStream().Write(bites, 0, bites.Length)
                                pkgPart = Nothing
                                bites = Nothing
                            Catch ex As Exception
                                Console.WriteLine(ex.Message)
                            End Try
                        Catch ex As Exception
                            Console.WriteLine(ex.Message)
                        End Try

                    End If


                    oZip.Close()

                    Try
                        FileSize = Round(My.Computer.FileSystem.GetFileInfo(ArchivoZIP).Length / 1024000, 0)
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try

                    Try
                        tmpArchivo = ArchivoZIP.Split("\")


                        nombreArchivo = String.Format("{0}{1}\{2}\{3}",
                                                      My.Settings.RutaDescargas,
                                                      Usuario,
                                                      Now.Date.ToShortDateString().Replace("/", ""),
                                                      tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", String.Format("_{0}.zip", HoraEnvio)))

                        If File.Exists(nombreArchivo) Then
                            Try
                                File.Delete(nombreArchivo)
                            Catch ex As Exception
                                Console.WriteLine(ex.Message)
                            End Try
                        End If

                        nombreArchivo = String.Format("{0}{1}\{2}\{3}",
                                                      My.MySettings.Default.RutaDescargas,
                                                      Usuario, Now.Date.ToShortDateString().Replace("/", ""),
                                                      tmpArchivo(tmpArchivo.Length - 1).Replace(".zip", String.Format("_{0}.zip", HoraEnvio)))

                        File.Copy(ArchivoZIP, nombreArchivo, True)
                    Catch ex As Exception
                        tmpArchivo = Archivo.Split("\")

                        nombreArchivo = String.Format("{0}{1}\{2}\{3}",
                                                      My.MySettings.Default.RutaDescargas,
                                                      Usuario, Now.Date.ToShortDateString().Replace("/", ""),
                                                      tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", String.Format("_{0}.xls", HoraEnvio)))

                        File.Copy(Archivo, nombreArchivo, True)
                        Console.Write(ex.Message)
                    End Try
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            End If

            Dim Mensaje As String = ""

            If Not IsNothing(Hoja) Then
                Mensaje &= "<table cellpadding='0' cellspacing='15' bgcolor='ffffff' width='600' align='center' style='font-family:arial;'>"
                Mensaje &= String.Format("<tr><td>{0}</td></tr>", ConvertDataTableToHTML(dtResumen))
                Mensaje &= "</table>"
            End If

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
            'PARA ADJUNTAR ARCHIVO ZIP AL CORREO A ENVIAR
            '******************************************************************************
            If correo.Attachments.Count = 0 Then
                Try
                    Adjunto = Nothing
                    Adjunto = New Attachment(ArchivoZIP)
                    correo.Attachments.Add(Adjunto)
                Catch ex As Exception

                End Try

            End If

            If FileSize <= 9 Then
                If (Tipo.Contains("SKY")) Then
                    Mensaje = "Adjunto a este email se encuentra un archivo comprimido con el reporte de  dispositivos Skywave."
                Else
                    Mensaje = "Estimado cliente, adjunto a este email se encuentra un archivo comprimido con el reporte programado de las unidades de su flota."
                End If

                If (Tipo.Contains("MGR")) Then
                    Mensaje &= "<table style='border: 0; margin: 0; padding: 0; border-collapse: collapse;'>"
                    Mensaje &= String.Format("<tr><td>Desde: </td><td>{0} {1}</td></tr>", Fecha2ANSI(Me.FechaInicial.ToString(), True), Me.HoraInicio)
                    Mensaje &= String.Format("<tr><td>Hasta: </td><td>{0} {1}</td></tr>", Fecha2ANSI(Me.FechaFinal.ToString(), True), Me.HoraFin)
                    Mensaje &= String.Format("<tr><td>Cliente: </td><td>{0}</td></tr>", Nombre)
                    Mensaje &= "</table>"
                End If
            Else
                Mensaje = String.Format("Estimado cliente, para descargar el archivo comprimido de su reporte por favor de un click <a href='http://www.huntermonitoreo.com/Geo/Reportes/Descargas/{0}/{1}/{2}'>aqui</a>",
                                        Usuario,
                                        Now.Date.ToShortDateString().Replace("/", ""),
                                        tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", ".zip").Replace(".zip", String.Format("_{0}.zip", HoraEnvio)))
            End If

            correo.Body = GeneraBodyEmail(Tipo, Nombre, Mensaje)
            correo.IsBodyHtml = True
            correo.Priority = MailPriority.Normal
            Dim smtp As New SmtpClient() With {.Host = My.Settings.ServidorEmail, .EnableSsl = False}
            '
            Try
                If My.Settings.EnviarEmails Then
                    smtp.Send(correo)
                    Console.ForegroundColor = ConsoleColor.Green
                    Console.WriteLine("Correo Enviado con Exito a las " & Now.ToString())
                Else
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Envio de Emails Desactivado")
                    Console.ForegroundColor = ConsoleColor.Gray
                End If
            Catch ex As Exception
                ErroresEnvio &= "ERR_SENDMAIL"
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(ex.Message)
                Console.ForegroundColor = ConsoleColor.Gray
            End Try

            Try
                If Not Directory.Exists(String.Format("{0}Usuarios\{1}", AppDomain.CurrentDomain.BaseDirectory, Usuario)) Then
                    Directory.CreateDirectory(String.Format("{0}Usuarios\{1}", AppDomain.CurrentDomain.BaseDirectory, Usuario))
                End If

                If Not Directory.Exists(String.Format("Usuarios\{0}\{1}", Usuario, Now.Date.ToShortDateString().Replace("/", ""))) Then
                    Directory.CreateDirectory(String.Format("Usuarios\{0}\{1}", Usuario, Now.Date.ToShortDateString().Replace("/", "")))
                End If

                If Archivo <> "" Then
                    tmpArchivo = Nothing
                    tmpArchivo = Archivo.Split("\")

                    'String.Format("{0}Usuarios\{1}\{2}\{3}", AppDomain.CurrentDomain.BaseDirectory, Usuario, Now.Date.ToShortDateString().Replace("/", ""), tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", String.Format("_{0}.zip", HoraEnvio)))
                    'String.Format("{0}Usuarios\{1}\{2}\{3}", AppDomain.CurrentDomain.BaseDirectory, Usuario, Now.Date.ToShortDateString().Replace("/", ""), tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", String.Format("_{0}.zip", HoraEnvio)))
                    'String.Format("{0}Usuarios\{1}\{2}\{3}", AppDomain.CurrentDomain.BaseDirectory, Usuario, Now.Date.ToShortDateString().Replace("/", ""), tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", String.Format("_{0}.zip", HoraEnvio)))
                    nombreArchivo = String.Format("{0}Usuarios\{1}\{2}\{3}", _
                                                  AppDomain.CurrentDomain.BaseDirectory, _
                                                  Usuario, Now.Date.ToShortDateString().Replace("/", ""), _
                                                  tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", String.Format("_{0}.zip", HoraEnvio)))

                    Try
                        If File.Exists(nombreArchivo) Then
                            Try
                                File.Delete(nombreArchivo)
                            Catch ex As Exception
                                Console.WriteLine(ex.Message)
                            End Try
                        End If


                        File.Copy(ArchivoZIP, nombreArchivo, True)
                    Catch ex As Exception

                        nombreArchivo = String.Format("{0}Usuarios\{1}\{2}\{3}", _
                                                      AppDomain.CurrentDomain.BaseDirectory, _
                                                      Usuario, _
                                                      Now.Date.ToShortDateString().Replace("/", ""), tmpArchivo(tmpArchivo.Length - 1))

                        File.Copy(Archivo, nombreArchivo, True)
                        Console.Write(ex.Message)
                    End Try
                Else
                    For JL As Integer = 0 To Archivos.Count - 1
                        Try
                            File.Delete(Archivos(JL))
                        Catch ex As Exception
                            Console.WriteLine("No se puede eliminar archivo: " & Archivos(JL))
                        End Try
                    Next
                End If
                'If FileSize > 9 Then
                '    Try
                '        If File.Exists(My.Settings.RutaDescargas & Usuario & "\" & Now.Date.ToShortDateString().Replace("/", "") & "\" & tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", "_" & HoraEnvio.ToString() & ".zip")) Then
                '            Try
                '                File.Delete(My.Settings.RutaDescargas & Usuario & "\" & Now.Date.ToShortDateString().Replace("/", "") & "\" & tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", "_" & HoraEnvio.ToString() & ".zip"))
                '            Catch ex As Exception

                '            End Try
                '        End If

                '        File.Copy(ArchivoZIP, My.Settings.RutaDescargas & Usuario & "\" & Now.Date.ToShortDateString().Replace("/", "") & "\" & tmpArchivo(tmpArchivo.Length - 1).Replace(".xls", "_" & HoraEnvio.ToString() & ".zip"), True)
                '    Catch ex As Exception
                '        File.Copy(Archivo, System.AppDomain.CurrentDomain.BaseDirectory & "Usuarios\" & Usuario & "\" & Now.Date.ToShortDateString().Replace("/", "") & "\" & tmpArchivo(tmpArchivo.Length - 1), True)
                '        Console.Write(ex.Message)
                '    End Try
                'End If

                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
                'PARA GUARDAR LOG EN CASO DE ENVIO CORRECTO
                '******************************************************************************
                GuardarLog(Tipo, ErroresEnvio)
                '******************************************************************************

                tmpArchivo = Nothing
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            correo = Nothing
            smtp = Nothing
        Catch ex As Exception
            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
            'PARA GUARDAR LOG EN CASO DE ERROR
            '******************************************************************************
            ErroresEnvio &= ex.Message
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(ex.Message)
            Console.ForegroundColor = ConsoleColor.Gray
            GuardarLog(Tipo, ErroresEnvio)
            '******************************************************************************
        Finally
            Try
                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20181115
                'PENDIENTE DE VERIFICAR ESTA CERRADA DE WORKBOOK EXCEL
                'YA QUE ESTA CAUSANDO UN WARNING
                '******************************************************************************
                wk.Close()
                wk = Nothing
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        End Try
    End Sub

    Public Function GetCourse(ByVal nCurso As Integer) As String
        Select Case nCurso
            Case 0
                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
                'PARA ASIGNAR ETIQUETA AL RUMBO CUANDO EL VALOR SEA 0
                '******************************************************************************
                'Return ""
                Return "N"
                '******************************************************************************
            Case 1 To 10
                Return "N"
            Case 351 To 360
                Return "N"
            Case 11 To 80
                Return "NE"
            Case 81 To 100
                Return "E"
            Case 101 To 170
                Return "SE"
            Case 171 To 190
                Return "S"
            Case 191 To 260
                Return "SO"
            Case 261 To 280
                Return "O"
            Case 281 To 350
                Return "NO"
        End Select
    End Function

    Public Function GetDistanceGEO(ByVal P1 As SqlGeography, ByVal p2 As SqlGeography) As Double
        Try
            Dim Distancia As Double = 0.0

            Distancia = P1.STDistance(p2)

            Return Distancia
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Function GetDistance(ByVal point1 As GeoCoordinate,
                               ByVal point2 As GeoCoordinate) As Double
        Dim result As Double = 0

        Dim Lat As Double = (point2.Latitude - point1.Latitude) * (Math.PI / 180)
        Dim Lon As Double = (point2.Longitude - point1.Longitude) * (Math.PI / 180)
        Dim a As Double = Math.Sin(Lat / 2) * Math.Sin(Lat / 2) + Math.Cos(point1.Latitude * (Math.PI / 180)) * Math.Cos(point2.Latitude * (Math.PI / 180)) * Math.Sin(Lon / 2) * Math.Sin(Lon / 2)

        Dim c As Double = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a))
        result = EarthRadius * c * 1000


        Return Math.Round(result, 2)
    End Function

    Function Secs2HorasMin(ByVal Segundos As Integer) As String
        Dim Tiempo As Double
        Dim tmpTiempo As String()

        Dim tHoras As String = ""
        Dim tMinutos As String = ""

        Try
            Tiempo = Round(Segundos / 3600, 2)
            tmpTiempo = Tiempo.ToString.Split(".")

            Try
                tMinutos = Round(CDbl(tmpTiempo(1)) * 60 / 100, 0)
            Catch ex As Exception
                tMinutos = "00"
            End Try

            If tmpTiempo(0).ToString.Length = 1 Then
                tHoras = "0" & tmpTiempo(0)
            Else
                tHoras = tmpTiempo(0)
            End If

            If tMinutos.Length = 1 Then
                tMinutos &= "0"
            End If

            Return String.Format("{0}:{1}",
                                 tHoras,
                                 tMinutos)

        Catch ex As Exception
            Return "00:00"

            Console.Write(ex.Message)
        Finally
            tHoras = Nothing
            tMinutos = Nothing
        End Try
    End Function

    Function Min2Text(ByVal CantidadMinutos As Double) As String
        Dim Meses As Integer = 0
        Dim Dias As Integer = 0
        Dim Horas As Integer = 0
        Dim Minutos As Integer = 0
        Dim Segundos As Double = 0
        Dim FechaHora As String = ""

        Try
            Horas = (CantidadMinutos - (Meses * 43200) - (Dias * 1440)) / 60
            Minutos = CantidadMinutos - (Meses * 43200) - (Dias * 1440) - (Horas * 60)
            Segundos = (CantidadMinutos - (Meses * 43200) - (Dias * 1440) - (Horas * 60) - Minutos) * 60.0

            FechaHora = ""
            If Meses > 0 Then
                FechaHora = Meses.ToString() & " Mes(es) "
            End If

            If Dias > 0 Then
                FechaHora = String.Format("{0}{1} dia(s) ", FechaHora, Dias)
            End If

            If Horas > 0 Then
                FechaHora = String.Format("{0}{1} hrs(s) ", FechaHora, Horas)
            End If

            FechaHora = String.Format("{0}{1} min(s) ", FechaHora, Minutos)
            If Segundos > 0 Then
                FechaHora = String.Format("{0}{1} seg(s)", FechaHora, Round(Segundos, 0))
            End If

            Return FechaHora
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function ValidarEmail(ByVal email As String) As Boolean
        If email = String.Empty Then Return False
        Dim re As Regex = New Regex("^[\w._%-]+@[\w.-]+\.[a-zA-Z]{2,4}$")
        Dim m As Match = re.Match(email)
        Return (m.Captures.Count <> 0)
    End Function

    Public Function KilometrosAMillas(ByVal Kilometros As Double) As Double
        Try
            Return Round(Kilometros / 1.609344, 3)
        Catch ex As Exception
            Return Round(Kilometros, 2)
        End Try
    End Function

    Public Function MillasAKilometros(ByVal Millas As Double) As Double
        Try
            Return Round(Millas * 1.609344, 0)
        Catch ex As Exception
            Return Round(Millas, 2)
        End Try
    End Function

    Public Function MillasAMillasNauticas(ByVal Millas As Double) As Double
        Try
            Return Round(Millas * 0.868976240408186, 0)
        Catch ex As Exception
            Return Round(Millas, 2)
        End Try
    End Function

    Public Function MillasNauticasAMillas(ByVal MillasNauticas As Double) As Double
        Try
            Return Round(MillasNauticas / 0.868976240408186, 2)
        Catch ex As Exception
            Return Round(MillasNauticas, 2)
        End Try
    End Function

    Public Function Decimal2GMS(ByVal Valor As Double, ByVal EsLatitud As Boolean) As String
        Dim tmpDecimal() As String
        Dim tmpMin As Double = 0
        Dim tmpSec As Double = 0

        Dim tmpGMS As String = Math.Truncate(Valor).ToString & "º "

        Try
            If Valor > 0 Then
                tmpMin = Valor - Abs(Math.Truncate(Valor))
            Else
                tmpMin = Valor + Abs(Math.Truncate(Valor))
            End If

            tmpGMS &= Abs(Truncate(tmpMin * 60)).ToString & "' "
            tmpDecimal = (tmpMin * 60).ToString.Split(".")

            Try
                tmpSec = CDbl("0." & tmpDecimal(1))
            Catch ex As Exception
                tmpSec = CDbl("60.0")
            End Try


            tmpGMS &= Abs(Truncate(tmpSec * 60)).ToString & " ''"

            If Valor < 0.0 Then
                If EsLatitud Then
                    tmpGMS = "S " & tmpGMS
                Else
                    tmpGMS = "W " & tmpGMS
                End If
            Else
                If EsLatitud Then
                    tmpGMS = "N " & tmpGMS
                Else
                    tmpGMS = "E " & tmpGMS
                End If
            End If

            Return tmpGMS.Replace("-", "")
        Catch ex As Exception
            Return Valor
        Finally
            tmpDecimal = Nothing
            tmpMin = Nothing
            tmpSec = Nothing

        End Try
    End Function

    Public Function Decimal2GM(ByVal Valor As Double, ByVal EsLatitud As Boolean) As String
        Dim tmpDecimal() As String
        Dim tmpMin As Double = 0
        Dim tmpSec As Double = 0

        Dim tmpGM As String = Math.Truncate(Valor).ToString & "º "

        Try
            If Valor > 0 Then
                tmpMin = Valor - Abs(Math.Truncate(Valor))
            Else
                tmpMin = Valor + Abs(Math.Truncate(Valor))
            End If

            tmpGM &= Abs(Truncate(tmpMin * 60)).ToString & "' "

            tmpDecimal = (tmpMin * 60).ToString.Split(".")

            'Try
            '    tmpSec = CDbl("0." & tmpDecimal(1))
            'Catch ex As Exception
            '    tmpSec = CDbl("60.0")
            'End Try


            'tmpGMS &= Abs(Truncate(tmpSec * 60)).ToString & """"

            If Valor < 0.0 Then
                If EsLatitud Then
                    tmpGM = "S " & tmpGM
                Else
                    tmpGM = "W " & tmpGM
                End If
            Else
                If EsLatitud Then
                    tmpGM = "N " & tmpGM
                Else
                    tmpGM = "E " & tmpGM
                End If
            End If

            Return tmpGM.Replace("-", "")
        Catch ex As Exception
            Return Valor
        Finally
            tmpDecimal = Nothing
            tmpMin = Nothing
            tmpSec = Nothing

        End Try
    End Function

    Public Sub GuardarLog(ByVal TipoReporte As String,
                                Optional ByVal ErrorEnvio As String = "")
        Dim SecuenciaLog As Integer
        Try
            SecuenciaLog = 0

            If Me.Secuencia > -999999 Then
                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
                'PARA MODIFICAR EL VALOR DE LA VARIABLE ErrorEnvio
                'PARA PROCESAR UN REPORTE PENDIENTE DE ENVIAR EN LA TABLA LOG REPORTEPROGRAMADOPENDIENTE
                'VERIFICA QUE LA VARIABLE MY.SETTINGS.ReporteProgramadoPendiente_GuardarLog = 1
                '******************************************************************************

                'If ErrorEnvio = "" Then
                '    DBS.spReporteProgradolog_Ingresar(IdUsuario, TipoReporte, "OK", Email)
                'Else
                '    DBS.spReporteProgradolog_Ingresar(IdUsuario, TipoReporte, ErrorEnvio, Email)
                'End If

                If ErrorEnvio = "" Then
                    ErrorEnvio = "OK"
                End If




                'DBS.spReporteProgradolog_Ingresar(IdUsuario, TipoReporte, ErrorEnvio, Email)

                Dim negocioLog As New ReporteProgramadoLogBL()
                Dim objLog As New ReporteProgramadoLogEN

                objLog.baseDatosId = 217
                objLog.usuarioId = IdUsuario
                objLog.tipoReporte = TipoReporte
                objLog.observaciones = ErrorEnvio
                objLog.email = Email


                Dim actualizar As Boolean = negocioLog.Insertar(objLog)

                If My.Settings.ReporteProgramadoPendiente_GuardarLog = "1" Then

                    'DBS.spReporteProgradolog_IngresarPendiente(IdUsuario, TipoReporte, Secuencia, , Me.FechaFinal.ToString(), ErrorEnvio, Me.Parametros)

                    Dim negocioPendiente As New ReporteProgramadoPendienteBL()
                    Dim objPendiente As New ReporteProgramadoPendienteEN

                    objPendiente.baseDatosId = 217
                    objPendiente.usuarioId = IdUsuario
                    objPendiente.tipoReporte = TipoReporte
                    objPendiente.secuencia = Secuencia

                    objPendiente.fechaInicial = Me.FechaInicial.ToString()
                    objPendiente.fechaFinal = Me.FechaFinal.ToString()
                    objPendiente.errorEnvio = ErrorEnvio
                    objPendiente.parametros = Parametros

                    actualizar = negocioPendiente.Insertar(objPendiente)

                End If
                '******************************************************************************

                Try

                    'Dim negocioLog As New ReporteProgramadoLogBL()
                    ''Dim objLog As New ReporteProgramadoLogEN

                    objLog.baseDatosId = 217
                    objLog.usuarioId = IdUsuario

                    objLog = negocioLog.GetUltimoId(objLog)


                    'SecuenciaLog = DBS25.BuscarUltimoResumen(IdUsuario)
                    SecuenciaLog = objLog.secuencia

                Catch ex As Exception
                    SecuenciaLog = 0
                End Try

                If SecuenciaLog > 0 And Me.Guardar And TipoReporte = "COM" Then
                    Try

                        Dim negocioResumen As New ReporteProgramadoResumenBL()
                        Dim objResumen As New ReporteProgramadoResumenEN

                        objResumen.baseDatosId = 217
                        objResumen.usuarioId = IdUsuario
                        objResumen.secuencia = SecuenciaLog
                        objResumen.tipoReporte = TipoReporte
                        objResumen.anio = Now.Year
                        objResumen.mes = Now.Month
                        objResumen.semana = 0
                        objResumen.dia = Now.Day
                        objResumen.totalBuenos = cFinalBueno
                        objResumen.totalRegulares = cFinalRegular
                        objResumen.totalMalos = cFinalMalo

                        actualizar = negocioResumen.Insertar(objResumen)

                        'DBS.spReporteProgramadoResumen_Ingresar(IdUsuario,
                        '                                        SecuenciaLog,
                        '                                        TipoReporte,
                        '                                        Now.Year,
                        '                                        Now.Month,
                        '                                        0,
                        '                                        Now.Day,
                        '                                        cFinalBueno,
                        '                                        cFinalRegular,
                        '                                        cFinalMalo)
                    Catch ex As Exception
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine(String.Format("{0} Error en Ingreso de Resumen de Reporte para {1}", ex.Message, IdUsuario))
                    Finally
                        cFinalBueno = 0
                        cFinalMalo = 0
                        cFinalRegular = 0
                    End Try
                End If
            ElseIf Me.Secuencia = -999999 And Me.Tipo = "CRE" Then

                If My.Settings.ReporteProgramadoPendiente_GuardarLog = "1" Then

                    'DBS.spReporteProgradolog_IngresarPendiente(IdUsuario, TipoReporte, Secuencia, , Me.FechaFinal.ToString(), ErrorEnvio, Me.Parametros)

                    Dim negocioPendiente As New ReporteProgramadoPendienteBL()
                    Dim objPendiente As New ReporteProgramadoPendienteEN

                    objPendiente.baseDatosId = 217
                    objPendiente.usuarioId = IdUsuario
                    objPendiente.tipoReporte = TipoReporte
                    objPendiente.secuencia = Secuencia
                    objPendiente.fechaInicial = Me.FechaInicial.ToString()
                    objPendiente.fechaFinal = Me.FechaFinal.ToString()
                    objPendiente.errorEnvio = ErrorEnvio
                    objPendiente.parametros = Parametros

                    Dim actualizar As Boolean = negocioPendiente.Insertar(objPendiente)

                End If
            End If

            
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
    'PARA AGREGAR ETIQUETA AL ESTADOGPS
    '******************************************************************************
    Public Function GetGPS_Status(ByVal nEstado As Integer) As String
        Select Case nEstado
            Case 1
                Return "Válido"
            Case Else
                Return "No Válido"
        End Select
    End Function
    '******************************************************************************

    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180822
    'PARA AGREGAR ETIQUETA AL ESTADOGPS
    '******************************************************************************
    Public Sub SetColumnsOrder(ByVal table As DataTable, ByVal columnNames As String())

        Dim columnIndex As Integer = 0

        For Each columnName In columnNames
            table.Columns(columnName).SetOrdinal(columnIndex)
            columnIndex += columnIndex
        Next


    End Sub
    '******************************************************************************

    Private Function GeneraBodyEmail(ByVal TipoReporte As String, ByVal NombreEntidad As String, ByVal MensajeEmail As String
                                     ) As String

        Dim TituloMail As String

        Select Case TipoReporte
            Case "TRYD"
                TituloMail = String.Format("Reportes Detalle de Trayectos de la Flota ({0})", Me.TituloIntervalo)
            Case "HISM"
                TituloMail = String.Format("Reporte Historico de la Flota ({0})", Me.TituloIntervalo)
            Case "EJE"
                TituloMail = String.Format("Reporte Ejecutivo de la Flota ({0})", Me.TituloIntervalo)
            Case "EVE"
                TituloMail = String.Format("Reporte Eventos de la Flota ({0})", Me.TituloIntervalo)
            Case "HTRD"
                TituloMail = String.Format("Reporte Detalle de Horas Trabajadas de la Flota ({0})", Me.TituloIntervalo)
            Case "PRM"
                TituloMail = String.Format("Reporte Protocolo de Manejo de la Flota ({0})", Me.TituloIntervalo)
            Case "MGR"
                TituloMail = String.Format("Reporte Gestion de Riesgo de la Flota ({0})", Me.TituloIntervalo)
            Case "RCM"
                TituloMail = String.Format("Reporte Resumen Calidad de Manejo de la Flota ({0})", Me.TituloIntervalo)
            Case "CTR"
                TituloMail = String.Format("Reporte Cuadro de Trabajo de la Flota ({0})", Me.TituloIntervalo)
            Case "FNT"
                TituloMail = String.Format("Reporte Financiera No Transmision de la Flota ({0})", Me.TituloIntervalo)
            Case "RUG"
                TituloMail = String.Format("Reporte Resumen Ubicacion Geografica de la Flota ({0})", Me.TituloIntervalo)
            Case "DUG"
                TituloMail = String.Format("Reporte Detalle Ubicacion Geografica de la Flota ({0})", Me.TituloIntervalo)
            Case "MGR2"
                TituloMail = String.Format("Reporte Gestion de Riesgo2 de la Flota ({0})", Me.TituloIntervalo)
            Case "TRY"
                TituloMail = String.Format("Reportes de Trayectos de la Flota ({0})", Me.TituloIntervalo)
            Case "COM"
                TituloMail = String.Format("Reporte de Comportamiento de la Flota ({0})", Me.TituloIntervalo)
            Case "CON"
                TituloMail = String.Format("Reporte Consolidado de la Flota ({0})", Me.TituloIntervalo)
            Case "VEL"
                TituloMail = String.Format("Reporte de Excesos de Velocidad de la Flota ({0})", Me.TituloIntervalo)
            Case "COM", "HIS"
                TituloMail = String.Format("Reporte Historico de la Flota ({0})", Me.TituloIntervalo)
            Case "ALR"
                TituloMail = String.Format("Reporte de Alertas de la Flota ({0})", Me.TituloIntervalo)
            Case "KMS"
                TituloMail = String.Format("Reporte de Kilometraje de la Flota ({0})", Me.TituloIntervalo)
            Case "ROM"
                TituloMail = String.Format("Reporte Evolucion Mensual de la Flota ({0})", Me.TituloIntervalo)
            Case "HTR"
                TituloMail = String.Format("Reporte de Horas Trabajadas de la Flota ({0})", Me.TituloIntervalo)
            Case "FLO"
                TituloMail = "Reporte de Estado de la Flota"
            Case "CMP"
                TituloMail = String.Format("Reporte Compacto de la Flota ({0})", Me.TituloIntervalo)
            Case "RES"
                TituloMail = String.Format("Reporte de Conteo de Eventos de la Flota ({0})", Me.TituloIntervalo)
            Case "RZN"
                TituloMail = String.Format("Reporte de Zonas de la Flota ({0})", Me.TituloIntervalo)
            Case "SKY"
                TituloMail = String.Format("Reporte de Dispositivos Skywave ({0})", Me.TituloIntervalo)
            Case Else
                TituloMail = String.Format("Reporte de Resumen de la Flota ({0})", Me.TituloIntervalo)
        End Select

        Dim path As String = String.Empty
        Dim body As String = String.Empty

        If IsNothing(Me.IdEntidad) Then
            path = String.Format("{0}\Plantillas\Default.html", Environment.CurrentDirectory)
        Else
            If Me.IdEntidad.Contains("20509959766") And TipoReporte = "EJE" Then
                path = String.Format("{0}\Plantillas\RelsaEjecutivo.html", Environment.CurrentDirectory)
            Else
                path = String.Format("{0}\Plantillas\Default.html", Environment.CurrentDirectory)
            End If
        End If


        Using reader As StreamReader = New StreamReader(path)
            body = reader.ReadToEnd()
        End Using

        body = body.Replace("{tituloEmail}", TituloMail)
        body = body.Replace("{nombreEntidad}", NombreEntidad)
        body = body.Replace("{mensajeEmail}", MensajeEmail)

        Return body

    End Function

End Class