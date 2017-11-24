using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

namespace Dialoges
{
    public class DialogPlayer : Singleton<DialogPlayer>
    {
        public Action<PersonDialog> onDialogChanged;
        public Action onFinishDialog;


        private PersonDialog currentDialog;
        public PersonDialog CurrentDialog
        {
            get
            {
                return currentDialog;
            }
        }
        public delegate void StateEventHandler(State e);
        public delegate void PathEventHandler(Path e);
        public event StateEventHandler onStateIn;
        public event PathEventHandler onPathGo;
        public State currentState;

        public void PlayState(State state, PersonDialog pd)
        {
            currentDialog = pd;
            if (onDialogChanged != null)
            {
                onDialogChanged.Invoke(pd);
            }
            onStateIn.Invoke(state);
            currentState = state;

            foreach (Path p in state.pathes.Where(p => p.auto))
            {
                if (PlayerResource.Instance.CheckCondition(p.condition))
                {
                    PlayPath(p);
                }
            }

            if (state.pathes.Where(p => PlayerResource.Instance.CheckCondition(p.condition)).Count() == 0)
            {
                if (onFinishDialog != null)
                {
                    onFinishDialog.Invoke();
                }
            }
        }
        public void PlayPath(Path p)
        {
            if (p.aimState != null)
            {
                PlayState(p.aimState, currentDialog);
                onPathGo.Invoke(p);
            }
            else
            {
     
                currentDialog.playing = false;
                currentDialog = null;
                if (onDialogChanged != null)
                {
                    onDialogChanged.Invoke(null);
                }
            }
        }
    }
}
