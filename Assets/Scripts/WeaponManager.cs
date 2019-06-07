using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    public bool isAttacking = false;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;

    void Start () {
        EquipWeapon(primaryWeapon);
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;

        //GameObject _weaponIns = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        GameObject _weaponIns = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();

        if (currentGraphics == null)
        {
            Debug.LogError("No WeaponGraphics on the weapon: " + _weaponIns.name);
        }

        if (isLocalPlayer)
        {
            SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentWeaponGraphics()
    {
        return currentGraphics;
    }

    public void Attack()
    {
        if (isAttacking) return;
        StartCoroutine(Reload_Corutine());
    }

    private IEnumerator Reload_Corutine()
    {
        Debug.Log("Attacking...");
        isAttacking = true;

        CmdOnAttack();

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
    }

    [Command]
    void CmdOnAttack()
    {
        RpcOnAttack();
    }

    [ClientRpc]
    void RpcOnAttack()
    {
        Animator anim = currentGraphics.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Attack");
        } else
        {
            Debug.Log("Animatorul nu a fost gasit");
        }
    }

    private static void SetLayerRecursively(GameObject _object, int _newLayer)
    {
        if (_object == null)
        {
            return;
        }

        _object.layer = _newLayer;

        foreach (Transform _child in _object.transform)
        {
            if (_child == null)
            {
                continue;
            }

            SetLayerRecursively(_child.gameObject, _newLayer);
        }
    }
}
