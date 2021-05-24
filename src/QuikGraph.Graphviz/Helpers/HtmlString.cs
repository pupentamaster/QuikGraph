﻿using JetBrains.Annotations;

namespace QuikGraph.Graphviz.Helpers
{
    /// <summary>
    /// String representing HTML content.
    /// </summary>
    internal struct HtmlString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlString"/> struct.
        /// </summary>
        /// <param name="html">HTML string.</param>
        public HtmlString([NotNull] string html)
        {
            String = html;
        }

        /// <summary>
        /// HTML string.
        /// </summary>
        [NotNull]
        public string String { get; }
    }
}