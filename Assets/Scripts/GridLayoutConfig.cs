using UnityEngine;
using UnityEngine.UI;

public class GridLayoutConfig : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private RectTransform containerRect;
    [SerializeField] private float padding = 20f;
    [SerializeField] private float spacing = 10f;
    
    public void ConfigureGrid(int rows, int columns)
    {
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;
        
        var cellSize = CalculateCellSize(rows, columns);
        gridLayout.cellSize = cellSize;
        gridLayout.spacing = new Vector2(spacing, spacing);
    }
    
    private Vector2 CalculateCellSize(int rows, int columns)
    {
        var availableWidth = containerRect.rect.width - (padding * 2);
        var availableHeight = containerRect.rect.height - (padding * 2);
        
        availableWidth -= spacing * (columns - 1);
        availableHeight -= spacing * (rows - 1);
        
        var cellWidth = availableWidth / columns;
        var cellHeight = availableHeight / rows;
        
        var cellSize = Mathf.Min(cellWidth, cellHeight);
        cellSize = Mathf.Max(cellSize, 50f);
        
        return new Vector2(cellSize, cellSize);
    }
}
