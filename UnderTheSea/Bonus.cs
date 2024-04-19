namespace UnderTheSea
{
    public class Bonus
    {
        public Verlet verlet;
        public Image sprite;
        public int type;

        public Bonus(PointF pos, int type)
        {
            switch (type)
            {
                case 1:
                    sprite = Image.FromFile("Resources/burbujas.png");
                    verlet = new Verlet(pos, new Size(55, 50));
                    break;
                case 2:
                    sprite = Image.FromFile("Resources/tridente.png");
                    verlet = new Verlet(pos, new Size(70, 60));
                    break;
            }
            this.type = type;
        }

        public void DrawSprite(Graphics g)
        {
            g.DrawImage(sprite, verlet.transform.position.X, verlet.transform.position.Y, verlet.transform.size.Width, verlet.transform.size.Height);
        }
    }
}
