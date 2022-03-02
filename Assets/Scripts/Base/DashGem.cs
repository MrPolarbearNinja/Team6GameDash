using UnityEngine;

namespace AGDDPlatformer
{
    public class DashGem : MonoBehaviour
    {
        public GameObject activeIndicator;
        public float cooldown = 2;
        public AudioSource source;
        float lastCollected;
        public bool isActive;

        public bool canPickUp;


        void Awake()
        {
            lastCollected = -cooldown * 2;
        }

        void Update()
        {
            if (!isActive && Time.time - lastCollected >= cooldown)
            {
                isActive = true;
                activeIndicator.SetActive(true);
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (!isActive || !canPickUp)
                return;

            PlayerController playerController = other.GetComponentInParent<PlayerController>();
            if (playerController != null)
            {
                playerController.ResetDash();
                isActive = false;
                lastCollected = Time.time;
                activeIndicator.SetActive(false);
                source.Play();
            }
        }
    }
}
