namespace UnderTheSea
{
    public partial class Form1 : Form
    {
        Sirena sirena;
        System.Windows.Forms.Timer timer1;
        private Image fondo;
        private Image fondoarena;
        private List<Pez> peces;
        private float lastSirenaY = 0;

        private int nivel = 1;
        private int puntuacion = 0;
        private int velocidadJuego = 15; // Velocidad base del juego

        private bool juegoDetenido = false; // Bandera para indicar si el juego está detenido

        public Form1()
        {
            InitializeComponent();
            Init();
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 15;
            timer1.Tick += new EventHandler(Update);
            timer1.Start();
            this.KeyDown += new KeyEventHandler(OnKeyboardPressed);
            this.KeyUp += new KeyEventHandler(OnKeyboardUp);
            this.Height = 600;
            this.Width = 330;
            this.Paint += new PaintEventHandler(OnRepaint);

            // Inicializar la lista de peces
            peces = new List<Pez>();

            // Generar peces aleatorios
            GenerarPecesAleatorios();
        }

        private void GenerarPecesAleatorios()
        {
            Random random = new Random();

            // Generar una cantidad de peces aleatorios
            int cantidadPeces = 11;
            for (int i = 0; i < cantidadPeces; i++)
            {
                // Generar posición aleatoria para el pez
                PointF posicionInicial = new PointF(random.Next(ClientSize.Width), random.Next(ClientSize.Height));

                // Cargar una imagen de pez aleatoria (asegúrate de tener imágenes nombradas pes1.png, pes2.png, etc.)
                string nombreImagenPez = $"Resources/pes{i + 1}.png";
                Image imagenPez = Image.FromFile(nombreImagenPez);

                // Tamaño fijo para todos los peces (ajustar según sea necesario)
                Size tamañoPez = new Size(50, 50);

                // Generar velocidad aleatoria para el pez
                float velocidadY = random.Next(1, 5); // Velocidad aleatoria entre 1 y 5

                // Crear instancia de Pez y agregarlo a la lista, pasando 'this' como referencia al formulario actual
                Pez pez = new Pez(posicionInicial, imagenPez, tamañoPez, velocidadY, this);
                peces.Add(pez);
            }
        }

        public void Init()
        {
            nivel = 1;
            Controlador.plataformas = new System.Collections.Generic.List<Plataformas>();
            Controlador.AddPlatform(new System.Drawing.PointF(100, 400));
            Controlador.startPlatformPosY = 300;
            Controlador.score = 0;
            Controlador.GenerateStartSequence();
            Controlador.balas.Clear();
            Controlador.bonuses.Clear();
            Controlador.enemigos.Clear();
            sirena = new Sirena();

            // Cargar las imágenes de fondo
            fondo = Image.FromFile("Resources/1fondo.png");
            fondoarena = Image.FromFile("Resources/1fondoarena.png");

            // Inicializar la lista de peces
            //peces = new List<Pez>();
        }

        private void OnKeyboardUp(object sender, KeyEventArgs e)
        {
            sirena.verlet.dx = 0;
            switch (e.KeyCode.ToString())
            {
                case "Space":
                    Controlador.CreateBullet(new PointF(sirena.verlet.transform.position.X + sirena.verlet.transform.size.Width / 2, sirena.verlet.transform.position.Y));
                    break;
            }
        }

        private void OnKeyboardPressed(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Right":
                    sirena.verlet.dx = 6;
                    break;
                case "Left":
                    sirena.verlet.dx = -6;
                    break;
                case "Space":
                    break;
            }
        }

        private void Update(object sender, EventArgs e)
        {
            // Actualizar el nivel según la puntuación
            ActualizarNivel();

            // Detener el juego si la bandera juegoDetenido es true
            if (juegoDetenido)
                return;

            if (sirena.verlet.transform.position.Y >= Controlador.plataformas[0].transform.position.Y + 200)
            {
                // Sirena cayó demasiado lejos, se considera como pérdida
                juegoDetenido = true;
                MostrarVentanaPerder();
            }
            else if (sirena.verlet.StandartCollidePlayerWithObjects(true, false))
            {
                // Sirena chocó con un enemigo u objeto, se considera como pérdida
                juegoDetenido = true;
                MostrarVentanaPerder();
            }

            // Actualizar la posición de los peces
            //UpdateFishPosition();

            if ((sirena.verlet.transform.position.Y >= Controlador.plataformas[0].transform.position.Y + 200) || sirena.verlet.StandartCollidePlayerWithObjects(true, false))
                Init();

            sirena.verlet.StandartCollidePlayerWithObjects(false, true);

            if (Controlador.balas.Count > 0)
            {
                for (int i = 0; i < Controlador.balas.Count; i++)
                {
                    if (Math.Abs(Controlador.balas[i].verlet.transform.position.Y - sirena.verlet.transform.position.Y) > 500)
                    {
                        Controlador.RemoveBullet(i);
                        continue;
                    }
                    Controlador.balas[i].MoveUp();
                }
            }
            if (Controlador.enemigos.Count > 0)
            {
                for (int i = 0; i < Controlador.enemigos.Count; i++)
                {
                    if (Controlador.enemigos[i].verlet.StandartCollide())
                    {
                        Controlador.RemoveEnemy(i);
                        break;
                    }
                }
            }

            // Actualizar la posición de los peces
            foreach (Pez pez in peces)
            {
                pez.Actualizar();
            }

            sirena.verlet.ApplyVerlet();
            FollowPlayer();

            Invalidate();
        }

