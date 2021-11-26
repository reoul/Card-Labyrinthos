using UnityEngine;

public class Order : MonoBehaviour
{
    [SerializeField] Renderer[] backRenderers;
    [SerializeField] Renderer[] middleRenderers;
    [SerializeField] string sortingLayerName;
    int originOrder;

    public void SetOriginOrder(int originOrder)
    {
        this.originOrder = originOrder;
        this.SetOrder(originOrder);
    }

    public void SetMostFrontOrder(bool isMostFront)
    {
        this.SetOrder(isMostFront ? 3800 : this.originOrder);
    }

    public void SetOrder(int order)
    {
        foreach (var renderer in this.backRenderers)
        {
            renderer.sortingLayerName = this.sortingLayerName;
            renderer.sortingOrder = order;
        }
        foreach (var renderer in this.middleRenderers)
        {
            renderer.sortingLayerName = this.sortingLayerName;
            renderer.sortingOrder = order + 1;
        }
    }
}
