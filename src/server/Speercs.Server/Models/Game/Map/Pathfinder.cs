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
            public int x, y;
            public int g, h;

            public int f {
                get { return g + h; }
            }

            public IPriorityQueueHandle<Node> pqHandle;

            public Node(int x, int y, int g, int h, Node parent) {
                this.x = x;
                this.y = y;
                this.g = g;
                this.h = h;
                this.parent = parent;
            }

            public int CompareTo(Node other) {
                return this.f - other.f;
            }
        }

        public Pathfinder(ISContext context, RoomPosition start, RoomPosition goal)
            : this(context, start, goal, tile => tile.isWalkable()) { }

        public Pathfinder(ISContext context, RoomPosition start, RoomPosition goal, Predicate<ITile> passable)
            : base(context) {
            this._start = start;
            this._goal = goal;
            this._passable = passable;
        }

        public List<RoomPosition> findPath() {
            if (_start == _goal) return new List<RoomPosition>();
            if (_start.roomX != _goal.roomX || _start.roomY != _goal.roomY)
                throw new NotImplementedException("Inter-room pathfinding not implemented yet");

            // add the start node to the open list
            _openList.Add(_nodeGrid[_start.x, _start.y] = new Node(_start.x, _start.y, 0, _goal.distance(_start), null));

            while (!_openList.IsEmpty) {
                Node curNode = _openList.DeleteMin(); // pop the next node off the open list
                curNode.open = false;

                // check if we've reached the goal
                if (curNode.x == _goal.x && curNode.y == _goal.y) {
                    // return the found path
                    var path = new List<RoomPosition>();
                    while (curNode.parent != null) {
                        path.Add(new RoomPosition(_start, curNode.x, curNode.y));
                        curNode = curNode.parent;
                    }

                    path.Reverse();
                    return path;
                }

                // add valid neighbors to the open list
                var x = curNode.x;
                var y = curNode.y;
                var g = curNode.g + 1;
                if (x > 0) tryOpenNode(x - 1, y, g, curNode);
                if (x < Room.MAP_EDGE_SIZE - 1) tryOpenNode(x + 1, y, g, curNode);
                if (y > 0) tryOpenNode(x, y - 1, g, curNode);
                if (y < Room.MAP_EDGE_SIZE - 1) tryOpenNode(x, y + 1, g, curNode);
            }

            // there isn't a path to the goal
            return null;
        }

        private void tryOpenNode(int x, int y, int g, Node parent) {
            var node = _nodeGrid[x, y];
            if (node == null) {
                if (_passable(new RoomPosition(_start, x, y).getTile(serverContext))) {
                    // unvisited node; add to open list
                    node = _nodeGrid[x, y] = new Node(x, y, g, _goal.distance(new RoomPosition(_goal, x, y)), parent);
                    _openList.Add(ref node.pqHandle, node);
                }
            } else if (node.open) {
                if (g < node.g) {
                    // this route is better
                    node.g = g;
                    node.parent = parent;
                    _openList.Replace(node.pqHandle, node); // update in the priority queue
                }
            }
        }

        private Predicate<ITile> _passable;
        private RoomPosition _start, _goal;

        private Node[,] _nodeGrid = new Node[Room.MAP_EDGE_SIZE, Room.MAP_EDGE_SIZE];
        private IntervalHeap<Node> _openList = new IntervalHeap<Node>();
    }
}