using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExplosionBarrel : MonoBehaviour
{
    [SerializeField]
    private float _sphereRadius;

    [SerializeField]
    private bool _isBulletHit;

    private MeshRenderer _mesh;
    [SerializeField]
    private float _explosionForce;

    private bool _hasExplode;

    private void Start()
    {
        _mesh = GetComponent<MeshRenderer>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, _sphereRadius);
    }

    void Explosion()
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, _sphereRadius, transform.forward, 0,
            1 << 6, QueryTriggerInteraction.UseGlobal);

        foreach (RaycastHit hit in hits)
        {
            AI enemy = hit.collider.GetComponent<AI>();
            if (enemy != null && _isBulletHit == true)
            {
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                if (rb !=null && _hasExplode ==false)
                {
                    rb.AddExplosionForce(_explosionForce, transform.position, _sphereRadius);
                    _hasExplode = true;
                }
               
                enemy.RagdollState();
                enemy.Damage();
            }
        }
    }

    private void FixedUpdate()
    {
        Explosion();
    }

    public void TriggerExplosion()
    {
        _isBulletHit = true;
        _mesh.enabled = false;
    }

    public IEnumerator ExplosionFireExtinguishedRoutine()
    {
        yield return new WaitForSeconds(0.59f);
        _isBulletHit = false;
        yield return new WaitForSeconds(0.5f);
        if (this.gameObject!=null)
            Destroy(this.gameObject);
    }
}
