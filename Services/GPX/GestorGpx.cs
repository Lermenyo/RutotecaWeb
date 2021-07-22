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
        private List<Tracks> _tracks;
        private bool disposedValue;

        public GestorGpx()
        {
        }

        public GestorGpx(Tracks track)
        {
            _tracks = new List<Tracks>();
            _tracks.Add(track);
        }
        public GestorGpx(List<Tracks> tracks)
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

        GpxWriter loadTrack(List<Tracks> tracks, 
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
                if (t.GpxMetadata.Any())
                {
                    var metaBD = t.GpxMetadata.First();
                    newMetadata.Bounds = new GpxBounds();
                    if (metaBD.minlat.HasValue)
                        newMetadata.Bounds.MinLatitude = (double)metaBD.minlat.Value;
                    if (metaBD.maxlat.HasValue)
                        newMetadata.Bounds.MaxLatitude = (double)metaBD.maxlat.Value;
                    if (metaBD.minlon.HasValue)
                        newMetadata.Bounds.MinLongitude = (double)metaBD.minlon.Value;
                    if (metaBD.minlat.HasValue)
                        newMetadata.Bounds.MinLatitude = (double)metaBD.minlon.Value;
                }
                newMetadata.Link = new GpxLink();
                newMetadata.Link.Href = link_Href;
                newMetadata.Link.MimeType = mimeType;
                newMetadata.Link.Text = link_Text;
                newMetadata.Name = name;
                newMetadata.Description = descripcion;
                newGpx.WriteMetadata(newMetadata);


                GpxTrack newTrack = new GpxTrack();
                newTrack.Comment = t.Comment;
                newTrack.Description = t.Description;
                newTrack.Name = t.Name;
                newTrack.Number = t.Number;
                newTrack.Source = t.Source;
                newTrack.Type = t.Type;
                if (t.GpxTrackSegment.Any())
                {
                    t.GpxTrackSegment.ToList().ForEach(s =>
                    {
                        var newSegment = new Gpx.GpxTrackSegment();
                        s.GpxPoint.ToList().ForEach(pt =>
                        {
                            if (pt.GpxWayPoint.Any())
                                newGpx.WriteWayPoint(SetGpxWayPoint(pt));
                            else
                                newSegment.TrackPoints.Add(SetGpxPoint(pt));
                        });
                        newTrack.Segments.Add(newSegment);
                    });
                }
                else
                {
                    var newSegment = new Gpx.GpxTrackSegment();
                    t.GpxPoint.ToList().ForEach(pt =>
                    {
                        if (pt.GpxWayPoint.Any())
                            newGpx.WriteWayPoint(SetGpxWayPoint(pt));
                        else
                            newSegment.TrackPoints.Add(SetGpxPoint(pt));
                    });
                    newTrack.Segments.Add(newSegment);
                }
                newGpx.WriteTrack(newTrack);
            }
            );
            return newGpx;
        }

        Gpx.GpxTrackPoint SetGpxPoint(Models.GpxPoint pt)
        {
            var pto = new Gpx.GpxTrackPoint();
            pto.Comment = pt.Comment;
            pto.Description = pt.Description;
            pto.Elevation = (Double?)pt.ele;
            if (pt.lat < 25)
            {
                pto.Latitude = (Double)(pt.lon ?? 0);
                pto.Longitude = (Double)(pt.lat ?? 0);
            }
            else
            {
                pto.Latitude = (Double)(pt.lat ?? 0);
                pto.Longitude = (Double)(pt.lon ?? 0);
            }
            pto.Name = pt.Name;
            pto.Source = pt.Source;
            pto.Symbol = pt.Symbol;
            pto.Time = pt.time;
            return pto;
        }


        Gpx.GpxWayPoint SetGpxWayPoint(Models.GpxPoint pt)
        {
            var pto = new Gpx.GpxWayPoint();
            pto.Comment = pt.Comment;
            pto.Description = pt.Description;
            pto.Elevation = (Double?)pt.ele;
            pto.Latitude = (Double)(pt.lat ?? 0);
            pto.Longitude = (Double)(pt.lon ?? 0);
            pto.Name = pt.Name;
            pto.Source = pt.Source;
            pto.Symbol = pt.Symbol;
            pto.Time = pt.time;
            pto.DisplayMode = pt.GpxWayPoint.FirstOrDefault()?.DisplayMode;
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
