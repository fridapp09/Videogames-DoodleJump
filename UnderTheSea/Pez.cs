using System.Drawing;

namespace UnderTheSea
{
    public class Pez
    {
        private Image sprite;
        private PointF posicion;
        private Size tamaño;
        private float velocidadY;
        private Form1 form;

        public Pez(PointF posicion, Image sprite, Size tamaño, float velocidadY, Form1 form)
        {
            this.posicion = posicion;
            this.sprite = sprite;
            this.tamaño = tamaño;
            this.velocidadY = velocidadY;
            this.form = form;
        }

        public void Actualizar()
        {
            // Actualiza la posición del pez
            posicion.Y += velocidadY;

            // Si el pez se sale de la pantalla, lo reposiciona arriba
            if (posicion.Y > form.ClientSize.Height)
            {
                Reposicionar();
            }
        }

        public void MoveWithSirena(float movimientoSirena)
        {
            // Mover el pez en la dirección del movimiento de la sirena
            posicion = new PointF(posicion.X, posicion.Y + movimientoSirena);
        }

        public void Dibujar(Graphics g)
        {
            // Dibuja el pez en su posición actual
            g.DrawImage(sprite, posicion.X, posicion.Y, tamaño.Width, tamaño.Height);
        }

        private void Reposicionar()
        {
            // Reposiciona el pez arriba de la pantalla con una posición y velocidad aleatoria
            Random random = new Random();
            posicion.X = random.Next(form.ClientSize.Width);
            posicion.Y = -tamaño.Height; // Reposiciona arriba de la pantalla
            velocidadY = random.Next(1, 5); // Velocidad aleatoria entre 1 y 5
        }
    }
}