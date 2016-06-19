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

	// Use this for initialization
	void Start () {

		quitMenu = quitMenu.GetComponent<Canvas> ();
		showControls = showControls.GetComponent<Canvas> ();
		quitMenu.enabled = false;
		showControls.enabled = false;
		controlHalo1 = controlHalo1.GetComponent<Image> ();
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
			if (Input.GetKey (KeyCode.Return)) {
				controlHalo1.enabled = true;
			} else {
				controlHalo1.enabled = false;
			}

			if (Input.GetKey (KeyCode.Escape)) {
				controlHalo1.enabled = true;
			} else {
//				controlHalo1.enabled = false;
			}
		}
	}

}
