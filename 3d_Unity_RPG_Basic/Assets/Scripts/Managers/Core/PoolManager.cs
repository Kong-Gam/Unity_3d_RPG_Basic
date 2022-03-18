using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 10)
        {
            // Pooling�� Object�� ����(Original)�� Root�� �����Ѵ�.
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            // count ��ŭ Object�� Poolable Component�� �����Ͽ� Pooling Stack�� Push�Ѵ�.
            for (int i = 0; i < count; i++)
                Push(Create());
        }

        Poolable Create()
        {
            // (Original)������Ʈ�� Poolable Component ������ ��ȯ
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
            
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root; // pooling object(Root)�� parent�� ����
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            // Pool Stack�� �ݳ��Ѵ�. �ݳ� ������ Pooling Info�� �ʱ�ȭ����
            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            // Pool���� ������ �����.

            if (_poolStack.Count > 0) // ������� �ʴ� pool�� �ִٸ� ����
                poolable = _poolStack.Pop();
            else // ������ Pool �߰�����
                poolable = Create();

            poolable.gameObject.SetActive(true);

            /* DontDestroyOnLoad ���� �뵵
             * ex) Pool�� Default count 5�� �ʱ�ȭ -> ������ 10���� ����
             *     -> ������ 5�� DontDestroyOnLoad ���Ϸ� ���� -> ������ 5�� Init�� ȣ������ʾ� DontDestroyOnLoad �ۿ� �����ȴ�. 
             * parant�� null�̸� poolalble parent�� DontDestroyOnLoad���� hierarchy(Scene,Cam��)���� �������־� DontDestroyOnLoad�� ���������� �Ѵ�.
             */
            if (parent == null) 
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;

    public void Init()
    {
        // Pool �ֻ��� Root ����
        if(_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject original, int count = 10)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root; // �ֻ�� @Pool_Root�� Parent�� ����

        _pool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        
        if(_pool.ContainsKey(name) == false)
        {
            // Ǯ�� Ű�� ������ (Ǯ��X) ����
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        // ����� ������ Ǯ�� �ٽ� ��ȯ�Ѵ�
        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original); //�����ü� �ִ� Pool�� ���ٸ� ����

        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;

        // ������ ��ȯ�Ѵ�.
        return _pool[name].Original; 
    }

    public void Clear()
    {
        // @Pool_Root ������ child pool���� ���� �����ش�.

        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
