using UnityEngine;
using OscJack;

public class Tester : MonoBehaviour
{
    void Update()
    {
        var data = OscMaster.GetData("/audio/loud");
        if (data != null) {
            var loud = (float)data[0];
            transform.localScale = Vector3.one * loud;
            Debug.Log(loud);
        }

		if (OscMaster.HasData ("/audio/attack")) {
			Camera.main.backgroundColor = Color.red;
			OscMaster.Remove ("/audio/attack");
		} else {
			Camera.main.backgroundColor = new Color(0.2f, 0.3f, 0.5f, 1.0f);
		}
    }
}
