﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TutorialManager : MonoBehaviourPunCallbacks
{
    [Header("WASD")]
    [SerializeField]
    private GameObject wSprite;
    [SerializeField]
    private GameObject aSprite;
    [SerializeField]
    private GameObject sSprite;
    [SerializeField]
    private GameObject dSprite;
    [SerializeField]
    private GameObject playerMovement;

    [Header("Mouse")]
    [SerializeField]
    private GameObject mouse;
    [SerializeField]
    private GameObject moveMouse;

    [Header("GuardDistraction")]
    [SerializeField]
    private GameObject throwRock;
    
    private GameObject WallLifts1;
    private GameObject WallLifts2;
    private GameObject WallLifts3;
    private GameObject WallLifts4;

    [Header("AdvanceToNextRoom")]
    [SerializeField]
    private GameObject advanceToNext;
    [SerializeField]
    private GameObject arrow;

    private GameObject cageGuard;
    private GameObject guardToDrag;

    [Header("Player")]
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject camera;

    private int tutorialCounter;
    private bool wPressed;
    private bool aPressed;
    private bool sPressed;
    private bool dPressed;
    private int keysPressed;
    private int cubesLookedAt;

    // Start is called before the first frame update
    void Start()
    {
        tutorialCounter = 0;
        keysPressed = 0;
        WallLifts1 = GameObject.Find("Room1").transform.GetChild(2).gameObject;
        WallLifts2 = GameObject.Find("Room2").transform.GetChild(1).gameObject;
        WallLifts3 = GameObject.Find("Room3").transform.GetChild(1).gameObject;
        cageGuard = GameObject.Find("Guards").transform.GetChild(0).gameObject;
        
       
    }

    // Update is called once per frame
    void Update()
    {
        // checks for when you press all of W A S D
        if (tutorialCounter == 0)
        {
            // checks each keycode and sets the bool to true + increases keysPressed counter
            if (Input.GetKeyDown(KeyCode.W) && !wPressed)
            {
                wPressed = true;
                keysPressed++;
                wSprite.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.A) && !aPressed)
            {
                aPressed = true;
                keysPressed++;
                aSprite.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.S) && !sPressed)
            {
                sPressed = true;
                keysPressed++;
                sSprite.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.D) && !dPressed)
            {
                dPressed = true;
                keysPressed++;
                dSprite.SetActive(true);
            }
            // once 4 keys are pressed, we destory the tooltip form the UI and move onto the next tutorial part
            if (keysPressed == 4)
            {
                tutorialCounter++;
                Destroy(playerMovement);
                // set active the mouse AI tooltip
                mouse.SetActive(true);
            }
        }
        // short animation to tell you to use the mouse to move the camera
        else if (tutorialCounter == 1)
        {
            // UI element has a script on itself that does a short animation and then deactivates, once it is deactivated we move onto next tutorial part
            if (mouse.activeSelf == false)
            {
                tutorialCounter++;
                // sets active the move mouse to red cubes tooltip
                moveMouse.SetActive(true);
            }
        }
        // checks if if the player is looking at the 2 cubes in the scene
        else if (tutorialCounter == 2)
        {
            // shoots out a raycast to see if user looks at cube
            Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hitInfo);

            // checks if the thing the raycast has collided with is the cube
            if(hitInfo.collider != null)
                if (hitInfo.collider.gameObject.tag == "Cube")
                {
                    // if it is the cube, we increase how many cubes we have looked at and destroy the red cube, revealing a green cube
                    cubesLookedAt++;
                    Destroy(hitInfo.collider.gameObject);
                }

            // if looked at both cubes, destroy the UI tooltip and move onto next part
            if (cubesLookedAt == 2)
            {
                tutorialCounter++;
                Destroy(moveMouse);
            }
        // this part moves the wall up and allows the user to proceed onto the next part
        } else if(tutorialCounter == 3) {

            // if the user is still in the old room, wall continues to rise
            if(transform.position.z < 93) 
            {
                if (advanceToNext.activeSelf == false)
                    advanceToNext.SetActive(true);

                WallLifts1.transform.position = new Vector3(WallLifts1.transform.position.x , WallLifts1.transform.position.y + 0.05f, WallLifts1.transform.position.z);
            }
            // if user is in the new room, wall comes back and we move onto next tutorial
            else 
            {
                advanceToNext.SetActive(false);
                WallLifts1.transform.position = new Vector3(WallLifts1.transform.position.x, 3, WallLifts1.transform.position.z);
                tutorialCounter++;
                throwRock.SetActive(true);
            }
        // checks if guard gets alerted by rock
        } else if (tutorialCounter == 4)
        {
            // once guard gets alerted by rock we can move onto the next part
            if (cageGuard.GetComponent<GuardAIPhoton>().GuardState == GuardAIPhoton.State.Alerted)
            {
                tutorialCounter++;
                Destroy(throwRock);
            }
        // this part moves the wall up and allows the user to proceed onto the next part
        } else if (tutorialCounter == 5)
        {
            // if the user is still in the old room, wall continues to rise
            if (transform.position.z < 142)
            {
                if (advanceToNext.activeSelf == false)
                    advanceToNext.SetActive(true);

                WallLifts2.transform.position = new Vector3(WallLifts2.transform.position.x, WallLifts2.transform.position.y + 0.05f, WallLifts2.transform.position.z);
            }
            // if user is in the new room, wall comes back and we move onto next tutorial
            else
            {
                advanceToNext.SetActive(false);
                WallLifts2.transform.position = new Vector3(WallLifts2.transform.position.x, 3, WallLifts2.transform.position.z);
                tutorialCounter++;
            }
        // checks if all the shooting range guards have been killed
        } else if (tutorialCounter == 6)
        {
            // once all guards have been found to be dead
            if (GameObject.Find("Environment/Interactables/DeadGuards").GetComponentsInChildren<Transform>().Length == 9)
            {
                // get a random number and set the guard to drag to be that random guard
                int random = Random.Range(0, 4);
                guardToDrag = GameObject.Find("Environment/Interactables/DeadGuards").transform.GetChild(random).gameObject;

                // spawn an arrow above that guard and set the arrow's position to be slightly above the guard
                Vector3 arrowPos = new Vector3(guardToDrag.transform.position.x, guardToDrag.transform.position.y + 5, guardToDrag.transform.position.z);
                Instantiate(arrow, arrowPos, arrow.transform.rotation);
                tutorialCounter++;
            }
        // this part moves the wall up and allows the user to proceed onto the next part
        }
        else if (tutorialCounter == 7)
        {
            // if the user and dead guard are still in the old room, wall continues to rise
            if (transform.position.x < 273 || guardToDrag.transform.position.x < 273)
            {
                if (advanceToNext.activeSelf == false)
                    advanceToNext.SetActive(true);

                WallLifts3.transform.position = new Vector3(WallLifts3.transform.position.x, WallLifts3.transform.position.y + 0.05f, WallLifts3.transform.position.z);
            }
            // if user and dead guard are in the new room, wall comes back and we move onto next tutorial
            else
            {
                advanceToNext.SetActive(false);
                WallLifts3.transform.position = new Vector3(WallLifts3.transform.position.x, 3, WallLifts3.transform.position.z);
                tutorialCounter++;
            }
        }


        //Debug.Log(tutorialCounter);
    }

}
