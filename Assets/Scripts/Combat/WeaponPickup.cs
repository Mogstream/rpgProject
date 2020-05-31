﻿using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        Fighter fighter;
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;

        private void OnTriggerEnter (Collider other) {
            if (other.gameObject == GameObject.FindWithTag ("Player")) {
                Pickup (other.GetComponent<Fighter> ());
            }
        }

        private void Pickup (Fighter fighter) {
            fighter.EquipWeapon (weapon);
            StartCoroutine (HideForSeconds (respawnTime));
        }

        private IEnumerator HideForSeconds (float seconds) {
            ShowPickup (false);
            yield return new WaitForSeconds (seconds);
            ShowPickup (true);
        }

        private void ShowPickup (bool shouldshow) {
            GetComponent<Collider> ().enabled = shouldshow;
            foreach (Transform child in transform) {
                child.gameObject.SetActive (shouldshow);
            }
        }

        public bool HandleRaycast (PlayerController callingController)
        {
            if (Input.GetMouseButtonDown (0)) {
                Pickup (callingController.GetComponent<Fighter> ());
            }
            return true;
        }

        public CursorType GetCursorType (CursorType cursorType)
        {
            return CursorType.WeaponPickup;
        }
    }
}