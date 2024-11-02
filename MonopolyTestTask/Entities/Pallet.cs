using MonopolyTestTask.Entities.Interfaces;

namespace MonopolyTestTask.Entities
{
    public class Pallet : GeometryObject, IMeasurable
    {
        public Pallet(int width, int height, int depth) : base(width, height, depth)
        {

        }

        private List<Box> _boxes = [];

        public List<Box> GetBoxes() => _boxes.ToList() ?? [];

        public bool AddBox(Box box)
        {
            if (box.Width <= Width &&
                box.Depth <= Depth)
            {
                _boxes.Add(box);
                return true;
            }

            return false;
        }

        public int Weigh => 30000 + _boxes.Sum(b => b.Weigh);

        public override decimal Volume => GetObjectVolume() + _boxes.Sum(b => b.Volume);

        public DateOnly ExpirationDate => _boxes.Count <= 0 ? new DateOnly() : _boxes.Min(b => b.ExpirationDate);
        
        public override string TypeName => nameof(Pallet);
    }
}
