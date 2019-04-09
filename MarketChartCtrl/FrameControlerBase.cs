using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MarketChartCtrl.IndicatorRenderItem;

namespace MarketChartCtrl
{
    public abstract class FrameControlerBase : IEnumerable<FrameView>
    {
        protected List<FrameView> _item = new List<FrameView>();
        public abstract IndicatorFrame Add(int index = -1, IndicatorRender rdr = null);
        public abstract void Clear();
        public abstract void RemoveAt(int index);
        public abstract bool Remove(FrameView item);

        public abstract IEnumerator<FrameView> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
