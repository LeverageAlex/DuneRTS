using UnityEngine;

public class EditableTile : Tile {

    [SerializeField] private Material selectedMaterial;


    private Material duneTileMaterial;
    private Material mountainTileMaterial;
    private Material flatSandTileMaterial;
    private Material plateauTileMaterial;
    private Material cityTileMaterial;

    private bool selected = false;
    private TileSelectionObserver tileSelectionObserver;

    // when tile is clicked
    void OnMouseDown() {
        SetSelected(!this.selected, true);
    }

    // when mouse moves over tile & mouse button pressed
    void OnMouseEnter() {
        if (Input.GetMouseButton(0)) SetSelected(true, true);
    }

    // select the tile (gets highlighted), observer called when notify = true
    public void SetSelected(bool selected, bool notify = false) {
        this.selected = selected;

        // tile selected -> highlight
        if (selected) {
            if (flatSandTileMaterial == null) {
                duneTileMaterial = duneTile.GetComponent<Renderer>().material;
                mountainTileMaterial = mountainTile.GetComponent<Renderer>().material;
                flatSandTileMaterial = flatSandTile.GetComponent<Renderer>().material;
                plateauTileMaterial = plateauTile.GetComponent<Renderer>().material;
                cityTileMaterial = cityTile.GetComponent<Renderer>().material;
            }

            duneTile.GetComponent<Renderer>().material = selectedMaterial;
            mountainTile.GetComponent<Renderer>().material = selectedMaterial;
            flatSandTile.GetComponent<Renderer>().material = selectedMaterial;
            plateauTile.GetComponent<Renderer>().material = selectedMaterial;
            cityTile.GetComponent<Renderer>().material = selectedMaterial;

            // notify observer
            if (notify && tileSelectionObserver != null) tileSelectionObserver.OnTileSelected(this);
        }

        // reset to default material
        else if (flatSandTileMaterial != null) {
            duneTile.GetComponent<Renderer>().material = duneTileMaterial;
            mountainTile.GetComponent<Renderer>().material = mountainTileMaterial;
            flatSandTile.GetComponent<Renderer>().material = flatSandTileMaterial;
            plateauTile.GetComponent<Renderer>().material = plateauTileMaterial;
            cityTile.GetComponent<Renderer>().material = cityTileMaterial;

            // notify observer
            if (notify && tileSelectionObserver != null) tileSelectionObserver.OnTileDeselected(this);
        }

    }

    public void setSelectionObserver(TileSelectionObserver observer) {
        tileSelectionObserver = observer;
    }

}

public interface TileSelectionObserver {

    public void OnTileSelected(EditableTile tile);
    public void OnTileDeselected(EditableTile tile);

}
