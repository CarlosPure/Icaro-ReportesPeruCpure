Imports System.Data.SqlClient
Imports NPOI.SS.UserModel
Imports System.Math
Imports NPOI.HSSF.UserModel
Imports System.IO
Imports System.Net
Imports System.Text
Imports Autosafe.Desarrollo.Geosys.Negocios


Public Class cCredenciales : Inherits cGeneracion

    Private dsEjecutivo As DataSet
    Private dsVariable As DataSet
    Private Const Reporte As String = "Reporte Ejecutivo de la Flota"

    Dim sCabecera As String
    'Dim IdEntidad As String


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
    Sub New(ByVal TipoReporte As String, ByVal IdUsuario As Integer, ByVal Intervalo As String, ByVal Parametros As String, ByVal Nombre As String, ByVal Secuencia As String, ByVal HoraInicio As String, ByVal HoraFin As String, ByVal Usuario As String, ByVal IdEntidad As String, ByVal Evento As String)
        MyBase.New(TipoReporte, IdUsuario, Intervalo, Parametros, Nombre, Secuencia, HoraInicio, HoraFin, Usuario, IdEntidad, Evento)
    End Sub
    '******************************************************************************



    Public Overrides Sub Generar(Datos As Object)
        Console.WriteLine(Datos.usuarioId.ToString() & ": Obteniendo Datos para el Reporte Credenciales")

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
            'PARA GUARDAR EL NOMBRE DE ARCHIVO
            'PARA GUARDAR EL IDENTIDAD
            '******************************************************************************
            Me.Email = Datos.email
            'Me.Archivo = String.Format("{0}_{1}.pdf", Me.Tipo, DBS25.getUsuarioxID(IdUsuario))
            Me.Archivo = String.Format("{0}_{1}.pdf", Me.Tipo, Me.IdUsuario)
            'Me.IdEntidad = DBS25.getIdEntidadxSecuencia(Me.Secuencia)
            'Me.Usuario = DBS25.getUsuarioxID(Datos.IdUsuario)
            '******************************************************************************

            Dim correo As String

            'For Each correo In DatoEmail

            GenerarReporte(Datos.usuarioId,
                          Datos.intervalo,
                          Datos.parametros,
                          sh,
                          "aaaaaa"
                          )



            'Next

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
                               ByVal Hoja As ISheet,
                               ByVal Correo As String
                               ) As String

        Dim err_mensaje As String = ""

        Try

            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
            'PARA GENERAR HTML
            '******************************************************************************
            'err_mensaje = "GeneraHTML Secuencia = " + Secuencia.ToString()

            'Dim pSecuencia As String = Me.Secuencia.ToString()
            'Dim pFechaInicial As String = Fecha2ANSI(FechaInicial.ToString(), True) + " " + HoraInicio
            'Dim pFechaFinal As String = Fecha2ANSI(FechaFinal.ToString(), True) + " " + HoraFin
            'Dim pUrl As String = My.Settings.Url_GeneraHTML + pSecuencia + "|" + pFechaInicial + "|" + pFechaFinal

            'Dim respuestaHTML As String = GeneraHTML(pUrl)

            'Me.Archivo = respuestaHTML
            ''******************************************************************************

            ''******************************************************************************
            ''TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20190206
            ''PARA VALIDAR GENERACION DE ARCHIVOS HTML Y PDF
            ''******************************************************************************

            'Dim extensionHTML As String = ".html"

            'If (respuestaHTML.Contains(extensionHTML)) Then

            '    '******************************************************************************
            '    'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
            '    'PARA GENERAR PDF
            '    '******************************************************************************
            '    err_mensaje = "GeneraPDF Secuencia = " + Secuencia.ToString()
            '    pUrl = My.Settings.Url_GeneraPDF + Me.Archivo

            '    Dim respuestaPDF As String = GeneraPDF(pUrl)
            '    Me.Archivo = respuestaPDF

            '    '******************************************************************************

            '    Dim extensionPDF As String = ".pdf"

            '    If (respuestaPDF.Contains(extensionPDF)) Then

            '        '******************************************************************************
            '        'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180912
            '        'DESCARGAR REPORTE PDF
            '        '******************************************************************************
            '        err_mensaje = "DownloadFile Secuencia = " + Secuencia.ToString()
            '        Dim urlpdf As String = My.Settings.Url_DownloadPDF + Archivo
            '        Console.WriteLine(urlpdf)
            '        Try


            '            Dim WC As New MyWebClient
            '            WC.DownloadFile(urlpdf, Archivo)

            '        Catch ex As Exception
            '            Console.WriteLine(ex.Message + " - Procedimiento DownloadFile")
            '            Throw ex
            '        End Try
            '        '******************************************************************************

            '    Else

            '        Dim nombreArchivo As String = String.Format("{0}{1}.txt",
            '                                                 System.AppDomain.CurrentDomain.BaseDirectory,
            '                                                 Me.Usuario)

            '        Dim ArchivoTEXT As String = nombreArchivo

            '        Dim fileTEXT As System.IO.StreamWriter
            '        fileTEXT = My.Computer.FileSystem.OpenTextFileWriter(ArchivoTEXT, False)
            '        fileTEXT.Write(respuestaHTML)
            '        fileTEXT.Close()

            '        Me.Archivo = nombreArchivo

            '    End If


            'Else

            '    Dim nombreArchivo As String = String.Format("{0}{1}.txt",
            '                                             System.AppDomain.CurrentDomain.BaseDirectory,
            '                                             Me.Usuario)

            '    Dim ArchivoTEXT As String = nombreArchivo

            '    Dim fileTEXT As System.IO.StreamWriter
            '    fileTEXT = My.Computer.FileSystem.OpenTextFileWriter(ArchivoTEXT, False)
            '    fileTEXT.Write(respuestaHTML)
            '    fileTEXT.Close()

            '    Me.Archivo = nombreArchivo

            'End If

            Dim AccessToken As String = My.Settings.TokenEmBlue
            Dim asunto As String = "Bienvenido a la familia Hunter - " & Me.Usuario & " -  Monitoreo GPS"
            Dim negocio As New ReporteProgramadoBL()

            If DatoEmail.Length = 0 Or Me.Nombre = "" Or Me.Usuario = "" Or Me.Parametros = "" Or My.Settings.UrlApiHunter = "" Or AccessToken = "" Or Me.Evento = "" Or asunto = "" Then
                GuardarLog(Me.Tipo, "Error")
            Else
                Dim resultado As String = negocio.enviar(DatoEmail, Me.Nombre, Me.Usuario, Me.Parametros, My.Settings.UrlApiHunter, AccessToken, Evento, asunto)

                Console.WriteLine(resultado)

                If resultado.Contains("success") Then
                    Console.WriteLine("Credenciales enviadas correctamente para el usuario: " & Me.IdUsuario & " - " & Me.Usuario)
                    GuardarLog(Me.Tipo, "")
                Else
                    Console.WriteLine("Error al enviar credenciales para el usuario: " & Me.IdUsuario & " - " & Me.Usuario)
                    GuardarLog(Me.Tipo, "Error")
                End If

            End If

            'Dim httpWebRequest = CType(WebRequest.Create(My.Settings.Url_EmBlueOne), HttpWebRequest)

            'httpWebRequest.ContentType = "application/json"
            'httpWebRequest.PreAuthenticate = True
            'httpWebRequest.Headers.Add("Authorization", "" + AccessToken)
            'httpWebRequest.Accept = "application/json"
            'httpWebRequest.Method = "POST"


            'Dim streamWriter1 = New StreamWriter(httpWebRequest.GetRequestStream())

            'For Each streamWriter As Object In streamWriter1

            'Next

            'Dim sb As New StringBuilder
            'sb.Append("{")
            'sb.Append("""email"": """ & Correo & """,")
            ''sb.Append(""" eventName"": """ & Me.Email & """,")
            'sb.Append("""eventName"": ""evento_hmmovil_credencial"",")
            ''sb.Append("""eventName"": """ & """evento_hmmovil_credencial,""")
            ''sb.Append(""" eventName: """ & """evento_hmmovil_credencial,""")
            'sb.Append("""attributes"": " & "{")
            'sb.Append("""nombre"": """ & Me.Nombre & """,")
            ''sb.Append("""nombre:""" & """" & Me.Nombre & ",""")
            'sb.Append("""usuario"": """ & Me.Usuario & """,")
            ''sb.Append("""usuario:""" & """" & Me.Usuario & ",""")
            ''sb.Append("""clave:""" & """" & Me.Parametros & """")
            'sb.Append("""clave"": """ & Me.Parametros & """")
            'sb.Append("}")
            'sb.Append("}")

            'streamWriter1.Write(sb.ToString())



            'Dim httpResponse as HttpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            'Dim httpResponse1 = CType(httpWebRequest.GetResponse(), HttpWebResponse)

            'Dim streamReader1 = New StreamReader(httpResponse1.GetResponseStream())

            'Dim result As Object = streamReader1.ReadToEnd()
            'Console.WriteLine(result)

            'using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            '{
            '    var result = streamReader.ReadToEnd();
            '    Console.WriteLine(result);
            '}

            '******************************************************************************


            ''Dim postData As String = "referer=http%3A%2F%2Fforums.zybez.net%2Findex.php%3Fapp%3Dcore%26module%3Dglobal%26section%3Dlogin&username=" & TextBox1.Text & "&password=" & TextBox2.Text & "&rememberMe=1"
            ''Dim tempCookies As New CookieContainer
            'Dim encoding As New UTF8Encoding
            ''Dim byteData As Byte() = encoding.GetBytes(postData)

            'Dim postReq As HttpWebRequest = DirectCast(WebRequest.Create(My.Settings.Url_EmBlueOne), HttpWebRequest)
            'postReq.Method = "POST"
            'postReq.KeepAlive = True
            ''postReq.CookieContainer = tempCookies
            'postReq.ContentType = "application/json"
            'postReq.Referer = "http://forums.zybez.net/index.php?app=core&module=global§ion=login&do=process"
            'postReq.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2.3) Gecko/20100401 Firefox/4.0 (.NET CLR 3.5.30729)"
            'postReq.ContentLength = byteData.Length

            'Dim postreqstream As Stream = postReq.GetRequestStream()
            'postreqstream.Write(byteData, 0, byteData.Length)
            'postreqstream.Close()
            'Dim postresponse As HttpWebResponse

            'postresponse = DirectCast(postReq.GetResponse(), HttpWebResponse)
            ''tempCookies.Add(postresponse.Cookies)
            ''logincookie = tempCookies
            'Dim postreqreader As New StreamReader(postresponse.GetResponseStream())

            'Dim thepage As String = postreqreader.ReadToEnd

            'RichTextBox1.Text = thepage




            'Dim request As HttpWebRequest = DirectCast(WebRequest.Create(My.Settings.Url_EmBlueOne), HttpWebRequest)
            ''request.CookieContainer = logincookie
            'request.ContentType = "application/json"
            'request.PreAuthenticate = True
            'request.Headers.Add("Authorization", "" + AccessToken)
            'request.Accept = "application/json"
            'request.Method = "POST"

            'Dim streamWriter1 = New StreamWriter(request.GetRequestStream())
            'streamWriter1.Write(sb.ToString())

            'Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
            'Dim reader As New StreamReader(response.GetResponseStream())
            'Dim line As String = reader.ReadLine()

            'While line IsNot Nothing
            '    'lineAction(line)
            '    line = reader.ReadLine()

            '    'Dim result As Object = line.()
            '    Console.WriteLine(line)

            'End While





            'Try
            '    EnviarMail(Me.Email,
            '           Me.Archivo,
            '           "Adjunto Email con el Reporte Solicitado",
            '            wk.GetSheet(0),
            '            Reporte,
            '            "EJE",
            '            Me.IdUsuario)
            'Catch ex As Exception
            '    ErroresEnvio &= "ERR_MAIL"
            'End Try



            'dsEjecutivo = Nothing
        Catch ex As Exception
            Console.WriteLine(ex.Message + " - Procedimiento GenerarReporte - " + err_mensaje)
            '******************************************************************************
            'TODO CAMBIOS REALIZADOS POR VICTOR.VEGA 20180920
            'PARA LLAMAR AL PROCESO GUARDARLOG UNA VEZ SE HAYA GENERADO UN ERROR
            '******************************************************************************
            GuardarLog(Tipo, "Error")
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

