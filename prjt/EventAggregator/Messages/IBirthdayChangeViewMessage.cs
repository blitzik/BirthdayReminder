using prjt.ViewModels;
using prjt.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.EventAggregator.Messages
{
    public interface IBirthdayChangeViewMessage<T>
    {
        ViewSide Side { get; }
        Type Type { get; }
        T ViewModel { get; }
    }
}
