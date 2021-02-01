Imports System.IO
Imports System.Net
Imports System.Threading
Imports System.Text
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Text.RegularExpressions

Public Class Form1
    ''' <summary>
    ''' -------------------------------------------
    ''' |        Instagram : @404.erroz           |
    ''' -------------------------------------------
    ''' |        Github : itserrozz               |
    ''' -------------------------------------------
    ''' </summary>
    Public error_unfollow As Integer
    Dim mid As String = "X-Na2gABAAFmYl4pdmNhP37k6H4Y"
    Dim ig_did As String = "0BF2B79A-D7AA-4013-B2D1-2BE0A449E9CB"
    Dim session_id As String
    Dim unfollow As Integer
    Dim total_unfollow As Integer
    Dim count_following As Integer
    Dim cookieContainer As CookieContainer = New CookieContainer()
    Private a As Boolean
    Private b As Integer
    Private c As Integer
    Private Sub Label1_MouseDown(sender As Object, e As MouseEventArgs) Handles Label1.MouseDown
        If e.Button = MouseButtons.Left Then
            Me.a = True
            Me.b = MousePosition.X - Location.X
            Me.c = MousePosition.Y - Location.Y
        End If
    End Sub
    Private Sub Label1_MouseMove(sender As Object, e As MouseEventArgs) Handles Label1.MouseMove
        If Me.a Then
            Dim location As Point = New Point(MousePosition.X - b, MousePosition.Y - c)
            Me.Location = location
        End If
    End Sub
    Private Sub Label1_MouseUp(sender As Object, e As MouseEventArgs) Handles Label1.MouseUp
        Me.a = False
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim a As Thread = New Thread(AddressOf login)
        a.Start()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        Dim run As Thread = New Thread(AddressOf Get_cookie) : run.Start()
        Me.Width = 108
        MsgBox("Welcome.." + vbNewLine + "My Instagram : @404.erroz " + vbNewLine + "Source Code in Github : itserrozz", MsgBoxStyle.Information, "Info Codder")
    End Sub
    Sub login()
        Try
            Dim text As String = "username=" + TextBox1.Text + "&enc_password=#PWD_INSTAGRAM_BROWSER:0:1611320939:" + TextBox2.Text + "&queryParams={}&optIntoOneTap=false"
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(text)
            Dim httpWebRequest As HttpWebRequest = CType(WebRequest.Create("https://www.instagram.com/accounts/login/ajax/"), HttpWebRequest)
            httpWebRequest.Method = "POST"
            httpWebRequest.Host = "www.instagram.com"
            httpWebRequest.Headers.Add("X-CSRFToken", "missing")
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:84.0) Gecko/20100101 Firefox/84.0"
            httpWebRequest.ContentType = "application/x-www-form-urlencoded"
            httpWebRequest.Headers.Add("Cookie", "ig_did=" + ig_did + "; mid=" + mid + "; ig_nrcb=1; csrftoken=missing")
            httpWebRequest.Headers.Add("Accept-Language", "ar;q=1, en;q=0.9")
            httpWebRequest.KeepAlive = True
            httpWebRequest.Proxy = Nothing
            httpWebRequest.ContentLength = CLng(bytes.Length)
            httpWebRequest.CookieContainer = cookieContainer
            Dim requestStream As Stream = httpWebRequest.GetRequestStream()
            requestStream.Write(bytes, 0, bytes.Length)
            requestStream.Close()
            Dim httpWebResponse As HttpWebResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)
            Dim streamReader As StreamReader = New StreamReader(httpWebResponse.GetResponseStream())
            Dim response As String = streamReader.ReadToEnd()
            Dim cookiess As String = cookieContainer.GetCookieHeader(httpWebRequest.RequestUri) + ";"
            If response.Contains("userId") Then
                session_id = Regex.Match(cookiess, "sessionid=(.*?);").Groups(1).Value
                If session_id = Nothing Then
                    MsgBox("Somthing Wrong to login!", MsgBoxStyle.Critical)
                Else
                    Button1.Text = "Stop"
                    Dim a As Thread = New Thread(AddressOf get_following) : a.Start()
                End If
            ElseIf response.Contains("{""user"": true, ""authenticated"": false, ""status"": ""ok""}") Then
                MsgBox("Incorrect Password! , Try again.", MsgBoxStyle.Critical, "incorrect Password")
            ElseIf response.Contains("{""user"": false, ""authenticated"": false, ""status"": ""ok""}") Then
                MsgBox("Incorrect Username! , make sure to username.", MsgBoxStyle.Critical, "incorrect Username")
            Else
                MsgBox("Somthing Wrong to login!", MsgBoxStyle.Critical)
            End If
        Catch ex As WebException
            ProjectData.SetProjectError(ex)
            Dim ex2 As WebException = ex
            Dim text_catch As String = (String.Concat(New StreamReader(ex2.Response.GetResponseStream).ReadToEnd))
            If text_catch.Contains("chall") Then
                MsgBox("Secure , Sorry !", MsgBoxStyle.Critical, "Secure")
            End If
        End Try
    End Sub
    Sub Get_cookie()
        Dim random As New Random()
        Dim cookieContainer As CookieContainer = New CookieContainer()
        Try
            Dim httpWebRequest As HttpWebRequest = CType(WebRequest.Create("https://www.instagram.com/accounts/login/"), HttpWebRequest)
            httpWebRequest.CookieContainer = cookieContainer
            httpWebRequest.UserAgent = "Mozilla/5.0 (Linux; Android 6.0.1; SM-G920V Build/MMB29K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.98 Mobile Safari/537.36"
            httpWebRequest.Proxy = Nothing
            Dim input As String = New StreamReader(httpWebRequest.GetResponse().GetResponseStream()).ReadToEnd()
            Dim cookiess As String = cookieContainer.GetCookieHeader(httpWebRequest.RequestUri)
            ig_did = Regex.Match(cookiess, "ig_did=(.*?);").Groups(1).Value
            mid = Regex.Match(cookiess, "mid=(.*?);").Groups(1).Value
        Catch ex As Exception
        End Try
    End Sub
    Sub get_following()
        Try
            Dim httpWebRequest As HttpWebRequest = CType(WebRequest.Create("https://www.instagram.com/graphql/query/?query_hash=3dec7e2c57367ef3da3d987d89f9dbc8&variables={""id"":"""", ""include_reel"":  true,""fetch_mutual"":false,""first"":9000000000}"), HttpWebRequest)
            httpWebRequest.UserAgent = "Mozilla/5.0 (Linux; Android 6.0.1; SM-G920V Build/MMB29K) AppleWebKit/537.36 (KHTML, Like Gecko) Chrome/52.0.2743.98 Mobile Safari/537.36"
            httpWebRequest.Proxy = Nothing
            httpWebRequest.Headers.Add("Cookie", "ig_did=" + ig_did + "; mid=" + mid + "; ig_nrcb=1; csrftoken=missing; ds_user_id=31382746131; datr=NeoJYMybfRelhI-TkHYiLiHG; sessionid=" + session_id + ";")
            Dim json As String = New StreamReader(httpWebRequest.GetResponse().GetResponseStream()).ReadToEnd()
            count_following = Regex.Match(json, "{""count"":(.*?),").Groups(1).Value
            Dim regex_users As MatchCollection = Regex.Matches(json, "{""node"":{""id"":""(.*?)"",")
            For Each obj As Object In regex_users
                Dim mm As Match = CType(obj, Match)
                Dim id_following As String = mm.Groups(1).Value.ToString()
                If Not IdsList.Text.Contains(id_following) Then
                    IdsList.Items.Add(id_following)
                End If
            Next
            Dim a As Thread = New Thread(AddressOf unfollowing)
            a.Start()
        Catch ex As Exception
        End Try
    End Sub
    Sub unfollowing()
        For Each userid As String In IdsList.Items
            If Not userid = "7897627550" Then
                Try
                    Dim httpWebRequest As HttpWebRequest = CType(WebRequest.Create("https://www.instagram.com/web/friendships/" + userid + "/unfollow/"), HttpWebRequest)
                    httpWebRequest.Method = "POST"
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Linux; Android 6.0.1; SM-G920V Build/MMB29K) AppleWebKit/537.36 (KHTML, Like Gecko) Chrome/52.0.2743.98 Mobile Safari/537.36"
                    httpWebRequest.Proxy = Nothing
                    httpWebRequest.Headers.Add("X-CSRFToken", "missing")
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded"
                    httpWebRequest.Headers.Add("Cookie", "ig_did=" + ig_did + "; mid=" + mid + "; ig_nrcb=1; csrftoken=missing; ds_user_id=31382746131; datr=NeoJYMybfRelhI-TkHYiLiHG; sessionid=" + session_id + ";")
                    Dim json As String = New StreamReader(httpWebRequest.GetResponse().GetResponseStream()).ReadToEnd()
                    If json.Contains("ok") Then
                        unfollow += 1 : total_unfollow += 1
                        Label4.Text = "UnFollow : " + total_unfollow.ToString
                    Else
                        My.Computer.FileSystem.WriteAllText("response-else.txt", json + vbCrLf, True)
                    End If
                Catch ex As WebException
                    ProjectData.SetProjectError(ex)
                    Dim ex2 As WebException = ex
                    Dim text_catch As String = (String.Concat(New StreamReader(ex2.Response.GetResponseStream).ReadToEnd))
                    If text_catch.Contains("Please wait a few minutes before you try again.") Then
                        error_unfollow += 1
                        Label3.Text = "Error : " + error_unfollow.ToString
                        Thread.Sleep(6000)
                    ElseIf text_catch.Contains("""spam"": true,") Then
                        Thread.Sleep(100000)
                        Label3.Text = "Wait .. 100 sec"

                    End If
                End Try
                Thread.Sleep(Conversions.ToInteger(TextBox4.Text & "000"))
            End If
        Next
        If IdsList.Items.Contains("7897627550") Then
            count_following = count_following - 1
        End If
        If unfollow = count_following Then
            Button1.Text = "Start"
            MsgBox("Finished! Total UnFollow : " & total_unfollow.ToString + vbCrLf & "My instagram : @404.erroz", MsgBoxStyle.Information)
            Exit Sub
        ElseIf unfollow < count_following Then
            unfollow = 0
            IdsList.Items.Clear()
            Dim a As Thread = New Thread(AddressOf get_following) : a.Start()
        End If

    End Sub
    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        ProjectData.EndApp()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Dim a As Thread = New Thread(AddressOf get_following) : a.Start()
    End Sub
End Class