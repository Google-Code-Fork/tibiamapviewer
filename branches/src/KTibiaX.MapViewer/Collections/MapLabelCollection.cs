using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using KTibiaX.MapViewer.Controls;
using KTibiaX.Analyzer.Events;
using System;
using System.Xml.Serialization;

namespace KTibiaX.MapViewer.Collections {
    [Serializable]
    public class MapLabelCollection : List<MapLabel> {

        /// <summary>
        /// Initializes a new instance of the <see cref="MapLabelCollection"/> class.
        /// </summary>
        public MapLabelCollection() { }

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
            base.ForEach(lbl => lbl.Map = map);
        }

        /// <summary>
        /// Updates the events.
        /// </summary>
        public void UpdateEvents() {
            base.ForEach(lbl => lbl.ContextMenuRequested += OnContextMenuRequested);
        }

        public new void Add(MapLabel value) {
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
        public MapLabelCollection(List<MapLabel> list) {
            this.AddRange(list);
        }

        /// <summary>
        /// Determines whether the specified point has a label.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>
        /// 	<c>true</c> if the specified point has a label; otherwise, <c>false</c>.
        /// </returns>
        public bool HasLabel(Point point) {
            var res = from label in this where label.InBounds(point) select label;
            return res.Count() > 0;
        }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public MapLabel GetLabel(Point point) {
            var res = from label in this where label.InBounds(point) select label;
            return res.Count() > 0 ? res.First() : null;
        }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <returns></returns>
        public MapLabel GetLabel(double oid) {
            var res = from label in this where label.OID == oid select label;
            return res.Count() > 0 ? res.First() : null;
        }

    }
}
