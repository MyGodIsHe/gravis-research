using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI
{
    public class NodeInputField : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TMP_InputField inputField;
        
        public void SetPosition(Vector2 position)
        {
            rectTransform.anchoredPosition = position;
        }

        public async Task<string> TypeText()
        {
            inputField.gameObject.SetActive(true);
            inputField.Select();

            bool ended = false;
            inputField.onEndEdit.AddListener(_ => ended = true);
            
            while (!ended)
            {
                await Task.Yield();
            }

            inputField.gameObject.SetActive(false);
            return inputField.text;
        }

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static NodeInputField Instance
        {
            get;
            private set;
        }
    }
}