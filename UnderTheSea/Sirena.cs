namespace UnderTheSea
{
    public class Sirena
    {
        public Verlet verlet;
        public Image sprite;

        public Sirena()
        {
            sprite = Image.FromFile("Resources/sirena.png");
            verlet = new Verlet(new PointF(100, 350), new Size(65, 80));
        }

        public void DrawSprite(Graphics g)
        {
            g.DrawImage(sprite, verlet.transform.position.X, verlet.transform.position.Y, verlet.transform.size.Width, verlet.transform.size.Height);
        }
    }
}
