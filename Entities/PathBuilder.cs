using System;
using System.Collections.Generic;
using System.Linq;

namespace Entities
{
    internal class PathBuilder
    {
        private readonly Map map;
        private readonly IList<Node> openList;
        private readonly IList<Node> closedList;
        private Position objectivePosition;
        private int maxCostValue;
        private IPosition initialPosition;

        internal PathBuilder(Map map)
        {
            if (map==null)
            {
                throw new ArgumentNullException();
            }
            if (map.IsEmpty)
            {
                throw new ArgumentException();
            }
            this.map = map;

            openList = new List<Node>();
            closedList = new List<Node>();
        }

        public IList<Position> GetPath(Character character, Position position, int maxPathDistance)
        {
            Validate(character);
            Validate(position);
            
            ClearLists();

            objectivePosition = position;
            maxCostValue = maxPathDistance;
            initialPosition = map.GetPosition(character);
            return GetPath();
        }

        private IList<Position> GetPath()
        {
            AddToOpenList(initialPosition);
            while (openList.Count > 0)
            {
                var minCostNode = GetMinCostNode();
                if (IsObjective(minCostNode))
                {
                    closedList.Add(minCostNode);
                    break;
                }
                if (IsATooBigPath())
                {
                    closedList.Clear();
                    break;
                }
                UpdateLists(minCostNode);
                CreateAdjacentNodes(minCostNode);
            }

            RemoveInitialPosition();

            return ConvertToPositionsList();
        }

        private bool IsATooBigPath()
        {
            return closedList.Count>maxCostValue;
        }

        private IList<Position> ConvertToPositionsList()
        {
            var returnList = new List<Position>();
            foreach (var node in closedList)
            {
                returnList.Add(node.Position as Position);
            }
            return returnList;
        }

        private void CreateAdjacentNodes(Node minCostNode)
        {
            AddAdjacent(minCostNode.Position.X, minCostNode.Position.Y - 1);
            AddAdjacent(minCostNode.Position.X, minCostNode.Position.Y + 1);
            AddAdjacent(minCostNode.Position.X + 1, minCostNode.Position.Y);
            AddAdjacent(minCostNode.Position.X - 1, minCostNode.Position.Y);
        }

        private void UpdateLists(Node minCostNode)
        {
            openList.Remove(minCostNode);
            closedList.Add(minCostNode);
        }

        private Node GetMinCostNode()
        {
            var minCost = openList.Min(y => y.Cost);
            var lowestValueNode = openList.First(x => x.Cost == minCost);
            return lowestValueNode;
        }

        private bool IsObjective(Node lowestValueNode)
        {
            return lowestValueNode.Position == objectivePosition;
        }

        private void RemoveInitialPosition()
        {
            if (closedList.Count > 0)
            {
                closedList.RemoveAt(0);
            }
        }

        private void ClearLists()
        {
            openList.Clear();
            closedList.Clear();
        }

        private bool IsValid(int cost)
        {
            return cost<=maxCostValue;
        }

        private void AddAdjacent(int adjacentX, int adjacentY)
        {
            var adjacentPosition = map.GetPosition(adjacentX, adjacentY);
            if (map.IsFree(adjacentPosition) && IsNotOnClosedList(adjacentPosition))
            {
                AddToOpenList(adjacentPosition);
            }
        }

        private bool IsNotOnClosedList(IPosition adjacentPosition)
        {
            return closedList.Any(x => x.Position == adjacentPosition) == false;
        }

        private void AddToOpenList(IPosition position)
        {
            var cost = GetCost(position);
            if (IsValid(cost))
            {
                if (IsOnTheOpenList(position, cost))
                {
                    UpdateCost(position, cost);
                }
                else
                {
                    openList.Add(new Node(position, cost));
                }
            }
        }

        private void UpdateCost(IPosition position, int cost)
        {
            openList.First(x => x.Position == position).Cost = cost;
        }

        private bool IsOnTheOpenList(IPosition position, int cost)
        {
            return openList.Any(x => x.Position == position && x.Cost > cost);
        }

        private int GetCost(IPosition position)
        {
            var cost = Math.Abs(position.X - objectivePosition.X) +
                       Math.Abs(position.Y - objectivePosition.Y);
            return cost;
        }

        private void Validate(Position position)
        {
            if (position == null)
            {
                throw new ArgumentNullException("position");
            }
            if (map.Exists(position) == false)
            {
                throw new ArgumentException("Position not exists.");
            }
        }

        private void Validate(Character character)
        {
            if (character == null)
            {
                throw new ArgumentNullException("character");
            }
            if (this.map.Exists(character) == false)
            {
                throw new ArgumentException("Character not exists on the map");
            }
            if (character.Velocity <= 0)
            {
                throw new ArgumentException("Character velocity is required on the algorithm");
            }
        }
        
        public IList<Position> GetPath(IPosition originPosition, IPosition objectivePosition, int maxPathDistance)
        {
            this.objectivePosition = objectivePosition as Position;
            this.maxCostValue = maxPathDistance;
            this.initialPosition = originPosition;
            return GetPath();
        }
    }

    public class Node
    {
        private readonly IPosition position;

        public Node(IPosition position, int cost)
        {
            this.position = position;
            this.Cost = cost;
        }

        public IPosition Position { get { return this.position; } }
        public int Cost { get; set; }
    }
}