using UnityEngine;
using System;
using System.Collections.Generic;

public class AStarGrid2D : Singleton<AStarGrid2D> {

    GridNode2D[,] gridData;
    
    [SerializeField]
    int
        gridSizeX;
    [SerializeField]
    int
        gridSizeY;
    [SerializeField]
    int gridCellSize;
    [SerializeField]
    LayerMask
        collisionLayer;
    [SerializeField]
    bool
        alwaysDrawWireFrame = false;
    [SerializeField]
    bool
        autoUpdateGrid = false;

    [SerializeField]
    float gridFactorOverlap = 2f;
    
    public bool AutoUpdateGrid { get { return autoUpdateGrid; } }
    
    public static int CellSize{get{return Instance.gridCellSize;}}
    
    //Astar will not use use usercontext, therefor this can be type System.Object
    SettlersEngine.SpatialAStar<GridNode2D,System.Object> aStar;
    
    public static LinkedList<GridNode2D> GetPath(Vector2 start, Vector2 end){
        start -= new Vector2(Instance.transform.position.x, Instance.transform.position.y);
        end -= new Vector2(Instance.transform.position.x, Instance.transform.position.y);
//        Vector2 onGridStart = new Vector2(Instance.GetClosestGridPoint(start.x) / Instance.gridCellSize, Instance.GetClosestGridPoint(start.y) / Instance.gridCellSize);
//        Vector2 onGridEnd = new Vector2(Instance.GetClosestGridPoint(end.x) / Instance.gridCellSize, Instance.GetClosestGridPoint(end.y) / Instance.gridCellSize);

        if (Instance.aStar != null) {
            return Instance.aStar.Search(start, end, null);
        } else return new LinkedList<GridNode2D>();
    }
    
    void Awake(){
        if(gridData == null){
            Debug.Log("AStarGrid2D: No grid found, recalculating...");
            CreateGrid();
        }
        
        aStar = new SettlersEngine.SpatialAStar<GridNode2D, object>(gridData);
        // clear own gridData to free memory
        gridData = null;
    }
                
    public void CreateGrid() {
        gridData = new GridNode2D[gridSizeX, gridSizeY];
        Int32 posX = GetClosestGridPoint(gameObject.transform.position.x);
        for (int x = 0; x < gridSizeX; x++) {
            Int32 posY = GetClosestGridPoint(gameObject.transform.position.y);
            for (int y = 0; y < gridSizeY; y++) {
                gridData [x, y] = new GridNode2D(posX, posY, !IsWalkableCheck(new Vector2(posX, posY)));
                posY += gridCellSize;
            }
            posX += gridCellSize;
        }
        if(!autoUpdateGrid){
            Debug.Log("AStarGrid2D: Grid calculation complete");
        }
    }
    
    Int32 GetClosestGridPoint(float value) {
        Int32 intValue = (Int32)value;
        return (Int32)(intValue - (intValue % gridCellSize));
    }
    
    bool IsWalkableCheck(Vector2 pos) {
        Vector2 start = new Vector2(pos.x - gridCellSize / gridFactorOverlap, pos.y - gridCellSize / gridFactorOverlap);
        Vector2 end = new Vector2(pos.x + gridCellSize / gridFactorOverlap, pos.y + gridCellSize / gridFactorOverlap);
        return Physics2D.OverlapArea(start, end, collisionLayer) == null;
    }
    
    #region GizmoDraw
    
    void OnDrawGizmos() {
        if (alwaysDrawWireFrame) {
            OnDrawGizmosSelected();
        }
    }
    
    void OnDrawGizmosSelected() {
        Color originalGizmoColor = Gizmos.color;
        Color defaultGridColor = Color.green;
        Color errorGridColor = Color.red;
        if (gridData == null || gridData.Length == 0) {
            return;
        }
        
        if (gridData.GetLength(0) != gridSizeX ||
            gridData.GetLength(1) != gridSizeY || 
            gridData [0, 0].X != GetClosestGridPoint(gameObject.transform.position.x) ||
            gridData [0, 0].Y != GetClosestGridPoint(gameObject.transform.position.y)
            ) {
            defaultGridColor = errorGridColor;
        } 
        
        int intervals = gridData.GetLength(0) * gridData.GetLength(1);
        intervals = (int)Mathf.Log10(intervals/100);
        
        if(intervals < 1){
            intervals = 1;
        }
        
        Int32 posX = GetClosestGridPoint(gameObject.transform.position.x);
        for (int x = 0; x < gridSizeX; x+=intervals) {
            Int32 posY = GetClosestGridPoint(gameObject.transform.position.y);
            for (int y = 0; y < gridSizeY; y+=intervals) {
                if (!gridData [x, y].IsWall) {
                    Gizmos.color = defaultGridColor;
                } else {
                    Gizmos.color = errorGridColor;
                }
                Gizmos.DrawWireCube(new Vector3(posX, posY, 0), new Vector3(gridCellSize, gridCellSize, gridCellSize));
                posY += gridCellSize * intervals;
            }
            posX += gridCellSize * intervals;
        }
        Gizmos.color = originalGizmoColor;
    }
    #endregion 
}
