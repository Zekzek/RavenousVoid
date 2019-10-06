using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtiliZek
{
    [Serializable]
    public class Resource
    {
        public delegate void OnChangeCallback();

        public OnChangeCallback OnFull;
        public OnChangeCallback OnEmpty;
        public OnChangeCallback OnChange;
        public OnChangeCallback OnMore;
        public OnChangeCallback OnLess;

        [SerializeField]
        private float current;
        [SerializeField]
        private float max = 100;
        private float min = 0;

        public float Current { get { return current; } }
        public float Max { get { return max; } }
        public bool Full { get; private set; }
        public bool Empty { get; private set; }

        public void Init()
        {
            if (current >= max)
            {
                current = max;
                Full = true;
            }
            if (current <= min)
            {
                current = min;
                Empty = true;
            }
        }

        public void Change(float amount)
        {
            current = Mathf.Clamp(current + amount, 0, max);
            Empty = false;

            if (!Full && amount > 0)
            {
                if (OnMore != null)
                    OnMore();
                if (OnChange != null)
                    OnChange();
                if (current >= max)
                {
                    current = max;
                    Full = true;
                    if (OnFull != null)
                        OnFull();
                }
            }
            else if (!Empty && amount < 0)
            {
                if (OnLess != null)
                    OnLess();
                if (OnChange != null)
                    OnChange();
                if (current <= min)
                {
                    current = min;
                    Empty = true;
                    if (OnEmpty != null)
                        OnEmpty();
                }
            }
        }
    }
}