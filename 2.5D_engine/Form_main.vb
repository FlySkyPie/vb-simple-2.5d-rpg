Public Class Form_main
    '=======================================程式啟動資料載入=======================================
    Private Sub Form_main_HandleCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.HandleCreated
        '繪圖準備
        Screen.Height = Me.Height - 36
        Screen.Width = Me.Width - 10
        Screen.Image = Image.FromFile("Panoramas\Panoramas000.png")
        DoubleBuffered = True

        GUI = New Bitmap(Screen.Width, Screen.Height)
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
    Private Sub Form_main_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Data_MapItemData_Load()  '載入地圖物件資料
        MapData_Load()      '載入地圖資料
        MonsterData_load()  '載入怪物資料
        CreatureObj_load()      '初始化怪物物件
        player_load()       '載入玩家資料

        '姑且丟一隻去測試吧
        creature_obj(1) = Data_monster(1) 'monster(1) 是一隻妖精
        creature_obj(1).ID = 1
        creature_obj(1).Stay_Map = 1
        creature_obj(1).Stay_X = 75
        creature_obj(1).Stay_Y = 33
        creature_obj(1).Stay_Angle = 0
        creature_obj(1).Life = True

        MapImage_Load() '載入地圖圖層
        Timer1.Enabled = True '開啟浮點運算用Timer (包含繪圖)
    End Sub
    Sub MapImage_Load()
        tmp_mapImage(0) = New Bitmap(Image.FromFile("Maps\" & Format(creature_obj(0).Stay_Map, "000") & "\1.png"))
        tmp_mapImage(1) = New Bitmap(Image.FromFile("Maps\" & Format(creature_obj(0).Stay_Map, "000") & "\3.png"))
    End Sub
    '======================================================================================================
    '======================================================================================================
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        'Timer1.Enabled = False
        '########浮點動態########
        keyboard_playermove()  '根據方向鍵讀值設定player的Target
        'ObjMove(creature_obj(0))
        For u = 0 To 99
            Creature_active(creature_obj(u))
        Next

        '########繪圖前置作業#########
        Dim Str_tmp As String
        Dim drawFont As New Font("Arial", 9)    '載入自型
        Dim drawBrush As New SolidBrush(Color.Black)    '載入筆刷
        Dim drawpen As New Pen(drawBrush)   '載入...筆? (VB的結構真奇怪= =)

        Dim g As Graphics
        g = Graphics.FromImage(GUI)
        g.Clear(Color.FromArgb(128, 128, 128)) '清空圖像暫存

        '#########以下開放繪製#########

        '繪製地圖 (地面)

        Dim tmp_P As Point
        Dim tmp As Bitmap
        tmp_P.X = (32 * creature_obj(0).Stay_X + 32 * creature_obj(0).Stay_Y) - Screen.Width / 2
        tmp_P.Y = 3200 - (1600 - 16 * creature_obj(0).Stay_X + 16 * creature_obj(0).Stay_Y) - Screen.Height / 2
        tmp = tmp_mapImage(0).Clone(New Rectangle(tmp_P.X, tmp_P.Y, Screen.Width, Screen.Height), tmp_mapImage(0).PixelFormat)  '建立複本
        g.DrawImage(tmp, New Point(0, 0))
        tmp.Dispose()   '釋放複本

        '圖層2(玩家後方)
        For i = -16 To 16
            For j = -16 To 16
                If -13 <= i + j And i + j <= 13 And -19 <= i - j And i - j < 0 Then
                    Dim x, y As Single
                    x = Fix(creature_obj(0).Stay_X) + i - creature_obj(0).Stay_X
                    y = Fix(creature_obj(0).Stay_Y) + j - creature_obj(0).Stay_Y
                    If Data_map(creature_obj(0).Stay_Map).Parameter(2, i + Fix(creature_obj(0).Stay_X), j + Fix(creature_obj(0).Stay_Y)) <> 0 Then
                        Dim Map_Element = Data_map(creature_obj(0).Stay_Map).Parameter(2, i + Fix(creature_obj(0).Stay_X), j + Fix(creature_obj(0).Stay_Y))
                        g.DrawImage(Data_MapItem(Map_Element).image, New Point(Screen.Width / 2 + 32 * x + 32 * y + 32 - Data_MapItem(Map_Element).CenterPoint.X, Screen.Height / 2 - (-16 * x + 16 * y) - Data_MapItem(Map_Element).CenterPoint.Y))
                    End If
                End If
            Next
        Next

        '繪製怪物
        For i = 1 To 99
            If creature_obj(i).Life Then
                If creature_obj(i).Stay_Map = creature_obj(0).Stay_Map Then
                    Dim j, k As Single
                    j = creature_obj(i).Stay_X - creature_obj(0).Stay_X
                    k = creature_obj(i).Stay_Y - creature_obj(0).Stay_Y
                    Dim x, y As Integer
                    x = Int(32 * j + 32 * k + 32)
                    y = Int(-(-16 * j + 16 * k))
                    If Math.Abs(x) < Screen.Width / 2 And Math.Abs(y) < Screen.Height / 2 Then '在繪製範圍內
                        g.DrawImage(Data_MapItem(4).image, New Point(Screen.Width / 2 + x - 64, Screen.Height / 2 + y - 51))
                    End If
                End If

            End If
        Next

        '繪製玩家角色
        Dim stand As Bitmap
        If creature_obj(0).Activity = 1 Then '走路
            If Animation(0) >= 8 Then Animation(0) = 1 Else Animation(0) += 1
            Dim direct = direction8(creature_obj(0).Stay_Angle * 180 / Pi - 45) '痾...浮點運算的世界和GUI差了45度角
            '判斷方向
            If direct >= 0 Then
                stand = New Bitmap(creature_obj(0).Image_move(direct, Animation(0)))
            Else
                stand = New Bitmap(creature_obj(0).Image_move(Math.Abs(direct), Animation(0)))
                stand.RotateFlip(RotateFlipType.Rotate180FlipY)
            End If
            g.DrawImage(stand, New Point(Screen.Width / 2 - stand.Width / 2, Screen.Height / 2 - stand.Height / 2 - 32))
        ElseIf creature_obj(0).Activity = 0 Then '原地站立
            Dim direct = direction8(creature_obj(0).Stay_Angle * 180 / Pi - 45) '痾...浮點運算的世界和GUI差了45度角
            '判斷方向
            If direct >= 0 Then
                stand = New Bitmap(creature_obj(0).Image_move(direct, 0))
            Else
                stand = New Bitmap(creature_obj(0).Image_move(Math.Abs(direct), 0))
                stand.RotateFlip(RotateFlipType.Rotate180FlipY)
            End If
            g.DrawImage(stand, New Point(Screen.Width / 2 - stand.Width / 2, Screen.Height / 2 - stand.Height / 2 - 32))
        ElseIf creature_obj(0).Activity = 2 Then '攻擊

            Dim direct = direction8(creature_obj(0).Stay_Angle * 180 / Pi - 45)

            If direct = 1 Or direct = 2 Then
                stand = New Bitmap(creature_obj(0).Image_attack(1, Animation(0)))
                g.DrawImage(stand, New Point(Screen.Width / 2 - 70, Screen.Height / 2 - stand.Height / 2 - 32))
            ElseIf direct = 3 Or direct = 4 Then
                stand = New Bitmap(creature_obj(0).Image_attack(3, Animation(0)))
                g.DrawImage(stand, New Point(Screen.Width / 2 - 78, Screen.Height / 2 - stand.Height / 2 - 32))
            ElseIf direct = -2 Or direct = -3 Then
                stand = New Bitmap(creature_obj(0).Image_attack(3, Animation(0)))
                stand.RotateFlip(RotateFlipType.Rotate180FlipY)
                g.DrawImage(stand, New Point(Screen.Width / 2 - 35, Screen.Height / 2 - stand.Height / 2 - 32))
            Else : direct = 0 Or direct = -1
                stand = New Bitmap(creature_obj(0).Image_attack(1, Animation(0)))
                stand.RotateFlip(RotateFlipType.Rotate180FlipY)
                g.DrawImage(stand, New Point(Screen.Width / 2 - 35, Screen.Height / 2 - stand.Height / 2 - 32))
            End If
            'g.DrawImage(stand, New Point(Screen.Width / 2 - 70, Screen.Height / 2 - stand.Height / 2 - 32))

            If Animation(0) = 5 Then    '攻擊時機
                Skill_NormalAttack(creature_obj(0))
            End If

            Animation(0) += 1
            If Animation(0) > 8 Then
                creature_obj(0).Activity = 0
            End If

        End If
        'stand = Nothing

        '圖層2(玩家前方)
        For i = -15 To 15
            For j = -15 To 15
                If -13 <= i + j And i + j <= 13 And 0 <= i - j And i - j <= 19 Then
                    Dim x, y As Single
                    x = Fix(creature_obj(0).Stay_X) + i - creature_obj(0).Stay_X
                    y = Fix(creature_obj(0).Stay_Y) + j - creature_obj(0).Stay_Y
                    If Data_map(creature_obj(0).Stay_Map).Parameter(2, i + Fix(creature_obj(0).Stay_X), j + Fix(creature_obj(0).Stay_Y)) <> 0 Then
                        Dim Map_Element = Data_map(creature_obj(0).Stay_Map).Parameter(2, i + Fix(creature_obj(0).Stay_X), j + Fix(creature_obj(0).Stay_Y))
                        g.DrawImage(Data_MapItem(Map_Element).image, New Point(Screen.Width / 2 + 32 * x + 32 * y + 32 - Data_MapItem(Map_Element).CenterPoint.X, Screen.Height / 2 - (-16 * x + 16 * y) - Data_MapItem(Map_Element).CenterPoint.Y))
                    End If
                End If
            Next
        Next

        '繪製地圖 (天空)
        tmp = tmp_mapImage(1).Clone(New Rectangle(tmp_P.X, tmp_P.Y, Screen.Width, Screen.Height), tmp_mapImage(0).PixelFormat)
        g.DrawImage(tmp, New Point(0, 0))
        tmp.Dispose()

        ''顯示滑鼠座標
        'Str_tmp = "滑鼠座標:" & "(" & CStr(Mouse_X) & "," & CStr(Mouse_Y) & ")"
        'g.DrawString(Str_tmp, drawFont, drawBrush, 0, Screen.Height - 20)
        ''顯示滑鼠(斜角座標)
        'Str_tmp = "滑鼠斜角座標:" & "(" & CStr(Mouse_Xd) & "," & CStr(Mouse_Yd) & ")"
        'g.DrawString(Str_tmp, drawFont, drawBrush, 130, Screen.Height - 20)
        'Str_tmp = "目標座標:" & "(" & CStr(player.Target_X) & "," & CStr(player.Target_Y) & ")"
        'g.DrawString(Str_tmp, drawFont, drawBrush, 360, Screen.Height - 20)

        '顯示玩家資料
        Str_tmp = "玩家名稱:" & creature_obj(0).Name & vbNewLine
        Str_tmp += "血量:" & creature_obj(0).Hp & vbNewLine
        Str_tmp += "魔量:" & creature_obj(0).Mp & vbNewLine
        Str_tmp += "等級:" & player_elment.Level & vbNewLine
        Str_tmp += "經驗值:" & player_elment.Exp & vbNewLine
        Str_tmp += "金錢:" & creature_obj(0).Money & vbNewLine
        Str_tmp += "攻擊力" & creature_obj(0).ATK & vbNewLine
        Str_tmp += "防禦力" & creature_obj(0).DEF & vbNewLine
        Str_tmp += vbNewLine
        Str_tmp += "所在地圖:" & Data_map(creature_obj(0).Stay_Map).Name & vbNewLine
        Str_tmp += "玩家座標:(" & CStr(creature_obj(0).Stay_X) & "," & CStr(creature_obj(0).Stay_Y) & ")" & vbNewLine
        Str_tmp += "玩家朝向:" & "(" & CStr(creature_obj(0).Stay_Angle) & ")" & vbNewLine
        g.DrawString(Str_tmp, drawFont, drawBrush, 0, 0)

        '顯示測試用怪物資訊
        Str_tmp = "怪物名稱:" & creature_obj(1).Name & vbNewLine
        Str_tmp += "類型:" & creature_obj(1).Type & vbNewLine
        Str_tmp += "活著?:" & creature_obj(1).Life & vbNewLine
        Str_tmp += "血量:" & creature_obj(1).Hp & vbNewLine
        Str_tmp += "魔量:" & creature_obj(1).Mp & vbNewLine
        Str_tmp += "所在地圖:" & Data_map(creature_obj(1).Stay_Map).Name & vbNewLine
        Str_tmp += "怪物所在座標:" & CStr(creature_obj(1).Stay_X) & "," & CStr(creature_obj(1).Stay_Y) & ")" & vbNewLine
        Str_tmp += "怪物朝向:" & "(" & CStr(creature_obj(1).Stay_Angle) & ")" & vbNewLine
        Str_tmp += "怪物的目標:(" & CStr(creature_obj(1).Target_X) & "," & CStr(creature_obj(1).Target_Y) & ")" & vbNewLine
        g.DrawString(Str_tmp, drawFont, drawBrush, 0, 300)



        '#########以上開放繪製#########
        Screen.Image = GUI  '顯示圖像暫存

        'Timer1.Enabled = True
    End Sub
    '八方位判定
    Function direction8(ByVal Angle As Single) As Integer
        If Angle >= 157.5 Or Angle <= -157.5 Then '左
            Return 2
        ElseIf Angle <= 157.5 And Angle >= 112.5 Then '左上
            Return 3
        ElseIf Angle <= 112.5 And Angle >= 67.5 Then '上
            Return 4
        ElseIf Angle <= 67.5 And Angle >= 22.5 Then '右上
            Return -3
        ElseIf Angle <= 22.5 And Angle >= -22.5 Then '右
            Return -2
        ElseIf Angle >= -67.5 And Angle <= -22.5 Then '右下
            Return -1
        ElseIf Angle >= -112.5 And Angle <= -67.5 Then '下
            Return 0
        ElseIf Angle >= -157.5 And Angle <= -112.5 Then '左下
            Return 1
        Else
            Return 9    '應該不會有意外吧...
        End If
    End Function

    '======================================================================================================
    '=======================================控制相關========================================================
    '##########滑鼠##########
    Private Sub Screen_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then      '左鍵
            MsgBox("你按了左鍵", 48, "測試respon")
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then  '右鍵
            'MsgBox("你按了右鍵", 48, "測試respon")
            '走路
            creature_obj(0).Activity = 1
            creature_obj(0).Target_X = creature_obj(0).Stay_X + Mouse_Xd
            creature_obj(0).Target_Y = creature_obj(0).Stay_Y + Mouse_Yd

        End If
    End Sub
    Private Sub Screen_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Screen.MouseMove
        Mouse_X = e.X
        Mouse_Y = Screen.Height - 1 - e.Y
        '將GUI取得的座標換算成浮點運算用的座標
        Dim tmp_X, tmp_Y As Single
        tmp_X = Mouse_X - Screen.Width / 2
        tmp_Y = Mouse_Y - Screen.Height / 2
        Mouse_Xd = (tmp_X - 2 * tmp_Y) / 64
        Mouse_Yd = (tmp_X + 2 * tmp_Y) / 64
    End Sub

    '##########鍵盤##########
    Private Sub Form_main_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        '方向鍵控角
        If e.KeyValue = Keys.Up Then key_up = True
        If e.KeyValue = Keys.Down Then key_down = True
        If e.KeyValue = Keys.Right Then key_right = True
        If e.KeyValue = Keys.Left Then key_left = True

        If e.KeyValue = Keys.Z Then '攻擊
            'MsgBox("你按了攻擊鍵(Z)", 48, "測試respon")
            If creature_obj(0).Activity <> 2 Then
                creature_obj(0).Activity = 0  '要攻擊?停下來再說!
                creature_obj(0).Activity = 2 '攻擊
                Animation(0) = 1
            End If
        ElseIf e.KeyValue = Keys.F7 Then
        ElseIf e.KeyValue = Keys.Escape Then
            Dim x = MsgBox("確認要離開遊戲?", MsgBoxStyle.OkCancel + 32, "離開遊戲")
            If x = 1 Then
                Me.Dispose()
            End If
        End If
    End Sub
    Private Sub Form_main_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyValue = Keys.Up Then key_up = False
        If e.KeyValue = Keys.Down Then key_down = False
        If e.KeyValue = Keys.Right Then key_right = False
        If e.KeyValue = Keys.Left Then key_left = False
    End Sub
    '根據方向鍵讀值設定player的Target
    Sub keyboard_playermove()
        keyboard_move = (key_up Or key_right Or key_left Or key_down) And Not (key_up And key_down) And Not (key_right And key_left)
        If keyboard_move Then
            creature_obj(0).Activity = 1
            Dim add_X, add_Y As Single
            add_X = 0 : add_Y = 0
            If key_up And Not key_right And Not key_left And Not key_down Then   '正上
                add_X = -0.5 : add_Y = 0.5
            ElseIf Not key_up And Not key_right And Not key_left And key_down Then '正下
                add_X = 0.5 : add_Y = -0.5
            ElseIf Not key_up And key_right And Not key_left And Not key_down Then  '正右
                add_X = 0.5 : add_Y = 0.5
            ElseIf Not key_up And Not key_right And key_left And Not key_down Then '正左
                add_X = -0.5 : add_Y = -0.5
            ElseIf key_up And Not key_right And key_left And Not key_down Then '上左
                add_X = -0.5 * 1.414
            ElseIf key_up And key_right And Not key_left And Not key_down Then '上右
                add_Y = 0.5 * 1.414
            ElseIf Not key_up And Not key_right And key_left And key_down Then '下左
                add_Y = -0.5 * 1.414
            ElseIf Not key_up And key_right And Not key_left And key_down Then '下右
                add_X = 0.5 * 1.414
            End If
            creature_obj(0).Target_X = creature_obj(0).Stay_X + add_X
            creature_obj(0).Target_Y = creature_obj(0).Stay_Y + add_Y
        End If
    End Sub
    '=====================================================================================================
    '======================================================================================================
    '########GUI繪圖########
    Sub Draw_GUI()

    End Sub
End Class
