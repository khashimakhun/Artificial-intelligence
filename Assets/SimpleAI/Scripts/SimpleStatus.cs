using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rapadura
{
    [System.Serializable]
    public class SimpleStatus : MonoBehaviour
    {
        int _health = 10;
        public int health
        {
            get
            {
                return _health;
            }
            set
            {
                if (value >= 0 && _health != value)
                {
                    _health = value;
                    actualHealth = _health;
                }
            }
        }

        int _actualHealth = 10;
        public int actualHealth
        {
            get
            {
                return _actualHealth;
            }
            set
            {
                if (value < 0)
                    value = 0;

                if (value > _health)
                    value = _health;

                _actualHealth = value;
            }
        }
    }
}