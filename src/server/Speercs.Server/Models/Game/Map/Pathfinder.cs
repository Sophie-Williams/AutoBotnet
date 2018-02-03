using System;
using System.Collections.Generic;
using C5;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility;

namespace Speercs.Server.Models.Game.Map {
    public class Pathfinder : DependencyObject {
        private class Node : IComparable<Node> {
            public bool open = true;
            public Node parent;
            public int X, Y;
            public int G, H;

            public int F {
                get { return G + H; }
            }

            public IPriorityQueueHandle<Node> pqHandle;

            public Node(int x, int y, int g, int h, Node parent) {
                X = x;
                Y = y;
                G = g;
                H = h;
                this.parent = parent;
            }

            public int CompareTo(Node other) {
                return this.F - other.F;
            }
        }

        public Pathfinder(ISContext context, RoomPosition start, RoomPosition goal)
            : this(context, start, goal, tile => tile.IsWalkable()) { }

        public Pathfinder(ISContext context, RoomPosition start, RoomPosition goal, Predicate<ITile> passable)
            : base(context) {
            this.start = start;
            this.goal = goal;
            this.passable = passable;
        }

        public List<RoomPosition> FindPath() {
            if (start == goal) return new List<RoomPosition>();
            if (start.RoomX != goal.RoomX || start.RoomY != goal.RoomY)
                throw new NotImplementedException("Inter-room pathfinding not implemented yet");

            // add the start node to the open list
            openList.Add(nodeGrid[start.X, start.Y] = new Node(start.X, start.Y, 0, goal.Distance(start), null));

            while (!openList.IsEmpty) {
                Node curNode = openList.DeleteMin(); // pop the next node off the open list
                curNode.open = false;

                // check if we've reached the goal
                if (curNode.X == goal.X && curNode.Y == goal.Y) {
                    // return the found path
                    var path = new List<RoomPosition>();
                    while (curNode.parent != null) {
                        path.Add(new RoomPosition(start, curNode.X, curNode.Y));
                        curNode = curNode.parent;
                    }

                    path.Reverse();
                    return path;
                }

                // add valid neighbors to the open list
                var x = curNode.X;
                var y = curNode.Y;
                var g = curNode.G + 1;
                if (x > 0) tryOpenNode(x - 1, y, g, curNode);
                if (x < Room.MapEdgeSize - 1) tryOpenNode(x + 1, y, g, curNode);
                if (y > 0) tryOpenNode(x, y - 1, g, curNode);
                if (y < Room.MapEdgeSize - 1) tryOpenNode(x, y + 1, g, curNode);
            }

            // there isn't a path to the goal
            return null;
        }

        private void tryOpenNode(int x, int y, int g, Node parent) {
            var node = nodeGrid[x, y];
            if (node == null) {
                if (passable(new RoomPosition(start, x, y).GetTile(ServerContext))) {
                    // unvisited node; add to open list
                    node = nodeGrid[x, y] = new Node(x, y, g, goal.Distance(new RoomPosition(goal, x, y)), parent);
                    openList.Add(ref node.pqHandle, node);
                }
            } else if (node.open) {
                if (g < node.G) {
                    // this route is better
                    node.G = g;
                    node.parent = parent;
                    openList.Replace(node.pqHandle, node); // update in the priority queue
                }
            }
        }

        private Predicate<ITile> passable;
        private RoomPosition start, goal;

        private Node[,] nodeGrid = new Node[Room.MapEdgeSize, Room.MapEdgeSize];
        private IntervalHeap<Node> openList = new IntervalHeap<Node>();
    }
}