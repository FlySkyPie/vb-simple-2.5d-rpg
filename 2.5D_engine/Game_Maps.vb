Module Game_Maps
    Public Sub Data_MapItemData_Load()
        '載入地圖物件庫
        Dim h As String = ""
        Dim r = My.Computer.FileSystem.OpenTextFileReader(momlact & "\Data\MapItemData.ini")
        Dim gg() As String
        Dim U As Integer
        For U = 0 To 99
            Data_MapItem(U) = New Obj_MapItem
            h = r.ReadLine()
            gg = Split(h)

            Data_MapItem(U).Name = gg(1)
            Data_MapItem(U).Pass = Val(gg(2))
            Data_MapItem(U).CenterPoint = New Point(Val(gg(3)), Val(gg(4)))

            Dim a = Format(U, "000")
            If My.Computer.FileSystem.FileExists(momlact & "\Tilesets\bg" & a & ".png") Then
                '將圖片載入記憶體
                Data_MapItem(U).image = New Bitmap(Image.FromFile(momlact & "\Tilesets\bg" & a & ".png"))
            End If
        Next
        r.Close()
    End Sub

    Public Sub MapData_Load()
        '初始化暫存
        For ss = 0 To 99
            Data_map(ss) = New Obj_map
            For k = 0 To 3
                For i = 0 To 99
                    For j = 0 To 99
                        Data_map(ss).Parameter(k, i, j) = 0
                    Next
                Next
            Next
        Next
        ReadMap()       '讀取地圖檔案(map file)
    End Sub
    Sub ReadMap()
        For ss = 0 To 99
            If My.Computer.FileSystem.FileExists(momlact & "\Maps\" & Format(ss, "000") & "\MapData.ini") Then  '確認地圖檔存在
                '開始讀取
                Dim h As String = ""
                Dim r = My.Computer.FileSystem.OpenTextFileReader(momlact & "\Maps\" & Format(ss, "000") & "\MapData.ini")
                Dim k() As String

                '讀取地圖名稱
                h = r.ReadLine()
                k = Split(h)
                Data_Map(ss).Name = k(1)

                h = r.ReadLine()    '[Parameter]

                Dim U As Integer
                For U = 0 To 3
                    For i = 0 To 99
                        h = r.ReadLine()
                        k = Split(h)
                        For j = 0 To 99
                            Data_Map(ss).Parameter(U, i, j) = Val(k(j))
                        Next
                    Next
                Next
                r.Close()
            End If
        Next

    End Sub
End Module
