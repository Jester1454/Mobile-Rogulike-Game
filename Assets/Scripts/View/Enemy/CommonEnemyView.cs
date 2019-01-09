using UnityEngine;

namespace View.Enemy
{
    public class CommonEnemyView : MonoBehaviour, IView
    {
        public TextMesh Text;
        public SpriteRenderer[] Sprites;
    
        private void Awake()
        {
            Text.GetComponent<MeshRenderer>().sortingLayerName = "Items";
            Text.GetComponent<MeshRenderer>().sortingOrder = 2;
        }
    
        public void ShowView(int damage, Vector2 worldPosition)
        {
            Sprites[UnityEngine.Random.Range(0, Sprites.Length)].gameObject.SetActive(true);
            Text.text = damage.ToString();
            gameObject.transform.position = worldPosition;
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}
