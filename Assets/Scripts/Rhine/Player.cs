using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhine
{
    public class Player : MonoBehaviour
    {
        public AudioSource source;

        public float speed = 12;
        public bool isStarted;

        public KeyCode leftKey = KeyCode.LeftArrow;
        public KeyCode rightKey = KeyCode.RightArrow;
        public KeyCode upKey = KeyCode.UpArrow;
        public KeyCode downKey = KeyCode.DownArrow;

        public Direction direction = Direction.Up;

        void Start()
        {

        }

        void Update()
        {
            if(!isStarted)
            {
                if(Input.GetKeyDown(upKey))
                {
                    isStarted = true;
                    source.Play();
                }
            }
            else
            {
                ListenInput();

                transform.localPosition += transform.up * speed * Time.deltaTime;
            }
        }

        public void TurnLine(Vector3 rotation)
        {
            transform.localEulerAngles = rotation;
        }

        public void UpdateDirection(Direction direction)
        {
            if (this.direction == direction)
                return;

            this.direction = direction;
            TurnLine(direction.ToRotation());
        }

        private void ListenInput()
        {
            if (Input.GetKey(leftKey))
            {
                if (Input.GetKey(upKey))
                {
                    UpdateDirection(Direction.LeftUp);
                }
                else if (Input.GetKey(downKey))
                {
                    UpdateDirection(Direction.LeftDown);
                }
                else
                {
                    UpdateDirection(Direction.Left);
                }
            }
            else if (Input.GetKey(rightKey))
            {
                if (Input.GetKey(upKey))
                {
                    UpdateDirection(Direction.RightUp);
                }
                else if (Input.GetKey(downKey))
                {
                    UpdateDirection(Direction.RightDown);
                }
                else
                {
                    UpdateDirection(Direction.Right);
                }
            }
            else if (Input.GetKey(downKey))
            {
                UpdateDirection(Direction.Down);
            }
            else if (Input.GetKey(upKey))
            {
                UpdateDirection(Direction.Up);
            }
        }
    }
}