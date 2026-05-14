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

        [Header("Camera Settings")]
        public string rotateCameraXInput ="Mouse X";
        public string rotateCameraYInput = "Mouse Y";
        [Header("Will's Settings")]
        public GM GM;
        public bool mapCollided = false;
        public bool focused = false;
        public bool caseFocused = false;
        public bool clueTriggered = false;
        public GameObject prompt;
        public GameObject itemToFocus;
        public GameObject dialogue;
        public bool clueInvestigated;
        public GameObject screen;
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
                tpCamera.target = itemToFocus.transform;//.transform;
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

        protected virtual void FocusInput()
        {
            if (mapCollided || clueTriggered)
            {
                if (mapCollided && !focused)
                {
                    prompt.GetComponent<TMP_Text>().text = "Press F to focus on map";
                } 
                if (clueTriggered && !focused)
                {
                    prompt.GetComponent<TMP_Text>().text = "Press F to investigate";
                }

                if (!focused)
                {
                    prompt.SetActive(true);
                    if (Input.GetKeyDown(focusInput) && !caseFocused)
                    { 
                        cc.isSprinting = false;
                        cc.input = Vector2.zero;
                        focused = true;
                        cc.lockMovement = true;
                        tpCamera.enabled = false;
                        //tpCamera.lockCamera = true;
                        //tpCamera._camera.enabled = false;
                        tpCamera.inspectCam.enabled = true;
                        
                        
                        if (mapCollided)
                        {
                            prompt.GetComponent<TMP_Text>().text = "Press F to unfocus on map";
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
                        tpCamera._camera.enabled = true;
                        tpCamera.inspectCam.enabled = false;
                        tpCamera.ReturnOldRotate();
                        tpCamera.lockCamera = false;
                        if (mapCollided)
                        {
                            prompt.GetComponent<TMP_Text>().text = "Press F to focus on map";
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
                if (Input.GetKeyDown(focusInput) && focused)
                {
                    cc.lockMovement = false;
                    focused = false;
                    tpCamera._camera.enabled = true;
                    tpCamera.inspectCam.enabled = false;
                    tpCamera.lockCamera = false;
                    tpCamera.ReturnOldRotate();
                }
                prompt.SetActive(false);
                //dialogue.SetActive(false);
            }
        }

        protected virtual void OpenCaseBoard()
        {
            if (!caseFocused && !focused)
            {
                if (Input.GetKeyDown(caseBoardInput))
                {
                    cc.isSprinting = false;
                    cc.input = Vector2.zero;
                    tpCamera.lockCamera = true;
                    tpCamera.enabled = false;
                    GM.caseBoard.SetActive(true);
                    cc.lockMovement = true;
                    caseFocused = true;
                }
            }
            else if (caseFocused && !focused)
            {
                if (Input.GetKeyDown(caseBoardInput))
                {
                    GM.caseBoard.SetActive(false);
                    cc.lockMovement = false;
                    tpCamera.enabled = true;
                    tpCamera.lockCamera = false;
                    caseFocused = false;
                }
            }
        }

        protected virtual void InspectClue()
        {
            
                if (focused && itemToFocus.GetComponent<Clue>().relevant)
                {
                    if(itemToFocus.GetComponent<Clue>().discovered == false)
                    {
                        itemToFocus.GetComponent<Clue>().discovered = true;
                        GM.cluesFound.Add(itemToFocus);
                        GM.DiscoverClue();
                        GM.clueCards.Add(Instantiate(GM.clueCardObj, GM.caseBoard.GetComponentInChildren<Transform>()));
                        itemToFocus.GetComponent<Clue>().clueCardObj = GM.clueCards.Last();
                        GM.clueCardObj.GetComponent<Clue>().clueName = itemToFocus.GetComponent<Clue>().clueName + " Clue";
                        
                        
                      
                    /* THIS IS WHERE WE LEFT OFF WITH THE NEXT TWO LINES FOR INK */
                    
                    
                    // if (GM.clueList.clues.Last<ClueInfo>().inkChoice != null && clueList.clues.Last<ClueInfo>().inkChoice.Length > 0)
                        {
                        //    GM.MakeChoice(GM.clueList.clues.Last<ClueInfo>().inkChoice[0]);
                        }
                }
                    //this.clueInvestigated = true;    
                    //clueInvestigated = true;
                    //GM._inkStory.variablesState["clueInspected"] = clue.GetComponent<Clue>().discovered;
                   // GM._inkStory.ChoosePathString("clueInspection");
                    //GM._inkStory.Continue();
                }
                else if(focused && !itemToFocus.GetComponent<Clue>().relevant) 
                {
                    GM._inkStory.ChoosePathString("clueInspection");
                    GM._inkStory.Continue();
                }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Focus"))
            {
                mapCollided = true;
                itemToFocus = other.gameObject;
            }
            if (other.gameObject.CompareTag("Clue"))
            {
                clueTriggered = true;
                itemToFocus = other.gameObject;
                
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
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Focus"))
            {
                mapCollided = false;
            }
            if (other.gameObject.CompareTag("Clue"))
            {
                clueTriggered = false;
                //clueToInvestigate = null;
            }
        }


        protected virtual void ExitGameInput()
        {
            // just a example to quit the application 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!Cursor.visible)
                    Cursor.visible = true;
                else
                    Application.Quit();
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