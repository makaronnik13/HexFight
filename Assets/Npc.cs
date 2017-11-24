using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Dialoges;
using System;

public class Npc : MonoBehaviour {

    public GameObject face;
    [SerializeField]
    private bool interractionEnable = true;
    public bool InterractionEnable
    {
        get
        {
            return interractionEnable;
        }
        set
        {
            if(interractionEnable != value)
            {
                if (interractionEnable)
                {
                    AddListeners();
                }
                else
                {
                    RemoveListeners();
                }
                interractionEnable = value;
                GetComponent<WarriorSelector>().enabled = interractionEnable;
            }
        }
    }

    private void OnEnable()
    {
        GetComponent<WarriorSelector>().enabled = interractionEnable;

        if (interractionEnable)
        {
            AddListeners();
        }
    }

    private void AddListeners()
    {
        Raycaster.Instance.AddListener((Vector3 v, GameObject go) => {
            if (GameController.Instance.Mode == GameController.GameMode.Adventure)
            {
                Click();
            }
        }, () => { }, 10, 0.1f, LayerRaycaster.InputType.mouseDown, 1, gameObject);
        Raycaster.Instance.AddListener((Vector3 v, GameObject go) => {
            if (GameController.Instance.Mode == GameController.GameMode.Adventure)
            {
                DoubleClick();
            }
        }, () => { }, 10, 0.1f, LayerRaycaster.InputType.mouseDoubleClick, 1, gameObject);
    }

    private void RemoveListeners()
    {
        if (Raycaster.Instance)
        {
            Raycaster.Instance.RemoveRaycaster(15, LayerRaycaster.InputType.mouseDown, 1, gameObject);
            Raycaster.Instance.RemoveRaycaster(15, LayerRaycaster.InputType.mouseDoubleClick, 1, gameObject);
        }
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void Click()
    {
        Vector3 aimPoint = transform.position + transform.forward * 1.5f;
        NavMeshHit hit;
        NavMesh.SamplePosition(aimPoint, out hit, 2, NavMesh.AllAreas);
        GameController.Instance.Warrior.GetComponent<FakeController>().GoTo(hit.position , false);
    }

    private void DoubleClick()
    {
        Vector3 aimPoint = transform.position + transform.forward * 1.5f;
        NavMeshHit hit;
        NavMesh.SamplePosition(aimPoint, out hit, 2, NavMesh.AllAreas);

        GameController.Instance.Warrior.GetComponent<FakeController>().GoTo(hit.position, true);
    }

    private void Update()
    {
        if (interractionEnable)
        {
            Vector3 aimPoint = transform.position + transform.forward * 1.5f;
            NavMeshHit hit;
            NavMesh.SamplePosition(aimPoint, out hit, 2, NavMesh.AllAreas);

            BattleWarrior w = GameController.Instance.Warrior;
            if (w && w.GetComponent<NavMeshAgent>().destination == hit.position && Vector3.Distance(hit.position, w.transform.position) <= w.GetComponent<NavMeshAgent>().stoppingDistance)
            {
                Talk();
            }
        }
    }

    private void Talk()
    {
        DialogPlayer.Instance.onPathGo += ShowDialogablePerson;
        DialogPlayer.Instance.onStateIn += ShowPlayerPerson;
        Camera.main.GetComponent<ThirdPersonCamera>().dialogablePerson = face;
        GameController.Instance.Mode = GameController.GameMode.Dialog;
        interractionEnable = false;
        GetComponent<PersonDialog>().Talk();
    }

    private void ShowPlayerPerson(State e)
    {
        Camera.main.GetComponent<ThirdPersonCamera>().dialogablePerson = GameController.Instance.Warrior.GetComponent<Npc>().face;
    }

    private void ShowDialogablePerson(Path e)
    {
        Camera.main.GetComponent<ThirdPersonCamera>().dialogablePerson = face;
    }

    public void StopTalk()
    {
        DialogPlayer.Instance.onPathGo -= ShowDialogablePerson;
        DialogPlayer.Instance.onStateIn -= ShowPlayerPerson;
        interractionEnable = true;
        GameController.Instance.Mode = GameController.GameMode.Adventure;
    }

}
