using System;
using UnityEngine;

namespace Core
{
    public enum PotentialType
    {
        Plus, Minus
    }

    [Serializable]
    public class MagneticPotential
    {
        public Vector3 position;
        public PotentialType type;
    }
}