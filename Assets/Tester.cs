using UnityEngine;
using OscJack;

public class Tester : MonoBehaviour
{
    void Update()
    {
        var data = OscMaster.Instance.GetData("/audio/loud");
        if (data != null) {
            var loud = (float)data[0];
            transform.localScale = Vector3.one * loud;
            Debug.Log(loud);
        }
    }
}
