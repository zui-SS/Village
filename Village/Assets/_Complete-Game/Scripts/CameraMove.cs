using Completed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Player player;
    [SerializeField] GameObject playerObjct;
    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        player = playerObjct.GetComponent<Player>();
        playerObjct = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObjct.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }
}
