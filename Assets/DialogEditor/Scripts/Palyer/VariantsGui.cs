using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dialoges
{
    public class VariantsGui : MonoBehaviour
    {
        private int currentVariant = 0;
        private List<Path> avaliablePathes = new List<Path>();
        public Text pathText;
        private Action<Path> onApply;

        private void Update()
        {
            if (GetComponent<Animator>().GetBool("Active"))
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    SwitchLeft();
                }

                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    SwitchRight();
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Apply();
                }
            }
        }

        public void Apply()
        {
            if (onApply!=null)
            {
                onApply.Invoke(avaliablePathes[currentVariant]);
            }
            GetComponent<Animator>().SetBool("Active", false);
        }

        public void SwitchRight()
        {
            currentVariant++;
            if (currentVariant > avaliablePathes.Count-1)
            {
                currentVariant = 0;
            }
            pathText.text = avaliablePathes[currentVariant].text;
        }

        public void SwitchLeft()
        {
            currentVariant--;
            if (currentVariant < 0)
            {
                currentVariant = avaliablePathes.Count - 1;
            }
            pathText.text = avaliablePathes[currentVariant].text;
        }

        public void ShowVariants(List<Path> pathes, Action<Path> onApply)
        {
            this.onApply = onApply;
            GetComponent<Animator>().SetBool("Active", true);
            avaliablePathes = pathes;
            currentVariant = 0;
            pathText.text = avaliablePathes[currentVariant].text;
        }


    }
}
