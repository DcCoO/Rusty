using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustStone : MonoBehaviour
{
    [SerializeField] int minHP, maxHP;
    [SerializeField] Color glowColor;
    int HP;

    private static readonly int glowColorProperty = Shader.PropertyToID("_GlowColor");

    [SerializeField] float glowSpeed;
    void Start()
    {
        HP = Random.Range(minHP, maxHP);
    }

    public void SetGlow(bool state)
    {
        if (state) StartCoroutine(GlowRoutine());
        else
        {
            StopAllCoroutines();
            GetComponent<Renderer>().material.SetColor(glowColorProperty, Color.black);
        }
    }

    IEnumerator GlowRoutine()
    {
        Material mat = GetComponent<Renderer>().material;
        float time = 0;
        while (true)
        {
            mat.SetColor(glowColorProperty, Color.Lerp(Color.black, glowColor, (Mathf.Sin(time) + 1) * 0.5f));
            time += Time.deltaTime * glowSpeed;
            yield return null;
        }
    }

    public RustStone GetHit()
    {
        --HP;
        if(HP == 0)
        {
            StopAllCoroutines();
            RustyBreakController.instance.DropRust();
            GetComponent<Renderer>().material.SetColor(glowColorProperty, Color.black);
            transform.parent = null;
            gameObject.AddComponent<Rigidbody>().AddForce((transform.position - RustyBreakController.instance.GetMeshPosition()).normalized * 10, ForceMode.Impulse);
            Destroy(gameObject, 3);
            return null;
        }
        return this;
    }
}