        private void ActualizarNivel()
        {
            // Aumentar el nivel cada vez que la puntuación alcance un múltiplo de 500
            if (Controlador.score >= nivel * 500)
            {
                nivel++;
                Controlador.AjustarVelocidadGeneracionEnemigos(nivel); // Ajusta la velocidad de generación de enemigos en función del nivel
            }
        }

        private void MostrarVentanaPerder()
        {
            // Mostrar ventana emergente con las opciones de reiniciar o salir del juego
            DialogResult result = MessageBox.Show("¡Has perdido! ¿Quieres reiniciar el juego?", "Perdiste", MessageBoxButtons.YesNo);

            // Manejar la respuesta del jugador
            if (result == DialogResult.Yes)
            {
                // Reiniciar el juego
                Init();
                juegoDetenido = false; // Reiniciar el juego, la bandera se restablece
            }
            else if (result == DialogResult.No)
            {
                // Salir del juego
                Application.Exit();
            }
        }

        private void UpdateFishPosition()
        {
            // Obtener la posición Y actual de la sirena
            float currentSirenaY = sirena.verlet.transform.position.Y;

            // Calcular el cambio en la posición de los peces basado en el movimiento de la sirena
            float movement = lastSirenaY - currentSirenaY;

            // Mover los peces
            foreach (Pez pez in peces)
            {
                pez.MoveWithSirena(movement);
            }

            // Actualizar la última posición Y de la sirena
            lastSirenaY = currentSirenaY;
        }

        public void FollowPlayer()
        {
            int offset = 300 - (int)sirena.verlet.transform.position.Y;
            sirena.verlet.transform.position.Y += offset;
            for (int i = 0; i < Controlador.plataformas.Count; i++)
            {
                var platform = Controlador.plataformas[i];
                platform.transform.position.Y += offset;
            }
            for (int i = 0; i < Controlador.balas.Count; i++)
            {
                var bullet = Controlador.balas[i];
                bullet.verlet.transform.position.Y += offset;
            }
            for (int i = 0; i < Controlador.enemigos.Count; i++)
            {
                var enemy = Controlador.enemigos[i];
                enemy.verlet.transform.position.Y += offset;
            }
            for (int i = 0; i < Controlador.bonuses.Count; i++)
            {
                var bonus = Controlador.bonuses[i];
                bonus.verlet.transform.position.Y += offset;
            }
        }

        private void OnRepaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Dibujar los fondos en capas con parallax
            DrawBackgrounds(g);

            // Dibujar los peces
            foreach (Pez pez in peces)
            {
                pez.Dibujar(g);
            }

            if (Controlador.plataformas.Count > 0)
            {
                for (int i = 0; i < Controlador.plataformas.Count; i++)
                    Controlador.plataformas[i].DrawSprite(g);
            }
            if (Controlador.balas.Count > 0)
            {
                for (int i = 0; i < Controlador.balas.Count; i++)
                    Controlador.balas[i].DrawSprite(g);
            }
            if (Controlador.enemigos.Count > 0)
            {
                for (int i = 0; i < Controlador.enemigos.Count; i++)
                    Controlador.enemigos[i].DrawSprite(g);
            }
            if (Controlador.bonuses.Count > 0)
            {
                for (int i = 0; i < Controlador.bonuses.Count; i++)
                    Controlador.bonuses[i].DrawSprite(g);
            }
            
            sirena.DrawSprite(g);

            // Dibujar el puntaje en el formulario
            Font drawFont = new Font("Arial", 12);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            string scoreText = "Score: " + Controlador.score;
            g.DrawString(scoreText, drawFont, drawBrush, new PointF(10, 10));
            g.DrawString("Nivel: " + nivel, drawFont, drawBrush, new PointF(10, 30));

        }

        private void DrawBackgrounds(Graphics g)
        {
            // Obtener las dimensiones del formulario
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            // Dibujar el fondo lejano (cielo o horizonte) ajustando al tamaño del formulario
            g.DrawImage(fondo, new Rectangle(0, 0, formWidth, formHeight));

            // Dibujar el fondo medio (montañas o edificios) ajustando al tamaño del formulario
            g.DrawImage(fondoarena, new Rectangle(0, 0, formWidth, formHeight));
        }
    }
}