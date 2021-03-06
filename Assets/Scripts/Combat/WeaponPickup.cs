﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        Fighter fighter;
        [SerializeField] Weapon weapon = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == GameObject.FindWithTag("Player"))
            { 
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                Destroy(gameObject);               
            }
        }
    }

}