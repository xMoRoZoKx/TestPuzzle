using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileView : ConnectableMonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private Color highlightColor;
    [SerializeField] private BoxCollider2D boxCollider;

    private TileSetting currentSetting;
    private Action onRotateAction;
    private Vector2 initialScale;

    public void SetTile(TileSetting setting, Action onRotate)
    {
        initialScale = transform.localScale;
        onRotateAction = onRotate;
        currentSetting = setting;

        if (setting.tile == null)
        {
            Debug.LogError("TileSetting does not contain a tile!", gameObject);
            return;
        }

        icon.SetSprite(setting.tile.icon);
        ApplyRotation();


        ResizeCollider();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RotateTile();
    }

    private void RotateTile()
    {
        currentSetting.rotate = GetNextRotation(currentSetting.rotate);
        ApplyRotation();
        onRotateAction?.Invoke();
    }

    private void ApplyRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, -(int)currentSetting.rotate);
    }

    private RotateType GetNextRotation(RotateType currentRotation)
    {
        return currentRotation switch
        {
            RotateType.R0 => RotateType.R90,
            RotateType.R90 => RotateType.R180,
            RotateType.R180 => RotateType.R270,
            RotateType.R270 => RotateType.R0,
            _ => RotateType.R0
        };
    }

    public void Highlight(bool value)
    {
        icon.color = value ? highlightColor : Color.white;
    }

    private void ResizeCollider()
    {
        if (boxCollider == null) return;

        Vector2 scaleFactor = new Vector2(
            initialScale.x / transform.localScale.x,
            initialScale.y / transform.localScale.y
        );

        boxCollider.size *= scaleFactor;

        boxCollider.offset = Vector2.zero;
    }
}
