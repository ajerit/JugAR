using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VirtualButtonEventHandler : MonoBehaviour, IVirtualButtonEventHandler {

    public float velocidadInicial = 30f;
	public float m_ButtonReleaseTimeDelay;
	VirtualButtonBehaviour[] virtualButtonBehaviours;
    private Rigidbody pelota;
    private Pelota script;

	// Use this for initialization
	void Start () {
		        // Register with the virtual buttons TrackableBehaviour
        virtualButtonBehaviours = GetComponentsInChildren<VirtualButtonBehaviour>();

        for (int i = 0; i < virtualButtonBehaviours.Length; ++i)
        {
            virtualButtonBehaviours[i].RegisterEventHandler(this);
        }
        
        pelota = GameObject.Find("Ball").GetComponent<Rigidbody>(); 
        script = GameObject.Find("Ball").GetComponent<Pelota>();
		
	}
	
    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonPressed: " + vb.VirtualButtonName);

        pelota.AddForce(new Vector3(velocidadInicial, 0, velocidadInicial));
        pelota.isKinematic = false;
        script.pelotaEnMov = true;

        BroadcastMessage("HandleVirtualButtonPressed", SendMessageOptions.DontRequireReceiver);
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonReleased: " + vb.VirtualButtonName);


        StartCoroutine(DelayOnButtonReleasedEvent(m_ButtonReleaseTimeDelay, vb.VirtualButtonName));
    }

    IEnumerator DelayOnButtonReleasedEvent(float waitTime, string buttonName)
    {
        yield return new WaitForSeconds(waitTime);

        BroadcastMessage("HandleVirtualButtonReleased", SendMessageOptions.DontRequireReceiver);
    }
}
