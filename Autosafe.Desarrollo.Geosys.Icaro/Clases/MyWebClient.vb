Imports System.Net

Public Class MyWebClient : Inherits WebClient

    Protected Overrides Function GetWebRequest(ByVal uri As Uri) As WebRequest
        Dim w As WebRequest = MyBase.GetWebRequest(uri)
        w.Timeout = 300 * 60 * 1000
        Return w
    End Function
End Class