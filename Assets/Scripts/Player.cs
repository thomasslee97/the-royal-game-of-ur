using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public Tile[] playerPath;
	public Tile endTile;
	public Piece[] pieces;
	public int playerID;
	public int heldPiece = -1;
	public bool isHoldingPiece = false;
	public PlayerController controller;
	public GameObject endDock;
	public GameObject startDock;
	public GameObject noMoves;
	public Dice myDice;
	public bool lastPiecePickedFromDock = false;

	private int heldPieces = 7;
	private int dockedPieces = 0;
	private int lastDiceRoll = -1;
	private bool myTurn = false;

	void Start(){
		noMoves.SetActive (false);

		for (int i = 0; i < pieces.Length; i++) {
			pieces [i].id = i;
			pieces [i].myPlayer = this;
		}
	}

	void Update(){
		if (Input.GetMouseButton (1)) {
			if (isHoldingPiece) {
				DropHeldPiece ();
			}
		}
	}

	public void setMyTurn(bool t){
		myTurn = t;

		for (int i = 0; i < pieces.Length; i++) {
			pieces [i].setMyTurn(t);
		}
	}

	public void DropHeldPiece(){
		pieces [heldPiece].transform.position = new Vector3 (pieces [heldPiece].transform.position.x, pieces [heldPiece].transform.position.y - 0.5f, pieces [heldPiece].transform.position.z);

		isHoldingPiece = false;

		pieces [heldPiece].getPossibleMove().resetSprite ();

		if (lastPiecePickedFromDock) {
			pieces [heldPiece].HardReset ();
		} else {
			pieces [heldPiece].myPlayer.placePiece (pieces [heldPiece].lastTile, false);
		}
		
		heldPiece = -1;

		setMyTurn (true);

		calculatePossibleMoves (lastDiceRoll);
	}

	public void calculatePossibleMoves(int diceRoll){
		myDice.Roll (diceRoll);

		if (diceRoll == 0) {
			StartCoroutine (DisplayRollAndWait (true));
		} else {

			bool validMoveFound = false;

			for (int i = 0; i < pieces.Length; i++) {
				if (pieces [i].hasExited) {
					continue;
				}
			
				lastDiceRoll = diceRoll;
			
				int potentialPosition = pieces [i].position + diceRoll;

				//Debug.Log ("Checking: " + i);
				if (isValidMove (potentialPosition, playerPath.Length)) {
					//Debug.Log ("Can move: " + i);

					validMoveFound = true;

					pieces [i].highlight ();
					pieces [i].setCanMove (true);

					if (potentialPosition >= playerPath.Length) {
						pieces [i].setPossibleMove (endTile, -1);
					} else {
						pieces [i].setPossibleMove (playerPath [potentialPosition], potentialPosition);
					}
				}
			}

			if (!validMoveFound) {
				StartCoroutine (DisplayRollAndWait (false));
			}
		}
	}

	public void alertPickedPiece(int id){
		heldPiece = id;
		isHoldingPiece = true;

		for (int i = 0; i < pieces.Length; i++) {
			if (pieces [i].isPicked) {
				pieces [i].getPossibleMove ().highlight ();
			} else {
				pieces [i].resetSprite ();
			}

			pieces [i].setCanMove (false);
		}
	}

	public void placePiece(Tile tile, bool endTurn){
		heldPieces--;

		isHoldingPiece = false;

		if (tile.occupyingPlayer != -1) {
			tile.myPiece.HardReset ();
		}

		if (tile.isEndDock) {
			pieces [heldPiece].transform.position = new Vector3 (endDock.transform.position.x - (1.0f * dockedPieces), endDock.transform.position.y, endDock.transform.position.z);
			pieces [heldPiece].resetSprite ();

			pieces [heldPiece].hasExited = true;

			dockedPieces++;
		} else {
			pieces [heldPiece].transform.position = new Vector3 (tile.transform.position.x, tile.transform.position.y, tile.transform.position.z - 1.0f);
			pieces [heldPiece].resetSprite ();

			int position = pieces [heldPiece].getPossibleMovePosition ();

			tile.isOccupied = true;
			tile.occupyingPlayer = playerID;
			tile.myPiece = pieces [heldPiece];

			if (endTurn) {
				pieces [heldPiece].position = position;
			}
			pieces [heldPiece].myTile = tile;
		}

		tile.resetSprite ();
		tile.isHighlighted = false;

		pieces [heldPiece].isPicked = false;
		pieces [heldPiece].isDocked = false;
		pieces [heldPiece].lastTile = tile;

		reshuffleDock ();

		heldPiece = -1;

		if (dockedPieces == 7) {
			Debug.Log ("Player " + (playerID + 1) + " wins");
			controller.alertPlayerWon (playerID);
		} else {
			if (endTurn) {
				controller.PlayerFinishedTurn ();
			}
		}
	}

	public void reshuffleDock(){
		//Debug.Log ("Shuffling");
		int piecesReDocked = 0;

		for (int i = 0; i < pieces.Length; i++) {
			if (pieces [i].isDocked && !pieces[i].hasExited) {
				pieces [i].transform.position = new Vector3 (startDock.transform.position.x + (1.0f * piecesReDocked), startDock.transform.position.y, startDock.transform.position.z);
				piecesReDocked++;
			}
		}
	}

	private bool isValidMove(int pos, int maxPos){
		bool valid = true;

		if (pos >= maxPos) {
			if (pos == maxPos) {
				return true;
			} else {
				return false;
			}
		}

		if (pos > -1) {
			Tile tile = playerPath [pos];

			if (tile.isOccupied) {
				if (tile.occupyingPlayer == playerID) {
					valid = false;
				}

				if (tile.isRosette && tile.isSafeZone) {
					valid = false;
				}
			}

			return valid;
		} else {
			return false;
		}
	}

	IEnumerator DisplayRollAndWait(bool zero){
		if (!zero) {
			noMoves.SetActive (true);
		}
		yield return new WaitForSeconds (1);
		noMoves.SetActive (false);
		controller.PlayerFinishedTurn ();
	}
}
