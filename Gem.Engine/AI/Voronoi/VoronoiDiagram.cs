using Gem.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Engine.AI.Voronoi
{
    /// <summary>
    /// Voronoi diagram using the fortune's algorithm, a sweep line algorithm for generating a Voronoi diagram
    /// </summary>
    public class VoronoiDiagram
    {

        private List<Vector2> places = new List<Vector2>();
        private List<VoronoiEdge> Edges = new List<VoronoiEdge>();
        private double width;
        private double height;
        private VoronoiParabola root;
        private double ly;
        private List<VoronoiEvent> deleted = new List<VoronoiEvent>();
        private List<Vector2> points = new List<Vector2>();
        private PriorityQueue<VoronoiEvent> queue;

        public IEnumerable<VoronoiEdge> GetEdges(List<Vector2> v, int w, int h)
        {
            places = v;
            width = w;
            height = h;
            root = null;
            if (Edges == null)
            {
                Edges = new List<VoronoiEdge>();
            }
            else
            {
                points.Clear();
                Edges.Clear();
            }

            foreach (var place in places)
            {
                queue.Enqueue(new VoronoiEvent(place, true));
            }

            VoronoiEvent e;
            while (queue.Count > 0)
            {
                e = queue.Dequeue();
                ly = e.Position.Y;
                if (deleted.Find(entry => entry == e) != deleted.Last())
                {
                    deleted.Remove(e); continue;
                }
                if (e.IsPlaceEvent)
                {
                    InsertParabola(e.Position);
                }
                else
                {
                    RemoveParabola(e);
                }
            }
            FinishEdge(root);

            foreach (var Edge in Edges)
            {
                if (Edge.Neighbour != null)
                {
                    Edge.Start = Edge.Neighbour.End;
                    Edge.Neighbour = null;
                }
            }
            return Edges;
        }

        public void InsertParabola(Vector2 p)
        {
            if (root == null) { root = new VoronoiParabola(p); return; }

            if (root.IsLeaf && root.Site.Y - p.Y < 1)
            {
                Vector2 fp = root.Site;
                root.IsLeaf = false;
                root.Left = new VoronoiParabola(fp);
                root.Right = new VoronoiParabola(p);
                Vector2 s = new Vector2((p.X + fp.X) / 2, (float)height);
                points.Add(s);
                if (p.X > fp.X)
                {
                    root.Edge = new VoronoiEdge(s, fp, p);
                }
                else
                {
                    root.Edge = new VoronoiEdge(s, p, fp);
                }
                Edges.Add(root.Edge);
                return;
            }
            VoronoiParabola par = GetParabolaByX(p.X);

            if (par.CEvent != null)
            {
                deleted.Add(par.CEvent);
                par.CEvent = null;
            }

            Vector2 start = new Vector2(p.X, (float)GetY(par.Site, p.X));
            points.Add(start);

            VoronoiEdge el = new VoronoiEdge(start, par.Site, p);
            VoronoiEdge er = new VoronoiEdge(start, p, par.Site);

            el.Neighbour = er;
            Edges.Add(el);

            par.Edge = er;
            par.IsLeaf = false;

            VoronoiParabola p0 = new VoronoiParabola(par.Site);
            VoronoiParabola p1 = new VoronoiParabola(p);
            VoronoiParabola p2 = new VoronoiParabola(par.Site);

            par.Right = p2;
            par.Left = new VoronoiParabola();
            par.Left.Edge = el;

            par.Left.Left = p0;
            par.Left.Right = p1;

            CheckCircle(p0);
            CheckCircle(p2);
        }

        public void RemoveParabola(VoronoiEvent e)
        {
            VoronoiParabola p1 = e.Arch;

            VoronoiParabola xl = VoronoiParabola.GetLeftParent(p1);
            VoronoiParabola xr = VoronoiParabola.GetRightParent(p1);

            VoronoiParabola p0 = VoronoiParabola.GetLeftChild(xl);
            VoronoiParabola p2 = VoronoiParabola.GetRightChild(xr);

            if (p0.CEvent != null) { deleted.Add(p0.CEvent); p0.CEvent = null; }
            if (p2.CEvent != null) { deleted.Add(p2.CEvent); p2.CEvent = null; }

            Vector2 p = new Vector2(e.Position.X, (float)GetY(p1.Site, e.Position.X));
            points.Add(p);

            xl.Edge.End = p;
            xr.Edge.End = p;

            VoronoiParabola higher = null;
            VoronoiParabola par = p1;
            while (par != root)
            {
                par = par.Parent;
                if (par == xl) higher = xl;
                if (par == xr) higher = xr;
            }
            higher.Edge = new VoronoiEdge(p, p0.Site, p2.Site);
            Edges.Add(higher.Edge);

            VoronoiParabola gparent = p1.Parent.Parent;
            if (p1.Parent.Left == p1)
            {
                if (gparent.Left == p1.Parent) gparent.Left = p1.Parent.Right;
                if (gparent.Right == p1.Parent) gparent.Right = p1.Parent.Right;
            }
            else
            {
                if (gparent.Left == p1.Parent) gparent.Left = p1.Parent.Left;
                if (gparent.Right == p1.Parent) gparent.Right = p1.Parent.Left;
            }

            CheckCircle(p0);
            CheckCircle(p2);
        }

        public void FinishEdge(VoronoiParabola n)
        {
            if (n.IsLeaf) { return; }

            double mx;
            if (n.Edge.Direction.X > 0.0) mx = Math.Max(width, n.Edge.Start.X + 10);
            else mx = Math.Min(0.0, n.Edge.Start.X - 10);

            Vector2 end = new Vector2((float)mx, (float)(mx * n.Edge.F + n.Edge.G));
            n.Edge.End = end;
            points.Add(end);

            FinishEdge(n.Left);
            FinishEdge(n.Right);

        }

        public double GetXOfEdge(VoronoiParabola par, double y)
        {
            VoronoiParabola left = VoronoiParabola.GetLeftChild(par);
            VoronoiParabola right = VoronoiParabola.GetRightChild(par);

            Vector2 p = left.Site;
            Vector2 r = right.Site;

            double dp = 2.0 * (p.Y - y);
            double a1 = 1.0 / dp;
            double b1 = -2.0 * p.X / dp;
            double c1 = y + dp / 4 + p.X * p.X / dp;

            dp = 2.0 * (r.Y - y);
            double a2 = 1.0 / dp;
            double b2 = -2.0 * r.X / dp;
            double c2 = ly + dp / 4 + r.X * r.X / dp;

            double a = a1 - a2;
            double b = b1 - b2;
            double c = c1 - c2;

            double disc = b * b - 4 * a * c;
            double x1 = (-b + Math.Sqrt(disc)) / (2 * a);
            double x2 = (-b - Math.Sqrt(disc)) / (2 * a);

            double ry;
            if (p.Y < r.Y) ry = Math.Max(x1, x2);
            else ry = Math.Min(x1, x2);

            return ry;
        }
                public VoronoiParabola GetParabolaByX(double xx)
        {
            VoronoiParabola par = root;
            double x = 0.0;

            while (!par.IsLeaf)
            {
                x = GetXOfEdge(par, ly);
                if (x > xx) par = par.Left;
                else par = par.Right;
            }
            return par;
        }

        public double GetY(Vector2 p, double x)
        {
            double dp = 2 * (p.Y - ly);
            double a1 = 1 / dp;
            double b1 = -2 * p.X / dp;
            double c1 = ly + dp / 4 + p.X * p.X / dp;

            return (a1 * x * x + b1 * x + c1);
        }

        public void CheckCircle(VoronoiParabola b)
        {
            VoronoiParabola lp = VoronoiParabola.GetLeftParent(b);
            VoronoiParabola rp = VoronoiParabola.GetRightParent(b);

            VoronoiParabola a = VoronoiParabola.GetLeftChild(lp);
            VoronoiParabola c = VoronoiParabola.GetRightChild(rp);

            if (a == null || c == null || a.Site == c.Site) return;

            Vector2 s = Vector2.Zero;
            s = GetEdgeIntersection(lp.Edge, rp.Edge);
            if (s == Vector2.Zero) return;

            double dx = a.Site.X - s.X;
            double dy = a.Site.Y - s.Y;

            double d = Math.Sqrt((dx * dx) + (dy * dy));

            if (s.Y - d >= ly) { return; }

            VoronoiEvent e = new VoronoiEvent(new Vector2(s.X, s.Y - (float)d), false);
            points.Add(e.Position);
            b.CEvent = e;
            e.Arch = b;
            queue.Enqueue(e);
        }

        public Vector2 GetEdgeIntersection(VoronoiEdge a, VoronoiEdge b)
        {
            double x = (b.G - a.G) / (a.F - b.F);
            double y = a.F * x + a.G;

            if ((x - a.Start.X) / a.Direction.X < 0) return Vector2.Zero;
            if ((y - a.Start.Y) / a.Direction.Y < 0) return Vector2.Zero;

            if ((x - b.Start.X) / b.Direction.X < 0) return Vector2.Zero;
            if ((y - b.Start.Y) / b.Direction.Y < 0) return Vector2.Zero;

            Vector2 p = new Vector2((float)x, (float)y);
            points.Add(p);
            return p;
        }

    }
}