using System;
using System.Collections.Generic;
using System.Linq;

namespace GaraioLogParser.Model.IISRecord
{
    public class IISLogRecordSet : ILogRecordSet
    {
        private IISLogRecord _element;
        private IISLogRecordSet _nextElement;

        private IISLogRecordSet _root;

        public IISLogRecordSet()
        {
            _element = null;
            _nextElement = null;
            _root = null;
        }

        protected IISLogRecordSet(IISLogRecordSet root, IISLogRecord newElement)
            : this()
        {
            _root = root;
            _element = newElement;
        }

        public virtual ILogRecord Element => _element;
        public virtual ILogRecordSet NextElement => _nextElement;
        public virtual bool IsLastElement => _nextElement == null;

        public IISLogRecord FindItem(IISLogRecord item) => _root?.FindItemIntoNextElement(item);

        private IISLogRecord FindItemIntoNextElement(IISLogRecord item)
        {
            if (_element.Equals(item)) return _element;
            else if(_nextElement != null) return _nextElement.FindItemIntoNextElement(item);
            return null;
        }

        public virtual bool Add(ILogRecord newItem) => Add((IISLogRecord)newItem);

        private bool Add(IISLogRecord newItem)
        {
            try
            {
                if (_element == null)
                {
                    _element = newItem;
                    _root = this;
                    _nextElement = null;
                }
                else
                {
                    var item = FindItem(newItem);
                    if (item == null)
                    {
                        var nextElem = new IISLogRecordSet(_root, newItem);
                        AddToLast(nextElem);
                    }
                    else
                    {
                        item.IncrementCounter();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool AddToLast(IISLogRecordSet nextElem)
        {
            if (_nextElement == null) _nextElement = nextElem;
            else _nextElement.AddToLast(nextElem);
            return true;
        }
    }
}
