﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wpf_proyectounicah
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Window
    {
        public MenuPrincipal(string usuario)
        {
            InitializeComponent();
            //Mostrar  el nombre del formulario
            lblUsuario.Content = string.Format("Hola {0}! ¿Qué deseas realizar?", usuario);
            
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            //Retornar el usuario al formulario de inicio de sesion
            IniciarSesion iniciarSesion = new IniciarSesion();
            iniciarSesion.Show();
            Close();
        
        }

        private void btnHabitaciones_Click(object sender, RoutedEventArgs e)
        {
            //Nueva instancia de la ventana de habitaciones
            Habitaciones habitaciones = new Habitaciones();
            habitaciones.Show(); 
        }
    }
}
