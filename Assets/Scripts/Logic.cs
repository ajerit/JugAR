using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Logic : MonoBehaviour, IVirtualButtonEventHandler {

	public float m_ButtonReleaseTimeDelay;
	VirtualButtonBehaviour[] virtualButtonBehaviours;

	bool turnoJugador = true;
	bool ganaJugador = false;
	bool ganaNPC = false;
	bool empate = false;

	public Text winText;
	public Text loseText;
	public Text tieText;

	// Puntos centrales de los planos
	public Transform[] modelPlaces;
	// Planos para las figuras
	public GameObject[] planePlaces;
	// Figuras de los jugadores
	public Transform[] modelPrefabs;

	int turnosJugador = 0;
	int turnosNPC = 0;
	public char[] Tablero;
	GameObject[] spawnPoints;
	GameObject currentPoint;
	int index;
	public string AirClick;
	public bool Clicked = false;
	public bool hit;

	public Material winMaterial;

	// Variables de figuras
	Transform X;
	Transform O;

	public void ResetGame() 
	{
		Tablero = new char[9];
		turnosJugador = 0;
		turnosNPC = 0;
		ganaJugador = false;
		ganaNPC = false;
		empate = false;
		Clicked = false;
		turnoJugador = true;

		if (GameObject.FindGameObjectsWithTag ("XOdelete") != null) {
			for (int i = 0; i < GameObject.FindGameObjectsWithTag ("XOdelete").Length; i++) {
				Destroy (GameObject.FindGameObjectsWithTag ("XOdelete") [i]);
			}
		}

		for (int i = 0; i < 9; i++) {
			if (!planePlaces [i].activeSelf)
				planePlaces [i].SetActive (true);
		}

		winText.enabled = false;
		loseText.enabled = false;
		tieText.enabled = false;

	}

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonPressed: " + vb.VirtualButtonName);
		ResetGame();
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

	// Use this for initialization
	void Start () {
		Tablero = new char[9];
		virtualButtonBehaviours = GetComponentsInChildren<VirtualButtonBehaviour>();

        for (int i = 0; i < virtualButtonBehaviours.Length; ++i)
        {
            virtualButtonBehaviours[i].RegisterEventHandler(this);
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit();

		if (turnoJugador) {
			
			if ((Input.GetMouseButtonUp(0) || Clicked) && !ganaNPC && !empate) {
				Debug.Log ("if mousebutton");
				RaycastHit hitInfo = new RaycastHit ();
				
				// Determinar donde ocurre el hit
				if (Clicked)
					hit = true;
				else
					hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

				if (hit) {
					AirClick = hitInfo.collider.name;

					turnosJugador++;
					turnoJugador = false;
					
					// Actualizar tablero con la jugada y mostrar 
					for (int i = 0; i < 9; i++) {
						if (AirClick == planePlaces[i].name) {
							Tablero[i] = 'X';
							Debug.Log ("JUGADOR PUSO EN: " + (i+1));
							GameObject.Find(planePlaces[i].name).SetActive(false);
							X = Instantiate(modelPrefabs[0], modelPlaces[i].transform) as Transform;
							X.transform.position = modelPlaces[i].transform.position;
							X.transform.rotation = planePlaces[i].transform.rotation;
							X.name = "x" + i;
							X.gameObject.SetActive(true);
							X.gameObject.tag = "XOdelete";
							//Debug.Log ("XXXXXXXXXXXXXX:" + X);
							//Vector3 pos = X.transform.position;
							//X.transform.position = new Vector3(pos.x, pos.y + 0.4f, pos.z);
						}
					}
					// Detectar jugada ganadora
					if (Tablero [0] == 'X' && Tablero [1] == 'X' && Tablero [2] == 'X') {
						ganaJugador = true;
						ColorearGanador("x", 0, 1, 2);
						Debug.Log ("You Win! - 1st row " + Tablero [0] + " " + Tablero [1] + " " + Tablero [2]);
					}
					if (Tablero [3] == 'X' && Tablero [4] == 'X' && Tablero [5] == 'X') {
						ganaJugador = true;
						ColorearGanador("x", 3, 4, 5);
						Debug.Log ("You Win! - 2nd row " + Tablero [3] + " " + Tablero [4] + " " + Tablero [5]);
					}
					if (Tablero [6] == 'X' && Tablero [7] == 'X' && Tablero [8] == 'X') {
						ganaJugador = true;
						ColorearGanador("x", 6, 7, 8);
						Debug.Log ("You Win! - 3rd row " + Tablero [6] + " " + Tablero [7] + " " + Tablero [8]);
					}
					if (Tablero [0] == 'X' && Tablero [3] == 'X' && Tablero [6] == 'X') {
						ganaJugador = true;
						ColorearGanador("x", 0, 3, 6);
						Debug.Log ("You Win! - 1st column " + Tablero [0] + " " + Tablero [3] + " " + Tablero [6]);
					}
					if (Tablero [1] == 'X' && Tablero [4] == 'X' && Tablero [7] == 'X') {
						ganaJugador = true;
						ColorearGanador("x", 1, 4, 7);
						Debug.Log ("You Win! - 2nd column " + Tablero [1] + " " + Tablero [4] + " " + Tablero [7]);
					}
					if (Tablero [2] == 'X' && Tablero [5] == 'X' && Tablero [8] == 'X') {
						ganaJugador = true;
						ColorearGanador("x", 2, 5, 8);
						Debug.Log ("You Win! - 3rd column " + Tablero [2] + " " + Tablero [5] + " " + Tablero [8]);
					}
					if (Tablero [0] == 'X' && Tablero [4] == 'X' && Tablero [8] == 'X') {
						ganaJugador = true;
						ColorearGanador("x", 0, 4, 8);
						Debug.Log ("You Win! - 1st Diagonal " + Tablero [0] + " " + Tablero [4] + " " + Tablero [8]);
					}
					if (Tablero [2] == 'X' && Tablero [4] == 'X' && Tablero [6] == 'X') {
						ganaJugador = true;
						ColorearGanador("x", 2, 4, 6);
						Debug.Log ("You Win! - 2nd Diagonal  " + Tablero [2] + " " + Tablero [4] + " " + Tablero [6]);
					}

					if (turnosJugador == 5 && !ganaJugador) {
						empate = true;
						Debug.Log("Empate " + turnosJugador);	
					}		
				}
				Clicked = false;
			}
			// Juega el NPC
			if (!turnoJugador && !ganaJugador && !empate) {
				StartCoroutine(JugadaNPC());
			}

			if (ganaJugador) {
				Debug.Log("GANA JUGADOR");
				winText.enabled = true;
			} else if (ganaNPC) {
				Debug.Log("GANA PC");
				loseText.enabled = true;
			} else if (empate) {
				Debug.Log("EMPATE");
				tieText.enabled = true;
			}
		}	
	}

	// Colorear de Rojo las fichas ganadoras
	void ColorearGanador (string who, int fUno, int fDos, int fTres)
	{
		Renderer[] renderers;

		renderers = GameObject.Find(who+fUno).GetComponentsInChildren<Renderer>();
		if (renderers != null) {
			foreach (Renderer item in renderers)
			{
				item.material = winMaterial;
			}
		}
		renderers = GameObject.Find(who+fDos).GetComponentsInChildren<Renderer>();
		if (renderers != null) {
			foreach (Renderer item in renderers)
			{
				item.material = winMaterial;
			}
		}
		renderers = GameObject.Find(who+fTres).GetComponentsInChildren<Renderer>();
		if (renderers != null) {
			foreach (Renderer item in renderers)
			{
				item.material = winMaterial;
			}
		}
	}

	IEnumerator JugadaNPC()
	{
		yield return new WaitForSeconds (0.4f);

		// NPC calcula jugada: Posicion random disponible
		spawnPoints = GameObject.FindGameObjectsWithTag("Plane");
		index = Random.Range(0, spawnPoints.Length);
		currentPoint = spawnPoints[index];
		turnosNPC++;

		// Actualizar tablero con la jugada y mostrar 
		for (int i = 0; i < 9; i++) {
			if (currentPoint.name == planePlaces[i].name) {
				Tablero[i] = 'O';
				Debug.Log ("PC PUSO EN: " + (i+1));
				GameObject.Find(planePlaces[i].name).SetActive(false);
				O = Instantiate(modelPrefabs[1], modelPlaces[i].transform) as Transform;
				O.transform.position = modelPlaces[i].transform.position;
				O.transform.rotation = planePlaces[i].transform.rotation;
				O.name = "o" + i;
				O.gameObject.SetActive(true);
				O.gameObject.tag = "XOdelete";
			}
		}
		if (Tablero [0] == 'O' && Tablero [1] == 'O' && Tablero [2] == 'O') {
			ganaNPC = true;
			ColorearGanador("o", 0, 1, 2);
			Debug.Log ("Computer Wins! - 1st row " + Tablero [0] + " " + Tablero [1] + " " + Tablero [2]);
		}
		if (Tablero [3] == 'O' && Tablero [4] == 'O' && Tablero [5] == 'O') {
			ganaNPC = true;
			ColorearGanador ("o", 3, 4, 5);
			Debug.Log ("Computer Wins! - 2nd row " + Tablero [3] + " " + Tablero [4] + " " + Tablero [5]);
		}
		if (Tablero [6] == 'O' && Tablero [7] == 'O' && Tablero [8] == 'O') {
			ganaNPC = true;
			ColorearGanador ("o", 6, 7, 8);
			Debug.Log ("Computer Wins! - 3rd row " + Tablero [6] + " " + Tablero [7] + " " + Tablero [8]);
		}
		if (Tablero [0] == 'O' && Tablero [3] == 'O' && Tablero [6] == 'O') {
			ganaNPC = true;
			ColorearGanador ("o", 0, 3, 6);
			Debug.Log ("Computer Wins! - 1st column " + Tablero [0] + " " + Tablero [3] + " " + Tablero [6]);
		}
		if (Tablero [1] == 'O' && Tablero [4] == 'O' && Tablero [7] == 'O') {
			ganaNPC = true;
			ColorearGanador ("o", 1, 4, 7);
			Debug.Log ("Computer Wins! - 2nd column " + Tablero [1] + " " + Tablero [4] + " " + Tablero [7]);
		}
		if (Tablero [2] == 'O' && Tablero [5] == 'O' && Tablero [8] == 'O') {
			ganaNPC = true;
			ColorearGanador ("o", 2, 5, 8);
			Debug.Log ("Computer Wins! - 3rd column " + Tablero [2] + " " + Tablero [5] + " " + Tablero [8]);
		}
		if (Tablero [0] == 'O' && Tablero [4] == 'O' && Tablero [8] == 'O') {
			ganaNPC = true;
			ColorearGanador ("o", 0, 4, 8);
			Debug.Log ("Computer Wins! - 1st Diagonal " + Tablero [0] + " " + Tablero [4] + " " + Tablero [8]);
		}
		if (Tablero [2] == 'O' && Tablero [4] == 'O' && Tablero [6] == 'O') {
			ganaNPC = true;
			ColorearGanador ("o", 2, 4, 6);
			Debug.Log ("Computer Wins! - 2nd Diagonal  " + Tablero [2] + " " + Tablero [4] + " " + Tablero [6]);
		}

		if (turnosNPC == 5 && !ganaNPC) {
		empate = true;
		Debug.Log ("Empate " + turnosNPC);
		}

		turnoJugador = true;
	}

}
