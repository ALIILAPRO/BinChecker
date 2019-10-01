Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.Threading
Public Class Form1
    Public Sub New()

        InitializeComponent()
        Control.CheckForIllegalCrossThreadCalls = False

    End Sub
    Private cookies As CookieContainer = New CookieContainer
    Dim RG As String = """scheme"":""(.*?)""|""type"":""(.*?)""|""bank"":{""name"":""(.*?)"",|""brand"":""(.*?)""|prepaid"":(.*?),|""number"":{""length"":(.*?),|"",""name"":""(.*?)"",""numeric"":"""
    Dim X As String


#Region "Get BIN"
    Function aliilapro(ByVal str As String, ByVal str2 As String, ByVal num As Integer) As String
        Dim lol As Match = Regex.Match(str, str2, RegexOptions.IgnoreCase)
        Return lol.Groups(num).Value
    End Function

    Public Sub GETBIN()
        Try
            Dim request As HttpWebRequest = DirectCast(WebRequest.Create("https://lookup.binlist.net/" & txtbin.Text), HttpWebRequest)
            request.Method = "GET"
            request.CookieContainer = Me.cookies
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:47.0) Gecko/20100101 Firefox/47.0"
            request.ContentType = "application/x-www-form-urlencoded"
            request.Proxy = Nothing
            Dim reader As New StreamReader(request.GetResponse.GetResponseStream, Encoding.UTF8)
            Dim input As String = reader.ReadToEnd
            Dim IT As New ListViewItem
            IT.Text = txtbin.Text & "|" & aliilapro(input, RG.Split("|")(0), 1)
            IT.SubItems.Add(aliilapro(input, RG.Split("|")(1), 1))
            IT.SubItems.Add(aliilapro(input, RG.Split("|")(2), 1))
            IT.SubItems.Add(aliilapro(input, RG.Split("|")(3), 1))
            IT.SubItems.Add(aliilapro(input, RG.Split("|")(4), 1).Replace("false", "No").Replace("true", "yes"))
            IT.SubItems.Add(aliilapro(input, RG.Split("|")(5), 1))
            IT.SubItems.Add(aliilapro(input, RG.Split("|")(6), 1))
            list_v.Items.Add(IT)
            Button1.Enabled = True
            Button2.Enabled = True
        Catch ex As Exception
            Button1.Enabled = True
            Button2.Enabled = True
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub
#End Region

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If txtbin.Text = String.Empty Then : Exit Sub
        End If
        Button1.Enabled = False
        Button2.Enabled = False
        Dim IHEB As New Thread(AddressOf GETBIN)
        IHEB.IsBackground = True
        IHEB.Start()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If list_v.Items.Count = Nothing Then
                Exit Sub
            End If
            Dim ID As New SaveFileDialog
            ID.Title = "Save List Bin"
            ID.Filter = "txt Files(*.txt)|*.txt"
            ID.FileName = "Bin Number"
            If ID.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim I As New StreamWriter(ID.FileName)
                For Each x As ListViewItem In list_v.Items
                    Dim StrLn As String = ""
                    For j = 0 To x.SubItems.Count - 1
                        StrLn += list_v.Text + x.SubItems(j).Text + "|"
                    Next
                    I.WriteLine(StrLn)
                Next
                I.Close()
                MsgBox("File Save In : " & ID.FileName, MsgBoxStyle.Information, "Info")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
