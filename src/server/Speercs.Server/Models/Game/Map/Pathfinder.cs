
using System;
using System.Collections.Generic;
using C5;
using Speercs.Server.Extensibility;

namespace Speercs.Server.Models.Game.Map
{
    public class Pathfinder
    {
        private class Node : IComparable<Node>
        {
            public bool open = true;
            public Node parent;
            public int X, Y;
            public int G, H;
            public int F { get { return G + H; } }
            
            public Node(int x, int y, int g, int h, Node parent)
            {
                X = x;
                Y = y;
                G = g;
                H = h;
                this.parent = parent;
            }
            
            public int CompareTo(Node other)
            {
                return this.F - other.F;
            }
        }
        
        public List<RoomPosition> FindPath(RoomPosition start, RoomPosition goal)
        {
            return FindPath(start, goal, tile => tile.IsWalkable());
        }
        
        public List<RoomPosition> FindPath(RoomPosition start, RoomPosition goal, Func<ITile, bool> passable)
        {
            if (start == goal) return new List<RoomPosition>();
            if (start.RoomX != goal.RoomX || start.RoomY != goal.RoomY)
                throw new NotImplementedException("Inter-room pathfinding not implemented yet");
            
            var nodeGrid = new Node[Room.MapEdgeSize, Room.MapEdgeSize];
            var openList = new IntervalHeap<Node>();
            var closedList = new List<Node>();
            
            // add the start node to the open list
            openList.Add(nodeGrid[start.X, start.Y] = new Node(start.X, start.Y, 0, start.Distance(goal), null));
            
            while (!openList.IsEmpty)
            {
                Node curNode = openList.DeleteMin(); // pop the next node off the open list
                curNode.open = false;
                closedList.Add(curNode);
                
                // check if we've reached the goal
                if (curNode.X == goal.X && curNode.Y == goal.Y)
                {
                    // return the found path
                    var path = new List<RoomPosition>();
                    while (curNode.parent != null)
                    {
                        path.Add(new RoomPosition(start, curNode.X, curNode.Y));
                        curNode = curNode.parent;
                    }
                    path.Reverse();
                    return path;
                }
                
                // add valid neighbors to the open list
                // TODO
            }
            
            // there isn't a path to the goal
            return null;
        }
    }
}