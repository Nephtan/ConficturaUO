using System;
using System.Collections.Generic;

namespace Server.Custom.Confictura.Champions
{
    // Runtime-only planning data. Population is read from persistent residents, never birth totals.
    public sealed class QueenBroodMassPlanner
    {
        private sealed class Node
        {
            public int Id, Weight, Component = -1;
            public Point3D Position;
            public Node Previous; public bool Routed;
        }
        private static readonly int[] DirectionX = {0,12,0,-12}, DirectionY = {-12,0,12,0};
        private readonly List<Node> m_Nodes = new List<Node>();
        private readonly Dictionary<Point2D,Node> m_Cells = new Dictionary<Point2D,Node>();
        private readonly Queue<Node> m_Open = new Queue<Node>();
        private List<Node> m_Component = new List<Node>(), m_Largest;
        private readonly Point3D m_Queen;
        private Node m_NearQueen, m_GoalNode, m_RouteCursor, m_ComponentEntry, m_LargestEntry;
        private double m_NearQueenDistance=Double.MaxValue;
        private readonly List<Point3D> m_Route=new List<Point3D>();
        public List<Point3D> Route { get { return m_Route; } }
        private readonly int m_CurrentGoal;
        private int m_Cursor, m_ComponentId, m_Stage, m_Rank, m_Mass, m_BestMass, m_BestComponent;
        private double m_X, m_Y, m_BestX, m_BestY, m_Nearest = Double.MaxValue, m_BestNearest = Double.MaxValue;
        private double m_BestDistance = Double.MaxValue;
        public bool Complete { get; private set; }
        public int GoalId { get; private set; }
        public Point3D Goal { get; private set; }
        public int Mass { get { return m_BestMass; } }
        public int LocalMass { get; private set; }
        public bool CurrentInLargest { get; private set; }
        public int CurrentLocalMass { get; private set; }
        public double CurrentDistance { get; private set; }
        public double GoalDistance { get { return Math.Sqrt(m_BestDistance); } }
        public Func<int,bool> Excluded;

        public QueenBroodMassPlanner(Point3D queen, int currentGoal)
        { m_Queen = queen; m_CurrentGoal = currentGoal; GoalId = -1; CurrentDistance = Double.MaxValue; }

        public void Add(int id, Point3D position, int weight)
        {
            if (weight <= 0) return;
            Node node = new Node(); node.Id = id; node.Position = position; node.Weight = weight;
            double near=Distance(position,m_Queen.X,m_Queen.Y);if(near<m_NearQueenDistance){m_NearQueenDistance=near;m_NearQueen=node;}
            m_Nodes.Add(node); m_Cells.Add(new Point2D(position.X,position.Y),node);
        }

        private static double Distance(Point3D p, double x, double y)
        { double dx = p.X-x, dy = p.Y-y; return dx*dx+dy*dy; }

        private void FinishComponent()
        {
            if (m_Component.Count == 0) return;
            if (m_Mass > m_BestMass || m_Mass == m_BestMass && m_Nearest < m_BestNearest)
            {
                m_Largest = m_Component; m_BestMass = m_Mass; m_BestX = m_X; m_BestY = m_Y;
                m_BestNearest = m_Nearest; m_BestComponent = m_ComponentId; m_LargestEntry=m_ComponentEntry;
            }
            m_Component = new List<Node>(); m_Mass = 0; m_X = m_Y = 0; m_Nearest = Double.MaxValue; m_ComponentEntry=null;
        }

        public void Advance(int work)
        {
            while (!Complete && work-- > 0)
            {
                if (m_Stage == 0)
                {
                    if (m_Open.Count == 0)
                    {
                        FinishComponent();
                        if (m_Cursor >= m_Nodes.Count) { m_Stage = 1; continue; }
                        Node first = m_Nodes[m_Cursor++]; if (first.Component >= 0) continue;
                        first.Component = ++m_ComponentId; m_Open.Enqueue(first);
                    }
                    Node node = m_Open.Dequeue(); m_Component.Add(node); m_Mass += node.Weight;
                    m_X += (double)node.Position.X*node.Weight; m_Y += (double)node.Position.Y*node.Weight;
                    double entryDistance=Distance(node.Position,m_Queen.X,m_Queen.Y);if(entryDistance<m_Nearest){m_Nearest=entryDistance;m_ComponentEntry=node;}
                    for (int i = 0; i < 4; ++i)
                    {
                        Node next;
                        if (m_Cells.TryGetValue(new Point2D(node.Position.X+DirectionX[i],node.Position.Y+DirectionY[i]),out next) && next.Component < 0)
                        { next.Component = m_ComponentId; m_Open.Enqueue(next); }
                    }
                }
                else if(m_Stage==1)
                {
                    if (m_Largest == null || m_Rank >= m_Largest.Count)
                    {
                        if(m_GoalNode==null){Complete=true;continue;}
                        Node entry=m_NearQueen!=null&&m_NearQueen.Component==m_BestComponent?m_NearQueen:m_LargestEntry;
                        if(entry==null){Complete=true;continue;}
                        m_Open.Enqueue(entry);entry.Routed=true;m_Stage=2;continue;
                    }
                    Node node = m_Largest[m_Rank++]; int local = 0;
                    for (int x = -24; x <= 24; x += 12)
                        for (int y = -24; y <= 24; y += 12)
                        {
                            Node other;
                            if (m_Cells.TryGetValue(new Point2D(node.Position.X+x,node.Position.Y+y),out other) && other.Component == m_BestComponent) local += other.Weight;
                        }
                    double distance = Distance(node.Position,m_BestX/m_BestMass,m_BestY/m_BestMass);
                    if (node.Id == m_CurrentGoal) { CurrentInLargest = true; CurrentLocalMass = local; CurrentDistance = Math.Sqrt(distance); }
                    if (Excluded != null && Excluded(node.Id)) continue;
                    if (GoalId < 0 || local > LocalMass || local == LocalMass && (distance < m_BestDistance || distance == m_BestDistance && node.Id < GoalId))
                    { GoalId = node.Id; Goal = node.Position; LocalMass = local; m_BestDistance = distance; m_GoalNode=node; }
                }
                else if(m_Stage==2)
                {
                    if(m_Open.Count==0){m_Route.Add(Goal);Complete=true;continue;}
                    Node node=m_Open.Dequeue();
                    if(node==m_GoalNode){m_RouteCursor=node;m_Stage=3;continue;}
                    for(int i=0;i<4;++i){Node next;if(m_Cells.TryGetValue(new Point2D(node.Position.X+DirectionX[i],node.Position.Y+DirectionY[i]),out next)&&next.Component==m_BestComponent&&!next.Routed){next.Routed=true;next.Previous=node;m_Open.Enqueue(next);}}
                }
                else
                {
                    if(m_RouteCursor==null){m_Route.Reverse();Complete=true;continue;}
                    m_Route.Add(m_RouteCursor.Position);m_RouteCursor=m_RouteCursor.Previous;
                }
            }
        }
    }
}
