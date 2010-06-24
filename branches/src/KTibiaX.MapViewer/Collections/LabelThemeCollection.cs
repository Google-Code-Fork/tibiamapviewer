using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KTibiaX.MapViewer.Model;

namespace KTibiaX.MapViewer.Collections {
    [Serializable]
    public class LabelThemeCollection : List<LabelTheme> {
        
        /// <summary>
        /// Gets the theme.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public LabelTheme GetTheme(string name) {
            if (!string.IsNullOrEmpty(name)) {
                var res = this.Where(th => th.Name.ToLower() == name.ToLower());
                return res.Count() > 0 ? res.First() : null;
            }
            return null;
        }

        /// <summary>
        /// Sets the theme.
        /// </summary>
        /// <param name="theme">The theme.</param>
        public void SetTheme(LabelTheme theme) {
            if (theme != null) {
                var index = this.IndexOf(GetTheme(theme.Name));
                if (index > 0) { this.Remove(this[index]); }
                this.Add(theme);
            }
        }
                
    }
}
