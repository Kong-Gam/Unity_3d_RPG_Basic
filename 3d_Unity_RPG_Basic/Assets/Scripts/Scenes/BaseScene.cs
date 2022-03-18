using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Defines.Scenes SceneType { get; protected set; } = Defines.Scenes.Unknown;
    private void Awake()
    {
        Init();
    }

    protected virtual void Init() // Child Scene���� ���������� �ʱ�ȭ�ؾ��ϴ� �κ� ����
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear(); // �߻� �޼ҵ�� Child���� Clear�� �ð��ش�
}
