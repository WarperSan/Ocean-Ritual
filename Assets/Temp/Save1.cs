using UnityEngine;

namespace Save
{
    public class Save1 : MonoBehaviour
    {
        public bool load;

        private void Start()
        {
            //this.StartCoroutine(this.LAC());
            _ = this.load ? SaveManager.Load(1) : SaveManager.Save(1, true);
        }
    }
}