using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenu : MonoBehaviour {

	public Canvas quitMenu;
	public Button play1;
	public Button play2;
	public Button quit;

	// Use this for initialization
	void Start () {

		quitMenu = quitMenu.GetComponent<Canvas> ();
		quitMenu.enabled = false;
		play1 = play1.GetComponent<Button> ();
		play2 = play2.GetComponent<Button> ();
		quit = quit.GetComponent<Button> ();

	}

	public void OnQuitClick(){
		play1.enabled = false;
		play2.enabled = false;
		quit.enabled = false;
		quitMenu.enabled = true;

	}

	public void OnQuitAbort(){
		play1.enabled = true;
		play2.enabled = true;
		quit.enabled = true;
		quitMenu.enabled = false;

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
	
	}
}
