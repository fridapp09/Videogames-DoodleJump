namespace UnderTheSea
{
    public class Bala
    {
        public Verlet verlet;
        public Image sprite;

        public Bala(PointF pos)
        {
            sprite = Image.FromFile("Resources/concha.png");
            verlet = new Verlet(pos, new Size(25, 20));
        }

        public void MoveUp()
        {
            verlet.transform.position.Y -= 15;
        }

        public void DrawSprite(Graphics g)
        {
            g.DrawImage(sprite, verlet.transform.position.X, verlet.transform.position.Y, verlet.transform.size.Width, verlet.transform.size.Height);
        }
    }
}
