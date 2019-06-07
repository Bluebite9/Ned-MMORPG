using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerAttack : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;

    // Use this for initialization
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
            weaponManager.Attack();
        }
    }

    [Client]
    private void Attack()
    {
        if (!isLocalPlayer || weaponManager.isAttacking)
        {
            return;
        }

        Debug.Log("ajunge in client attack");

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {
            Debug.Log("ajunge in if");
            // if (_hit.collider.tag == PLAYER_TAG)
            //{
            CmdPlayerHit(_hit.collider.name, currentWeapon.damage, transform.name);
            //}
        }
    }

    [Command]
    void CmdPlayerHit(string _playerID, int _damage, string _sourceID)
    {
        Debug.Log(_playerID + " has been hit.");

        //Player _player = GameManager.GetPlayer(_playerID);
        //_player.RpcTakeDamage(_damage, _sourceID);
    }
}
