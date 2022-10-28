using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleSpawner : MonoBehaviour
{
    [SerializeField] AudioSource eagleSound;
    [SerializeField] GameObject eaglePrefab;
    [SerializeField] int spawnZPos = 7;
    [SerializeField] Player player;
    [SerializeField] float timeOut = 5;
    [SerializeField] float timer = 0;

    int playerLastMaxTravel = 0;

    private void SpawnEagle()
    {
        player.enabled = false;
        var position = new Vector3(
            player.transform.position.x, 
            (float)-0.5, 
            player.CurrentTravel + spawnZPos);
        var rotation = Quaternion.Euler(0, 180, 0);
        var eagleObject = Instantiate(eaglePrefab, position, rotation);
        var eagle = eagleObject.GetComponent<Eagle>();
        eagle.SetUpTarget(player);
        eagleSound.PlayDelayed((float)0.75);
    }
    
    private void Update()
    {
        //jika player maju
        if (player.MaxTravel != playerLastMaxTravel)
        {
            //maka reset timer
            timer = 0;
            playerLastMaxTravel = player.MaxTravel;
            return;
        }

        //jika ga maju jalankan timer
        if (timer < timeOut)
        {
            timer += Time.deltaTime;
            return;
        }

        //jika sudah timeout
        if (player.IsJumping() == false && player.IsDie == false)
            SpawnEagle();
    }
}
