Module Game_skill
    Public Sub Skill_NormalAttack(ByVal attacker As Obj_creature) '普通攻擊
        For i = 0 To 99
            '被施術者活著、不是施術者、被施術者和施術者在同個地圖
            If creature_obj(i).Life And attacker.ID <> creature_obj(i).ID And creature_obj(i).Stay_Map = attacker.Stay_Map Then
                Dim thyta, x, y, r As Single
                '這堆是為了達到參謀給的攻擊範圍= =
                x = attacker.Stay_X - 0.5 * Math.Cos(attacker.Stay_Angle)
                y = attacker.Stay_Y - 0.5 * Math.Sin(attacker.Stay_Angle)
                r = ((x - creature_obj(i).Stay_X) ^ 2 + (y - creature_obj(i).Stay_Y) ^ 2) ^ 0.5
                If r <= 2 And r >= 0.5 Then   '在攻擊範圍內
                    thyta = Math.Atan2(creature_obj(i).Stay_Y - attacker.Stay_Y, creature_obj(i).Stay_X - attacker.Stay_X)
                    If Math.Abs(attacker.Stay_Angle - thyta) < Pi / 2 Then '在攻擊角內
                        creature_obj(i).Hp = creature_obj(i).Hp - 10
                        creature_obj(i).Stay_X = creature_obj(i).Stay_X + 1 * Math.Cos(attacker.Stay_Angle)
                        creature_obj(i).Stay_Y = creature_obj(i).Stay_Y + 1 * Math.Sin(attacker.Stay_Angle)
                        If creature_obj(i).Hp <= 0 Then
                            creature_obj(i).Hp = 0
                            creature_obj(i).Life = False
                        End If
                    End If
                End If

            End If
        Next
    End Sub

End Module
