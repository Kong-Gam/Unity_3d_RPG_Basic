using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension // Ȯ�� �޼��� ( Object�� �޼��� ó�� ����� �� �ֵ��� ���� )
{
   public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Utils.GetOrAddComponent<T>(go);
    }

   /*public static void BindEvent(this GameObject go, Action<PointerEventData> action, Defines.UIEvent type = Defines.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }*/
}
