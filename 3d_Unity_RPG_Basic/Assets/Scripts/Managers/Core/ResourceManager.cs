using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object 
    {
        if(typeof(T) == typeof(GameObject)) 
        {
            // ��θ� ���� �̸� ����
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            // pool������� �ҷ��´�.
            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;

        }

        // pool�� ������ ���� Load�Ѵ�.
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        if(original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // poolable Object�̸� pool���� pop�Ͽ� �����Ѵ�. 
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Poolable poolable = go.GetComponent<Poolable>();
        // poolable Object�̸� pool�� push�Ͽ� ��ȯ�Ѵ�. 
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
