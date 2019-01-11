Imports xNet

Public Class Req

    Private URI As String
    Public Event RequestComplete(sender As Object, Result As String)

    Public Sub Requ(ByVal name As String)
        Try
           
            Using request As New HttpRequest
                request.Proxy = Nothing
                request.UserAgent = Http.ChromeUserAgent
                request.KeepAlive = True
                request.Cookies = New CookieDictionary(False)
                request.IgnoreProtocolErrors = True
                request.AllowAutoRedirect = False

                Dim rep As HttpResponse = request.Get(Me.URI & name)
                Dim Response As String = ""

                If rep.HasRedirect Then
                    Response = request.Get(rep.RedirectAddress.ToString(), Nothing).ToString()
                Else
                    Response = rep.ToString()
                End If

                RaiseEvent RequestComplete(Me, Response)

            End Using
        Catch ex As Exception
            RaiseEvent RequestComplete(Me, ex.Message)
        End Try
    End Sub

    Public Sub SetURI(ByVal URI As String)
        Me.URI = URI
    End Sub
End Class
