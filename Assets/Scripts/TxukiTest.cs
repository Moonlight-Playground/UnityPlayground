using UnityEngine;
using Moonlight;

public class TxukiTest : MonoBehaviour
{
    protected void Start()
    {

    }

    [ContextMenu("Testing Method")]
    public void TestingMethod()
    {
        GameObject go = new GameObject();

    }

    public void Print()
    {
        Debug.Log("Text");
    }
}
