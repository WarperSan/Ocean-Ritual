using System.Collections;
using UnityEngine;

namespace Chain
{
    public class Connector : BaseTransposable
    {
        public Cogwheel gearToConnect;
        public Machinery machineryToFollow;
        public Vector3 positionOffset = new(0, 0, -1);
        public Vector3 rotationOffset;
        
        private Machinery mainMachinery;
    
    
        private void OnEnable()
        {
            ChainEvents.OnMovieClipBegin += Initialize;
        }
    
        private void Start()
        {
            SetPositionsAndRotation();
        }

        private void SetConnectingGearAccordingly()
        {
            // float speed;
            // speed = (machineryToFollow.machinerySpeed / gearToConnect.Data.TeethCount) * connectingGear.Data.TeethCount;
            mainMachinery.machinerySpeed = machineryToFollow.machinerySpeed;
        }

        private IEnumerator FollowRoutine()
        {
            SetConnectingGearAccordingly();
            while (true)
            {
                SetPositionsAndRotation();
                yield return new WaitForFixedUpdate();
            }
        }

        private void SetPositionsAndRotation()
        {
            transform.rotation = gearToConnect.transform.rotation * Quaternion.Euler(rotationOffset);
            transform.position = gearToConnect.transform.position + positionOffset;
        }
    
        public override void Initialize()
        {
            mainMachinery = GetComponent<Machinery>();
            StartCoroutine(nameof(FollowRoutine));
        }
    
        private void OnDisable()
        {
            ChainEvents.OnMovieClipBegin -= Initialize;
        }
    }

}
