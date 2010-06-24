using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;
using KTibiaX.Analyzer.Events;
using KTibiaX.MapViewer.Controls;

namespace KTibiaX.MapViewer.Collections {
    [Serializable]
    public class MapMarkCollection : List<MapMark> {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MapLabelCollection"/> class.
        /// </summary>
        public MapMarkCollection() { }

        [XmlIgnore]
        public Controls.MapViewer CurrentMap { get; set; }

        /// <summary>
        /// Occurs when [context menu requested].
        /// </summary>
        public event EventHandler<PositionEventArgs> ContextMenuRequested;

        /// <summary>
        /// Updates the map.
        /// </summary>
        /// <param name="map">The map.</param>
        public void UpdateMap(Controls.MapViewer map) {
            CurrentMap = map;
            base.ForEach(mrk => mrk.Map = map);
        }

        /// <summary>
        /// Updates the events.
        /// </summary>
        public void UpdateEvents() {
            base.ForEach(mrk => mrk.ContextMenuRequested += OnContextMenuRequested);
        }

        public new void Add(MapMark value) {
            value.Map = CurrentMap;
            value.ContextMenuRequested += OnContextMenuRequested;
            base.Add(value);
        }

        /// <summary>
        /// Called when [context menu requested].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="KTibiaX.Analyzer.Events.PositionEventArgs"/> instance containing the event data.</param>
        private void OnContextMenuRequested(object source, PositionEventArgs e) {
            if (ContextMenuRequested != null) ContextMenuRequested(this, e);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapLabelCollection"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public MapMarkCollection(List<MapMark> list) {
            this.AddRange(list);
        }

        /// <summary>
        /// Determines whether the specified point has a mark.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>
        /// 	<c>true</c> if the specified point has a mark; otherwise, <c>false</c>.
        /// </returns>
        public bool HasMark(Point point) {
            var res = from mark in this where mark.InBounds(point) select mark;
            return res.Count() > 0;
        }

        /// <summary>
        /// Gets the mark.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public MapMark GetMark(Point point) {
            var res = from mark in this where mark.InBounds(point) select mark;
            return res.Count() > 0 ? res.First() : null;
        }

        /// <summary>
        /// Gets the mark.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <returns></returns>
        public MapMark GetMark(double oid) {
            var res = from mark in this where mark.OID == oid select mark;
            return res.Count() > 0 ? res.First() : null;
        }

    }
}
