using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private float moveSpeed = 7f;
    CharacterController cc;
    private float gravity = -20f;
    private float yVelocity = 0;
    private float jumpPower = 10f;

    private bool isJumping = false;

    public float hp = 20;
    private float maxHp = 20;
    public Slider hpSlider;
    public GameObject hitEffect;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run)
            return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir);

        if(cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0;
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
        cc.Move(dir * moveSpeed * Time.deltaTime);

        hpSlider.value = hp / maxHp;
    }

    public void DamageAction(int damage)
    {
        hp -= damage;
        if(hp > 0)
        {
            StartCoroutine(PlayerHitEffect());
        }
    }

    IEnumerator PlayerHitEffect()
    {
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hitEffect.SetActive(false);
    }
}
