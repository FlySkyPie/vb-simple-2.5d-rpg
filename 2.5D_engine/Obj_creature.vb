Public Class Obj_creature
    Public Name As String
    Public Hp As Integer
    Public Mp As Integer

    Public Life As Boolean
    Public ID As Integer
    Public Type As Integer  '0=人或NPC  1以上=怪物

    Public Stay_Map As Integer
    Public Stay_X As Single
    Public Stay_Y As Single
    Public Stay_Angle As Single
    Public Activity As Integer              '動作 0-站立    1-普通攻擊  .etc
    Public Time As Integer

    Public Target_X As Single
    Public Target_Y As Single

    Public ATK As Integer
    Public DEF As Integer
    Public MATK As Integer
    Public MDEF As Integer
    Public SPD As Integer

    Public Money As Integer

    '繪圖用變數
    Public Image_move(4, 8) As Bitmap '(方向,畫格)
    Public CP_move(4, 8) As Point '(方向,畫格)    圖形的中心點座標
    Public Image_attack(4, 8) As Bitmap '(方向,畫格)
    Public CP_attack(4, 8) As Point '(方向,畫格)  圖形的中心點座標
End Class

Public Class Obj_player
    Public Sex As Boolean 'true is male ; false is female

    Public Level As Integer
    Public Exp As Integer

    Public Item(99, 1) As Integer   '(,0)物品編號 (,1)物品數量
End Class
