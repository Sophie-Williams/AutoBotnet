using System;
using System.Collections.Generic;
using C5;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Map {
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
                return f - other.f;
            }
        }

        public Pathfinder(ISContext context, RoomPosition start, RoomPosition goal)
            : this(context, start, goal, tile => tile.walkable) { }

        public Pathfinder(ISContext context, RoomPosition start, RoomPosition goal, Predicate<Tile> passable)
            : base(context) {
            _start = start;
            _goal = goal;
            _passable = passable;
        }

        public List<RoomPosition> findPath(bool includeGoal = true) {
            if (_start == _goal) return new List<RoomPosition>();
            if (_start.roomPos.x != _goal.roomPos.x || _start.roomPos.y != _goal.roomPos.y)
                throw new NotImplementedException("Inter-room pathfinding not implemented yet");

            // add the start node to the open list
            _openList.Add(_nodeGrid[_start.pos.x, _start.pos.y] =
                new Node(_start.pos.x, _start.pos.y, 0, _goal.pathDistance(_start), null));

            var curNode = default(Node);
            while (!_openList.IsEmpty) {
                curNode = _openList.DeleteMin(); // pop the next node off the open list
                curNode.open = false;

                // check if we've reached the goal
                var dx = _goal.pos.x - curNode.x;
                var dy = _goal.pos.y - curNode.y;
                if (System.Math.Abs(dx) + System.Math.Abs(dy) <= (includeGoal ? 0 : 1)) {
                    // return the found path
                    var path = new List<RoomPosition>();
                    while (curNode.parent != null) {
                        path.Add(new RoomPosition(_start.roomPos, new Point(curNode.x, curNode.y)));
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
                if (_passable(new RoomPosition(_start.roomPos, new Point(x, y)).getTile(serverContext))) {
                    // unvisited node; add to open list
                    node = _nodeGrid[x, y] = new Node(x, y, g,
                        _goal.pathDistance(new RoomPosition(_start.roomPos, new Point(x, y))), parent);
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

        private Predicate<Tile> _passable;
        private RoomPosition _start, _goal;

        private Node[,] _nodeGrid = new Node[Room.MAP_EDGE_SIZE, Room.MAP_EDGE_SIZE];
        private IntervalHeap<Node> _openList = new IntervalHeap<Node>();
    }
}