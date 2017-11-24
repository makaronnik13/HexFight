using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using System;

namespace Dialoges
{
    [RequireComponent(typeof(AudioSource))]
    public class DialogGui : Singleton<DialogGui>
    {
        private State currentState;

        private AudioSource source;
        private AudioSource Source
        {
            get
            {
                if (!source)
                {
                    source = GetComponent<AudioSource>();
                }
                return source;
            }
        }

        void OnEnable()
        {
            DialogPlayer.Instance.onStateIn += StateIn;
        }

        void OnDisable()
        {
            if (DialogPlayer.Instance)
            {
                DialogPlayer.Instance.onStateIn -= StateIn;
            }
        }

        private void StateIn(State e)
        {
            GetComponentInChildren<StateGui>().ShowState(e.description, Skipped);
            currentState = e;
            Invoke("Skipped", e.time);
        }


        private void Skipped()
        {
            GetComponentInChildren<StateGui>().HideState();
            List <Path> avaliablePathes = currentState.pathes.Where(p => PlayerResource.Instance.CheckCondition(p.condition)).ToList();
            avaliablePathes = avaliablePathes.Where(p=>!p.auto).ToList();
            if (avaliablePathes.Count>0)
            {
                GetComponentInChildren<VariantsGui>().ShowVariants(avaliablePathes, Apply);
            }
        }

        private void Apply(Path appliedPath)
        {
            DialogPlayer.Instance.PlayPath(appliedPath);
        }
    }
}
