using RutotecaWeb.Models;
using Gpx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RutotecaWeb.Services
{
    class GestorGpx: IDisposable
    {
        private List<Track> _tracks;
        private bool disposedValue;

        public GestorGpx()
        {
        }

        public GestorGpx(Track track)
        {
            _tracks = new List<Track>();
            _tracks.Add(track);
        }
        public GestorGpx(List<Track> tracks)
        {
            _tracks = tracks;
        }

        /// <summary>
        /// Guarda todo en un fichero
        /// </summary>
        public void SaveInFile(string filePath)
        {
            var fileStream = File.OpenWrite(filePath);
            GpxWriter writer = loadTrack(_tracks, fileStream);
            writer.Dispose();
        }

        /// <summary>
        /// Guarda todo en un fichero
        /// </summary>
        public void SaveInStream(Stream stream,
                            string link_Href = "https://rutoteca.es/",
                            string link_Text = "Rutoteca",
                            string mimeType = "text/html",
                            string name = "Rutoteca",
                            string descripcion = "Visita Rutoteca el mayor almacen de rutas homologadas de España")
        {
            GpxWriter writer = loadTrack(_tracks, 
                                         stream,
                                         link_Href,
                                         link_Text,
                                         mimeType,
                                         name,
                                         descripcion);
            writer.Dispose();
        }

        #region Crear Fichero GPX
        
        GpxWriter loadTrack(List<Track> tracks, 
                            Stream stream,
                            string link_Href = "https://rutoteca.es/",
                            string link_Text = "Rutoteca",
                            string mimeType = "text/html",
                            string name = "Rutoteca",
                            string descripcion = "Visita Rutoteca el mayor almacen de rutas homologadas de España")
        {
            GpxWriter newGpx = new GpxWriter(stream);
            tracks.ForEach(t =>
            {

                var newMetadata = new Gpx.GpxMetadata();
                newMetadata.Link = new GpxLink();
                newMetadata.Link.Href = link_Href;
                newMetadata.Link.MimeType = mimeType;
                newMetadata.Link.Text = link_Text;
                newMetadata.Name = name;
                newMetadata.Description = descripcion;
                newGpx.WriteMetadata(newMetadata);

                GpxTrack newTrack = new GpxTrack();
                var newSegment = new Gpx.GpxTrackSegment();
                var inicio = t.GpxPoint.First().Time;
                t.GpxPoint.ToList().ForEach(pt =>
                {
                    newSegment.TrackPoints.Add(SetGpxPoint(pt, inicio));
                });
                newTrack.Segments.Add(newSegment);
                newGpx.WriteTrack(newTrack);
            }
            );
            return newGpx;
        }

        Gpx.GpxTrackPoint SetGpxPoint(Models.GpxPoint pt, DateTime? inicio)
        {
            /*
             * Sacar elevacion
             https://secure.geonames.org/srtm3JSON?lat=43.528641&lng=-6.523797&username=galwayireland
            {"srtm3":107,"lng":-6.523797,"lat":43.528641}
             */
            var pto = new Gpx.GpxTrackPoint();
            pto.Elevation = pt.Ele??1;
            if (pt.Lat < 25)
            {
                pto.Latitude = (Double)(pt.Lon ?? 0);
                pto.Longitude = (Double)(pt.Lat ?? 0);
            }
            else
            {
                pto.Latitude = (Double)(pt.Lat ?? 0);
                pto.Longitude = (Double)(pt.Lon ?? 0);
            }
            if (inicio.HasValue && pt.Time.HasValue)
            {
                DateTime dt = DateTime.Now;
                TimeSpan ts = pt.Time.Value - inicio.Value;
                pto.Time = dt + ts;
            }
            else
            {
                pto.Time = DateTime.Now;
            }
            return pto;
        }


        Gpx.GpxWayPoint SetGpxWayPoint(Models.GpxPoint pt)
        {
            var pto = new Gpx.GpxWayPoint();
            pto.Elevation = (Double?)pt.Ele;
            pto.Latitude = (Double)(pt.Lat ?? 0);
            pto.Longitude = (Double)(pt.Lon ?? 0);
            return pto;
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminar el estado administrado (objetos administrados)
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~GestorGpx()
        // {
        //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
