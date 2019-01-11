Imports System.Net
Imports System.Text
Imports System.IO
Imports System.ComponentModel
Imports System
Imports System.Text.RegularExpressions
Imports System.Threading

Public Class Form1

    '=========================================================================
    '=>YOUTUBE : >> SUBSCRIBE FOR MORE :https://www.youtube.com/c/XSLAYERTN  |
    '=>FACEBOOK : >> https://www.facebook.com/XSLAYER404/                    |
    '=>INSTAGRAM : >> https://www.instagram.com/ih3_b                        |
    '=>GITHUB : >> https://github.com/X-SLAYER                               |
    '=========================================================================

    Public Sub New()
        InitializeComponent()
        Control.CheckForIllegalCrossThreadCalls = False
        txtLocal.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    End Sub

    Dim ITEM As New Dictionary(Of Req, ListViewItem)
    Dim LV As New Dictionary(Of WebClient, ListViewItem)

    Public Sub GET_INFO(ByVal URL As String)
        On Error Resume Next
        Dim itm As New ListViewItem
        itm.UseItemStyleForSubItems = False
        itm.Text = "Checking URL ..."
        itm.SubItems.Add("Getting Data...").ForeColor = Color.Blue
        itm.SubItems.Add("Getting Data...").ForeColor = Color.Blue
        itm.ImageIndex = 0
        LvDownloads.Items.Add(itm)
        Dim X As New Req
        X.SetURI("https://www.easy-youtube-mp3.com/download.php?v=")
        AddHandler X.RequestComplete, AddressOf X_ScannerCompleted
        ITEM.Add(X, itm)
        X.Requ(URL.Split("=")(1))
    End Sub

    Private Sub X_ScannerCompleted(sender As Object, e As String)
        Try
            LvDownloads.Items(ITEM(sender).Index).Text = Regex.Match(e, "<p><b>Title:<\/b>(.*?)<\/p>").Groups(1).Value
            LvDownloads.Items(ITEM(sender).Index).Tag = Regex.Match(e, " <a class=""btn btn-lg btn-success"" href=""(.*?)"" target=""_blank"" download=""file.mp3"">").Groups(1).Value
            LvDownloads.Items(ITEM(sender).Index).SubItems(1).ForeColor = Color.Blue
            LvDownloads.Items(ITEM(sender).Index).SubItems(2).Text = "Downloading ..."
            Using X As New WebClient
                AddHandler X.DownloadFileCompleted, AddressOf FINISHED
                AddHandler X.DownloadProgressChanged, AddressOf Progress
                X.DownloadFileTaskAsync(New Uri(LvDownloads.Items(ITEM(sender).Index).Tag), txtLocal.Text & "\" & ModifyName(LvDownloads.Items(ITEM(sender).Index).Text) & ".mp3")
                LV.Add(X, LvDownloads.Items(ITEM(sender).Index))
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub Progress(sender As Object, e As DownloadProgressChangedEventArgs)
        On Error Resume Next
        LvDownloads.Items(LV(sender).Index).SubItems(1).Text = F(e.BytesReceived) & " / " & F(e.TotalBytesToReceive)
    End Sub
    Private Sub FINISHED(sender As Object, e As AsyncCompletedEventArgs)
        If e.Cancelled = True Then
            LvDownloads.Items(LV(sender).Index).SubItems(2).Text = "Cancelled..."
            LvDownloads.Items(LV(sender).Index).SubItems(1).ForeColor = Color.Purple
            LvDownloads.Items(LV(sender).Index).SubItems(2).ForeColor = Color.Purple
        ElseIf Not e.Error Is Nothing Then
            LvDownloads.Items(LV(sender).Index).SubItems(2).Text = e.Error.Message
            LvDownloads.Items(LV(sender).Index).SubItems(1).ForeColor = Color.Red
            LvDownloads.Items(LV(sender).Index).SubItems(2).ForeColor = Color.Red
        ElseIf Not e.UserState Is Nothing Then
            LvDownloads.Items(LV(sender).Index).SubItems(2).Text = "Successfully..."
            LvDownloads.Items(LV(sender).Index).SubItems(1).ForeColor = Color.Green
            LvDownloads.Items(LV(sender).Index).SubItems(2).ForeColor = Color.Green
        End If
    End Sub

    Public Sub START(ByVal str As String)
        Dim thread As New Thread(New ThreadStart(Sub() GET_INFO(str)))
        thread.Start()
    End Sub

#Region "Used Functions"

    Private Sub btnBrowser_Click(sender As Object, e As EventArgs) Handles btnBrowser.Click ' Browse Folder
        Dim I As New FolderBrowserDialog
        I.ShowNewFolderButton = True
        I.RootFolder = Environment.SpecialFolder.Desktop
        I.Description = "Save Audio File"
        If I.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtLocal.Text = I.SelectedPath
        End If
    End Sub

    Private Function F(ByVal type As Double) As String 'File Size
        Dim types As String() = {"B", "KB", "MB", "GB"}
        Dim typees As Double = type
        Dim CSA As Integer = 0
        While typees >= 1024 AndAlso CSA + 1 < types.Length
            CSA += 1
            typees = typees / 1024
        End While
        Return [String].Format("{0:0.##} {1}", typees, types(CSA))
    End Function

    Private Function ModifyName(ByVal URL As String) As String 'Caracters That Windowse Cant Accepte Will Be Removed
        Return URL.Replace("\", Nothing).Replace("/", Nothing).Replace(":", Nothing).Replace("*", Nothing).Replace("?", Nothing).Replace("""", Nothing).Replace("<", Nothing).Replace(">", Nothing).Replace("|", Nothing)
    End Function

#End Region

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not (txturl.Text = String.Empty) Then
            START(txturl.Text)
            txturl.Text = Nothing
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://www.youtube.com/c/XSLAYERTN")
    End Sub
End Class
