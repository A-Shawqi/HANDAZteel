using HANDAZ.PEB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.PEB.Entities
{
    public class Grid
    {

        int gridId;
        Node startNode;
        Node endNode;

        public int GridId
        {
            get
            {
                return gridId;
            }

            set
            {
                gridId = value;
            }
        }

        public Node StartNode
        {
            get
            {
                return startNode;
            }

            set
            {
                startNode = value;
            }
        }

        public Node EndNode
        {
            get
            {
                return endNode;
            }

            set
            {
                endNode = value;
            }
        }

        public Grid(int _gridId, Node _startNode, Node _endNode)
        {
            GridId = _gridId;
            StartNode = _startNode;
            EndNode = _endNode;
        }
    }
}
