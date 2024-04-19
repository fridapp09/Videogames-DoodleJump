namespace UnderTheSea
{
    public class Plataformas
    {
        Image sprite;
        public Transform transform;
        public int sizeX;
        public int sizeY;
        public bool isTouchedByPlayer;

        public Plataformas(PointF pos)
        {
            sprite = Image.FromFile("Resources/coral.png");
            sizeX = 100;
            sizeY = 30;
            transform = new Transform(pos, new Size(sizeX, sizeY));
            isTouchedByPlayer = false;
        }

        public void DrawSprite(Graphics g)
        {
            g.DrawImage(sprite, transform.position.X, transform.position.Y, transform.size.Width, transform.size.Height);
        }
    }
}
