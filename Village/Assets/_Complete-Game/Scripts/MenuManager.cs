using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject MenuObject;
    [SerializeField] GameObject MenuInsideObject;

    bool menuState;


    // Update is called once per frame
    void Update()
    {

        if (menuState == false)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                MenuObject.gameObject.SetActive(true);
                menuState = true;


                // �}�E�X�J�[�\����\���ɂ��A�ʒu�Œ����
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;


            }
        }

        else
        {
            if (Input.GetButtonDown("Cancel"))
            {
                MenuObject.gameObject.SetActive(false);
                MenuInsideObject.gameObject.SetActive(false);
                menuState = false;

                // �}�E�X�J�[�\�����\���ɂ��A�ʒu���Œ�
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;


            }
        }
    }
}