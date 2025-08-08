# Unity Match-3 Grid System (Zenject + DOTween)

A modular **Match-3 style grid system** built in Unity, leveraging **Zenject** for dependency injection and **DOTween** for smooth animations.  
This project demonstrates how to create, visualize, and interact with a 2D grid of game pieces, including match detection, popping animations, and neighbor handling.

---

## ✨ Features

- **Grid Generation**
  - Dynamically creates a 2D grid of nodes.
  - Assigns random colors to each node from a predefined enum.
- **Neighbor Calculation**
  - Precomputes neighbors for each cell based on grid directions.
- **Match Detection**
  - Recursive flood-fill style search for same-colored connected nodes.
- **Popping Animation**
  - Uses DOTween to animate matching nodes before destroying them.
- **Event-Driven Architecture**
  - Custom event system for handling clicks and matches.
- **Zenject Integration**
  - Loose coupling via dependency injection.
- **Input Handling**
  - Uses Unity’s new Input System to detect clicks on grid positions.

---

## 🛠️ Technologies Used

- **Unity** (2021+)
- **Zenject** (Dependency Injection)
- **DOTween** (Animation)
- **Unity New Input System**
- C# 9.0+
  
---

## 📂 Project Structure

```
/Scripts
│
├── Core
│   ├── ConstantValues.cs         # Predefined directions for grid neighbor search
│   ├── EventManager.cs           # Static event system for raising/subscribing events
│   ├── GameEvent<T>.cs           # Generic event wrapper
│   ├── OnClick.cs                # Struct for passing click position data
│
├── Grid
│   ├── Node.cs                   # Base data class for a grid cell
│   ├── NodeType.cs               # Enum for node types (Empty, Normal, Locked)
│   ├── NodeColor.cs              # Enum for node colors
│   ├── GridManager.cs            # Handles grid creation, neighbor assignment, and queries
│   ├── GridVisualizer.cs         # Creates visual representations and handles animations
│   ├── GridDebugger.cs           # Prints neighbor information for debugging
│   ├── NodeHandler.cs            # Moves nodes recursively if spaces are empty
│
├── Gameplay
│   ├── MatchManager.cs           # Detects matches in the grid
│   ├── InputManager.cs           # Handles click detection and converts screen coords to grid coords
│
├── Installers
│   ├── ZenjectInstaller.cs       # Zenject bindings for all components
```

---

## 🚀 How It Works

1. **Initialization**
   - `ZenjectInstaller` binds all key managers.
   - `GridManager` generates the grid and assigns neighbors.
   - `GridVisualizer` spawns node prefabs with random colors.
   
2. **Input**
   - `InputManager` detects clicks and raises an `OnClick` event.
   
3. **Match Detection**
   - `MatchManager` listens for `OnClick` and searches for connected nodes of the same color.
   
4. **Popping Animation**
   - `GridVisualizer` animates matched nodes using DOTween's `DOPunchScale` before destroying them.
   
5. **Debugging**
   - `GridDebugger` can print all neighbors for a given node position.

---

## 🎮 Setup Instructions

1. **Install Dependencies**
   - [Zenject](https://github.com/modesttree/Zenject)
   - [DOTween](http://dotween.demigiant.com/)
   - [Unity Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/index.html)

2. **Scene Setup**
   - Create an empty GameObject and attach `ZenjectInstaller`.
   - Assign references for:
     - `GridManager`
     - `GridVisualizer`
     - `MatchManager`
     - `GridDebugger`
   - Set grid size, spawn rate, and prefabs.

3. **Prefabs**
   - Create a `GridPrefab` (empty cell) and `NodePrefabs` for each color.
   - Assign them in `GridVisualizer`.

4. **Play**
   - Click on nodes to trigger match detection and popping.

---

## 📜 Example Code Snippets

**Match Detection Example**
```csharp
public List<Node> MatchedPieces(Vector2Int startPoint)
{
    Node startNode = _gridManager.Cells[startPoint.x, startPoint.y];
    HashSet<Node> visited = new();
    List<Node> matches = new();
    SearchRecursive(startNode, startNode.NodeColorValue, ref visited, matches);
    return matches;
}
```

**Neighbor Calculation**
```csharp
private List<Vector2Int> SetNeighboors(int x, int y)
{
    List<Vector2Int> newNeighboors = new();
    foreach (Vector2Int dir in ConstantValues.Directions)
    {
        int newX = dir.x + x;
        int newY = dir.y + y;

        if (newX >= 0 && newX < _size.x && newY >= 0 && newY < _size.y)
        {
            newNeighboors.Add(new Vector2Int(newX, newY));
        }
    }
    return newNeighboors;
}
```
