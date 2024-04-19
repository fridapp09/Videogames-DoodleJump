namespace UnderTheSea
{
    public static class Controlador
    {
        public static List<Plataformas> plataformas;
        public static List<Bala> balas = new List<Bala>();
        public static List<Enemigos> enemigos = new List<Enemigos>();
        public static List<Bonus> bonuses = new List<Bonus>();
        public static int startPlatformPosY = 400;
        public static int score = 0;

        public static void AddPlatform(PointF position)
        {
            Plataformas plataforma = new Plataformas(position);
            plataformas.Add(plataforma);
        }

        public static void CreateBullet(PointF pos)
        {
            var bala = new Bala(pos);
            balas.Add(bala);
        }

        public static void GenerateStartSequence()
        {
            Random r = new Random();
            for (int i = 0; i < 11; i++)
            {
                int x = r.Next(0, 250);
                int y = r.Next(30, 45);
                startPlatformPosY -= y;
                PointF position = new PointF(x, startPlatformPosY);
                Plataformas plataforma = new Plataformas(position);
                plataformas.Add(plataforma);
            }
        }
         
        public static void GenerateRandomPlatform()
        {
            ClearPlatforms();
            Random r = new Random();
            int numberOfPlatforms = 3; // Define el número de plataformas a generar

            for (int i = 0; i < numberOfPlatforms; i++)
            {
                int x = r.Next(0, 200);
                int y = r.Next(20, 35);// Reducir la distancia vertical entre las plataformas
                startPlatformPosY -= y;
                PointF position = new PointF(x, startPlatformPosY);
                Plataformas plataforma = new Plataformas(position);
                plataformas.Add(plataforma);

                // Ajusta los valores para cambiar la frecuencia de generación de enemigos y bonificaciones
                int chanceOfEnemy = 2; // Ajusta este valor para cambiar la frecuencia de generación de enemigos
                int chanceOfBonus = 1; // Ajusta este valor para cambiar la frecuencia de generación de bonificaciones

                var c = r.Next(1, 10);

                if (c <= chanceOfEnemy)
                {
                    CreateEnemy(plataforma);
                }
                else if (c <= chanceOfEnemy + chanceOfBonus)
                {
                    CreateBonus(plataforma);
                }
            }
        }

        public static void CreateBonus(Plataformas plataforma)
        {
            Random r = new Random();
            var bonusType = r.Next(1, 3);

            switch (bonusType)
            {
                case 1:
                    var bonus = new Bonus(new PointF(plataforma.transform.position.X + (plataforma.sizeX / 2) - 7, plataforma.transform.position.Y - 15), bonusType);
                    bonuses.Add(bonus);
                    break;
                case 2:
                    bonus = new Bonus(new PointF(plataforma.transform.position.X + (plataforma.sizeX / 2) - 15, plataforma.transform.position.Y - 30), bonusType);
                    bonuses.Add(bonus);
                    break;
            }
        }

        public static void CreateEnemy(Plataformas plataforma)
        {
            Random r = new Random();
            var enemyType = r.Next(1, 4);

            switch (enemyType)
            {
                case 1:
                    var enemy = new Enemigos(new PointF(plataforma.transform.position.X + (plataforma.sizeX / 2) - 20, plataforma.transform.position.Y - 40), enemyType);
                    enemigos.Add(enemy);
                    break;
                case 2:
                    enemy = new Enemigos(new PointF(plataforma.transform.position.X + (plataforma.sizeX / 2) - 35, plataforma.transform.position.Y - 50), enemyType);
                    enemigos.Add(enemy);
                    break;
                case 3:
                    enemy = new Enemigos(new PointF(plataforma.transform.position.X + (plataforma.sizeX / 2) - 35, plataforma.transform.position.Y - 60), enemyType);
                    enemigos.Add(enemy);
                    break;
            }
        }

        public static void RemoveEnemy(int i)
        {
            enemigos.RemoveAt(i);
            // Aumentar el puntaje cada vez que se elimina un enemigo
            Controlador.score += 15;
        }

        public static void RemoveBullet(int i)
        {
            balas.RemoveAt(i);
        }

        public static void ClearPlatforms()
        {
            for (int i = 0; i < plataformas.Count; i++)
            {
                if (plataformas[i].transform.position.Y >= 700)
                {
                    plataformas.RemoveAt(i);
                }
            }
            for (int i = 0; i < bonuses.Count; i++)
            {
                if (bonuses[i].verlet.transform.position.Y >= 700)
                {
                    bonuses.RemoveAt(i);
                }
            }

            for (int i = 0; i < enemigos.Count; i++)
            {
                if (enemigos[i].verlet.transform.position.Y >= 700)
                {
                    enemigos.RemoveAt(i);
                }
            }
        }
        public static void AjustarVelocidadGeneracionEnemigos(int nivel)
        {
            // Ajusta la velocidad de generación de enemigos en función del nivel
            int nuevaFrecuencia = 2 + nivel; // Ajusta este valor según la progresión de dificultad deseada
        }
    }
}
