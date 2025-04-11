using Unity.VisualScripting;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private float _crystalExistTimer;

    public void SetupCrystal(float _crystalDuration) {
        _crystalExistTimer = _crystalDuration;
    }

    private void Update() {
        _crystalExistTimer -= Time.deltaTime;
        
        if (_crystalExistTimer < 0)
            SelfDestroy();
    }

    public void SelfDestroy() {
        Destroy(this.gameObject);
    }
}
