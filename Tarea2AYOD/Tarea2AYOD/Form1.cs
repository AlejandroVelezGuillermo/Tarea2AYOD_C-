using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace Tarea2AYOD
{
    public partial class btnLeerAccesoDirecto : Form
    {
        string filePath = "datos.txt"; // Ruta del archivo

        string dataFilePath = "datos.dat"; // Ruta del archivo de datos
        string indexFilePath = "index.dat"; // Ruta del archivo de índice
        public btnLeerAccesoDirecto()
        {
            InitializeComponent();
        }
        //Implementar las operaciones para archivos secuenciales.
        private void btnEscribir_Click(object sender, EventArgs e)
        {
            string data = txtData.Text;

            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine(data);
                    }

                    MessageBox.Show("Datos escritos en el archivo correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al escribir en el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Ingrese datos antes de escribir en el archivo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLeer_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string content = reader.ReadToEnd();
                    MessageBox.Show("Contenido del archivo:\n" + content, "Lectura exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        //Implementar las operaciones para archivos secuenciales indexados.
        private void btnEscribir2_Click(object sender, EventArgs e)
        {
            string data = txtData1.Text;

            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    long position;
                    using (FileStream dataFileStream = new FileStream(dataFilePath, FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(dataFileStream))
                        {
                            position = dataFileStream.Position;
                            writer.WriteLine(data);
                        }
                    }

                    using (FileStream indexFileStream = new FileStream(indexFilePath, FileMode.Append, FileAccess.Write))
                    {
                        using (BinaryWriter indexWriter = new BinaryWriter(indexFileStream))
                        {
                            indexWriter.Write(position);
                        }
                    }

                    MessageBox.Show("Datos escritos en el archivo correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al escribir en el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Ingrese datos antes de escribir en el archivo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLeer2_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> dataRecords = new List<string>();

                using (FileStream indexFileStream = new FileStream(indexFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader indexReader = new BinaryReader(indexFileStream))
                    {
                        while (indexFileStream.Position < indexFileStream.Length)
                        {
                            long position = indexReader.ReadInt64();

                            using (FileStream dataFileStream = new FileStream(dataFilePath, FileMode.Open, FileAccess.Read))
                            {
                                dataFileStream.Seek(position, SeekOrigin.Begin);

                                using (StreamReader dataReader = new StreamReader(dataFileStream))
                                {
                                    string data = dataReader.ReadLine();
                                    dataRecords.Add(data);
                                }
                            }
                        }
                    }
                }

                string content = string.Join(Environment.NewLine, dataRecords);
                MessageBox.Show("Contenido del archivo:\n" + content, "Lectura exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }







        //Implementar las operaciones para archivos de acceso directo.
        private void btnCrearAccesoDirect_Click(object sender, EventArgs e)
        {
            // Obtener la ruta del archivo al que se quiere crear el acceso directo
            string archivoObjetivo = txtRutaArchivo.Text;

            // Obtener la ruta y nombre para el nuevo acceso directo
            string rutaAccesoDirecto = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MiAccesoDirecto.lnk");

            // Crear el objeto de acceso directo
            var accesoDirecto = new WshShell().CreateShortcut(rutaAccesoDirecto) as IWshShortcut;

            // Configurar las propiedades del acceso directo
            if (accesoDirecto != null)
            {
                accesoDirecto.TargetPath = archivoObjetivo;
                accesoDirecto.Save();
                MessageBox.Show("Acceso directo creado con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLeerAccesoDirect_Click(object sender, EventArgs e)
        {
            // Obtener la ruta del acceso directo a leer
            string rutaAccesoDirecto = txtRutaAccesoDirecto.Text;

            try
            {
                // Verificar si el archivo de acceso directo existe
                if (System.IO.File.Exists(rutaAccesoDirecto))
                {
                    // Leer el acceso directo
                    var accesoDirecto = new WshShell().CreateShortcut(rutaAccesoDirecto) as IWshShortcut;

                    // Mostrar la información del acceso directo
                    if (accesoDirecto != null)
                    {
                        MessageBox.Show($"Información del acceso directo:\n\n" +
                                        $"Destino: {accesoDirecto.TargetPath}\n" +
                                        $"Icono: {accesoDirecto.IconLocation}\n" +
                                        $"Trabaja en: {accesoDirecto.WorkingDirectory}",
                                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("El archivo de acceso directo no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar leer el acceso directo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificarAccesoDirecto_Click(object sender, EventArgs e)
        {
            // Obtener la ruta del acceso directo a modificar
            string rutaAccesoDirecto = txtRutaAccesoDirecto.Text;

            // Verificar si el archivo de acceso directo existe
            if (System.IO.File.Exists(rutaAccesoDirecto))
            {
                // Leer el acceso directo
                var accesoDirecto = new WshShell().CreateShortcut(rutaAccesoDirecto) as IWshShortcut;

                // Modificar el acceso directo
                if (accesoDirecto != null)
                {
                    accesoDirecto.TargetPath = txtNuevoDestino.Text;
                    accesoDirecto.Save();
                    MessageBox.Show("Acceso directo modificado con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("El archivo de acceso directo no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
