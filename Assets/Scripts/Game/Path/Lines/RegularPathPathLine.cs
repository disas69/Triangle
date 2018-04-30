using Game.Path.Lines.Base;

namespace Game.Path.Lines
{
    public class RegularPathPathLine : PathLine<PathLineSettings>
    {
        public override LineType LineType
        {
            get { return LineType.Regular; }
        }
    }
}