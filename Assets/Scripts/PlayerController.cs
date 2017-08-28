using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public Player[] players;
	public GameObject winPlayerOne;
	public GameObject winPlayerTwo;

	private int currentPlayerTurn = 0;
	private bool endOfGame = false;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < players.Length; i++) {
			players [i].controller = this;
		}

		winPlayerOne.SetActive (false);
		winPlayerTwo.SetActive (false);

		NextPlayerTurn ();		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void NextPlayerTurn(){
		players [currentPlayerTurn].setMyTurn (true);
		Debug.Log ("Player: " + currentPlayerTurn);

		int moves = RollDice ();

		Debug.Log ("Dice roll: " + moves);

		players [currentPlayerTurn].calculatePossibleMoves (moves);
	}

	public void PlayerFinishedTurn (){
		players [currentPlayerTurn].myDice.Reset ();
		players [currentPlayerTurn].setMyTurn (false);

		if (currentPlayerTurn == 0) {
			currentPlayerTurn = 1;
		} else {
			currentPlayerTurn = 0;
		}

		NextPlayerTurn ();
	}

	private int RollDice(){
		Random.InitState((int)System.DateTime.Now.Ticks);

		return Random.Range (0, 4);
	}

	public void alertPlayerClicked(Tile tile){
		players [currentPlayerTurn].placePiece (tile, true);
	}

	public void alertPlayerWon(int id){
		if (id == 0) {
			winPlayerOne.SetActive (true);
		} else {
			winPlayerTwo.SetActive (true);
		}

		StartCoroutine (WaitAndReload ());
	}

	IEnumerator WaitAndReload(){
		yield return new WaitForSeconds (2);
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}
}
