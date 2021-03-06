using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class Chisel : SingletonMonoBehaviour<Chisel>
{
    Transform tf;
    [Header("Hammer")]
    [SerializeField] Transform hammerPivot;
    [SerializeField] Vector3 idleAngle, hitAngle, farAngle;

    [Header("Chisel")]
    [SerializeField] Transform chisel;

    RustStone currentRustStone;

    public bool disappearing;
    bool canHammer;
    //lockers
    bool hammering = false;
    void Start()
    {
        tf = transform;    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) HammerHit();
        if (canHammer) HammerHit();
    }

    public void SetRust(RustStone rustStone)
    {
        if (currentRustStone != null) currentRustStone.SetGlow(false);
        currentRustStone = rustStone;
        currentRustStone.SetGlow(true);
        StartCoroutine(MoveRoutine(rustStone.transform.position));
    }

    IEnumerator MoveRoutine(Vector3 position)
    {
        Vector3 begin = tf.position;
        for(float t = 0; t < 1; t += Time.deltaTime * 3)
        {
            tf.position = Vector3.Lerp(begin, position, -(Cos(PI * t) - 1) * 0.5f);
            yield return null;
        }
        tf.position = position;
    }

    public void Disappear()
    {
        if (disappearing) return;

        if (currentRustStone != null)
        {
            currentRustStone.SetGlow(false);
            currentRustStone = null;
        }
        StartCoroutine(DisappearRoutine());
    }

    IEnumerator DisappearRoutine()
    {
        disappearing = true;
        canHammer = false;
        Vector3 begin = tf.position;
        for(float t = 0; t < 1; t += Time.deltaTime * 4)
        {
            tf.position = Vector3.Lerp(begin, 0.3f * Vector3.up, t);
            yield return null;
        }
        tf.position = 0.3f * Vector3.up;
        disappearing = false;
    }

    public void CanHammer(bool state) => canHammer = state;

    public void HammerHit()
    {
        if (hammering) return;
        if(currentRustStone != null) StartCoroutine(HammerHitRoutine());
    }

    IEnumerator HammerHitRoutine()
    {
        hammering = true;

        //hammer movement
        for (float t = 0; t < 1; t += Time.deltaTime * 3)
        {
            hammerPivot.localEulerAngles = Vector3.Lerp(hitAngle, farAngle, -(Cos(PI * t) - 1) * 0.5f);
            yield return null;
        }
        hammerPivot.localEulerAngles = farAngle;

        for (float t = 0; t < 1; t += Time.deltaTime * 5)
        {
            hammerPivot.localEulerAngles = Vector3.Lerp(farAngle, hitAngle, t * t * t);
            yield return null;
        }
        hammerPivot.localEulerAngles = hitAngle;

        //chisel movement
        Vector3 beginChisel = chisel.position;
        Vector3 endChisel = chisel.position - chisel.up * 0.0025f;
        for (float t = 0; t < 1; t += Time.deltaTime * 12)
        {
            chisel.position = Vector3.Lerp(beginChisel, endChisel, Sin(PI * t));
            yield return null;
        }
        chisel.position = beginChisel;

        //particles
        GameObject g = Instantiate(ParticleController.instance.GetRustPieces());
        g.transform.position = tf.position;
        g.transform.LookAt(chisel.position, Vector3.up);
        print(g.transform.localScale);
        Destroy(g, 3);

        //rust stone
        currentRustStone = currentRustStone.GetHit();
        if (currentRustStone == null)
        {
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(DisappearRoutine());
        }

        hammering = false;
    }
}
