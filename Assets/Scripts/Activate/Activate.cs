using UnityEngine;

public class Activate : MonoBehaviour
{

    [SerializeField] protected GameObject notification;
    [SerializeField] protected GameObject activatedObject;  //������������ ������ ��� ������������ Buttons

    protected string savePath;
    public delegate void IsActivated(int index);
}
