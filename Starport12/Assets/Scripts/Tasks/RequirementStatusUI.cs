using Smallgroup.Starport.Assets.Scripts.GameResources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequirementStatusUI : MonoBehaviour {

    public GameResource ActualResource;
    public GameResource BaseResource;
    //public 



    public Text TitleText;
    public Text RatioText;

    public Image CompletionBar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (BaseResource != null && ActualResource != null)
        {
            if (TitleText != null)
            {
                TitleText.text = BaseResource.ResourceType.Name;
            }

            if (RatioText != null)
            {
                RatioText.text = ActualResource.Amount + "/" + BaseResource.Amount;
            }

            if (CompletionBar != null)
            {
                var ratio = 1 - ActualResource.Amount / (float)BaseResource.Amount;
                ratio = Mathf.Max(0, Mathf.Min(1, ratio));

                var parent = CompletionBar.transform.parent as RectTransform;
                var top = ratio * parent.rect.height;

                CompletionBar.rectTransform.offsetMin = new Vector2(-parent.rect.width/2, -parent.rect.height/2 );
                CompletionBar.rectTransform.offsetMax = new Vector2(parent.rect.width/2,  parent.rect.height/2 - top);
                CompletionBar.color = BaseResource.ResourceType.ColorNoTransparent;
               
                //CompletionBar.rectTransform.rect.Set(0, top, parent.rect.width, parent.rect.height - top);
                //CompletionBar.rectTransform.ForceUpdateRectTransforms();
            }
        }
	}
}
