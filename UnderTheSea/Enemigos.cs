namespace UnderTheSea
{
    public class Enemigos : Sirena
    {
        public Enemigos(PointF pos, int type)
        {
            switch (type)
            {
                case 1:
                    sprite = Image.FromFile("Resources/anguila.png");
                    verlet = new Verlet(pos, new Size(80, 60));
                    break;
                case 2:
                    sprite = Image.FromFile("Resources/tiburon.png");
                    verlet = new Verlet(pos, new Size(80, 60));
                    break;
                case 3:
                    sprite = Image.FromFile("Resources/medusa.png");
                    verlet = new Verlet(pos, new Size(70, 60));
                    break;
            }
        }
    }
}
