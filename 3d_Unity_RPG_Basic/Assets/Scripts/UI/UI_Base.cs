using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    public abstract void Init();

    private void Start()
    {
        Init();
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type); // enum type���� string �迭�� �޴´�.
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects); // Ÿ���� Key�� ����Ͽ� Object���� Value�� ��´�.

        for(int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject)) // ������Ʈ�� �ƴ� �� GameObject�� ��� ����ó��
                objects[i] = Utils.FindChild(gameObject, names[i], true);
            else
                objects[i] = Utils.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        // Key�� �������� �ʴٸ� null ��ȯ + objects�� Key�� Value�� ��´�
        // Enum�� int�� ����ȯ�Ͽ� �ε��������Ͽ� Object�� TŸ������ ��ȯ

        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    public static void BindEvent(GameObject go, Action<PointerEventData> action, Defines.UIEvents type = Defines.UIEvents.Click)
    {
        UI_EventHandler evt = Utils.GetOrAddComponent<UI_EventHandler>(go);

        // Object�� UI_EventHandler�� �ٿ� �̺�Ʈ �ݹ�޼ҵ带 ����� �� �ְ� �Ѵ�.
        // �� ���ǵǾ��ִ� �̺�Ʈ�� �߻��ϸ� �ش� action�� ��ϵ� �޼ҵ尡 ����ȴ�.

        switch (type)
        {
            case Defines.UIEvents.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Defines.UIEvents.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}
