using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, Iatacavel // Classe Turret: Representa uma torre que ataca inimigos dentro de um determinado alcance.

{
    [SerializeField] protected float targetingRange = 5f;    // Raio de alcance da torre para identificar inimigos.

    [SerializeField] protected LayerMask enemyMask;    // Máscara de camada para identificar quais objetos são inimigos.

    [SerializeField] protected GameObject bulletPrefab;    // Prefab da bala que a torre irá disparar.

    [SerializeField] protected Transform firingPoint;    // Ponto de onde as balas serão disparados.

    [SerializeField] private float bps = 1f;    // Dano por segundo (disparos por segundo).

    protected Transform target;    // Referência ao inimigo alvo.

    protected float timeUntilFire;    // Tempo até o próximo disparo da torre.



    public virtual void Atacar()    // Método virtual para atacar. 

    {
        //implementação nas classes.
    }
    // Update is called once per frame
    private void Update()    // Método chamado a cada quadro para verificar e atacar inimigos.

    {
        if (target == null)        // Se não houver alvo ele ira procurar um.

        {
            FindTarget();
            return;
        }
        if (!CheckTargetIsInRange())
        {
            target = null;        // Verifica se o alvo está fora do alcance para efetuar o disparo.

        }
        else
        {
            timeUntilFire += Time.deltaTime;            // Aumenta o tempo até o próximo disparo.

            if (timeUntilFire >= 1f / bps)            // Verifica se pode disparar.

            {
                Shoot();// Realiza o disparo no inimigo.
                timeUntilFire = 0f;  // Reseta o tempo até o próximo disparo.
            }
        }

    }
    protected virtual void Shoot()    // Método protegido para disparar.

    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);        // Instancia um objeto de bala na posição do ponto de disparo.

        // Obtém o script da bala e define o alvo que ira matar.
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }
    private bool CheckTargetIsInRange()    // Verifica se o alvo está dentro do alcance da torre para disparar.

    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
    private void FindTarget()    // Procura por um alvo inimigo dentro do alcance.

    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        // Se houver inimigos, define o primeiro como alvo para ficar dando tiros apenas nele.
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }
}