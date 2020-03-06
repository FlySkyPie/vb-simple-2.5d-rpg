Public Class Form_MapEditor
    Dim momlact As String = Application.StartupPath
    Dim D_P As New Point
    Dim GUI As Bitmap
    Dim GUI2 As Bitmap
    Dim GUI3(4) As Bitmap
    Dim GUI4 As Bitmap

    Dim sm As Integer '模式(圖層)

    Dim MapItem(100) As Obj_MapItem
    Dim map_tmp As Obj_map

    Dim mini_block(100) As Color '繪圖用
    Private Sub Form_MapEditor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For i = 0 To 99
            mini_block(i) = New Color
            Dim a = Format(i, "000")
            If My.Computer.FileSystem.FileExists(momlact & "\Tilesets\bg" & a & ".png") Then
                '取得顏色
                Dim tmp_map As Bitmap
                tmp_map = New Bitmap(Image.FromFile(momlact & "\Tilesets\bg" & a & ".png"))
                mini_block(i) = tmp_map.GetPixel(tmp_map.Width / 2, tmp_map.Height / 2)

            Else
                mini_block(i) = Color.FromArgb(255, 255, 255)
            End If
        Next

        ''地圖數據初始化(建立檔案用)
        For i = 0 To 99
            MapItem(i) = New Obj_MapItem
            '    MapItem(i).Name = "未命名"
            '    MapItem(i).Pass = False
            '    MapItem(i).CenterPoint = New Point(16, 8)
        Next

        '載入地圖物件庫
        Dim h As String = ""
        Dim r = My.Computer.FileSystem.OpenTextFileReader(momlact & "\Data\MapItemData.ini")
        Dim gg() As String
        Dim U As Integer
        For U = 0 To 99
            h = r.ReadLine()
            gg = Split(h)

            MapItem(U).Name = gg(1)
            MapItem(U).Pass = Val(gg(2))
            MapItem(U).CenterPoint = New Point(Val(gg(3)), Val(gg(4)))
        Next
        r.Close()

        '地圖變數初始化
        map_tmp = New Obj_map
        For k = 0 To 3
            For i = 0 To 99
                For j = 0 To 99
                    map_tmp.Parameter(k, i, j) = 0
                Next
            Next
        Next

        '表單物件初始化
        Label1.Location = New Point(500, Me.Height - 65)
        Label2.Location = New Point(600, Me.Height - 65)
        PictureBox1.Size = New Size(700, 700)
        GroupBox1.Location = New Point(750, 10)
        GroupBox2.Location = New Point(750 + GroupBox1.Width + 15, 10)
        GroupBox2.Size = New Size(Me.Width - (750 + GroupBox1.Width + 15) - 20, Me.Height - (165 + 45))
        GroupBox3.Location = New Point(750, 10 + GroupBox1.Height + 10)
        Panel1.Width = GroupBox2.Width - 20
        Panel1.Height = GroupBox2.Height - 200
        Button2.Left = GroupBox2.Width - (Button2.Width + 10)
        '繪製Form背景參考圖
        GUI = New Bitmap(Me.Width - 10, Me.Height - 36)
        Dim g As Graphics
        g = Graphics.FromImage(GUI)
        g.DrawImageUnscaled(New Bitmap(Image.FromFile("MapEditor/001.png")), New Point(0, Me.Height - 336))
        g.DrawImageUnscaled(New Bitmap(Image.FromFile("MapEditor/002.png")), New Point(Me.Width - 220, Me.Height - 195))
        Me.BackgroundImage = GUI
        '繪製網格(載入後不再更動)
        GUI2 = New Bitmap(PictureBox1.Width, PictureBox1.Height)
        Dim g2 As Graphics
        Dim drawBrush As New SolidBrush(Color.FromArgb(128, 128, 128))
        Dim drawpen As New Pen(drawBrush, 1)
        g2 = Graphics.FromImage(GUI2)
        For i = 1 To 99
            g2.DrawLine(drawpen, New Point(7 * i, 0), New Point(7 * i, 700))
            g2.DrawLine(drawpen, New Point(0, 7 * i), New Point(700, 7 * i))
        Next
        '繪製底圖
        For i = 0 To 3
            GUI3(i) = New Bitmap(PictureBox1.Width, PictureBox1.Height)
        Next

        Dim g3 As Graphics
        g3 = Graphics.FromImage(GUI3(0))
        g3.Clear(mini_block(0))

        '繪製暫存
        GUI4 = New Bitmap(PictureBox1.Width, PictureBox1.Height)
        pic_show()

        '物件庫重繪
        group2_reload()
    End Sub
    Private Sub PictureBox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        D_P.X = e.X
        D_P.Y = e.Y
    End Sub

    Private Sub PictureBox1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseUp
        '繪製方塊
        Dim g3 As Graphics
        Dim drawBrush As New SolidBrush(Color.FromArgb(10, 196, 10))
        Dim drawpen As New Pen(drawBrush, 1)
        g3 = Graphics.FromImage(GUI3(sm))
        Dim j, k As Integer
        If D_P.X \ 7 < e.X \ 7 Then j = 1 Else j = -1
        If D_P.Y \ 7 < e.Y \ 7 Then k = 1 Else k = -1
        If (e.X \ 7) >= 0 And (e.X \ 7) <= 99 And (e.Y \ 7) >= 0 And (e.Y \ 7) <= 99 Then
            For x = D_P.X \ 7 To e.X \ 7 Step j
                For y = D_P.Y \ 7 To e.Y \ 7 Step k
                    'g3.FillRectangle(drawBrush, x * 7, 1 + y * 7, 7, 7)
                    'g3.DrawImage(Image.FromFile("Tilesets/mini_bg" & Format(NumericUpDown1.Value, "000") & ".png"), x * 7, 1 + y * 7)
                    map_tmp.Parameter(sm, x, 99 - y) = NumericUpDown1.Value
                Next
            Next
            pic_reDraw()

        End If
    End Sub

    Private Sub PictureBox1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        Label1.Text = "(" & e.X & "," & 700 - e.Y & ")"
        Label2.Text = "(" & e.X \ 7 & "," & (700 - e.Y) \ 7 & ")"
    End Sub

    Private Sub RadioButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton1.Click, RadioButton2.Click, RadioButton3.Click, RadioButton4.Click
        sm = CType(sender, RadioButton).Tag
        pic_reDraw()    '重繪
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim OpenFileDialog As New OpenFileDialog
        OpenFileDialog.InitialDirectory = "Tilesets\"
        OpenFileDialog.Filter = "PNG|*.png"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = OpenFileDialog.FileName

            Dim a = Format(NumericUpDown1.Value, "000")
            If My.Computer.FileSystem.FileExists(momlact & "\Tilesets\bg" & a(0) & ".png") Then
                My.Computer.FileSystem.DeleteFile(momlact & "\Tilesets\bg" & a(0) & ".png", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            End If
            My.Computer.FileSystem.CopyFile(FileName, momlact & "\Tilesets\bg" & a(0) & ".png")
            group2_reload()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        '將控件上的資料存至變數
        group2_save()
        '儲存資料
        Dim h As String
        h = ""
        Dim k As Integer
        For k = 0 To 99
            h += Format(k, "000") + Space(1)
            h += MapItem(k).Name + Space(1)
            If MapItem(k).Pass Then
                h += "1" + Space(1)
            Else
                h += "0" + Space(1)
            End If
            h += CStr(MapItem(k).CenterPoint.X) + Space(1)
            h += CStr(MapItem(k).CenterPoint.Y) + Space(1)
            h += vbNewLine

        Next
        My.Computer.FileSystem.WriteAllText(momlact & "\Data\MapItemData.ini", h, False)
    End Sub
    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        group2_reload()
    End Sub
    Sub group2_reload() '刷新物件庫控件資料
        TextBox1.Text = MapItem(NumericUpDown1.Value).Name
        CheckBox1.Checked = MapItem(NumericUpDown1.Value).Pass
        TextBox2.Text = MapItem(NumericUpDown1.Value).CenterPoint.X
        TextBox3.Text = MapItem(NumericUpDown1.Value).CenterPoint.Y

        Dim a = Format(NumericUpDown1.Value, "000")
        If My.Computer.FileSystem.FileExists(momlact & "\Tilesets\bg" & a & ".png") Then
            PB1.Visible = True
            PB1.Image = Image.FromFile(momlact & "\Tilesets\bg" & a & ".png")
            PB1.Left = Panel1.Width / 2 - PB1.Width / 2
            PB1.Top = Panel1.Height / 2 - PB1.Height / 2
        Else
            PB1.Visible = False
        End If
    End Sub
    Sub group2_save()   '將控件上的資料存至變數
        MapItem(NumericUpDown1.Value).Name = TextBox1.Text
        MapItem(NumericUpDown1.Value).Pass = Val(CheckBox1.Checked)
        MapItem(NumericUpDown1.Value).CenterPoint = New Point(Val(TextBox2.Text), Val(TextBox3.Text))
    End Sub

    Sub save_map(ByVal ID As Integer, ByVal name As String)
        Dim a = Format(ID, "000")

        If My.Computer.FileSystem.DirectoryExists(momlact & "\Maps\" & a) Then  '判斷資料夾是否存在
            If My.Computer.FileSystem.FileExists(momlact & "\Maps\" & a & "\MapData.ini") Then '判斷地圖檔是否已經存在
                Dim X = MsgBox("地圖" & a & "已經存在" & vbNewLine & "是否要覆蓋原檔案?", MsgBoxStyle.OkCancel + 32, "注意")
                If X = 1 Then
                    '刪除檔案
                    My.Computer.FileSystem.DeleteFile(momlact & "\Maps\" & a & "\MapData.ini", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                    '新建檔案
                    My.Computer.FileSystem.WriteAllText(momlact & "\Maps\" & a & "\MapData.ini", String.Empty, False)
                    save_map_filesave(a, name)
                    MsgBox("地圖儲存完成!", MsgBoxStyle.OkOnly + 48, "提醒")
                End If
            Else
                save_map_filesave(a, name)
                MsgBox("地圖儲存完成!", MsgBoxStyle.OkOnly + 48, "提醒")

            End If
        Else '如果資料夾不存在，就建一個吧!
            My.Computer.FileSystem.CreateDirectory(momlact & "\Maps\" & a)
            My.Computer.FileSystem.WriteAllText(momlact & "\Maps\" & a & "\MapData.ini", String.Empty, False)
            save_map_filesave(a, name)
            MsgBox("地圖儲存完成!", MsgBoxStyle.OkOnly + 48, "提醒")
        End If

    End Sub

    Sub save_map_filesave(ByVal ID As String, ByVal name As String)
        Dim h As String
        If TextBox4.Text = "" Then
            h = "[MapName]" + Space(1) + "未命名" + vbNewLine
        Else
            h = "[MapName]" + Space(1) + name + vbNewLine
        End If

        h += "[MapParameter]" + vbNewLine
        Dim k As Integer
        For k = 0 To 3
            For i = 0 To 99
                For j = 0 To 99
                    h += CStr(map_tmp.Parameter(k, i, j)) + Space(1)
                Next
                h += vbNewLine
            Next
        Next
        My.Computer.FileSystem.WriteAllText(momlact & "\Maps\" & ID & "\MapData.ini", h, False)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        save_map(NumericUpDown2.Value, TextBox4.Text)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        load_map(NumericUpDown2.Value)
        TextBox4.Text = map_tmp.Name
    End Sub
    Sub load_map(ByVal ID As Integer)
        Dim a = Format(ID, "000")
        If My.Computer.FileSystem.FileExists(momlact & "\Maps\" & a & "\MapData.ini") Then  '確認地圖檔存在
            '開始讀取
            Dim h As String = ""
            Dim r = My.Computer.FileSystem.OpenTextFileReader(momlact & "\Maps\" & a & "\MapData.ini")
            Dim k() As String

            '讀取地圖名稱
            h = r.ReadLine()
            k = Split(h)
            map_tmp.Name = k(1)

            h = r.ReadLine() '[Parameter]

            Dim U As Integer
            For U = 0 To 3
                For i = 0 To 99
                    h = r.ReadLine()
                    k = Split(h)
                    For j = 0 To 99
                        map_tmp.Parameter(U, i, j) = Val(k(j))
                    Next
                Next
            Next
            r.Close()

            pic_reDraw()
        Else
            MsgBox("找不到" & "\Maps\" & a & "\MapData.ini", 48, "錯誤")
        End If
    End Sub
    Sub pic_reDraw()    '重繪
        '繪製方塊
        Dim drawBrush As New SolidBrush(Color.FromArgb(10, 196, 10))
        Dim drawpen As New Pen(drawBrush, 1)
        For k = 0 To 3

            Dim g3 As Graphics
            g3 = Graphics.FromImage(GUI3(k))
            g3.Clear(Color.Empty)
            For i = 0 To 99
                For j = 0 To 99
                    If map_tmp.Parameter(k, i, 99 - j) <> 0 Then
                        drawBrush.Color = mini_block(map_tmp.Parameter(k, i, 99 - j))
                        g3.FillRectangle(drawBrush, i * 7, 1 + j * 7, 7, 7)
                    End If

                Next
            Next
            'GUI3(k).Save("test" & Format(k, "000") & ".png")
        Next

        pic_show()  '顯示至pic
    End Sub
    Sub pic_show()
        Dim g4 As Graphics
        g4 = Graphics.FromImage(GUI4)
        g4.Clear(Color.Empty)

        For i = 0 To sm
            If i <> sm Then
                '透明化
                Dim g5 As Graphics
                Dim GUI5 As Bitmap
                GUI5 = New Bitmap(GUI4.Width, GUI4.Height)
                g5 = Graphics.FromImage(GUI5)
                Dim cm As System.Drawing.Imaging.ColorMatrix = New System.Drawing.Imaging.ColorMatrix()
                cm.Matrix00 = 1
                cm.Matrix11 = 1
                cm.Matrix22 = 1
                cm.Matrix44 = 1
                cm.Matrix33 = 0.25   'CType(50 / 100, Single)    '飽和度
                Dim ia As System.Drawing.Imaging.ImageAttributes = New System.Drawing.Imaging.ImageAttributes()
                ia.SetColorMatrix(cm)
                g5.DrawImage(GUI3(i), New Rectangle(0, 0, GUI3(i).Width, GUI3(i).Height), 0, 0, GUI3(i).Width, GUI3(i).Height, GraphicsUnit.Pixel, ia)
                g4.DrawImage(GUI5, New Point(0, 0))
            Else
                g4.DrawImage(GUI3(i), New Point(0, 0))
            End If
        Next

        g4.DrawImage(GUI2, New Point(0, 0)) '繪製網格
        PictureBox1.Image = GUI4
    End Sub
    Private Sub PB1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PB1.MouseDown
        TextBox2.Text = CStr(e.X)
        TextBox3.Text = CStr(e.Y)
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim X = MsgBox("這將會花上一些時間，確定要繼續?", MsgBoxStyle.OkCancel + 48, "注意")
        If X = 1 Then
            Me.Refresh()
            Dim tmp_mapImage As Bitmap
            tmp_mapImage = New Bitmap(6400, 3200)
            Dim g As Graphics
            g = Graphics.FromImage(tmp_mapImage)
            Dim k As Integer
            For ss = 0 To 3
                If ss <> 2 Then '2號圖層為獨立物件
                    If ss = 0 Or ss = 3 Then g.Clear(Color.Empty)
                    For i = 0 To 99
                        For j = 99 To 0 Step -1
                            k += 1
                            Me.Text = "MapEditor::Alpha 0.1.1(圖層輸出中" & Fix((k / 30000) * 10000) / 100 & "%)"
                            If map_tmp.Parameter(ss, i, j) <> 0 Then
                                g.DrawImage(Image.FromFile("Tilesets\bg" & Format(map_tmp.Parameter(ss, i, j), "000") & ".png"), New Point(32 * i + 32 * j + 32 - MapItem(map_tmp.Parameter(ss, i, j)).CenterPoint.X, 1600 - (-16 * i + 16 * j) - MapItem(map_tmp.Parameter(ss, i, j)).CenterPoint.Y))
                            End If
                        Next
                    Next
                    If ss = 1 Or ss = 3 Then tmp_mapImage.Save("Maps\" & Format(NumericUpDown2.Value, "000") & "\" & ss & ".png")
                End If
            Next
            Me.Text = "MapEditor::Alpha 0.1.1"
            MsgBox("輸出完成!", MsgBoxStyle.OkOnly, "提醒")
        End If
    End Sub
End Class