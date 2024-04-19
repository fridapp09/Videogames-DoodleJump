namespace UnderTheSea
{
    public class Verlet
    {
        public Transform transform;
        public float gravity;
        float a;

        public float dx;
        bool usedBonus = false;

        public Verlet(PointF position, Size size)
        {
            transform = new Transform(position, size);
            gravity = 0;
            a = 0.4f;
            dx = 0;
        }

        public void ApplyVerlet()
        {
            CalculateVerlet();
        }

        public void CalculateVerlet()
        {
            if (dx != 0)
            {
                transform.position.X += dx;
            }
            if (transform.position.Y < 700)
            {
                transform.position.Y += gravity;
                gravity += a;

                if (gravity > -25 && usedBonus)
                {
                    Controlador.GenerateRandomPlatform();
                    Controlador.startPlatformPosY = -200;
                    Controlador.GenerateStartSequence();
                    Controlador.startPlatformPosY = 0;
                    usedBonus = false;
                }

                Collide();
            }
        }

        public bool StandartCollidePlayerWithObjects(bool forMonsters, bool forBonuses)
        {
            bool collided = false; // Variable para indicar si ha ocurrido una colisión

            for (int i = 0; i < Controlador.enemigos.Count; i++)
            {
                var enemigo = Controlador.enemigos[i];
                PointF delta = new PointF();
                delta.X = (transform.position.X + transform.size.Width / 2) - (enemigo.verlet.transform.position.X + enemigo.verlet.transform.size.Width / 2);
                delta.Y = (transform.position.Y + transform.size.Height / 2) - (enemigo.verlet.transform.position.Y + enemigo.verlet.transform.size.Height / 2);
                if (Math.Abs(delta.X) <= transform.size.Width / 2 + enemigo.verlet.transform.size.Width / 2 &&
                    Math.Abs(delta.Y) <= transform.size.Height / 2 + enemigo.verlet.transform.size.Height / 2)
                {
                    // Si no se ha utilizado el bono, aumentar el puntaje y marcar la colisión como verdadera
                    if (!usedBonus)
                    {
                        Controlador.score += 15; // Aumentar el puntaje por matar a un enemigo
                        collided = true;
                    }
                }
            }
            if (forBonuses)
            {
                for (int i = 0; i < Controlador.bonuses.Count; i++)
                {
                    var bonus = Controlador.bonuses[i];
                    PointF delta = new PointF();
                    delta.X = (transform.position.X + transform.size.Width / 2) - (bonus.verlet.transform.position.X + bonus.verlet.transform.size.Width / 2);
                    delta.Y = (transform.position.Y + transform.size.Height / 2) - (bonus.verlet.transform.position.Y + bonus.verlet.transform.size.Height / 2);
                    if (Math.Abs(delta.X) <= transform.size.Width / 2 + bonus.verlet.transform.size.Width / 2 &&
                        Math.Abs(delta.Y) <= transform.size.Height / 2 + bonus.verlet.transform.size.Height / 2)
                    {
                        if (bonus.type == 1 && !usedBonus)
                        {
                            Controlador.score += 5; // Aumentar el puntaje por tocar las burbujas
                            usedBonus = true;
                            AddForce(-30);
                            collided = true;
                        }
                        // Si el bono es del tipo 2 y no se ha utilizado, aumentar el puntaje y aplicar fuerza
                        else if (bonus.type == 2 && !usedBonus)
                        {
                            Controlador.score += 10; // Aumentar el puntaje por tocar el tridente
                            usedBonus = true;
                            AddForce(-60);
                            collided = true;
                        }
                    }
                }
            }
            return collided; // Devolver el valor de colisión
        }

        public bool StandartCollide()
        {
            for (int i = 0; i < Controlador.balas.Count; i++)
            {
                var balas = Controlador.balas[i];
                PointF delta = new PointF();
                delta.X = (transform.position.X + transform.size.Width / 2) - (balas.verlet.transform.position.X + balas.verlet.transform.size.Width / 2);
                delta.Y = (transform.position.Y + transform.size.Height / 2) - (balas.verlet.transform.position.Y + balas.verlet.transform.size.Height / 2);
                if (Math.Abs(delta.X) <= transform.size.Width / 2 + balas.verlet.transform.size.Width / 2)
                {
                    if (Math.Abs(delta.Y) <= transform.size.Height / 2 + balas.verlet.transform.size.Height / 2)
                    {
                        Controlador.RemoveBullet(i);
                        return true;
                    }
                }
            }
            return false;
        }

        public void Collide()
        {
            for (int i = 0; i < Controlador.plataformas.Count; i++)
            {
                var plataforma = Controlador.plataformas[i];
                if (transform.position.X + transform.size.Width / 2 >= plataforma.transform.position.X && transform.position.X + transform.size.Width / 2 <= plataforma.transform.position.X + plataforma.transform.size.Width)
                {
                    if (transform.position.Y + transform.size.Height >= plataforma.transform.position.Y && transform.position.Y + transform.size.Height <= plataforma.transform.position.Y + plataforma.transform.size.Height)
                    {
                        if (gravity > 0)
                        {
                            AddForce();
                            if (!plataforma.isTouchedByPlayer)
                            {
                                Controlador.score += 20;
                                Controlador.GenerateRandomPlatform();
                                plataforma.isTouchedByPlayer = true;
                            }
                        }
                    }
                }
            }
        }

        public void AddForce(int force = -13)
        {
            gravity = force;
        }
    }
}
