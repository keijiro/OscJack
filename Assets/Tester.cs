using UnityEngine;
using OscJack;

public class Tester : MonoBehaviour
{
    void Update()
    {
        var data = OscDirectory.Instance.GetData("/audio/loud");
        if (data != null) Debug.Log(data[0]);
    }
}
