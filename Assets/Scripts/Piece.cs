using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
	public int id = -1;
	public int position = -1;
	public bool isDocked = true;
	public bool isPicked = false;
	public bool hasExited = false;
	public Player myPlayer;
	public Tile myTile;
	public Tile lastTile;
	public Sprite pieceHighlight;

	private bool myTurn = false;
	private bool canMove = false;
	private Sprite mySprite;
	private Tile possibleMove;
	private int possibleMovePosition;

	void Start(){
		mySprite = this.GetComponent<SpriteRenderer> ().sprite;
	}

	void OnMouseDown(){
		if (myTurn && canMove && !myPlayer.isHoldingPiece) {
			//Debug.Log ("Clicked");
			myPlayer.lastPiecePickedFromDock = isDocked;
			isPicked = true;
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + 0.5f, transform.localPosition.z);

			if (myTile != null) {
				myTile.Reset ();
			}

			myPlayer.alertPickedPiece (id);
		}else if (!myTurn && myTile != null){
			myTile.clicked();
		}
	}

	public void HardReset(){
		position = -1;
		isDocked = true;
		isPicked = false;
		hasExited = false;
		myTurn = false;
		canMove = false;
		possibleMove = null;
		possibleMovePosition = -1;
		myTile = null;
		lastTile = null;

		myPlayer.reshuffleDock ();
	}

	public void setMyTurn(bool t){
		myTurn = t;
		setCanMove (false);
		resetSprite ();
		possibleMove = null;
		possibleMovePosition = -1;
	}

	public void setCanMove(bool m){
		canMove = m;
	}

	public void highlight(){
		this.GetComponent<SpriteRenderer> ().sprite = pieceHighlight;
	}

	public void resetSprite(){
		this.GetComponent<SpriteRenderer> ().sprite = mySprite;
	}

	public void setPossibleMove(Tile t, int i){
		possibleMove = t;
		possibleMovePosition = i;
	}

	public Tile getPossibleMove(){
		return possibleMove;
	}

	public int getPossibleMovePosition(){
		return possibleMovePosition;
	}
}
