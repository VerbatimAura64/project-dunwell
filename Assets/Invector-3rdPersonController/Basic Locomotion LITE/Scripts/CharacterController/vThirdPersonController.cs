using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Invector.CharacterController
{
    public class vThirdPersonController : vThirdPersonAnimator, IPointerDownHandler, IDragHandler
    {
        protected virtual void Start()
        {
#if !UNITY_EDITOR
                Cursor.visible = false;
#endif
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Rotate the item with the mouse
            Debug.Log("Rotating: ");// + itemToFocus.name);
            //_rotation += eventData.delta;
            //itemToFocus.transform.Rotate(_rotation * Time.deltaTime);
            //transform.position = Input.mousePosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Store the original position and parent of the card
            Debug.Log("Pointer down on card: ");// + itemToFocus.name);
            //_canvasGroup = GetComponent<CanvasGroup>();
            //_startPosition = transform.position;
            //_originalParent = transform.parent;
        }

        public virtual void Sprint(bool value)
        {                                   
            isSprinting = value;            
        }

        public virtual void Strafe()
        {
            if (locomotionType == LocomotionType.OnlyFree) return;
            isStrafing = !isStrafing;
        }

        public virtual void Jump()
        {
            // conditions to do this action
            bool jumpConditions = isGrounded && !isJumping;
            // return if jumpCondigions is false
            if (!jumpConditions) return;
            // trigger jump behaviour
            jumpCounter = jumpTimer;            
            isJumping = true;
            // trigger jump animations            
            if (_rigidbody.linearVelocity.magnitude < 1)
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                animator.CrossFadeInFixedTime("JumpMove", 0.2f);
        }

        public virtual void Focus()
        {
            //Debug.Log("Focus on map");
        }

        public virtual void RotateWithAnotherTransform(Transform referenceTransform)
        {
            var newRotation = new Vector3(transform.eulerAngles.x, referenceTransform.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), strafeRotationSpeed * Time.fixedDeltaTime);
            targetRotation = transform.rotation;
        }

        
    }
}