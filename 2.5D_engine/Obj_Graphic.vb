Public Class Obj_Graphic
    Public Enable As Boolean '
    Public ID As Integer        '編號
    Public Type As Integer ' 0 地圖物件; 1 生物
    Public X As Single
    Public Y As Single
    Public Function D(ByVal Px As Single, ByVal Py As Single) As Single
        Dim Xt = X - Px
        Dim Yt = Y - Py
        Dim Xd = (Xt - Yt) / 2
        Dim Yd = (Yt - Xt) / 2
        If Xd >= 0 Then
            Return (Xd ^ 2 + Yd ^ 2) ^ 0.5
        Else
            Return -((Xd ^ 2 + Yd ^ 2) ^ 0.5)
        End If

    End Function
End Class
