using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace CeilingLight.UIShaders
{
    /// <summary>
    /// This class controls the Text component that shows the
    /// progress of a generic UI bar. This class works on
    /// all the shaders that have an exposed parameter named Progress.
    /// In the Update method the material is directly interrogated on the
    /// Progress parameter.
    /// </summary>
    
    [RequireComponent(typeof(Text))]
    public class ProgressTextLabel : MonoBehaviour
    {
        public GameObject barObject;
        public bool percentageSymbol;
        public bool percentageMultiplicator;

        Material m_Material;
        Text m_Text;
        string result;

        void Awake()
        {
            m_Material = barObject.GetComponent<Image>().material;
            m_Text = GetComponent<Text>();
        }


        void Update()
        {
            if (m_Material)
            {
                float perc = m_Material.GetFloat("_Progress") * (1f + (99f * boolToInt(percentageMultiplicator)));
                int percInt = (int)perc;
                if(perc > 1)
                {
                    result = percInt.ToString();
                }
                else
                {
                    result = perc.ToString();

                }


                if (percentageMultiplicator && percentageSymbol)
                {
                    result += "%";
                }
                m_Text.text = result;
            }
        }

        int boolToInt(bool _value)
        {
            return _value ? 1: 0;
        }
    }

}