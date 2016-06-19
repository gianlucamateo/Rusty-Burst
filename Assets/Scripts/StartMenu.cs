using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenu : MonoBehaviour {

	public Canvas quitMenu;
	public Canvas showControls;
	public Button play1;
	public Button play2;
	public Button controls;
	public Button quit;
	public Image controlHalo1;
	public Image controlHalo2;

	// Use this for initialization
	void Start () {

		quitMenu = quitMenu.GetComponent<Canvas> ();
		showControls = showControls.GetComponent<Canvas> ();
		quitMenu.enabled = false;
		showControls.enabled = false;
		controlHalo1 = controlHalo1.GetComponent<Image> ();
		controlHalo2 = controlHalo2.GetComponent<Image> ();
		play1 = play1.GetComponent<Button> ();
		play2 = play2.GetComponent<Button> ();
		quit = quit.GetComponent<Button> ();
		controls = controls.GetComponent<Button> ();

	}

	public void OnQuitClick(){
		play1.enabled = false;
		play2.enabled = false;
		quit.enabled = false;
		controls.enabled = false;
		quitMenu.enabled = true;

	}

	public void OnQuitAbort(){
		play1.enabled = true;
		play2.enabled = true;
		quit.enabled = true;
		controls.enabled = true;
		quitMenu.enabled = false;

	}
	public void OnControlsClick(){
		play1.enabled = false;
		play2.enabled = false;
		quit.enabled = false;
		controls.enabled = false;
		showControls.enabled = true;

	}
	public void OnControlsBack(){
		play1.enabled = true;
		play2.enabled = true;
		quit.enabled = true;
		controls.enabled = true;
		showControls.enabled = false;

	}

	public void OnPlay1(){
		SceneManager.LoadScene("Track1");
	}

	public void OnPlay2(){
		SceneManager.LoadScene("Track2");
	}
	public void OnQuitConfirm(){
		Application.Quit ();
	}

	// Update is called once per frame
	void Update () {
	
		if(showControls.enabled == true){

//			if (Input.GetAxis ("Joy1Steering")>0.1) {
//				Debug.Log (Input.GetAxis ("Joy1Steering"));
//			}
//
//			if (Input.GetAxis ("Joy1Steering")!=0) {
//				Debug.Log ("joy2");
//			}

			if (System.Math.Abs(Input.GetAxis ("Joy1Steering"))>0.1) {
				controlHalo1.color = new Color32 (220, 0, 0, 255);
			} else {
				controlHalo1.color = new Color32 (255, 255, 255, 30);
			}

			if (System.Math.Abs(Input.GetAxis ("Joy2Steering"))>0.1) {
				controlHalo2.color = new Color32 (0, 0, 220, 255);
			} else {
				controlHalo2.color = new Color32 (255, 255, 255, 30);
			}
//			if (Input.GetKey (KeyCode.Return)) {
//				controlHalo1.color = new Color32 (220, 0, 0, 255);
//			} else {
//				controlHalo1.color = new Color32 (255, 255, 255, 30);
//			}
//
//			if (Input.GetKey (KeyCode.Escape)) {
//				controlHalo2.color = new Color32 (0, 0, 220, 255);
//			} else {
//				controlHalo2.color = new Color32 (255, 255, 255, 30);
//			}
		}
	}

}
