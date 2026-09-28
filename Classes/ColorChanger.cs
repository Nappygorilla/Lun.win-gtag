using UnityEngine;

namespace StupidTemplate.Classes
{
    public class ColorChanger : MonoBehaviour
    {
        public void Start()
        {
            if (colors == null)
            {
                Destroy(this);
                return;
            }

            targetRenderer = GetComponent<Renderer>();
            if (targetRenderer == null || colors == null)
            {
                Destroy(this);
                return;
            }

            if (colors.IsFlat())
            {
                Update();
                Destroy(this);
                return;
            }

            Update();
        }

        public void Update()
        {
            if (targetRenderer == null || colors == null)
                return;

            targetRenderer.enabled = !colors.transparent;

            if (colors.transparent)
                return;

            targetRenderer.material.color = colors.GetCurrentColor();
        }

        public Renderer targetRenderer;
        public ExtGradient colors;
    }
}
