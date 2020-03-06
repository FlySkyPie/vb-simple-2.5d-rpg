Module Variable
    Public momlact As String = Application.StartupPath
    Public Const Pi = 3.141592653589
    Public creature_obj(100) As Obj_creature    '動態怪物物件 這決定整個遊戲能夠存在多少生物(包括玩家)
    Public Data_Monster(100) As Obj_creature
    Public Data_MapItem(100) As Obj_MapItem
    Public Data_Map(100) As Obj_map

    '玩家資料
    'Public player =creature_obj(0)       '這是玩家的肉體
    Public player_elment As Obj_player  '這是玩家靈魂 (´･ω･`) 

    '操作運算用變數
    Public Mouse_X, Mouse_Y As Integer
    Public Mouse_Xd, Mouse_Yd As Single
    Public keyboard_move As Boolean '鍵盤行走判斷

    '繪圖用變數
    Public GUI As Bitmap   '圖像暫存
    Public tmp_mapImage(2) As Bitmap  '地圖圖層暫存
    Public Animation(99) As Integer     '提醒我到時候這要刪掉，用creature.Time取代
    Public key_up, key_down, key_right, key_left As Boolean '鍵盤控角

End Module
