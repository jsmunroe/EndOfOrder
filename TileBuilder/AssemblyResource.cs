using System;
using System.IO;
using System.Reflection;
using TileBuilder.Contracts;

namespace TileBuilder
{
    public class AssemblyResource : IResource
    {
        private readonly Assembly _assembly;
        private readonly string _name;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_assembly">Assembly that contains the resource.</param>
        /// <param name="a_name">Resource name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_assembly"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_name"/> is null.</exception>
        public AssemblyResource(Assembly a_assembly, string a_name)
        {
            #region Argument Validation

            if (a_assembly == null)
                throw new ArgumentNullException(nameof(a_assembly));

            if (a_name == null)
                throw new ArgumentNullException(nameof(a_name));

            #endregion

            _assembly = a_assembly;
            _name = a_name;
        }

        /// <summary>
        /// Open the resource stream and return it.
        /// </summary>
        /// <returns>Resource stream.</returns>
        public Stream Open()
        {
            return _assembly.GetManifestResourceStream(_name);
        }
    }
}