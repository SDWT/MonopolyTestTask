using MonopolyTestTask.Entities.Base;
using MonopolyTestTask.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyTestTask.Entities
{
    public abstract class GeometryObject(int width, int height, int depth) : BaseEntity, IVolumable
    {
        /// <summary>
        /// Ширина объекта
        /// </summary>
        public int Width { get; } = width;

        /// <summary>
        /// Высота объекта
        /// </summary>
        public int Height { get; } = height;

        /// <summary>
        /// Глубина объекта
        /// </summary>
        public int Depth { get; } = depth;

        public virtual decimal Volume => (decimal)Depth * Width * Height;
        public decimal GetObjectVolume() => (decimal)Depth * Width * Height;

        public virtual string TypeName => nameof(GeometryObject);
    }
}
