using System.Collections.Generic;

using Windows.ApplicationModel.DataTransfer;

namespace Practice04_DndD.Models
{
    public class DragDropCompletedData
    {
        public DataPackageOperation DropResult { get; set; }

        public IReadOnlyList<object> Items { get; set; }
    }
}
