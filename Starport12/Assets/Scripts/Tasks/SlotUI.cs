using Smallgroup.Starport.Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Smallgroup.Starport.Assets.Scripts.Tasks
{
    public class SlotUI : MonoBehaviour
    {
        public JobSlotAssignment SlotAssignment;

        public Text NameText;
        public Image IconImage;

        public RectTransform SkillTemplate;

        private Sprite _oldSprite;

        private CharacterData _oldCharacter;

        private List<GameObject> _skillPanels = new List<GameObject>();

        private void Start()
        {
            _oldSprite = IconImage.sprite;
        }

        private void Update()
        {

            CharacterData character = null;
            if (SlotAssignment != null && SlotAssignment.Character != null)
            {
                character = SlotAssignment.Character;
            }

            if (_oldCharacter != character)
            {
                // update skills.
                _skillPanels.ForEach(s => Destroy(s));
                if (character != null)
                {
                    for (var i = 0; i < character.ResourceAbilities.Count; i++)
                    {
                        var panel = Instantiate(SkillTemplate.gameObject, SkillTemplate.parent);
                        panel.SetActive(true);
                        var rect = panel.transform as RectTransform;
                        panel.transform.localPosition += new Vector3(0, -i * (rect.rect.height + 4), 0);
                        _skillPanels.Add(panel);


                    }

                }
                _oldCharacter = character;
            }


            if (character != null)
            {
                NameText.text = character.DisplayName;
                IconImage.sprite = character.Icon;


                for (var i = 0; i < _skillPanels.Count; i++)
                {
                    _skillPanels[i].GetComponentInChildren<Text>().text = "" + character.ResourceAbilities[i].Amount;
                    //_skillPanels[i].getcom
                    if (character.ResourceAbilities[i].ResourceType.Icon != null)
                    {
                        _skillPanels[i].GetComponentsInChildren<Image>()[1].color = Color.white;
                        _skillPanels[i].GetComponentsInChildren<Image>()[1].overrideSprite = character.ResourceAbilities[i].ResourceType.Icon;

                    }
                    else
                    {
                        _skillPanels[i].GetComponentsInChildren<Image>()[1].color = character.ResourceAbilities[i].ResourceType.Color;

                    }
                }

            } else {
                NameText.text = "";
                IconImage.sprite = _oldSprite;
                //for (var i = 0; i < _skillPanels.Count; i++)
                //{
                //    _skillPanels[i].GetComponentInChildren<Text>().text = "" + character.ResourceAbilities[i].Amount;
                //    _skillPanels[i].GetComponentInChildren<Image>().color = character.ResourceAbilities[i].ResourceType.Color;
                //}
            }

            if (NameText != null)
            {
                if (SlotAssignment.Character != null)
                {
                } else
                {
                    // unassigned
                }
            }
        }

    }
}
