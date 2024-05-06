using UnityEngine;

namespace DreamScape.Core {

    public class ActionManager : MonoBehaviour {
        
        public static ActionManager Instance {get; private set;}
        private IAction currentAction;

        private void Awake() {
            Instance = this;
        }

        /// <summary>
        /// cancels the current action (example: is current action is movement
        /// it will cancel it and leave room for the action paramete)
        /// </summary>
        /// <param name="action">this is the action that ges room to works and is the new current action as the previous one is cancled</param>
        public void StartAction(IAction action) {
            if (currentAction == action) return;

            if (currentAction != null) {
                currentAction.CancelAction();
            }

            currentAction = action;
        }

    }
}