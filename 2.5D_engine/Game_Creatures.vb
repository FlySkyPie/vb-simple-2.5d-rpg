Module Game_Creatures
    '載入怪物資料

    '####################初始化並載入怪物資料####################
    Public Sub MonsterData_load()
        For i = 0 To 99
            Data_monster(i) = New Obj_creature
        Next
        MonsterData_Read()
    End Sub
    '####################載入怪物設定檔####################
    Sub MonsterData_Read()

        Dim h As String = ""
        Dim r = My.Computer.FileSystem.OpenTextFileReader(momlact & "\Data\MonsterData.ini")
        Dim gg() As String
        Dim U As Integer
        For U = 0 To 99
            'ID 與 名稱
            h = r.ReadLine()
            gg = Split(h)
            Data_monster(U) = New Obj_creature
            Data_monster(U).Type = Val(gg(1))
            Data_monster(U).Name = gg(3)
            '能力值
            h = r.ReadLine()
            gg = Split(h)
            Data_monster(U).Hp = Val(gg(0))
            Data_monster(U).Mp = Val(gg(1))
            Data_monster(U).ATK = Val(gg(2))
            Data_monster(U).DEF = Val(gg(3))
            Data_monster(U).MATK = Val(gg(4))
            Data_monster(U).MDEF = Val(gg(5))
            Data_monster(U).SPD = Val(gg(6))

            r.ReadLine() '[move_CP]標題
            For j = 0 To 4
                h = r.ReadLine()
                gg = Split(h)
                For m = 0 To 8
                    Data_monster(U).CP_move(j, m).X = Val(gg(m))
                Next
            Next
            For j = 0 To 4
                h = r.ReadLine()
                gg = Split(h)
                For m = 0 To 8
                    Data_monster(U).CP_move(j, m).Y = Val(gg(m))
                Next
            Next

            r.ReadLine() '[attack_CP]標題
            For j = 0 To 4
                h = r.ReadLine()
                gg = Split(h)
                For m = 0 To 8
                    Data_monster(U).CP_attack(j, m).X = Val(gg(m))
                Next
            Next
            For j = 0 To 4
                h = r.ReadLine()
                gg = Split(h)
                For m = 0 To 8
                    Data_monster(U).CP_attack(j, m).Y = Val(gg(m))
                Next
            Next
        Next
        r.Close()

        '將圖片載入記憶體
        For k = 0 To 99
            For i = 0 To 4
                For j = 0 To 8
                    If My.Computer.FileSystem.FileExists(momlact & "\Characters\monster" & Format(k, "000") & "_move_0" & i & j & ".png") Then

                        Data_monster(k).Image_move(i, j) = New Bitmap(Image.FromFile("Characters\monster" & Format(k, "000") & "_move_0" & i & j & ".png"))
                    End If

                Next
            Next
        Next
    End Sub
    '####################載入玩家圖形####################
    Sub PlayerImage_load()
        For i = 0 To 4
            For j = 0 To 8
                creature_obj(0).Image_move(i, j) = New Bitmap(Image.FromFile("Characters\player_move_0" & i & j & ".png"))
            Next
        Next
        For j = 1 To 8
            creature_obj(0).Image_attack(1, j) = New Bitmap(Image.FromFile("Characters\player_attack_0" & "1" & j & ".png"))
        Next
        For j = 1 To 8
            creature_obj(0).Image_attack(3, j) = New Bitmap(Image.FromFile("Characters\player_attack_0" & "3" & j & ".png"))
        Next
    End Sub
    '####################載入玩家角色####################
    Public Sub player_load()
        creature_obj(0).ID = 0
        creature_obj(0).Type = 0
        creature_obj(0).Name = "勇者(暫定)"
        creature_obj(0).Hp = 100
        creature_obj(0).Mp = 100
        creature_obj(0).ATK = 20
        creature_obj(0).DEF = 20
        creature_obj(0).MATK = 10
        creature_obj(0).MDEF = 10
        creature_obj(0).SPD = 5
        creature_obj(0).Stay_Map = 1
        creature_obj(0).Stay_X = 40
        creature_obj(0).Stay_Y = 56
        creature_obj(0).Money = 500
        creature_obj(0).Life = True

        player_elment = New Obj_player
        player_elment.Sex = False  '女的XDD
        player_elment.Level = 1
        player_elment.Exp = 0

        '====================
        PlayerImage_load() '載入玩家圖形
    End Sub
    '####################生物物件初始化####################
    Public Sub CreatureObj_load()
        For i = 0 To 99
            creature_obj(i) = New Obj_creature
            creature_obj(i).Life = False
        Next
    End Sub

    '########某種程度上,怪物的AI都寫在這裡########
    Public Sub Creature_active(ByVal creature As Obj_creature)
        If creature.Life Then    '確認活著再來執行"活動"
            '生物時鐘
            creature.Time += 1
            If creature.Time > 8 Then creature.Time = 1

            If creature.Type = 0 Then
                ObjMove(creature)
            ElseIf creature.Type = 1 Then '如果是妖精的話
                If creature.Stay_Map = creature_obj(0).Stay_Map Then '在同一張地圖就去追玩家!
                    If (creature_obj(0).Stay_X - creature.Stay_X) ^ 2 + (creature_obj(0).Stay_Y - creature.Stay_Y) ^ 2 > 1 Then
                        creature.Activity = 1
                        creature.Target_X = creature_obj(0).Stay_X
                        creature.Target_Y = creature_obj(0).Stay_Y
                        ObjMove(creature)
                    Else
                        creature.Activity = 0
                    End If
                End If

            Else    '其他怪物 .etc monster


            End If
        End If
    End Sub

    Public Sub ObjMove(ByVal Obj As Obj_creature)
        If Obj.Activity = 1 Then '判斷生物在走路? 那就走吧!
            Obj.Stay_Angle = Math.Atan2(Obj.Target_Y - Obj.Stay_Y, Obj.Target_X - Obj.Stay_X)
            Dim x, y As Integer
            Dim speed As Single
            speed = Obj.SPD ^ 0.5 / 10 '行走速度標準
            x = Fix(Obj.Stay_X + 0.6 * Math.Cos(Obj.Stay_Angle))
            y = Fix(Obj.Stay_Y + 0.6 * Math.Sin(Obj.Stay_Angle))
            Dim boo_tmp2 As Boolean
            boo_tmp2 = True
            For i = 0 To 3
                boo_tmp2 = boo_tmp2 And Data_MapItem(Data_Map(Obj.Stay_Map).Parameter(i, x, y)).Pass
            Next
            '那塊地可以走嗎?
            If boo_tmp2 Then
                Obj.Stay_X = Obj.Stay_X + speed * Math.Cos(Obj.Stay_Angle)
                Obj.Stay_Y = Obj.Stay_Y + speed * Math.Sin(Obj.Stay_Angle)
                '到達目標後
                If (Obj.Stay_X - Obj.Target_X) ^ 2 + (Obj.Stay_Y - Obj.Target_Y) ^ 2 < 0.2 Then
                    Obj.Activity = 0
                End If
            Else
                Obj.Activity = 0
            End If
        End If
    End Sub


End Module
