using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public bool isLastTile = false;
	public bool isRosette = false;
	public bool isSafeZone = false;
	public bool isHighlighted = false;
	public bool isOccupied = false;
	public bool isEndDock = false;
	public int occupyingPlayer = -1;
	public Piece myPiece;
	public Sprite tileHighlight;

	private SpriteRenderer myRenderer;
	private Sprite mySprite;
	private PlayerController controller;

	void Start(){
		myRenderer = GetComponent<SpriteRenderer> ();
		mySprite = myRenderer.sprite;

		controller = GameObject.FindGameObjectsWithTag ("MainCamera")[0].GetComponent<PlayerController> ();
	}

	void OnMouseDown(){
		//Debug.Log ("Clicked");

		clicked ();
	}

	public void clicked(){
		if (isHighlighted) {
			controller.alertPlayerClicked (this);
		}
	}

	public void Reset(){
		resetSprite ();
		occupyingPlayer = -1;
		isOccupied = false;
		isHighlighted = false;
	}

	public void highlight(){
		myRenderer.sprite = tileHighlight;
		isHighlighted = true;
	}

	public void resetSprite(){
		myRenderer.sprite = mySprite;
	}
}
