Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports System.Net
Imports System.Text
Imports Autosafe.Desarrollo.Geosys.Entidades
Imports Autosafe.Desarrollo.Geosys.Negocios

Public Class cEjecutivo : Inherits cGeneracion

    Private dsEjecutivo As DataSet
    Private dsVariable As DataSet
    Private Const Reporte As String = "Reporte Ejecutivo de la Flota"

    Dim sCabecera As String
    Dim IdEntidad As String


    Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String)
        MyBase.New(TipoReporte, IdUsuario, Intervalo, Parametros)

    End Sub

    '******************************************************************************
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 201801105
    'PARA AGREGAR NUEVAS COLUMNAS
    'NOMBRE
    'SECUENCIA
    'HORAINICIO
    'HORAFIN
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
    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20200331
    'PARA AGREGAR NUEVA COLUMNA
    'USUARIO
    '******************************************************************************
    'IDENTIDAD
    '******************************************************************************
    Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String, ByVal Nombre As String, ByVal Secuencia As String, ByVal HoraInicio As String, ByVal HoraFin As String, ByVal Usuario As String, ByVal IdEntidad As String)
        MyBase.New(TipoReporte, IdUsuario, Intervalo, Parametros, Nombre, Secuencia, HoraInicio, HoraFin, Usuario, IdEntidad)
    End Sub
    '******************************************************************************

    Public Overrides Sub Generar(Datos As Object)
        Console.WriteLine(Datos.usuarioId.ToString() & ": Obteniendo Datos para el Reporte Ejecutivo")

        Try
            DatoTipo = Datos.tipoReporte.Split(";")
            DatoActivos = Datos.activos.Split(";")
            DatoEmail = Datos.email.Split(";")

            Me.IdEntidad = Datos.entidadId

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
            Dim negocio As New ActivoBL()
            Dim obj As New ActivoEN
            obj.baseDatosId = 25
            obj.usuarioId = 0
            obj.entidadId = Me.IdEntidad

            Dim cantidad As Integer = negocio.ListarVehiculosActivos(obj)

            If cantidad > 0 Then
                GenerarReporte(Datos.usuarioId,
                          Datos.intervalo,
                          Datos.parametros,
                          sh
                          )
            Else
                GuardarLog(Me.Tipo, "SINVEHICULOS")
                Console.WriteLine(String.Format("El usuario :  {0} no cuenta con vehiculos activos", Me.Usuario))

            End If

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
                               ByVal Hoja As ISheet) As String

        Dim err_mensaje As String = ""

        Try

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
            'PARA GENERAR HTML
            '******************************************************************************
            err_mensaje = "GeneraHTML Secuencia = " + Secuencia.ToString()

            Dim pSecuencia As String = Me.Secuencia.ToString()
            Dim pFechaInicial As String = Fecha2ANSI(FechaInicial.ToString(), True) + " " + HoraInicio
            Dim pFechaFinal As String = Fecha2ANSI(FechaFinal.ToString(), True) + " " + HoraFin
            Dim pUrl As String = My.Settings.Url_GeneraHTML + pSecuencia + "|" + pFechaInicial + "|" + pFechaFinal

            Dim respuestaHTML As String = GeneraHTML(pUrl)

            Me.Archivo = respuestaHTML
            '******************************************************************************

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20190206
            'PARA VALIDAR GENERACION DE ARCHIVOS HTML Y PDF
            '******************************************************************************

            Dim extensionHTML As String = ".html"

            If (respuestaHTML.Contains(extensionHTML)) Then

                '******************************************************************************
                'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
                'PARA GENERAR PDF
                '******************************************************************************
                err_mensaje = "GeneraPDF Secuencia = " + Secuencia.ToString()
                pUrl = My.Settings.Url_GeneraPDF + Me.Archivo

                Dim respuestaPDF As String = GeneraPDF(pUrl)
                Me.Archivo = respuestaPDF

                '******************************************************************************

                Dim extensionPDF As String = ".pdf"

                If (respuestaPDF.Contains(extensionPDF)) Then

                    '******************************************************************************
                    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
                    'DESCARGAR REPORTE PDF
                    '******************************************************************************
                    err_mensaje = "DownloadFile Secuencia = " + Secuencia.ToString()
                    Dim urlpdf As String = My.Settings.Url_DownloadPDF + Archivo
                    Console.WriteLine(urlpdf)
                    Try


                        Dim WC As New MyWebClient
                        WC.DownloadFile(urlpdf, Archivo)

                    Catch ex As Exception
                        Console.WriteLine(ex.Message + " - Procedimiento DownloadFile")
                        Throw ex
                    End Try
                    '******************************************************************************

                Else

                    Dim nombreArchivo As String = String.Format("{0}{1}.txt",
                                                             System.AppDomain.CurrentDomain.BaseDirectory,
                                                             Me.Usuario)

                    Dim ArchivoTEXT As String = nombreArchivo

                    Dim fileTEXT As System.IO.StreamWriter
                    fileTEXT = My.Computer.FileSystem.OpenTextFileWriter(ArchivoTEXT, False)
                    fileTEXT.Write(respuestaHTML)
                    fileTEXT.Close()

                    Me.Archivo = nombreArchivo

                End If


            Else

                Dim nombreArchivo As String = String.Format("{0}{1}.txt",
                                                         System.AppDomain.CurrentDomain.BaseDirectory,
                                                         Me.Usuario)

                Dim ArchivoTEXT As String = nombreArchivo

                Dim fileTEXT As System.IO.StreamWriter
                fileTEXT = My.Computer.FileSystem.OpenTextFileWriter(ArchivoTEXT, False)
                fileTEXT.Write(respuestaHTML)
                fileTEXT.Close()

                Me.Archivo = nombreArchivo

            End If

            '******************************************************************************





            Try
                EnviarMail(Me.Email,
                       Me.Archivo,
                       "Adjunto Email con el Reporte Solicitado",
                        wk.GetSheet(0),
                        Reporte,
                        "EJE",
                        Me.IdUsuario)
            Catch ex As Exception
                ErroresEnvio &= "ERR_MAIL"
            End Try



            dsEjecutivo = Nothing
        Catch ex As Exception
            Console.WriteLine(ex.Message + " - Procedimiento GenerarReporte - " + err_mensaje)
            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180920
            'PARA LLAMAR AL PROCESO GUARDARLOG UNA VEZ SE HAYA GENERADO UN ERROR
            '******************************************************************************
            GuardarLog(Tipo, ex.Message)
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



End Class

