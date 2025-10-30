using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̳���MonoBehaviour�ĵ���ģʽ����
/// </summary>
//Լ�����̳иû���������̳�MonoBehaviour
public class SingletonMonoBase<T> : MonoBehaviour where T : MonoBehaviour
{
    //volatile�ؼ��֣���֤���߳��µĿɼ���
    private static volatile T _instance;
    private static object _lock = new object(); //������
    
    public static T Instance
    {
        get
        {
            //˫�ؼ����DCL��֤�����������̰߳�ȫ
            if (!_instance)
            {
                lock (_lock)
                {
                    if (!_instance)
                    {
                        GameObject obj = new GameObject();
                        //��̬���ض�Ӧ�ĵ���ģʽ�ű�������ʽ��
                        _instance = obj.AddComponent<T>();
                        //Ϊ������GameObject����
                        obj.name = typeof(T).ToString();
                        //������ʱ���Ƴ����󣬱�֤����������Ϸ���������ж�����
                        DontDestroyOnLoad(obj);
                    }
                }
            }
            return _instance;
        }
    }

    public static T GetInstance()
    {
        //˫�ؼ����ȷ���̰߳�ȫ
        if (!_instance)
        {
            lock (_lock)
            {
                if (!_instance)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                    obj.name = typeof(T).ToString();
                    DontDestroyOnLoad(obj);
                }
            }
        }
        return _instance;
    }

    //���������麯��:�����п��Զ�Awake������д
    protected virtual void Awake()
    {
        //��_instance���и�ֵ
        _instance = this as T;
    }
}
