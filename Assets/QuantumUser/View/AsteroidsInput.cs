using Photon.Deterministic;
using Quantum;
using UnityEngine;

namespace QuantumUser.View
{
    public class AsteroidsInput : MonoBehaviour
    {
        private void OnEnable()
        {
            QuantumCallback.Subscribe(this, (CallbackPollInput callBack) => PollInput(callBack));
        }

        private void PollInput(CallbackPollInput callBack)
        {
            var input = new Quantum.Input
            {
                // Note: Use GetKey() instead of GetKeyDown/Up. Quantum calculates up/down internally.
                Left = UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.LeftArrow),
                Right = UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.RightArrow),
                Up = UnityEngine.Input.GetKey(KeyCode.W) || UnityEngine.Input.GetKey(KeyCode.UpArrow),
                Fire = UnityEngine.Input.GetKey(KeyCode.Space)
            };

            callBack.SetInput(input, DeterministicInputFlags.Repeatable);
        }
    }
}
