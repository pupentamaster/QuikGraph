﻿using System;
using System.IO;
using System.Net;
using JetBrains.Annotations;

namespace QuikGraph.Graphviz
{
    /// <summary>
    /// Helper extensions to render graphs to graphviz
    /// </summary>
    public static class GraphvizExtensions
    {
        /// <summary>
        /// Renders a graph to the GraphViz DOT format.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>Graph serialized in DOT format.</returns>
        [Pure]
        [NotNull]
        public static string ToGraphviz<TVertex, TEdge>([NotNull] this IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new GraphvizAlgorithm<TVertex, TEdge>(graph);
            return algorithm.Generate();
        }

        /// <summary>
        /// Renders a graph to the GraphViz DOT format.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <param name="initAlgorithm">Delegate that initializes the DOT generation algorithm.</param>
        /// <returns>Graph serialized in DOT format.</returns>
        public static string ToGraphviz<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> graph,
            [NotNull, InstantHandle] Action<GraphvizAlgorithm<TVertex, TEdge>> initAlgorithm)
            where TEdge : IEdge<TVertex>
        {
            if (initAlgorithm is null)
                throw new ArgumentNullException(nameof(initAlgorithm));

            var algorithm = new GraphvizAlgorithm<TVertex, TEdge>(graph);
            initAlgorithm(algorithm);
            return algorithm.Generate();
        }

        /// <summary>
        /// Performs a layout of <paramref name="graph"/> from DOT format to an
        /// SVG (Scalable Vector Graphics) file by calling Agl through
        /// the http://rise4fun.com/ REST services.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>The svg graph.</returns>
        [Pure]
        [NotNull]
        public static string ToSvg<TVertex, TEdge>([NotNull] this IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            return ToSvg(ToGraphviz(graph));
        }

        /// <summary>
        /// Performs a layout of <paramref name="graph"/> from DOT format to an
        /// SVG (Scalable Vector Graphics) file by calling Agl through
        /// the http://rise4fun.com/ REST services.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <param name="initAlgorithm">Delegate that initializes the DOT generation algorithm.</param>
        /// <returns>The svg graph.</returns>
        [Pure]
        [NotNull]
        public static string ToSvg<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> graph,
            [NotNull, InstantHandle] Action<GraphvizAlgorithm<TVertex, TEdge>> initAlgorithm)
            where TEdge : IEdge<TVertex>
        {
            return ToSvg(ToGraphviz(graph, initAlgorithm));
        }

        /// <summary>
        /// Performs a layout from DOT to  an SVG (Scalable Vector Graphics) file
        /// by calling Agl through the http://rise4fun.com/ REST services.
        /// </summary>
        /// <param name="dot">The dot graph</param>
        /// <returns>The svg graph.</returns>
        [Pure]
        [NotNull]
        public static string ToSvg([NotNull] string dot)
        {
            if (dot is null)
                throw new ArgumentNullException(nameof(dot));

            var request = WebRequest.Create("http://rise4fun.com/rest/ask/Agl/");
            request.Method = "POST";
            // Write dot
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(dot);
            }

            // Read Svg
            WebResponse response = request.GetResponse();
            Stream streamResponse = response.GetResponseStream();
            if (streamResponse is null)
                return string.Empty;
            using (var reader = new StreamReader(streamResponse))
            {
                return reader.ReadToEnd();  // Svg
            }
        }
    }
}