using System;
using UnityEngine;

namespace DreamScape.Combat {

    public class Health : MonoBehaviour {

        [SerializeField] private float health = 100;
        [SerializeField] private HealthAnimations healthAnimations;

        public bool isAlive {get; private set;} = true;
 
        public event EventHandler OnDeath;

        private Collider myCollider;

        private void Start() {
            TryGetComponent(out myCollider);
            OnDeath += Health_OnDeath;
        }

        private void Health_OnDeath(object sender, EventArgs e) {
            healthAnimations.PlayDeathAnimation();
            isAlive = false;
            myCollider.enabled = false;
            OnDeath -= Health_OnDeath;
        }

        public void Damage(float amount) {
            health = Mathf.Max(health - amount, 0); print(health);

            if (health <= 0) {
                OnDeath?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    [Serializable]
    internal class HealthAnimations {

        [SerializeField] private Animator animator;

        private const string ON_DEATH_TRIGGER = "OnDeath";

        public void PlayDeathAnimation() {
            animator.SetTrigger(ON_DEATH_TRIGGER);
        }
    }
 
}
