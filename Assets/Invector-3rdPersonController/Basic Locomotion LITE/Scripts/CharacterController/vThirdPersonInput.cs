using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using static GM;
using System.Linq;




#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

namespace Invector.CharacterController
{
    public class vThirdPersonInput : MonoBehaviour
    {
        #region variables

        [Header("Default Inputs")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode caseBoardInput = KeyCode.C;
        public KeyCode focusInput = KeyCode.F;
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;
        public KeyCode interactInput = KeyCode.E;

        [Header("Camera Settings")]
        public string rotateCameraXInput ="Mouse X";
        public string rotateCameraYInput = "Mouse Y";
        [Header("Will's Settings")]
        public GM GM;
        public bool mapCollided = false;
        public bool focused = false;
        public bool caseFocused = false;
        public bool clueTriggered = false;
        public bool doorCollided = false;
        public bool terminalCollided = false;
        public GameObject prompt;
        public GameObject itemToFocus;
        public GameObject dialogue;
        public GameObject door;
        public GameObject terminal;
        public bool clueInvestigated;
        public GameObject screen;
        public ClueCardManager ccm;
        //public GameObject clueToInvestigate;

        protected vThirdPersonCamera tpCamera;                // acess camera info        
        [HideInInspector]
        public string customCameraState;                    // generic string to change the CameraState        
        [HideInInspector]
        public string customlookAtPoint;                    // generic string to change the CameraPoint of the Fixed Point Mode        
        [HideInInspector]
        public bool changeCameraState;                      // generic bool to change the CameraState        
        [HideInInspector]
        public bool smoothCameraState;                      // generic bool to know if the state will change with or without lerp  
        [HideInInspector]
        public bool keepDirection;                          // keep the current direction in case you change the cameraState

        protected vThirdPersonController cc;                // access the ThirdPersonController component                

        #endregion

        protected virtual void Start()
        {
            CharacterInit();
            prompt.SetActive(false);
        }

        protected virtual void CharacterInit()
        {
            cc = GetComponent<vThirdPersonController>();
            if (cc != null)
                cc.Init();

            tpCamera = FindAnyObjectByType<vThirdPersonCamera>();
            if (tpCamera) tpCamera.SetMainTarget(this.transform);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        protected virtual void LateUpdate()
        {
            if (cc == null) return;             // returns if didn't find the controller		    
            InputHandle();                      // update input methods
            UpdateCameraStates();               // update camera states
        }

        protected virtual void FixedUpdate()
        {
            cc.AirControl();
            CameraInput();
        }

        protected virtual void Update()
        {
            cc.UpdateMotor();                   // call ThirdPersonMotor methods               
            cc.UpdateAnimator();                // call ThirdPersonAnimator methods
            //SetFocus();                         // 		               
        }

        protected virtual void InputHandle()
        {
            ExitGameInput();
            InteractInput();
            FocusInput();
            OpenCaseBoard();

            if (!cc.lockMovement)
            {
                MoveCharacter();
                SprintInput();
                StrafeInput();
                JumpInput();
                CameraInput();
            }
        }

        protected virtual void SetFocus()
        {
            if (focused)
            {
                tpCamera._lockOnTarget.position = itemToFocus.transform.localPosition;//.transform;
                tpCamera.currentTarget = tpCamera.target;
            } else
            {
                tpCamera.target = this.transform;
                tpCamera.currentTarget = tpCamera.target;
            }
            
            //focused = true;
            //cc.lockMovement = true;
            //Debug.Log(focused);
        }

        #region Basic Locomotion Inputs      

        protected virtual void MoveCharacter()
        {            
            cc.input.x = Input.GetAxis(horizontalInput);
            cc.input.y = Input.GetAxis(verticallInput);
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
                cc.Strafe();
        }

        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput))
                cc.Sprint(true);
            else if(Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput))
                cc.Jump();
        }

        protected virtual void InteractInput()
        {
            if (doorCollided)
            {
                //prompt.SetActive(true);
                if (!door.GetComponent<Door>().locked)
                {
                    prompt.GetComponent<TMP_Text>().text = "Press E to open";
                }
                else
                {
                    if(door.name.Equals("APT4") && door.GetComponent<Door>().knocked)
                    {
                        prompt.GetComponent<TMP_Text>().text = "Press E to Brute force";
                    }
                    else
                    {
                        prompt.GetComponent<TMP_Text>().text = "Press E to Knock";
                    }
                }
                
            }

            if (terminalCollided)
            {
                prompt.GetComponent<TMP_Text>().text = "Press E to use terminal";

                if (Input.GetKeyDown(interactInput))
                {
                    if (terminal.GetComponent<Terminal>().storageTerminal) { 
                        if (terminal.GetComponent<Terminal>().door.locked) {
                            if (!GM.dialogue.activeInHierarchy)
                                GM.dialogue.SetActive(true);
                            GM.typeWriter._readyForNewText = true;
                            GM.typeWriter.PrepareForNewText(GM.dialogue);
                            terminal.GetComponent<Terminal>().StartTerminal();
                        }
                    } else
                    {
                        if (terminal.GetComponent<Terminal>().door.locked)
                        {
                            terminal.GetComponent<Terminal>().StartTerminal();
                            if (!GM.dialogue.activeInHierarchy)
                                GM.dialogue.SetActive(true);
                            GM.typeWriter.PrepareForNewText(GM.dialogue);
                            
                        }
                    }
                    
                }
            }
            else
            {
                //prompt.SetActive(false);
            }

            if (Input.GetKeyDown(interactInput) && (doorCollided || terminalCollided))
            {
                if(door.GetComponent<Door>().enabled && !door.GetComponent<Door>().locked)
                {
                    door.GetComponent<Door>().OpenDoor();
                    door.GetComponent<Door>().enabled = false;
                    doorCollided = false;
                    prompt.SetActive(false);
                }
                else if(door.GetComponent<Door>().enabled && door.GetComponent<Door>().locked)
                {
                    //Knock();
                    if(!door.name.Equals("APT4"))
                    {
                        Debug.Log("Knock Knock " + door.name);
                        Knock();
                        if(!GM.dialogue.activeInHierarchy)
                            GM.dialogue.SetActive(true);
                        GM.typeWriter._readyForNewText = true;
                        GM.typeWriter.PrepareForNewText(GM.dialogue);
                        GM._inkStory.ChoosePathString("wrongApt");
                        GM.dialogue.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GM._inkStory.Continue();
                        GM._inkStory.Continue();
                    }
                    else
                    {
                        if (door.GetComponent<Door>().knocked)
                        {
                            Debug.Log("Brute Forcing " + door.name);
                            GM.managerAlerted = true;
                            door.GetComponent<Door>().forced = true;
                            door.GetComponent<Door>().UnlockDoor();
                            doorCollided = false;
                            prompt.SetActive(false);
                            if (!GM.dialogue.activeInHierarchy)
                                GM.dialogue.SetActive(true);
                            GM.typeWriter._readyForNewText = true;
                            GM.typeWriter.PrepareForNewText(GM.dialogue);
                            GM._inkStory.ChoosePathString("bruteForce");
                            GM.dialogue.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GM._inkStory.Continue();
                        }
                        else
                        {

                            Knock();
                            if (!GM.dialogue.activeInHierarchy)
                                GM.dialogue.SetActive(true);
                            GM.typeWriter._readyForNewText = true;
                            GM.typeWriter.PrepareForNewText(GM.dialogue);
                            GM._inkStory.ChoosePathString("dextersDoor");
                            GM.dialogue.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GM._inkStory.Continue();
                        }
                        
                    }

                    

                }


            }

        }

        protected virtual void FocusInput()
        {
            if (mapCollided || clueTriggered )
            {
                prompt.SetActive(true);
                if (!focused)
                {
                    prompt.SetActive(true);
                    if (Input.GetKeyDown(focusInput) && !caseFocused)
                    { 
                        focused = true;
                        if (itemToFocus.GetComponent<Clue>()!= null)
                            if(!itemToFocus.GetComponent<Clue>().discovered && clueTriggered)
                                GM.dialogue.SetActive(true);
                        cc.isSprinting = false;
                        cc.input = Vector2.zero;
                        
                        cc.lockMovement = true;
                        //tpCamera.lockCamera = true;
                        //tpCamera.enabled = false;
                        //tpCamera.lockCamera = true;
                        tpCamera._camera.enabled = false;
                        tpCamera.inspectCam.enabled = true;
                        
                        
                        if (mapCollided)
                        {
                            tpCamera._camera.GetComponent<Camera>().fieldOfView = 60;
                            prompt.GetComponent<TMP_Text>().text = "Press F to back out";
                        } 
                        else if (clueTriggered)
                        {
                            
                            InspectClue();
                            prompt.GetComponent<TMP_Text>().text = "Press F to back out";
                        }
                            SetFocus();
                    }
                }
                else
                {
                    if (Input.GetKeyDown(focusInput))
                    {
                        focused = false;
                        cc.lockMovement = false;
                        tpCamera.enabled = true;
                        tpCamera.inspectCam.enabled = false;
                        tpCamera._camera.enabled = true;
                        
                        //tpCamera.ReturnOldRotate();
                        tpCamera.lockCamera = false;
                        if (mapCollided)
                        {
                            tpCamera._camera.GetComponent<Camera>().fieldOfView = 98.5f;
                            prompt.GetComponent<TMP_Text>().text = "Press F to focus";
                        }
                        else if (clueTriggered)
                        {
                            prompt.GetComponent<TMP_Text>().text = "Press F to investigate";
                        }
                        SetFocus();
                    }
                }
            } else
            {
                /*if (Input.GetKeyDown(focusInput) && focused)
                {
                    cc.lockMovement = false;
                    focused = false;
                    tpCamera._camera.enabled = true;
                    tpCamera.inspectCam.enabled = false;
                    tpCamera.lockCamera = false;
                    tpCamera.ReturnOldRotate();
                }*/
                //prompt.SetActive(false);
                //dialogue.SetActive(false);
            }
        }

        protected virtual void OpenCaseBoard()
        {
            if (!caseFocused && !focused)
            {
                if (Input.GetKeyDown(caseBoardInput))
                {
                    if (!Cursor.visible)
                        Cursor.lockState = CursorLockMode.Confined;
                        Cursor.visible = true;
                    cc.isSprinting = false;
                    cc.input = Vector2.zero;
                    tpCamera.enabled = false;
                    GM.caseBoard.SetActive(true);
                    cc.lockMovement = true;
                    caseFocused = true;
                    ccm.UpdateCardPosition();
                }
            }
            else if (caseFocused && !focused)
            {
                if (Input.GetKeyDown(caseBoardInput))
                {
                    if (Cursor.visible)
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                    GM.caseBoard.SetActive(false);
                    cc.lockMovement = false;
                    tpCamera.enabled = true;
                    tpCamera.ReturnOldRotate();
                    caseFocused = false;
                }
            }
        }

        protected virtual void InspectClue()
        {
            Clue foundClue = itemToFocus.GetComponent<Clue>();
                if (focused && foundClue.relevant)
                {
                    
                    if (foundClue.discovered == false)
                    {
                        foundClue.discovered = true;
                        GM.clueCardObj.GetComponent<Clue>().clueName = foundClue.clueName ;
                        foundClue.clueCardObj = Instantiate(GM.clueCardObj, GM.caseBoard.GetComponentInChildren<Transform>());
                        foundClue.clueCardObj.GetComponent<Clue>().discovered = true;
                        //foundClue.clueCardObj.GetComponent<Clue>().clueName = foundClue.clueName + " Clue";
                        GM.cluesFound.Add(foundClue.gameObject);
                        ccm.clueCards.Add(foundClue.clueCardObj);
                        //foundClue.clueCardObj.GetComponent <Clue>().cardClueName.text = foundClue.clueName + " Clue";
                        ccm.clueCards.Last().GetComponent<Clue>().cardClueName = foundClue.clueCardObj.GetComponentInChildren<TextMeshProUGUI>();
                        ccm.clueCards.Last().GetComponent<Clue>().isClueCard = true;
                        GM.dialogue.SetActive(true);
                        GM.DiscoverClue();

                    }
                    
                }
                else if(focused && !foundClue.relevant) 
                {
                if (!GM.dialogue.activeInHierarchy)
                    GM.dialogue.SetActive(true);
                GM.typeWriter._readyForNewText = true;
                GM.typeWriter.PrepareForNewText(GM.dialogue);
                GM._inkStory.ChoosePathString("notRelevant");
                GM.AdvanceDialogue();//GM.dialogue.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GM._inkStory.Continue();
               

            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Focus"))
            {
                mapCollided = true;
                itemToFocus = other.gameObject;
                if (!focused)
                    prompt.GetComponent<TMP_Text>().text = "Press F to focus";
            }
            if (other.gameObject.CompareTag("Clue"))
            {
                clueTriggered = true;
                itemToFocus = other.gameObject;
                if (!focused)
                    prompt.GetComponent<TMP_Text>().text = "Press F to investigate";
            }
            if (other.gameObject.CompareTag("Terminal"))
            {
                terminalCollided = true;
                terminal = other.gameObject;
                //if(Input.GetKeyDown(interactInput))
                    //terminal.GetComponent<Terminal>().StartTerminal();
                prompt.SetActive(true);
                //itemToFocus = other.gameObject;
            }
            if (other.gameObject.CompareTag("Door"))
            {
                doorCollided = true;
                door = other.gameObject;
                if (doorCollided)
                {
                    
                    if (!door.GetComponent<Door>().locked)
                    {
                        prompt.GetComponent<TMP_Text>().text = "Press E to open";
                        
                    }
                    else
                    {
                        if(door.name.Equals("APT4") && door.GetComponent<Door>().knocked)
                        {
                             prompt.GetComponent<TMP_Text>().text = "Press E to Brute force";
                        }
                        else
                            prompt.GetComponent<TMP_Text>().text = "Press E to Knock";
                    }

                }
                prompt.SetActive(true);


            }
                    //GM.cluesFound.Add(other.gameObject);
                    //other.gameObject.GetComponent<Clue>().discovered = true;
                
                //mapToFocus = other.gameObject;
                //InspectClue(other.gameObject);
                //GM.clueList = new Clue { GM.cluesFound.FindIndex(0) };
                /*GM.clueList[] = new Clue
                {

                }*/
                //GM.clueList.clues = new Clue() other.gameObject.GetComponent<Clue>();
                
                //clueToInvestigate = other.gameObject;
            
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Focus"))
            {
                mapCollided = false;
                prompt.SetActive(false);
            }
            if (other.gameObject.CompareTag("Clue"))
            {
                clueTriggered = false;
                prompt.SetActive(false);
            }
            if (other.gameObject.CompareTag("Terminal"))
            {
                terminalCollided = false;
                prompt.SetActive(false);
            }
            if (other.gameObject.CompareTag("Door"))
            {
                doorCollided = false;   
                door  = null;
                prompt.SetActive(false);
            }
        }

        public void Knock()
        {
            door.GetComponent<Door>().knocked = true;
        }


        protected virtual void ExitGameInput()
        {
            // just a example to quit the application 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!Cursor.visible)
                    Cursor.visible = true;
                else
                {
                    Application.Quit();
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                }
            }
        }

        #endregion

        #region Camera Methods

        protected virtual void CameraInput()
        {
            if (tpCamera == null)
                    return;
            if (!focused || !caseFocused)
            {

                
                var Y = Input.GetAxis(rotateCameraYInput);
                var X = Input.GetAxis(rotateCameraXInput);

                tpCamera.RotateCamera(X, Y);

                // tranform Character direction from camera if not KeepDirection
                if (!keepDirection)
                    cc.UpdateTargetDirection(tpCamera != null ? tpCamera.transform : null);
                // rotate the character with the camera while strafing        
                RotateWithCamera(tpCamera != null ? tpCamera.transform : null);
            }
        }

        protected virtual void UpdateCameraStates()
        {
            // CAMERA STATE - you can change the CameraState here, the bool means if you want lerp of not, make sure to use the same CameraState String that you named on TPCameraListData
            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<vThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }            
        }

        protected virtual void RotateWithCamera(Transform cameraTransform)
        {
            if (cc.isStrafing && !cc.lockMovement && !cc.lockMovement)
            {                
                cc.RotateWithAnotherTransform(cameraTransform);                
            }
        }

        #endregion     
    }
}