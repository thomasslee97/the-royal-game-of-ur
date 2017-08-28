using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {

	public GameObject[] spotsBlack;
	public GameObject[] spotsWhite;
	public bool rolled = false;

	// Use this for initialization
	void Start () {
		Reset ();
	}

	public void Reset(){
		foreach (GameObject gob in spotsBlack) {
			gob.SetActive (false);
		}

		foreach (GameObject gob in spotsWhite) {
			gob.SetActive (false);
		}
	}

	public void Roll(int outcome){
		int selected = 0;
		List<int> rolls = new List<int> ();

		while (selected < outcome) {
			int roll = UnityEngine.Random.Range (0, 3);

			if (!rolls.Contains (roll)) {
				rolls.Add (roll);

				spotsWhite [roll].SetActive (true);
				spotsBlack [roll].SetActive (false);

				selected++;
			}
		}

		for (int i = 0; i < 4; i++) {
			if (!spotsWhite [i].activeSelf) {
				spotsBlack [i].SetActive (true);
			}
		}

		rolled = true;
	}
}
